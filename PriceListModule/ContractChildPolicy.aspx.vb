
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractChildPolicy
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

    Dim blankrow As Integer = 0
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim txtrmtypcodenew As New ArrayList
    Dim txtrmtypnamenew As New ArrayList
    Dim txtmealcodenew As New ArrayList
    Dim txtmaxchildnew As New ArrayList
    Dim txtmaxebnew As New ArrayList
    Dim txtnoofchildnew As New ArrayList
    Dim txtfromagenew As New ArrayList
    Dim txttoagenew As New ArrayList
    Dim txtBabyCotNo As New ArrayList '*** Danny Added 18/03/2018
    Dim txtsharingnew As New ArrayList
    Dim txtEBnew As New ArrayList
    Dim CopyRowlist As New ArrayList
    Dim ddlEbTypenew As New ArrayList



#End Region
#Region "Enum GridCol"
    Enum GridCol

        Tranid = 1
        season = 2
        Fromdate = 3
        Todate = 4
        applicableto = 5
        daysoftheweek = 6
        Edit = 9
        View = 10
        Delete = 11
        Copy = 12

        DateCreated = 13
        UserCreated = 14
        DateModified = 15
        UserModified = 16


    End Enum
#End Region


    '*** Danny 19/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    Private Sub ShowHide()
        Try
            If IsNothing(Session("1002")) Then ''*** Danny 1002 is Excel Document number 
                Session("1002") = String.Empty
            End If
            If Session("1002").ToString() <> "SHOW" Then
                grdRoomrates.Columns(9).Visible = False '*** Danny DISSABLING "No Of Baby Cot" Column
            End If
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx=>ShowHide()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    '*** Danny 19/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = False Then
            Session("GV_HotelData") = Nothing


            ' NumbersHtml(txtminnights)       'accept only number
            '   NumbersHtml(txtfillrate)
            NumbersHtml(txtamt1)
            divcopy1.Style("display") = "none"




        Else
            If Session("GV_HotelData") Is Nothing = False Then

                dt = Session("GV_HotelData")





                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                    If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "No Of Baby Cot" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" And col.ColumnName <> "CLineno" And col.ColumnName <> "EBType" Then  '' Added 25/01/17
                        Dim bfield As New TemplateField
                        'Call Function


                        bfield.HeaderTemplate = New ClassChildPolicy(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClassChildPolicy(ListItemType.Item, col.ColumnName, fld2)
                        grdRoomrates.Columns.Add(bfield)



                    End If
                Next

                grdRoomrates.Visible = True
                grdRoomrates.DataSource = dt
                'InstantiateIn Grid View
                grdRoomrates.DataBind()
            End If
        End If
    End Sub
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()

        'changed by shahul on 17/03/2018
        'If Session("Calledfrom") = "Offers" Then
        '    FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
        'Else
        '    FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")
        'End If
        '' FillGrid(hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        Dim CalledfromValue As String = ""

        Dim ConMaxappid As String = ""
        Dim ConMAxappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        If IsPostBack = False Then
            ConMaxappid = 1
            ConMAxappname = objUser.GetAppName(Session("dbconnectionName"), ConMaxappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConMAxappname, String), "ContractChildPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConMAxappname, String), "ContractChildPolicy.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractChildPolicy.aspx", ConMaxappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConMaxappid, intMenuID)

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
                    btncopycontract.Visible = False
                    If Count = 1 Then
                        btnselect.Visible = True
                        btncopycontract.Visible = True
                    Else
                        btnselect.Visible = False
                        btncopycontract.Visible = False
                    End If
                End If

            Else
                btncopycontract.Visible = False
                btnselect.Visible = False

            End If


            txtconnection.Value = Session("dbconnectionName")

            If Session("Calledfrom") = "Offers" Then
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = hdncontractid.Value 'CType(Request.QueryString("contractid"), String)

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                ViewState("partycode") = hdnpartycode.Value

                btnselect.Style.Add("display", "block")
                divoffer.Style.Add("display", "block")

                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                gv_SearchResult.Columns(4).Visible = False

                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                    txtpromotionid.Text = Session("OfferRefCode")
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

                wucCountrygroup.sbSetPageState("", "OFFERSCHILD", CType(Session("OfferState"), String))

                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdnpromotionid.Value
                Page.Title = "Promotion ChildPolicy "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1

                FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, "Desc")


            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                divoffer.Style.Add("display", "none")

                btnselect.Style.Add("display", "none")
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)

                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True


                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value

                lblHeading.Text = txthotelname.Text + " - " + hdncontractid.Value + " - " + lblHeading.Text
                Page.Title = "Contract ChildPolicy "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1

                FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")

                wucCountrygroup.sbSetPageState("", "CONTRACTCHILD", CType(Session("ContractState"), String))

            End If


            lblbookingvaltype.Visible = False
            ddlBookingValidity.Visible = False





            If ddlRoomfrom.Value <> "[Select]" Then

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomfrom, "rmtypname", "rmtypcode", strSqlQry, True, ddlRoomfrom.Value)
            Else

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomfrom, "rmtypname", "rmtypcode", strSqlQry, True)

            End If

            If ddlroomto.Value <> "[Select]" Then

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlroomto, "rmtypname", "rmtypcode", strSqlQry, True, ddlroomto.Value)
            Else

                strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                         " from partyrmtyp  " & _
                          " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlroomto, "rmtypname", "rmtypcode", strSqlQry, True)

            End If


            If ddlCopymeal.Value <> "[Select]" Then

                strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                         " from partyrmcat prc ,rmcatmast rc " & _
                          " where  prc.rmcatcode=rc.rmcatcode  and rc.accom_extra='L' and prc.partycode='" + hdnpartycode.Value + "' order by isnull(rc.rankorder,999)"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True, ddlCopymeal.Value)


            Else

                strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                         " from partyrmcat prc ,rmcatmast rc " & _
                          " where  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" + hdnpartycode.Value + "' isnull(rc.rankorder,999)"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True)

            End If


            '  PanelMain.Visible = False

            'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


            End If

            Session.Add("1002", objUtils.GetString("strDBConnection", "select option_selected from reservation_parameters where param_id=2")) '*** Danny Added 19/03/2018
        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
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
                    Dim txtmealcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                    txtmealcode.Text = roomtypes.ToString
                End If
            Else
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtrmtypname As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                    Dim txtrmtypcode As TextBox = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                    txtrmtypname.Text = rmtypname.ToString
                    txtrmtypcode.Text = roomtypes.ToString

                    Dim txtnoofchidauto As AjaxControlToolkit.AutoCompleteExtender

                    txtnoofchidauto = grdRoomrates.Rows(hdnMainGridRowid.Value).FindControl("txtnoofchild_AutoCompleteExtender")
                    txtnoofchidauto.ContextKey = txtrmtypcode.Text

                End If
            End If




            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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
                filterCond = "h.promotionid  in (select promotionid from view_offers_countries(nolock) where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or h.promotionid  in (select promotionid from view_offers_agents(nolock) where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "h.promotionid  in (select promotionid from view_offers_agents(nolock) where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            filterCond = ""
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = " select c.childpolicyid plistcode , c.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_childpolicy_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  c.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by c.promotionid,h.approved,h.promotionname,c.childpolicyid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

            'strSqlQry = " select h.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,  " _
            '    & " case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status from view_offers_header h(nolock),view_offers_detail d(nolock) where isnull(h.commissiontype,'')='Special commissionable Rates' and  h.promotionid<>'" & hdnpromotionid.Value & "' and  h.promotionid =d.promotionid   " _
            '    & " and h.partycode='" + hdnpartycode.Value.Trim + "'  " + filterCond + " group by h.promotionid,h.approved  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "
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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillroomgrid(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 7
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
        ModalExtraPopup.Hide()
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    Protected Sub check_changed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox
        Dim lblselect As Label

        Dim objbtn As CheckBox = CType(sender, CheckBox)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        Dim txtseasoncodeCurr As Label = row.FindControl("txtseasoncode")

        For Each grdRow As GridViewRow In grdseason.Rows
            If grdRow.RowIndex <> rowid Then
                chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                lblselect = CType(grdRow.FindControl("lblselect"), Label)
                Dim txtseasoncode As Label = grdRow.FindControl("txtseasoncode")

                If txtseasoncodeCurr.Text.ToUpper.Trim = txtseasoncode.Text.ToUpper.Trim Then
                    chkSelect.Checked = objbtn.Checked
                End If
            End If
        Next
        objbtn.Focus()
    End Sub
    Sub FillRoomdetails()
        '  createdatatable()
        createdatarows()
        grdRoomrates.Visible = True

        lable12.Visible = True
        btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False


        btnfillrate.Visible = True
        txtfillrate.Visible = True
        divcopy1.Style("display") = "block"
    End Sub
    Protected Sub btnClearPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.txtViewPolicy.Value = ""

    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    ' strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
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


    'Private Sub createdatatable()
    '    Try

    '        cnt = 0
    '        Session("GV_HotelData") = Nothing
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
    '        strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
    '                  & hdnpartycode.Value & "' "  ' p.rmcatcode " '
    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        cnt = mySqlCmd.ExecuteScalar
    '        mySqlConn.Close()


    '        ReDim arr(cnt + 1)
    '        ReDim arrRName(cnt + 1)
    '        Dim i As Long = 0

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '        strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
    '                    & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

    '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '        mySqlReader = mySqlCmd.ExecuteReader()
    '        While mySqlReader.Read = True
    '            arr(i) = mySqlReader("rmcatcode")
    '            i = i + 1
    '        End While
    '        mySqlReader.Close()
    '        mySqlConn.Close()
    '        'select rmcatcode from partyrmcat where partycode='3'
    '        'Here in Array store room types
    '        '-------------------------------------
    '        Dim tf As New TemplateField
    '        dt = New DataTable

    '        dt.Columns.Add(New DataColumn("Room_Type", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Room Type Name", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Meal Plan", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Price Pax", GetType(String)))


    '        'create columns of this room types in data table
    '        For i = 0 To cnt - 1
    '            dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
    '        Next
    '        dt.Columns.Add(New DataColumn("Unityesno", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Noofextraperson", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Extra Person Supplement", GetType(String)))
    '        dt.Columns.Add(New DataColumn("No.of.Nights Room Rate", GetType(String)))
    '        dt.Columns.Add(New DataColumn("Min Nights", GetType(String)))

    '        Session("GV_HotelData") = dt


    '        ' If dt.Columns.Count >= 7 Then Exit Sub
    '        'fill controls from previous form
    '        ' Now  Bind Column Dynamically 
    '        Dim fld2 As String = ""
    '        Dim col As DataColumn
    '        For Each col In dt.Columns
    '            If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" And col.ColumnName <> "Extra Person Supplement" And col.ColumnName <> "No.of.Nights Room Rate" And col.ColumnName <> "Min Nights" Then
    '                Dim bfield As New TemplateField
    '                'Call Function
    '                bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
    '                bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
    '                grdRoomrates.Columns.Add(bfield)


    '            End If
    '        Next
    '        grdRoomrates.Visible = True
    '        grdRoomrates.DataSource = dt
    '        'InstantiateIn Grid View
    '        grdRoomrates.DataBind()


    '    Catch ex As Exception
    '        objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    '    End Try
    'End Sub



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


            strSqlQry = "select distinct seasonname  from view_contractseasons(nolock) where contractid='" & maxstate & "' and seasonname like '" & Trim(prefixText) & "%' order by seasonname "


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





    Private Sub FillGrid(ByVal strsortby As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            If Session("Calledfrom") = "Offers" Then

                'gv_SearchResult.Columns(2).Visible = True
                'gv_SearchResult.Columns(3).Visible = True
                'gv_SearchResult.Columns(4).Visible = False
                If strsortby = "frmdate" Or strsortby = "todate" Then


                    strSqlQry = "with ctee as(select h.childpolicyid tranid,h.promotionid,oh.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser," & _
                        " h.moddate ,h.moduser  from view_contracts_childpolicy_header h(nolock), view_offers_header oh (nolock) ,view_contracts_childpolicy_dates d(nolock)  where h.promotionid=oh.promotionid and h.childpolicyid=d.childpolicyid  and   and h.promotionid='" & contractid & _
                    "' group by h.childpolicyid,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee order by convert(datetime," & strsortby & ",103)  " & strsortorder & " "


                Else

                    strSqlQry = "select h.childpolicyid tranid,h.promotionid,oh.promotionname,'' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser," & _
                    " h.moddate ,h.moduser  from view_contracts_childpolicy_header h(nolock), view_offers_header oh (nolock) ,view_contracts_childpolicy_dates d(nolock)  where  h.promotionid=oh.promotionid and h.childpolicyid=d.childpolicyid  and h.promotionid='" & contractid & _
                     "' group by h.childpolicyid,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & "  " & strsortorder & " "


                End If
            Else

                If strsortby = "frmdate" Or strsortby = "todate" Then
                    strSqlQry = " with ctee as( select h.childpolicyid tranid,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') frmdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                             " h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_childpolicy_header h(nolock)  where  h.contractid='" & contractid & "' and h.seasons <>'' union all " _
                             & " select h.childpolicyid tranid,'' seasoncode, convert(varchar(10),min(d.fromdate),103) frmdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                             & "   from view_contracts_childpolicy_header h(nolock),view_contracts_childpolicy_dates d(nolock)  where  h.childpolicyid=d.childpolicyid and     h.contractid='" & contractid & "' and h.seasons ='' group by h.childpolicyid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser ) " _
                             & "  select * from ctee order by  convert(datetime," & strsortby & ",103)  " & strsortorder & ""

                Else

                    ' rosalin Change for view header

                    '      strSqlQry = "select h.childpolicyid tranid,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') frmdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                    '" h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_childpolicy_header h(nolock)  where  h.contractid='" & contractid & "' and h.seasons <>'' union all " _
                    '& " select h.childpolicyid tranid,'' seasoncode, convert(varchar(10),min(d.fromdate),103) frmdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                    '& "   from view_contracts_childpolicy_header h(nolock),view_contracts_childpolicy_dates d(nolock)  where  h.childpolicyid=d.childpolicyid and     h.contractid='" & contractid & "' and h.seasons ='' group by h.childpolicyid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & " " & strsortorder & ""


                    strSqlQry = "exec New_ContractChildPolicy_Header '" & CType(contractid, String) & "'"

                End If
            End If



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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        Dim lnCnt As Integer = 0

        Dim txtchild As TextBox

        Dim ToDt As Date = Nothing
        Dim flg As Boolean = False

        Dim j As Long = 1
        Dim txt As TextBox, txtnights As TextBox
        Dim cnt As Long

        Dim GvRowRMType As GridViewRow
        Dim gvrowfrmla As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Dim m As Long = 0


        Dim tickedornot As Boolean = False
        Dim tickeddates As Boolean = False
        Dim chkSelect As CheckBox
        tickedornot = False
        For Each grdRow In grdseason.Rows
            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)

            If chkSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        For Each grdrow In grdDates.Rows
            Dim txtfromdate As TextBox = grdrow.findcontrol("txtfromdate")
            Dim txttodate As TextBox = grdrow.findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                tickeddates = True
                Exit For
            End If

        Next


        If tickedornot = False And tickeddates = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Season or Enter Dates Manually');", True)
            ValidateSave = False
            Exit Function
        End If




        tickedornot = False
        Dim chkSelect1 As CheckBox
        tickedornot = False
        For Each grdRow In grdWeekDays.Rows
            chkSelect1 = CType(grdRow.FindControl("chkSelect"), CheckBox)

            If chkSelect1.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one weekday');", True)
            ValidateSave = False
            Exit Function
        End If


        Dim seasonname As String = ""
        Dim chk2 As CheckBox
        Dim txtmealcode1 As Label
        Session("seasons") = Nothing
        Dim oldseason As String = ""

        For Each grdRow As GridViewRow In grdseason.Rows
            chk2 = grdRow.FindControl("chkseason")
            txtmealcode1 = grdRow.FindControl("txtseasoncode")

            If chk2.Checked = True And oldseason <> RTrim(LTrim(txtmealcode1.Text)) Then 'added rtrim & ltrim - changed by mohamed on 09/06/2018
                seasonname = seasonname + RTrim(LTrim(txtmealcode1.Text)) + ","
                oldseason = RTrim(LTrim(txtmealcode1.Text))
            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If

        Session("seasons") = seasonname





        ' ----------------- detail grid validation
        Dim chdagestart As Boolean = False

        For Each gvRow In grdRoomrates.Rows
            lnCnt += 1


            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtmaxchild As TextBox = gvRow.FindControl("txtmaxchild")
            Dim txtmaxeb As TextBox = gvRow.FindControl("txtmaxeb")
            Dim txtnoofchild As TextBox = gvRow.FindControl("txtnoofchild")
            Dim txtfromage As TextBox = gvRow.FindControl("txtfromage")
            Dim txttoage As TextBox = gvRow.FindControl("txttoage")
            Dim txt_Baby_CotNo As TextBox = gvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
            Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")
            Dim txtEB As TextBox = gvRow.FindControl("txtEB")

            Dim ddlebtype As HtmlSelect = gvRow.FindControl("ddlEbType")


            If txtrmtypcode.Text <> "" Then
                flg = True
            End If


            If txtrmtypname.Text <> "" Then

                If txtrmtypcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Room Type :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If
                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal Plan :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If

                If txtmaxchild.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Maximum Child Allowed can not be blank :" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtmaxchild)
                    Exit Function

                End If

                'If txtmaxeb.Text = "" Then

                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Maximum Extrabed Allowed can not be blank.:" & lnCnt & "');", True)
                '    ValidateSave = False
                '    SetFocus(txtmaxeb)
                '    Exit Function

                'End If

                If txtnoofchild.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No of Children can not be blank.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtnoofchild)
                    Exit Function

                End If

                If ddlebtype.Value = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Extra Bed Type.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtnoofchild)
                    Exit Function

                End If


                If txtfromage.Text <> "" And txtfromage.Text = 0 Then
                    chdagestart = True
                End If

                If txtfromage.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From age grid can not be blank.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtfromage)
                    Exit Function

                End If

                If txttoage.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To age grid can not be blank.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txttoage)
                    Exit Function

                End If
                If txt_Baby_CotNo.Text = "" Then '*** Danny Added  18/03/2018
                    txt_Baby_CotNo.Text = 0
                End If
                If txtfromage.Text <> "" And txttoage.Text <> "" Then
                    If Val(txtfromage.Text) > Val(txttoage.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To age  grid can not be less than From Age.:" & lnCnt & "');", True)
                        ValidateSave = False
                        SetFocus(txttoage)
                        Exit Function
                    End If
                End If

                If txtsharing.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Child Sharing  grid can not be blank.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtsharing)
                    Exit Function

                End If

                If txtEB.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Child Extrabed  grid can not be blank.:" & lnCnt & "');", True)
                    ValidateSave = False
                    SetFocus(txtsharing)
                    Exit Function

                End If




            End If



        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter  one  row in Child Policy :" & lnCnt & "');", True)
            ValidateSave = False
            Exit Function
        End If

        If chdagestart = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Child  age start from 0 onwards  ' );", True)
            ValidateSave = False
            Exit Function
        End If



        '----------------------------

        Dim contfromdate As Date
        Dim conttodate As Date

        If Session("Calledfrom") <> "Offers" Then
            'Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "Select min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'")
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "Select min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock) Where c.contractid='" & hdncontractid.Value & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                contfromdate = ds.Tables(0).Rows(0).Item("fromdate")
                conttodate = ds.Tables(0).Rows(0).Item("todate")
            End If
        End If





        ''''''''''' Dates Overlapping for Manual Dates

        Dim dtdatesnew As New DataTable
        Dim dsdates As New DataSet
        Dim dr As DataRow
        Dim xmldates As String = ""




        dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
        dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))


        For Each gvRow1 In grdDates.Rows
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
        Dim ds1 As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        parm(0) = New SqlParameter("@datesxml", CType(xmldates, String))
        'parm(1) = New SqlParameter("@contractfromdate", DBNull.Value)
        'parm(2) = New SqlParameter("@contracttodate", DBNull.Value)

        parms.Add(parm(0))

        'For i = 0 To 2
        '    parms.Add(parm(i))
        'Next

        ds1 = New DataSet()
        ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkoverlapdates", parms)

        If ds1.Tables.Count > 0 Then
            If ds1.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds1.Tables(0).Rows(0)("fromdateC")) = False Then
                    strMsg = "Dates Are Overlapping Please check " + "\n"
                    For i = 0 To ds1.Tables(0).Rows.Count - 1

                        strMsg += "  Date -  " + ds1.Tables(0).Rows(i)("fromdateC") + "\n"
                    Next
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    ValidateSave = False
                    Exit Function
                End If
            End If
        End If


        '''''''''''''''''
        '--------------------------------------------- Validate Manual Date Grid belongs to contract From and To date
        Dim flgdt1 As Boolean = False
        For Each gvRow In grdDates.Rows
            'dpFDate = gvRow.FindControl("FrmDate")
            'dpTDate = gvRow.FindControl("ToDate")
            Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
            If grdDates.Rows.Count > 0 And txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                        ValidateSave = False
                        SetFocus(txtFromDate)
                        Exit Function
                    End If
                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(txtToDate)
                        ValidateSave = False
                        Exit Function
                    End If
                    If Session("Calledfrom") = "Offers" Then

                        If Not (Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            txtFromDate.Text = ""
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            txtFromDate.Text = ""
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            txtToDate.Text = ""
                            txtFromDate.Text = ""
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                    Else
                        If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(conttodate) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(conttodate)) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belongs to the Contracts Period');", True)
                            SetFocus(txtToDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < ObjDate.ConvertDateromTextBoxToDatabase(contfromdate) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Contracts Period.');", True)
                            SetFocus(txtFromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                    End If




                    'If ToDt <> Nothing Then
                    '    If ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) <= ToDt Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                    '        SetFocus(txtFromDate)
                    '        ValidateSave = False
                    '        Exit Function
                    '    End If
                    'End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    flgdt1 = True

                ElseIf txtFromDate.Text <> "" And txtToDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                    SetFocus(txtToDate)
                    ValidateSave = False
                    Exit Function
                ElseIf txtFromDate.Text = "" And txtToDate.Text <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                    SetFocus(txtFromDate)
                    ValidateSave = False
                    Exit Function
                End If
            End If
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''






        ValidateSave = True
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('WeekDays Should not be Empty .');", True)
                FindDatePeriod = False
                Exit Function
            End If

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist



            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdseason.Rows

                Dim txtmealcode1 As Label = GVRow.FindControl("txtseasoncode")
                Dim chkseason As CheckBox = GVRow.FindControl("chkseason")

                If chkseason.Checked = True Then

                    For Each GVRow1 In grdRoomrates.Rows

                        Dim ds As DataSet
                        Dim parms2 As New List(Of SqlParameter)
                        Dim parm2(14) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")
                        Dim txtfromage As TextBox = GVRow1.FindControl("txtfromage")
                        Dim txttoage As TextBox = GVRow1.FindControl("txttoage")

                        If txtrmtypcode.Text <> "" Then

                            parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                            parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm2(2) = New SqlParameter("@fromdate", Format(CType(GVRow.Cells(3).Text, Date), "yyyy/MM/dd"))
                            parm2(3) = New SqlParameter("@todate", Format(CType(GVRow.Cells(4).Text, Date), "yyyy/MM/dd"))
                            parm2(4) = New SqlParameter("@subseasoncode", CType(txtmealcode1.Text, String))
                            parm2(5) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm2(6) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm2(7) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm2(8) = New SqlParameter("@promotionid", "")
                            parm2(9) = New SqlParameter("@weekdays", CType(weekdaystr, String))
                            parm2(10) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm2(11) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))
                            parm2(12) = New SqlParameter("@fromage", CType(Val(txtfromage.Text), Decimal))
                            parm2(13) = New SqlParameter("@toage", CType(Val(txttoage.Text), Decimal))
                            'parm2(13) = New SqlParameter("@NoBabyCot", CType(Val(txt_Baby_CotNo.Text), Integer))
                            'Dim txt_Baby_CotNo As TextBox = GVRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                            For i = 0 To 13
                                parms2.Add(parm2(i))
                            Next



                            ds = New DataSet()
                            ' ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkchildpolicy_Test", parms2)
                            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "New_sp_chkchildpolicy", parms2)


                            If ds.Tables.Count > 0 Then
                                If ds.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds.Tables(0).Rows(0)("childpolicyid")) = False Then
                                        strMsg = "Child Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("childpolicyid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
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


            ''''''''''' Manual Dates to check

            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    For Each GVRow1 In grdRoomrates.Rows



                        Dim ds1 As DataSet
                        Dim parms3 As New List(Of SqlParameter)
                        Dim parm3(13) As SqlParameter

                        Dim txtrmtypcode As TextBox = GVRow1.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = GVRow1.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = GVRow1.FindControl("txtmealcode")
                        Dim txtfromage As TextBox = GVRow1.FindControl("txtfromage")
                        Dim txttoage As TextBox = GVRow1.FindControl("txttoage")

                        If txtrmtypcode.Text <> "" Then

                            parm3(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                            parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                            parm3(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                            parm3(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                            parm3(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                            parm3(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                            parm3(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                            parm3(7) = New SqlParameter("@promotionid", IIf(Session("Calledfrom") <> "Offers", "", CType(hdnpromotionid.Value, String)))
                            parm3(8) = New SqlParameter("@weekdays", CType(weekdaystr, String))
                            parm3(9) = New SqlParameter("@rmtypcode", CType(txtrmtypcode.Text, String))
                            parm3(10) = New SqlParameter("@mealcode", CType(txtmealcode.Text, String))
                            parm3(11) = New SqlParameter("@fromage", CType(Val(txtfromage.Text), Decimal))
                            parm3(12) = New SqlParameter("@toage", CType(Val(txttoage.Text), Decimal))

                            For i = 0 To 12
                                parms3.Add(parm3(i))
                            Next



                            ds1 = New DataSet()
                            ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkchildpolicy_manual", parms3)


                            If ds1.Tables.Count > 0 Then
                                If ds1.Tables(0).Rows.Count > 0 Then
                                    If IsDBNull(ds1.Tables(0).Rows(0)("childpolicyid")) = False Then
                                        If Session("Calledfrom") = "Offers" Then
                                            strMsg = "Child Policy already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("childpolicyid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
                                        Else
                                            strMsg = "Child Policy already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("childpolicyid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
                                        End If

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


            '''''''


        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function


    Private Function childmissingage() As String

        Dim txtrmtypcode As TextBox
        Dim txtmealcode As TextBox

        Dim GvSeasonShowSub As GridView
        Dim lblRowId As Label

        Dim dtchildage As New DataTable
        Dim dschild As New DataSet
        Dim dr As DataRow
        Dim xmlchildage As String = ""

        childmissingage = ""

        dtchildage.Columns.Add(New DataColumn("partycode", GetType(String)))
        dtchildage.Columns.Add(New DataColumn("rmtypcode", GetType(String)))
        dtchildage.Columns.Add(New DataColumn("mealcode", GetType(String)))
        dtchildage.Columns.Add(New DataColumn("fromage", GetType(String)))
        dtchildage.Columns.Add(New DataColumn("toage", GetType(String)))



        For Each gvRow As GridViewRow In grdRoomrates.Rows

            txtmealcode = gvRow.FindControl("txtmealcode")
            txtrmtypcode = gvRow.FindControl("txtrmtypcode")
            Dim txtfromage As TextBox = gvRow.FindControl("txtfromage")
            Dim txttoage As TextBox = gvRow.FindControl("txttoage")

            If txtrmtypcode.Text <> "" Then

                dr = dtchildage.NewRow
                dr("partycode") = CType(hdnpartycode.Value, String)
                dr("rmtypcode") = CType(txtrmtypcode.Text, String)
                dr("mealcode") = CType(txtmealcode.Text, String)
                dr("fromage") = CType(txtfromage.Text, String)
                dr("toage") = CType(txttoage.Text, String)
                dtchildage.Rows.Add(dr)
            End If

        Next

        dschild.Clear()
        If dtchildage IsNot Nothing Then
            If dtchildage.Rows.Count > 0 Then
                dschild.Tables.Add(dtchildage)
                xmlchildage = objUtils.GenerateXML(dschild)
            End If
        Else
            xmlchildage = "<NewDataSet />"
        End If

        Dim parammsg As SqlParameter
        Dim ErrMsg As String = ""


        Dim strMsg As String = ""
        Dim ds As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim parm(0) As SqlParameter

        parm(0) = New SqlParameter("@childxml", CType(xmlchildage, String))

        parms.Add(parm(0))

        'For i = 0 To 2
        '    parms.Add(parm(i))
        'Next


        ds = New DataSet()
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_checkmissingchildages", parms)
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("rmtypcode")) = False Then
                    'strMsg = "Missing Child Age's Period  " + "\n"
                    'For i = 0 To ds.Tables(0).Rows.Count - 1

                    '    strMsg += " From Date -  " + ds.Tables(0).Rows(i)("fromdateC") + " - Todate  " + ds.Tables(0).Rows(i)("Todate") + "\n"
                    'Next
                    strMsg = "Room Types - " + ds.Tables(0).Rows(0)("rmtypcode") + " - Meal Plan  " + ds.Tables(0).Rows(0)("mealcode") + " - " + ds.Tables(0).Rows(0)("errmessage")


                    '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                    childmissingage = strMsg
                    Exit Function
                End If
            End If
        End If





        'CheckMissingdates = True
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
                    fnCalculateVATValue() 'changed by mohamed on 18/03/2018
                    Dim Str1 As String = childmissingage()
                    If Str1 <> "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & Str1 & "' );", True)
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

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_child", mySqlConn, sqlTrans)
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
                    '''''''''''''''''''''''


                    If ViewState("State") = "New" Or ViewState("State") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("CHLDPOLICY", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_childpolicy_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = IIf(txtApplicableTo.Text = "", "", CType(Replace(txtApplicableTo.Text, ",  ", ","), String))
                        ' mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = CType(Replace(Session("seasons"), ",  ", ","), String)
                        If Session("Calledfrom") <> "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed


                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        If Session("Calledfrom") <> "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If

                    ' Rosalin 2019-10-29 Handled in the  sp_add_New_edit_ChildSeason 
                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_ChildSeason Where  policy_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_ChildPolicy Where  child_policy_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_childpolicy_detail Where  childpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_childpolicy_weekdays Where  childpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    If Session("Calledfrom") = "Offers" Then

                        mySqlCmd = New SqlCommand("DELETE FROM New_edit_ChildSeason_spl Where  policy_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("DELETE FROM New_edit_ChildPolicy_spl Where  child_policy_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()
                    End If


                    '------------------------------------Inserting Data weekdays
                    Dim GvRow As GridViewRow
                    For Each GvRow In grdWeekDays.Rows
                        Dim lblorder As Label = GvRow.FindControl("lblSrNo")
                        Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")
                        If chkSelect.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_contracts_childpolicy_weekdays", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@dayoftheweek", SqlDbType.VarChar, 30)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekorder", SqlDbType.Int, 4)).Value = CType(lblorder.Text.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next

                    ''''''''''''''''

                    Dim kl As Integer = 1


                    Dim promotiontypes As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select promotiontypes from  view_offers_header(nolock) where  promotionid='" & hdnpromotionid.Value & "'")


                    Dim chk2 As CheckBox
                    Dim txtmealcode1 As Label

                    Dim countrygroup As String = ""
                    Dim agentsGroup As String = ""

                    If Session("CountryList") <> "" Then
                        countrygroup = wucCountrygroup.checkcountrylist.ToString.Trim
                    End If

                    If Session("AgentList") <> "" Then
                        agentsGroup = wucCountrygroup.checkagentlist.ToString.Trim
                    End If


                    ''' Manual Dates
                    ''' 
                    ''Value in hdn variable , so splting to get string correctly

                    ' BY Rosalin 2019-10-29
                    'Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    'For i = 0 To arrcountry.Length - 1
                    'If arrcountry(i) <> "" Then   grdDates



                    For Each GvRow1 As GridViewRow In grdDates.Rows

                        Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                        Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                        ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                        If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                            'mySqlCmd = New SqlCommand("sp_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                            mySqlCmd = New SqlCommand("New_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            If Session("Calledfrom") = "Offers" Then
                                'If promotiontypes.Contains("Special Rates") = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                'Else
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                'End If
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentsGroup, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                            'mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = CType(arrcountry(i), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 8000)).Value = CType(countrygroup, String)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                            kl = kl + 1
                        End If
                    Next



                    For Each grdRow As GridViewRow In grdseason.Rows
                        chk2 = grdRow.FindControl("chkseason")
                        txtmealcode1 = grdRow.FindControl("txtseasoncode")

                        If chk2.Checked = True Then

                            ' mySqlCmd = New SqlCommand("sp_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                            mySqlCmd = New SqlCommand("New_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                            If Session("Calledfrom") = "Offers" Then
                                ' If promotiontypes.Contains("Special Rates") = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                'Else
                                ' mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                'End If
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                            End If


                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentsGroup, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                            ' mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = CType(arrcountry(i), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 8000)).Value = CType(countrygroup, String)


                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If
                    Next

                    'End If country group


                    ' Next
                    ' End If
                    'kl = 1

                    'If Session("AgentList") <> "" Then

                    '    ''Value in hdn variable , so splting to get string correctly

                    '    'rosalin 2019-10-29
                    '    ' Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    ' For i = 0 To arragents.Length - 1

                    '    Dim agentsGroup As String = wucCountrygroup.checkagentlist.ToString.Trim
                    '    If agentsGroup <> "" Then

                    '        '     If arragents(i) <> "" Then


                    '        For Each GvRow1 As GridViewRow In grdDates.Rows

                    '            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                    '            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                    '            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                    '            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    '                mySqlCmd = New SqlCommand("sp_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                    '                mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '                If Session("Calledfrom") = "Offers" Then
                    '                    If promotiontypes.Contains("Special Rates") = True Then
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                    '                    Else
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                    '                    End If
                    '                Else
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                    '                End If
                    '                'mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(i), String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentsGroup, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                    '                'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""

                    '                mySqlCmd.ExecuteNonQuery()
                    '                mySqlCmd.Dispose() 'command disposed
                    '                kl = kl + 1
                    '            End If
                    '        Next


                    '        For Each grdRow As GridViewRow In grdseason.Rows
                    '            chk2 = grdRow.FindControl("chkseason")
                    '            txtmealcode1 = grdRow.FindControl("txtseasoncode")

                    '            If chk2.Checked = True Then

                    '                mySqlCmd = New SqlCommand("sp_add_New_edit_ChildSeason", mySqlConn, sqlTrans)
                    '                mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '                If Session("Calledfrom") = "Offers" Then
                    '                    If promotiontypes.Contains("Special Rates") = True Then
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                    '                    Else
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                    '                    End If
                    '                Else
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                    '                End If
                    '                'mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(i), String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(agentsGroup, String)
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                    '                'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""


                    '                mySqlCmd.ExecuteNonQuery()
                    '                mySqlCmd.Dispose() 'command disposed

                    '            End If
                    '        Next


                    '    End If
                    '    'Next  Agent list 
                    'End If
                    ''''' Rosalin  2019-10-29



                    ' '''' save season manual dates
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_childpolicy_dates Where childpolicyid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'Dim kl As Integer = 1
                    'For Each GvRow1 As GridViewRow In grdDates.Rows

                    '    Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                    '    Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                    '    ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                    '    If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    '        mySqlCmd = New SqlCommand("sp_add_edit_contracts_childpolicy_dates", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure

                    '        mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@dlineno", SqlDbType.Int)).Value = kl
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")



                    '        mySqlCmd.ExecuteNonQuery()
                    '        mySqlCmd.Dispose() 'command disposed
                    '        kl = kl + 1
                    '    End If
                    'Next

                    '''''''''''''''''''''



                    ''''''''''Detail Saving

                    Dim k As Long
                    Dim j As Long = 1
                    Dim txt As TextBox

                    ' Dim GvRow As GridViewRow
                    Dim srno As Long = 0
                    Dim hotelcategory As String = ""
                    j = 0
                    Dim m As Long = 0
                    Dim n As Long = 0
                    Dim cnt As Long = grdRoomrates.Columns.Count
                    Dim a As Long = cnt - 13 '12 '*** Danny Changed 18/03/2018
                    Dim b As Long = 0
                    Dim header As Long = 0
                    Dim heading(cnt + 1) As String
                    Dim flag As Boolean = False


                    For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                        If Not txt Is Nothing Then
                            heading(header) = txt.Text
                        End If
                    Next

                    ' Dim m As Long = 0

                    'Dim GvRow1 As GridViewRow


                    Dim rx As Integer = 0
                    Dim aa As Integer = 1

                    Dim ratesstring As String = ""
                    Dim lsChildcategory_taxdetail As String
                    Dim rates As String = ""

                    For Each GvRow In grdRoomrates.Rows
                        Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                        Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                        Dim txtmaxchild As TextBox = GvRow.FindControl("txtmaxchild")
                        Dim txtmaxeb As TextBox = GvRow.FindControl("txtmaxeb")
                        Dim txtnoofchild As TextBox = GvRow.FindControl("txtnoofchild")
                        Dim txtfromage As TextBox = GvRow.FindControl("txtfromage")
                        Dim txttoage As TextBox = GvRow.FindControl("txttoage")
                        Dim txt_Baby_CotNo As TextBox = GvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                        Dim txtsharing As TextBox = GvRow.FindControl("txtsharing")
                        Dim txtEB As TextBox = GvRow.FindControl("txtEB")
                        Dim ddlEbType As HtmlSelect = GvRow.FindControl("ddlEbType")

                        Dim txtTV1SH As TextBox = Nothing
                        Dim txtNTV1SH As TextBox = Nothing
                        Dim txtVAT1SH As TextBox = Nothing
                        txtTV1SH = GvRow.FindControl("txtTVSH")
                        txtNTV1SH = GvRow.FindControl("txtNTVSH")
                        txtVAT1SH = GvRow.FindControl("txtVATSH")

                        Dim txtTV2EB As TextBox = Nothing
                        Dim txtNTV2EB As TextBox = Nothing
                        Dim txtVAT2EB As TextBox = Nothing
                        txtTV2EB = GvRow.FindControl("txtTVEB")
                        txtNTV2EB = GvRow.FindControl("txtNTVEB")
                        txtVAT2EB = GvRow.FindControl("txtVATEB")

                        Select Case txtnoofchild.Text
                            Case "All"
                                txtnoofchild.Text = 0
                            Case "First Child"
                                txtnoofchild.Text = 1
                            Case "Second Child"
                                txtnoofchild.Text = 2
                            Case "Third Child"
                                txtnoofchild.Text = 3
                            Case "Fourth Child"
                                txtnoofchild.Text = 4
                            Case "Fifth Child"
                                txtnoofchild.Text = 5
                            Case "Sixth Child"
                                txtnoofchild.Text = 6
                            Case "Seventh Child"
                                txtnoofchild.Text = 7
                            Case "Eighth Child"
                                txtnoofchild.Text = 8
                            Case "Nineth Child"
                                txtnoofchild.Text = 9
                            Case "Tenth Child"
                                txtnoofchild.Text = 10

                        End Select
                        ratesstring = ""
                        lsChildcategory_taxdetail = ""
                        If txtrmtypcode.Text <> "" Then
                            If n = 0 Then
                                For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                            If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" Then

                                                rates = txt.Text
                                                Select Case txt.Text
                                                    Case "Free"
                                                        rates = "-3"
                                                    Case "Incl"
                                                        rates = "-1"
                                                    Case "N.Incl"
                                                        rates = "-2"
                                                    Case "N/A"
                                                        rates = "-4"
                                                    Case "On Request"
                                                        rates = "-5"

                                                End Select
                                                ratesstring = ratesstring + ";" + CType(heading(j), String) + "," + CType(CType(rates, Decimal), String)

                                                Dim txtTV3Dyn As TextBox = Nothing
                                                Dim txtNTV3Dyn As TextBox = Nothing
                                                Dim txtVAT3Dyn As TextBox = Nothing
                                                txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                lsChildcategory_taxdetail += CType(heading(j), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                            End If
                                        End If
                                    End If
                                Next
                                If ratesstring.Length > 0 Then
                                    ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                Else
                                    ratesstring = ""
                                End If

                                '' Old Table 
                                mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017

                                Select Case CType(txtsharing.Text, String)
                                    Case "Free"
                                        txtsharing.Text = "-3"
                                    Case "Incl"
                                        txtsharing.Text = "-1"
                                    Case "N.Incl"
                                        txtsharing.Text = "-2"
                                    Case "N/A"
                                        txtsharing.Text = "-4"
                                    Case "On Request"
                                        txtsharing.Text = "-5"
                                End Select
                                Select Case CType(txtEB.Text, String)
                                    Case "Free"
                                        txtEB.Text = "-3"
                                    Case "Incl"
                                        txtEB.Text = "-1"
                                    Case "N.Incl"
                                        txtEB.Text = "-2"
                                    Case "N/A"
                                        txtEB.Text = "-4"
                                    Case "On Request"
                                        txtEB.Text = "-5"
                                End Select



                                mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(Val(txtsharing.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)

                                mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)




                                mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed



                                aa = aa + 1

                                m = j
                            Else
                                k = 0

                                For j = n To (m + n) - 1
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                            If heading(k) <> "RoomTypecode" And heading(k) <> "Select_RoomType" And heading(k) <> "Meal_Plan" And heading(k) <> "MaxChild Allowed" And heading(k) <> "MaxEB Allowed" And heading(k) <> "No.of Children" And heading(k) <> "From Age" And heading(k) <> "To Age" And heading(k) <> "No Of Baby Cot" And heading(k) <> "Charge for Sharing" And heading(k) <> "Charge for EB" And heading(k) <> "EBType" Then

                                                rates = txt.Text
                                                Select Case txt.Text
                                                    Case "Free"
                                                        rates = "-3"
                                                    Case "Incl"
                                                        rates = "-1"
                                                    Case "N.Incl"
                                                        rates = "-2"
                                                    Case "N/A"
                                                        rates = "-4"
                                                    Case "On Request"
                                                        rates = "-5"

                                                End Select
                                                ratesstring = ratesstring + ";" + CType(heading(k), String) + "," + CType(CType(rates, Decimal), String)

                                                Dim txtTV3Dyn As TextBox = Nothing
                                                Dim txtNTV3Dyn As TextBox = Nothing
                                                Dim txtVAT3Dyn As TextBox = Nothing
                                                txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                lsChildcategory_taxdetail += CType(heading(k), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                            End If
                                        End If
                                    End If
                                    k = k + 1
                                Next
                                If ratesstring.Length > 0 Then
                                    ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                Else
                                    ratesstring = ""
                                End If


                                '' Old Table 
                                mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure

                                mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017

                                Select Case CType(txtsharing.Text, String)
                                    Case "Free"
                                        txtsharing.Text = "-3"
                                    Case "Incl"
                                        txtsharing.Text = "-1"
                                    Case "N.Incl"
                                        txtsharing.Text = "-2"
                                    Case "N/A"
                                        txtsharing.Text = "-4"
                                    Case "On Request"
                                        txtsharing.Text = "-5"
                                End Select

                                Select Case CType(txtEB.Text, String)
                                    Case "Free"
                                        txtEB.Text = "-3"
                                    Case "Incl"
                                        txtEB.Text = "-1"
                                    Case "N.Incl"
                                        txtEB.Text = "-2"
                                    Case "N/A"
                                        txtEB.Text = "-4"
                                    Case "On Request"
                                        txtEB.Text = "-5"
                                End Select

                                mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)

                                mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)



                                mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed


                                aa = aa + 1
                            End If
                            b = j
                            n = j
                        End If
                    Next


                    j = 0
                    m = 0
                    n = 0
                    a = cnt - 13
                    b = 0

                    rx = 0
                    aa = 1

                    If Session("Calledfrom") = "Offers" Then



                        For Each GvRow In grdRoomrates.Rows
                            Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                            Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                            Dim txtmaxchild As TextBox = GvRow.FindControl("txtmaxchild")
                            Dim txtmaxeb As TextBox = GvRow.FindControl("txtmaxeb")
                            Dim txtnoofchild As TextBox = GvRow.FindControl("txtnoofchild")
                            Dim txtfromage As TextBox = GvRow.FindControl("txtfromage")
                            Dim txttoage As TextBox = GvRow.FindControl("txttoage")
                            Dim txt_Baby_CotNo As TextBox = GvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                            Dim txtsharing As TextBox = GvRow.FindControl("txtsharing")
                            Dim txtEB As TextBox = GvRow.FindControl("txtEB")
                            Dim ddlEbType As HtmlSelect = GvRow.FindControl("ddlEbType")

                            Dim txtTV1SH As TextBox = Nothing
                            Dim txtNTV1SH As TextBox = Nothing
                            Dim txtVAT1SH As TextBox = Nothing
                            txtTV1SH = GvRow.FindControl("txtTVSH")
                            txtNTV1SH = GvRow.FindControl("txtNTVSH")
                            txtVAT1SH = GvRow.FindControl("txtVATSH")

                            Dim txtTV2EB As TextBox = Nothing
                            Dim txtNTV2EB As TextBox = Nothing
                            Dim txtVAT2EB As TextBox = Nothing
                            txtTV2EB = GvRow.FindControl("txtTVEB")
                            txtNTV2EB = GvRow.FindControl("txtNTVEB")
                            txtVAT2EB = GvRow.FindControl("txtVATEB")

                            Select Case txtnoofchild.Text
                                Case "All"
                                    txtnoofchild.Text = 0
                                Case "First Child"
                                    txtnoofchild.Text = 1
                                Case "Second Child"
                                    txtnoofchild.Text = 2
                                Case "Third Child"
                                    txtnoofchild.Text = 3
                                Case "Fourth Child"
                                    txtnoofchild.Text = 4
                                Case "Fifth Child"
                                    txtnoofchild.Text = 5
                                Case "Sixth Child"
                                    txtnoofchild.Text = 6
                                Case "Seventh Child"
                                    txtnoofchild.Text = 7
                                Case "Eighth Child"
                                    txtnoofchild.Text = 8
                                Case "Nineth Child"
                                    txtnoofchild.Text = 9
                                Case "Tenth Child"
                                    txtnoofchild.Text = 10

                            End Select
                            ratesstring = ""
                            lsChildcategory_taxdetail = ""
                            If txtrmtypcode.Text <> "" Then
                                If n = 0 Then
                                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                        txt = GvRow.FindControl("txt" & j)
                                        If txt Is Nothing Then
                                        Else
                                            If txt.Text <> Nothing Then
                                                '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" Then

                                                    rates = txt.Text
                                                    Select Case txt.Text
                                                        Case "Free"
                                                            rates = "-3"
                                                        Case "Incl"
                                                            rates = "-1"
                                                        Case "N.Incl"
                                                            rates = "-2"
                                                        Case "N/A"
                                                            rates = "-4"
                                                        Case "On Request"
                                                            rates = "-5"

                                                    End Select
                                                    ratesstring = ratesstring + ";" + CType(heading(j), String) + "," + CType(CType(rates, Decimal), String)

                                                    Dim txtTV3Dyn As TextBox = Nothing
                                                    Dim txtNTV3Dyn As TextBox = Nothing
                                                    Dim txtVAT3Dyn As TextBox = Nothing
                                                    txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                    txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                    txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                    lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                    lsChildcategory_taxdetail += CType(heading(j), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                    lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                    lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                End If
                                            End If
                                        End If
                                    Next
                                    If ratesstring.Length > 0 Then
                                        ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                    Else
                                        ratesstring = ""
                                    End If



                                    ''' New Table saving   1st   N= 0 if 
                                    mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                                    mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                    Select Case CType(txtsharing.Text, String)
                                        Case "Free"
                                            txtsharing.Text = "-3"
                                        Case "Incl"
                                            txtsharing.Text = "-1"
                                        Case "N.Incl"
                                            txtsharing.Text = "-2"
                                        Case "N/A"
                                            txtsharing.Text = "-4"
                                        Case "On Request"
                                            txtsharing.Text = "-5"
                                    End Select

                                    mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                    Select Case CType(txtEB.Text, String)
                                        Case "Free"
                                            txtEB.Text = "-3"
                                        Case "Incl"
                                            txtEB.Text = "-1"
                                        Case "N.Incl"
                                            txtEB.Text = "-2"
                                        Case "N/A"
                                            txtEB.Text = "-4"
                                        Case "On Request"
                                            txtEB.Text = "-5"
                                    End Select
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                    mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)
                                    If Session("Calledfrom") = "Offers" Then
                                        ' If promotiontypes.Contains("Special Rates") = True Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                        'Else
                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                        'End If
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = DBNull.Value

                                    mySqlCmd.ExecuteNonQuery()
                                    mySqlCmd.Dispose() 'command disposed
                                    aa = aa + 1

                                    m = j
                                Else  ' N= 0 if condition
                                    k = 0

                                    For j = n To (m + n) - 1
                                        txt = GvRow.FindControl("txt" & j)
                                        If txt Is Nothing Then
                                        Else
                                            If txt.Text <> Nothing Then
                                                '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                If heading(k) <> "RoomTypecode" And heading(k) <> "Select_RoomType" And heading(k) <> "Meal_Plan" And heading(k) <> "MaxChild Allowed" And heading(k) <> "MaxEB Allowed" And heading(k) <> "No.of Children" And heading(k) <> "From Age" And heading(k) <> "To Age" And heading(k) <> "No Of Baby Cot" And heading(k) <> "Charge for Sharing" And heading(k) <> "Charge for EB" And heading(k) <> "EBType" Then

                                                    rates = txt.Text
                                                    Select Case txt.Text
                                                        Case "Free"
                                                            rates = "-3"
                                                        Case "Incl"
                                                            rates = "-1"
                                                        Case "N.Incl"
                                                            rates = "-2"
                                                        Case "N/A"
                                                            rates = "-4"
                                                        Case "On Request"
                                                            rates = "-5"

                                                    End Select
                                                    ratesstring = ratesstring + ";" + CType(heading(k), String) + "," + CType(CType(rates, Decimal), String)

                                                    Dim txtTV3Dyn As TextBox = Nothing
                                                    Dim txtNTV3Dyn As TextBox = Nothing
                                                    Dim txtVAT3Dyn As TextBox = Nothing
                                                    txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                    txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                    txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                    lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                    lsChildcategory_taxdetail += CType(heading(k), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                    lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                    lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                End If
                                            End If
                                        End If
                                        k = k + 1
                                    Next
                                    If ratesstring.Length > 0 Then
                                        ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                    Else
                                        ratesstring = ""
                                    End If




                                    '' New Table saving  1st  N= 0  if else
                                    mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                    mySqlCmd.CommandType = CommandType.StoredProcedure

                                    mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                    'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                    Select Case CType(txtsharing.Text, String)
                                        Case "Free"
                                            txtsharing.Text = "-3"
                                        Case "Incl"
                                            txtsharing.Text = "-1"
                                        Case "N.Incl"
                                            txtsharing.Text = "-2"
                                        Case "N/A"
                                            txtsharing.Text = "-4"
                                        Case "On Request"
                                            txtsharing.Text = "-5"
                                    End Select

                                    mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                    Select Case CType(txtEB.Text, String)
                                        Case "Free"
                                            txtEB.Text = "-3"
                                        Case "Incl"
                                            txtEB.Text = "-1"
                                        Case "N.Incl"
                                            txtEB.Text = "-2"
                                        Case "N/A"
                                            txtEB.Text = "-4"
                                        Case "On Request"
                                            txtEB.Text = "-5"
                                    End Select
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                    mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                    mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                                    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)

                                    If Session("Calledfrom") = "Offers" Then
                                        ' If promotiontypes.Contains("Special Rates") = True Then
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                        'Else
                                        '    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                        'End If
                                    Else
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                    End If
                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = DBNull.Value


                                    mySqlCmd.ExecuteNonQuery()
                                    mySqlCmd.Dispose() 'command disposed
                                    aa = aa + 1
                                End If
                                b = j
                                n = j
                            End If
                        Next

                    Else  ' Offer form Else
                        Dim tseasonname As String = ""
                        For Each grdRow As GridViewRow In grdseason.Rows  ''' Season  records checking - Grid 
                            chk2 = grdRow.FindControl("chkseason")
                            txtmealcode1 = grdRow.FindControl("txtseasoncode")

                            j = 0
                            m = 0
                            n = 0
                            a = cnt - 13
                            b = 0

                            rx = 0
                            aa = 1

                            If chk2.Checked = True And (tseasonname = "" Or tseasonname <> txtmealcode1.Text) Then  ' Season 

                                For Each GvRow In grdRoomrates.Rows  ''' Season Room Rate
                                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                                    Dim txtmaxchild As TextBox = GvRow.FindControl("txtmaxchild")
                                    Dim txtmaxeb As TextBox = GvRow.FindControl("txtmaxeb")
                                    Dim txtnoofchild As TextBox = GvRow.FindControl("txtnoofchild")
                                    Dim txtfromage As TextBox = GvRow.FindControl("txtfromage")
                                    Dim txttoage As TextBox = GvRow.FindControl("txttoage")
                                    Dim txt_Baby_CotNo As TextBox = GvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                                    Dim txtsharing As TextBox = GvRow.FindControl("txtsharing")
                                    Dim txtEB As TextBox = GvRow.FindControl("txtEB")
                                    Dim ddlEbType As HtmlSelect = GvRow.FindControl("ddlEbType")

                                    Dim txtTV1SH As TextBox = Nothing
                                    Dim txtNTV1SH As TextBox = Nothing
                                    Dim txtVAT1SH As TextBox = Nothing
                                    txtTV1SH = GvRow.FindControl("txtTVSH")
                                    txtNTV1SH = GvRow.FindControl("txtNTVSH")
                                    txtVAT1SH = GvRow.FindControl("txtVATSH")

                                    Dim txtTV2EB As TextBox = Nothing
                                    Dim txtNTV2EB As TextBox = Nothing
                                    Dim txtVAT2EB As TextBox = Nothing
                                    txtTV2EB = GvRow.FindControl("txtTVEB")
                                    txtNTV2EB = GvRow.FindControl("txtNTVEB")
                                    txtVAT2EB = GvRow.FindControl("txtVATEB")

                                    Select Case txtnoofchild.Text
                                        Case "All"
                                            txtnoofchild.Text = 0
                                        Case "First Child"
                                            txtnoofchild.Text = 1
                                        Case "Second Child"
                                            txtnoofchild.Text = 2
                                        Case "Third Child"
                                            txtnoofchild.Text = 3
                                        Case "Fourth Child"
                                            txtnoofchild.Text = 4
                                        Case "Fifth Child"
                                            txtnoofchild.Text = 5
                                        Case "Sixth Child"
                                            txtnoofchild.Text = 6
                                        Case "Seventh Child"
                                            txtnoofchild.Text = 7
                                        Case "Eighth Child"
                                            txtnoofchild.Text = 8
                                        Case "Nineth Child"
                                            txtnoofchild.Text = 9
                                        Case "Tenth Child"
                                            txtnoofchild.Text = 10

                                    End Select
                                    ratesstring = ""
                                    lsChildcategory_taxdetail = ""
                                    If txtrmtypcode.Text <> "" Then
                                        If n = 0 Then
                                            For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                                txt = GvRow.FindControl("txt" & j)
                                                If txt Is Nothing Then
                                                Else
                                                    If txt.Text <> Nothing Then
                                                        '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                        If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" Then

                                                            rates = txt.Text
                                                            Select Case txt.Text
                                                                Case "Free"
                                                                    rates = "-3"
                                                                Case "Incl"
                                                                    rates = "-1"
                                                                Case "N.Incl"
                                                                    rates = "-2"
                                                                Case "N/A"
                                                                    rates = "-4"
                                                                Case "On Request"
                                                                    rates = "-5"

                                                            End Select
                                                            ratesstring = ratesstring + ";" + CType(heading(j), String) + "," + CType(CType(rates, Decimal), String)

                                                            Dim txtTV3Dyn As TextBox = Nothing
                                                            Dim txtNTV3Dyn As TextBox = Nothing
                                                            Dim txtVAT3Dyn As TextBox = Nothing
                                                            txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                            txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                            txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                            lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                            lsChildcategory_taxdetail += CType(heading(j), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                        End If
                                                    End If
                                                End If
                                            Next
                                            If ratesstring.Length > 0 Then
                                                ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                            Else
                                                ratesstring = ""
                                            End If

                                            ''' New Table saving ' contract Season  n = 0 if
                                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                            Select Case CType(txtsharing.Text, String)
                                                Case "Free"
                                                    txtsharing.Text = "-3"
                                                Case "Incl"
                                                    txtsharing.Text = "-1"
                                                Case "N.Incl"
                                                    txtsharing.Text = "-2"
                                                Case "N/A"
                                                    txtsharing.Text = "-4"
                                                Case "On Request"
                                                    txtsharing.Text = "-5"
                                            End Select

                                            mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                            Select Case CType(txtEB.Text, String)
                                                Case "Free"
                                                    txtEB.Text = "-3"
                                                Case "Incl"
                                                    txtEB.Text = "-1"
                                                Case "N.Incl"
                                                    txtEB.Text = "-2"
                                                Case "N/A"
                                                    txtEB.Text = "-4"
                                                Case "On Request"
                                                    txtEB.Text = "-5"
                                            End Select
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                            mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)
                                            If Session("Calledfrom") = "Offers" Then
                                                'If promotiontypes.Contains("Special Rates") = True Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                                'Else
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                                'End If
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(hdnpartycode.Value, String) + "_" + CType(txtmealcode1.Text.Trim, String)

                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                            aa = aa + 1

                                            m = j
                                        Else  ''' N= 0 
                                            k = 0

                                            For j = n To (m + n) - 1
                                                txt = GvRow.FindControl("txt" & j)
                                                If txt Is Nothing Then
                                                Else
                                                    If txt.Text <> Nothing Then
                                                        '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                        If heading(k) <> "RoomTypecode" And heading(k) <> "Select_RoomType" And heading(k) <> "Meal_Plan" And heading(k) <> "MaxChild Allowed" And heading(k) <> "MaxEB Allowed" And heading(k) <> "No.of Children" And heading(k) <> "From Age" And heading(k) <> "To Age" And heading(k) <> "No Of Baby Cot" And heading(k) <> "Charge for Sharing" And heading(k) <> "Charge for EB" And heading(k) <> "EBType" Then

                                                            rates = txt.Text
                                                            Select Case txt.Text
                                                                Case "Free"
                                                                    rates = "-3"
                                                                Case "Incl"
                                                                    rates = "-1"
                                                                Case "N.Incl"
                                                                    rates = "-2"
                                                                Case "N/A"
                                                                    rates = "-4"
                                                                Case "On Request"
                                                                    rates = "-5"

                                                            End Select
                                                            ratesstring = ratesstring + ";" + CType(heading(k), String) + "," + CType(CType(rates, Decimal), String)

                                                            Dim txtTV3Dyn As TextBox = Nothing
                                                            Dim txtNTV3Dyn As TextBox = Nothing
                                                            Dim txtVAT3Dyn As TextBox = Nothing
                                                            txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                            txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                            txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                            lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                            lsChildcategory_taxdetail += CType(heading(k), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                        End If
                                                    End If
                                                End If
                                                k = k + 1
                                            Next
                                            If ratesstring.Length > 0 Then
                                                ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                            Else
                                                ratesstring = ""
                                            End If



                                            '' New Table saving   ' contract Season  n = 0 if ELse
                                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                            Select Case CType(txtsharing.Text, String)
                                                Case "Free"
                                                    txtsharing.Text = "-3"
                                                Case "Incl"
                                                    txtsharing.Text = "-1"
                                                Case "N.Incl"
                                                    txtsharing.Text = "-2"
                                                Case "N/A"
                                                    txtsharing.Text = "-4"
                                                Case "On Request"
                                                    txtsharing.Text = "-5"
                                            End Select

                                            mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                            Select Case CType(txtEB.Text, String)
                                                Case "Free"
                                                    txtEB.Text = "-3"
                                                Case "Incl"
                                                    txtEB.Text = "-1"
                                                Case "N.Incl"
                                                    txtEB.Text = "-2"
                                                Case "N/A"
                                                    txtEB.Text = "-4"
                                                Case "On Request"
                                                    txtEB.Text = "-5"
                                            End Select
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                            mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)

                                            If Session("Calledfrom") = "Offers" Then
                                                ' If promotiontypes.Contains("Special Rates") = True Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                                'Else
                                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                                'End If
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(hdnpartycode.Value, String) + "_" + CType(txtmealcode1.Text.Trim, String)


                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                            aa = aa + 1
                                        End If
                                        b = j
                                        n = j
                                    End If
                                Next  ' Season Room Rate




                            End If
                            tseasonname = txtmealcode1.Text
                        Next ' Season  records checking - Grid 



                        kl = 1
                        For Each GvRow1 As GridViewRow In grdDates.Rows  '' Date Grid ' Contract Manual Dates

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then


                                For Each GvRow In grdRoomrates.Rows
                                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                                    Dim txtmaxchild As TextBox = GvRow.FindControl("txtmaxchild")
                                    Dim txtmaxeb As TextBox = GvRow.FindControl("txtmaxeb")
                                    Dim txtnoofchild As TextBox = GvRow.FindControl("txtnoofchild")
                                    Dim txtfromage As TextBox = GvRow.FindControl("txtfromage")
                                    Dim txttoage As TextBox = GvRow.FindControl("txttoage")
                                    Dim txt_Baby_CotNo As TextBox = GvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                                    Dim txtsharing As TextBox = GvRow.FindControl("txtsharing")
                                    Dim txtEB As TextBox = GvRow.FindControl("txtEB")
                                    Dim ddlEbType As HtmlSelect = GvRow.FindControl("ddlEbType")

                                    Dim txtTV1SH As TextBox = Nothing
                                    Dim txtNTV1SH As TextBox = Nothing
                                    Dim txtVAT1SH As TextBox = Nothing
                                    txtTV1SH = GvRow.FindControl("txtTVSH")
                                    txtNTV1SH = GvRow.FindControl("txtNTVSH")
                                    txtVAT1SH = GvRow.FindControl("txtVATSH")

                                    Dim txtTV2EB As TextBox = Nothing
                                    Dim txtNTV2EB As TextBox = Nothing
                                    Dim txtVAT2EB As TextBox = Nothing
                                    txtTV2EB = GvRow.FindControl("txtTVEB")
                                    txtNTV2EB = GvRow.FindControl("txtNTVEB")
                                    txtVAT2EB = GvRow.FindControl("txtVATEB")

                                    Select Case txtnoofchild.Text
                                        Case "All"
                                            txtnoofchild.Text = 0
                                        Case "First Child"
                                            txtnoofchild.Text = 1
                                        Case "Second Child"
                                            txtnoofchild.Text = 2
                                        Case "Third Child"
                                            txtnoofchild.Text = 3
                                        Case "Fourth Child"
                                            txtnoofchild.Text = 4
                                        Case "Fifth Child"
                                            txtnoofchild.Text = 5
                                        Case "Sixth Child"
                                            txtnoofchild.Text = 6
                                        Case "Seventh Child"
                                            txtnoofchild.Text = 7
                                        Case "Eighth Child"
                                            txtnoofchild.Text = 8
                                        Case "Nineth Child"
                                            txtnoofchild.Text = 9
                                        Case "Tenth Child"
                                            txtnoofchild.Text = 10

                                    End Select
                                    ratesstring = ""
                                    lsChildcategory_taxdetail = ""
                                    If txtrmtypcode.Text <> "" Then
                                        If n = 0 Then
                                            For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                                txt = GvRow.FindControl("txt" & j)
                                                If txt Is Nothing Then
                                                Else
                                                    If txt.Text <> Nothing Then
                                                        '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                        If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" Then

                                                            rates = txt.Text
                                                            Select Case txt.Text
                                                                Case "Free"
                                                                    rates = "-3"
                                                                Case "Incl"
                                                                    rates = "-1"
                                                                Case "N.Incl"
                                                                    rates = "-2"
                                                                Case "N/A"
                                                                    rates = "-4"
                                                                Case "On Request"
                                                                    rates = "-5"

                                                            End Select
                                                            ratesstring = ratesstring + ";" + CType(heading(j), String) + "," + CType(CType(rates, Decimal), String)

                                                            Dim txtTV3Dyn As TextBox = Nothing
                                                            Dim txtNTV3Dyn As TextBox = Nothing
                                                            Dim txtVAT3Dyn As TextBox = Nothing
                                                            txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                            txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                            txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                            lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                            lsChildcategory_taxdetail += CType(heading(j), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                        End If
                                                    End If
                                                End If
                                            Next
                                            If ratesstring.Length > 0 Then
                                                ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                            Else
                                                ratesstring = ""
                                            End If



                                            ''' New Table saving    ''' Date Grid n = 0 If 
                                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                            Select Case CType(txtsharing.Text, String)
                                                Case "Free"
                                                    txtsharing.Text = "-3"
                                                Case "Incl"
                                                    txtsharing.Text = "-1"
                                                Case "N.Incl"
                                                    txtsharing.Text = "-2"
                                                Case "N/A"
                                                    txtsharing.Text = "-4"
                                                Case "On Request"
                                                    txtsharing.Text = "-5"
                                            End Select

                                            mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                            Select Case CType(txtEB.Text, String)
                                                Case "Free"
                                                    txtEB.Text = "-3"
                                                Case "Incl"
                                                    txtEB.Text = "-1"
                                                Case "N.Incl"
                                                    txtEB.Text = "-2"
                                                Case "N/A"
                                                    txtEB.Text = "-4"
                                                Case "On Request"
                                                    txtEB.Text = "-5"
                                            End Select
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                            mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)
                                            If Session("Calledfrom") = "Offers" Then
                                                ' If promotiontypes.Contains("Special Rates") = True Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                                ' Else
                                                '     mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                                ' End If
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(hdnpartycode.Value, String) + "_" + CType("Manual" + CStr(kl), String)

                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                            aa = aa + 1

                                            m = j
                                        Else  ' N= 0
                                            k = 0

                                            For j = n To (m + n) - 1
                                                txt = GvRow.FindControl("txt" & j)
                                                If txt Is Nothing Then
                                                Else
                                                    If txt.Text <> Nothing Then
                                                        '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                                        If heading(k) <> "RoomTypecode" And heading(k) <> "Select_RoomType" And heading(k) <> "Meal_Plan" And heading(k) <> "MaxChild Allowed" And heading(k) <> "MaxEB Allowed" And heading(k) <> "No.of Children" And heading(k) <> "From Age" And heading(k) <> "To Age" And heading(k) <> "No Of Baby Cot" And heading(k) <> "Charge for Sharing" And heading(k) <> "Charge for EB" And heading(k) <> "EBType" Then

                                                            rates = txt.Text
                                                            Select Case txt.Text
                                                                Case "Free"
                                                                    rates = "-3"
                                                                Case "Incl"
                                                                    rates = "-1"
                                                                Case "N.Incl"
                                                                    rates = "-2"
                                                                Case "N/A"
                                                                    rates = "-4"
                                                                Case "On Request"
                                                                    rates = "-5"

                                                            End Select
                                                            ratesstring = ratesstring + ";" + CType(heading(k), String) + "," + CType(CType(rates, Decimal), String)

                                                            Dim txtTV3Dyn As TextBox = Nothing
                                                            Dim txtNTV3Dyn As TextBox = Nothing
                                                            Dim txtVAT3Dyn As TextBox = Nothing
                                                            txtTV3Dyn = GvRow.FindControl("txtTV" & j)
                                                            txtNTV3Dyn = GvRow.FindControl("txtNTV" & j)
                                                            txtVAT3Dyn = GvRow.FindControl("txtVAT" & j)
                                                            lsChildcategory_taxdetail += IIf(lsChildcategory_taxdetail = "", "", ";")
                                                            lsChildcategory_taxdetail += CType(heading(k), String) & "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtNTV3Dyn.Text)), Decimal), String)
                                                            lsChildcategory_taxdetail += "," & CType(CType(IIf(Val(rates) <= 0, 0, Val(txtVAT3Dyn.Text)), Decimal), String)
                                                        End If
                                                    End If
                                                End If
                                                k = k + 1
                                            Next
                                            If ratesstring.Length > 0 Then
                                                ratesstring = Right(ratesstring, Len(ratesstring) - 1)
                                            Else
                                                ratesstring = ""
                                            End If




                                            '' New Table saving  ' Date Grid n = 0 If else
                                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_childpolicy_detail_new", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                            'mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = aa
                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@fromage", SqlDbType.Decimal)).Value = CType(txtfromage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@toage", SqlDbType.Decimal)).Value = CType(txttoage.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchildallowed", SqlDbType.Int)).Value = CType(Val(txtmaxchild.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@maxeballowed", SqlDbType.Int)).Value = CType(Val(txtmaxeb.Text), Integer)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@noofchildren", SqlDbType.Int)).Value = CType(Val(txtnoofchild.Text), Integer)

                                            Select Case CType(txtsharing.Text, String)
                                                Case "Free"
                                                    txtsharing.Text = "-3"
                                                Case "Incl"
                                                    txtsharing.Text = "-1"
                                                Case "N.Incl"
                                                    txtsharing.Text = "-2"
                                                Case "N/A"
                                                    txtsharing.Text = "-4"
                                                Case "On Request"
                                                    txtsharing.Text = "-5"
                                            End Select

                                            mySqlCmd.Parameters.Add(New SqlParameter("@sharingcharge", SqlDbType.Decimal)).Value = CType(txtsharing.Text, Decimal)

                                            Select Case CType(txtEB.Text, String)
                                                Case "Free"
                                                    txtEB.Text = "-3"
                                                Case "Incl"
                                                    txtEB.Text = "-1"
                                                Case "N.Incl"
                                                    txtEB.Text = "-2"
                                                Case "N/A"
                                                    txtEB.Text = "-4"
                                                Case "On Request"
                                                    txtEB.Text = "-5"
                                            End Select
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebcharge", SqlDbType.Decimal)).Value = CType(txtEB.Text, Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@childcategories", SqlDbType.VarChar, 8000)).Value = CType(ratesstring.ToString, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ebtype", SqlDbType.VarChar, 20)).Value = CType(ddlEbType.Value, String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@SHVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1SH.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBNonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@EBVATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT2EB.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ChildCategories_TaxDetail", SqlDbType.VarChar, 8000)).Value = lsChildcategory_taxdetail
                                            mySqlCmd.Parameters.Add(New SqlParameter("@NoBabyCot", SqlDbType.Int)).Value = CType(Val(txt_Baby_CotNo.Text), Integer) '*** Danny Added 21/03/2017



                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                            mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 5000)).Value = CType(txtrmtypcode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 5000)).Value = CType(txtmealcode.Text.Trim, String)

                                            If Session("Calledfrom") = "Offers" Then
                                                ' If promotiontypes.Contains("Special Rates") = True Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 1
                                                'Else
                                                '   mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                                'End If
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@promotiontype", SqlDbType.Int)).Value = 0
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(hdnpartycode.Value, String) + "_" + CType("Manual" + CStr(kl), String)


                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                            aa = aa + 1
                                        End If
                                        b = j
                                        n = j
                                    End If
                                Next








                            End If

                            kl = kl + 1
                        Next ' Contract Manual Dates
                    End If



                    strMsg = "Saved Succesfully!!"

                ElseIf ViewState("State") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    ' '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_child", mySqlConn, sqlTrans)
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
                    mySqlCmd = New SqlCommand("sp_del_contracts_childpolicy_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@childpolicyid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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
                FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, "Desc")

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)

                ModalPopupDays.Hide()  '' Added shahul 08/08/18
            End If


        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ModalPopupDays.Hide()  '' Added shahul 08/08/18
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

    Private Sub ShowDynamicGridnew()

        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String
        Dim cnt1 As Long = 0

        StrQry = "select count( distinct clineno) from view_contracts_childpolicy_detail where childpolicyid='" & txtplistcode.Text & "'"

        cnt1 = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), StrQry)
        fillDategrd(grdRoomrates, False, cnt1)
        createdatacolumns()

        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long = 0
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next
        Dim m As Long = 0
        Dim rmcatcode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim rmtypecode As String = ""
        Dim mealcode As String = ""


        Try


            Dim strQry1 As String = ""



            'strQry1 = "select count( distinct clineno) from view_contracts_childpolicy_detail where childpolicyid='" & txtplistcode.Text & "'"

            'cnt1 = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry1)

            '' If cnt1 < 7 Then cnt1 = 7
            'fillDategrd(grdRoomrates, False, cnt1)
            strQry1 = ""
            strQry1 = "select * from view_contracts_childpolicy_detail d(nolock) where childpolicyid='" & txtplistcode.Text & "'"


            Dim ac As Integer = 1

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strQry1, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        Dim txtrmtypcode As TextBox = gvrow.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = gvrow.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = gvrow.FindControl("txtmealcode")
                        Dim txtmaxchild As TextBox = gvrow.FindControl("txtmaxchild")
                        Dim txtmaxeb As TextBox = gvrow.FindControl("txtmaxeb")
                        Dim txtnoofchild As TextBox = gvrow.FindControl("txtnoofchild")
                        Dim txtfromage As TextBox = gvrow.FindControl("txtfromage")
                        Dim txttoage As TextBox = gvrow.FindControl("txttoage")
                        Dim txtsharing As TextBox = gvrow.FindControl("txtsharing")
                        Dim txtEB As TextBox = gvrow.FindControl("txtEB")
                        Dim ddlEbType As HtmlSelect = gvrow.FindControl("ddlEbType")



                        If txtrmtypcode Is Nothing = False Then


                            'If IsDBNull(mySqlReader("roomtypes")) = False Then
                            '    txtrmtypcode.Text = mySqlReader("roomtypes")

                            '    Dim strMealPlans As String = ""
                            '    Dim strCondition As String = ""
                            '    strMealPlans = mySqlReader("roomtypes")
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
                            '    StrQry = ""
                            '    StrQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock)  where u.rmtypcode =rmtypcode and u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'')  rmtypname " _
                            '        & " from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") "

                            '    txtrmtypname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry) '"select rmtypname from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") ")
                            'End If

                            'If IsDBNull(mySqlReader("maxchildallowed")) = False Then
                            '    txtmaxchild.Text = mySqlReader("maxchildallowed")

                            'End If
                            'If IsDBNull(mySqlReader("maxeballowed")) = False Then
                            '    txtmaxeb.Text = mySqlReader("maxeballowed")

                            'End If
                            'If IsDBNull(mySqlReader("mealplans")) = False Then
                            '    txtmealcode.Text = mySqlReader("mealplans")

                            'End If
                            'If IsDBNull(mySqlReader("fromage")) = False Then
                            '    txtfromage.Text = mySqlReader("fromage")

                            'End If
                            'If IsDBNull(mySqlReader("toage")) = False Then
                            '    txttoage.Text = mySqlReader("toage")

                            'End If
                            'If IsDBNull(mySqlReader("sharingcharge")) = False Then
                            '    txtsharing.Text = mySqlReader("sharingcharge")

                            'End If
                            'If IsDBNull(mySqlReader("ebcharge")) = False Then
                            '    txtEB.Text = mySqlReader("ebcharge")

                            'End If


                            'If IsDBNull(mySqlReader("noofchildren")) = False Then

                            '    Select Case mySqlReader("noofchildren")
                            '        Case 0
                            '            txtnoofchild.Text = "All"
                            '        Case 1
                            '            txtnoofchild.Text = "First Child"
                            '        Case 2
                            '            txtnoofchild.Text = "Second Child"

                            '    End Select
                            'End If




                            StrQryTemp = "select * from dbo.fn_childrmcatgory('" & txtplistcode.Text & "'," & ac & " )"

                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("rmcatcode")) = False Then
                                        rmcatcode = myReader("rmcatcode")
                                    End If
                                    If IsDBNull(myReader("value")) = False Then
                                        value = myReader("value")
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                        '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                        If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" Then

                                            For s = 0 To grdRoomrates.Columns.Count - 13 '12 '*** Danny Changed 18/03/2018
                                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)


                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = rmcatcode Then
                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                        Else
                                                            'txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvrow.RowIndex) + s + gvrow.RowIndex) 
                                                            txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 14) * gvrow.RowIndex) + s + gvrow.RowIndex) '13 '*** Danny Changed 18/03/2018
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                Select Case value
                                                                    Case "-3.000"
                                                                        value = "Free"
                                                                    Case "-1.000"
                                                                        value = "Incl"
                                                                    Case "-2.000"
                                                                        value = "N.Incl"
                                                                    Case "-4.000"
                                                                        value = "N/A"
                                                                    Case "-5.000"
                                                                        value = "On Request"
                                                                End Select

                                                                txt.Text = value
                                                            End If
                                                        End If

                                                        GoTo go1
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Next
go1:                            End While
                            End If
                            clsDBConnect.dbConnectionClose(myConn)
                            clsDBConnect.dbCommandClose(myCmd)
                            clsDBConnect.dbReaderClose(myReader)
                            ac = ac + 1
                            ' End If '''
                        End If

                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub

    Private Sub ShowDynamicGrid()
        Dim j As Long = 0
        Dim txt As TextBox
        Dim cnt As Long = 0
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim s As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If txt.Text <> Nothing Then
                heading(header) = txt.Text
            End If
        Next
        Dim m As Long = 0
        Dim rmcatcode As String = ""
        Dim value As String = ""
        Dim Linno As Integer
        Dim rmtypecode As String = ""
        Dim mealcode As String = ""
        Dim StrQry As String
        Dim headerlabel As New TextBox
        Dim myConn As New SqlConnection
        Dim myCmd As New SqlCommand
        Dim myReader As SqlDataReader
        Dim StrQryTemp As String

        Try


            Dim strQry1 As String = ""
            Dim cnt1 As Integer = 0


            'strQry1 = "select count( distinct clineno) from view_contracts_childpolicy_detail where childpolicyid='" & txtplistcode.Text & "'"

            'cnt1 = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry1)

            '' If cnt1 < 7 Then cnt1 = 7
            'fillDategrd(grdRoomrates, False, cnt1)
            strQry1 = ""
            strQry1 = "select * from view_contracts_childpolicy_detail d(nolock) where childpolicyid='" & txtplistcode.Text & "'"




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(strQry1, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        Dim txtrmtypcode As TextBox = gvrow.FindControl("txtrmtypcode")
                        Dim txtrmtypname As TextBox = gvrow.FindControl("txtrmtypname")
                        Dim txtmealcode As TextBox = gvrow.FindControl("txtmealcode")
                        Dim txtmaxchild As TextBox = gvrow.FindControl("txtmaxchild")
                        Dim txtmaxeb As TextBox = gvrow.FindControl("txtmaxeb")
                        Dim txtnoofchild As TextBox = gvrow.FindControl("txtnoofchild")
                        Dim txtfromage As TextBox = gvrow.FindControl("txtfromage")
                        Dim txttoage As TextBox = gvrow.FindControl("txttoage")
                        Dim txtsharing As TextBox = gvrow.FindControl("txtsharing")
                        Dim txtEB As TextBox = gvrow.FindControl("txtEB")
                        Dim ddlEbType As HtmlSelect = gvrow.FindControl("ddlEbType")

                        Select Case txtnoofchild.Text
                            Case "All"
                                txtnoofchild.Text = 0
                            Case "First Child"
                                txtnoofchild.Text = 1
                            Case "Second Child"
                                txtnoofchild.Text = 2

                        End Select


                        If IsDBNull(mySqlReader("roomtypes")) = False Then

                            If IsDBNull(mySqlReader("roomtypes")) = False Then
                                txtrmtypcode.Text = mySqlReader("roomtypes")

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
                                StrQry = ""
                                StrQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock)  where u.rmtypcode =rmtypcode and u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'')  rmtypname " _
                                    & " from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") "

                                txtrmtypname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry) '"select rmtypname from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") ")

                            End If

                            If IsDBNull(mySqlReader("mealplans")) = False Then
                                txtmealcode.Text = mySqlReader("mealplans")

                            End If
                            If IsDBNull(mySqlReader("maxchildallowed")) = False Then
                                txtmaxchild.Text = mySqlReader("maxchildallowed")

                            End If
                            If IsDBNull(mySqlReader("maxeballowed")) = False Then
                                txtmaxeb.Text = mySqlReader("maxeballowed")

                            End If
                            If IsDBNull(mySqlReader("noofchildren")) = False Then

                                Select Case mySqlReader("noofchildren")
                                    Case 0
                                        txtnoofchild.Text = "All"
                                    Case 1
                                        txtnoofchild.Text = "First Child"
                                    Case 2
                                        txtnoofchild.Text = "Second Child"

                                End Select
                            End If

                            If IsDBNull(mySqlReader("fromage")) = False Then
                                txtfromage.Text = mySqlReader("fromage")

                            End If
                            If IsDBNull(mySqlReader("toage")) = False Then
                                txttoage.Text = mySqlReader("toage")

                            End If
                            If IsDBNull(mySqlReader("sharingcharge")) = False Then
                                txtsharing.Text = mySqlReader("sharingcharge")

                            End If
                            If IsDBNull(mySqlReader("ebcharge")) = False Then
                                txtEB.Text = mySqlReader("ebcharge")

                            End If
                            If IsDBNull(mySqlReader("ebtype")) = False Then
                                ddlEbType.Value = mySqlReader("ebtype")

                            End If

                            If txtrmtypcode Is Nothing = False Then



                                ' StrQryTemp = "select d.* from view_contracts_childpolicy_detail d(nolock) where childpolicyid='" & txtplistcode.Text & "' and clineno=" & mySqlReader("clineno")


                                StrQryTemp = "select * from dbo.fn_childrmcatgory('" & txtplistcode.Text & "'," & mySqlReader("clineno") & " )"

                                myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                                myCmd = New SqlCommand(StrQryTemp, myConn)
                                myReader = myCmd.ExecuteReader
                                If myReader.HasRows Then
                                    While myReader.Read
                                        If IsDBNull(myReader("rmcatcode")) = False Then
                                            rmcatcode = myReader("rmcatcode")
                                        End If
                                        If IsDBNull(myReader("value")) = False Then
                                            value = myReader("value")
                                        Else
                                            value = ""
                                        End If

                                        For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                            '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                                            If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "EBType" And heading(j) <> "Clineno" Then

                                                For s = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                                                    headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                                    If headerlabel.Text <> Nothing Then
                                                        If headerlabel.Text = rmcatcode Then
                                                            If gvrow.RowIndex = 0 Then
                                                                txt = gvrow.FindControl("txt" & s)
                                                            Else
                                                                'txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                                txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 14) * gvrow.RowIndex) + s + gvrow.RowIndex) '13 '*** Danny Changed 18/03/2018
                                                            End If
                                                            If txt Is Nothing Then
                                                            Else
                                                                If value = "" Then
                                                                    txt.Text = ""
                                                                Else
                                                                    Select Case value
                                                                        Case "-3.000"
                                                                            value = "Free"
                                                                        Case "-1.000"
                                                                            value = "Incl"
                                                                        Case "-2.000"
                                                                            value = "N.Incl"
                                                                        Case "-4.000"
                                                                            value = "N/A"
                                                                        Case "-5.000"
                                                                            value = "On Request"
                                                                    End Select

                                                                    txt.Text = value
                                                                End If
                                                            End If

                                                            GoTo go1
                                                        End If
                                                    End If
                                                Next
                                            End If
                                        Next
go1:                                End While
                                End If
                                clsDBConnect.dbConnectionClose(myConn)
                                clsDBConnect.dbCommandClose(myCmd)
                                clsDBConnect.dbReaderClose(myReader)
                                ' End If '''
                            End If
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub





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

            grdRoomrates.Enabled = True
            wucCountrygroup.Disable(True)
            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btncopyratesnextrow.Enabled = True
            grdWeekDays.Enabled = True

            grdDates.Enabled = True
            grdseason.Enabled = True

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then

            grdDates.Enabled = False

            grdRoomrates.Enabled = False

            btncopyratesnextrow.Enabled = False
            btnfillrate.Enabled = False
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            btncopyratesnextrow.Enabled = False
            txtApplicableTo.Enabled = False
            wucCountrygroup.Disable(False)
            grdWeekDays.Enabled = False

            grdDates.Enabled = False
            grdseason.Enabled = False

        ElseIf ViewState("State") = "Edit" Then


            grdDates.Enabled = True

            grdRoomrates.Enabled = True

            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True
            btncopyratesnextrow.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            grdWeekDays.Enabled = True

            grdDates.Enabled = True
            grdseason.Enabled = True
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
            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate from view_contractseasons(nolock) where contractid='" & contractid & "' and seasonname='" & subseasonname & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Private Sub fillseasoncopy()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & CType(Session("contractid"), String) & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
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
                '  wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

                ShowRecordcopycontract(CType(lbltran.Text.Trim, String))
                'ShowRoomdetails(CType(lbltran.Text.Trim, String))

                If Session("Calledfrom") = "Offers" Then
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    ' Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    filldaysgrid()
                    fillweekdaysoffers(CType(hdnpromotionid.Value, String))

                    fillDategrd(grdDates, True)
                    ShowDatesnew(CType(hdnpromotionid.Value, String))

                    createdatacolumnsoffers()
                    createdatarowsoffer(lbltran.Text)
                    ' SetgridbindDataoffers(lbltran.Text)
                    SetgridbindData()

                    lblHeading.Text = "Copy Child Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Child Policy "
                    btnSave.Visible = True
                    txtplistcode.Text = ""
                    btnSave.Text = "Save"

                Else

                    wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                    wucCountrygroup.sbSetPageState(CType(Session("contractid"), String), Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()

                    filldaysgrid()
                    fillweekdays(CType(lbltran.Text.Trim, String))

                    fillDategrd(grdDates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))

                    createdatacolumns()
                    createdatarows()
                    SetgridbindData()

                    lblHeading.Text = "Copy Child Policy - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Child Policy "
                    fillseasoncopy()
                End If












            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            For Each GVRow In grdDates.Rows
                '  chkSelect = GVRow.FindControl("chkSelect")
                'If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    dpFDate = GVRow.FindControl("txtfromDate")
                    fDate(n) = CType(dpFDate.Text, String)
                    dpTDate = GVRow.FindControl("txtToDate")
                    tDate(n) = CType(dpTDate.Text, String)
                    'lblseason = GVRow.FindControl("lblseason")
                    'excl(n) = CType(lblseason.Text, String)
                Else
                    deletedrow = deletedrow + 1
                End If
                n = n + 1
                k = k + 1
            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdDates.Rows.Count > 1 Then
                fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDates, False, grdDates.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
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
            For Each GVRow In grdDates.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                fDate(n) = CType(dpFDate.Text, String)
                dpTDate = GVRow.FindControl("txtToDate")
                tDate(n) = CType(dpTDate.Text, String)
                'lblseason = GVRow.FindControl("lblseason")
                'excl(n) = CType(lblseason.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDates.Rows
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
            txtStayFromDt = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Sub seasonsgridfill()
        Try
            Dim myDS As New DataSet
            gv_Seasons.Visible = True
            strSqlQry = ""

            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdseason.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdseason.DataBind()

            Else
                grdseason.DataBind()

            End If

            For i As Integer = grdseason.Rows.Count - 1 To 1 Step -1
                Dim row As GridViewRow = grdseason.Rows(i)
                Dim previousRow As GridViewRow = grdseason.Rows(i - 1)
                Dim J As Integer = 2
                Dim k As Integer = 1
                If row.Cells(J).Text = previousRow.Cells(J).Text Then
                    If previousRow.Cells(J).RowSpan = 0 Then
                        If row.Cells(J).RowSpan = 0 Then
                            previousRow.Cells(J).RowSpan += 2
                            previousRow.Cells(k).RowSpan += 2

                        Else
                            previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                            previousRow.Cells(k).RowSpan = row.Cells(k).RowSpan + 1

                        End If
                        row.Cells(J).Visible = False
                        row.Cells(k).Visible = False
                    End If
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    'Sub seasonsgridfill()
    '    'Try
    '    '    Dim myDS As New DataSet
    '    '    gv_Seasons.Visible = True
    '    '    strSqlQry = ""


    '    '    strSqlQry = "select distinct seasonname,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' order by seasonname "

    '    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '    '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '    '    myDataAdapter.Fill(myDS)
    '    '    gv_Seasons.DataSource = myDS

    '    '    If myDS.Tables(0).Rows.Count > 0 Then
    '    '        gv_Seasons.DataBind()

    '    '    Else
    '    '        gv_Seasons.DataBind()

    '    '    End If
    '    'Catch ex As Exception
    '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    '    objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    'Finally

    '    '    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
    '    '    clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
    '    'End Try
    'End Sub
    Private Sub filldaysgrid()
        Dim chkSelect As CheckBox

        Dim dt As New DataTable
        Dim chkAll As CheckBox


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
            chkAll = grdWeekDays.HeaderRow.FindControl("chkweekAll")
            chkSelect.Checked = True

            chkAll.Checked = True
        Next





    End Sub
    Private Sub ShowRecordcopycontract(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            mySqlCmd = New SqlCommand(" Select * from view_contracts_childpolicy_header(nolock) Where childpolicyid='" & RefCode & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("childpolicyid")) = False Then
                        txtplistcode.Text = mySqlReader("childpolicyid")

                    End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If


                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)

                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)


                    End If


                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from view_contracts_childpolicy_header(nolock) where   contractid='" & hdncontractid.Value & "' and childpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "UNAPPROVED"

                    'Else
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "APPROVED"
                    'End If
                    Session("seasons") = ""
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

                        grdseason.Visible = True
                        strSqlQry = ""

                        If Session("seasons") <> "" Then
                            strSqlQry = "select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected  from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname  Not IN (" & strCondition & ") " _
                             & " union all " _
                               & " select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,1 selected  from view_contractseasons  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  3 "



                        Else
                            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName "
                        End If

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                        myDataAdapter.Fill(myDS)
                        grdseason.DataSource = myDS

                        If myDS.Tables(0).Rows.Count > 0 Then
                            grdseason.DataBind()


                        Else
                            grdseason.DataBind()

                        End If

                        Dim chkSelect As CheckBox
                        Dim lblselect As Label
                        For Each grdRow In grdseason.Rows
                            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                            lblselect = CType(grdRow.FindControl("lblselect"), Label)

                            If lblselect.Text = "1" Then
                                chkSelect.Checked = True
                                If ViewState("State") = "Copy" Then
                                    chkSelect.Enabled = True
                                    'Else
                                    '    chkSelect.Enabled = False
                                End If

                            End If

                        Next


                        For i As Integer = grdseason.Rows.Count - 1 To 1 Step -1
                            Dim row As GridViewRow = grdseason.Rows(i)
                            Dim previousRow As GridViewRow = grdseason.Rows(i - 1)
                            Dim J As Integer = 2
                            Dim k As Integer = 1
                            If row.Cells(J).Text = previousRow.Cells(J).Text Then
                                If previousRow.Cells(J).RowSpan = 0 Then
                                    If row.Cells(J).RowSpan = 0 Then
                                        previousRow.Cells(J).RowSpan += 2
                                        previousRow.Cells(k).RowSpan += 2

                                    Else
                                        previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                                        previousRow.Cells(k).RowSpan = row.Cells(k).RowSpan + 1

                                    End If
                                    row.Cells(J).Visible = False
                                    row.Cells(k).Visible = False
                                End If
                            End If
                        Next

                    End If

                    '------------------------------
                    'Changed by mohamed on 21/02/2018
                    If IsDBNull(mySqlReader("VATCalculationRequired")) = False Then
                        chkVATCalculationRequired.Checked = IIf(CType(mySqlReader("VATCalculationRequired"), String) = "1", True, False)
                    Else
                        chkVATCalculationRequired.Checked = False
                    End If
                    txtServiceCharges.Text = "0"
                    TxtMunicipalityFees.Text = "0"
                    txtTourismFees.Text = "0"
                    txtVAT.Text = "0"

                    If IsDBNull(mySqlReader("ServiceChargePerc")) = False Then
                        txtServiceCharges.Text = mySqlReader("ServiceChargePerc")
                    End If
                    If IsDBNull(mySqlReader("MunicipalityFeePerc")) = False Then
                        TxtMunicipalityFees.Text = mySqlReader("MunicipalityFeePerc")
                    End If
                    If IsDBNull(mySqlReader("TourismFeePerc")) = False Then
                        txtTourismFees.Text = mySqlReader("TourismFeePerc")
                    End If
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        txtVAT.Text = mySqlReader("VATPerc")
                    End If
                    '------------------------------
                End If
            End If

            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If


            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            mySqlCmd = New SqlCommand(" Select * from view_contracts_childpolicy_header(nolock) Where childpolicyid='" & RefCode & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("childpolicyid")) = False Then
                        txtplistcode.Text = mySqlReader("childpolicyid")

                    End If
                    If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If

                    'If IsDBNull(mySqlReader("subseascode")) = False Then
                    '    txtseasonname.Text = mySqlReader("subseascode")
                    '    filldates()

                    'End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)

                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",    ", ","), String)


                    End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from view_contracts_childpolicy_header(nolock) where   contractid='" & hdncontractid.Value & "' and childpolicyid ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
                    End If
                    Session("seasons") = ""
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

                        grdseason.Visible = True
                        strSqlQry = ""

                        If Session("seasons") <> "" Then

                            ' Rosalin 2019-10-29
                            strCondition = strCondition.Replace("'", "")

                            'strSqlQry = "select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected  from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname  Not IN (" & strCondition & ") " _
                            ' & " union all " _
                            '   & " select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,1 selected  from view_contractseasons  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  3 "

                            strSqlQry = " New_ContractSeason_ChildPolicy '" & CType(hdncontractid.Value, String) & "' ,'" & CType(strCondition, String) & "'"

                        Else

                            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName "

                        End If

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                        myDataAdapter.Fill(myDS)
                        grdseason.DataSource = myDS

                        If myDS.Tables(0).Rows.Count > 0 Then
                            grdseason.DataBind()


                        Else
                            grdseason.DataBind()

                        End If

                        Dim chkSelect As CheckBox
                        Dim lblselect As Label
                        For Each grdRow In grdseason.Rows
                            chkSelect = CType(grdRow.FindControl("chkseason"), CheckBox)
                            lblselect = CType(grdRow.FindControl("lblselect"), Label)

                            If lblselect.Text = "1" Then
                                chkSelect.Checked = True
                                If ViewState("State") = "Copy" Then
                                    chkSelect.Enabled = True
                                    'Else
                                    '    chkSelect.Enabled = False
                                End If

                            End If

                        Next

                        For i As Integer = grdseason.Rows.Count - 1 To 1 Step -1
                            Dim row As GridViewRow = grdseason.Rows(i)
                            Dim previousRow As GridViewRow = grdseason.Rows(i - 1)
                            Dim J As Integer = 2
                            Dim k As Integer = 1
                            If row.Cells(J).Text = previousRow.Cells(J).Text Then
                                If previousRow.Cells(J).RowSpan = 0 Then
                                    If row.Cells(J).RowSpan = 0 Then
                                        previousRow.Cells(J).RowSpan += 2
                                        previousRow.Cells(k).RowSpan += 2

                                    Else
                                        previousRow.Cells(J).RowSpan = row.Cells(J).RowSpan + 1
                                        previousRow.Cells(k).RowSpan = row.Cells(k).RowSpan + 1

                                    End If
                                    row.Cells(J).Visible = False
                                    row.Cells(k).Visible = False
                                End If
                            End If
                        Next

                    End If

                    '------------------------------
                    'Changed by mohamed on 21/02/2018
                    If IsDBNull(mySqlReader("VATCalculationRequired")) = False Then
                        chkVATCalculationRequired.Checked = IIf(CType(mySqlReader("VATCalculationRequired"), String) = "1", True, False)
                    Else
                        chkVATCalculationRequired.Checked = False
                    End If
                    txtServiceCharges.Text = "0"
                    TxtMunicipalityFees.Text = "0"
                    txtTourismFees.Text = "0"
                    txtVAT.Text = "0"

                    If IsDBNull(mySqlReader("ServiceChargePerc")) = False Then
                        txtServiceCharges.Text = mySqlReader("ServiceChargePerc")
                    End If
                    If IsDBNull(mySqlReader("MunicipalityFeePerc")) = False Then
                        TxtMunicipalityFees.Text = mySqlReader("MunicipalityFeePerc")
                    End If
                    If IsDBNull(mySqlReader("TourismFeePerc")) = False Then
                        txtTourismFees.Text = mySqlReader("TourismFeePerc")
                    End If
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        txtVAT.Text = mySqlReader("VATPerc")
                    End If
                    '------------------------------
                End If
            End If

            If chkctrygrp.Checked = True Then
                divuser.Style("display") = "block"
            Else
                divuser.Style("display") = "none"
            End If


            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
    End Sub
    Private Sub fillweekdays(ByVal refcode As String)

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        Dim dt2 As DataSet
        Dim chk As CheckBox
        dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_contracts_childpolicy_weekdays(nolock) where childpolicyid='" & refcode & "'")
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
    Private Sub ShowRoomdetails(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdRoomrates.Visible = True
            strSqlQry = ""


            Dim strQry As String
            Dim cnt As Integer = 0


            strQry = "select count( distinct clineno) from view_contracts_childpolicy_detail where childpolicyid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            If cnt < 7 Then cnt = 7
            fillDategrd(grdRoomrates, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_contracts_childpolicy_detail d  where   childpolicyid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdRoomrates.Rows

                    Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                    Dim txtrmtypname As TextBox = GvRow.findcontrol("txtrmtypname")
                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtmaxchild As TextBox = GvRow.FindControl("txtmaxchild")
                    Dim txtmaxeb As TextBox = GvRow.FindControl("txtmaxeb")
                    Dim txtnoofchild As TextBox = GvRow.FindControl("txtnoofchild")
                    Dim txtfromage As TextBox = GvRow.FindControl("txtfromage")
                    Dim txttoage As TextBox = GvRow.FindControl("txttoage")
                    Dim txtsharing As TextBox = GvRow.FindControl("txtsharing")
                    Dim txtEB As TextBox = GvRow.FindControl("txtEB")
                    Dim ddlEBtype As HtmlSelect = GvRow.Findcontrol("ddlEbType")


                    Select Case txtnoofchild.Text
                        Case "All"
                            txtnoofchild.Text = 0
                        Case "First Child"
                            txtnoofchild.Text = 1
                        Case "Second Child"
                            txtnoofchild.Text = 2

                    End Select

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
                        If IsDBNull(mySqlReader("maxchildallowed")) = False Then
                            txtmaxchild.Text = mySqlReader("maxchildallowed")

                        End If
                        If IsDBNull(mySqlReader("maxeballowed")) = False Then
                            txtmaxeb.Text = mySqlReader("maxeballowed")

                        End If
                        If IsDBNull(mySqlReader("noofchildren")) = False Then

                            Select Case mySqlReader("noofchildren")
                                Case 0
                                    txtnoofchild.Text = "All"
                                Case 1
                                    txtnoofchild.Text = "First Child"
                                Case 2
                                    txtnoofchild.Text = "Second Child"

                            End Select
                        End If

                        If IsDBNull(mySqlReader("fromage")) = False Then
                            txtfromage.Text = mySqlReader("fromage")

                        End If
                        If IsDBNull(mySqlReader("toage")) = False Then
                            txttoage.Text = mySqlReader("toage")

                        End If
                        If IsDBNull(mySqlReader("sharingcharge")) = False Then
                            txtsharing.Text = mySqlReader("sharingcharge")

                        End If
                        If IsDBNull(mySqlReader("ebcharge")) = False Then
                            txtEB.Text = mySqlReader("ebcharge")

                        End If

                        If IsDBNull(mySqlReader("ebtype")) = False Then
                            ddlEBtype.Value = mySqlReader("ebtype")

                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            If (ViewState("State") = "New" Or ViewState("State") = "Copy") And Session("Calledfrom") = "Offers" Then

                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"
            Else
                strSqlQry = "select count( distinct fromdate) from view_contracts_childpolicy_dates(nolock) where childpolicyid='" & RefCode & "'"
            End If

            '   strSqlQry = "select count( distinct fromdate) from view_contracts_childpolicy_dates(nolock) where childpolicyid='" & RefCode & "'" '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open 
            If (ViewState("State") = "New" Or ViewState("State") = "Copy") And Session("Calledfrom") = "Offers" Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
            Else
                mySqlCmd = New SqlCommand("Select * from view_contracts_childpolicy_dates(nolock) Where childpolicyid='" & RefCode & "'", mySqlConn)
            End If

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdDates.Rows
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
                            'If IsDBNull(mySqlReader("seasonname")) = False Then
                            '    lblseason.Text = CType(mySqlReader("seasonname"), String)
                            '    txtseasonname.Text = CType(mySqlReader("seasonname"), String)
                            'End If
                            Exit For
                        End If
                    Next
                End While
            End If
            '  txtseasonname.Enabled = False


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
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

                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()



                If Session("Calledfrom") = "Offers" Then
                    createdatacolumnsoffers()
                    createdatarows()
                    SetgridbindData()
                Else
                    createdatacolumns()
                    createdatarows()
                    SetgridbindData()
                End If




                ' ShowDynamicGridnew()
                'ShowDynamicGrid()
                'createdatarows()

                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit Child Policy - " + ViewState("hotelname")
                Page.Title = " Child Policy  "
            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                'createdatacolumns()
                'createdatarows()
                'SetgridbindData

                If Session("Calledfrom") = "Offers" Then
                    createdatacolumnsoffers()
                    createdatarows()
                    SetgridbindData()
                Else
                    createdatacolumns()
                    createdatarows()
                    SetgridbindData()
                End If


                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View Child Policy - " + ViewState("hotelname")
                Page.Title = "  Child Policy "
            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True

                PanelMain.Style("display") = "block"
                ViewState("State") = "Delete"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                If Session("Calledfrom") = "Offers" Then
                    createdatacolumnsoffers()
                    createdatarows()
                    SetgridbindData()
                Else
                    createdatacolumns()
                    createdatarows()
                    SetgridbindData()
                End If

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete Child Policy - " + ViewState("hotelname")
                Page.Title = " Child Policy "
            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"

                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                ShowRecord(CType(lbltran.Text.Trim, String))
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                If Session("Calledfrom") = "Offers" Then
                    createdatacolumnsoffers()
                    createdatarows()
                    SetgridbindData()
                Else
                    createdatacolumns()
                    createdatarows()
                    SetgridbindData()
                End If

                DisableControl()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Child Policy - " + ViewState("hotelname")
                Page.Title = Page.Title + " " + " Child Policy "
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ") order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"

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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
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
    Private Sub createdatarowsoffer(ByVal plistcode As String)
        Dim cnt As Long
        Dim k As Long = 0
        Dim mealplanstr As String = ""
        Dim rmtypcode As String = ""
        Dim myDS As New DataSet
        Dim mealcount As Integer = 0
        Try
            ViewState("mealcount") = Nothing
            Session("MealPlans") = Nothing

            'If ViewState("OfferCopy") = 1 Then
            '    Session("GV_HotelData") = Nothing
            'End If



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select count(pr.clineno) from  view_contracts_childpolicy_detail pr(nolock) where pr.childpolicyid='" & plistcode & "'"



            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()
            If cnt = 0 Then cnt = 7
            Dim StrQry As String = ""
            Dim rmtypname As String = ""
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String
            Dim arr_meal(cnt + 1) As String
            Dim arr_pricepax(cnt + 1) As String
            Dim arr_unityesno(cnt + 1) As String
            Dim arr_extraper(cnt + 1) As String
            Dim arr_fromage(cnt + 1) As String
            Dim arr_toage(cnt + 1) As String
            Dim arr_NoBabyCot(cnt + 1) As String '*** Danny Added 18/03/2018
            Dim arr_sharingcharge(cnt + 1) As String
            Dim arr_ebcharge(cnt + 1) As String
            Dim arr_lineno(cnt + 1) As String
            Dim arr_ebtype(cnt + 1) As String
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))




            'strSqlQry = "select roomtypes,mealplans,maxchildallowed,maxeballowed,noofchildren ,fromage,toage,sharingcharge,ebcharge from  view_contracts_childpolicy_detail pr where  pr.childpolicyid='" & txtplistcode.Text & "'"
            '*** Danny Added 18/03/2018 "NoBabyCot"
            strSqlQry = "select roomtypes,mealplans,maxchildallowed,maxeballowed,noofchildren ,fromage,toage,NoBabyCot,sharingcharge,ebcharge,pr.clineno,pr.ebtype from  view_contracts_childpolicy_detail pr cross apply dbo.SplitString1colsWithOrderField(pr.roomtypes,',')  rm  " _
                & " join partyrmtyp p  on rm.Item1 =p.rmtypcode and p.partycode =pr.partycode  where  pr.childpolicyid='" & plistcode & "' and rm.ordNo=1 order by ISNULL(p.rankord,99) ,fromage "


            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True

                If IsDBNull(mySqlReader("roomtypes")) = False Then

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
                    StrQry = ""

                    If Session("Calledfrom") = "Offers" Then

                        '     StrQry = "select distinct stuff((select  ',' + u.rmtypcode   from partyrmtyp u(nolock)  where u.rmtypcode =v.rmtypcode and u.partycode =v.partycode and u.partycode='" & hdnpartycode.Value & "' and v.rmtypcode in (" & strCondition & ")   group by u.rmtypcode    for xml path('')),1,1,'')  rmtypname " _
                        '& " from partyrmtyp p , view_offers_rmtype v(nolock) where p.partycode =v.partycode  and p.rmtypcode =v.rmtypcode and v.promotionid ='" & hdnpromotionid.Value & "' and  p.partycode='" & hdnpartycode.Value & "' and p.inactive=0  and v.rmtypcode in (" & strCondition & ") "

                        StrQry = "select distinct stuff((select  ',' + u.rmtypcode   from partyrmtyp u(nolock) ,view_offers_rmtype v(nolock)  where u.rmtypcode =v.rmtypcode and u.partycode =v.partycode and u.partycode='" & hdnpartycode.Value & "' and v.promotionid ='" & hdnpromotionid.Value & "' and  " _
                            & " v.rmtypcode in (" & strCondition & ")   group by u.rmtypcode    for xml path('')),1,1,'') " ' rmtypname " _
                        '& " from partyrmtyp p , view_offers_rmtype v(nolock) where p.partycode =v.partycode  and p.rmtypcode =v.rmtypcode and v.promotionid ='" & hdnpromotionid.Value & "' and  p.partycode='" & hdnpartycode.Value & "' and p.inactive=0  and v.rmtypcode in (" & strCondition & ") "


                        rmtypcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry)

                        StrQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock) ,view_offers_rmtype v(nolock)  where u.rmtypcode =v.rmtypcode and u.partycode =v.partycode and u.partycode='" & hdnpartycode.Value & "' and v.promotionid ='" & hdnpromotionid.Value & "' and " _
                            & " v.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'') " ' rmtypname " _
                        '& " from partyrmtyp p , view_offers_rmtype v(nolock) where p.partycode =v.partycode  and p.rmtypcode =v.rmtypcode and v.promotionid ='" & hdnpromotionid.Value & "' and  p.partycode='" & hdnpartycode.Value & "' and p.inactive=0  and v.rmtypcode in (" & strCondition & ") "

                        rmtypname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry)

                        arr_rows(k) = rmtypcode
                    Else
                        StrQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock)  where u.rmtypcode =rmtypcode and u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'')  rmtypname " _
                     & " from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") "

                        arr_rows(k) = mySqlReader("roomtypes")
                    End If




                End If


                arr_rname(k) = rmtypname ' mySqlReader("roomtypes")
                arr_meal(k) = mySqlReader("mealplans")
                arr_pricepax(k) = mySqlReader("maxchildallowed")
                arr_unityesno(k) = mySqlReader("maxeballowed")
                arr_extraper(k) = mySqlReader("noofchildren")
                arr_fromage(k) = mySqlReader("fromage")
                arr_toage(k) = mySqlReader("toage")
                arr_NoBabyCot(k) = mySqlReader("NoBabyCot") '*** Danny Added 18/03/2018


                arr_sharingcharge(k) = DecRound(mySqlReader("sharingcharge"))



                arr_ebcharge(k) = DecRound(mySqlReader("ebcharge"))
                arr_lineno(k) = mySqlReader("clineno")
                arr_ebtype(k) = mySqlReader("ebtype")
                k = k + 1
            End While
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)

            'Here add rows.....

            dt = Session("GV_HotelData")




            Dim dr As DataRow


            dr = Nothing


            'End If
            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow


                dr("RoomTypecode") = arr_rows(row)
                dr("Select_RoomType") = arr_rname(row)
                dr("Meal_Plan") = arr_meal(row)
                dr("MaxChild Allowed") = arr_pricepax(row)
                dr("MaxEB Allowed") = arr_unityesno(row)
                dr("No.of Children") = arr_extraper(row)
                dr("From Age") = arr_fromage(row)
                dr("To Age") = arr_toage(row)
                dr("No Of Baby Cot") = arr_NoBabyCot(row) '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                dr("Charge for Sharing") = arr_sharingcharge(row)
                dr("Charge for EB") = arr_ebcharge(row)
                dr("CLineno") = arr_lineno(row)
                dr("EBType") = arr_ebtype(row)
                dt.Rows.Add(dr)
            Next


            If ViewState("OfferCopy") = 1 Then
                For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                    Dim row1 As DataRow = dt.Rows(i)
                    If row1.Item(0) Is Nothing Then
                        dt.Rows.Remove(row1)
                    ElseIf row1.Item(0).ToString = "" Then
                        dt.Rows.Remove(row1)
                    End If
                Next
            End If

            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()

            '  Roomratesrowsetting()

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub createdatarows()
        Dim cnt As Long
        Dim k As Long = 0
        Dim mealplanstr As String = ""

        Dim myDS As New DataSet
        Dim mealcount As Integer = 0
        Try
            ViewState("mealcount") = Nothing
            Session("MealPlans") = Nothing





            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select count(pr.clineno) from  view_contracts_childpolicy_detail pr(nolock) where pr.childpolicyid='" & txtplistcode.Text & "'"



            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()
            If cnt = 0 Then cnt = 7
            Dim StrQry As String = ""
            Dim rmtypname As String = ""
            Dim arr_rows(cnt + 1) As String
            Dim arr_rname(cnt + 1) As String
            Dim arr_meal(cnt + 1) As String
            Dim arr_pricepax(cnt + 1) As String
            Dim arr_unityesno(cnt + 1) As String
            Dim arr_extraper(cnt + 1) As String
            Dim arr_fromage(cnt + 1) As String
            Dim arr_toage(cnt + 1) As String
            Dim arr_NoBabyCot(cnt + 1) As String '*** Danny Added 18/03/2018
            Dim arr_sharingcharge(cnt + 1) As String
            Dim arr_ebcharge(cnt + 1) As String
            Dim arr_lineno(cnt + 1) As String
            Dim arr_ebtype(cnt + 1) As String
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))




            'strSqlQry = "select roomtypes,mealplans,maxchildallowed,maxeballowed,noofchildren ,fromage,toage,sharingcharge,ebcharge from  view_contracts_childpolicy_detail pr where  pr.childpolicyid='" & txtplistcode.Text & "'"
            '*** Danny Added 18/03/2018 "NoBabyCot"
            strSqlQry = "select  * from ( select  distinct roomtypes,mealplans,maxchildallowed,maxeballowed,noofchildren ,fromage,toage,NoBabyCot,sharingcharge,ebcharge,pr.clineno,pr.ebtype,ISNULL(p.rankord,99)rankord from  view_contracts_childpolicy_detail pr cross apply dbo.SplitString1colsWithOrderField(pr.roomtypes,',')  rm  " _
                & " join partyrmtyp p  on rm.Item1 =p.rmtypcode and p.partycode =pr.partycode  where  pr.childpolicyid='" & txtplistcode.Text & "' and rm.ordNo=1  ) rs  order by ISNULL(rankord,99) ,fromage "


            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            While mySqlReader.Read = True

                If IsDBNull(mySqlReader("roomtypes")) = False Then

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
                    StrQry = ""
                    StrQry = "select distinct stuff((select  ',' + u.rmtypname   from partyrmtyp u(nolock)  where u.rmtypcode =rmtypcode and u.partycode =partycode and partycode='" & hdnpartycode.Value & "' and u.rmtypcode in (" & strCondition & ")   group by rmtypname    for xml path('')),1,1,'')  rmtypname " _
                        & " from partyrmtyp where partycode='" & hdnpartycode.Value & "' and rmtypcode in (" & strCondition & ") "

                    rmtypname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrQry)
                End If

                arr_rows(k) = mySqlReader("roomtypes")
                arr_rname(k) = rmtypname ' mySqlReader("roomtypes")
                arr_meal(k) = mySqlReader("mealplans")
                arr_pricepax(k) = mySqlReader("maxchildallowed")
                arr_unityesno(k) = mySqlReader("maxeballowed")
                arr_extraper(k) = mySqlReader("noofchildren")
                arr_fromage(k) = mySqlReader("fromage")
                arr_toage(k) = mySqlReader("toage")

               
                arr_NoBabyCot(k) = Val(mySqlReader("NoBabyCot")) '*** Danny Added 18/03/2018


                'Select Case DecRound(mySqlReader("sharingcharge"))
                '    Case "-3"
                '        arr_sharingcharge(k) = "Free"
                '    Case "-1"
                '        arr_sharingcharge(k) = "Incl"
                '    Case "-2"
                '        arr_sharingcharge(k) = "N.Incl"
                '    Case "-4"
                '        arr_sharingcharge(k) = "N/A"
                '    Case "-5"
                '        arr_sharingcharge(k) = "On Request"
                'End Select

                arr_sharingcharge(k) = DecRound(mySqlReader("sharingcharge"))

                'Select Case DecRound(mySqlReader("ebcharge"))
                '    Case "-3"
                '        arr_ebcharge(k) = "Free"
                '    Case "-1"
                '        arr_ebcharge(k) = "Incl"
                '    Case "-2"
                '        arr_ebcharge(k) = "N.Incl"
                '    Case "-4"
                '        arr_ebcharge(k) = "N/A"
                '    Case "-5"
                '        arr_ebcharge(k) = "On Request"
                'End Select

                arr_ebcharge(k) = DecRound(mySqlReader("ebcharge"))
                arr_lineno(k) = mySqlReader("clineno")
                arr_ebtype(k) = mySqlReader("ebtype")
                k = k + 1
            End While
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)

            'Here add rows.....

            dt = Session("GV_HotelData")
            Dim dr As DataRow


            dr = Nothing


            'End If
            Dim row As Long

            For row = 0 To k - 1
                dr = dt.NewRow


                dr("RoomTypecode") = arr_rows(row)
                dr("Select_RoomType") = arr_rname(row)
                dr("Meal_Plan") = arr_meal(row)
                dr("MaxChild Allowed") = arr_pricepax(row)
                dr("MaxEB Allowed") = arr_unityesno(row)
                dr("No.of Children") = arr_extraper(row)
                dr("From Age") = arr_fromage(row)
                dr("To Age") = arr_toage(row)
                dr("No Of Baby Cot") = arr_NoBabyCot(row) '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                dr("Charge for Sharing") = arr_sharingcharge(row)
                dr("Charge for EB") = arr_ebcharge(row)
                dr("CLineno") = arr_lineno(row)
                dr("EBType") = arr_ebtype(row)
                dt.Rows.Add(dr)
            Next


            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()

            '  Roomratesrowsetting()

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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

        '  strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"

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

        '   strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

        If Session("Calledfrom") = "Offers" Then
            strSqlQry = "select v.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock),view_offers_meal v(nolock)  " _
                & " where v.mealcode=p.mealcode and v.mealcode=m.mealcode and v.promotionid='" & hdnpromotionid.Value & "' and  m.active=1 and p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
        Else
            strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and m.active=1 and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
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
    Private Sub fillweekdaysoffers(ByVal refcode As String)

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        Dim dt2 As DataSet
        Dim chk As CheckBox
        dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select dm.mktcode from view_offers_header h(nolock) cross apply dbo.splitallotmkt(h.daysoftheweek,',')  dm  where h.promotionid='" & hdnpromotionid.Value & "'")
        If dt2.Tables(0).Rows.Count > 0 Then

            For Each gvRow In grdWeekDays.Rows
                chk = gvRow.FindControl("chkSelect")
                chk.Checked = False
            Next
        End If
        Dim i As Integer
        For i = 0 To dt2.Tables(0).Rows.Count - 1

            Select Case dt2.Tables(0).Rows(i).Item(0).ToString
                Case "SUNDAY"
                    chk = CType(grdWeekDays.Rows(0).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "MONDAY"
                    chk = CType(grdWeekDays.Rows(1).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "TUESDAY"
                    chk = CType(grdWeekDays.Rows(2).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "WEDNESDAY"
                    chk = CType(grdWeekDays.Rows(3).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "THURSDAY"
                    chk = CType(grdWeekDays.Rows(4).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "FRIDAY"
                    chk = CType(grdWeekDays.Rows(5).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

                Case "SATURDAY"
                    chk = CType(grdWeekDays.Rows(6).FindControl("chkSelect"), CheckBox)
                    chk.Checked = True

            End Select
        Next
        'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        'clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(SqlConn)              'connection close 


    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        ViewState("State") = "New"


        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        lblstatus.Visible = False
        lblstatustext.Visible = False

        If Session("Calledfrom") = "Offers" Then
            divoffer.Style.Add("display", "block")

            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            filldaysgrid()
            fillweekdaysoffers(CType(hdnpromotionid.Value, String))

            divcopy1.Style("display") = "none"

            btncopyratesnextrow.Visible = True
            lable12.Visible = True

            fillDategrd(grdDates, True)
            ShowDatesnew(CType(hdnpromotionid.Value, String))

            fillroomgrid(grdRoomrates, True)
            createdatacolumnsoffers()

            btnSave.Visible = True

            btnSave.Text = "Save"
            lblHeading.Text = "New Child Policy - " + ViewState("hotelname") + " -" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + " Child Policy -" + ViewState("hotelname")

            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            lblratetype.Text = "Promotions"
        Else

            wucCountrygroup.Visible = True
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")

            fillseason()

            grdDates.Visible = True

            txtplistcode.Text = ""
            filldaysgrid()
            seasonsgridfill()
            fillDategrd(grdDates, True)
            divcopy1.Style("display") = "none"

            btncopyratesnextrow.Visible = True
            lable12.Visible = True

            fillroomgrid(grdRoomrates, True)
            createdatacolumns()

            btnSave.Visible = True

            btnSave.Text = "Save"
            lblHeading.Text = "New Child Policy - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + " Child Policy -" + ViewState("hotelname")

            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            lblratetype.Text = "Contract"
        End If



        chkVATCalculationRequired.Checked = True
        Call sbFillTaxDetail() 'changed by mohamed on 21/02/2018

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
            strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.active=1 and rc.accom_extra='L' and prc.partycode='" _
                      & hdnpartycode.Value & "' "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.active=1 and rc.accom_extra='L' and prc.partycode='" _
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
            dt.Columns.Add(New DataColumn("No Of Baby Cot", GetType(String))) '*** Danny Added 18/03/2018
            dt.Columns.Add(New DataColumn("Charge for Sharing", GetType(String)))
            dt.Columns.Add(New DataColumn("Charge for EB", GetType(String)))
            dt.Columns.Add(New DataColumn("CLineno", GetType(String)))  '' Added 25/01/17
            dt.Columns.Add(New DataColumn("EBType", GetType(String)))


            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next

            Session("GV_HotelData") = dt


            For i = grdRoomrates.Columns.Count - 1 To 13 Step -1 '12 '*** Danny Changed 18/03/2018
                grdRoomrates.Columns.RemoveAt(i)
            Next


            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "No Of Baby Cot" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" And col.ColumnName <> "CLineno" And col.ColumnName <> "EBType" Then '' Added 25/01/17
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
                Dim txt_Baby_CotNo As TextBox = gvRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")
                Dim lblCLineno As Label = gvRow.FindControl("lblCLineno") '' Added 25/01/17

                Dim txtEB As TextBox = gvRow.FindControl("txtEB")
                Dim ddlebtype As HtmlSelect = gvRow.FindControl("ddlEbType")


                dr("RoomTypecode") = txtrmtypcode.Text
                dr("Select_RoomType") = txtrmtypname.Text
                dr("Meal_Plan") = txtmealcode.Text
                dr("MaxChild Allowed") = txtmaxchild.Text
                dr("MaxEB Allowed") = txtmaxeb.Text
                dr("No.of Children") = txtnoofchild.Text
                dr("From Age") = txtfromage.Text
                dr("To Age") = txttoage.Text
                dr("No Of Baby Cot") = txt_Baby_CotNo.Text '*** Danny Added 18/03/2018
                dr("Charge for Sharing") = txtsharing.Text
                dr("Charge for EB") = txtEB.Text
                dr("CLineno") = lblCLineno.Text
                dr("EBType") = ddlebtype.value

                dt.Rows.Add(dr)



            Next


            grdRoomrates.DataSource = dt
            grdRoomrates.DataBind()




        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub createdatacolumnsoffers()

        Dim dt As New DataTable
        Dim dr As DataRow

        Dim gvRow As GridViewRow

        Try

            cnt = 0
            Session("GV_HotelData") = Nothing
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select count(distinct v.rmcatcode) from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
             & "  rc.active=1 and v.promotionid='" & hdnpromotionid.Value & "' and  prc.rmcatcode=rc.rmcatcode and rc.active=1 and rc.accom_extra='L' and prc.partycode='" _
                      & hdnpartycode.Value & "' "


            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()

            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = " select rmcatcode from  (select distinct v.rmcatcode,isnull(rc.rankorder,999) rankorder from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
       & "  rc.active=1 and v.promotionid='" & hdnpromotionid.Value & "' and  prc.rmcatcode=rc.rmcatcode and rc.active=1 and rc.accom_extra='L' and prc.partycode='" _
                & hdnpartycode.Value & "') rs  order by rs.rankorder "
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
            dt.Columns.Add(New DataColumn("No Of Baby Cot", GetType(String)))  '*** Danny Added 18/03/2018
            dt.Columns.Add(New DataColumn("Charge for Sharing", GetType(String)))
            dt.Columns.Add(New DataColumn("Charge for EB", GetType(String)))
            dt.Columns.Add(New DataColumn("CLineno", GetType(String)))  '' Added 25/01/17
            dt.Columns.Add(New DataColumn("EBType", GetType(String)))


            'create columns of this room types in data table
            For i = 0 To cnt - 1
                dt.Columns.Add(New DataColumn(arr(i), GetType(String)))
            Next

            Session("GV_HotelData") = dt


            For i = grdRoomrates.Columns.Count - 1 To 13 Step -1 '12 '*** Danny Changed 18/03/2018
                grdRoomrates.Columns.RemoveAt(i)

                'grdRoomrates.HeaderRow.Cells.RemoveAt(i)

            Next


            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                '*** Danny Added 18/03/2018 (And col.ColumnName <> "No Of Baby Cot")
                If col.ColumnName <> "RoomTypecode" And col.ColumnName <> "Select_RoomType" And col.ColumnName <> "Meal_Plan" And col.ColumnName <> "MaxChild Allowed" And col.ColumnName <> "MaxEB Allowed" And col.ColumnName <> "No.of Children" And col.ColumnName <> "From Age" And col.ColumnName <> "To Age" And col.ColumnName <> "No Of Baby Cot" And col.ColumnName <> "Charge for Sharing" And col.ColumnName <> "Charge for EB" And col.ColumnName <> "CLineno" And col.ColumnName <> "EBType" Then '' Added 25/01/17
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
                Dim txt_Baby_CotNo As TextBox = gvRow.FindControl("txt_Baby_CotNo")  '*** Danny Added 18/03/2018
                Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")
                Dim lblCLineno As Label = gvRow.FindControl("lblCLineno") '' Added 25/01/17

                Dim txtEB As TextBox = gvRow.FindControl("txtEB")
                Dim ddlebtype As HtmlSelect = gvRow.FindControl("ddlEbType")


                dr("RoomTypecode") = txtrmtypcode.Text
                dr("Select_RoomType") = txtrmtypname.Text
                dr("Meal_Plan") = txtmealcode.Text
                dr("MaxChild Allowed") = txtmaxchild.Text
                dr("MaxEB Allowed") = txtmaxeb.Text
                dr("No.of Children") = txtnoofchild.Text
                dr("From Age") = txtfromage.Text
                dr("To Age") = txttoage.Text
                dr("No Of Baby Cot") = txt_Baby_CotNo.Text  '*** Danny Added 18/03/2018
                dr("Charge for Sharing") = txtsharing.Text
                dr("Charge for EB") = txtEB.Text
                dr("CLineno") = lblCLineno.Text
                dr("EBType") = ddlebtype.Value

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

    End Sub
    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click
        clearall()
        Panelsearch.Enabled = True
        Session("GV_HotelData") = Nothing
        PanelMain.Style("display") = "none"
        'Panelsearch.Style("display")="block")
        grdDates.DataSource = CreateDataSource(0)
        grdDates.DataBind()
        lblHeading.Text = "Child Policy  -" + ViewState("hotelname")
        wucCountrygroup.clearsessions()
        Session("GV_HotelData") = Nothing
        wucCountrygroup.sbSetPageState("", "CONTRACTCHILD", CType(Session("ContractState"), String))
        Response.Redirect(Request.RawUrl)

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

    End Sub
    Protected Sub grdRoomrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRoomrates.RowDataBound
        Dim l As Integer

        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim txtrmtypcode As TextBox = e.Row.FindControl("txtrmtypcode")
            Dim txtmealcode As TextBox = e.Row.FindControl("txtmealcode")
            Dim txtmaxchild As TextBox = e.Row.FindControl("txtmaxchild")
            Dim txtmaxeb As TextBox = e.Row.FindControl("txtmaxeb")
            Dim txtnoofchild As TextBox = e.Row.FindControl("txtnoofchild")
            Dim txtfromage As TextBox = e.Row.FindControl("txtfromage")
            Dim txttoage As TextBox = e.Row.FindControl("txttoage")
            Dim txt_Baby_CotNo As TextBox = e.Row.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018 
            Dim txtsharing As TextBox = e.Row.FindControl("txtsharing")
            Dim txtEB As TextBox = e.Row.FindControl("txtEB")

            Numberssrvctrl(txtmaxchild)
            Numberssrvctrl(txtmaxeb)
            '  Numberssrvctrl(txtnoofchild)
            Numberssrvctrl(txtfromage)
            Numberssrvctrl(txtmaxeb)
            Numberssrvctrl(txttoage)
            Numberssrvctrl(txt_Baby_CotNo)  '*** Danny Added 18/03/2018
            'Numberssrvctrl(txtsharing)
            'Numberssrvctrl(txtEB)
            ' t.Attributes.Add("onchange", "$find('txtnoofchild_AutoCompleteExtender').set_contextKey(this.value);")


            Dim txt1SH As TextBox = e.Row.FindControl("txtsharing")
            Dim txtTV1SH As TextBox = Nothing
            Dim txtNTV1SH As TextBox = Nothing
            Dim txtVAT1SH As TextBox = Nothing
            txtTV1SH = e.Row.FindControl("txtTVSH")
            txtNTV1SH = e.Row.FindControl("txtNTVSH")
            txtVAT1SH = e.Row.FindControl("txtVATSH")

            If txt1SH IsNot Nothing Then
                txt1SH.Attributes.Add("onchange", "javascript:calculateVAT('" & txt1SH.ClientID & "','" & txtTV1SH.ClientID & "','" & txtNTV1SH.ClientID & "','" & txtVAT1SH.ClientID & "');")
            End If

            Dim txt2EB As TextBox = e.Row.FindControl("txtEB")
            Dim txtTV2EB As TextBox = Nothing
            Dim txtNTV2EB As TextBox = Nothing
            Dim txtVAT2EB As TextBox = Nothing
            txtTV2EB = e.Row.FindControl("txtTVEB")
            txtNTV2EB = e.Row.FindControl("txtNTVEB")
            txtVAT2EB = e.Row.FindControl("txtVATEB")

            If txt2EB IsNot Nothing Then
                txt2EB.Attributes.Add("onchange", "javascript:calculateVAT('" & txt2EB.ClientID & "','" & txtTV2EB.ClientID & "','" & txtNTV2EB.ClientID & "','" & txtVAT2EB.ClientID & "');")
            End If

            'Dynamic Starts Here
            For i = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                'l = IIf(e.Row.RowIndex >= 1, ((grdRoomrates.Columns.Count - 12) * e.Row.RowIndex), 0)
                l = IIf(e.Row.RowIndex >= 1, ((grdRoomrates.Columns.Count - 13) * e.Row.RowIndex), 0) '12 '*** Danny Changed 18/03/2018

                Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & i)

                Dim txt3Dyn As TextBox = e.Row.FindControl("txt" & i + l)
                Dim txtTV3Dyn As TextBox = Nothing
                Dim txtNTV3Dyn As TextBox = Nothing
                Dim txtVAT3Dyn As TextBox = Nothing
                txtTV3Dyn = e.Row.FindControl("txtTV" & i + l)
                txtNTV3Dyn = e.Row.FindControl("txtNTV" & i + l)
                txtVAT3Dyn = e.Row.FindControl("txtVAT" & i + l)
                If txt3Dyn IsNot Nothing Then
                    If txt3Dyn.Style("FieldHeader") <> "" Then
                        If txt3Dyn.Style("FieldHeader").Contains("Sr No") = False And txt3Dyn.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt3Dyn.Style("FieldHeader").Contains("Extra Person Supplement") = False And txt3Dyn.Style("FieldHeader").Contains("Min Nights") = False And txt3Dyn.Style("FieldHeader").Contains("Pkg") = False And txt3Dyn.Style("FieldHeader").Contains("Remark") = False Then
                            txt3Dyn.Attributes.Add("onchange", "javascript:calculateVAT('" & txt3Dyn.ClientID & "','" & txtTV3Dyn.ClientID & "','" & txtNTV3Dyn.ClientID & "','" & txtVAT3Dyn.ClientID & "');")
                        End If
                    End If
                End If
            Next i

        End If

        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strRowId As String = e.Row.RowIndex
            Dim strFoucsColumnIndex = "3"
            e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
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
        Dim maxchild(count) As String
        Dim maxeb(count) As String
        Dim noofchild(count) As String
        Dim fromage(count) As String
        Dim toage(count) As String
        Dim Baby_CotNo(count) As String '*** Danny Added 18/03/2018
        Dim sharing(count) As String
        Dim EB(count) As String
        Dim EBtype(count) As String


        '   blankrow = 0

        '   CopyRow = 0


        Try

            For Each GVRow In grdRoomrates.Rows


                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtmaxchild As TextBox = GVRow.FindControl("txtmaxchild")
                Dim txtmaxeb As TextBox = GVRow.FindControl("txtmaxeb")
                Dim txtnoofchild As TextBox = GVRow.FindControl("txtnoofchild")
                Dim txtfromage As TextBox = GVRow.FindControl("txtfromage")
                Dim txttoage As TextBox = GVRow.FindControl("txttoage")
                Dim txt_Baby_CotNo As TextBox = GVRow.FindControl("txt_Baby_CotNo")  '*** Danny Added 18/03/2018 
                Dim txtsharing As TextBox = GVRow.FindControl("txtsharing")

                Dim txtEB As TextBox = GVRow.FindControl("txtEB")
                Dim ddlEbType As HtmlSelect = GVRow.FindControl("ddlEbType")



                If chkSelect.Checked = True Then
                    CopyRowlist.Add(n)
                    CopyRow = n
                End If

                rmtypcode(n) = CType(txtrmtypcode.Text, String)
                rmtypname(n) = CType(txtrmtypname.Text, String)
                mealcode(n) = CType(txtmealcode.Text, String)
                maxchild(n) = CType(txtmaxchild.Text, String)
                maxeb(n) = CType(txtmaxeb.Text, String)
                noofchild(n) = CType(txtnoofchild.Text, String)
                fromage(n) = CType(txtfromage.Text, String)
                toage(n) = CType(txttoage.Text, String)
                Baby_CotNo(n) = CType(txt_Baby_CotNo.Text, String) '*** Danny Added  18/03/2018
                sharing(n) = CType(txtsharing.Text, String)
                EB(n) = CType(txtEB.Text, String)
                EBtype(n) = CType(ddlEbType.Value, String)



                txtrmtypcodenew.Add(CType(txtrmtypcode.Text, String))
                txtrmtypnamenew.Add(CType(txtrmtypname.Text, String))
                txtmealcodenew.Add(CType(txtmealcode.Text, String))
                txtmaxchildnew.Add(CType(txtmaxchild.Text, String))
                txtmaxebnew.Add(CType(txtmaxeb.Text, String))
                txtnoofchildnew.Add(CType(txtnoofchild.Text, String))
                txtfromagenew.Add(CType(txtfromage.Text, String))
                txttoagenew.Add(CType(txttoage.Text, String))
                txtBabyCotNo.Add(CType(txt_Baby_CotNo.Text, String)) '*** Danny Added  18/03/2018
                txtsharingnew.Add(CType(txtsharing.Text, String))
                txtEBnew.Add(CType(txtEB.Text, String))
                ddlEBtypenew.Add(CType(ddlEbType.Value, String))

                If chkSelect.Checked = False And (txtrmtypname.Text = "") Then
                    blankrow = blankrow + 1
                End If

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
        txtmaxchildnew.Clear()
        txtmaxebnew.Clear()
        txtnoofchildnew.Clear()
        txtfromagenew.Clear()
        txttoagenew.Clear()
        txtBabyCotNo.Clear() '*** danny Added 18/03/2018
        txtsharingnew.Clear()
        txtEBnew.Clear()
        CopyRowlist.Clear()
        ddlEBtypenew.clear()


    End Sub
    Protected Sub btncopyratesnextrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyratesnextrow.Click
        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        setdynamicvalues()
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdRoomrates.Rows.Count '+ 1


            Dim n As Integer = 0

            Dim txtnoofchidauto As AjaxControlToolkit.AutoCompleteExtender







            For Each GVRow In grdRoomrates.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then




                Dim chkSelect As CheckBox = GVRow.FindControl("chkSelect")

                Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                Dim txtmaxchild As TextBox = GVRow.FindControl("txtmaxchild")
                Dim txtmaxeb As TextBox = GVRow.FindControl("txtmaxeb")
                Dim txtnoofchild As TextBox = GVRow.FindControl("txtnoofchild")
                Dim txtfromage As TextBox = GVRow.FindControl("txtfromage")
                Dim txttoage As TextBox = GVRow.FindControl("txttoage")
                Dim txt_Baby_CotNo As TextBox = GVRow.FindControl("txt_Baby_CotNo") '*** Danny Added 18/03/2018
                Dim txtsharing As TextBox = GVRow.FindControl("txtsharing")

                Dim txtEB As TextBox = GVRow.FindControl("txtEB")

                Dim ddlEbType As HtmlSelect = GVRow.FindControl("ddlEbType")



                txtnoofchidauto = GVRow.FindControl("txtnoofchild_AutoCompleteExtender")


                If n > CopyRow And txtrmtypname.Text = "" Then

                    txtrmtypcode.Text = txtrmtypcodenew.Item(CopyRow)
                    txtrmtypname.Text = txtrmtypnamenew.Item(CopyRow)
                    txtmealcode.Text = txtmealcodenew.Item(CopyRow)
                    txtmaxchild.Text = txtmaxchildnew.Item(CopyRow)
                    txtmaxeb.Text = txtmaxebnew.Item(CopyRow)
                    txtnoofchild.Text = txtnoofchildnew.Item(CopyRow)
                    txtfromage.Text = txtfromagenew.Item(CopyRow)
                    txttoage.Text = txttoagenew.Item(CopyRow)
                    txt_Baby_CotNo.Text = txtBabyCotNo.Item(CopyRow)
                    txtsharing.Text = txtsharingnew.Item(CopyRow)
                    txtEB.Text = txtEBnew.Item(CopyRow)
                    ddlEbType.Value = ddlEbTypenew.item(CopyRow)
                    txtnoofchidauto.ContextKey = txtrmtypcode.Text
                    Exit For

                End If
                n = n + 1
            Next
            CopyClick = 0
            ClearArray()

            Call fnCalculateVATValue()
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

        Dim k As Long
        Dim j As Long = 1
        Dim txt As TextBox
        Dim GvRow As GridViewRow
        j = 0
        Dim m As Long
        Dim n As Long = 0
        Dim cnt As Long = grdRoomrates.Columns.Count
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If Not txt Is Nothing Then
                    heading(header) = txt.Text
                End If

            Next
            For Each GvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                        If heading(j) = "RoomTypecode" Or heading(j) = "Select_RoomType" Or heading(j) = "Meal_Plan" Or heading(j) = "MaxChild Allowed" Or heading(j) = "MaxEB Allowed" Or heading(j) = "No.of Children" Or heading(j) = "From Age" Or heading(j) = "To Age" Or heading(j) = "No Of Baby Cot" Or heading(j) = "Charge for Sharing" Or heading(j) = "Charge for EB" And heading(j) <> "CLineno" And heading(j) <> "EBType" Then ''Added 25/01/17
                        Else
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If heading(j) = ddlCopymeal.Value Then
                                    txt.Text = txtfillrate.Text
                                End If
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                        If heading(k) = "RoomTypecode" Or heading(k) = "Select_RoomType" Or heading(k) = "Meal_Plan" Or heading(k) = "MaxChild Allowed" Or heading(k) = "MaxEB Allowed" Or heading(k) = "No.of Children" Or heading(k) = "From Age" Or heading(k) = "To Age" Or heading(k) = "No Of Baby Cot" Or heading(k) = "Charge for Sharing" Or heading(k) = "Charge for EB" And heading(k) <> "CLineno" And heading(k) <> "EBType" Then ''Added 25/01/17en
                        Else
                            ' txt = GvRow.FindControl("txt" & b + a + 3)
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If heading(k) = ddlCopymeal.Value Then
                                    txt.Text = txtfillrate.Text
                                End If
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next

            Call fnCalculateVATValue()
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        'Dim roomcat As String
        'Dim roomcatstring As String
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
        'Dim fnd As Integer = 0
        'Try
        '    For Each GvRow In grdRoomrates.Rows
        '        chk = GvRow.FindControl("ChkSelect")
        '        If chk.Checked = True Then
        '            cnt_checked = cnt_checked + 1
        '        End If
        '    Next
        '    If cnt_checked = 0 Then
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row.');", True)
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
        '                    If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
        '                    Else

        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                arr_room(room) = txt.Text
        '                            End If
        '                        End If
        '                        'pkg = pkg + 1
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
        '                    If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
        '                    Else

        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> Nothing Then
        '                                arr_room(room) = txt.Text
        '                            End If
        '                        End If
        '                        'pkg = pkg + 1
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
        '    Dim validprice As Integer
        '    room = 0
        '    'pkg = 0
        '    n = 0
        '    b = 0
        '    validprice = 0
        '    Dim ds As DataSet
        '    For Each GvRow In grdRoomrates.Rows
        '        If n = 0 Then
        '            For j = 0 To cnt - 8
        '                If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
        '                Else
        '                    ' txt = GvRow.FindControl("txt" & b + a + 3)
        '                    txt = GvRow.FindControl("txt" & j)
        '                    If txt Is Nothing Then
        '                    Else
        '                        txt = GvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            'Check the price exists in the line
        '                            roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
        '                            If heading(room) = roomcatstring Then
        '                                If Val(txt.Text) <> "0" Or txt.Text <> "" Then
        '                                    validprice = 1
        '                                End If
        '                            End If
        '                            roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
        '                            If heading(room) = roomcat Then
        '                                If validprice = 1 Then
        '                                    txt.Text = arr_room(room)
        '                                End If
        '                            End If
        '                        End If
        '                    End If
        '                    room = room + 1
        '                End If
        '            Next
        '            m = j
        '        Else
        '            k = 0
        '            For j = n To (m + n) - 1
        '                If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
        '                Else
        '                    txt = GvRow.FindControl("txt" & j)
        '                    If txt Is Nothing Then
        '                    Else
        '                        'Check the price exists in the line
        '                        roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
        '                        If heading(room) = roomcatstring Then
        '                            If Val(txt.Text) <> "0" Or txt.Text <> "" Then
        '                                validprice = 1
        '                            End If
        '                        End If
        '                        roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
        '                        If heading(room) = roomcat Then
        '                            'If validprice = 1 Then
        '                            If txt.Enabled = True Then
        '                                txt.Text = arr_room(room) + Val(txtfillrate.Value)
        '                                arr_room(room) = txt.Text
        '                            End If
        '                            'End If
        '                        End If
        '                    End If
        '                    room = room + 1
        '                End If
        '                k = k + 1
        '            Next
        '        End If
        '        b = j
        '        n = j
        '        room = 0
        '        validprice = 0
        '    Next

        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function getFillRateType(ByVal prefixText As String) As List(Of String)
        Dim promotionlist As New List(Of String)



        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Try


            strSqlQry = "select distinct namecode from extracodes  where type=2  and namecode like '" & Trim(prefixText) & "%' order by namecode "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    promotionlist.Add(myDS.Tables(0).Rows(i)("namecode").ToString())
                Next

            End If



            Return promotionlist



            'Try
            '    promotionlist.Add("Free")
            '    promotionlist.Add("Incl")
            '    promotionlist.Add("N.Incl")
            '    promotionlist.Add("N/A")
            '    promotionlist.Add("On Request")

            ' Return promotionlist
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

            'partycode = Convert.ToString(HttpContext.Current.Session("partycode").ToString())

            'strSqlQry = "sp_fillchild '" & partycode & "','" & contextKey & "'"

            strSqlQry = "select childs from listofchilds where childs like  '" & Trim(prefixText) & "%' order by childorder"

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


            'roomclasslist.Add("All")
            'roomclasslist.Add("First Child")
            'roomclasslist.Add("Second Child")
            'roomclasslist.Add("Third Child")
            'roomclasslist.Add("Fourth Child")
            'roomclasslist.Add("Fifth Child")
            'roomclasslist.Add("Sixth Child")
            'roomclasslist.Add("Seventh Child")
            'roomclasslist.Add("Eighth Child")
            'roomclasslist.Add("Nineth Child")
            'roomclasslist.Add("Tenth Child")

            Return roomclasslist
        Catch ex As Exception
            Return roomclasslist
        End Try

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







    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click
        ' setadddynamicvalues()


        Try
            rowgridadding("ADD")
            Dim gridNewrow As GridViewRow
            gridNewrow = grdRoomrates.Rows(grdRoomrates.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = grdRoomrates.ClientID
            Dim strFoucsColumnIndex As String = "3"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdRoomrates.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            Call fnCalculateVATValue()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Private Sub rowgridadding(ByVal mode As String)
        '  Dim dt As New DataTable
        Dim dr As DataRow

        Dim cnt1 As Long = 0

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
        strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                  & hdnpartycode.Value & "' "  ' p.rmcatcode " '
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        cnt1 = mySqlCmd.ExecuteScalar
        mySqlConn.Close()

        ReDim arr(cnt1 - 1)
        '  ReDim arrRName(cnt + 1)
        Dim aa As Long = 0

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                    & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader()
        While mySqlReader.Read = True
            arr(aa) = mySqlReader("rmcatcode")
            aa = aa + 1
        End While
        mySqlReader.Close()
        mySqlConn.Close()

        Dim m As Long = 0
        Dim a As Long = cnt - 14 '13 '*** Danny Changed 18/03/2018
        Dim b As Long = 0
        Dim room As Long = 0
        Dim row_id As Long
        Dim j As Long = 0
        Dim n As Long = 0
        Dim k As Long = 0

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
                    Dim txtmaxchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(4).FindControl("txtmaxchild"), TextBox)
                    Dim txtmaxeb As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(5).FindControl("txtmaxeb"), TextBox)
                    Dim txtnoofchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(6).FindControl("txtnoofchild"), TextBox)
                    Dim txtfromage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(7).FindControl("txtfromage"), TextBox)
                    Dim txttoage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(8).FindControl("txttoage"), TextBox)
                    Dim txt_Baby_CotNo As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(9).FindControl("txt_Baby_CotNo"), TextBox) '*** Danny Added 18/03/2018
                    Dim txtsharing As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(9).FindControl("txtsharing"), TextBox)

                    Dim txtEB As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(10).FindControl("txtEB"), TextBox)

                    Dim lblCLineno As Label = DirectCast(grdRoomrates.Rows(rowIndex).Cells(10).FindControl("lblCLineno"), Label)  '' Added 25/01/17
                    Dim ddlEbType As HtmlSelect = DirectCast(grdRoomrates.Rows(rowIndex).Cells(10).FindControl("ddlEbType"), HtmlSelect)


                    drCurrentRow = dt.NewRow()



                    dt.Rows(i - 1)("RoomTypecode") = txtrmtypcode.Text
                    dt.Rows(i - 1)("Select_RoomType") = txtrmtypname.Text
                    dt.Rows(i - 1)("Meal_Plan") = txtmealcode.Text
                    dt.Rows(i - 1)("MaxChild Allowed") = txtmaxchild.Text
                    dt.Rows(i - 1)("MaxEB Allowed") = txtmaxeb.Text
                    dt.Rows(i - 1)("No.of Children") = txtnoofchild.Text
                    dt.Rows(i - 1)("From Age") = txtfromage.Text
                    dt.Rows(i - 1)("To Age") = txttoage.Text
                    dt.Rows(i - 1)("No Of Baby Cot") = txt_Baby_CotNo.Text '*** Danny Added 18/03/2018
                    dt.Rows(i - 1)("Charge for Sharing") = txtsharing.Text
                    dt.Rows(i - 1)("Charge for EB") = txtEB.Text
                    dt.Rows(i - 1)("CLineno") = lblCLineno.Text '' Added 25/01/17
                    dt.Rows(i - 1)("EBType") = ddlEbType.Value


                    '''''''''''''''''''''bining dynamic fields

                    Dim gvRow As GridViewRow = grdRoomrates.Rows(rowIndex)


                    Dim rmcatcode As String = ""
                    Dim value As String = ""

                    Dim rmtypecode As String = ""
                    Dim mealcode As String = ""
                    Dim headerlabel As New TextBox

                    Dim txt As TextBox
                    Dim cnt As Long = 0
                    Dim header As Long = 0
                    cnt = grdRoomrates.Columns.Count
                    Dim heading(cnt + 1) As String
                    For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                        If txt.Text <> Nothing Then
                            heading(header) = txt.Text
                        End If
                    Next

                    Dim arr_room(header + 1) As String

                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        '*** Danny Added 18/03/2018 (And heading(j) <> "No Of Baby Cot")
                        If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "CLineno" And heading(j) <> "EBType" Then  '' Added 25/01/17

                            For s = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)


                                If headerlabel.Text <> Nothing Then
                                    For lAi = 0 To arr.GetUpperBound(0)
                                        If headerlabel.Text = arr(lAi) Then
                                            If gvRow.RowIndex = 0 Then
                                                txt = gvRow.FindControl("txt" & s)
                                            Else
                                                'txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvRow.RowIndex) + s + gvRow.RowIndex)
                                                txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 14) * gvRow.RowIndex) + s + gvRow.RowIndex) '13 '*** Danny Changed 18/03/2018
                                            End If

                                            dt.Rows(i - 1)(arr(lAi)) = txt.Text


                                        End If

                                    Next




                                End If
                            Next
                        End If
                    Next
go1:


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
    Private Sub Setdeletebinddata()
        '  Dim dt As New DataTable
        Dim dr As DataRow

        Dim cnt1 As Long = 0

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
        strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                  & hdnpartycode.Value & "' "  ' p.rmcatcode " '
        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        cnt1 = mySqlCmd.ExecuteScalar
        mySqlConn.Close()

        ReDim arr(cnt1 - 1)
        '  ReDim arrRName(cnt + 1)
        Dim aa As Long = 0

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='L' and prc.partycode='" _
                    & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        mySqlReader = mySqlCmd.ExecuteReader()
        While mySqlReader.Read = True
            arr(aa) = mySqlReader("rmcatcode")
            aa = aa + 1
        End While
        mySqlReader.Close()
        mySqlConn.Close()

        Dim m As Long = 0
        Dim a As Long = cnt - 14 '13 '*** Danny Changed 18/03/2018
        Dim b As Long = 0
        Dim room As Long = 0
        Dim row_id As Long
        Dim j As Long = 0
        Dim n As Long = 0
        Dim k As Long = 0

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
                    Dim txtmaxchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(4).FindControl("txtmaxchild"), TextBox)
                    Dim txtmaxeb As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(5).FindControl("txtmaxeb"), TextBox)
                    Dim txtnoofchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(6).FindControl("txtnoofchild"), TextBox)
                    Dim txtfromage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(7).FindControl("txtfromage"), TextBox)
                    Dim txttoage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(8).FindControl("txttoage"), TextBox)
                    Dim txt_Baby_CotNo As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(9).FindControl("txt_Baby_CotNo"), TextBox) '*** Danny Added 18/03/2018
                    Dim txtsharing As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(10).FindControl("txtsharing"), TextBox)

                    Dim txtEB As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(11).FindControl("txtEB"), TextBox)
                    Dim lblCLineno As Label = DirectCast(grdRoomrates.Rows(rowIndex).Cells(12).FindControl("lblCLineno"), Label)  '' Added 25/01/17
                    Dim ddlEbType As HtmlSelect = DirectCast(grdRoomrates.Rows(rowIndex).FindControl("ddlEbType"), HtmlSelect)


                    '   drCurrentRow = dt.NewRow()



                    dt.Rows(i - 1)("RoomTypecode") = txtrmtypcode.Text
                    dt.Rows(i - 1)("Select_RoomType") = txtrmtypname.Text
                    dt.Rows(i - 1)("Meal_Plan") = txtmealcode.Text
                    dt.Rows(i - 1)("MaxChild Allowed") = txtmaxchild.Text
                    dt.Rows(i - 1)("MaxEB Allowed") = txtmaxeb.Text
                    dt.Rows(i - 1)("No.of Children") = txtnoofchild.Text
                    dt.Rows(i - 1)("From Age") = txtfromage.Text
                    dt.Rows(i - 1)("To Age") = txttoage.Text
                    dt.Rows(i - 1)("No Of Baby Cot") = txt_Baby_CotNo.Text '*** Danny Added 18/03/2018
                    dt.Rows(i - 1)("Charge for Sharing") = txtsharing.Text
                    dt.Rows(i - 1)("Charge for EB") = txtEB.Text
                    dt.Rows(i - 1)("CLineno") = lblCLineno.Text '' Added 25/01/17
                    dt.Rows(i - 1)("EBType") = ddlEbType.Value


                    '''''''''''''''''''''bining dynamic fields

                    Dim gvRow As GridViewRow = grdRoomrates.Rows(rowIndex)


                    Dim rmcatcode As String = ""
                    Dim value As String = ""

                    Dim rmtypecode As String = ""
                    Dim mealcode As String = ""
                    Dim headerlabel As New TextBox

                    Dim txt As TextBox
                    Dim cnt As Long = 0
                    Dim header As Long = 0
                    cnt = grdRoomrates.Columns.Count
                    Dim heading(cnt + 1) As String
                    For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                        If txt.Text <> Nothing Then
                            heading(header) = txt.Text
                        End If
                    Next

                    Dim arr_room(header + 1) As String

                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        '*** Danny Added 18/03/2018 (And heading(j) <> "No Of Baby Cot")
                        If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "CLineno" And heading(j) <> "EBType" Then

                            For s = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)


                                If headerlabel.Text <> Nothing Then
                                    For lAi = 0 To arr.GetUpperBound(0)
                                        If headerlabel.Text = arr(lAi) Then
                                            If gvRow.RowIndex = 0 Then
                                                txt = gvRow.FindControl("txt" & s)
                                            Else
                                                'txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvRow.RowIndex) + s + gvRow.RowIndex)
                                                txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 14) * gvRow.RowIndex) + s + gvRow.RowIndex) '13 '*** Danny Changed 18/03/2018
                                            End If

                                            dt.Rows(i - 1)(arr(lAi)) = txt.Text


                                        End If

                                    Next




                                End If
                            Next
                        End If
                    Next
go1:


                    rowIndex += 1
                Next
                ' dt.Rows.Add(drCurrentRow)
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
                    Dim txtmaxchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxchild"), TextBox)
                    Dim txtmaxeb As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxeb"), TextBox)
                    Dim txtnoofchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnoofchild"), TextBox)
                    Dim txtfromage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtfromage"), TextBox)
                    Dim txttoage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txttoage"), TextBox)
                    Dim txt_Baby_CotNo As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txt_Baby_CotNo"), TextBox) '*** Danny Added 18/03/2018
                    Dim txtsharing As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtsharing"), TextBox)

                    Dim txtEB As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtEB"), TextBox)
                    Dim lblCLineno As Label = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("lblCLineno"), Label)  '' Added 25/01/17

                    Dim ddlEbType As HtmlSelect = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("ddlEbType"), HtmlSelect)

                    txtrmtypcode.Text = dt.Rows(i)("RoomTypecode").ToString()
                    txtrmtypname.Text = dt.Rows(i)("Select_RoomType").ToString()
                    txtmealcode.Text = dt.Rows(i)("Meal_Plan").ToString()
                    txtmaxchild.Text = dt.Rows(i)("MaxChild Allowed").ToString()
                    txtmaxeb.Text = dt.Rows(i)("MaxEB Allowed").ToString()
                    txtnoofchild.Text = dt.Rows(i)("No.of Children").ToString()
                    txtfromage.Text = dt.Rows(i)("From Age").ToString()
                    txttoage.Text = dt.Rows(i)("To Age").ToString()
                    txt_Baby_CotNo.Text = dt.Rows(i)("No Of Baby Cot").ToString() '*** Danny Added 18/03/2018
                    txtsharing.Text = dt.Rows(i)("Charge for Sharing").ToString()
                    txtEB.Text = dt.Rows(i)("Charge for EB").ToString()
                    lblCLineno.Text = dt.Rows(i)("CLineno").ToString() '' Added 25/01/17
                    ddlEbType.Value = dt.Rows(i)("EBType").ToString()




                    rowIndex += 1
                Next
            End If
        End If
    End Sub

    'commented by 'changed by mohamed on 26/02/2018
    '    Private Sub SetgridbindDataoffers(ByVal plistcode As String)
    '        Dim rowIndex As Integer = 0
    '        If Session("GV_HotelData") IsNot Nothing Then
    '            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
    '            If dt.Rows.Count > 0 Then
    '                For i As Integer = 0 To dt.Rows.Count - 1
    '                    Dim txtrmtypcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypcode"), TextBox)
    '                    Dim txtrmtypname As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypname"), TextBox)
    '                    Dim txtmealcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmealcode"), TextBox)
    '                    Dim txtmaxchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxchild"), TextBox)
    '                    Dim txtmaxeb As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxeb"), TextBox)
    '                    Dim txtnoofchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnoofchild"), TextBox)
    '                    Dim txtfromage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtfromage"), TextBox)
    '                    Dim txttoage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txttoage"), TextBox)
    '                    Dim txtsharing As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtsharing"), TextBox)

    '                    Dim txtEB As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtEB"), TextBox)
    '                    Dim lblCLineno As Label = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("lblCLineno"), Label) '' Added 25/01/17
    '                    Dim ddlEbType As HtmlSelect = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("ddlEbType"), HtmlSelect)



    '                    txtrmtypcode.Text = dt.Rows(i)("RoomTypecode").ToString()
    '                    txtrmtypname.Text = dt.Rows(i)("Select_RoomType").ToString()
    '                    txtmealcode.Text = dt.Rows(i)("Meal_Plan").ToString()
    '                    txtmaxchild.Text = dt.Rows(i)("MaxChild Allowed").ToString()
    '                    txtmaxeb.Text = dt.Rows(i)("MaxEB Allowed").ToString()
    '                    ' txtnoofchild.Text = dt.Rows(i)("No.of Children").ToString()
    '                    txtfromage.Text = DecRound(dt.Rows(i)("From Age").ToString())
    '                    txttoage.Text = dt.Rows(i)("To Age").ToString()

    '                    txtEB.Text = dt.Rows(i)("Charge for EB").ToString()
    '                    txtsharing.Text = dt.Rows(i)("Charge for Sharing").ToString()
    '                    lblCLineno.Text = dt.Rows(i)("CLineno").ToString()  '' Added 25/01/17
    '                    ddlEbType.Value = dt.Rows(i)("EBType").ToString()

    '                    Select Case DecRound(dt.Rows(i)("Charge for Sharing").ToString())
    '                        Case "-3"
    '                            txtsharing.Text = "Free"
    '                        Case "-1"
    '                            txtsharing.Text = "Incl"
    '                        Case "-2"
    '                            txtsharing.Text = "N.Incl"
    '                        Case "-4"
    '                            txtsharing.Text = "N/A"
    '                        Case "-5"
    '                            txtsharing.Text = "On Request"
    '                    End Select

    '                    Select Case DecRound(dt.Rows(i)("Charge for EB").ToString())
    '                        Case "-3"
    '                            txtEB.Text = "Free"
    '                        Case "-1"
    '                            txtEB.Text = "Incl"
    '                        Case "-2"
    '                            txtEB.Text = "N.Incl"
    '                        Case "-4"
    '                            txtEB.Text = "N/A"
    '                        Case "-5"
    '                            txtEB.Text = "On Request"
    '                    End Select



    '                    If IsDBNull(dt.Rows(i)("No.of Children").ToString()) = False Then

    '                        Select Case dt.Rows(i)("No.of Children").ToString()
    '                            Case 0
    '                                txtnoofchild.Text = "All"
    '                            Case 1
    '                                txtnoofchild.Text = "First Child"
    '                            Case 2
    '                                txtnoofchild.Text = "Second Child"
    '                            Case 3
    '                                txtnoofchild.Text = "Third Child"
    '                            Case 4
    '                                txtnoofchild.Text = "Fourth Child"
    '                            Case 5
    '                                txtnoofchild.Text = "Fifth Child"
    '                            Case 6
    '                                txtnoofchild.Text = "Sixth Child"
    '                            Case 7
    '                                txtnoofchild.Text = "Seventh Child"
    '                            Case 8
    '                                txtnoofchild.Text = "Eighth Child"
    '                            Case 9
    '                                txtnoofchild.Text = "Nineth Child"
    '                            Case 10
    '                                txtnoofchild.Text = "Tenth Child"

    '                        End Select
    '                    End If



    '                    Dim gvRow As GridViewRow = grdRoomrates.Rows(rowIndex)
    '                    Dim myConn As New SqlConnection
    '                    Dim myCmd As New SqlCommand
    '                    Dim myReader As SqlDataReader
    '                    Dim StrQryTemp As String
    '                    Dim cnt1 As Long = 0

    '                    Dim rmcatcode As String = ""
    '                    Dim value As String = ""
    '                    Dim Linno As Integer
    '                    Dim rmtypecode As String = ""
    '                    Dim mealcode As String = ""
    '                    Dim headerlabel As New TextBox

    '                    Dim txt As TextBox
    '                    Dim cnt As Long = 0
    '                    Dim header As Long = 0
    '                    Dim rmcatcount As Long = grdRoomrates.Columns.Count - 13
    '                    cnt = grdRoomrates.Columns.Count
    '                    Dim heading(cnt + 1) As String
    '                    For header = 0 To cnt - 13
    '                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
    '                        If txt Is Nothing Then txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header + rmcatcount + 1)
    '                        If txt.Text <> Nothing Then
    '                            heading(header) = txt.Text
    '                        End If
    '                    Next


    '                    'If Session("GV_HotelData") IsNot Nothing Then
    '                    '    Dim dt1 As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
    '                    '    For Each column As DataColumn In dt.Columns
    '                    '        If column.ColumnName = "RoomTypecode" Or column.ColumnName = "Select_RoomType" Or column.ColumnName = "Meal_Plan" Or column.ColumnName = "MaxChild Allowed" Or column.ColumnName = "MaxEB Allowed" Or column.ColumnName = "No.of Children" Or column.ColumnName = "From Age" Or column.ColumnName = "To Age" Or column.ColumnName = "Charge for Sharing" Or column.ColumnName = "Charge for EB" Or column.ColumnName = "EBType" Or column.ColumnName = "CLineno" Then
    '                    '        Else
    '                    '            heading(header) = column.ColumnName
    '                    '        End If
    '                    '    Next
    '                    'End If

    '                    'StrQryTemp = "select * from dbo.fn_childrmcatgory('" & txtplistcode.Text & "'," & i + 1 & " )"

    '                    StrQryTemp = "select * from dbo.fn_childrmcatgory('" & plistcode & "'," & lblCLineno.Text & " )"
    '                    myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '                    myCmd = New SqlCommand(StrQryTemp, myConn)
    '                    myReader = myCmd.ExecuteReader
    '                    If myReader.HasRows Then
    '                        While myReader.Read
    '                            If IsDBNull(myReader("rmcatcode")) = False Then
    '                                rmcatcode = myReader("rmcatcode")
    '                            End If
    '                            If IsDBNull(myReader("value")) = False Then
    '                                value = myReader("value")
    '                            Else
    '                                value = ""
    '                            End If

    '                            For j = 0 To cnt - 13
    '                                If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "CLineno" And heading(j) <> "EBType" Then  '' Added 25/01/17

    '                                    For s = 0 To grdRoomrates.Columns.Count - 13
    '                                        headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
    '                                        If headerlabel Is Nothing Then headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s + rmcatcount + 1)
    '                                        If headerlabel.Text <> Nothing Then
    '                                            If headerlabel.Text = rmcatcode Then
    '                                                If gvRow.RowIndex = 0 Then
    '                                                    txt = gvRow.FindControl("txt" & s)
    '                                                    If txt Is Nothing Then txt = gvRow.FindControl("txt" & s + grdRoomrates.Columns.Count)
    '                                                Else
    '                                                    txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvRow.RowIndex) + s + gvRow.RowIndex)
    '                                                    If txt Is Nothing Then txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvRow.RowIndex) + s + grdRoomrates.Columns.Count + gvRow.RowIndex)
    '                                                End If
    '                                                If txt Is Nothing Then
    '                                                Else
    '                                                    If value = "" Then
    '                                                        txt.Text = ""
    '                                                    Else
    '                                                        Select Case DecRound(value)
    '                                                            Case "-3"
    '                                                                value = "Free"
    '                                                            Case "-1"
    '                                                                value = "Incl"
    '                                                            Case "-2"
    '                                                                value = "N.Incl"
    '                                                            Case "-4"
    '                                                                value = "N/A"
    '                                                            Case "-5"
    '                                                                value = "On Request"
    '                                                        End Select

    '                                                        txt.Text = value
    '                                                    End If
    '                                                End If

    '                                                GoTo go1
    '                                            End If
    '                                        End If
    '                                    Next
    '                                End If
    '                            Next
    'go1:
    '                        End While
    '                    End If

    '                    clsDBConnect.dbConnectionClose(myConn)
    '                    clsDBConnect.dbCommandClose(myCmd)
    '                    clsDBConnect.dbReaderClose(myReader)

    '                    rowIndex += 1
    '                Next
    '            End If
    '        End If
    '    End Sub
    Private Sub SetgridbindData()
        Dim rowIndex As Integer = 0
        If Session("GV_HotelData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtrmtypcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypcode"), TextBox)
                    Dim txtrmtypname As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtrmtypname"), TextBox)
                    Dim txtmealcode As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmealcode"), TextBox)
                    Dim txtmaxchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxchild"), TextBox)
                    Dim txtmaxeb As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtmaxeb"), TextBox)
                    Dim txtnoofchild As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnoofchild"), TextBox)
                    Dim txtfromage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtfromage"), TextBox)
                    Dim txttoage As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txttoage"), TextBox)
                    Dim txt_Baby_CotNo As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txt_Baby_CotNo"), TextBox) '*** Danny Added 18/03/2018 
                    Dim txtsharing As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtsharing"), TextBox)

                    Dim txtEB As TextBox = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtEB"), TextBox)
                    Dim lblCLineno As Label = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("lblCLineno"), Label) '' Added 25/01/17
                    Dim ddlEbType As HtmlSelect = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("ddlEbType"), HtmlSelect)




                    txtrmtypcode.Text = dt.Rows(i)("RoomTypecode").ToString()
                    txtrmtypname.Text = dt.Rows(i)("Select_RoomType").ToString()
                    txtmealcode.Text = dt.Rows(i)("Meal_Plan").ToString()
                    txtmaxchild.Text = dt.Rows(i)("MaxChild Allowed").ToString()
                    txtmaxeb.Text = dt.Rows(i)("MaxEB Allowed").ToString()
                    ' txtnoofchild.Text = dt.Rows(i)("No.of Children").ToString()
                    txtfromage.Text = DecRound(dt.Rows(i)("From Age").ToString())
                    txttoage.Text = dt.Rows(i)("To Age").ToString()
                    txt_Baby_CotNo.Text = dt.Rows(i)("No Of Baby Cot").ToString() '*** Danny Added 18/03/2018
                    txtEB.Text = dt.Rows(i)("Charge for EB").ToString()
                    txtsharing.Text = dt.Rows(i)("Charge for Sharing").ToString()
                    lblCLineno.Text = dt.Rows(i)("CLineno").ToString()  '' Added 25/01/17
                    ddlEbType.Value = dt.Rows(i)("EBType").ToString()

                    Dim txtnoofchidauto As AjaxControlToolkit.AutoCompleteExtender

                    txtnoofchidauto = DirectCast(grdRoomrates.Rows(rowIndex).Cells(1).FindControl("txtnoofchild_AutoCompleteExtender"), AjaxControlToolkit.AutoCompleteExtender)
                    txtnoofchidauto.ContextKey = txtrmtypcode.Text

                    Select Case DecRound(dt.Rows(i)("Charge for Sharing").ToString())
                        Case "-3"
                            txtsharing.Text = "Free"
                        Case "-1"
                            txtsharing.Text = "Incl"
                        Case "-2"
                            txtsharing.Text = "N.Incl"
                        Case "-4"
                            txtsharing.Text = "N/A"
                        Case "-5"
                            txtsharing.Text = "On Request"
                    End Select

                    Select Case DecRound(dt.Rows(i)("Charge for EB").ToString())
                        Case "-3"
                            txtEB.Text = "Free"
                        Case "-1"
                            txtEB.Text = "Incl"
                        Case "-2"
                            txtEB.Text = "N.Incl"
                        Case "-4"
                            txtEB.Text = "N/A"
                        Case "-5"
                            txtEB.Text = "On Request"
                    End Select



                    If IsDBNull(dt.Rows(i)("No.of Children").ToString()) = False Then

                        Select Case dt.Rows(i)("No.of Children").ToString()
                            Case 0
                                txtnoofchild.Text = "All"
                            Case 1
                                txtnoofchild.Text = "First Child"
                            Case 2
                                txtnoofchild.Text = "Second Child"
                            Case 3
                                txtnoofchild.Text = "Third Child"
                            Case 4
                                txtnoofchild.Text = "Fourth Child"
                            Case 5
                                txtnoofchild.Text = "Fifth Child"
                            Case 6
                                txtnoofchild.Text = "Sixth Child"
                            Case 7
                                txtnoofchild.Text = "Seventh Child"
                            Case 8
                                txtnoofchild.Text = "Eighth Child"
                            Case 9
                                txtnoofchild.Text = "Nineth Child"
                            Case 10
                                txtnoofchild.Text = "Tenth Child"

                        End Select
                    End If



                    Dim gvRow As GridViewRow = grdRoomrates.Rows(rowIndex)
                    Dim myConn As New SqlConnection
                    Dim myCmd As New SqlCommand
                    Dim myReader As SqlDataReader
                    Dim StrQryTemp As String
                    Dim cnt1 As Long = 0

                    Dim rmcatcode As String = ""
                    Dim value As String = ""
                    Dim Linno As Integer
                    Dim rmtypecode As String = ""
                    Dim mealcode As String = ""
                    Dim headerlabel As New TextBox

                    Dim txt As TextBox
                    Dim cnt As Long = 0
                    Dim header As Long = 0
                    cnt = grdRoomrates.Columns.Count
                    Dim heading(cnt + 1) As String
                    For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                        If txt.Text <> Nothing Then
                            heading(header) = txt.Text
                        End If
                    Next


                    'StrQryTemp = "select * from dbo.fn_childrmcatgory('" & txtplistcode.Text & "'," & i + 1 & " )"

                    StrQryTemp = "select * from dbo.fn_childrmcatgory('" & txtplistcode.Text & "'," & lblCLineno.Text & " )"
                    myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myCmd = New SqlCommand(StrQryTemp, myConn)
                    myReader = myCmd.ExecuteReader
                    If myReader.HasRows Then
                        While myReader.Read
                            If IsDBNull(myReader("rmcatcode")) = False Then
                                rmcatcode = myReader("rmcatcode")
                            End If
                            If IsDBNull(myReader("value")) = False Then
                                value = myReader("value")
                            Else
                                value = ""
                            End If

                            For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                                '*** Danny Added 18/03/2018 (And heading(j) <> "No Of Baby Cot")
                                If heading(j) <> "RoomTypecode" And heading(j) <> "Select_RoomType" And heading(j) <> "Meal_Plan" And heading(j) <> "MaxChild Allowed" And heading(j) <> "MaxEB Allowed" And heading(j) <> "No.of Children" And heading(j) <> "From Age" And heading(j) <> "To Age" And heading(j) <> "No Of Baby Cot" And heading(j) <> "Charge for Sharing" And heading(j) <> "Charge for EB" And heading(j) <> "CLineno" And heading(j) <> "EBType" Then  '' Added 25/01/17

                                    For s = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                                        headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                        If headerlabel.Text <> Nothing Then
                                            If headerlabel.Text = rmcatcode Then
                                                If gvRow.RowIndex = 0 Then
                                                    txt = gvRow.FindControl("txt" & s)
                                                Else
                                                    'txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 13) * gvRow.RowIndex) + s + gvRow.RowIndex)
                                                    txt = gvRow.FindControl("txt" & ((grdRoomrates.Columns.Count - 14) * gvRow.RowIndex) + s + gvRow.RowIndex) '13 '*** Danny Changed 18/03/2018
                                                End If
                                                If txt Is Nothing Then
                                                Else
                                                    If value = "" Then
                                                        txt.Text = ""
                                                    Else
                                                        Select Case DecRound(value)
                                                            Case "-3"
                                                                value = "Free"
                                                            Case "-1"
                                                                value = "Incl"
                                                            Case "-2"
                                                                value = "N.Incl"
                                                            Case "-4"
                                                                value = "N/A"
                                                            Case "-5"
                                                                value = "On Request"
                                                        End Select

                                                        txt.Text = value
                                                    End If
                                                End If

                                                GoTo go1
                                            End If
                                        End If
                                    Next
                                End If
                            Next
go1:
                        End While
                    End If

                    clsDBConnect.dbConnectionClose(myConn)
                    clsDBConnect.dbCommandClose(myCmd)
                    clsDBConnect.dbReaderClose(myReader)

                    rowIndex += 1
                Next
            End If

            Call fnCalculateVATValue()
        End If
    End Sub
    Private Sub setdynamicvalues()
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow
        'If Session("GV_HotelData") Is Nothing = True Then Exit Sub
        'Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String

        For header = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            heading(header) = IIf(txt.Text = Nothing, "", txt.Text)
        Next

        'For header = 0 To grdRoomrates.HeaderRow.Cells.Count - 12
        '    txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
        '    heading(header) = txt.Text
        '    'heading(header) = grdRoomrates.HeaderRow.Cells(header).Text
        'Next

        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 14 '13 '*** Danny Changed 18/03/2018
        Dim b As Long = 0



        Dim chk As CheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try



            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        If chk.Checked = True Then
                            row_id = GvRow.RowIndex
                            '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                            If heading(j) = "RoomTypecode" Or heading(j) = "Select_RoomType" Or heading(j) = "Meal_Plan" Or heading(j) = "MaxChild Allowed" Or heading(j) = "MaxEB Allowed" Or heading(j) = "No.of Children" Or heading(j) = "From Age" Or heading(j) = "To Age" Or heading(j) = "No Of Baby Cot" Or heading(j) = "Charge for Sharing" Or heading(j) = "Charge for EB" Or heading(j) = "EBType" Or heading(j) = "CLineno" Then

                            Else
                                If pkg = 0 Then
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            arr_pkg(pkg) = txt.Text
                                        End If
                                    End If

                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            arr_cancdays(pkg) = txt.Text
                                        End If
                                    End If

                                End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                pkg = pkg + 1
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
                            '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                            If heading(k) = "RoomTypecode" Or heading(k) = "Select_RoomType" Or heading(k) = "Meal_Plan" Or heading(k) = "MaxChild Allowed" Or heading(k) = "MaxEB Allowed" Or heading(k) = "No.of Children" Or heading(k) = "From Age" Or heading(k) = "To Age" Or heading(k) = "No Of Baby Cot" Or heading(k) = "Charge for Sharing" Or heading(k) = "Charge for EB" Or heading(k) = "EBType" Or heading(k) = "CLineno" Then
                            Else
                                If pkg = 0 Then
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            arr_pkg(pkg) = txt.Text
                                        End If
                                    End If

                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            arr_cancdays(pkg) = txt.Text
                                        End If
                                    End If
                                End If
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If
                                pkg = pkg + 1
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
            room = 0
            pkg = 0
            n = 0
            b = 0



            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                If n = 0 Then
                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018

                        If txtrmtypcode.Text = "" Then
                            ' If GvRow.RowIndex = row_id + 1 Then
                            '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                            If heading(j) = "RoomTypecode" Or heading(j) = "Select_RoomType" Or heading(j) = "Meal_Plan" Or heading(j) = "MaxChild Allowed" Or heading(j) = "MaxEB Allowed" Or heading(j) = "No.of Children" Or heading(j) = "From Age" Or heading(j) = "To Age" Or heading(j) = "No Of Baby Cot" Or heading(j) = "Charge for Sharing" Or heading(j) = "Charge for EB" Or heading(j) = "EBType" Or heading(j) = "CLineno" Then
                            Else
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_pkg(pkg)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_cancdays(pkg)
                                    End If
                                End If
                                room = room + 1
                            End If
                            'End If
                        End If

                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If txtrmtypcode.Text = "" Then
                            'If GvRow.RowIndex = row_id + 1 Then
                            '*** Danny Added 18/03/2018 (Or heading(j) = "No Of Baby Cot")
                            If heading(k) = "RoomTypecode" Or heading(k) = "Select_RoomType" Or heading(k) = "Meal_Plan" Or heading(k) = "MaxChild Allowed" Or heading(k) = "MaxEB Allowed" Or heading(k) = "No.of Children" Or heading(k) = "From Age" Or heading(k) = "To Age" Or heading(k) = "No Of Baby Cot" Or heading(k) = "Charge for Sharing" Or heading(k) = "Charge for EB" Or heading(k) = "EBType" Or heading(k) = "CLineno" Then
                            Else
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_pkg(pkg)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_cancdays(pkg)
                                    End If
                                End If
                                room = room + 1
                            End If
                            ' End If
                        End If

                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Private Sub setadddynamicvalues()
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow
        'If Session("GV_HotelData") Is Nothing = True Then Exit Sub
        'Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String


        If Session("GV_HotelData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
            For Each column As DataColumn In dt.Columns
                '*** Danny Added 18/03/2018 (Or column.ColumnName = "No Of Baby Cot")
                If column.ColumnName = "RoomTypecode" Or column.ColumnName = "Select_RoomType" Or column.ColumnName = "Meal_Plan" Or column.ColumnName = "MaxChild Allowed" Or column.ColumnName = "MaxEB Allowed" Or column.ColumnName = "No.of Children" Or column.ColumnName = "From Age" Or column.ColumnName = "To Age" Or column.ColumnName = "No Of Baby Cot" Or column.ColumnName = "Charge for Sharing" Or column.ColumnName = "Charge for EB" Or column.ColumnName = "EBType" Or column.ColumnName = "CLineno" Then
                Else
                    heading(header) = column.ColumnName
                End If
            Next
        End If

        'For header = 0 To cnt - 12
        '    'txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)

        '    'heading(header) = IIf(txt.Text = Nothing, "", txt.Text)
        'Next

        'For header = 0 To grdRoomrates.HeaderRow.Cells.Count - 12
        '    txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
        '    heading(header) = txt.Text
        '    'heading(header) = grdRoomrates.HeaderRow.Cells(header).Text
        'Next

        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 14 '13 '*** Danny Changed 18/03/2018
        Dim b As Long = 0



        Dim chk As CheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try



            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        'If chk.Checked = True Then
                        row_id = GvRow.RowIndex
                        '*** Danny Added (Or heading(header) = "No Of Baby Cot")
                        If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "No Of Baby Cot" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Or heading(header) = "EBType" Or heading(header) = "CLineno" Then

                        Else
                            If pkg = 0 Then
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_pkg(pkg) = txt.Text
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_cancdays(pkg) = txt.Text
                                    End If
                                End If

                            End If
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    arr_room(room) = txt.Text
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                        End If
                        ' End If
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    pkg = 0
                    For j = n To (m + n) - 1
                        'If chk.Checked = True Then
                        row_id = GvRow.RowIndex
                        '*** Danny Added 18/03/2018 (Or heading(header) = "No Of Baby Cot")
                        If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "No Of Baby Cot" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Or heading(header) = "EBType" Or heading(header) = "CLineno" Then
                        Else
                            If pkg = 0 Then
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_pkg(pkg) = txt.Text
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        arr_cancdays(pkg) = txt.Text
                                    End If
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    arr_room(room) = txt.Text
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                        End If
                        ' End If
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next
            '--------------------------------------------------------------------------------------------
            'Noe Fill Record to Cell
            room = 0
            pkg = 0
            n = 0
            b = 0

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If n = 0 Then
                    For j = 0 To cnt - 14 '13 '*** Danny Changed 18/03/2018
                        If GvRow.RowIndex = row_id + 1 Then
                            '*** Danny Added 18/03/2018 (Or heading(header) = "No Of Baby Cot")
                            If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "No Of Baby Cot" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Or heading(header) = "EBType" Or heading(header) = "CLineno" Then
                            Else
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_pkg(pkg)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_cancdays(pkg)
                                    End If
                                End If
                                room = room + 1
                            End If
                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If GvRow.RowIndex = row_id + 1 Then
                            '*** Danny Added 18/03/2018 (Or heading(header) = "No Of Baby Cot")
                            If heading(header) = "RoomTypecode" Or heading(header) = "Select_RoomType" Or heading(header) = "Meal_Plan" Or heading(header) = "MaxChild Allowed" Or heading(header) = "MaxEB Allowed" Or heading(header) = "No.of Children" Or heading(header) = "From Age" Or heading(header) = "To Age" Or heading(header) = "No Of Baby Cot" Or heading(header) = "Charge for Sharing" Or heading(header) = "Charge for EB" Or heading(header) = "EBType" Or heading(header) = "CLineno" Then
                            Else
                                txt = GvRow.FindControl("txt" & b + a + 1)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_pkg(pkg)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & b + a + 2)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        txt.Text = arr_cancdays(pkg)
                                    End If
                                End If
                                room = room + 1
                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        'rowgridadding("DELETE")
        If Session("GV_HotelData") IsNot Nothing Then
            Dim dt As DataTable = DirectCast(Session("GV_HotelData"), DataTable)
            Dim drCurrentRow As DataRow = Nothing

            Dim rowIndex As Integer = 0

            For Each gvRow In grdRoomrates.Rows

                Dim chkSelect As CheckBox = gvRow.FindControl("chkSelect")

                If chkSelect.Checked = True Then
                    rowIndex = Convert.ToInt32(gvRow.RowIndex)
                    Exit For
                End If

            Next
            If dt.Rows.Count > 1 Then

                Setdeletebinddata()
                dt.Rows.Remove(dt.Rows(rowIndex))
                drCurrentRow = dt.NewRow()
                Session("GV_HotelData") = dt
                grdRoomrates.DataSource = dt
                grdRoomrates.DataBind()




                'For i As Integer = 0 To grdRoomrates.Rows.Count - 2
                '    grdRoomrates.Rows(i).Cells(0).Text = Convert.ToString(i + 1)
                'Next
                SetOldData()
                ' SetgridbindData()
            End If

            Call fnCalculateVATValue()
        End If
    End Sub




    Protected Sub btnRemark_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemark.Click
        'If ValidatePage() = False Then
        '    Exit Sub
        'End If

        GenRemark()
    End Sub
#Region "Public Sub GenRemark()"
    Public Sub GenRemark()

        Dim strRemark As String = ""
        Dim txtFAge As HtmlInputText
        Dim txtTAge As HtmlInputText
        Dim ddlShare As HtmlSelect
        Dim ddlChild As HtmlSelect
        Dim ddlRmCatCode As HtmlSelect
        Dim ddlMlPlan As HtmlSelect
        Dim ddlMlType As HtmlSelect
        Dim ddlChild2ndabove As HtmlSelect
        Dim ddlRoomCategory2child As HtmlSelect
        Dim dpFromDate As EclipseWebSolutions.DatePicker.DatePicker
        Dim dpToDate As EclipseWebSolutions.DatePicker.DatePicker
        Dim roomtype As String
        Dim roomcategory As String
        Dim txtmealplan As TextBox
        Dim txtEBmelacode As TextBox
        Dim ddlChild2ndEBabove As HtmlSelect
        Dim ddlRoomCategory2EBchild As HtmlSelect
        Dim flgEB As Boolean = True


        strRemark = ""

        ' strRemark = strRemark & "For" & " " & (roomtype) & " -- occupancies" & " " & roomcategory & vbCrLf
        ' - "occupancies" & (roomcategory)
        If flgEB = True Then
            For Each gvRow In grdRoomrates.Rows

                Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
                Dim txtrmtypname As TextBox = gvRow.FindControl("txtrmtypname")
                Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
                Dim txtmaxchild As TextBox = gvRow.FindControl("txtmaxchild")
                Dim txtmaxeb As TextBox = gvRow.FindControl("txtmaxeb")
                Dim txtnoofchild As TextBox = gvRow.FindControl("txtnoofchild")
                Dim txtfromage As TextBox = gvRow.FindControl("txtfromage")
                Dim txttoage As TextBox = gvRow.FindControl("txttoage")
                Dim txt_Baby_CotNo As TextBox = gvRow.FindControl("txt_Baby_CotNo")
                Dim txtsharing As TextBox = gvRow.FindControl("txtsharing")

                Dim txtEB As TextBox = gvRow.FindControl("txtEB")


                strRemark = strRemark + IIf(strRemark.Length > 0, vbCrLf, "")
                If txtfromage.Text <> "" And txtsharing.Text <> " " Then
                    ' strRemark = strRemark & txtrmtypname.Text & "  " & "CHILD SHARING On " & txtmealcode.Text & " basis"


                    If txtmaxchild.Text <> "" Then

                        strRemark = strRemark + " " + " Maximum " & txtmaxchild.Text & " Children allowed in " & txtrmtypname.Text & " On " & txtmealcode.Text & " basis."
                        If txtnoofchild.Text.ToUpper = UCase("All") Then
                            strRemark = strRemark & " " & txtnoofchild.Text & " "

                            If txtsharing.Text.ToUpper = UCase("Free") Or txtsharing.Text.ToUpper = UCase("Incl") Then
                                strRemark = strRemark & " children  can stay " & IIf(txtsharing.Text.ToUpper = UCase("Incl"), "Free", "Free") & "  with parents in existing bedding on the same meal plan."
                            ElseIf txtsharing.Text.ToUpper = UCase("N.Incl") Or txtsharing.Text.ToUpper = UCase("On Request") Or txtsharing.Text.ToUpper = UCase("N/A") Then
                                strRemark = strRemark & " Child Sharing is not applicable."
                            Else 'If txtsharing.Text.ToUpper <> UCase("N.Incl") Or txtsharing.Text.ToUpper <> UCase("On Request") Then
                                strRemark = strRemark & " children  can stay  will be charged AED " & txtsharing.Text & " /-  with parents in existing bedding on the same meal plan."

                            End If
                        Else
                            strRemark = strRemark & " " & txtnoofchild.Text & " From " & txtfromage.Text & " To " & txttoage.Text & txt_Baby_CotNo.Text & " "
                            If txtsharing.Text.ToUpper = UCase("Free") Or txtsharing.Text.ToUpper = UCase("Incl") Then
                                strRemark = strRemark & " can stay " & IIf(txtsharing.Text.ToUpper = UCase("Incl"), "Free", "Free") & "  with parents in existing bedding on the same meal plan."
                            ElseIf txtsharing.Text.ToUpper = UCase("N.Incl") Or txtsharing.Text.ToUpper = UCase("On Request") Or txtsharing.Text.ToUpper = UCase("N/A") Then
                                strRemark = strRemark & " Child Sharing is not applicable."
                            Else 'If txtsharing.Text.ToUpper <> UCase("N.Incl") Or txtsharing.Text.ToUpper <> UCase("On Request") Then
                                strRemark = strRemark & "  can stay will be charged AED " & txtsharing.Text & " /-  with parents in existing bedding on the same meal plan."

                            End If
                        End If
                    End If
                    If txtmaxeb.Text <> "" Then
                        If txtnoofchild.Text = "All" Then
                            If txtEB.Text.ToUpper = UCase("Free") Or txtEB.Text.ToUpper = UCase("Incl") Then
                                strRemark = strRemark & " " & txtnoofchild.Text & " Children can avail of Extra Bed free on the same meal plan - Max  " & txtmaxeb.Text & " Extra Bed."
                            ElseIf txtEB.Text.ToUpper = UCase("N.Incl") Or txtEB.Text.ToUpper = UCase("On Request") Or txtEB.Text.ToUpper = UCase("N/A") Then
                                strRemark = strRemark & " Child Extra Bed is not applicable."
                            Else 'If txtEB.Text.ToUpper <> UCase("N.Incl") Or txtEB.Text.ToUpper <> UCase("On Request") Then
                                strRemark = strRemark & " " & txtnoofchild.Text & " Children will be charged  AED " & txtEB.Text & " /- of Extra Bed  on the same meal plan - Max  " & txtmaxeb.Text & " Extra Bed."

                            End If
                        Else
                            If txtEB.Text.ToUpper = UCase("Free") Or txtEB.Text.ToUpper = UCase("Incl") Then
                                strRemark = strRemark & " " & txtnoofchild.Text & " From " & txtfromage.Text & " To " & txttoage.Text & " No Of Baby Cot " & txt_Baby_CotNo.Text & "  can avail of Extra bed Free on the same meal plan - " & txtmaxeb.Text & " Extra Bed."
                            ElseIf txtEB.Text.ToUpper = UCase("N.Incl") Or txtEB.Text.ToUpper = UCase("On Request") Or txtEB.Text.ToUpper = UCase("N/A") Then
                                strRemark = strRemark & " Child Extra Bed is not applicable."
                            Else 'If txtEB.Text.ToUpper <> UCase("N.Incl") Or txtEB.Text.ToUpper <> UCase("On Request") Then
                                strRemark = strRemark & " " & txtnoofchild.Text & " From " & txtfromage.Text & " To " & txttoage.Text & " No Of Baby Cot " & txt_Baby_CotNo.Text & " will be charged AED " & txtEB.Text & " /- of Extra Bed  on the same meal plan - Max  " & txtmaxeb.Text & " Extra Bed."

                            End If

                        End If
                        ' strRemark = strRemark & " " & txtnoofchild.Text & " From " & txtfromage.Text & " To " & txttoage.Text & " "
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
            objUtils.WritErrorLog("ContractChildPolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try
            'strSqlQry = "select h.contractid,h.childpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            '     " h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek   from view_contracts_childpolicy_header h(nolock),view_contracts_search d(nolock)  where h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"

            If Session("Calledfrom") = "Offers" Then

                ' commented by rosalin 2019-10-27
                strSqlQry = "sp_show_copychild '' , '" & CType(hdnpartycode.Value, String) & "'"

                '                strSqlQry = "select h.contractid as contractid,h.childpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek    from view_contracts_childpolicy_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and     h.seasons <>'' union all " _
                '& " select h.contractid as contractid,h.childpolicyid plistcode,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
                '& " ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek  from view_contracts_childpolicy_header h(nolock),view_contracts_childpolicy_dates d(nolock),view_contracts_search s(nolock)  where  isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.childpolicyid=d.childpolicyid and      h.seasons ='' group by h.contractid, h.childpolicyid,h.applicableto  "



            Else

                ' commented by rosalin 2019-10-27
                strSqlQry = "sp_show_copychild '" & CType(hdncontractid.Value, String) & "' , '" & CType(hdnpartycode.Value, String) & "'"


                '                strSqlQry = "select h.contractid as contractid,h.childpolicyid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek    from view_contracts_childpolicy_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and    h.contractid<>'" & hdncontractid.Value & "' and h.seasons <>'' union all " _
                '& " select h.contractid as contractid,h.childpolicyid plistcode,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
                '& " ,dbo.fn_get_weekdays_fromtable('view_contracts_childpolicy_header',h.childpolicyid) DaysoftheWeek  from view_contracts_childpolicy_header h(nolock),view_contracts_childpolicy_dates d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.childpolicyid=d.childpolicyid and     h.contractid<>'" & hdncontractid.Value & "' and h.seasons ='' group by h.contractid, h.childpolicyid,h.applicableto  "



            End If





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
            objUtils.WritErrorLog("ContractChildpolicy.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Private Sub sortgvsearch()
        'changed by shahul on 17/03/2018
        If Session("Calledfrom") = "Offers" Then
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("seasoncode", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("frmdate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("todate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("h.applicableto", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5

                    FillGrid("DaysoftheWeek", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 6

                    FillGrid("h.adddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 7
                    FillGrid("h.adduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 8
                    FillGrid("h.moddate", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 9
                    FillGrid("h.moduser", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            End Select
        Else
            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("tranid", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 1
                    FillGrid("seasoncode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 2
                    FillGrid("frmdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 3
                    FillGrid("todate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 4
                    FillGrid("h.applicableto", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
                Case 5

                    FillGrid("DaysoftheWeek", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
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

    Protected Sub grdpromotion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdpromotion.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lblpromotionid As Label
            Dim lblpromotionname As Label, lblapplicable As Label, lblplistcode As Label
            ViewState("OfferCopy") = 0
            lblpromotionid = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            lblpromotionname = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionname")
            lblapplicable = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblapplicableto")
            lblplistcode = grdpromotion.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            If lblpromotionid.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then

                PanelMain.Visible = True
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblpromotionid.Text.Trim, String))
                PanelMain.Style("display") = "block"

                ShowRecordcopycontract(CType(lblplistcode.Text.Trim, String))

                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)
                dv_SearchResult.Style.Add("display", "none")

                ViewState("OfferCopy") = 1

             
                'strSqlQry = "select count( clineno) from view_contracts_childpolicy_detail(nolock) where childpolicyid='" & lblplistcode.Text & "'"


                ''" ' and subseasnname = '" & subseasonname & "'"
                'cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
                'If cnt = 0 Then cnt = 7
                '' fillDategrd(grdRoomrates, False, cnt)

                ''  fillroomgrid(grdRoomrates, False, cnt)
                createdatacolumnsoffers()
                createdatarowsoffer(lblplistcode.Text)
                SetgridbindData()
                '  SetgridbindDataoffers(lblplistcode.Text)
                lblHeading.Text = "Copy Child Policy - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Promotion Child Policy "


                filldaysgrid()
                fillweekdaysoffers(CType(hdnpromotionid.Value, String))
                'ShowDynamicGrid()
                fillDategrd(grdDates, True)
                ShowDatesnew(CType(hdnpromotionid.Value, String))
                grdRoomrates.Visible = True
                btnSave.Visible = True
                PanelMain.Visible = True
                txtplistcode.Text = ""

                lblratetype.Text = "Promotion"

                

                'Rosalin 2019-11-12
                'wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, ViewState("State"))
                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                '##
                ' Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                btnSave.Visible = True
                btnSave.Text = "Save"





            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    'changed by mohamed on 21/02/2018
    Function fnCalculateVATValue() As Boolean
        Dim l As Integer
        For Each grdRow As GridViewRow In grdRoomrates.Rows
            If (grdRow.RowType = DataControlRowType.DataRow) Then
                Dim txt1SH As TextBox = grdRow.FindControl("txtsharing")
                Dim txtTV1SH As TextBox = Nothing
                Dim txtNTV1SH As TextBox = Nothing
                Dim txtVAT1SH As TextBox = Nothing
                txtTV1SH = grdRow.FindControl("txtTVSH")
                txtNTV1SH = grdRow.FindControl("txtNTVSH")
                txtVAT1SH = grdRow.FindControl("txtVATSH")

                If txt1SH IsNot Nothing Then
                    Call assignVatValueToTextBox(txt1SH, txtTV1SH, txtNTV1SH, txtVAT1SH)
                End If

                Dim txt2EB As TextBox = grdRow.FindControl("txtEB")
                Dim txtTV2EB As TextBox = Nothing
                Dim txtNTV2EB As TextBox = Nothing
                Dim txtVAT2EB As TextBox = Nothing
                txtTV2EB = grdRow.FindControl("txtTVEB")
                txtNTV2EB = grdRow.FindControl("txtNTVEB")
                txtVAT2EB = grdRow.FindControl("txtVATEB")

                If txt2EB IsNot Nothing Then
                    Call assignVatValueToTextBox(txt2EB, txtTV2EB, txtNTV2EB, txtVAT2EB)
                End If

                'Dynamic Starts Here
                For i = 0 To grdRoomrates.Columns.Count - 14 '13 '*** Danny Changed 18/03/2018
                    'l = IIf(grdRow.RowIndex >= 1, ((grdRoomrates.Columns.Count - 12) * grdRow.RowIndex), 0)
                    l = IIf(grdRow.RowIndex >= 1, ((grdRoomrates.Columns.Count - 13) * grdRow.RowIndex), 0) '12 '*** Danny Changed 18/03/2018

                    Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & i)

                    Dim txt3Dyn As TextBox = grdRow.FindControl("txt" & i + l)
                    Dim txtTV3Dyn As TextBox = Nothing
                    Dim txtNTV3Dyn As TextBox = Nothing
                    Dim txtVAT3Dyn As TextBox = Nothing
                    txtTV3Dyn = grdRow.FindControl("txtTV" & i + l)
                    txtNTV3Dyn = grdRow.FindControl("txtNTV" & i + l)
                    txtVAT3Dyn = grdRow.FindControl("txtVAT" & i + l)
                    If txt3Dyn IsNot Nothing And txtTV3Dyn IsNot Nothing Then
                        If txt3Dyn.Style("FieldHeader") <> "" Then
                            If txt3Dyn.Style("FieldHeader").Contains("Sr No") = False And txt3Dyn.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt3Dyn.Style("FieldHeader").Contains("Extra Person Supplement") = False And txt3Dyn.Style("FieldHeader").Contains("Min Nights") = False And txt3Dyn.Style("FieldHeader").Contains("Pkg") = False And txt3Dyn.Style("FieldHeader").Contains("Remark") = False Then
                                Call assignVatValueToTextBox(txt3Dyn, txtTV3Dyn, txtNTV3Dyn, txtVAT3Dyn)
                            End If
                        End If
                    End If
                Next i
            End If 'If (grdRow.RowType = DataControlRowType.DataRow) Then
        Next
        Return True
    End Function

    'changed by mohamed on 21/02/2018
    Sub assignVatValueToTextBox(ByRef txt1 As TextBox, ByRef txtTV1 As TextBox, ByRef txtNTV1 As TextBox, ByRef txtVAT1 As TextBox)
        Dim clsServ As New clsServices
        Dim lRetValue As clsMaster()
        Dim lsSqlQry As String = ""

        If txt1.Text.Trim = "" Or chkVATCalculationRequired.Checked = False Then
            txtTV1.Text = IIf(txt1.Text.Trim = "", "", Val(txt1.Text))
            txtNTV1.Text = ""
            txtVAT1.Text = ""
        Else
            lsSqlQry = "execute sp_calculate_taxablevalue_onlycost " & Val(txt1.Text) & ","
            lsSqlQry += Val(txtServiceCharges.Text) & "," & Val(TxtMunicipalityFees.Text) & ","
            lsSqlQry += Val(txtVAT.Text) & "," & Val(txtTourismFees.Text)
            lRetValue = clsServ.getCommonArrayOfCodeAndNameFromSqlQuery(Session("dbConnectionName"), lsSqlQry)
            For li As Integer = lRetValue.GetLowerBound(0) To lRetValue.GetUpperBound(0)
                If lRetValue(li).ListText = "taxablevalue" Then
                    txtTV1.Text = Val(lRetValue(li).ListValue)
                End If
                If lRetValue(li).ListText = "nontaxablevalue" Then
                    txtNTV1.Text = Val(lRetValue(li).ListValue)
                End If
                If lRetValue(li).ListText = "vatvalue" Then
                    txtVAT1.Text = Val(lRetValue(li).ListValue)
                End If
            Next
        End If
    End Sub

    'changed by mohamed on 21/02/2018
    Protected Sub VATTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Call fnCalculateVATValue()
    End Sub

    'changed by mohamed on 21/02/2018
    Protected Sub chkVATCalculationRequired_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVATCalculationRequired.CheckedChanged
        If chkVATCalculationRequired.Checked = True Then
            If Val(txtServiceCharges.Text) + Val(TxtMunicipalityFees.Text) + Val(txtTourismFees.Text) + Val(txtVAT.Text) = 0 Then
                sbFillTaxDetail()
            End If
        Else
            txtServiceCharges.Text = "0"
            TxtMunicipalityFees.Text = "0"
            txtTourismFees.Text = "0"
            txtVAT.Text = "0"
        End If
        Call fnCalculateVATValue()
    End Sub

    'changed by mohamed on 21/02/2018
    Sub sbFillTaxDetail()
        'chkVATCalculationRequired.Checked = False
        txtServiceCharges.Text = ""
        TxtMunicipalityFees.Text = ""
        txtTourismFees.Text = ""
        txtVAT.Text = ""

        Dim dsVatDet As DataSet
        'strSqlQry = "select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'"
        strSqlQry = "execute sp_get_taxdetail_frommaster '" & hdnpartycode.Value & "',5105" '"select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'"
        dsVatDet = objUtils.GetDataFromDatasetnew(Session("dbConnectionName"), strSqlQry)

        If dsVatDet.Tables(0).Rows.Count > 0 Then
            With dsVatDet.Tables(0).Rows(0)
                'chkVATCalculationRequired.Checked = True
                txtServiceCharges.Text = .Item("servicechargeperc").ToString
                TxtMunicipalityFees.Text = .Item("municipalityfeeperc").ToString
                txtTourismFees.Text = .Item("tourismfeeperc").ToString
                txtVAT.Text = .Item("vatperc").ToString
            End With
        End If
    End Sub

    Protected Sub grdRoomrates_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRoomrates.DataBinding
        ShowHide()
    End Sub

    'Protected Sub grdRoomrates_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRoomrates.DataBound
    '    ShowHide()
    'End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        ModalViewrates.Hide()
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete

    End Sub
End Class

