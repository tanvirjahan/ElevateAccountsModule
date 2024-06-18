
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractChkInOutPolicy
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
    Private cnt As Long
    Private arr(1) As String
    Private arrRName(1) As String


    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Private dt As New DataTable


    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim txtrmtypcodenew As New ArrayList
    Dim txtrmtypnamenew As New ArrayList
    Dim txtmealcodenew As New ArrayList

    Dim txtnoofdaysnew As New ArrayList
    Dim txtunitsnew As New ArrayList
    Dim txtchargenew As New ArrayList
    Dim txtperchargenew As New ArrayList
    Dim txtvaluenew As New ArrayList
    Dim txtnightsnew As New ArrayList
    Dim txtdaysnew As New ArrayList

    Dim CopyRownoshow As Integer = 0
    Dim CopyClicknoshow As Integer = 0


    Dim txtrmtypcodenoshownew As New ArrayList
    Dim txtrmtypnamenoshownew As New ArrayList
    Dim txtmealcodenoshownew As New ArrayList
    Dim ddlnoshow1new As New ArrayList
    Dim txtchargenoshownew As New ArrayList
    Dim txtpercnoshownew As New ArrayList
    Dim txtvaluenoshownew As New ArrayList
    Dim txtnightsnoshownew As New ArrayList





#End Region
#Region "Enum GridCol"
    Enum GridCol

        tranid = 1
        Fromdate = 2
        Todate = 3
        applicableto = 4
        Edit = 7
        View = 8
        Delete = 9
        Copy = 10
        DateCreated = 11
        UserCreated = 12
        DateModified = 13
        UserModified = 14


    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = False Then
            ' Session("GV_HotelData") = Nothing


            ' NumbersHtml(txtminnights)       'accept only number




        Else

        End If
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
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        ' FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)
        Dim CalledfromValue As String = ""

        Dim checkioappid As String = ""
        Dim checkioappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        If IsPostBack = False Then
            checkioappid = 1
            checkioappname = objUser.GetAppName(Session("dbconnectionName"), checkioappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(checkioappname, String), "ContractChkInOutPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(checkioappname, String), "ContractChkInOutPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractChkInOutPolicy.aspx", checkioappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, checkioappid, intMenuID)

            If functionalrights <> "" Then
                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "07" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnselect.Visible = False
                    If Count = 1 Then
                        btncopycontract.Visible = True
                    Else
                        btncopycontract.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    'btncopycontract.Visible = False
                    If Count = 1 Then
                        btnselect.Visible = True
                        btncopycontract.Visible = True
                    Else
                        btnselect.Visible = False
                        btncopycontract.Visible = False
                    End If
                End If

            Else

                btnselect.Visible = False
                btncopycontract.Visible = False

            End If

            If Session("Calledfrom") = "Offers" Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                txtconnection.Value = Session("dbconnectionName")

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Session("partycode") = hdnpartycode.Value
                divoffer.Style.Add("display", "block")
                btnselect.Style.Add("display", "block")
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                gv_SearchResult.Columns(4).Visible = False
                gv_SearchResult.Columns(5).Visible = False
                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionid.Text = Session("OfferRefCode")
                    Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,commissiontype  from view_offers_header (nolock) where  promotionid='" & hdnpromotionid.Value & "'")
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                        txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                        hdncommtype.Value = ds.Tables(0).Rows(0).Item("commissiontype")
                    End If

                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                        hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                    End If


                End If




                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))


                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdnpromotionid.Value
                Page.Title = "CHECKIN OUT POLICY"

                Page.Title = "Promotion CheckInOut Policy "
                FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1

                fillDategrd(grdexdates, True)

                FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                txtconnection.Value = Session("dbconnectionName")
                'txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                'txtToDate.Text = Now.ToString("dd/MM/yyyy")


                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Session("partycode") = hdnpartycode.Value

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                '  wucCountrygroup.Visible = False
                divoffer.Style.Add("display", "none")
                btnselect.Style.Add("display", "none")
                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True
                gv_SearchResult.Columns(5).Visible = True

                wucCountrygroup.sbSetPageState("", "CONTRACTCHECKINOUT", CType(Session("ContractState"), String))

                '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "CHECKIN OUT POLICY"

                ' btnreset_Click(Nothing, Nothing)
                fillDategrd(grdexdates, True)

                FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")
            End If


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
            chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

            txtchkintime.Attributes.Add("onchange", "chktime('" & txtchkintime.ClientID & "','" + CType("1", String) + "')")
            txtChkouttime.Attributes.Add("onchange", "chktime('" & txtChkouttime.ClientID & "','" + CType("1", String) + "')")

            If Session("Calledfrom") <> "Offers" Then
                btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
                btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            End If

            ' Enablegrid()
            hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")

            Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function getfillunits(ByVal prefixText As String) As List(Of String)
        Dim unitlist As New List(Of String)

        Try
            unitlist.Add("% of Nights")
            unitlist.Add("Value")
            unitlist.Add("Rate TBA")


            Return unitlist
        Catch ex As Exception
            Return unitlist
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getfillchargetypenoshow(ByVal prefixText As String) As List(Of String)
        Dim chargelist As New List(Of String)

        Try
            chargelist.Add("Nights")
            chargelist.Add("% of Nights")
            chargelist.Add("% of Entire Stay")
            chargelist.Add("Value")
            chargelist.Add("% of remaining Nights")


            Return chargelist
        Catch ex As Exception
            Return chargelist
        End Try

    End Function
#Region "Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 5
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True

    End Sub
#End Region
#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    Sub FillRoomdetails()
        createdatatable()
        createdatarows()
        grdRoomrates.Visible = True

        lable12.Visible = True
        btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False




    End Sub
    Protected Sub btnClearPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtViewPolicy.Value = ""

    End Sub
    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    '   strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
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
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            grdpromotion.Visible = True


            Dim MyDs As New DataTable
            Dim countryList As String = ""
            Dim agentList As String = ""
            Dim filterCond As String = ""
            If wucCountrygroup.checkcountrylist.ToString().Trim <> "" Then
                countryList = wucCountrygroup.checkcountrylist.ToString().Trim.Replace(",", "','")
                filterCond = "h.promotionid  in (select promotionid from view_offers_countries where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "h.promotionid  in (select promotionid from view_offers_agents where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            filterCond = ""
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = " select c.checkinoutpolicyid plistcode, h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_checkinout_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by h.promotionid,h.approved,h.promotionname,c.checkinoutpolicyid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                grdpromotion.DataSource = MyDs
                grdpromotion.DataBind()
                grdpromotion.Visible = True
            Else
                grdpromotion.Visible = False
            End If

            ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub

    Private Sub createdatatable()
        Try

            cnt = 0
            Session("GV_HotelData") = Nothing
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
            strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                      & hdnpartycode.Value & "' "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(i) = mySqlReader("rmcatcode")
                i = i + 1
            End While
            mySqlReader.Close()
            mySqlConn.Close()
            'select rmcatcode from partyrmcat where partycode='3'
            'Here in Array store room types
            '-------------------------------------
            Dim tf As New TemplateField
            dt = New DataTable

            dt.Columns.Add(New DataColumn("Room_Type", GetType(String)))
            dt.Columns.Add(New DataColumn("Room Type Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Meal Plan", GetType(String)))
            dt.Columns.Add(New DataColumn("Price Pax", GetType(String)))


            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next
            dt.Columns.Add(New DataColumn("Unityesno", GetType(String)))
            dt.Columns.Add(New DataColumn("Noofextraperson", GetType(String)))
            dt.Columns.Add(New DataColumn("Extra Person Supplement", GetType(String)))
            dt.Columns.Add(New DataColumn("No.of.Nights Room Rate", GetType(String)))
            dt.Columns.Add(New DataColumn("Min Nights", GetType(String)))

            Session("GV_HotelData") = dt


            ' If dt.Columns.Count >= 7 Then Exit Sub
            'fill controls from previous form
            ' Now  Bind Column Dynamically 
            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" And col.ColumnName <> "Extra Person Supplement" And col.ColumnName <> "No.of.Nights Room Rate" And col.ColumnName <> "Min Nights" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
                    grdRoomrates.Columns.Add(bfield)


                End If
            Next
            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()


        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim count As Integer
        Dim GVRow As GridViewRow

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        count = gvchildage.Rows.Count + 1

        'count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Try
            For Each GVRow In grdpromodates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
                n = n + 1
            Next
            fillDategrd(grdpromodates, False, grdpromodates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromodates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                'lblseason = GVRow.FindControl("lblseason")
                'lblseason.Text = excl(n)

                n = n + 1
            Next



            Dim txtStayFromDt As TextBox
            txtStayFromDt = TryCast(grdpromodates.Rows(grdpromodates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)


        Dim count As Integer

        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        count = gvchildage.Rows.Count + 1

        Dim GVRow As GridViewRow
        '  count = grdDates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim lblseason As Label

        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdpromodates.Rows
                '  chkSelect = GVRow.FindControl("chkSelect")
                'If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

                k = k + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdpromodates.Rows.Count > 1 Then
                fillDategrd(grdpromodates, False, grdpromodates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdpromodates, False, grdpromodates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromodates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)
                    'lblseason = GVRow.FindControl("lblseason")
                    'lblseason.Text = excl(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try


    End Sub
    Protected Sub ReadMoreLinkButtonpromotion_Click(ByVal sender As Object, ByVal e As EventArgs)
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
            ModalExtraPopup1.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getseasonlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim seasonlist As New List(Of String)
        Dim maxstate As String
        Try
            ' contextKey = Convert.ToString(HttpContext.Current.Session("partycode").ToString())
            maxstate = Convert.ToString(HttpContext.Current.Session("contractid").ToString())


            strSqlQry = "select distinct seasonname  from view_contractseasons_rate(nolock) where contractid='" & maxstate & "' and seasonname like '" & Trim(prefixText) & "%' order by seasonname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    seasonlist.Add(myDS.Tables(0).Rows(i)("seasonname").ToString())
                Next

            End If

            Return seasonlist
        Catch ex As Exception
            Return seasonlist
        End Try

    End Function


#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region
    Private Function BuildCondition() As String
        strWhereCond = ""
        'If txtSupplierCode.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " upper(partymaxacc_header.partycode) LIKE '" & Trim(txtSupplierCode.Text.Trim.ToUpper) & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(partymaxacc_header.partycode) LIKE '" & Trim(txtSupplierCode.Text.Trim.ToUpper) & "%'"
        '    End If
        'End If

        'If txtSupplierName.Text.Trim <> "" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " upper(partymast.partyname) LIKE '" & Trim(txtSupplierName.Text.Trim.ToUpper) & "%'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(partymast.partyname) LIKE '" & Trim(txtSupplierName.Text.Trim.ToUpper) & "%'"
        '    End If
        'End If
        'If ddlSupplierType.Value.Trim <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " partymaxacc_header.sptypecode= '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND partymaxacc_header.sptypecode= '" & Trim(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text) & "'"
        '    End If
        'End If
        'If ddlSupplierTypeName.Value.Trim <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then
        '        strWhereCond = " sptypemast.sptypename = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND sptypemast.sptypename = '" & Trim(ddlSupplierTypeName.Items(ddlSupplierTypeName.SelectedIndex).Text) & "'"
        '    End If
        'End If
        BuildCondition = strWhereCond
    End Function





    Private Sub FillGrid(ByVal StrSortBy As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            If Session("Calledfrom") = "Offers" Then
                If StrSortBy = "frmdate" Or StrSortBy = "todate" Then

                    strSqlQry = " With ctee as (select h.checkinoutpolicyid tranid,h.promotionid,o.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate," & _
                     " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_checkinout_header h(nolock) join view_offers_header o(nolock) on h.promotionid =o.promotionid  " _
                     & " join view_contracts_checkinout_offerdates d on h.checkinoutpolicyid =d.checkinoutpolicyid  where  h.promotionid='" & contractid & "' group by h.checkinoutpolicyid,h.promotionid,o.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) " & _
                        "select * from ctee order by convert(datetime," & StrSortBy & " ,103) " & strsortorder & ""
                Else
                    strSqlQry = "select h.checkinoutpolicyid tranid,h.promotionid,o.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate," & _
                     " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_checkinout_header h(nolock)  join view_offers_header o(nolock) on h.promotionid =o.promotionid  " _
                     & " join view_contracts_checkinout_offerdates d on h.checkinoutpolicyid =d.checkinoutpolicyid   where  h.promotionid='" & contractid & "' group by h.checkinoutpolicyid,h.promotionid,o.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser Order by " & StrSortBy & " " & strsortorder & " "

                End If
            Else
                If StrSortBy = "frmdate" Or StrSortBy = "todate" Then

                    strSqlQry = " With ctee as (select h.checkinoutpolicyid tranid,'' promotionid, '' promotionname ,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') frmdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                     " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_checkinout_header h(nolock)  where  h.contractid='" & contractid & "') " & _
                        "select * from ctee order by convert(datetime," & StrSortBy & " ,103) " & strsortorder & ""
                Else
                    strSqlQry = "select h.checkinoutpolicyid tranid,'' promotionid, '' promotionname ,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') frmdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                     " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_checkinout_header h(nolock)  where  h.contractid='" & contractid & "' Order by " & StrSortBy & " " & strsortorder & " "

                End If
            End If

            ' strSqlQry = "select '' tranid,'' frmdate,'' todate,'' countrygroups,'' adddate, '' adduser, '' moddate, '' moduser"


            'strSqlQry = " select plistcode,subseasoncode as seasoncode, from cplisthnew where contractid='" & contractid & "'"
            'strSqlQry = "select '' plistcode,'' seasoncode,'' frmdate,'' todate,'' countrygroups,'' DaysoftheWeek,'' adddate, '' adduser, '' moddate, '' moduser"




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
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Private Function OfferValidateSave() As Boolean
        Dim gvRow As GridViewRow


        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable Should not be Blank');", True)
            OfferValidateSave = False
            Exit Function
        End If

        Dim tickedornot As Boolean = False
        Dim chkSelect As CheckBox
        tickedornot = False


        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Session("seasons") = Nothing


        Dim tickedornotrmtype As Boolean = False
        Dim chkrmtyp As CheckBox
        tickedornotrmtype = False
        For Each grdRow In grdroomtype.Rows
            chkrmtyp = CType(grdRow.FindControl("chkrmtyp"), CheckBox)

            If chkrmtyp.Checked = True Then
                tickedornotrmtype = True
                Exit For
            End If
        Next

        If tickedornotrmtype = False Then
            Enablegrid()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Room type');", True)
            OfferValidateSave = False
            Exit Function
        End If

        Dim rmtypname As String = ""

        Dim txtrmtypcode As Label
        Session("Roomtypes") = Nothing

        For Each grdRow As GridViewRow In grdroomtype.Rows
            chk2 = grdRow.FindControl("chkrmtyp")
            txtrmtypcode = grdRow.FindControl("txtrmtypcode")

            If chk2.Checked = True Then
                rmtypname = rmtypname + txtrmtypcode.Text + ","

            End If
        Next

        If rmtypname.Length > 0 Then
            rmtypname = rmtypname.Substring(0, rmtypname.Length - 1)
        End If

        Session("Roomtypes") = rmtypname

        Dim tickedornotmeal As Boolean = False
        Dim chkmeal As CheckBox
        tickedornotmeal = False
        For Each grdRow In grdmealplan.Rows
            chkmeal = CType(grdRow.FindControl("chkmeal"), CheckBox)

            If chkmeal.Checked = True Then
                tickedornotmeal = True
                Exit For
            End If
        Next

        If tickedornotmeal = False Then
            Enablegrid()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Meal Plan');", True)
            OfferValidateSave = False
            Exit Function
        End If

        Dim mealplan As String = ""

        Dim txtmealcode As Label
        Session("mealplans") = Nothing

        For Each grdRow As GridViewRow In grdmealplan.Rows
            chk2 = grdRow.FindControl("chkmeal")
            txtmealcode = grdRow.FindControl("txtmealcode")

            If chk2.Checked = True Then
                mealplan = mealplan + txtmealcode.Text + ","

            End If
        Next

        If mealplan.Length > 0 Then
            mealplan = mealplan.Substring(0, mealplan.Length - 1)
        End If

        Session("mealplans") = mealplan

        Dim flg As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")

            If txtfrom.Text <> "" And txtto.Text <> "" Then
                flg = True
                Exit For
            End If

        Next

        'If flg = False Then
        '    Enablegrid()
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter One Row');", True)
        '    OfferValidateSave = False
        '    Exit Function
        'End If



        Dim lnCnt As Integer = 0
        'Dim lnCntnoshow As Integer = 0

        'Dim ToDt As Date = Nothing


        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1

            Dim ddlselect As HtmlSelect = gvRow.FindControl("ddlselect")
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")
            Dim ddlcharge As HtmlSelect = gvRow.FindControl("ddlcharge")
            Dim txtchargetype As TextBox = gvRow.FindControl("txtchargetype")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim ddlconditions As HtmlSelect = gvRow.FindControl("ddlconditions")
            Dim txtdays As TextBox = gvRow.FindControl("txtdays")


            'If txtrmtypcode.Text <> "" Then
            '    flg = True
            'End If




            If txtfrom.Text <> "" And txtto.Text <> "" Then

                If CType(Format(CType(txtto.Text, DateTime), "HH:MM"), String) < CType(Format(CType(txtfrom.Text, DateTime), "HH:MM"), String) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Hours should be greater than From Hour Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (txtchargetype.Text) = "" And ddlcharge.Value.ToUpper = "YES" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Type Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If txtchargetype.Text <> "" And (txtchargetype.Text = "% of Nights") And ddlcharge.Value.ToUpper = "YES" And txtpercharge.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Percentage of Charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargetype.Text <> "" And (txtchargetype.Text.ToUpper = "Value".ToUpper) And ddlcharge.Value.ToUpper = "YES" And txtvalue.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Value of Charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If


            End If



        Next

        For Each gvRow In grdexdates.Rows
            lnCnt += 1


            Dim txtfrom As TextBox = gvRow.FindControl("txtfromDate")


            If txtfrom.Text <> "" Then
                If Session("Calledfrom") = "Offers" Then
                    If Not (Format(CType(txtfrom.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtfrom.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' No Check In/Out Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                        txtfrom.Text = ""
                        SetFocus(txtfrom)
                        OfferValidateSave = False
                        Exit Function
                    End If


                End If


            End If

        Next
        'If flg = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one  Row Cancellation  :" & lnCnt & "');", True)
        '    ValidateSave = False
        '    Exit Function
        'End If


        'For Each gvRow In grdnoshow.Rows
        '    lnCntnoshow += 1

        '    Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
        '    Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
        '    Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

        '    Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")
        '    Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
        '    Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
        '    Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
        '    Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")


        '    If txtrmtypname.Text <> "" Then

        '        If txtrmtypcode.Text = "" Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Proper Roomtype Noshow Grid Row No:" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function
        '        End If

        '        If txtmealcode.Text = "" Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Noshow Grid  Row No:" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function
        '        End If





        '        If (txtchargenoshow.Text) = "" Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Basis Noshow Grid  Row No:" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function
        '        End If

        '        If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Nights") And txtchargenoshow.Text = "" Then
        '            Enablegrid()
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function

        '        End If

        '        If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Nights") And txtpercnoshow.Text = "" And txtnightsnoshow.Text = "" Then
        '            Enablegrid()
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge or Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function

        '        End If

        '        If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Entire Stay") And txtpercnoshow.Text = "" Then
        '            Enablegrid()
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function

        '        End If

        '        If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of remaining Nights") And txtpercnoshow.Text = "" Then
        '            Enablegrid()
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function

        '        End If

        '        If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Value") And txtvaluenoshow.Text = "" Then
        '            Enablegrid()
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Value of Charge Noshow Grid  Row No :" & lnCntnoshow & "');", True)
        '            ValidateSave = False
        '            Exit Function

        '        End If


        '    End If



        'Next


        ''''''''''' Dates Overlapping

        Dim dtdatesnew As New DataTable
        Dim dsdates As New DataSet
        Dim dr As DataRow
        Dim xmldates As String = ""




        dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


        For Each gvRow1 In grdpromodates.Rows
            Dim txtfromdate As TextBox = gvRow1.Findcontrol("txtfromdate")
            Dim txttodate As TextBox = gvRow1.Findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                dr = dtdatesnew.NewRow

                dr("fromdate") = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                dr("todate") = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                dtdatesnew.Rows.Add(dr)

            End If

        Next
        dsdates.Clear()
        If dtdatesnew IsNot Nothing Then
            If dtdatesnew.Rows.Count > 0 Then
                dsdates.Tables.Add(dtdatesnew)
                xmldates = objUtils.GenerateXML(dsdates)
            End If
        Else
            xmldates = "<NewDataSet />"
        End If

        Dim strMsg As String = ""
        Dim ds As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))


        parms.Add(parm(0))


        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("fromdateC")) = False Then
                    strMsg = "Dates Are Overlapping Please check " + "\n"
                    For i = 0 To ds.Tables(0).Rows.Count - 1

                        strMsg += "  Date -  " + ds.Tables(0).Rows(i)("fromdateC") + "\n"
                    Next

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    OfferValidateSave = False
                    Exit Function
                End If
            End If
        End If


        '''''''''''''''''
        Dim flgdt As Boolean = False

        For Each gvRow In grdpromodates.Rows
            Dim txtfromdate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txttodate As TextBox = gvRow.FindControl("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                'If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value) Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & "  ');", True)
                '    txtfromdate.Text = ""
                '    SetFocus(txtfromdate)
                '    OfferValidateSave = False
                '    Exit Function
                'End If
                If Not (Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                    txtfromdate.Text = ""
                    SetFocus(txtfromdate)
                    OfferValidateSave = False
                    Exit Function
                End If
                If Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                    txtfromdate.Text = ""
                    SetFocus(txtfromdate)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (Format(CType(txttodate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                    txttodate.Text = ""
                    txtfromdate.Text = ""
                    SetFocus(txtfromdate)
                    OfferValidateSave = False
                    Exit Function
                End If

            End If

        Next

        If validatedategrid() = False Then
            OfferValidateSave = False
            Exit Function
        End If



        OfferValidateSave = True
    End Function
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable Should not be Blank');", True)
            ValidateSave = False
            Exit Function
        End If

        Dim tickedornot As Boolean = False
        Dim chkSelect As CheckBox
        tickedornot = False
        For Each grdRow In gv_Seasons.Rows
            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)

            If chkSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False And Session("Calledfrom") <> "Offers" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Season');", True)
            ValidateSave = False
            Exit Function
        End If

        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Session("seasons") = Nothing

        For Each grdRow As GridViewRow In gv_Seasons.Rows
            chk2 = grdRow.FindControl("chkseason")
            txtmealcode1 = grdRow.FindControl("txtseasoncode")

            If chk2.Checked = True Then
                seasonname = seasonname + LTrim(RTrim(txtmealcode1.Text)) + ","

            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If

        Session("seasons") = seasonname

        Dim tickedornotrmtype As Boolean = False
        Dim chkrmtyp As CheckBox
        tickedornotrmtype = False
        For Each grdRow In grdroomtype.Rows
            chkrmtyp = CType(grdRow.FindControl("chkrmtyp"), CheckBox)

            If chkrmtyp.Checked = True Then
                tickedornotrmtype = True
                Exit For
            End If
        Next

        If tickedornotrmtype = False Then
            Enablegrid()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Room type');", True)
            ValidateSave = False
            Exit Function
        End If

        Dim rmtypname As String = ""

        Dim txtrmtypcode As Label
        Session("Roomtypes") = Nothing

        For Each grdRow As GridViewRow In grdroomtype.Rows
            chk2 = grdRow.FindControl("chkrmtyp")
            txtrmtypcode = grdRow.FindControl("txtrmtypcode")

            If chk2.Checked = True Then
                rmtypname = rmtypname + txtrmtypcode.Text + ","

            End If
        Next

        If rmtypname.Length > 0 Then
            rmtypname = rmtypname.Substring(0, rmtypname.Length - 1)
        End If

        Session("Roomtypes") = rmtypname

        Dim tickedornotmeal As Boolean = False
        Dim chkmeal As CheckBox
        tickedornotmeal = False
        For Each grdRow In grdmealplan.Rows
            chkmeal = CType(grdRow.FindControl("chkmeal"), CheckBox)

            If chkmeal.Checked = True Then
                tickedornotmeal = True
                Exit For
            End If
        Next

        If tickedornotmeal = False Then
            Enablegrid()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Meal Plan');", True)
            ValidateSave = False
            Exit Function
        End If

        Dim mealplan As String = ""

        Dim txtmealcode As Label
        Session("mealplans") = Nothing

        For Each grdRow As GridViewRow In grdmealplan.Rows
            chk2 = grdRow.FindControl("chkmeal")
            txtmealcode = grdRow.FindControl("txtmealcode")

            If chk2.Checked = True Then
                mealplan = mealplan + txtmealcode.Text + ","

            End If
        Next

        If mealplan.Length > 0 Then
            mealplan = mealplan.Substring(0, mealplan.Length - 1)
        End If

        Session("mealplans") = mealplan


        Dim flg As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")

            If txtfrom.Text <> "" And txtto.Text <> "" Then
                flg = True
                Exit For
            End If

        Next

        'If flg = False Then
        '    Enablegrid()
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter One Row');", True)
        '    ValidateSave = False
        '    Exit Function
        'End If


        Dim lnCnt As Integer = 0
        'Dim lnCntnoshow As Integer = 0

        'Dim ToDt As Date = Nothing


        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1

            Dim ddlselect As HtmlSelect = gvRow.FindControl("ddlselect")
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")
            Dim ddlcharge As HtmlSelect = gvRow.FindControl("ddlcharge")
            Dim txtchargetype As TextBox = gvRow.FindControl("txtchargetype")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim ddlconditions As HtmlSelect = gvRow.FindControl("ddlconditions")
            Dim txtdays As TextBox = gvRow.FindControl("txtdays")


            'If txtrmtypcode.Text <> "" Then
            '    flg = True
            'End If


            If txtfrom.Text <> "" And txtto.Text <> "" Then

                If CType(Format(CType(txtto.Text, DateTime), "HH:MM"), String) < CType(Format(CType(txtfrom.Text, DateTime), "HH:MM"), String) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Hours should be greater than From Hour Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If (txtchargetype.Text) = "" And ddlcharge.Value.ToUpper = "YES" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Type Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If txtchargetype.Text <> "" And (txtchargetype.Text = "% of Nights") And ddlcharge.Value.ToUpper = "YES" And txtpercharge.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Percentage of Charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtchargetype.Text <> "" And (txtchargetype.Text.ToUpper = "Value".ToUpper) And ddlcharge.Value.ToUpper = "YES" And txtvalue.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Value of Charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If


            End If



        Next
        'If flg = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one  Row Cancellation  :" & lnCnt & "');", True)
        '    ValidateSave = False
        '    Exit Function
        'End If



        If validatedategrid() = False Then
            ValidateSave = False
            Exit Function
        End If




        ValidateSave = True
    End Function
    Public Function validatedategrid() As Boolean
        Dim gvrow As GridViewRow
        validatedategrid = True

        For Each gvrow In grdexdates.Rows

            Dim txtfromdate As TextBox = gvrow.FindControl("txtfromDate")

            If txtfromdate.Text <> "" Then

                If Left(Right(txtfromdate.Text, 4), 2) <> "20" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date  Belongs to 21 st century  ');", True)
                    validatedategrid = False
                    SetFocus(txtfromdate)
                    Exit Function
                End If

                If Session("Calledfrom") = "Offers" Then

                    If Not (Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' No Check In/Out Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        validatedategrid = False
                        Exit Function
                    End If


                Else

                    If Not (Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value), Date), "yyyy/MM/dd")) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' No Check In/Out Date Should belong to the Contracts Period   " & hdnconfromdate.Value & " to  " & hdncontodate.Value & " ');", True)
                        txtfromdate.Text = ""
                        SetFocus(txtfromdate)
                        validatedategrid = False
                        Exit Function
                    End If


                End If


            End If


        Next

    End Function
    Public Function checkforexisting() As Boolean

        checkforexisting = True
        Try
            If FindDatePeriod() = False Then
                checkforexisting = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function offerFindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        offerFindDatePeriod = True
        Try

            '   CopyRow = 0

            Dim weekdaystr As String = ""

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdpromodates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")



                If txtfromdate.Text <> "" And txttodate.Text <> "" Then



                    Dim ds As DataSet
                    Dim parms2 As New List(Of SqlParameter)
                    Dim parm2(11) As SqlParameter



                    parm2(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm2(4) = New SqlParameter("@subseasoncode", "")
                    parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(8) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                    parm2(9) = New SqlParameter("@rmtypcode", CType(Session("Roomtypes"), String))
                    parm2(10) = New SqlParameter("@mealcode", CType(Session("mealplans"), String))


                    For i = 0 To 10
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcheckinoutpolicy_offer", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("checkinoutpolicyid")) = False Then
                                strMsg = "CheckIn And CheckOut  Policy already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds.Tables(0).Rows(0)("checkinoutpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                offerFindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If




                End If


            Next



        Catch ex As Exception
            offerFindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("Contractchkinoutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



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

            For Each GVRow In grdDates.Rows



                If GVRow.Cells(1).Text <> "" Then



                    Dim ds As DataSet
                    Dim parms2 As New List(Of SqlParameter)
                    Dim parm2(11) As SqlParameter



                    parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(GVRow.Cells(1).Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(GVRow.Cells(2).Text, Date), "yyyy/MM/dd"))
                    parm2(4) = New SqlParameter("@subseasoncode", CType(GVRow.Cells(3).Text, String))
                    parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(8) = New SqlParameter("@promotionid", "")
                    parm2(9) = New SqlParameter("@rmtypcode", CType(Session("Roomtypes"), String))
                    parm2(10) = New SqlParameter("@mealcode", CType(Session("mealplans"), String))


                    For i = 0 To 10
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcheckinoutpolicy", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("checkinoutpolicyid")) = False Then
                                strMsg = "CheckIn And CheckOut  Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("checkinoutpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If




                End If


            Next



        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("Contractchkinoutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function


#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            Dim strMsg As String = ""
            If Page.IsValid = True Then
                If ViewState("State") = "New" Or ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then
                    If Session("Calledfrom") = "Offers" Then

                        If OfferValidateSave() = False Then
                            Exit Sub
                        End If

                        If offerFindDatePeriod() = False Then
                            Exit Sub
                        End If
                    Else
                        If ValidateSave() = False Then
                            Exit Sub
                        End If
                        If checkforexisting() = False Then
                            Exit Sub
                        End If
                    End If

                    If chkctrygrp.Checked = True And Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    ' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_checkin", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'If Session("Calledfrom") = "Offers" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                    'End If


                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    ' '''''''''''''''''''''''

                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("CHECKIN", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_checkinout_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = CType(Replace(Session("seasons"), ",  ", ","), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkintype", SqlDbType.Int)).Value = CType(ddlcheckin.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkintime", SqlDbType.VarChar, 20)).Value = CType(txtchkintime.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkouttype", SqlDbType.Int)).Value = CType(ddlcheckout.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkouttime", SqlDbType.VarChar, 20)).Value = CType(txtChkouttime.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed

                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_checkinout_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = CType(Replace(Session("seasons"), ",  ", ","), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkintype", SqlDbType.Int)).Value = CType(ddlcheckin.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkintime", SqlDbType.VarChar, 20)).Value = CType(txtchkintime.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkouttype", SqlDbType.Int)).Value = CType(ddlcheckout.SelectedIndex, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@checkouttime", SqlDbType.VarChar, 20)).Value = CType(txtChkouttime.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_detail Where  checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_restricted  Where  checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_countries Where checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_agents  Where checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_roomtypes Where checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_mealplans  Where checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_checkinout_offerdates  Where checkinoutpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    '------------------------------------Inserting Offer Dates
                    If Session("Calledfrom") = "Offers" Then
                        Dim kl As Integer = 1
                        For Each GvRow1 As GridViewRow In grdpromodates.Rows

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_contracts_checkinout_offerdates", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure

                                mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = kl
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")



                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                                kl = kl + 1
                            End If
                        Next
                    End If


                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then


                                mySqlCmd = New SqlCommand("sp_add_contracts_checkinout_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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

                                mySqlCmd = New SqlCommand("sp_add_contracts_checkinout_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                    End If


                    '------------------------------------Inserting Room types
                    Dim GvRow As GridViewRow
                    For Each GvRow In grdroomtype.Rows
                        Dim txtrmtypcode As Label = GvRow.FindControl("txtrmtypcode")
                        Dim chkrmtyp As CheckBox = GvRow.FindControl("chkrmtyp")
                        If chkrmtyp.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_contracts_checkinout_roomtypes", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtypcode.Text, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next

                    '------------------------------------Inserting Meal Plans

                    For Each GvRow In grdmealplan.Rows
                        Dim txtmealcode As Label = GvRow.FindControl("txtmealcode")
                        Dim chkmeal As CheckBox = GvRow.FindControl("chkmeal")
                        If chkmeal.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_contracts_checkinout_mealplans", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtmealcode.Text, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next

                    '------------------------------------Inserting detail
                    Dim m As Integer = 1
                    For Each GvRow In grdRoomrates.Rows

                        Dim ddlselect As HtmlSelect = GvRow.FindControl("ddlselect")
                        Dim txtfrom As TextBox = GvRow.FindControl("txtfrom")
                        Dim txtto As TextBox = GvRow.FindControl("txtto")
                        Dim ddlcharge As HtmlSelect = GvRow.FindControl("ddlcharge")
                        Dim txtchargetype As TextBox = GvRow.FindControl("txtchargetype")
                        Dim txtnights As TextBox = GvRow.FindControl("txtnights")
                        Dim txtpercharge As TextBox = GvRow.FindControl("txtpercharge")
                        Dim txtvalue As TextBox = GvRow.FindControl("txtvalue")
                        Dim ddlconditions As HtmlSelect = GvRow.FindControl("ddlconditions")
                        Dim txtdays As TextBox = GvRow.FindControl("txtdays")

                        If txtfrom.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_edit_contracts_checkinout_detail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = m
                            mySqlCmd.Parameters.Add(New SqlParameter("@checkinouttype", SqlDbType.VarChar, 50)).Value = CType(ddlselect.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromhours", SqlDbType.VarChar, 10)).Value = CType(txtfrom.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@tohours", SqlDbType.VarChar, 10)).Value = CType(txtto.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@chargeyesno", SqlDbType.Int)).Value = IIf(ddlcharge.Value = "Yes", 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@chargetype", SqlDbType.VarChar, 50)).Value = CType(txtchargetype.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal)).Value = CType(Val(txtpercharge.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@value", SqlDbType.Decimal)).Value = CType(Val(txtvalue.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@condition", SqlDbType.VarChar, 100)).Value = CType(ddlconditions.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@requestbeforedays", SqlDbType.Int)).Value = CType(Val(txtdays.Text), Integer)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If

                        m = m + 1

                    Next


                    '------------------------------------Inserting restriced dates
                    Dim n As Integer = 1
                    For Each GvRow In grdexdates.Rows

                        Dim ddlExcl As HtmlSelect = GvRow.FindControl("ddlExcl")
                        Dim txtfromDate As TextBox = GvRow.FindControl("txtfromDate")

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_checkinout_restricted", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        If txtfromDate.Text <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int)).Value = n
                            mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 30)).Value = CType(ddlExcl.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@restrictdate", SqlDbType.VarChar, 10)).Value = CType(txtfromDate.Text, String)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                        End If

                        n = n + 1
                    Next





                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_checkinout_header"
                    'mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 30)).Value = CType(txtplistcode.Text.Trim, String)

                    'mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = txthotelname.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 100)).Value = hdnpartycode.Value
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moddate ", SqlDbType.DateTime)).Value = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "yyyy/MM/dd")
                    'mySqlCmd.Parameters.Add(New SqlParameter("@pricecode", SqlDbType.VarChar, 100)).Value = ""
                    'mySqlCmd.ExecuteNonQuery()



                    strMsg = "Saved Succesfully!!"


                ElseIf ViewState("State") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    ' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_checkin", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'If Session("Calledfrom") = "Offers" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                    'End If


                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    ' '''''''''''''''''''''''


                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_contracts_checkinout_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@checkinoutpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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
                btnreset_Click(sender, e)


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                ModalPopupDays.Hide()  '' Added shahul 08/08/18
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region









#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region








    Private Sub DisableControl()
        If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

            txtplistcode.Text = ""
            grdDates.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            btnAddLinesDates.Enabled = True
            btndeletedates.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btncopyratesnextrow.Enabled = True

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then

            grdDates.Enabled = False

            grdRoomrates.Enabled = False

            btncopyratesnextrow.Enabled = False

            txtApplicableTo.Enabled = False

            wucCountrygroup.Disable(False)
            btnAddLinesDates.Enabled = False
            btndeletedates.Enabled = False
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            btncopyratesnextrow.Enabled = False


        ElseIf ViewState("State") = "Edit" Then


            grdDates.Enabled = True

            grdRoomrates.Enabled = True

            btncopyratesnextrow.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            btnAddLinesDates.Enabled = True
            btndeletedates.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btncopyratesnextrow.Enabled = True
        End If
    End Sub
#Region "Private Sub ShowDates(ByVal RefCode As String ,ByVal subseasonname As String)"

    Private Sub ShowDates(ByVal contractid As String, ByVal subseasonname As String)
        Try
            Dim myDS As New DataSet
            grdDates.Visible = True
            strSqlQry = ""

            ' strSqlQry = "select '' clinenno, '' fromdate,'' todate "

            'strSqlQry = "select 1  clinenno, '15/08/2016' fromdate,'20/08/2016' todate union all select 2  clinenno, '21/08/2016' fromdate,'31/08/2016' todate"
            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate from view_contractseasons_rate(nolock) where contractid='" & contractid & "' and seasonname='" & subseasonname & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdDates.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdDates.DataBind()

            Else
                grdDates.DataBind()

            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
#End Region



    'Private Sub FillMealplans()
    '    Try
    '        Dim myDS As New DataSet
    '        grdmealplan.Visible = True
    '        strSqlQry = ""

    '        strSqlQry = "select  a.mealcode,b.mealname  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "'"

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '        myDataAdapter.Fill(myDS)
    '        grdmealplan.DataSource = myDS

    '        If myDS.Tables(0).Rows.Count > 0 Then
    '            grdmealplan.DataBind()

    '        Else
    '            grdmealplan.DataBind()

    '        End If
    '        Dim chkSelect As CheckBox

    '        For Each grdRow In grdmealplan.Rows
    '            chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
    '            chkSelect.Checked = True

    '        Next


    '        'strSqlQry = ""

    '        'strSqlQry = "select  a.mealcode,b.mealname  from contracts_countries(nolock) where contractid='" & hdncontractid.Value & "'"

    '        'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '        'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '        'myDataAdapter.Fill(myDS)
    '        'grdmealplan.DataSource = myDS

    '        'If myDS.Tables(0).Rows.Count > 0 Then
    '        '    grdmealplan.DataBind()

    '        'Else
    '        '    grdmealplan.DataBind()

    '        'End If





    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    Finally

    '        clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '        clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
    '    End Try
    'End Sub
    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname,c.fromdate,c.todate from view_contracts_search c(nolock),view_contractseasons_rate a(nolock) Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)

                    End If

                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        hdnconfromdate.Value = Format(mySqlReader("fromdate"), "dd/MM/yyyy")


                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        hdncontodate.Value = Format(mySqlReader("todate"), "dd/MM/yyyy")


                    End If
                End If





                mySqlCmd.Dispose()
                mySqlReader.Close()

                mySqlConn.Close()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    'Sub seasonsgridfill()
    '    Try
    '        Dim myDS As New DataSet

    '        strSqlQry = ""


    '        strSqlQry = "select distinct seasonname from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' order by seasonname "

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '        myDataAdapter.Fill(myDS)
    '        gv_Seasons.DataSource = myDS

    '        If myDS.Tables(0).Rows.Count > 0 Then
    '            gv_Seasons.DataBind()

    '        Else
    '            gv_Seasons.DataBind()

    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    Finally

    '        clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '        clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
    '    End Try
    'End Sub
    Private Sub ShowRecordcopy(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_checkinout_header(nolock) Where checkinoutpolicyid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("checkinoutpolicyid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("checkinoutpolicyid"), String)
                    End If
                    'If IsDBNull(mySqlReader("contractid")) = False Then
                    '    hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    'End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",     ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("checkintype")) = False Then
                        ddlcheckin.SelectedIndex = CType(mySqlReader("checkintype"), String)
                    End If
                    If IsDBNull(mySqlReader("checkintime")) = False Then
                        txtchkintime.Text = CType(mySqlReader("checkintime"), String)
                    End If
                    If IsDBNull(mySqlReader("checkouttype")) = False Then
                        ddlcheckout.SelectedIndex = CType(mySqlReader("checkouttype"), String)
                    End If
                    If IsDBNull(mySqlReader("checkouttime")) = False Then
                        txtChkouttime.Text = CType(mySqlReader("checkouttime"), String)
                    End If

                    If IsDBNull(mySqlReader("seasons")) = False Then
                        Session("seasons") = CType(RTrim(LTrim(mySqlReader("seasons"))), String)
                    End If
                    Dim strMealPlans As String = ""
                    Dim strCondition As String = ""
                    If Session("seasons") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strMealPlans = Session("seasons") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        If strMealPlans.Length > 0 Then
                            Dim mString As String() = strMealPlans.Split(",")
                            For i As Integer = 0 To mString.Length - 1
                                If strCondition = "" Then
                                    strCondition = "'" & mString(i) & "'"
                                Else
                                    strCondition &= ",'" & mString(i) & "'"
                                End If
                            Next
                        End If
                    End If

                    Dim myDS As New DataSet
                    gv_Seasons.Visible = True
                    strSqlQry = ""

                    strSqlQry = "select  distinct seasonname seasonname,0 selected from view_contractseasons_rate(nolock)  where contractid='" & hdncopycontractid.Value & "' and seasonname Not IN (" & strCondition & ") " _
                      & " union all " _
                        & " select  distinct seasonname subseasname,1 selected  from view_contractseasons_rate(nolock)  where contractid='" & hdncopycontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  1 "

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    gv_Seasons.DataSource = myDS

                    If myDS.Tables(0).Rows.Count > 0 Then
                        gv_Seasons.DataBind()


                    Else
                        gv_Seasons.DataBind()

                    End If
                    Dim chkSelect As CheckBox
                    Dim lblselect As Label
                    For Each grdRow In gv_Seasons.Rows
                        chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                        lblselect = CType(grdRow.FindControl("lblselect"), Label)

                        If lblselect.Text = "1" Then
                            chkSelect.Checked = True
                            ' chkSelect.Enabled = False
                        End If

                    Next
                    filldates()



                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_cancelpolicy_header(nolock) where  cancelpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "UNAPPROVED"

                    'Else
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "APPROVED"
                    'End If

                End If
            End If


            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_checkinout_header(nolock) Where checkinoutpolicyid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("checkinoutpolicyid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("checkinoutpolicyid"), String)
                    End If
                    If IsDBNull(mySqlReader("contractid")) = False Then
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

                    If IsDBNull(mySqlReader("checkintype")) = False Then
                        ddlcheckin.SelectedIndex = CType(mySqlReader("checkintype"), String)
                    End If
                    If IsDBNull(mySqlReader("checkintime")) = False Then
                        txtchkintime.Text = CType(mySqlReader("checkintime"), String)
                    End If
                    If IsDBNull(mySqlReader("checkouttype")) = False Then
                        ddlcheckout.SelectedIndex = CType(mySqlReader("checkouttype"), String)
                    End If
                    If IsDBNull(mySqlReader("checkouttime")) = False Then
                        txtChkouttime.Text = CType(mySqlReader("checkouttime"), String)
                    End If


                    If IsDBNull(mySqlReader("seasons")) = False Then
                        Session("seasons") = CType(RTrim(LTrim(mySqlReader("seasons"))), String)
                    End If
                    Dim strMealPlans As String = ""
                    Dim strCondition As String = ""
                    If Session("seasons") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strMealPlans = Session("seasons") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        If strMealPlans.Length > 0 Then
                            Dim mString As String() = strMealPlans.Split(",")
                            For i As Integer = 0 To mString.Length - 1
                                If strCondition = "" Then
                                    strCondition = "'" & mString(i) & "'"
                                Else
                                    strCondition &= ",'" & mString(i) & "'"
                                End If
                            Next
                        End If
                    End If
                    Dim myDS As New DataSet
                    If Session("Calledfrom") <> "Offers" Then
                        gv_Seasons.Visible = True
                        strSqlQry = ""

                        strSqlQry = "select  distinct seasonname seasonname,0 selected from view_contractseasons_rate(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname Not IN (" & strCondition & ") " _
                          & " union all " _
                            & " select  distinct seasonname subseasname,1 selected  from view_contractseasons_rate(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  1 "

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                        myDataAdapter.Fill(myDS)
                        gv_Seasons.DataSource = myDS

                        If myDS.Tables(0).Rows.Count > 0 Then
                            gv_Seasons.DataBind()


                        Else
                            gv_Seasons.DataBind()

                        End If
                        Dim chkSelect As CheckBox
                        Dim lblselect As Label
                        For Each grdRow In gv_Seasons.Rows
                            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                            lblselect = CType(grdRow.FindControl("lblselect"), Label)

                            If lblselect.Text = "1" Then
                                chkSelect.Checked = True
                                ' chkSelect.Enabled = False
                            End If

                        Next
                        filldates()
                    End If





                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_checkinout_header(nolock) where  checkinoutpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
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
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdRoomrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0


            strQry = "select count( distinct clineno) from view_contracts_checkinout_detail(nolock) where checkinoutpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdRoomrates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_checkinout_detail d  where   d.checkinoutpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdRoomrates.Rows

                    Dim ddlselect As HtmlSelect = GvRow.FindControl("ddlselect")
                    Dim txtfrom As TextBox = GvRow.FindControl("txtfrom")
                    Dim txtto As TextBox = GvRow.FindControl("txtto")
                    Dim ddlcharge As HtmlSelect = GvRow.FindControl("ddlcharge")
                    Dim txtchargetype As TextBox = GvRow.FindControl("txtchargetype")
                    Dim txtnights As TextBox = GvRow.FindControl("txtnights")
                    Dim txtpercharge As TextBox = GvRow.FindControl("txtpercharge")
                    Dim txtvalue As TextBox = GvRow.FindControl("txtvalue")
                    Dim ddlconditions As HtmlSelect = GvRow.FindControl("ddlconditions")
                    Dim txtdays As TextBox = GvRow.FindControl("txtdays")

                    If mySqlReader.Read = True Then



                        If IsDBNull(mySqlReader("checkinouttype")) = False Then
                            ddlselect.Value = mySqlReader("checkinouttype")

                        End If


                        If IsDBNull(mySqlReader("fromhours")) = False Then
                            txtfrom.Text = mySqlReader("fromhours")

                        End If
                        If IsDBNull(mySqlReader("tohours")) = False Then
                            txtto.Text = mySqlReader("tohours")

                        End If
                        If IsDBNull(mySqlReader("chargeyesno")) = False Then
                            ddlcharge.Value = IIf(mySqlReader("chargeyesno") = 1, "Yes", "No")

                        End If
                        If IsDBNull(mySqlReader("chargetype")) = False Then
                            txtchargetype.Text = mySqlReader("chargetype")

                        End If
                        If IsDBNull(mySqlReader("percentage")) = False Then
                            txtpercharge.Text = IIf(mySqlReader("percentage") = 0, "", mySqlReader("percentage"))

                        End If
                        If IsDBNull(mySqlReader("value")) = False Then
                            txtvalue.Text = IIf(mySqlReader("value") = 0, "", DecRound(mySqlReader("value")))

                        End If

                        If IsDBNull(mySqlReader("condition")) = False Then
                            ddlconditions.Value = mySqlReader("condition")

                        End If
                        If IsDBNull(mySqlReader("requestbeforedays")) = False Then
                            txtdays.Text = IIf(mySqlReader("requestbeforedays") = 0, "", mySqlReader("requestbeforedays"))

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            '''''''''''
            Enablegrid()


            Dim TxtFromDateGlobal As New TextBox
            Dim TxtToDateGlobal As New TextBox
            Dim ddlExcl As HtmlSelect

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            If Session("Calledfrom") = "Offers" Then
                myCommand = New SqlCommand("Select * from view_contracts_checkinout_restricted(nolock) Where checkinoutpolicyid='" & RefCode & "'  and convert(varchar(10),(convert(datetime,restrictdate,103)),111) between '" & Format(CType(hdnpromofrmdate.Value, Date), "yyyy/MM/dd") & "'  and  '" & Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd") & "'", SqlConn)
            Else
                myCommand = New SqlCommand("Select * from view_contracts_checkinout_restricted(nolock) Where checkinoutpolicyid='" & RefCode & "'", SqlConn)
            End If

            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Dim ct As Integer
            If mySqlReader.HasRows Then
                If Session("Calledfrom") = "Offers" Then
                    ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from view_contracts_checkinout_restricted(nolock) Where checkinoutpolicyid='" & RefCode & "' and convert(varchar(10),(convert(datetime,restrictdate,103)),111)  between '" & Format(CType(hdnpromofrmdate.Value, Date), "yyyy/MM/dd") & "'  and  '" & Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd") & "'")
                Else
                    ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from view_contracts_checkinout_restricted(nolock) Where checkinoutpolicyid='" & RefCode & "'")
                End If

                fillDategrd(grdexdates, False, ct)

                For Each gvRow In grdexdates.Rows
                    mySqlReader.Read()
                    If IsDBNull(mySqlReader("restrictdate")) = False Then
                        TxtFromDateGlobal = gvRow.FindControl("txtfromDate")
                        TxtFromDateGlobal.Text = Format(CType(mySqlReader("restrictdate"), Date), "dd/MM/yyyy")

                    End If


                    If IsDBNull(mySqlReader("datetype")) = False Then
                        ddlExcl = gvRow.FindControl("ddlExcl")
                        ddlExcl.Value = CType((mySqlReader("datetype")), String)

                    End If
                Next

            Else
                fillDategrd(grdexdates, True)
            End If










        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkinoutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            Dim lbltran As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            If lbltran.Text.Trim = "" Then Exit Sub

            If e.CommandName <> "View" Then
                If Session("Calledfrom") = "Offers" Then
                    Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdnpromotionid.Value & "'")

                    If offerexists Is Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save the Offer Main Details First');", True)
                        Exit Sub

                    End If
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
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                fillDategrd(grdexdates, True)
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                FillRoomtypemealplan()

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Update"
                If Session("Calledfrom") = "Offers" Then
                    lblHeading.Text = "Edit Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Else
                    lblHeading.Text = "Edit Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                End If

                Page.Title = " Check In/Check Out Policy  "
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillDategrd(grdexdates, True)
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                FillRoomtypemealplan()
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                If Session("Calledfrom") = "Offers" Then
                    lblHeading.Text = "View Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Else
                    lblHeading.Text = "View Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                End If
                Page.Title = " Check In/Check Out Policy "
            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                ViewState("State") = "Delete"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillDategrd(grdexdates, True)
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                FillRoomtypemealplan()
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                If Session("Calledfrom") = "Offers" Then
                    lblHeading.Text = "Delete Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Else
                    lblHeading.Text = "Delete Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                End If
                Page.Title = " Check In/Check Out Policy "
            ElseIf e.CommandName = "Copy" Then
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillDategrd(grdexdates, True)
                ShowRecord(CType(lbltran.Text.Trim, String))



                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                If Session("Calledfrom") = "Offers" Then
                    ShowDatesnew(CType(hdnpromotionid.Value, String))
                    FillRoomtypemealplanoffer(hdnpromotionid.Value)
                    Showdetailsgrid(CType(lbltran.Text.Trim, String))
                    lblHeading.Text = "Copy Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Else
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    FillRoomtypemealplan()
                    Showdetailsgrid(CType(lbltran.Text.Trim, String))
                    lblHeading.Text = "Copy Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                End If
                Page.Title = " Check In/Check Out Policy "
            End If

            If Session("Calledfrom") = "Offers" Then
                divpromo.Style.Add("display", "block")
                divseason.Style.Add("display", "none")
                dv_SearchResult.Style.Add("display", "none")
                divoffer.Style.Add("display", "block")
            Else
                divpromo.Style.Add("display", "none")
                divseason.Style.Add("display", "block")
                dv_SearchResult.Style.Add("display", "block")
                divoffer.Style.Add("display", "none")
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub gv_Seasons_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Seasons.RowDataBound
        Dim txtseasoncode As Label
        Dim chkseason As CheckBox

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            txtseasoncode = e.Row.FindControl("txtseasoncode")
            chkseason = e.Row.FindControl("chkseason")



            chkseason.Attributes.Add("onChange", "datefill('" & chkseason.ClientID & "')")


        End If
    End Sub
    Private Sub filldates()
        Try
            Dim myDS As New DataSet
            grdDates.Visible = True
            strSqlQry = ""

            Dim seasonname As String = ""
            Dim chk2 As CheckBox
            Dim txtmealcode1 As Label

            For Each gvRow As GridViewRow In gv_Seasons.Rows
                chk2 = gvRow.FindControl("chkseason")
                txtmealcode1 = gvRow.FindControl("txtseasoncode")

                If chk2.Checked = True Then
                    seasonname = seasonname + txtmealcode1.Text + ","

                End If
            Next

            If seasonname.Length > 0 Then
                seasonname = seasonname.Substring(0, seasonname.Length - 1)
            End If


            Dim strCondition As String = ""
            If seasonname.Length > 0 Then
                Dim mString As String() = seasonname.Split(",")
                For i As Integer = 0 To mString.Length - 1
                    If strCondition = "" Then
                        strCondition = "'" & mString(i) & "'"
                    Else
                        strCondition &= ",'" & mString(i) & "'"
                    End If
                Next
            End If

            'If strCondition = "" Then
            '    grdDates.DataSource = Nothing
            '    grdDates.DataBind()
            '    grdDates.Visible = False

            '    Exit Sub
            'End If
            If strCondition <> "" Then
                strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName from view_contractseasons_rate(nolock) where contractid='" & hdncontractid.Value & "' and Seasonname IN (" & strCondition & ") order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"

            Else
                strSqlQry = "select '' fromdate,'' todate,'' SeasonName"
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdDates.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdDates.DataBind()

            Else
                grdDates.DataBind()

            End If

            For i As Integer = grdDates.Rows.Count - 1 To 1 Step -1
                Dim row As GridViewRow = grdDates.Rows(i)
                Dim previousRow As GridViewRow = grdDates.Rows(i - 1)
                Dim J As Integer = 3
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


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub

    Private Sub Roomratesrowsetting()

        Dim k As Long
        Dim j As Long = 1
        Dim txt As TextBox
        Dim txtunityesno As TextBox
        Dim GvRow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        Dim m As Long
        Dim n As Long = 0
        Dim cnt As Long = grdRoomrates.Columns.Count
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False

        Try
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If Not txt Is Nothing Then
                    heading(header) = txt.Text
                End If


                If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then  'col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson"

                Else
                    If Len(heading(header)) >= 14 Then
                        grdRoomrates.Columns(txt.Columns).HeaderStyle.Width = 75
                        grdRoomrates.Columns(txt.Columns).ItemStyle.Width = 75
                        grdRoomrates.Columns(txt.Columns).ControlStyle.Width = 75
                        grdRoomrates.Columns(txt.Columns).FooterStyle.Width = 75
                        txt.Width = 50
                    End If
                End If
                '----------------------------------------------------------------------------------------------------------------------------

                ''This below is text box so need to wrap the heading which is creating dynamically
                'If heading(header) = "Compulsory Nights" Then
                '    txt.TextMode = TextBoxMode.MultiLine
                '    txt.Height = 28
                '    txt.Width = 75
                '    txt.Wrap = True
                '    txt.Style("OVERFLOW") = "hidden"
                'End If

            Next


            For Each GvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8
                        'If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Noofextraperson" Then
                        If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                            'txt = GvRow.FindControl("txtHead" & j)
                            'txt.Style("text-align") = "center"


                        Else
                            txt = GvRow.FindControl("txt" & j)
                            txtunityesno = GvRow.FindControl("txtunityesno")
                            If txt Is Nothing Then
                            Else
                                ' Numbers(txt)
                                If txtunityesno.Text = "1" Then
                                    txt.Enabled = False
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 1)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    '  Numbers(txt)
                                    If txtunityesno.Text = "1" Then
                                        txt.Enabled = False
                                    End If
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    '  Numbers(txt)
                                    If txtunityesno.Text = "1" Then
                                        txt.Enabled = False
                                    End If
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 3)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    ' Numbers(txt)
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 4)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    charcters(txt)
                                End If
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        'If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Noofextraperson" Then
                        If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                ' Numbers(txt)
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 1)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    ' Numbers(txt)

                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    ' Numbers(txt)
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 3)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    '  Numbers(txt)
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 4)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    charcters(txt)
                                End If
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub
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
#Region "charcters"
    Public Sub charcters(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
    Private Sub createdatarows()
        Dim cnt As Long
        Dim k As Long = 0
        Dim mealplanstr As String = ""
        Dim lblMealCode As Label
        Dim chksel As CheckBox
        Dim myDS As New DataSet
        Dim mealcount As Integer = 0
        Try
            ViewState("mealcount") = Nothing
            Session("MealPlans") = Nothing
            'For Each gvrow In grdmealplan.Rows
            '    chksel = gvrow.FindControl("chkSelect")
            '    lblMealCode = gvrow.FindControl("lblmealcode")
            '    If chksel.Checked = True Then
            '        mealplanstr = mealplanstr + "," + lblMealCode.Text
            '        mealcount = mealcount + 1
            '    End If
            'Next

            'Session("MealPlans") = mealplanstr
            'ViewState("mealcount") = mealcount


            'Dim strMealPlans As String = ""
            'Dim strCondition As String = ""
            'If Session("MealPlans") Is Nothing Then strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
            'If strMealPlans.Length > 0 Then
            '    Dim mString As String() = strMealPlans.Split(",")
            '    For i As Integer = 0 To mString.Length - 1
            '        If strCondition = "" Then
            '            strCondition = "'" & mString(i) & "'"
            '        Else
            '            strCondition &= ",'" & mString(i) & "'"
            '        End If
            '    Next
            'End If

            'If ddlmealfrom.Value <> "[Select]" Then

            '    strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
            '             " from partymeal ,mealmast " & _
            '              " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True, ddlmealfrom.Value)
            'Else

            '    strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
            '             " from partymeal ,mealmast " & _
            '              " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True)

            'End If

            'If ddlmealto.Value <> "[Select]" Then

            '    strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
            '             " from partymeal ,mealmast " & _
            '              " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True, ddlmealto.Value)
            'Else

            '    strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
            '             " from partymeal ,mealmast " & _
            '              " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True)

            'End If




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'strSqlQry = "SELECT count(partyrmtyp.rmtypcode) FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " & _
            '            "INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode where(partyrmtyp.rmtypcode = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) " & _
            '            "and partyrmtyp.partycode=partymeal.partycode and partyrmtyp.partycode='" & hdnpartycode.Value & "'"

            strSqlQry = "select count(pr.rmtypcode) from  partyrmtyp pr where pr.inactive=0 and pr.partycode='" & hdnpartycode.Value & "'"

            'strSqlQry = "sp_getrowsrmtypecountplist'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "' "

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()

            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String
            Dim arr_meal(cnt + 1) As String
            Dim arr_pricepax(cnt + 1) As String
            Dim arr_unityesno(cnt + 1) As String
            Dim arr_extraper(cnt + 1) As String
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))




            strSqlQry = "select rmtypcode,rmtypname,'' mealcode,0 pricepax, 0 unityesno ,0 noofextraperson from  partyrmtyp pr where pr.inactive=0 and pr.partycode='" & hdnpartycode.Value & "'  order by pr.rankord"

            '   strSqlQry = "sp_getrows_plist'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "' "
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr_rows(k) = mySqlReader("rmtypcode")
                arr_rname(k) = mySqlReader("rmtypname")
                arr_meal(k) = mySqlReader("mealcode")
                arr_pricepax(k) = mySqlReader("pricepax")
                arr_unityesno(k) = mySqlReader("unityesno")
                arr_extraper(k) = mySqlReader("noofextraperson")
                k = k + 1
            End While
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)

            'Here add rows.....
            'Now Get Rows From sellmast Based on SPType
            Session("GV_HotelData") = dt
            Dim dr As DataRow


            dr = Nothing
            'grdRoomrates.Columns.Clear()
            'grdRoomrates.Dispose()
            'grdRoomrates.DataBind()

            'End If
            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1

                dr("Room_Type_Code") = arr_rows(row)
                dr("Room Type Name") = arr_rname(row)
                dr("Meal Plan") = "" 'arr_meal(row)
                dr("Price Pax") = IIf(arr_unityesno(row) = "1", IIf(arr_pricepax(row) = "0", "", arr_pricepax(row)), "")
                dr("Extra Person Supplement") = ""

                dr("unityesno") = arr_unityesno(row)
                dr("noofextraperson") = arr_extraper(row)

                dr("No.of.Nights Room Rate") = "1"
                dr("Min Nights") = "1"
                dt.Rows.Add(dr)
            Next


            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()

            Roomratesrowsetting()

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub





    Sub seasonsgridfill()
        Try
            Dim myDS As New DataSet
            gv_Seasons.Visible = True
            strSqlQry = ""


            strSqlQry = "select distinct seasonname,0 selected from view_contractseasons_rate(nolock) where contractid='" & hdncontractid.Value & "' order by seasonname "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            gv_Seasons.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_Seasons.DataBind()

            Else
                gv_Seasons.DataBind()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Private Sub FillRoomtypemealplanoffer(ByVal promotionid As String)
        Try
            Dim myDS As New DataSet
            grdroomtype.Visible = True
            strSqlQry = ""

            strSqlQry = "select v.rmtypcode,p.rmtypname from view_offers_rmtype v, partyrmtyp p(nolock) where  v.rmtypcode=p.rmtypcode and v.partycode=p.partycode and  p.inactive=0 and v.partycode='" & hdnpartycode.Value & "' and v.promotionid='" & promotionid & "'  order by isnull(p.rankord,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdroomtype.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdroomtype.DataBind()

            Else
                grdroomtype.DataBind()

            End If





            strSqlQry = ""
            Dim myDS1 As New DataSet

            strSqlQry = "select v.mealcode as mealcode,m.mealname mealname from view_offers_meal v(nolock), partymeal p(nolock),mealmast m(nolock) where v.mealcode=p.mealcode and p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' and v.promotionid='" & promotionid & "' order by isnull(m.rankorder,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS1)
            grdmealplan.DataSource = myDS1

            If myDS1.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()

            Else
                grdmealplan.DataBind()

            End If


            Dim dt As New DataTable
            Dim chksel As CheckBox
            Dim gvRow As GridViewRow

            Dim i As Integer
            Dim dtmeal As New DataTable
            Dim chkselmeal As CheckBox

            For Each gvRow In grdroomtype.Rows
                chksel = gvRow.FindControl("chkrmtyp")
                chksel.Checked = True

            Next



            For Each gvRow In grdmealplan.Rows
                chkselmeal = gvRow.FindControl("chkmeal")

                chkselmeal.Checked = True

            Next


            'dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmtypcode from view_contracts_checkinout_roomtypes(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
            'For Each gvRow In grdroomtype.Rows
            '    chksel = gvRow.FindControl("chkrmtyp")
            '    txtrmtypcode = gvRow.FindControl("txtrmtypcode")
            '    For i = 0 To dt.Rows.Count - 1
            '        If dt.Rows(i)(0).ToString = txtrmtypcode.Text Then
            '            chksel.Checked = True
            '        End If
            '    Next
            'Next




            'Dim txtmealcode As Label
            'dtmeal = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select mealcode from view_contracts_checkinout_mealplans(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
            'For Each gvRow In grdmealplan.Rows
            '    chkselmeal = gvRow.FindControl("chkmeal")
            '    txtmealcode = gvRow.FindControl("txtmealcode")
            '    For i = 0 To dtmeal.Rows.Count - 1
            '        If dtmeal.Rows(i)(0).ToString = txtmealcode.Text Then
            '            chkselmeal.Checked = True
            '        End If
            '    Next
            'Next





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Private Sub FillRoomtypemealplan()
        Try
            Dim myDS As New DataSet
            grdroomtype.Visible = True
            strSqlQry = ""

            '  strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"
            If Session("Calledfrom") = "Offers" Then
                strSqlQry = "select v.rmtypcode,p.rmtypname from  partyrmtyp p(nolock),view_offers_rmtype v where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.inactive=0 and p.partycode='" & hdnpartycode.Value & "' order by isnull(p.rankord,999)"
            Else
                strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdroomtype.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdroomtype.DataBind()

            Else
                grdroomtype.DataBind()

            End If





            strSqlQry = ""
            Dim myDS1 As New DataSet

            ' strSqlQry = "select p.mealcode as mealcode,m.mealname mealname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
            If Session("Calledfrom") = "Offers" Then
                strSqlQry = "select v.mealcode as mealcode,m.mealname mealname from  partymeal p(nolock),mealmast m(nolock),view_offers_meal v(nolock)  " _
                    & " where v.mealcode=p.mealcode and v.mealcode=m.mealcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
            Else
                strSqlQry = "select p.mealcode as mealcode,m.mealname mealname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS1)
            grdmealplan.DataSource = myDS1

            If myDS1.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()

            Else
                grdmealplan.DataBind()

            End If



            If ViewState("State") <> "New" Then

                Dim dt As New DataTable
                Dim chksel As CheckBox
                Dim gvRow As GridViewRow
                Dim txtrmtypcode As Label
                Dim i As Integer
                dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmtypcode from view_contracts_checkinout_roomtypes(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
                For Each gvRow In grdroomtype.Rows
                    chksel = gvRow.FindControl("chkrmtyp")
                    txtrmtypcode = gvRow.FindControl("txtrmtypcode")
                    For i = 0 To dt.Rows.Count - 1
                        If dt.Rows(i)(0).ToString = txtrmtypcode.Text Then
                            chksel.Checked = True
                        End If
                    Next
                Next


                Dim dtmeal As New DataTable
                Dim chkselmeal As CheckBox

                Dim txtmealcode As Label
                dtmeal = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select mealcode from view_contracts_checkinout_mealplans(nolock) where checkinoutpolicyid='" & txtplistcode.Text & "'").Tables(0)
                For Each gvRow In grdmealplan.Rows
                    chkselmeal = gvRow.FindControl("chkmeal")
                    txtmealcode = gvRow.FindControl("txtmealcode")
                    For i = 0 To dtmeal.Rows.Count - 1
                        If dtmeal.Rows(i)(0).ToString = txtmealcode.Text Then
                            chkselmeal.Checked = True
                        End If
                    Next
                Next

            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        ViewState("State") = "New"
        lblstatus.Visible = False
        lblstatustext.Visible = False

        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False



        If Session("Calledfrom") = "Offers" Then

            divoffer.Style.Add("display", "block")

            txtpromotionid.Text = hdnpromotionid.Value
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select isnull(promotionname,'') promotionname , ApplicableTo,pm.mktcode  from view_offers_header h (nolock) cross apply dbo.splitallotmkt(h.promotiontypes,',')  pm  where   pm.mktcode ='Special Rates' and promotionid='" & hdnpromotionid.Value & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                txtpromoitonname.Text = ds.Tables(0).Rows(0).Item("promotionname")
                txtApplicableTo.Text = ds.Tables(0).Rows(0).Item("ApplicableTo")
                hdncommtype.Value = ds.Tables(0).Rows(0).Item("mktcode")
            End If

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
            End If


            divpromo.Style.Add("display", "block")
            divseason.Style.Add("display", "none")
            dv_SearchResult.Style.Add("display", "none")
            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            clearall()
            DisableControl()
            wucCountrygroup.Visible = True
            btncopyratesnextrow.Visible = True
            fillDategrd(grdpromodates, True)
            ShowDatesnew(CType(hdnpromotionid.Value, String))
            fillDategrd(grdexdates, True)
            btnSave.Visible = True



            btnSave.Text = "Save"
            lblHeading.Text = "New Check In/Check Out Policy - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + " Check In/Check Out Policy -" + ViewState("hotelname") + "-" + hdnpromotionid.Value
            fillroomgrid(grdRoomrates, True)
            FillRoomtypemealplanoffer(hdnpromotionid.Value)

        Else

            divpromo.Style.Add("display", "block")
            divseason.Style.Add("display", "none")
            dv_SearchResult.Style.Add("display", "none")
            Session("contractid") = hdncontractid.Value
            wucCountrygroup.Visible = True
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            fillseason()
            FillRoomtypemealplan()
            grdDates.Visible = True
            clearall()
            DisableControl()

            wucCountrygroup.Visible = True


            seasonsgridfill()
            btncopyratesnextrow.Visible = True

            lable12.Visible = True

            fillroomgrid(grdRoomrates, True)

            fillDategrd(grdexdates, True)

            divpromo.Style.Add("display", "none")
            divseason.Style.Add("display", "block")
            dv_SearchResult.Style.Add("display", "block")

            btnSave.Visible = True

            btnSave.Text = "Save"
            lblHeading.Text = "New Check In/Check Out Policy - " + ViewState("hotelname") + "-" + hdncontractid.Value
            Page.Title = Page.Title + " " + " Check In/Check Out Policy -" + ViewState("hotelname") + "-" + hdncontractid.Value

            ' divuser.Style("display") = "none"
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            divoffer.Style.Add("display", "none")

        End If




    End Sub
    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            '  strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            Else
                strSqlQry = "select count( distinct fromdate) from view_contracts_checkinout_offerdates(nolock) where checkinoutpolicyid='" & RefCode & "'"
            End If

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdpromodates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            '  mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
            Else
                mySqlCmd = New SqlCommand("Select * from view_contracts_checkinout_offerdates(nolock) Where checkinoutpolicyid='" & RefCode & "'", mySqlConn)

            End If

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdpromodates.Rows
                        dpFDate = gvRow.FindControl("txtfromdate")
                        dpTDate = gvRow.FindControl("txttodate")
                        '     Dim lblseason As Label = gvRow.FindControl("lblseason")
                        If dpFDate.Text = "" And dpFDate.Text = "" Then
                            If IsDBNull(mySqlReader("fromdate")) = False Then
                                dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

                            End If
                            If IsDBNull(mySqlReader("todate")) = False Then
                                dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                            End If

                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChkInOutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub btndeletedates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeletedates.Click

        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdexdates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim ddlExcl As HtmlSelect
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdexdates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    ddlExcl = GVRow.FindControl("ddlExcl")
                    excl(n) = CType(ddlExcl.Value, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdexdates.Rows.Count > 1 Then
                fillDategrd(grdexdates, False, grdexdates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdexdates, False, grdexdates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdexdates.Rows
                If GVRow.RowIndex < count Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpFDate.Text = fDate(n)
                    dpTDate = GVRow.FindControl("txtToDate")
                    dpTDate.Text = tDate(n)
                    ddlExcl = GVRow.FindControl("ddlExcl")
                    ddlExcl.Value = excl(n)
                    n = n + 1
                End If
            Next
            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btnAddLinesDates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLinesDates.Click
        '  Createdatacolumns("add")

        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdexdates.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim excl(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim ddlExcl As HtmlSelect

        Try
            For Each GVRow In grdexdates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                ddlExcl = GVRow.FindControl("ddlExcl")
                excl(n) = CType(ddlExcl.Value, String)
                n = n + 1
            Next
            fillDategrd(grdexdates, False, grdexdates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdexdates.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpFDate.Text = fDate(n)
                dpTDate = GVRow.FindControl("txtToDate")
                dpTDate.Text = tDate(n)
                ddlExcl = GVRow.FindControl("ddlExcl")
                ddlExcl.Value = excl(n)
                n = n + 1
            Next
            Dim gridNewrow As GridViewRow
            gridNewrow = grdexdates.Rows(grdexdates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdexdates.ClientID
            Dim strFoucsColumnIndex As String = "1"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdexdates.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Private Sub createdatacolumns()

        Dim dt As New DataTable
        Dim dr As DataRow

        Dim gvRow As GridViewRow

        Try

            cnt = 0
            Session("GV_HotelData") = Nothing
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
            strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                      & hdnpartycode.Value & "' "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True
                arr(i) = mySqlReader("rmcatcode")
                i = i + 1
            End While
            mySqlReader.Close()
            mySqlConn.Close()


            Dim tf As New TemplateField
            dt = New DataTable

            dt.Columns.Add(New DataColumn("RoomTypecode", GetType(String)))
            dt.Columns.Add(New DataColumn("Select_RoomType", GetType(String)))
            dt.Columns.Add(New DataColumn("Meal_Plan", GetType(String)))
            dt.Columns.Add(New DataColumn("MaxChild Allowed", GetType(String)))
            dt.Columns.Add(New DataColumn("MaxEB Allowed", GetType(String)))
            dt.Columns.Add(New DataColumn("No.of Children", GetType(String)))
            dt.Columns.Add(New DataColumn("From Age", GetType(String)))
            dt.Columns.Add(New DataColumn("To Age", GetType(String)))
            dt.Columns.Add(New DataColumn("Charge for Sharing", GetType(String)))
            dt.Columns.Add(New DataColumn("Charge for EB", GetType(String)))

            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next

            Session("GV_HotelData") = dt


            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassChildPolicy(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassChildPolicy(ListItemType.Item, col.ColumnName, fld2)
                    grdRoomrates.Columns.Add(bfield)


                End If
            Next
            grdRoomrates.Visible = True


            For Each gvRow In grdRoomrates.Rows

                dr = dt.NewRow

                Dim chkSelect As CheckBox = gvRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
                Dim txtmaxchild As TextBox = gvRow.FindControl("txtmaxchild")
                Dim txtmaxeb As TextBox = gvRow.FindControl("txtmaxeb")
                Dim txtnoofchild As TextBox = gvRow.FindControl("txtnoofchild")
                Dim txtfromage As TextBox = gvRow.FindControl("txtfromage")
                Dim txttoage As TextBox = gvRow.FindControl("txttoage")
                Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")

                Dim txtEB As TextBox = gvRow.FindControl("txtEB")


                dr("RoomTypecode") = txtrmtypcode.Text
                dr("Select_RoomType") = txtrmtypname.Text
                dr("Meal_Plan") = txtmealcode.Text
                dr("MaxChild Allowed") = txtmaxchild.Text
                dr("MaxEB Allowed") = txtmaxeb.Text
                dr("No.of Children") = txtnoofchild.Text
                dr("From Age") = txtfromage.Text
                dr("To Age") = txttoage.Text
                dr("Charge for Sharing") = txtsharing.Text
                dr("Charge for EB") = txtEB.Text

                dt.Rows.Add(dr)



            Next


            grdRoomrates.DataSource = dt
            grdRoomrates.DataBind()




        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Sub clearall()
        ' fillDategrd(grdDates, True)
        txtplistcode.Text = ""
        txtchkintime.Text = ""
        txtChkouttime.Text = ""
        grdRoomrates.Enabled = True
        txtApplicableTo.Enabled = True
        wucCountrygroup.Disable(True)

    End Sub
    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        clearall()
        Panelsearch.Enabled = True
        Session("GV_HotelData") = Nothing
        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")
        grdDates.DataSource = CreateDataSource(0)
        grdDates.DataBind()
        lblHeading.Text = "Checkin/Out Policy  -" + ViewState("hotelname") + "-" + hdncontractid.Value
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "CONTRACTCHECKINOUT", CType(Session("ContractState"), String))
        ddlorder.SelectedIndex = 0
        ddlorderby.SelectedIndex = 1

        If Session("Calledfrom") = "Offers" Then

            lblHeading.Text = "Checkin/Out Policy  -" + ViewState("hotelname") + "-" + hdnpromotionid.Value

            FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
        Else
            lblHeading.Text = "Checkin/Out Policy  -" + ViewState("hotelname") + "-" + hdncontractid.Value

            FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")
        End If

    End Sub
    Private Function ValidatePage() As Boolean
        ValidatePage = True
        Dim GvRow As GridViewRow
        Dim gvRow1 As GridViewRow


        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable Should not be Blank');", True)
            ValidatePage = False
            Exit Function
        End If

        Dim flg As Boolean
        flg = False

        For Each GvRow In grdDates.Rows

            If GvRow.Cells(1).Text <> "" And GvRow.Cells(2).Text <> "" Then
                flg = True
                Exit For
            End If
        Next


        'flg = False
        'Dim chksel As CheckBox
        'For Each gvRow1 In grdmealplan.Rows
        '    chksel = gvRow1.FindControl("chkSelect")
        '    If chksel.Checked = True Then
        '        flg = True
        '        Exit For
        '    End If
        'Next

        'If flg = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Meal Plan Cannot be Blank');", True)
        '    ValidatePage = False
        '    Exit Function
        'End If

    End Function


    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        filldates()
        Enablegrid()

    End Sub
    Protected Sub grdRoomrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRoomrates.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strRowId As String = e.Row.RowIndex
            Dim strFoucsColumnIndex = "2"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        End If



        If (e.Row.RowType = DataControlRowType.DataRow) Then


            Dim txtchargetype As TextBox = e.Row.FindControl("txtchargetype")
            ' Dim txtnights As TextBox = e.Row.FindControl("txtnights")
            Dim txtpercharge As TextBox = e.Row.FindControl("txtpercharge")
            Dim txtvalue As TextBox = e.Row.FindControl("txtvalue")
            Dim txtdays As TextBox = e.Row.FindControl("txtdays")
            Dim ddlcharge As HtmlSelect = e.Row.FindControl("ddlcharge")
            Dim txtfrom As TextBox = e.Row.FindControl("txtfrom")
            Dim txtto As TextBox = e.Row.FindControl("txtto")


            Numberssrvctrl(txtdays)
            ' Numberssrvctrl(txtnights)
            Numberssrvctrl(txtpercharge)
            Numberssrvctrl(txtvalue)

            'If txtcharge.Text <> "" And (txtcharge.Text = "Nights") Then
            '    txtnights.Enabled = True
            '    txtpercharge.Enabled = False
            '    txtvalue.Enabled = False


            'End If

            If txtchargetype.Text <> "" And (txtchargetype.Text = "% of Nights") Then
                txtpercharge.Enabled = True
                ' txtnights.Enabled = True
                txtvalue.Enabled = False

            End If
            'If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay") Then
            '    txtpercharge.Enabled = True
            '    txtnights.Enabled = False
            '    txtvalue.Enabled = False

            'End If

            If txtchargetype.Text <> "" And (txtchargetype.Text = "Value") Then
                txtvalue.Enabled = True
                ' txtnights.Enabled = False
                txtpercharge.Enabled = False

            End If

            txtfrom.Attributes.Add("onchange", "chkfrmtime('" & txtchargetype.ClientID & "','" & txtfrom.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")
            txtto.Attributes.Add("onchange", "chktime('" & txtto.ClientID & "','" + CType(e.Row.RowIndex, String) + "')")

            ddlcharge.Attributes.Add("onChange", "disabledbox('" & ddlcharge.ClientID & "','" + txtvalue.ClientID + "','" + txtpercharge.ClientID + "','" + txtchargetype.ClientID + "','" + CType(e.Row.RowIndex, String) + "')")

        End If




    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdRoomrates.Rows.Count + 1

        Dim n As Integer = 0



        Dim lselect(count) As String
        Dim from(count) As String
        Dim tocount(count) As String
        Dim charge(count) As String
        Dim chargetype(count) As String
        Dim conditions(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim days(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim ddlselect As HtmlSelect = GVRow.FindControl("ddlselect")
                Dim txtfrom As TextBox = GVRow.FindControl("txtfrom")
                Dim txtto As TextBox = GVRow.FindControl("txtto")
                Dim ddlcharge As HtmlSelect = GVRow.FindControl("ddlcharge")
                Dim txtchargetype As TextBox = GVRow.FindControl("txtchargetype")
                Dim ddlconditions As HtmlSelect = GVRow.FindControl("ddlconditions")
                Dim txtpercharge As TextBox = GVRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = GVRow.FindControl("txtvalue")
                Dim txtnights As TextBox = GVRow.FindControl("txtnights")
                Dim txtdays As TextBox = GVRow.FindControl("txtdays")





                If chkSelect.Checked = True Then

                    CopyRow = n
                End If

                lselect(n) = CType(ddlselect.Value, String)
                from(n) = CType(txtfrom.Text, String)
                tocount(n) = CType(txtto.Text, String)
                charge(n) = CType(ddlcharge.Value, String)
                chargetype(n) = CType(txtchargetype.Text, String)
                conditions(n) = CType(ddlconditions.Value, String)
                percharge(n) = CType(txtpercharge.Text, String)
                value(n) = CType(txtvalue.Text, String)
                nights(n) = CType(txtnights.Text, String)
                days(n) = CType(txtdays.Text, String)




                txtrmtypcodenew.Add(CType(ddlselect.Value, String))
                txtrmtypnamenew.Add(CType(txtfrom.Text, String))
                txtmealcodenew.Add(CType(txtto.Text, String))
                txtnoofdaysnew.Add(CType(ddlcharge.Value, String))
                txtunitsnew.Add(CType(txtchargetype.Text, String))
                txtchargenew.Add(CType(ddlconditions.Value, String))
                txtperchargenew.Add(CType(txtpercharge.Text, String))
                txtvaluenew.Add(CType(txtvalue.Text, String))
                txtnightsnew.Add(CType(txtnights.Text, String))
                txtdaysnew.Add(CType(txtdays.Text, String))




                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub

    Sub ClearArray()
        txtrmtypcodenew.Clear()
        txtrmtypnamenew.Clear()
        txtmealcodenew.Clear()
        txtunitsnew.Clear()
        txtnoofdaysnew.Clear()
        txtchargenew.Clear()
        txtperchargenew.Clear()
        txtvaluenew.Clear()
        txtnightsnew.Clear()
        txtdaysnew.Clear()



    End Sub
    Sub ClearArraynoshow()
        txtrmtypcodenoshownew.Clear()
        txtrmtypnamenoshownew.Clear()
        txtmealcodenoshownew.Clear()

        ddlnoshow1new.Clear()
        txtchargenoshownew.Clear()
        txtpercnoshownew.Clear()
        txtvaluenoshownew.Clear()
        txtnightsnoshownew.Clear()




    End Sub
    Protected Sub btncopyratesnextrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyratesnextrow.Click



        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdRoomrates.Rows.Count '+ 1


            Dim n As Integer = 0


            For Each GVRow In grdRoomrates.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim ddlselect As HtmlSelect = GVRow.FindControl("ddlselect")
                Dim txtfrom As TextBox = GVRow.FindControl("txtfrom")
                Dim txtto As TextBox = GVRow.FindControl("txtto")
                Dim ddlcharge As HtmlSelect = GVRow.FindControl("ddlcharge")
                Dim txtchargetype As TextBox = GVRow.FindControl("txtchargetype")
                Dim ddlconditions As HtmlSelect = GVRow.FindControl("ddlconditions")
                Dim txtpercharge As TextBox = GVRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = GVRow.FindControl("txtvalue")
                Dim txtnights As TextBox = GVRow.FindControl("txtnights")
                Dim txtdays As TextBox = GVRow.FindControl("txtdays")




                If n > CopyRow And txtchargetype.Text = "" Then

                    ddlselect.Value = txtrmtypcodenew.Item(CopyRow)
                    txtfrom.Text = txtrmtypnamenew.Item(CopyRow)
                    txtto.Text = txtmealcodenew.Item(CopyRow)
                    ddlcharge.Value = txtnoofdaysnew.Item(CopyRow)
                    txtchargetype.Text = txtunitsnew.Item(CopyRow)

                    ddlconditions.Value = txtchargenew.Item(CopyRow)
                    txtpercharge.Text = txtperchargenew.Item(CopyRow)
                    txtvalue.Text = txtvaluenew.Item(CopyRow)
                    txtnights.Text = txtnightsnew.Item(CopyRow)
                    txtdays.Text = txtdaysnew.Item(CopyRow)


                    Exit For

                End If
                n = n + 1
            Next


            Dim gridcopyrow As GridViewRow
            gridcopyrow = grdRoomrates.Rows(n)
            Dim strRowId As String = gridcopyrow.ClientID
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strFoucsColumnIndex As String = "2"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(n, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            CopyClick = 0
            ClearArray()
            ' setdynamicvalues()

            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

        'Dim j As Long = 1
        'Dim txt As TextBox
        'Dim cnt As Long
        'Dim GvRow As GridViewRow

        'Dim srno As Long = 0
        'Dim hotelcategory As String = ""
        'j = 0
        'grdRoomrates.Visible = True
        'cnt = grdRoomrates.Columns.Count
        'Dim n As Long = 0
        'Dim k As Long = 0

        'Dim header As Long = 0
        'Dim heading(cnt + 1) As String
        'Try
        '    For header = 0 To cnt - 8
        '        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
        '        heading(header) = txt.Text
        '    Next
        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try
        'Dim arr_room(header + 1) As String
        'Dim m As Long = 0
        'Dim a As Long = cnt - 10
        'Dim b As Long = 0

        'Dim chk As HtmlInputCheckBox
        'Dim cnt_checked As Long
        ''Dim GvRow1 As GridViewRow
        'Try
        '    For Each GvRow In grdRoomrates.Rows
        '        chk = GvRow.FindControl("ChkSelect")
        '        If chk.Checked = True Then
        '            cnt_checked = cnt_checked + 1
        '        End If
        '    Next
        '    If cnt_checked = 0 Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row');", True)
        '        SetFocus(cnt_checked)
        '        Exit Sub
        '    End If


        '    Dim arr(3) As String
        '    Dim arr_pkg(3) As String
        '    Dim arr_cancdays(3) As String

        '    Dim room As Long = 0
        '    Dim row_id As Long
        '    Dim pkg As Long = 0

        '    For Each GvRow In grdRoomrates.Rows
        '        chk = GvRow.FindControl("ChkSelect")
        '        If n = 0 Then
        '            For j = 0 To cnt - 8
        '                If chk.Checked = True Then
        '                    row_id = GvRow.RowIndex

        '                    If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Then
        '                    Else
        '                        If pkg = 0 Then
        '                            txt = GvRow.FindControl("txt" & b + a + 1)
        '                            If txt Is Nothing Then
        '                            Else
        '                                If txt.Text <> Nothing Then
        '                                    arr_pkg(pkg) = txt.Text
        '                                End If
        '                            End If

        '                            txt = GvRow.FindControl("txt" & b + a + 2)
        '                            If txt Is Nothing Then
        '                            Else
        '                                If txt.Text <> Nothing Then
        '                                    arr_cancdays(pkg) = txt.Text
        '                                End If
        '                            End If

        '                        End If
        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                arr_room(room) = txt.Text
        '                            End If
        '                        End If
        '                        pkg = pkg + 1
        '                        room = room + 1
        '                    End If
        '                End If
        '            Next

        '            m = j
        '            'a = j
        '        Else
        '            k = 0
        '            pkg = 0
        '            For j = n To (m + n) - 1
        '                If chk.Checked = True Then
        '                    row_id = GvRow.RowIndex
        '                    If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Then
        '                    Else
        '                        If pkg = 0 Then
        '                            txt = GvRow.FindControl("txt" & b + a + 1)
        '                            If txt Is Nothing Then
        '                            Else
        '                                If txt.Text <> Nothing Then
        '                                    arr_pkg(pkg) = txt.Text
        '                                End If
        '                            End If

        '                            txt = GvRow.FindControl("txt" & b + a + 2)
        '                            If txt Is Nothing Then
        '                            Else
        '                                If txt.Text <> Nothing Then
        '                                    arr_cancdays(pkg) = txt.Text
        '                                End If
        '                            End If
        '                        End If
        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                arr_room(room) = txt.Text
        '                            End If
        '                        End If
        '                        pkg = pkg + 1
        '                        room = room + 1
        '                    End If
        '                End If
        '                k = k + 1
        '            Next

        '        End If

        '        b = j
        '        n = j
        '    Next
        '    '--------------------------------------------------------------------------------------------
        '    'Noe Fill Record to Cell
        '    room = 0
        '    pkg = 0
        '    n = 0
        '    b = 0

        '    For Each GvRow In grdRoomrates.Rows
        '        chk = GvRow.FindControl("ChkSelect")
        '        If n = 0 Then
        '            For j = 0 To cnt - 8
        '                If GvRow.RowIndex = row_id + 1 Then
        '                    If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Then
        '                    Else
        '                        txt = GvRow.FindControl("txt" & b + a + 1)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                txt.Text = arr_pkg(pkg)
        '                            End If
        '                        End If

        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Enabled = True Then
        '                                txt.Text = arr_room(room)
        '                            End If
        '                        End If

        '                        txt = GvRow.FindControl("txt" & b + a + 2)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                txt.Text = arr_cancdays(pkg)
        '                            End If
        '                        End If
        '                        room = room + 1
        '                    End If
        '                End If
        '            Next
        '            m = j
        '        Else
        '            k = 0
        '            For j = n To (m + n) - 1
        '                If GvRow.RowIndex = row_id + 1 Then
        '                    If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Then
        '                    Else
        '                        txt = GvRow.FindControl("txt" & b + a + 1)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                txt.Text = arr_pkg(pkg)
        '                            End If
        '                        End If

        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Enabled = True Then
        '                                txt.Text = arr_room(room)
        '                            End If
        '                        End If

        '                        txt = GvRow.FindControl("txt" & b + a + 2)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                txt.Text = arr_cancdays(pkg)
        '                            End If
        '                        End If
        '                        room = room + 1
        '                    End If
        '                End If
        '                k = k + 1
        '            Next
        '        End If
        '        b = j
        '        n = j
        '    Next
        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        'End Try
    End Sub





    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function getFillRateType(ByVal prefixText As String) As List(Of String)
        Dim promotionlist As New List(Of String)

        Try
            promotionlist.Add("Free")
            promotionlist.Add("Incl")
            promotionlist.Add("N.Incl")
            promotionlist.Add("N/A")
            promotionlist.Add("On Request")

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getfillchilds(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim roomclasslist As New List(Of String)
        Dim partycode As String
        Try

            strSqlQry = ""

            partycode = Convert.ToString(HttpContext.Current.Session("partycode").ToString())

            strSqlQry = "sp_fillchild '" & partycode & "','" & contextKey & "'"

            ' strSqlQry = "select roomclassname,roomclasscode from  room_classification where active=1  and  roomclassname like  '" & Trim(prefixText) & "%' order by roomclassname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    roomclasslist.Add(myDS.Tables(0).Rows(i)("childs").ToString())
                    'roomclasslist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("childs").ToString(), myDS.Tables(0).Rows(i)("roomclasscode").ToString()))
                Next

            End If

            Return roomclasslist
        Catch ex As Exception
            Return roomclasslist
        End Try

    End Function



    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click
        ' setadddynamicvalues()
        ' rowgridadding("ADD")

        Dim count As Integer
        Dim GVRow As GridViewRow


        count = grdRoomrates.Rows.Count + 1

        Dim n As Integer = 0

        Dim chkSelect As CheckBox

        Dim ddlselect As HtmlSelect
        Dim txtfrom As TextBox
        Dim txtto As TextBox
        Dim ddlcharge As HtmlSelect
        Dim txtchargetype As TextBox
        Dim ddlconditions As HtmlSelect
        Dim txtpercharge As TextBox
        Dim txtvalue As TextBox
        Dim txtnights As TextBox
        Dim txtdays As TextBox


        Dim lselect(count) As String
        Dim from(count) As String
        Dim tocount(count) As String
        Dim charge(count) As String
        Dim chargetype(count) As String
        Dim conditions(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim days(count) As String


        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows

                ddlselect = GVRow.FindControl("ddlselect")
                txtfrom = GVRow.FindControl("txtfrom")
                txtto = GVRow.FindControl("txtto")
                ddlcharge = GVRow.FindControl("ddlcharge")
                txtchargetype = GVRow.FindControl("txtchargetype")
                ddlconditions = GVRow.FindControl("ddlconditions")
                txtpercharge = GVRow.FindControl("txtpercharge")
                txtvalue = GVRow.FindControl("txtvalue")
                txtnights = GVRow.FindControl("txtnights")
                txtdays = GVRow.FindControl("txtdays")


                chkSelect = GVRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                lselect(n) = CType(ddlselect.Value, String)
                from(n) = CType(txtfrom.Text, String)
                tocount(n) = CType(txtto.Text, String)
                charge(n) = CType(ddlcharge.Value, String)
                chargetype(n) = CType(txtchargetype.Text, String)
                conditions(n) = CType(ddlconditions.Value, String)
                percharge(n) = CType(txtpercharge.Text, String)
                value(n) = CType(txtvalue.Text, String)
                nights(n) = CType(txtnights.Text, String)
                days(n) = CType(txtdays.Text, String)



                n = n + 1
            Next
            fillroomgrid(grdRoomrates, False, grdRoomrates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdRoomrates.Rows
                If n = i Then

                    Exit For
                End If

                ddlselect = GVRow.FindControl("ddlselect")
                txtfrom = GVRow.FindControl("txtfrom")
                txtto = GVRow.FindControl("txtto")
                ddlcharge = GVRow.FindControl("ddlcharge")
                txtchargetype = GVRow.FindControl("txtchargetype")
                ddlconditions = GVRow.FindControl("ddlconditions")
                txtpercharge = GVRow.FindControl("txtpercharge")
                txtvalue = GVRow.FindControl("txtvalue")
                txtnights = GVRow.FindControl("txtnights")
                txtdays = GVRow.FindControl("txtdays")

                chkSelect = GVRow.FindControl("chkSelect")


                ddlselect.Value = lselect(n)
                txtfrom.Text = from(n)
                txtto.Text = tocount(n)
                ddlcharge.Value = charge(n)
                txtchargetype.Text = chargetype(n)
                ddlconditions.Value = conditions(n)
                txtpercharge.Text = percharge(n)
                txtvalue.Text = value(n)
                txtnights.Text = nights(n)
                txtdays.Text = days(n)


                n = n + 1

            Next

            Dim gridNewrow As GridViewRow
            gridNewrow = grdRoomrates.Rows(grdRoomrates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strFoucsColumnIndex As String = "2"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdRoomrates.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Private Sub rowgridadding(ByVal mode As String)
        '  Dim dt As New DataTable
        Dim dr As DataRow

        Dim gvRow As GridViewRow

        Dim rowIndex As Integer = 0

        If Session("GV_HotelData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
            Dim drCurrentRow As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                For i As Integer = 1 To dt.Rows.Count
                    'Dim txtname As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtName"), TextBox)
                    'Dim txtprice As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(2).FindControl("txtPrice"), TextBox)

                    Dim txtrmtypcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypcode"), TextBox)
                    Dim txtrmtypname As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(2).FindControl("txtrmtypname"), TextBox)
                    Dim txtmealcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(3).FindControl("txtmealcode"), TextBox)
                    Dim txtnoofdays As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(4).FindControl("txtnoofdays"), TextBox)
                    Dim txtunits As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(5).FindControl("txtunits"), TextBox)
                    Dim txtcharge As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(6).FindControl("txtcharge"), TextBox)
                    Dim txtnights As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(7).FindControl("txtnights"), TextBox)
                    Dim txtpercharge As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(8).FindControl("txtpercharge"), TextBox)
                    Dim txtvalue As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(9).FindControl("txtvalue"), TextBox)




                    drCurrentRow = dt.NewRow()



                    dt.Rows(i - 1)("RoomTypecode") = txtrmtypcode.Text
                    dt.Rows(i - 1)("Select_RoomType") = txtrmtypname.Text
                    dt.Rows(i - 1)("Meal_Plan") = txtmealcode.Text
                    dt.Rows(i - 1)("NoofDays") = txtnoofdays.Text
                    dt.Rows(i - 1)("Units") = txtunits.Text
                    dt.Rows(i - 1)("Charge") = txtcharge.Text
                    dt.Rows(i - 1)("Nights") = txtnights.Text
                    dt.Rows(i - 1)("Percentage") = txtpercharge.Text
                    dt.Rows(i - 1)("Value") = txtvalue.Text



                    rowIndex += 1
                Next
                dt.Rows.Add(drCurrentRow)
                Session("GV_HotelData") = dt
                grdRoomrates.DataSource = dt
                grdRoomrates.DataBind()
            End If

        End If





        grdRoomrates.DataSource = dt
        grdRoomrates.DataBind()

        SetOldData()
        '  setadddynamicvalues()
        '

        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

    End Sub
    Private Sub SetOldData()
        Dim rowIndex As Integer = 0
        If Session("GV_HotelData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtrmtypcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypcode"), TextBox)
                    Dim txtrmtypname As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypname"), TextBox)
                    Dim txtmealcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmealcode"), TextBox)
                    Dim txtnoofdays As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnoofdays"), TextBox)
                    Dim txtunits As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtunits"), TextBox)
                    Dim txtcharge As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtcharge"), TextBox)
                    Dim txtnights As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnights"), TextBox)
                    Dim txtpercharge As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtpercharge"), TextBox)
                    Dim txtvalue As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtvalue"), TextBox)



                    txtrmtypcode.Text = dt.Rows(i)("RoomTypecode").ToString()
                    txtrmtypname.Text = dt.Rows(i)("Select_RoomType").ToString()
                    txtmealcode.Text = dt.Rows(i)("Meal_Plan").ToString()
                    txtnoofdays.Text = dt.Rows(i)("NoofDays").ToString()
                    txtunits.Text = dt.Rows(i)("Units").ToString()
                    txtcharge.Text = dt.Rows(i)("Charge").ToString()
                    txtnights.Text = dt.Rows(i)("Nights").ToString()
                    txtpercharge.Text = dt.Rows(i)("Percentage").ToString()
                    txtvalue.Text = dt.Rows(i)("Value").ToString()



                    rowIndex += 1
                Next
            End If
        End If
    End Sub


    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdRoomrates.Rows.Count + 1


        Dim n As Integer = 0

        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0


        Dim ddlselect As HtmlSelect
        Dim txtfrom As TextBox
        Dim txtto As TextBox
        Dim ddlcharge As HtmlSelect
        Dim txtchargetype As TextBox
        Dim ddlconditions As HtmlSelect
        Dim txtpercharge As TextBox
        Dim txtvalue As TextBox
        Dim txtnights As TextBox
        Dim txtdays As TextBox


        Dim lselect(count) As String
        Dim from(count) As String
        Dim tocount(count) As String
        Dim charge(count) As String
        Dim chargetype(count) As String
        Dim conditions(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim days(count) As String

        Try
            For Each GVRow In grdRoomrates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    ddlselect = GVRow.FindControl("ddlselect")
                    txtfrom = GVRow.FindControl("txtfrom")
                    txtto = GVRow.FindControl("txtto")
                    ddlcharge = GVRow.FindControl("ddlcharge")
                    txtchargetype = GVRow.FindControl("txtchargetype")
                    ddlconditions = GVRow.FindControl("ddlconditions")
                    txtpercharge = GVRow.FindControl("txtpercharge")
                    txtvalue = GVRow.FindControl("txtvalue")
                    txtnights = GVRow.FindControl("txtnights")
                    txtdays = GVRow.FindControl("txtdays")


                    lselect(n) = CType(ddlselect.Value, String)
                    from(n) = CType(txtfrom.Text, String)
                    tocount(n) = CType(txtto.Text, String)
                    charge(n) = CType(ddlcharge.Value, String)
                    chargetype(n) = CType(txtchargetype.Text, String)
                    conditions(n) = CType(ddlconditions.Value, String)
                    percharge(n) = CType(txtpercharge.Text, String)
                    value(n) = CType(txtvalue.Text, String)
                    nights(n) = CType(txtnights.Text, String)
                    days(n) = CType(txtdays.Text, String)



                    n = n + 1

                Else
                    deletedrow = deletedrow + 1

                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillroomgrid(grdRoomrates, False, count)


            Dim i As Integer = n
            n = 0

            For Each GVRow In grdRoomrates.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then


                    ddlselect = GVRow.FindControl("ddlselect")
                    txtfrom = GVRow.FindControl("txtfrom")
                    txtto = GVRow.FindControl("txtto")
                    ddlcharge = GVRow.FindControl("ddlcharge")
                    txtchargetype = GVRow.FindControl("txtchargetype")
                    ddlconditions = GVRow.FindControl("ddlconditions")
                    txtpercharge = GVRow.FindControl("txtpercharge")
                    txtvalue = GVRow.FindControl("txtvalue")
                    txtnights = GVRow.FindControl("txtnights")
                    txtdays = GVRow.FindControl("txtdays")



                    ddlselect.Value = lselect(n)
                    txtfrom.Text = from(n)
                    txtto.Text = tocount(n)
                    ddlcharge.Value = charge(n)
                    txtchargetype.Text = chargetype(n)
                    ddlconditions.Value = conditions(n)
                    txtpercharge.Text = percharge(n)
                    txtvalue.Text = value(n)
                    txtnights.Text = nights(n)
                    txtdays.Text = days(n)


                    n = n + 1
                End If
            Next


            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub




    Protected Sub btnRemark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemark.Click
        'If ValidatePage() = False Then
        '    Exit Sub
        'End If



        GenRemark()
        disablegrid()

    End Sub
    Private Sub Enablegrid()
        For Each gvRow In grdRoomrates.Rows

            Dim ddlselect As HtmlSelect = gvRow.FindControl("ddlselect")
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")
            Dim ddlcharge As HtmlSelect = gvRow.FindControl("ddlcharge")
            Dim txtchargetype As TextBox = gvRow.FindControl("txtchargetype")
            Dim ddlconditions As HtmlSelect = gvRow.FindControl("ddlconditions")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtdays As TextBox = gvRow.FindControl("txtdays")


            ddlselect.Disabled = False
            txtfrom.Enabled = True
            txtto.Enabled = True
            ddlcharge.Disabled = False
            txtchargetype.Enabled = True
            ddlconditions.Disabled = False
            txtnights.Enabled = True
            txtpercharge.Enabled = True
            txtvalue.Enabled = True
            txtdays.Enabled = True

            If ddlcharge.Value = "No" Then

                txtvalue.Enabled = False
                txtpercharge.Enabled = False
                txtchargetype.Enabled = False
            Else

                txtvalue.Enabled = True
                txtpercharge.Enabled = True
                txtchargetype.Enabled = True
            End If

            If txtchargetype.Text <> "" And (txtchargetype.Text = "% of Nights") Then
                txtpercharge.Enabled = True

                txtvalue.Enabled = False

            End If


            If txtchargetype.Text <> "" And (txtchargetype.Text = "Value") Then
                txtvalue.Enabled = True

                txtpercharge.Enabled = False

            End If





        Next

        If ddlcheckin.SelectedIndex = 1 Then
            txtchkintime.Enabled = False
        Else
            txtchkintime.Enabled = True
        End If
        If ddlcheckout.SelectedIndex = 1 Then
            txtChkouttime.Enabled = False
        Else
            txtChkouttime.Enabled = True
        End If



    End Sub
    Private Sub disablegrid()

        For Each gvRow In grdRoomrates.Rows

            Dim ddlselect As HtmlSelect = gvRow.FindControl("ddlselect")
            Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
            Dim txtto As TextBox = gvRow.FindControl("txtto")
            Dim ddlcharge As HtmlSelect = gvRow.FindControl("ddlcharge")
            Dim txtchargetype As TextBox = gvRow.FindControl("txtchargetype")
            Dim ddlconditions As HtmlSelect = gvRow.FindControl("ddlconditions")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtdays As TextBox = gvRow.FindControl("txtdays")


            ddlselect.Disabled = True
            txtfrom.Enabled = False
            txtto.Enabled = False
            ddlcharge.Disabled = True
            txtchargetype.Enabled = False
            ddlconditions.Disabled = True

            txtpercharge.Enabled = False
            txtvalue.Enabled = False
            txtdays.Enabled = False



        Next


    End Sub
#Region "Public Sub GenRemark()"
    Public Sub GenRemark()

        Dim strRemark As String = ""

        Dim basecurrency As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")

        Dim flgEB As Boolean = True

        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Dim roomname As String = ""
        Dim mealname As String = ""

        For Each gvRow As GridViewRow In gv_Seasons.Rows
            chk2 = gvRow.FindControl("chkseason")
            txtmealcode1 = gvRow.FindControl("txtseasoncode")

            If chk2.Checked = True Then
                seasonname = seasonname + txtmealcode1.Text + ","

            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If


        For Each gvRow As GridViewRow In grdroomtype.Rows
            chk2 = gvRow.FindControl("chkrmtyp")
            'txtmealcode1 = gvRow.Cells(2).Text.Trim      ' gvRow.FindControl("txtrmtypcode")

            If chk2.Checked = True Then
                roomname = roomname + gvRow.Cells(2).Text.Trim + ","

            End If
        Next

        If roomname.Length > 0 Then
            roomname = roomname.Substring(0, roomname.Length - 1)
        End If

        For Each gvRow As GridViewRow In grdmealplan.Rows
            chk2 = gvRow.FindControl("chkmeal")
            txtmealcode1 = gvRow.FindControl("txtmealcode")

            If chk2.Checked = True Then
                mealname = mealname + txtmealcode1.Text + ","

            End If
        Next

        If mealname.Length > 0 Then
            mealname = mealname.Substring(0, mealname.Length - 1)
        End If



        strRemark = "Check In Check Out Policy for "

        strRemark = strRemark + seasonname + " Seasons ." + vbCrLf

        strRemark = strRemark + "Applicable to - " + roomname + " On " + mealname + vbCrLf

        If ddlcheckin.Value = "Fixed Time" Then
            strRemark = strRemark + "Check In Time : " + txtchkintime.Text + " ." + vbCrLf
        Else
            strRemark = strRemark + "Check In Time : " + ddlcheckin.Value + " ." + vbCrLf
        End If
        If ddlcheckout.Value = "Fixed Time" Then
            strRemark = strRemark + "Check Out Time : " + txtChkouttime.Text + " ." + vbCrLf
        Else
            strRemark = strRemark + "Check Out Time : " + ddlcheckout.Value + " ." + vbCrLf
        End If


        ' strRemark = strRemark & "For" & " " & (roomtype) & " -- occupancies" & " " & roomcategory & vbCrLf
        ' - "occupancies" & (roomcategory)
        If flgEB = True Then
            For Each gvRow In grdRoomrates.Rows

                Dim ddlselect As HtmlSelect = gvRow.FindControl("ddlselect")
                Dim txtfrom As TextBox = gvRow.FindControl("txtfrom")
                Dim txtto As TextBox = gvRow.FindControl("txtto")
                Dim ddlcharge As HtmlSelect = gvRow.FindControl("ddlcharge")
                Dim txtchargetype As TextBox = gvRow.FindControl("txtchargetype")
                Dim ddlconditions As HtmlSelect = gvRow.FindControl("ddlconditions")
                Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
                Dim txtnights As TextBox = gvRow.FindControl("txtnights")
                Dim txtdays As TextBox = gvRow.FindControl("txtdays")


                If ddlselect.Value <> "" And txtfrom.Text <> "" Then

                    strRemark = strRemark + vbCrLf + ddlselect.Value

                    If ddlcharge.Value = "No" Then
                        strRemark = strRemark + " will not be charged extra."
                    Else
                        strRemark = strRemark + " From : " + txtfrom.Text + " To :" + txtto.Text + " will be charged."
                    End If


                    If txtchargetype.Text.ToUpper = UCase("% of Nights") Then
                        strRemark = strRemark & " " + txtpercharge.Text + " % of per night."


                    ElseIf txtchargetype.Text.ToUpper = "Value" Then
                        strRemark = strRemark & " " + basecurrency + " " + (txtvalue.Text) + "."
                    ElseIf txtchargetype.Text.ToUpper = "Rate TBA" Then
                        strRemark = strRemark & " extra.Rate to be advised later." + vbCrLf
                    End If

                    If ddlconditions.Value = "Subject to Availability" Then
                        strRemark = strRemark + " This request is subject to availability"
                    Else
                        strRemark = strRemark + "This request will be guaranteed by the hotel."
                    End If
                    If Val(txtdays.Text) <> 0 Or txtdays.Text <> "" Then
                        strRemark = strRemark + " and the request has to be made before " + txtdays.Text + " days."
                    End If

                    strRemark = strRemark

                End If

            Next

            For Each gvRow In grdexdates.Rows
                Dim ddlExcl As HtmlSelect = gvRow.Findcontrol("ddlExcl")
                Dim txtfromDate As TextBox = gvRow.findcontrol("txtfromDate")

                If ddlExcl.Value <> "" And ddlExcl.Value = "Checkin" Then
                    strRemark = strRemark + vbCrLf + "Check In will not be allowed on " + txtfromDate.Text + "."
                ElseIf ddlExcl.Value <> "" And ddlExcl.Value = "Checkout" Then
                    strRemark = strRemark + vbCrLf + "Check Out will not be allowed on " + txtfromDate.Text + "."
                ElseIf ddlExcl.Value <> "" And ddlExcl.Value = "Both" Then
                    strRemark = strRemark + vbCrLf + "Check In & Check Out will not be allowed on " + txtfromDate.Text + "."
                End If


            Next



        End If



        txtViewPolicy.Value = strRemark

    End Sub
#End Region

    Protected Sub btnClearPolicy_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearPolicy.Click
        Me.txtViewPolicy.Value = ""
        Enablegrid()
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
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try

            If Session("Calledfrom") = "Offers" Then


                strSqlQry = "select h.contractid,h.checkinoutpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
               " h.applicableto     from view_contracts_checkinout_header h(nolock),view_contracts_search d(nolock)  where  isnull(d.withdraw,0)=0  and h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "'"

            Else
                strSqlQry = "select h.contractid,h.checkinoutpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
               " h.applicableto     from view_contracts_checkinout_header h(nolock),view_contracts_search d(nolock)  where isnull(d.withdraw,0)=0  and h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"

            End If


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
            objUtils.WritErrorLog("ContractChkInOutpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
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
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"



                If Session("Calledfrom") = "Offers" Then
                    divoffer.Style.Add("display", "block")
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()
                    Showdetailsgrid(CType(lbltran.Text.Trim, String))
                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(hdnpromotionid.Value, String))
                    FillRoomtypemealplanoffer(hdnpromotionid.Value)

                    lblHeading.Text = "Copy CheckIn/Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "CheckIn/Out Policy "

                Else
                    wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                    wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
                    ShowRecordcopy(CType(lbltran.Text.Trim, String))

                    Showdetailsgrid(CType(lbltran.Text.Trim, String))
                    ' wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                    FillRoomtypemealplan()
                    wucCountrygroup.sbShowCountry()

                    btnSave.Visible = True
                    txtplistcode.Text = ""
                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy CheckIn/Out Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "CheckIn/Out Policy "
                    fillseason()
                    divoffer.Style.Add("display", "none")
                End If
                btnSave.Visible = True
                '  wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))





            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub grdexdates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdexdates.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            Dim strGridName As String = grdexdates.ClientID
            Dim strRowId As String = e.Row.RowIndex
            Dim strFoucsColumnIndex = "1"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        End If
    End Sub
    Private Sub sortgvsearch()
        If Session("Calledfrom") = "Offers" Then
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("promotionid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("frmdate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("todate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("h.applicableto", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("h.adddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.adduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.moddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        Else
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("h.promotionid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("frmdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        End If

    End Sub
    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub grdpromotion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdpromotion.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lblpromotionid As Label
            Dim lblpromotionname As Label, lblapplicable As Label, lblplistcode As Label

            lblpromotionid = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            lblpromotionname = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionname")
            lblapplicable = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapplicableto")
            lblplistcode = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            If lblpromotionid.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then
                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)

                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lblplistcode.Text.Trim, Nothing, ViewState("State"))
                fillDategrd(grdexdates, True)
                ShowRecord(CType(lblplistcode.Text.Trim, String))



                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                If Session("Calledfrom") = "Offers" Then
                    ShowDatesnew(CType(hdnpromotionid.Value, String))
                    FillRoomtypemealplanoffer(hdnpromotionid.Value)
                    Showdetailsgrid(CType(lblplistcode.Text.Trim, String))
                    lblHeading.Text = "Copy Check In/Check Out Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                End If
                Page.Title = " Check In/Check Out Policy "


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChkInOutPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class

