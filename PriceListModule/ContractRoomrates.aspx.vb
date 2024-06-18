Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class ContractRoomrates
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

#End Region
#Region "Enum GridCol"
    Enum GridCol

        plistcode = 1
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
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = False Then
            Session("GV_HotelData") = Nothing

            NumbersHtml(txtminnights)       'accept only number
            NumbersHtml(txtfillrate)
            NumbersHtml(txtamt1)
            divcopy1.Style("display") = "none"


            'If Session("GV_HotelData") Is Nothing = False Then
            '    dt = Session("GV_HotelData")
            '    'fill controls from previous form
            '    ' Now  Bind Column Dynamically 
            '    Dim fld2 As String = ""
            '    Dim col As DataColumn
            '    For Each col In dt.Columns
            '        If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" Then
            '            Dim bfield As New TemplateField
            '            'Call Function
            '            bfield.HeaderTemplate = New ClassPriceList(ListItemType.Header, col.ColumnName, fld2)
            '            bfield.ItemTemplate = New ClassPriceList(ListItemType.Item, col.ColumnName, fld2)
            '            grdRoomrates.Columns.Add(bfield)


            '        End If
            '    Next
            '    grdRoomrates.Visible = True
            '    grdRoomrates.DataSource = dt
            '    'InstantiateIn Grid View
            '    grdRoomrates.DataBind()
            'End If
        Else

            'hdnpartycode is coming blank in most of the cases, so it is assignd in page init if page post back 'changed by mohamed on 21/02/2018
            If Session("Calledfrom") = "Offers" Then
                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
            ElseIf Session("Calledfrom") = "Contracts" Then
                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
            End If


            If Session("GV_HotelData") Is Nothing = False Then
                dt = Session("GV_HotelData")


                Dim cnt1 As Integer = grdRoomrates.Columns.Count




                ' If dt.Columns.Count >= 7 Then Exit Sub
                'fill controls from previous form
                ' Now  Bind Column Dynamically 
                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" Then


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
                GrdRoomRatesDataBind() 'grdRoomrates.DataBind
            End If
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

            strSqlQry = " select c.plistcode , c.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_cplisthnew c(nolock)  where  isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  c.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by c.promotionid,h.approved,h.promotionname,c.plistcode order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

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
            objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            ModalCopyrates.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Sub GrdRoomRatesDataBind()
        ViewState("lbExtraSuppRequired") = False
        ViewState("liExtraSuppColumnNo") = Nothing
        ViewState("liUnitColumnNo") = Nothing
        grdRoomrates.DataBind()
        If ViewState("liExtraSuppColumnNo") IsNot Nothing Then
            grdRoomrates.Columns(ViewState("liExtraSuppColumnNo")).Visible = ViewState("lbExtraSuppRequired")
            grdRoomrates.Columns(ViewState("liUnitColumnNo")).Visible = ViewState("lbExtraSuppRequired")
        End If


        Dim extraperson As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select max(isnull(noofextraperson,0)) from view_partymaxaccomodation d where partycode='" & hdnpartycode.Value & "'")

        If extraperson = 0 Then

            Dim headerlabel As New TextBox
            Dim txt As TextBox
            Dim cnt As Long = 0
            Dim header As Long = 0
            cnt = grdRoomrates.Columns.Count
            Dim heading(cnt + 1) As String
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If txt.Text <> Nothing Then
                    heading(header) = txt.Text
                End If
            Next

            For j = 0 To cnt - 8
                If heading(j) <> "No.of.Nights Room Rate" And heading(j) <> "Unityesno" And heading(j) <> "Extra Person Supplement" And heading(j) <> "Unityesno" And heading(j) <> "Noofextraperson" And heading(j) <> "Booking Code" Then

                    For s = 0 To grdRoomrates.Columns.Count - 8
                        headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                        If headerlabel.Text <> Nothing Then
                            If headerlabel.Text = "Extra Person Supplement" Then
                                grdRoomrates.Columns(s + 7).ItemStyle.CssClass = "displaynone"
                                grdRoomrates.Columns(s + 7).HeaderStyle.CssClass = "displaynone"
                            Else

                                grdRoomrates.Columns(s + 7).ItemStyle.CssClass = ""
                                grdRoomrates.Columns(s + 7).HeaderStyle.CssClass = ""
                            End If
                            If headerlabel.Text = "Min Nights" Then
                                grdRoomrates.Columns(s + 7).ItemStyle.CssClass = "displaynone"
                                grdRoomrates.Columns(s + 7).HeaderStyle.CssClass = "displaynone"
                            
                            End If
                        End If



                    Next
                End If
            Next

        End If
    End Sub

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        sortgvsearch()
        '  FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")


    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String


        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        Dim CalledfromValue As String = ""
        Dim ConRmappid As String = ""
        Dim ConRmappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""


        If IsPostBack = False Then

            ConRmappid = 1
            ConRmappname = objUser.GetAppName(Session("dbconnectionName"), ConRmappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else



                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConRmappname, String), "ContractRoomRates.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConRmappname, String), "ContractRoomRates.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractRoomRates.aspx", ConRmappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConRmappid, intMenuID)

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
                        btncopycontract.Visible = True
                    End If
                End If
            Else
                btnselect.Visible = False
                btncopycontract.Visible = False

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

                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdnpromotionid.Value
                Page.Title = "Promotion Room Rates "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1

                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

                FillGrid("plistcode", hdnpromotionid.Value, hdnpartycode.Value, "Desc")
                PanelMain.Style.Add("display", "none")

            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)


                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = hdncontractid.Value 'CType(Request.QueryString("contractid"), String)

                divoffer.Style.Add("display", "none")

                wucCountrygroup.sbSetPageState("", "CONTRACTROOMRATES", CType(Session("ContractState"), String))
                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                ViewState("partycode") = hdnpartycode.Value

                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdncontractid.Value
                Page.Title = "Contract Room Rates "

                Dim commissionable As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(Commissionable,0) from view_contracts_search(nolock) where contractid='" & hdncontractid.Value & "'")

                ViewState("commissionable") = commissionable

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1



                FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")
                PanelMain.Style.Add("display", "none")


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

            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                      & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

            If ddlrmcat.Value <> "[Select]" Then

                strSqlQry = "select rmcatcode from ( select prc.rmcatcode,isnull(rc.rankorder,999) rankorder   from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' " _
                            & " and prc.partycode='" & hdnpartycode.Value & "'  and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139) union all " _
                            & " select 'Extra Person Supplement' rmcatcode,9999 rankorder) results order by  rankorder"

                'strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                '        & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmcat, "rmcatcode", "rmcatcode", strSqlQry, True, ddlrmcat.Value)
            Else

                'strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and prc.partycode='" _
                '    & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

                strSqlQry = "select rmcatcode from ( select prc.rmcatcode,isnull(rc.rankorder,999) rankorder   from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' " _
                          & " and prc.partycode='" & hdnpartycode.Value & "'  and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139) union all " _
                          & " select 'Extra Person Supplement' rmcatcode,9999 rankorder) results order by  rankorder"

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlrmcat, "rmcatcode", "rmcatcode", strSqlQry, True)

            End If



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

        If Session("Calledfrom") = "Offers" Then
            btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdnpromotionid.Value & "')")
            btnAddNew.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
            btncopycontract.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
        Else
            btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        End If



        hdntbldata.Text = ""
        hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")
        'ClientScript.GetPostBackEventReference(Me, String.Empty)
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RoomratesWindowPostBack") Then

        '    FillGrid(hdncontractid.Value, hdnpartycode.Value, "Desc")
        'End If
        chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

        btnclear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure  want to clear the Rates?')==false)return false;")

        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Sub GenerateGridColumns(ByVal lsMode As String, ByVal liRowIndex As Integer)
        Dim dtStayPeriod As New DataTable
        Dim drStayPeriod As DataRow
        dtStayPeriod.Columns.Add(New DataColumn("FromDate", GetType(Date)))
        dtStayPeriod.Columns.Add(New DataColumn("ToDate", GetType(Date)))

        Dim txtStayfromDate As TextBox
        Dim txtStayToDate As TextBox

        If lsMode.Trim.ToUpper <> "BeLoad" Then
            For Each lRow As GridViewRow In grdpromodates.Rows
                txtStayfromDate = lRow.FindControl("txtfromDate")
                txtStayToDate = lRow.FindControl("txtToDate")
                If IsDate(txtStayfromDate.Text) And IsDate(txtStayToDate.Text) Then
                    drStayPeriod = dtStayPeriod.NewRow()
                    drStayPeriod("FromDate") = txtStayfromDate.Text
                    drStayPeriod("ToDate") = txtStayToDate.Text
                    dtStayPeriod.Rows.Add(drStayPeriod)
                End If
            Next
        End If

        If lsMode.Trim.ToUpper = "DELETE" Then
            If (dtStayPeriod.Rows.Count > liRowIndex) Then
                dtStayPeriod.Rows(liRowIndex).Delete()
            End If
        End If
        If lsMode.Trim.ToUpper = "ADD" Or dtStayPeriod.Rows.Count = 0 Then
            drStayPeriod = dtStayPeriod.NewRow()
            drStayPeriod("FromDate") = DBNull.Value
            drStayPeriod("ToDate") = DBNull.Value
            dtStayPeriod.Rows.Add(drStayPeriod)
        End If
        grdpromodates.DataSource = dtStayPeriod
        grdpromodates.DataBind()
    End Sub
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        '   Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("ADD", 0)
        '    row.FindControl("imgStayAdd").Visible = False
        '    Dim dpFDate As TextBox
        '    dpFDate = TryCast(grdpromodates.Rows(grdpromodates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
        '    dpFDate.Focus()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try


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

        'Try
        '    Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        '    GenerateGridColumns("DELETE", row.RowIndex)
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try

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
    Private Sub createdatatableoffer()
        Try

            cnt = 0
            Session("GV_HotelData") = Nothing

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "sp_checkExtrapax'" & CType(hdnpartycode.Value, String) & "'"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlCmd.ExecuteNonQuery()


            mySqlConn.Close()




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select count(distinct v.rmcatcode) from partyrmcat prc,rmcatmast rc,view_offers_accomodation v(nolock) where  v.rmcatcode =prc.rmcatcode  and  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and v.promotionid = '" & hdnpromotionid.Value & "' and rc.active=1 and prc.partycode='" _
                      & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select rmcatcode,rankorder from   (select distinct v.rmcatcode,isnull(rc.rankorder,999) rankorder from partyrmcat prc,rmcatmast rc,view_offers_accomodation v(nolock) where v.rmcatcode =prc.rmcatcode  and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and v.promotionid = '" & hdnpromotionid.Value & "' and rc.active=1 and prc.partycode='" _
                        & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139) ) rs  order by isnull(rankorder,999)"  ' p.rmcatcode " '

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

            dt.Columns.Add(New DataColumn("Room_Type_Code", GetType(String)))
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
            dt.Columns.Add(New DataColumn("Booking Code", GetType(String)))
            'dt.Columns.Add(New DataColumn("Compulsory Nights", GetType(String)))
            'dt.Columns.Add(New DataColumn("Remark", GetType(String)))
            Session("GV_HotelData") = dt



            ' If dt.Columns.Count >= 7 Then Exit Sub
            'fill controls from previous form
            ' Now  Bind Column Dynamically 

            Dim cnt1 As Integer = grdRoomrates.Columns.Count

            Dim flag As Boolean = False

            Dim unitcount As Integer = 0



            For i = grdRoomrates.Columns.Count - 1 To 7 Step -1
                grdRoomrates.Columns.RemoveAt(i)
            Next



            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" Then


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
            GrdRoomRatesDataBind() 'grdRoomrates.DataBind()


        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Private Sub createdatatable()
        Try

            cnt = 0
            Session("GV_HotelData") = Nothing

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "sp_checkExtrapax'" & CType(hdnpartycode.Value, String) & "'"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlCmd.ExecuteNonQuery()

            'cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            ' strSqlQry = "select count(rmcatcode) from partyrmcat where partycode='" & hdnpartycode.Value & "'"
            strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and rc.active=1 and prc.partycode='" _
                      & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and rc.active=1 and prc.partycode='" _
                        & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

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

            dt.Columns.Add(New DataColumn("Room_Type_Code", GetType(String)))
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

            dt.Columns.Add(New DataColumn("Booking Code", GetType(String)))

            'dt.Columns.Add(New DataColumn("Compulsory Nights", GetType(String)))
            'dt.Columns.Add(New DataColumn("Remark", GetType(String)))
            Session("GV_HotelData") = dt



            ' If dt.Columns.Count >= 7 Then Exit Sub
            'fill controls from previous form
            ' Now  Bind Column Dynamically 

            Dim cnt1 As Integer = grdRoomrates.Columns.Count
            'Dim header As Long = 0
            'Dim heading(cnt1 + 1) As String
            Dim flag As Boolean = False

            Dim unitcount As Integer = 0

            'Dim txt As TextBox
            'For header = 0 To cnt1 - 8
            '    txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            '    If Not txt Is Nothing Then
            '        heading(header) = txt.Text
            '    End If
            'Next


            For i = grdRoomrates.Columns.Count - 1 To 7 Step -1
                grdRoomrates.Columns.RemoveAt(i)
            Next



            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" Then


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
            GrdRoomRatesDataBind() 'grdRoomrates.DataBind()


        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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
            ' contextKey = Convert.ToString(HttpContext.Current.Viewstate("partycode").ToString())
            maxstate = Convert.ToString(HttpContext.Current.Session("ContractRefCode").ToString())


            strSqlQry = "select distinct SeasonName from view_contractseasons_rate(nolock) where contractid='" & maxstate & "' and SeasonName like '" & Trim(prefixText) & "%' order by SeasonName "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    seasonlist.Add(myDS.Tables(0).Rows(i)("SeasonName").ToString())
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

                If strsortby = "frmdate" Or strsortby = "todate" Then
                    strSqlQry = "with ctee as(select h.plistcode,h.promotionid,oh.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek,h.adddate,h.adduser," & _
        " h.moddate ,h.moduser  from view_cplisthnew h(nolock), view_offers_header oh (nolock) ,view_cplisthnew_offerdates d(nolock)  where h.promotionid=oh.promotionid and h.plistcode=d.plistcode and  h.partycode='" & hdnpartycode.Value & "' and   and h.promotionid='" & contractid & _
        "' group by h.plistcode,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee order by convert(datetime," & strsortby & ",103)  " & strsortorder & " "
                Else
                    strSqlQry = "select h.plistcode,h.promotionid,oh.promotionname,'' seasoncode,convert(varchar(10),min(d.fromdate),103) frmdate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek,h.adddate,h.adduser," & _
        " h.moddate ,h.moduser  from view_cplisthnew h(nolock), view_offers_header oh (nolock) ,view_cplisthnew_offerdates d(nolock)  where  h.promotionid=oh.promotionid and h.plistcode=d.plistcode and h.partycode='" & hdnpartycode.Value & "'  and h.promotionid='" & contractid & _
        "' group by h.plistcode,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & "  " & strsortorder & " "
                End If

            Else

                If strsortby = "frmdate" Or strsortby = "todate" Then
                    strSqlQry = "with ctee as(select h.plistcode,'' promotionid,'' promotionname,h.subseascode as seasoncode,convert(varchar(10),min(cs.fromdate),103) frmdate, convert(varchar(10),max(cs.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek,h.adddate,h.adduser," & _
        " h.moddate ,h.moduser  from view_cplisthnew h(nolock), view_contractseasons cs(nolock)  where h.partycode='" & hdnpartycode.Value & "' and  h.contractid=cs.contractid and  h.subseascode =cs.seasonname and h.contractid='" & contractid & "' and cs.contractid='" & contractid & _
        "' group by h.plistcode,h.subseascode,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee order by convert(datetime," & strsortby & ",103)  " & strsortorder & " "

                Else

                    '            strSqlQry = "select h.plistcode,'' promotionid,'' promotionname,h.subseascode as seasoncode,convert(varchar(10),min(cs.fromdate),103) frmdate, convert(varchar(10),max(cs.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek,h.adddate,h.adduser," & _
                    '" h.moddate ,h.moduser  from view_cplisthnew h(nolock), view_contractseasons cs(nolock)  where h.partycode='" & hdnpartycode.Value & "' and  h.contractid=cs.contractid and  h.subseascode =cs.seasonname and   h.contractid='" & contractid & "' and cs.contractid='" & contractid & _
                    '"' group by h.plistcode,h.subseascode,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & "  " & strsortorder & " "

                    'Added By rosalin 2019-10-26
                    strSqlQry = "exec New_ContractRoomRate_Header '" & CType(contractid, String) & "','" & CType(hdnpartycode.Value, String) & "'"


                End If
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

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Private Function OfferValidateSave() As Boolean
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
        Dim a As Long = cnt - 11
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Dim m As Long = 0




        For header = 0 To cnt - 7
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If Not txt Is Nothing Then
                heading(header) = txt.Text
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

        Try

            Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing

            b = 0
            n = 0
            flag = False
            For Each gvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8


                        If heading(j) = "Min Nights" Or heading(j) = "Booking Code" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Or heading(j) = "Booking Code" Then 'changed by mohamed on 28/03/2018 --removed extra person supplements condition
                        Else


                            txt = gvRow.FindControl("txt" & j)

                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    If heading(j) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 --added this condition
                                    Else
                                        flag = True
                                    End If
                                    'Exit For 'changed by mohamed on 21/02/2018
                                    '' GoTo Err
                                    'changed by mohamed on 21/02/2018
                                    txtTV1 = gvRow.FindControl("txtTV" & j)
                                    txtNTV1 = gvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = gvRow.FindControl("txtVAT" & j)

                                    If txt IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If

                        End If

                    Next
                    m = j
                Else
                    k = 0

                    For j = n To (m + n) - 1
                        txt = gvRow.FindControl("txt" & j)


                        If heading(k) = "Min Nights" Or heading(k) = "Booking Code" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Or heading(k) = "Booking Code" Then 'changed by mohamed on 28/03/2018 --removed extra person supplements condition
                        Else


                            txt = gvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    If heading(k) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 --added this condition
                                    Else
                                        flag = True
                                    End If
                                    ''GoTo Err
                                    'Exit For 'changed by mohamed on 21/02/2018
                                    'changed by mohamed on 21/02/2018
                                    txtTV1 = gvRow.FindControl("txtTV" & j)
                                    txtNTV1 = gvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = gvRow.FindControl("txtVAT" & j)

                                    If txt IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If


                        End If


                        k = k + 1
                    Next

                End If
                b = j
                n = j

            Next
            'Err:



        Catch ex As Exception
            flag = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


        'If flag = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
        '    OfferValidateSave = False
        '    Exit Function
        'End If


        '        b = 0
        '        n = 0
        '        flag = False
        '        For Each gvRow In grdRoomrates.Rows
        '            If n = 0 Then
        '                For j = 0 To cnt - 8
        '                    txt = gvRow.FindControl("txt" & j)
        '                    If txt Is Nothing Then
        '                    Else
        '                        If txt.Text <> Nothing Then
        '                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
        '                            Else


        '                                txt = gvRow.FindControl("txt" & j)

        '                                If txt Is Nothing Then
        '                                Else
        '                                    If txt.Text <> "" Then
        '                                        flag = True
        '                                        GoTo Err
        '                                    End If
        '                                End If

        '                            End If
        '                        End If
        '                    End If
        '                Next
        '                m = j
        '            Else
        '                k = 0

        '                For j = n To (m + n) - 1
        '                    txt = gvRow.FindControl("txt" & j)
        '                    If txt Is Nothing Then
        '                    Else
        '                        If txt.Text <> Nothing Then
        '                            If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
        '                            Else


        '                                txt = gvRow.FindControl("txt" & j)
        '                                 If txt Is Nothing Then
        '                                Else
        '                                    If txt.Text <> "" Then
        '                                        flag = True
        '                                        GoTo Err
        '                                    End If
        '                                End If


        '                            End If
        '                        End If
        '                    End If
        '                    k = k + 1
        '                Next

        '            End If
        '            b = j
        '            n = j

        '        Next
        'Err:    If flag = False Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
        '            OfferValidateSave = False
        '            Exit Function
        '        End If


        '        b = 0
        '        n = 0
        '        For Each gvRow In grdRoomrates.Rows
        '            If n = 0 Then
        '                For j = 0 To cnt - 8
        '                    If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then

        '                    Else
        '                        txt = gvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> "" Then
        '                                flag = True
        '                                GoTo Err
        '                            End If
        '                        End If

        '                    End If
        '                Next
        '                m = j
        '            Else
        '                k = 0
        '                For j = n To (m + n) - 1
        '                    If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
        '                    Else
        '                        txt = gvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> "" Then
        '                                flag = True
        '                                GoTo Err
        '                            End If
        '                        End If

        '                    End If
        '                    k = k + 1
        '                Next
        '            End If
        '            b = j
        '            n = j
        '        Next



        'Err:    If flag = False Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
        '            OfferValidateSave = False
        '            Exit Function
        '        End If


        flag = False

        b = 0
        n = 0
        For Each gvRow In grdRoomrates.Rows
            If n = 0 Then
                For j = 0 To cnt - 8
                    If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & b + a + 1)
                        If txt Is Nothing Then
                        Else
                            If txt.Text = "" Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No.of Nights can not be left blank.');", True)
                                SetFocus(txt)
                                OfferValidateSave = False
                                Exit Function
                            End If
                        End If

                        txt = gvRow.FindControl("txt" & b + a + 2)
                        If txt Is Nothing Then
                        Else
                            'If txt.Text = "" Then
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minium nights can not be left blank.');", True)
                            '    SetFocus(txt)
                            '    OfferValidateSave = False
                            '    Exit Function
                            'End If
                        End If

                    End If
                Next
                m = j
            Else
                k = 0
                For j = n To (m + n) - 1
                    If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & b + a + 1)
                        If txt Is Nothing Then
                        Else
                            If txt.Text = "" Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No.of Nights can not be left blank.');", True)
                                SetFocus(txt)
                                OfferValidateSave = False
                                Exit Function
                            End If
                        End If

                        txt = gvRow.FindControl("txt" & b + a + 2)
                        If txt Is Nothing Then
                        Else
                            'If txt.Text = "" Then
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minium nights can not be left blank.');", True)
                            '    SetFocus(txt)
                            '    OfferValidateSave = False
                            '    Exit Function
                            'End If
                        End If

                    End If
                    k = k + 1
                Next
            End If
            b = j
            n = j
        Next


        OfferValidateSave = True
    End Function

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
        Dim a As Long = cnt - 11
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Dim m As Long = 0


        If txtseasonname.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Season ' );", True)
            ValidateSave = False
            Exit Function
        End If

        For header = 0 To cnt - 7
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If Not txt Is Nothing Then
                heading(header) = txt.Text
            End If

        Next
        Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing
        Try

            b = 0
            n = 0
            flag = False
            For Each gvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8


                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Or heading(j) = "Booking Code" Then 'Or heading(j) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                        Else


                            txt = gvRow.FindControl("txt" & j)

                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    If heading(j) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 --added this condition
                                    Else
                                        flag = True
                                    End If
                                    'Exit For 'changed by mohamed on 21/02/2018
                                    'changed by mohamed on 21/02/2018
                                    txtTV1 = gvRow.FindControl("txtTV" & j)
                                    txtNTV1 = gvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = gvRow.FindControl("txtVAT" & j)

                                    If txt IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If

                        End If

                    Next
                    m = j
                Else
                    k = 0

                    For j = n To (m + n) - 1
                        txt = gvRow.FindControl("txt" & j)


                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Or heading(k) = "Booking Code" Then 'Or heading(k) = "Extra Person Supplement"  'changed by mohamed on 28/03/2018
                        Else


                            txt = gvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> "" Then
                                    If heading(k) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 --added this condition
                                    Else
                                        flag = True
                                    End If
                                    ''GoTo Err
                                    'Exit For'changed by mohamed on 21/02/2018
                                    'changed by mohamed on 21/02/2018
                                    txtTV1 = gvRow.FindControl("txtTV" & j)
                                    txtNTV1 = gvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = gvRow.FindControl("txtVAT" & j)

                                    If txt IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If


                        End If


                        k = k + 1
                    Next

                End If
                b = j
                n = j

            Next
            'Err:


        Catch ex As Exception
            flag = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        'If flag = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
        '    ValidateSave = False
        '    Exit Function
        'End If

        '        B = 0
        '        n = 0
        '        For Each gvRow In grdRoomrates.Rows
        '            If n = 0 Then
        '                For j = 0 To cnt - 8
        '                    If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
        '                    Else
        '                        txt = gvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> "" Then
        '                                flag = True
        '                                GoTo Err
        '                            End If
        '                        End If

        '                    End If
        '                Next
        '                m = j
        '            Else
        '                k = 0
        '                For j = n To (m + n) - 1
        '                    If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
        '                    Else
        '                        txt = gvRow.FindControl("txt" & j)
        '                        If txt Is Nothing Then
        '                        Else
        '                            If txt.Text <> "" Then
        '                                flag = True
        '                                GoTo Err
        '                            End If
        '                        End If

        '                    End If
        '                    k = k + 1
        '                Next
        '            End If
        '            b = j
        '            n = j
        '        Next



        'Err:    If flag = False Then
        '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
        '            ValidateSave = False
        '            Exit Function
        '        End If


        flag = False

        b = 0
        n = 0
        For Each gvRow In grdRoomrates.Rows
            If n = 0 Then
                For j = 0 To cnt - 8
                    If heading(j) = "Min Nights" Or heading(j) = "Booking Code" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                    Else
                        'txt = gvRow.FindControl("txt" & b + a + 1)
                        'If txt Is Nothing Then
                        'Else
                        '    If txt.Text = "" Then
                        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No.of Nights can not be left blank.');", True)
                        '        SetFocus(txt)
                        '        ValidateSave = False
                        '        Exit Function
                        '    End If
                        'End If

                        txt = gvRow.FindControl("txt" & b + a + 2)
                        If txt Is Nothing Then
                        Else
                            'If (txt.Text = "") Or Val(txt.Text = 0) Then
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minium nights can not be left blank.');", True)
                            '    SetFocus(txt)
                            '    ValidateSave = False
                            '    Exit Function
                            'End If
                        End If

                    End If
                Next
                m = j
            Else
                k = 0
                For j = n To (m + n) - 1
                    If heading(k) = "Min Nights" Or heading(k) = "Booking Code" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                    Else
                        'txt = gvRow.FindControl("txt" & b + a + 1)
                        'If txt Is Nothing Then
                        'Else
                        '    If txt.Text = "" Then
                        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No.of Nights can not be left blank.');", True)
                        '        SetFocus(txt)
                        '        ValidateSave = False
                        '        Exit Function
                        '    End If
                        'End If

                        txt = gvRow.FindControl("txt" & b + a + 2)
                        If txt Is Nothing Then
                        Else
                            'If (txt.Text = "") Or Val(txt.Text = 0) Then  ' Then
                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Minium nights can not be left blank.');", True)
                            '    SetFocus(txt)
                            '    ValidateSave = False
                            '    Exit Function
                            'End If
                        End If

                    End If
                    k = k + 1
                Next
            End If
            b = j
            n = j
        Next


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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Week Days Should not be Empty.');", True)
                FindDatePeriod = False
                Exit Function

            End If

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdDates.Rows



                Dim ds As DataSet
                Dim parms2 As New List(Of SqlParameter)
                Dim parm2(12) As SqlParameter


                If GVRow.Cells(1).Text <> "" Then
                    parm2(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(GVRow.Cells(1).Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(GVRow.Cells(2).Text, Date), "yyyy/MM/dd"))
                    parm2(4) = New SqlParameter("@subseasoncode", CType(txtseasonname.Text, String))
                    parm2(5) = New SqlParameter("@mealcode", CType(Session("MealPlans"), String))
                    parm2(6) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(7) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(8) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(9) = New SqlParameter("@supagentcode", CType(supagentcode, String))
                    parm2(10) = New SqlParameter("@promotionid", "")
                    parm2(11) = New SqlParameter("@weekdays", CType(weekdaystr, String))

                    For i = 0 To 11
                        parms2.Add(parm2(i))
                    Next


                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkplisthnew", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("plistcode")) = False Then
                                strMsg = "Price List already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("plistcode") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
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
            objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function OfferFindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        OfferFindDatePeriod = True
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
                OfferFindDatePeriod = False
                Exit Function

            End If

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdpromodates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                Dim ds As DataSet
                Dim parms2 As New List(Of SqlParameter)
                Dim parm2(12) As SqlParameter


                If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                    parm2(0) = New SqlParameter("@contractid", "")
                    parm2(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm2(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm2(4) = New SqlParameter("@subseasoncode", "")
                    parm2(5) = New SqlParameter("@mealcode", CType(Session("MealPlans"), String))
                    parm2(6) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm2(7) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(8) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(9) = New SqlParameter("@supagentcode", CType(supagentcode, String))
                    parm2(10) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                    parm2(11) = New SqlParameter("@weekdays", CType(weekdaystr, String))

                    For i = 0 To 11
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkplisthnew_offer", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("plistcode")) = False Then
                                strMsg = "Price List already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds.Tables(0).Rows(0)("plistcode") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                OfferFindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If

                End If


            Next



        Catch ex As Exception
            OfferFindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing

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
                        Dim roomexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_partymaxaccomodation(nolock) where partycode='" & hdnpartycode.Value & "'")

                        If roomexist = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Hotel Max Occupancy Adult Combination not Update Please update.');", True)
                            Exit Sub
                        End If

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
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_roomrates", mySqlConn, sqlTrans)
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
                        optionval = objUtils.GetAutoDocNo("PLIST", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_cplisthnew", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 100)).Value = CType(txtseasonname.Text.ToUpper, String) 'instead of 20, length is changed as 100 --changed by mohamed on 20/05/2018
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("MealPlans"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@commissionable", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@formulainput", SqlDbType.VarChar, 5000)).Value = DBNull.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrygroupyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)



                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed
                    ElseIf ViewState("State") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_edit_cplisthnew", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@subseascode", SqlDbType.VarChar, 100)).Value = CType(txtseasonname.Text.ToUpper, String) 'instead of 20, length is changed as 100 --changed by mohamed on 20/05/2018
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 500)).Value = CType(Replace(Session("MealPlans"), ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@commissionable", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@formulaid", SqlDbType.VarChar, 20)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@formulainput", SqlDbType.VarChar, 5000)).Value = ""
                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrygroupyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)


                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If


                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_RoomRates Where  plistcode='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_Roomrate_weekdays Where  plistcode='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    If Session("Calledfrom") = "Offers" Then

                        mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersRoomRates_Spl Where  price_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                      

                    End If

                    mySqlCmd = New SqlCommand("DELETE FROM edit_cplisthnew_offerdates  Where  plistcode='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersRoomRates_Spl  Where  price_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()




                    '------------------------------------Inserting Data weekdays
                    Dim GvRow As GridViewRow
                    For Each GvRow In grdWeekDays.Rows
                        Dim lblorder As Label = GvRow.FindControl("lblSrNo")
                        Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")
                        If chkSelect.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_cplisthnew_weekdays", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@dayoftheweek", SqlDbType.VarChar, 30)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekorder", SqlDbType.Int, 4)).Value = CType(lblorder.Text.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next


                    '------------------------------------Inserting Offer Dates
                    If Session("Calledfrom") = "Offers" Then
                        Dim kl As Integer = 1
                        For Each GvRow1 As GridViewRow In grdpromodates.Rows

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_cplisthnew_dates_offers", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure

                                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = kl
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")



                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                                kl = kl + 1
                            End If
                        Next
                    End If

                    Dim ix As Integer
                    Dim ds1 As DataSet
                    If Session("Calledfrom") = "Offers" Then

                        mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersMain_spl Where price_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersMain_spl Where  promotion_mas_id = '" & CType(hdnpromotionid.Value, String) & "' and isnull(price_id,'')='' ", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        Dim countrygroup As String = ""
                        Dim agentGroup As String = ""

                        If Session("CountryList") <> "" Then

                            countrygroup = wucCountrygroup.checkcountrylist.ToString.Trim
                        End If
                        If Session("AgentList") <> "" Then

                            agentGroup = wucCountrygroup.checkagentlist.ToString.Trim
                        End If

                        ''Value in hdn variable , so splting to get string correctly
                        'Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        'For i = 0 To arrcountry.Length - 1

                        '    If arrcountry(i) <> "" Then

                        '  ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                        '   If ds1.Tables(0).Rows.Count > 0 Then

                        'For ix = 0 To ds1.Tables(0).Rows.Count - 1


                        'hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("Offer_Start_Date")
                        'hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("Offer_End_Date")


                        For Each GvRow1 As GridViewRow In grdpromodates.Rows

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                                '### Roselin
                                mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain_spl_New", mySqlConn, sqlTrans)
                                '  mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain_spl", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure

                                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentGroup, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                mySqlCmd.Parameters.Add(New SqlParameter("@Cccode", SqlDbType.VarChar, 8000)).Value = CType(countrygroup, String)  'CType(arrcountry(i), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@createdby", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                                mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 0
                                mySqlCmd.Parameters.Add(New SqlParameter("@offerfromdate", SqlDbType.DateTime)).Value = DBNull.Value ' Format(CType(ds1.Tables(0).Rows(ix).Item("Offer_Start_Date").ToString, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@offertodate", SqlDbType.DateTime)).Value = DBNull.Value '  Format(CType(ds1.Tables(0).Rows(ix).Item("Offer_End_Date").ToString, Date), "yyyy/MM/dd")
                                mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1


                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next
                        '   Next
                        ' End If
                        'End If
                        '    Next

                    End If

                    'If Session("AgentList") <> "" Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then

                    '            ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                    '            For ix = 0 To ds1.Tables(0).Rows.Count - 1

                    '                For Each GvRow1 As GridViewRow In grdpromodates.Rows

                    '                    Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                    '                    Dim txttodate As TextBox = GvRow1.FindControl("txttodate")

                    '                    ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                    '                    If txtfromdate.Text <> "" And txttodate.Text <> "" Then


                    '                        mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain_spl", mySqlConn, sqlTrans)
                    '                        mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(i), String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast where agentcode='" & CType(arragents(i), String) & "'")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@Cccode", SqlDbType.VarChar, 100)).Value = ""
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@createdby", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 1
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@offerfromdate", SqlDbType.DateTime)).Value = Format(CType(ds1.Tables(0).Rows(ix).Item("Offer_Start_Date").ToString, Date), "yyyy/MM/dd")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@offertodate", SqlDbType.DateTime)).Value = Format(CType(ds1.Tables(0).Rows(ix).Item("Offer_End_Date").ToString, Date), "yyyy/MM/dd")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1



                    '                        mySqlCmd.ExecuteNonQuery()
                    '                        mySqlCmd.Dispose() 'command disposed
                    '                    End If
                    '                Next
                    '            Next


                    '        End If
                    '    Next

                    'End If




                    ' End If

                    mySqlCmd = New SqlCommand("DELETE FROM edit_cplisthnew_countries Where plistcode='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM edit_cplisthnew_agents Where plistcode='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                        ''Value in hdn variable , so splting to get string correctly
                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then




                                mySqlCmd = New SqlCommand("sp_add_cplisthnew_countries", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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




                                mySqlCmd = New SqlCommand("sp_add_cplisthnew_agents", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                            End If
                        Next

                    End If


                    ''''''''''''' Price detail saving
                    Dim k As Long
                    Dim j As Long = 1
                    Dim txt As TextBox
                    Dim txtunityesno As TextBox
                    ' Dim GvRow As GridViewRow
                    Dim srno As Long = 0
                    Dim hotelcategory As String = ""
                    j = 0
                    Dim m As Long = 0
                    Dim n As Long = 0
                    Dim cnt As Long = grdRoomrates.Columns.Count
                    Dim a As Long = cnt - 11
                    Dim b As Long = 0
                    Dim header As Long = 0
                    Dim heading(cnt + 1) As String
                    Dim flag As Boolean = False


                    For header = 0 To cnt - 7
                        txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                        If Not txt Is Nothing Then
                            heading(header) = txt.Text
                        End If
                    Next

                    ' Dim m As Long = 0
                    Dim txtrmtyp As TextBox
                    'Dim GvRow1 As GridViewRow
                    Dim chksel As CheckBox
                    Dim lblcode As Label
                    Dim oldrmtyp As String = ""
                    Dim oldmeal As String = ""
                    Dim rx As Integer = 0
                    Dim extrapx As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1139")

                    For Each GvRow In grdRoomrates.Rows
                        If n = 0 Then
                            For j = 0 To cnt - 8
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        If heading(j) = "Min Nights" Or heading(j) = "Booking Code" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then 'Or heading(j) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                                        Else
                                            txtrmtyp = GvRow.FindControl("txtrmtypcode")

                                            'mySqlCmd = New SqlCommand("sp_add_edit_cplistdnew", mySqlConn, sqlTrans)
                                            mySqlCmd = New SqlCommand("sp_add_edit_New_RoomRates", mySqlConn, sqlTrans)

                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Hotel_Season_ID", SqlDbType.NVarChar, 200)).Value = CType(hdnpartycode.Value + "_" + txtseasonname.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Hotel_Room_ID", SqlDbType.NVarChar, 200)).Value = CType(hdnpartycode.Value + "_" + CType(txtrmtyp.Text, String) + "_" + CType(heading(j), String) + "_" + CType(GvRow.Cells(3).Text, String), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Offer_Start_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Offer_End_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Plist_Start_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Plist_End_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Price_ID", SqlDbType.NVarChar, 400)).Value = CType(txtplistcode.Text.Trim, String) 'CType(CType(hdncontractid.Value, String) + "_" + hdnpartycode.Value + "_" + CType(GvRow.Cells(2).Text, String) + "_" + CType(heading(j), String) + "_" + CType(GvRow.Cells(3).Text, String), String)

                                            txt = GvRow.FindControl("txt" & j)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> Nothing Then
                                                    If txt.Text = "" Then
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@Unit_Price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                    Else
                                                        Select Case txt.Text
                                                            Case "N/A"
                                                                txt.Text = "-4"
                                                            Case "On Request"
                                                                txt.Text = "-5"
                                                        End Select

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@Unit_Price", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                    End If
                                                End If
                                            End If
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Adult_Price_Extra", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Extra_Pax_Price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Adult_Price_Sharing", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            txt = GvRow.FindControl("txt" & b + a + 1)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> Nothing Then
                                                    If txt.Text = "" Then
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = DBNull.Value
                                                    Else
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(txt.Text, Integer)
                                                    End If
                                                End If
                                            End If

                                            txt = GvRow.FindControl("txt" & b + a + 3)
                                            If txt Is Nothing Then
                                            Else

                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 400)).Value = ""
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 400)).Value = CType(txt.Text, String)
                                                End If

                                            End If

                                            '  mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.NVarChar, 400)).Value = CType((txt.Text), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Is_VAT", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Service_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Municipality_Per", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Tourism_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VAT_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@CreatedBy", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)

                                            'If txtrmtyp.Text <> oldrmtyp Or GvRow.Cells(3).Text <> oldmeal Then
                                            '    rx = rx + 1
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = rx
                                            'Else
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = rx
                                            'End If
                                            'oldrmtyp = txtrmtyp.Text
                                            'oldmeal = GvRow.Cells(3).Text

                                            ' mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)

                                            If txtrmtyp Is Nothing Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                            End If

                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = IIf(CType(heading(j), String) = "Extra Person Supplement", extrapx, CType(heading(j), String))
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(3).Text, String)
                                            If Session("Calledfrom") = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@ratetype", SqlDbType.VarChar, 20)).Value = ""
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@ratetype", SqlDbType.VarChar, 20)).Value = CType(Session("Calledfrom"), String)
                                            End If




                                            'txt = GvRow.FindControl("txt" & b + a + 2)
                                            'If txt Is Nothing Then
                                            'Else
                                            '    If txt.Text <> Nothing Then
                                            '        If txt.Text = "" Then
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@compalsorynights", SqlDbType.Int)).Value = DBNull.Value
                                            '        Else
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@compalsorynights", SqlDbType.Int)).Value = CType(txt.Text, Integer)
                                            '        End If
                                            '    End If
                                            'End If
                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                        End If
                                    End If
                                End If
                            Next
                            m = j
                        Else
                            k = 0

                            For j = n To (m + n) - 1
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing Then
                                        If heading(k) = "Min Nights" Or heading(k) = "Booking Code" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then 'Or heading(k) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                                        Else
                                            '   mySqlCmd = New SqlCommand("sp_add_edit_cplistdnew", mySqlConn, sqlTrans) sp_add_edit_New_RoomRates
                                            mySqlCmd = New SqlCommand("sp_add_edit_New_RoomRates", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            txtrmtyp = GvRow.FindControl("txtrmtypcode")

                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Hotel_Season_ID", SqlDbType.NVarChar, 200)).Value = CType(hdnpartycode.Value + "_" + txtseasonname.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Hotel_Room_ID", SqlDbType.NVarChar, 200)).Value = CType(hdnpartycode.Value + "_" + CType(txtrmtyp.Text, String) + "_" + CType(heading(k), String) + "_" + CType(GvRow.Cells(3).Text, String), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Offer_Start_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Offer_End_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Plist_Start_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Plist_End_Date", SqlDbType.Date)).Value = System.DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Price_ID", SqlDbType.NVarChar, 400)).Value = CType(txtplistcode.Text.Trim, String) 'CType(CType(hdncontractid.Value, String) + "_" + hdnpartycode.Value + "_" + CType(GvRow.Cells(2).Text, String) + "_" + CType(heading(k), String) + "_" + CType(GvRow.Cells(3).Text, String), String)

                                            txt = GvRow.FindControl("txt" & j)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> Nothing Then
                                                    If txt.Text = "" Then
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@Unit_Price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                    Else
                                                        Select Case txt.Text
                                                            Case "N/A"
                                                                txt.Text = "-4"
                                                            Case "On Request"
                                                                txt.Text = "-5"
                                                        End Select

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@Unit_Price", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                    End If
                                                End If
                                            End If

                                            mySqlCmd.Parameters.Add(New SqlParameter("@Adult_Price_Extra", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Extra_Pax_Price", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Adult_Price_Sharing", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            txt = GvRow.FindControl("txt" & b + a + 1)
                                            If txt Is Nothing Then
                                            Else
                                                If txt.Text <> Nothing Then
                                                    If txt.Text = "" Then
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = DBNull.Value
                                                    Else
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(txt.Text, Integer)
                                                    End If
                                                End If
                                            End If

                                            txt = GvRow.FindControl("txt" & b + a + 3)
                                            If txt Is Nothing Then
                                            Else

                                                If txt.Text = "" Then
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 400)).Value = ""
                                                Else
                                                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 400)).Value = CType(txt.Text, String)
                                                End If

                                            End If

                                            '  mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.NVarChar, 400)).Value = CType(txt.Text, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Is_VAT", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Service_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Municipality_Per", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@Tourism_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VAT_Per", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@CreatedBy", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)

                                            '  mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)

                                            If txtrmtyp Is Nothing Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                            End If

                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = IIf(CType(heading(k), String) = "Extra Person Supplement", extrapx, CType(heading(k), String))
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(3).Text, String)
                                            If Session("Calledfrom") = "" Then
                                                mySqlCmd.Parameters.Add(New SqlParameter("@ratetype", SqlDbType.VarChar, 20)).Value = ""
                                            Else
                                                mySqlCmd.Parameters.Add(New SqlParameter("@ratetype", SqlDbType.VarChar, 20)).Value = CType(Session("Calledfrom"), String)
                                            End If





                                            'If txtrmtyp.Text <> oldrmtyp Or GvRow.Cells(3).Text <> oldmeal Then
                                            '    rx = rx + 1
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = rx
                                            'Else
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = rx
                                            'End If
                                            'oldrmtyp = txtrmtyp.Text
                                            'oldmeal = GvRow.Cells(3).Text


                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)

                                            'If txtrmtyp Is Nothing Then
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                            'Else
                                            '    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                            'End If

                                            'mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = IIf(CType(heading(k), String) = "Extra Person Supplement", extrapx, CType(heading(k), String))
                                            'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(3).Text, String)

                                            'txt = GvRow.FindControl("txt" & j)
                                            'If txt Is Nothing Then
                                            'Else
                                            '    If txt.Text <> Nothing Then
                                            '        If txt.Text = "" Then
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                            '        Else
                                            '            Select Case txt.Text
                                            '                Case "N/A"
                                            '                    txt.Text = "-4"
                                            '                Case "On Request"
                                            '                    txt.Text = "-5"
                                            '            End Select

                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                            '            txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                            '            txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                            '            txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                            '        End If
                                            '    End If
                                            'End If
                                            'txt = GvRow.FindControl("txt" & b + a + 1)
                                            'If txt Is Nothing Then
                                            'Else
                                            '    If txt.Text <> Nothing Then
                                            '        If txt.Text = "" Then
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = DBNull.Value
                                            '        Else
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@nights", SqlDbType.Int)).Value = CType(txt.Text, Integer)
                                            '        End If
                                            '    End If
                                            'End If
                                            'txt = GvRow.FindControl("txt" & b + a + 2)
                                            'If txt Is Nothing Then
                                            'Else
                                            '    If txt.Text <> Nothing Then
                                            '        If txt.Text = "" Then
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@compalsorynights", SqlDbType.Int)).Value = DBNull.Value
                                            '        Else
                                            '            mySqlCmd.Parameters.Add(New SqlParameter("@compalsorynights", SqlDbType.Int)).Value = CType(txt.Text, Integer)
                                            '        End If
                                            '    End If
                                            'End If
                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed
                                        End If
                                    End If
                                End If
                                k = k + 1
                            Next

                        End If
                        b = j
                        n = j

                    Next

                    '-----------------

                    'If Session("Calledfrom") = "Offers" And CType(Session("promotypes"), String).Contains("Special Rates") = True Then

                    '    mySqlCmd = New SqlCommand("sp_update_offermain_spl", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(hdnpromofrmdate.Value, Date), "yyyy/MM/dd")
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(hdnpromotodate.Value, Date), "yyyy/MM/dd")

                    '    mySqlCmd.ExecuteNonQuery()
                    '    mySqlCmd.Dispose() 'command disposed

                    'End If



                    ''-----------------


                    ''''''''''''''''''''''''''''
                    'If Session("Calledfrom") <> "Offers" Then
                    '    '''' Insert into Minimum nights table 

                    '    mySqlCmd = New SqlCommand("sp_add_insertminimumnights", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)

                    '    mySqlCmd.ExecuteNonQuery()
                    '    mySqlCmd.Dispose() 'command disposed


                    '    ''''''''''''''''''''''''
                    'End If




                    strMsg = "Saved Succesfully!!"

                ElseIf ViewState("State") = "Delete" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_roomrates", mySqlConn, sqlTrans)
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
                    mySqlCmd = New SqlCommand("sp_del_cplisthnew", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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
                FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                ModalPopupDays.Hide()  '' Added shahul 08/08/18

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("State") = "New" Then

            btngenerate.Enabled = True
            grdmealplan.Enabled = True
            grdDates.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            txtApplicableTo.Text = ""
            txtplistcode.Text = ""
            txtseasonname.Enabled = True
            grdWeekDays.Enabled = True

            ddlmealcode.Disabled = False
            ddlrmcat.Disabled = False
            txtfillamt.Enabled = True
            btnfillamt.Enabled = True
            txtminnights.Disabled = False


        ElseIf ViewState("State") = "Copy" Then

            btngenerate.Enabled = True
            grdmealplan.Enabled = True
            grdDates.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)

            txtplistcode.Text = ""
            txtseasonname.Enabled = True
            grdWeekDays.Enabled = True

            ddlmealcode.Disabled = False
            ddlrmcat.Disabled = False
            txtfillamt.Enabled = True
            btnfillamt.Enabled = True
            txtminnights.Disabled = False

        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then

            grdDates.Enabled = False
            grdmealplan.Enabled = False
            btngenerate.Enabled = False
            grdRoomrates.Enabled = False
            btnfillminnts.Enabled = False
            btncopyratesnextrow.Enabled = False
            btnfillrate.Enabled = False
            ddlfillrow.Enabled = False

            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False
            txtseasonname.Enabled = False
            grdWeekDays.Enabled = False

            ddlmealcode.Disabled = True
            ddlrmcat.Disabled = True
            txtfillamt.Enabled = False
            btnfillamt.Enabled = False
            txtminnights.Disabled = True


        ElseIf ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then


            grdDates.Enabled = True
            grdmealplan.Enabled = True
            btngenerate.Enabled = True
            grdRoomrates.Enabled = True
            btnfillminnts.Enabled = True
            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            ddlfillrow.Enabled = True
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)
            txtseasonname.Enabled = True
            grdWeekDays.Enabled = True

            ddlmealcode.Disabled = False
            ddlrmcat.Disabled = False
            txtfillamt.Enabled = True
            btnfillamt.Enabled = True
            txtminnights.Disabled = False

        End If
    End Sub
    Private Sub ShowRecordcopyoffer(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + RefCode + "')") = "1" Then
            '    mySqlCmd = New SqlCommand(" Select * from cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'Else
            '    mySqlCmd = New SqlCommand(" Select * from edit_cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'End If
            mySqlCmd = New SqlCommand(" Select * from view_cplisthnew Where plistcode='" & RefCode & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("plistcode")) = False Then
                        txtplistcode.Text = mySqlReader("plistcode")

                    End If

                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        txtseasonname.Text = mySqlReader("subseascode")
                        filldates()

                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("countrygroupyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If

                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        'txtpromot.Text = CType(mySqlReader("promotionid"), String)
                        'txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    Else
                        'txtpromotionid.Text = ""
                        'txtpromotionname.Text = ""
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

                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_cplisthnew(nolock) where   contractid='" & hdncontractid.Value & "' and plistcode ='" & CType(RefCode, String) & "'") <> "" Then
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "UNAPPROVED"

                    'Else
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "APPROVED"
                    'End If
                    If IsDBNull(mySqlReader("mealplans")) = False Then
                        Session("MealPlans") = CType(mySqlReader("mealplans"), String)
                    End If

                    If Session("MealPlans") = "" Then
                        Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    End If


                    Dim strMealPlans As String = ""
                    Dim strCondition As String = ""
                    If Session("MealPlans") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strMealPlans = Session("MealPlans") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
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
                    grdmealplan.Visible = True
                    strSqlQry = ""



                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = "select  v.mealcode,b.mealname,1 selected ,b.rankorder from partymeal a ,mealmast b ,view_offers_meal v(nolock) where  a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  v.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b ,view_offers_meal v(nolock) where a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "


                    Else
                        strSqlQry = "select  a.mealcode,b.mealname,0 selected ,b.rankorder from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  a.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "

                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    grdmealplan.DataSource = myDS

                    If myDS.Tables(0).Rows.Count > 0 Then
                        grdmealplan.DataBind()

                    Else
                        grdmealplan.DataBind()

                    End If

                    ddlmealcode.Items.Clear()
                    ddlmealcode.Items.Add("[Select]")

                    Dim chkSelect As CheckBox
                    Dim lblselect As Label
                    Dim lblmealcode As Label
                    For Each grdRow In grdmealplan.Rows
                        chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                        lblselect = CType(grdRow.FindControl("lblselect"), Label)
                        lblmealcode = CType(grdRow.findcontrol("lblmealcode"), Label)


                        If lblselect.Text = "1" Then
                            chkSelect.Checked = True
                            '  chkSelect.Enabled = False
                            ddlmealcode.Items.Add(lblmealcode.Text)
                        End If

                    Next

                    btngenerate.Style("display") = "block"

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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
    End Sub
    Private Sub ShowRecordcopy(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + RefCode + "')") = "1" Then
            '    mySqlCmd = New SqlCommand(" Select * from cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'Else
            '    mySqlCmd = New SqlCommand(" Select * from edit_cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'End If
            mySqlCmd = New SqlCommand(" Select * from view_cplisthnew Where plistcode='" & RefCode & "' and contractid='" & hdncopycontractid.Value & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("plistcode")) = False Then
                        txtplistcode.Text = mySqlReader("plistcode")

                    End If

                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        txtseasonname.Text = mySqlReader("subseascode")
                        filldates()

                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("countrygroupyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If

                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        'txtpromot.Text = CType(mySqlReader("promotionid"), String)
                        'txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    Else
                        'txtpromotionid.Text = ""
                        'txtpromotionname.Text = ""
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

                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_cplisthnew(nolock) where   contractid='" & hdncontractid.Value & "' and plistcode ='" & CType(RefCode, String) & "'") <> "" Then
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "UNAPPROVED"

                    'Else
                    '    lblstatustext.Visible = True
                    '    lblstatus.Visible = True
                    '    lblstatus.Text = "APPROVED"
                    'End If
                    If IsDBNull(mySqlReader("mealplans")) = False Then
                        Session("MealPlans") = CType(mySqlReader("mealplans"), String)
                    End If

                    If Session("MealPlans") = "" Then
                        Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    End If


                    Dim strMealPlans As String = ""
                    Dim strCondition As String = ""
                    If Session("MealPlans") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strMealPlans = Session("MealPlans") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
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
                    grdmealplan.Visible = True
                    strSqlQry = ""



                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = "select  v.mealcode,b.mealname,1 selected ,b.rankorder from partymeal a ,mealmast b ,view_offers_meal v(nolock) where  a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  v.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b ,view_offers_meal v(nolock) where a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "


                    Else
                        strSqlQry = "select  a.mealcode,b.mealname,0 selected ,b.rankorder from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  a.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "

                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    grdmealplan.DataSource = myDS

                    If myDS.Tables(0).Rows.Count > 0 Then
                        grdmealplan.DataBind()

                    Else
                        grdmealplan.DataBind()

                    End If

                    ddlmealcode.Items.Clear()
                    ddlmealcode.Items.Add("[Select]")

                    Dim chkSelect As CheckBox
                    Dim lblselect As Label
                    Dim lblmealcode As Label
                    For Each grdRow In grdmealplan.Rows
                        chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                        lblselect = CType(grdRow.FindControl("lblselect"), Label)
                        lblmealcode = CType(grdRow.findcontrol("lblmealcode"), Label)


                        If lblselect.Text = "1" Then
                            chkSelect.Checked = True
                            '  chkSelect.Enabled = False
                            ddlmealcode.Items.Add(lblmealcode.Text)
                        End If

                    Next

                    btngenerate.Style("display") = "block"

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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
    End Sub
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open


            mySqlCmd = New SqlCommand(" Select * from view_cplisthnew Where plistcode='" & RefCode & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("plistcode")) = False Then
                        txtplistcode.Text = mySqlReader("plistcode")

                    End If

                    If IsDBNull(mySqlReader("subseascode")) = False Then
                        txtseasonname.Text = mySqlReader("subseascode")
                        filldates()

                    End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    End If
                    If IsDBNull(mySqlReader("countrygroupyesno")) = False Then
                        chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupyesno"), String) = "1", True, False)
                    Else
                        chkctrygrp.Checked = False
                    End If

                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        hdnpromotionid.Value = CType(mySqlReader("promotionid"), String)
                        'txtpromot.Text = CType(mySqlReader("promotionid"), String)
                        'txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    Else
                        'txtpromotionid.Text = ""
                        'txtpromotionname.Text = ""
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

                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_cplisthnew(nolock) where    plistcode ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
                    End If
                    If IsDBNull(mySqlReader("mealplans")) = False Then
                        Session("MealPlans") = CType(mySqlReader("mealplans"), String)
                    End If

                    If Session("MealPlans") = "" Then
                        Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    End If


                    Dim strMealPlans As String = ""
                    Dim strCondition As String = ""
                    If Session("MealPlans") Is Nothing = False Then 'strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
                        strMealPlans = Session("MealPlans") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
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
                    grdmealplan.Visible = True
                    strSqlQry = ""

                    If Session("Calledfrom") = "Offers" Then
                        strSqlQry = "select  v.mealcode,b.mealname,0 selected ,b.rankorder from partymeal a ,mealmast b,view_offers_meal v(nolock) where  a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and " _
                            & " a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  a.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b,view_offers_meal v(nolock) where a.mealcode =v.mealcode  and v.promotionid ='" & hdnpromotionid.Value & "' and  " _
                    & " a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "

                    Else
                        strSqlQry = "select  a.mealcode,b.mealname,0 selected ,b.rankorder from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and a.mealcode  not IN (" & strCondition & ") " _
                  & " union all " _
                    & " select  a.mealcode,b.mealname,1 selected ,b.rankorder  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' and  a.mealcode IN (" & strCondition & ")  order by  4 "
                    End If



                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS)
                    grdmealplan.DataSource = myDS

                    If myDS.Tables(0).Rows.Count > 0 Then
                        grdmealplan.DataBind()

                    Else
                        grdmealplan.DataBind()

                    End If
                    Dim chkSelect As CheckBox
                    Dim lblselect As Label
                    ddlmealcode.Items.Clear()
                    ddlmealcode.Items.Add("[Select]")
                    For Each grdRow In grdmealplan.Rows
                        chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                        lblselect = CType(grdRow.FindControl("lblselect"), Label)
                        Dim lblmealcode As Label = grdRow.findcontrol("lblmealcode")

                        If lblselect.Text = "1" Then
                            chkSelect.Checked = True
                            ddlmealcode.Items.Add(lblmealcode.Text)
                            '  chkSelect.Enabled = False
                        End If

                    Next

                    btngenerate.Style("display") = "block"



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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
    End Sub
#End Region
#Region "Private Sub ShowDates(ByVal RefCode As String ,ByVal subseasonname As String)"

    Private Sub ShowDates(ByVal contractid As String, ByVal subseasonname As String)
        Try
            Dim myDS As New DataSet
            grdDates.Visible = True
            strSqlQry = ""

            ' strSqlQry = "select '' clinenno, '' fromdate,'' todate "

            'strSqlQry = "select 1  clinenno, '15/08/2016' fromdate,'20/08/2016' todate union all select 2  clinenno, '21/08/2016' fromdate,'31/08/2016' todate"
            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate from contracts_seasons(nolock) where contractid='" & contractid & "' and subseasname='" & subseasonname & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
#End Region

    Private Sub FillMealplansoffers(ByVal promotionid As String)
        Try
            Dim myDS As New DataSet
            grdmealplan.Visible = True
            strSqlQry = ""

            ' strSqlQry = "select  a.mealcode,b.mealname,0 selected,b.rankorder  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' order by ISNULL(b.rankorder,99)"
            strSqlQry = "select v.mealcode as mealcode,m.mealname mealname,0 selected,m.rankorder from view_offers_meal v(nolock), partymeal p(nolock),mealmast m(nolock) where m.active=1 and v.mealcode=p.mealcode and p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' and v.promotionid='" & promotionid & "' order by isnull(m.rankorder,999)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdmealplan.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()

            Else
                grdmealplan.DataBind()

            End If
            Dim chkSelect As CheckBox


            For Each grdRow In grdmealplan.Rows
                chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                Dim chkMealAll As CheckBox = grdmealplan.HeaderRow.FindControl("chkMealAll")
                chkSelect.Checked = True
                chkMealAll.Checked = True

            Next





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub

    Private Sub FillMealplans()
        Try
            Dim myDS As New DataSet
            grdmealplan.Visible = True
            strSqlQry = ""

            strSqlQry = "select  a.mealcode,b.mealname,0 selected,b.rankorder  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "' order by ISNULL(b.rankorder,99)"

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdmealplan.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdmealplan.DataBind()

            Else
                grdmealplan.DataBind()

            End If
            Dim chkSelect As CheckBox


            For Each grdRow In grdmealplan.Rows
                chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                Dim chkMealAll As CheckBox = grdmealplan.HeaderRow.FindControl("chkMealAll")
                chkSelect.Checked = True
                chkMealAll.Checked = True

            Next


            'strSqlQry = ""

            'strSqlQry = "select  a.mealcode,b.mealname  from contracts_countries(nolock) where contractid='" & hdncontractid.Value & "'"

            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            'myDataAdapter.Fill(myDS)
            'grdmealplan.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    grdmealplan.DataBind()

            'Else
            '    grdmealplan.DataBind()

            'End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)

                    End If

                End If


                mySqlCmd.Dispose()
                mySqlReader.Close()

                mySqlConn.Close()

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


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
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        'If e.CommandName = "Page" Then Exit Sub
        'If e.CommandName = "Sort" Then Exit Sub
        'If e.CommandName = "EditRow" Then Exit Sub
        'If e.CommandName = "View" Then Exit Sub
        'If e.CommandName = "DeleteRow" Then Exit Sub
        'If e.CommandName = "Copy" Then Exit Sub
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            Dim lbltran As Label
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
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
                'PanelMain.Visible = True
                PanelMain.Style.Add("display", "block")
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                divoffer.Style.Add("display", "none")

                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                If Session("Calledfrom") = "Offers" Then

                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    'Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    ' wucCountrygroup.sbShowCountry()

                    dv_SearchResult.Style.Add("display", "none")
                    divpromodates.Style.Add("display", "block")

                    lblseason.Visible = False
                    txtseasonname.Visible = False
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Edit Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                    Page.Title = "Promotion Room Rates "

                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    lblratetype.Text = "Promotion"
                    ShowDynamicGridoffer(CType(lbltran.Text.Trim, String))
                Else
                    dv_SearchResult.Style.Add("display", "block")
                    divpromodates.Style.Add("display", "none")

                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Edit Room Rates - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Room Rates "
                    dv_SearchResult.Style.Add("display", "block")
                    divpromodates.Style.Add("display", "none")
                    lblseason.Visible = True
                    txtseasonname.Visible = True
                    lblratetype.Text = "Contract"
                    ShowDynamicGrid()
                End If



                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                btnSave.Visible = True
                btnSave.Text = "Update"



                divcopy1.Style("display") = "block"
                DisableControl()

            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                'PanelMain.Visible = True
                PanelMain.Style.Add("display", "block")
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                If Session("Calledfrom") = "Offers" Then
                    dv_SearchResult.Style.Add("display", "none")
                    divpromodates.Style.Add("display", "block")

                    lblseason.Visible = False
                    txtseasonname.Visible = False
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "View Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                    Page.Title = "Promotion Room Rates "
                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    lblratetype.Text = "Promotion"
                    ShowDynamicGridoffer(CType(lbltran.Text.Trim, String))
                Else
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "View Room Rates - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Room Rates "

                    dv_SearchResult.Style.Add("display", "block")
                    divpromodates.Style.Add("display", "none")
                    lblseason.Visible = True
                    txtseasonname.Visible = True
                    lblratetype.Text = "Contracts"
                    ShowDynamicGrid()
                End If

                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                DisableControl()
                btnSave.Visible = False

                btngenerate.Style("display") = "none"


            ElseIf e.CommandName = "DeleteRow" Then
                'PanelMain.Visible = True
                PanelMain.Style.Add("display", "block")
                Panelsearch.Enabled = False
                ViewState("State") = "Delete"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                If Session("Calledfrom") = "Offers" Then
                    lblseason.Visible = False
                    txtseasonname.Visible = False
                    dv_SearchResult.Style.Add("display", "none")
                    divpromodates.Style.Add("display", "block")

                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Delete Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                    Page.Title = "Promotion Room Rates "
                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    lblratetype.Text = "Promotion"
                    ShowDynamicGridoffer(CType(lbltran.Text.Trim, String))
                Else
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Delete Room Rates - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Room Rates "
                    dv_SearchResult.Style.Add("display", "block")
                    divpromodates.Style.Add("display", "none")
                    lblseason.Visible = True
                    txtseasonname.Visible = True
                    lblratetype.Text = "Contract"
                    ShowDynamicGrid()
                End If

                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"

                btngenerate.Style("display") = "none"

            ElseIf e.CommandName = "Copy" Then
                'PanelMain.Visible = True
                PanelMain.Style.Add("display", "block")
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                If Session("Calledfrom") = "Offers" Then
                    lblseason.Visible = False
                    txtseasonname.Visible = False
                    dv_SearchResult.Style.Add("display", "none")
                    divpromodates.Style.Add("display", "block")

                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Copy Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                    Page.Title = "Promotion Room Rates "
                    fillDategrd(grdpromodates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))
                    lblratetype.Text = "Promotion"
                    ShowDynamicGridoffer(CType(lbltran.Text.Trim, String))
                Else
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Copy Room Rates - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    lblseason.Visible = True
                    txtseasonname.Visible = True
                    dv_SearchResult.Style.Add("display", "block")
                    divpromodates.Style.Add("display", "none")
                    Page.Title = "Contract Room Rates "
                    lblratetype.Text = "Contract"
                    ShowDynamicGrid()
                End If

                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"

                btngenerate.Style("display") = "none"

                divcopy1.Style("display") = "block"
                DisableControl()
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
    'Protected Sub txtseasonname_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtseasonname.TextChanged
    '    filldates()

    'End Sub
    Private Sub ShowDynamicGridoffer(ByVal plistcode As String)
        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox
        Dim txtNTV1 As TextBox
        Dim txtVAT1 As TextBox

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
        For header = 0 To cnt - 8
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
        Dim txtrmtypcode As TextBox
        Try
            Dim extrapx As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1139")


            ''  If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + txtplistcode.Text + "')") = "1" Then
            'StrQry = "select distinct clineno,rmtypcode,mealcode from view_cplistdnew(nolock) where plistcode='" & plistcode & "'"
            ''Else
            ''StrQry = "select distinct clineno,rmtypcode,mealcode from edit_cplistdnew(nolock) where plistcode='" & txtplistcode.Text & "'"
            ''End If

            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status_offer('" + txtplistcode.Text + "')") = "1" Then
                StrQry = "select distinct rmtypcode,mealcode from New_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "'"
            Else
                StrQry = "select distinct rmtypcode,mealcode from New_edit_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "'"
            End If



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        txtrmtypcode = gvrow.FindControl("txtrmtypcode")
                        'If IsDBNull(mySqlReader("clineno")) = False Then
                        '    Linno = mySqlReader("clineno")
                        'End If
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            rmtypecode = mySqlReader("rmtypcode")
                        End If
                        If IsDBNull(mySqlReader("mealcode")) = False Then
                            mealcode = mySqlReader("mealcode")
                        End If
                        If txtrmtypcode Is Nothing = False Then
                            ' If txtrmtypcode.Text = rmtypecode And gvrow.Cells(3).Text = mealcode Then


                            'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + txtplistcode.Text + "')") = "1" Then
                            ' StrQryTemp = "select * from view_cplistdnew(nolock) where plistcode='" & plistcode & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            'Else
                            '    StrQryTemp = "select * from edit_cplistdnew(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            'End If

                            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status_offer('" + txtplistcode.Text + "')") = "1" Then
                                StrQryTemp = "select rmcatcode,Nights,Booking_Code bookingcode,isnull(Unit_Price,0) Unit_Price from New_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'Adult EB',Nights,'' bookingcode,isnull(Adult_Price_Extra,0)  Unit_Price from New_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'EXTRA PAX',Nights,'' bookingcode,isnull(Extra_pax_price,0)  Unit_Price from New_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            Else
                                StrQryTemp = "select rmcatcode,Nights,Booking_Code bookingcode,isnull(Unit_Price,0) Unit_Price from New_edit_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'Adult EB',Nights,'' bookingcode,isnull(Adult_Price_Extra,0)  Unit_Price from New_edit_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'EXTRA PAX',Nights,'' bookingcode,isnull(Extra_pax_price,0)  Unit_Price from New_edit_OffersRoomRates_Spl(nolock) where price_id='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            End If


                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("rmcatcode")) = False Then
                                        rmcatcode = myReader("rmcatcode")
                                    End If
                                    If IsDBNull(myReader("Unit_Price")) = False Then
                                        value = DecRound(myReader("Unit_Price"))
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 8
                                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                                        Else
                                            For s = 0 To grdRoomrates.Columns.Count - 8
                                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If ((headerlabel.Text = rmcatcode) Or (headerlabel.Text.Trim.ToUpper = "Extra Person Supplement".ToUpper And rmcatcode.ToString.ToUpper = extrapx.ToUpper)) Then
                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                            txtTV1 = gvrow.FindControl("txtTV" & s) 'changed by mohamed on 21/02/2018
                                                            txtNTV1 = gvrow.FindControl("txtNTV" & s)
                                                            txtVAT1 = gvrow.FindControl("txtVAT" & s)
                                                        Else
                                                            txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                            txtTV1 = gvrow.FindControl("txtTV" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex) 'changed by mohamed on 21/02/2018
                                                            txtNTV1 = gvrow.FindControl("txtNTV" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                            txtVAT1 = gvrow.FindControl("txtVAT" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                Select Case value
                                                                    Case "-4"
                                                                        value = "N/A"
                                                                    Case "-5"
                                                                        value = "On Request"
                                                                End Select
                                                                If txt.Enabled = True Then
                                                                    txt.Text = value
                                                                    Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1) 'changed by mohamed on 21/02/2018
                                                                End If

                                                                End If
                                                        End If

                                                        'changed by shahul and mohamed on 07/04/2018
                                                        'txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 7 + gvrow.RowIndex)
                                                        txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 9 + gvrow.RowIndex)

                                                        'txt = gvrow.FindControl("txt" & b + a + 1)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("nights")) = False Then
                                                                txt.Text = myReader("nights")

                                                            Else
                                                                txt.Text = ""
                                                            End If

                                                        End If

                                                        txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 8 + gvrow.RowIndex)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("bookingcode")) = False Then
                                                                txt.Text = myReader("bookingcode")
                                                            Else
                                                                txt.Text = ""
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
                            ' End If '''
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
    Private Sub ShowDynamicGrid()
        Dim j As Long = 0
        Dim txt As TextBox

        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox
        Dim txtNTV1 As TextBox
        Dim txtVAT1 As TextBox

        Dim cnt As Long = 0
        Dim gvrow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim a As Long = cnt - 11
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String

        Dim s As Long = 0
       
        For header = 0 To cnt - 8
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
        Dim txtrmtypcode As TextBox
        Try
            Dim extrapx As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1139")


            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + txtplistcode.Text + "')") = "1" Then
                StrQry = "select distinct rmtypcode,mealcode from New_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "'"
            Else
                StrQry = "select distinct rmtypcode,mealcode from New_edit_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "'"
            End If



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        txtrmtypcode = gvrow.FindControl("txtrmtypcode")
                        'If IsDBNull(mySqlReader("clineno")) = False Then
                        '    Linno = mySqlReader("clineno")
                        'End If
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            rmtypecode = mySqlReader("rmtypcode")
                        End If
                        If IsDBNull(mySqlReader("mealcode")) = False Then
                            mealcode = mySqlReader("mealcode")
                        End If
                        If txtrmtypcode Is Nothing = False Then
                            ' If txtrmtypcode.Text = rmtypecode And gvrow.Cells(3).Text = mealcode Then


                            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + txtplistcode.Text + "')") = "1" Then
                                StrQryTemp = "select rmcatcode,Nights,bookingcode,isnull(Unit_Price,0) Unit_Price from New_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'Adult EB',Nights,bookingcode,isnull(Adult_Price_Extra,0)  Unit_Price from New_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'EXTRA PAX',Nights,bookingcode,isnull(Extra_pax_price,0)  Unit_Price from New_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            Else
                                StrQryTemp = "select rmcatcode,Nights,bookingcode,isnull(Unit_Price,0) Unit_Price from New_edit_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'Adult EB',Nights,bookingcode,isnull(Adult_Price_Extra,0)  Unit_Price from New_edit_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                                StrQryTemp += " union all  select distinct 'EXTRA PAX',Nights,bookingcode,isnull(Extra_pax_price,0)  Unit_Price from New_edit_RoomRates(nolock) where plistcode='" & txtplistcode.Text & "' and rmtypcode='" & txtrmtypcode.Text & "' and mealcode='" & gvrow.Cells(3).Text & "'"
                            End If


                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("rmcatcode")) = False Then
                                        rmcatcode = myReader("rmcatcode")
                                    End If
                                    If IsDBNull(myReader("unit_price")) = False Then
                                        value = DecRound(myReader("unit_price"))
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 8
                                        If heading(j) = "Min Nights" Or heading(j) = "Booking Code" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                                        Else
                                            For s = 0 To grdRoomrates.Columns.Count - 8
                                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If ((headerlabel.Text = rmcatcode) Or (headerlabel.Text.Trim.ToUpper = "Extra Person Supplement".ToUpper And rmcatcode.ToString.ToUpper = extrapx.ToUpper)) Then
                                                        If gvrow.RowIndex = 0 Then
                                                            txt = gvrow.FindControl("txt" & s)
                                                            txtTV1 = gvrow.FindControl("txtTV" & s) 'changed by mohamed on 21/02/2018
                                                            txtNTV1 = gvrow.FindControl("txtNTV" & s)
                                                            txtVAT1 = gvrow.FindControl("txtVAT" & s)
                                                        Else
                                                            txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                            txtTV1 = gvrow.FindControl("txtTV" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex) 'changed by mohamed on 21/02/2018
                                                            txtNTV1 = gvrow.FindControl("txtNTV" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                            txtVAT1 = gvrow.FindControl("txtVAT" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + s + gvrow.RowIndex)
                                                        End If
                                                        If txt Is Nothing Then
                                                        Else
                                                            If value = "" Then
                                                                txt.Text = ""
                                                            Else
                                                                Select Case value
                                                                    Case "-4"
                                                                        value = "N/A"
                                                                    Case "-5"
                                                                        value = "On Request"
                                                                End Select
                                                                If txt.Enabled = True Then
                                                                    txt.Text = value
                                                                    Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1) 'changed by mohamed on 21/02/2018
                                                                End If

                                                            End If
                                                        End If

                                                        'changed by shahul and mohamed on 07/04/2018
                                                        'txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 7 + gvrow.RowIndex)

                                                        'txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 10 + gvrow.RowIndex)

                                                        ''txt = gvrow.FindControl("txt" & b + a + 1)
                                                        'If txt Is Nothing Then
                                                        'Else
                                                        '    If IsDBNull(myReader("nights")) = False Then
                                                        '        txt.Text = myReader("nights")

                                                        '    Else
                                                        '        txt.Text = 1
                                                        '    End If

                                                        'End If

                                                        txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 9 + gvrow.RowIndex)

                                                        'txt = gvrow.FindControl("txt" & b + a + 1)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If IsDBNull(myReader("nights")) = False Then
                                                                txt.Text = myReader("nights")

                                                            Else
                                                                txt.Text = 1
                                                            End If
                                                          
                                                        End If

                                                        txt = gvrow.FindControl("txt" & ((grdRoomrates.Columns.Count - 8) * gvrow.RowIndex) + grdRoomrates.Columns.Count - 8 + gvrow.RowIndex)
                                                        If txt Is Nothing Then
                                                        Else
                                                            '  txt.Text = myReader("bookingcode")
                                                            If IsDBNull(myReader("bookingcode")) = False Then
                                                                txt.Text = myReader("bookingcode")
                                                            Else
                                                                txt.Text = ""
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
                            ' End If '''
                        End If
                    Next
                End While
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbReaderClose(mySqlReader)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
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
    Private Sub fillweekdays(ByVal refcode As String)

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        Dim dt2 As DataSet
        Dim chk As CheckBox

        If Session("Calledfrom") = "Offers" Then
            dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_cplisthnew_weekdays where plistcode='" & refcode & "'")
        Else
            dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_cplisthnew_weekdays where plistcode='" & refcode & "'")
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
    Private Sub filldates()
        Try
            Dim myDS As New DataSet
            grdDates.Visible = True
            strSqlQry = ""

            If txtseasonname.Text <> "" Then

                strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' and seasonname='" & txtseasonname.Text & "' order by convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                grdDates.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    grdDates.DataBind()
                    btngenerate.Style("display") = "block"

                Else
                    grdDates.DataBind()

                End If
            Else
                strSqlQry = "select '' fromdate,'' todate "


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                grdDates.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    grdDates.DataBind()

                Else
                    grdDates.DataBind()

                End If
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        Dim unitcount As Integer = 0

        Try
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If Not txt Is Nothing Then
                    heading(header) = txt.Text
                End If


                If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Or heading(header) = "Booking Code" Then  'col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson"

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
                        If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Or heading(header) = "Booking Code" Then
                            'txt = GvRow.FindControl("txtHead" & j)
                            'txt.Style("text-align") = "center"


                        Else
                            txt = GvRow.FindControl("txt" & j)
                            txtunityesno = GvRow.FindControl("txtunityesno")
                            If txtunityesno.Text = "1" Then
                                unitcount = unitcount + 1
                            End If
                            If txt Is Nothing Then
                            Else
                                ' Numbers(txt)
                                If txtunityesno.Text = "1" Then
                                    ' txt.Enabled = False
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 1)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    '  Numbers(txt)
                                    If txtunityesno.Text = "1" Then
                                        '  txt.Enabled = False
                                    End If
                                End If
                            End If
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    '  Numbers(txt)
                                    If txtunityesno.Text = "1" Then
                                        ' txt.Enabled = False
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
                        If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Or heading(header) = "Booking Code" Then
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






            'For Each GvRow In grdRoomrates.Rows
            '    For j = 0 To cnt - 8
            '        Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & j)

            '        If txthead.Text = "UNIT" Then
            '            grdRoomrates.Columns(txthead.Columns).Visible = False

            '        End If

            '    Next
            'Next



        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0


            ' strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            If (ViewState("State") = "New" Or ViewState("CopyFrom") = "CopyFrom") Then
                '  mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            Else
                strSqlQry = "select count( distinct fromdate) from view_cplisthnew_offerdates(nolock) where plistcode='" & RefCode & "'"

            End If


            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdpromodates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            'mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)

            If (ViewState("State") = "New" Or ViewState("CopyFrom") = "CopyFrom") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)

            Else
                mySqlCmd = New SqlCommand("Select * from view_cplisthnew_offerdates(nolock) Where plistcode='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click


        Dim roomexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_partymaxaccomodation(nolock) where partycode='" & hdnpartycode.Value & "'")

        If roomexist = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Hotel Max Occupancy Adult Combination not Update Please update.');", True)
            Exit Sub
        End If

        Dim roomcat As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='A' and rc.active=1 and prc.partycode='" _
                     & hdnpartycode.Value & "' and prc.rmcatcode not in (select option_selected from reservation_parameters where param_id=1139)  ")


        If roomcat = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Hotel Room Category Not Selected In Master Please Update.');", True)
            Exit Sub
        End If

        ViewState("State") = "New"
        PanelMain.Style.Add("display", "block")
        lblstatus.Visible = False
        lblstatustext.Visible = False
        lblsort.Visible = False
        ddlfilldata.Visible = False
        Panelsearch.Enabled = False
        txthotelname.Enabled = False


        If Session("Calledfrom") = "Offers" Then

            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            fillDategrd(grdpromodates, True)
            ShowDatesnew(CType(hdnpromotionid.Value, String))

            filldaysgrid()
            fillweekdaysoffers(CType(hdnpromotionid.Value, String))
            FillMealplansoffers(hdnpromotionid.Value)
            btncommisioncal.Style("display") = "none"

            wucCountrygroup.Visible = True
            divcopy1.Style("display") = "none"
            btngenerate.Style("display") = "block"
            btnSave.Style("display") = "none"

            dv_SearchResult.Style.Add("display", "none")
            divpromodates.Style.Add("display", "block")
            '  btnreset.Style("display") = "none"

            btncopyratesnextrow.Visible = False
            btnfillminnts.Visible = False
            txtminnights.Visible = False
            lable12.Visible = False
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            ddlfillrow.Visible = False

            ddlmealcode.Visible = False
            ddlrmcat.Visible = False
            txtfillamt.Visible = False
            btnfillamt.Visible = False
            btnSave.Visible = True
            btnSave.Text = "Save"
            btngenerate.Enabled = True
            grdmealplan.Enabled = True
            grdWeekDays.Enabled = True

            lblHeading.Text = "New Room Rates - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "Room Rates -" + ViewState("hotelname") + "-" + hdnpromotionid.Value
            wucCountrygroup.sbShowCountry()

            lblseason.Style.Add("display", "none")
            txtseasonname.Style.Add("display", "none")
            lblratetype.Text = "Promotion"
            lblsort.Visible = True
            ddlfilldata.Visible = True

        Else
            Session("contractid") = hdncontractid.Value
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            DisableControl()
            dv_SearchResult.Style.Add("display", "block")
            divpromodates.Style.Add("display", "none")

            fillseason()
            grdDates.Visible = True
            filldaysgrid()
            FillMealplans()
            btncommisioncal.Style("display") = "none"

            wucCountrygroup.Visible = True
            divcopy1.Style("display") = "none"
            btngenerate.Style("display") = "none"
            btnSave.Style("display") = "none"
            '  btnreset.Style("display") = "none"

            btncopyratesnextrow.Visible = False
            btnfillminnts.Visible = False
            txtminnights.Visible = False
            lable12.Visible = False
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            ddlfillrow.Visible = False

            ddlmealcode.Visible = False
            ddlrmcat.Visible = False
            txtfillamt.Visible = False
            btnfillamt.Visible = False
            btnSave.Visible = True
            btnSave.Text = "Save"
            btngenerate.Enabled = True
            grdmealplan.Enabled = True
            grdWeekDays.Enabled = True

            lblHeading.Text = "New Room Rates - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "Room Rates -" + ViewState("hotelname")
            wucCountrygroup.sbShowCountry()
            lblseason.Style.Add("display", "block")
            txtseasonname.Style.Add("display", "block")
            lblratetype.Text = "Contracts"
        End If

        chkVATCalculationRequired.Checked = True
        Call sbFillTaxDetail() 'changed by mohamed on 21/02/2018
    End Sub



    Protected Sub btnreset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset.Click

        Session("MealPlans") = Nothing
        grdRoomrates.Visible = False
        grdDates.Visible = False

        'PanelMain.Visible = False
        PanelMain.Style.Add("display", "none")
        Panelsearch.Enabled = True
        lblHeading.Text = "Room Rates -" + ViewState("hotelname")
        wucCountrygroup.clearsessions()
        btngenerate.Enabled = True
        grdmealplan.Enabled = True
        txtseasonname.Text = ""
        txtseasonname.Enabled = True
        filldates()
        Page.Title = "Contract Room Rates "
        'dt = Session("GV_HotelData")
        'Dim fld2 As String = ""
        'Dim col As DataColumn

        'For i = grdRoomrates.Columns.Count - 1 To 7 Step -1
        '    grdRoomrates.Columns.RemoveAt(i)
        'Next

        'grdRoomrates.DataSource = Nothing
        'grdRoomrates.DataBind()

        Session("GV_HotelData") = Nothing
        wucCountrygroup.sbSetPageState("", "CONTRACTROOMRATES", CType(Session("ContractState"), String))
        Response.Redirect(Request.RawUrl)

        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('RoomratesWindowPostBack', '')"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
    Private Function ValidatePage() As Boolean
        ValidatePage = True
        Dim GvRow As GridViewRow
        Dim gvRow1 As GridViewRow


        If txtseasonname.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Season');", True)
            ValidatePage = False
            Exit Function
        End If

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

        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates grid cannot be blank');", True)
            ValidatePage = False
            Exit Function
        End If
        flg = False
        Dim chksel As CheckBox
        For Each gvRow1 In grdmealplan.Rows
            chksel = gvRow1.FindControl("chkSelect")
            If chksel.Checked = True Then
                flg = True
                Exit For
            End If
        Next

        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Meal Plan Cannot be Blank');", True)
            ValidatePage = False
            Exit Function
        End If

    End Function
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
            For Each gvrow In grdmealplan.Rows
                chksel = gvrow.FindControl("chkSelect")
                lblMealCode = gvrow.FindControl("lblmealcode")
                If chksel.Checked = True Then
                    mealplanstr = mealplanstr + "," + lblMealCode.Text
                    mealcount = mealcount + 1
                End If
            Next

            If mealplanstr.Length > 0 Then
                Session("MealPlans") = Right(mealplanstr, Len(mealplanstr) - 1) 'mealplanstr
                ViewState("mealcount") = mealcount
            End If


            Dim strMealPlans As String = ""
            Dim strCondition As String = ""
            If Session("MealPlans") Is Nothing Then strMealPlans = "" Else strMealPlans = Right(mealplanstr, Len(mealplanstr) - 1)
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

            If ddlmealfrom.Value <> "[Select]" Then

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True, ddlmealfrom.Value)
            Else

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True)

            End If

            If ddlmealto.Value <> "[Select]" Then

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True, ddlmealto.Value)
            Else

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True)

            End If




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'strSqlQry = "SELECT count(partyrmtyp.rmtypcode) FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " & _
            '            "INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode where(partyrmtyp.rmtypcode = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) " & _
            '            "and partyrmtyp.partycode=partymeal.partycode and partyrmtyp.partycode='" & hdnpartycode.Value & "'"

            strSqlQry = "sp_getrowsrmtypecountplist'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "' "

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


            'strSqlQry = "SELECT partyrmtyp.rmtypcode, rmtypmast.rmtypname, partymeal.mealcode,rmtypmast.remarks " _
            '                           & " FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " _
            '                           & " INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode inner join mealmast on partymeal.mealcode=mealmast.mealcode where(partyrmtyp.rmtypcode " _
            '                           & " = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) and partyrmtyp.partycode=partymeal.partycode and " _
            '                           & "partyrmtyp.partycode='" & hdnpartycode.Value & "' and partymeal.partycode='" _
            '                           & hdnpartycode.Value & "' "



            'Dim strMealPlans As String = ""
            'Dim strCondition As String = ""
            'If Session("MealPlans") Is Nothing Then strMealPlans = "" Else strMealPlans = Session("MealPlans")
            'If strMealPlans.Length > 0 Then
            '    Dim mString As String() = strMealPlans.Split(",")
            '    For i As Integer = 0 To mString.Length - 1
            '        If strCondition = "" Then
            '            strCondition = "'" & mString(i) & "'"
            '        Else
            '            strCondition &= ",'" & mString(i) & "'"
            '        End If
            '    Next
            '    strSqlQry &= " and partymeal.mealcode IN (" & strCondition & ") order by isnull(mealmast.rankorder,999),mealcode, isnull(partyrmtyp.rankord,999)"
            'Else
            '    strSqlQry &= " and partymeal.mealcode IN ('" & strMealPlans & "') order by isnull(mealmast.rankorder,999),mealcode, isnull(partyrmtyp.rankord,999)"
            'End If


            strSqlQry = "sp_getrows_plist'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "'," & ddlfilldata.SelectedIndex
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
                dr("Meal Plan") = arr_meal(row)
                dr("Price Pax") = IIf(arr_unityesno(row) = "1", IIf(arr_pricepax(row) = "0", "", arr_pricepax(row)), "")
                dr("Extra Person Supplement") = ""

                dr("unityesno") = arr_unityesno(row)
                dr("noofextraperson") = arr_extraper(row)

                dr("No.of.Nights Room Rate") = "1"
                 dr("Min Nights") = "1"
                dr("Booking Code") = ""
                dt.Rows.Add(dr)
            Next


            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            GrdRoomRatesDataBind() 'grdRoomrates.DataBind()

            Roomratesrowsetting()

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub createdatarowsoffer()
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
            For Each gvrow In grdmealplan.Rows
                chksel = gvrow.FindControl("chkSelect")
                lblMealCode = gvrow.FindControl("lblmealcode")
                If chksel.Checked = True Then
                    mealplanstr = mealplanstr + "," + lblMealCode.Text
                    mealcount = mealcount + 1
                End If
            Next

            If mealplanstr.Length > 0 Then
                Session("MealPlans") = Right(mealplanstr, Len(mealplanstr) - 1) 'mealplanstr
                ViewState("mealcount") = mealcount
            End If



            Dim strMealPlans As String = ""
            Dim strCondition As String = ""
            If Session("MealPlans") Is Nothing Then strMealPlans = "" Else strMealPlans = Right(mealplanstr, Len(mealplanstr) - 1)
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

            If ddlmealfrom.Value <> "[Select]" Then

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True, ddlmealfrom.Value)
            Else

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfrom, "mealcode", "mealcode", strSqlQry, True)

            End If

            If ddlmealto.Value <> "[Select]" Then

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ")  order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True, ddlmealto.Value)
            Else

                strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                         " from partymeal ,mealmast " & _
                          " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strCondition & ") order by partymeal.mealcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealto, "mealcode", "mealcode", strSqlQry, True)

            End If




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'strSqlQry = "SELECT count(partyrmtyp.rmtypcode) FROM partymeal INNER JOIN partyrmtyp ON partymeal.partycode = partyrmtyp.partycode " & _
            '            "INNER JOIN rmtypmast ON partyrmtyp.rmtypcode = rmtypmast.rmtypcode where(partyrmtyp.rmtypcode = rmtypmast.rmtypcode And partyrmtyp.inactive = 0) " & _
            '            "and partyrmtyp.partycode=partymeal.partycode and partyrmtyp.partycode='" & hdnpartycode.Value & "'"

            strSqlQry = "sp_getrowsrmtypecountplist_offer'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "','" & hdnpromotionid.Value & "' "

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





            strSqlQry = "sp_getrows_plist_offer'" & CType(hdnpartycode.Value, String) & "' , '" & CType(mealplanstr, String) & "'," & ddlfilldata.SelectedIndex & ",'" & hdnpromotionid.Value & "'"
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

            ' Dim offerminnights As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 minnights from view_offers_detail(nolock) where promotionid ='" & hdnpromotionid.Value & "'")
            Dim offerminnights As Integer = "1"

            For row = 0 To k - 1
                dr = dt.NewRow
                'For i = 1 To cnt - 1

                dr("Room_Type_Code") = arr_rows(row)
                dr("Room Type Name") = arr_rname(row)
                dr("Meal Plan") = arr_meal(row)
                dr("Price Pax") = IIf(arr_unityesno(row) = "1", IIf(arr_pricepax(row) = "0", "", arr_pricepax(row)), "")
                dr("Extra Person Supplement") = ""

                dr("unityesno") = arr_unityesno(row)
                dr("noofextraperson") = arr_extraper(row)

                dr("No.of.Nights Room Rate") = "1"
                dr("Min Nights") = IIf(offerminnights = 0, "1", offerminnights)
                dr("Booking Code") = ""
                dt.Rows.Add(dr)
            Next


            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            GrdRoomRatesDataBind() 'grdRoomrates.DataBind()

            Roomratesrowsetting()

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btngenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerate.Click
        If Session("Calledfrom") <> "Offers" Then
            If ValidatePage() = False Then
                Exit Sub
            End If
        End If
        Session("GV_HotelData") = Nothing

        If Session("Calledfrom") = "Offers" Then
            createdatatableoffer()
            createdatarowsoffer()
        Else
            createdatatable()
            createdatarows()
        End If



        grdRoomrates.Visible = True
        txtseasonname.Enabled = False
        lable12.Visible = True
        btncopyratesnextrow.Visible = True
        grdWeekDays.Enabled = False
        btnfillminnts.Visible = True
        txtminnights.Visible = True
        btnfillrate.Visible = True
        txtfillrate.Visible = True
        ddlfillrow.Visible = True
        divcopy1.Style("display") = "block"

        'ViewState("isclicked") = 1

        btngenerate.Enabled = False
        grdmealplan.Enabled = False

        btnSave.Style("display") = "block"
        btnreset.Style("display") = "block"

        If ViewState("State") = "Edit" Then
            ShowDynamicGrid()
        End If
        btnaddsupamount.Style("display") = "block"
        'If ViewState("commissionable") = 1 Then

        '    btncommisioncal.Style("display") = "block"
        'Else
        '    btncommisioncal.Style("display") = "none"

        'End If

        ddlmealcode.Visible = True
        ddlrmcat.Visible = True
        txtfillamt.Visible = True
        btnfillamt.Visible = True



        ddlmealcode.Items.Clear()
        ddlmealcode.Items.Add("[Select]")
        For Each gvrow In grdmealplan.Rows
            Dim chkselect As CheckBox = gvrow.findcontrol("chkSelect")
            Dim lblmealcode As Label = gvrow.findcontrol("lblmealcode")

            If chkselect.Checked = True Then
                ddlmealcode.Items.Add(lblmealcode.Text)
            End If

        Next


    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        filldates()
        lblsort.Visible = True
        ddlfilldata.Visible = True
    End Sub

    Protected Sub grdRoomrates_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdRoomrates.PreRender

    End Sub
    Protected Sub grdRoomrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRoomrates.RowDataBound

        '' Commented shahul 21/01/17 becoz its not working
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = grdRoomrates.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "4"
        '    e.Row.Attributes("onclick") = "javascript: SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript: return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If

        Dim txtunityesno As TextBox
        Dim txtnoextra As TextBox
        Dim txtrmtypcode As TextBox
        Dim unitcount1 As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sum(unityesno) from partyrmtyp where inactive=0 and partycode='" & hdnpartycode.Value & "'")

        Dim l As Integer
        Dim noextrapax As Integer = 0

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            txtunityesno = e.Row.FindControl("txtunityesno")
            txtnoextra = e.Row.FindControl("txtnoofextraperson")
            txtrmtypcode = e.Row.FindControl("txtrmtypcode")

            Dim sglColIndex As Integer = -1, dblColIndex As Integer = -1, tplColIndex As Integer = -1
            Dim maxeb As Integer = 0

            If Session("Calledfrom") = "Offers" Then

                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_partymaxacc_header where promotionid='" & hdnpromotionid.Value & "'") <> "" Then

                    maxeb = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select isnull(max(h.maxadults),0) from view_partymaxaccomodation h,view_partymaxacc_header vh where vh.tranid=h.tranid and  " _
                                         & " isnull(vh.promotionid,'')='" & hdnpromotionid.Value & "' and   " _
                                         & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "'  group by  h.pricepax  " _
                                         & " having   isnull(max(h.maxadults),0) > case when isnull(h.pricepax,0) <>0  then isnull(h.pricepax,0) else  2 end  ")

                    'noextrapax = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select distinct isnull(max(h.noofextraperson),0) from view_partymaxaccomodation h ,view_partymaxacc_header vh where vh.tranid=h.tranid and  " _
                    '          & " isnull(vh.promotionid,'')='" & hdnpromotionid.Value & "' and   " _
                    '          & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "' ")

                Else

                    maxeb = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select isnull(max(h.maxadults),0) from view_partymaxaccomodation h where " _
                                & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "'  group by  h.pricepax  " _
                                & " having   isnull(max(h.maxadults),0) > case when isnull(h.pricepax,0) <>0  then isnull(h.pricepax,0) else  2 end  ")

                    'noextrapax = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select distinct isnull(max(h.noofextraperson),0) from view_partymaxaccomodation h where " _
                    '          & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "' ")

                End If
              

            Else

                maxeb = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select isnull(max(h.maxadults),0) from view_partymaxaccomodation h where   " _
                                   & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "'  group by  h.pricepax  " _
                                   & " having   isnull(max(h.maxadults),0) > case when isnull(h.pricepax,0) <>0  then isnull(h.pricepax,0) else  2 end  ")

                'noextrapax = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select distinct isnull(max(h.noofextraperson),0) from view_partymaxaccomodation h where " _
                '            & "   h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(hdnpartycode.Value, String) & "' ")

            End If

            For i = 0 To grdRoomrates.Columns.Count - 8


                l = IIf(e.Row.RowIndex >= 1, ((grdRoomrates.Columns.Count - 7) * e.Row.RowIndex), 0)

                Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & i)

                Dim txt1 As TextBox = e.Row.FindControl("txt" & i + l)

                Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = e.Row.FindControl("txtTV" & i + l)
                txtNTV1 = e.Row.FindControl("txtNTV" & i + l)
                txtVAT1 = e.Row.FindControl("txtVAT" & i + l)

                'If txt1.Style("FieldHeader") <> "" Then
                '    If txt1.Style("FieldHeader").Contains("UNIT") Then
                '        maxeb = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), " select isnull(max(d.maxadults),0) from view_maxaccom_details d,view_partymaxaccomodation h where d.partycode =h.partycode and d.rmtypcode =h.rmtypcode  and  h.rmtypcode='" & txtrmtypcode.Text & "' and h.partycode='" & CType(Session("Contractparty"), String) & "'  group by  h.pricepax having   isnull(max(d.maxadults),0)> isnull(h.pricepax,0) ")
                '    ElseIf (Not txt1.Style("FieldHeader").Contains("Extra Person Supplement")) And (Not txt1.Style("FieldHeader").Contains("UNIT")) And (Not txt1.Style("FieldHeader").Contains("Adult EB")) Then
                '        maxeb = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(max(maxadults),0)  from view_maxaccom_details where rmtypcode='" & txtrmtypcode.Text & "' and partycode='" & CType(Session("Contractparty"), String) & "' having  isnull(max(maxadults),0) > 2")
                '    End If
                'End If


                If txt1.Style("FieldHeader") <> "" Then
                    If txt1.Style("FieldHeader").Contains("SGL") Then
                        sglColIndex = i
                    End If

                    If txt1.Style("FieldHeader").Contains("DBL") Then
                        dblColIndex = i
                    End If
                    If txt1.Style("FieldHeader").Contains("TPL") Then
                        tplColIndex = i
                    End If

                    If txtunityesno.Text = 0 Then
                        If txt1.Style("FieldHeader").Contains("UNIT") Then
                            ViewState("liUnitColumnNo") = i + 7
                            If unitcount1 = 0 Then
                                'grdRoomrates.Columns(i + 7).Visible = False
                                txt1.Enabled = False
                                txtTV1.Enabled = False 'changed by mohamed on 21/02/2018
                                txtNTV1.Enabled = False
                                txtVAT1.Enabled = False
                            Else
                                'grdRoomrates.Columns(i + 7).Visible = True
                                txt1.Enabled = False
                                txtTV1.Enabled = False 'changed by mohamed on 21/02/2018
                                txtNTV1.Enabled = False
                                txtVAT1.Enabled = False

                            End If

                        End If
                    ElseIf txtunityesno.Text = 1 Then
                        If (Not txt1.Style("FieldHeader").Contains("Extra Person Supplement")) And (Not txt1.Style("FieldHeader").Contains("UNIT")) And (Not txt1.Style("FieldHeader").Contains("Adult EB")) Then
                            txt1.Enabled = False
                            txtTV1.Enabled = False 'changed by mohamed on 21/02/2018
                            txtNTV1.Enabled = False
                            txtVAT1.Enabled = False
                            ' txt1.Text = ""
                        End If
                    End If

                    If maxeb = 0 And txt1.Style("FieldHeader").Contains("Adult EB") Then
                        ViewState("liAdulEBColumnNo") = i + 7
                        txt1.Enabled = False
                        txtTV1.Enabled = False 'changed by mohamed on 21/02/2018
                        txtNTV1.Enabled = False
                        txtVAT1.Enabled = False
                    ElseIf maxeb <> 0 And txt1.Style("FieldHeader").Contains("Adult EB") Then
                        txt1.Enabled = True
                        txtTV1.Enabled = True 'changed by mohamed on 21/02/2018
                        txtNTV1.Enabled = True
                        txtVAT1.Enabled = True

                    End If

                    If txt1.Style("FieldHeader").Contains("Extra Person Supplement") Then

                        ViewState("liExtraSuppColumnNo") = i + 7
                        If txtunityesno.Text = 1 Then 'And txtnoextra.Text <> 0 Then
                            txt1.Enabled = True
                            ViewState("lbExtraSuppRequired") = True
                            'ElseIf txtunityesno.Text = 1 And noextrapax = 0 Then 'And txtnoextra.Text <> 0 Then
                            '    txt1.Enabled = False
                            '    ViewState("lbExtraSuppRequired") = True
                        ElseIf txtunityesno.Text = 0 Then 'And txtnoextra.Text = 0 Then
                            txt1.Enabled = False

                            If unitcount1 = 0 Then
                                'grdRoomrates.Columns(i + 7).Visible = False
                            End If
                        End If
                    End If

                    'changed by mohamed on 21/02/2018
                    If txt1.Style("FieldHeader").Contains("Sr No") = False And txt1.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt1.Style("FieldHeader").Contains("Min Nights") = False And txt1.Style("FieldHeader").Contains("Pkg") = False And txt1.Style("FieldHeader").Contains("Remark") = False And txt1.Style("FieldHeader").Contains("SGL") = False And txt1.Style("FieldHeader").Contains("DBL") = False And txt1.Style("FieldHeader").Contains("TPL") = False Then 'And txt1.Style("FieldHeader").Contains("Extra Person Supplement") = False 'changed by mohamed on 28/03/2018
                        txt1.Attributes.Add("onchange", "javascript:calculateVAT('" & txt1.ClientID & "','" & txtTV1.ClientID & "','" & txtNTV1.ClientID & "','" & txtVAT1.ClientID & "');")
                    End If

                    'changed by mohamed on 21/02/2018
                    If txtTV1 IsNot Nothing Then
                        txtTV1.TabIndex = -1
                        txtNTV1.TabIndex = -1
                        txtVAT1.TabIndex = -1
                    End If

                End If
            Next

            'grdRoomrates.Columns(liExtraSuppColumnNo).Visible = lbExtraSuppRequired

            Dim txtSgl As TextBox = Nothing
            Dim txtSglTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
            Dim txtSglNTV1 As TextBox = Nothing
            Dim txtSglVAT1 As TextBox = Nothing

            Dim txtDbl As TextBox = Nothing
            Dim txtDblTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
            Dim txtDblNTV1 As TextBox = Nothing
            Dim txtDblVAT1 As TextBox = Nothing

            Dim txttpl As TextBox = Nothing
            Dim txttplTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
            Dim txttplNTV1 As TextBox = Nothing
            Dim txttplVAT1 As TextBox = Nothing

            If sglColIndex <> -1 Then
                txtSgl = e.Row.FindControl("txt" & l + sglColIndex)
                txtSglTV1 = e.Row.FindControl("txtTV" & l + sglColIndex) 'changed by mohamed on 21/02/2018
                txtSglNTV1 = e.Row.FindControl("txtNTV" & l + sglColIndex)
                txtSglVAT1 = e.Row.FindControl("txtVAT" & l + sglColIndex)
            End If
            If dblColIndex <> -1 Then
                txtDbl = e.Row.FindControl("txt" & l + dblColIndex)
                txtDblTV1 = e.Row.FindControl("txtTV" & l + dblColIndex) 'changed by mohamed on 21/02/2018
                txtDblNTV1 = e.Row.FindControl("txtNTV" & l + dblColIndex)
                txtDblVAT1 = e.Row.FindControl("txtVAT" & l + dblColIndex)
            End If
            If tplColIndex <> -1 Then
                txttpl = e.Row.FindControl("txt" & l + tplColIndex)
                txttplTV1 = e.Row.FindControl("txtTV" & l + tplColIndex) 'changed by mohamed on 21/02/2018
                txttplNTV1 = e.Row.FindControl("txtNTV" & l + tplColIndex)
                txttplVAT1 = e.Row.FindControl("txtVAT" & l + tplColIndex)
            End If

            If txttpl IsNot Nothing And txtDbl IsNot Nothing Then
                txttpl.Attributes.Add("onchange", "javascript:validatedbltpl('" & txtDbl.ClientID & "','" & txttpl.ClientID & "');calculateVAT('" & txttpl.ClientID & "','" & txttplTV1.ClientID & "','" & txttplNTV1.ClientID & "','" & txttplVAT1.ClientID & "');") 'changed by mohamed on 21/02/2018
                txtDbl.Attributes.Add("onchange", "javascript:validatedbltpl('" & txtDbl.ClientID & "','" & txttpl.ClientID & "');calculateVAT('" & txtDbl.ClientID & "','" & txtDblTV1.ClientID & "','" & txtDblNTV1.ClientID & "','" & txtDblVAT1.ClientID & "');") 'changed by mohamed on 21/02/2018
            End If

            If txtSgl IsNot Nothing And txtDbl IsNot Nothing Then
                txtSgl.Attributes.Add("onchange", "javascript:validatesgldbl('" & txtSgl.ClientID & "','" & txtDbl.ClientID & "');calculateVAT('" & txtSgl.ClientID & "','" & txtSglTV1.ClientID & "','" & txtSglNTV1.ClientID & "','" & txtSglVAT1.ClientID & "');") 'changed by mohamed on 21/02/2018
                txtDbl.Attributes.Add("onchange", "javascript:validatesgldbl('" & txtSgl.ClientID & "','" & txtDbl.ClientID & "');calculateVAT('" & txtDbl.ClientID & "','" & txtDblTV1.ClientID & "','" & txtDblNTV1.ClientID & "','" & txtDblVAT1.ClientID & "');") 'changed by mohamed on 21/02/2018
            End If


        End If


    End Sub

    Protected Sub btncopyratesnextrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyratesnextrow.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        grdRoomrates.Visible = True
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0

        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("contractroomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Try
            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                If chk.Checked = True Then
                    cnt_checked = cnt_checked + 1
                End If
            Next
            If cnt_checked = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row');", True)
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then 'Or heading(header) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                            Else
                                If pkg = 0 And heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018
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

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then 'Or heading(header) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                            Else
                                If pkg = 0 And heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018
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

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
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
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If GvRow.RowIndex = row_id + 1 Then
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then 'Or heading(header) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                            Else
                                If heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018 then 
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.Text = arr_pkg(pkg)
                                        End If
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
                                    End If
                                End If

                                If heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018 then 
                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.Text = arr_cancdays(pkg)
                                        End If
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then 'Or heading(header) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                            Else
                                If heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018 then 
                                    txt = GvRow.FindControl("txt" & b + a + 1)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.Text = arr_pkg(pkg)
                                        End If
                                    End If
                                End If

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = arr_room(room)

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
                                    End If
                                End If

                                If heading(header) <> "Extra Person Supplement" Then 'extra persion supplement is added here 'changed by mohamed on 28/03/2018 then
                                    txt = GvRow.FindControl("txt" & b + a + 2)
                                    If txt Is Nothing Then
                                    Else
                                        If txt.Text <> Nothing Then
                                            txt.Text = arr_cancdays(pkg)
                                        End If
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
            objUtils.WritErrorLog("Contractroomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnfillminnts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillminnts.Click
        Dim k As Long
        Dim j As Long = 1
        Dim txt As TextBox
        Dim txt1 As TextBox
        Dim txt2 As TextBox
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
            If txtminnights.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Nights');", True)

                Exit Sub
            End If

            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If Not txt Is Nothing Then
                    heading(header) = txt.Text
                End If

            Next
            For Each GvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            txt1 = GvRow.FindControl("txt" & b + a + 1)
                            ' txt2 = GvRow.FindControl("txt" & b + a)
                            If ddlfillrow.SelectedIndex = 0 Then
                                If txt1 Is Nothing Then
                                Else
                                    txt1.Text = txtminnights.Value
                                End If
                            Else
                                If txt Is Nothing Then
                                Else
                                    txt.Text = txtminnights.Value
                                End If
                            End If

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else
                            txt = GvRow.FindControl("txt" & b + a + 2)
                            txt1 = GvRow.FindControl("txt" & b + a + 1)
                            '  txt2 = GvRow.FindControl("txt" & b + a)
                            If ddlfillrow.SelectedIndex = 0 Then
                                If txt1 Is Nothing Then
                                Else
                                    txt1.Text = txtminnights.Value
                                End If
                            Else
                                If txt Is Nothing Then
                                Else
                                    txt.Text = txtminnights.Value
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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            fnCalculateVATValue()
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function getFillRateType(ByVal prefixText As String) As List(Of String)
        Dim promotionlist As New List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Try
            'promotionlist.Add("Free")
            'promotionlist.Add("Incl")
            'promotionlist.Add("N.Incl")

            strSqlQry = "select distinct namecode from extracodes  where type=1  and namecode like '" & Trim(prefixText) & "%' order by namecode "

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



            'promotionlist.Add("N/A")
            'promotionlist.Add("On Request")

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    Protected Sub btnapply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnapply.Click
        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        Dim mealcount As Integer
        mealcount = grdmealplan.Rows.Count
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim roomcat As String
        Dim roomcatstring As String
        Dim header As Long = 0
        Dim heading((cnt - 8) * 7 * ViewState("mealcount")) As String
        Try

            If ddlRoomfrom.Value = ddlroomto.Value Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Room Type  From and To Should not be same .');", True)
                Exit Sub
            End If

            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room((header * ViewState("mealcount")) + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Dim fnd As Integer = 0

        Dim mealcode As String
        Try
            'For Each GvRow In grdRoomrates.Rows
            '    mealcode = GvRow.Cells(3).Text
            '    If chk.Checked = True Then
            '        cnt_checked = cnt_checked + 1
            '    End If
            'Next
            'If cnt_checked = 0 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row.');", True)
            '    SetFocus(cnt_checked)
            '    Exit Sub
            'End If


            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0
            Dim txtrmtypcode As TextBox

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                mealcode = GvRow.Cells(3).Text
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If txtrmtypcode.Text = ddlRoomfrom.Value And mealcode = IIf(ddlmealfrom.Value <> "[Select]", ddlmealfrom.Value, mealcode) Then
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
                        If txtrmtypcode.Text = ddlRoomfrom.Value And mealcode = IIf(ddlmealfrom.Value <> "[Select]", ddlmealfrom.Value, mealcode) Then
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
                txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                mealcode = GvRow.Cells(3).Text
                If n = 0 Then
                    For j = 0 To cnt - 8

                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                        Else
                            ' txt = GvRow.FindControl("txt" & b + a + 3)
                            If txtrmtypcode.Text = ddlroomto.Value And mealcode = IIf(ddlmealto.Value <> "[Select]", ddlmealto.Value, mealcode) Then
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
                                                ' txt.Text = arr_room(room)
                                                txt.Text = arr_room(room) + Val(txtamt1.Value)
                                                arr_room(room) = txt.Text
                                            End If
                                        End If
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
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else
                            If txtrmtypcode.Text = ddlroomto.Value And mealcode = IIf(ddlmealto.Value <> "[Select]", ddlmealto.Value, mealcode) Then
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
                                            txt.Text = arr_room(room) + Val(txtamt1.Value)
                                            arr_room(room) = txt.Text
                                        End If
                                        'End If
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
                room = 0
                validprice = 0
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnaddsupamount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddsupamount.Click
        ShowRoomtypes.Style("height") = 200
        gv_Showroomtypes.Visible = False

        Dim strmeal As String = mealplancheck()

        If ddlRoomfromnew.Value <> "[Select]" Then

            strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                     " from partyrmtyp  " & _
                      " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomfromnew, "rmtypname", "rmtypcode", strSqlQry, True, ddlRoomfrom.Value)
        Else

            strSqlQry = " select  partyrmtyp.rmtypname ,partyrmtyp.rmtypcode " & _
                     " from partyrmtyp  " & _
                      " where  partyrmtyp.inactive=0 and partycode='" + hdnpartycode.Value + "' order by rankord"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomfromnew, "rmtypname", "rmtypcode", strSqlQry, True)

        End If

        If ddlmealfromnew.Value <> "[Select]" Then

            strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                     " from partymeal ,mealmast " & _
                      " where partymeal.mealcode=mealmast.mealcode and  partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strmeal & ")  order by partymeal.mealcode"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfromnew, "mealcode", "mealcode", strSqlQry, True, ddlmealfrom.Value)
        Else

            strSqlQry = " select  mealmast.mealcode ,partymeal.mealcode " & _
                     " from partymeal ,mealmast " & _
                      " where  partymeal.mealcode=mealmast.mealcode and partycode='" + hdnpartycode.Value + "' and partymeal.mealcode IN (" & strmeal & ") order by partymeal.mealcode"
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmealfromnew, "mealcode", "mealcode", strSqlQry, True)

        End If

        Call fnCalculateVATValue()

        ModalExtraPopup.Show()
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        ModalExtraPopup.Hide()
    End Sub

    Protected Sub gv_Showroomtypes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Showroomtypes.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim txtsupamount As TextBox = e.Row.FindControl("txtsupamount")
            Numberssrvctrl(txtsupamount)
        End If
    End Sub
    Private Function mealplancheck() As String

        Dim k As Long = 0
        Dim mealplanstr As String = ""
        Dim lblMealCode As Label
        Dim chksel As CheckBox
        Dim strCondition As String = ""
        Dim mealcount As Integer = 0
        Dim strMealPlans As String = ""

        If Session("MealPlans") Is Nothing Then
            Session("MealPlans") = Nothing
            For Each gvrow In grdmealplan.Rows
                chksel = gvrow.FindControl("chkSelect")
                lblMealCode = gvrow.FindControl("lblmealcode")
                If chksel.Checked = True Then
                    mealplanstr = mealplanstr + "," + lblMealCode.Text
                    mealcount = mealcount + 1
                End If
            Next

            Session("MealPlans") = mealplanstr
            ViewState("mealcount") = mealcount




            If Session("MealPlans") Is Nothing Then strMealPlans = "" Else strMealPlans = Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
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
        Else
            strMealPlans = Session("MealPlans") ' Right(Session("MealPlans"), Len(Session("MealPlans")) - 1)
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

        mealplancheck = strCondition

        Return mealplancheck
    End Function
    Protected Sub btnfillrooms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillrooms.Click


        If ddlRoomfromnew.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Room Type  ');", True)
            GoTo showpopup

        End If
        If ddlmealfromnew.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Meal  ');", True)
            GoTo showpopup
        End If
        ShowRoomtypes.Style("height") = 500

        gv_Showroomtypes.Visible = True
        btnok.Style("display") = "block"
        Dim strmeal As String = mealplancheck()


        Dim MyDs As New DataTable
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        strSqlQry = "select pr.rmtypcode,pr.rmtypname,p.mealcode  from  partyrmtyp pr(nolock),partymeal p(nolock),mealmast m(nolock)  where  m.mealcode =p.mealcode  and  pr.partycode =p.partycode  and pr.inactive=0 " _
              & " and pr.partycode='" & hdnpartycode.Value & "' and p.mealcode  IN (" & strmeal & ") and  pr.rmtypcode <>'" & ddlRoomfromnew.Value & "' and p.mealcode='" & ddlmealfromnew.Value & "'   order by isnull(m.rankorder,999),p.mealcode, isnull(pr.rankord,999)"



        myCommand = New SqlCommand(strSqlQry, SqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True

        Else

            gv_Showroomtypes.Visible = False

        End If

showpopup:
        ModalExtraPopup.Show()
    End Sub

    Protected Sub btnok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnok.Click
        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        Dim j As Long = 1
        Dim txt As TextBox
        Dim cnt As Long
        Dim GvRow As GridViewRow

        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        Dim mealcount As Integer
        mealcount = grdmealplan.Rows.Count
        cnt = grdRoomrates.Columns.Count
        Dim n As Long = 0
        Dim k As Long = 0
        Dim roomcat As String
        Dim roomcatstring As String
        Dim header As Long = 0
        Dim heading((cnt - 8) * 7 * ViewState("mealcount")) As String
        Try

            'If ddlRoomfrom.Value = ddlroomto.Value Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Room Type  From and To Should not be same .');", True)
            '    Exit Sub
            'End If

            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room((header * ViewState("mealcount")) + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
        Dim cnt_checked As Long
        'Dim GvRow1 As GridViewRow
        Dim fnd As Integer = 0

        Dim mealcode As String
        Try
            'For Each GvRow In grdRoomrates.Rows
            '    mealcode = GvRow.Cells(3).Text
            '    If chk.Checked = True Then
            '        cnt_checked = cnt_checked + 1
            '    End If
            'Next
            'If cnt_checked = 0 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row.');", True)
            '    SetFocus(cnt_checked)
            '    Exit Sub
            'End If


            Dim arr(3) As String
            Dim arr_pkg(3) As String
            Dim arr_cancdays(3) As String

            Dim room As Long = 0
            Dim row_id As Long
            Dim pkg As Long = 0
            Dim txtrmtypcode As TextBox

            For Each GvRow In grdRoomrates.Rows
                chk = GvRow.FindControl("ChkSelect")
                txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                mealcode = GvRow.Cells(3).Text
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If txtrmtypcode.Text = ddlRoomfromnew.Value And mealcode = IIf(ddlmealfromnew.Value <> "[Select]", ddlmealfromnew.Value, mealcode) Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                            Else

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing And Val(txt.Text) > 0 Then
                                        arr_room(room) = txt.Text
                                    End If
                                End If

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
                        If txtrmtypcode.Text = ddlRoomfromnew.Value And mealcode = IIf(ddlmealfromnew.Value <> "[Select]", ddlmealfromnew.Value, mealcode) Then
                            row_id = GvRow.RowIndex
                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                            Else

                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    If txt.Text <> Nothing And Val(txt.Text) > 0 Then
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
            Dim gvrow1 As GridViewRow
            Dim txtrmtyp As Label
            Dim txtsupamt As TextBox
            Dim meal As String = ""
            room = 0
            'pkg = 0
            n = 0
            b = 0
            validprice = 0
            Dim ds As DataSet
            For Each GvRow In grdRoomrates.Rows
                txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                mealcode = GvRow.Cells(3).Text
                If n = 0 Then
                    For j = 0 To cnt - 8

                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Or heading(j) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 'check here again
                        Else


                            For Each gvrow1 In gv_Showroomtypes.Rows
                                txtrmtyp = gvrow1.FindControl("txtrmtypcode")
                                meal = gvrow1.Cells(3).Text
                                txtsupamt = gvrow1.FindControl("txtsupamount")

                                If txtrmtypcode.Text = txtrmtyp.Text And mealcode = meal Then
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
                                                ' If validprice = 1 Then
                                                ' txt.Text = arr_room(room)
                                                If arr_room(room) <> 0 And Val(arr_room(room)) > 0 Then
                                                    txt.Text = arr_room(room) + Val(txtsupamt.Text)


                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                                    If txtTV1 IsNot Nothing Then
                                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                                    End If
                                                End If
                                                ' arr_room(room) = txt.Text
                                                'End If
                                            End If
                                        End If

                                    End If
                                    room = room + 1
                                End If


                            Next

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Or heading(k) = "Extra Person Supplement" Then 'changed by mohamed on 28/03/2018 'check here again
                        Else

                            For Each gvrow1 In gv_Showroomtypes.Rows
                                txtrmtyp = gvrow1.FindControl("txtrmtypcode")
                                meal = gvrow1.Cells(3).Text
                                txtsupamt = gvrow1.FindControl("txtsupamount")
                                If txtrmtypcode.Text = txtrmtyp.Text And mealcode = meal Then
                                    txt = GvRow.FindControl("txt" & j)
                                    If txt Is Nothing Then
                                    Else
                                        'Check the price exists in the line
                                        roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast(nolock) where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                        If heading(room) = roomcatstring Then
                                            If Val(txt.Text) <> "0" Or txt.Text <> "" Then
                                                validprice = 1
                                            End If
                                        End If
                                        roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast(nolock) where allotreqd='Yes'  and rmcatcode='" & Trim(heading(room)) & "'")
                                        If heading(room) = roomcat Then
                                            'If validprice = 1 Then
                                            If txt.Enabled = True And heading(room) <> "UNIT" Then
                                                If arr_room(room) <> 0 And Val(arr_room(room)) > 0 Then
                                                    txt.Text = arr_room(room) + Val(txtsupamt.Text)


                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                                    If txtTV1 IsNot Nothing Then
                                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                                    End If
                                                End If
                                                'arr_room(room) = txt.Text
                                            ElseIf txt.Enabled = True And heading(room) = "UNIT" Then
                                                If arr_room(room + 2) <> 0 And Val(arr_room(room + 2)) > 0 Then
                                                    txt.Text = arr_room(room + 2) + Val(txtsupamt.Text)


                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                                    If txtTV1 IsNot Nothing Then
                                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                                    End If
                                                End If
                                            End If
                                            'End If
                                        End If
                                    End If
                                    room = room + 1
                                End If
                            Next

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
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btncommisioncal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncommisioncal.Click
        ''''''''''''' Price detail saving
        Dim k As Long
        Dim j As Long = 1
        Dim txt As TextBox
        Dim txtunityesno As TextBox
        ' Dim GvRow As GridViewRow
        Dim srno As Long = 0
        Dim hotelcategory As String = ""
        j = 0
        Dim m As Long = 0
        Dim n As Long = 0
        Dim cnt As Long = grdRoomrates.Columns.Count
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False


        For header = 0 To cnt - 8
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If Not txt Is Nothing Then
                heading(header) = txt.Text
            End If
        Next

        ' Dim m As Long = 0
        Dim txtrmtyp As TextBox
        'Dim GvRow1 As GridViewRow
        Dim chksel As CheckBox
        Dim lblcode As Label
        Dim oldrmtyp As String = ""
        Dim oldmeal As String = ""
        Dim rx As Integer = 0

        Dim ratesstring As String = ""
        Dim rates As String = ""

        For Each GvRow In grdRoomrates.Rows
            If n = 0 Then
                For j = 0 To cnt - 8
                    txt = GvRow.FindControl("txt" & j)
                    If txt Is Nothing Then
                    Else
                        If txt.Text <> Nothing Then
                            If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                            Else
                                txtrmtyp = GvRow.FindControl("txtrmtypcode")
                                rates = txt.Text
                                Select Case txt.Text
                                    Case "N/A"
                                        rates = "-4"
                                    Case "On Request"
                                        rates = "-5"
                                End Select
                                ratesstring = ratesstring + ";" + txtrmtyp.Text + "," + CType(GvRow.Cells(3).Text, String) + "," + CType(heading(j), String) + "," + CType(CType(rates, Decimal), String)


                            End If
                        End If
                    End If
                Next
                m = j
            Else
                k = 0

                For j = n To (m + n) - 1
                    txt = GvRow.FindControl("txt" & j)
                    If txt Is Nothing Then
                    Else
                        If txt.Text <> Nothing Then
                            If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                            Else


                                txtrmtyp = GvRow.FindControl("txtrmtypcode")
                                rates = txt.Text
                                Select Case txt.Text
                                    Case "N/A"
                                        rates = "-4"
                                    Case "On Request"
                                        rates = "-5"
                                End Select

                                ratesstring = ratesstring + ";" + txtrmtyp.Text + "," + CType(GvRow.Cells(3).Text, String) + "," + CType(heading(k), String) + "," + CType(CType(rates, Decimal), String)


                            End If
                        End If
                    End If
                    k = k + 1
                Next

            End If
            b = j
            n = j

        Next
        grdviewrates.Columns(2).Visible = True
        grdviewrates.Columns(3).Visible = True
        grdviewrates.Columns(4).Visible = True
        grdviewrates.Columns(5).Visible = True
        If ratesstring.Length > 0 Then
            ratesstring = Right(ratesstring, Len(ratesstring) - 1)
            Dim formula As String = ""
            Dim formulaname As String = ""
            Dim sqlstr As String = ""

            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select   distinct formulaid,stuff((select ';' + u.term1 + ',' +convert(varchar(100),u.value) from view_contractcommission u(nolock) where u.term1 = term1 and u.contractid='" & hdncontractid.Value & "' order by u.clineno for xml path('')),1,1,'') formulaname  from view_contractcommission(nolock) where contractid ='" & hdncontractid.Value & "'group by formulaid ")
            If ds.Tables(0).Rows.Count > 0 Then
                formula = ds.Tables(0).Rows(0).Item("formulaid")
                formulaname = ds.Tables(0).Rows(0).Item("formulaname")
            End If

            Dim myds As New DataSet

            sqlstr = ""
            sqlstr = "sp_calculate_commission_viewrates'" & CType(ratesstring, String) & "' , '" & CType(formula, String) & "','" & CType(formulaname, String) & "','" & CType(hdnpartycode.Value, String) & "' "

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(sqlstr, SqlConn)
            myDataAdapter.Fill(myds)
            grdviewrates.DataSource = myds

            If myds.Tables(0).Rows.Count > 0 Then
                grdviewrates.DataBind()
            Else
                grdviewrates.PageIndex = 0
                grdviewrates.DataBind()

            End If
        End If

        If grdviewrates.Rows.Count > 0 Then
            Dim unitcol As Boolean = False
            Dim sglcol As Boolean = False
            Dim dblcol As Boolean = False
            Dim tplcol As Boolean = False
            For Each gvrow In grdviewrates.Rows

                Dim lblunit As Label = gvrow.findcontrol("lblunit")
                Dim lblsgl As Label = gvrow.findcontrol("lblsgl")
                Dim lbldbl As Label = gvrow.findcontrol("lbldbl")
                Dim lbltpl As Label = gvrow.findcontrol("lbltpl")


                If lblunit.Text <> "" Then
                    unitcol = True
                End If
                If lblsgl.Text <> "" Then
                    sglcol = True
                End If
                If lbldbl.Text <> "" Then
                    dblcol = True
                End If
                If lbltpl.Text <> "" Then
                    tplcol = True
                End If


            Next
            If unitcol = False Then
                grdviewrates.Columns(2).Visible = False
            Else
                grdviewrates.Columns(2).Visible = True
            End If
            If sglcol = False Then
                grdviewrates.Columns(3).Visible = False
            Else
                grdviewrates.Columns(3).Visible = True
            End If
            If dblcol = False Then
                grdviewrates.Columns(4).Visible = False
            Else
                grdviewrates.Columns(4).Visible = True
            End If
            If tplcol = False Then
                grdviewrates.Columns(5).Visible = False
            Else
                grdviewrates.Columns(5).Visible = True
            End If

        End If


        ModalViewrates.Show()
    End Sub

    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            If Session("Calledfrom") = "Offers" Then


                strSqlQry = "sp_show_copyroomrates  '' , '" & CType(hdnpartycode.Value, String) & "'"

                'strSqlQry = "select h.contractid,h.plistcode,h.subseascode as seasoncode,convert(varchar(10),min(cs.fromdate),103) fromdate, convert(varchar(10),max(cs.todate),103) todate,h.applicableto , " _
                '    & " dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek  from view_cplisthnew h(nolock), view_contractseasons_rate cs(nolock) ,view_contracts_search s(nolock)  where  " _
                '    & " isnull(s.withdraw,0)=0  and s.contractid=cs.contractid and  h.partycode='" & hdnpartycode.Value & "' and h.contractid=cs.contractid and  h.subseascode =cs.seasonname  group by h.contractid,h.plistcode,h.subseascode,h.applicableto"
            Else

                ' commented by rosalin 2019-10-21
                strSqlQry = "sp_show_copyroomrates'" & CType(hdncontractid.Value, String) & "' , '" & CType(hdnpartycode.Value, String) & "'"

                'strSqlQry = "select h.contractid,h.plistcode,h.subseascode as seasoncode,convert(varchar(10),min(cs.fromdate),103) fromdate, convert(varchar(10),max(cs.todate),103) todate,h.applicableto ,dbo.fn_get_weekdays_fromtable('view_cplisthnew_weekdays',h.plistcode) DaysoftheWeek  " _
                '    & " from view_cplisthnew h(nolock), view_contractseasons_rate cs(nolock), view_contracts_search d(nolock) where isnull(d.withdraw,0)=0  and d.contractid=cs.contractid and h.partycode='" & hdnpartycode.Value & "' and h.contractid=cs.contractid and  h.subseascode =cs.seasonname and h.contractid<>'" & hdncontractid.Value & _
                '"' group by h.contractid,h.plistcode,h.subseascode,h.applicableto"


            End If





            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(myds)
            grdcopyrates.DataSource = myds

            If myds.Tables(0).Rows.Count > 0 Then
                grdcopyrates.DataBind()
            Else
                grdcopyrates.PageIndex = 0
                grdcopyrates.DataBind()

            End If


            ModalCopyrates.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Protected Sub grdcopyrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdcopyrates.RowCommand

        Try
            Dim lbltran As Label
            Dim lblcontract As Label
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            lbltran = grdcopyrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            lblcontract = grdcopyrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then
                hdncopycontractid.Value = CType(lblcontract.Text, String)
                PanelMain.Style.Add("display", "block")
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecordcopy(CType(lbltran.Text.Trim, String))

                'If Session("Calledfrom") = "Offers" Then
                '    ViewState("CopyFrom") = "CopyFrom"
                '    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                '    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                '    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                '    wucCountrygroup.sbShowCountry()

                '    dv_SearchResult.Style.Add("display", "none")
                '    divpromodates.Style.Add("display", "block")

                '    lblseason.Visible = False
                '    txtseasonname.Visible = False
                '    createdatatableoffer()
                '    createdatarowsoffer()
                '    lblHeading.Text = "Edit Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                '    Page.Title = "Promotion Room Rates "

                '    fillDategrd(grdpromodates, True)
                '    ShowDatesnew(CType(hdnpromotionid.Value, String))
                '    lblratetype.Text = "Promotion"
                '    filldaysgrid()
                '    fillweekdaysoffers(CType(hdnpromotionid.Value, String))

                '    ShowDynamicGridoffer(CType(lbltran.Text, String))
                '    btnSave.Visible = True
                '    btnSave.Text = "Save"
                '    btnSave.Style("display") = "block"
                'Else



                wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
                createdatatable()
                createdatarows()
                ShowDynamicGrid()
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbShowCountry()
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Room Rates - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Contract Room Rates "


                btnaddsupamount.Style("display") = "block"
                divcopy1.Style("display") = "block"
                btnSave.Style("display") = "block"
                fillseason()
                ' End If

                btngenerate.Style("display") = "none"

            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomRates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Protected Sub grdviewrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdviewrates.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then

                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = DecRound(e.Row.Cells(2).Text)
                    Select Case e.Row.Cells(2).Text
                        Case "-5"
                            e.Row.Cells(2).Text = "On Request"
                        Case "-4"
                            e.Row.Cells(2).Text = "N/A"
                    End Select
                End If
                If e.Row.Cells(3).Text <> "&nbsp;" Then
                    e.Row.Cells(3).Text = DecRound(e.Row.Cells(3).Text)
                    Select Case e.Row.Cells(3).Text
                        Case "-5"
                            e.Row.Cells(3).Text = "On Request"
                        Case "-4"
                            e.Row.Cells(3).Text = "N/A"
                    End Select
                End If
                If e.Row.Cells(4).Text <> "&nbsp;" Then
                    e.Row.Cells(4).Text = DecRound(e.Row.Cells(4).Text)

                    Select Case e.Row.Cells(4).Text
                        Case "-5"
                            e.Row.Cells(4).Text = "On Request"
                        Case "-4"
                            e.Row.Cells(4).Text = "N/A"
                    End Select
                End If
                If e.Row.Cells(5).Text <> "&nbsp;" Then
                    e.Row.Cells(5).Text = DecRound(e.Row.Cells(5).Text)

                    Select Case e.Row.Cells(5).Text
                        Case "-5"
                            e.Row.Cells(5).Text = "On Request"
                        Case "-4"
                            e.Row.Cells(5).Text = "N/A"
                    End Select

                End If




            End If


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnclear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclear.Click
        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

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

        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Try
            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                heading(header) = txt.Text
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("contractroomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room(header + 1) As String
        Dim m As Long = 0
        Dim a As Long = cnt - 10
        Dim b As Long = 0

        Dim chk As HtmlInputCheckBox
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
                    For j = 0 To cnt - 8
                        'If chk.Checked = True Then
                        row_id = GvRow.RowIndex
                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then 'Or heading(j) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                        Else
                            'If pkg = 0 Then
                            '    txt = GvRow.FindControl("txt" & b + a + 1)
                            '    If txt Is Nothing Then
                            '    Else
                            '        If txt.Text <> Nothing Then
                            '            arr_pkg(pkg) = txt.Text
                            '        End If
                            '    End If

                            '    txt = GvRow.FindControl("txt" & b + a + 2)
                            '    If txt Is Nothing Then
                            '    Else
                            '        If txt.Text <> Nothing Then
                            '            arr_cancdays(pkg) = txt.Text
                            '        End If
                            '    End If

                            'End If
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    ' arr_room(room) = txt.Text
                                    txt.Text = ""

                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                    If txtTV1 IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                        End If
                        'End If '''
                    Next

                    m = j
                    'a = j
                Else
                    k = 0
                    pkg = 0
                    For j = n To (m + n) - 1
                        ' If chk.Checked = True Then
                        row_id = GvRow.RowIndex
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then 'Or heading(k) = "Extra Person Supplement" 'changed by mohamed on 28/03/2018
                        Else
                            'If pkg = 0 Then
                            '    txt = GvRow.FindControl("txt" & b + a + 1)
                            '    If txt Is Nothing Then
                            '    Else
                            '        If txt.Text <> Nothing Then
                            '            arr_pkg(pkg) = txt.Text
                            '        End If
                            '    End If

                            '    txt = GvRow.FindControl("txt" & b + a + 2)
                            '    If txt Is Nothing Then
                            '    Else
                            '        If txt.Text <> Nothing Then
                            '            arr_cancdays(pkg) = txt.Text
                            '        End If
                            '    End If
                            'End If
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If txt.Text <> Nothing Then
                                    'arr_room(room) = txt.Text
                                    txt.Text = ""

                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                    If txtTV1 IsNot Nothing Then
                                        Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                    End If
                                End If
                            End If
                            pkg = pkg + 1
                            room = room + 1
                        End If
                        'End If ''''
                        k = k + 1
                    Next

                End If

                b = j
                n = j
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("Contractroomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnfillamt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillamt.Click
        Dim k As Long
        Dim j As Long = 1
        Dim txt As TextBox
        Dim txt1 As TextBox
        Dim txt2 As TextBox
        Dim GvRow As GridViewRow
        j = 0
        Dim m As Long
        Dim n As Long = 0
        Dim cnt As Long = grdRoomrates.Columns.Count
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String

        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        Try
            If txtfillamt.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter Rate');", True)

                Exit Sub
            End If

            For header = 0 To cnt - 8
                txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
                If Not txt Is Nothing Then
                    heading(header) = txt.Text
                End If

            Next
            For Each GvRow In grdRoomrates.Rows
                If n = 0 Then
                    For j = 0 To cnt - 8
                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                        Else
                            txt = GvRow.FindControl("txt" & j)


                            If ddlmealcode.Value = CType(GvRow.Cells(3).Text, String) And heading(j) = ddlrmcat.Value Then
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = txtfillamt.Text

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
                                    End If
                                End If

                            End If

                        End If
                    Next
                    m = j
                Else
                    k = 0
                    For j = n To (m + n) - 1
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else

                            txt = GvRow.FindControl("txt" & j)
                            If ddlmealcode.Value = CType(GvRow.Cells(3).Text, String) And heading(k) = ddlrmcat.Value Then
                                If txt Is Nothing Then
                                Else
                                    If txt.Enabled = True Then
                                        txt.Text = txtfillamt.Text

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
                                    End If
                                End If

                            End If
                        End If
                        k = k + 1
                    Next
                End If
                b = j
                n = j
            Next

            fnCalculateVATValue()
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub sortgvsearch()

        If Session("Calledfrom") = "Offers" Then

            Select Case ddlorder.SelectedIndex
                Case 0
                    FillGrid("plistcode", hdnpromotionid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
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
                    FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
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
    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub grdpromodates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdpromodates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
                Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)


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

                PanelMain.Style.Add("display", "block")
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))
                ShowRecordcopyoffer(CType(lblplistcode.Text.Trim, String))


                Panelsearch.Enabled = False

                ViewState("CopyFrom") = "CopyFrom"
                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)

                dv_SearchResult.Style.Add("display", "none")
                divpromodates.Style.Add("display", "block")
                lblseason.Visible = False
                txtseasonname.Visible = False
                createdatatableoffer()
                createdatarowsoffer()
                lblHeading.Text = "Copy Room Rates - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Promotion Room Rates "

                fillDategrd(grdpromodates, True)
                ShowDatesnew(CType(hdnpromotionid.Value, String))
                lblratetype.Text = "Promotion"

                filldaysgrid()
                fillweekdays(CType(hdnpromotionid.Value, String))

                ' Rosalin 2019-11-11
                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")

                ' wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, ViewState("State"))
                'Session("isAutoTick_wuccountrygroupusercontrol") = 1

                wucCountrygroup.sbShowCountry()
                ShowDynamicGridoffer(CType(lblplistcode.Text, String))
                btnSave.Visible = True
                btnSave.Text = "Save"
                btnSave.Style("display") = "block"

                btngenerate.Style("display") = "none"


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub grdpromotion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdpromotion.RowDataBound

    End Sub

    'changed by mohamed on 21/02/2018
    Function fnCalculateVATValue() As Boolean
        Dim l As Integer
        For Each grdRow As GridViewRow In grdRoomrates.Rows
            If (grdRow.RowType = DataControlRowType.DataRow) Then


                For i = 0 To grdRoomrates.Columns.Count - 8
                    l = IIf(grdRow.RowIndex >= 1, ((grdRoomrates.Columns.Count - 7) * grdRow.RowIndex), 0)

                    Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & i)

                    Dim txt1 As TextBox = grdRow.FindControl("txt" & i + l)
                    Dim txtTV1 As TextBox = Nothing
                    Dim txtNTV1 As TextBox = Nothing
                    Dim txtVAT1 As TextBox = Nothing
                    txtTV1 = grdRow.FindControl("txtTV" & i + l)
                    txtNTV1 = grdRow.FindControl("txtNTV" & i + l)
                    txtVAT1 = grdRow.FindControl("txtVAT" & i + l)
                    If txt1 IsNot Nothing Then
                        If txt1.Style("FieldHeader") <> "" Then
                            If txt1.Style("FieldHeader").Contains("Sr No") = False And txt1.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt1.Style("FieldHeader").Contains("Min Nights") = False And txt1.Style("FieldHeader").Contains("Pkg") = False And txt1.Style("FieldHeader").Contains("Remark") = False Then 'And txt1.Style("FieldHeader").Contains("Extra Person Supplement") = False 'changed by mohamed on 28/03/2018
                                Call assignVatValueToTextBox(txt1, txtTV1, txtNTV1, txtVAT1)
                            End If
                        End If
                    End If
                Next i
            End If
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
            txtNTV1.Text = "" 'IIf(txt1.Text.Trim = "", "", 0)
            txtVAT1.Text = "" 'IIf(txt1.Text.Trim = "", "", 0)
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
        strSqlQry = "execute sp_get_taxdetail_frommaster '" & hdnpartycode.Value & "',5104" '"select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'"
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

    'Protected Sub btnCalculateVATInclusive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculateVATInclusive.Click
    '    Try
    '        Dim l As Integer
    '        For Each grdRow As GridViewRow In grdRoomrates.Rows
    '            If (grdRow.RowType = DataControlRowType.DataRow) Then


    '                For i = 0 To grdRoomrates.Columns.Count - 8
    '                    l = IIf(grdRow.RowIndex >= 1, ((grdRoomrates.Columns.Count - 7) * grdRow.RowIndex), 0)

    '                    Dim txthead As TextBox = grdRoomrates.HeaderRow.FindControl("txtHead" & i)

    '                    Dim txt1 As TextBox = grdRow.FindControl("txt" & i + l)
    '                    Dim txtTV1 As TextBox = Nothing
    '                    Dim txtNTV1 As TextBox = Nothing
    '                    Dim txtVAT1 As TextBox = Nothing
    '                    txtTV1 = grdRow.FindControl("txtTV" & i + l)
    '                    txtNTV1 = grdRow.FindControl("txtNTV" & i + l)
    '                    txtVAT1 = grdRow.FindControl("txtVAT" & i + l)
    '                    If txt1 IsNot Nothing Then
    '                        If txt1.Style("FieldHeader") <> "" Then
    '                            If txt1.Style("FieldHeader").Contains("Sr No") = False And txt1.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt1.Style("FieldHeader").Contains("Min Nights") = False And txt1.Style("FieldHeader").Contains("Pkg") = False And txt1.Style("FieldHeader").Contains("Remark") = False Then 'And txt1.Style("FieldHeader").Contains("Extra Person Supplement") = False 'changed by mohamed on 28/03/2018
    '                                Call assignVatValueToTextBox_VATExclusive(txt1, txtTV1, txtNTV1, txtVAT1)
    '                            End If
    '                        End If
    '                    End If
    '                Next i
    '            End If
    '        Next
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractRoomrates.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub

    ''changed by mohamed on 21/02/2018
    'Sub assignVatValueToTextBox_VATExclusive(ByRef txt1 As TextBox, ByRef txtTV1 As TextBox, ByRef txtNTV1 As TextBox, ByRef txtVAT1 As TextBox)
    '    Dim dsVatDet As DataSet
    '    Dim lsSqlQry As String = ""

    '    If txt1.Text.Trim = "" Or chkVATCalculationRequired.Checked = False Then
    '        txtTV1.Text = IIf(txt1.Text.Trim = "", "", Val(txt1.Text))
    '        txtNTV1.Text = "" 'IIf(txt1.Text.Trim = "", "", 0)
    '        txtVAT1.Text = "" 'IIf(txt1.Text.Trim = "", "", 0)
    '    Else
    '        lsSqlQry = "select * from dbo.fn_calculate_taxablevalue_onlycost_InclusiveOfTax (" & Val(txt1.Text) & ","
    '        lsSqlQry += Val(txtServiceCharges.Text) & "," & Val(TxtMunicipalityFees.Text) & ","
    '        lsSqlQry += Val(txtVAT.Text) & "," & Val(txtTourismFees.Text) & ")"
    '        dsVatDet = objUtils.GetDataFromDatasetnew(Session("dbConnectionName"), lsSqlQry)
    '        If dsVatDet.Tables.Count > 0 Then
    '            If dsVatDet.Tables(0).Rows.Count > 0 Then
    '                txtTV1.Text = Val(dsVatDet.Tables(0).Rows(0)("costtaxablevalue"))
    '                txtNTV1.Text = Val(dsVatDet.Tables(0).Rows(0)("costnontaxablevalue"))
    '                txtVAT1.Text = Val(dsVatDet.Tables(0).Rows(0)("costvatvalue"))
    '                txt1.Text = Val(dsVatDet.Tables(0).Rows(0)("VATInclusive"))
    '            End If
    '        End If
    '    End If
    'End Sub

    'Protected Sub btnCalculateVATInclusiveHelper_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculateVATInclusiveHelper.Click
    '    Dim lsSqlQry As String
    '    lsSqlQry = "window.open('VATInclusiveRateCalculator.aspx?VATPerc=" & txtVAT.Text
    '    lsSqlQry += "&ST=" & txtServiceCharges.Text & "&MT=" & TxtMunicipalityFees.Text & "&TF=" & txtTourismFees.Text
    '    lsSqlQry += "','_blank','status=1,scrollbars=1,top=150,left=150,width=650,height=250');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", lsSqlQry, True)
    'End Sub

    Protected Sub btnselect_Click1(sender As Object, e As System.EventArgs) Handles btnselect.Click

    End Sub
End Class
