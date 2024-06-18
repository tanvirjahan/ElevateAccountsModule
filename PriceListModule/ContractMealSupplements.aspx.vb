
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices

Partial Class PriceListModule_ContractMealSupplements
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

        Supplementid = 1
        Season = 2
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
            '  NumbersHtml(txtfillrate)
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
            If Session("GV_HotelData") Is Nothing = False Then
                dt = Session("GV_HotelData")


             

                ' If dt.Columns.Count >= 7 Then Exit Sub
                'fill controls from previous form
                ' Now  Bind Column Dynamically 
                Dim fld2 As String = ""
                Dim col As DataColumn
                For Each col In dt.Columns
                    If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" And col.ColumnName <> "Extra Person Supplement" And col.ColumnName <> "No.of.Nights Room Rate" And col.ColumnName <> "Min Nights" Then
                        Dim bfield As New TemplateField
                        'Call Function


                        bfield.HeaderTemplate = New ClassPriceListMeal(ListItemType.Header, col.ColumnName, fld2)
                        bfield.ItemTemplate = New ClassPriceListMeal(ListItemType.Item, col.ColumnName, fld2)
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
        ' FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")




    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String

        Dim CalledfromValue As String = ""
        Dim ConExappid As String = ""
        Dim ConExappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        If IsPostBack = False Then
            ConExappid = 1
            ConExappname = objUser.GetAppName(Session("dbconnectionName"), ConExappid)
            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConExappname, String), "ContractMealSupplements.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConExappname, String), "ContractMealSupplements.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If
            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractMealSupplements.aspx", ConExappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConExappid, intMenuID)
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

                    'Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select min(d.fromdate) fromdate, max(d.todate) todate  from view_offers_detail d(nolock) where  d.promotionid='" & txtpromotionid.Text & "'")
                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from view_offers_detail d(nolock) where  d.promotionid='" & hdnpromotionid.Value & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        hdnpromofrmdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                        hdnpromotodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                    End If


                End If

                'Rosalin 2019-11-07
                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(hdnpromotionid.Value.Trim, String))
                wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, ViewState("State"))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()

                '###

                ' wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

                'wucCountrygroup.sbSetPageState("", "OFFERSMEAL", CType(Session("OfferState"), String))

                lblHeading.Text = txthotelname.Text + " - " + lblHeading.Text + " - " + hdnpromotionid.Value
                Page.Title = "Promotion Meal Supplements "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1

                FillGrid("plistcode", hdnpromotionid.Value, hdnpartycode.Value, "Desc")


                If ddlCopymeal.Value <> "[Select]" Then

                    'Rosalin 2019-11-03
                    'strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                    '         " from partyrmcat prc ,rmcatmast rc,view_offers_supplement v(nolock) " & _
                    '          " where v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1  and   prc.rmcatcode=rc.rmcatcode  and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' order by isnull(rc.rankorder,999)"

                    strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                             " from partyrmcat prc ,rmcatmast rc,view_offers_supplement v(nolock) " & _
                              " where v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1  and   prc.rmcatcode=rc.rmcatcode  and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' group by prc.rmcatcode ,rc.rankorder order by isnull(rc.rankorder,999)"

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True, ddlCopymeal.Value)


                Else
                    'Rosalin 2019-11-03
                    'strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                    '         " from partyrmcat prc ,rmcatmast rc,view_offers_supplement v(nolock) " & _
                    '          " where   v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1  and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' order by isnull(rc.rankorder,999)"

                    strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                             " from partyrmcat prc ,rmcatmast rc,view_offers_supplement v(nolock) " & _
                              " where   v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1  and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' group by prc.rmcatcode ,rc.rankorder order by isnull(rc.rankorder,999)"

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True)

                End If

            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Contractparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)
                Session("contractid") = hdncontractid.Value

                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                '  wucCountrygroup.Visible = False
                wucCountrygroup.sbSetPageState("", "CONTRACTMEAL", CType(Session("ContractState"), String))

                '   hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text
                Session("partycode") = hdnpartycode.Value

                lblHeading.Text = txthotelname.Text + " - " + hdncontractid.Value + " - " + lblHeading.Text
                Page.Title = "Contract Meal Supplements "

                lblbookingvaltype.Visible = False
                ddlBookingValidity.Visible = False


                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, "Desc")

                Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "Select isnull(min(c.fromdate),'') fromdate,isnull(max(c.todate),'') todate from view_contracts_search c(nolock) Where c.contractid='" & hdncontractid.Value & "'")
                If ds1.Tables(0).Rows.Count > 0 Then
                    hdnconfromdate.Value = ds1.Tables(0).Rows(0).Item("fromdate")
                    hdncontodate.Value = ds1.Tables(0).Rows(0).Item("todate")
                End If

                If ddlCopymeal.Value <> "[Select]" Then

                    strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                             " from partyrmcat prc ,rmcatmast rc " & _
                              " where  prc.rmcatcode=rc.rmcatcode  and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' order by isnull(rc.rankorder,999)"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True, ddlCopymeal.Value)


                Else

                    strSqlQry = " select  prc.rmcatcode rmcatname ,prc.rmcatcode  " & _
                             " from partyrmcat prc ,rmcatmast rc " & _
                              " where  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" + hdnpartycode.Value + "' isnull(rc.rankorder,999)"
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCopymeal, "rmcatname", "rmcatcode", strSqlQry, True)

                End If

            End If




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





            '  PanelMain.Visible = False



            'btnCancel.Attributes.Add("onclick", "javascript :if(confirm('Are you sure you want to cancel?')==false)return false;")

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

            If Session("Calledfrom") = "Offers" Then
                '   btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdnpromotionid.Value & "')")
            '   btnAddNew.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
            '    btncopycontract.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
            Else
                btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
                btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            End If

            Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region

#Region "related to user control wucCountrygroup"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        wucCountrygroup.fnbtnVsProcess(txtvsprocesssplit, dlList)
    End Sub

    Sub FillRoomdetails()

        Dim rmcatname As String = ""
        Dim chk2 As CheckBox
        Dim lblrmcatcode As Label
        Session("rmcatname") = Nothing
        Dim oldseason As String = ""

        For Each grdRow As GridViewRow In grdrmcategory.Rows
            chk2 = grdRow.FindControl("chkSelect")
            lblrmcatcode = grdRow.FindControl("lblrmcatcode")

            If chk2.Checked = True Then
                rmcatname = rmcatname + lblrmcatcode.Text + ","

            End If
        Next

        If rmcatname.Length > 0 Then
            rmcatname = rmcatname.Substring(0, rmcatname.Length - 1)
        End If

        Session("rmcatname") = rmcatname

        If Session("rmcatname") = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Room category');", True)

            Exit Sub
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

        lable12.Visible = True
        btncopyratesnextrow.Visible = True
        ' grdWeekDays.Enabled = False

        If ViewState("State") = "Edit" Then
            ShowDynamicGrid()
        End If


        btnfillrate.Visible = True
        txtfillrate.Visible = True
        divcopy1.Style("display") = "block"
    End Sub

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
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

            strSqlQry = " select c.mealsupplementid plistcode , c.promotionid,max(h.promotionname) promotionname ,max(h.applicableto)applicableto, convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status   " _
            & "   from view_offers_header h(nolock),view_offers_detail d(nolock), view_contracts_mealsupp_header c(nolock)  where isnull(h.active,0)=0 and h.promotionid=c.promotionid   and  " _
            & " h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "' and  c.promotionid<>'" + hdnpromotionid.Value + "'  " + filterCond + "  group by c.promotionid,h.approved,h.promotionname,c.mealsupplementid order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub createdatatableoffer()
        Try


            Dim strMealPlans As String = ""
            Dim strCondition As String = ""
            If Session("rmcatname") Is Nothing = False Then
                strMealPlans = Session("rmcatname")
                If strMealPlans.Length > 0 Then
                    Dim mString As String() = strMealPlans.Split(",")
                    For m As Integer = 0 To mString.Length - 1
                        If strCondition = "" Then
                            strCondition = "'" & mString(m) & "'"
                        Else
                            strCondition &= ",'" & mString(m) & "'"
                        End If
                    Next
                End If
            End If


            cnt = 0
            Session("GV_HotelData") = Nothing
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select count(distinct v.rmcatcode) from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1 and " _
                & " prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                     & hdnpartycode.Value & "'" ' and  prc.rmcatcode IN (" & strCondition & ")   "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select  rmcatcode from (select distinct  v.rmcatcode,isnull(rc.rankorder,999) rankorder from partyrmcat prc,rmcatmast rc ,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode  and v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1 and " _
                & " prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                      & hdnpartycode.Value & "' ) rs  order by isnull(rs.rankorder,999)"  ' p.rmcatcode " '

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

            Session("GV_HotelData") = dt

            For i = grdRoomrates.Columns.Count - 1 To 7 Step -1
                grdRoomrates.Columns.RemoveAt(i)
            Next


            ' If dt.Columns.Count >= 7 Then Exit Sub
            'fill controls from previous form
            ' Now  Bind Column Dynamically 
            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" And col.ColumnName <> "Extra Person Supplement" And col.ColumnName <> "No.of.Nights Room Rate" And col.ColumnName <> "Min Nights" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassPriceListMeal(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassPriceListMeal(ListItemType.Item, col.ColumnName, fld2)
                    grdRoomrates.Columns.Add(bfield)


                End If
            Next
            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()


        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Private Sub createdatatable()
        Try


            Dim strMealPlans As String = ""
            Dim strCondition As String = ""
            If Session("rmcatname") Is Nothing = False Then
                strMealPlans = Session("rmcatname")
                If strMealPlans.Length > 0 Then
                    Dim mString As String() = strMealPlans.Split(",")
                    For m As Integer = 0 To mString.Length - 1
                        If strCondition = "" Then
                            strCondition = "'" & mString(m) & "'"
                        Else
                            strCondition &= ",'" & mString(m) & "'"
                        End If
                    Next
                End If
            End If


            cnt = 0
            Session("GV_HotelData") = Nothing
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                     & hdnpartycode.Value & "'" ' and  prc.rmcatcode IN (" & strCondition & ")   "  ' p.rmcatcode " '
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            cnt = mySqlCmd.ExecuteScalar
            mySqlConn.Close()


            ReDim arr(cnt + 1)
            ReDim arrRName(cnt + 1)
            Dim i As Long = 0

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                      & hdnpartycode.Value & "'   order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

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

            Session("GV_HotelData") = dt

            For i = grdRoomrates.Columns.Count - 1 To 7 Step -1
                grdRoomrates.Columns.RemoveAt(i)
            Next


            ' If dt.Columns.Count >= 7 Then Exit Sub
            'fill controls from previous form
            ' Now  Bind Column Dynamically 
            Dim fld2 As String = ""
            Dim col As DataColumn
            For Each col In dt.Columns
                If col.ColumnName <> "Room_Type_Code" And col.ColumnName <> "Room Type Name" And col.ColumnName <> "Meal Plan" And col.ColumnName <> "Price Pax" And col.ColumnName <> "Unityesno" And col.ColumnName <> "Noofextraperson" And col.ColumnName <> "Extra Person Supplement" And col.ColumnName <> "No.of.Nights Room Rate" And col.ColumnName <> "Min Nights" Then
                    Dim bfield As New TemplateField
                    'Call Function
                    bfield.HeaderTemplate = New ClassPriceListMeal(ListItemType.Header, col.ColumnName, fld2)
                    bfield.ItemTemplate = New ClassPriceListMeal(ListItemType.Item, col.ColumnName, fld2)
                    grdRoomrates.Columns.Add(bfield)


                End If
            Next
            grdRoomrates.Visible = True
            grdRoomrates.DataSource = dt
            'InstantiateIn Grid View
            grdRoomrates.DataBind()


        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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


            strSqlQry = "select distinct subseasname from contracts_seasons(nolock) where contractid='" & maxstate & "' and subseasname like '" & Trim(prefixText) & "%' order by subseasname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    seasonlist.Add(myDS.Tables(0).Rows(i)("subseasname").ToString())
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
                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                        Else

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
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else

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
            objUtils.WritErrorLog("ContractMealSu[pplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub



    Private Sub FillGrid(ByVal strsortby As String, ByVal contractid As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If

        Try

            If Session("Calledfrom") = "Offers" Then

                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True
                gv_SearchResult.Columns(4).Visible = False

                If strsortby = "fromdate" Or strsortby = "todate" Then

                    strSqlQry = "with ctee as(select h.mealsupplementid plistcode,h.promotionid,oh.promotionname, '' seasoncode,convert(varchar(10),min(d.fromdate),103) FromDate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser," & _
                      " h.moddate ,h.moduser  from view_contracts_mealsupp_header h(nolock), view_offers_header oh (nolock) ,view_contracts_mealsupp_dates d(nolock)  where h.promotionid=oh.promotionid and h.mealsupplementid=d.mealsupplementid  and   and h.promotionid='" & contractid & _
                    "' group by h.mealsupplementid,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser) select * from ctee order by convert(datetime," & strsortby & ",103)  " & strsortorder & " "



                Else

                    strSqlQry = "select h.mealsupplementid plistcode,h.promotionid,oh.promotionname,'' seasoncode,convert(varchar(10),min(d.fromdate),103) FromDate, convert(varchar(10),max(d.todate),103) todate,h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser," & _
                 " h.moddate ,h.moduser  from view_contracts_mealsupp_header h(nolock), view_offers_header oh (nolock) ,view_contracts_mealsupp_dates d(nolock)  where  h.promotionid=oh.promotionid and h.mealsupplementid=d.mealsupplementid  and h.promotionid='" & contractid & _
                  "' group by h.mealsupplementid,h.promotionid,oh.promotionname,h.applicableto,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & "  " & strsortorder & " "


                End If

            Else

                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_SearchResult.Columns(4).Visible = True

                If strsortby = "fromdate" Or strsortby = "todate" Then
                    strSqlQry = "with ctee as (select h.mealsupplementid plistcode,'' promotionid,'' promotionname,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                  " h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_mealsupp_header h(nolock)  where  h.contractid='" & contractid & "' and h.seasons <>'' union all " _
                  & " select h.mealsupplementid plistcode,'' promotionid,'' promotionname,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                  & "   from view_contracts_mealsupp_header h(nolock),view_contracts_mealsupp_dates d(nolock)  where  h.mealsupplementid=d.mealsupplementid and     h.contractid='" & contractid & "' and h.seasons ='' group by h.mealsupplementid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser ) " _
                    & " select * from ctee order by convert(datetime," & strsortby & ",103)  " & strsortorder & ""

                Else
                    strSqlQry = "select h.mealsupplementid plistcode,'' promotionid,'' promotionname,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
                  " h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_mealsupp_header h(nolock)  where  h.contractid='" & contractid & "' and h.seasons <>'' union all " _
                  & " select h.mealsupplementid plistcode,'' promotionid,'' promotionname,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser  " _
                  & "   from view_contracts_mealsupp_header h(nolock),view_contracts_mealsupp_dates d(nolock)  where  h.mealsupplementid=d.mealsupplementid and     h.contractid='" & contractid & "' and h.seasons ='' group by h.mealsupplementid,h.applicableto  ,h.adddate,h.adduser,h.moddate ,h.moduser order by " & strsortby & " " & strsortorder & ""

                End If
            End If



            'strSqlQry = "select h.mealsupplementid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,'" & contractid & "') fromdate, dbo.fn_get_seasonmaxdate(h.seasons,'" & contractid & "')  todate," & _
            '  " h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek,h.adddate,h.adduser,h.moddate ,h.moduser   from view_contracts_mealsupp_header h(nolock)  where  h.contractid='" & contractid & "'"


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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


        'Dim tickedornot As Boolean = False
        'Dim chkSelect As CheckBox
        'tickedornot = False
        'For Each grdRow In grdseason.Rows
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

        Dim tickedornot As Boolean = False
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
                tickedornot = True
                Exit For
            End If

        Next

        If tickedornot = False Then
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

            If chk2.Checked = True And oldseason <> RTrim(LTrim(txtmealcode1.Text)) Then 'rtrim / ltrim is added --changed by mohamed on 09/06/2018
                seasonname = seasonname + RTrim(LTrim(txtmealcode1.Text)) + ","
                oldseason = RTrim(LTrim(txtmealcode1.Text))
            End If
        Next

        If seasonname.Length > 0 Then
            seasonname = seasonname.Substring(0, seasonname.Length - 1)
        End If

        Session("seasons") = seasonname


        Dim contfromdate As Date
        Dim conttodate As Date
        Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "Select min(c.fromdate) fromdate,max(c.todate) todate from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'")
        If ds.Tables(0).Rows.Count > 0 Then
            contfromdate = ds.Tables(0).Rows(0).Item("fromdate")
            conttodate = ds.Tables(0).Rows(0).Item("todate")
        End If


        '--------------------------------------------- Validate Exclusive Date Grid belongs to contract From and To date
        Dim flgdt As Boolean = False
        For Each gvRow In grdexdates.Rows
            'dpFDate = gvRow.FindControl("FrmDate")
            'dpTDate = gvRow.FindControl("ToDate")
            Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
            If grdexdates.Rows.Count > 0 And txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century ');", True)
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

                    ' If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < Format(CType(contfromdate, Date), "yyyy/MM/dd") Then
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


                    'If ToDt <> Nothing Then
                    '    If ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) <= ToDt Then
                    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date Overlapping.');", True)
                    '        SetFocus(txtFromDate)
                    '        ValidateSave = False
                    '        Exit Function
                    '    End If
                    'End If
                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    flgdt = True

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


        ''''''''''' Dates Overlapping

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



        '--------------------------------------------- Validate Manual Season Date Grid belongs to contract From and To date
        Dim flgdt1 As Boolean = False
        For Each gvRow In grdDates.Rows
            'dpFDate = gvRow.FindControl("FrmDate")
            'dpTDate = gvRow.FindControl("ToDate")
            Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
            If grdDates.Rows.Count > 0 And txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century ');", True)
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

                    ' If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < Format(CType(contfromdate, Date), "yyyy/MM/dd") Then
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



        For header = 0 To cnt - 8
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If Not txt Is Nothing Then
                heading(header) = txt.Text
            End If

        Next

        Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        B = 0
        n = 0
        For Each gvRow In grdRoomrates.Rows
            If n = 0 Then
                For j = 0 To cnt - 8

                    If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & j)
                        If txt Is Nothing Then
                        Else
                            If txt.Text <> "" Then
                                flag = True
                                'GoTo Err 'changed by mohamed on 21/02/2018
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
                    If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & j)
                        If txt Is Nothing Then
                        Else
                            If txt.Text <> "" Then
                                flag = True
                                'GoTo Err 'changed by mohamed on 21/02/2018
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



Err:    If flag = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
            ValidateSave = False
            Exit Function
        End If


        flag = False




        ValidateSave = True
    End Function

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
        Dim a As Long = cnt - 10
        Dim b As Long = 0
        Dim header As Long = 0
        Dim heading(cnt + 1) As String
        Dim flag As Boolean = False
        Dim lblcode As Label
        Dim m As Long = 0




        Dim tickedornot As Boolean = False
        Dim chkSelect As CheckBox
        tickedornot = False


        For Each grdrow In grdDates.Rows
            Dim txtfromdate As TextBox = grdrow.findcontrol("txtfromdate")
            Dim txttodate As TextBox = grdrow.findcontrol("txttodate")

            If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                tickedornot = True
                Exit For
            End If

        Next


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
            OfferValidateSave = False
            Exit Function
        End If








        '--------------------------------------------- Validate Exclusive Date Grid belongs to contract From and To date
        Dim flgdt As Boolean = False
        For Each gvRow In grdexdates.Rows
            'dpFDate = gvRow.FindControl("FrmDate")
            'dpTDate = gvRow.FindControl("ToDate")
            Dim txtFromDate As TextBox = gvRow.FindControl("txtfromdate")
            Dim txtToDate As TextBox = gvRow.FindControl("txttodate")
            If grdexdates.Rows.Count > 0 And txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then

                    If Left(Right(txtFromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                        OfferValidateSave = False
                        SetFocus(txtFromDate)
                        Exit Function
                    End If

                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(txtToDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    ' If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < Format(CType(contfromdate, Date), "yyyy/MM/dd") Then
                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotion Period.');", True)
                        SetFocus(txtFromDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value)) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belongs to the Promotion Period');", True)
                        SetFocus(txtToDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotion Period.');", True)
                        SetFocus(txtFromDate)
                        OfferValidateSave = False
                        Exit Function
                    End If



                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    flgdt = True

                ElseIf txtFromDate.Text <> "" And txtToDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                    SetFocus(txtToDate)
                    OfferValidateSave = False
                    Exit Function
                ElseIf txtFromDate.Text = "" And txtToDate.Text <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                    SetFocus(txtFromDate)
                    OfferValidateSave = False
                    Exit Function
                End If
            End If
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''


        ''''''''''' Dates Overlapping

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
                    OfferValidateSave = False
                    Exit Function
                End If
            End If
        End If


        '''''''''''''''''



        '--------------------------------------------- Validate Manual Season Date Grid belongs to contract From and To date
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
                        OfferValidateSave = False
                        SetFocus(txtFromDate)
                        Exit Function
                    End If

                    If ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        SetFocus(txtToDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    ' If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < Format(CType(contfromdate, Date), "yyyy/MM/dd") Then
                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period.');", True)
                        SetFocus(txtFromDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value)) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belongs to the Promotions Period');", True)
                        SetFocus(txtToDate)
                        OfferValidateSave = False
                        Exit Function
                    End If

                    If Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd") < ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period.');", True)
                        SetFocus(txtFromDate)
                        OfferValidateSave = False
                        Exit Function
                    End If



                    ToDt = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    flgdt1 = True

                ElseIf txtFromDate.Text <> "" And txtToDate.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter To Date.');", True)
                    SetFocus(txtToDate)
                    OfferValidateSave = False
                    Exit Function
                ElseIf txtFromDate.Text = "" And txtToDate.Text <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date.');", True)
                    SetFocus(txtFromDate)
                    OfferValidateSave = False
                    Exit Function
                End If
            End If
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''



        For header = 0 To cnt - 8
            txt = grdRoomrates.HeaderRow.FindControl("txtHead" & header)
            If Not txt Is Nothing Then
                heading(header) = txt.Text
            End If

        Next

        Dim txtTV1 As TextBox = Nothing 'changed by mohamed on 21/02/2018
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        b = 0
        n = 0
        For Each gvRow In grdRoomrates.Rows
            If n = 0 Then
                For j = 0 To cnt - 8

                    If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & j)
                        If txt Is Nothing Then
                        Else
                            If txt.Text <> "" Then
                                flag = True
                                'GoTo Err 'changed by mohamed on 21/02/2018
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
                    If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
                    Else
                        txt = gvRow.FindControl("txt" & j)
                        If txt Is Nothing Then
                        Else
                            If txt.Text <> "" Then
                                flag = True
                                'GoTo Err 'changed by mohamed on 21/02/2018
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



Err:    If flag = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Atleast 1 Entry for prices should be done in the Grid.');", True)
            OfferValidateSave = False
            Exit Function
        End If


        flag = False




        OfferValidateSave = True
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
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_Meals", mySqlConn, sqlTrans)
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
                        optionval = objUtils.GetAutoDocNo("ML", mySqlConn, sqlTrans)
                        txtplistcode.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_mealsupp_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        ' mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = IIf(txtApplicableTo.Text = "", "", CType(Replace(txtApplicableTo.Text, ",  ", ","), String))

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)



                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()                  'command disposed
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_mealsupp_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                        '   mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = IIf(txtApplicableTo.Text = "", "", CType(Replace(txtApplicableTo.Text, ",  ", ","), String))
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasons", SqlDbType.VarChar, 5000)).Value = IIf(Session("seasons") = "", "", CType(Replace(Session("seasons"), ",  ", ","), String))
                            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)

                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()
                    End If


                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_detail Where  mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_weekdays Where  mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_MealSupplement Where  meal_supplement_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_MealSupplement_Rates Where  supp_id='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    '------------------------------------Inserting Data weekdays
                    Dim GvRow As GridViewRow
                    For Each GvRow In grdWeekDays.Rows
                        Dim lblorder As Label = GvRow.FindControl("lblSrNo")
                        Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")
                        If chkSelect.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_add_contracts_mealsupp_weekdays", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@dayoftheweek", SqlDbType.VarChar, 30)).Value = CType(GvRow.Cells(2).Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@weekorder", SqlDbType.Int, 4)).Value = CType(lblorder.Text.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                    Next


                    mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_excl_dates Where  mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    ''------------------------------------Inserting Date Exclusive
                    Dim i As Integer = 1
                    For Each GvRow In grdexdates.Rows

                        Dim txtfromDate As TextBox = GvRow.FindControl("txtfromDate")
                        Dim txtToDate As TextBox = GvRow.FindControl("txtToDate")
                        Dim ddlExcl As HtmlSelect = GvRow.FindControl("ddlExcl")
                        If txtfromDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_contracts_mealsupp_excl_dates", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@clineno", SqlDbType.Int)).Value = i
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@exclfor", SqlDbType.VarChar, 50)).Value = CType(ddlExcl.Value, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose()
                        End If
                        i = i + 1
                    Next




                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_countries Where mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_agents Where mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    'If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    '    For i = 0 To arrcountry.Length - 1

                    '        If arrcountry(i) <> "" Then




                    '            mySqlCmd = New SqlCommand("sp_add_contracts_mealsupp_countries", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    ' End If

                    'If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then




                    '            mySqlCmd = New SqlCommand("sp_add_contracts_mealsupp_agents", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    ' End If


                    '''' save season manual dates
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_mealsupp_dates Where mealsupplementid='" & CType(txtplistcode.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    Dim kl As Integer = 1

                    Dim countrygroup As String = ""
                    Dim agentsGroup As String = ""

                    If Session("CountryList") <> "" Then
                        countrygroup = wucCountrygroup.checkcountrylist.ToString.Trim
                    End If

                    If Session("AgentList") <> "" Then
                        agentsGroup = wucCountrygroup.checkagentlist.ToString.Trim
                    End If

                    'If Session("CountryList") <> "" Then

                    'Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    ' Dim arrcountry As String() = Session("CountryList").ToString.Trim.Split(",")
                    'For i = 0 To arrcountry.Length - 1

                    'If arrcountry(i) <> "" Then

                    ' If countrygroup <> "" Then

                    For Each GvRow1 As GridViewRow In grdDates.Rows

                        Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                        Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                        ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                        If txtfromdate.Text <> "" And txttodate.Text <> "" Then
                            mySqlCmd = New SqlCommand("New_add_new_edit_mealsupplements", mySqlConn, sqlTrans)
                            ' mySqlCmd = New SqlCommand("sp_add_new_edit_mealsupplements", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                            mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                            mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentsGroup, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 8000)).Value = CType(countrygroup, String)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed 
                            kl = kl + 1
                        End If
                    Next

                    'End If


                    ' Next


                    'End If

                    'kl = 1
                    'If Session("AgentList") <> "" Then
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then

                    '            For Each GvRow1 As GridViewRow In grdDates.Rows

                    '                Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                    '                Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                    '                ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                    '                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    '                    mySqlCmd = New SqlCommand("sp_add_new_edit_mealsupplements", mySqlConn, sqlTrans)
                    '                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(i), String)
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")
                    '                    mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""

                    '                    mySqlCmd.ExecuteNonQuery()
                    '                    mySqlCmd.Dispose() 'command disposed
                    '                    kl = kl + 1
                    '                End If
                    '            Next


                    '        End If

                    '    Next
                    'End If






                    '''''''''''''''''''''

                    Dim chk2 As CheckBox
                    Dim txtmealcode1 As Label

                    If Session("CountryList") <> "" Then

                        Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                        For i = 0 To arrcountry.Length - 1

                            If arrcountry(i) <> "" Then

                                For Each grdRow As GridViewRow In grdseason.Rows
                                    chk2 = grdRow.FindControl("chkseason")
                                    txtmealcode1 = grdRow.FindControl("txtseasoncode")

                                    If chk2.Checked = True Then

                                        mySqlCmd = New SqlCommand("sp_add_new_edit_mealsupplements", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = ""
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                        mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = CType(arrcountry(i), String)

                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose() 'command disposed

                                    End If
                                Next


                            End If
                        Next

                    End If
                    If Session("AgentList") <> "" Then
                        Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                        For i = 0 To arragents.Length - 1


                            If arragents(i) <> "" Then

                                For Each grdRow As GridViewRow In grdseason.Rows
                                    chk2 = grdRow.FindControl("chkseason")
                                    txtmealcode1 = grdRow.FindControl("txtseasoncode")

                                    If chk2.Checked = True Then

                                        mySqlCmd = New SqlCommand("sp_add_new_edit_mealsupplements", mySqlConn, sqlTrans)
                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATCalculationRequired", SqlDbType.Int)).Value = IIf(chkVATCalculationRequired.Checked = True, 1, 0) 'changed by mohamed on 21/02/2018
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Text)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(i), String)
                                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(i), String) & "'")
                                        mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""

                                        mySqlCmd.ExecuteNonQuery()
                                        mySqlCmd.Dispose() 'command disposed

                                    End If
                                Next



                            End If
                        Next


                    End If



                    ''''''''''''' Price detail saving


                    If Session("Calledfrom") <> "Offers" Then

                        For Each grdRow As GridViewRow In grdseason.Rows
                            chk2 = grdRow.FindControl("chkseason")
                            txtmealcode1 = grdRow.FindControl("txtseasoncode")


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



                            If chk2.Checked = True Then

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

                                                        mySqlCmd = New SqlCommand("sp_add_new_edit_MealSupplement_Rates", mySqlConn, sqlTrans)
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomname", SqlDbType.NVarChar, 250)).Value = CType(txtrmtyp.Text, String) ' CType(CType(GvRow.Cells(2).Text, String), String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
                                                        If txtrmtyp Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                                        End If

                                                        ' oldmeal = GvRow.Cells(3).Text

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(3).Text, String)

                                                        txt = GvRow.FindControl("txt" & j)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If txt.Text <> Nothing Then
                                                                If grdRoomrates.Columns(j + 7).HeaderStyle.CssClass.Trim.ToLower = "displaynone" Then '***changed by Danny on 28/04/2018
                                                                    txt.Text = ""
                                                                End If
                                                                If txt.Text = "" Then
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                Else
                                                                    Select Case txt.Text
                                                                        Case "Free"
                                                                            txt.Text = "-3"
                                                                        Case "Incl"
                                                                            txt.Text = "-1"
                                                                        Case "N.Incl"
                                                                            txt.Text = "-2"
                                                                        Case "N/A"
                                                                            txt.Text = "-4"
                                                                        Case "On Request"
                                                                            txt.Text = "-5"
                                                                    End Select
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")


                                                                End If
                                                            End If
                                                        End If

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
                                                txtrmtyp = GvRow.FindControl("txtrmtypcode")
                                                If txt.Text <> Nothing Then
                                                    If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                                                    Else

                                                        mySqlCmd = New SqlCommand("sp_add_new_edit_MealSupplement_Rates", mySqlConn, sqlTrans)
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomname", SqlDbType.NVarChar, 250)).Value = CType(txtrmtyp.Text, String) ' CType(CType(GvRow.Cells(2).Text, String), String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)

                                                        If txtrmtyp Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                                        End If

                                                        txt = GvRow.FindControl("txt" & j)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If txt.Text <> Nothing Then
                                                                If grdRoomrates.Columns(k + 7).HeaderStyle.CssClass.Trim.ToLower = "displaynone" Then '***changed by Danny on 28/04/2018
                                                                    txt.Text = ""
                                                                End If
                                                                If txt.Text = "" Then
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                Else
                                                                    Select Case txt.Text
                                                                        Case "Free"
                                                                            txt.Text = "-3"
                                                                        Case "Incl"
                                                                            txt.Text = "-1"
                                                                        Case "N.Incl"
                                                                            txt.Text = "-2"
                                                                        Case "N/A"
                                                                            txt.Text = "-4"
                                                                        Case "On Request"
                                                                            txt.Text = "-5"
                                                                    End Select
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType(txtmealcode1.Text.Trim, String)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(3).Text, Date), "yyyy/MM/dd")
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(grdRow.Cells(4).Text, Date), "yyyy/MM/dd")

                                                                End If
                                                            End If
                                                        End If



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

                            End If
                        Next


                    End If

                    kl = 1
                    If grdDates.Rows.Count <> 0 Then '  Session("Calledfrom") = "Offers" Then

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

                        For Each GvRow1 As GridViewRow In grdDates.Rows

                            Dim txtfromdate As TextBox = GvRow1.FindControl("txtfromdate")
                            Dim txttodate As TextBox = GvRow1.FindControl("txttodate")
                            ' Dim lblseason As Label = gvrow.FindControl("lblseason")

                            If txtfromdate.Text <> "" And txttodate.Text <> "" Then

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

                                                        mySqlCmd = New SqlCommand("sp_add_new_edit_MealSupplement_Rates_offers", mySqlConn, sqlTrans)
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomname", SqlDbType.NVarChar, 250)).Value = CType(CType(GvRow.Cells(2).Text, String), String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(j), String)
                                                        If txtrmtyp Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                                        End If

                                                        ' oldmeal = GvRow.Cells(3).Text

                                                        'mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(GvRow.Cells(3).Text, String)

                                                        txt = GvRow.FindControl("txt" & j)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If txt.Text <> Nothing Then
                                                                If grdRoomrates.Columns(j + 7).HeaderStyle.CssClass.Trim.ToLower = "displaynone" Then '***changed by Danny on 28/04/2018
                                                                    txt.Text = ""
                                                                End If
                                                                If txt.Text = "" Then
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                Else
                                                                    Select Case txt.Text
                                                                        Case "Free"
                                                                            txt.Text = "-3"
                                                                        Case "Incl"
                                                                            txt.Text = "-1"
                                                                        Case "N.Incl"
                                                                            txt.Text = "-2"
                                                                        Case "N/A"
                                                                            txt.Text = "-4"
                                                                        Case "On Request"
                                                                            txt.Text = "-5"
                                                                    End Select
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")

                                                                End If
                                                            End If
                                                        End If

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
                                                txtrmtyp = GvRow.FindControl("txtrmtypcode")
                                                If txt.Text <> Nothing Then
                                                    If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                                                    Else

                                                        mySqlCmd = New SqlCommand("sp_add_new_edit_MealSupplement_Rates_offers", mySqlConn, sqlTrans)
                                                        mySqlCmd.CommandType = CommandType.StoredProcedure

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value, String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@plistcode", SqlDbType.VarChar, 100)).Value = CType(txtplistcode.Text, String)

                                                        mySqlCmd.Parameters.Add(New SqlParameter("@roomname", SqlDbType.NVarChar, 250)).Value = CType(CType(GvRow.Cells(2).Text, String), String)
                                                        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(heading(k), String)

                                                        If txtrmtyp Is Nothing Then
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                                                        Else
                                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtyp.Text, String)
                                                        End If

                                                        txt = GvRow.FindControl("txt" & j)
                                                        If txt Is Nothing Then
                                                        Else
                                                            If txt.Text <> Nothing Then
                                                                If grdRoomrates.Columns(k + 7).HeaderStyle.CssClass.Trim.ToLower = "displaynone" Then '***changed by Danny on 28/04/2018
                                                                    txt.Text = ""
                                                                End If
                                                                If txt.Text = "" Then
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value 'changed by mohamed on 21/02/2018
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = DBNull.Value
                                                                Else
                                                                    Select Case txt.Text
                                                                        Case "Free"
                                                                            txt.Text = "-3"
                                                                        Case "Incl"
                                                                            txt.Text = "-1"
                                                                        Case "N.Incl"
                                                                            txt.Text = "-2"
                                                                        Case "N/A"
                                                                            txt.Text = "-4"
                                                                        Case "On Request"
                                                                            txt.Text = "-5"
                                                                    End Select
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal, 20, 3)).Value = CType(txt.Text, Decimal)

                                                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)

                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@seasonname", SqlDbType.VarChar, 100)).Value = CType("Manual" + CStr(kl), String)
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                                                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")

                                                                End If
                                                            End If
                                                        End If



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





                                kl = kl + 1
                            End If
                        Next


                    End If

                    ''''''''''''''''''''''''''''




                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_mealsupp_header"
                    'mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = ""
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
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_Meals", mySqlConn, sqlTrans)
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
                    mySqlCmd = New SqlCommand("sp_del_contracts_mealsupp_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@mealsupplementid", SqlDbType.VarChar, 20)).Value = CType(txtplistcode.Text.Trim, String)
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            grdWeekDays.Enabled = True
            grdexdates.Enabled = True
            txtApplicableTo.Enabled = True

            btnAddLinesDates.Enabled = True
            btndeleterow.Enabled = True
            chkctrygrp.Enabled = True

            grdDates.Enabled = True


        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then

            grdDates.Enabled = False
            grdmealplan.Enabled = False
            btngenerate.Enabled = False
            grdRoomrates.Enabled = False

            btncopyratesnextrow.Enabled = False
            btnfillrate.Enabled = False
            grdWeekDays.Enabled = False
            grdexdates.Enabled = False
            txtApplicableTo.Enabled = False

            btnAddLinesDates.Enabled = False
            btndeleterow.Enabled = False

            chkctrygrp.Enabled = False
            grdDates.Enabled = False




        ElseIf ViewState("State") = "Edit" Then


            grdDates.Enabled = True
            grdmealplan.Enabled = True
            btngenerate.Enabled = True
            grdRoomrates.Enabled = True

            btncopyratesnextrow.Enabled = True
            btnfillrate.Enabled = True
            grdWeekDays.Enabled = True
            grdexdates.Enabled = True
            txtApplicableTo.Enabled = True

            btnAddLinesDates.Enabled = True
            btndeleterow.Enabled = True
            chkctrygrp.Enabled = True
            grdDates.Enabled = True

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
#End Region



    Private Sub FillMealplans()
        Try
            Dim myDS As New DataSet
            grdmealplan.Visible = True
            strSqlQry = ""

            strSqlQry = "select  a.mealcode,b.mealname  from partymeal a ,mealmast b where a.mealcode=b.mealcode and a.partycode='" & hdnpartycode.Value & "'"

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
                chkSelect.Checked = True

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    Private Sub fillseason()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname subseasname from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Sub seasonsgridfill()
        Try
            Dim myDS As New DataSet
            gv_Seasons.Visible = True
            strSqlQry = ""


            'strSqlQry = "select distinct seasonname subseasname,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "' order by subseasname "

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Weekdays Should not be Empty');", True)
                OfferFindDatePeriod = False
                Exit Function
            End If

            'Rosalin 2019-10-30
            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")



            ''' Manual dates to check
            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(9) As SqlParameter

                    parm3(0) = New SqlParameter("@contractid", "")
                    parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm3(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm3(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm3(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm3(7) = New SqlParameter("@promotionid", CType(hdnpromotionid.Value, String))
                    parm3(8) = New SqlParameter("@weekdays", CType(weekdaystr, String))


                    For i = 0 To 8
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkmealsupplement_manual_offer", parms3)


                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("mealsupplementid")) = False Then
                                strMsg = "Meal Supplements already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("mealsupplementid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Weekdays Should not be Empty');", True)
                FindDatePeriod = False
                Exit Function
            End If

            ' Rosalin 2019-30-10
            Session("CountryList") = Nothing
            Session("AgentList") = Nothing
            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist


            Dim supagentcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=520")

            For Each GVRow In grdseason.Rows

                Dim txtmealcode1 As Label = GVRow.FindControl("txtseasoncode")
                Dim chkseason As CheckBox = GVRow.FindControl("chkseason")

                If chkseason.Checked = True Then

                    Dim ds As DataSet
                    Dim parms2 As New List(Of SqlParameter)
                    Dim parm2(10) As SqlParameter


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

                    For i = 0 To 9
                        parms2.Add(parm2(i))
                    Next



                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkmealsupplement", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("mealsupplementid")) = False Then
                                strMsg = "Meal Supplements already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("mealsupplementid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                                FindDatePeriod = False
                                Exit Function
                            End If
                        End If
                    End If

                End If


            Next

            ''' Manual dates to check
            For Each GVRow In grdDates.Rows

                Dim txtfromdate As TextBox = GVRow.FindControl("txtfromdate")
                Dim txttodate As TextBox = GVRow.FindControl("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    Dim ds1 As DataSet
                    Dim parms3 As New List(Of SqlParameter)
                    Dim parm3(9) As SqlParameter

                    parm3(0) = New SqlParameter("@contractid", CType(hdncontractid.Value, String))
                    parm3(1) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))
                    parm3(2) = New SqlParameter("@fromdate", Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"))
                    parm3(3) = New SqlParameter("@todate", Format(CType(txttodate.Text, Date), "yyyy/MM/dd"))
                    parm3(4) = New SqlParameter("@plistcode", CType(txtplistcode.Text, String))
                    parm3(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm3(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm3(7) = New SqlParameter("@promotionid", "")
                    parm3(8) = New SqlParameter("@weekdays", CType(weekdaystr, String))


                    For i = 0 To 8
                        parms3.Add(parm3(i))
                    Next

                    ds1 = New DataSet()
                    ds1 = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkmealsupplement_manual", parms3)


                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)("mealsupplementid")) = False Then
                                strMsg = "Meal Supplements already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds1.Tables(0).Rows(0)("mealsupplementid") + " - Country " + ds1.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds1.Tables(0).Rows(0)("agentname")
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractMealsupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Sub ShowRecordcopycontract(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_plistapprove_status('" + RefCode + "')") = "1" Then
            '    mySqlCmd = New SqlCommand(" Select * from cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'Else
            '    mySqlCmd = New SqlCommand(" Select * from edit_cplisthnew Where plistcode='" & RefCode & "' ", mySqlConn)
            'End If
            mySqlCmd = New SqlCommand(" Select * from view_contracts_mealsupp_header(nolock) Where mealsupplementid='" & RefCode & "' and contractid='" & hdncopycontractid.Value & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("mealsupplementid")) = False Then
                        txtplistcode.Text = mySqlReader("mealsupplementid")

                    End If

                    'If IsDBNull(mySqlReader("subseascode")) = False Then
                    '    txtseasonname.Text = mySqlReader("subseascode")
                    '    filldates()

                    'End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


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

                    'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from view_contracts_mealsupp_header(nolock) where   contractid='" & hdncontractid.Value & "' and mealsupplementid ='" & CType(RefCode, String) & "'") <> "" Then
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

                    'If Session("MealPlans") = "" Then
                    '    Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    'End If


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
                            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName " ' convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"
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
                    Session("rmcatname") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_contracts_mealsupp_detail  u(nolock) where u.mealsupplementid = mealsupplementid  and u.mealsupplementid ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    '  End If


                    Dim strroomcategory As String = ""
                    Dim strConditionrm As String = ""
                    If Session("rmcatname") Is Nothing = False Then
                        strroomcategory = Session("rmcatname")
                        If strroomcategory.Length > 0 Then
                            Dim mString1 As String() = strroomcategory.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strConditionrm = "" Then
                                    strConditionrm = "'" & mString1(i) & "'"
                                Else
                                    strConditionrm &= ",'" & mString1(i) & "'"
                                End If
                            Next
                        End If
                    End If



                    Dim myDS1 As New DataSet
                    grdrmcategory.Visible = True
                    strSqlQry = ""

                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = "select v.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                            & " v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1 and  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                   & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                   & "union all " _
                   & " select prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc ,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                   & " rc.active=1 and  v.rmcatcode =rc.rmcatcode  and  v.promotionid='" & hdnpromotionid.Value & "'   and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"



                    Else


                        strSqlQry = "select distinct prc.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                         & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                         & "union all " _
                         & " select distinct prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"

                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS1)
                    grdrmcategory.DataSource = myDS1

                    If myDS1.Tables(0).Rows.Count > 0 Then
                        grdrmcategory.DataBind()

                    Else
                        grdrmcategory.DataBind()

                    End If
                    Dim chkSelectrm As CheckBox
                    Dim lblselect1 As Label

                    For Each grdRow In grdrmcategory.Rows
                        chkSelectrm = CType(grdRow.FindControl("chkSelect"), CheckBox)

                        lblselect1 = CType(grdRow.FindControl("lblselect"), Label)
                        Dim lblrmcatcode As Label = grdRow.findcontrol("lblrmcatcode")

                        If lblselect1.Text = "1" Then
                            chkSelectrm.Checked = True

                        End If

                    Next


                    '   filldates()

                    ' btngenerate.Visible = False

                End If
            End If
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
        End Try
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
            mySqlCmd = New SqlCommand(" Select * from view_contracts_mealsupp_header(nolock) Where mealsupplementid='" & RefCode & "'", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("mealsupplementid")) = False Then
                        txtplistcode.Text = mySqlReader("mealsupplementid")

                    End If

                    'If IsDBNull(mySqlReader("subseascode")) = False Then
                    '    txtseasonname.Text = mySqlReader("subseascode")
                    '    filldates()

                    'End If
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


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

                    Session("seasons") = ""
                    If IsDBNull(mySqlReader("seasons")) = False Then
                        Session("seasons") = CType(RTrim(LTrim(mySqlReader("seasons"))), String)
                    End If

                    'If Session("MealPlans") = "" Then
                    '    Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    'End If


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
                            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName " ' convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"
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
                    Session("rmcatname") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_contracts_mealsupp_detail  u(nolock) where u.mealsupplementid = mealsupplementid  and u.mealsupplementid ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    '  End If


                    Dim strroomcategory As String = ""
                    Dim strConditionrm As String = ""
                    If Session("rmcatname") Is Nothing = False Then
                        strroomcategory = Session("rmcatname")
                        If strroomcategory.Length > 0 Then
                            Dim mString1 As String() = strroomcategory.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strConditionrm = "" Then
                                    strConditionrm = "'" & mString1(i) & "'"
                                Else
                                    strConditionrm &= ",'" & mString1(i) & "'"
                                End If
                            Next
                        End If
                    End If



                    Dim myDS1 As New DataSet
                    grdrmcategory.Visible = True
                    strSqlQry = ""

                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = "select v.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                            & " v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1 and  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                   & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                   & "union all " _
                   & " select prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc ,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                   & " rc.active=1 and  v.rmcatcode =rc.rmcatcode  and  v.promotionid='" & hdnpromotionid.Value & "'   and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"



                    Else


                        strSqlQry = "select prc.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                         & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                         & "union all " _
                         & " select prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"

                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS1)
                    grdrmcategory.DataSource = myDS1

                    If myDS1.Tables(0).Rows.Count > 0 Then
                        grdrmcategory.DataBind()

                    Else
                        grdrmcategory.DataBind()

                    End If
                    Dim chkSelectrm As CheckBox
                    Dim lblselect1 As Label

                    For Each grdRow In grdrmcategory.Rows
                        chkSelectrm = CType(grdRow.FindControl("chkSelect"), CheckBox)

                        lblselect1 = CType(grdRow.FindControl("lblselect"), Label)
                        Dim lblrmcatcode As Label = grdRow.findcontrol("lblrmcatcode")

                        If lblselect1.Text = "1" Then
                            chkSelectrm.Checked = True

                        End If

                    Next


                    '   filldates()

                    ' btngenerate.Visible = False

                End If
            End If
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
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

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Dim ObjDate As New clsDateTime
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            mySqlCmd = New SqlCommand(" Select * from view_contracts_mealsupp_header(nolock) Where mealsupplementid='" & RefCode & "' ", mySqlConn)

            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("mealsupplementid")) = False Then
                        txtplistcode.Text = mySqlReader("mealsupplementid")

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
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


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

                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from view_contracts_mealsupp_header(nolock) where   contractid='" & hdncontractid.Value & "' and mealsupplementid ='" & CType(RefCode, String) & "'") <> "" Then
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

                    'If Session("MealPlans") = "" Then
                    '    Session("MealPlans") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.mealcode from view_cplistdnew  u(nolock) where u.plistcode = plistcode  and u.plistcode ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    'End If


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
                    grdseason.Visible = True
                    strSqlQry = ""
                    If Session("Calledfrom") <> "Offers" Then
                        If Session("seasons") <> "" Then
                            strSqlQry = "select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected  from view_contractseasons(nolock)  where contractid='" & hdncontractid.Value & "' and seasonname  Not IN (" & strCondition & ") " _
                             & " union all " _
                               & " select  convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,1 selected  from view_contractseasons  where contractid='" & hdncontractid.Value & "' and seasonname IN (" & strCondition & ")  order by  3 "



                        Else
                            strSqlQry = "select convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate,SeasonName,0 selected from view_contractseasons(nolock) where contractid='" & hdncontractid.Value & "'  order by SeasonName " ' convert(varchar(10),fromdate,111),convert(varchar(10),todate,111)" ' and subseasnname = '" & subseasonname & "'"
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





                    'Session("rmcatname") = ""
                    'If Session("rmcatname") = "" Then
                    Session("rmcatname") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_contracts_mealsupp_detail  u(nolock) where u.mealsupplementid = mealsupplementid  and u.mealsupplementid ='" & RefCode & "'  for xml path('')),1,1,'' ) ")
                    '  End If


                    Dim strroomcategory As String = ""
                    Dim strConditionrm As String = ""
                    If Session("rmcatname") Is Nothing = False Then
                        strroomcategory = Session("rmcatname")
                        If strroomcategory.Length > 0 Then
                            Dim mString1 As String() = strroomcategory.Split(",")
                            For i As Integer = 0 To mString1.Length - 1
                                If strConditionrm = "" Then
                                    strConditionrm = "'" & mString1(i) & "'"
                                Else
                                    strConditionrm &= ",'" & mString1(i) & "'"
                                End If
                            Next
                        End If
                    End If



                    Dim myDS1 As New DataSet
                    grdrmcategory.Visible = True
                    strSqlQry = ""
                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = "select distinct v.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                            & " v.promotionid='" & hdnpromotionid.Value & "' and rc.active=1 and  prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                   & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                   & "union all " _
                   & " select distinct prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc ,view_offers_supplement v(nolock) where v.rmcatcode=prc.rmcatcode and " _
                   & " rc.active=1 and  v.rmcatcode =rc.rmcatcode  and  v.promotionid='" & hdnpromotionid.Value & "'   and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"



                    Else
                        strSqlQry = "select prc.rmcatcode,0 selected,isnull(rc.rankorder,999)  rankorder  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                   & hdnpartycode.Value & "' and prc.rmcatcode  not IN (" & strConditionrm & ") " _
                   & "union all " _
                   & " select prc.rmcatcode,1 selected,isnull(rc.rankorder,999)  rankorder from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" & hdnpartycode.Value & "' and prc.rmcatcode   IN (" & strConditionrm & ")  order by  3"

                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    myDataAdapter.Fill(myDS1)
                    grdrmcategory.DataSource = myDS1

                    If myDS1.Tables(0).Rows.Count > 0 Then
                        grdrmcategory.DataBind()

                    Else
                        grdrmcategory.DataBind()

                    End If
                    Dim chkSelectrm As CheckBox
                    Dim lblselect1 As Label

                    For Each grdRow In grdrmcategory.Rows
                        chkSelectrm = CType(grdRow.FindControl("chkSelect"), CheckBox)

                        lblselect1 = CType(grdRow.FindControl("lblselect"), Label)
                        Dim lblrmcatcode As Label = grdRow.findcontrol("lblrmcatcode")

                        If lblselect1.Text = "1" Then
                            chkSelectrm.Checked = True

                        End If

                    Next

                    btngenerate.Style("display") = "block"



                    ' filldates()

                    ' btngenerate.Visible = False

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                clsDBConnect.dbConnectionClose(mySqlConn)
            End If
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
        dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select weekorder from view_contracts_mealsupp_weekdays(nolock) where mealsupplementid='" & refcode & "'")
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
    Private Sub ShowDates(ByVal RefCode As String)
        Try


            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox

            Dim TxtFromDateGlobal As New TextBox
            Dim TxtToDateGlobal As New TextBox
            Dim ddlExcl As HtmlSelect

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from view_contracts_mealsupp_excl_dates(nolock) Where mealsupplementid='" & RefCode & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Dim ct As Integer
            If mySqlReader.HasRows Then
                ct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "Select count(*) from view_contracts_mealsupp_excl_dates(nolock) Where mealsupplementid='" & RefCode & "'")
                fillDategrd(grdexdates, False, ct)

                For Each gvRow In grdexdates.Rows
                    mySqlReader.Read()
                    dpFDate = gvRow.FindControl("txtfromdate")
                    dpTDate = gvRow.FindControl("txttodate")
                    ddlExcl = gvRow.FindControl("ddlExcl")
                    If IsDBNull(mySqlReader("fromdate")) = False Then

                        dpFDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then

                        dpTDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                    End If
                    If IsDBNull(mySqlReader("exclfor")) = False Then
                        ddlExcl.Value = CType(mySqlReader("exclfor"), String)

                    End If


                Next

            Else
                fillDategrd(grdexdates, True)
            End If


            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open            
            'mySqlCmd = New SqlCommand("Select * from view_contracts_mealsupp_excl_dates(nolock) Where mealsupplementid='" & RefCode & "'", mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            'If mySqlReader.HasRows Then
            '    While mySqlReader.Read()
            '        For Each gvRow In grdexdates.Rows
            '            dpFDate = gvRow.FindControl("txtfromdate")
            '            dpTDate = gvRow.FindControl("txttodate")
            '            '  Dim ddlExcl As HtmlSelect = gvRow.FindControl("ddlExcl")
            '            If dpFDate.Text = "" And dpFDate.Text = "" Then
            '                If IsDBNull(mySqlReader("fromdate")) = False Then
            '                    dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

            '                End If
            '                If IsDBNull(mySqlReader("todate")) = False Then
            '                    dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
            '                End If
            '                If IsDBNull(mySqlReader("exclfor")) = False Then
            '                    ddlExcl.Value = CType(mySqlReader("exclfor"), String)

            '                End If
            '                Exit For
            '            End If
            '        Next
            '    End While
            'End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ' clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection  
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close   
        End Try
    End Sub
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



            StrQry = "select distinct d.mlineno,d.rmtypcode from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid and  h.mealsupplementid='" & plistcode & "'" ' and h.contractid='" & hdncontractid.Value & "'"




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        txtrmtypcode = gvrow.FindControl("txtrmtypcode")
                        If IsDBNull(mySqlReader("mlineno")) = False Then
                            Linno = mySqlReader("mlineno")
                        End If
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            rmtypecode = mySqlReader("rmtypcode")
                        End If

                        If txtrmtypcode Is Nothing = False Then
                            'If rmtypecode <> "" Then
                            'If txtrmtypcode.Text = rmtypecode Then ' And gvrow.Cells(3).Text = mealcode Then


                            'StrQryTemp = "select d.* from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid  and  h.mealsupplementid='" & txtplistcode.Text & "'  and rmtypcode='" & txtrmtypcode.Text & "'" ' and h.contractid='" & hdncontractid.Value & "'"

                            StrQryTemp = "select d.* from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid  and  h.mealsupplementid='" & plistcode & "'  and rmtypcode='" & txtrmtypcode.Text & "'" ' 


                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("rmcatcode")) = False Then
                                        rmcatcode = myReader("rmcatcode")
                                    End If
                                    If IsDBNull(myReader("cprice")) = False Then
                                        value = DecRound(myReader("cprice"))
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 8
                                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                                        Else
                                            For s = 0 To grdRoomrates.Columns.Count - 8
                                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = rmcatcode Then
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
                                                                Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1) 'changed by mohamed on 21/02/2018
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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



            StrQry = "select distinct d.mlineno,d.rmtypcode from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid and  h.mealsupplementid='" & txtplistcode.Text & "'" ' and h.contractid='" & hdncontractid.Value & "'"




            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            mySqlCmd = New SqlCommand(StrQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read
                    For Each gvrow In grdRoomrates.Rows

                        txtrmtypcode = gvrow.FindControl("txtrmtypcode")
                        If IsDBNull(mySqlReader("mlineno")) = False Then
                            Linno = mySqlReader("mlineno")
                        End If
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            rmtypecode = mySqlReader("rmtypcode")
                        End If

                        If txtrmtypcode Is Nothing = False Then
                            'If rmtypecode <> "" Then
                            'If txtrmtypcode.Text = rmtypecode Then ' And gvrow.Cells(3).Text = mealcode Then


                            'StrQryTemp = "select d.* from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid  and  h.mealsupplementid='" & txtplistcode.Text & "'  and rmtypcode='" & txtrmtypcode.Text & "'" ' and h.contractid='" & hdncontractid.Value & "'"

                            StrQryTemp = "select d.* from view_contracts_mealsupp_detail d(nolock), view_contracts_mealsupp_header h(nolock) where h.mealsupplementid=d.mealsupplementid  and  h.mealsupplementid='" & txtplistcode.Text & "'  and rmtypcode='" & txtrmtypcode.Text & "'" ' 


                            myConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            myCmd = New SqlCommand(StrQryTemp, myConn)
                            myReader = myCmd.ExecuteReader
                            If myReader.HasRows Then
                                While myReader.Read
                                    If IsDBNull(myReader("rmcatcode")) = False Then
                                        rmcatcode = myReader("rmcatcode")
                                    End If
                                    If IsDBNull(myReader("cprice")) = False Then
                                        value = DecRound(myReader("cprice"))
                                    Else
                                        value = ""
                                    End If

                                    For j = 0 To cnt - 8
                                        If heading(j) = "Min Nights" Or heading(j) = "No.of.Nights Room Rate" Or heading(j) = "Unityesno" Or heading(j) = "Extra Person Supplement" Or heading(j) = "Unityesno" Or heading(j) = "Noofextraperson" Then
                                        Else
                                            For s = 0 To grdRoomrates.Columns.Count - 8
                                                headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                                                If headerlabel.Text <> Nothing Then
                                                    If headerlabel.Text = rmcatcode Then
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
                                                                Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1) 'changed by mohamed on 21/02/2018
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            clsDBConnect.dbConnectionClose(mySqlConn)
        End Try
    End Sub
#End Region
    Private Sub ShowDatesnew(ByVal RefCode As String)
        Try



            Dim strSqlQry As String = ""
            Dim cnt As Integer = 0

            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                strSqlQry = "select count( distinct fromdate) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"
            Else
                strSqlQry = "select count( distinct fromdate) from view_contracts_mealsupp_dates(nolock) where mealsupplementid='" & RefCode & "'"
            End If

            '" ' and subseasnname = '" & subseasonname & "'"
            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSqlQry)
            If cnt = 0 Then cnt = 1
            fillDategrd(grdDates, False, cnt)

            Dim gvRow As GridViewRow
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open      
            If (ViewState("State") = "New" Or ViewState("State") = "Copy") Then
                mySqlCmd = New SqlCommand("Select * from view_offers_detail(nolock) Where promotionid='" & RefCode & "'", mySqlConn)
            Else
                mySqlCmd = New SqlCommand("Select * from view_contracts_mealsupp_dates(nolock) Where mealsupplementid='" & RefCode & "'", mySqlConn)
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
            objUtils.WritErrorLog("ContractMEalsupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName <> "View" Then

                If Session("Calledfrom") = "Offers" Then
                    ' Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from  edit_offers_header(nolock) where promotionid='" & hdnpromotionid.Value & "'")

                    Dim offerexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top(1) 't' from  new_edit_offersmain_spl (nolock) where Promotion_ID like '" & hdnpromotionid.Value & "%'")

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

                'wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                'Session("isAutoTick_wuccountrygroupusercontrol") = 1
                'wucCountrygroup.sbShowCountry()

                If Session("Calledfrom") = "Offers" Then

                    'Rosalin
                    wucCountrygroup.sbSetPageState("", "OFFERSMEAL", ViewState("State"))
                    wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    '##
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Meal Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Meal Supplements "
                    lblratetype.Text = "Promotion"
                Else
                    '## Rosalin 
                    wucCountrygroup.sbSetPageState("", "CONTRACTMEAL", ViewState("State"))
                    wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                    Session("isAutoTick_wuccountrygroupusercontrol") = 1
                    wucCountrygroup.sbShowCountry()
                    '##
                    divoffer.Style.Add("display", "none")
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Edit Meal Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Meal Supplements "
                End If



                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                ShowDynamicGrid()
                fillDategrd(grdexdates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                grdRoomrates.Visible = True
                btnSave.Visible = True
                btnSave.Text = "Update"

                PanelMain.Visible = True


            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))

                If Session("Calledfrom") = "Offers" Then
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "View Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Meal Supplements "
                    lblratetype.Text = "Promotion"
                Else
                    divoffer.Style.Add("display", "none")
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "View Meal Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Meal Supplements "
                End If

                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                ShowDynamicGrid()
                fillDategrd(grdexdates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                DisableControl()
                btnSave.Visible = False

            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                ShowRecord(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                If Session("Calledfrom") = "Offers" Then
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Delete Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Meal Supplements "
                    lblratetype.Text = "Promotion"
                Else
                    divoffer.Style.Add("display", "none")
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Delete Meal Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Meal Supplements "
                End If
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                ShowDynamicGrid()
                fillDategrd(grdexdates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"

            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"
                ShowRecord(CType(lbltran.Text.Trim, String))
                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                If Session("Calledfrom") = "Offers" Then
                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Copy Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Meal Supplements "
                    lblratetype.Text = "Promotion"
                Else
                    divoffer.Style.Add("display", "none")
                    createdatatable()
                    createdatarows()
                    lblHeading.Text = "Copy Meal Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Meal Supplements "
                End If
                filldaysgrid()
                fillweekdays(CType(lbltran.Text.Trim, String))
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                ShowDynamicGrid()
                fillDategrd(grdexdates, True)
                ShowDates(CType(lbltran.Text.Trim, String))

                fillDategrd(grdDates, True)
                ShowDatesnew(CType(lbltran.Text.Trim, String))

                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"

            End If
            ' btnregenerate_Click(sender, e)

        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            Session("seasons") = Nothing
            Dim oldseason As String = ""

            For Each gvRow As GridViewRow In grdseason.Rows
                chk2 = gvRow.FindControl("chkseason")
                txtmealcode1 = gvRow.FindControl("txtseasoncode")

                If chk2.Checked = True And oldseason <> txtmealcode1.Text Then
                    seasonname = seasonname + RTrim(LTrim(txtmealcode1.Text)) + ","
                    oldseason = RTrim(LTrim(txtmealcode1.Text))
                End If
            Next

            If seasonname.Length > 0 Then
                seasonname = seasonname.Substring(0, seasonname.Length - 1)
            End If

            Session("seasons") = seasonname

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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






            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


            strSqlQry = "select count(v.rmtypcode) from  partyrmtyp pr,view_offers_rmtype v(nolock) where v.partycode =pr.partycode and v.rmtypcode=pr.rmtypcode  and v.promotionid='" & hdnpromotionid.Value & "' and  pr.inactive=0 and pr.partycode='" & hdnpartycode.Value & "'"

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




            strSqlQry = "select v.rmtypcode,pr.rmtypname,'' mealcode,0 pricepax, 0 unityesno ,0 noofextraperson from  partyrmtyp pr,view_offers_rmtype v(nolock) where v.partycode =pr.partycode and " _
                & " v.rmtypcode=pr.rmtypcode  and v.promotionid='" & hdnpromotionid.Value & "' and pr.inactive=0 and pr.partycode='" & hdnpartycode.Value & "'  order by pr.rankord"

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
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






            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    n = n + 1
                    'lblseason = GVRow.FindControl("lblseason")
                    'excl(n) = CType(lblseason.Text, String)
                Else
                    deletedrow = deletedrow + 1
                End If

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
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click

        Dim roomcat As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select count(prc.rmcatcode) from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.active=1 and rc.accom_extra='M' and prc.partycode='" _
                     & hdnpartycode.Value & "'")


        If roomcat = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Hotel Room Category Not Selected In Master Please Update.');", True)
            Exit Sub
        End If

        ViewState("State") = "New"

        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        lblstatus.Visible = False
        lblstatustext.Visible = False

        If Session("Calledfrom") = "Offers" Then

            Dim roomexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_offers_supplement c(nolock) join rmcatmast r on c.rmcatcode=r.rmcatcode where r.active=1 and r.accom_extra='M' and  promotionid='" & hdnpromotionid.Value & "' ")

            If roomexist = "" Then
                ViewState("State") = ""
                PanelMain.Style("display") = "none"
                Panelsearch.Enabled = True
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Promotion Meal Supplements not selected Please update.');", True)
                Exit Sub
            End If


            divoffer.Style.Add("display", "block")
            ' wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(ViewState("State"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "NEW")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
            filldaysgrid()
            fillweekdaysoffers(CType(hdnpromotionid.Value, String))
            FillMealplans()
            btncopyratesnextrow.Visible = False

            lable12.Visible = False
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            fillDategrd(grdDates, True)
            ShowDatesnew(CType(hdnpromotionid.Value, String))
            fillDategrd(grdexdates, True)
            fillcategory()
            FillRoomdetails()

            lblratetype.Text = "Promotions"

            btnSave.Text = "Save"
            lblHeading.Text = "New Meal Supplements - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "Meal Supplements -" + ViewState("hotelname") + "-" + hdnpromotionid.Value
        Else

            divoffer.Style.Add("display", "none")
            Session("contractid") = hdncontractid.Value
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")

            fillseason()
            grdDates.Visible = True
            grdexdates.Visible = True
            fillDategrd(grdexdates, True)
            fillDategrd(grdDates, True)

            filldaysgrid()
            FillMealplans()
            seasonsgridfill()
            wucCountrygroup.Visible = True

            divcopy1.Style("display") = "none"

            btncopyratesnextrow.Visible = False


            lable12.Visible = False
            btnfillrate.Visible = False
            txtfillrate.Visible = False
            fillcategory()
            FillRoomdetails()


            btnSave.Visible = True
            lblratetype.Text = "Contract"

            btnSave.Text = "Save"
            lblHeading.Text = "New Meal Supplements - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "Meal Supplements -" + ViewState("hotelname")

            ' divuser.Style("display") = "none"
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
        End If

        chkVATCalculationRequired.Checked = True
        Call sbFillTaxDetail() 'changed by mohamed on 21/02/2018
    End Sub
    Sub fillcategory()
        Try


            Dim myDS As New DataSet
            grdmealplan.Visible = True
            strSqlQry = ""

            If Session("Calledfrom") = "Offers" Then
                strSqlQry = "select rmcatcode,selected  from  (select distinct s.rmcatcode,1 selected,isnull(rc.rankorder,999) rankorder from partyrmcat prc,rmcatmast rc,view_offers_supplement s where s.rmcatcode=prc.rmcatcode and s.promotionid='" & hdnpromotionid.Value & "' and  rc.active=1 and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                     & hdnpartycode.Value & "') rs order by isnull(rs.rankorder,999)"
            Else

                strSqlQry = "select prc.rmcatcode,0 selected from partyrmcat prc,rmcatmast rc where rc.active=1 and prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
                     & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"
            End If


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            grdrmcategory.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdrmcategory.DataBind()

            Else
                grdrmcategory.DataBind()

            End If
            Dim chkSelect As CheckBox

            For Each grdRow In grdrmcategory.Rows
                chkSelect = CType(grdRow.FindControl("chkSelect"), CheckBox)
                chkSelect.Checked = True

            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
        '    Dim myDS As New DataSet

        '    dlrmcategory.Visible = True
        '    strSqlQry = ""


        '    ' strSqlQry = "select  promtoiontype from promotion_types(nolock)  order by autoid "
        '    strSqlQry = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra='M' and prc.partycode='" _
        '              & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"  ' p.rmcatcode " '

        '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        '    myDataAdapter.Fill(myDS)

        '    dlrmcategory.DataSource = myDS

        '    If myDS.Tables(0).Rows.Count > 0 Then

        '        dlrmcategory.DataBind()

        '    Else

        '        dlrmcategory.DataBind()

        '    End If
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'Finally

        '    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
        '    clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        'End Try
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
        ' Response.Redirect(Request.RawUrl)
        lblHeading.Text = "Meal Supplements  -" + ViewState("hotelname") + " -" + hdncontractid.Value
        wucCountrygroup.clearsessions()
        Page.Title = "Contract Meal Supplements "
        wucCountrygroup.sbSetPageState("", "CONTRACTMEAL", CType(Session("ContractState"), String))
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

        'If flg = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Dates grid cannot be blank');", True)
        '    ValidatePage = False
        '    Exit Function
        'End If
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

    Protected Sub btngenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerate.Click
        'If ValidatePage() = False Then
        '    Exit Sub
        'End If




    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        filldates()

    End Sub
    Protected Sub grdRoomrates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRoomrates.RowDataBound

        Dim txtunityesno As TextBox
        Dim txtnoextra As TextBox

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            txtunityesno = e.Row.FindControl("txtunityesno")
            txtnoextra = e.Row.FindControl("txtnoofextraperson")
            'For i = 0 To grdRoomrates.Columns.Count - 8
            '    Dim l = IIf(e.Row.RowIndex >= 1, ((grdRoomrates.Columns.Count - 7) * e.Row.RowIndex), 0)
            '    Dim txt1 As TextBox = e.Row.FindControl("txt" & i + l)
            '    If txt1.Style("FieldHeader") <> "" Then
            '        If txtunityesno.Text = 0 Then
            '            If txt1.Style("FieldHeader").Contains("UNIT") Then
            '                txt1.Enabled = False
            '            End If
            '        ElseIf txtunityesno.Text = 1 Then
            '            If (Not txt1.Style("FieldHeader").Contains("Extra Person Supplement")) And (Not txt1.Style("FieldHeader").Contains("UNIT")) Then
            '                txt1.Enabled = False
            '            End If
            '        End If

            '        If txt1.Style("FieldHeader").Contains("Extra Person Supplement") Then
            '            If txtunityesno.Text = 1 And txtnoextra.Text <> 0 Then
            '                txt1.Enabled = True
            '            Else
            '                txt1.Enabled = False
            '            End If


            '        End If


            '    End If
            'Next

            '-----------------------
            'changed by mohamed on 21/02/2018
            Dim l As Integer
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

                If txt1.Style("FieldHeader") <> "" Then
                    'changed by mohamed on 21/02/2018
                    If txt1.Style("FieldHeader").Contains("Sr No") = False And txt1.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt1.Style("FieldHeader").Contains("Extra Person Supplement") = False And txt1.Style("FieldHeader").Contains("Min Nights") = False And txt1.Style("FieldHeader").Contains("Pkg") = False And txt1.Style("FieldHeader").Contains("Remark") = False Then
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
            '-----------------------
            
        End If

        ''' Commented shahul 21/01/17 becoz its not working
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = grdRoomrates.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "2"
        '    e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If


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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
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

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
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
                            If heading(header) = "Min Nights" Or heading(header) = "No.of.Nights Room Rate" Or heading(header) = "Unityesno" Or heading(header) = "Extra Person Supplement" Or heading(header) = "Unityesno" Or heading(header) = "Noofextraperson" Then
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

                                        txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                        txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                        txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                        If txtTV1 IsNot Nothing Then
                                            Call assignVatValueToTextBox(txt, txtTV1, txtNTV1, txtVAT1)
                                        End If
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
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

        'changed by mohamed on 21/02/2018
        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing

        Try
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
                            'txt = GvRow.FindControl("txt" & b + a + 3)
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If heading(j) = ddlCopymeal.Value Then
                                    txt.Text = txtfillrate.Text

                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                    If txtTV1 IsNot Nothing Then
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
                        If heading(k) = "Min Nights" Or heading(k) = "No.of.Nights Room Rate" Or heading(k) = "Unityesno" Or heading(k) = "Extra Person Supplement" Or heading(k) = "Unityesno" Or heading(k) = "Noofextraperson" Then
                        Else
                            ' txt = GvRow.FindControl("txt" & b + a + 3)
                            txt = GvRow.FindControl("txt" & j)
                            If txt Is Nothing Then
                            Else
                                If heading(k) = ddlCopymeal.Value Then
                                    txt.Text = txtfillrate.Text

                                    txtTV1 = GvRow.FindControl("txtTV" & j) 'changed by mohamed on 21/02/2018
                                    txtNTV1 = GvRow.FindControl("txtNTV" & j)
                                    txtVAT1 = GvRow.FindControl("txtVAT" & j)
                                    If txtTV1 IsNot Nothing Then
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
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        '    objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        '    'For Each GvRow In grdRoomrates.Rows
        '    '    chk = GvRow.FindControl("ChkSelect")
        '    '    If chk.Checked = True Then
        '    '        cnt_checked = cnt_checked + 1
        '    '    End If
        '    'Next
        '    'If cnt_checked = 0 Then
        '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select at least one row.');", True)
        '    '    SetFocus(cnt_checked)
        '    '    Exit Sub
        '    'End If


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
        '    objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            'promotionlist.Add("Free")
            'promotionlist.Add("Incl")
            'promotionlist.Add("N.Incl")
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
        Dim heading((cnt - 8) * 7) As String
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        Dim arr_room((header) + 1) As String
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
                        If txtrmtypcode.Text = ddlRoomfrom.Value Then 'And mealcode = IIf(ddlmealfrom.Value <> "[Select]", ddlmealfrom.Value, mealcode) Then
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
                        If txtrmtypcode.Text = ddlRoomfrom.Value Then 'And mealcode = IIf(ddlmealfrom.Value <> "[Select]", ddlmealfrom.Value, mealcode) Then
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
                            If txtrmtypcode.Text = ddlroomto.Value Then 'And mealcode = IIf(ddlmealto.Value <> "[Select]", ddlmealto.Value, mealcode) Then
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
                            If txtrmtypcode.Text = ddlroomto.Value Then 'And mealcode = IIf(ddlmealto.Value <> "[Select]", ddlmealto.Value, mealcode) Then
                                txt = GvRow.FindControl("txt" & j)
                                If txt Is Nothing Then
                                Else
                                    'Check the price exists in the line
                                    roomcatstring = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='No'  and rmcatcode='" & Trim(heading(room)) & "'")
                                    If heading(room) = roomcatstring Then
                                        If Val(txt.Text) <> "0" Or txt.Text <> "" Then
                                            validprice = 1
                                        End If
                                    End If
                                    roomcat = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatcode from rmcatmast where allotreqd='No'  and rmcatcode='" & Trim(heading(room)) & "'")
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

    Protected Sub btnAddLinesDates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLinesDates.Click


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
            Dim strFoucsColumnIndex As String = "0"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdexdates.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
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
                Else
                    deletedrow = deletedrow + 1
                End If
                n = n + 1
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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub grdexdates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdexdates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
                Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
                'Dim btnImgRmv As ImageButton = CType(e.Row.FindControl("btnImgRmv"), ImageButton)
                txtFromDate.Attributes.Add("onchange", "setdate();")
                txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                txtFromDate.Attributes.Add("onchange", "checkfromdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

            End If
            If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
                Dim strGridName As String = grdexdates.ClientID
                Dim strRowId As String = e.Row.RowIndex
                Dim strFoucsColumnIndex = "0"
                e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
                e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
            End If
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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try



            'strSqlQry = "select h.contractid,h.mealsupplementid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
            '     " h.applicableto  ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek   from view_contracts_mealsupp_header h(nolock),view_contracts_search d(nolock)  where h.contractid =d.contractid and d.partycode='" & hdnpartycode.Value & "' and   h.contractid <>'" & hdncontractid.Value & "'"

            If Session("Calledfrom") = "Offers" Then

                strSqlQry = "New_Copy_mealsupplement '" & CType(hdnpartycode.Value, String) & "' , ''"

                '               strSqlQry = "select h.contractid as contractid,h.mealsupplementid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek    from view_contracts_mealsupp_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and     h.seasons <>'' union all " _
                '& " select h.contractid as contractid,h.mealsupplementid plistcode,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
                '& " ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek  from view_contracts_mealsupp_header h(nolock),view_contracts_mealsupp_dates d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.mealsupplementid=d.mealsupplementid and      h.seasons ='' group by h.contractid, h.mealsupplementid,h.applicableto  "


            Else
                ' Added By rosalin 2019-10-22 



                strSqlQry = "New_Copy_mealsupplement '" & CType(hdnpartycode.Value, String) & "' , '" & CType(hdncontractid.Value, String) & "'"



                '               strSqlQry = "select h.contractid as contractid,h.mealsupplementid plistcode,h.seasons seasoncode,dbo.fn_get_seasonmindate(h.seasons,h.contractid) fromdate, dbo.fn_get_seasonmaxdate(h.seasons,h.contractid)  todate," & _
                '" h.applicableto,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek    from view_contracts_mealsupp_header h(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and    h.contractid<>'" & hdncontractid.Value & "' and h.seasons <>'' union all " _
                '& " select h.contractid as contractid,h.mealsupplementid plistcode,'' seasoncode, convert(varchar(10),min(d.fromdate),103) fromdate  ,convert(varchar(10),max(d.todate),103) todate, h.applicableto   " _
                '& " ,dbo.fn_get_weekdays_fromtable('view_contracts_mealsupp_header',h.mealsupplementid) DaysoftheWeek  from view_contracts_mealsupp_header h(nolock),view_contracts_mealsupp_dates d(nolock),view_contracts_search s(nolock)  where isnull(s.withdraw,0)=0  and h.contractid= s.contractid and s.partycode='" & hdnpartycode.Value & "' and  h.mealsupplementid=d.mealsupplementid and     h.contractid<>'" & hdncontractid.Value & "' and h.seasons ='' group by h.contractid, h.mealsupplementid,h.applicableto  "

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
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                ShowRecordcopycontract(CType(lbltran.Text.Trim, String))

                If Session("Calledfrom") = "Offers" Then

                    Dim roomexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_offers_supplement c(nolock) join rmcatmast r on c.rmcatcode=r.rmcatcode where r.active=1 and r.accom_extra='M' and  promotionid='" & hdnpromotionid.Value & "' ")

                    If roomexist = "" Then
                        ViewState("State") = ""
                        PanelMain.Style("display") = "none"
                        Panelsearch.Enabled = True
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Promotion Meal Supplements not selected Please update.');", True)
                        Exit Sub
                    End If


                    'wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(hdnpromotionid.Value, String))
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

                    'wucCountrygroup.sbSetPageState("", "OFFERSMEAL", CType(lbltran.Text.Trim, String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()

                    createdatatableoffer()
                    createdatarowsoffer()
                    lblHeading.Text = "Meal Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Meal Supplements "
                    lblratetype.Text = "Promotion"

                    filldaysgrid()
                    fillweekdaysoffers(CType(hdnpromotionid.Value, String))

                    ShowDynamicGrid()
                    fillDategrd(grdexdates, True)
                    ShowDates(CType(lbltran.Text.Trim, String))

                    fillDategrd(grdDates, True)
                    ShowDatesnew(CType(hdnpromotionid.Value, String))


                Else
                    wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
                    wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
                    createdatatable()
                    createdatarows()

                    filldaysgrid()
                    fillweekdays(CType(lbltran.Text.Trim, String))
                    wucCountrygroup.sbShowCountry()
                    ShowDynamicGrid()
                    fillDategrd(grdexdates, True)
                    ShowDates(CType(lbltran.Text.Trim, String))

                    fillDategrd(grdDates, True)
                    ShowDatesnew(CType(lbltran.Text.Trim, String))

                    lblHeading.Text = "Copy Meal Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                    Page.Title = "Contract Meal Supplements "
                End If


              

                lblstatus.Visible = False
                lblstatustext.Visible = False
                btnSave.Visible = True
                txtplistcode.Text = ""
                btnSave.Text = "Save"
              
                fillseason()


            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractMealSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnregenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnregenerate.Click

        Dim lblrmcatcode As Label
        Dim chksel As CheckBox
        Dim rmcatstr As String = ""
        Dim rmcatcount As Integer = 0
        Session("rmcategorys") = Nothing
        For Each gvrow In grdrmcategory.Rows
            chksel = gvrow.FindControl("chkSelect")
            lblrmcatcode = gvrow.FindControl("lblrmcatcode")
            If chksel.Checked = True Then
                rmcatstr = rmcatstr + "," + lblrmcatcode.Text
                rmcatcount += 1
            End If
        Next

        If rmcatcount = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Room category Should not blank Please select one Category ');", True)
            Exit Sub
        End If
        If rmcatstr.Length > 0 Then
            Session("rmcategorys") = Right(rmcatstr, Len(rmcatstr) - 1) 'mealplanstr


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
                If heading(j) <> "Min Nights" And heading(j) <> "No.of.Nights Room Rate" And heading(j) <> "Unityesno" And heading(j) <> "Extra Person Supplement" And heading(j) <> "Unityesno" And heading(j) <> "Noofextraperson" Then

                    For s = 0 To grdRoomrates.Columns.Count - 8
                        headerlabel = grdRoomrates.HeaderRow.FindControl("txtHead" & s)
                        If headerlabel.Text <> Nothing Then
                            If Convert.ToString(Session("rmcategorys")).ToUpper.Trim.Contains(headerlabel.Text.ToUpper.Trim) = False Then

                                ' headerlabel.Visible = False
                                grdRoomrates.Columns(s + 7).ItemStyle.CssClass = "displaynone"
                                grdRoomrates.Columns(s + 7).HeaderStyle.CssClass = "displaynone"
                            Else
                                ' headerlabel.Visible = True
                                ' grdRoomrates.Columns(s + 7).Visible = True
                                grdRoomrates.Columns(s + 7).ItemStyle.CssClass = ""
                                grdRoomrates.Columns(s + 7).HeaderStyle.CssClass = ""
                            End If
                        End If

                    Next
                End If
            Next

        

        End If

       
        ' FillRoomdetails()
    End Sub
    Private Sub sortgvsearch()
        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("plistcode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("seasoncode", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("fromdate", hdncontractid.Value, hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
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
    End Sub
    Protected Sub ddlorder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorder.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub ddlorderby_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlorderby.SelectedIndexChanged
        sortgvsearch()
    End Sub

    Protected Sub grdrmcategory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdrmcategory.RowDataBound
        Dim lblrmcatcode As Label
        Dim chkselect As CheckBox

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            lblrmcatcode = e.Row.FindControl("lblrmcatcode")
            chkselect = e.Row.FindControl("chkSelect")



            chkselect.Attributes.Add("onChange", "categoryfill('" & chkselect.ClientID & "')")


        End If
    End Sub

    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtFromDate As TextBox = CType(e.Row.FindControl("txtfromdate"), TextBox)
                Dim txtToDate As TextBox = CType(e.Row.FindControl("txttodate"), TextBox)
                'Dim btnImgRmv As ImageButton = CType(e.Row.FindControl("btnImgRmv"), ImageButton)

                If Session("Calledfrom") = "Offers" Then

                    txtFromDate.Attributes.Add("onchange", "setdate();")
                    txtToDate.Attributes.Add("onchange", "checkdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                    txtFromDate.Attributes.Add("onchange", "checkfromdatespromo('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

                Else
                    txtFromDate.Attributes.Add("onchange", "setdate();")
                    txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
                    txtFromDate.Attributes.Add("onchange", "checkfromdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
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

                'Dim roomexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_offers_supplement c(nolock) join rmcatmast r on c.rmcatcode=r.rmcatcode where r.active=1 and r.accom_extra='M' and  promotionid='" & hdnpromotionid.Value & "' ")

                'If roomexist = "" Then
                '    'ViewState("State") = ""
                '    'PanelMain.Style("display") = "none"
                '    'Panelsearch.Enabled = True
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In this Promotion Meal Supplements not selected Please update.');", True)
                '    Exit Sub
                'End If


              


                PanelMain.Style.Add("display", "block")
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))
                ShowRecordcopyoffer(CType(lblplistcode.Text.Trim, String))

                txtpromotionid.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)
                dv_SearchResult.Style.Add("display", "none")

                createdatatableoffer()
                createdatarowsoffer()
                lblHeading.Text = "Copy Meal Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Page.Title = "Promotion Meal Supplements "


                filldaysgrid()
                fillweekdays(CType(hdnpromotionid.Value, String))
                'ShowDynamicGrid()
                fillDategrd(grdexdates, True)
                ShowDates(CType(lblplistcode.Text.Trim, String))

                fillDategrd(grdDates, True)
                ShowDatesnew(CType(hdnpromotionid.Value, String))
                grdRoomrates.Visible = True
                btnSave.Visible = True
                PanelMain.Visible = True


                lblratetype.Text = "Promotion"


                'Rosalin 2019-11-07
                'wucCountrygroup.sbSetPageState("", "OFFERSMEAL", CType(lblplistcode.Text.Trim, String))

                wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                '###
                wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")

                'Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                ShowDynamicGridoffer(CType(lblplistcode.Text, String))
                btnSave.Visible = True
                btnSave.Text = "Save"

                lblstatus.Style.Add("display", "none")
                lblstatustext.Style.Add("display", "none")
                btngenerate.Style("display") = "none"


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
                            If txt1.Style("FieldHeader").Contains("Sr No") = False And txt1.Style("FieldHeader").Contains("No.of.Nights Room Rate") = False And txt1.Style("FieldHeader").Contains("Extra Person Supplement") = False And txt1.Style("FieldHeader").Contains("Min Nights") = False And txt1.Style("FieldHeader").Contains("Pkg") = False And txt1.Style("FieldHeader").Contains("Remark") = False Then
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
        strSqlQry = "execute sp_get_taxdetail_frommaster '" & hdnpartycode.Value & "',5102" ' "select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'" 'changed by mohamed on 17/04/2018
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
End Class

