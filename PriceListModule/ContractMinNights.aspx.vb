
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractMinNights
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

    Dim ddloptionsnew As New ArrayList
    Dim txtminnightsnew As New ArrayList
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList

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


    Dim iCurrecntIndex As Integer = 20

   

#End Region
#Region "Enum GridCol"
    Enum GridCol
        tranid = 1
        Fromdate = 2
        Todate = 3
        applicableto = 4
        linkcode = 5
        Edit = 6
        View = 7
        Delete = 8
        Copy = 9
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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex


        ' FillGrid("h.minnightsid", hdncontractid.Value, hdnpartycode.Value, "DESC")
        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("h.minnightsid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("frmdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("isnull(h.linkcode,'')", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 7
                FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 8
                FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select


    End Sub

#End Region
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            '   grdpromotion.Visible = True


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

            strSqlQry = " select c.exhibitionid plistcode, h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_exhibition_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by h.promotionid,h.approved,h.promotionname,c.exhibitionid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

            'strSqlQry = " select h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,  " _
            '    & " case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status from view_offers_header h(nolock),view_offers_detail d(nolock) where isnull(h.commissiontype,'')='Special commissionable Rates' and  h.promotionid<>'" & hdnpromotionid.Value & "' and  h.promotionid =d.promotionid   " _
            '    & " and h.partycode='" + hdnpartycode.Value.Trim + "'  " + filterCond + " group by h.promotionid,h.approved  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                'grdpromotion.DataSource = MyDs
                'grdpromotion.DataBind()
                'grdpromotion.Visible = True
            Else
                '  grdpromotion.Visible = False
            End If

            '  ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String

        Dim CalledfromValue As String = ""

        Dim Minappid As String = ""
        Dim Minappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""
        If IsPostBack = False Then
            Minappid = 1
            Minappname = objUser.GetAppName(Session("dbconnectionName"), Minappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else


                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                CalledfromValue = Me.SubMenuUserControl1.menuidval
                objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(Minappname, String), "ContractMinNights.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
             btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractMinNights.aspx", Minappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, Minappid, intMenuID)

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


            If Session("Calledfrom") = "Offers" Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))


                txtconnection.Value = Session("dbconnectionName")
              
                hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")
                divoffer.Style.Add("display", "block")
                btncopycontract.Style.Add("display", "block")
                btnselect.Style.Add("display", "block")

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
             

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)

                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionidnew.Text = Session("OfferRefCode")
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


                End If
                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Page.Title = "Minimum Nights"


                FillGrid("h.minnightsid", hdnpromotionid.Value, hdnpartycode.Value, "DESC")


            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))


                txtconnection.Value = Session("dbconnectionName")
                'txtfromDate.Text = Now.ToString("dd/MM/yyyy")
                'txtToDate.Text = Now.ToString("dd/MM/yyyy")


                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
                'Session("contractid") = hdncontractid.Value
                'Session("partycode") = hdnpartycode.Value

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                '  wucCountrygroup.Visible = False

                wucCountrygroup.sbSetPageState("", "CONTRACTMINNIGHTS", CType(Session("ContractState"), String))
                divoffer.Style.Add("display", "none")


                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = lblHeading.Text + " - " + ViewState("hotelname") + " - " + hdncontractid.Value

                Page.Title = "Minimum Nights"


                FillGrid("h.minnightsid", hdncontractid.Value, hdnpartycode.Value, "DESC")
            End If

           
        
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

        btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")

        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            ' Dim txtbox As TextBox
            Dim roomtypes As String
            Dim rmtypname As String

            '   roomtypes = getroomtypes()

            Dim chkSelect As CheckBox

            Dim strChkdStringVal As StringBuilder = New StringBuilder()
            Dim tickedornot As Boolean

            tickedornot = False
            For Each grdRow In gv_Showroomtypes.Rows
                chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                chkSelect = grdRow.FindControl("chkrmtype")
                If chkSelect.Checked = True Then
                    tickedornot = True
                    Exit For
                End If
            Next

            If tickedornot = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Row');", True)
                ModalExtraPopup.Show()
                Exit Sub
            End If



            Dim chk2 As CheckBox
            Dim txtrmtypcode1 As Label
            Dim lblrmtypname As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode1 = gvRow.FindControl("txtrmtypcode")
                lblrmtypname = gvRow.FindControl("lblrmtypname")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode1.Text + ","
                    rmtypname = rmtypname + lblrmtypname.Text + ","

                End If
            Next

            If roomtypes Is Nothing Then
                roomtypes = " "

            Else
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
                rmtypname = rmtypname.Substring(0, rmtypname.Length - 1)
            End If

            If gv_Showroomtypes.HeaderRow.Cells(3).Text = "Meal Plan" Then
                If hdnMainGridRowid.Value <> "" Then


                    Dim txtmealcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                    txtmealcode.Text = roomtypes.ToString


                End If
            Else
                If hdnMainGridRowid.Value <> "" Then

                    Dim txtrmtypname As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                    Dim txtrmtypcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                    txtrmtypname.Text = rmtypname.ToString
                    txtrmtypcode.Text = roomtypes.ToString





                End If
            End If


            ViewState("noshowclick") = Nothing

            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function getfillunits(ByVal prefixText As String) As List(Of String)
        Dim unitlist As New List(Of String)

        Try
            unitlist.Add("Days")
            unitlist.Add("Hours")


            Return unitlist
        Catch ex As Exception
            Return unitlist
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getfillchargetype(ByVal prefixText As String) As List(Of String)
        Dim chargelist As New List(Of String)

        Try
            chargelist.Add("Nights")
            chargelist.Add("% of Nights")
            chargelist.Add("% of Entire Stay")
            chargelist.Add("Value")


            Return chargelist
        Catch ex As Exception
            Return chargelist
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
    Protected Sub btnClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear1.Click
        ViewState("noshowclick") = Nothing
        ModalExtraPopup.Hide()
    End Sub
    Sub FillRoomdetails()
        createdatatable()

        grdRoomrates.Visible = True

        lable12.Visible = True
        btncopyratesnextrow.Visible = True
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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub






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
            lngcnt = 2
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





    Private Sub FillGrid(ByVal StrOrderby As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            'strSqlQry = "select h.plistcode,h.subseascode as seasoncode,convert(varchar(10),min(cs.fromdate),103) frmdate, convert(varchar(10),max(cs.todate),103) todate,h.applicableto as countrygroups,'' DaysoftheWeek,h.adddate,h.adduser," & _
            '    " h.moddate ,h.moduser  from cplisthnew h, contracts_seasons cs  where h.contractid=cs.contractid and  h.subseascode =cs.subseasname and h.contractid='" & contractid & _
            '    "' group by h.plistcode,h.subseascode,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser"
            If StrOrderby = "frmdate" Or StrOrderby = "todate" Then
                strSqlQry = "with ctee as(select h.minnightsid tranid,convert(varchar(10),cast(min(d.fromdate)as datetime),103)  frmdate, convert(varchar(10),cast(max(d.todate)as datetime),103) todate,h.applicableto,isnull(h.linkcode,'') linkcode, h.adddate,h.adduser,h.moddate ,h.moduser  from view_contracts_minnights_header h,view_contracts_minnights_detail d " _
                          & " where h.minnightsid=d.minnightsid and h.contractid='" & hdncontractid.Value & "'  group by h.minnightsid,h.applicableto,h.linkcode,h.adddate,h.adduser,h.moddate ,h.moduser ) select * from ctee order by convert(datetime, " & StrOrderby & ",103) " & strsortorder & ""
            Else


                strSqlQry = "select h.minnightsid tranid,convert(varchar(10),cast(min(d.fromdate)as datetime),103)  frmdate, convert(varchar(10),cast(max(d.todate)as datetime),103) todate,h.applicableto,isnull(h.linkcode,'') linkcode,h.adddate,h.adduser,h.moddate ,h.moduser  from view_contracts_minnights_header h,view_contracts_minnights_detail d " _
                            & " where h.minnightsid=d.minnightsid and h.contractid='" & hdncontractid.Value & "'  group by h.minnightsid,h.applicableto,h.linkcode,h.adddate,h.adduser,h.moddate ,h.moduser order by " & StrOrderby & " " & strsortorder & ""
            End If
            'strSqlQry = "select '' tranid,'' frmdate,'' todate,'' countrygroups,'' adddate, '' adduser, '' moddate, '' moduser"


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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub





    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable Should not be Blank');", True)
            ValidateSave = False
            Exit Function
        End If

        'Dim tickedornot As Boolean = False
        'Dim chkSelect As CheckBox
        'tickedornot = False
        'For Each grdRow In gv_Seasons.Rows
        '    chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)

        '    If chkSelect.Checked = True Then
        '        tickedornot = True
        '        Exit For
        '    End If
        'Next

        'If tickedornot = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Season');", True)
        '    ValidateSave = False
        '    Exit Function
        'End If

        'Dim seasonname As String = ""
        'Dim chk2 As CheckBox
        'Dim txtmealcode1 As Label
        'Session("seasons") = Nothing

        'For Each grdRow As GridViewRow In gv_Seasons.Rows
        '    chk2 = grdRow.FindControl("chkseason")
        '    txtmealcode1 = grdRow.FindControl("txtseasoncode")

        '    If chk2.Checked = True Then
        '        seasonname = seasonname + txtmealcode1.Text + ","

        '    End If
        'Next

        'If seasonname.Length > 0 Then
        '    seasonname = seasonname.Substring(0, seasonname.Length - 1)
        'End If

        'Session("seasons") = seasonname

        Dim contracttodate As String

        Dim lnCnt As Integer = 0
        'Dim lnCntnoshow As Integer = 0

        'Dim ToDt As Date = Nothing
        Dim flg As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1

            Dim txtrmtypcode As TextBox = GvRow.findcontrol("txtrmtypcode")
            Dim txtfromDate As TextBox = gvRow.FindControl("txtfromDate")
            Dim txtToDate As TextBox = gvRow.FindControl("txtToDate")
            Dim txtminnights As TextBox = gvRow.FindControl("txtminnights")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

            Dim ddloptions As HtmlSelect = gvRow.FindControl("ddloptions")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")


            If txtfromDate.Text <> "" And txtToDate.Text <> "" Then
                flg = True
            End If


            If txtfromDate.Text <> "" And txtToDate.Text <> "" Then


                If txtmealcode.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If
                If txtfromDate.Text = "" Or txtToDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date :" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtfromDate)
                    Exit Function
                End If
                If txtfromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") < Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Date.Row No :" & lnCnt & "');", True)
                        SetFocus(txtfromDate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period.Row No :" & lnCnt & "');", True)
                        SetFocus(txtfromDate)
                        ValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value)) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belongs to the Contracts Period.Row No :" & lnCnt & "');", True)
                        SetFocus(txtToDate)
                        ValidateSave = False
                        Exit Function
                    End If

                End If

                If txtminnights.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Minnights Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If



            End If



        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one  Row   :" & lnCnt & "');", True)
            ValidateSave = False
            Exit Function
        End If



        ValidateSave = True
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try

            '   CopyRow = 0

            Dim chksel As CheckBox
            Dim weekdaystr As String = ""


            For Each GVRow In grdWeekDays.Rows
                chksel = GVRow.FindControl("chkSelect")

                If chksel.Checked = True Then
                    weekdaystr = weekdaystr + "," + GVRow.Cells(2).Text

                End If
            Next
            If weekdaystr.Length > 0 Then
                weekdaystr = Right(weekdaystr, Len(weekdaystr) - 1) 'mealplanstr
            End If

            If weekdaystr.Length = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Week Days Should not be Empty.');", True)
                FindDatePeriod = False
                Exit Function

            End If


            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            'For Each GVRow In grdDates.Rows



            '    If GVRow.Cells(1).Text <> "" Then

            For Each GVRow In grdRoomrates.Rows

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtfromDate As TextBox = GVRow.FindControl("txtfromDate")
                Dim txtToDate As TextBox = GVRow.FindControl("txtToDate")
                Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")

                Dim ddloptions As HtmlSelect = GVRow.FindControl("ddloptions")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")

                If txtfromDate.Text <> "" And txtToDate.Text <> "" Then

                    Dim ds As DataSet
                    Dim parms2 As New List(Of SqlParameter)
                    Dim parm2(11) As SqlParameter



                    parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))
                    'parm2(4) = New SqlParameter("@subseasoncode", CType(GVRow.Cells(3).Text, String))
                    parm2(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(7) = New SqlParameter("@promotionid", "")
                    parm2(8) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                    parm2(9) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))
                    parm2(10) = New SqlParameter("@weekdays", CType(weekdaystr, String))


                    For i = 0 To 10
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkMinnights", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("minnightsid")) = False Then
                                strMsg = "Minimum Nights already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("minnightsid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
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
            objUtils.WritErrorLog("Contractminnights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
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

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start


                    ' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_minnights", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    ' '''''''''''''''''''''''


                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("MINNIGHTS", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_minnights_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@linkcode", SqlDbType.VarChar, 20)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed

                    ElseIf ViewState("State") = "Edit" Then

                        Dim linkcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(linkcode,'') from view_contracts_minnights_header where minnightsid='" & CType(txtplistcode.Text.Trim, String) & "'")


                        mySqlCmd = New SqlCommand("sp_mod_contracts_minnights_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@linkcode", SqlDbType.VarChar, 20)).Value = CType(linkcode, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_MLOS Where MLOS_ID='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_MLOS_weekdays Where  MLOS_ID='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    '------------------------------------Inserting Data weekdays
                    Dim GvRow As GridViewRow
                    For Each GvRow In grdWeekDays.Rows
                        Dim lblorder As Label = GvRow.FindControl("lblSrNo")
                        Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")
                        If chkSelect.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_edit_MLOS_weekdays", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@MLOS_ID", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@dayoftheweek", SqlDbType.VarChar, 30)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekorder", SqlDbType.Int, 4)).Value = CType(lblorder.Text.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_minnights_agents  Where minnightsid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_minnights_countries Where minnightsid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then


                                mySqlCmd = New SqlCommand("sp_add_contracts_minnights_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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

                                mySqlCmd = New SqlCommand("sp_add_contracts_minnights_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                    End If
                    Dim arrOcpncy As String()

                    Dim m As Integer = 1
                    For Each GvRow In grdRoomrates.Rows
                        Dim txtrmtypcode As TextBox = GvRow.findcontrol("txtrmtypcode")
                        Dim txtfromDate As TextBox = GvRow.findcontrol("txtfromDate")
                        Dim txtToDate As TextBox = GvRow.findcontrol("txtToDate")
                        Dim txtminnights As TextBox = GvRow.findcontrol("txtminnights")
                        Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                        Dim txtrmtypname As TextBox = GvRow.findcontrol("txtrmtypname")

                        Dim ddloptions As HtmlSelect = GvRow.findcontrol("ddloptions")
                        Dim ddlweekoptions As HtmlSelect = GvRow.findcontrol("ddlweekoptions")

                        If txtfromDate.Text <> "" And txtToDate.Text <> "" Then

                            If CType(txtrmtypcode.Text, String) = "" And CType(txtrmtypname.Text, String) = "All" Then
                                Dim rmtypcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + rmtypcode from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                                arrOcpncy = rmtypcode.ToString.Trim.Split(",")
                            Else
                                arrOcpncy = txtrmtypcode.Text.ToString.Trim.Split(",")
                            End If



                            For i = 0 To arrOcpncy.Length - 1
                                If arrOcpncy(i) <> "" Then

                                    Dim mealcode As String() = txtmealcode.Text.ToString.Trim.Split(",")
                                    For j = 0 To mealcode.Length - 1

                                        If mealcode(j) <> "" Then

                                            Dim rmcatcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_partymaxaccomodation  u(nolock) where u.rmtypcode='" & CType(arrOcpncy(i), String) & "' and  u.partycode ='" & CType(hdnpartycode.Value.Trim, String) & "'  for xml path('')),1,1,'' ) ")

                                            Dim rmcatcode As String() = rmcatcode1.ToString.Trim.Split(",")

                                            For k = 0 To rmcatcode.Length - 1

                                                If rmcatcode(k) <> "" Then


                                                    mySqlCmd = New SqlCommand("sp_add_contracts_minnights_detail", mySqlConn, sqlTrans)
                                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                    If CType(txtrmtypcode.Text, String) = "" And CType(txtrmtypname.Text, String) = "All" Then
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypname.Text, String)
                                                    Else
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text, String)
                                                    End If

                                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd"), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@minnights", SqlDbType.Int)).Value = CType(txtminnights.Text, Integer)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@nightsoption", SqlDbType.VarChar, 50)).Value = CType(ddloptions.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@weekoption", SqlDbType.VarChar, 50)).Value = CType(ddlweekoptions.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@Contract_ID", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@Promotion_ID", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 100)).Value = CType(arrOcpncy(i), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(rmcatcode(k), String)
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(mealcode(j), String)
                                                    mySqlCmd.ExecuteNonQuery()
                                                    mySqlCmd.Dispose()



                                                End If

                                            Next

                                        End If

                                    Next
                                End If
                            Next

                            m = m + 1




                        End If

                            Next


                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_minnights_header"
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

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_minnights", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""



                    'mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd.Dispose()
                    ' '''''''''''''''''''''''


                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_contracts_minnights_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@minnightsid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            btnAddrow.Enabled = True
            btndeleterow.Enabled = True

            grdRoomrates.Enabled = True
            txtApplicableTo.Enabled = True
            txtplistcode.Text = ""
            wucCountrygroup.Disable(True)
        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then



            grdRoomrates.Enabled = False

            btncopyratesnextrow.Enabled = False
            btnfillrate.Enabled = False
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False




        ElseIf ViewState("State") = "Edit" Then


            grdRoomrates.Enabled = True

            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)


        End If
    End Sub





    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select min(c.applicableto)applicableto,min(a.seasonname) seasonname,min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
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





                mySqlCmd.Dispose()
                mySqlReader.Close()

                mySqlConn.Close()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_minnights_header(nolock) Where minnightsid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("minnightsid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("minnightsid"), String)
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




                    'If IsDBNull(mySqlReader("seasons")) = False Then
                    '    Session("seasons") = CType(mySqlReader("seasons"), String)
                    'End If
                    'Dim strMealPlans As String = ""
                    'Dim strCondition As String = ""
                    'If Session("seasons") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    '    strMealPlans = Session("seasons") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                    '    If strMealPlans.Length > 0 Then
                    '        Dim mString As String() = strMealPlans.Split(",")
                    '        For i As Integer = 0 To mString.Length - 1
                    '            If strCondition = "" Then
                    '                strCondition = "'" & mString(i) & "'"
                    '            Else
                    '                strCondition &= ",'" & mString(i) & "'"
                    '            End If
                    '        Next
                    '    End If
                    'End If

                    'Dim myDS As New DataSet
                    'gv_Seasons.Visible = True
                    'strSqlQry = ""

                    'strSqlQry = "select  distinct seasonname seasonname,0 selected from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname Not IN (" & strCondition & ") " _
                    '  & " union all " _
                    '    & " select  distinct seasonname subseasname,1 selected  from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  1 "

                    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    'myDataAdapter.Fill(myDS)
                    'gv_Seasons.DataSource = myDS

                    'If myDS.Tables(0).Rows.Count > 0 Then
                    '    gv_Seasons.DataBind()


                    'Else
                    '    gv_Seasons.DataBind()

                    'End If
                    'Dim chkSelect As CheckBox
                    'Dim lblselect As Label
                    'For Each grdRow In gv_Seasons.Rows
                    '    chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                    '    lblselect = CType(grdRow.FindControl("lblselect"), Label)

                    '    If lblselect.Text = "1" Then
                    '        chkSelect.Checked = True
                    '        ' chkSelect.Enabled = False
                    '    End If

                    'Next
                    'filldates()



                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_minnights_header(nolock) where  minnightsid ='" & CType(RefCode, String) & "'") <> "" Then
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
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdRoomrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0


            strQry = "select count( distinct clineno) from view_contracts_minnights_detail(nolock) where minnightsid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            fillDategrd(grdRoomrates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select distinct * from view_contracts_minnights_detail d(nolock)  where   d.minnightsid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdRoomrates.Rows

                    Dim txtrmtypcode As TextBox = GvRow.findcontrol("txtrmtypcode")
                    Dim txtfromDate As TextBox = GvRow.findcontrol("txtfromDate")
                    Dim txtToDate As TextBox = GvRow.findcontrol("txtToDate")
                    Dim txtminnights As TextBox = GvRow.findcontrol("txtminnights")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")

                    Dim ddloptions As HtmlSelect = GvRow.findcontrol("ddloptions")
                    Dim txtrmtypname As TextBox = GvRow.findcontrol("txtrmtypname")
                    Dim ddlweekoptions As HtmlSelect = GvRow.findcontrol("ddlweekoptions")

                    If mySqlReader.Read = True Then


                        If IsDBNull(mySqlReader("nightsoption")) = False Then
                            ddloptions.Value = mySqlReader("nightsoption")

                        End If

                        If IsDBNull(mySqlReader("weekoption")) = False Then
                            ddlweekoptions.Value = mySqlReader("weekoption")

                        End If


                        If IsDBNull(mySqlReader("minnights")) = False Then
                            txtminnights.Text = mySqlReader("minnights")

                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then
                            txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                        End If

                        If IsDBNull(mySqlReader("fromdate")) = False Then

                            txtfromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                        End If
                        If IsDBNull(mySqlReader("roomtypes")) = False Then
                            txtrmtypcode.Text = mySqlReader("roomtypes")
                            Dim strMealPlans As String = ""
                            Dim strCondition As String = ""
                            strMealPlans = mySqlReader("roomtypes")
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
                            strQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock)  where u.rmtypcode =rmtypcode and u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'')  rmtypname " _
                                & " from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") "

                            txtrmtypname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry) '"select rmtypname from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") ")

                            If txtrmtypname.Text = "" Then txtrmtypname.Text = "All"
                        End If

                        If IsDBNull(mySqlReader("mealplans")) = False Then
                            txtmealcode.Text = mySqlReader("mealplans")

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            '''''''''''



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub

    Private Sub fillweekdays(ByVal refcode As String)

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        Dim dt2 As DataSet
        Dim chk As CheckBox

        If Session("Calledfrom") = "Offers" Then
            dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_New_MLOS_weekdays where MLOS_ID='" & refcode & "'")
        Else
            dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_New_MLOS_weekdays where MLOS_ID='" & refcode & "'")
        End If



        If dt2.Tables(0).Rows.Count > 0 Then

            For Each gvRow In grdWeekDays.Rows
                chk = gvRow.FindControl("chkSelect")
                chk.Checked = False
            Next
        End If
        Dim i As Integer
        For i = 0 To dt2.Tables(0).Rows.Count - 1

            Select Case dt2.Tables(0).Rows(i).Item(0).ToString
                Case "1"
                    chk = CType(grdWeekDays.Rows(0).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "2"
                    chk = CType(grdWeekDays.Rows(1).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "3"
                    chk = CType(grdWeekDays.Rows(2).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "4"
                    chk = CType(grdWeekDays.Rows(3).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "5"
                    chk = CType(grdWeekDays.Rows(4).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "6"
                    chk = CType(grdWeekDays.Rows(5).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "7"
                    chk = CType(grdWeekDays.Rows(6).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

            End Select
        Next
        'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        'clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(SqlConn)              'connection close 


    End Sub
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            Dim lblpricelistid As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltranid")
            lblpricelistid = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpricelistid")
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
                If CType(lblpricelistid.Text, String).Contains("EXH") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Entry Generated From Exhibition Please modify Exhibition page');", True)
                    Exit Sub
                ElseIf CType(lblpricelistid.Text, String).Contains("PL") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Entry Generated From Room Rates Please modify Room Rates page');", True)
                    Exit Sub
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
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit Minimum Nights - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Minimum Nights  "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Edit Minimum Nights - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Minimum Nights "
                End If
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View Minimum Nights - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Minimum Nights "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "View Minimum Nights - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Minimum Nights "
                End If

            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete Minimum Nights - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Minimum Nights "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Delete Minimum Nights - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Minimum Nights "
                End If

            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillseason()
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Minimum Nights - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + " Minimum Nights "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Copy Minimum Nights - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Minimum Nights "
                End If
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


    Protected Sub btnrmtyp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex





        Dim strrmtypename As String = CType(grdRoomrates.Rows(rowid).FindControl("txtrmtypcode"), TextBox).Text


        Dim strRoomType As String = CType(strrmtypename, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        If Session("Calledfrom") = "Offers" Then
            strSqlQry = "select v.rmtypcode,p.rmtypname from  partyrmtyp p(nolock),view_offers_rmtype v where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.inactive=0 and p.partycode='" & hdnpartycode.Value & "' order by isnull(p.rankord,999)"
        Else
            strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"
        End If


        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If

        gv_Showroomtypes.HeaderRow.Cells(3).Text = "RoomType Name"

        ChkExistingRoomtypes(strrmtypename)


        ModalExtraPopup.Show()


    End Sub
    Protected Sub btnmeal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdRoomrates.Rows(rowid).FindControl("txtmealcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        If Session("Calledfrom") = "Offers" Then
            strSqlQry = "select v.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock),view_offers_meal v(nolock)  " _
                & " where v.mealcode=p.mealcode and v.mealcode=m.mealcode and v.promotionid='" & hdnpromotionid.Value & "' and  p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
        Else

            strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
        End If
        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Meal Plan"

        'ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)


        ModalExtraPopup.Show()


    End Sub

    Private Sub ChkExistingRoomtypes(ByVal prm_strChkRoomtypes As String)
        Dim chkSelect As CheckBox
        Dim txtrmtypcode As Label


        Dim arrString As String()


        If prm_strChkRoomtypes <> "" Then
            arrString = prm_strChkRoomtypes.Split(",") 'spliting for ';' 1st

            For k = 0 To arrString.Length - 1
                'If arrString(k) <> "" Then

                '  Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                For Each grdRow In gv_Showroomtypes.Rows
                    chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                    txtrmtypcode = CType(grdRow.FindControl("txtrmtypcode"), Label)
                    If arrString(k) = txtrmtypcode.Text Then
                        chkSelect.Checked = True

                    End If

                Next
                'End If
            Next
            'Else
            '    'first case when not selected , chk all
            '    For Each grdRow In gvOccupancy.Rows
            '        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
            '        chkSelect.Checked = True

            '    Next


        End If
    End Sub
    Private Sub filldaysgrid()
        Dim chkSelect As CheckBox

        Dim dt As New DataTable



        dt.Columns.Add("SrNo")
        dt.Columns.Add("days")

        dt.Rows.Add(1, "SUNDAY")
        dt.Rows.Add(2, "MONDAY")
        dt.Rows.Add(3, "TUESDAY")
        dt.Rows.Add(4, "WEDNESDAY")
        dt.Rows.Add(5, "THURSDAY")
        dt.Rows.Add(6, "FRIDAY")
        dt.Rows.Add(7, "SATURDAY")


        grdWeekDays.DataSource = dt

        grdWeekDays.DataBind()

        For Each gvRow In grdWeekDays.Rows
            chkSelect = gvRow.FindControl("chkSelect")
            chkSelect.Checked = True
        Next





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
        filldaysgrid()
        DisableControl()
        lblstatus.Visible = False
        lblstatustext.Visible = False


        ' filldaysgrid()
        'FillMealplans()
        ' seasonsgridfill()
        wucCountrygroup.Visible = True

        'divcopy1.Style("display") = "none"



        btncopyratesnextrow.Visible = True

        lable12.Visible = True
        btnfillrate.Visible = False
        txtfillrate.Visible = False
        fillroomgrid(grdRoomrates, True)

        'createdatacolumns()
        ' FillRoomdetails()

        btnSave.Visible = True

        btnSave.Text = "Save"
        lblHeading.Text = "New Minimum Nights - " + ViewState("hotelname")
        Page.Title = Page.Title + " " + " Minimum Nights -" + ViewState("hotelname")

        ' divuser.Style("display") = "none"
        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        wucCountrygroup.sbShowCountry()



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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Sub clearall()
        ' fillDategrd(grdDates, True)
        txtplistcode.Text = ""
        txtApplicableTo.Text = ""
        grdRoomrates.Enabled = True
        txtApplicableTo.Enabled = True
        chkctrygrp.Checked = False
        wucCountrygroup.Disable(True)

    End Sub
    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        clearall()
        Panelsearch.Enabled = True
        Session("GV_HotelData") = Nothing
        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")

        lblHeading.Text = "Minimum Nights  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "CONTRACTMINNIGHTS", CType(Session("ContractState"), String))
        ddlorder.SelectedIndex = 0
        ddlorderby.SelectedIndex = 1
        FillGrid("h.minnightsid", hdncontractid.Value, hdnpartycode.Value, "DESC")

    End Sub
    Private Function ValidatePage() As Boolean
        ValidatePage = True
        Dim GvRow As GridViewRow
        Dim gvRow1 As GridViewRow
        Dim txtfromDate As TextBox
        Dim txtToDate As TextBox
        Dim txtrmtypcode As TextBox
        Dim txtmealcode As TextBox
        Dim ToDt As Date = Nothing
        Dim Rmtype As String = ""
        Dim meal As String = ""

        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Applicable Should not be Blank');", True)
            ValidatePage = False
            Exit Function
        End If

        For Each lRow As GridViewRow In grdRoomrates.Rows

            txtfromDate = lRow.FindControl("txtfromDate")
            txtToDate = lRow.FindControl("txtToDate")
            txtrmtypcode = lRow.FindControl("txtrmtypcode")
            txtmealcode = lRow.FindControl("txtmealcode")

            If txtrmtypcode IsNot Nothing Then
                If txtfromDate.Text <> "" And txtToDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) <= ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(txtToDate)
                        ValidatePage = False
                        Exit Function
                    End If


                    If ToDt <> Nothing Then
                        If ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text) <= ToDt And txtrmtypcode.Text = Rmtype And txtmealcode.Text = meal Then
                            'If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") <= ToDt Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Date Overlapping..');", True)
                            SetFocus(txtfromDate)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                    If (ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value)) Or (ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(hdnconfromdate.Value)) Then
                        'If Format(CType(txtSeasonfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From Date Should belongs to the Contracts Period.');", True)
                        SetFocus(txtfromDate)
                        ValidatePage = False
                        Exit Function
                    End If

                    If (ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) > ObjDate.ConvertDateromTextBoxToDatabase(hdncontodate.Value)) Then
                        'If (Format(CType(txtSeasonToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date Should belongs to the Contracts Period.');", True)
                        SetFocus(txtToDate)
                        ValidatePage = False
                        Exit Function
                    End If


                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    Rmtype = txtrmtypcode.Text
                    meal = txtmealcode.Text

                End If
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



    Protected Sub grdRoomrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRoomrates.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = grdRoomrates.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "3"
        '    e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If




        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim txtrmtypcode As TextBox = e.Row.FindControl("txtrmtypcode")
            Dim txtmealcode As TextBox = e.Row.FindControl("txtmealcode")
            Dim txtminnights As TextBox = e.Row.FindControl("txtminnights")
            Dim txtrmtypname As TextBox = e.Row.FindControl("txtrmtypname")

            Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
            Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
            'iCurrecntIndex = iCurrecntIndex + 1
            'txtFromDate.TabIndex = iCurrecntIndex
            Numberssrvctrl(txtminnights)
            txtFromDate.Attributes.Add("onchange", "setdate();")
            txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
            txtFromDate.Attributes.Add("onchange", "checkfromdates('" & txtrmtypname.ClientID & "','" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
            ' t.Attributes.Add("onchange", "$find('txtnoofchild_AutoCompleteExtender').set_contextKey(this.value);")
        End If




    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdRoomrates.Rows.Count + 1

        Dim n As Integer = 0



        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim minnights(count) As String
        Dim options(count) As String

        Dim fDate(count) As String
        Dim tDate(count) As String

        Dim dpFDate As TextBox
        Dim dpTDate As TextBox


        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                Dim ddloptions As HtmlSelect = GVRow.FindControl("ddloptions")


                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")




                If chkSelect.Checked = True Then

                    CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                minnights(n) = CType(txtminnights.Text, String)
                options(n) = CType(ddloptions.Value, String)
                fDate(n) = CType(dpFDate.Text, String)
                tDate(n) = CType(dpTDate.Text, String)




                txtrmtypcodenew.Add(CType(txtrmtypcode.Text, String))
                txtrmtypnamenew.Add(CType(txtrmtypname.Text, String))
                txtmealcodenew.Add(CType(txtmealcode.Text, String))
                txtminnightsnew.Add(CType(txtminnights.Text, String))
                ddloptionsnew.Add(CType(ddloptions.Value, String))
                fDatenew.Add(CType(dpFDate.Text, String))
                tDatenew.Add(CType(dpTDate.Text, String))




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
        txtminnightsnew.Clear()
        ddloptionsnew.Clear()
        fDatenew.Clear()
        tDatenew.Clear()




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
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox


            For Each GVRow In grdRoomrates.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                Dim ddloptions As HtmlSelect = GVRow.FindControl("ddloptions")




                If n > CopyRow And dpFDate.Text = "" Then

                    txtrmtypcode.Text = txtrmtypcodenew.Item(CopyRow)
                    txtrmtypname.Text = txtrmtypnamenew.Item(CopyRow)
                    txtmealcode.Text = txtmealcodenew.Item(CopyRow)
                    txtminnights.Text = txtminnightsnew.Item(CopyRow)
                    ddloptions.Value = ddloptionsnew.Item(CopyRow)

                    If CType(fDatenew.Item(CopyRow), String) <> "" Then
                        dpFDate.Text = Format(CType(fDatenew.Item(CopyRow), Date), "dd/MM/yyyy")
                        dpTDate.Text = Format(CType(tDatenew.Item(CopyRow), Date), "dd/MM/yyyy")
                    End If



                    Exit For

                End If
                n = n + 1
            Next

            Dim gridcopyrow As GridViewRow
            gridcopyrow = grdRoomrates.Rows(n)
            Dim strRowId As String = gridcopyrow.ClientID
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strFoucsColumnIndex As String = "3"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(n, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)
            CopyClick = 0
            ClearArray()
            Enablegrid()
            ' setdynamicvalues()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try


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

        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtMinnights As TextBox
        Dim ddloptions As HtmlSelect
        Dim ddlweekoptions As HtmlSelect



        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim minnights(count) As String
        Dim options(count) As String
        Dim weekoptions(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows
                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtmealcode = GVRow.FindControl("txtmealcode")
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")

                txtMinnights = GVRow.FindControl("txtminnights")
                ddloptions = GVRow.FindControl("ddloptions")
                ddlweekoptions = GVRow.FindControl("ddlweekoptions")



                chkSelect = GVRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                minnights(n) = CType(txtMinnights.Text, String)
                options(n) = CType(ddloptions.Value, String)
                fDate(n) = CType(dpFDate.Text, String)
                tDate(n) = CType(dpTDate.Text, String)
                weekoptions(n) = CType(ddlweekoptions.Value, String)




                n = n + 1
            Next
            fillroomgrid(grdRoomrates, False, grdRoomrates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdRoomrates.Rows
                If n = i Then

                    Exit For
                End If
                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtmealcode = GVRow.FindControl("txtmealcode")
                txtMinnights = GVRow.FindControl("txtminnights")
                ddloptions = GVRow.FindControl("ddloptions")
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                ddlweekoptions = GVRow.FindControl("ddlweekoptions")

                chkSelect = GVRow.FindControl("chkSelect")


                txtrmtypcode.Text = rmtypcode(n)
                txtrmtypname.Text = rmtypname(n)
                txtmealcode.Text = mealcode(n)
                txtMinnights.Text = minnights(n)
                ddloptions.Value = options(n)
                dpFDate.Text = fDate(n)
                dpTDate.Text = tDate(n)
                ddlweekoptions.Value = weekoptions(n)



                n = n + 1

            Next
            Dim txtfromdate As TextBox = grdRoomrates.Rows(grdRoomrates.Rows.Count - 1).FindControl("txtfromDate")
            txtfromdate.Focus()
            'Dim txtToDate As TextBox = grdRoomrates.Rows(grdRoomrates.Rows.Count - 1).FindControl("txtToDate")
            'If txtToDate.Text = "" Then
            '    txtToDate.Focus()
            'End If
            Dim gridNewrow As GridViewRow
            gridNewrow = grdRoomrates.Rows(grdRoomrates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strFoucsColumnIndex As String = "3"
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


        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtMinnights As TextBox
        Dim ddloptions As HtmlSelect
        Dim ddlweekoptions As HtmlSelect



        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim minnights(count) As String
        Dim options(count) As String
        Dim weekoptions(count) As String

        Try
            For Each GVRow In grdRoomrates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtmealcode = GVRow.FindControl("txtmealcode")
                    txtMinnights = GVRow.FindControl("txtminnights")
                    ddloptions = GVRow.FindControl("ddloptions")
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    ddlweekoptions = GVRow.FindControl("ddlweekoptions")


                    rmtypcode(n) = CType(txtrmtypcode.Text, String)
                    rmtypname(n) = CType(txtrmtypname.Text, String)
                    mealcode(n) = CType(txtmealcode.Text, String)
                    minnights(n) = CType(txtMinnights.Text, String)
                    options(n) = CType(ddloptions.Value, String)
                    fDate(n) = CType(dpFDate.Text, String)
                    tDate(n) = CType(dpTDate.Text, String)
                    weekoptions(n) = CType(ddlweekoptions.Value, String)



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
            'If gv_Filldata.Rows.Count > 1 Then
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0

            For Each GVRow In grdRoomrates.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then


                    txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtmealcode = GVRow.FindControl("txtmealcode")
                    txtMinnights = GVRow.FindControl("txtminnights")
                    ddloptions = GVRow.FindControl("ddloptions")
                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    ddlweekoptions = GVRow.FindControl("ddlweekoptions")



                    txtrmtypcode.Text = rmtypcode(n)
                    txtrmtypname.Text = rmtypname(n)
                    txtmealcode.Text = mealcode(n)
                    txtMinnights.Text = minnights(n)
                    ddloptions.Value = options(n)
                    ddlweekoptions.Value = weekoptions(n)
                    dpFDate.Text = fDate(n)
                    dpTDate.Text = tDate(n)


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
        If ValidatePage() = False Then
            Exit Sub
        End If

        GenRemark()
        disablegrid()


    End Sub
    Private Sub Enablegrid()
        For Each gvRow In grdRoomrates.Rows

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim dpFDate As TextBox = gvRow.FindControl("txtfromDate")
            Dim dpTDate As TextBox = gvRow.FindControl("txtToDate")

            Dim txtMinnights As TextBox = gvRow.FindControl("txtminnights")
            Dim ddloptions As HtmlSelect = gvRow.FindControl("ddloptions")
            Dim btnrmtyp As Button = gvRow.Findcontrol("btnrmtyp")
            Dim btnmeal As Button = gvRow.Findcontrol("btnmeal")

            Dim ImgBtnFrmDt As ImageButton = gvRow.Findcontrol("ImgBtnFrmDt")
            Dim ImgBtnToDt As ImageButton = gvRow.Findcontrol("ImgBtnToDt")

            dpFDate.Enabled = True
            dpTDate.Enabled = True
            txtMinnights.Enabled = True
            ddloptions.Disabled = False

            btnrmtyp.Enabled = True
            btnmeal.Enabled = True

            ImgBtnToDt.Enabled = True
            ImgBtnFrmDt.Enabled = True




        Next


    End Sub
    Private Sub disablegrid()
        For Each gvRow In grdRoomrates.Rows

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim dpFDate As TextBox = gvRow.FindControl("txtfromDate")
            Dim dpTDate As TextBox = gvRow.FindControl("txtToDate")

            Dim txtMinnights As TextBox = gvRow.FindControl("txtminnights")
            Dim ddloptions As HtmlSelect = gvRow.FindControl("ddloptions")
            Dim btnrmtyp As Button = gvRow.Findcontrol("btnrmtyp")
            Dim btnmeal As Button = gvRow.Findcontrol("btnmeal")

            Dim ImgBtnFrmDt As ImageButton = gvRow.Findcontrol("ImgBtnFrmDt")
            Dim ImgBtnToDt As ImageButton = gvRow.Findcontrol("ImgBtnToDt")

            dpFDate.Enabled = False
            dpTDate.Enabled = False
            txtMinnights.Enabled = False
            ddloptions.Disabled = True

            btnrmtyp.Enabled = False
            btnmeal.Enabled = False

            ImgBtnToDt.Enabled = False
            ImgBtnFrmDt.Enabled = False


        Next


    End Sub
#Region "Public Sub GenRemark()"
    Public Sub GenRemark()

        Dim strRemark As String = ""

        Dim basecurrency As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")

        Dim flgEB As Boolean = True




        strRemark = ""
        '    strRemark = strRemark + txtseasonname.Text + " Season"
        ' strRemark = strRemark & "For" & " " & (roomtype) & " -- occupancies" & " " & roomcategory & vbCrLf
        ' - "occupancies" & (roomcategory)
        If flgEB = True Then
            For Each gvRow In grdRoomrates.Rows

                Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
                Dim dpFDate As TextBox = gvRow.FindControl("txtfromDate")
                Dim dpTDate As TextBox = gvRow.FindControl("txtToDate")

                Dim txtMinnights As TextBox = gvRow.FindControl("txtminnights")
                Dim ddloptions As HtmlSelect = gvRow.FindControl("ddloptions")




                '  strRemark = strRemark + IIf(strRemark.Length > 0, vbCrLf, "")



                If txtrmtypname.Text <> "" And txtrmtypcode.Text <> " " Then



                    If txtMinnights.Text <> "" Then

                        strRemark = strRemark & vbCrLf + "" + txtrmtypname.Text & " On " & txtmealcode.Text & " ."




                        If ddloptions.Value = "Over All" Then
                            strRemark = strRemark & vbCrLf + "Minimum Over All stay of " & txtMinnights.Text & " nights for stay from  " + dpFDate.Text + " to " + dpTDate.Text + "."
                            ' strRemark = strRemark & " " + ddloptions.Value + " this period " + dpFDate.Text + " to " + dpTDate.Text + "."
                        ElseIf ddloptions.Value = "Within Period" Then
                            strRemark = strRemark & vbCrLf + "Minimum stay of  " & txtMinnights.Text & " nights for stay during the period " + dpFDate.Text + " to " + dpTDate.Text + "."

                        ElseIf ddloptions.Value = "CheckIn" Then
                            strRemark = strRemark & vbCrLf + "Minimum stay of  " & txtMinnights.Text & " nights for Check in between " + dpFDate.Text + " to " + dpTDate.Text + "."

                        End If



                    End If


                    strRemark = strRemark

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

                '  wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))


                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                '  wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")

                wucCountrygroup.sbShowCountry()

                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Minimum Nights - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Minimum Nights "
                fillseason()
            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMinNights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub







    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            strSqlQry = "select h.contractid, h.minnightsid plistcode,min(d.fromdate) fromdate,max(d.todate) todate,min(h.applicableto) applicableto  from view_contracts_minnights_header h(nolock),view_contracts_minnights_detail d(nolock) ,view_contracts_search s(nolock) " _
                       & " where isnull(s.withdraw,0)=0  and h.contractid=s.contractid and h.minnightsid=d.minnightsid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "' group by h.contractid,h.minnightsid"


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
            objUtils.WritErrorLog("ContractMinnights.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Public Sub sortgvsearch()
        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("h.minnightsid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("frmdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("isnull(h.linkcode,'')", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 7
                FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 8
                FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select
    End Sub

    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub
End Class

