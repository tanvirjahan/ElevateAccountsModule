
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractCancPolicy
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
    Dim txtfromnoofdaysnew As New ArrayList
    Dim txtunitsnew As New ArrayList
    Dim txtchargenew As New ArrayList
    Dim txtperchargenew As New ArrayList
    Dim txtvaluenew As New ArrayList
    Dim txtnightsnew As New ArrayList

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
        Tranid = 0
        season = 2
        Fromdate = 2
        Todate = 3
        applicableto = 4
        Edit = 8
        View = 9
        Delete = 10
        Copy = 11
       

    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = False Then
            ' Session("GV_HotelData") = Nothing


            ' NumbersHtml(txtminnights)       'accept only number
       
        

           
        Else
            'If Session("GV_HotelData") Is Nothing = False Then

            '    dt = Session("GV_HotelData")





            '    Dim fld2 As String = ""
            '    Dim col As DataColumn
            '    For Each col In dt.Columns
            '        If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" Then
            '            Dim bfield As New TemplateField
            '            'Call Function


            '            bfield.HeaderTemplate = New ClassChildPolicy(ListItemType.Header, col.ColumnName, fld2)
            '            bfield.ItemTemplate = New ClassChildPolicy(ListItemType.Item, col.ColumnName, fld2)
            '            grdRoomrates.Columns.Add(bfield)



            '        End If
            '    Next

            '    grdRoomrates.Visible = True
            '    grdRoomrates.DataSource = dt
            '    'InstantiateIn Grid View
            '    grdRoomrates.DataBind()
            'End If
        End If
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        'FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)


        Dim CalledfromValue As String = ""
        Dim Cancappid As String = ""
        Dim cancappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        If IsPostBack = False Then
            Cancappid = 1
            cancappname = objUser.GetAppName(Session("dbconnectionName"), Cancappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(cancappname, String), "ContractCancPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(cancappname, String), "ContractCancPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractCancPolicy.aspx", Cancappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, Cancappid, intMenuID)
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
                    ' btncopycontract.Visible = False
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


                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)

                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                gv_SearchResult.Columns(4).Visible = False
                gv_SearchResult.Columns(5).Visible = False
                gv_SearchResult.Columns(6).Visible = False

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


                Page.Title = "Promotion CancellationPolicy "
                FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
                '  PanelMain.Visible = False
                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")


            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                txtconnection.Value = Session("dbconnectionName")
                divoffer.Style.Add("display", "none")
                btnselect.Style.Add("display", "none")

                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Session("partycode") = hdnpartycode.Value

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True
                gv_SearchResult.Columns(5).Visible = True
                gv_SearchResult.Columns(6).Visible = True

                wucCountrygroup.sbSetPageState("", "CONTRACTCANCEL", CType(Session("ContractState"), String))

                '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value
                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdncontractid.Value

                'lblbookingvaltype.Visible = False
                'ddlBookingValidity.Visible = False
                Page.Title = "Contract CancellationPolicy "
                FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")
                '  PanelMain.Visible = False
                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")

            End If

        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If

            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If
            hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")
            chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

            If Session("Calledfrom") <> "Offers" Then
                btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
                btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            End If



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

                    If ViewState("noshowclick") = 1 Then
                        Dim txtmealcode As TextBox = grdnoshow.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                        txtmealcode.Text = roomtypes.ToString
                    Else
                        Dim txtmealcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                        txtmealcode.Text = roomtypes.ToString
                    End If
                    
                End If
            Else
                If hdnMainGridRowid.Value <> "" Then
                    If ViewState("noshowclick") = 1 Then
                        Dim txtrmtypname As TextBox = grdnoshow.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                        Dim txtrmtypcode As TextBox = grdnoshow.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                        txtrmtypname.Text = rmtypname.ToString
                        txtrmtypcode.Text = roomtypes.ToString
                    Else
                        Dim txtrmtypname As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                        Dim txtrmtypcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                        txtrmtypname.Text = rmtypname.ToString
                        txtrmtypcode.Text = roomtypes.ToString
                    End If
                   



                End If
            End If


            ViewState("noshowclick") = Nothing

            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            chargelist.Add("% with Max Nights")


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
            chargelist.Add("% with Max Nights")


            Return chargelist
        Catch ex As Exception
            Return chargelist
        End Try

    End Function
    Private Sub sortgvsearch()
        If Session("Calledfrom") = "Offers" Then
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("promotionid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("seasoncode", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("frmdate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("todate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.applicableto", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.adddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.adduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 9
                    FillGrid("h.moddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 10
                    FillGrid("h.moduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        Else
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("seasoncode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("frmdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5
                    FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6
                    FillGrid("h.adddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.adduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moddate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 9
                    FillGrid("h.moduser", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        End If
        
    End Sub
    Protected Sub ddlorder_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub
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

            strSqlQry = " select c.cancelpolicyid plistcode, h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_cancelpolicy_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  h.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by h.promotionid,h.approved,h.promotionname,c.cancelpolicyid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

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
        createdatarows()
        grdRoomrates.Visible = True

        lable12.Visible = True
        btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False


        btnfillrate.Visible = True
        txtfillrate.Visible = True

    End Sub
    Protected Sub imgManStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

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
            For Each GVRow In grdMaualDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
                n = n + 1
            Next
            fillDategrd(grdMaualDates, False, grdMaualDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdMaualDates.Rows
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
            txtStayFromDt = TryCast(grdMaualDates.Rows(grdMaualDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
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
    Protected Sub imgManSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)


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
            For Each GVRow In grdMaualDates.Rows
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

            If grdMaualDates.Rows.Count > 1 Then
                fillDategrd(grdMaualDates, False, grdMaualDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdMaualDates, False, grdMaualDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdMaualDates.Rows
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
    Protected Sub btnClearPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtViewPolicy.Value = ""
    
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub



   '  End Function


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

                    strSqlQry = " With ctee as (select h.cancelpolicyid tranid,h.promotionid,o.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate," & _
                   " h.applicableto ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock)  join view_offers_header o(nolock) on h.promotionid =o.promotionid " _
                   & " join view_contracts_cancelpolicy_offerdates  d on h.cancelpolicyid =d.cancelpolicyid   where  h.promotionid='" & contractid & "' group by h.cancelpolicyid,h.promotionid,o.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) " & _
                        "select * from ctee order by convert(datetime," & StrSortBy & " ,103) " & strsortorder & ""
                Else

                    strSqlQry = "select h.cancelpolicyid tranid,h.promotionid,o.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate," & _
                   " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock) join view_offers_header o(nolock) on h.promotionid =o.promotionid  " _
                   & " join view_contracts_cancelpolicy_offerdates  d on h.cancelpolicyid =d.cancelpolicyid where  h.promotionid='" & contractid & "' group by h.cancelpolicyid,h.promotionid,o.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser order by " & StrSortBy & " " & strsortorder & " "
                End If
            Else
                If StrSortBy = "frmdate" Or StrSortBy = "todate" Then

                    strSqlQry = " With ctee as (select h.cancelpolicyid tranid,'' promotionid, '' promotionname ,dbo.fn_get_exhitionnames (h.seasons,h.exhibitions) seasoncode,dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,0) frmdate, dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,1)  todate," & _
                   " h.applicableto ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock)  where  h.contractid='" & contractid & "') " & _
                        "select * from ctee order by convert(datetime," & StrSortBy & " ,103) " & strsortorder & ""
                Else

                    ' strSqlQry = "select h.cancelpolicyid tranid,'' promotionid, '' promotionname,dbo.fn_get_exhitionnames (h.seasons,h.exhibitions) seasoncode,dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,0) frmdate, dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,1)  todate," & _
                    '" h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock)  where  h.contractid='" & contractid & "' order by " & StrSortBy & " " & strsortorder & " "



                    strSqlQry = "select h.cancelpolicyid tranid,'' promotionid, '' promotionname,dbo.fn_get_exhitionnames (h.seasons,h.exhibitions) seasoncode,dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,0) frmdate, dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & contractid & "', h.exhibitions,1)  todate," & _
                  " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock)  where  h.contractid='" & contractid & "' and (h.seasons<>''or h.exhibitions<>'')  union all " _
                  & " select h.cancelpolicyid tranid,'' promotionid, '' promotionname,'' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate  , convert(varchar(10),max(d.todate),103)  todate," & _
                  " h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_cancelpolicy_header h(nolock) ,view_contracts_cancelpolicy_dates d  where h.cancelpolicyid=d.cancelpolicyid and  h.contractid='" & contractid & "' and (h.seasons=''or h.exhibitions='')  group by h.cancelpolicyid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser order by " & StrSortBy & " " & strsortorder & " "

                End If
            End If
           
            'strSqlQry = "select '' tranid,'' frmdate,'' todate,'' countrygroups,'' adddate, '' adduser, '' moddate, '' moduser"
            'fn_get_seasonmindate, fn_get_seasonmaxdate is replaced with fn_get_seasonminmaxdate_includeexhibition on 17/04/2018 --changed by mohamed on 17/04/2018
            'fn_get_exhitionnames is added, instead of showing only season name, we are showing exhibition name also.--changed by mohamed on 17/04/2018

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Private Function OfferValidateSave() As Boolean
        Dim gvRow As GridViewRow


        Dim tickedornot As Boolean = False
        Dim chkSelect As CheckBox
      

        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Session("seasons") = Nothing

       


        Dim exhibitions As String = ""
        Dim chkExhi As CheckBox
        Dim txtExhicode As Label
        ViewState("exhibitions") = Nothing

        For Each grdRow As GridViewRow In grdexhibition.Rows
            chkExhi = grdRow.FindControl("chkExhi")
            txtExhicode = grdRow.FindControl("txtExhicode")

            If chkExhi.Checked = True Then
                exhibitions = exhibitions + txtExhicode.Text + ","

            End If
        Next

        If exhibitions.Length > 0 Then
            exhibitions = exhibitions.Substring(0, exhibitions.Length - 1)
        End If

        ViewState("exhibitions") = exhibitions





        Dim lnCnt As Integer = 0
        Dim lnCntnoshow As Integer = 0

        Dim ToDt As Date = Nothing
        Dim flg As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtnoofdays As TextBox = gvRow.FindControl("txtnoofdays")
            Dim txtunits As TextBox = gvRow.FindControl("txtunits")
            Dim txtcharge As TextBox = gvRow.FindControl("txtcharge")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim txtfromnoofdays As TextBox = gvRow.FindControl("txtfromnoofdays")



            If txtrmtypcode.Text <> "" Then
                flg = True
            End If


            If txtrmtypname.Text <> "" Then

                If txtrmtypcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Proper Roomtype  Row No:" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (txtnoofdays.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter To No.of Days or Hours Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (txtfromnoofdays.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From No.of Days or Hours Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (txtunits.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Unit -Days/Hours Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If (txtcharge.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Basis Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If


                If txtcharge.Text <> "" And (txtcharge.Text = "Nights") And txtnights.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% of Nights") And txtpercharge.Text = "" And txtnights.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge or Percentage of Charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay") And txtpercharge.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Row No  :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "Value") And txtvalue.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Value of Charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% with Max Nights") And txtnights.Text = "" And txtpercharge.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Nights and Percentage  of Charge Row No :" & lnCnt & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If




            End If



        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one  Row Cancellation  :" & lnCnt & "');", True)
            OfferValidateSave = False
            Exit Function
        End If


        For Each gvRow In grdnoshow.Rows
            lnCntnoshow += 1

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

            Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")
            Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
            Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
            Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
            Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")


            If txtrmtypname.Text <> "" Then

                If txtrmtypcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Proper Roomtype Noshow Grid Row No:" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Noshow Grid  Row No:" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If





                If (txtchargenoshow.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Basis Noshow Grid  Row No:" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function
                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Nights") And txtchargenoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Nights") And txtpercnoshow.Text = "" And txtnightsnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge or Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Entire Stay") And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of remaining Nights") And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Value") And txtvaluenoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Value of Charge Noshow Grid  Row No :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% with Max Nights") And txtnightsnoshow.Text = "" And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Nights and Percentage  of Charge Row No :" & lnCntnoshow & "');", True)
                    OfferValidateSave = False
                    Exit Function

                End If


            End If



        Next




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


        OfferValidateSave = True
    End Function
    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


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

        Dim exhibitions As String = ""
        Dim chkExhi As CheckBox
        Dim txtExhicode As Label
        ViewState("exhibitions") = Nothing

        For Each grdRow As GridViewRow In grdexhibition.Rows
            chkExhi = grdRow.FindControl("chkExhi")
            txtExhicode = grdRow.FindControl("txtExhicode")

            If chkExhi.Checked = True Then
                exhibitions = exhibitions + txtExhicode.Text + ","

            End If
        Next

        If exhibitions.Length > 0 Then
            exhibitions = exhibitions.Substring(0, exhibitions.Length - 1)
        End If

        ViewState("exhibitions") = exhibitions

        Dim tickeddates As Boolean = False

        For Each grdrow In grdMaualDates.Rows
            Dim txtfromdate As TextBox = grdrow.findcontrol("txtfromdate")
            Dim txttodate As TextBox = grdrow.findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                tickeddates = True
                Exit For
            End If

        Next


        If tickedornot = False And Session("Calledfrom") <> "Offers" And exhibitions.Length = 0 And tickeddates = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Season or Exhibitions or Manaul Dates');", True)
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
                seasonname = seasonname + RTrim(LTrim(txtmealcode1.Text)) + ","

            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If

        Session("seasons") = seasonname


       

       



        Dim lnCnt As Integer = 0
        Dim lnCntnoshow As Integer = 0

        Dim ToDt As Date = Nothing
        Dim flg As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtnoofdays As TextBox = gvRow.FindControl("txtnoofdays")
            Dim txtunits As TextBox = gvRow.FindControl("txtunits")
            Dim txtcharge As TextBox = gvRow.FindControl("txtcharge")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim txtfromnoofdays As TextBox = gvRow.FindControl("txtfromnoofdays")
            Dim chknonrefund As CheckBox = gvRow.FindControl("chknonrefund")


            If txtrmtypcode.Text <> "" Then
                flg = True
            End If


            If txtrmtypname.Text <> "" Then

                If txtrmtypcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Proper Roomtype  Row No:" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If
               
                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If (txtnoofdays.Text) = "" And chknonrefund.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter To No.of Days or Hours Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If (txtfromnoofdays.Text) = "" And chknonrefund.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From No.of Days or Hours Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If (txtunits.Text) = "" And chknonrefund.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Unit -Days/Hours Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If (txtcharge.Text) = "" And chknonrefund.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Basis Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If


                If txtcharge.Text <> "" And (txtcharge.Text = "Nights") And txtnights.Text = "" And chknonrefund.Checked = False Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% of Nights") And txtpercharge.Text = "" And txtnights.Text = "" And chknonrefund.Checked = False Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge or Percentage of Charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay") And txtpercharge.Text = "" And chknonrefund.Checked = False Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Row No  :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "Value") And txtvalue.Text = "" And chknonrefund.Checked = False Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Value of Charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtcharge.Text <> "" And (txtcharge.Text = "% with Max Nights") And txtnights.Text = "" And txtpercharge.Text = "" And chknonrefund.Checked = False Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Nights and Percentage  of Charge Row No :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function

                End If




            End If



        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter one  Row Cancellation  :" & lnCnt & "');", True)
            ValidateSave = False
            Exit Function
        End If


        For Each gvRow In grdnoshow.Rows
            lnCntnoshow += 1

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

            Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")
            Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
            Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
            Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
            Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")


            If txtrmtypname.Text <> "" Then

                If txtrmtypcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Proper Roomtype Noshow Grid Row No:" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan Noshow Grid  Row No:" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

              

              

                If (txtchargenoshow.Text) = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Charge Basis Noshow Grid  Row No:" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Nights") And txtchargenoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Nights") And txtpercnoshow.Text = "" And txtnightsnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter No.of Nights to charge or Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Entire Stay") And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No  :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of remaining Nights") And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Percentage of Charge Noshow Grid Row No :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If

                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Value") And txtvaluenoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Value of Charge Noshow Grid  Row No :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If
                If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% with Max Nights") And txtnightsnoshow.Text = "" And txtpercnoshow.Text = "" Then
                    Enablegrid()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter  Nights and Percentage  of Charge Row No :" & lnCntnoshow & "');", True)
                    ValidateSave = False
                    Exit Function

                End If


            End If



        Next





        ValidateSave = True
    End Function
    Public Function OfferFindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        OfferFindDatePeriod = True
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

                    For Each GVRow1 In grdRoomrates.Rows

                        Dim ds As DataSet
                        Dim parms2 As New List(Of SqlParameter)
                        Dim parm2(11) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")


                        If txtrmtypcode.Text <> "" Then

                            parm2(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                            parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm2(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                            parm2(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                            parm2(4) = New SqlParameter("@subseasoncode", "")
                            parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm2(8) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                            parm2(9) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm2(10) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))


                            For i = 0 To 10
                                parms2.Add(parm2(i))
                            Next



                            ds = New DataSet()
                            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcancelpolicy_offer", parms2)


                            If ds.Tables.Count > 0 Then
                                If ds.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds.Tables(0).Rows(0)("cancelpolicyid")) = False Then

                                        strMsg = "Cancellation Policy already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds.Tables(0).Rows(0)("cancelpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")


                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                        OfferFindDatePeriod = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If


                    Next
                End If


            Next



        Catch ex As Exception
            OfferFindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("Contractcancpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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



                If GVRow.Cells(1).Text <> "" And GVRow.Cells(1).Text <> "&nbsp;" Then

                    For Each GVRow1 In grdRoomrates.Rows

                        Dim ds As DataSet
                        Dim parms2 As New List(Of SqlParameter)
                        Dim parm2(11) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")


                        If txtrmtypcode.Text <> "" Then

                            parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                            parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm2(2) = New SqlParameter("@fromdate", Format(CType(GVRow.Cells(1).Text, Date), "yyyy/MM/dd"))
                            parm2(3) = New SqlParameter("@todate", Format(CType(GVRow.Cells(2).Text, Date), "yyyy/MM/dd"))
                            parm2(4) = New SqlParameter("@subseasoncode", CType(GVRow.Cells(3).Text, String))
                            parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm2(8) = New SqlParameter("@promotionid", "")
                            parm2(9) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm2(10) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))


                            For i = 0 To 10
                                parms2.Add(parm2(i))
                            Next



                            ds = New DataSet()
                            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcancelpolicy", parms2)


                            If ds.Tables.Count > 0 Then
                                If ds.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds.Tables(0).Rows(0)("cancelpolicyid")) = False Then

                                        strMsg = "Cancellation Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("cancelpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")


                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                        FindDatePeriod = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If


                    Next
                End If


            Next


            '''' Exhibition checking

        

            Dim chkExhi As CheckBox
            For Each grdRow As GridViewRow In grdexhibition.Rows
                chkExhi = grdRow.FindControl("chkExhi")
                Dim txtExhicode As Label = grdRow.FindControl("txtExhicode")

                If chkExhi.Checked = True Then
                    For Each GVRow1 In grdRoomrates.Rows

                        Dim ds1 As DataSet
                        Dim parms3 As New List(Of SqlParameter)
                        Dim parm3(12) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")


                        If txtrmtypcode.Text <> "" Then



                            parm3(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                            parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm3(2) = New SqlParameter("@fromdate", Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd"))
                            parm3(3) = New SqlParameter("@todate", Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd"))
                            parm3(4) = New SqlParameter("@subseasoncode", IIf(CType(Replace(Session("seasons"), ",  ", ","), String) = "", "", CType(Replace(Session("seasons"), ",  ", ","), String)))
                            parm3(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm3(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm3(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm3(8) = New SqlParameter("@promotionid", "")
                            parm3(9) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm3(10) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))
                            parm3(11) = New SqlParameter("@exhibitioncode", CType(txtExhicode.Text, String))


                            For i = 0 To 11
                                parms3.Add(parm3(i))
                            Next



                            ds1 = New DataSet()
                            ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcancelpolicy_exhibition", parms3)


                            If ds1.Tables.Count > 0 Then
                                If ds1.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds1.Tables(0).Rows(0)("cancelpolicyid")) = False Then

                                        strMsg = "Cancellation Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("cancelpolicyid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")


                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                        FindDatePeriod = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If


                    Next


            End If
        Next

            '''


            ''  Manual Dates checking

            For Each grdRow As GridViewRow In grdMaualDates.Rows

                Dim txtfromdate As TextBox = grdRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = grdRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                    For Each GVRow1 In grdRoomrates.Rows

                        Dim ds1 As DataSet
                        Dim parms3 As New List(Of SqlParameter)
                        Dim parm3(12) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")


                        If txtrmtypcode.Text <> "" Then



                            parm3(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                            parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm3(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                            parm3(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                            parm3(4) = New SqlParameter("@subseasoncode", IIf(CType(Replace(Session("seasons"), ",  ", ","), String) = "", "", CType(Replace(Session("seasons"), ",  ", ","), String)))
                            parm3(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm3(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm3(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm3(8) = New SqlParameter("@promotionid", "")
                            parm3(9) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm3(10) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))
                         



                            For i = 0 To 10
                                parms3.Add(parm3(i))
                            Next



                            ds1 = New DataSet()
                            ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkcancelpolicy_manual", parms3)


                            If ds1.Tables.Count > 0 Then
                                If ds1.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds1.Tables(0).Rows(0)("cancelpolicyid")) = False Then

                                        strMsg = "Cancellation Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("cancelpolicyid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")


                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                        FindDatePeriod = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If


                    Next


                End If
            Next


            ''''''


        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("Contractcancpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


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
                        If OfferFindDatePeriod() = False Then
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
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_cancel", mySqlConn, sqlTrans)
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
                        optionval = objUtils.GetAutoDocNo("CANPOLICY", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_cancelpolicy_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        If Session("Calledfrom") <> "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(CType(Replace(Session("seasons"), ",  ", ","), String) = "", "", CType(Replace(Session("seasons"), ",  ", ","), String)) 'CType(Replace(Session("seasons"), ",  ", ","), String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If
                       
                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitions", SqlDbType.VarChar, 5000)).Value = IIf(ViewState("exhibitions") = "", "", CType(Replace(ViewState("exhibitions"), ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed

                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_cancelpolicy_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        If Session("Calledfrom") <> "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(CType(Replace(Session("seasons"), ",  ", ","), String) = "", "", CType(Replace(Session("seasons"), ",  ", ","), String)) 'CType(Replace(Session("seasons"), ",  ", ","), String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitions", SqlDbType.VarChar, 5000)).Value = IIf(ViewState("exhibitions") = "", "", CType(Replace(ViewState("exhibitions"), ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_detail Where  cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_noshowearly Where  cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_countries Where cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_agents Where cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_offerdates Where cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_cancelpolicy_dates Where cancelpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    '------------------------------------Inserting Offer Dates
                    If Session("Calledfrom") = "Offers" Then
                        Dim jk As Integer = 1
                        For Each GvRow1 As GridViewRow In grdpromodates.Rows

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_contracts_cancelpolicy_offerdates", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure

                                mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = jk
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")



                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                                jk = jk + 1
                            End If
                        Next
                    End If

                    Dim kl As Integer = 1
                    For Each GvRow1 As GridViewRow In grdMaualDates.Rows

                        Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                        Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                        ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                        If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                            mySqlCmd = New SqlCommand("sp_add_contracts_cancelpolicy_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = kl
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")



                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                            kl = kl + 1
                        End If
                    Next

                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then


                                mySqlCmd = New SqlCommand("sp_add_contracts_cancelpolicy_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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

                                mySqlCmd = New SqlCommand("sp_add_contracts_cancelpolicy_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                    End If

                    Dim k As Integer = 1
                    For Each GVRow In grdRoomrates.Rows

                        Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                        Dim txtnoofdays As TextBox = GVRow.FindControl("txtnoofdays")
                        Dim txtunits As TextBox = GVRow.FindControl("txtunits")
                        Dim txtcharge As TextBox = GVRow.FindControl("txtcharge")
                        Dim txtnights As TextBox = GVRow.FindControl("txtnights")
                        Dim txtpercharge As TextBox = GVRow.Findcontrol("txtpercharge")
                        Dim txtvalue As TextBox = GVRow.Findcontrol("txtvalue")
                        Dim txtfromnoofdays As TextBox = GVRow.FindControl("txtfromnoofdays")

                        Dim chknonrefund As CheckBox = GVRow.FindControl("chknonrefund")

                        If txtrmtypname.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_cancelpolicy_detail", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure


                            mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid ", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = k
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nodayshours", SqlDbType.Int)).Value = CType(Val(txtnoofdays.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@dayshours", SqlDbType.VarChar, 20)).Value = CType(txtunits.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@chargebasis", SqlDbType.VarChar, 30)).Value = CType(txtcharge.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nightstocharge", SqlDbType.Int)).Value = CType(Val(txtnights.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@percentagetocharge", SqlDbType.Decimal)).Value = CType(Val(txtpercharge.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@valuetocharge", SqlDbType.Decimal)).Value = CType(Val(txtvalue.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromnodayhours", SqlDbType.Int)).Value = CType(Val(txtfromnoofdays.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nonrefund", SqlDbType.Int)).Value = CType(IIf(chknonrefund.Checked = True, 1, 0), Integer)


                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                        End If


                        k = k + 1

                    Next

                    Dim j As Integer = 1
                    For Each GVRow In grdnoshow.Rows

                        Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                        Dim txtnoofdays As TextBox = GVRow.FindControl("txtnoofdays")
                        Dim ddlnoshow As HtmlSelect = GVRow.FindControl("ddlnoshow")
                        Dim txtchargenoshow As TextBox = GVRow.FindControl("txtchargenoshow")
                        Dim txtnightsnoshow As TextBox = GVRow.FindControl("txtnightsnoshow")
                        Dim txtpercnoshow As TextBox = GVRow.Findcontrol("txtpercnoshow")
                        Dim txtvaluenoshow As TextBox = GVRow.Findcontrol("txtvaluenoshow")


                        If txtrmtypname.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_cancelpolicy_noshowearly", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure


                            mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid ", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = j
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@noshowearly", SqlDbType.VarChar, 20)).Value = CType(ddlnoshow.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@chargebasis", SqlDbType.VarChar, 30)).Value = CType(txtchargenoshow.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@nightstocharge", SqlDbType.Int)).Value = CType(Val(txtnightsnoshow.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@percentagetocharge", SqlDbType.Decimal)).Value = CType(Val(txtpercnoshow.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@valuetocharge", SqlDbType.Decimal)).Value = CType(Val(txtvaluenoshow.Text), Decimal)


                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                        End If


                        j = j + 1

                    Next


                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_cancelpolicy_header"
                    'mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = CType(txtApplicableTo.Text.Trim, String)
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

                    '' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_cancel", mySqlConn, sqlTrans)
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
                    mySqlCmd = New SqlCommand("sp_del_contracts_cancelpolicy_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@cancelpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            txtApplicableTo.Enabled = True
            grdDates.Enabled = True
            grdnoshow.Enabled = True
            grdRoomrates.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btnaddrownoshow.Enabled = True
            btndelrownoshow.Enabled = True
            gv_Seasons.Enabled = True
            grdexhibition.Enabled = True

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then

            grdDates.Enabled = False

            grdRoomrates.Enabled = False

            btncopyratesnextrow.Enabled = False
            btnfillrate.Enabled = False

            grdnoshow.Enabled = False
            txtApplicableTo.Enabled = False
            wucCountrygroup.Disable(False)
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            btnaddrownoshow.Enabled = False
            btndelrownoshow.Enabled = False

            gv_Seasons.Enabled = False
            grdexhibition.Enabled = False


        ElseIf ViewState("State") = "Edit" Then


            grdDates.Enabled = True

            grdRoomrates.Enabled = True

            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            grdnoshow.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btnaddrownoshow.Enabled = True
            btndelrownoshow.Enabled = True
            gv_Seasons.Enabled = True
            grdexhibition.Enabled = True

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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname from view_contracts_search c(nolock),view_contractseasons_rate a(nolock) Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)

                    End If

                End If





                mySqlCmd.Dispose()
                mySqlReader.Close()

                mySqlConn.Close()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            mySqlCmd = New SqlCommand("Select * from view_contracts_cancelpolicy_header(nolock) Where cancelpolicyid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("cancelpolicyid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("cancelpolicyid"), String)
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
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


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

                    If strCondition <> "" Then
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

                    Else
                        seasonsgridfill()
                    End If
                    

                    ''''''''''' Exhibition

                    If IsDBNull(mySqlReader("exhibitions")) = False Then
                        ViewState("exhibitions") = CType(mySqlReader("exhibitions"), String)
                    End If
                    Dim strexhibitions As String = ""
                    Dim strExhiCondition As String = ""
                    If ViewState("exhibitions") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strexhibitions = ViewState("exhibitions") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        If strexhibitions.Length > 0 Then
                            Dim mString1 As String() = strexhibitions.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strExhiCondition = "" Then
                                    strExhiCondition = "'" & mString1(i) & "'"
                                Else
                                    strExhiCondition &= ",'" & mString1(i) & "'"
                                End If
                            Next
                        End If
                    End If

                    Dim myDSExhi As New DataSet
                    grdexhibition.Visible = True
                    strSqlQry = ""

                    If strExhiCondition = "" Then

                        strSqlQry = "select * from ( select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate  ,convert(varchar(10),d.todate,103) todate,0 selected  from  view_contracts_exhibitions d(nolock),exhibition_master e(nolock),view_contracts_exhibition_header h(nolock)  " _
                      & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and  h.contractid='" & hdncopycontractid.Value & "' ) h  order by h.fromdate desc "



                    Else
                        strSqlQry = "select * from ( select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate  ,convert(varchar(10),d.todate,103) todate,0 selected  from  view_contracts_exhibitions d(nolock),exhibition_master e(nolock),view_contracts_exhibition_header h(nolock)  " _
                          & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and  h.contractid='" & hdncopycontractid.Value & "' and d.exhibitioncode not in (" & strExhiCondition & ") union all  " _
                            & "  select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate,  convert(varchar(10),d.todate,103) todate,1 selected  from  view_contracts_exhibitions d(nolock),exhibition_master e(nolock),view_contracts_exhibition_header h(nolock)  " _
                            & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and   h.contractid='" & hdncopycontractid.Value & "'  and d.exhibitioncode  in (" & strExhiCondition & ") ) h  order by h.fromdate desc "

                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.SelectCommand.CommandTimeout = 0
                    myDataAdapter.Fill(myDSExhi)
                    grdexhibition.DataSource = myDSExhi

                    If myDSExhi.Tables(0).Rows.Count > 0 Then
                        grdexhibition.DataBind()


                    Else
                        grdexhibition.DataBind()

                    End If
                    Dim chkExhi As CheckBox
                    Dim lblexhiselect As Label
                    For Each grdRow In grdexhibition.Rows
                        chkExhi = CType(grdRow.FindControl("chkExhi"), CheckBox)
                        lblexhiselect = CType(grdRow.FindControl("lblexhiselect"), Label)

                        If lblexhiselect.Text = "1" Then
                            chkExhi.Checked = True

                        End If

                    Next


                    '''''''''''''

                    lblstatus.Visible = False
                    lblstatustext.Visible = False

                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_exhibition_header(nolock) where  exhibitionid ='" & CType(RefCode, String) & "'") <> "" Then
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_cancelpolicy_header(nolock) Where cancelpolicyid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("cancelpolicyid")) = False Then
                        txtplistcode.Text = CType(mySqlReader("cancelpolicyid"), String)
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
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("seasons")) = False Then
                        Session("seasons") = CType(LTrim(RTrim(mySqlReader("seasons"))), String)
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

                    If strCondition <> "" Then
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
                    Else
                        seasonsgridfill()
                    End If

                   



                    ''''''''''' Exhibition

                    If IsDBNull(mySqlReader("exhibitions")) = False Then
                        ViewState("exhibitions") = CType(mySqlReader("exhibitions"), String)
                    End If
                    Dim strexhibitions As String = ""
                    Dim strExhiCondition As String = ""
                    If ViewState("exhibitions") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strexhibitions = ViewState("exhibitions") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        If strexhibitions.Length > 0 Then
                            Dim mString1 As String() = strexhibitions.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strExhiCondition = "" Then
                                    strExhiCondition = "'" & mString1(i) & "'"
                                Else
                                    strExhiCondition &= ",'" & mString1(i) & "'"
                                End If
                            Next
                        End If
                    End If

                    Dim myDSExhi As New DataSet
                    grdexhibition.Visible = True
                    strSqlQry = ""

                    If strExhiCondition = "" Then

                        strSqlQry = "select * from ( select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate  ,convert(varchar(10),d.todate,103) todate,0 selected  from  view_exhibitions d,exhibition_master e,view_contracts_exhibition_header h  " _
                      & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and  h.contractid='" & hdncontractid.Value & "' ) h  order by h.fromdate desc "



                    Else
                        strSqlQry = "select * from ( select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate  ,convert(varchar(10),d.todate,103) todate,0 selected  from  view_exhibitions d,exhibition_master e,view_contracts_exhibition_header h  " _
                          & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and  h.contractid='" & hdncontractid.Value & "' and d.exhibitioncode not in (" & strExhiCondition & ") union all  " _
                            & "  select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate,  convert(varchar(10),d.todate,103) todate,1 selected  from  view_exhibitions d,exhibition_master e,view_contracts_exhibition_header h  " _
                            & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and   h.contractid='" & hdncontractid.Value & "'  and d.exhibitioncode  in (" & strExhiCondition & ") ) h  order by h.fromdate desc "

                    End If




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDSExhi)
                    grdexhibition.DataSource = myDSExhi

                    If myDSExhi.Tables(0).Rows.Count > 0 Then
                        grdexhibition.DataBind()


                    Else
                        grdexhibition.DataBind()

                    End If
                    Dim chkExhi As CheckBox
                    Dim lblexhiselect As Label
                    For Each grdRow In grdexhibition.Rows
                        chkExhi = CType(grdRow.FindControl("chkExhi"), CheckBox)
                        lblexhiselect = CType(grdRow.FindControl("lblexhiselect"), Label)

                        If lblexhiselect.Text = "1" Then
                            chkExhi.Checked = True

                        End If

                    Next


                    '''''''''''''



                    'If IsDBNull(mySqlReader("promotionid")) = False Then
                    '    txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                    '    txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    'Else
                    '    txtpromotionid.Text = ""
                    '    txtpromotionname.Text = ""
                    'End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_cancelpolicy_header(nolock) where  cancelpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Private Sub Showdetailsgridoffer(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdRoomrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0

            Dim roomtypes As String = ""
            Dim mealplans As String = ""

            If Session("Calledfrom") = "Offers" Then
                strQry = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

                strQry = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
            End If


            strQry = "select count( distinct clineno) from view_contracts_cancelpolicy_detail(nolock) where cancelpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            fillDategrd(grdRoomrates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_cancelpolicy_detail d  where   d.cancelpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdRoomrates.Rows

                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                    Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtnoofdays As TextBox = GvRow.FindControl("txtnoofdays")
                    Dim txtunits As TextBox = GvRow.FindControl("txtunits")
                    Dim txtcharge As TextBox = GvRow.FindControl("txtcharge")
                    Dim txtnights As TextBox = GvRow.FindControl("txtnights")
                    Dim txtpercharge As TextBox = GvRow.Findcontrol("txtpercharge")
                    Dim txtvalue As TextBox = GvRow.Findcontrol("txtvalue")
                    Dim txtfromnoofdays As TextBox = GvRow.FindControl("txtfromnoofdays")

                    If mySqlReader.Read = True Then

                        If IsDBNull(roomtypes) = False Then
                            txtrmtypcode.Text = roomtypes
                            Dim strMealPlans As String = ""
                            Dim strCondition As String = ""
                            strMealPlans = roomtypes
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

                        End If

                        ' If IsDBNull(mySqlReader("mealplans")) = False Then
                        txtmealcode.Text = mealplans

                        ' End '

                        If IsDBNull(mySqlReader("nodayshours")) = False Then
                            txtnoofdays.Text = mySqlReader("nodayshours")

                        End If
                        If IsDBNull(mySqlReader("fromnodayhours")) = False Then
                            txtfromnoofdays.Text = mySqlReader("fromnodayhours")

                        End If

                        If IsDBNull(mySqlReader("dayshours")) = False Then
                            txtunits.Text = mySqlReader("dayshours")

                        End If
                        If IsDBNull(mySqlReader("chargebasis")) = False Then
                            txtcharge.Text = mySqlReader("chargebasis")

                        End If
                        If IsDBNull(mySqlReader("nightstocharge")) = False Then
                            txtnights.Text = IIf(mySqlReader("nightstocharge") = 0, "", (mySqlReader("nightstocharge")))

                        End If
                        If IsDBNull(mySqlReader("percentagetocharge")) = False Then
                            txtpercharge.Text = IIf(mySqlReader("percentagetocharge") = 0, "", DecRound(mySqlReader("percentagetocharge")))

                        End If
                        If IsDBNull(mySqlReader("valuetocharge")) = False Then
                            txtvalue.Text = IIf(mySqlReader("valuetocharge") = 0, "", DecRound(mySqlReader("valuetocharge")))

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            ''''' Fill No show Grid
            strQry = ""
            cnt = 0
            strSqlQry = ""

            strQry = "select count( distinct clineno) from view_contracts_cancelpolicy_noshowearly(nolock) where cancelpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdnoshow, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_cancelpolicy_noshowearly d  where   d.cancelpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdnoshow.Rows

                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                    Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtnoofdays As TextBox = GvRow.FindControl("txtnoofdays")
                    Dim ddlnoshow As HtmlSelect = GvRow.FindControl("ddlnoshow")
                    Dim txtchargenoshow As TextBox = GvRow.FindControl("txtchargenoshow")
                    Dim txtnightsnoshow As TextBox = GvRow.FindControl("txtnightsnoshow")
                    Dim txtpercnoshow As TextBox = GvRow.Findcontrol("txtpercnoshow")
                    Dim txtvaluenoshow As TextBox = GvRow.Findcontrol("txtvaluenoshow")

                    If mySqlReader.Read = True Then

                        If IsDBNull(roomtypes) = False Then
                            txtrmtypcode.Text = roomtypes
                            Dim strMealPlans As String = ""
                            Dim strCondition As String = ""
                            strMealPlans = roomtypes
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

                        End If

                        If IsDBNull(mealplans) = False Then
                            txtmealcode.Text = mealplans

                        End If




                        If IsDBNull(mySqlReader("noshowearly")) = False Then
                            ddlnoshow.Value = mySqlReader("noshowearly")

                        End If
                        If IsDBNull(mySqlReader("chargebasis")) = False Then
                            txtchargenoshow.Text = mySqlReader("chargebasis")

                        End If
                        If IsDBNull(mySqlReader("nightstocharge")) = False Then
                            txtnightsnoshow.Text = IIf(mySqlReader("nightstocharge") = 0, "", (mySqlReader("nightstocharge")))

                        End If
                        If IsDBNull(mySqlReader("percentagetocharge")) = False Then
                            txtpercnoshow.Text = IIf(mySqlReader("percentagetocharge") = 0, "", DecRound(mySqlReader("percentagetocharge")))

                        End If
                        If IsDBNull(mySqlReader("valuetocharge")) = False Then
                            txtvaluenoshow.Text = IIf(mySqlReader("valuetocharge") = 0, "", DecRound(mySqlReader("valuetocharge")))

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            '''''''''''
            Enablegrid()




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdRoomrates.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0


            strQry = "select count( distinct clineno) from view_contracts_cancelpolicy_detail(nolock) where cancelpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            fillDategrd(grdRoomrates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_cancelpolicy_detail d  where   d.cancelpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                myCommand.CommandTimeout = 0
                mySqlReader = myCommand.ExecuteReader


               
                For Each GvRow In grdRoomrates.Rows

                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                    Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtnoofdays As TextBox = GvRow.FindControl("txtnoofdays")
                    Dim txtunits As TextBox = GvRow.FindControl("txtunits")
                    Dim txtcharge As TextBox = GvRow.FindControl("txtcharge")
                    Dim txtnights As TextBox = GvRow.FindControl("txtnights")
                    Dim txtpercharge As TextBox = GvRow.Findcontrol("txtpercharge")
                    Dim txtvalue As TextBox = GvRow.Findcontrol("txtvalue")
                    Dim txtfromnoofdays As TextBox = GvRow.FindControl("txtfromnoofdays")

                    Dim chknonrefund As CheckBox = GvRow.Findcontrol("chknonrefund")

                    If mySqlReader.Read = True Then

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

                        End If

                        If IsDBNull(mySqlReader("mealplans")) = False Then
                            txtmealcode.Text = mySqlReader("mealplans")

                        End If


                        If IsDBNull(mySqlReader("nodayshours")) = False Then
                            txtnoofdays.Text = mySqlReader("nodayshours")

                        End If
                        If IsDBNull(mySqlReader("fromnodayhours")) = False Then
                            txtfromnoofdays.Text = mySqlReader("fromnodayhours")

                        End If
                        If IsDBNull(mySqlReader("dayshours")) = False Then
                            txtunits.Text = mySqlReader("dayshours")

                        End If
                        If IsDBNull(mySqlReader("chargebasis")) = False Then
                            txtcharge.Text = mySqlReader("chargebasis")

                        End If
                        If IsDBNull(mySqlReader("nightstocharge")) = False Then
                            txtnights.Text = IIf(mySqlReader("nightstocharge") = 0, "", (mySqlReader("nightstocharge")))

                        End If
                        If IsDBNull(mySqlReader("percentagetocharge")) = False Then
                            txtpercharge.Text = IIf(mySqlReader("percentagetocharge") = 0, "", DecRound(mySqlReader("percentagetocharge")))

                        End If
                        If IsDBNull(mySqlReader("valuetocharge")) = False Then
                            txtvalue.Text = IIf(mySqlReader("valuetocharge") = 0, "", DecRound(mySqlReader("valuetocharge")))

                        End If

                        If IsDBNull(mySqlReader("nonrefund")) = False Then
                            If mySqlReader("nonrefund") = 1 Then
                                chknonrefund.Checked = True
                            Else
                                chknonrefund.Checked = False
                            End If
                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            ''''' Fill No show Grid
            strQry = ""
            cnt = 0
            strSqlQry = ""

            strQry = "select count( distinct clineno) from view_contracts_cancelpolicy_noshowearly(nolock) where cancelpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdnoshow, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_cancelpolicy_noshowearly d(nolock)  where   d.cancelpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                myCommand.CommandTimeout = 0
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdnoshow.Rows

                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                    Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtnoofdays As TextBox = GvRow.FindControl("txtnoofdays")
                    Dim ddlnoshow As HtmlSelect = GvRow.FindControl("ddlnoshow")
                    Dim txtchargenoshow As TextBox = GvRow.FindControl("txtchargenoshow")
                    Dim txtnightsnoshow As TextBox = GvRow.FindControl("txtnightsnoshow")
                    Dim txtpercnoshow As TextBox = GvRow.Findcontrol("txtpercnoshow")
                    Dim txtvaluenoshow As TextBox = GvRow.Findcontrol("txtvaluenoshow")

                    If mySqlReader.Read = True Then

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

                        End If

                        If IsDBNull(mySqlReader("mealplans")) = False Then
                            txtmealcode.Text = mySqlReader("mealplans")

                        End If

                      

                    
                        If IsDBNull(mySqlReader("noshowearly")) = False Then
                            ddlnoshow.Value = mySqlReader("noshowearly")

                        End If
                        If IsDBNull(mySqlReader("chargebasis")) = False Then
                            txtchargenoshow.Text = mySqlReader("chargebasis")

                        End If
                        If IsDBNull(mySqlReader("nightstocharge")) = False Then
                            txtnightsnoshow.Text = IIf(mySqlReader("nightstocharge") = 0, "", (mySqlReader("nightstocharge")))

                        End If
                        If IsDBNull(mySqlReader("percentagetocharge")) = False Then
                            txtpercnoshow.Text = IIf(mySqlReader("percentagetocharge") = 0, "", DecRound(mySqlReader("percentagetocharge")))

                        End If
                        If IsDBNull(mySqlReader("valuetocharge")) = False Then
                            txtvaluenoshow.Text = IIf(mySqlReader("valuetocharge") = 0, "", DecRound(mySqlReader("valuetocharge")))

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If


            '''''''''''
            Enablegrid()




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                ShowManualDatesnew(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit Cancellation Policy - " + ViewState("hotelname")
                Page.Title = "Cancellation Policy "

            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                ShowManualDatesnew(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View Cancellation Policy - " + ViewState("hotelname")
                Page.Title = " Cancellation Policy "
            ElseIf e.CommandName = "DeleteRow" Then

                ViewState("State") = "Delete"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                ShowManualDatesnew(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete Cancellation Policy - " + ViewState("hotelname")
                Page.Title = "Cancellation Policy "
            ElseIf e.CommandName = "Copy" Then

                ViewState("State") = "Copy"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                If Session("Calledfrom") = "Offers" Then
                    ShowDatesnew(CType(hdnpromotionid.Value, String))
                    lblHeading.Text = "Copy Cancellation Policy - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
                    Page.Title = " Cancellation Policy "
                Else
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    lblHeading.Text = "Copy Cancellation Policy - " + ViewState("hotelname") + "-" + hdncontractid.Value
                    Page.Title = " Cancellation Policy "

                End If

                Showdetailsgrid(CType(lbltran.Text.Trim, String))
                ShowManualDatesnew(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"

            End If

            If Session("Calledfrom") = "Offers" Then
                divpromo.Style.Add("display", "block")
                divseason.Style.Add("display", "none")
                dv_SearchResult.Style.Add("display", "none")
            Else
                divpromo.Style.Add("display", "none")
                divseason.Style.Add("display", "block")
                dv_SearchResult.Style.Add("display", "block")
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                lbtext.Text = lbtext.Text.Substring(0, 10)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Protected Sub gv_Seasons_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Seasons.RowDataBound
        Dim txtseasoncode As Label
        Dim chkseason As CheckBox
     

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            txtseasoncode = e.Row.FindControl("txtseasoncode")
            chkseason = e.Row.FindControl("chkseason")



            chkseason.Attributes.Add("onChange", "datefill('" & chkseason.ClientID & "')")


        End If
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

            Dim myDSnew As New DataSet
            strSqlQry = ""
            strSqlQry = "select * from ( select distinct e.exhibitionname , d.exhibitioncode ,convert(varchar(10),d.fromdate,103) fromdate  ,convert(varchar(10),d.todate,103) todate,0 selected  from  view_contracts_exhibitions d(nolock),exhibition_master e(nolock),view_contracts_exhibition_header h(nolock)  " _
                & " where d.exhibitioncode =e.exhibitioncode  and h.exhibitionid =d.exhibitionid  and  h.contractid='" & hdncontractid.Value & "') h  order by h.fromdate desc "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDSnew)
            grdexhibition.DataSource = myDSnew

            If myDSnew.Tables(0).Rows.Count > 0 Then
                grdexhibition.DataBind()

            Else
                grdexhibition.DataBind()

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
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
                strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName from view_contractseasons_rate(nolock) where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ") order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"

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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    'Private Sub filldates()
    '    Try
    '        Dim myDS As New DataSet
    '        grdDates.Visible = True
    '        strSqlQry = ""

    '        strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' and seasonname='" & txtseasonname.Text & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '        myDataAdapter.Fill(myDS)
    '        grdDates.DataSource = myDS

    '        If myDS.Tables(0).Rows.Count > 0 Then
    '            grdDates.DataBind()

    '        Else
    '            grdDates.DataBind()

    '        End If




    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    Finally

    '        clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '        clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
    '    End Try
    'End Sub

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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnrmtypnoshow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex





        Dim strrmtypename As String = CType(grdnoshow.Rows(rowid).FindControl("txtrmtypcode"), TextBox).Text


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

        ViewState("noshowclick") = 1
        Enablegrid()
        ModalPopupNoshow.Show()


    End Sub
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

        Enablegrid()
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

        Enablegrid()
        ModalExtraPopup.Show()


    End Sub
    Protected Sub btnmealnoshow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdnoshow.Rows(rowid).FindControl("txtmealcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        'strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
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

        ViewState("noshowclick") = 1
        ModalPopupNoshow.Show()

        Enablegrid()
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
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        ViewState("State") = "New"
        wucCountrygroup.Visible = True
        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        lblstatus.Visible = False
        lblstatustext.Visible = False

        If Session("Calledfrom") = "Offers" Then

            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

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




            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            clearall()
            DisableControl()

            btncopyratesnextrow.Visible = True

            gv_Seasons.Visible = False
            grdexhibition.Visible = False
            divpromo.Style.Add("display", "block")
            divseason.Style.Add("display", "none")
            dv_SearchResult.Style.Add("display", "none")

            fillroomgrid(grdRoomrates, True)
            fillDategrd(grdnoshow, True)
            fillDategrd(grdpromodates, True)
            ShowDatesnew(CType(hdnpromotionid.Value, String))

            lable12.Visible = True
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            lblratetype.Text = "Offers"

            lblHeading.Text = "New Cancellation Policy - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + " Cancellation Policy -" + ViewState("hotelname") + "-" + hdnpromotionid.Value


            Dim roomtypes As String = ""
            Dim rmtypenames As String = ""
            Dim mealplans As String = ""
            Dim strQry As String = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

            strQry = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

            strQry = "select distinct stuff((select  ',' + p.rmtypname     from view_offers_header h(nolock) join  view_offers_rmtype u(nolock) on h.promotionid=u.promotionid  join partyrmtyp p on p.rmtypcode=u.rmtypcode and p.partycode=h.partycode   where   p.inactive=0 and u.promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            rmtypenames = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)


            For Each GvRow In grdRoomrates.Rows

                Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                Dim btnrmtyp As Button = GvRow.FindControl("btnrmtyp")
                Dim btnmeal As Button = GvRow.FindControl("btnmeal")

                txtrmtypcode.Text = roomtypes
                txtmealcode.Text = mealplans
                txtrmtypname.Text = rmtypenames

                'btnrmtyp.Enabled = False
                'btnmeal.Enabled = False
            Next

        Else
            Session("contractid") = hdncontractid.Value
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            fillseason()
            gv_Seasons.Visible = True
            grdexhibition.Visible = True
            grdDates.Visible = True
            grdMaualDates.Visible = True
            clearall()
            DisableControl()
            seasonsgridfill()


            btncopyratesnextrow.Visible = True
            divpromo.Style.Add("display", "none")
            divseason.Style.Add("display", "block")
            dv_SearchResult.Style.Add("display", "block")

            lable12.Visible = True
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            fillroomgrid(grdRoomrates, True)
            fillDategrd(grdnoshow, True)
            fillDategrd(grdMaualDates, True)
            lblratetype.Text = "Contract"

            lblHeading.Text = "New Cancellation Policy - " + ViewState("hotelname") + "-" + hdncontractid.Value
            Page.Title = Page.Title + " " + " Cancellation Policy -" + ViewState("hotelname") + "-" + hdncontractid.Value

            ' divuser.Style("display") = "none"
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
        End If

       
       






       
        'createdatacolumns()
        ' FillRoomdetails()

        btnSave.Visible = True

        btnSave.Text = "Save"
       



    End Sub
    Private Sub ShowManualDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            Else
                If Session("Calledfrom") = "Offers" Then
                    strSqlQry = "select count( distinct fromdate) from view_contracts_cancelpolicy_offerdates(nolock) where cancelpolicyid='" & RefCode & "'"
                Else
                    strSqlQry = "select count( distinct fromdate) from view_contracts_cancelpolicy_dates(nolock) where cancelpolicyid='" & RefCode & "'"
                End If

            End If



            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdMaualDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open    
            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)

            Else
                If Session("Calledfrom") = "Offers" Then
                    mySqlCmd = New SqlCommand("Select * from view_contracts_cancelpolicy_offerdates(nolock) Where cancelpolicyid='" & RefCode & "'", mySqlConn)
                Else
                    mySqlCmd = New SqlCommand("Select * from view_contracts_cancelpolicy_dates(nolock) Where cancelpolicyid='" & RefCode & "'", mySqlConn)
                End If

            End If

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()

                    If Session("Calledfrom") = "Offers" Then

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
                    Else
                        For Each gvRow In grdMaualDates.Rows
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

                    End If
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractCancpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            Else
                strSqlQry = "select count( distinct fromdate) from view_contracts_cancelpolicy_offerdates(nolock) where cancelpolicyid='" & RefCode & "'"
            End If



            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdpromodates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open    
            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)

            Else
                mySqlCmd = New SqlCommand("Select * from view_contracts_cancelpolicy_offerdates(nolock) Where cancelpolicyid='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractCancpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Sub clearall()
        ' fillDategrd(grdDates, True)
        txtplistcode.Text = ""
        grdRoomrates.Enabled = True
        grdnoshow.Enabled = True
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

        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "CONTRACTCANCEL", CType(Session("ContractState"), String))
        ddlorder.SelectedIndex = 0
        ddlorderby.SelectedIndex = 1
        
        If Session("Calledfrom") = "Offers" Then

            lblHeading.Text = "Cancellation Policy  -" + ViewState("hotelname") + "-" + hdnpromotionid.Value
            
            FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
        Else
            lblHeading.Text = "Cancellation Policy  -" + ViewState("hotelname") + "-" + hdncontractid.Value
            
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
            Dim strFoucsColumnIndex = "3"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        End If




        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim txtrmtypcode As TextBox = e.Row.FindControl("txtrmtypcode")
            Dim txtmealcode As TextBox = e.Row.FindControl("txtmealcode")
            Dim txtnoofdays As TextBox = e.Row.FindControl("txtnoofdays")
            Dim txtnights As TextBox = e.Row.FindControl("txtnights")
            Dim txtpercharge As TextBox = e.Row.FindControl("txtpercharge")
            Dim txtvalue As TextBox = e.Row.FindControl("txtvalue")
            Dim txtcharge As TextBox = e.Row.FindControl("txtcharge")


            Numberssrvctrl(txtnoofdays)
            Numberssrvctrl(txtnights)
            Numberssrvctrl(txtpercharge)
            Numberssrvctrl(txtvalue)

            If txtcharge.Text <> "" And (txtcharge.Text = "Nights") Then
                txtnights.Enabled = True
                txtpercharge.Enabled = False
                txtvalue.Enabled = False


            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% of Nights") Then
                txtpercharge.Enabled = True
                txtnights.Enabled = True
                txtvalue.Enabled = False

            End If
            If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay") Then
                txtpercharge.Enabled = True
                txtnights.Enabled = False
                txtvalue.Enabled = False

            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "Value") Then
                txtvalue.Enabled = True
                txtnights.Enabled = False
                txtpercharge.Enabled = False

            End If


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
        Dim noofdays(count) As String
        Dim units(count) As String
        Dim charge(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim fromnoofdays(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtnoofdays As TextBox = GVRow.FindControl("txtnoofdays")
                Dim txtunits As TextBox = GVRow.FindControl("txtunits")
                Dim txtcharge As TextBox = GVRow.FindControl("txtcharge")
                Dim txtpercharge As TextBox = GVRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = GVRow.FindControl("txtvalue")
                Dim txtnights As TextBox = GVRow.FindControl("txtnights")
                Dim txtfromnoofdays As TextBox = GVRow.FindControl("txtfromnoofdays")





                If chkSelect.Checked = True Then

                    CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                noofdays(n) = CType(txtnoofdays.Text, String)
                units(n) = CType(txtunits.Text, String)
                charge(n) = CType(txtcharge.Text, String)
                percharge(n) = CType(txtpercharge.Text, String)
                value(n) = CType(txtvalue.Text, String)
                nights(n) = CType(txtnights.Text, String)
                fromnoofdays(n) = CType(txtfromnoofdays.Text, String)




                txtrmtypcodenew.Add(CType(txtrmtypcode.Text, String))
                txtrmtypnamenew.Add(CType(txtrmtypname.Text, String))
                txtmealcodenew.Add(CType(txtmealcode.Text, String))
                txtnoofdaysnew.Add(CType(txtnoofdays.Text, String))
                txtunitsnew.Add(CType(txtunits.Text, String))
                txtchargenew.Add(CType(txtcharge.Text, String))
                txtperchargenew.Add(CType(txtpercharge.Text, String))
                txtvaluenew.Add(CType(txtvalue.Text, String))
                txtnightsnew.Add(CType(txtnights.Text, String))
                txtfromnoofdaysnew.Add(CType(txtfromnoofdays.Text, String))




                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub
    Private Sub copylinesnoshow()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdnoshow.Rows.Count + 1

        Dim n As Integer = 0



        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim ddlnoshow(count) As String
        Dim chargenoshow(count) As String
        Dim perchargenoshow(count) As String
        Dim valuenoshow(count) As String
        Dim nightsnoshow(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In grdnoshow.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim ddlnoshow1 As HtmlSelect = GVRow.FindControl("ddlnoshow")

                Dim txtchargenoshow As TextBox = GVRow.FindControl("txtchargenoshow")
                Dim txtpercnoshow As TextBox = GVRow.FindControl("txtpercnoshow")
                Dim txtvaluenoshow As TextBox = GVRow.FindControl("txtvaluenoshow")
                Dim txtnightsnoshow As TextBox = GVRow.FindControl("txtnightsnoshow")





                If chkSelect.Checked = True Then

                    CopyRownoshow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                ddlnoshow(n) = CType(ddlnoshow1.Value, String)

                chargenoshow(n) = CType(txtchargenoshow.Text, String)
                perchargenoshow(n) = CType(txtpercnoshow.Text, String)
                valuenoshow(n) = CType(txtvaluenoshow.Text, String)
                nightsnoshow(n) = CType(txtnightsnoshow.Text, String)




                txtrmtypcodenoshownew.Add(CType(txtrmtypcode.Text, String))
                txtrmtypnamenoshownew.Add(CType(txtrmtypname.Text, String))
                txtmealcodenoshownew.Add(CType(txtmealcode.Text, String))
                ddlnoshow1new.Add(CType(ddlnoshow1.Value, String))

                txtchargenoshownew.Add(CType(txtchargenoshow.Text, String))
                txtpercnoshownew.Add(CType(txtpercnoshow.Text, String))
                txtvaluenoshownew.Add(CType(txtvaluenoshow.Text, String))
                txtnightsnoshownew.Add(CType(txtnightsnoshow.Text, String))




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

        Enablegrid()

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

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtnoofdays As TextBox = GVRow.FindControl("txtnoofdays")
                Dim txtunits As TextBox = GVRow.FindControl("txtunits")
                Dim txtcharge As TextBox = GVRow.FindControl("txtcharge")
                Dim txtpercharge As TextBox = GVRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = GVRow.FindControl("txtvalue")
                Dim txtnights As TextBox = GVRow.FindControl("txtnights")
                Dim txtfromnoofdays As TextBox = GVRow.FindControl("txtfromnoofdays")




                If n > CopyRow And txtrmtypname.Text = "" Then

                    txtrmtypcode.Text = txtrmtypcodenew.Item(CopyRow)
                    txtrmtypname.Text = txtrmtypnamenew.Item(CopyRow)
                    txtmealcode.Text = txtmealcodenew.Item(CopyRow)
                    txtnoofdays.Text = txtnoofdaysnew.Item(CopyRow)
                    txtunits.Text = txtunitsnew.Item(CopyRow)

                    txtcharge.Text = txtchargenew.Item(CopyRow)
                    txtpercharge.Text = txtperchargenew.Item(CopyRow)
                    txtvalue.Text = txtvaluenew.Item(CopyRow)
                    txtnights.Text = txtnightsnew.Item(CopyRow)
                    txtfromnoofdays.Text = txtfromnoofdaysnew.Item(CopyRow)

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



    Protected Sub btnfillrate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillrate.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim roomcat As String
        Dim roomcatstring As String
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Dim fnd As Integer = 0
        Try
            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If chk.Checked = True Then
                    cnt_checked = cnt_checked + 1
                End If
            Next
            If cnt_checked = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row.');", True)
                SetFocus(cnt_checked)
                Exit Sub
            End If


            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                            Else

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    pkg = 0
                    For j = n To (m + n) - 1
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                            Else

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                'pkg = pkg + 1
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next


            '--------------------------------------------------------------------------------------------
            'Noe Fill Record to Cell
            Dim validprice As Integer
            room = 0
            'pkg = 0
            n = 0
            b = 0
            validprice = 0
            Dim ds As DataSet
            For Each GvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                        Else
                            ' txt = GvRow.FindControl("txt" & b + a + 3)
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    'Check the price exists in the line
                                    roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                    If heading(room) = roomcatstring Then
                                        If Val(txt.Text) <> "0" Or txt.Text <> "" Then
                                            validprice = 1
                                        End If
                                    End If
                                    roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                    If heading(room) = roomcat Then
                                        If validprice = 1 Then
                                            txt.Text = arr_room(room)
                                        End If
                                    End If
                                End If
                            End If
                            room = room + 1
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                'Check the price exists in the line
                                roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                If heading(room) = roomcatstring Then
                                    If Val(txt.Text) <> "0" Or txt.Text <> "" Then
                                        validprice = 1
                                    End If
                                End If
                                roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                If heading(room) = roomcat Then
                                    'If validprice = 1 Then
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room) + Val(txtfillrate.Value)
                                        arr_room(room) = txt.Text
                                    End If
                                    'End If
                                End If
                            End If
                            room = room + 1
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
                room = 0
                validprice = 0
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        Dim txtnoofdays As TextBox
        Dim txtunits As TextBox
        Dim txtcharge As TextBox
        Dim txtpercharge As TextBox
        Dim txtvalue As TextBox
        Dim txtnights As TextBox
        Dim txtfromnoofdays As TextBox
        Dim chknonrefund As CheckBox


        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim noofdays(count) As String
        Dim units(count) As String
        Dim charge(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim fromnoofdays(count) As String
        Dim nonrefund(count) As String



        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows
                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtmealcode = GVRow.FindControl("txtmealcode")
                txtnoofdays = GVRow.FindControl("txtnoofdays")
                txtunits = GVRow.FindControl("txtunits")
                txtcharge = GVRow.FindControl("txtcharge")
                txtpercharge = GVRow.FindControl("txtpercharge")
                txtvalue = GVRow.FindControl("txtvalue")
                txtnights = GVRow.FindControl("txtnights")
                txtfromnoofdays = GVRow.FindControl("txtfromnoofdays")
                chknonrefund = GVRow.FindControl("chknonrefund")

                chkSelect = GVRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                noofdays(n) = CType(txtnoofdays.Text, String)
                units(n) = CType(txtunits.Text, String)
                charge(n) = CType(txtcharge.Text, String)
                percharge(n) = CType(txtpercharge.Text, String)
                value(n) = CType(txtvalue.Text, String)
                nights(n) = CType(txtnights.Text, String)
                fromnoofdays(n) = CType(txtfromnoofdays.Text, String)

                If chknonrefund.Checked = True Then
                    nonrefund(n) = 1
                Else
                    nonrefund(n) = 0
                End If



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
                txtnoofdays = GVRow.FindControl("txtnoofdays")
                txtunits = GVRow.FindControl("txtunits")
                txtcharge = GVRow.FindControl("txtcharge")
                txtpercharge = GVRow.FindControl("txtpercharge")
                txtvalue = GVRow.FindControl("txtvalue")
                txtnights = GVRow.FindControl("txtnights")
                chkSelect = GVRow.FindControl("chkSelect")
                txtfromnoofdays = GVRow.FindControl("txtfromnoofdays")
                chknonrefund = GVRow.FindControl("chknonrefund")


                txtrmtypcode.Text = rmtypcode(n)
                txtrmtypname.Text = rmtypname(n)
                txtmealcode.Text = mealcode(n)
                txtnoofdays.Text = noofdays(n)
                txtunits.Text = units(n)
                txtcharge.Text = charge(n)
                txtpercharge.Text = percharge(n)
                txtvalue.Text = value(n)
                txtnights.Text = nights(n)
                txtfromnoofdays.Text = fromnoofdays(n)

                If nonrefund(n) = 1 Then
                    chknonrefund.Checked = True
                Else
                    chknonrefund.Checked = False
                End If



                n = n + 1
            Next
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

        'Try

        '    cnt = 0
        '    Session("GV_HotelData") = Nothing
        '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        '    strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
        '              & hdnpartycode.Value & "' "  ' p.rmcatcode " '
        '    mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        '    cnt = mySqlCmd.ExecuteScalar
        '    mySqlConn.Close()

        '    ReDim arr(cnt + 1)
        '    ReDim arrRName(cnt + 1)
        '    Dim i As Long = 0

        '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
        '                & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

        '    mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        '    mySqlReader = mySqlCmd.ExecuteReader()
        '    While mySqlReader.Read = True
        '        arr(i) = mySqlReader("rmcatcode")
        '        i = i + 1
        '    End While
        '    mySqlReader.Close()
        '    mySqlConn.Close()


        '    Dim tf As New TemplateField
        '    dt = New DataTable

        '    dt.Columns.Add(New DataColumn("RoomTypecode", GetType(String)))
        '    dt.Columns.Add(New DataColumn("Select_RoomType", GetType(String)))
        '    dt.Columns.Add(New DataColumn("Meal_Plan", GetType(String)))
        '    dt.Columns.Add(New DataColumn("MaxChild Allowed", GetType(String)))
        '    dt.Columns.Add(New DataColumn("MaxEB Allowed", GetType(String)))
        '    dt.Columns.Add(New DataColumn("No.of Children", GetType(String)))
        '    dt.Columns.Add(New DataColumn("From Age", GetType(String)))
        '    dt.Columns.Add(New DataColumn("To Age", GetType(String)))
        '    dt.Columns.Add(New DataColumn("Charge for Sharing", GetType(String)))
        '    dt.Columns.Add(New DataColumn("Charge for EB", GetType(String)))

        '    'create columns of this room types in data table
        '    For i = 0 To cnt - 1
        '        dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
        '    Next
        '    'Dim fld2 As String = ""
        '    'Dim col As DataColumn
        '    'For Each col In dt.Columns
        '    '    If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" Then
        '    '        Dim bfield As New TemplateField
        '    '        'Call Function
        '    '        bfield.HeaderTemplate = New ClassChildPolicy(ListItemType.Header, col.ColumnName, fld2)
        '    '        bfield.ItemTemplate = New ClassChildPolicy(ListItemType.Item, col.ColumnName, fld2)
        '    '        grdRoomrates.Columns.Add(bfield)


        '    '    End If
        '    'Next

        '    Session("GV_HotelData") = dt

        '    For Each gvRow In grdRoomrates.Rows

        '        dr = dt.NewRow

        '        Dim chkSelect As CheckBox = gvRow.FindControl("chkSelect")
        '        If mode.Trim.ToUpper <> "DELETE" Or (mode.Trim.ToUpper = "DELETE" And chkSelect.Checked = False) Then
        '            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
        '            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
        '            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
        '            Dim txtmaxchild As TextBox = gvRow.FindControl("txtmaxchild")
        '            Dim txtmaxeb As TextBox = gvRow.FindControl("txtmaxeb")
        '            Dim txtnoofchild As TextBox = gvRow.FindControl("txtnoofchild")
        '            Dim txtfromage As TextBox = gvRow.FindControl("txtfromage")
        '            Dim txttoage As TextBox = gvRow.FindControl("txttoage")
        '            Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")

        '            Dim txtEB As TextBox = gvRow.FindControl("txtEB")


        '            dr("RoomTypecode") = txtrmtypcode.Text
        '            dr("Select_RoomType") = txtrmtypname.Text
        '            dr("Meal_Plan") = txtmealcode.Text
        '            dr("MaxChild Allowed") = txtmaxchild.Text
        '            dr("MaxEB Allowed") = txtmaxeb.Text
        '            dr("No.of Children") = txtnoofchild.Text
        '            dr("From Age") = txtfromage.Text
        '            dr("To Age") = txttoage.Text
        '            dr("Charge for Sharing") = txtsharing.Text
        '            dr("Charge for EB") = txtEB.Text

        '            dt.Rows.Add(dr)
        '        End If


        '    Next


        '    If mode.Trim.ToUpper = "ADD" Then
        '        dr = dt.NewRow
        '        dt.Rows.Add(dr)

        '    End If



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


        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim txtnoofdays As TextBox
        Dim txtunits As TextBox
        Dim txtcharge As TextBox
        Dim txtpercharge As TextBox
        Dim txtvalue As TextBox
        Dim txtnights As TextBox
        Dim txtfromnoofdays As TextBox
        Dim chknonrefund As CheckBox


        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim noofdays(count) As String
        Dim units(count) As String
        Dim charge(count) As String
        Dim percharge(count) As String
        Dim value(count) As String
        Dim nights(count) As String
        Dim fromnoofdays(count) As String
        Dim nonrefund(count) As String

        Try
            For Each GVRow In grdRoomrates.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtmealcode = GVRow.FindControl("txtmealcode")
                    txtnoofdays = GVRow.FindControl("txtnoofdays")
                    txtunits = GVRow.FindControl("txtunits")
                    txtcharge = GVRow.FindControl("txtcharge")
                    txtpercharge = GVRow.FindControl("txtpercharge")
                    txtvalue = GVRow.FindControl("txtvalue")
                    txtnights = GVRow.FindControl("txtnights")
                    txtfromnoofdays = GVRow.FindControl("txtfromnoofdays")
                    chknonrefund = GVRow.FindControl("chknonrefund")


                    rmtypcode(n) = CType(txtrmtypcode.Text, String)
                    rmtypname(n) = CType(txtrmtypname.Text, String)
                    mealcode(n) = CType(txtmealcode.Text, String)
                    noofdays(n) = CType(txtnoofdays.Text, String)
                    units(n) = CType(txtunits.Text, String)
                    charge(n) = CType(txtcharge.Text, String)
                    percharge(n) = CType(txtpercharge.Text, String)
                    value(n) = CType(txtvalue.Text, String)
                    nights(n) = CType(txtnights.Text, String)
                    fromnoofdays(n) = CType(txtfromnoofdays.Text, String)

                    If chknonrefund.Checked = True Then
                        nonrefund(n) = 1
                    Else
                        nonrefund(n) = 0
                    End If


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
                    txtnoofdays = GVRow.FindControl("txtnoofdays")
                    txtunits = GVRow.FindControl("txtunits")
                    txtcharge = GVRow.FindControl("txtcharge")
                    txtpercharge = GVRow.FindControl("txtpercharge")
                    txtvalue = GVRow.FindControl("txtvalue")
                    txtnights = GVRow.FindControl("txtnights")
                    txtfromnoofdays = GVRow.FindControl("txtfromnoofdays")
                    chknonrefund = GVRow.FindControl("chknonrefund")


                    txtrmtypcode.Text = rmtypcode(n)
                    txtrmtypname.Text = rmtypname(n)
                    txtmealcode.Text = mealcode(n)
                    txtnoofdays.Text = noofdays(n)
                    txtunits.Text = units(n)
                    txtcharge.Text = charge(n)
                    txtpercharge.Text = percharge(n)
                    txtvalue.Text = value(n)
                    txtnights.Text = nights(n)
                    txtfromnoofdays.Text = fromnoofdays(n)

                    If nonrefund(n) = 1 Then
                        chknonrefund.Checked = True
                    Else
                        chknonrefund.Checked = False
                    End If


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

        'If txtseasonname.Text = "" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Season.');", True)
        '    ' SetFocus(txtseasonname)
        '    Exit Sub
        'End If

        GenRemark()
        disablegrid()

    End Sub
    Private Sub Enablegrid()
        For Each gvRow In grdRoomrates.Rows

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtnoofdays As TextBox = gvRow.FindControl("txtnoofdays")
            Dim txtunits As TextBox = gvRow.FindControl("txtunits")
            Dim txtcharge As TextBox = gvRow.FindControl("txtcharge")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim btnrmtyp As Button = gvRow.Findcontrol("btnrmtyp")
            Dim btnmeal As Button = gvRow.Findcontrol("btnmeal")

            txtnoofdays.Enabled = True
            txtunits.Enabled = True
            txtcharge.Enabled = True
            txtnights.Enabled = True
            txtpercharge.Enabled = True
            txtvalue.Enabled = True
            btnrmtyp.Enabled = True
            btnmeal.Enabled = True


            If txtcharge.Text <> "" And (txtcharge.Text = "Nights") Then
                txtnights.Enabled = True
                txtpercharge.Enabled = False
                txtvalue.Enabled = False

            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% of Nights") Then
                txtpercharge.Enabled = True
                txtnights.Enabled = True
                txtvalue.Enabled = False

            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay") Then
                txtpercharge.Enabled = True
                txtnights.Enabled = False
                txtvalue.Enabled = False

            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "Value") Then
                txtvalue.Enabled = True
                txtnights.Enabled = False
                txtpercharge.Enabled = False

            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% with Max Nights") Then
                txtvalue.Enabled = False
                txtnights.Enabled = True
                txtpercharge.Enabled = True

            End If



        Next

        For Each gvRow In grdnoshow.Rows

            Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")


            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

            Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
            Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
            Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
            Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")
            Dim btnrmtypnosow As Button = gvRow.Findcontrol("btnrmtypnosow")
            Dim btnmealnoshow As Button = gvRow.Findcontrol("btnmealnoshow")

            ddlnoshow.Disabled = False
            txtchargenoshow.Enabled = True
            txtnightsnoshow.Enabled = True
            txtpercnoshow.Enabled = True
            txtvaluenoshow.Enabled = True
            btnrmtypnosow.Enabled = True
            btnmealnoshow.Enabled = True

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Nights") Then
                txtnightsnoshow.Enabled = True
                txtpercnoshow.Enabled = False
                txtvaluenoshow.Enabled = False

            End If

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Nights") Then
                txtpercnoshow.Enabled = True
                txtnightsnoshow.Enabled = True
                txtvaluenoshow.Enabled = False

            End If

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of Entire Stay") Then
                txtpercnoshow.Enabled = True
                txtnightsnoshow.Enabled = False
                txtvaluenoshow.Enabled = False

            End If

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% of remaining Nights") Then
                txtpercnoshow.Enabled = True
                txtnightsnoshow.Enabled = False
                txtvaluenoshow.Enabled = False

            End If

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "Value") Then
                txtvaluenoshow.Enabled = True
                txtnightsnoshow.Enabled = False
                txtpercnoshow.Enabled = False

            End If

            If txtchargenoshow.Text <> "" And (txtchargenoshow.Text = "% with Max Nights") Then
                txtvaluenoshow.Enabled = False
                txtnightsnoshow.Enabled = True
                txtpercnoshow.Enabled = True

            End If

        Next
    End Sub
    Private Sub disablegrid()

        For Each gvRow In grdRoomrates.Rows

            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtnoofdays As TextBox = gvRow.FindControl("txtnoofdays")
            Dim txtunits As TextBox = gvRow.FindControl("txtunits")
            Dim txtcharge As TextBox = gvRow.FindControl("txtcharge")
            Dim txtnights As TextBox = gvRow.FindControl("txtnights")
            Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
            Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")
            Dim btnrmtyp As Button = gvRow.Findcontrol("btnrmtyp")
            Dim btnmeal As Button = gvRow.Findcontrol("btnmeal")

            txtnoofdays.Enabled = False
            txtunits.Enabled = False
            txtcharge.Enabled = False
            txtnights.Enabled = False
            txtpercharge.Enabled = False
            txtvalue.Enabled = False
            btnrmtyp.Enabled = False
            btnmeal.Enabled = False



        Next

        For Each gvRow In grdnoshow.Rows

            Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")


            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

            Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
            Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
            Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
            Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")
            Dim btnrmtypnosow As Button = gvRow.Findcontrol("btnrmtypnosow")
            Dim btnmealnoshow As Button = gvRow.Findcontrol("btnmealnoshow")

            ddlnoshow.Disabled = True
            txtchargenoshow.Enabled = False
            txtnightsnoshow.Enabled = False
            txtpercnoshow.Enabled = False
            txtvaluenoshow.Enabled = False
            btnrmtypnosow.Enabled = False
            btnmealnoshow.Enabled = False
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


        strRemark = ""
        strRemark = strRemark + seasonname + " Season"
        ' strRemark = strRemark & "For" & " " & (roomtype) & " -- occupancies" & " " & roomcategory & vbCrLf
        ' - "occupancies" & (roomcategory)
        If flgEB = True Then
            For Each gvRow In grdRoomrates.Rows

                Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
                Dim txtnoofdays As TextBox = gvRow.FindControl("txtnoofdays")
                Dim txtunits As TextBox = gvRow.FindControl("txtunits")
                Dim txtcharge As TextBox = gvRow.FindControl("txtcharge")
                Dim txtnights As TextBox = gvRow.FindControl("txtnights")
                Dim txtpercharge As TextBox = gvRow.FindControl("txtpercharge")
                Dim txtvalue As TextBox = gvRow.FindControl("txtvalue")




                '  strRemark = strRemark + IIf(strRemark.Length > 0, vbCrLf, "")



                If txtrmtypname.Text <> "" And txtrmtypcode.Text <> " " Then



                    If txtnoofdays.Text <> "" Then

                        strRemark = strRemark & vbCrLf + "" + txtrmtypname.Text & " On " & txtmealcode.Text & " ."

                        If txtunits.Text.ToUpper = UCase("Days") Then
                            strRemark = strRemark & vbCrLf + "Cancellation " & txtnoofdays.Text & " Days prior arrival will be charged "

                            If txtcharge.Text = "Nights" Then
                                If txtnights.Text = "1" Then
                                    strRemark = strRemark & " " + (txtnights.Text) + " night."
                                Else
                                    strRemark = strRemark & " " + (txtnights.Text) + " nights."
                                End If

                            ElseIf txtcharge.Text = "% of Nights" Then
                                If txtnights.Text = "1" Then
                                    strRemark = strRemark & " " + txtpercharge.Text + "% of " + txtnights.Text + " night."
                                Else
                                    strRemark = strRemark & " " + txtpercharge.Text + "% of " + txtnights.Text + " nights."
                                End If
                            ElseIf txtcharge.Text = "% of Entire Stay" Then
                                strRemark = strRemark & " " + (txtpercharge.Text) + "% of entire stay."
                            ElseIf txtcharge.Text = "Value" Then
                                strRemark = strRemark & " " + basecurrency + " " + (txtvalue.Text) + "."
                            End If
                        Else
                            strRemark = strRemark & vbCrLf + "Cancellation " & txtnoofdays.Text & " Hours prior arrival will be charged "

                            If txtcharge.Text = "Nights" Then
                                If txtnights.Text = "1" Then
                                    strRemark = strRemark & " " + txtnights.Text + " night ."
                                Else
                                    strRemark = strRemark & " " + txtnights.Text + " nights."
                                End If

                            ElseIf txtcharge.Text = "% of Nights" Then
                                If txtnights.Text = "1" Then
                                    strRemark = strRemark & " " + txtpercharge.Text + "% of " + txtnights.Text + " night."
                                Else
                                    strRemark = strRemark & " " + txtpercharge.Text + "% of " + txtnights.Text + " nights."
                                End If

                            ElseIf txtcharge.Text = "% of Entire Stay" Then
                                strRemark = strRemark & " " + txtpercharge.Text + "% of entire stay."
                            ElseIf txtcharge.Text = "Value" Then
                                strRemark = strRemark & " " + basecurrency + " " + txtvalue.Text + "."
                            End If

                        End If
                    End If


                    strRemark = strRemark

                End If

            Next

            For Each gvRow In grdnoshow.Rows


                Dim ddlnoshow As HtmlSelect = gvRow.FindControl("ddlnoshow")
                Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")

                Dim txtchargenoshow As TextBox = gvRow.FindControl("txtchargenoshow")
                Dim txtnightsnoshow As TextBox = gvRow.FindControl("txtnightsnoshow")
                Dim txtpercnoshow As TextBox = gvRow.FindControl("txtpercnoshow")
                Dim txtvaluenoshow As TextBox = gvRow.FindControl("txtvaluenoshow")

                If txtrmtypname.Text <> "" And txtmealcode.Text <> "" Then
                    strRemark = strRemark & vbCrLf + "" + txtrmtypname.Text & " On " & txtmealcode.Text & " ."
                End If
                If UCase(ddlnoshow.Value) = UCase("No Show") And txtchargenoshow.Text <> "" Then

                    strRemark = strRemark & vbCrLf + "" & ddlnoshow.Value & "   will be charged "

                    If txtchargenoshow.Text = "Nights" Then
                        If txtnightsnoshow.Text = "1" Then
                            strRemark = strRemark & " " + txtnightsnoshow.Text + " night."
                        Else
                            strRemark = strRemark & " " + txtnightsnoshow.Text + " nights."
                        End If

                    ElseIf txtchargenoshow.Text = "% of Nights" Then
                        If txtnightsnoshow.Text = "1" Then
                            strRemark = strRemark & " " + txtpercnoshow.Text + "% of " + txtnightsnoshow.Text + " night."
                        Else
                            strRemark = strRemark & " " + txtpercnoshow.Text + "% of " + txtnightsnoshow.Text + " nights."
                        End If

                    ElseIf txtchargenoshow.Text = "% of Entire Stay" Then
                        strRemark = strRemark & " " + txtpercnoshow.Text + "% of entire stay."
                    ElseIf txtchargenoshow.Text = "Value" Then
                        strRemark = strRemark & " " + basecurrency + " " + Val(txtvaluenoshow.Text) + " to be charged."
                    ElseIf txtchargenoshow.Text = "% of remaining Nights" Then
                        strRemark = strRemark & " " + txtpercnoshow.Text + "% remaining nights charged."
                    End If
                ElseIf UCase(ddlnoshow.Value) <> UCase("No Show") And txtchargenoshow.Text <> "" Then
                    strRemark = strRemark & vbCrLf + "" & ddlnoshow.Value & " will be charged "

                    If txtchargenoshow.Text = "Nights" Then
                        If txtnightsnoshow.Text = "1" Then
                            strRemark = strRemark & " " + txtnightsnoshow.Text + " night."
                        Else
                            strRemark = strRemark & " " + txtnightsnoshow.Text + " nights."
                        End If
                    ElseIf txtchargenoshow.Text = "% of Nights" Then
                        If txtnightsnoshow.Text = "1" Then
                            strRemark = strRemark & " " + txtpercnoshow.Text + "% of " + txtnightsnoshow.Text + " night."
                        Else
                            strRemark = strRemark & " " + txtpercnoshow.Text + "% of " + txtnightsnoshow.Text + " nights."
                        End If
                    ElseIf txtchargenoshow.Text = "% of Entire Stay" Then
                        strRemark = strRemark & " " + txtpercnoshow.Text + "% of entire stay."
                    ElseIf txtchargenoshow.Text = "Value" Then
                        strRemark = strRemark & " " + basecurrency + " " + txtvaluenoshow.Text + " to be charged."
                    ElseIf txtchargenoshow.Text = "% of remaining Nights" Then
                        strRemark = strRemark & " " + txtpercnoshow.Text + "% of remaining nights."
                    End If

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


    Protected Sub grdnoshow_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdnoshow.RowDataBound
  
        If (e.Row.RowType = DataControlRowType.DataRow) Then


            Dim txtnightsnoshow As TextBox = e.Row.FindControl("txtnightsnoshow")
            Dim txtpercnoshow As TextBox = e.Row.FindControl("txtpercnoshow")
            Dim txtvaluenoshow As TextBox = e.Row.FindControl("txtvaluenoshow")
            Dim txtcharge As TextBox = e.Row.FindControl("txtchargenoshow")


            Numberssrvctrl(txtnightsnoshow)
            Numberssrvctrl(txtpercnoshow)
            Numberssrvctrl(txtvaluenoshow)



            If txtcharge.Text <> "" And (txtcharge.Text = "Nights" Or txtcharge.Text = "% of Nights") Then
                txtnightsnoshow.Enabled = True
            Else
                txtnightsnoshow.Enabled = False
            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% of Entire Stay" Or txtcharge.Text = "% of Nights") Then
                txtpercnoshow.Enabled = True
            Else
                txtpercnoshow.Enabled = False
            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "Value") Then
                txtvaluenoshow.Enabled = True
            Else
                txtvaluenoshow.Enabled = False
            End If

            If txtcharge.Text <> "" And (txtcharge.Text = "% with Max Nights") Then
                txtpercnoshow.Enabled = True
                txtnightsnoshow.Enabled = True
            Else
                txtvaluenoshow.Enabled = False
            End If



        End If

        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            Dim strGridName As String = grdnoshow.ClientID
            Dim strRowId As String = e.Row.RowIndex
            Dim strFoucsColumnIndex As String = "4"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        End If

    End Sub

    Protected Sub btnaddrownoshow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddrownoshow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow


        count = grdnoshow.Rows.Count + 1

        Dim n As Integer = 0

        Dim chkSelect As CheckBox

        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim ddlnoshow As HtmlSelect

        Dim txtchargenoshow As TextBox
        Dim txtperchargenoshow As TextBox
        Dim txtvaluenoshow As TextBox
        Dim txtnightsnoshow As TextBox


        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim noshow(count) As String

        Dim chargenoshow(count) As String
        Dim perchargenoshow(count) As String
        Dim valuenoshow(count) As String
        Dim nightsnoshow(count) As String


        '   CopyRow = 0


        Try

            For Each GVRow In grdnoshow.Rows
                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtmealcode = GVRow.FindControl("txtmealcode")
                ddlnoshow = GVRow.FindControl("ddlnoshow")

                txtchargenoshow = GVRow.FindControl("txtchargenoshow")
                txtperchargenoshow = GVRow.FindControl("txtpercnoshow")
                txtvaluenoshow = GVRow.FindControl("txtvaluenoshow")
                txtnightsnoshow = GVRow.FindControl("txtnightsnoshow")


                chkSelect = GVRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                noshow(n) = CType(ddlnoshow.Value, String)

                chargenoshow(n) = CType(txtchargenoshow.Text, String)
                perchargenoshow(n) = CType(txtperchargenoshow.Text, String)
                valuenoshow(n) = CType(txtvaluenoshow.Text, String)
                nightsnoshow(n) = CType(txtnightsnoshow.Text, String)




                n = n + 1
            Next
            fillDategrd(grdnoshow, False, grdnoshow.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdnoshow.Rows
                If n = i Then

                    Exit For
                End If
                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtmealcode = GVRow.FindControl("txtmealcode")
                ddlnoshow = GVRow.FindControl("ddlnoshow")


                txtchargenoshow = GVRow.FindControl("txtchargenoshow")
                txtperchargenoshow = GVRow.FindControl("txtpercnoshow")
                txtvaluenoshow = GVRow.FindControl("txtvaluenoshow")
                txtnightsnoshow = GVRow.FindControl("txtnightsnoshow")
                chkSelect = GVRow.FindControl("chkSelect")


                txtrmtypcode.Text = rmtypcode(n)
                txtrmtypname.Text = rmtypname(n)
                txtmealcode.Text = mealcode(n)
                ddlnoshow.Value = noshow(n)

                txtchargenoshow.Text = chargenoshow(n)
                txtperchargenoshow.Text = perchargenoshow(n)
                txtvaluenoshow.Text = valuenoshow(n)
                txtnightsnoshow.Text = nightsnoshow(n)



                n = n + 1

            Next
            Dim gridNewrow As GridViewRow
            gridNewrow = grdnoshow.Rows(grdnoshow.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdnoshow.ClientID
            Dim strFoucsColumnIndex As String = "4"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdnoshow.Rows.Count - 1, String) + "','" + strGridName + "', '" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndelrownoshow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelrownoshow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdnoshow.Rows.Count + 1


        Dim n As Integer = 0

        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0


        Dim txtrmtypcode As TextBox
        Dim txtrmtypname As TextBox
        Dim txtmealcode As TextBox
        Dim ddlnoshow As HtmlSelect

        Dim txtchargenoshow As TextBox
        Dim txtperchargenoshow As TextBox
        Dim txtvaluenoshow As TextBox
        Dim txtnightsnoshow As TextBox


        Dim rmtypcode(count) As String
        Dim rmtypname(count) As String
        Dim mealcode(count) As String
        Dim noshow(count) As String

        Dim chargenoshow(count) As String
        Dim perchargenoshow(count) As String
        Dim valuenoshow(count) As String
        Dim nightsnoshow(count) As String

        Try
            For Each GVRow In grdnoshow.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtmealcode = GVRow.FindControl("txtmealcode")
                    ddlnoshow = GVRow.FindControl("ddlnoshow")

                    txtchargenoshow = GVRow.FindControl("txtchargenoshow")
                    txtperchargenoshow = GVRow.FindControl("txtpercnoshow")
                    txtvaluenoshow = GVRow.FindControl("txtvaluenoshow")
                    txtnightsnoshow = GVRow.FindControl("txtnightsnoshow")


                    rmtypcode(n) = CType(txtrmtypcode.Text, String)
                    rmtypname(n) = CType(txtrmtypname.Text, String)
                    mealcode(n) = CType(txtmealcode.Text, String)
                    noshow(n) = CType(ddlnoshow.Value, String)

                    chargenoshow(n) = CType(txtchargenoshow.Text, String)
                    perchargenoshow(n) = CType(txtperchargenoshow.Text, String)
                    valuenoshow(n) = CType(txtvaluenoshow.Text, String)
                    nightsnoshow(n) = CType(txtnightsnoshow.Text, String)



                    n = n + 1

                Else
                    deletedrow = deletedrow + 1

                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(grdnoshow, False, count)



            Dim i As Integer = n
            n = 0

            For Each GVRow In grdnoshow.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then


                    txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtmealcode = GVRow.FindControl("txtmealcode")
                    ddlnoshow = GVRow.FindControl("ddlnoshow")

                    txtchargenoshow = GVRow.FindControl("txtchargenoshow")
                    txtperchargenoshow = GVRow.FindControl("txtpercnoshow")
                    txtvaluenoshow = GVRow.FindControl("txtvaluenoshow")
                    txtnightsnoshow = GVRow.FindControl("txtnightsnoshow")



                    txtrmtypcode.Text = rmtypcode(n)
                    txtrmtypname.Text = rmtypname(n)
                    txtmealcode.Text = mealcode(n)
                    ddlnoshow.Value = noshow(n)

                    txtchargenoshow.Text = chargenoshow(n)
                    txtperchargenoshow.Text = perchargenoshow(n)
                    txtvaluenoshow.Text = valuenoshow(n)
                    txtnightsnoshow.Text = nightsnoshow(n)


                    n = n + 1
                End If
            Next

            Enablegrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btncopyrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrow.Click
        Enablegrid()

        CopyClicknoshow = 2
        ' Addlines()
        copylinesnoshow()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdnoshow.Rows.Count '+ 1


            Dim n As Integer = 0


            For Each GVRow In grdnoshow.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim ddlnoshow As HtmlSelect = GVRow.FindControl("ddlnoshow")

                Dim txtchargenoshow As TextBox = GVRow.FindControl("txtchargenoshow")
                Dim txtpercnoshow As TextBox = GVRow.FindControl("txtpercnoshow")
                Dim txtvaluenoshow As TextBox = GVRow.FindControl("txtvaluenoshow")
                Dim txtnightsnoshow As TextBox = GVRow.FindControl("txtnightsnoshow")




                If n > CopyRownoshow And txtrmtypname.Text = "" Then

                    txtrmtypcode.Text = txtrmtypcodenoshownew.Item(CopyRownoshow)
                    txtrmtypname.Text = txtrmtypnamenoshownew.Item(CopyRownoshow)
                    txtmealcode.Text = txtmealcodenoshownew.Item(CopyRownoshow)
                    ddlnoshow.Value = ddlnoshow1new.Item(CopyRownoshow)


                    txtchargenoshow.Text = txtchargenoshownew.Item(CopyRownoshow)
                    txtpercnoshow.Text = txtpercnoshownew.Item(CopyRownoshow)
                    txtvaluenoshow.Text = txtvaluenoshownew.Item(CopyRownoshow)
                    txtnightsnoshow.Text = txtnightsnoshownew.Item(CopyRownoshow)

                    Exit For

                End If
                n = n + 1
            Next

            Dim gridcopyrow As GridViewRow
            gridcopyrow = grdnoshow.Rows(n)
            Dim strRowId As String = gridcopyrow.ClientID
            Dim strGridName As String = grdnoshow.ClientID
            Dim strFoucsColumnIndex As String = "3"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(n, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

           
            CopyClicknoshow = 0
            ClearArraynoshow()
            Enablegrid()
            ' setdynamicvalues()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
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
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try

            'If Session("Calledfrom") = "Offers" Then

            '    strSqlQry = "select h.contractid,h.cancelpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            '    " h.applicableto     from view_contracts_cancelpolicy_header h(nolock),view_contracts_search d(nolock)  where h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"

            'Else
            If Session("Calledfrom") = "Offers" Then

                '        strSqlQry = "select h.contractid,h.cancelpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto     from view_contracts_cancelpolicy_header h(nolock),view_contracts_search d(nolock)  where isnull(d.withdraw,0)=0  and h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "'"

                strSqlQry = "sp_show_copycancel  '' , '" & CType(hdnpartycode.Value, String) & "'"
            Else
                '     strSqlQry = "select h.contractid,h.cancelpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto     from view_contracts_cancelpolicy_header h(nolock),view_contracts_search d(nolock)  where isnull(d.withdraw,0)=0  and h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"

                'changed into procedure Rosalin 2019/10/21
                strSqlQry = "sp_show_copycancel'" & CType(hdncontractid.Value, String) & "' , '" & CType(hdnpartycode.Value, String) & "'"

                '  strSqlQry = "select h.contractid,h.cancelpolicyid plistcode,dbo.fn_get_exhitionnames (h.seasons,h.exhibitions) seasoncode,dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & hdncontractid.Value & "', h.exhibitions,0) fromdate, dbo.fn_get_seasonminmaxdate_includeexhibition(h.seasons,'" & hdncontractid.Value & "', h.exhibitions,1)  todate," & _
                '" h.applicableto    from view_contracts_cancelpolicy_header h(nolock)  where  h.contractid<>'" & hdncontractid.Value & "' and (h.seasons<>''or h.exhibitions<>'')"
                '  '  union all " _
                '  '& " select h.cancelpolicyid tranid,'' promotionid, '' promotionname,'' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate  , convert(varchar(10),max(d.todate),103)  todate," & _
                '  '" h.applicableto    from view_contracts_cancelpolicy_header h(nolock) ,view_contracts_cancelpolicy_dates d  where h.cancelpolicyid=d.cancelpolicyid and  h.contractid<>'" & hdncontractid.Value & "' and (h.seasons=''or h.exhibitions='')  group by h.cancelpolicyid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser " ' order by " & StrSortBy & " " & strsortorder & " "


            End If
        
            '    End If


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
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
            objUtils.WritErrorLog("ContractCancpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()
                    Showdetailsgridoffer(CType(lbltran.Text.Trim, String))
                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(hdnpromotionid.Value, String))

                    lblHeading.Text = "Copy Cancellation Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Cancellation Policy "

                Else
                    wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                    wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
                    ShowRecordcopy(CType(lbltran.Text.Trim, String))
                    Showdetailsgrid(CType(lbltran.Text.Trim, String))
                    wucCountrygroup.sbShowCountry()


                    btnSave.Visible = True
                    txtplistcode.Text = ""
                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy Cancellation Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Cancellation Policy "
                    fillseason()
                End If
                


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCancPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub grdexhibition_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdexhibition.RowDataBound
       
    End Sub

    Protected Sub grdpromodates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdpromodates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
                Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
                'Dim btnImgRmv As ImageButton = CType(e.Row.FindControl("btnImgRmv"), ImageButton)

                If Session("Calledfrom") = "Offers" Then

                    txtFromDate.Attributes.Add("onchange", "setdate();")
                    txtToDate.Attributes.Add("onchange", "checkdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                    txtFromDate.Attributes.Add("onchange", "checkfromdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

              
                End If


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
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

                ViewState("State") = "Copy"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))
                ShowRecord(CType(lblplistcode.Text.Trim, String))

                ShowDatesnew(CType(hdnpromotionid.Value, String))
                lblHeading.Text = "Copy Cancellation Policy - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
                Page.Title = " Cancellation Policy "



                ' Showdetailsgrid(CType(lblplistcode.Text.Trim, String))
                Showdetailsgridoffer(lblplistcode.Text)

                wucCountrygroup.sbSetPageState(lblplistcode.Text.Trim, Nothing, ViewState("State"))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"


                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)







            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gv_SearchResult_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowCreated

    End Sub
End Class

