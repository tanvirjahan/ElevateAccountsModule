Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing.Color
Imports System.Collections.ArrayList

Imports System.Linq
'Imports System.Drawing
Imports System.IO
Imports System.IO.File
Imports System.Text
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports System

Partial Class Default3
    Inherits System.Web.UI.Page
    Dim objUser As New clsUser
    Private wbPart As WorkbookPart = Nothing
    Private document As SpreadsheetDocument = Nothing
    Private Connection As SqlConnection
    Private wbPart1 As WorkbookPart = Nothing
    Private document1 As SpreadsheetDocument = Nothing


#Region "Global Decalration"
    Dim objUtils As New clsUtils
    Dim iFlag As Integer = 0
    Dim iApprovalFlag As Integer = 0

    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim tabflag_tracking As Boolean = True
    Dim tabflag_pending As Boolean = True

#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim privilege As String = "0"
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Not CType(Session("sAppId"), String) Is Nothing Then
                    If CType(Session("sAppId"), String) = "1" Then
                        RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                        FillTrackingDashBoard()
                        FillApprovalTrackingDashBoard()
                        Dim intGroupId As Integer
                        intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                        privilege = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select privilegeid from group_privilege_detail where appid=1 and privilegeid=15 and groupid=" & intGroupId)

                        If privilege = "15" Then
                            FillPendingGrid()
                        Else
                            TabContainer1.Tabs.Remove(TabPanel3)
                            Div_pendingsearchbox.Visible = False
                        End If

                        'If gv_pendingcontracts.Rows.Count = 0 Then
                        '    TabContainer1.Tabs.Remove(TabPanel3)
                        '    'Div_pendingsearchbox.Visible = False
                        '    tabflag_pending = False
                        'End If

                        If gvTracking.Rows.Count = 0 And gvApprovalTracking.Rows.Count = 0 Then
                            TabContainer1.Tabs.Remove(TabPanel1)

                            tabflag_tracking = False

                        End If


                        If gvTracking.Rows.Count > 0 Or gvApprovalTracking.Rows.Count > 0 Or gv_pendingcontracts.Rows.Count > 0 Then
                            dvContractdashBoard.Visible = True
                        Else
                            dvContractdashBoard.Visible = False
                        End If

                    Else
                        dvContractdashBoard.Visible = False
                    End If
                Else
                    dvContractdashBoard.Visible = False
                End If


                '' Create a Dynamic datatable ---- Start
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcCountry = New DataColumn("Value", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcCountry)
                Session("sDtDynamicTracking") = dtDynamic

                Dim dtDynamicPending = New DataTable()
                Dim dcCodePending = New DataColumn("Code", GetType(String))
                Dim dcCountryPending = New DataColumn("Value", GetType(String))
                dtDynamicPending.Columns.Add(dcCodePending)
                dtDynamicPending.Columns.Add(dcCountryPending)
                Session("sDtDynamicPending") = dtDynamicPending

                'Dim dtDynamic1 = New DataTable()
                'Dim dcCode1 = New DataColumn("Code", GetType(String))
                'Dim dcCountry1 = New DataColumn("Value", GetType(String))
                'dtDynamic1.Columns.Add(dcCode1)
                'dtDynamic1.Columns.Add(dcCountry1)
                'Session("sDtDynamic") = dtDynamic
                '--------end
                '' Create a Dynamic datatable ---- Start
                Dim dtHotelDetails = New DataTable()
                Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsValue = New DataColumn("Value", GetType(String))
                Dim dcTrackingStatus = New DataColumn("TrackingStatus", GetType(String))
                dtHotelDetails.Columns.Add(dcGroupDetailsType)
                dtHotelDetails.Columns.Add(dcGroupDetailsCode)
                dtHotelDetails.Columns.Add(dcGroupDetailsValue)
                dtHotelDetails.Columns.Add(dcTrackingStatus)
                Session("sDtHotelDetails") = dtHotelDetails
                '--------end
            End If



            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SuppliersWindowPostBack") Then
                FillTrackingDashBoard()
                FillApprovalTrackingDashBoard()
                Dim intGroupId As Integer
                intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                privilege = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select privilegeid from group_privilege_detail where appid=1 and privilegeid=15 and groupid=" & intGroupId)

                If privilege = "15" Then
                    FillPendingGrid_forVS()
                End If




            End If

            If (Me.IsPostBack) Then

                FillTrackingDashBoard()
                FillApprovalTrackingDashBoard()
                Dim intGroupId As Integer
                intGroupId = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
                privilege = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select privilegeid from group_privilege_detail where appid=1 and privilegeid=15 and groupid=" & intGroupId)

                If privilege = "15" Then
                    FillPendingGrid_forVS()
                Else
                    TabContainer1.Tabs.Remove(TabPanel3)
                    Div_pendingsearchbox.Visible = False
                End If
                'If tabflag_pending = False Then
                '    TabContainer1.Tabs.Remove(TabPanel3)
                '    'Div_pendingsearchbox.Visible = False
                'End If

                If gvTracking.Rows.Count = 0 And gvApprovalTracking.Rows.Count = 0 Then 'If tabflag_tracking = False Then
                    TabContainer1.Tabs.Remove(TabPanel1)
                End If


                ''If gvTracking.Rows.Count = 0 And gvApprovalTracking.Rows.Count = 0 Then
                ''    TabContainer1.Tabs.Remove(TabPanel1)
                ''End If
                'If gv_pendingcontracts.Rows.Count = 0 Then
                '    TabContainer1.Tabs.Remove(TabPanel3)
                '    'Div_pendingsearchbox.Visible = False
                'End If

                ''If gvTracking.Rows.Count > 0 Or gvApprovalTracking.Rows.Count > 0 Or gv_pendingcontracts.Rows.Count > 0 Then
                ''    dvContractdashBoard.Visible = True

                ''End If

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MainPage.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region




    Protected Sub lbUpdate_Click(sender As Object, e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbUpdate As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbUpdate.NamingContainer, GridViewRow)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Session.Add("SupState", "Edit")
        Session.Add("SupRefCode", CType(hdCHotelCode.Value.Trim, String))
        Session.Add("sEmailCode", CType(hdEmailId_.Value.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('PriceListModule/SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(hdCHotelCode.Value.Trim, String) + "&EmailId=" + CType(hdEmailId_.Value, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub





    Private Sub FillTrackingDashBoard()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TrackStatusName As New List(Of String)
        Try
            strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,rtrim(EmailSubject)EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,TaskStartDate,TaskCompleteDate from VIEW_TRACKING where  AssignedTo= '" & Session("GlobalUserName") & "'  and TaskCompleteDate is null order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                gvTracking.DataSource = myDS
                gvTracking.DataBind()
            Else
                gvTracking.DataSource = Nothing
                gvTracking.DataBind()
            End If




        Catch ex As Exception
        End Try
    End Sub



#Region "Private Sub FillPendingGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillPendingGrid()
        Dim myDS As New DataSet
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter


        Dim pagevaluecus = RowsPerPageCUS.SelectedValue

        Dim strSqlQry, strsqlqry1 As String
        'gv_SearchResult.Visible = True
        'lblMsg.Visible = False

        'If gv_SearchResult.PageIndex < 0 Then
        '    gv_SearchResult.PageIndex = 0
        'End If
        strSqlQry = ""
        strsqlqry1 = ""
        Try


            ''If chkshowall.Checked = True Then
            'strSqlQry = "select contractid ,partycode,partyname,'Contract' promotionname,applicableto,convert(varchar(10),(convert(datetime,fromdate,111)),103) fromdate,convert(varchar(10),(convert(datetime,todate,111)),103) todate  from view_contracts_search where [status]='No' and withdraw=0  and contractid<>'' "
            ''Else
            'strsqlqry1 = " union select promotionid contractid,partycode ,partyname, promotionname,applicableto,fromdate,todate from view_offer_search where [status]='No' and (activestate='With Drawn' or activestate='Active' )  and promotionid<>''  order by partyname ,contractid   "
            ''End If


            strSqlQry = "select contractid ,partycode,partyname,'Contract' promotionname,applicableto,convert(varchar(10),(convert(datetime,fromdate,111)),103) fromdate,convert(varchar(10),(convert(datetime,todate,111)),103) todate,activestate  from view_contracts_search_pending where [status]='No' and withdraw=0  and contractid<>'' "
            'Else
            strsqlqry1 = " union select promotionid contractid,partycode ,partyname, promotionname,applicableto,fromdate,todate,activestate from view_offer_search_pending where [status]='No' and (activestate='With Drawn' or activestate='Active' )  and promotionid<>''  order by partyname ,contractid   "
            'End If


            'If Trim(BuildCondition) <> "" Then
            strSqlQry = strSqlQry & strsqlqry1

            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            Dim pliststr As String = ""
            'If ViewState("MyAutoNo") <> 1 Then
            '    pliststr = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "pricelist_links", "calledfromlist", "Autoid", Request.QueryString("AutoNo"))
            'End If

            'If pliststr <> "" Then
            '    strSqlQry = strSqlQry & " WHERE contractid='" & pliststr & "'"

            '    ''strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'Else

            '    If Trim(BuildCondition) <> "" Then


            '        strSqlQry = strSqlQry & "  " & BuildCondition()

            '        strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            '    Else

            '        strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            '    End If

            'End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gv_pendingcontracts.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gv_pendingcontracts.PageSize = pagevaluecus
                gv_pendingcontracts.DataBind()
            Else
                gv_pendingcontracts.PageIndex = 0
                gv_pendingcontracts.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("mainpage.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub


#End Region

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        Dim dtt As DataTable
        dtt = Session("sDtDynamicPending")

        If dtt.Rows.Count > 0 Then
            FillPendingGrid_forVS()
        Else
            FillPendingGrid()
        End If



    End Sub
    Private Sub FillApprovalTrackingDashBoard()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TrackStatusName As New List(Of String)
        Try
            'strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,rtrim(EmailSubject)EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,convert(varchar(16),ApprovalStart,103)+ ' ' + convert(varchar(5),ApprovalStart,108)ApprovalStart,convert(varchar(16),ApprovalEnd,103)+ ' ' + convert(varchar(5),ApprovalEnd,108)ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,AssignedTo,AssignedToName,TaskStartDate,TaskCompleteDate,convert(varchar(16),ApprovalAssignmentDate,103)+ ' ' + convert(varchar(5),ApprovalAssignmentDate,108)ApprovalAssignmentDate,Approver from VIEW_TRACKING where  Approver= '" & Session("GlobalUserName") & "'  and ApprovalAssignmentDate is not null  and ApprovalEnd is null order by EmailLineNo"
            strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,rtrim(EmailSubject)EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,convert(varchar(16),ApprovalStart,103)+ ' ' + convert(varchar(5),ApprovalStart,108)ApprovalStart,convert(varchar(16),ApprovalEnd,103)+ ' ' + convert(varchar(5),ApprovalEnd,108)ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103) AssignedDate,AssignedTo,AssignedToName,TaskStartDate,TaskCompleteDate,convert(varchar(16),ApprovalAssignmentDate,103)ApprovalAssignmentDate,Approver from VIEW_TRACKING where  Approver= '" & Session("GlobalUserName") & "'  and ApprovalAssignmentDate is not null  and ApprovalEnd is null order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    dvContractdashBoard.Visible = True
            'Else
            '    dvContractdashBoard.Visible = False
            'End If
            gvApprovalTracking.DataSource = myDS
            gvApprovalTracking.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Protected Sub gvTracking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTracking.RowDataBound
        Try

            'gvTracking.Columns("yourColumnName").Frozen = True
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).CssClass = "lockedHeader"

                e.Row.Cells(2).CssClass = "lockedHeaderAssDate"
                e.Row.Cells(4).CssClass = "lockedHeader"
                e.Row.Cells(6).CssClass = "lockedHeader"
                e.Row.Cells(8).CssClass = "lockedHeader"
                e.Row.Cells(10).CssClass = "lockedHeader"
                e.Row.Cells(12).CssClass = "lockedHeaderLast"
                ' e.Row.Cells(8).CssClass = "lockedHeaderNext"


            Else
                If iFlag = 0 Then

                    e.Row.Cells(0).CssClass = "locked"
                    'e.Row.Cells(0).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    'e.Row.Cells(1).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    e.Row.Cells(2).CssClass = "lockedAssDate"
                    'e.Row.Cells(2).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    'e.Row.Cells(3).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    e.Row.Cells(4).CssClass = "locked"
                    e.Row.Cells(6).CssClass = "locked"
                    e.Row.Cells(8).CssClass = "locked"
                    e.Row.Cells(10).CssClass = "locked"
                    e.Row.Cells(12).CssClass = "lockedLast"
                    iFlag = 1
                Else

                    e.Row.Cells(0).CssClass = "lockedAlternative"
                    'e.Row.Cells(0).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    'e.Row.Cells(1).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    e.Row.Cells(2).CssClass = "lockedAlternativeAssDate"
                    'e.Row.Cells(2).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    'e.Row.Cells(3).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    e.Row.Cells(4).CssClass = "lockedAlternative"
                    e.Row.Cells(6).CssClass = "lockedAlternative"
                    e.Row.Cells(8).CssClass = "lockedAlternative"
                    e.Row.Cells(10).CssClass = "lockedAlternative"
                    e.Row.Cells(12).CssClass = "lockedAlternativeLast"
                    'e.Row.Cells(8).CssClass = "lockedAlternativeNext"
                    iFlag = 0
                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblStartDate As Label = CType(e.Row.FindControl("lblStartDate"), Label)
                Dim lblComplete As Label = CType(e.Row.FindControl("lblComplete"), Label)
                Dim lbStart As LinkButton = CType(e.Row.FindControl("lbStart"), LinkButton)
                Dim lbComplete As LinkButton = CType(e.Row.FindControl("lbComplete"), LinkButton)
                Dim lblStartDate1 As Label = CType(e.Row.FindControl("lblStartDate1"), Label)
                Dim lblComplete1 As Label = CType(e.Row.FindControl("lblComplete1"), Label)
                Dim lbStart1 As LinkButton = CType(e.Row.FindControl("lbStart1"), LinkButton)
                Dim lbComplete1 As LinkButton = CType(e.Row.FindControl("lbComplete1"), LinkButton)
                lbStart.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to start task now?')==false)return false;")
                lbComplete.Attributes.Add("onclick", "javascript:if(confirm(' Are you sure to mark the task as complete?')==false)return false;")

                If lblStartDate.Text = "" Then
                    lbStart.Visible = True
                    lblStartDate.Visible = False
                    lblComplete.Visible = False
                    lbComplete.Visible = False
                    lbStart1.Visible = True
                    lblStartDate1.Visible = False
                    lblComplete1.Visible = False
                    lbComplete1.Visible = False
                Else
                    lbStart.Visible = False
                    lblStartDate.Visible = True
                    lbStart1.Visible = False
                    lblStartDate1.Visible = True
                    If lblComplete.Text = "" Then
                        lbComplete.Visible = True
                        lblComplete.Visible = False
                        lbComplete1.Visible = True
                        lblComplete1.Visible = False
                    Else
                        lbComplete.Visible = False
                        lblComplete.Visible = True
                        lbComplete1.Visible = False
                        lblComplete1.Visible = True
                    End If

                End If



                Dim lblProgressStage As Label = CType(e.Row.FindControl("lblProgressStage"), Label)
                If lblProgressStage.Text = "Reassigned" Then
                    'e.Row.CssClass = "Pending"
                    e.Row.BackColor = System.Drawing.Color.Pink
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub


    Protected Sub lnkapp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbcountries As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbcountries.NamingContainer, GridViewRow)
        Dim lbcontractid As LinkButton = CType(gvRow.FindControl("lnkcontoffID"), LinkButton)
        Dim strcontractid As String = lbcontractid.Text
        Dim lbloffername As Label = CType(gvRow.FindControl("lblOfferName"), Label)

        Dim myDS, myDS1 As New DataSet
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter

        Dim strSqlQry, strsqlqry1 As String

        strSqlQry = ""
        strsqlqry1 = ""
        Try


            If lbloffername.Text.ToLower = "contract" Then

                strSqlQry = "select c.ctrycode,c.ctryname from edit_contracts_countries e   join ctrymast c  on e.ctrycode= c.ctrycode where e.contractid='" & strcontractid & "'  "

                strsqlqry1 = "select a.agentname ,e.ctrycode+'-'+c.ctryname ctrycodename from edit_contracts_agents e join agentmast a on  e.agentcode=a.agentcode  left join ctrymast c on e.ctrycode=c.ctrycode where e.contractid='" & strcontractid & "'  "
            Else
                strSqlQry = "select c.ctrycode,c.ctryname from edit_offers_countries e  join ctrymast c  on e.ctrycode= c.ctrycode where e.promotionid='" & strcontractid & "'  "

                strsqlqry1 = "select a.agentname ,'' ctrycodename from edit_offers_agents e join agentmast a on  e.agentcode=a.agentcode  where e.promotionid='" & strcontractid & "'  "
            End If





            Dim pliststr As String = ""



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            gvDisplayCountries.DataSource = myDS


            If myDS.Tables(0).Rows.Count > 0 Then
                gvDisplayCountries.DataBind()
            Else
                gvDisplayCountries.PageIndex = 0
                gvDisplayCountries.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If




            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strsqlqry1, SqlConn)
            myDataAdapter.Fill(myDS1)
            gvDisplayAgents.DataSource = myDS1
            If myDS1.Tables(0).Rows.Count > 0 Then
                gvDisplayAgents.DataBind()
            Else
                gvDisplayAgents.PageIndex = 0
                gvDisplayAgents.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

            DisplayAGents_Countries.Show()
            ' meContractTracking.Show()
        Catch ex As Exception

        End Try
    End Sub



    Protected Sub lnkexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbcountries As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbcountries.NamingContainer, GridViewRow)
        Dim lbcontractid As LinkButton = CType(gvRow.FindControl("lnkcontoffID"), LinkButton)
        Dim strcontractid As String = lbcontractid.Text
        Dim lbloffername As Label = CType(gvRow.FindControl("lblOfferName"), Label)
        Dim lblPARTYCODE As Label = CType(gvRow.FindControl("lblPARTYCODE"), Label)

        Dim strSqlQry, strsqlqry1 As String

        strSqlQry = ""
        strsqlqry1 = ""
        Try


            If lbloffername.Text.ToLower = "contract" Then
                CONTRACTEXCELPRINT(strcontractid, lblPARTYCODE.Text)

            Else
                OFFERSEXCELPRINT(strcontractid, lblPARTYCODE.Text)

            End If











        Catch ex As Exception

        End Try
    End Sub


    Private Sub CONTRACTEXCELPRINT(ByVal ContractID As String, ByVal PARTYCODE As String)



        Try
            Dim FolderPath As String = "~\ExcelTemplates\"
            Dim FileName As String = "ContractPrint.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "ContractPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            document = SpreadsheetDocument.Open(outputPath, True)
            wbPart = document.WorkbookPart

            Dim wsName As String = "Index"

            UpdateValue(wsName, "C3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "C5", ContractID, 3, True, False)

            wsName = "Main Details"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)
            UpdateValue(wsName, "A6", "From Date", 2, True, False)

            Dim dt1 As New DataTable
            strSqlQry = "select isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),applicableto,0 countrygroups, isnull(activestate,'') activestate  from  view_contracts_search where contractid ='" & ContractID & "' order by convert(varchar(10),fromdate , 111)"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt1)
            SqlConn.Close()
            If dt1.Rows.Count > 0 Then
                For i As Integer = 0 To dt1.Rows.Count
                    UpdateValue(wsName, "B6", dt1.Rows(0)(0).ToString, 3, True, False)
                    UpdateValue(wsName, "B7", dt1.Rows(0)(1).ToString, 3, True, False)
                    UpdateValue(wsName, "B8", dt1.Rows(0)(2).ToString, 3, True, False)
                    UpdateValue(wsName, "B10", dt1.Rows(0)(4).ToString, 3, True, False)
                Next
            End If


            Dim dt As New DataTable
            strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_contractcountry  v where v.ctrycode =p.ctrycode  and v.contractid ='" & ContractID & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)


            SqlConn.Close()
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    UpdateValue(wsName, "A14", dt.Rows(0)("Country").ToString, 3, True, True, "F14", True)
                Next
            End If

            Dim rs As New DataTable


            strSqlQry = "select isnull(seasonname,''),isnull(convert(varchar(10),fromdate , 105),'')fromdate,isnull(convert(varchar(10),todate , 105),'')todate,isnull(MinNight,'') from view_contractseasons where contractid='" & ContractID & "' order by convert(varchar(10),fromdate , 111) "
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rs)

            UpdateTableValue(wsName, rs, 0, 19, 3, True)

            SqlConn.Close()
            wsName = "Commission"

            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", ContractID, 3, True, False)


            Dim dt3 As New DataTable
            strSqlQry = "select tranid,isnull(seasons,'')seasons from view_contracts_commission_header where contractid = '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt3)
            SqlConn.Close()

            'Dim iLine As Integer = 11

            Dim iLineNo2 As Integer = 10
            'Dim addressname As String = ""

            If dt3.Rows.Count > 0 Then
                For i As Integer = 0 To dt3.Rows.Count - 1
                    Dim x As Integer
                    Dim y As Integer
                    Dim ex As Integer

                    UpdateValue(wsName, "A" & iLineNo2 - 1.ToString, "Commission Id", 2, True, False)
                    UpdateValue(wsName, "B" & iLineNo2 - 1.ToString, "Formula Name", 2, True, False)
                    UpdateValue(wsName, "C" & iLineNo2 - 1.ToString, "Formula String", 2, True, False)



                    If dt3.Rows(i)("seasons") <> "" Then
                        UpdateValue(wsName, "D" & iLineNo2 - 1.ToString, "Season Name", 2, True, False)
                        UpdateValue(wsName, "E" & iLineNo2 - 1.ToString, "From Date", 2, True, False)
                        UpdateValue(wsName, "F" & iLineNo2 - 1.ToString, "To Date", 2, True, False)
                    Else
                        UpdateValue(wsName, "E" & iLineNo2 - 1.ToString, "From Date", 2, True, False)
                        UpdateValue(wsName, "F" & iLineNo2 - 1.ToString, "To Date", 2, True, False)
                    End If
                    UpdateValue(wsName, "G" & iLineNo2 - 1.ToString, "Room Categories", 2, True, False)
                    UpdateValue(wsName, "H" & iLineNo2 - 1.ToString, "Room Types", 2, True, False)
                    UpdateValue(wsName, "I" & iLineNo2 - 1.ToString, "Meal Plans", 2, True, False)
                    UpdateValue(wsName, "J" & iLineNo2 - 1.ToString, "Applicable To", 2, True, False)

                    Dim dt34 As New DataTable
                    strSqlQry = "select distinct h.tranid, v.formulaname,formulastring=isnull(stuff((select ';' + c.term1+','+ltrim(rtrim(convert(varchar(20),c.value)))  from view_contracts_commissions c,commissionformula_header h where c.formulaid=h.formulaid  and  c.tranid ='" & dt3.Rows(i)("tranid").ToString & "'    order by c.clineno for xml path('')),1,1,''),'') from view_contracts_commission_header h, view_contracts_commissions c, commissionformula_header v  where h.tranid = c.tranid And c.formulaid = v.formulaid and h.tranid ='" & dt3.Rows(i)("tranid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dt34)


                    'Dim MyAdapter As OleDbDataAdapter = New OleDbDataAdapter
                    'Dim MyTable As System.Data.DataTable = New DataTable()

                    ''MyAdapter.Fill(MyTable, rs1)    ' rs is the ADODB.Recordset

                    UpdateTableValue(wsName, dt34, 0, iLineNo2, 3, True)
                    SqlConn.Close()
                    'xlSheet.Range("B" & iLineNo2.ToString).wraptext = True
                    'xlSheet.Range("C" & iLineNo2.ToString).wraptext = True
                    'x = rs1.RecordCount

                    If dt3.Rows(i)("seasons") <> "" Then
                        Dim rs2 As New DataTable
                        strSqlQry = "select isnull(cs.Item1,'') seascode,isnull(convert(varchar(10),s.fromdate , 105),'')fromdate,isnull(convert(varchar(10),s.todate , 105),'')todate from view_contracts_commission_header h"
                        strSqlQry += " cross apply dbo.SplitString1colsWithOrderField(h.seasons,',') cs "
                        strSqlQry += " join view_contractseasons s on h.contractid=s.contractid and cs.Item1=s.SeasonName "
                        strSqlQry += " where h.contractid ='" & ContractID & "' and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(cs.Item1,'')<>'' order by convert(varchar(10),s.fromdate , 111) "

                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rs2)

                        UpdateTableValue(wsName, rs2, 3, iLineNo2, 3, True)
                        SqlConn.Close()
                        y = rs2.Rows.Count
                    Else
                        Dim rssd As New DataTable
                        strSqlQry = "SELECT isnull(fromdate,''),isnull(todate,'') from view_contracts_commission_detail WHERE tranid='" & dt3.Rows(i)("tranid").ToString & "' order by convert(varchar(10),fromdate , 111) "
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rssd)

                        UpdateTableValue(wsName, rssd, 4, iLineNo2, 3, True)
                        SqlConn.Close()
                        ex = rssd.Rows.Count

                    End If

                    Dim rs3 As New DataTable
                    '  strSqlQry = "select distinct isnull(prm.rmtypname,''),isnull(h.mealplans,''),h.applicableto from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s  join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>''"
                    strSqlQry = " select   rmtypname, mealplans ,applicableto  from ( select distinct isnull(prm.rmtypname,'') rmtypname,isnull(h.mealplans,'') mealplans,h.applicableto , " _
                                   & " isnull(prm.rankord,999) rankord  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s  join partyrmtyp prm on s.Item1=prm.rmtypcode " _
                                   & " join  view_contracts_search vs on vs.contractid=h.contractid and vs.partycode=prm.partycode   and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and  isnull(h.roomtypes,'')<>'' ) ts  order by   rankord"

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs3)


                    UpdateTableValue(wsName, rs3, 7, iLineNo2, 3, True)
                    SqlConn.Close()
                    Dim z As Integer
                    z = rs3.Rows.Count



                    Dim rs4 As New DataTable
                    strSqlQry = "select isnull(r.rmcatname,'')   from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join view_contracts_search ch on h.contractid=ch.contractid join rmcatmast r on s.Item1=r.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>'' "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs4)
                    UpdateTableValue(wsName, rs4, 6, iLineNo2, 3, True)
                    SqlConn.Close()
                    Dim d As Integer
                    d = rs4.Rows.Count



                    Dim Maxint As Integer = Math.Max(x, Math.Max(y, Math.Max(z, Math.Max(ex, d))))


                    iLineNo2 = iLineNo2 + Maxint + 2

                Next
            End If

            wsName = "Max Occupancy"

            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", ContractID, 3, True, False)



            Dim dtmx As New DataTable


            strSqlQry = "select distinct tranid from view_partymaxaccomodation where partycode= '" & PARTYCODE & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtmx)


            Dim iLinmx As Integer = 9

            Dim em1 As Integer
            Dim em As Integer
            Dim em2 As Integer



            Dim dt23 As New DataTable
            If dtmx.Rows.Count > 0 Then
                For i As Integer = 0 To dtmx.Rows.Count - 1

                    UpdateValue(wsName, "A" & iLinmx - 1.ToString, "Max Occ.ID", 2, True, False)
                    UpdateValue(wsName, "B" & iLinmx - 1.ToString, "Room Name", 2, True, False)
                    UpdateValue(wsName, "C" & iLinmx - 1.ToString, "Room Classification", 2, True, False)
                    UpdateValue(wsName, "D" & iLinmx - 1.ToString, "Unit yes/no", 2, True, False)
                    UpdateValue(wsName, "E" & iLinmx - 1.ToString, "Price Adult Occupancy only for Unit", 2, True, False)
                    UpdateValue(wsName, "F" & iLinmx - 1.ToString, "Price Pax", 2, True, False)
                    UpdateValue(wsName, "G" & iLinmx - 1.ToString, "Max Adults", 2, True, False)
                    UpdateValue(wsName, "H" & iLinmx - 1.ToString, "Max Child", 2, True, False)
                    UpdateValue(wsName, "I" & iLinmx - 1.ToString, "Max Infant", 2, True, False)
                    UpdateValue(wsName, "J" & iLinmx - 1.ToString, "Max EB", 2, True, False)
                    UpdateValue(wsName, "K" & iLinmx - 1.ToString, "No of Extra Person Supplement for Unit Only", 2, True, False)
                    UpdateValue(wsName, "L" & iLinmx - 1.ToString, "Max Total Occupancy without infant", 2, True, False)
                    UpdateValue(wsName, "M" & iLinmx - 1.ToString, "Rank Order", 2, True, False)
                    UpdateValue(wsName, "N" & iLinmx - 1.ToString, "Occupancy Combinations", 2, True, False)
                    UpdateValue(wsName, "O" & iLinmx - 1.ToString, "Start with 0 based", 2, True, False)

                    ' UpdateValue(wsName, "A" & iLinmx - 1.ToString, "Max Occ.ID", 2, True, False)
                    'UpdateValue(wsName, "B" & iLinmx - 1.ToString, "Room Name", 2, True, False)
                    'UpdateValue(wsName, "C" & iLinmx - 1.ToString, "Room Classification", 2, True, False)
                    'UpdateValue(wsName, "D" & iLinmx - 1.ToString, "Unit yes/no", 2, True, False)
                    'UpdateValue(wsName, "E" & iLinmx - 1.ToString, "Price Adult Occupancy only for Unit", 2, True, False)
                    'UpdateValue(wsName, "F" & iLinmx - 1.ToString, "Price Pax", 2, True, False)
                    'UpdateValue(wsName, "G" & iLinmx - 1.ToString, "Max Adults", 2, True, False)
                    'UpdateValue(wsName, "H" & iLinmx - 1.ToString, "Max Child", 2, True, False)
                    'UpdateValue(wsName, "I" & iLinmx - 1.ToString, "Max Infant", 2, True, False)
                    'UpdateValue(wsName, "J" & iLinmx - 1.ToString, "Max EB", 2, True, False)
                    'UpdateValue(wsName, "K" & iLinmx - 1.ToString, "Max Total Occupancy without infant", 2, True, False)
                    'UpdateValue(wsName, "L" & iLinmx - 1.ToString, "Rank Order", 2, True, False)
                    'UpdateValue(wsName, "M" & iLinmx - 1.ToString, "Occupancy Combinations", 2, True, False)
                    'UpdateValue(wsName, "N" & iLinmx - 1.ToString, "Start with 0 based", 2, True, False)

                    'xlSheet.Range("E" & iLinmx - 1.ToString).wraptext = True
                    'xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
                    'xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True

                    Dim dtmx1 As New DataTable
                    '  strSqlQry = "select rmtypcode from view_partymaxaccomodation where partycode= '" & partycode & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
                    strSqlQry = "select v.rmtypcode from view_partymaxaccomodation v ,partyrmtyp  p  where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and    v.partycode= '" & PARTYCODE & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  p.rankord"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtmx1)

                    Dim rsmx As New DataTable
                    '  strSqlQry = "select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status,case when isnull(prm.unityesno,0)=0 then 0 else m.pricepax  end  pricepaxunit,case when isnull(prm.unityesno,0)=0 then 2 else m.pricepax  end pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb,isnull(m.noofextraperson,''), m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & partycode & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

                    strSqlQry = "  select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status," _
                                            & " case when isnull(prm.unityesno,0)=0 then 0 else m.pricepax  end  pricepaxunit, " _
                                            & " case when isnull(prm.unityesno,0)=0 then 2 else m.pricepax  end pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb, " _
                                            & " isnull(m.noofextraperson,'') noofextraperson, m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm, " _
                                            & " view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode  " _
                                            & " and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid  " _
                                            & "  And h.partycode='" & PARTYCODE & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx)
                    UpdateTableValue(wsName, rsmx, 0, iLinmx, 3, True)
                    SqlConn.Close()
                    em = rsmx.Rows.Count


                    Dim rsmx2 As New DataTable
                    strSqlQry = "select  isnull(start0based,'') from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & PARTYCODE & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx2)

                    UpdateTableValue(wsName, rsmx2, 14, iLinmx, 3, True)
                    SqlConn.Close()
                    em2 = rsmx2.Rows.Count
                    '   iLinmx = iLinmx - dtmx1.Rows.Count



                    If dtmx1.Rows.Count > 0 Then
                        For idt As Integer = 0 To dtmx1.Rows.Count - 1

                            Dim rsmx1 As New DataTable
                            strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & PARTYCODE & "' for xml path('')),1,1,''),'') "
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsmx1)

                            UpdateTableValue(wsName, rsmx1, 13, iLinmx, 3, True)
                            SqlConn.Close()
                            em1 = rsmx1.Rows.Count
                            iLinmx = iLinmx + 1

                        Next

                    End If


                    Dim Maxint As Integer = Math.Max(em1, Math.Max(em2, em))

                    iLinmx = iLinmx + Maxint + 4

                Next

            End If


            wsName = "Room Rates"

            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)

            Dim dtrr2 As New DataTable
            strSqlQry = "select plistcode from view_cplisthnew  where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtrr2)
            Dim iLine2 As Integer = 7
            Dim ei2 As Integer
            Dim ei3 As Integer
            Dim ei4 As Integer
            Dim ws7 As Integer
            If dtrr2.Rows.Count > 0 Then
                For i As Integer = 0 To dtrr2.Rows.Count - 1

                    UpdateValue(wsName, "A" & iLine2.ToString, "PriceList Code", 2, True, False)
                    UpdateValue(wsName, "B" & iLine2.ToString, "Aplicable to", 2, True, False)

                    Dim rse2 As New DataTable
                    strSqlQry = "select plistcode,applicableto from  view_cplisthnew where plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse2)

                    iLine2 = iLine2 + 1
                    UpdateTableValue(wsName, rse2, 0, iLine2, 3, True)
                    SqlConn.Close()
                    ei2 = rse2.Rows.Count



                    iLine2 = iLine2 + 1

                    Dim rse32 As New DataTable
                    strSqlQry = "select  isnull(c.subseascode,''),isnull(convert(varchar(10),d.fromdate , 103),'')fromdate,isnull(convert(varchar(10),d.todate , 103),'')todate from view_cplisthnew c,view_contractseasons d  where c.subseascode=d.SeasonName and c.plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and d.contractid='" & ContractID & "' order by convert(varchar(10),d.fromdate , 111)"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse32)

                    SqlConn.Close()
                    ei4 = rse32.Rows.Count


                    If ei4 > 0 Then
                        UpdateValue(wsName, "A" & iLine2.ToString, "Season", 2, True, False)
                        UpdateValue(wsName, "B" & iLine2.ToString, "From Date", 2, True, False)
                        UpdateValue(wsName, "C" & iLine2.ToString, "To Date", 2, True, False)
                    End If
                    iLine2 = iLine2 + 1

                    UpdateTableValue(wsName, rse32, 0, iLine2, 3, True)

                    iLine2 = iLine2 + ei4 + 1
                    Dim fromrange As Integer, torange As Integer
                    fromrange = iLine2
                    torange = IIf(rse32.Rows.Count > 0, iLine2 + rse32.Rows.Count, iLine2)

                    'xlSheet.Range("B" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
                    'xlSheet.Range("C" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
                    Dim rsw2 As New DataTable
                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_cplisthnew_weekdays   where  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' for xml path('')),1,1,''),'') "
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsw2)
                    ws7 = rsw2.Rows.Count

                    If rsw2.Rows.Count > 0 Then
                        UpdateValue(wsName, "B" & iLine2.ToString, rsw2.Rows(0).ToString, 3, True, True, "C" & iLine2.ToString, True)
                        UpdateTableValue(wsName, rsw2, 1, iLine2, 3, True)
                    End If
                    SqlConn.Close()

                    'xlSheet.Range("B" & iLine2.ToString).wraptext = True
                    'xlSheet.Range("B" & iLine2.ToString).rowheight = 30
                    UpdateValue(wsName, "A" & iLine2.ToString, "Days of the week", 2, True, False)
                    iLine2 = iLine2 + 2

                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_roomrates]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@plistcode", dtrr2.Rows(i)("plistcode").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)
                                con.Close()
                            End Using
                        End Using
                    End Using


                    'Dim rssp72 As New ADODB.Recordset
                    'rssp72 = convertToADODB(dtt)
                    'Dim ii3 As Integer = 65
                    'For Each column As DataColumn In dt.Columns
                    '    Name(ii) = column.ColumnName
                    '    iLine2 = iLine2 + 2
                    '    Dim sss3 = Chr(ii3).ToString
                    '    For OO As Integer = 0 To dtt.Columns.Count - 1
                    '        UpdateValue(wsName, sss3.ToString() + iLine2.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)
                    '        ii3 += 1
                    '        sss3 = Chr(ii3).ToString
                    '    Next

                    iLine2 = iLine2 + ws7  '+ 1

                    Dim ik As Integer = 65


                    Dim sss = Chr(ik).ToString
                    For OO As Integer = 0 To dtt.Columns.Count - 1


                        UpdateValue(wsName, sss.ToString() & iLine2.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)

                        ik += 1
                        UpdateValue(wsName, sss.ToString() & iLine2.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)

                        sss = Chr(ik).ToString
                    Next



                    If dtt.Rows.Count > 0 Then
                        iLine2 = iLine2 + 1
                        UpdateTableValue(wsName, dtt, 0, iLine2, 3, True)

                        ei3 = dtt.Rows.Count

                        fromrange = iLine2
                        torange = IIf(dtt.Rows.Count > 0, iLine2 + dtt.Rows.Count, iLine2)
                        'xlSheet.Range("C" & fromrange.ToString & ":" & "C" & torange.ToString).NumberFormat = "####"
                    End If

                    iLine2 = iLine2 + ei3 + 3

                Next

            End If


            wsName = "Exhibition Supplements"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", ContractID, 3, True, False)

            Dim dte As New DataTable
            strSqlQry = "select d.exhibitionid,d.elineno from view_contracts_exhibition_detail d, view_contracts_exhibition_header h   where h.exhibitionid=d.exhibitionid  and  h.contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dte)




            Dim iLinee As Integer = 8

            UpdateValue(wsName, "A" & iLinee - 1.ToString, "Exhibition Id", 2, True, False)

            UpdateValue(wsName, "B" & iLinee - 1.ToString, "Applicable To", 2, True, False)
            UpdateValue(wsName, "C" & iLinee - 1.ToString, "Exhibition Name", 2, True, False)
            UpdateValue(wsName, "D" & iLinee - 1.ToString, "From Date", 2, True, False)
            UpdateValue(wsName, "E" & iLinee - 1.ToString, "To Date", 2, True, False)

            UpdateValue(wsName, "F" & iLinee - 1.ToString, "Room Type", 2, True, False)
            UpdateValue(wsName, "G" & iLinee - 1.ToString, "Meal Plan", 2, True, False)
            UpdateValue(wsName, "H" & iLinee - 1.ToString, "Supplement Amount", 2, True, False)
            UpdateValue(wsName, "I" & iLinee - 1.ToString, "Min Stay", 2, True, False)

            iLinee = iLinee + 1

            Dim ei As Integer
            Dim ze As Integer
            If dte.Rows.Count > 0 Then
                For i As Integer = 0 To dte.Rows.Count - 1





                    strSqlQry = "select h.exhibitionid,h.applicableto, e.exhibitionname,convert(varchar(10),d.fromdate,105),convert(varchar(10),d.todate,105) ,'', isnull(d.mealplans,''),supplementvalue,isnull(d.minstay,'') from view_contracts_exhibition_detail d join   " _
                          & "  exhibition_master e on d.exhibitioncode=e.exhibitioncode join  view_contracts_exhibition_header h on d.exhibitionid=h.exhibitionid and  " _
                          & " d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "' and d.elineno=" & dte.Rows(i)("elineno").ToString
                    Dim rse As New DataTable

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse)
                    SqlConn.Close()
                    ei = rse.Rows.Count
                    UpdateTableValue(wsName, rse, 0, iLinee, 3, True)

                    'strSqlQry = "select distinct isnull(mealplans,''),supplementvalue,isnull(minstay,'') from view_contracts_exhibition_detail where exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
                    'Dim rsr As New DataTable
                    'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    'myDataAdapter.Fill(rsr)
                    'SqlConn.Close()

                    'ze = rsr.Rows.Count

                    'UpdateTableValue(wsName, rsr, 6, iLinee, 3, True)

                    Dim dter As New DataTable
                    strSqlQry = "select distinct roomtypes,exhibitioncode from view_contracts_exhibition_detail  where  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "' and elineno=" & dte.Rows(i)("elineno").ToString
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dter)


                    If dter.Rows(0)("roomtypes").ToString = "All" Then
                        strSqlQry = "select roomtypes from view_contracts_exhibition_detail  where  roomtypes='All' and exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "' and elineno=" & dte.Rows(i)("elineno").ToString
                        UpdateValue(wsName, "F" & iLinee.ToString, "All", 2, True, False)

                        '  iLinee = iLinee + 1

                    ElseIf dter.Rows(0)("roomtypes").ToString <> "All" Then

                        strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  " _
                            & " d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "' and d.elineno=" & dte.Rows(i)("elineno").ToString & "  and p.partycode ='" & PARTYCODE & "' and  " _
                            & " d.exhibitioncode='" & dter.Rows(0)("exhibitioncode").ToString & "' and d.elineno=" & dte.Rows(i)("elineno").ToString & "  for xml path('')),1,1,''),'') "
                        Dim rser As New DataTable
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rser)


                        SqlConn.Close()
                        '  y = rser.Rows.Count

                        iLinee = iLinee

                        UpdateTableValue(wsName, rser, 5, iLinee, 3, True)

                        iLinee = iLinee + 1
                    End If


                    Dim x As Integer
                    Dim y As Integer
                    'If dter.Rows.Count > 0 Then
                    '    For er As Integer = 0 To dter.Rows.Count - 1




                    '    Next

                    'End If


                    Dim Maxint As Integer = Math.Max(x, Math.Max(y, Math.Max(ei, ze)))


                    iLinee = iLinee + Maxint + 1

                Next
            End If


            wsName = "Meal Supplement"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", ContractID, 3, True, False)


            Dim dtmr As New DataTable
            strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header  where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtmr)
            Dim iLinmr As Integer = 7
            Dim m7 As Integer
            Dim s7 As Integer
            Dim e7 As Integer
            Dim w7 As Integer
            Dim mn7 As Integer
            Dim tm7 As Integer
            If dtmr.Rows.Count > 0 Then
                ' Dim conn As New ADODB.Connection
                For i As Integer = 0 To dtmr.Rows.Count - 1

                    UpdateValue(wsName, "A" & iLinmr.ToString, "Supplement ID", 2, True, False)
                    UpdateValue(wsName, "B" & iLinmr.ToString, "Applicable To", 2, True, False)
                    iLinmr = iLinmr + 1

                    strSqlQry = "select mealsupplementid,applicableto from view_contracts_mealsupp_header where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
                    Dim rsm7 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsm7)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsm7, 0, iLinmr, 3, True)



                    m7 = rsm7.Rows.Count

                    iLinmr = iLinmr + m7 + 1

                    UpdateValue(wsName, "A" & iLinmr.ToString, "Season", 2, True, False)
                    UpdateValue(wsName, "B" & iLinmr.ToString, "From Date", 2, True, False)
                    UpdateValue(wsName, "C" & iLinmr.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "D" & iLinmr.ToString, "Manual From Date not linked to Seasons", 2, True, False)
                    UpdateValue(wsName, "E" & iLinmr.ToString, "Manual To Date not linked to Seasons", 2, True, False)

                    UpdateValue(wsName, "F" & iLinmr.ToString, "Excluded From Date", 2, True, False)
                    UpdateValue(wsName, "G" & iLinmr.ToString, "Excluded To Date", 2, True, False)
                    UpdateValue(wsName, "H" & iLinmr.ToString, "Exclusive For", 2, True, False)


                    iLinmr = iLinmr + 1


                    Dim rscm As New DataTable
                    strSqlQry = "select  isnull(q.Item1,''), isnull(convert(varchar(10),s.fromdate , 105),''),isnull(convert(varchar(10),s.todate , 105),'') from view_contracts_mealsupp_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & ContractID & "' and h.mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' order by convert(varchar(10),s.fromdate , 111) "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscm)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rscm, 0, iLinmr, 3, True)



                    tm7 = rscm.Rows.Count


                    strSqlQry = "select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from  view_contracts_mealsupp_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
                    Dim rsmc As New DataTable

                    mn7 = rsmc.Rows.Count
                    iLinmr = iLinmr

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmc)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsmc, 3, iLinmr, 3, True)

                    iLinmr = iLinmr
                    Dim rsed As New DataTable

                    strSqlQry = " select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),exclfor from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsed)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsed, 5, iLinmr, 3, True)


                    e7 = rsed.Rows.Count


                    Dim rsw As New DataTable
                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_mealsupp_weekdays  where  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' for xml path('')),1,1,''),'') "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsw)
                    SqlConn.Close()


                    w7 = rsw.Rows.Count
                    Dim Maxint As Integer = Math.Max(tm7, Math.Max(mn7, m7))


                    iLinmr = iLinmr + Maxint + 1

                    UpdateValue(wsName, "B" & iLinmr.ToString, rsw.Rows(0).ToString, 3, True, True, "C" & iLinmr.ToString, True)
                    UpdateTableValue(wsName, rsw, 1, iLinmr, 3, True)
                    UpdateValue(wsName, "A" & iLinmr.ToString, "Days of the week", 2, True, False)





                    '            'If conn.State = ConnectionState.Open Then
                    '            '    conn.Close()
                    '            'End If

                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_mealsupplements]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@mealsupplementid", dtmr.Rows(i)("mealsupplementid").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)
                                'If dtt.Rows(i)(i) = "-3" Then
                                '    "Free"

                                '    "Incl"
                                '    txt.Text = "-1"
                                '    Case "N.Incl"
                                '    txt.Text = "-2"
                                '    Case "N/A"
                                '    txt.Text = "-4"
                                '    Case "On Request"
                                '    txt.Text = "-5"

                            End Using
                        End Using
                    End Using




                    '            Dim rssp7 As New ADODB.Recordset
                    '            rssp7 = convertToADODB(dtt)
                    iLinmr = iLinmr + w7 + 1


                    'Dim name(dtt.Columns.Count) As String
                    Dim ii As Integer = 65
                    'For Each column As DataColumn In dt.Columns
                    'name(ii) = column.ColumnName

                    Dim sss = Chr(ii).ToString
                    For OO As Integer = 0 To dtt.Columns.Count - 1


                        UpdateValue(wsName, sss.ToString() & iLinmr.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)

                        ii += 1
                        UpdateValue(wsName, sss.ToString() & iLinmr.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)

                        sss = Chr(ii).ToString
                    Next





                    'Next


                    If dtt.Rows.Count > 0 Then
                        iLinmr = iLinmr + 1
                        UpdateTableValue(wsName, dtt, 0, iLinmr, 3, True)

                        s7 = dtt.Rows.Count

                    End If


                    iLinmr = iLinmr + s7 + 3

                Next

            End If


            wsName = "Child Policy"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", ContractID, 3, True, False)



            Dim dtcpi As New DataTable
            strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtcpi)
            SqlConn.Close()
            Dim iLine8 As Integer = 7

            If dtcpi.Rows.Count > 0 Then
                For i As Integer = 0 To dtcpi.Rows.Count - 1





                    UpdateValue(wsName, "A" & iLine8.ToString, "ChildPolicy Id", 2, True, False)
                    UpdateValue(wsName, "B" & iLine8.ToString, "Applicable To", 2, True, False)
                    iLine8 = iLine8 + 1




                    Dim ml8 As Integer
                    Dim tm8 As Integer
                    Dim co8 As Integer
                    Dim d8 As Integer
                    Dim chk8 As Integer
                    Dim ei31 As Integer

                    strSqlQry = "select childpolicyid,applicableto from view_contracts_childpolicy_header  where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
                    Dim rs8 As New DataTable

                    chk8 = rs8.Rows.Count
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs8)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rs8, 0, iLine8, 3, True)

                    'xlSheet.Range("A" & iLine8.ToString) = ""

                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_childpolicy_weekdays where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' for xml path('')),1,1,''),'') "
                    Dim rss8 As New DataTable

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rss8)
                    SqlConn.Close()

                    iLine8 = iLine8 + chk8 + 2
                    UpdateValue(wsName, "B" & iLine8.ToString, rss8.Rows(0).ToString, 3, True, True, "C" & iLine8.ToString, True)
                    UpdateTableValue(wsName, rss8, 1, iLine8, 3, True)

                    UpdateValue(wsName, "A" & iLine8.ToString, "Days of the week", 2, True, False)


                    iLine8 = iLine8 + 1

                    ''shahul 21/03/17

                    UpdateValue(wsName, "A" & iLine8.ToString, "Season", 2, True, False)
                    UpdateValue(wsName, "B" & iLine8.ToString, "From Date", 2, True, False)
                    UpdateValue(wsName, "C" & iLine8.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "D" & iLine8.ToString, "Manual From Date not linked to Seasons", 2, True, False)
                    UpdateValue(wsName, "E" & iLine8.ToString, "Manual To Date not linked to Seasons", 2, True, False)
                    '    UpdateValue(wsName, "F" & iLine8.ToString, "Days of the week", 2, True, False)

                    iLine8 = iLine8 + 1
                    ''shahul 21/03/17
                    strSqlQry = "select  isnull(q.Item1,''), isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from view_contracts_childpolicy_header h  " _
                        & " cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & ContractID & "' and h.childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' order by convert(varchar(10),fromdate , 111) "
                    Dim rscp As New DataTable

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    SqlConn.Close()
                    tm8 = rscp.Rows.Count

                    'iLine8 = iLine8 + 2
                    UpdateTableValue(wsName, rscp, 0, iLine8, 3, True)

                    ''shahul 21/03/17

                    'xlSheet.Range("D" & iLine8 - 1.ToString).WrapText = True




                    ''shahul 21/03/17
                    strSqlQry = "select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from  view_contracts_childpolicy_dates where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' order by convert(varchar(10),fromdate , 111) "
                    Dim rsm8 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsm8)
                    SqlConn.Close()
                    co8 = rsm8.Rows.Count
                    iLine8 = iLine8 + 1
                    UpdateTableValue(wsName, rsm8, 3, iLine8, 3, True)  ''shahul 21/03/17



                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_childpolicy]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@childpolicyid", dtcpi.Rows(i)("childpolicyid").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)

                            End Using
                        End Using
                    End Using


                    '            Dim rssp721 As New ADODB.Recordset
                    '            rssp721 = convertToADODB(dtt)
                    If dtt.Rows.Count > 0 Then
                        Dim Maxint As Integer = Math.Max(chk8, Math.Max(co8, tm8))


                        iLine8 = iLine8 + Maxint + 1

                        Dim ii2 As Integer = 65
                        'For Each column As DataColumn In dt.Columns
                        'name(ii) = column.ColumnName

                        Dim sss2 = Chr(ii2).ToString

                        For OO As Integer = 0 To dtt.Columns.Count - 1
                            '                    xlSheet.Range(sss2.ToString() + iLine8.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
                            ii2 += 1
                            UpdateValue(wsName, sss2.ToString() & iLine8.ToString, dtt.Columns(OO).ColumnName.ToString(), 2, True, False)
                            'xlSheet.Range(sss2.ToString() + (iLine8).ToString).FONT.BOLD = True
                            sss2 = Chr(ii2).ToString

                        Next


                        iLine8 = iLine8 + 1
                        UpdateTableValue(wsName, dtt, 0, iLine8, 3, True)
                        'xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rssp721)
                        ei31 = dtt.Rows.Count
                    End If

                    iLine8 = iLine8 + ei31 + 4

                Next

            End If


            wsName = "Cancellation Policy"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)


            Dim dtcn As New DataTable
            strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtcn)
            Dim iLinecr2 As Integer = 8





            If dtcn.Rows.Count > 0 Then
                For i As Integer = 0 To dtcn.Rows.Count - 1

                    Dim rm2 As Integer
                    Dim ml2 As Integer

                    Dim co2 As Integer
                    Dim ce2 As Integer

                    Dim ns As Integer

                    UpdateValue(wsName, "A" & iLinecr2.ToString, "Cancellation ID", 2, True, False)
                    UpdateValue(wsName, "B" & iLinecr2.ToString, "Applicable To", 2, True, False)
                    UpdateValue(wsName, "C" & iLinecr2.ToString, "Season", 2, True, False)
                    UpdateValue(wsName, "D" & iLinecr2.ToString, "Pricelist From Date", 2, True, False)
                    UpdateValue(wsName, "E" & iLinecr2.ToString, "Pricelist To Date", 2, True, False)
                    'Shahul 21/03/17
                    ' UpdateValue(wsName, "F" & iLinecr2.ToString, "Exhibition Code", 2, True, False)
                    UpdateValue(wsName, "F" & iLinecr2.ToString, "Exhibition Name", 2, True, False)
                    UpdateValue(wsName, "G" & iLinecr2.ToString, "From", 2, True, False)
                    UpdateValue(wsName, "H" & iLinecr2.ToString, "To", 2, True, False)


                    '' shahul 21/03/17
                    strSqlQry = "select  d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & ContractID & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' order by convert(varchar(10),s.fromdate , 111)"
                    Dim rscp As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    SqlConn.Close()
                    iLinecr2 = iLinecr2 + 1
                    co2 = rscp.Rows.Count
                    UpdateTableValue(wsName, rscp, 0, iLinecr2, 3, True)

                    ''' shahul 21/03/17
                    strSqlQry = "select m.exhibitionname,isnull(convert(varchar(10),d.fromdate,105),''),isnull(convert(varchar(10),d.todate,105),'') from view_contracts_cancelpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.exhibitions,',') e inner join view_contracts_exhibition_detail d on e.Item1=d.exhibitioncode inner join view_contracts_exhibition_header eh on d.exhibitionid=eh.exhibitionid and eh.contractid=h.contractid inner join exhibition_master m on d.exhibitioncode=m.exhibitioncode where h.contractid='" & ContractID & "' and h.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
                    Dim rs2e As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs2e)
                    SqlConn.Close()
                    ce2 = rs2e.Rows.Count
                    'Shahul 21/03/17
                    UpdateTableValue(wsName, rs2e, 5, iLinecr2, 3, True)
                    If ce2 Or co2 <> 0 Then
                        If ce2 > co2 Then
                            iLinecr2 = iLinecr2 + ce2 + 2
                        ElseIf co2 > ce2 Then
                            iLinecr2 = iLinecr2 + co2 + 2
                        End If
                    Else
                        iLinecr2 = iLinecr2 + 2
                    End If

                    UpdateValue(wsName, "A" & iLinecr2.ToString, "Room Type", 2, True, False)
                    UpdateValue(wsName, "B" & iLinecr2.ToString, "Meal Plan", 2, True, False)
                    UpdateValue(wsName, "C" & iLinecr2.ToString, "From No.of Days or Hours ", 2, True, False)
                    UpdateValue(wsName, "D" & iLinecr2.ToString, "To No.of Days or Hours", 2, True, False)
                    UpdateValue(wsName, "E" & iLinecr2.ToString, "Units Days or Hours", 2, True, False)
                    UpdateValue(wsName, "F" & iLinecr2.ToString, "Charge Basis", 2, True, False)
                    UpdateValue(wsName, "G" & iLinecr2.ToString, "Nights to charge", 2, True, False)
                    UpdateValue(wsName, "H" & iLinecr2.ToString, "Percentage to charge", 2, True, False)
                    UpdateValue(wsName, "I" & iLinecr2.ToString, "Value to charge", 2, True, False)
                    iLinecr2 = iLinecr2 + 1

                    Dim dtcl As New DataTable
                    strSqlQry = "select clineno from view_contracts_cancelpolicy_detail  where cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtcl)


                    Dim rs2r As New DataTable

                    If dtcl.Rows.Count > 0 Then
                        For cl As Integer = 0 To dtcl.Rows.Count - 1

                            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_header  h join view_contracts_search v on h.contractid =v.contractid join view_contracts_cancelpolicy_detail d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and clineno='" & dtcl.Rows(cl)("clineno").ToString & "' for xml path('')),1,1,''),'') "

                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rs2r)
                            SqlConn.Close()


                        Next
                    End If
                    rm2 = rs2r.Rows.Count
                    UpdateTableValue(wsName, rs2r, 0, iLinecr2, 3, True)


                    strSqlQry = "select mealplans,fromnodayhours,nodayshours, dayshours,isnull(chargebasis,'')chargebasis,isnull(nightstocharge,0), percentagetocharge,valuetocharge from view_contracts_cancelpolicy_detail where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                    Dim rsr2 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsr2)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsr2, 1, iLinecr2, 3, True)

                    ml2 = rsr2.Rows.Count


                    If ml2 Or rm2 <> 0 Then
                        If ml2 > rm2 Then
                            iLinecr2 = iLinecr2 + ml2 + 2

                        Else
                            iLinecr2 = iLinecr2 + rm2 + 2
                        End If
                    Else

                        iLinecr2 = iLinecr2 + 2

                    End If

                    ''shahul 21/03/17
                    strSqlQry = "select noshowearly from view_contracts_cancelpolicy_noshowearly  where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"



                    Dim dtns As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtns)
                    SqlConn.Close()

                    ' ''Shahul 21/03/17
                    'strSqlQry = "select mealplans,noshowearly,chargebasis,nightstocharge,percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                    'Dim rsr21 As New DataTable
                    'Dim ml21 As Integer
                    'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    'myDataAdapter.Fill(rsr21)
                    'SqlConn.Close()

                    'ml21 = rsr21.Rows.Count



                    UpdateValue(wsName, "A" & iLinecr2.ToString, "Room Type", 2, True, False)
                    UpdateValue(wsName, "B" & iLinecr2.ToString, "Meal Plan", 2, True, False)

                    UpdateValue(wsName, "C" & iLinecr2.ToString, "No Show/Early Checkout", 2, True, False)

                    UpdateValue(wsName, "D" & iLinecr2.ToString, "Charge Basis", 2, True, False)
                    UpdateValue(wsName, "E" & iLinecr2.ToString, "Nights to charge", 2, True, False)
                    UpdateValue(wsName, "F" & iLinecr2.ToString, "Percentage to charge", 2, True, False)
                    UpdateValue(wsName, "G" & iLinecr2.ToString, "Value to charge", 2, True, False)


                    iLinecr2 = iLinecr2 + 1
                    'UpdateTableValue(wsName, rsr21, 1, iLinecr2, 3, True)

                    If dtns.Rows.Count > 0 Then
                        For i2 As Integer = 0 To dtns.Rows.Count - 1

                            strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & PARTYCODE & "'  for xml path('')),1,1,''),''), mealplans,d.noshowearly,chargebasis,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
                            Dim rsrns4 As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsrns4)
                            SqlConn.Close()

                            ns = rsrns4.Rows.Count
                            UpdateTableValue(wsName, rsrns4, 0, iLinecr2, 3, True)

                            iLinecr2 = iLinecr2 + 1
                        Next
                    End If



                    iLinecr2 = iLinecr2 + ns + 3
                Next
            End If



            wsName = "CheckInCheckOutpolicy"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)





            Dim dtc As New DataTable
            strSqlQry = "select checkinoutpolicyid from view_contracts_checkinout_header  where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtc)
            Dim linelbct As Integer = 7

            If dtc.Rows.Count > 0 Then
                For i As Integer = 0 To dtc.Rows.Count - 1

                    Dim rm As Integer
                    Dim ml As Integer
                    Dim tm As Integer
                    Dim co As Integer
                    Dim de As Integer
                    Dim chk As Integer

                    UpdateValue(wsName, "A" & linelbct.ToString, "CheckIn/OutPolicyId", 2, True, False)
                    UpdateValue(wsName, "B" & linelbct.ToString, "Applicable To", 2, True, False)
                    UpdateValue(wsName, "C" & linelbct.ToString, "Season", 2, True, False)
                    UpdateValue(wsName, "D" & linelbct.ToString, "Pricelist From Date", 2, True, False)
                    UpdateValue(wsName, "E" & linelbct.ToString, "Pricelist To Date", 2, True, False)

                    linelbct = linelbct + 1


                    strSqlQry = "select  d.checkinoutpolicyid,d.applicableto,q.Item1 seasons, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105)  from view_contracts_checkinout_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q  join  view_contractseasons s on s.SeasonName=q.Item1 and  s.contractid= '" & ContractID & "' and  d.checkinoutpolicyid  ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' order by convert(varchar(10),s.fromdate , 111) "
                    Dim rscp As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    SqlConn.Close()

                    chk = rscp.Rows.Count

                    UpdateTableValue(wsName, rscp, 0, linelbct, 3, True)
                    strSqlQry = "select checkintime,checkouttime from view_contracts_checkinout_header where checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
                    Dim rsc As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsc)
                    SqlConn.Close()

                    co = rsc.Rows.Count

                    linelbct = linelbct + chk + 1
                    UpdateValue(wsName, "B" & linelbct.ToString, "CheckOut Time", 2, True, False)

                    UpdateValue(wsName, "A" & linelbct.ToString, "CheckIn Time", 2, True, False)
                    linelbct = linelbct + 1
                    UpdateTableValue(wsName, rsc, 0, linelbct, 3, True)


                    strSqlQry = "select isnull(stuff((select ',' + mealcode from view_contracts_checkinout_mealplans where  checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' for xml path('')),1,1,''),'') "

                    Dim rscm As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscm)
                    SqlConn.Close()
                    ml = rscm.Rows.Count


                    linelbct = linelbct + co + 1
                    UpdateValue(wsName, "B" & linelbct.ToString, rscm.Rows(0).ToString, 3, True, True, "D" & linelbct.ToString, True)
                    UpdateTableValue(wsName, rscm, 1, linelbct, 3, True)

                    UpdateValue(wsName, "A" & linelbct.ToString, "Meal Plan", 2, True, False)




                    strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & PARTYCODE & "' for xml path('')),1,1,''),'') "
                    Dim rscr As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscr)
                    SqlConn.Close()
                    rm = rscr.Rows.Count

                    linelbct = linelbct + 2
                    UpdateValue(wsName, "B" & linelbct.ToString, rscr.Rows(0).ToString, 3, True, True, "D" & linelbct.ToString, True)
                    UpdateTableValue(wsName, rscr, 1, linelbct, 3, True)
                    UpdateValue(wsName, "A" & linelbct.ToString, "Room Type", 2, True, False)

                    'xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
                    'xlSheet.Range("e:C").Merge()
                    'xlSheet.Range("B" & linelbct.ToString).wraptext = True


                    strSqlQry = "select checkinouttype,	isnull(fromhours,''),isnull(tohours,''),case when chargeyesno=1 then  'Yes' else 'No' end  chargeyesno,chargetype,percentage,condition,isnull(requestbeforedays,'') from view_contracts_checkinout_detail where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
                    Dim rst As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rst)
                    SqlConn.Close()
                    tm = rst.Rows.Count

                    linelbct = linelbct + 2

                    UpdateValue(wsName, "A" & linelbct.ToString, "CheckIn/CheckoutType", 2, True, False) '' shahul 21/03/17
                    UpdateValue(wsName, "B" & linelbct.ToString, "From", 2, True, False) '' shahul 21/03/17
                    UpdateValue(wsName, "C" & linelbct.ToString, "To", 2, True, False)
                    UpdateValue(wsName, "D" & linelbct.ToString, "Charge Y/N", 2, True, False)
                    UpdateValue(wsName, "E" & linelbct.ToString, "Charge Type", 2, True, False)
                    UpdateValue(wsName, "F" & linelbct.ToString, "Percentage", 2, True, False)
                    UpdateValue(wsName, "G" & linelbct.ToString, "Conditions", 2, True, False)
                    UpdateValue(wsName, "H" & linelbct.ToString, "Requestbeforedays", 2, True, False) '' shahul 21/03/17


                    linelbct = linelbct + 1
                    '            xlSheet.Range("H" & linelbct - 1.ToString).wraptext = True
                    UpdateTableValue(wsName, rst, 0, linelbct, 3, True)
                    '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rst)

                    strSqlQry = "select isnull(datetype,''),isnull(convert(varchar(10),restrictdate, 105),'')restrictdate from view_contracts_checkinout_restricted where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
                    Dim rsd As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsd)
                    SqlConn.Close()
                    de = rsd.Rows.Count

                    If tm = 0 Then
                        linelbct = linelbct + 1
                    Else
                        linelbct = linelbct + tm + 1

                    End If

                    UpdateValue(wsName, "A" & linelbct.ToString, "Date Type", 2, True, False)
                    UpdateValue(wsName, "B" & linelbct.ToString, "Restrict Date", 2, True, False)

                    linelbct = linelbct + 1

                    UpdateTableValue(wsName, rsd, 0, linelbct, 3, True)
                    linelbct = linelbct + de + 3


                Next
            End If



            wsName = "General Policy"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)




            Dim iLineg As Integer = 7


            Dim dtg As New DataTable
            strSqlQry = "select genpolicyid from view_contracts_genpolicy_header  where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtg)
            SqlConn.Close()
            Dim g As Integer

            If dtg.Rows.Count > 0 Then
                For i As Integer = 0 To dtg.Rows.Count - 1

                    UpdateValue(wsName, "A" & iLineg.ToString, "Genpolicy Id", 2, True, False)
                    UpdateValue(wsName, "B" & iLineg.ToString, "Applicable To", 2, True, False)
                    UpdateValue(wsName, "D" & iLineg.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "C" & iLineg.ToString, "From Date", 2, True, False)
                    iLineg = iLineg + 1


                    Dim rsg As New DataTable
                    strSqlQry = "select distinct genpolicyid,applicableto,isnull(fromdate,''),isnull(todate,'') from view_contracts_genpolicy_header  where contractid= '" & ContractID & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsg)
                    SqlConn.Close()
                    g = rsg.Rows.Count

                    UpdateTableValue(wsName, rsg, 0, iLineg, 3, True)

                    Dim rsp As New DataTable
                    Dim y As Integer
                    strSqlQry = "select isnull(policytext,'') from view_contracts_genpolicy_header  where contractid= '" & ContractID & "' and genpolicyid= '" & dtg.Rows(i)("genpolicyid").ToString & "' "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsp)
                    SqlConn.Close()
                    y = rsp.Rows.Count

                    iLineg = iLineg + g + 1



                    UpdateValue(wsName, "A" & iLineg.ToString, "Policy", 2, True, False)
                    iLineg = iLineg + 1
                    UpdateValue(wsName, "A" & iLineg.ToString, rsp.Rows(0).ToString, 3, True, True, "J" & iLineg.ToString, True)

                    UpdateTableValue(wsName, rsp, 0, iLineg, 3, True)



                    'xlSheet.Range("A14:I14").Merge()

                    If g > y Then
                        iLineg = iLineg + g + 3
                    Else

                        iLineg = iLineg + y + 3
                    End If



                Next
            End If



            wsName = "Minimum Nights"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)


            Dim dtm As New DataTable
            strSqlQry = "select minnightsid from view_contracts_minnights_header where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtm)

            SqlConn.Close()

            Dim iLiner As Integer = 7


            If dtm.Rows.Count > 0 Then
                For i As Integer = 0 To dtm.Rows.Count - 1
                    UpdateValue(wsName, "A" & iLiner.ToString, "Minnight Id", 2, True, False)
                    UpdateValue(wsName, "B" & iLiner.ToString, "Applicable To", 2, True, False)
                    UpdateValue(wsName, "C" & iLiner.ToString, "RoomType", 2, True, False)
                    UpdateValue(wsName, "E" & iLiner.ToString, "From Date", 2, True, False)
                    UpdateValue(wsName, "D" & iLiner.ToString, "Meal Plans", 2, True, False)
                    UpdateValue(wsName, "F" & iLiner.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "G" & iLiner.ToString, "Min.Nights", 2, True, False)
                    UpdateValue(wsName, "H" & iLiner.ToString, "Options", 2, True, False)
                    ' UpdateValue(wsName, "I" & iLiner.ToString, "Options", 2, True, False)

                    iLiner = iLiner + 1


                    Dim dtm2 As New DataTable
                    strSqlQry = "select clineno from view_contracts_minnights_detail where minnightsid= '" & dtm.Rows(i)("minnightsid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtm2)
                    SqlConn.Close()
                    Dim x As Integer
                    Dim x1 As Integer

                    If dtm2.Rows.Count > 0 Then
                        For i2 As Integer = 0 To dtm2.Rows.Count - 1
                            Dim rsm1 As New DataTable


                            Dim dtersm As New DataTable
                            strSqlQry = "select distinct roomtypes from view_contracts_minnights_detail  where  clineno='" & dtm2.Rows(i2)("clineno").ToString & "' and minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(dtersm)

                            '            Dim ys2 As Integer
                            '            Dim xs As Integer
                            '            Dim ys As Integer
                            If dtersm.Rows.Count > 0 Then
                                For ers As Integer = 0 To dtersm.Rows.Count - 1

                                    If (dtersm.Rows(ers)("roomtypes").ToString = "All" Or dtersm.Rows(ers)("roomtypes").ToString = "") Then  ''' Shahul 21/03/17
                                        UpdateValue(wsName, "C" & iLiner.ToString, "All", 2, True, False)

                                    Else

                                        strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_minnights_header h join view_contracts_search v on h.contractid =v.contractid join view_contracts_minnights_detail d on h.minnightsid =d.minnightsid  cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode where d.minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'  for xml path('')),1,1,''),'') "
                                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                        myDataAdapter.Fill(rsm1)
                                        SqlConn.Close()
                                        x1 = rsm1.Rows.Count
                                        UpdateTableValue(wsName, rsm1, 2, iLiner, 3, True)
                                    End If
                                Next
                            End If
                            Dim rsm As New DataTable
                            strSqlQry = "select distinct D.minnightsid ,H.applicableto from view_contracts_minnights_detail d,view_contracts_minnights_header h where d.minnightsid ='" & dtm.Rows(i)("minnightsid").ToString & "' and d.clineno='" & dtm2.Rows(i2)("clineno").ToString & "' and contractid= '" & ContractID & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsm)
                            SqlConn.Close()
                            x = rsm.Rows.Count

                            UpdateTableValue(wsName, rsm, 0, iLiner, 3, True)


                            Dim rsr As New DataTable
                            Dim y As Integer
                            strSqlQry = "select distinct mealplans, isnull(convert(varchar(10),convert(date,fromdate) , 105),'') ,isnull(convert(varchar(10),convert(date,todate) , 105),''),isnull(minnights,''),nightsoption from view_contracts_minnights_detail where minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsr)
                            SqlConn.Close()

                            y = rsr.Rows.Count
                            UpdateTableValue(wsName, rsr, 3, iLiner, 3, True)

                            iLiner = iLiner + 1
                        Next

                    End If
                    iLiner = iLiner + 2
                Next
            End If


            wsName = "SplEventsPriceList"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)



            Dim iLines As Integer = 7


            Dim dts As New DataTable
            strSqlQry = ""
            strSqlQry = "select splistcode from  view_contracts_specialevents_header where contractid= '" & ContractID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dts)

            If dts.Rows.Count > 0 Then
                For i As Integer = 0 To dts.Rows.Count - 1

                    Dim rsp1 As New DataTable
                    strSqlQry = ""
                    strSqlQry = "select isnull(remarks,'') remarks,case when isnull(compulsory,0)=0 then 'All Compulsory' else case when  isnull(compulsory,0)=1 then 'Any One Compulsory' else 'Optional in Special Events' end end " _
                        & " compulsory from view_contracts_specialevents_header  where contractid= '" & ContractID & "' and splistcode= '" & dts.Rows(i)("splistcode").ToString & "' "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsp1)
                    SqlConn.Close()


                    iLines = iLines + g + 1

                    UpdateValue(wsName, "A" & iLines.ToString, "Compulsory", 2, True, False)
                    iLines = iLines + 1

                    UpdateValue(wsName, "A" & iLines.ToString, rsp1.Rows(0)("compulsory").ToString, 3, True, True, "B" & iLines.ToString, True)

                    iLines = iLines + 2

                    UpdateValue(wsName, "A" & iLines.ToString, "Remarks", 2, True, False)
                    iLines = iLines + 1
                    UpdateValue(wsName, "A" & iLines.ToString, rsp1.Rows(0)("remarks").ToString, 3, True, True, "J" & iLines.ToString, True)

                    iLines = iLines + 2


                    UpdateValue(wsName, "A" & iLines.ToString, "Splistcode", 2, True, False)
                    UpdateValue(wsName, "B" & iLines.ToString, "Applicable To", 2, True, False)
                    UpdateValue(wsName, "C" & iLines.ToString, "From Date", 2, True, False)
                    UpdateValue(wsName, "D" & iLines.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "E" & iLines.ToString, "SplEvents Name", 2, True, False)
                    UpdateValue(wsName, "F" & iLines.ToString, "RoomType", 2, True, False)
                    UpdateValue(wsName, "G" & iLines.ToString, "Meal Plans", 2, True, False)
                    UpdateValue(wsName, "H" & iLines.ToString, "Room Occupancy", 2, True, False)
                    UpdateValue(wsName, "I" & iLines.ToString, "Adult Rate", 2, True, False)

                    UpdateValue(wsName, "J" & iLines.ToString, "Child Age From", 2, True, False)
                    UpdateValue(wsName, "K" & iLines.ToString, "Child Age To", 2, True, False)
                    UpdateValue(wsName, "L" & iLines.ToString, "Child Rate", 2, True, False)
                    UpdateValue(wsName, "M" & iLines.ToString, "Remarks", 2, True, False)
                    '   UpdateValue(wsName, "J" & iLines.ToString, "Child Age From,Child Age To,Child Rate", 2, True, False)
                    iLines = iLines + 1

                    Dim x As Integer




                    Dim dters As New DataTable
                    strSqlQry = ""
                    strSqlQry = "select distinct roomtypes,mealplans,splineno,roomcategory,adultrate from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "' order by splineno "
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dters)

                    Dim ys2 As Integer
                    Dim xs As Integer
                    Dim ys As Integer
                    Dim y As Integer

                    If dters.Rows.Count > 0 Then
                        For ers As Integer = 0 To dters.Rows.Count - 1


                            strSqlQry = ""
                            strSqlQry = "select  h.splistcode,h.applicableto,isnull(convert(varchar(10),fromdate , 103),'') ,isnull(convert(varchar(10),todate , 103),''),p.spleventname  " _
                                & " from view_contracts_specialevents_detail d join party_splevents  p on d.spleventcode= p.spleventcode join view_contracts_specialevents_header h on h.splistcode=d.splistcode " _
                                & " and p.partycode= '" & PARTYCODE & "'  and d.splistcode='" & dts.Rows(i)("splistcode").ToString & "' and  splineno=" & dters.Rows(ers)("splineno").ToString & " order by splineno"
                            '" & dtm.Rows(i)("minnightsid").ToString & "'"
                            Dim rsms As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsms)
                            SqlConn.Close()
                            x = rsms.Rows.Count
                            UpdateTableValue(wsName, rsms, 0, iLines, 3, True)


                            strSqlQry = ""
                            strSqlQry = "select  adultrate from view_contracts_specialevents_detail where splistcode='" & dts.Rows(i)("splistcode").ToString & "' and splineno=" & dters.Rows(ers)("splineno").ToString & " order by splineno"

                            Dim rsr1 As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsr1)
                            SqlConn.Close()
                            iLines = iLines


                            UpdateTableValue(wsName, rsr1, 8, iLines, 3, True)



                            strSqlQry = ""

                            '' strSqlQry = "select distinct adultrate,childdetails from view_contracts_specialevents_detail where splistcode='" & dts.Rows(i)("splistcode").ToString & "'"

                            'changed by danny
                            'strSqlQry = " select    ce.Item1 childagefrom, cr.Item1 childageto," _
                            '    & "  case cv.Item1 when -1 then 'Incl' when -2 then 'N/Incl' when -3 then 'Free' when -4 then 'N/A'   when -5 then 'On Request' else cv.Item1 end childrate  " _
                            '    & "  from view_contracts_specialevents_detail d cross apply dbo.SplitString1colsWithOrderField(d.childdetails,';') cd " _
                            '    & " cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') ce cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') cr  cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') cv " _
                            '    & " where splistcode='" & dts.Rows(i)("splistcode").ToString & "' and splineno=" & dters.Rows(ers)("splineno").ToString & "  and ce.ordno=1 and cr.ordno=2 and cv.ordno=3 order by splineno,ce.Item1"
                            strSqlQry = " select    ce.Item1 childagefrom, cr.Item1 childageto," _
                                & "  case cv.Item1 when '-1' then 'Incl' when '-2' then 'N/Incl' when '-3' then 'Free' when '-4' then 'N/A'   when '-5' then 'On Request' else cv.Item1 end childrate  " _
                                & "  from view_contracts_specialevents_detail d cross apply dbo.SplitString1colsWithOrderField(d.childdetails,';') cd " _
                                & " cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') ce cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') cr  cross apply dbo.SplitString1colsWithOrderField(cd.item1,',') cv " _
                                & " where splistcode='" & dts.Rows(i)("splistcode").ToString & "' and splineno=" & dters.Rows(ers)("splineno").ToString & "  and ce.ordno=1 and cr.ordno=2 and cv.ordno=3 order by splineno,ce.Item1"

                            Dim rsr As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsr)
                            SqlConn.Close()
                            iLines = iLines
                            y = rsr.Rows.Count

                            UpdateTableValue(wsName, rsr, 9, iLines, 3, True)


                            strSqlQry = ""
                            strSqlQry = "select  detailremarks from view_contracts_specialevents_detail where splistcode='" & dts.Rows(i)("splistcode").ToString & "' and splineno=" & dters.Rows(ers)("splineno").ToString & " order by splineno"

                            Dim rsr2 As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsr2)
                            SqlConn.Close()
                            iLines = iLines


                            UpdateTableValue(wsName, rsr2, 12, iLines, 3, True)



                            If dters.Rows(ers)("roomtypes").ToString = "All" Then




                                UpdateValue(wsName, "F" & iLines.ToString, "All", 2, True, False)

                                iLines = iLines

                            Else

                                strSqlQry = "select isnull(stuff((select ',' +   p.rmtypname from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join " _
                                    & " partyrmtyp p on q.Item1=p.rmtypcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and  splineno=" & dters.Rows(ers)("splineno").ToString & " and  " _
                                    & " p.partycode ='" & PARTYCODE & "'  for xml path('')),1,1,''),'') "
                                Dim rsers As New DataTable
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rsers)
                                SqlConn.Close()
                                UpdateTableValue(wsName, rsers, 5, iLines, 3, True)

                                ys = rsers.Rows.Count
                                iLines = iLines
                                'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
                                iLines = iLines
                            End If


                            If dters.Rows(ers)("mealplans").ToString = "All" Then


                                UpdateValue(wsName, "G" & iLines.ToString, "All", 2, True, False)
                                iLines = iLines

                            Else

                                strSqlQry = "select isnull(stuff((select ',' +  mealplans from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and splineno ='" & dters.Rows(ers)("splineno").ToString & "' for xml path('')),1,1,''),'') "
                                Dim rserss As New DataTable
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rserss)
                                SqlConn.Close()

                                UpdateTableValue(wsName, rserss, 6, iLines, 3, True)
                                ys2 = rserss.Rows.Count

                                'xlSheet.Range("G" & iLines.ToString).WrapText = True

                                'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
                                iLines = iLines
                            End If

                            If dters.Rows(ers)("roomcategory").ToString = "All" Then


                                UpdateValue(wsName, "H" & iLines.ToString, "All", 2, True, False)
                                iLines = iLines
                            Else
                                Dim rsersr As New DataTable
                                strSqlQry = "select isnull(stuff((select ',' +   p.rmcatcode from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomcategory,',') q join partyrmcat p  " _
                                    & " on q.Item1=p.rmcatcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and  d.splineno=" & dters.Rows(ers)("splineno").ToString & " and " _
                                    & " p.partycode ='" & PARTYCODE & "'   for xml path('')),1,1,''),'') "
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rsersr)
                                SqlConn.Close()

                                UpdateTableValue(wsName, rsersr, 7, iLines, 3, True)


                                'xlSheet.Range("H" & iLines.ToString).WrapText = True
                                'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
                                'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()

                            End If

                            iLines = iLines + 1 + y
                        Next

                    End If

                    If x > y Then
                        iLines = iLines + x + 2
                    Else
                        iLines = iLines + y + 2

                    End If
                    'Dim z As Integer = Maxcount(x, y)
                Next
            End If


            wsName = "Construction"
            UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", ContractID, 3, True, False)





            Dim iLine5 As Integer = 7
            Dim dt4 As New DataTable
            strSqlQry = "select h.constructionid from hotels_construction h join partymast p on p.partycode=h.partycode and p.partyname='" & ViewState("hotelname") & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt4)

            If dt4.Rows.Count > 0 Then
                For i As Integer = 0 To dt4.Rows.Count - 1
                    UpdateValue(wsName, "A" & iLine5.ToString, "Construction Id", 2, True, False)
                    UpdateValue(wsName, "B" & iLine5.ToString, "From Date", 2, True, False)
                    UpdateValue(wsName, "C" & iLine5.ToString, "To Date", 2, True, False)
                    UpdateValue(wsName, "D" & iLine5.ToString, "Reason", 2, True, False)
                    Dim x As Integer
                    iLine5 = iLine5 + 1

                    Dim rs5 As New DataTable
                    strSqlQry = "select constructionid, isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),isnull(Reason,'') from hotels_construction where constructionid  ='" & dt4.Rows(i)("constructionid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs5)
                    SqlConn.Close()

                    UpdateTableValue(wsName, rs5, 0, iLine5, 3, True)
                    x = rs5.Rows.Count

                    iLine5 = iLine5 + x + 2
                Next
            End If

            '''''''''''' start Commented taking time
            '''' Final Calculated Rate
            ' '''' Final Calculated Rate
            'wsName = "Final Calculated Rates"
            'UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            'UpdateValue(wsName, "B5",contractid, 3, True, False)

            'Dim sellingexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_final_contracted_rates where contractid='" &contractid & "'")
            'If sellingexists <> "" Then


            '    ' strSqlQry = "select distinct plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" &contractid & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"
            '    ''' shahul 21/03/17
            '    strSqlQry = "select  plistcode,rmtypname as roomtype , rmcatcode as roomcategory,view_final_contracted_rates.mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb, " _
            '        & " noofadulteb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,  " _
            '        & " totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates,mealmast m where   " _
            '        & " view_final_contracted_rates.mealcode=m.mealcode and contractid = '" &contractid & "' order by rmtyporder,rmcatorder,agecombination,pfromdate,m.rankorder"


            '    Dim rsrf As New DataTable
            '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            '    myDataAdapter.Fill(rsrf)
            '    SqlConn.Close()
            '    Dim yf As Integer
            '    Dim iLinesf As Integer = 8
            '    yf = rsrf.Rows.Count
            '    '        xlSheet.Range("J" & iLinesf - 1.ToString).wraptext = True

            '    '        xlSheet.Range("AA" & iLinesf - 1.ToString).wraptext = True
            '    UpdateTableValue(wsName, rsrf, 0, iLinesf, 3, True)
            'End If


            ''        ''''''''End Final Calculated Rate

            ' ''        '--- Contract Rates for Other Meal Plan
            'wsName = "Contract Rates-Other Meal Plan"
            'UpdateValue(wsName, "B4", ViewState("hotelname"), 3, True, False)
            'UpdateValue(wsName, "B5",contractid, 3, True, False)
            'Dim iLinesfo As Integer = 8
            ' ''' shahul 21/03/17
            'strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,othmealcode as mealplan,mealcode basemeal ,accommodationid,agecombination,adults,child,totalpaxwithinpricepax, " _
            '    & "maxeb,noofadulteb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,mealsupplementid,adultmealprice,adultmealrmcatdetails, " _
            '    & " totalchildmealcharge,childmealdetails,totalprice,nights,minstay,minstayoption,commissionformulaid, " _
            '    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder,othmealcode from view_final_contracted_rates_othmeal where contractid = '" &contractid & "' order by rmtyporder, " _
            '    & " rmcatorder,agecombination,pfromdate"
            'Dim rsrfo As New DataTable
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(rsrfo)
            'SqlConn.Close()



            'Dim yfoo As Integer

            'yfoo = rsrfo.Rows.Count


            'If yfoo > 0 Then

            '    UpdateTableValue(wsName, rsrfo, 0, iLinesfo, 3, True)
            'End If

            ''''''''''''  end Commented taking time

            document.Close()

            '    '''''''''''
            FolderPath = "~\ExcelTemp\"

            GC.Collect()
            GC.WaitForPendingFinalizers()


            '    'DownloadFiles.aspx

            Dim strpop As String
            strpop = "window.open('PriceListModule/DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)



            objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Exported succesfully : ", Session("GlobalUserName"))

        Catch ex As Exception
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub



    Private Sub OFFERSEXCELPRINT(ByVal OFFERID As String, ByVal PARTYCODE As String)








        Try
            Dim xlApp
            Dim xlBook
            Dim xlSheet

            Dim FolderPath As String = "~\ExcelTemplates\"
            Dim FileName As String = "OfferPrint.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "OfferPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            document = SpreadsheetDocument.Open(outputPath, True)
            wbPart = document.WorkbookPart




            Dim activestate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(activestate,'') activestate  from view_Offer_search where promotionid='" & OFFERID & "'")

            Dim wsName As String = "Index"

            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", OFFERID, 3, True, False)
            UpdateValue(wsName, "B5", activestate, 3, True, False)


            wsName = "Main Detail"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)
            'UpdateValue(wsName, "B5", OFFERID, 3, True, False)
            'UpdateValue(wsName, "A6", "From Date", 3, True, False)



            Dim dt31 As New DataTable

            strSqlQry = "select distinct q.item1 as promotiontypes FROM view_offers_header cross apply dbo.SplitString1colsWithOrderField(promotiontypes,',') q  where  promotionid= '" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt31)
            SqlConn.Close()
            Dim rsi As New DataTable
            Dim iLineNo1m As Integer = 6
            UpdateValue(wsName, "A" & iLineNo1m.ToString, "PromotionId", 3, True, False)
            UpdateValue(wsName, "B" & iLineNo1m.ToString, "Promotion Name", 3, True, False)
            UpdateValue(wsName, "C" & iLineNo1m.ToString, "Applicable To", 3, True, False)

            iLineNo1m = iLineNo1m + 1

            strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto from view_offers_header h where  h.partycode='" & PARTYCODE & "' and promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsi)
            SqlConn.Close()
            UpdateTableValue(wsName, rsi, 0, iLineNo1m, 2, True)
            Dim Y As Integer

            Y = rsi.Rows.Count




            '    xlSheet.Range("C" & iLineNo1m - 1.ToString).WRAPTEXT = True

            'UpdateValue(wsName, "A8", "Countries", 3, True, False)
            iLineNo1m = iLineNo1m + 2

            Dim dto As New DataTable
            strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_offers_countries v where v.ctrycode =p.ctrycode  and v.promotionid ='" & OFFERID & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dto)
            SqlConn.Close()
            If dto.Rows.Count > 0 Then

                UpdateValue(wsName, "A" & iLineNo1m.ToString, "Countries", 3, True, False)
                iLineNo1m = iLineNo1m + 1
                For i As Integer = 0 To dto.Rows.Count - 1

                    UpdateValue(wsName, "A" & iLineNo1m.ToString, dto.Rows(0)("Country").ToString, 3, True, True, "F" & iLineNo1m.ToString, True)
                Next
            End If



            Dim rt As Integer
            iLineNo1m = iLineNo1m + 3
            UpdateValue(wsName, "A" & iLineNo1m.ToString, "Promotion Types", 3, True, False)
            Dim rso As New DataTable
            strSqlQry = "select promotiontypes from view_offers_header where promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rso)
            SqlConn.Close()
            UpdateTableValue(wsName, rso, 1, iLineNo1m, 2, True)



            'xlSheet.Range("B" & iLineNo1m).WrapText = True
            UpdateValue(wsName, "C" & iLineNo1m.ToString, "Days Of The Week", 3, True, False)

            strSqlQry = "SELECT daysoftheweek from view_offers_header where promotionid='" & OFFERID & "'"
            Dim rsdd As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdd)
            SqlConn.Close()
            rt = rsdd.Rows.Count
            If rt > 0 Then
                UpdateTableValue(wsName, rsdd, 3, iLineNo1m, 2, True)
                'xlSheet.Range("D" & iLineNo1m.ToString).WrapText = True
            End If
            iLineNo1m = iLineNo1m + 2

            Dim e2 As Integer

            Dim xg As Integer


            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1

                    If dt31.Rows(i)("promotiontypes") = "Early Bird Discount" Then
                        e2 = 1 + e2

                    ElseIf dt31.Rows(i)("promotiontypes") = "Free Nights" Then
                        e2 = e2 + 1


                    Else
                        e2 = e2
                    End If
                Next
            End If
            Dim rsdd222 As New DataTable
            Dim filldt As Integer = 0
            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If e2 = 1 Then


                        If e2 = 1 And dt31.Rows(i)("promotiontypes") = "Early Bird Discount" Then


                            UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "From Date", 3, True, False)
                            UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "To Date", 3, True, False)
                            UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Booking Code", 3, True, False)
                            UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Discount Type", 3, True, False)
                            UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Discount % or Value", 3, True, False)
                            UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, "Additional Discount % Or Value", 3, True, False)
                            UpdateValue(wsName, "G" & iLineNo1m - 1.ToString, "Booking Validity Options", 3, True, False)
                            UpdateValue(wsName, "H" & iLineNo1m - 1.ToString, "Booking Validity From/Book By", 3, True, False)
                            UpdateValue(wsName, "I" & iLineNo1m - 1.ToString, "Booking Validity To", 3, True, False)
                            UpdateValue(wsName, "J" & iLineNo1m - 1.ToString, "Booking Validity Days/Month", 3, True, False)
                            UpdateValue(wsName, "K" & iLineNo1m - 1.ToString, "Min.Nights", 3, True, False)
                            UpdateValue(wsName, "L" & iLineNo1m - 1.ToString, "Max.Nights", 3, True, False)



                            strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate,isnull(bookingcode,'')bookingcode, " _
                                & " case when isnull(discounttype,'')='Percentage' then '' else discounttype end,case when isnull(discountamount,0)=0 then 0 else discountamount end ,case when isnull(additionalamount,0)=0 then 0 else additionalamount end ,   " _
                              & " bookingvalidityoptions,case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'')='01-01-1900' then ''  else isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'') end  bookingvalidityfromdate,case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'')='01-01-1900' then ''  else isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'') end bookingvaliditytodate ,  " _
                              & " isnull(cast(nullif(bookingvaliditydaysmonths, 0) as varchar(10)),'') bookingvaliditydaysmonths,isnull(cast(nullif(minnights, 0) as varchar(10)),'') minnights,isnull(cast(nullif(maxnights, 0) as varchar(10)),'')  maxnights  from view_offers_DETAIL   " _
                              & " where promotionid='" & OFFERID & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsdd222)
                            SqlConn.Close()
                            UpdateTableValue(wsName, rsdd222, 0, iLineNo1m, 2, True)

                            'xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
                            'xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"


                            xg = rsdd222.Rows.Count


                        ElseIf e2 = 1 And dt31.Rows(i)("promotiontypes") = "Free Nights" Then

                            UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "From Date", 3, True, False)
                            UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "To Date", 3, True, False)
                            UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Booking Code", 3, True, False)
                            UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Booking Validity Options", 3, True, False)
                            UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Booking Validity From/Book By", 3, True, False)
                            UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, "Booking Validity To", 3, True, False)
                            UpdateValue(wsName, "G" & iLineNo1m - 1.ToString, "Booking Validity Days/Month", 3, True, False)
                            UpdateValue(wsName, "H" & iLineNo1m - 1.ToString, "Min.Nights", 3, True, False)
                            UpdateValue(wsName, "I" & iLineNo1m - 1.ToString, "Max.Nights", 3, True, False)
                            UpdateValue(wsName, "J" & iLineNo1m - 1.ToString, "Apply To", 3, True, False)
                            UpdateValue(wsName, "K" & iLineNo1m - 1.ToString, "Allow Multi Stay", 3, True, False)
                            UpdateValue(wsName, "L" & iLineNo1m - 1.ToString, "Stay For", 3, True, False)
                            UpdateValue(wsName, "M" & iLineNo1m - 1.ToString, "Pay For", 3, True, False)
                            UpdateValue(wsName, "N" & iLineNo1m - 1.ToString, "Max FreeNights", 3, True, False)
                            UpdateValue(wsName, "O" & iLineNo1m - 1.ToString, "Max Multiples", 3, True, False)

                            iLineNo1m = iLineNo1m + 1
                            strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate,isnull(bookingcode,'')bookingcode,bookingvalidityoptions,case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'')='01-01-1900' then ''  else  isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'') end bookingvalidityfromdate, " _
                                & " case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'')='01-01-1900' then '' else      isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'') end  bookingvaliditytodate,  " _
                                & " isnull(cast(nullif(bookingvaliditydaysmonths, 0) as varchar(10)),'') bookingvaliditydaysmonths   ,isnull(cast(nullif(minnights, 0) as varchar(10)),'') minnights ,  " _
                                & " isnull(cast(nullif(maxnights, 0) as varchar(10)),'') maxnights ,isnull(applyto,''), isnull(cast(nullif(allowmultistay, 0) as varchar(10)),'') allowmultistay , isnull(cast(nullif(stayfor, 0) as varchar(10)),'') stayfor ,isnull(cast(nullif(payfor, 0) as varchar(10)),'') payfor ,  " _
                                & " isnull(cast(nullif(maxfeenights, 0) as varchar(10)),'') maxfeenights ,isnull(cast(nullif(maxmultiples, 0) as varchar(10)),'') maxmultiples  from view_offers_DETAIL  where promotionid='" & OFFERID & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsdd222)
                            SqlConn.Close()
                            UpdateTableValue(wsName, rsdd222, 0, iLineNo1m, 2, True)
                            'xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
                            'xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
                            xg = rsdd222.Rows.Count
                        End If

                    ElseIf e2 = 2 Then

                        UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "From Date", 3, True, False)
                        UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "To Date", 3, True, False)
                        UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Booking Code", 3, True, False)
                        UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Discount Type", 3, True, False)
                        UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Discount % or Value", 3, True, False)
                        UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, "Additional Discount % Or Value", 3, True, False)
                        UpdateValue(wsName, "G" & iLineNo1m - 1.ToString, "Booking Validity Options", 3, True, False)
                        UpdateValue(wsName, "H" & iLineNo1m - 1.ToString, "Booking Validity From/Book By", 3, True, False)
                        UpdateValue(wsName, "I" & iLineNo1m - 1.ToString, "Booking Validity To", 3, True, False)
                        UpdateValue(wsName, "J" & iLineNo1m - 1.ToString, "Booking Validity Days/Month", 3, True, False)

                        UpdateValue(wsName, "K" & iLineNo1m - 1.ToString, "Min.Nights", 3, True, False)
                        UpdateValue(wsName, "L" & iLineNo1m - 1.ToString, "Max.Nights", 3, True, False)
                        UpdateValue(wsName, "M" & iLineNo1m - 1.ToString, "Apply To", 3, True, False)
                        UpdateValue(wsName, "N" & iLineNo1m - 1.ToString, "Allow Multi Stay", 3, True, False)
                        UpdateValue(wsName, "O" & iLineNo1m - 1.ToString, "Stay For", 3, True, False)
                        UpdateValue(wsName, "P" & iLineNo1m - 1.ToString, "Pay For", 3, True, False)
                        UpdateValue(wsName, "Q" & iLineNo1m - 1.ToString, "Max FreeNights", 3, True, False)
                        UpdateValue(wsName, "R" & iLineNo1m - 1.ToString, "Max Multiples", 3, True, False)


                        iLineNo1m = iLineNo1m + 1



                        strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate ,  " _
                            & " isnull(bookingcode,'')bookingcode,case when isnull(discounttype,'')=0 then '' else discounttype end,case when isnull(discountamount,'')=0 then '' else discountamount end ,  " _
                            & " case when isnull(additionalamount,0)=0 then '' else additionalamount end ,bookingvalidityoptions,case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'')='01-01-1900' then '' else isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'') end bookingvalidityfromdate,  " _
                            & " case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'')='01-01-1900' then '' else     isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'') end  bookingvaliditytodate,  " _
                            & " isnull(cast(nullif(bookingvaliditydaysmonths, 0) as varchar(10)),'') bookingvaliditydaysmonths ,  " _
                            & " isnull(cast(nullif(minnights, 0) as varchar(10)),'') minnights ,isnull(cast(nullif(maxnights, 0) as varchar(10)),'') maxnights ,isnull(applyto,''),case when isnull(allowmultistay,'')=0 then '' else allowmultistay end,  " _
                            & " isnull(cast(nullif(stayfor, 0) as varchar(10)),'') stayfor,isnull(cast(nullif(payfor, 0) as varchar(10)),'') payfor ,isnull(cast(nullif(maxfeenights, 0) as varchar(10)),'') maxfeenights ,isnull(cast(nullif(maxmultiples, 0) as varchar(10)),'')  maxmultiples  from view_offers_DETAIL  where promotionid='" & OFFERID & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdd222)
                        SqlConn.Close()

                        UpdateTableValue(wsName, rsdd222, 0, iLineNo1m, 2, True)



                        'xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
                        'xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
                        xg = rsdd222.Rows.Count


                    Else

                        UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "From Date", 3, True, False)
                        UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "To Date", 3, True, False)
                        UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Booking Code", 3, True, False)
                        UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Booking Validity Options", 3, True, False)
                        UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Booking Validity From/Book By", 3, True, False)
                        UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, "Booking Validity To", 3, True, False)
                        UpdateValue(wsName, "G" & iLineNo1m - 1.ToString, "Booking Validity Days/Month", 3, True, False)
                        UpdateValue(wsName, "H" & iLineNo1m - 1.ToString, "Min.Nights", 3, True, False)
                        UpdateValue(wsName, "I" & iLineNo1m - 1.ToString, "Max.Nights", 3, True, False)

                        strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate,isnull(bookingcode,'')bookingcode,  " _
                            & " bookingvalidityoptions,case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'')='01-01-1900' then '' else isnull(CONVERT(VARCHAR(10), convert(date,bookingvalidityfromdate), 105),'') end bookingvalidityfromdate,  " _
                            & " case when isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'')='01-01-1900' then '' else    isnull(CONVERT(VARCHAR(10), convert(date,bookingvaliditytodate), 105),'') end  bookingvaliditytodate,  " _
                            & " isnull(cast(nullif(bookingvaliditydaysmonths, 0) as varchar(10)),'')   bookingvaliditydaysmonths  ,  " _
                            & " isnull(cast(nullif(minnights, 0) as varchar(10)),'')  minnights ,isnull(cast(nullif(maxnights, 0) as varchar(10)),'') maxnights  from view_offers_detail  where promotionid='" & OFFERID & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        If filldt = 0 Then
                            myDataAdapter.Fill(rsdd222)
                        End If
                        filldt = 1
                        UpdateTableValue(wsName, rsdd222, 0, iLineNo1m, 2, True)
                        SqlConn.Close()

                        xg = rsdd222.Rows.Count
                    End If
                Next

            End If
            Dim xr As Integer
            Dim xu As Integer
            Dim xm1 As Integer
            Dim xm As Integer
            Dim xc As Integer
            Dim xms As Integer
            Dim x2m As Integer
            Dim xa1 As Integer

            iLineNo1m = iLineNo1m + xg + 3
            UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Room Type", 3, True, False)

            Dim rsdd2221 As New DataTable

            strSqlQry = "select  p.rmtypname from view_offers_rmtype d  cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.promotionid='" & OFFERID & "' and p.partycode ='" & PARTYCODE & "' order by rankord"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdd2221)
            SqlConn.Close()


            UpdateTableValue(wsName, rsdd2221, 0, iLineNo1m, 2, True)

            'xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True
            xr = rsdd2221.Rows.Count


            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If dt31.Rows(i)("promotiontypes") = "Room Upgrade" Then

                        Dim rsdd22212 As New DataTable
                        strSqlQry = " select r.rmtypname from view_offers_rmtype s join partyrmtyp r on s.rmtypeupgrade=r.rmtypcode and  ( isnull(s.rmtypeupgrade,'')<>'[Select]' and  isnull(s.rmtypeupgrade,'')<>'') and  s.promotionid='" & OFFERID & "' and r.partycode ='" & PARTYCODE & "' order by rankord"

                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdd22212)
                        SqlConn.Close()
                        UpdateTableValue(wsName, rsdd22212, 1, iLineNo1m, 2, True)

                        'xlSheet.Range("B" & iLineNo1m.ToString).wraptext = True

                        UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "Room Upgrade", 3, True, False)
                        xu = rsdd22212.Rows.Count
                    End If
                Next
            End If
            Dim rsdmm As New DataTable


            strSqlQry = "select  p.mealname from view_offers_meal d  cross apply dbo.SplitString1colsWithOrderField(d.mealcode,',') q join mealmast p on q.Item1=p.mealcode and  d.promotionid='" & OFFERID & "' order by rankorder "
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdmm)
            SqlConn.Close()

            If xu > 0 Then
                UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Meal Type", 3, True, False)
                UpdateTableValue(wsName, rsdmm, 2, iLineNo1m, 2, True)


            Else
                UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "Meal Type", 3, True, False)
                UpdateTableValue(wsName, rsdmm, 1, iLineNo1m, 2, True)
            End If

            xm1 = rsdmm.Rows.Count
            'xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Meal"
            'xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True

            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If dt31.Rows(i)("promotiontypes") = "Meal Upgrade" Then

                        Dim rsdmm1 As New DataTable
                        strSqlQry = "select r.mealname from view_offers_meal s join mealmast r on r.mealcode=s.mealupgrade and ( isnull(s.mealupgrade,'')<>'[Select]' and  isnull(s.mealupgrade,'')<>'') and promotionid='" & OFFERID & "' order by rankorder"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdmm1)
                        SqlConn.Close()
                        If xu > 0 Then
                            UpdateTableValue(wsName, rsdmm1, 3, iLineNo1m, 2, True)
                            UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Meal Upgrade", 3, True, False)
                        Else
                            UpdateTableValue(wsName, rsdmm1, 2, iLineNo1m, 2, True)
                            UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Meal Upgrade", 3, True, False)
                        End If
                        xm = rsdmm1.Rows.Count
                    End If
                Next
            End If
            Dim rsdmma As New DataTable




            strSqlQry = "select  p.rmcatname from view_offers_accomodation d  cross apply dbo.SplitString1colsWithOrderField(d.rmcatcode,',') q join rmcatmast p on q.Item1=p.rmcatcode and  d.promotionid='" & OFFERID & "' order by rankorder"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdmma)
            SqlConn.Close()
            If xu > 0 And xm > 0 Then
                UpdateTableValue(wsName, rsdmma, 4, iLineNo1m, 2, True)
                UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Accomodation", 3, True, False)
            ElseIf xu > 0 And xm = 0 Then
                UpdateTableValue(wsName, rsdmma, 3, iLineNo1m, 2, True)
                UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Accomodation", 3, True, False)
            ElseIf xu = 0 And xm = 0 Then
                UpdateTableValue(wsName, rsdmma, 2, iLineNo1m, 2, True)
                UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Accomodation", 3, True, False)
            ElseIf xu = 0 And xm > 0 Then
                UpdateTableValue(wsName, rsdmma, 3, iLineNo1m, 2, True)
                UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Accomodation", 3, True, False)
            End If

            xc = rsdmma.Rows.Count
            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If dt31.Rows(i)("promotiontypes") = "Accomodation Upgrade" Then

                        Dim rsdmm1a As New DataTable

                        strSqlQry = " select r.rmcatname from view_offers_accomodation s join rmcatmast on r.rmcatcode=s.rmcatupgrade and  ( isnull(s.rmcatupgrade,'')<>'[Select]' and  isnull(s.rmcatupgrade,'')<>'') and promotionid='" & OFFERID & "' order by rankorder"


                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdmm1a)
                        SqlConn.Close()
                        If xu > 0 And xm > 0 Then

                            UpdateTableValue(wsName, rsdmm1a, 5, iLineNo1m, 2, True)
                            UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, "Accomodation Upgrade", 3, True, False)
                            'xlSheet.Range("F" & iLineNo1m.ToString).wraptext = True
                        ElseIf xu > 0 And xm = 0 Then
                            UpdateTableValue(wsName, rsdmm1a, 4, iLineNo1m, 2, True)
                            UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Accomodation Upgrade", 3, True, False)
                        ElseIf xu = 0 And xm = 0 Then
                            UpdateTableValue(wsName, rsdmm1a, 3, iLineNo1m, 2, True)
                            UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Accomodation Upgrade", 3, True, False)
                        ElseIf xu = 0 And xm > 0 Then
                            UpdateTableValue(wsName, rsdmm1a, 4, iLineNo1m, 2, True)
                            UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Accomodation Upgrade", 3, True, False)
                        End If
                        x2m = rsdmm1a.Rows.Count


                    End If


                Next
            End If
            Dim rsdmmam As New DataTable

            strSqlQry = "Select d.rmcatcode from view_offers_supplement d,rmcatmast r   where r.rmcatcode=d.rmcatcode and promotionid='" & OFFERID & "' order by rankorder"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdmmam)
            SqlConn.Close()


            If rsdmmam.Rows.Count > 0 Then



                If x2m = 0 And xu = 0 And xm = 0 Then
                    UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Meal Supplement", 3, True, False)
                    UpdateTableValue(wsName, rsdmmam, 3, iLineNo1m, 2, True)

                End If
                If xu > 0 And xm > 0 And x2m > 0 Then

                    UpdateValue(wsName, "G" & iLineNo1m - 1.ToString, "Meal Supplement", 3, True, False)
                    UpdateTableValue(wsName, rsdmmam, 6, iLineNo1m, 2, True)

                ElseIf xu > 0 And xm = 0 And x2m = 0 Or xu = 0 And xm > 0 And x2m = 0 Or xu = 0 And xm = 0 And x2m > 0 Then

                    UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Meal Supplement", 3, True, False)
                    UpdateTableValue(wsName, rsdmmam, 4, iLineNo1m, 2, True)

                End If

            End If



            xms = rsdmmam.Rows.Count
            Dim Maxint As Integer = Math.Max(xu, Math.Max(xr, Math.Max(xm1, Math.Max(xm, Math.Max(xms, Math.Max(xc, x2m))))))

            'Dim dt2o1 As New DataTable
            'strSqlQry = "select isnull(max(rmcount),0) as rmcount  from view_maxvariable  where partycode= '" & PARTYCODE & "' and promotionid='" & OFFERID & "'"
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(dt2o1)
            'SqlConn.Close()

            iLineNo1m = iLineNo1m + Maxint + 2



            Dim dt2o As New DataTable
            strSqlQry = "select inventorytype,combinetype,commissiontype,isnull(specialoccassion,'') specialoccassion ,remarks,arrivaltransfer,departuretransfer,isnull(applydiscounttype,'') applydiscounttype,isnull(applydiscountids,'') applydiscountids  from view_offers_header where promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt2o)
            SqlConn.Close()
            Dim xd2 As Integer

            If dt2o.Rows.Count > 0 Then


                For i As Integer = 0 To dt2o.Rows.Count - 1
                    UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Inventory Type", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, dt2o.Rows(i)("inventorytype"), 2, True, False)
                    If dt2o.Rows(i)("applydiscounttype") <> "" Then


                        UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Apply Offer To", 3, True, False)
                        UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, dt2o.Rows(i)("applydiscounttype"), 2, True, False)

                    End If
                    If dt2o.Rows(i)("applydiscountids") <> "" Then
                        UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Apply DiscountID", 3, True, False)

                        UpdateValue(wsName, "F" & iLineNo1m - 1.ToString, dt2o.Rows(i)("applydiscountids"), 2, True, False)


                    End If



                    Dim c As Integer
                    iLineNo1m = iLineNo1m + 1


                    UpdateValue(wsName, "A" & iLineNo1m, "Combine", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo1m, dt2o.Rows(i)("combinetype"), 2, True, False)

                    If dt2o.Rows(i)("combinetype") = "Combinable with Specific" Then
                        'xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Hotel"
                        strSqlQry = "Select h.promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.combinableid and c.promotionid='" & OFFERID & "'"
                        Dim rsdmm1ac As New DataTable
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdmm1ac)
                        SqlConn.Close()

                        UpdateValue(wsName, "C" & iLineNo1m, "Promotion Name", 3, True, False)
                        UpdateTableValue(wsName, rsdmm1ac, 3, iLineNo1m, 2, True)


                        'xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True
                        c = 1
                        xd2 = rsdmm1ac.Rows.Count

                    ElseIf dt2o.Rows(i)("combinetype") = "CombinE Mandatory With" Then
                        'xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Hotel"
                        strSqlQry = "Select h.promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.combinableid and c.promotionid='" & OFFERID & "'"
                        Dim rsdmm1ac As New DataTable
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdmm1ac)
                        SqlConn.Close()

                        UpdateValue(wsName, "C" & iLineNo1m, "Promotion Name", 3, True, False)
                        UpdateTableValue(wsName, rsdmm1ac, 3, iLineNo1m, 2, True)

                        'xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True

                        xd2 = rsdmm1ac.Rows.Count
                        c = 3

                    End If


                    If c <> 3 And c <> 1 Then

                        UpdateValue(wsName, "C" & iLineNo1m.ToString, "Commission", 3, True, False)
                        UpdateValue(wsName, "D" & iLineNo1m.ToString, dt2o.Rows(i)("commissiontype"), 2, True, False)



                    Else

                        UpdateValue(wsName, "F" & iLineNo1m.ToString, dt2o.Rows(i)("commissiontype"), 2, True, False)
                        UpdateValue(wsName, "E" & iLineNo1m.ToString, "Commission", 3, True, False)


                    End If

                Next
            End If

            iLineNo1m = iLineNo1m + xd2 + 3

            UpdateValue(wsName, "A" & iLineNo1m.ToString, "Non Refundable", 3, True, False)
            UpdateValue(wsName, "B" & iLineNo1m.ToString, "Apply Discount to Exhibition supplement", 3, True, False)
            UpdateValue(wsName, "C" & iLineNo1m.ToString, "Arrival Transfer", 3, True, False)
            UpdateValue(wsName, "D" & iLineNo1m.ToString, "Departure Transfer", 3, True, False)


            'xlSheet.Range("B" & iLineNo1m.ToString).wraptext = True

            'xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True

            'xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True

            iLineNo1m = iLineNo1m + 1



            Dim rsdmm1a1 As New DataTable
            strSqlQry = "select  case when ISNULL(nonrefundable,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(applytdiscountoexhibition,0)=0 then 'No' else 'Yes' end  status, case when ISNULL(arrivaltransfer,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(departuretransfer,0)=0 then 'No' else 'Yes' end  status   from view_offers_header h where  promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rsdmm1a1)
            SqlConn.Close()

            UpdateTableValue(wsName, rsdmm1a1, 0, iLineNo1m, 2, True)
            iLineNo1m = iLineNo1m + 2
            Dim xa As Integer
            Dim xd As Integer
            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If dt2o.Rows.Count > 0 Then
                        For c As Integer = 0 To dt2o.Rows.Count - 1
                            If dt31.Rows(i)("promotiontypes") = "Complimentary Airport Transfer" And dt2o.Rows(c)("arrivaltransfer") = 1 And dt2o.Rows(c)("departuretransfer") = 1 Then

                                UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Arrivals", 3, True, False)
                                UpdateValue(wsName, "E" & iLineNo1m - 1.ToString, "Departure", 3, True, False)
                                iLineNo1m = iLineNo1m + 1

                                Dim rsdmm1a1a As New DataTable
                                strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Arrival' and promotionid='" & OFFERID & "'"
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rsdmm1a1a)
                                SqlConn.Close()
                                UpdateTableValue(wsName, rsdmm1a1a, 3, iLineNo1m, 2, True)


                                'xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True
                                xa = rsdmm1a1a.Rows.Count

                                Dim rsdmm1a1ad As New DataTable
                                strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Departure' and promotionid='" & OFFERID & "'"
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rsdmm1a1ad)
                                SqlConn.Close()

                                UpdateTableValue(wsName, rsdmm1a1ad, 4, iLineNo1m, 2, True)



                                'xlSheet.Range("E" & iLineNo1m.ToString).wraptext = True
                                xd = rsdmm1a1ad.Rows.Count

                            End If
                        Next
                    End If

                Next
            End If

            If xd <> 0 Or xa <> 0 Then
                If xd > xa Then
                    iLineNo1m = iLineNo1m + 3 + xd
                Else
                    iLineNo1m = iLineNo1m + 3 + xa
                End If
            Else
                iLineNo1m = iLineNo1m + 3

            End If
            Dim d As Integer
            Dim xaf As Integer
            Dim xb2 As Integer
            Dim sp As Integer

            If dt31.Rows.Count > 0 Then
                For i As Integer = 0 To dt31.Rows.Count - 1
                    If dt31.Rows(i)("promotiontypes") = "Select flights only" Then
                        Dim rsdmm1a1f As New DataTable
                        UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Flight", 3, True, False)
                        strSqlQry = "select flightcode from view_offers_flight where  promotionid='" & OFFERID & "'"
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                        myDataAdapter.Fill(rsdmm1a1f)
                        SqlConn.Close()

                        UpdateTableValue(wsName, rsdmm1a1f, 0, iLineNo1m, 2, True)

                        xaf = rsdmm1a1f.Rows.Count
                    End If

                    If dt31.Rows(i)("promotiontypes") = "Inter Hotels" Then
                        If xaf >= 1 Then
                            UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "Hotel Name", 3, True, False)
                            UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Min.Stay", 3, True, False)

                            Dim rs1h As New DataTable

                            strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & OFFERID & "'"

                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rs1h)
                            SqlConn.Close()
                            UpdateTableValue(wsName, rs1h, 1, iLineNo1m, 2, True)

                            xa1 = rs1h.Rows.Count
                            d = 1


                        Else

                            UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Hotel Name", 3, True, False)
                            UpdateValue(wsName, "B" & iLineNo1m - 1.ToString, "Min.Stay", 3, True, False)
                            Dim rs1h As New DataTable

                            strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & OFFERID & "'"
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rs1h)
                            SqlConn.Close()
                            UpdateTableValue(wsName, rs1h, 0, iLineNo1m, 2, True)

                            xa1 = rs1h.Rows.Count
                            d = 1
                        End If

                    End If

                    If dt31.Rows(i)("promotiontypes") = "Special Occasion" Then


                        xb2 = 1
                        If dt2o.Rows.Count > 0 Then

                            For c As Integer = 0 To dt2o.Rows.Count - 1
                                If xaf >= 1 And xa1 >= 1 Then

                                    UpdateValue(wsName, "D" & iLineNo1m - 1.ToString, "Special Occasion", 3, True, False)
                                    UpdateValue(wsName, "D" & iLineNo1m.ToString, dt2o.Rows(c)("specialoccassion"), 2, True, False)



                                    'xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True

                                ElseIf xaf = 0 And xa1 >= 1 Then
                                    UpdateValue(wsName, "C" & iLineNo1m - 1.ToString, "Special Occasion", 3, True, False)
                                    UpdateValue(wsName, "C" & iLineNo1m.ToString, dt2o.Rows(c)("specialoccassion"), 2, True, False)
                                    'xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True
                                Else
                                    UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Special Occasion", 3, True, False)
                                    UpdateValue(wsName, "A" & iLineNo1m.ToString, dt2o.Rows(c)("specialoccassion"), 2, True, False)

                                    'xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True
                                End If

                            Next

                        End If
                    End If




                Next
            End If

            If xa1 <> 0 Then
                iLineNo1m = iLineNo1m + xa1 + 3

            ElseIf xa = 1 Or xb2 = 1 Then

                iLineNo1m = iLineNo1m + 3

            Else

                iLineNo1m = iLineNo1m

            End If
            UpdateValue(wsName, "A" & iLineNo1m - 1.ToString, "Remarks", 3, True, False)
            For c As Integer = 0 To dt2o.Rows.Count - 1
                UpdateValue(wsName, "A" & iLineNo1m.ToString, dt2o.Rows(c)("remarks").ToString, 3, True, True, "A" & iLineNo1m.ToString, True)


                'xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True


            Next


            wsName = "Commission"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)




            Dim iLineNo As Integer = 6
            Dim ycon As Integer
            Dim x As Integer
            Dim z As Integer

            Dim dt3 As New DataTable
            strSqlQry = "select tranid from view_contracts_commission_header where promotionid = '" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt3)
            SqlConn.Close()
            If dt3.Rows.Count > 0 Then
                For i As Integer = 0 To dt3.Rows.Count - 1


                    UpdateValue(wsName, "A" & iLineNo - 1.ToString, "Promotionid", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo - 1.ToString, "Promotionname", 3, True, False)
                    UpdateValue(wsName, "C" & iLineNo - 1.ToString, "ApplicableTo", 3, True, False)

                    strSqlQry = "select h.promotionid,h.promotionname,applicableto   from view_offers_header h WHERE h.partycode='" & PARTYCODE & "' and h.promotionid='" & OFFERID & "'"
                    Dim rsic As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsic)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsic, 0, iLineNo, 2, True)
                    'xlSheet.Range("B" & iLineNo.ToString).wraptext = True
                    iLineNo = iLineNo + 3
                    UpdateValue(wsName, "A" & iLineNo - 1.ToString, "From Date", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo - 1.ToString, "To Date", 3, True, False)



                    UpdateValue(wsName, "C" & iLineNo - 1.ToString, "Room Classification", 3, True, False)

                    '            xlSheet.Range("B" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
                    '            xlSheet.Range("C" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
                    '          
                    strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,d.fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,d.todate), 105),'')todate  from view_contracts_commission_detail d where d.tranid='" & dt3.Rows(i)("tranid").ToString & "'"
                    Dim rsic1 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsic1)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsic1, 0, iLineNo, 2, True)
                    ycon = rsic1.Rows.Count

                    strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & OFFERID & "' and isnull(h.roomcategory,'')<>'' order by prm.rankorder for xml path('')),1,1,''),'') "

                    Dim rs2 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs2)
                    SqlConn.Close()
                    z = rs2.Rows.Count
                    UpdateTableValue(wsName, rs2, 2, iLineNo, 2, True)

                    'xlSheet.Range("C" & iLineNo.ToString).wraptext = True
                    If ycon > z Then
                        iLineNo = iLineNo + ycon + 3
                    Else
                        iLineNo = iLineNo + z + 3
                    End If

                    UpdateValue(wsName, "A" & iLineNo - 1.ToString, "Room Type", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo - 1.ToString, "Meal Plan", 3, True, False)


                    strSqlQry = "select roomType=isnull(stuff((select ',' + prm.rmtypname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and partycode='" & PARTYCODE & "' and isnull(h.roomtypes,'')<>''  order by rankord for xml path('')),1,1,''),'')"
                    Dim rs1 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs1)
                    SqlConn.Close()

                    UpdateTableValue(wsName, rs1, 0, iLineNo, 2, True)

                    '            xlSheet.Range("A" & iLineNo.ToString).wraptext = True
                    '            xlSheet.Range("B" & iLineNo.ToString).wraptext = True
                    '            'xlSheet.Range("C" & iLineNo2.ToString).wraptext = True
                    x = rs1.Rows.Count
                    Dim mealvar As Integer
                    strSqlQry = "select  p.mealname from view_contracts_commission_header d  cross apply dbo.SplitString1colsWithOrderField(d.mealplans,',') q join mealmast p on q.Item1=p.mealcode  and d.tranid='" & dt3.Rows(i)("tranid").ToString & "' and d.promotionid='" & OFFERID & "' order by rankorder "
                    Dim rs11 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs11)
                    SqlConn.Close()

                    UpdateTableValue(wsName, rs11, 1, iLineNo, 2, True)
                    mealvar = rs11.Rows.Count
                    If x > mealvar Then
                        iLineNo = iLineNo + x + 3
                    Else
                        iLineNo = iLineNo + mealvar + 3
                    End If



                    UpdateValue(wsName, "A" & iLineNo - 1.ToString, "Formulaname", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo - 1.ToString, "Formulastring", 3, True, False)

                    strSqlQry = "select distinct v.formulaname,formulastring=isnull(stuff((select ';' + c.term1+','+ltrim(rtrim(convert(varchar(20),c.value)))  from view_contracts_commissions c,commissionformula_header h where c.formulaid=h.formulaid  and  c.tranid ='" & dt3.Rows(i)("tranid").ToString & "'    order by c.clineno for xml path('')),1,1,''),'') from view_contracts_commission_header h, view_contracts_commissions c, commissionformula_header v  where h.tranid = c.tranid And c.formulaid = v.formulaid and h.tranid ='" & dt3.Rows(i)("tranid").ToString & "'"
                    Dim rs3 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs3)
                    SqlConn.Close()
                    ' Dim iLineNo1 As Integer = 10
                    UpdateTableValue(wsName, rs3, 0, iLineNo, 2, True)

                    'xlSheet.Range("A" & iLineNo.ToString).wraptext = True
                    'xlSheet.Range("B" & iLineNo.ToString).wraptext = True
                    x = rs1.Rows.Count
                Next
            End If


            wsName = "Max Occupancy"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)


            Dim dtmx As New DataTable


            strSqlQry = "select distinct tranid from view_partymaxacc_header where partycode='" & PARTYCODE & "' and promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtmx)
            SqlConn.Close()

            Dim iLinmx As Integer = 7

            Dim em1 As Integer
            Dim em As Integer
            Dim em2 As Integer




            Dim dt23 As New DataTable
            If dtmx.Rows.Count > 0 Then
                For i As Integer = 0 To dtmx.Rows.Count - 1

                    strSqlQry = "select h.promotionid,h.promotionname ,applicableto from view_offers_header h WHERE h.partycode='" & PARTYCODE & "' and h.promotionid='" & OFFERID & "'"
                    Dim rsicm As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsicm)
                    SqlConn.Close()


                    UpdateValue(wsName, "A" & iLinmx - 1.ToString, "Promotionid", 3, True, False)
                    UpdateValue(wsName, "B" & iLinmx - 1.ToString, "Promotionname", 3, True, False)

                    UpdateValue(wsName, "C" & iLinmx - 1.ToString, "Applicable To", 3, True, False)
                    UpdateTableValue(wsName, rsicm, 0, iLinmx, 2, True)


                    iLinmx = iLinmx + 3
                    UpdateValue(wsName, "A" & iLinmx - 1.ToString, "Max Occ.ID", 3, True, False)
                    UpdateValue(wsName, "B" & iLinmx - 1.ToString, "Room Name", 3, True, False)
                    UpdateValue(wsName, "C" & iLinmx - 1.ToString, "Room Classification", 3, True, False)
                    UpdateValue(wsName, "D" & iLinmx - 1.ToString, "unit yes/no", 3, True, False)
                    UpdateValue(wsName, "E" & iLinmx - 1.ToString, "Price Adult Occupancy only for Unit", 3, True, False)


                    UpdateValue(wsName, "F" & iLinmx - 1.ToString, "Max Adults", 3, True, False)
                    UpdateValue(wsName, "G" & iLinmx - 1.ToString, "Max Child", 3, True, False)
                    UpdateValue(wsName, "H" & iLinmx - 1.ToString, "Max Infant", 3, True, False)

                    UpdateValue(wsName, "I" & iLinmx - 1.ToString, "Max EB", 3, True, False)
                    UpdateValue(wsName, "J" & iLinmx - 1.ToString, "No of Extra Person Supplement for Unit Only", 3, True, False)
                    UpdateValue(wsName, "k" & iLinmx - 1.ToString, "Max Total Occupancy without infant", 3, True, False)
                    UpdateValue(wsName, "l" & iLinmx - 1.ToString, "Rank Order", 3, True, False)
                    UpdateValue(wsName, "N" & iLinmx - 1.ToString, "Start with 0 based", 3, True, False)

                    UpdateValue(wsName, "M" & iLinmx - 1.ToString, "Occupancy Combinations", 3, True, False)





                    Dim dtmx1 As New DataTable
                    strSqlQry = "select v.rmtypcode from view_partymaxaccomodation v ,partyrmtyp  p  where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and    v.partycode= '" & PARTYCODE & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  p.rankord"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtmx1)
                    SqlConn.Close()
                    strSqlQry = "  select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status," _
                             & " PRICEPAX, " _
                             & "m.maxadults,m.maxchilds,maxinfant,m.maxeb, " _
                             & " isnull(m.noofextraperson,'') noofextraperson, m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm, " _
                             & " view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode  " _
                             & " and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid  " _
                             & "  And h.partycode='" & PARTYCODE & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

                    Dim rsmx As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx)
                    SqlConn.Close()
                    em = rsmx.Rows.Count
                    UpdateTableValue(wsName, rsmx, 0, iLinmx, 2, True)

                    'xlSheet.Range("M" & iLinmx.ToString).wraptext = True
                    'If conn1.State = ConnectionState.Open Then
                    '    conn1.Close()
                    'End If
                    strSqlQry = "select  start0based from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & PARTYCODE & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord "
                    Dim rsmx2 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx2)
                    SqlConn.Close()
                    em2 = rsmx2.Rows.Count

                    UpdateTableValue(wsName, rsmx2, 13, iLinmx, 2, True)


                    If dtmx1.Rows.Count > 0 Then
                        For idt As Integer = 0 To dtmx1.Rows.Count - 1



                            strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & PARTYCODE & "' order by maxadults for xml path('')),1,1,''),'') "

                            Dim rsmx1 As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsmx1)
                            SqlConn.Close()
                            em = rsmx1.Rows.Count

                            UpdateTableValue(wsName, rsmx1, 12, iLinmx, 2, True)

                            iLinmx = iLinmx + 1



                        Next

                    End If




                    Dim Maxintmo As Integer = Math.Max(em, Math.Max(em1, em2))
                    iLinmx = iLinmx + 3 + Maxintmo

                Next

            End If

            wsName = "Room Rates"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)


            Dim dtrr2 As New DataTable
            strSqlQry = "select plistcode from view_cplisthnew  where promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtrr2)
            SqlConn.Close()
            Dim iLine2 As Integer = 5
            Dim ei2 As Integer
            Dim ei3 As Integer
            Dim ei4 As Integer
            Dim ei4r As Integer

            If dtrr2.Rows.Count > 0 Then
                For i As Integer = 0 To dtrr2.Rows.Count - 1

                    strSqlQry = "select h.plistcode,h.promotionid,p.promotionname,h.applicableto  from view_offers_header p, view_cplisthnew  h WHERE p.promotionid=h.promotionid and h.partycode='" & PARTYCODE & "' and h.promotionid='" & OFFERID & "' and plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
                    Dim rse2 As New DataTable
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse2)
                    SqlConn.Close()
                    ei2 = rse2.Rows.Count

                    UpdateValue(wsName, "A" & iLine2, "PriceList Code", 3, True, False)
                    UpdateValue(wsName, "B" & iLine2, "Promotionid", 3, True, False)
                    UpdateValue(wsName, "C" & iLine2, "Promotionname", 3, True, False)
                    UpdateValue(wsName, "D" & iLine2, "Aplicable to", 3, True, False)

                    iLine2 = iLine2 + 1
                    UpdateTableValue(wsName, rse2, 0, iLine2, 2, True)

                    iLine2 = iLine2 + 3
                    strSqlQry = "select  isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from view_cplisthnew_offerdates WHERE plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
                    Dim rse32 As New DataTable
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse32)
                    SqlConn.Close()
                    ei4 = rse32.Rows.Count
                    UpdateValue(wsName, "A" & iLine2 - 1, "From Date", 3, True, False)
                    UpdateValue(wsName, "B" & iLine2 - 1, "To Date", 3, True, False)

                    UpdateTableValue(wsName, rse32, 0, iLine2, 2, True)



                    '            xlSheet.Range("B" & iLine2.ToString).NumberFormat = "dd/mm/yyyy;@"
                    '            xlSheet.Range("A" & iLine2.ToString).NumberFormat = "dd/mm/yyyy;@"
                    iLine2 = iLine2 + ei4 + 1
                    Dim fromrange As Integer, torange As Integer
                    fromrange = iLine2
                    torange = IIf(rse32.Rows.Count > 0, iLine2 + rse32.Rows.Count, iLine2)

                    'xlSheet.Range("B" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
                    'xlSheet.Range("C" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"

                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_cplisthnew_weekdays   where  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' for xml path('')),1,1,''),'') "
                    Dim rsw2 As New DataTable
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsw2)
                    SqlConn.Close()

                    UpdateTableValue(wsName, rsw2, 1, iLine2, 2, True)
                    UpdateValue(wsName, "A" & iLine2, "Days of the week", 3, True, False)


                    '      

                    'strSqlQry = "select  c.mealplans,m.mealname from view_cplisthnew c,mealmast m where m.mealcode=c.mealplans and  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and  c.promotionid='" & OFFERID & "'"
                    'Dim rse32r As New DataTable
                    'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    'myDataAdapter.Fill(rse32r)
                    'SqlConn.Close()



                    'ei4r = rse32r.Rows.Count
                    'UpdateTableValue(wsName, rse32r, 0, iLine2, 2, True)
                    'UpdateValue(wsName, "A" & iLine2 - 1, "Meal Code", 3, True, False)
                    'UpdateValue(wsName, "B" & iLine2 - 1, "Meal Name", 3, True, False)




                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_roomrates]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@plistcode", dtrr2.Rows(i)("plistcode").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)

                            End Using
                        End Using
                    End Using



                    Dim ii3 As Integer = 65
                    'For Each column As DataColumn In dt.Columns
                    'name(ii) = column.ColumnName
                    iLine2 = iLine2 + 2
                    Dim sss3 = Chr(ii3).ToString
                    For OO As Integer = 0 To dtt.Columns.Count - 1

                        UpdateValue(wsName, sss3.ToString() + iLine2.ToString(), dtt.Columns(OO).ColumnName.ToString(), 3, True, False)
                        ii3 += 1

                        sss3 = Chr(ii3).ToString
                    Next

                    If dtt.Rows.Count > 0 Then
                        iLine2 = iLine2 + 1
                        UpdateTableValue(wsName, dtt, 0, iLine2, 2, True)

                        ei3 = dtt.Rows.Count

                        fromrange = iLine2
                        torange = IIf(dtt.Rows.Count > 0, iLine2 + dtt.Rows.Count, iLine2)
                        'UpdateValue(wsName, "C" & fromrange.ToString & ":" & "C" & torange.ToString).NumberFormat = "####"

                    End If

                    iLine2 = iLine2 + 3 + ei3

                Next

            End If

            wsName = "Exhibition Supplements"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)


            Dim dte As New DataTable
            strSqlQry = "select exhibitionid from view_contracts_exhibition_header  where promotionid= '" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dte)
            Dim iLinee As Integer = 6
            Dim ei As Integer
            Dim ze As Integer
            Dim chkc As Integer

            If dte.Rows.Count > 0 Then
                For i As Integer = 0 To dte.Rows.Count - 1



                    strSqlQry = "select c.exhibitionid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h, view_contracts_exhibition_header c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "' and  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
                    Dim rscpcr As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscpcr)
                    SqlConn.Close()
                    chkc = rscpcr.Rows.Count

                    UpdateValue(wsName, "A" & iLinee - 1, "Exhibition Id", 3, True, False)
                    UpdateValue(wsName, "B" & iLinee - 1, "PromotionId", 3, True, False)
                    UpdateValue(wsName, "C" & iLinee - 1, "Promotion Name", 3, True, False)
                    UpdateValue(wsName, "D" & iLinee - 1, "Applicable To", 3, True, False)
                    UpdateTableValue(wsName, rscpcr, 0, iLinee, 2, True)





                    iLinee = iLinee + 3

                    UpdateValue(wsName, "A" & iLinee - 1, "Exhibition Name", 3, True, False)
                    UpdateValue(wsName, "B" & iLinee - 1, "From Date", 3, True, False)
                    UpdateValue(wsName, "C" & iLinee - 1, "To Date", 3, True, False)
                    UpdateValue(wsName, "D" & iLinee - 1, "Room Type", 3, True, False)
                    UpdateValue(wsName, "E" & iLinee - 1, "Meal Plan", 3, True, False)
                    UpdateValue(wsName, "F" & iLinee - 1, "Supplement Amount", 3, True, False)
                    UpdateValue(wsName, "G" & iLinee - 1, "With Drawn", 3, True, False)




                    strSqlQry = "select e.exhibitionname,isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from view_contracts_exhibition_detail d join exhibition_master e on d.exhibitioncode=e.exhibitioncode join  view_contracts_exhibition_header h on d.exhibitionid=h.exhibitionid and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
                    Dim rse As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rse)
                    SqlConn.Close()

                    ei = rse.Rows.Count

                    UpdateTableValue(wsName, rse, 0, iLinee, 2, True)


                    strSqlQry = "select distinct mealplans,supplementvalue,isnull(withdraw,'') from view_contracts_exhibition_detail where exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"

                    Dim rsr As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsr)
                    SqlConn.Close()
                    ze = rsr.Rows.Count
                    UpdateTableValue(wsName, rsr, 4, iLinee, 2, True)

                    Dim dter As New DataTable
                    strSqlQry = "select distinct exhibitioncode from view_contracts_exhibition_detail  where  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dter)

                    SqlConn.Close()

                    Dim xee As Integer
                    Dim yee As Integer
                    If dter.Rows.Count > 0 Then
                        For er As Integer = 0 To dter.Rows.Count - 1





                            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'  and p.partycode ='" & PARTYCODE & "' and d.exhibitioncode='" & dter.Rows(er)("exhibitioncode").ToString & "' for xml path('')),1,1,''),'') "
                            Dim rser As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rser)
                            yee = rser.Rows.Count
                            iLinee = iLinee

                            UpdateTableValue(wsName, rser, 3, iLinee, 2, True)

                            iLinee = iLinee + yee


                        Next

                    End If

                    Dim Maxintr As Integer = Math.Max(ze, Math.Max(ei, yee))

                    iLinee = iLinee + Maxintr + 3
                Next
            End If

            wsName = "Meal Supplements"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)




            Dim dtmr As New DataTable
            strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header where promotionid= '" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtmr)
            Dim iLinmr As Integer = 8
            Dim m7 As Integer
            Dim s7 As Integer
            Dim e7 As Integer
            Dim mn7 As Integer


            If dtmr.Rows.Count > 0 Then
                ' Dim conn As New ADODB.Connection
                For i As Integer = 0 To dtmr.Rows.Count - 1

                    UpdateValue(wsName, "C" & iLinmr - 1, "Promotion Name", 3, True, False)
                    UpdateValue(wsName, "B" & iLinmr - 1, "PromotionId", 3, True, False)
                    UpdateValue(wsName, "A" & iLinmr - 1, "Supplement ID", 3, True, False)
                    UpdateValue(wsName, "D" & iLinmr - 1, "Applicable To", 3, True, False)




                    strSqlQry = "select mealsupplementid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_contracts_mealsupp_header c   " _
                        & " where h.promotionid=c.promotionid and   h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "'  and  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"

                    Dim rsm7 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsm7)
                    SqlConn.Close()
                    m7 = rsm7.Rows.Count
                    UpdateTableValue(wsName, rsm7, 0, iLinmr, 2, True)


                    iLinmr = iLinmr + m7 + 3

                    UpdateValue(wsName, "A" & iLinmr - 1, "Manual From Date not linked to Seasons", 3, True, False)
                    UpdateValue(wsName, "B" & iLinmr - 1, "Manual To Date not linked to Seasons", 3, True, False)
                    UpdateValue(wsName, "C" & iLinmr - 1, "Excluded From Date", 3, True, False)
                    UpdateValue(wsName, "D" & iLinmr - 1, "Excluded To Date", 3, True, False)
                    UpdateValue(wsName, "E" & iLinmr - 1, "Excl For", 3, True, False)

                    strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from  view_contracts_mealsupp_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
                    Dim rsmc As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmc)
                    SqlConn.Close()
                    mn7 = rsmc.Rows.Count
                    UpdateTableValue(wsName, rsmc, 0, iLinmr, 2, True)





                    strSqlQry = " select  isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate,exclfor from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"

                    Dim rsed As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsed)
                    SqlConn.Close()

                    UpdateTableValue(wsName, rsed, 2, iLinmr, 2, True)
                    e7 = rsed.Rows.Count


                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_mealsupp_weekdays  where  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' for xml path('')),1,1,''),'') "


                    Dim rsw As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsw)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsw, 5, iLinmr, 2, True)
                    UpdateValue(wsName, "F" & iLinmr - 1, "Days of the week", 3, True, False)




                    If mn7 > e7 Then
                        iLinmr = iLinmr + mn7
                    Else
                        iLinmr = iLinmr + e7
                    End If

                    'strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & OFFERID & "' and isnull(h.roomcategory,'')<>'' for xml path('')),1,1,''),'')"

                    'Dim rs2 As New ADODB.Recordset
                    'rs2 = GetResultAsRecordSet(strSqlQry)
                    'z = rs2.RecordCount
                    'xlSheet.Range("C" & iLineNo.ToString).CopyFromRecordset(rs2)
                    'xlSheet.Range("C" & iLineNo.ToString).wraptext = True
                    'strSqlQry = " select  convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105) from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
                    'Dim rsedq As New ADODB.Recordset
                    'rsedq = GetResultAsRecordSet(strSqlQry)
                    'e17 = rsed.RecordCount

                    'xlSheet.Range("C" & iLinmr.ToString).CopyFromRecordset(rsedq)

                    'If conn.State = ConnectionState.Open Then
                    '    conn.Close()
                    'End If

                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_mealsupplements]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@mealsupplementid", dtmr.Rows(i)("mealsupplementid").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)
                                'If dtt.Rows(i)(i) = "-3" Then
                                '    "Free"

                                '    "Incl"
                                '    txt.Text = "-1"
                                '    Case "N.Incl"
                                '    txt.Text = "-3"
                                '    Case "N/A"
                                '    txt.Text = "-4"
                                '    Case "On Request"
                                '    txt.Text = "-5"

                            End Using
                        End Using
                    End Using





                    iLinmr = iLinmr + 1


                    'Dim name(dtt.Columns.Count) As String
                    Dim ii As Integer = 65
                    'For Each column As DataColumn In dt.Columns
                    'name(ii) = column.ColumnName
                    Dim sss = Chr(ii).ToString
                    For OO As Integer = 0 To dtt.Columns.Count - 1


                        UpdateValue(wsName, sss.ToString() & iLinmr.ToString, dtt.Columns(OO).ColumnName.ToString(), 3, True, False)

                        ii += 1
                        UpdateValue(wsName, sss.ToString() & iLinmr.ToString, dtt.Columns(OO).ColumnName.ToString(), 3, True, False)

                        sss = Chr(ii).ToString
                    Next








                    If dtt.Rows.Count > 0 Then
                        iLinmr = iLinmr + 1
                        UpdateTableValue(wsName, dtt, 0, iLinmr, 2, True)
                        s7 = dtt.Rows.Count

                    End If


                    iLinmr = iLinmr + s7 + 3

                Next

            End If



            wsName = "Child Policy"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)



            Dim dtcpi As New DataTable
            strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtcpi)
            SqlConn.Close()
            Dim iLine8 As Integer = 6

            If dtcpi.Rows.Count > 0 Then
                For i As Integer = 0 To dtcpi.Rows.Count - 1

                    UpdateValue(wsName, "B" & iLine8, "PromotionId", 3, True, False)
                    UpdateValue(wsName, "A" & iLine8, "ChildPolicy Id", 3, True, False)
                    UpdateValue(wsName, "C" & iLine8, "Promotion Name", 3, True, False)
                    UpdateValue(wsName, "D" & iLine8, "Applicable To", 3, True, False)



                    Dim chk8 As Integer
                    Dim ei31 As Integer

                    'strSqlQry = "select c.childpolicyid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d ,view_contracts_childpolicy_header c where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "'  and  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"

                    strSqlQry = "select c.childpolicyid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_contracts_childpolicy_header c  " _
                        & " where h.promotionid=c.promotionid and   h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "'  and  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"

                    Dim rs8 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rs8)
                    SqlConn.Close()
                    chk8 = rs8.Rows.Count
                    iLine8 = iLine8 + 1
                    UpdateTableValue(wsName, rs8, 0, iLine8, 2, True)

                    iLine8 = iLine8 + 2
                    UpdateValue(wsName, "B" & iLine8, "Manual To Date not linked to Seasons", 3, True, False)
                    UpdateValue(wsName, "A" & iLine8, "Manual From Date not linked to Seasons", 3, True, False)
                    UpdateValue(wsName, "C" & iLine8, "Days of the week", 3, True, False)

                    iLine8 = iLine8 + 1

                    strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from  view_contracts_childpolicy_dates  where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"

                    Dim rsmc As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmc)
                    SqlConn.Close()
                    mn7 = rsmc.Rows.Count


                    UpdateTableValue(wsName, rsmc, 0, iLine8, 2, True)


                    strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_childpolicy_weekdays where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' for xml path('')),1,1,''),'') "

                    Dim rss8 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rss8)
                    SqlConn.Close()



                    UpdateTableValue(wsName, rss8, 2, iLine8, 2, True)
                    iLine8 = iLine8 + mn7 + 1







                    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
                    Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
                    Dim dtt As New DataTable
                    Using con As New SqlConnection(constring)
                        Using cmd1 As New SqlCommand("[sp_print_childpolicy]")
                            cmd1.CommandType = CommandType.StoredProcedure
                            cmd1.Parameters.AddWithValue("@childpolicyid", dtcpi.Rows(i)("childpolicyid").ToString)

                            Using sda As New SqlDataAdapter()
                                cmd1.Connection = con
                                sda.SelectCommand = cmd1

                                sda.Fill(dtt)

                            End Using
                        End Using
                    End Using


                    If dtt.Rows.Count > 0 Then

                        Dim ii2 As Integer = 65


                        Dim sss2 = Chr(ii2).ToString

                        For OO As Integer = 0 To dtt.Columns.Count - 1
                            UpdateValue(wsName, sss2.ToString() & iLine8.ToString, dtt.Columns(OO).ColumnName.ToString(), 3, True, False)

                            ii2 += 1

                            sss2 = Chr(ii2).ToString

                        Next


                        iLine8 = iLine8 + 1
                        UpdateTableValue(wsName, dtt, 0, iLine8, 2, True)

                        ei31 = dtt.Rows.Count

                    End If

                    iLine8 = iLine8 + ei31 + 3

                Next

            End If



            wsName = "Cancel Policy"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)



            Dim dtcn As New DataTable
            strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where  promotionid= '" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtcn)
            SqlConn.Close()
            Dim iLinecr2 As Integer = 6


            If dtcn.Rows.Count > 0 Then
                For i As Integer = 0 To dtcn.Rows.Count - 1

                    Dim rm2 As Integer
                    Dim ml2 As Integer
                    Dim chk1cc As Integer
                    Dim cocc As Integer
                    Dim ns As Integer

                    UpdateValue(wsName, "A" & iLinecr2, "Cancellation ID", 3, True, False)
                    UpdateValue(wsName, "B" & iLinecr2, "PromotionId", 3, True, False)
                    UpdateValue(wsName, "C" & iLinecr2, "Promotion Name", 3, True, False)
                    UpdateValue(wsName, "D" & iLinecr2, "Applicable To", 3, True, False)


                    strSqlQry = "select c.cancelpolicyid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h, view_contracts_cancelpolicy_header   c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "' AND  cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                    Dim rscp As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    chk1cc = rscp.Rows.Count

                    iLinecr2 = iLinecr2 + 1
                    UpdateTableValue(wsName, rscp, 0, iLinecr2, 2, True)
                    SqlConn.Close()

                    iLinecr2 = iLinecr2 + 3

                    UpdateValue(wsName, "A" & iLinecr2 - 1, "Promotion From Date", 3, True, False)
                    UpdateValue(wsName, "B" & iLinecr2 - 1, "Promotion To Date", 3, True, False)


                    strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from view_contracts_cancelpolicy_offerdates where cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                    Dim rsc As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsc)
                    cocc = rsc.Rows.Count
                    UpdateTableValue(wsName, rsc, 0, iLinecr2, 2, True)
                    SqlConn.Close()

                    iLinecr2 = iLinecr2 + cocc + 2

                    UpdateValue(wsName, "A" & iLinecr2 - 1, "Room Type", 3, True, False)
                    UpdateValue(wsName, "B" & iLinecr2 - 1, "Meal Plan", 3, True, False)
                    UpdateValue(wsName, "D" & iLinecr2 - 1, "To No.of Days or Hours", 3, True, False)
                    UpdateValue(wsName, "E" & iLinecr2 - 1, "Unit -Days/Hours", 3, True, False)

                    UpdateValue(wsName, "C" & iLinecr2 - 1.ToString, "From No.of Days or Hours ", 3, True, False)
                    UpdateValue(wsName, "G" & iLinecr2 - 1, "No. Of Nights to charge", 3, True, False)
                    UpdateValue(wsName, "F" & iLinecr2 - 1, "Charge Basis", 3, True, False)
                    UpdateValue(wsName, "H" & iLinecr2 - 1, "Percentage to charge", 3, True, False)
                    UpdateValue(wsName, "I" & iLinecr2 - 1, "Value to charge", 3, True, False)



                    '        strSqlQry = "select distinct d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" &contractid & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and h.promotionid= '" & OFFERID & "'"
                    '        Dim rscp As New ADODB.Recordset
                    '        rscp = GetResultAsRecordSet(strSqlQry)
                    '        co2 = rscp.RecordCount
                    '        xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)

                    '            '        strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto,d.fromdate,d.todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status    from view_offers_header h,view_offers_detail d, view_contracts_cancelpolicy_header c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='RI13' and  h.promotionid= '" & OFFERID & "'"
                    '            '        Dim rs2e As New ADODB.Recordset
                    '            '        rs2e = GetResultAsRecordSet(strSqlQry)
                    '            '        ce2 = rs2e.RecordCount
                    '            '        xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rs2e)

                    Dim dtcl As New DataTable
                    strSqlQry = "select clineno from view_contracts_cancelpolicy_detail  where cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtcl)


                    Dim rs2r As New DataTable

                    If dtcl.Rows.Count > 0 Then
                        For cl As Integer = 0 To dtcl.Rows.Count - 1

                            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from  view_contracts_cancelpolicy_detail d  cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and clineno='" & dtcl.Rows(cl)("clineno").ToString & "' and p.partycode='" & PARTYCODE & "' for xml path('')),1,1,''),'') "

                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rs2r)
                            SqlConn.Close()


                        Next
                    End If
                    rm2 = rs2r.Rows.Count
                    UpdateTableValue(wsName, rs2r, 0, iLinecr2, 2, True)


                    strSqlQry = "select mealplans,fromnodayhours,nodayshours, dayshours,isnull(chargebasis,'')chargebasis,isnull(nightstocharge,0), percentagetocharge,valuetocharge from view_contracts_cancelpolicy_detail where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                    Dim rsr2 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsr2)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rsr2, 1, iLinecr2, 2, True)

                    ml2 = rsr2.Rows.Count


                    If ml2 Or rm2 <> 0 Then
                        If ml2 > rm2 Then
                            iLinecr2 = iLinecr2 + ml2 + 2

                        Else
                            iLinecr2 = iLinecr2 + rm2 + 2
                        End If
                    Else

                        iLinecr2 = iLinecr2 + 2

                    End If



                    strSqlQry = "select noshowearly from view_contracts_cancelpolicy_noshowearly  where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"


                    Dim dtns As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtns)
                    SqlConn.Close()


                    UpdateValue(wsName, "A" & iLinecr2 - 1, "Room Type", 3, True, False)
                    UpdateValue(wsName, "C" & iLinecr2 - 1, "No Show/Early Checkout", 3, True, False)
                    UpdateValue(wsName, "D" & iLinecr2 - 1, "Charge Basis", 3, True, False)
                    UpdateValue(wsName, "F" & iLinecr2 - 1, "Percentage to charge", 3, True, False)
                    UpdateValue(wsName, "B" & iLinecr2 - 1, "Meal Plan", 3, True, False)
                    UpdateValue(wsName, "G" & iLinecr2 - 1, "Value to charge", 3, True, False)
                    UpdateValue(wsName, "E" & iLinecr2 - 1, "No.of Nights to charge", 3, True, False)



                    If dtns.Rows.Count > 0 Then
                        For i2 As Integer = 0 To dtns.Rows.Count - 1
                            strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & PARTYCODE & "'  for xml path('')),1,1,''),''), mealplans,d.noshowearly,chargebasis,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

                            Dim rsrns4 As New DataTable
                            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                            myDataAdapter.Fill(rsrns4)
                            SqlConn.Close()

                            ns = rsrns4.Rows.Count
                            UpdateTableValue(wsName, rsrns4, 0, iLinecr2, 2, True)

                            iLinecr2 = iLinecr2 + 1
                        Next
                    End If



                    iLinecr2 = iLinecr2 + ns + 3

                Next
            End If


            wsName = "Check InOut Policy"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)

            Dim dtc As New DataTable
            strSqlQry = "select checkinoutpolicyid FROM view_contracts_checkinout_header where promotionid='" & OFFERID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dtc)
            SqlConn.Close()
            Dim linelbct As Integer = 6

            If dtc.Rows.Count > 0 Then
                For i As Integer = 0 To dtc.Rows.Count - 1

                    Dim rm As Integer
                    Dim ml As Integer
                    Dim tm As Integer
                    Dim co As Integer
                    Dim de As Integer
                    Dim chk As Integer

                    UpdateValue(wsName, "A" & linelbct - 1, "CheckIn/OutPolicyId", 3, True, False)
                    UpdateValue(wsName, "B" & linelbct - 1, "PromotionId", 3, True, False)
                    UpdateValue(wsName, "C" & linelbct - 1, "Promotion Name", 3, True, False)
                    UpdateValue(wsName, "D" & linelbct - 1, "Applicable To", 3, True, False)
                    UpdateValue(wsName, "E" & linelbct - 1, "CheckIn Time", 3, True, False)
                    UpdateValue(wsName, "F" & linelbct - 1, "Checkout Time", 3, True, False)

                    strSqlQry = "select c.checkinoutpolicyid ,h.promotionid,h.promotionname ,h.applicableto,isnull(c.checkintime,0) checkintime,isnull(c.checkouttime,0) checkouttime  from view_offers_header h, view_contracts_checkinout_header  c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & PARTYCODE & "'  and  h.promotionid='" & OFFERID & "'"
                    Dim rscp As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    SqlConn.Close()
                    chk = rscp.Rows.Count
                    UpdateTableValue(wsName, rscp, 0, linelbct, 2, True)

                    linelbct = linelbct + 3

                    strSqlQry = "select isnull(CONVERT(VARCHAR(10), convert(date,fromdate), 105),'')fromdate,isnull(CONVERT(VARCHAR(10), convert(date,todate), 105),'')todate from view_contracts_checkinout_offerdates where checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
                    Dim rsc As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsc)
                    SqlConn.Close()
                    co = rsc.Rows.Count

                    UpdateValue(wsName, "A" & linelbct - 1, "Promotion From Date", 3, True, False)
                    UpdateValue(wsName, "B" & linelbct - 1, "Promotion To Date", 3, True, False)

                    UpdateTableValue(wsName, rsc, 0, linelbct, 2, True)


                    linelbct = linelbct + co + 1

                    strSqlQry = "select isnull(stuff((select ',' + mealcode from view_contracts_checkinout_mealplans where  checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' for xml path('')),1,1,''),'') "

                    Dim rscm As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscm)
                    SqlConn.Close()
                    ml = rscm.Rows.Count
                    UpdateTableValue(wsName, rscm, 3, linelbct, 2, True)
                    UpdateValue(wsName, "C" & linelbct, "Meal Plan", 3, True, False)


                    strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & PARTYCODE & "' for xml path('')),1,1,''),'') "
                    Dim rscr As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscr)
                    SqlConn.Close()
                    UpdateTableValue(wsName, rscr, 1, linelbct, 2, True)
                    UpdateValue(wsName, "A" & linelbct, "Room Type", 3, True, False)
                    rm = rscr.Rows.Count



                    If ml > rm Then
                        linelbct = linelbct + ml + 3
                    Else
                        linelbct = linelbct + rm + 3
                    End If

                    strSqlQry = "select checkinouttype,	fromhours,tohours,case when ISNULL(chargeyesno,0)=0 then 'No' else 'Yes' end,chargetype,percentage,value,condition,isnull(requestbeforedays,'') from view_contracts_checkinout_detail where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
                    Dim rst As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rst)
                    SqlConn.Close()
                    tm = rst.Rows.Count


                    UpdateValue(wsName, "A" & linelbct - 1, "CheckIn/CheckoutType", 3, True, False)
                    UpdateValue(wsName, "B" & linelbct - 1, "From", 3, True, False)
                    UpdateValue(wsName, "C" & linelbct - 1, "To", 3, True, False)
                    UpdateValue(wsName, "D" & linelbct - 1, "Charge Y/N", 3, True, False)
                    UpdateValue(wsName, "F" & linelbct - 1, "Percentage", 3, True, False)
                    UpdateValue(wsName, "G" & linelbct - 1, "Value", 3, True, False)
                    UpdateValue(wsName, "E" & linelbct - 1, "Charge Type", 3, True, False)

                    UpdateValue(wsName, "I" & linelbct - 1, "Requestbeforedays", 3, True, False)
                    UpdateValue(wsName, "H" & linelbct - 1, "Conditions", 3, True, False)

                    UpdateTableValue(wsName, rst, 0, linelbct, 2, True)

                    strSqlQry = "select ISNULL(datetype,''),ISNULL(restrictdate,'') from view_contracts_checkinout_restricted where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"

                    Dim rsd As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsd)
                    SqlConn.Close()
                    de = rsd.Rows.Count

                    linelbct = linelbct + tm + 3
                    UpdateValue(wsName, "A" & linelbct - 1, "Date Type", 3, True, False)
                    UpdateValue(wsName, "B" & linelbct - 1, "No CheckIn-/Out", 3, True, False)

                    UpdateTableValue(wsName, rsd, 0, linelbct, 2, True)


                    linelbct = linelbct + de + 3


                Next
            End If


            document.Close()

            Dim strpop As String
            strpop = "window.open('PriceListModule/DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




            ''''


            ''''''''''''29/03/17 as per Madam Commented taking time to load 

            'wsName = "Final Offer Rates"
            'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)


            ''Dim sellingexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_final_contracted_rates where promotionid='" & OFFERID & "'")
            ''If sellingexists <> "" Then


            '' strSqlQry = "select distinct plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" &contractid & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"

            'strSqlQry = "select promotionid,calculatedid,autoid,plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb, " _
            '    & " noofadulteb,noofchildeb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,discounttype,discount,addldiscount,exhibitionprice,adultebprice,adultebpricedisc,extrapaxprice,extrapaxpricedisc,totalsharingcharge,totalsharingdiscount,totalebcharge,totalebdiscount,totalprice,pricewithfreenight,nights,minstay,minstayoption,stayfor,freenights,rmtypupgradefrom,rmtypupgradefromname,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_offer_rates where promotionid = '" & OFFERID & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"


            'Dim rsrf As New DataTable
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(rsrf)
            'SqlConn.Close()
            'Dim yf As Integer
            'Dim iLinesf As Integer = 7
            'yf = rsrf.Rows.Count

            'UpdateTableValue(wsName, rsrf, 0, iLinesf, 2, True)



            ''        ''''''''End Final Calculated Rate

            ' ''        '--- Contract Rates for Other Meal Plan
            'wsName = "Offer Rates – Other Meal Plan"
            'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)

            'Dim iLinesfo As Integer = 8

            'strSqlQry = "select promotionid,calculatedid,autoid,plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan, basemeal ,accommodationid,agecombination,adults,child,totalpaxwithinpricepax, " _
            '    & "maxeb,noofadulteb,noofchildeb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,discounttype,discount,addldiscount,exhibitionprice,adultebprice,adultebpricedisc,extrapaxprice,extrapaxpricedisc,totalsharingcharge,totalsharingdiscount,totalebcharge,totalebdiscount,mealsupplementid,adultmealprice,adultmealdisc,adultmealrmcatdetails, " _
            '    & " totalchildmealcharge,totalchildmealdisc,childmealdetails,totalprice,pricewithfreenight,nights,minstay,minstayoption,stayfor,freenights,rmtypupgradefrom,rmtypupgradefromname,commissionformulaid, " _
            '    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_offer_rates_othmeal where promotionid ='" & OFFERID & "' order by rmtyporder, " _
            '    & " rmcatorder,agecombination,pfromdate"
            'Dim rsrfo As New DataTable
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(rsrfo)
            'SqlConn.Close()



            'Dim yfoo As Integer

            'yfoo = rsrfo.Rows.Count


            'If yfoo > 0 Then

            '    UpdateTableValue(wsName, rsrfo, 0, iLinesfo, 2, True)
            'End If

            '''''''''''' Commented taking time





            '  objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Exported succesfully : ", Session("GlobalUserName"))

        Catch ex As Exception
            objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub





    Public Function UpdateValue(ByVal sheetName As String, ByVal addressName As String, ByVal value As String, ByVal styleIndex As Integer, ByVal isString As Boolean, Optional ByVal isMerge As Boolean = False, Optional ByVal toaddressname As String = "", Optional ByVal wrapcell As Boolean = False) As Boolean
        ' Assume failure.
        Dim updated As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()

        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            If isString Then
                ' Either retrieve the index of an existing string,
                ' or insert the string into the shared string table
                ' and get the index of the new item.
                Dim stringIndex As Integer = InsertSharedStringItem(wbPart, value)

                cell.CellValue = New CellValue(stringIndex.ToString())
                cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
            Else
                cell.CellValue = New CellValue(value)
                cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
            End If


            If styleIndex > 0 Then
                cell.StyleIndex = styleIndex

            End If

            If isMerge Then
                Dim MergeCells As New MergeCells()
                If (ws.Elements(Of MergeCells)().Count() > 0) Then
                    MergeCells = ws.Elements(Of MergeCells).First()
                Else
                    MergeCells = New MergeCells()
                    ' Insert a MergeCells object into the specified position.
                    If (ws.Elements(Of CustomSheetView)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of CustomSheetView)().First())
                    ElseIf (ws.Elements(Of DataConsolidate)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of DataConsolidate)().First())
                    ElseIf (ws.Elements(Of SortState)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SortState)().First())
                    ElseIf (ws.Elements(Of AutoFilter)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of AutoFilter)().First())
                    ElseIf (ws.Elements(Of Scenarios)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of Scenarios)().First())
                    ElseIf (ws.Elements(Of ProtectedRanges)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of ProtectedRanges)().First())
                    ElseIf (ws.Elements(Of SheetProtection)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetProtection)().First())
                    ElseIf (ws.Elements(Of SheetCalculationProperties)().Count() > 0) Then
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetCalculationProperties)().First())
                    Else
                        ws.InsertAfter(MergeCells, ws.Elements(Of SheetData)().First())
                    End If
                End If
                Dim mergeCell As MergeCell = New MergeCell()

                'append a MergeCell to the mergeCells for each set of merged cells
                mergeCell.Reference = New StringValue(addressName + ":" + toaddressname)
                MergeCells.Append(mergeCell)
            End If

            If wrapcell Then
                cell.StyleIndex = InsertCellFormat(wbPart, GenerateCellFormat())


            End If

            ' Save the worksheet.
            ws.Save()
            updated = True
        End If

        Return updated
    End Function
    Private Function GenerateCellFormat() As CellFormat
        Dim cellFormat1 As New CellFormat() With { _
            .NumberFormatId = CType(0, UInt32Value), _
            .FontId = CType(0, UInt32Value), _
            .FillId = CType(0, UInt32Value), _
            .BorderId = CType(0, UInt32Value), _
            .FormatId = CType(0, UInt32Value), _
            .ApplyAlignment = True _
           }
        Dim alignment1 As New Alignment() With { _
            .WrapText = True _
           }

        cellFormat1.Append(alignment1)
        Return cellFormat1
    End Function
    Public Function InsertCellFormat(ByVal workbookpart As WorkbookPart, ByVal cellformat As CellFormat) As UInteger
        Dim cellFormats As CellFormats = workbookpart.WorkbookStylesPart.Stylesheet.Elements(Of CellFormats)().First()
        cellFormats.Append(cellformat)
        Dim indexcount As UInteger
        indexcount = cellFormats.Count
        If indexcount > 0 Then indexcount = indexcount - 1
        Return indexcount
    End Function
    Public Function UpdateTableValue(ByVal sheetName As String, ByVal rs As DataTable, ByVal colnumber As Integer, ByVal rownumber As Integer, ByVal styleIndex As Integer, ByVal isString As Boolean) As Boolean
        ' Assume failure.
        Dim updated As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()

        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet


            Dim numrows As Integer
            numrows = rs.Rows.Count
            Dim iLineNo As Integer = rownumber
            Dim addressName As String = ""
            Dim Value As String = ""

            If rs.Rows.Count > 0 Then


                For i As Integer = 0 To rs.Rows.Count - 1

                    For k As Integer = 0 To rs.Columns.Count - 1
                        If 65 + colnumber + k > 90 Then
                            addressName = "A" + Trim(Chr(65 + colnumber + k - 26)) + Trim(Str(iLineNo))
                        Else
                            addressName = Trim(Chr(65 + colnumber + k)) + Trim(Str(iLineNo))
                        End If

                        Value = rs.Rows(i).Item(k).ToString()
                        Dim cell As Cell
                        cell = InsertCellInWorksheet(ws, addressName)

                        If isString Then
                            ' Either retrieve the index of an existing string,
                            ' or insert the string into the shared string table
                            ' and get the index of the new item.
                            Dim stringIndex As Integer = InsertSharedStringItem(wbPart, Value)

                            cell.CellValue = New CellValue(stringIndex.ToString())
                            cell.DataType = New EnumValue(Of CellValues)(CellValues.SharedString)
                        Else
                            cell.CellValue = New CellValue(Value)
                            cell.DataType = New EnumValue(Of CellValues)(CellValues.Number)
                        End If


                        If styleIndex > 0 Then
                            cell.StyleIndex = styleIndex
                        End If

                    Next
                    iLineNo = iLineNo + 1

                Next
            End If



            ' Save the worksheet.
            ws.Save()
            updated = True
        End If

        Return updated
    End Function


    Private Function InsertSharedStringItem(ByVal wbPart As WorkbookPart, ByVal value As String) As Integer
        Dim index As Integer = 0
        Dim found As Boolean = False
        Dim stringTablePart = wbPart.GetPartsOfType(Of SharedStringTablePart)().FirstOrDefault()

        ' If the shared string table is missing, something's wrong.
        ' Just return the index that you found in the cell.
        ' Otherwise, look up the correct text in the table.
        If stringTablePart Is Nothing Then
            ' Create it.
            stringTablePart = wbPart.AddNewPart(Of SharedStringTablePart)()
        End If

        Dim stringTable = stringTablePart.SharedStringTable
        If stringTable Is Nothing Then
            stringTable = New SharedStringTable()
        End If

        ' Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
        For Each item As SharedStringItem In stringTable.Elements(Of SharedStringItem)()
            If item.InnerText = value Then
                found = True
                Exit For
            End If
            index += 1
        Next

        If Not found Then
            stringTable.AppendChild(New SharedStringItem(New Text(value)))
            stringTable.Save()
        End If

        Return index
    End Function

    Private Function GetRow(ByVal wsData As SheetData, ByVal rowIndex As UInt32) As Row
        Dim row = wsData.Elements(Of Row)().Where(Function(r) r.RowIndex.Value = rowIndex).FirstOrDefault()
        If row Is Nothing Then
            row = New Row()
            row.RowIndex = rowIndex
            wsData.Append(row)
        End If
        Return row
    End Function

    Private Function GetRowIndex(ByVal address As String) As UInt32
        Dim rowPart As String
        Dim l As UInt32
        Dim result As UInt32 = 0

        For i As Integer = 0 To address.Length - 1
            If UInt32.TryParse(address.Substring(i, 1), l) Then
                rowPart = address.Substring(i, address.Length - i)
                If UInt32.TryParse(rowPart, l) Then
                    result = l
                    Exit For
                End If
            End If
        Next
        Return result
    End Function

    Private Function InsertCellInWorksheet(ByVal ws As Worksheet, ByVal addressName As String) As Cell
        Dim sheetData As SheetData = ws.GetFirstChild(Of SheetData)()
        Dim cell As Cell = Nothing

        Dim rowNumber As UInt32 = GetRowIndex(addressName)
        Dim row As Row = GetRow(sheetData, rowNumber)

        ' If the cell you need already exists, return it.
        ' If there is not a cell with the specified column name, insert one.  
        Dim refCell As Cell = row.Elements(Of Cell)().Where(Function(c) c.CellReference.Value = addressName).FirstOrDefault()
        If refCell IsNot Nothing Then
            cell = refCell
        Else
            cell = CreateCell(row, addressName)
        End If
        Return cell
    End Function
    Private Function CreateCell(ByVal row As Row, ByVal address As [String]) As Cell
        Dim cellResult As Cell
        Dim refCell As Cell = Nothing

        ' Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
        For Each cell As Cell In row.Elements(Of Cell)()
            'If String.Compare(cell.CellReference.Value, address, True) > 0 Then
            If GetColumnIndex(cell.CellReference.Value) > GetColumnIndex(address) Then
                refCell = cell
                Exit For
            End If
        Next

        cellResult = New Cell()
        cellResult.CellReference = address

        row.InsertBefore(cellResult, refCell)
        Return cellResult
    End Function

    Private Shared Function GetColumnIndex(ByVal cellRef As String) As System.Nullable(Of Integer)
        If String.IsNullOrEmpty(cellRef) Then
            Return Nothing
        End If

        cellRef = cellRef.ToUpper()

        Dim columnIndex As Integer = -1
        Dim mulitplier As Integer = 1

        For Each c As Char In cellRef.ToCharArray().Reverse()
            If Char.IsLetter(c) Then
                columnIndex += mulitplier * (Asc(c) - 64)
                mulitplier = mulitplier * 26
            End If
        Next

        Return columnIndex
    End Function
    Protected Sub lbMailLink_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbUpdate As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbUpdate.NamingContainer, GridViewRow)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Dim strCode As String = hdEmailId_.Value
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""

        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                lblFromPopup.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                lblDatePopup.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                lblSubjectPopup.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                lblBodyPopup.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                'EmailNo
                If strAttachment.Trim = "Y" Then
                    FillAttachmentsPopup(strCode)
                Else
                    DLAttachmentPopup.DataBind()
                End If


            End If
        End If

        FillAdditionalEmails(strCode, hdCHotelCode.Value)


        hdTrackpopupStatus.Value = "Y"
        meContractTracking.Show()
    End Sub
    Private Sub FillAttachmentsPopup(ByVal strCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,FileName,Content from Email_Inbox_Attachment where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        DLAttachmentPopup.DataSource = myDS
        DLAttachmentPopup.DataBind()

    End Sub

    Protected Sub lbAttachmentPopup_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            hdTrackpopupStatus.Value = "N"
            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lblfile As Label = CType(dlItem.FindControl("lblfile"), Label)

            Dim strpop As String
            strpop = "window.open('PriceListModule/Download.aspx?filename=" & lblfile.Text.Trim & "');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            '   Dim str As String = ex.Message
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Sub DLAttachmentPopup_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DLAttachmentPopup.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim lblfile As Label = CType(e.Item.FindControl("lblfile"), Label)
            Dim imgAttachmentType As Image = CType(e.Item.FindControl("imgAttachmentType"), Image)
            Dim filePath As String = "~\Attachment\" '+ lblfile.Text
            Dim path As String = Server.MapPath(filePath)
            Dim strExt As String = System.IO.Path.GetExtension(lblfile.Text.Replace(vbCrLf, " ")).ToUpper
            '  Dim strExt As String = System.IO.Path.GetExtension(str).ToUpper
            Select Case strExt.Trim
                Case ".PDF"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/pdf.png"
                Case ".JPG", ".JPEG", ".GIF", ".PNG"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/jpg file.png"
                Case ".DOCX"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/Word_2007.png"
                Case ".DOC"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/MS_word2003.png"
                Case ".XLSX", ".XLS"
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/excel.ico"
                Case Else
                    imgAttachmentType.ImageUrl = "~/Images/crystaltoolbar/mail-attachment2.png"
            End Select

        End If
    End Sub

    Protected Sub lbStart_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbStart As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbStart.NamingContainer, GridViewRow)
        Dim lblComplete As Label = CType(gvRow.FindControl("lblComplete"), Label)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "update Contract_Email set TaskStartDate=CONVERT(datetime, '" & strDate & "', 103) where EmailId='" & hdEmailId_.Value & "' and HotelId='" & hdCHotelCode.Value & "'"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()
            FillTrackingDashBoard()
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try

    End Sub

    Protected Sub lbApprovalStart_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbStart As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbStart.NamingContainer, GridViewRow)
        Dim lblComplete As Label = CType(gvRow.FindControl("lblApprovalComplete"), Label)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "update Contract_Email set ApprovalStartDate=CONVERT(datetime, '" & strDate & "', 103) where EmailId='" & hdEmailId_.Value & "' and HotelId='" & hdCHotelCode.Value & "'"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()
            FillApprovalTrackingDashBoard()
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try

    End Sub

    Protected Sub lbComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbComplete As LinkButton = CType(sender, LinkButton)
        Dim lbStart As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbComplete.NamingContainer, GridViewRow)
        Dim lblStartDate As Label = CType(gvRow.FindControl("lblStartDate"), Label)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Dim lblAssignedTo As Label = CType(gvRow.FindControl("lblAssignedTo"), Label)
        Dim strSqlQry As String = ""
        strSqlQry = "select count(*)cnt from Contract_Clarify where HotelId='" & hdCHotelCode.Value & "' and MailId='" & hdEmailId_.Value & "' and AssignedUser='" & lblAssignedTo.Text & "' and Clarified=0"
        Dim SqlConn As New SqlConnection
        Dim myDataAdapter As New SqlDataAdapter
        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        Dim myDt As New DataTable
        myDataAdapter.Fill(myDt)
        If myDt.Rows.Count > 0 Then
            If myDt.Rows(0)("cnt").ToString > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Status is not For Publishing, cannot click complete.' );", True)
                Exit Sub
            End If
        End If


        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "update Contract_Email set TaskCompleteDate=CONVERT(datetime, '" & strDate & "', 103) where EmailId='" & hdEmailId_.Value & "' and HotelId='" & hdCHotelCode.Value & "'"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()
            FillTrackingDashBoard()
            'If gvTracking.Rows.Count > 0 Or gvApprovalTracking.Rows.Count > 0 Then
            '    dvContractdashBoard.Visible = True
            'Else
            '    dvContractdashBoard.Visible = False
            'End If
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
    End Sub

    Protected Sub lbApprovalComplete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdTrackpopupStatus.Value = "N"
        Dim lbComplete As LinkButton = CType(sender, LinkButton)
        Dim lbStart As LinkButton = CType(sender, LinkButton)
        Dim gvRow As GridViewRow = CType(lbComplete.NamingContainer, GridViewRow)
        Dim lblStartDate As Label = CType(gvRow.FindControl("lblApprovalStartDate"), Label)
        Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
        Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)
        Dim lblApprover As Label = CType(gvRow.FindControl("lblApprover"), Label)
        Dim strSqlQry As String = ""
        strSqlQry = "select count(*)cnt from Contract_ClarifyForApproval where HotelId='" & hdCHotelCode.Value & "' and MailId='" & hdEmailId_.Value & "' and AssignedUser='" & lblApprover.Text & "' and Clarified=0"
        Dim SqlConn As New SqlConnection
        Dim myDataAdapter As New SqlDataAdapter
        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        Dim myDt As New DataTable
        myDataAdapter.Fill(myDt)
        If myDt.Rows.Count > 0 Then
            If myDt.Rows(0)("cnt").ToString > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Status is not for Publishing, cannot click complete.' );", True)
                Exit Sub
            End If
        End If


        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "update Contract_Email set ApprovalCompleteDate=CONVERT(datetime, '" & strDate & "', 103) where EmailId='" & hdEmailId_.Value & "' and HotelId='" & hdCHotelCode.Value & "'"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()
            FillApprovalTrackingDashBoard()
            'If gvTracking.Rows.Count > 0 Or gvApprovalTracking.Rows.Count > 0 Then
            '    dvContractdashBoard.Visible = True
            'Else
            '    dvContractdashBoard.Visible = False
            'End If
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
    End Sub

    Protected Sub lbInboxCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lbl1_click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub lbCloseCategorypending_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ''hdTrackpopupStatus.Value = "N"
            'Dim dtsHotelDetails As New DataTable
            'dtsHotelDetails = Session("sDtHotelDetails")

            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lb As Button = CType(dlItem.FindControl("lbInboxCategorypending"), Button)

            'If dtsHotelDetails.Rows.Count > 0 Then

            '    Dim i As Integer
            '    For i = dtsHotelDetails.Rows.Count - 1 To 0 Step i - 1
            '        'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
            '        dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
            '        'End If
            '        dtsHotelDetails.AcceptChanges()
            '    Next
            'End If
            'Session("sDtHotelDetails") = dtsHotelDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicPending")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Value").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicPending") = dtDynamics
            dlInboxSearch_pending.DataSource = dtDynamics
            dlInboxSearch_pending.DataBind()



            ' '' Create a Dynamic datatable ---- Start
            'Dim ClearDataTable = New DataTable()
            'Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            'Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            'Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            'ClearDataTable.Columns.Add(dcGroupDetailsType)
            'ClearDataTable.Columns.Add(dcGroupDetailsCode)
            'ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            ''gvHotels.DataSource = ClearDataTable
            ''gvHotels.DataBind()

            FillPendingGrid_forVS()



        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbCloseCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            hdTrackpopupStatus.Value = "N"
            Dim dtsHotelDetails As New DataTable
            dtsHotelDetails = Session("sDtHotelDetails")

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbInboxCategory"), LinkButton)

            If dtsHotelDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelDetails.Rows.Count - 1 To 0 Step i - 1
                    'If lb.Text.Trim = dtsHotelDetails.Rows(i)("Type").ToString.Trim Then
                    dtsHotelDetails.Rows.Remove(dtsHotelDetails.Rows(i))
                    'End If
                    dtsHotelDetails.AcceptChanges()
                Next
            End If
            Session("sDtHotelDetails") = dtsHotelDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicTracking")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Value").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamicTracking") = dtDynamics
            dlInboxSearch.DataSource = dtDynamics
            dlInboxSearch.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsCountry = New DataColumn("Value", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsCountry)
            'gvHotels.DataSource = ClearDataTable
            'gvHotels.DataBind()

            FillTrackingForVS()
            FillApprovalTrackingForVS()
            hdTrackpopupStatus.Value = "N"
            meContractTracking.Hide()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillTrackingForVS()

        Dim dtt As DataTable
        dtt = Session("sDtDynamicTracking")

        Dim strHotelValue As String = ""
        Dim strEmailcodeValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strEmailDateValue As String = ""
        Dim strEmailSubjectValue As String = ""
        Dim strUpdateType As String = ""
        Dim strAssignedTo As String = ""
        Dim strProgressStage As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "EMAIL CODE" Then
                    If strEmailcodeValue <> "" Then
                        strEmailcodeValue = strEmailcodeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailcodeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL DATE" Then
                    If strEmailDateValue <> "" Then
                        strEmailDateValue = strEmailDateValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailDateValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL SUBJECT" Then
                    If strEmailSubjectValue <> "" Then
                        strEmailSubjectValue = strEmailSubjectValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailSubjectValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TEXT" Then
                    If strTextValue <> "" Then
                        strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strTextValue = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "UPDATE TYPE" Then
                    If strUpdateType <> "" Then
                        strUpdateType = strUpdateType + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strUpdateType = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "ASSIGNED TO" Then
                    If strAssignedTo <> "" Then
                        strAssignedTo = strAssignedTo + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strAssignedTo = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "PROGRESS STAGE" Then
                    If strProgressStage <> "" Then
                        strProgressStage = strProgressStage + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strProgressStage = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
            Next
        End If


        Dim strWhereCond As String = ""
        '  lblMsg.Visible = False


        Dim strSqlQry As String = ""


        strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,rtrim(EmailSubject)EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,AssignedTo,AssignedToName,TaskStartDate,TaskCompleteDate from VIEW_TRACKING where  AssignedTo= '" & Session("GlobalUserName") & "'  and TaskCompleteDate is null "

        If strHotelValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            Else
                strWhereCond = strWhereCond & " AND upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            End If
        End If


        If strEmailcodeValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            End If
        End If

        If strHotelStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelStatus) =" & Trim(strHotelStatusValue) & ""
            Else
                strWhereCond = strWhereCond & " AND HotelStatus =" & Trim(strHotelStatusValue) & ""
            End If
        End If

        If strTrackingStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(TrackingStatus)=" & Trim(strTrackingStatusValue) & ""
            Else

                strWhereCond = strWhereCond & " AND   TrackingStatus=" & Trim(strTrackingStatusValue) & ""
            End If
        End If

        If strFromEmailValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
            End If
        End If

        If strEmailDateValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailDate) IN ( " & Trim(strEmailDateValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailDate) IN (" & Trim(strEmailDateValue) & ")"
            End If
        End If
        If strEmailSubjectValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailSubject) IN ( " & Trim(strEmailSubjectValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailSubject) IN (" & Trim(strEmailSubjectValue) & ")"
            End If
        End If
        If strProgressStage <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ProgressStage) IN ( " & Trim(strProgressStage) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(ProgressStage) IN (" & Trim(strProgressStage) & ")"
            End If
        End If

        If strUpdateType <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(UpdateTypeName) IN ( " & Trim(strUpdateType) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(UpdateTypeName) IN (" & Trim(strUpdateType) & ")"
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
                        strWhereCond1 = "  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond
        Else
            strSqlQry = strSqlQry
        End If



        Dim myDS As New DataSet
        Try
            '   strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd from VIEW_TRACKING order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gvTracking.DataSource = myDS
            gvTracking.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillPendingGrid_forVS()

        Dim dtt As DataTable
        dtt = Session("sDtDynamicPending")

        Dim strHotelValue As String = ""

        Dim strTextValue As String = ""

        Dim strctryValue As String = ""
        Dim strcityValue As String = ""
        Dim strsectorValue As String = ""
        Dim strcategoryValue As String = ""
        Dim strhotchainValue As String = ""
        Dim pagevaluecus = RowsPerPageCUS.SelectedValue

        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTEL" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If



                If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                    If strctryValue <> "" Then
                        strctryValue = strctryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strctryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If



                If dtt.Rows(i)("Code").ToString = "CITY" Then
                    If strcityValue <> "" Then
                        strcityValue = strcityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strcityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If




                If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                    If strsectorValue <> "" Then
                        strsectorValue = strsectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strsectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                    If strcategoryValue <> "" Then
                        strcategoryValue = strcategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strcategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If




                If dtt.Rows(i)("Code").ToString = "HOTELCHAIN" Then
                    If strhotchainValue <> "" Then
                        strhotchainValue = strhotchainValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strhotchainValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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


        Dim strWhereCond As String = ""
        '  lblMsg.Visible = False


        Dim strSqlQry, strSqlQry1 As String

        strSqlQry = "select v.contractid ,v.partycode,v.partyname,'Contract' promotionname,v.applicableto,convert(varchar(10),(convert(datetime,v.fromdate,111)),103) fromdate,convert(varchar(10),(convert(datetime,v.todate,111)),103) todate,v.activestate  from view_contracts_search_pending  v  left join partymast p on  v.partycode =p.partycode   inner join ctrymast  c on p.ctrycode=c.ctrycode  inner join citymast ct on p.citycode=ct.citycode inner join sectormaster s  on p.sectorcode= s.sectorcode inner join catmast cat  on p.catcode=cat.catcode  left join hotelchainmaster ch on p.hotelchaincode =ch.hotelchaincode where [status]='No' and v.withdraw=0  and contractid<>'' "
        'Else
        strSqlQry1 = " union select v.promotionid contractid,v.partycode ,v.partyname, v.promotionname,v.applicableto,v.fromdate,v.todate,v.activestate from view_offer_search_pending v   left join partymast p on  v.partycode =p.partycode   inner join ctrymast  c on p.ctrycode=c.ctrycode  inner join citymast ct on p.citycode=ct.citycode inner join sectormaster s  on p.sectorcode= s.sectorcode inner join catmast cat  on p.catcode=cat.catcode  left join hotelchainmaster ch on p.hotelchaincode =ch.hotelchaincode where [status]='No' and (v.activestate='With Drawn' or v.activestate='Active' )   and promotionid<>''  "
        'End If




        'If Trim(BuildCondition) <> "" Then




        If strHotelValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(p.partyname) in ( " & Trim(strHotelValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(p.partyname) in (" & Trim(strHotelValue.Trim.ToUpper) & ") "
            End If
        End If


        If strctryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(c.ctryname)  in ( " & Trim(strctryValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(c.ctryname)  in ( " & Trim(strctryValue.Trim.ToUpper) & ") "
            End If
        End If


        If strcityValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ct.cityname)  in ( " & Trim(strcityValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(ct.cityname)  in ( " & Trim(strcityValue.Trim.ToUpper) & ") "
            End If
        End If


        If strsectorValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(s.sectorname)  in ( " & Trim(strsectorValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(s.sectorname)  in ( " & Trim(strsectorValue.Trim.ToUpper) & ") "
            End If
        End If


        If strcategoryValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(cat.catname)  in ( " & Trim(strcategoryValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(cat.catname)  in ( " & Trim(strcategoryValue.Trim.ToUpper) & ") "
            End If
        End If

        If strhotchainValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ch.hotelchainname)  in ( " & Trim(strhotchainValue.Trim.ToUpper) & ") "
            Else
                strWhereCond = strWhereCond & " AND upper(ch.hotelchainname)  in (" & Trim(strhotchainValue.Trim.ToUpper) & ") "
            End If
        End If
        Dim strcontractid As String = ""
        Dim strofferid As String = ""
        If strTextValue <> "" Then

            Dim lsMainArr As String()
            Dim strValue As String = ""
            Dim strWhereCond1 As String = ""
            lsMainArr = objUtils.splitWithWords(strTextValue, ",")
            For i = 0 To lsMainArr.GetUpperBound(0)
                strValue = ""
                strValue = lsMainArr(i)
                If strValue <> "" Then
                    'If Trim(strWhereCond1) = "" Then
                    '    strWhereCond1 = "  upper(p.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  "
                    'Else
                    '    strWhereCond1 = strWhereCond1 & " OR  upper(p.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    'End If


                    If Trim(strWhereCond1) = "" Then
                        strWhereCond1 = " ( upper(p.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(c.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   c.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(s.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(ct.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(cat.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   p.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR   (upper(p.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(c.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   c.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(s.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(ct.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(cat.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   p.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                    End If


                    'strcontractid = strcontractid & "OR v.contractid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                    'strofferid = strofferid & "OR v.promotionid like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond & strcontractid
        Else
            strSqlQry = strSqlQry
        End If




        If Trim(strWhereCond) <> "" Then
            strSqlQry1 = strSqlQry1 & " and " & strWhereCond & strofferid
        Else
            strSqlQry1 = strSqlQry1
        End If





        strSqlQry = strSqlQry & strSqlQry1 & "order by partyname ,contractid "

        Dim myDS As New DataSet
        Try
            '   strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd from VIEW_TRACKING order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_pendingcontracts.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_pendingcontracts.PageSize = pagevaluecus
            End If
            gv_pendingcontracts.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Private Sub FillApprovalTrackingForVS()

        Dim dtt As DataTable
        dtt = Session("sDtDynamicTracking")

        Dim strHotelValue As String = ""
        Dim strEmailcodeValue As String = ""
        Dim strHotelStatusValue As String = ""
        Dim strTrackingStatusValue As String = ""
        Dim strFromEmailValue As String = ""
        Dim strEmailDateValue As String = ""
        Dim strEmailSubjectValue As String = ""
        Dim strUpdateType As String = ""
        Dim strAssignedTo As String = ""
        Dim strProgressStage As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELS" Then
                    If strHotelValue <> "" Then
                        strHotelValue = strHotelValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "EMAIL CODE" Then
                    If strEmailcodeValue <> "" Then
                        strEmailcodeValue = strEmailcodeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailcodeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTEL STATUS" Then
                    If strHotelStatusValue <> "" Then
                        strHotelStatusValue = strHotelStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "TRACKING STATUS" Then
                    If strTrackingStatusValue <> "" Then
                        strTrackingStatusValue = strTrackingStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strTrackingStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "FROM EMAIL" Then
                    If strFromEmailValue <> "" Then
                        strFromEmailValue = strFromEmailValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strFromEmailValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL DATE" Then
                    If strEmailDateValue <> "" Then
                        strEmailDateValue = strEmailDateValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailDateValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "EMAIL SUBJECT" Then
                    If strEmailSubjectValue <> "" Then
                        strEmailSubjectValue = strEmailSubjectValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strEmailSubjectValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "TEXT" Then
                    If strTextValue <> "" Then
                        strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strTextValue = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "UPDATE TYPE" Then
                    If strUpdateType <> "" Then
                        strUpdateType = strUpdateType + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strUpdateType = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "ASSIGNED TO" Then
                    If strAssignedTo <> "" Then
                        strAssignedTo = strAssignedTo + "," + dtt.Rows(i)("Value").ToString
                    Else
                        strAssignedTo = dtt.Rows(i)("Value").ToString
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "PROGRESS STAGE" Then
                    If strProgressStage <> "" Then
                        strProgressStage = strProgressStage + ", '" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strProgressStage = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
            Next
        End If


        Dim strWhereCond As String = ""
        '  lblMsg.Visible = False


        Dim strSqlQry As String = ""


        strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,rtrim(EmailSubject)EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,convert(varchar(16),ApprovalStart,103)+ ' ' + convert(varchar(5),ApprovalStart,108)ApprovalStart,convert(varchar(16),ApprovalEnd,103)+ ' ' + convert(varchar(5),ApprovalEnd,108)ApprovalEnd,UpdateTypeCode,UpdateTypeName,convert(varchar(10),ValidFrom,103)ValidFrom,convert(varchar(10),ValidTo,103)ValidTo,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,AssignedTo,AssignedToName,TaskStartDate,TaskCompleteDate,convert(varchar(16),ApprovalAssignmentDate,103)+ ' ' + convert(varchar(5),ApprovalAssignmentDate,108)ApprovalAssignmentDate,Approver from VIEW_TRACKING where   Approver= '" & Session("GlobalUserName") & "'  and ApprovalAssignmentDate is not null  and ApprovalEnd is null "

        If strHotelValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            Else
                strWhereCond = strWhereCond & " AND upper(HotelName) = " & Trim(strHotelValue.Trim.ToUpper) & " "
            End If
        End If


        If strEmailcodeValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  IN ( " & Trim(strEmailcodeValue) & ")"
            End If
        End If

        If strHotelStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(HotelStatus) =" & Trim(strHotelStatusValue) & ""
            Else
                strWhereCond = strWhereCond & " AND HotelStatus =" & Trim(strHotelStatusValue) & ""
            End If
        End If

        If strTrackingStatusValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(TrackingStatus)=" & Trim(strTrackingStatusValue) & ""
            Else

                strWhereCond = strWhereCond & " AND   TrackingStatus=" & Trim(strTrackingStatusValue) & ""
            End If
        End If

        If strFromEmailValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailFrom) IN ( " & Trim(strFromEmailValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailFrom) IN (" & Trim(strFromEmailValue) & ")"
            End If
        End If

        If strEmailDateValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailDate) IN ( " & Trim(strEmailDateValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailDate) IN (" & Trim(strEmailDateValue) & ")"
            End If
        End If
        If strEmailSubjectValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(EmailSubject) IN ( " & Trim(strEmailSubjectValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(EmailSubject) IN (" & Trim(strEmailSubjectValue) & ")"
            End If
        End If
        If strProgressStage <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(ProgressStage) IN ( " & Trim(strProgressStage) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(ProgressStage) IN (" & Trim(strProgressStage) & ")"
            End If
        End If

        If strUpdateType <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(UpdateTypeName) IN ( " & Trim(strUpdateType) & ")"
            Else

                strWhereCond = strWhereCond & " AND upper(UpdateTypeName) IN (" & Trim(strUpdateType) & ")"
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
                        strWhereCond1 = "  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(HotelName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   HotelStatus like '%" & Trim(strValue.Trim.ToUpper) & "%' or TrackingStatus LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailFrom) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') or  upper(EmailSubject) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%')  or  upper(EmailDate) LIKE ('%" & Trim(strValue.Trim.ToUpper) & "%') "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = " (" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If



        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond
        Else
            strSqlQry = strSqlQry
        End If



        Dim myDS As New DataSet
        Try
            '   strSqlQry = "select EmailLineNo,EmailId,EmailNo,HotelCode,rtrim(HotelName)HotelName,HotelStatus,TrackingStatus,EmailSubject,EmailDate,EmailTime,ProgressStage,UpdateStart,UpdateEnd,ApprovalStart,ApprovalEnd from VIEW_TRACKING order by EmailLineNo"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gvApprovalTracking.DataSource = myDS
            gvApprovalTracking.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Protected Sub btnvsprocess_pending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess_pending.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit_pending.Text.Replace("___", "<").Replace("...", ">").Replace("FW:", "FW.")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELCHAIN"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("HOTELCHAIN", lsProcessText, "H")
                    IsProcessType = "H"
                Case "HOTEL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("HOTEL", lsProcessText, "H")
                    IsProcessType = "H"

                Case "CITY"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("CITY", lsProcessText, "H")
                    IsProcessType = "H"
                Case "SECTOR"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("SECTOR", lsProcessText, "H")
                    IsProcessType = "H"
                Case "CATEGORY"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("CATEGORY", lsProcessText, "H")
                    IsProcessType = "H"
                Case "COUNTRY"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("COUNTRY", lsProcessText, "H")
                    IsProcessType = "H"

                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopupPending("TEXT", lsProcessText, "H")
                    IsProcessType = "H"


            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicPending")
        dlInboxSearch_pending.DataSource = dttDyn
        dlInboxSearch_pending.DataBind()
        FillPendingGrid_forVS()
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text.Replace("___", "<").Replace("...", ">").Replace("FW:", "FW.")
        Dim lsProcessText As String = ""
        Dim lsMainArr As String()
        Dim IsProcessType As String = ""
        Dim IsProcessValue As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "HOTELS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("HOTELS", lsProcessText, "H")
                    IsProcessType = "H"
                Case "EMAIL CODE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("EMAIL CODE", lsProcessText, "EC")
                    IsProcessType = "TN"
                Case "HOTEL STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("HOTEL STATUS", lsProcessText, "HS")
                    IsProcessType = "HS"
                Case "TRACKING STATUS"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("TRACKING STATUS", lsProcessText, "TS")
                    IsProcessType = "TS"
                Case "FROM EMAIL"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("FROM EMAIL", lsProcessText, "FM")
                    IsProcessType = "FM"
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("TEXT", lsProcessText, "T")
                    IsProcessType = "T"
                Case "EMAIL DATE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("EMAIL DATE", lsProcessText, "ED")
                    IsProcessType = "ED"
                Case "EMAIL SUBJECT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    lsProcessText = lsProcessText.Replace("FW.", "FW:")
                    sbAddToDataTablePopup("EMAIL SUBJECT", lsProcessText, "ES")
                    IsProcessType = "ES"
                Case "UPDATE TYPE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("UPDATE TYPE", lsProcessText, "UT")
                    IsProcessType = "UT"
                Case "ASSIGNED TO"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("ASSIGNED TO", lsProcessText, "AT")
                    IsProcessType = "AT"
                Case "PROGRESS STAGE"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTablePopup("PROGRESS STAGE", lsProcessText, "PS")
                    IsProcessType = "PS"
            End Select
        Next

        Dim dttDyn As DataTable
        dttDyn = Session("sDtDynamicTracking")
        dlInboxSearch.DataSource = dttDyn
        dlInboxSearch.DataBind()
        FillTrackingForVS()
        FillApprovalTrackingForVS()
    End Sub
    Function sbAddToDataTablePopup(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicTracking")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicTracking") = dtt
            End If
        End If
        Return True
    End Function
    Function sbAddToDataTablePopupPending(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicPending")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim, lsValue.Trim)
                Session("sDtDynamicPending") = dtt
            End If
        End If
        Return True
    End Function

    Protected Sub lbClarify_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            hdTrackpopupStatus.Value = "N"
            Dim lbClarify As LinkButton = CType(sender, LinkButton)
            Dim gvRow As GridViewRow = CType(lbClarify.NamingContainer, GridViewRow)
            Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
            Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)

            Dim lblEmailCode As Label = CType(gvRow.FindControl("lblEmailCode"), Label)
            Dim lblCHotelName As Label = CType(gvRow.FindControl("lblCHotelName"), Label)
            Dim lblAssignedDate As Label = CType(gvRow.FindControl("lblAssignedDate"), Label)
            Dim lblAssignedTo As Label = CType(gvRow.FindControl("lblAssignedTo"), Label)

            Dim strSqlQry As String = ""
            strSqlQry = "select count(*)cnt from Contract_Clarify where HotelId='" & hdCHotelCode.Value & "' and MailId='" & hdEmailId_.Value & "' and AssignedUser='" & lblAssignedTo.Text & "' and Clarified=0"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            Dim myDt As New DataTable
            myDataAdapter.Fill(myDt)
            If myDt.Rows.Count > 0 Then
                If myDt.Rows(0)("cnt").ToString > 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Status is not For Publishing, cannot click clarify.' );", True)
                    Exit Sub
                End If
            End If


            lblEmailCodePopup.Text = lblEmailCode.Text
            lblHotelNamePopup.Text = lblCHotelName.Text
            lblAssignDate.Text = lblAssignedDate.Text
            Dim strCode As String = hdEmailId_.Value
            hdPopupHotelCode.Value = hdCHotelCode.Value
            hdPopupMailId.Value = hdEmailId_.Value
            hdAssignedUser.Value = lblAssignedTo.Text
            FillClarifyGrid()
            meClarify.Show()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lbApprovalClarify_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            hdTrackpopupStatus.Value = "N"
            Dim lbClarify As LinkButton = CType(sender, LinkButton)
            Dim gvRow As GridViewRow = CType(lbClarify.NamingContainer, GridViewRow)
            Dim hdEmailId_ As HiddenField = CType(gvRow.FindControl("hdEmailId_"), HiddenField)
            Dim hdCHotelCode As HiddenField = CType(gvRow.FindControl("hdCHotelCode"), HiddenField)

            Dim lblEmailCode As Label = CType(gvRow.FindControl("lblEmailCode"), Label)
            Dim lblCHotelName As Label = CType(gvRow.FindControl("lblCHotelName"), Label)
            Dim lblApprovalAssignedDate As Label = CType(gvRow.FindControl("lblApprovalAssignedDate"), Label)
            Dim lblApprover As Label = CType(gvRow.FindControl("lblApprover"), Label)

            Dim strSqlQry As String = ""
            strSqlQry = "select count(*)cnt from Contract_ClarifyForApproval where HotelId='" & hdCHotelCode.Value & "' and MailId='" & hdEmailId_.Value & "' and AssignedUser='" & lblApprover.Text & "' and Clarified=0"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            Dim myDt As New DataTable
            myDataAdapter.Fill(myDt)
            If myDt.Rows.Count > 0 Then
                If myDt.Rows(0)("cnt").ToString > 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Status is not For Publishing, cannot click clarify.' );", True)
                    Exit Sub
                End If
            End If


            lblEmailCodePopup1.Text = lblEmailCode.Text
            lblHotelNamePopup1.Text = lblCHotelName.Text
            lblApprovalAssignDate.Text = lblApprovalAssignedDate.Text
            Dim strCode As String = hdEmailId_.Value
            hdPopupHotelCode1.Value = hdCHotelCode.Value
            hdPopupMailId1.Value = hdEmailId_.Value
            hdAssignedUser1.Value = lblApprover.Text
            FillApprovalClarifyGrid()
            meApprovalClarify.Show()

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub brnClarifySubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles brnClarifySubmit.Click
        hdTrackpopupStatus.Value = "N"
        If txtClarifyRemark.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Remarks.' );", True)
            Exit Sub
        End If
        Dim strSqlQry As String = ""
        strSqlQry = "select count(*)cnt from Contract_Clarify where HotelId='" & hdPopupHotelCode.Value & "' and MailId='" & hdPopupMailId.Value & "' and AssignedUser='" & hdAssignedUser.Value & "' and Clarified=0"
        Dim SqlConn As New SqlConnection
        Dim myDataAdapter As New SqlDataAdapter
        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        Dim myDt As New DataTable
        myDataAdapter.Fill(myDt)
        If myDt.Rows.Count > 0 Then
            If myDt.Rows(0)("cnt").ToString > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Previous clarification is still pending.' );", True)
                Exit Sub
            End If
        End If




        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "insert into Contract_Clarify (HotelId,MailId,ClarifyRemarks,ClarifyDate,Clarified,AssignedUser) values ('" & hdPopupHotelCode.Value & "' ,'" & hdPopupMailId.Value & "' ,'" & txtClarifyRemark.Text.Replace("'", "''").Trim & "',CONVERT(datetime, '" & strDate & "', 103),0,'" & hdAssignedUser.Value & "'); Insert into contractsClarifyLogs (UserName,HotelId,MailId,ProgressStatus,LogDate,Comments,AssignedDate) values ('" & hdAssignedUser.Value & "','" & hdPopupHotelCode.Value & "','" & hdPopupMailId.Value & "','Pending Clarification',CONVERT(datetime, '" & strDate & "', 103),'" & txtClarifyRemark.Text.Replace("'", "''").Trim & "',CONVERT(datetime, '" & lblAssignDate.Text & "', 103) )"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()




            txtClarifyRemark.Text = ""
            FillTrackingDashBoard()
            FillClarifyGrid()
        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
    End Sub

    Protected Sub btnApprovalClarifySubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprovalClarifySubmit.Click
        hdTrackpopupStatus.Value = "N"
        If txtAppClarifyRemarks.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Remarks.' );", True)
            Exit Sub
        End If
        Dim strSqlQry As String = ""
        strSqlQry = "select count(*)cnt from Contract_ClarifyForApproval where HotelId='" & hdPopupHotelCode1.Value & "' and MailId='" & hdPopupMailId1.Value & "' and AssignedUser='" & hdAssignedUser1.Value & "' and Clarified=0"
        Dim SqlConn As New SqlConnection
        Dim myDataAdapter As New SqlDataAdapter
        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        Dim myDt As New DataTable
        myDataAdapter.Fill(myDt)
        If myDt.Rows.Count > 0 Then
            If myDt.Rows(0)("cnt").ToString > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Previous clarification is still pending.' );", True)
                Exit Sub
            End If
        End If


        Dim strDate As String = Date.Now.ToString("dd/MM/yyyy") & " " & Date.Now.ToString("hh:mm")
        Dim strQuery As String = "insert into Contract_ClarifyForApproval (HotelId,MailId,ClarifyRemarks,ClarifyDate,Clarified,AssignedUser) values ('" & hdPopupHotelCode1.Value & "' ,'" & hdPopupMailId1.Value & "' ,'" & txtAppClarifyRemarks.Text.Replace("'", "''").Trim & "',CONVERT(datetime, '" & strDate & "', 103),0,'" & hdAssignedUser1.Value & "'); Insert into ContractsClarifyForApprovalLogs (UserName,HotelId,MailId,ProgressStatus,LogDate,Comments,AssignedDate) values ('" & hdAssignedUser1.Value & "','" & hdPopupHotelCode1.Value & "','" & hdPopupMailId1.Value & "','Pending Clarification',CONVERT(datetime, '" & strDate & "', 103),'" & txtAppClarifyRemarks.Text.Replace("'", "''").Trim & "',CONVERT(datetime, '" & lblApprovalAssignDate.Text & "', 103) )"
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            sqlTrans = mySqlConn.BeginTransaction()
            Dim norow As Integer = objUtils.ExecuteNonQuerynew(Session("dbconnectionName"), strQuery, mySqlConn, sqlTrans)
            sqlTrans.Commit()
            txtAppClarifyRemarks.Text = ""
            FillApprovalTrackingDashBoard()
            FillApprovalClarifyGrid()

        Catch ex As Exception
            sqlTrans.Rollback()
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
    End Sub

    Private Sub FillClarifyGrid()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Try
            ' strSqlQry = "select EmailLineNo,(select partyname from partymast where partycode=cc.HotelId)HotelName,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,cc.ClarifyRemarks,case when cc.Clarified=1 then ' Clarified' else 'Pending Clarification' end Status from Contract_Clarify CC,VIEW_CONTRACT_EMAIL_WITH_LINENO V,Contract_Email E where CC.MailId=v.EmailId and cc.HotelId=v.HotelId and CC.MailId=E.EmailId and cc.HotelId=E.HotelId and E.AssignedTo=CC.AssignedUser and cc.HotelId='" & hdPopupHotelCode.Value & "' and cc.MailId='" & hdPopupMailId.Value & "' and cc.AssignedUser='" & hdAssignedUser.Value.Trim & "'"
            strSqlQry = "select EmailLineNo,(select partyname from partymast where partycode=cc.HotelId)HotelName,convert(varchar(16),CC.LogDate,103)+ ' ' + convert(varchar(5),CC.LogDate,108) AssignedDate,cc.Comments ClarifyRemarks,case when cc.ProgressStatus='Reassigned' then ' Clarified' else 'Pending Clarification' end Status from ContractsClarifyLogs CC,VIEW_CONTRACT_EMAIL_WITH_LINENO V,Contract_Email E where CC.MailId=v.EmailId and cc.HotelId=v.HotelId and CC.MailId=E.EmailId and cc.HotelId=E.HotelId and E.AssignedTo=CC.UserName and cc.HotelId='" & hdPopupHotelCode.Value & "' and cc.MailId='" & hdPopupMailId.Value & "' and cc.UserName='" & hdAssignedUser.Value.Trim & "'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    dvContractdashBoard.Visible = True
            'Else
            '    dvContractdashBoard.Visible = False
            'End If
            gvClarify.DataSource = myDS
            gvClarify.DataBind()

        Catch ex As Exception
        End Try
    End Sub

    Private Sub FillApprovalClarifyGrid()
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Try
            ' strSqlQry = "select EmailLineNo,(select partyname from partymast where partycode=cc.HotelId)HotelName,convert(varchar(16),AssignedDate,103)+ ' ' + convert(varchar(5),AssignedDate,108) AssignedDate,cc.ClarifyRemarks,case when cc.Clarified=1 then ' Clarified' else 'Pending Clarification' end Status from Contract_Clarify CC,VIEW_CONTRACT_EMAIL_WITH_LINENO V,Contract_Email E where CC.MailId=v.EmailId and cc.HotelId=v.HotelId and CC.MailId=E.EmailId and cc.HotelId=E.HotelId and E.AssignedTo=CC.AssignedUser and cc.HotelId='" & hdPopupHotelCode.Value & "' and cc.MailId='" & hdPopupMailId.Value & "' and cc.AssignedUser='" & hdAssignedUser.Value.Trim & "'"
            strSqlQry = "select EmailLineNo,(select partyname from partymast where partycode=cc.HotelId)HotelName,convert(varchar(16),CC.LogDate,103)+ ' ' + convert(varchar(5),CC.LogDate,108) AssignedDate,cc.Comments ClarifyRemarks,case when cc.ProgressStatus='Approval Reassigned' then ' Clarified' else 'Pending Clarification' end Status from ContractsClarifyForApprovalLogs CC,VIEW_CONTRACT_EMAIL_WITH_LINENO V,Contract_Email E where CC.MailId=v.EmailId and cc.HotelId=v.HotelId and CC.MailId=E.EmailId and cc.HotelId=E.HotelId and E.AssignedTo=CC.UserName and cc.HotelId='" & hdPopupHotelCode1.Value & "' and cc.MailId='" & hdPopupMailId1.Value & "' and cc.UserName='" & hdAssignedUser1.Value.Trim & "'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    dvContractdashBoard.Visible = True
            'Else
            '    dvContractdashBoard.Visible = False
            'End If
            gvApprovalClarify.DataSource = myDS
            gvApprovalClarify.DataBind()

        Catch ex As Exception
        End Try
    End Sub
    Protected Sub lbInboxCategorypending_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Protected Sub lbAdditionalEmails_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbAdditionalEmails As LinkButton = CType(sender, LinkButton)
        Dim dlRow As DataListItem = CType(lbAdditionalEmails.NamingContainer, DataListItem)
        Dim hdAdditionalEmails As HiddenField = CType(dlRow.FindControl("hdAdditionalEmails"), HiddenField)

        Dim strCode As String = hdAdditionalEmails.Value
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select EmailId,EmailFrom,'Sub: '+EmailSubject EmailSubject,CONVERT(VARCHAR(11), EmailDate, 113) + ' ' +right(convert(varchar(32),EmailDate,100),8)EmailDate,EmailFullSource,EmailAttachment,RIGHT('000000'+CAST(EmailId AS VARCHAR(6)),6)EmailNo from Email_Inbox where EmailId=" & strCode
        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")
        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                lblFromPopup.Text = Server.HtmlEncode(myDS.Tables(0).Rows(0)("EmailFrom").ToString)
                lblDatePopup.Text = myDS.Tables(0).Rows(0)("EmailDate").ToString
                lblSubjectPopup.Text = myDS.Tables(0).Rows(0)("EmailSubject").ToString
                lblBodyPopup.Text = (myDS.Tables(0).Rows(0)("EmailFullSource").ToString.Replace("img", "img style='Display:none;'")).Replace(":blue", ":#06788B")
                strAttachment = myDS.Tables(0).Rows(0)("EmailAttachment").ToString
                'EmailNo
                If strAttachment.Trim = "Y" Then
                    FillAttachmentsPopup(strCode)
                Else
                    DLAttachmentPopup.DataBind()
                End If

            End If
        End If
    End Sub

    Private Sub FillAdditionalEmails(ByVal strCode As String, ByVal strhotelCode As String)
        Dim SqlConn As SqlConnection
        Dim myDataAdapter As SqlDataAdapter
        Dim myDS As New DataSet
        Dim strAttachment As String = ""
        Dim strSqlQry As String = "select " & strCode & " AdditionalEmailId,RIGHT('000000'+CAST(" & strCode & " AS VARCHAR(6)),6)+'(Original)' as AdditionalEmailIdName union select AdditionalEmailId,RIGHT('000000'+CAST(AdditionalEmailId AS VARCHAR(6)),6)AdditionalEmailIdName from Contract_Email_Additional where EmailId='" & strCode & "' and HotelId='" & strhotelCode & "'  order by AdditionalEmailId desc"

        SqlConn = clsDBConnect.dbConnectionnew(HttpContext.Current.Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS, "Details")

        If myDS.Tables.Count > 0 Then
            If myDS.Tables(0).Rows.Count > 0 Then
                dlAdditionalEmails.DataSource = myDS
                dlAdditionalEmails.DataBind()
            Else
                dlAdditionalEmails.DataBind()
            End If
        Else
            dlAdditionalEmails.DataBind()
        End If
        hdTrackpopupStatus.Value = "Y"

    End Sub

    Protected Sub lbCloseTrack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lbCloseTrack.Click
        hdTrackpopupStatus.Value = "N"
        If hdTrackpopupStatus.Value = "Y" Then
            meContractTracking.Show()
        Else
            meContractTracking.Hide()
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If hdTrackpopupStatus Is Nothing Then
            meContractTracking.Hide()
        Else
            If hdTrackpopupStatus.Value = "Y" Then
                meContractTracking.Show()
            Else
                meContractTracking.Hide()
            End If
        End If

    End Sub

    Protected Sub gvApprovalTracking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvApprovalTracking.RowDataBound
        Try

            'gvTracking.Columns("yourColumnName").Frozen = True
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).CssClass = "lockedHeader"

                e.Row.Cells(2).CssClass = "lockedHeaderAssDate"
                e.Row.Cells(4).CssClass = "lockedHeader"
                e.Row.Cells(6).CssClass = "lockedHeader"
                e.Row.Cells(8).CssClass = "lockedHeader"
                e.Row.Cells(10).CssClass = "lockedHeader"
                e.Row.Cells(12).CssClass = "lockedHeaderLast"
                ' e.Row.Cells(8).CssClass = "lockedHeaderNext"


            Else
                If iApprovalFlag = 0 Then

                    e.Row.Cells(0).CssClass = "locked"
                    'e.Row.Cells(0).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    'e.Row.Cells(1).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    e.Row.Cells(2).CssClass = "lockedAssDate"
                    'e.Row.Cells(2).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    'e.Row.Cells(3).BackColor = Drawing.Color.FromArgb(255, 215, 243)
                    e.Row.Cells(4).CssClass = "locked"
                    e.Row.Cells(6).CssClass = "locked"
                    e.Row.Cells(8).CssClass = "locked"
                    e.Row.Cells(10).CssClass = "locked"
                    e.Row.Cells(12).CssClass = "lockedLast"
                    iApprovalFlag = 1
                Else

                    e.Row.Cells(0).CssClass = "lockedAlternative"
                    'e.Row.Cells(0).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    'e.Row.Cells(1).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    e.Row.Cells(2).CssClass = "lockedAlternativeAssDate"
                    'e.Row.Cells(2).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    'e.Row.Cells(3).BackColor = Drawing.Color.FromArgb(255, 255, 191)
                    e.Row.Cells(4).CssClass = "lockedAlternative"
                    e.Row.Cells(6).CssClass = "lockedAlternative"
                    e.Row.Cells(8).CssClass = "lockedAlternative"
                    e.Row.Cells(10).CssClass = "lockedAlternative"
                    e.Row.Cells(12).CssClass = "lockedAlternativeLast"
                    'e.Row.Cells(8).CssClass = "lockedAlternativeNext"
                    iApprovalFlag = 0
                End If

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lblStartDate As Label = CType(e.Row.FindControl("lblApprovalStartDate"), Label)
                Dim lblComplete As Label = CType(e.Row.FindControl("lblApprovalComplete"), Label)
                Dim lbStart As LinkButton = CType(e.Row.FindControl("lbApprovalStart"), LinkButton)
                Dim lbComplete As LinkButton = CType(e.Row.FindControl("lbApprovalComplete"), LinkButton)
                Dim lblStartDate1 As Label = CType(e.Row.FindControl("lblApprovalStartDate1"), Label)
                Dim lblComplete1 As Label = CType(e.Row.FindControl("lblApprovalComplete1"), Label)
                Dim lbStart1 As LinkButton = CType(e.Row.FindControl("lbApprovalStart1"), LinkButton)
                Dim lbComplete1 As LinkButton = CType(e.Row.FindControl("lbApprovalComplete1"), LinkButton)
                lbStart.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to start approval now?')==false)return false;")
                lbComplete.Attributes.Add("onclick", "javascript:if(confirm(' Are you sure to mark the approval as complete?')==false)return false;")

                If lblStartDate.Text = "" Then
                    lbStart.Visible = True
                    lblStartDate.Visible = False
                    lblComplete.Visible = False
                    lbComplete.Visible = False
                    lbStart1.Visible = True
                    lblStartDate1.Visible = False
                    lblComplete1.Visible = False
                    lbComplete1.Visible = False
                Else
                    lbStart.Visible = False
                    lblStartDate.Visible = True
                    lbStart1.Visible = False
                    lblStartDate1.Visible = True
                    If lblComplete.Text = "" Then
                        lbComplete.Visible = True
                        lblComplete.Visible = False
                        lbComplete1.Visible = True
                        lblComplete1.Visible = False
                    Else
                        lbComplete.Visible = False
                        lblComplete.Visible = True
                        lbComplete1.Visible = False
                        lblComplete1.Visible = True
                    End If

                End If



                Dim lblProgressStage As Label = CType(e.Row.FindControl("lblProgressStage"), Label)
                If lblProgressStage.Text = "Reassigned" Then
                    'e.Row.CssClass = "Pending"
                    e.Row.BackColor = System.Drawing.Color.Pink
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub gv_pendingcontracts_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_pendingcontracts.PageIndexChanging
        gv_pendingcontracts.PageIndex = e.NewPageIndex

        Dim dtt As DataTable
        dtt = Session("sDtDynamicPending")

        If dtt.Rows.Count > 0 Then
            FillPendingGrid_forVS()
        Else
            FillPendingGrid()
        End If
    End Sub

    Protected Sub gv_pendingcontracts_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_pendingcontracts.RowCommand


        ''If e.CommandName = "Contract" Then

        'Dim strpop As String = ""
        ''    Dim lbComplete As LinkButton = gv_pendingcontracts.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lnkcontoffID")
        ''    Dim lblpartycode As Label = gv_pendingcontracts.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblPartycode")
        ''    Dim lbloffername As Label = gv_pendingcontracts.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblOfferName")

        ''    If lbloffername.Text.ToLower = "contract" Then
        ''        Session.Add("ContractState", "PendingView")
        ''        strpop = "window.open('PriceListModule/ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + lblpartycode.Text + "&State=View&contractid=" + CType(lbComplete.Text.Trim, String) + "','ContractMain');"

        'strpop = "window.open('PriceListModule/ContractMain.aspx?Calledfrom=Contracts&appid=1&partycode=AN06&State=View&contractid=CON/000017','ContractMain');window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        ''Else
        ' ''Session.Add("OfferState", "PendingView")
        ' ''strpop = "window.open('PriceListModule/OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lbComplete.Text & "&partycode=" + lblpartycode.Text + "&State=View&offerid=" + CType(lbComplete.Text.Trim, String) + "','OfferMain');"
        ''    End If

        ''End If
    End Sub










    Protected Sub gv_pendingcontracts_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_pendingcontracts.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim lbComplete As LinkButton = CType(e.Row.FindControl("lnkcontoffID"), LinkButton)
                Dim lblpartycode As Label = CType(e.Row.FindControl("lblPartycode"), Label)
                Dim lbloffername As Label = CType(e.Row.FindControl("lblOfferName"), Label)
                ' Dim lnkapplicableto As LinkButton = CType(e.Row.FindControl("lnkapp"), LinkButton)

                Dim strpop As String = ""
                If lbloffername.Text.ToLower = "contract" Then
                    Session.Add("ContractState", "PendingView")

                    Session("Calledfrom") = "Contracts"

                    strpop = "window.open('PriceListModule/ContractMain.aspx?Calledfrom=Contracts&appid=" + CType(Request.QueryString("appid"), String) + "&partycode=" + lblpartycode.Text + "&contractid=" + CType(lbComplete.Text.Trim, String) + "','ContractMain');"




                Else
                    Session.Add("OfferState", "PendingView")
                    strpop = "window.open('PriceListModule/OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lbComplete.Text & "&partycode=" + lblpartycode.Text + "&offerid=" + CType(lbComplete.Text.Trim, String) + "','OfferMain');"
                    ' strpop = "window.open('PriceListModule/OfferMain.aspx?Calledfrom=Offers&appid=" + CType(Request.QueryString("appid"), String) + "&promotionid=" & lbComplete.Text & "&partycode=" + lblpartycode.Text + "&State=View','OfferMain');"
                    ' 
                End If

                lbComplete.Attributes.Add("onclick", strpop)






                ' lnkapplicableto.Attributes.Add("onclick", "javascript:displaycountries_agents ('" + lbComplete.Text + "');")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub





End Class
