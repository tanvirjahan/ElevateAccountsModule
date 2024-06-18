Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Linq

Partial Class PriceListModule_Countrygroup
    Inherits System.Web.UI.UserControl
    Dim myCommand As SqlCommand
    Dim SqlConn As SqlConnection
    Dim msgError As String
    Dim ctrygrpFlag As Boolean
    Dim objUtils As New clsUtils


    'Dim _gsPageMode As String = ""
    'Dim _gsPageName As String = ""
    'Dim _gsTranId As String = ""

    'Public Property gsTranId As String
    '    Get
    '        Return _gsTranId
    '    End Get
    '    Set(ByVal value As String)
    '        _gsTranId = value
    '    End Set
    'End Property

    'Public Property gsPageName As String
    '    Get
    '        Return _gsPageName
    '    End Get
    '    Set(ByVal value As String)
    '        _gsPageName = value
    '    End Set
    'End Property

    'Public Property gsPageMode As String
    '    Get
    '        Return _gsPageMode
    '    End Get
    '    Set(ByVal value As String)
    '        _gsPageMode = value
    '    End Set
    'End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                'BindGrid()
                '  gv_ShowCountries.Visible = False
                ' gv_showagents.Visible = False

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

                Dim dtcountry As New DataTable
                dtcountry.Columns.Add(New DataColumn("countrygroupcode", GetType(String)))
                dtcountry.Columns.Add(New DataColumn("countrycode", GetType(String)))
                dtcountry.Columns.Add(New DataColumn("selected", GetType(String)))
                Session("CountrySelected_Sess") = dtcountry


                Dim dtagents As New DataTable
                dtagents.Columns.Add(New DataColumn("agentcode", GetType(String)))
                dtagents.Columns.Add(New DataColumn("ctrycode", GetType(String)))
                dtagents.Columns.Add(New DataColumn("selected", GetType(String)))
                Session("AgentSelected_Sess") = dtagents

                If hdnPageMode.Value.Trim.ToUpper <> "NEW" Then 'If CType(Session("ContractState"), String) <> "New" Then 'changed by mohamed on 05/11/2016
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                End If

                sbShowCountry()

                'Btnshowagents_Click(Nothing, Nothing) 'commented / changed by mohamed on 31/10/2016
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Public Sub getShowCountriesgroups()

        '    Try
        '        Dim strSqlQry As String
        '        Dim MyDs As New DataTable
        '        Dim myDataAdapter As SqlDataAdapter
        '        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        '        strSqlQry = "select countrygroupcode,countrygroupname from  countrygroup where  active=1 "

        '        myCommand = New SqlCommand(strSqlQry, SqlConn) 
        '        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '        myDataAdapter.Fill(MyDs)

        '        If MyDs.Rows.Count > 0 Then
        '            gv_ShowCountriesgrp.DataSource = MyDs
        '            gv_ShowCountriesgrp.DataBind()

        '            gv_ShowCountriesgrp.Visible = True

        '        Else

        '            gv_ShowCountriesgrp.Visible = False

        '        End If

        '    Catch ex As Exception
        '        If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
        '        msgError = "In Bind Country Group Grid WUC " & ex.ToString()

        '    End Try

    End Sub


    Public Sub getShowCountries()

        'Try
        '    Dim strSqlQry As String
        '    Dim MyDs As New DataTable
        '    Dim myDataAdapter As SqlDataAdapter
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        '    strSqlQry = "select b.ctrycode,ctryname from countrygroup_detail a,ctrymast b where  a.ctrycode=b.ctrycode and b.active=1 order by b.ctryname  "

        '    myCommand = New SqlCommand(strSqlQry, SqlConn)
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(MyDs)

        '    If MyDs.Rows.Count > 0 Then
        '        gv_ShowCountries.DataSource = MyDs
        '        gv_ShowCountries.DataBind()

        '        gv_ShowCountries.Visible = True

        '    Else

        '        gv_ShowCountries.Visible = False

        '    End If

        'Catch ex As Exception
        '    If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
        '    msgError = "In Bind Country Group Grid WUC " & ex.ToString()
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

    End Sub
    Public Function checkcountrylist() As String
        Dim chkSelect As CheckBox
        Dim txtctrycode As Label

        Dim countrycodestr As String = ""

        For Each gvRow In gv_ShowCountries.Rows
            chkSelect = gvRow.FindControl("chkCtry2")
            txtctrycode = gvRow.FindControl("txtctrycode")



            If chkSelect.Checked = True Then
                countrycodestr = countrycodestr + "," + txtctrycode.Text
            End If


        Next
        If countrycodestr = "" Then
            Return ""
        Else
            Return Right(countrycodestr, Len(countrycodestr) - 1)
        End If

    End Function

    Public Function checkCustomerGroupFitlerList() As String
        Dim lsCustomerGroupList As String = ""
        Dim dlList As DataList = Me.Parent.FindControl("dllist")  'Session("dllist_countryuserctrl")
        If dlList IsNot Nothing Then
            For Each lListItem As DataListItem In dlList.Items
                Dim lnkCode As Button = lListItem.FindControl("lnkCode")
                Dim lnkValue As Button = lListItem.FindControl("lnkValue")
                If lnkCode IsNot Nothing And lnkValue IsNot Nothing Then
                    If lnkCode.Text = "CUSTOMERGROUP" Then
                        lsCustomerGroupList += IIf(lsCustomerGroupList = "", "", ",") & lnkCode.Text.Trim & ":" & lnkValue.Text.Trim
                    End If
                End If
            Next
        End If
        Return lsCustomerGroupList
    End Function

    Public Sub Disable(ByVal isflag As Boolean)

        gv_showagents.Enabled = isflag
        gv_ShowCountries.Enabled = isflag


    End Sub
    Public Function checkagentlist() As String
        Dim chkSelect As CheckBox
        Dim txtagentcode As Label

        Dim agentcodestr As String = ""

        For Each gvRow In gv_showagents.Rows
            chkSelect = gvRow.FindControl("chk2")
            txtagentcode = gvRow.FindControl("txtagentcode")



            If chkSelect.Checked = True Then
                agentcodestr = agentcodestr + "," + txtagentcode.Text
            End If


        Next
        If agentcodestr = "" Then
            Return ""
        Else
            Return Right(agentcodestr, Len(agentcodestr) - 1)
        End If

    End Function

    Public Sub FillGridCountry(ByVal lsQryStr As String)

        Dim isAutoTick As Boolean
        If Session("isAutoTick_wuccountrygroupusercontrol") IsNot Nothing Then
            isAutoTick = IIf(Session("isAutoTick_wuccountrygroupusercontrol") = 1, True, False)
        End If


        Dim myDS As New DataSet
        Dim myDataAdapter As SqlDataAdapter
        Dim strSqlQry As String = ""
        gv_ShowCountries.Visible = True
        If gv_ShowCountries.PageIndex < 0 Then
            gv_ShowCountries.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            Session("countrygriddatasource_usercontrol") = Nothing
            'If market <> "" Then
            '    strSqlQry = "select b.ctrycode,ctryname,a.countrygroupcode from countrygroup_detail a,ctrymast b where  a.ctrycode=b.ctrycode and b.active=1 and  a.countrygroupcode in (" + market + ") order by b.ctryname "
            'Else
            '    strSqlQry = "select b.ctrycode,ctryname,a.countrygroupcode from countrygroup_detail a,ctrymast b where  a.ctrycode=b.ctrycode and b.active=1 order by b.ctryname  "
            'End If

            strSqlQry = lsQryStr

            Dim dtCountrys As DataTable
            Dim dvCountrys As New DataView
            dtCountrys = Session("CountrySelected_Sess")
            If Session("CountrySelected_Sess") IsNot Nothing Then
                dvCountrys = New DataView(dtCountrys)
            End If

            '  strSqlQry = strSqlQry & " ORDER BY ctryname"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(myDS)

            'Dim lDataViewUnSelCnt As DataView
            'lDataViewUnSelCnt = New DataView(myDS.Tables(0))
            'lDataViewUnSelCnt.RowFilter = "chkselect=0"
            'If lDataViewUnSelCnt.ToTable.Rows.Count > 0 Then
            'myDS.Tables(0).Rows.Add({"", "", "", "", 1, "", "", 0}) 'commented / changed by mohamed on 30/10/2016
            'End If
            'lDataViewUnSelCnt.RowFilter = ""

            Dim lDataView As DataView
            lDataView = New DataView(myDS.Tables(0))
            lDataView.Sort = "chkselect desc, ordertype, ctryname, plgrpname, countrygroupname "

            Dim lclsCtry As List(Of MyClasses.CountryModel) = DirectCast((From cust In lDataView.ToTable
                Select New MyClasses.CountryModel With {.chkselect = cust("chkselect"),
                    .ctrycode = cust("ctrycode"),
                    .ctryname = cust("ctryname"),
                    .plgrpcode = cust("plgrpcode"),
                    .plgrpname = cust("plgrpname"),
                    .countrygroupcode = cust("countrygroupcode"),
                    .countrygroupname = cust("countrygroupname"),
                    .ordertype = cust("ordertype")
                }).ToList, List(Of MyClasses.CountryModel))

            'changed by mohamed on 20/12/2016
            If isAutoTick = True Then
                If hdnPageMode.Value.Trim.ToUpper <> "NEW" Then
                    If hdnPageName.Value.Trim.ToUpper <> "" Then 'else 'changed by mohamed on 05/11/2016
                        If hdnTranId.Value.Trim <> "" Then
                            Dim dtRegion As DataTable, dvRegion As DataView
                            dvRegion = New DataView(myDS.Tables(0))
                            dtRegion = dvRegion.ToTable(True, {"plgrpname", "plgrpcode"})
                            For Each dvRowRegion As DataRow In dtRegion.Rows
                                sbAddToDataTable("REGION", dvRowRegion("plgrpname"), "R")
                            Next

                            'changed by mohamed on 14/08/2018
                            Dim lDsFilterList As DataSet
                            lDsFilterList = objUtils.ExecuteQuerySqlnew(Session("DbConnectionName"), "select * from dbo.fn_get_country_or_agent_code_fromtable ('" & hdnPageName.Value.Trim & "','" & hdnTranId.Value.Trim & "','CUSTOMERGROUPFILTER') cc")
                            If lDsFilterList.Tables.Count > 0 Then
                                If lDsFilterList.Tables(0).Rows.Count > 0 Then
                                    For Each dvRowRegion As DataRow In lDsFilterList.Tables(0).Rows
                                        sbAddToDataTable(dvRowRegion("ctrycode"), dvRowRegion("agentcode"), "AG")
                                    Next
                                End If
                            End If

                            Dim dlList As DataList = Me.Parent.FindControl("dllist")  'Session("dllist_countryuserctrl")
                            If dlList IsNot Nothing Then
                                Dim dtDynamics As New DataTable
                                dtDynamics = Session("sDtDynamic")
                                dlList.DataSource = dtDynamics
                                dlList.DataBind()
                            End If
                        End If
                    End If
                End If
                
            End If

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    myDS.Tables(0).Columns.Add(New DataColumn("sortorder", GetType(String)))

            '    For j As Integer = 0 To myDS.Tables(0).Rows.Count - 1

            '        'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            '        dvCountrys.RowFilter = "trim(countrycode)='" & myDS.Tables(0).Rows(j)("ctrycode").ToString.Trim & "'"
            '        If dvCountrys.ToTable.Rows.Count > 0 Then
            '            myDS.Tables(0).Rows(j)("sortorder") = "1"
            '        Else
            '            myDS.Tables(0).Rows(j)("sortorder") = "0"
            '        End If

            '        ''   myDS.Tables(0).Rows(j)("sortorder") = "0"
            '        'For k As Integer = 0 To dvCountrys.ToTable.Rows.Count - 1
            '        '    If dvCountrys.ToTable.Rows(k)("countrycode").ToString.Trim = myDS.Tables(0).Rows(j)("ctrycode").ToString.Trim Then
            '        '        myDS.Tables(0).Rows(j)("sortorder") = "1"
            '        '        Exit For
            '        '    Else
            '        '        myDS.Tables(0).Rows(j)("sortorder") = "0"
            '        '    End If
            '        'Next

            '    Next
            '    dvCountrys.RowFilter = "" 'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            'End If



            'Dim dataView As DataView = New DataView(myDS.Tables(0))
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    'dataView.Sort = "sortorder desc"
            'End If

            ' ''changed by mohamed on 05/10/2016 as to show only ticked countries on edit
            'If isAutoTick = True Then
            '    'dataView.RowFilter = "chkselect=1"
            '    For Each dtRow As DataRow In dataView.ToTable.Rows
            '        sbAddToDataTable("COUNTRY", dtRow.Item("ctryname").ToString, "C")
            '    Next

            '    Dim dlList As DataList = Me.Parent.FindControl("dllist")  'Session("dllist_countryuserctrl")
            '    If dlList IsNot Nothing Then
            '        Dim dtDynamics As New DataTable
            '        dtDynamics = Session("sDtDynamic")
            '        dlList.DataSource = dtDynamics
            '        dlList.DataBind()
            '    End If
            '    'Else
            '    '    dataView.RowFilter = ""
            'End If
            'dataView.RowFilter = ""

            'gv_ShowCountries.DataSource = dataView.ToTable 'dataView 'changed by mohamed on 05/10/2016

            ' gv_ShowCountries.DataSource = myDS

            ViewState("ctrydatatable_ctryusercontrol") = lclsCtry
            gv_ShowCountries.DataSource = lclsCtry

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_ShowCountries.DataBind()
            Else
                gv_ShowCountries.DataBind()

            End If
            Session("countrygriddatasource_usercontrol") = myDS

            GrdCountryGrouping_OnDataBound()

            Dim dtCountry As DataTable
            Dim dvCountry As New DataView
            dtCountry = Session("CountrySelected_Sess")
            If Session("CountrySelected_Sess") IsNot Nothing Then
                dvCountry = New DataView(dtCountry)
            End If

            Dim chkSelect As CheckBox
            Dim txtctrycode As Label
            Dim lblCountryGroupCode As Label
            'If ViewState("PromotionHeaderState") = "New" Then


            Dim lsAvailableCtryCode As String = ""

            'If lsQryStr <> "" Then 'changed by mohamed on 03/10/2016
            For Each gvRow In gv_ShowCountries.Rows
                chkSelect = gvRow.FindControl("chkCtry2")
                txtctrycode = gvRow.FindControl("txtctrycode")
                lblCountryGroupCode = gvRow.FindControl("lblCountryGroupCode")

                'If lsQryStr <> "" Then 'changed by mohamed on 03/10/2016
                '    'chkSelect.Checked = True
                '    If Session("CountrySelected_Sess") IsNot Nothing Then
                '        dvCountry.RowFilter = "countrycode='" & txtctrycode.Text & "'"
                '        If dvCountry.ToTable.Rows.Count > 0 Then
                '            For i As Integer = 0 To dvCountry.ToTable.Rows.Count - 1
                '                If dvCountry.ToTable.Rows(i)("countrycode").ToString.Trim = txtctrycode.Text.Trim Then ' And dvCountry.ToTable.Rows(i)("countrygroupcode").ToString.Trim = lblCountryGroupCode.Text.Trim Then 'changed by mohamed on 04/10/2016 as country group is not required to be checked, as they are shown in one line 
                '                    chkSelect.Checked = True
                '                End If
                '            Next

                '        End If
                '    End If
                'End If

                'changed by mohamed on 03/10/2016
                lsAvailableCtryCode = lsAvailableCtryCode & IIf(lsAvailableCtryCode.Trim = "", "", ",") & "'" & txtctrycode.Text.Trim & "'"
                Session("AllCountriesList_WucCountryGroupUserControl") = lsAvailableCtryCode
            Next
            'End If 'changed by mohamed on 03/10/2016

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            msgError = "In Bind Country  Grid WUC " & ex.ToString()
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)


    End Sub
    Public Sub FillGridAgent(ByVal market As String)
        Dim isAutoTick As Boolean
        If Session("isAutoTick_wuccountrygroupusercontrol") IsNot Nothing Then
            isAutoTick = IIf(Session("isAutoTick_wuccountrygroupusercontrol") = 1, True, False)
        End If

        Dim myDS As New DataSet
        Dim myDataAdapter As SqlDataAdapter
        Dim strSqlQry As String = ""
        gv_showagents.Visible = True
        If gv_showagents.PageIndex < 0 Then
            gv_showagents.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            Dim lsDeselectAgentList As String = ""
            If Session("FindAgentInGrid_agentcode_notselected") IsNot Nothing Then
                lsDeselectAgentList = Session("FindAgentInGrid_agentcode_notselected")
            End If


            'Dim lsagentcontract As String = ""
            'If Session("contractid") IsNot Nothing Then
            '    lsagentcontract = Session("contractid")
            'End If
            'Dim lsMaxid As String = ""
            'If Session("Maxid") IsNot Nothing Then
            '    lsMaxid = Session("Maxid")
            'End If


            Dim lsSqlQryShowEdit As String = ","
            Dim lsSqlQryShowEditField As String = " null "

            If isAutoTick = True Then
                'If hdnPageName.Value.Trim.ToUpper = "MAXOCCUPANCY" Then 'If Session("Maxid") IsNot Nothing Then 'changed by mohamed on 05/11/2016
                '    ' If lsMaxid.Trim <> "" Then 'changed by mohamed on 12/10/2016
                '    If hdnTranId.Value.Trim <> "" Then  'changed by mohamed on 05/11/2016
                '        lsSqlQryShowEdit = " Left join  partymaxacc_agents cc on a.agentcode =cc.agentcode  and cc.tranid ='" & hdnTranId.Value.Trim & "' and ltrim(rtrim(cc.tranid))<>'' ,"
                '        lsSqlQryShowEditField = " cc.tranid"
                '    End If
                'ElseIf hdnPageName.Value.Trim.ToUpper = "CONTRACTMAIN" Then 'else 'changed by mohamed on 05/11/2016
                '    If hdnTranId.Value.Trim <> "" Then 'changed by mohamed on 12/10/2016

                '        'lsSqlQryShowEdit = " Left join view_contractagents cc on a.agentcode =cc.agentcode and cc.contractid ='" & lsagentcontract & "' and ltrim(rtrim(cc.contractid))<>''," 'changed by mohamed on 12/10/2016

                '        'this method has been revised on 26/10/2016 -- changed by mohamed on 26/10/2016
                '        'lsSqlQryShowEdit = " Left join (select cc1.contractid, cc1.ctrycode, cca.mktcode agentcode from contracts_agents cc1 "
                '        'lsSqlQryShowEdit += " cross apply dbo.splitallotmkt(cc1.agentcode,',') cca "
                '        'lsSqlQryShowEdit += " where cc1.contractid ='" & lsagentcontract & "') cc"
                '        'lsSqlQryShowEdit += " on a.ctrycode =cc.ctrycode and cc.contractid ='" & lsagentcontract & "' and cc.agentcode=a.agentcode "
                '        lsSqlQryShowEdit = " Left join view_contractagents cc "
                '        lsSqlQryShowEdit += " on cc.contractid ='" & hdnTranId.Value.Trim & "' and cc.agentcode=a.agentcode "
                '        lsSqlQryShowEdit += " and ltrim(rtrim(cc.contractid))<>'', "

                '        lsSqlQryShowEditField = " cc.contractid"
                '    End If
                'End If

                If hdnPageName.Value.Trim.ToUpper <> "" Then 'else 'changed by mohamed on 05/11/2016
                    If hdnTranId.Value.Trim <> "" Then 'changed by mohamed on 12/10/2016
                        lsSqlQryShowEdit = " Left join dbo.fn_get_country_or_agent_code_fromtable ('" & hdnPageName.Value.Trim & "','" & hdnTranId.Value.Trim & "','AGENT') cc "
                        lsSqlQryShowEdit += " on cc.tranid ='" & hdnTranId.Value.Trim & "' and cc.agentcode=a.agentcode "
                        lsSqlQryShowEdit += " and ltrim(rtrim(cc.tranid))<>'', "
                        lsSqlQryShowEditField = " cc.tranid"
                    End If
                End If
            End If


            Dim lsSelectedAgentList As String = ""
            If Session("SelectedAgentList_WucCountryGroupUserControl") IsNot Nothing Then
                If Session("SelectedAgentList_WucCountryGroupUserControl").ToString.Trim <> "" Then
                    lsSelectedAgentList = Replace(Session("SelectedAgentList_WucCountryGroupUserControl"), "'", "")
                End If
            End If

            'changed by mohamed on 20/10/2016
            If Session("FindAgentInGrid_agentcode") IsNot Nothing Then
                If Session("FindAgentInGrid_agentcode").ToString.Trim <> "" Then
                    lsSelectedAgentList = IIf(lsSelectedAgentList.Trim = "", "", lsSelectedAgentList & ",") & Session("FindAgentInGrid_agentcode").ToString.Trim
                End If
            End If

            If market <> "" Then

                strSqlQry = "select distinct a.agentcode,agentname, a.ctrycode + ' - ' + b.ctryname ctrycodename, b.ctryname, a.ctrycode, 2 ordertype,  "
                strSqlQry += " case when " & lsSqlQryShowEditField & "  is null then case when sel.mktcode is null then 0 else 1 end else 1 end chkselect "
                strSqlQry += " , isnull(c1.customergroupcode,'') customergroupcode, isnull(c1.customergroupname,'') customergroupname " 'changed by mohamed on 13/08/2018
                strSqlQry += " from agentmast a "
                strSqlQry += "  left join dbo.splitallotmkt('" & lsSelectedAgentList & "',',') sel on sel.mktcode=a.agentcode " '"SelectedAgentList_WucCountryGroupUserControl"
                strSqlQry += "  left join dbo.splitallotmkt('" & lsDeselectAgentList & "',',') desel on desel.mktcode=a.agentcode " 'changed by mohamed on 31/10/2016
                strSqlQry += "  left join view_customergroup c1 (nolock) on c1.agentcode=a.agentcode" 'changed by mohamed on 13/08/2018 '25/08/2018
                strSqlQry += lsSqlQryShowEdit
                strSqlQry += " ctrymast b where  a.ctrycode=b.ctrycode and a.active=1 and  b.active=1 and "

                strSqlQry += " (" & lsSqlQryShowEditField & " is not null or sel.mktcode is not null) and " 'commented / changed by mohamed on 30/10/2016
                strSqlQry += " desel.mktcode is null and " 'changed by mohamed on 31/10/2016
                strSqlQry += " a.ctrycode in (" + market + ") order by a.agentname "


                ' Rosalin On 2019-10-27
                'If (hdnPageName.Value.Trim = "OFFERSMAIN" And hdnTranId.Value.Trim <> "") Then
                '    strSqlQry = " New_OfferAgentGroupSelection '" & CType(hdnTranId.Value.Trim, String) & "'"
                'End If


                'strSqlQry = "select a.agentcode,agentname,a.ctrycode, case when cc.contractid is null then 0 else 1 end chkselect from agentmast a Left join  contracts_agents cc on a.agentcode =cc.agentcode  and " _
                '    & " cc.contractid ='" & lsagentcontract & "' ,ctrymast b where  a.ctrycode=b.ctrycode and a.active=1 and  b.active=1 and " _
                '    & " a.ctrycode in (" + market + ") order by a.agentname "
            Else
                'strSqlQry = "select agentcode,agentname, a.ctrycode + ' - ' + b.ctryname ctrycodename, b.ctryname, a.ctrycode, 2 ordertype, 0 chkselect from agentmast a, ctrymast b where a.ctrycode=b.ctrycode and a.active=1 order by a.agentname  "
                strSqlQry = "select agentcode,agentname, a.ctrycode + ' - ' + b.ctryname ctrycodename, b.ctryname, a.ctrycode, 2 ordertype, 0 chkselect, isnull(c.customergroupcode,'') customergroupcode, isnull(c.customergroupname,'') customergroupname from ctrymast b, agentmast a left join view_customergroup c (nolock) on c.agentcode=a.agentcode where a.ctrycode=b.ctrycode and a.active=1 order by a.agentname  "
            End If

            Dim dtAgents As DataTable
            Dim dvAgents As New DataView
            dtAgents = Session("AgentSelected_Sess")
            If Session("AgentSelected_Sess") IsNot Nothing Then
                dvAgents = New DataView(dtAgents)
            End If

         

            '  strSqlQry = strSqlQry & " ORDER BY ctryname"

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(myDS)

            ' Rosalin On 2019-10-27

            Dim dtAgent As DataTable = New DataTable()
            dtAgent = myDS.Tables(0)
          
            'If market <> "" Then
            '    If myDS.Tables(0).Rows.Count > 0 Then
            '        ' filter market based
            '        dtAgent = myDS.Tables(0).Select("ctrycode in ( " & market & ")").CopyToDataTable()
            '    End If
            'End If

            ' end Rosalin


            'If myDS.Tables(0).Rows.Count > 0 Then
            '    myDS.Tables(0).Columns.Add(New DataColumn("sortorder", GetType(String)))

            '    '    For j As Integer = 0 To myDS.Tables(0).Rows.Count - 1
            '    '        'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            '    '        dvAgents.RowFilter = "trim(agentcode)='" & myDS.Tables(0).Rows(j)("agentcode").ToString.Trim & "'"
            '    '        If dvAgents.ToTable.Rows.Count > 0 Then
            '    '            myDS.Tables(0).Rows(j)("sortorder") = "1"
            '    '            myDS.Tables(0).Rows(j)("chkselect") = 1
            '    '        Else
            '    '            myDS.Tables(0).Rows(j)("sortorder") = "0"
            '    '        End If
            '    '        ''   myDS.Tables(0).Rows(j)("sortorder") = "0"
            '    '        'For k As Integer = 0 To dvAgents.ToTable.Rows.Count - 1
            '    '        '    If dvAgents.ToTable.Rows(k)("agentcode").ToString.Trim = myDS.Tables(0).Rows(j)("agentcode").ToString.Trim Then
            '    '        '        myDS.Tables(0).Rows(j)("sortorder") = "1"
            '    '        '        Exit For
            '    '        '    Else
            '    '        '        myDS.Tables(0).Rows(j)("sortorder") = "0"
            '    '        '    End If
            '    '        'Next
            '    '    Next
            '    '    dvAgents.RowFilter = "" 'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            'End If

            'Dim lsAgntCode As String ', lRIndex As Integer
            'For Each lrow As DataRow In dtAgents.Rows
            '    lsAgntCode = lrow("agentcode")
            '    Dim lmatches = From row In myDS.Tables(0)
            '      Let lAgentCode = row.Field(Of String)("AgentCode")
            '      Where lAgentCode = lsAgntCode
            '    Select row

            '    For Each itm In lmatches
            '        itm("chkselect") = 1
            '        'lRIndex = myDS.Tables(0).Rows.IndexOf(itm)
            '        'myDS.Tables(0).Rows(lRIndex)("chkSelect") = 1
            '    Next
            'Next

            'Dim lmatches = From row In myDS.Tables(0)
            'Select row
            'For Each itm In lmatches
            '    dvAgents.RowFilter = "trim(agentcode)='" & itm("agentcode") & "'"
            '    If dvAgents.ToTable.Rows.Count > 0 Then
            '        itm("chkselect") = 1
            '        itm("sortorder") = "1"
            '    Else
            '        itm("sortorder") = "0"
            '    End If
            'Next

            'Dim lDataViewUnSelCnt As DataView
            'lDataViewUnSelCnt = New DataView(myDS.Tables(0))
            'lDataViewUnSelCnt.RowFilter = "chkselect=0"
            'If lDataViewUnSelCnt.ToTable.Rows.Count > 0 Then
            'myDS.Tables(0).Rows.Add({"", "", "", "", "", 1, 0}) 'commented / changed by mohamed on 30/10/2016
            'End If
            'lDataViewUnSelCnt.RowFilter = ""



            ' Dim dataView As DataView = New DataView(myDS.Tables(0))
            'Rosalin
            Dim dataView As DataView = New DataView(dtAgent)
            If myDS.Tables(0).Rows.Count > 0 Then
                'dataView.Sort = "sortorder desc" '-commented by mohamed on 04/10/2016
                'dataView.Sort = "ctryname, sortorder desc,agentname"
                dataView.Sort = "chkselect desc, ordertype, ctryname, agentname"
            End If

            Dim lclsagent As List(Of MyClasses.CountryAgentModel) = DirectCast(((From cust In dataView
                Select New MyClasses.CountryAgentModel With {.chkselect = cust("chkselect"),
                    .ctrycode = cust("ctrycode"),
                    .ctryname = cust("ctryname"),
                    .agentcode = cust("agentcode"),
                    .agentname = cust("agentname"),
                    .ctrycodename = cust("ctrycodename"),
                    .ordertype = cust("ordertype"),
                    .customergroupcode = cust("customergroupcode"),
                    .customergroupname = cust("customergroupname")
                 })).ToList, List(Of MyClasses.CountryAgentModel))

            'changed by mohamed on 14/08/2018

            ViewState("agentdatatable_ctryusercontrol") = lclsagent
            gv_showagents.DataSource = lclsagent


            'gv_showagents.DataSource = dataView


            '  gv_showagents.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_showagents.DataBind()
            Else
                gv_showagents.DataBind()

            End If


            'Dim dtAgent As DataTable
            'Dim dvAgent As New DataView
            'dtAgent = Session("AgentSelected_Sess")
            'If Session("AgentSelected_Sess") IsNot Nothing Then
            '    dvAgent = New DataView(dtAgent)
            'End If

            'Dim chkSelect As CheckBox
            'Dim txtagentcode As Label

            'If market <> "" Then
            '    For Each gvRow In gv_showagents.Rows
            '        chkSelect = gvRow.FindControl("chk2")
            '        txtagentcode = gvRow.FindControl("txtagentcode")
            '        ' chkSelect.Checked = True
            '        If Session("AgentSelected_Sess") IsNot Nothing Then
            '            '  dvAgent.RowFilter = "agentcode='" & txtagentcode.Text & "'"

            '            'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            '            dvAgent.RowFilter = "trim(agentcode)='" & txtagentcode.Text.Trim & "'"
            '            If dvAgent.ToTable.Rows.Count > 0 Then
            '                chkSelect.Checked = True
            '            End If

            '            'If dvAgent.ToTable.Rows.Count > 0 Then
            '            '    For i As Integer = 0 To dvAgent.ToTable.Rows.Count - 1
            '            '        If dvAgent.ToTable.Rows(i)("agentcode").ToString.Trim = txtagentcode.Text.Trim Then
            '            '            chkSelect.Checked = True
            '            '        End If
            '            '    Next

            '            'End If
            '        End If
            '    Next
            '    dvAgent.RowFilter = "" 'Instead of ineer loop, filter is used - changed by mohamed on 04/10/2016
            'End If

            GrdAgentGrouping_OnDataBound() 'changed by mohamed on 04/10/2016

            'Dim chkSelect As CheckBox

            'If market <> "" Then
            '    For Each gvRow In gv_showagents.Rows
            '        chkSelect = gvRow.FindControl("chk2")
            '        chkSelect.Checked = True
            '    Next
            'End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            msgError = "In Bind Agent  Grid WUC " & ex.ToString()
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Sub
    Public Sub getShowagents()

        Try
            Dim strSqlQry As String
            Dim MyDs As New DataTable
            Dim myDataAdapter As SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select a.agentcode,a.agentname, isnull(c.customergroupcode,'') customergroupcode, isnull(c.customergroupname,'') customergroupname from agentmast a (nolock) left join view_customergroup c (nolock) on c.agentcode=a.agentcode where a.active=1 order by a.agentname  "

            myCommand = New SqlCommand(strSqlQry, SqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(MyDs)

            If MyDs.Rows.Count > 0 Then
                gv_showagents.DataSource = MyDs
                gv_showagents.DataBind()

                gv_showagents.Visible = True

            Else

                gv_showagents.Visible = False

            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            msgError = "In Bind Country Group Grid WUC " & ex.ToString()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    'Public Sub Btnshowctry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnshowctry.Click
    '    sbShowCountry()
    'End Sub
    Sub clearsessions()
        'Changed by mohamed on 15/08/2018
        Session("SelectedCountriesList_WucCountryGroupUserControl") = Nothing
        ViewState("agentdatatable_ctryusercontrol") = Nothing

        '   Session("Maxid") = Nothing
        Session("SelectedCountriesList_WucCountryGroupUserControl") = Nothing
        Session("SelectedAgentList_WucCountryGroupUserControl") = Nothing
        Session("AllCountriesList_WucCountryGroupUserControl") = Nothing
        Session("skipSelectAllCountryChangeEvent") = Nothing
        Session("countrygriddatasource_usercontrol") = Nothing
        Session("GridViewCountryRow") = Nothing


        ' '' Clear the session  20/03/18

        Session("FindAgentInGrid_ctrycode") = Nothing
        Session("FindAgentInGrid_agentcode") = Nothing
        Session("FindAgentInGrid_agentcode_notselected") = Nothing

        'Session("contractid") = Nothing
        'Session("Maxid") = Nothing

        Dim dt1 As DataTable
        dt1 = New DataTable
        If Session("CountrySelected_Sess") IsNot Nothing Then
            dt1 = Session("CountrySelected_Sess")
            dt1.Clear()
        Else
            dt1.Columns.Add(New DataColumn("countrygroupcode", GetType(String)))
            dt1.Columns.Add(New DataColumn("countrycode", GetType(String)))
            dt1.Columns.Add(New DataColumn("selected", GetType(String)))
        End If
        Session("CountrySelected_Sess") = dt1

        dt1 = New DataTable
        If Session("AgentSelected_Sess") IsNot Nothing Then
            dt1 = Session("AgentSelected_Sess")
            dt1.Clear()
        Else
            dt1.Columns.Add(New DataColumn("agentcode", GetType(String)))
            dt1.Columns.Add(New DataColumn("ctrycode", GetType(String)))
            dt1.Columns.Add(New DataColumn("selected", GetType(String)))
        End If
        Session("AgentSelected_Sess") = dt1

        dt1 = New DataTable
        If Session("sDtDynamic") IsNot Nothing Then
            dt1 = Session("sDtDynamic")
            dt1.Clear()
        Else
            Dim dcCode = New DataColumn("Code", GetType(String))
            Dim dcValue = New DataColumn("Value", GetType(String))
            Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
            dt1.Columns.Add(dcCode)
            dt1.Columns.Add(dcValue)
            dt1.Columns.Add(dcCodeAndValue)
        End If
        Session("sDtDynamic") = dt1

        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        gv_ShowCountries.DataSource = Nothing
        gv_showagents.DataSource = Nothing
        gv_ShowCountries.DataBind()
        gv_showagents.DataBind()

        '10/12/2016

        Dim dlList As DataList = Me.Parent.FindControl("dlList")  'Session("dllist_countryuserctrl")
        If dlList IsNot Nothing Then
            dlList.DataSource = Nothing
            dlList.DataBind()
        End If

    End Sub
    Sub sbShowCountry()

        Dim isAutoTick As Boolean
        If Session("isAutoTick_wuccountrygroupusercontrol") IsNot Nothing Then
            isAutoTick = IIf(Session("isAutoTick_wuccountrygroupusercontrol") = 1, True, False)
        End If

        Dim strMarket As String = ""
        Dim plgrpcode As String = ""
        Dim chkSel As CheckBox
        Dim lblctrygrp As Label

        Dim lsCountriesSelectedList As String = ""
        Dim lsCountriesNotSelectedList As String = "" 'Changed by mohamed on 20/12/2016

        ctrygrpFlag = False


        Dim dr As DataRow
        Dim dtcountry As New DataTable
        If Session("CountrySelected_Sess") Is Nothing Then
            dtcountry.Columns.Add(New DataColumn("countrygroupcode", GetType(String)))
            dtcountry.Columns.Add(New DataColumn("countrycode", GetType(String)))
            dtcountry.Columns.Add(New DataColumn("selected", GetType(String)))
            Session("CountrySelected_Sess") = dtcountry
        End If
        dtcountry = Session("CountrySelected_Sess")
        dtcountry.Clear()

        Dim chkSelect As CheckBox
        Dim txtctrycode As Label
        Dim lblctrygroupcode As Label
        Dim iFlag As Integer = 0
        If gv_ShowCountries.Rows.Count > 0 Then
            iFlag = 0
            For Each gvRow In gv_ShowCountries.Rows
                chkSelect = gvRow.FindControl("chkCtry2")
                If chkSelect.Checked = True Then



                    txtctrycode = gvRow.FindControl("txtctrycode")
                    lblctrygroupcode = gvRow.FindControl("lblCountryGroupCode")

                    lsCountriesSelectedList = lsCountriesSelectedList & IIf(lsCountriesSelectedList.Trim = "", "", ",") & "'" & txtctrycode.Text & "'"

                    If txtctrycode.Text <> "" Then
                        'instead of inner for loop, used filter - 'changed by mohamed on 05/10/2016
                        Dim dvCountry As DataView
                        dvCountry = New DataView(dtcountry)
                        dvCountry.RowFilter = "countrycode='" & txtctrycode.Text & "'"
                        'For i = 0 To dtcountry.Rows.Count - 1
                        '    If dtcountry.Rows(i)("countrycode").ToString = txtctrycode.Text Then ' And lblctrygroupcode.Text = dtcountry.Rows(i)("countrygroupcode").ToString Then 'changed by mohamed on 04/10/2016 as country group is not required to be checked, as they are shown in one line 
                        '        iFlag = 1
                        '        Exit For 'changed by mohamed on 05/10/2016
                        '    End If
                        'Next
                        'If iFlag = 0 Then
                        If dvCountry.ToTable.Rows.Count = 0 Then
                            dr = dtcountry.NewRow()
                            dr("countrycode") = txtctrycode.Text
                            dr("countrygroupcode") = lblctrygroupcode.Text
                            dr("selected") = "Y"
                            dtcountry.Rows.Add(dr)
                        End If
                    End If
                End If
            Next
        End If
        Session("CountrySelected_Sess") = dtcountry

        'changed by mohamed on 20/10/2016
        If Session("FindAgentInGrid_ctrycode") IsNot Nothing Then
            If Session("FindAgentInGrid_ctrycode").ToString.Trim <> "" Then
                lsCountriesSelectedList = IIf(lsCountriesSelectedList.Trim = "", "", lsCountriesSelectedList & ",") & "'" & Replace(Session("FindAgentInGrid_ctrycode").ToString.Trim, ",", "','") & "'"
            End If
        End If

        'Changed by mohamed on 20/12/2016
        If Session("FindAgentInGrid_ctrycode_notselected") IsNot Nothing Then
            If Session("FindAgentInGrid_ctrycode_notselected").ToString.Trim <> "" Then
                lsCountriesNotSelectedList = "'" & Replace(Session("FindAgentInGrid_ctrycode_notselected").ToString.Trim, ",", "','") & "'"
            End If
        End If

        Dim chkSelAgent As CheckBox
        Dim strMarketAg As String = ""
        Dim plgrpcodeAg As String = ""
        Dim txtAgentCodes As Label
        Dim lblCountryCodes As Label
        Dim dragent As DataRow
        Dim dtagents As New DataTable
        If Session("AgentSelected_Sess") Is Nothing Then
            dtagents.Columns.Add(New DataColumn("agentcode", GetType(String)))
            dtagents.Columns.Add(New DataColumn("ctrycode", GetType(String)))
            dtagents.Columns.Add(New DataColumn("selected", GetType(String)))
            Session("AgentSelected_Sess") = dtagents
        End If
        dtagents = Session("AgentSelected_Sess")
        dtagents.Clear()

        If gv_showagents.Rows.Count > 0 Then
            For Each gvRow In gv_showagents.Rows
                chkSelAgent = gvRow.FindControl("chk2")
                txtAgentCodes = gvRow.FindControl("txtagentcode")
                lblCountryCodes = gvRow.FindControl("lblCountryCode")
                If chkSelAgent.Checked = True Then
                    plgrpcodeAg = txtAgentCodes.Text  'gvRow.Cells(1).Text.Trim
                    If plgrpcodeAg <> "" Then
                        dragent = dtagents.NewRow()
                        dragent("agentcode") = plgrpcodeAg
                        dragent("ctrycode") = lblCountryCodes.Text
                        dragent("selected") = "Y"
                        strMarketAg = strMarketAg + "'" + plgrpcodeAg + "',"
                        dtagents.Rows.Add(dragent)
                    End If
                End If
            Next
        End If
        Session("AgentSelected_Sess") = dtagents




        Session("skipSelectAllCountryChangeEvent") = 1

        Dim lsSqlQry As String = ""

        lsSqlQry = ";with cte as ("
        lsSqlQry += " select distinct c.ctrycode, c.ctryname, p.plgrpname, g.countrygroupname, g.countrygroupcode, p.plgrpcode "
        lsSqlQry += "	from ctrymast c (nolock) "
        lsSqlQry += "	join plgrpmast p (nolock) on p.plgrpcode=c.plgrpcode"
        lsSqlQry += "	left join countrygroup_detail gd (nolock) on gd.ctrycode=c.ctrycode"
        lsSqlQry += "	left join countrygroup g (nolock) on gd.countrygroupcode=g.countrygroupcode"
        lsSqlQry += " where 1=1"



        Dim lsSqlQryText As String = "", lsSqlQryCountry As String = "", lsSqlQryRegion As String = "", lsSqlQryCountryGroup As String = ""
        Dim lb_Is_Validate_VS_selected As Boolean = False
        Dim lsSqlQryAll As String = ""

        Dim dtDynamics As New DataTable
        dtDynamics = Session("sDtDynamic")
        If dtDynamics IsNot Nothing Then 'changed by mohamed on 26/04/2018 as to avoid error if it is nothing
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                Dim lsCode As String
                Dim lsValue As String
                If dtDynamics.Rows.Count > 0 Then
                    lb_Is_Validate_VS_selected = True
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCode = dtDynamics.Rows(j)("code").ToString.ToUpper.Trim
                        lsValue = dtDynamics.Rows(j)("value").ToString.ToUpper.Trim
                        Select Case lsCode
                            Case "TEXT"
                                lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " (c.ctryname like '%" & lsValue & "%' or p.plgrpname like '%" & lsValue & "%' or g.countrygroupname like '%" & lsValue & "%')"
                            Case "COUNTRY"
                                lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " c.ctryname like '%" & lsValue & "%'"
                            Case "REGION"
                                lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " p.plgrpname like '%" & lsValue & "%'"
                            Case "COUNTRYGROUP"
                                lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " g.countrygroupname like '%" & lsValue & "%'"
                                'Case "CUSTOMERGROUP" 'changed by mohamed on 12/08/2018
                                '    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " g.countrygroupname like '%" & lsValue & "%'"
                        End Select
                    Next



                    'lsSqlQry += " and ( "

                    'If lsSqlQryText.Trim <> "" Then
                    '    lsSqlQry += " (" & lsSqlQryText & ")"
                    'End If

                    'If lsSqlQryCountry.Trim <> "" Then
                    '    lsSqlQry += " (" & lsSqlQryCountry & ")"
                    'End If

                    'If lsSqlQryRegion.Trim <> "" Then
                    '    lsSqlQry += " (" & lsSqlQryRegion & ")"
                    'End If

                    'If lsSqlQryCountryGroup.Trim <> "" Then
                    '    lsSqlQry += " (" & lsSqlQryCountryGroup & ")"
                    'End If

                    'lsSqlQry += ")"
                End If
            End If
        End If

        If lsSqlQryAll.Trim <> "" Then
            lsSqlQry += " and ( " & lsSqlQryAll & IIf(lsCountriesSelectedList.Trim = "", "", IIf(lsSqlQryAll.Trim = "", "", " or ") & " c.ctrycode in (" & lsCountriesSelectedList & ")") & " ) "


        End If

        'Changed by mohamed on 20/12/2016
        If lsCountriesNotSelectedList.Trim <> "" Then
            lsSqlQry += " and c.ctrycode not in (" & lsCountriesNotSelectedList & ")"
        End If

        'changed by mohamed on 05/11/2016
        'Dim lsContractId As String = ""
        'Dim lsMaxid As String = ""
        'If Session("contractid") IsNot Nothing Then
        '    lsContractId = Session("contractid")
        'End If

        'If Session("Maxid") IsNot Nothing Then
        '    lsMaxid = Session("Maxid")
        'End If

        Dim lsSqlQryShowEdit As String = ""
        Dim lsSqlQryShowWhereCondition As String = "" 'Changed by mohamed on 05/10/2016
        Dim lsSqlQryShowEditField As String = ", case when sel.mktcode is null then 0 else 1 end chkselect"

        If hdnPageMode.Value.Trim.ToUpper <> "NEW" Then ' changed by mohamed on 05/11/2016 'If CType(Session("ContractState"), String) <> "New" Then 'changed by mohamed on 22/10/2016
            If 1 = 1 Then ' isAutoTick = True Then 'changed by mohamed on 22/10/2016
                'If hdnPageName.Value.Trim.ToUpper = "MAXOCCUPANCY" Then ' If Session("Maxid") IsNot Nothing Then 'changed by mohamed on 05/11/2016
                '    'If lsMaxid.Trim <> "" Then 'changed by mohamed on 12/10/2016
                '    If hdnTranId.Value.Trim <> "" Then 'changed by mohamed on 05/11/2016
                '        lsSqlQryShowEdit = " left join  partymaxacc_countries cc on c.ctrycode =cc.ctrycode and cc.tranid ='" & hdnTranId.Value.Trim & "' and ltrim(rtrim(cc.tranid))<>''"
                '        If isAutoTick = True Then
                '            lsSqlQryShowEditField = ", case when cc.tranid is null then case when sel.mktcode is null then 0 else 1 end else 1 end chkselect"
                '            lsSqlQryShowWhereCondition = " and cc.tranid is not null" 'Changed by mohamed on 05/10/2016
                '        Else
                '            lsSqlQryShowEditField = ", case when sel.mktcode is null then 0 else 1 end chkselect"
                '            lsSqlQryShowWhereCondition = "" 'Changed by mohamed on 05/10/2016
                '        End If
                '    End If
                'ElseIf hdnPageName.Value.Trim.ToUpper = "CONTRACTMAIN" Then 'else 'changed by mohamed on 05/11/2016
                '    'If lsContractId.Trim <> "" Then 'changed by mohamed on 12/10/2016
                '    If hdnTranId.Value.Trim <> "" Then 'changed by mohamed on 05/11/2016 

                '        'this method has been revised on 26/10/2016 -- changed by mohamed on 26/10/2016
                '        'lsSqlQryShowEdit = " left join  view_contracts_agents_countries cc on c.ctrycode =cc.ctrycode and cc.contractid ='" & lsContractId & "' and ltrim(rtrim(cc.contractid))<>''"
                '        lsSqlQryShowEdit = " left join view_contractcountry cc on c.ctrycode =cc.ctrycode and cc.contractid ='" & hdnTranId.Value.Trim & "' and ltrim(rtrim(cc.contractid))<>''"

                '        If isAutoTick = True Then
                '            lsSqlQryShowEditField = ", case when cc.contractid is null then case when sel.mktcode is null then 0 else 1 end else 1 end chkselect"
                '            lsSqlQryShowWhereCondition = " and cc.contractid is not null" 'Changed by mohamed on 05/10/2016
                '        Else
                '            lsSqlQryShowEditField = ", case when sel.mktcode is null then 0 else 1 end chkselect"
                '            lsSqlQryShowWhereCondition = "" 'Changed by mohamed on 05/10/2016
                '        End If
                '    End If
                'End If


                If hdnPageName.Value.Trim.ToUpper <> "" Then 'else 'changed by mohamed on 05/11/2016
                    If hdnTranId.Value.Trim <> "" Then
                        lsSqlQryShowEdit = " Left join dbo.fn_get_country_or_agent_code_fromtable ('" & hdnPageName.Value.Trim & "','" & hdnTranId.Value.Trim & "','COUNTRY') cc "
                        lsSqlQryShowEdit += " on c.ctrycode =cc.ctrycode and cc.tranid ='" & hdnTranId.Value.Trim & "' and ltrim(rtrim(cc.tranid))<>''"

                        If isAutoTick = True Then
                            lsSqlQryShowEditField = ", case when cc.tranid is null then case when sel.mktcode is null then 0 else 1 end else 1 end chkselect"
                            lsSqlQryShowWhereCondition = " and cc.tranid is not null" 'Changed by mohamed on 05/10/2016
                        Else
                            lsSqlQryShowEditField = ", case when sel.mktcode is null then 0 else 1 end chkselect"
                            lsSqlQryShowWhereCondition = "" 'Changed by mohamed on 05/10/2016
                        End If
                    End If
                End If

                'commented by mohamed on 20/10/2016
                'Else
                '    lsSqlQryShowEdit = " "
                '    lsSqlQryShowEditField = ", case when sel.mktcode is null then 0 else 1 end chkselect "
            End If
        End If



        lsSqlQry += " ) select distinct  c.ctrycode, c.ctryname, c.plgrpname, c.plgrpcode, 2 ordertype, "
        lsSqlQry += " isnull(stuff((select distinct ', ' + c1.countrygroupname from cte c1 where c1.ctrycode=c.ctrycode and c1.plgrpcode=c.plgrpcode for xml path('')),1,2,''),'') countrygroupname, "
        lsSqlQry += " isnull(stuff((select distinct ', ' + c2.countrygroupcode from cte c2 where c2.ctrycode=c.ctrycode and c2.plgrpcode=c.plgrpcode for xml path('')),1,2,''),'') countrygroupcode "
        lsSqlQry += lsSqlQryShowEditField
        lsSqlQry += " from cte c "
        lsSqlQry += " left join dbo.splitallotmkt('" & Replace(lsCountriesSelectedList, "'", "") & "',',') sel on sel.mktcode=c.ctrycode "
        lsSqlQry += lsSqlQryShowEdit

        lsSqlQry += " where 2 = 2 " & lsSqlQryShowWhereCondition 'Changed by mohamed on 05/10/2016

        If hdnPageMode.Value.Trim.ToUpper <> "NEW" Then ' If CType(Session("ContractState"), String) <> "New" Then 'changed by mohamed on 05/11/2016
            If isAutoTick = True And lsSqlQryShowEdit.Trim <> "" Then
                lsSqlQry += " and (sel.mktcode=c.ctrycode or c.ctrycode =cc.ctrycode) "
            Else
                'Changed by mohamed on 20/12/2016 to show always selected
                'If lb_Is_Validate_VS_selected = False Then
                'If lsSqlQryShowEdit.Trim <> "" Then
                '    lsSqlQry += " and (sel.mktcode=c.ctrycode or c.ctrycode =cc.ctrycode) "
                'Else
                '    lsSqlQry += " and (sel.mktcode=c.ctrycode) "
                'End If
                'End If
                lsSqlQry += " and (sel.mktcode=c.ctrycode) " '20/12/2016
            End If
        Else
            lsSqlQry += " and (sel.mktcode=c.ctrycode) " '20/12/2016
        End If

        lsSqlQry += " order by c.ctryname, c.plgrpname, countrygroupname "
        'If strMarket.Length > 0 Then
        '    strMarket = strMarket.Substring(0, Len(strMarket) - 1)
        'End If
        If lsSqlQry = "" Then
            'gv_ShowCountries.DataSource = Nothing
            'gv_ShowCountries.DataBind()

            'If hdStatus.Value.Trim.ToUpper = "CANCEL" Or hdStatus.Value.Trim.ToUpper = "OK" Then
            '    gv_ShowCountries.DataSource = Nothing
            '    gv_ShowCountries.DataBind()
            '    gv_showagents.DataSource = Nothing
            '    gv_showagents.DataBind()
            '    'Else
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Atleast One Country Group and Generate Country.');", True)
            'End If
            'hdStatus.Value = ""
            'Exit Sub
        Else
            ' Rosalin on 2019-10-27
            'If (hdnPageName.Value.Trim = "OFFERSMAIN") And hdnTranId.Value.Trim <> "" Then
            '    lsSqlQry = " New_OfferCountryGroupSelection '" & CType(hdnTranId.Value.Trim, String) & "'"
            'End If

            FillGridCountry(lsSqlQry)

            'If chkCountrySpecific.Checked = False Then
            '    'gv_ShowCountries.Visible = False
            '    gv_showagents.Visible = False
            'Else
            '    'gv_ShowCountries.Visible = True
            '    gv_showagents.Visible = True
            'End If
            hdCountryStatus.Value = ""
            Btnshowagents_Click(Nothing, Nothing)
        End If

        Session("isAutoTick_wuccountrygroupusercontrol") = Nothing
    End Sub

    Public Sub Btnshowagents_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles Btnshowagents.Click
        Dim strMarket As String = ""
        Dim plgrpcode As String = ""


        Dim chkSelAgent As CheckBox
        Dim strMarketAg As String = ""
        Dim plgrpcodeAg As String = ""
        Dim txtAgentCodes As Label
        Dim lblCountryCodes As Label


        Dim dragent As DataRow
        Dim dtagents As New DataTable
        dtagents = Session("AgentSelected_Sess")
        dtagents.Clear()
        For Each gvRow In gv_showagents.Rows
            chkSelAgent = gvRow.FindControl("chk2")
            txtAgentCodes = gvRow.FindControl("txtagentcode")
            lblCountryCodes = gvRow.FindControl("lblCountryCode")
            If chkSelAgent.Checked = True Then
                plgrpcodeAg = txtAgentCodes.Text  'gvRow.Cells(1).Text.Trim
                If plgrpcodeAg <> "" Then
                    dragent = dtagents.NewRow()
                    dragent("agentcode") = plgrpcodeAg
                    dragent("ctrycode") = lblCountryCodes.Text
                    dragent("selected") = "Y"
                    strMarketAg = strMarketAg + "'" + plgrpcodeAg + "',"
                    dtagents.Rows.Add(dragent)
                End If
            End If
        Next
        Session("AgentSelected_Sess") = dtagents


        Dim lsAvailableCtryCode As String = ""
        'Dim dr As DataRow
        'Dim dtcountry As New DataTable
        'dtcountry = Session("CountrySelected_Sess")

        Dim chkSel As CheckBox
        Dim lblctrygrp As Label
        For Each gvRow In gv_ShowCountries.Rows
            chkSel = gvRow.FindControl("chkCtry2")
            lblctrygrp = gvRow.FindControl("txtctrycode")

            'changed by mohamed on 03/10/2016
            lsAvailableCtryCode = lsAvailableCtryCode & IIf(lsAvailableCtryCode.Trim = "", "", ",") & "'" & lblctrygrp.Text.Trim & "'"

            If chkSel.Checked = True Then
                plgrpcode = lblctrygrp.Text  'gvRow.Cells(1).Text.Trim
                If plgrpcode <> "" Then

                    'dr = dtagents.NewRow()
                    'dr("countrycode") = plgrpcode
                    'dr("selected") = "Y"
                    'dtagents.Rows.Add(dr)
                    strMarket = strMarket + "'" + plgrpcode + "',"
                End If
            End If
        Next
        If strMarket.Length > 0 Then
            strMarket = strMarket.Substring(0, Len(strMarket) - 1)
        End If

        Session("SelectedCountriesList_WucCountryGroupUserControl") = strMarket
        Session("AllCountriesList_WucCountryGroupUserControl") = lsAvailableCtryCode
        Session("SelectedAgentList_WucCountryGroupUserControl") = strMarketAg

        If strMarket = "" Then
            gv_showagents.DataSource = Nothing
            gv_showagents.DataBind()
            Session("skipSelectAllCountryChangeEvent") = Nothing
            If hdCountryStatus.Value.Trim.ToUpper = "CANCEL" Or hdCountryStatus.Value.Trim.ToUpper = "OK" Then
                'gv_showagents.DataSource = Nothing
                'gv_showagents.DataBind()
            Else

                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Atleast One Country .');", True)
            End If

            hdCountryStatus.Value = ""

            Exit Sub
        Else
            'fnGetSelectedCountriesList()
            Session("skipSelectAllCountryChangeEvent") = Nothing
            FillGridAgent(strMarket)

            'If chkAgentSpecific.Checked = False Then
            '    gv_showagents.Visible = False
            'Else
            '    gv_showagents.Visible = True
            'End If
        End If
        Session("skipSelectAllCountryChangeEvent") = Nothing
    End Sub

    'Protected Sub chk2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim chkSelect As CheckBox = CType(sender, CheckBox)
    '    Dim row As GridViewRow = CType((CType(sender, CheckBox)).NamingContainer, GridViewRow)
    '    Dim lb As Label = CType(row.FindControl("txtcountrygrpcode"), Label)
    '    Session("GridViewCountryGroupRow") = Nothing
    '    Dim lblCountryGroupCode As Label
    '    hdStatus.Value = ""

    '    If chkSelect.Checked = False Then
    '        Dim strFlag As String = "0"
    '        If gv_ShowCountries.Rows.Count > 0 Then
    '            For Each gvRow In gv_ShowCountries.Rows
    '                Dim chkselectCountry As CheckBox = CType(gvRow.FindControl("chkCtry2"), CheckBox)
    '                If chkselectCountry.Checked = True Then

    '                    lblCountryGroupCode = gvRow.FindControl("lblCountryGroupCode")
    '                    If lblCountryGroupCode.Text.Trim = lb.Text.Trim Then
    '                        strFlag = "1"
    '                        Session("GridViewCountryGroupRow") = row
    '                        Dim str As String = "'" + chkSelect.ClientID + "'"
    '                        Dim strScript As String = "javascript: fnConfirm(" + str + ")"
    '                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
    '                        Exit For
    '                    End If

    '                End If
    '            Next
    '            If strFlag = "0" Then
    '                hdStatus.Value = "Ok"
    '                sbShowCountry()
    '            End If
    '        End If
    '    Else
    '        sbShowCountry()
    '    End If

    'End Sub

    'Protected Sub btnHidProcessCheckBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidProcessCheckBox.Click
    '    If hdStatus.Value.Trim <> "" Then
    '        Dim row As GridViewRow = Session("GridViewCountryGroupRow")
    '        Session("GridViewCountryGroupRow") = Nothing
    '        Dim chkSelect As CheckBox = CType(row.FindControl("chk2"), CheckBox)
    '        If chkSelect IsNot Nothing Then
    '            If hdStatus.Value.Trim.ToUpper = "CANCEL" Then
    '                chkSelect.Checked = True
    '            ElseIf hdStatus.Value.Trim.ToUpper = "OK" Then
    '                'process removing
    '                sbShowCountry()
    '            End If
    '        End If
    '    End If
    'End Sub

    Protected Sub chkCtryAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim strFlag As String = "0"
        If chkSelect.Checked = False Then
            If gv_showagents.Rows.Count > 0 Then
                For Each gvRow In gv_showagents.Rows
                    Dim chkselectAgnt As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                    If chkselectAgnt.Checked = True Then
                        Session("skipSelectAllCountryChangeEvent") = 1
                        strFlag = "1"
                        Dim str As String = "'" + chkSelect.ClientID + "'"
                        Dim strScript As String = "javascript:fnConfirmCountryAll(" + str + ")"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
                        Exit For
                    End If
                Next
            End If

            If strFlag = "0" Then
                GoTo assignChkValue
            End If
        Else
            GoTo assignChkValue
        End If
        Exit Sub
assignChkValue:
        If Session("skipSelectAllCountryChangeEvent") = Nothing Then
            'Unselect all the country
            For Each ctryRow As GridViewRow In gv_ShowCountries.Rows

                Dim lblordertype As Label = CType(ctryRow.FindControl("lblordertype"), Label)
                If lblordertype.Text <> "1" Then
                    Dim chkctry2 As CheckBox = CType(ctryRow.FindControl("chkctry2"), CheckBox)
                    chkctry2.Checked = chkSelect.Checked
                    sbCountrySelectAndSort(chkctry2, IIf(chkSelect.Checked = True, 1, 0), "")

                    ctryRow.CssClass = IIf(chkSelect.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") ' changed by mohamed on 30/10/2016
                End If
            Next

            If chkSelect.Checked = False Then
                gv_showagents.DataSource = Nothing
                gv_showagents.DataBind()
            End If

            'commented / changed by mohamed on 30/10/2016
            'gv_ShowCountries.DataSource = ViewState("ctrydatatable_ctryusercontrol")
            'gv_ShowCountries.DataBind()
            'Btnshowagents_Click(Nothing, Nothing)

            Dim lchkAll As CheckBox = gv_ShowCountries.HeaderRow.FindControl("chkCtryAll")
            lchkAll.Checked = chkSelect.Checked
        End If
    End Sub


    Protected Sub chk2All_agent_checkedchanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelectHead As CheckBox = CType(sender, CheckBox)
        For Each gvRow In gv_showagents.Rows
            Dim lblordertype As Label = CType(gvRow.FindControl("lblordertype"), Label)
            If lblordertype.Text <> "1" Then
                Dim chkselectAgnt As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                chkselectAgnt.Checked = chkSelectHead.Checked
                sbAgentSelectAndSort(chkselectAgnt, IIf(chkSelectHead.Checked = True, 1, 0), "")
            End If
        Next

        ''added / changed by mohamed on 30/10/2016
        'If chkSelectHead.Checked = False Then
        '    gv_showagents.DataSource = Nothing
        '    gv_showagents.DataBind()
        'End If

        'commented / changed by mohamed on 30/10/2016
        'gv_showagents.DataSource = ViewState("agentdatatable_ctryusercontrol")
        'gv_showagents.DataBind()
        'GrdAgentGrouping_OnDataBound()

        'Dim chkAll As CheckBox = gv_showagents.HeaderRow.FindControl("chkAll")
        'chkAll.Checked = chkSelectHead.Checked
    End Sub

    Protected Sub chk2_agent_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk2_agent As CheckBox = CType(sender, CheckBox)
        sbAgentSelectAndSortAndAssign(sender, IIf(chk2_agent.Checked = True, 1, 0), "")


    End Sub

    Protected Sub chkCtry2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim row As GridViewRow = CType((CType(sender, CheckBox)).NamingContainer, GridViewRow)
        Dim lb As Label = CType(row.FindControl("txtctrycode"), Label)
        Session("GridViewCountryRow") = Nothing
        Dim lblCountryCode As Label
        hdCountryStatus.Value = ""



        If chkSelect.Checked = False Then
            Dim strFlag As String = "0"
            If gv_showagents.Rows.Count > 0 Then
                For Each gvRow In gv_showagents.Rows
                    Dim chkselectAgnt As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                    If chkselectAgnt.Checked = True Then

                        lblCountryCode = gvRow.FindControl("lblCountryCode")
                        If lblCountryCode.Text.Trim = lb.Text.Trim Then
                            strFlag = "1"
                            Session("GridViewCountryRow") = row
                            Dim str As String = "'" + chkSelect.ClientID + "'"
                            Dim strScript As String = "javascript:fnConfirmCountry(" + str + ")"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
                            Exit For
                        End If

                    End If
                Next
                If strFlag = "0" Then
                    hdCountryStatus.Value = "Ok"
                    sbCountrySelectAndSortAndAssign(sender, 0, "") 'changed by mohamed on 19/10/2016
                    row.CssClass = IIf(chkSelect.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") 'changed by mohamed on 30/10/2016
                    Btnshowagents_Click(sender, e)
                End If
            Else
                Session("SelectedCountriesList_WucCountryGroupUserControl") = Nothing
                Session("SelectedAgentList_WucCountryGroupUserControl") = Nothing
            End If
        Else
            row.CssClass = IIf(chkSelect.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") 'changed by mohamed on 30/10/2016
            sbCountrySelectAndSortAndAssign(sender, 1, "") 'changed by mohamed on 19/10/2016

            'Btnshowagents_Click(sender, e) 'commented / changed by mohamed on 30/10/2016

            'changed by mohamed on 19/10/2016
            'For Each lgvrow As GridViewRow In gv_showagents.Rows
            '    Dim lbAgentCtry As Label = CType(lgvrow.FindControl("lblCountryCode"), Label)
            '    Dim lchk2 As CheckBox = CType(lgvrow.FindControl("chk2"), CheckBox)
            '    If lbAgentCtry IsNot Nothing Then
            '        If lbAgentCtry.Text.ToUpper.Trim = lb.Text.ToUpper.Trim Then
            '            lchk2.Checked = True
            '        End If
            '    End If
            'Next
        End If

        chkSelect.Focus()
    End Sub
    Sub sbCountrySelectAndSortAndAssign(ByVal sender As Object, ByVal liCheckBoxValue As Integer, ByVal ctryCode As String)
        sbCountrySelectAndSort(sender, liCheckBoxValue, ctryCode)

        'commented / changed by mohamed on 30/10/2016
        'gv_ShowCountries.DataSource = ViewState("ctrydatatable_ctryusercontrol")
        'gv_ShowCountries.DataBind()
        'GrdCountryGrouping_OnDataBound()
    End Sub
    Sub sbCountrySelectAndSort(ByVal sender As Object, ByVal liCheckBoxValue As Integer, ByVal ctryCode As String)
        Dim lst As List(Of MyClasses.CountryModel) = ViewState("ctrydatatable_ctryusercontrol")
        Dim id As String = ""
        If ctryCode.Trim = "" Then
            Dim chkbx As CheckBox = sender
            Dim hf As Label = chkbx.Parent.FindControl("txtctrycode")
            id = hf.Text
        Else
            id = ctryCode
        End If
        If lst.Exists(Function(x) x.ctrycode.Equals(id)) = True Then 'this condition is added
            Dim item = lst.Single(Function(w) w.ctrycode.Equals(id))
            item.chkselect = liCheckBoxValue
        End If
        Dim lst1 = (From x In lst Order By x.chkselect Descending, x.ordertype, x.ctryname Ascending Select x).ToList()
        ViewState("ctrydatatable_ctryusercontrol") = lst1
        'gv_ShowCountries.DataSource = lst1
        'gv_ShowCountries.DataBind()
    End Sub

    Sub sbAgentSelectAndSortAndAssign(ByVal sender As Object, ByVal liCheckBoxValue As Integer, ByVal agentCode As String)
        sbAgentSelectAndSort(sender, liCheckBoxValue, agentCode)

        'commented / changed by mohamed on 30/10/2016
        'gv_showagents.DataSource = ViewState("agentdatatable_ctryusercontrol")
        'gv_showagents.DataBind()
        'GrdAgentGrouping_OnDataBound()
    End Sub

    Sub sbAgentSelectAndSort(ByVal sender As Object, ByVal liCheckBoxValue As Integer, ByVal agentCode As String)
        Dim lst As List(Of MyClasses.CountryAgentModel) = ViewState("agentdatatable_ctryusercontrol")
        Dim id As String = ""
        If agentCode.Trim = "" Then
            Dim chkbx As CheckBox = sender
            Dim hf As Label = chkbx.Parent.FindControl("txtagentcode")
            id = hf.Text
        Else
            id = agentCode
        End If
        Dim item = lst.Single(Function(w) w.agentcode.Equals(id))
        item.chkselect = liCheckBoxValue
        Dim lst1 = (From x In lst Order By x.chkselect Descending, x.ordertype, x.ctryname Ascending, x.agentname Ascending Select x).ToList()
        ViewState("agentdatatable_ctryusercontrol") = lst1
        'gv_ShowCountries.DataSource = lst1
        'gv_ShowCountries.DataBind()
    End Sub

    Protected Sub btnHidProcessCheckBox_Country_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidProcessCheckBox_Country.Click
        If hdCountryStatus.Value.Trim <> "" Then
            Dim row As GridViewRow = Session("GridViewCountryRow")
            Session("GridViewCountryRow") = Nothing
            Dim chkSelect As CheckBox = CType(row.FindControl("chkCtry2"), CheckBox)
            If chkSelect IsNot Nothing Then
                If hdCountryStatus.Value.Trim.ToUpper = "CANCEL" Then
                    chkSelect.Checked = True
                ElseIf hdCountryStatus.Value.Trim.ToUpper = "OK" Then
                    'process removing
                    sbCountrySelectAndSortAndAssign(chkSelect, 0, "") 'changed by mohamed on 19/10/2016
                    row.CssClass = IIf(chkSelect.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") 'changed by mohamed on 30/10/2016
                    Btnshowagents_Click(sender, e)
                End If
            End If
        End If
    End Sub

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub chkCountrySpecific_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCountrySpecific.CheckedChanged
    '    Dim chkSel As CheckBox
    '    Dim strFlag As String = "0"
    '    'For Each gvRow In gv_ShowCountriesgrp.Rows '06/08
    '    '    chkSel = gvRow.FindControl("chk2")
    '    '    If chkSel.Checked = True Then
    '    '        strFlag = "1"
    '    '        Exit For
    '    '    End If
    '    'Next
    '    If strFlag = "0" Then
    '        If chkCountrySpecific.Checked = False Then
    '            chkCountrySpecific.Checked = True
    '        Else
    '            chkCountrySpecific.Checked = False
    '        End If
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Atleast One Country Group .');", True)
    '        Exit Sub
    '    End If


    '    If chkCountrySpecific.Checked = False Then
    '        'gv_ShowCountries.Visible = False
    '        gv_showagents.Visible = False
    '        chkAgentSpecific.Checked = False
    '        UnCheckCheckAllgvCountry()
    '        UnCheckCheckAllgvAgent()

    '    Else
    '        'gv_ShowCountries.Visible = True
    '        If chkAgentSpecific.Checked = False Then
    '            gv_showagents.Visible = False
    '            UnCheckCheckAllgvAgent()
    '        Else
    '            gv_showagents.Visible = True
    '        End If
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' 
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub chkAgentSpecific_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAgentSpecific.CheckedChanged

    '    Dim chkSel As CheckBox
    '    Dim lblctrygrp As Label
    '    Dim strFlag As String = "0"
    '    For Each gvRow In gv_ShowCountries.Rows
    '        chkSel = gvRow.FindControl("chkCtry2")
    '        lblctrygrp = gvRow.FindControl("txtctrycode")
    '        If chkSel.Checked = True Then
    '            strFlag = "1"
    '            Exit For
    '        End If
    '    Next
    '    'If strFlag = "0" Then
    '    '    'If chkAgentSpecific.Checked = False Then
    '    '    '    chkAgentSpecific.Checked = True
    '    '    'Else
    '    '    '    chkAgentSpecific.Checked = False
    '    '    'End If
    '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Atleast One Country .');", True)
    '    '    Exit Sub
    '    'End If


    '    'If chkAgentSpecific.Checked = False Then
    '    '    gv_showagents.Visible = False
    '    '    UnCheckCheckAllgvAgent()
    '    'Else
    '    Btnshowagents_Click(sender, e)
    '    gv_showagents.Visible = True
    '    'End If




    'End Sub
    ' ''' <summary>
    ' ''' UnCheckCheckAllgvAgent
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub UnCheckCheckAllgvAgent()

    '    If chkAgentSpecific.Checked = False Then
    '        If gv_showagents.Rows.Count > 0 Then


    '            Dim row As GridViewRow
    '            Dim ChkBoxHeader As CheckBox = CType(gv_showagents.HeaderRow.FindControl("chkAll"), CheckBox)
    '            ChkBoxHeader.Checked = False
    '            For Each row In gv_showagents.Rows
    '                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chk2"), CheckBox)
    '                ChkBoxRows.Checked = False
    '            Next
    '        End If
    '    End If


    'End Sub
    ' ''' <summary>
    ' ''' UnCheckCheckAllgvCountry
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub UnCheckCheckAllgvCountry()

    '    If chkAgentSpecific.Checked = False Then
    '        If gv_ShowCountries.Rows.Count > 0 Then

    '            Dim row As GridViewRow
    '            Dim ChkBoxHeader As CheckBox = CType(gv_ShowCountries.HeaderRow.FindControl("chkAll"), CheckBox)
    '            ChkBoxHeader.Checked = False
    '            For Each row In gv_ShowCountries.Rows
    '                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkCtry2"), CheckBox)
    '                ChkBoxRows.Checked = False
    '            Next
    '        End If
    '    End If


    'End Sub

    Function fnbtnVsProcess(ByVal txtvsprocesssplit As TextBox, ByVal dlList As DataList) As Boolean
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessRegion As String = ""
        Dim lsProcessCountryGroup As String = ""

        Dim lsCode As String = ""
        Dim lsValue As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        Dim lbShowAgentPopup As Boolean = False

        For i = 0 To lsMainArr.GetUpperBound(0)
            lsCode = lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
            lsValue = lsMainArr(i).Split(":")(1).ToString.ToUpper.Trim
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "REGION"
                    lsProcessRegion = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("REGION", lsProcessRegion, "R")
                Case "COUNTRYGROUP"
                    lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRYGROUP", lsProcessCountryGroup, "CG")
                Case "CUSTOMERGROUP" 'changed by mohamed on 12/08/2018
                    lbShowAgentPopup = True
                    lsProcessCountryGroup = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMERGROUP", lsProcessCountryGroup, "AG")
                Case "TEXT"
                    lsProcessText = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessText, "T")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCountry, "C")
            End Select
        Next
        'objUtils.sbSetSelectedValueForHTMLSelect("[Select]", ddlsccode)

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        'sbShowCountry() 'changed by mohamed on 21/12/2016
        fnCodeAndValue_ButtonClick(Nothing, Nothing, Nothing, lsCode, lsValue)
        Return True
    End Function
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If Not dtt Is Nothing Then
            If dtt.Rows.Count >= 0 Then
                For i = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                        iFlag = 1
                    End If
                Next
                If iFlag = 0 Then
                    dtt.NewRow()
                    dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ":" & lsValue.Trim.ToUpper)
                    Session("sDtDynamic") = dtt
                End If
            End If
        End If
   
        Return True
    End Function

    Function fnCodeAndValue_ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs, ByRef dlList As DataList, Optional ByVal asLinkCode As String = Nothing, Optional ByVal asLinkValue As String = Nothing) As Boolean


        Dim lsLnkCode As String = "", lsValue As String
        If asLinkCode Is Nothing Then
            asLinkCode = ""
        End If
        If asLinkValue Is Nothing Then
            asLinkValue = ""
        End If

        If asLinkCode.Trim = "" Or asLinkValue.Trim = "" Then
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

            lsLnkCode = lnkcode.Text.Trim.ToUpper
            lsValue = lnkvalue.Text.Trim.ToUpper
        Else
            lsLnkCode = asLinkCode
            lsValue = asLinkValue
        End If

        'Dim lblCountryCodeAgn As Label

        'Dim strFlag As String = "0"
        'hdCountryStatus.Value = ""

        'Session("GridViewCountryRow") = Nothing
        'Session("dllist_selected_sender") = sender
        'Session("dllist_selected_e") = e
        'Session("dllist_selected_dlItem") = dlItem
        'Session("dllist_countryuserctrl") = dlList
        'Session("dllist_countryuserctrl_ClientID") = dlList.ID

        'Dim lbConsiderRow As Boolean

        'changed by mohamed on 13/08/2018 as to show Agent if Customer Group selected
        Dim lsSqlQry As String = ""
        If lsLnkCode.Trim.ToUpper = "CUSTOMERGROUP" Then
            Dim lsAvailableAgentInAgentGrid As String = ""
            'Dim lclsAgent As List(Of MyClasses.CountryAgentModel) = ViewState("agentdatatable_ctryusercontrol")
            'If lclsAgent IsNot Nothing Then
            '    Dim lclsagentSelected = From xrow In lclsAgent
            '                            Where xrow.chkselect = 1
            '                            Select xrow
            '    For Each lrow As MyClasses.CountryAgentModel In lclsagentSelected
            '        lsAvailableAgentInAgentGrid += IIf(lsAvailableAgentInAgentGrid.Trim = "", "", ",") & lrow.agentcode
            '    Next
            'End If

            Dim lclsagent As List(Of MyClasses.CountryAgentModel) = ViewState("agentdatatable_ctryusercontrol")
            If lclsagent IsNot Nothing Then
                Dim lclsagentSelected = From xrow In lclsagent
                                        Where xrow.chkselect = 1 And xrow.customergroupname.ToUpper.Trim = lsValue.Trim
                                        Select xrow
                For Each lrow As MyClasses.CountryAgentModel In lclsagentSelected
                    lsAvailableAgentInAgentGrid += IIf(lsAvailableAgentInAgentGrid.Trim = "", "", ",") & lrow.agentcode
                Next
            End If


            lsSqlQry = ";with cte as ("
            lsSqlQry += " select distinct c.Agentcode, c.Agentname, c.ctrycode, ca.customergroupcode, ca.customergroupname "
            lsSqlQry += "	from Agentmast c (nolock) "
            lsSqlQry += "	join view_customergroup ca (nolock) on ca.agentcode=c.agentcode"
            lsSqlQry += " where 1=1"

            Dim lsSqlQryText As String = "", lsSqlQryCountry As String = "", lsSqlQryRegion As String = "", lsSqlQryCountryGroup As String = ""
            Dim lb_Is_Validate_VS_selected As Boolean = False
            Dim lsSqlQryAll As String = ""
            lblCountryPopupType.Text = "A"
            lblCountryPopupHead.Text = "Select Agent"
            Select Case lsLnkCode.Trim.ToUpper
                Case "CUSTOMERGROUP"
                    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " ca.customergroupname like '%" & lsValue & "%'"
            End Select

            If lsSqlQryAll.Trim <> "" Then
                lsSqlQry += " and ( " & lsSqlQryAll & " ) "
            End If

            lsSqlQry += ") select case when sel.mktcode is null then 0 else 1 end chkselect, c.* from cte c "
            lsSqlQry += " left join dbo.splitallotmkt('" & lsAvailableAgentInAgentGrid & "',',') sel on sel.mktcode=c.Agentcode"
            lsSqlQry += " order by case when sel.mktcode is null then 1 else 0 end, c.agentname "


            Dim dsAgent As DataSet
            dsAgent = objUtils.ExecuteQuerySqlnew(Session("dbconnectionname"), lsSqlQry)
            If dsAgent.Tables(0).Rows.Count > 0 Then
                gvShowAgentPopup.DataSource = dsAgent.Tables(0)
                gvShowAgentPopup.DataBind()
                ModalPopupShowAgent.Show()
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Agent is available for this country' );", True)
            End If
        Else
            Dim lsAvailablectryInctryGrid As String = ""
            Dim lclsctry As List(Of MyClasses.CountryModel) = ViewState("ctrydatatable_ctryusercontrol")
            If lclsctry IsNot Nothing Then
                Dim lclsagentSelected = From xrow In lclsctry
                                        Where xrow.chkselect = 1
                                        Select xrow
                For Each lrow As MyClasses.CountryModel In lclsagentSelected
                    lsAvailablectryInctryGrid += IIf(lsAvailablectryInctryGrid.Trim = "", "", ",") & lrow.ctrycode
                Next
            End If

            lsSqlQry = ";with cte as ("
            lsSqlQry += " select distinct c.ctrycode, c.ctryname "
            lsSqlQry += "	from ctrymast c (nolock) "
            lsSqlQry += "	join plgrpmast p (nolock) on p.plgrpcode=c.plgrpcode"
            lsSqlQry += "	left join countrygroup_detail gd (nolock) on gd.ctrycode=c.ctrycode"
            lsSqlQry += "	left join countrygroup g (nolock) on gd.countrygroupcode=g.countrygroupcode"
            lsSqlQry += " where 1=1"

            Dim lsSqlQryText As String = "", lsSqlQryCountry As String = "", lsSqlQryRegion As String = "", lsSqlQryCountryGroup As String = ""
            Dim lb_Is_Validate_VS_selected As Boolean = False
            Dim lsSqlQryAll As String = ""
            lblCountryPopupType.Text = "C"
            lblCountryPopupHead.Text = "Select Country"
            'lsValue = lnkvalue.Text.Trim.ToUpper
            Select Case lsLnkCode.Trim.ToUpper ' lnkcode.Text.Trim.ToUpper

                Case "TEXT"
                    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " (c.ctryname like '%" & lsValue & "%' or p.plgrpname like '%" & lsValue & "%' or g.countrygroupname like '%" & lsValue & "%')"
                Case "COUNTRY"
                    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " c.ctryname like '%" & lsValue & "%'"
                Case "REGION"
                    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " p.plgrpname like '%" & lsValue & "%'"
                Case "COUNTRYGROUP"
                    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " g.countrygroupname like '%" & lsValue & "%'"
                    'Case "CUSTOMERGROUP" 'changed by mohamed on 12/08/2018
                    '    lsSqlQryAll += IIf(lsSqlQryAll.Trim = "", "", " or ") & " g.customergroupname like '%" & lsValue & "%'"
            End Select

            If lsSqlQryAll.Trim <> "" Then
                lsSqlQry += " and ( " & lsSqlQryAll & " ) "
            End If

            lsSqlQry += ") select c.*, case when sel.mktcode is null then 0 else 1 end chkselect from cte c "
            lsSqlQry += " left join dbo.splitallotmkt('" & lsAvailablectryInctryGrid & "',',') sel on sel.mktcode=c.ctrycode"
            lsSqlQry += " order by case when sel.mktcode is null then 1 else 0 end, c.ctryname "

            Dim ds As DataSet
            ds = objUtils.ExecuteQuerySqlnew(Session("dbConnectionName"), lsSqlQry)
            If ds.Tables.Count > 0 Then
                gvShowCountryPopup.DataSource = ds
            Else
                gvShowCountryPopup.DataSource = Nothing
            End If
            gvShowCountryPopup.DataBind()

            ModalPopupShowCountry.Show()
        End If
        Return True
    End Function

    Function fnCloseButtonClick(ByVal sender As Object, ByVal e As System.EventArgs, ByRef dlList As DataList) As Boolean
        Dim myButton As Button = CType(sender, Button)
        Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
        Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
        Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
        Dim lblCountryCodeAgn As Label

        Dim strFlag As String = "0"
        hdCountryStatus.Value = ""

        Session("GridViewCountryRow") = Nothing
        Session("dllist_selected_sender") = sender
        Session("dllist_selected_e") = e
        Session("dllist_selected_dlItem") = dlItem
        Session("dllist_countryuserctrl") = dlList
        Session("dllist_countryuserctrl_ClientID") = dlList.ID

        Dim lbConsiderRow As Boolean

        For Each ctryRow As GridViewRow In gv_ShowCountries.Rows
            Dim lblctryname As Label = ctryRow.FindControl("lblctryname")
            Dim lblplgrpName As Label = ctryRow.FindControl("lblplgrpName")
            Dim lblCountryGroupName As Label = ctryRow.FindControl("lblCountryGroupName")
            Dim lblCountryCode As Label = ctryRow.FindControl("txtctrycode")

            Dim lblCustomerGroupName As Label = ctryRow.FindControl("lblCustomerGroupName") 'Changed by mohamed on 12/08/2018

            lbConsiderRow = False
            Select Case lnkcode.Text.Trim.ToUpper
                Case "COUNTRY"
                    If lblctryname.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    End If
                Case "REGION"
                    If lblplgrpName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    End If
                Case "COUNTRYGROUP"
                    If lblCountryGroupName.Text.ToUpper.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    End If
                    'Case "CUSTOMERGROUP" 'changed by mohamed on 12/08/2018
                    '    If lblCustomerGroupName.Text.ToUpper.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                    '        lbConsiderRow = True
                    '    End If
                Case "TEXT"
                    If lblctryname.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    ElseIf lblplgrpName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    ElseIf lblCountryGroupName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                        lbConsiderRow = True
                    End If
            End Select

            If lbConsiderRow = True Then
                If gv_showagents.Rows.Count > 0 Then
                    For Each gvRow In gv_showagents.Rows
                        Dim chkselectCountry As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                        If chkselectCountry.Checked = True Then
                            lblCountryCodeAgn = gvRow.FindControl("lblCountryCode")
                            If lblCountryCode.Text.Trim = lblCountryCodeAgn.Text.Trim Then
                                strFlag = "1"

                                Dim strScript As String = "javascript:fnConfirmCountryList()"
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
                                Exit For
                            End If
                        Else 'handle here changed by moahmed on 13/08/2018
                        End If
                    Next
                End If
            End If
            If strFlag = "1" Then
                Exit For
            End If
        Next

        If strFlag = "0" Then
            hdCountryStatus.Value = "OkSERVERSIDE"
            fnCloseButtonClickAfterConfirmation()
        End If
        Return True
    End Function

    Function fnCloseButtonClickAfterConfirmation() As Boolean
        Try
            Dim lbRequireClientButtonClick As Boolean = False
            If Session("dllist_selected_dlItem") IsNot Nothing And Left(hdCountryStatus.Value.Trim.ToUpper, 2) = "OK" Then

                If hdCountryStatus.Value.Trim.ToUpper = "OK" Then
                    lbRequireClientButtonClick = True
                End If

                Dim dlItem As DataListItem = Session("dllist_selected_dlItem")
                Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
                Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
                Dim dlList As DataList = Me.Parent.FindControl(Session("dllist_countryuserctrl_ClientID"))  'Session("dllist_countryuserctrl")
                Dim dtDynamics As New DataTable
                dtDynamics = Session("sDtDynamic")
                If Session("sDtDynamic") IsNot Nothing Then
                    If dtDynamics.Rows.Count > 0 Then
                        Dim j As Integer
                        For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                            If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
                                dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                            End If
                        Next
                    End If
                    Session("sDtDynamic") = dtDynamics
                End If
                dlList.DataSource = dtDynamics
                dlList.DataBind()


                'Unselect before refreshing country

                Dim lblCountryCodeAgn As Label
                Dim lbConsiderRow As Boolean, lbConsiderRowCustomerGroup As Boolean = False
                If lnkcode.Text.Trim.ToUpper <> "CUSTOMERGROUP" Then 'changed by mohamed on 14/08/2018
                    For Each ctryRow As GridViewRow In gv_ShowCountries.Rows
                        Dim chkctry2 As CheckBox = CType(ctryRow.FindControl("chkctry2"), CheckBox)
                        Dim lblctryname As Label = ctryRow.FindControl("lblctryname")
                        Dim lblplgrpName As Label = ctryRow.FindControl("lblplgrpName")
                        Dim lblCountryGroupName As Label = ctryRow.FindControl("lblCountryGroupName")
                        Dim lblCountryCode As Label = ctryRow.FindControl("txtctrycode")

                        Dim lblCustomerGroupName As Label = ctryRow.FindControl("lblCustomerGroupName")

                        lbConsiderRow = False
                        Select Case lnkcode.Text.Trim.ToUpper
                            Case "COUNTRY"
                                If lblctryname.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                End If
                            Case "REGION"
                                If lblplgrpName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                End If
                            Case "COUNTRYGROUP"
                                If lblCountryGroupName.Text.ToUpper.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                End If
                                'Case "CUSTOMERGROUP"
                                '    lbConsiderRowCustomerGroup = True
                            Case "TEXT"
                                If lblctryname.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                ElseIf lblplgrpName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                ElseIf lblCountryGroupName.Text.ToUpper.Trim.IndexOf(lnkvalue.Text.Trim.ToUpper) >= 0 Then
                                    lbConsiderRow = True
                                End If
                        End Select

                        If lbConsiderRow = True Then
                            chkctry2.Checked = False
                            If gv_showagents.Rows.Count > 0 Then
                                For Each gvRow In gv_showagents.Rows
                                    Dim chkselectAgnt As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                                    If chkselectAgnt.Checked = True Then
                                        lblCountryCodeAgn = gvRow.FindControl("lblCountryCode")
                                        If lblCountryCode.Text.Trim = lblCountryCodeAgn.Text.Trim Then
                                            chkselectAgnt.Checked = False
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                Else 'changed by mohamed on 14/08/2018
                    'changed by mohamed on 14/08/2018
                    Dim lAgentCountry As New List(Of String)
                    Dim lAgentCountryFromOtherSource As New List(Of String)
                    If lnkcode.Text.Trim.ToUpper = "CUSTOMERGROUP" Then
                        For Each gvRow As GridViewRow In gv_showagents.Rows
                            Dim chkselectAgnt As CheckBox = CType(gvRow.FindControl("chk2"), CheckBox)
                            Dim lblCustomerGroupName As Label = gvRow.FindControl("lblCustomerGroupName")
                            lblCountryCodeAgn = gvRow.FindControl("lblCountryCode")
                            If lnkvalue.Text.Trim.ToUpper = lblCustomerGroupName.Text.Trim.ToUpper Then
                                chkselectAgnt.Checked = False
                                If lAgentCountry.IndexOf(lblCountryCodeAgn.Text.Trim.ToUpper) < 0 And lAgentCountryFromOtherSource.IndexOf(lblCountryCodeAgn.Text.Trim.ToUpper) < 0 Then
                                    lAgentCountry.Add(lblCountryCodeAgn.Text.Trim.ToUpper)
                                End If
                            Else
                                If lAgentCountryFromOtherSource.IndexOf(lblCountryCodeAgn.Text.Trim.ToUpper) < 0 Then
                                    lAgentCountryFromOtherSource.Add(lblCountryCodeAgn.Text.Trim.ToUpper)
                                End If
                                If lAgentCountry.IndexOf(lblCountryCodeAgn.Text.Trim.ToUpper) >= 0 Then
                                    lAgentCountry.Remove(lblCountryCodeAgn.Text.Trim.ToUpper)
                                End If
                            End If
                        Next

                        For Each ctryRow As GridViewRow In gv_ShowCountries.Rows
                            Dim lblCountryCode As Label = ctryRow.FindControl("txtctrycode")
                            Dim chkctry2 As CheckBox = CType(ctryRow.FindControl("chkctry2"), CheckBox)
                            If lAgentCountry.IndexOf(lblCountryCode.Text.Trim.ToUpper) >= 0 Then
                                chkctry2.Checked = False
                            End If
                        Next
                    End If
                End If

                'Refresh Country & Agent
                sbShowCountry()
            End If

            'If lbRequireClientButtonClick = True Then
            '    Dim strScript As String = "javascript:fnBtnDummyProcess()"
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
            'Else
            '    Session("dllist_selected_dlItem") = Nothing
            '    Session("dllist_countryuserctrl") = Nothing
            '    Session("GridViewCountryRow") = Nothing
            '    Session("dllist_selected_sender") = Nothing
            '    Session("dllist_selected_e") = Nothing
            '    Session("dllist_countryuserctrl_ClientID") = Nothing
            'End If

            Session("dllist_selected_dlItem") = Nothing
            Session("dllist_countryuserctrl") = Nothing
            Session("GridViewCountryRow") = Nothing
            Session("dllist_selected_sender") = Nothing
            Session("dllist_selected_e") = Nothing
            Session("dllist_countryuserctrl_ClientID") = Nothing
            Return True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return False
        End Try

    End Function

    Protected Sub btnHidProcessCheckBox_Country_List_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidProcessCheckBox_Country_List.Click
        fnCloseButtonClickAfterConfirmation()
    End Sub

    Protected Sub BtnDummyProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDummyProcess.Click
        'test
        'If Session("dllist_selected_sender") IsNot Nothing Then
        '    fnCloseButtonClick(Session("dllist_selected_sender"), Session("dllist_selected_e"), Session("dllist_countryuserctrl"))
        'End If
    End Sub

    Protected Sub btnHidProcessCheckBox_Country_All_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidProcessCheckBox_Country_All.Click
        Try

            If Left(hdCountryStatus.Value.Trim.ToUpper, 2) = "OK" Then
                'Unselect all the country
                For Each ctryRow As GridViewRow In gv_ShowCountries.Rows
                    Dim lblordertype As Label = CType(ctryRow.FindControl("lblordertype"), Label)
                    If lblordertype.Text <> "1" Then
                        Dim chkctry2 As CheckBox = CType(ctryRow.FindControl("chkctry2"), CheckBox)
                        chkctry2.Checked = False
                        sbCountrySelectAndSort(chkctry2, 0, "") 'changed by mohamed on 19/10/2016
                        ctryRow.CssClass = IIf(chkctry2.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") 'changed by mohamed on 30/10/2016
                    End If

                Next

                'commented / changed by mohamed on 30/10/2016
                'gv_ShowCountries.DataSource = ViewState("ctrydatatable_ctryusercontrol")
                'gv_ShowCountries.DataBind()
                'GrdCountryGrouping_OnDataBound()

                'Refresh Agent
                gv_showagents.DataSource = Nothing
                gv_showagents.DataBind()
            End If
            Session("skipSelectAllCountryChangeEvent") = Nothing
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub txtSearchAgent_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchAgent.TextChanged
        
    End Sub

    Protected Sub btnSearchAgentNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAgentNext.Click
        'If gv_showagents.Rows.Count > 0 Then
        '    If gv_showagents.SelectedIndex >= 0 Then
        '        sbFindAgentInGrid(gv_showagents.SelectedIndex)
        '    Else
        '        sbFindAgentInGrid(-1)
        '    End If
        'End If

        Try
            If fnFindAgentInGrid(-1) = True Then
                txtSearchAgent.Text = ""
            End If
        Catch ex As Exception
            txtSearchAgent.Text = ""
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Session("FindAgentInGrid_ctrycode") = Nothing
            Session("FindAgentInGrid_agentcode") = Nothing
        End Try
    End Sub

    Function fnFindAgentInGrid(ByVal lbStartingRow As Integer) As Boolean

        Session("FindAgentInGrid_ctrycode") = ""
        Session("FindAgentInGrid_agentcode") = ""
        If txtSearchAgent.Text.Trim <> "" Then
            If 1 = 2 Then ' CType(Session("CountrySelected_Sess"), DataTable).Rows.Count > 0 Then ' changed by mohamed on 04/10/2016, as this condition is not needed
                'If gv_showagents.Rows.Count > 0 Then
                For Each gvRow As GridViewRow In gv_showagents.Rows
                    Dim lblagentname As Label = gvRow.FindControl("lblagentname")
                    Dim chk2 As CheckBox = gvRow.FindControl("chk2")
                    If lblagentname IsNot Nothing Then
                        If Left(lblagentname.Text.Trim.ToUpper, txtSearchAgent.Text.Trim.Length) = txtSearchAgent.Text.Trim.ToUpper Then
                            chk2.Focus()
                            Return True
                        End If
                    End If
                Next
            Else
                If txtSearchAgentCtryCode.Text.Trim <> "" Then

                    Dim lsAgentCodeList As String = objUtils.GetString(Session("dbConnectionName"), "select ISNULL(stuff((select ',' + agentcode from agentmast (nolock) where agentname like '" & txtSearchAgent.Text.Trim & "' for xml path('')),1,1,''),'') agentcode")
                    Dim lsCtryName As String = objUtils.GetString(Session("dbConnectionName"), "select ctryname from ctrymast (nolock) where ctrycode='" & txtSearchAgentCtryCode.Text & "'")

                    Session("FindAgentInGrid_ctrycode") = txtSearchAgentCtryCode.Text.Trim
                    Session("FindAgentInGrid_agentcode") = lsAgentCodeList.Trim

                    If lsCtryName.Trim <> "" Then
                        Dim dsCtry As DataSet
                        If Session("countrygriddatasource_usercontrol") IsNot Nothing Then
                            dsCtry = Session("countrygriddatasource_usercontrol")
                            Dim dvCtry As DataView = New DataView(dsCtry.Tables(0))
                            dvCtry.RowFilter = "ctrycode='" & txtSearchAgentCtryCode.Text & "'"
                            If dvCtry.ToTable.Rows.Count = 0 Then
                                'not commented 'commented - changed by mohamed on 20/10/2016
                                sbAddToDataTable("COUNTRY", lsCtryName, "C")

                                Dim dlList As DataList = Me.Parent.FindControl("dllist")  'Session("dllist_countryuserctrl")
                                If dlList IsNot Nothing Then
                                    Dim dtDynamics As New DataTable
                                    dtDynamics = Session("sDtDynamic")
                                    dlList.DataSource = dtDynamics
                                    dlList.DataBind()
                                End If

                                sbShowCountry()
                            Else
                                sbCountrySelectAndSortAndAssign(Nothing, 1, txtSearchAgentCtryCode.Text)

                                'added / changed by mohamed on 30/10/2016
                                For Each gvRow As GridViewRow In gv_ShowCountries.Rows
                                    Dim txtctrycode As Label = gvRow.FindControl("txtctrycode")
                                    Dim chkCtry2 As CheckBox = gvRow.FindControl("chkCtry2")
                                    If txtctrycode IsNot Nothing Then
                                        If txtctrycode.Text.Trim.ToUpper = txtSearchAgentCtryCode.Text.Trim.ToUpper Then
                                            chkCtry2.Checked = True
                                            chkCtry2.Focus()

                                            gvRow.CssClass = IIf(chkCtry2.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") ' changed by mohamed on 30/10/2016

                                            Btnshowagents_Click(Nothing, Nothing)
                                            'For Each gvRowAg As GridViewRow In gv_showagents.Rows
                                            '    Dim lblagentname As Label = gvRowAg.FindControl("lblagentname")
                                            '    Dim chk2 As CheckBox = gvRowAg.FindControl("chk2")
                                            '    If lblagentname IsNot Nothing Then
                                            '        If lblagentname.Text.Trim.ToUpper = txtSearchAgent.Text.Trim.ToUpper Then
                                            '            chk2.Checked = True
                                            '            chk2.Focus()
                                            '            Return True
                                            '        End If
                                            '    End If
                                            'Next
                                            'Return True
                                        End If
                                    End If
                                Next

                                'Btnshowagents_Click(Nothing, Nothing)  'commented / changed by mohamed on 30/10/2016

                            End If
                        End If
                    End If


                    'For Each gvRow As GridViewRow In gv_ShowCountries.Rows
                    '    Dim txtctrycode As Label = gvRow.FindControl("txtctrycode")
                    '    Dim chkCtry2 As CheckBox = gvRow.FindControl("chkCtry2")
                    '    If txtctrycode IsNot Nothing Then
                    '        If txtctrycode.Text.Trim.ToUpper = txtSearchAgentCtryCode.Text.Trim.ToUpper Then
                    '            chkCtry2.Checked = True
                    '            chkCtry2.Focus()
                    '            Btnshowagents_Click(Nothing, Nothing)
                    '            For Each gvRowAg As GridViewRow In gv_showagents.Rows
                    '                Dim lblagentname As Label = gvRowAg.FindControl("lblagentname")
                    '                Dim chk2 As CheckBox = gvRowAg.FindControl("chk2")
                    '                If lblagentname IsNot Nothing Then
                    '                    If lblagentname.Text.Trim.ToUpper = txtSearchAgent.Text.Trim.ToUpper Then
                    '                        chk2.Checked = True
                    '                        chk2.Focus()
                    '                        Return True
                    '                    End If
                    '                End If
                    '            Next
                    '            Return True
                    '        End If
                    '    End If
                    'Next
                End If
            End If
        End If
        Return True
    End Function

    Public Function fnGetSelectedCountriesList() As String
        Dim lsCountryList As String = "", lsAvailableCtryCode As String = ""
        For Each gvRow As GridViewRow In gv_ShowCountries.Rows
            Dim txtctrycode As Label = gvRow.FindControl("txtctrycode")
            Dim chkCtry2 As CheckBox = gvRow.FindControl("chkCtry2")
            If txtctrycode IsNot Nothing Then
                If chkCtry2.Checked = True Then
                    lsCountryList = lsCountryList & IIf(lsCountryList.Trim = "", "", ",") & "'" & txtctrycode.Text & "'"
                End If

                'changed by mohamed on 03/10/2016
                lsAvailableCtryCode = lsAvailableCtryCode & IIf(lsAvailableCtryCode.Trim = "", "", ",") & "'" & txtctrycode.Text.Trim & "'"
            End If
        Next

        Session("SelectedCountriesList_WucCountryGroupUserControl") = lsCountryList
        Session("AllCountriesList_WucCountryGroupUserControl") = lsAvailableCtryCode
        Return lsCountryList
    End Function

    Protected Sub gv_ShowCountries_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_ShowCountries.PreRender
        If gv_ShowCountries.Rows.Count > 0 Then
            Dim gRow As GridViewRow = gv_ShowCountries.Rows(gv_ShowCountries.Rows.Count - 1)
            Dim imgExpand As ImageButton = CType(gRow.FindControl("imgExpand"), ImageButton)
            Dim lblOrderType As Label = CType(gRow.FindControl("lblOrderType"), Label)
            If lblOrderType.Text = 1 Then
                gRow.Style.Add("display", "none")
            End If
        End If
    End Sub

    Protected Sub gv_ShowCountries_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_ShowCountries.RowCommand
        If e.CommandName = "cmdShowAgent" Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim lImgBtn As ImageButton = CType(row.FindControl("lnkShowAgent"), ImageButton)
            lImgBtn.Focus()
            Dim lsAvailableAgentInAgentGrid As String = ""
            Dim LsCtryCode As String
            LsCtryCode = e.CommandArgument
            If LsCtryCode.Trim <> "" Then

                Dim lclsagent As List(Of MyClasses.CountryAgentModel) = ViewState("agentdatatable_ctryusercontrol")
                If lclsagent IsNot Nothing Then
                    Dim lclsagentSelected = From xrow In lclsagent
                                            Where xrow.chkselect = 1 And xrow.ctrycode = LsCtryCode
                                            Select xrow
                    For Each lrow As MyClasses.CountryAgentModel In lclsagentSelected
                        lsAvailableAgentInAgentGrid += IIf(lsAvailableAgentInAgentGrid.Trim = "", "", ",") & lrow.agentcode
                    Next
                End If

                Dim lsSqlQry As String
                lsSqlQry = "select case when sel.mktcode is null then 0 else 1 end chkselect, a.agentcode, a.agentname, a.ctrycode from agentmast a (nolock) "
                lsSqlQry += " left join dbo.splitallotmkt('" & lsAvailableAgentInAgentGrid & "',',') sel on sel.mktcode=a.agentcode "
                lsSqlQry += "    where a.ctrycode='" & LsCtryCode & "' and isnull(a.active,0)=1"
                lsSqlQry += " order by case when sel.mktcode is null then 0 else 1 end desc, agentname "

                Dim dsAgent As DataSet
                dsAgent = objUtils.ExecuteQuerySqlnew(Session("dbconnectionname"), lsSqlQry)
                If dsAgent.Tables(0).Rows.Count > 0 Then
                    gvShowAgentPopup.DataSource = dsAgent.Tables(0)
                    gvShowAgentPopup.DataBind()
                    ModalPopupShowAgent.Show()
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Agent is available for this country' );", True)
                End If
            End If
        End If
    End Sub



    Protected Sub gv_ShowCountries_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_ShowCountries.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            Dim dtRow As DataRow = objUtils.fnGridViewObjectToDataRow(e.Row) ' objUtils.fnGridViewRowToDataRow(e.Row)
            chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim imgExpand As ImageButton = CType(e.Row.FindControl("imgExpand"), ImageButton)
            imgExpand.CssClass = "ctryhideallgrid"

            If dtRow("ordertype") = 1 Then
                imgExpand.Style.Add("display", "block")
                chkCtry2.Style.Add("display", "none")

                If e.Row.RowIndex = gv_ShowCountries.Rows.Count - 1 Then
                    imgExpand.Style.Add("display", "none")
                End If

            Else
                e.Row.CssClass = IIf(dtRow("chkselect") = 1, "ctryselectedcls_row", "ctrynotselectedclss_row")
            End If



            Dim lblctrynameDisplay As Label = e.Row.FindControl("lblctrynameDisplay")
            Dim lblcountrygroupnameDisplay As Label = e.Row.FindControl("lblcountrygroupnameDisplay")
            Dim lblplgrpnameDisplay As Label = e.Row.FindControl("lblplgrpnameDisplay")
            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCtryGrp As String = ""
            Dim lsSearchTextCustomerGrpGrp As String = ""
            Dim lsSearchTextRegion As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCtry = ""
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "COUNTRYGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtryGrp = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "CUSTOMERGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCustomerGrpGrp = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "REGION" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextRegion = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCtryGrp = lsSearchTextCtry
                            lsSearchTextRegion = lsSearchTextCtry
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblctrynameDisplay.Text = Regex.Replace(lblctrynameDisplay.Text, lsSearchTextCtry.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCtryGrp.Trim <> "" Then
                            lblcountrygroupnameDisplay.Text = Regex.Replace(lblcountrygroupnameDisplay.Text, lsSearchTextCtryGrp.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        'Handle this in the agent grid
                        'If lsSearchTextCustomerGrpGrp.Trim <> "" Then
                        '    lblcountrygroupnameDisplay.Text = Regex.Replace(lblcountrygroupnameDisplay.Text, lsSearchTextCustomerGrpGrp.Trim(), _
                        '        Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                        '                    RegexOptions.IgnoreCase)
                        'End If
                        If lsSearchTextRegion.Trim <> "" Then
                            lblplgrpnameDisplay.Text = Regex.Replace(lblplgrpnameDisplay.Text, lsSearchTextRegion.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If
    End Sub

    Protected Sub gv_showagents_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_showagents.PreRender
        If gv_showagents.Rows.Count > 0 Then
            Dim gRow As GridViewRow = gv_showagents.Rows(gv_showagents.Rows.Count - 1)
            Dim imgExpand As ImageButton = CType(gRow.FindControl("imgExpand"), ImageButton)
            Dim lblOrderType As Label = CType(gRow.FindControl("lblOrderType"), Label)
            If lblOrderType.Text = 1 Then
                gRow.Style.Add("display", "none")
            End If
        End If
    End Sub

    Protected Sub gv_showagents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_showagents.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim chkCtry2 As CheckBox = e.Row.FindControl("chk2")
            Dim dtRow As DataRow = objUtils.fnGridViewObjectToDataRow(e.Row) ' objUtils.fnGridViewRowToDataRow(e.Row)
            chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim imgExpand As ImageButton = CType(e.Row.FindControl("imgExpand"), ImageButton)
            imgExpand.CssClass = "agenthideallgrid"

            If dtRow("ordertype") = 1 Then
                imgExpand.Style.Add("display", "block")
                chkCtry2.Style.Add("display", "none")


            Else
                e.Row.CssClass = IIf(dtRow("chkselect") = 1, "agentselectedcls_row", "agentnotselectedclss_row")
            End If

            'Dim txtagentcode As Label
            'txtagentcode = e.Row.FindControl("txtagentcode")

            'Dim dtAgent As DataTable
            'Dim dvAgent As New DataView
            'dtAgent = Session("AgentSelected_Sess")
            'If Session("AgentSelected_Sess") IsNot Nothing Then
            '    dvAgent = New DataView(dtAgent)
            '    dvAgent.RowFilter = "trim(agentcode)='" & txtagentcode.Text.Trim & "'"
            '    If dvAgent.ToTable.Rows.Count > 0 Then
            '        chkCtry2.Checked = True
            '    End If
            'End If

            'Changed by Mohamed on 13/08/2018
            'Dim lblCustomerGroupName As Label = e.Row.FindControl("lblCustomerGroupName")
            Dim lblCustomerGroupNameDisplay As Label = e.Row.FindControl("lblCustomerGroupNameDisplay")
            Dim lsSearchTextCustomerGrpGrp As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCustomerGrpGrp = ""
                        If "CUSTOMERGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCustomerGrpGrp = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        'If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lsSearchTextCustomerGrpGrp = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        'End If

                        If lsSearchTextCustomerGrpGrp.Trim <> "" Then
                            lblCustomerGroupNameDisplay.Text = Regex.Replace(lblCustomerGroupNameDisplay.Text, lsSearchTextCustomerGrpGrp.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Protected Sub GrdAgentGrouping_OnDataBound()
        For i As Integer = gv_showagents.Rows.Count - 1 To 1 Step -1
            Dim row As GridViewRow = gv_showagents.Rows(i)
            Dim previousRow As GridViewRow = gv_showagents.Rows(i - 1)
            Dim J As Integer = 0
            If row.Cells(J).Text = previousRow.Cells(J).Text Then
                If previousRow.Cells(J).RowSpan = 0 Then
                    If row.Cells(J).RowSpan = 0 Then
                        previousRow.Cells(J).RowSpan += 2
                    Else
                        previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                    End If
                    row.Cells(J).Visible = False
                End If
            End If
        Next
    End Sub

    Protected Sub GrdCountryGrouping_OnDataBound()
        'For i As Integer = gv_ShowCountries.Rows.Count - 1 To 1 Step -1
        '    Dim row As GridViewRow = gv_ShowCountries.Rows(i)
        '    Dim previousRow As GridViewRow = gv_ShowCountries.Rows(i - 1)
        '    Dim J As Integer = 1
        '    If CType(row.FindControl("chkCtry2"), CheckBox).Checked = CType(previousRow.FindControl("chkCtry2"), CheckBox).Checked Then
        '        If previousRow.Cells(J).RowSpan = 0 Then
        '            If row.Cells(J).RowSpan = 0 Then
        '                previousRow.Cells(J).RowSpan += 2
        '            Else
        '                previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
        '            End If
        '            row.Cells(J).Visible = False
        '        End If
        '    End If
        'Next
    End Sub

    Protected Sub btnPopupShowCountrySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPopupShowCountrySave.Click
        Dim strFlag As Integer = 0
        Try
            Session("FindAgentInGrid_ctrycode") = ""
            Session("FindAgentInGrid_ctrycode_notselected") = ""

            Dim lsNotSelectedAgents As String = ""
            Dim lsSelectedCtryCode As String = ""

            For Each lgvRow As GridViewRow In gvShowCountryPopup.Rows
                Dim chkselcountry As CheckBox = lgvRow.FindControl("chkselcountry")
                Dim lblctrycode As Label = lgvRow.FindControl("lblctrycode")
                If chkselcountry IsNot Nothing Then
                    If chkselcountry.Checked = True Then
                        lsSelectedCtryCode += IIf(lsSelectedCtryCode.Trim = "", "", ",") & lblctrycode.Text
                    Else
                        lsNotSelectedAgents += IIf(lsNotSelectedAgents.Trim = "", "", ",") & lblctrycode.Text
                    End If
                End If
            Next

            If lsSelectedCtryCode.Trim <> "" Or lsNotSelectedAgents.Trim <> "" Then
                Session("FindAgentInGrid_ctrycode") = lsSelectedCtryCode.Trim
                Session("FindAgentInGrid_ctrycode_notselected") = lsNotSelectedAgents.Trim
            End If

            For Each lgvRowAgn As GridViewRow In gv_showagents.Rows
                Dim lblctrycode As Label = lgvRowAgn.FindControl("lblCountryCode")
                If lblctrycode IsNot Nothing Then
                    If ("," & lsNotSelectedAgents & ",").Trim.ToUpper.Contains("," & lblctrycode.Text.ToUpper.Trim & ",") = True Then
                        strFlag = 1
                        Dim strScript As String = "javascript:fnConfirmCountryListDataList()"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strScript, True)
                        Exit For
                    End If
                End If
            Next

            If strFlag = 0 Then
                sbShowCountry()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If strFlag = 0 Then
                Session("FindAgentInGrid_ctrycode") = Nothing
                Session("FindAgentInGrid_ctrycode_notselected") = Nothing
            End If
        End Try
    End Sub

    Protected Sub btnHidProcess_Country_DataList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHidProcess_Country_DataList.Click
        Try
            If Left(hdCountryStatus.Value.Trim.ToUpper, 2) = "OK" Then
                sbShowCountry()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Session("FindAgentInGrid_ctrycode") = Nothing
            Session("FindAgentInGrid_ctrycode_notselected") = Nothing
        End Try
    End Sub

    Protected Sub btnPopupShowAgentSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPopupShowAgentSave.Click
        Try
            'First Add all the country

            Session("FindAgentInGrid_ctrycode") = ""
            Session("FindAgentInGrid_agentcode") = ""
            Session("FindAgentInGrid_agentcode_notselected") = ""

            Dim lsSelectedAgents As String = ""
            Dim lsNotSelectedAgents As String = ""
            Dim lsSelectedCtryCodeAll As String = ""
            Dim lsSelectedCtryCode As String = ""
            Dim llistNotSelectedCtryCode As New List(Of String)
            Dim llistSelectedCtryCode As New List(Of String)
            Dim lctrygvRow As GridViewRow
            Dim chkCtry2 As CheckBox

            For Each lgvRow As GridViewRow In gvShowAgentPopup.Rows
                Dim chkselagent As CheckBox = lgvRow.FindControl("chkselagent")
                Dim lblagentcode As Label = lgvRow.FindControl("lblagentcode")
                Dim lblctrycode As Label = lgvRow.FindControl("lblctrycode")
                If chkselagent IsNot Nothing Then
                    lsSelectedCtryCode = lblctrycode.Text
                    If chkselagent.Checked = True Then
                        lsSelectedAgents += IIf(lsSelectedAgents.Trim = "", "", ",") & lblagentcode.Text
                        lsSelectedCtryCodeAll += IIf(lsSelectedCtryCodeAll.Trim = "", "", ",") & lblctrycode.Text
                        'If listSelectedCtryCode.IndexOf(lblctrycode.Text) < 0 Then 'changed by mohamed on 14/08/2018
                        '    listSelectedCtryCode.Add(lblctrycode.Text)
                        'End If
                        If llistNotSelectedCtryCode.IndexOf(lblctrycode.Text) >= 0 Then
                            llistNotSelectedCtryCode.Remove(lblctrycode.Text)
                        End If
                        If llistSelectedCtryCode.IndexOf(lblctrycode.Text) < 0 Then
                            llistSelectedCtryCode.Add(lblctrycode.Text)
                        End If
                    Else
                        lsNotSelectedAgents += IIf(lsNotSelectedAgents.Trim = "", "", ",") & lblagentcode.Text
                        If llistNotSelectedCtryCode.IndexOf(lblctrycode.Text) < 0 And llistSelectedCtryCode.IndexOf(lblctrycode.Text) < 0 Then
                            llistNotSelectedCtryCode.Add(lblctrycode.Text)
                        End If
                    End If
                End If
            Next
            Dim lsNotSelectedCtryCodeAll As String = ""
            For Each lCtryCode As String In llistNotSelectedCtryCode
                lsNotSelectedCtryCodeAll += IIf(lsNotSelectedCtryCodeAll.Trim = "", "", ",") & lCtryCode.ToString
            Next

            If lsSelectedCtryCodeAll.Trim <> "" Or lsNotSelectedCtryCodeAll.Trim <> "" Then
                Session("FindAgentInGrid_ctrycode") = lsSelectedCtryCodeAll.Trim
                Session("FindAgentInGrid_ctrycode_notselected") = lsNotSelectedCtryCodeAll.Trim

                sbShowCountry()
            End If

            If lsSelectedAgents.Trim <> "" Or lsNotSelectedAgents.Trim <> "" Then
                Session("FindAgentInGrid_ctrycode") = lsSelectedCtryCode
                Session("FindAgentInGrid_agentcode") = lsSelectedAgents.Trim
                Session("FindAgentInGrid_agentcode_notselected") = lsNotSelectedAgents.Trim
            End If

            'For Each lctrygvRow In gv_ShowCountries.Rows
            '    chkCtry2 = lctrygvRow.FindControl("chkCtry2")
            '    Dim lblctrycode As Label = lctrygvRow.FindControl("txtctrycode")
            '    If listSelectedCtryCode.IndexOf(lblctrycode.Text.Trim) >= 0 Then
            '        lsSelectedCtryCode = lblctrycode.Text.Trim
            '        'If lblctrycode.Text.Trim.ToLower = lsSelectedCtryCode.Trim.ToLower Then 'changed by mohamed on 14/08/2018
            '        Session("FindAgentInGrid_ctrycode") = lsSelectedCtryCode 'changed by mohamed on 14/08/2018
            '        If lsSelectedAgents.Trim <> "" Then
            '            chkCtry2.Checked = True
            '            sbCountrySelectAndSort(chkCtry2, 1, "") 'changed by mohamed on 19/10/2016
            '            lctrygvRow.CssClass = IIf(chkCtry2.Checked = True, "ctryselectedcls_row", "ctrynotselectedclss_row") 'changed by mohamed on 30/10/2016
            '        End If
            '        listSelectedCtryCode.Remove(lblctrycode.Text.Trim)
            '        'Exit For
            '        'End If
            '    End If

            'Next

            ''changed by mohamed on 14/08/2018
            'If listSelectedCtryCode.Count > 0 Then
            '    Dim lst As List(Of MyClasses.CountryModel) = ViewState("ctrydatatable_ctryusercontrol")
            '    Dim lsNotAvailCtryInCtryGrid As String = ""
            '    Dim lstItem As New MyClasses.CountryModel

            '    Dim lclsagentSelected = From xrow In listSelectedCtryCode
            '            Select xrow
            '    For Each lrow In lclsagentSelected
            '        lsNotAvailCtryInCtryGrid += IIf(lsNotAvailCtryInCtryGrid.Trim = "", "", ",") & lrow.ToString
            '    Next

            '    Dim dsAgent As DataSet, lsSqlQry As String = ""
            '    lsSqlQry = ";with cte as ("
            '    lsSqlQry += " select distinct c.ctrycode, c.ctryname, p.plgrpname, g.countrygroupname, g.countrygroupcode, p.plgrpcode "
            '    lsSqlQry += "	from ctrymast c (nolock) "
            '    lsSqlQry += "	join plgrpmast p (nolock) on p.plgrpcode=c.plgrpcode"
            '    lsSqlQry += "	left join countrygroup_detail gd (nolock) on gd.ctrycode=c.ctrycode"
            '    lsSqlQry += "	left join countrygroup g (nolock) on gd.countrygroupcode=g.countrygroupcode"
            '    lsSqlQry += " where c.ctrycode in ('" & Replace(lsNotAvailCtryInCtryGrid.Trim, ",", "','") & "')"

            '    dsAgent = objUtils.ExecuteQuerySqlnew(Session("dbconnectionname"), lsSqlQry)
            '    If dsAgent.Tables(0).Rows.Count > 0 Then
            '        lstItem = New MyClasses.CountryModel
            '        For Each lctry As DataRow In dsAgent.Tables(0).Rows
            '            With lstItem
            '                .chkselect = 1
            '                .ctrycode = lctry("ctrycode")
            '                .ctryname = lctry("ctryname")
            '                .plgrpcode = lctry("plgrpcode")
            '                .plgrpname = lctry("plgrpname")
            '                .countrygroupcode = lctry("countrygroupcode")
            '                .countrygroupname = lctry("countrygroupname")
            '                .ordertype = lctry("ordertype")
            '            End With
            '            lst.Add(lstItem)
            '        Next

            '        ViewState("ctrydatatable_ctryusercontrol") = lst
            '        gv_ShowCountries.DataSource = lst
            '        gv_ShowCountries.DataBind()
            '    End If
            'End If

            Btnshowagents_Click(Nothing, Nothing)

            If chkCtry2 IsNot Nothing Then
                chkCtry2.Focus()
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Session("FindAgentInGrid_ctrycode") = Nothing
            Session("FindAgentInGrid_agentcode") = Nothing
            Session("FindAgentInGrid_ctrycode_notselected") = Nothing
        End Try
    End Sub

    Protected Sub gvShowAgentPopup_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShowAgentPopup.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkselagent As CheckBox = e.Row.FindControl("chkselagent")
            Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            chkselagent.Checked = IIf(dtRow("chkselect") = 1, True, False)
        End If
    End Sub

    Protected Sub gvShowCountryPopup_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShowCountryPopup.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chkselagent As CheckBox = e.Row.FindControl("chkselcountry")
            Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            chkselagent.Checked = IIf(dtRow("chkselect") = 1, True, False)
        End If
    End Sub

    Sub sbSetPageState(ByVal asTranId As String, Optional ByVal asPageName As String = Nothing, Optional ByVal asPageMode As String = Nothing)
        If asPageMode IsNot Nothing Then
            hdnPageMode.Value = asPageMode

            If hdnPageMode.Value.Trim.ToUpper <> "NEW" Then 'If CType(Session("ContractState"), String) <> "New" Then 'changed by mohamed on 05/11/2016
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
            End If
        End If
        If asPageName IsNot Nothing Then
            hdnPageName.Value = asPageName
        End If

        If asTranId Is Nothing Then
            hdnTranId.Value = ""
        Else
            hdnTranId.Value = asTranId
        End If
    End Sub



End Class
