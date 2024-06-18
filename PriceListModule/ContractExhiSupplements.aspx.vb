Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports ColServices


Partial Class ContractExhiSupplements
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


    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter

    Dim blankrow As Integer = 0
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim count As Integer = 0
    Dim Exhnamenew As New ArrayList
    Dim Roomtypenew As New ArrayList
    Dim Mealplannew As New ArrayList
    Dim Suppamountnew As New ArrayList
    Dim Minstaynew As New ArrayList
    Dim withdrawnnew As New ArrayList
    Dim withdrawnamtnew As New ArrayList
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList
    Dim Exhicodenew As New ArrayList
    Dim CopyRowlist As New ArrayList


#End Region
#Region "Enum GridCol"
    Enum GridCol
        ExhibitionId = 1
        Applicableto = 2
        Edit = 6
        View = 7
        Delete = 8
        Copy = 9
        DateCreated = 10
        UserCreated = 11
        DateModified = 12
        UserModified = 13

    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        Dim CalledfromValue As String = ""
        Dim ConExappid As String = ""
        Dim ConExappname As String = ""

        Dim Count As Integer
        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim functionalrights As String = ""

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
                                                       CType(ConExappname, String), "ContractExhiSupplements.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                 btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval

                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConExappname, String), "ContractExhiSupplements.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnprint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View, 0, GridCol.Copy)
                End If

            End If

            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractExhiSupplements.aspx", ConExappid, CalledfromValue)

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

            If Session("Calledfrom") = "Offers" Then

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                ViewState("Menucalling") = Me.SubMenuUserControl1.menuidval

                txtconnection.Value = Session("dbconnectionName")

                hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")
                divoffer.Style.Add("display", "block")
                btncopycontract.Style.Add("display", "block")
                btnselect.Style.Add("display", "block")

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                hdncontractid.Value = CType(Session("contractid"), String)


                Page.Title = "Exhibition" + " - " + PanelMain.GroupingText
                gv_SearchResult.Columns(2).Visible = True
                gv_SearchResult.Columns(3).Visible = True

                gv_Filldata.Columns(9).Visible = False
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
                wucCountrygroup.sbSetPageState("", "OFFERSEXHIBITION", CType(Session("OfferState"), String))

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text




                lblHeading.Text = lblHeading.Text + " - " + txthotelname.Text + " - " + hdnpromotionid.Value

                Page.Title = "Exhibition Supplements "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                FillGrid("exhibitionid", hdnpromotionid.Value, "Desc")


            Else

                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                ViewState("Menucalling") = Me.SubMenuUserControl1.menuidval
                '  objUtils.Clear_All_contract_sessions()
                txtconnection.Value = Session("dbconnectionName")

                hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")
                btnselect.Style.Add("display", "none")
                divoffer.Style.Add("display", "none")
                ' btncopycontract.Style.Add("display", "none")
                hdnpartycode.Value = CType(Session("Contractparty"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Page.Title = " Contract Exhibition" + " - " + PanelMain.GroupingText

                gv_SearchResult.Columns(2).Visible = False
                gv_SearchResult.Columns(3).Visible = False
                gv_Filldata.Columns(9).Visible = True
                gv_Filldata.Columns(10).Visible = False
                wucCountrygroup.sbSetPageState("", "CONTRACTEXHIBITION", CType(Session("ContractState"), String))

                txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = txthotelname.Text




                lblHeading.Text = lblHeading.Text + " - " + txthotelname.Text + " - " + hdncontractid.Value

                Page.Title = "Contract Exhibition Supplements "

                ddlorder.SelectedIndex = 0
                ddlorderby.SelectedIndex = 1
                FillGrid("exhibitionid", hdncontractid.Value, "Desc")

            End If



            'FillGrid("partymaxacc_header.tranid", hdnpartycode.Value, "Desc")
            '   PanelMain.Visible = False

        Else
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
        End If

        chkctrygrp.Attributes.Add("onChange", "showusercontrol('" & chkctrygrp.ClientID & "')")

        hdndecimal.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters(nolock) where param_id=1140")
        Numberssrvctrl(txtminnights)

        'If Session("Calledfrom") = "Offers" Then
        '    btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdnpromotionid.Value & "')")
        '    '  btnAd("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
        'Else
        '    btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontraddNew.Attributes.Add("onclick", "return Checkcommission('" & hdnpromotionid.Value & "','" & hdncommtype.Value & "')")
        '    '  btncopycontract.Attributes.Adctid.Value & "')")
        '    ' btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        'End If
        If Session("Calledfrom") <> "Offers" Then
            btnAddNew.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            btncopycontract.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        End If



        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If
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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = "select a.agentname, a.ctrycode from agentmast a where a.active=1 and a.agentname like  '%" & Trim(prefixText) & "%'"

            'Dim wc As New PriceListModule_Countrygroup
            'wc = wucCountrygroup
            'lsCountryList = wc.fnGetSelectedCountriesList
            If HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl") IsNot Nothing Then
                lsCountryList = HttpContext.Current.Session("SelectedCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    '  strSqlQry += " and a.ctrycode in (" & lsCountryList & ")"
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
#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex


        'FillGrid("exnibitionid", hdnpartycode.Value, "Desc")
        sortgvsearch()


    End Sub

#End Region
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal

        Rdamt = Math.Round(Val(Ramt), CType(hdndecimal.Value, Integer))
        Return Rdamt
    End Function
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
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub FillGrid(ByVal strorderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = ""
        Try

            '   If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then

            If Session("Calledfrom") = "Offers" Then

                strSqlQry = "SELECT e.exhibitionid,ISNULL(e.promotionid,'') promotionid,h.promotionname, e.applicableto,'' SpecificCountriesAgents, e.adddate,e.adduser,e.moddate,e.moduser FROM view_contracts_exhibition_header e(nolock),view_offers_header h(nolock) where " _
               & " e.promotionid=h.promotionid and e.promotionid='" & hdnpromotionid.Value & "' order by " & strorderby & " " & strsortorder & ""
            Else
                strSqlQry = "SELECT exhibitionid,'' promotionid,'' promotionname, applicableto,'' SpecificCountriesAgents, ISNULL(promotionid,'') promotionid,adddate,adduser,moddate,moduser FROM view_contracts_exhibition_header(nolock) where " _
                & " contractid='" & hdncontractid.Value & "' order by " & strorderby & " " & strsortorder & ""
            End If

           




            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_exhibition_header Where exhibitionid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("exhibitionid")) = False Then
                        txttranid.Text = CType(mySqlReader("exhibitionid"), String)
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
                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        txtpromotionid.Text = CType(mySqlReader("promotionid"), String)
                        txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  pricecode from vw_promotion_header where  promotionid ='" & CType(mySqlReader("promotionid"), String) & "'")
                    Else
                        txtpromotionid.Text = ""
                        txtpromotionname.Text = ""
                    End If


                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  't' from edit_contracts_exhibition_header(nolock) where  exhibitionid ='" & CType(RefCode, String) & "'") <> "" Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "UNAPPROVED"

                    Else
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = "APPROVED"
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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub

    Private Sub ShowRecordCopy(ByVal RefCode As String) 'changed by mohamed on 24/02/2018

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_contracts_exhibition_header Where exhibitionid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    'If IsDBNull(mySqlReader("exhibitionid")) = False Then
                    '    txttranid.Text = CType(mySqlReader("exhibitionid"), String)
                    'End If
                    'If IsDBNull(mySqlReader("contractid")) = False Then
                    '    hdncontractid.Value = CType(mySqlReader("contractid"), String)
                    'End If
                    'If IsDBNull(mySqlReader("countrygroupsyesno")) = False Then
                    '    chkctrygrp.Checked = IIf(CType(mySqlReader("countrygroupsyesno"), String) = "1", True, False)
                    'Else
                    '    chkctrygrp.Checked = False
                    'End If
                    'If IsDBNull(mySqlReader("applicableto")) = False Then
                    '    txtApplicableTo.Text = CType(mySqlReader("applicableto"), String)
                    '    txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


                    'End If
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


            'If chkctrygrp.Checked = True Then
            '    divuser.Style("display") = "block"
            'Else
            '    divuser.Style("display") = "none"
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getpromotionlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim promotionlist As New List(Of String)
        Dim maxstate As String
        Try
            ' contextKey = Convert.ToString(HttpContext.Current.Session("partycode").ToString())
            maxstate = Convert.ToString(HttpContext.Current.Session("State").ToString())

            If CType(maxstate, String) = "Edit" Then
                strSqlQry = "select  pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' and  partycode='" & contextKey & "' and pricecode like  '" & prefixText & "%'"
            Else
                strSqlQry = "select  pricecode,promotionid from vw_promotion_header where  isnull(pricecode,'')<>'' and active=1 and convert(varchar(10),frmdate,111) >=GETDATE() and partycode='" & contextKey & "' and pricecode like  '" & prefixText & "%'"
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
                    ' promotionlist.Add(myDS.Tables(0).Rows(i)("pricecode").ToString())
                    promotionlist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("pricecode").ToString(), myDS.Tables(0).Rows(i)("promotionid").ToString()))
                Next

            End If

            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function GetExhibitionlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim roomclasslist As New List(Of String)
        Dim conid As String
        Dim offers As String = ""
        Dim promotionid As String = ""


        Try
            If Not HttpContext.Current.Session("contractid") Is Nothing Then
                conid = Convert.ToString(HttpContext.Current.Session("contractid").ToString())
            End If

            If Not HttpContext.Current.Session("OfferRefCode") Is Nothing Then
                promotionid = Convert.ToString(HttpContext.Current.Session("OfferRefCode").ToString())
            End If

            If Not HttpContext.Current.Session("Calledfrom") Is Nothing Then
                offers = Convert.ToString(HttpContext.Current.Session("Calledfrom").ToString())
            Else
                offers = ""
            End If


            If offers = "Offers" Then
                strSqlQry = " select distinct h.exhibitionname,h.exhibitioncode  from exhibition_master h,exhibition_detail d,view_offers_detail c(nolock)   where h.active=1 and  h.exhibitioncode =d.exhibitioncode  and c.promotionid ='" & promotionid & "' and  ((convert(varchar(10),d.fromdate,111) between c.fromdate   and c.todate )   " _
              & "  or   (convert(varchar(10),d.todate,111) between c.fromdate   and c.todate)  or (convert(varchar(10),d.fromdate,111) < c.fromdate  and  convert(varchar(10),d.todate,111)>c.todate))  and  h.exhibitionname like  '" & Trim(prefixText) & "%'  order by h.exhibitionname "

            Else

                strSqlQry = " select distinct  h.exhibitionname,h.exhibitioncode  from exhibition_master h,exhibition_detail d,view_contracts_search c(nolock)   where h.active=1 and  h.exhibitioncode =d.exhibitioncode  and c.contractid ='" & conid & "' and  ((convert(varchar(10),d.fromdate,111) between c.fromdate   and c.todate )   " _
              & "  or   (convert(varchar(10),d.todate,111) between c.fromdate   and c.todate)  or (convert(varchar(10),d.fromdate,111) < c.fromdate  and  convert(varchar(10),d.todate,111)>c.todate))  and  h.exhibitionname like  '" & Trim(prefixText) & "%'  order by h.exhibitionname "

            End If

            ' strSqlQry = "select exhibitionname,exhibitioncode from  exhibition_master where active=1  and  exhibitionname like  '" & Trim(prefixText) & "%' order by exhibitionname "


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'roomclasslist.Add(myDS.Tables(0).Rows(i)("roomclassname").ToString())
                    roomclasslist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("exhibitionname").ToString(), myDS.Tables(0).Rows(i)("exhibitioncode").ToString()))
                Next

            End If

            Return roomclasslist
        Catch ex As Exception
            Return roomclasslist
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
            lngcnt = 5
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
        'Dim lngcnt As Long
        'Dim cnt As Integer
        'If ViewState("State") = "New" Then
        '    cnt = 1
        'Else
        '    If Session("RefCode") <> "" Then
        '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        '        strSqlQry = "select count(*) from partymaxaccom_dates where tranid='" + Session("RefCode") + "'"

        '        mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
        '        cnt = mySqlCmd.ExecuteScalar
        '        mySqlConn.Close()
        '    End If
        'End If

        'If blnload = True Then
        '    lngcnt = IIf(cnt = 0, 1, cnt) '10
        'Else
        '    lngcnt = count
        'End If
        'grd.DataSource = CreateDataSource(lngcnt)
        'grd.DataBind()
        'grd.Visible = True
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



    Protected Sub btnAddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddrow.Click

        ' Createdatacolumns("add")

        ' Addlines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim Exhname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim Suppamount(count) As String
        Dim Minstay(count) As String
        Dim withdrawn(count) As String
        Dim withdrawnamt(count) As String
        Dim Exhicode(count) As String
        Dim eventtype(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtExhname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtSuppamount As TextBox
        Dim txtMinstay As TextBox
        Dim chkwithdrawn As CheckBox
        Dim chkSelect As CheckBox
        Dim txtExhicode As TextBox
        Dim chkwithdrawnamt As CheckBox
        Dim ddleventtype As HtmlSelect
        '   CopyRow = 0


        Try

            Dim roomtypes As String = ""
            Dim mealplans As String = ""

            If Session("Calledfrom") = "Offers" Then
                Dim strQry As String = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

                strQry = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
            End If



            For Each GVRow In gv_Filldata.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                chkSelect = GVRow.FindControl("chkSelect")
                txtExhicode = GVRow.FindControl("txtExhicode")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")
                ddleventtype = GVRow.FindControl("ddleventtype")

                If chkSelect.Checked = True Then
                    ' CopyRow = n
                End If

                fDate(n) = CType(dpFDate.Text, String)
                tDate(n) = CType(dpTDate.Text, String)
                Exhname(n) = CType(txtExhname.Text, String)

                
                Roomtype(n) = CType(txtRoomtype.Text, String)
                Mealplan(n) = CType(txtMealplan.Text, String)
                eventtype(n) = CType(ddleventtype.Value, String)

                Suppamount(n) = CType(txtSuppamount.Text, String)
                Minstay(n) = CType(txtMinstay.Text, String)
                Exhicode(n) = CType(txtExhicode.Text, String)
                If chkwithdrawn.Checked = True Then
                    withdrawn(n) = 1
                Else
                    withdrawn(n) = 0
                End If
                If chkwithdrawnamt.Checked = True Then
                    withdrawnamt(n) = 1
                Else
                    withdrawnamt(n) = 0
                End If

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtSuppamount IsNot Nothing Then
                    Call assignVatValueToTextBox(txtSuppamount, txtTV1, txtNTV1, txtVAT1)
                End If

                n = n + 1
            Next
            fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count + 1)



            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Filldata.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                txtExhicode = GVRow.FindControl("txtExhicode")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")
                ddleventtype = GVRow.FindControl("ddleventtype")

                dpFDate.Text = fDate(n)
                dpTDate.Text = tDate(n)
                txtExhname.Text = Exhname(n)
                ddleventtype.Value = eventtype(n)

                'If Session("Calledfrom") = "Offers" Then
                '    txtRoomtype.Text = roomtypes
                '    txtMealplan.Text = mealplans
                'Else
                txtRoomtype.Text = Roomtype(n)
                txtMealplan.Text = Mealplan(n)
                '   End If

                txtSuppamount.Text = Suppamount(n)
                txtMinstay.Text = Minstay(n)
                If withdrawn(n) = 1 Then
                    chkwithdrawn.Checked = True
                Else
                    chkwithdrawn.Checked = False
                End If
                txtExhicode.Text = Exhicode(n)
                If withdrawnamt(n) = 1 Then
                    chkwithdrawnamt.Checked = True
                Else
                    chkwithdrawnamt.Checked = False
                End If

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtSuppamount IsNot Nothing Then
                    Call assignVatValueToTextBox(txtSuppamount, txtTV1, txtNTV1, txtVAT1)
                End If

                n = n + 1
            Next

            If Session("Calledfrom") = "Offers" Then
                For Each GVRow In gv_Filldata.Rows

                    Dim txtrmtypcode As TextBox = GVRow.FindControl("txtrmtypcode")
                    Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")

                    txtrmtypcode.Text = roomtypes
                    txtmealcode.Text = mealplans

                Next
            End If


            Dim gridNewrow As GridViewRow
            gridNewrow = gv_Filldata.Rows(gv_Filldata.Rows.Count - 1)
            Dim strRowId As String = gridNewrow.ClientID
            Dim strGridName As String = gv_Filldata.ClientID
            Dim strFoucsColumnIndex As String = "2"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(gv_Filldata.Rows.Count - 1, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            'If CopyClick = 0 Then
            '    ClearArray()
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try



    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim Exhname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim Suppamount(count) As String
        Dim Minstay(count) As String
        Dim withdrawn(count) As String
        Dim withdrawnamt(count) As String
        Dim Exhicode(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtExhname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtSuppamount As TextBox
        Dim txtMinstay As TextBox
        Dim chkwithdrawn As CheckBox
        Dim chkSelect As CheckBox
        Dim txtExhicode As TextBox
        Dim chkwithdrawnamt As CheckBox

        '   CopyRow = 0
        ClearArray()
        blankrow = 0
        Try

            For Each GVRow In gv_Filldata.Rows

                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                chkSelect = GVRow.FindControl("chkSelect")
                txtExhicode = GVRow.FindControl("txtExhicode")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtSuppamount IsNot Nothing Then
                    Call assignVatValueToTextBox(txtSuppamount, txtTV1, txtNTV1, txtVAT1)
                End If

                If chkSelect.Checked = True Then
                    CopyRowlist.Add(n)
                    CopyRow = n

                    fDate(n) = CType(dpFDate.Text, String)
                    tDate(n) = CType(dpTDate.Text, String)
                    Exhname(n) = CType(txtExhname.Text, String)
                    Roomtype(n) = CType(txtRoomtype.Text, String)
                    Mealplan(n) = CType(txtMealplan.Text, String)
                    Suppamount(n) = CType(txtSuppamount.Text, String)
                    Minstay(n) = CType(txtMinstay.Text, String)
                    Exhicode(n) = CType(txtExhicode.Text, String)
                    If chkwithdrawn.Checked = True Then
                        withdrawn(n) = 1
                    Else
                        withdrawn(n) = 0
                    End If
                    If chkwithdrawnamt.Checked = True Then
                        withdrawnamt(n) = 1
                    Else
                        withdrawnamt(n) = 0
                    End If

                    Exhnamenew.Add(CType(txtExhname.Text, String))
                    Roomtypenew.Add(CType(txtRoomtype.Text, String))
                    Mealplannew.Add(CType(txtMealplan.Text, String))
                    Suppamountnew.Add(CType(txtSuppamount.Text, String))
                    Minstaynew.Add(CType(txtMinstay.Text, String))
                    Exhicodenew.Add(CType(txtExhicode.Text, String))

                    If chkwithdrawn.Checked = True Then
                        withdrawnnew.Add("1")
                    Else
                        withdrawnnew.Add("0")

                    End If
                    If chkwithdrawnamt.Checked = True Then
                        withdrawnamtnew.Add("1")
                    Else
                        withdrawnamtnew.Add("0")

                    End If
                    fDatenew.Add(CType(dpFDate.Text, String))
                    tDatenew.Add(CType(dpTDate.Text, String))
                End If
                If chkSelect.Checked = False And (txtExhname.Text = "") Then
                    blankrow = blankrow + 1
                End If

                n = n + 1

            Next
            Dim k As Integer
            k = blankrow + CopyRowlist.Count
            Do While blankrow < (CopyRowlist.Count)

                btnAddrow_Click(Nothing, Nothing)


                blankrow = blankrow + 1
            Loop




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub

    Private Sub Addlines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim Exhname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim Suppamount(count) As String
        Dim Minstay(count) As String
        Dim withdrawn(count) As String
        Dim withdrawnamt(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtExhname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtSuppamount As TextBox
        Dim txtMinstay As TextBox
        Dim chkwithdrawn As CheckBox
        Dim chkSelect As CheckBox
        Dim chkwithdrawnamt As CheckBox

        '   CopyRow = 0


        Try

            For Each GVRow In gv_Filldata.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                chkSelect = GVRow.FindControl("chkSelect")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")

                If chkSelect.Checked = True Then
                    CopyRow = n
                End If

                fDate(n) = CType(dpFDate.Text, String)
                tDate(n) = CType(dpTDate.Text, String)
                Exhname(n) = CType(txtExhname.Text, String)
                Roomtype(n) = CType(txtRoomtype.Text, String)
                Mealplan(n) = CType(txtMealplan.Text, String)
                Suppamount(n) = CType(txtSuppamount.Text, String)
                Minstay(n) = CType(txtMinstay.Text, String)
                If chkwithdrawn.Checked = True Then
                    withdrawn(n) = 1
                Else
                    withdrawn(n) = 0
                End If
                If chkwithdrawnamt.Checked = True Then
                    withdrawnamt(n) = 1
                Else
                    withdrawnamt(n) = 0
                End If

                Exhnamenew.Add(CType(txtExhname.Text, String))
                Roomtypenew.Add(CType(txtRoomtype.Text, String))
                Mealplannew.Add(CType(txtMealplan.Text, String))
                Suppamountnew.Add(CType(txtSuppamount.Text, String))
                Minstaynew.Add(CType(txtMinstay.Text, String))

                If chkwithdrawn.Checked = True Then
                    withdrawnnew.Add("1")
                Else
                    withdrawnnew.Add("0")

                End If
                If chkwithdrawnamt.Checked = True Then
                    withdrawnamtnew.Add("1")
                Else
                    withdrawnamtnew.Add("0")

                End If
                fDatenew.Add(CType(dpFDate.Text, String))
                tDatenew.Add(CType(dpTDate.Text, String))

                n = n + 1
            Next
            fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In gv_Filldata.Rows
                If n = i Then
                    Exit For
                End If
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")

                dpFDate.Text = fDate(n)
                dpTDate.Text = tDate(n)
                txtExhname.Text = Exhname(n)
                txtRoomtype.Text = Roomtype(n)
                txtMealplan.Text = Mealplan(n)
                txtSuppamount.Text = Suppamount(n)
                txtMinstay.Text = Minstay(n)
                If withdrawn(n) = 1 Then
                    chkwithdrawn.Checked = True
                Else
                    chkwithdrawn.Checked = False
                End If
                If withdrawnamt(n) = 1 Then
                    chkwithdrawnamt.Checked = True
                Else
                    chkwithdrawnamt.Checked = False
                End If

                n = n + 1
            Next
            If CopyClick = 0 Then
                ClearArray()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub

    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex




        Dim exhicode As String = CType(gv_Filldata.Rows(rowid).FindControl("txtExhicode"), TextBox).Text

        Dim parms As New List(Of SqlParameter)
        Dim parm(3) As SqlParameter
        parm(0) = New SqlParameter("@exhicode", CType(exhicode, String))
        parm(1) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
        parm(2) = New SqlParameter("@promotionid", IIf(Session("Calledfrom") = "Offers", CType(hdnpromotionid.Value, String), ""))


        For i = 0 To 2
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_exhidates", parms)
        If Not ds Is Nothing Then
            If ds.Tables.Count > 0 Then
                hdnMainGridRowid.Value = rowid
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtfromDate As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtfromDate")
                    Dim txtToDate As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtToDate")
                    Dim txtsuppamount As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtsuppamount")

                    txtfromDate.Text = ds.Tables(0).Rows(0)("fromdate")
                    txtToDate.Text = ds.Tables(0).Rows(0)("todate")
                    txtfromDate.Focus()
                End If




            End If
        End If

    End Sub





    Private Function ValidateSave() As Boolean
        Dim gvRow As GridViewRow


        Dim lnCnt As Integer = 0

        Dim txtchild As TextBox

        Dim ToDt As Date = Nothing
        Dim flg As Boolean = False

        For Each gvRow In gv_Filldata.Rows
            lnCnt += 1




            Dim txtExhiname As TextBox = gvRow.FindControl("txtExhiname")
            Dim txtrmtypcode As TextBox = gvRow.FindControl("txtrmtypcode")
            Dim txtmealcode As TextBox = gvRow.FindControl("txtmealcode")
            Dim txtfromDate As TextBox = gvRow.FindControl("txtfromDate")
            Dim txtToDate As TextBox = gvRow.FindControl("txtToDate")
            Dim txtsuppamount As TextBox = gvRow.FindControl("txtsuppamount")
            Dim txtMinstay As TextBox = gvRow.FindControl("txtMinstay")

            Dim txtExhicode As TextBox = gvRow.FindControl("txtExhicode")



            If txtExhiname.Text <> "" Then
                flg = True
            End If

            'changed by mohamed on 21/02/2018
            Dim txtTV1 As TextBox = Nothing
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing
            txtTV1 = gvRow.FindControl("txtTV")
            txtNTV1 = gvRow.FindControl("txtNTV")
            txtVAT1 = gvRow.FindControl("txtVAT")

            If txtsuppamount IsNot Nothing Then
                Call assignVatValueToTextBox(txtsuppamount, txtTV1, txtNTV1, txtVAT1)
            End If

            If txtExhiname.Text <> "" Then

                If txtExhicode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Proper Exhibition :" & lnCnt & "');", True)
                    ValidateSave = False
                    Exit Function
                End If
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

                If Val(txtsuppamount.Text) < 1 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Supplement Amount :" & lnCnt & "');", True)
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

                    If Left(Right(txtfromDate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  :" & lnCnt & "');", True)
                        ValidateSave = False
                        SetFocus(txtfromDate)
                        Exit Function
                    End If


                    If Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") < Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Date.Row No :" & lnCnt & "');", True)
                        SetFocus(txtfromDate)
                        ValidateSave = False
                        Exit Function
                    End If
                    If Session("Calledfrom") = "Offers" Then

                        'If Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") < ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value) Then
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & "  ');", True)
                        '    txtfromDate.Text = ""
                        '    SetFocus(txtfromDate)
                        '    ValidateSave = False
                        '    Exit Function
                        'End If

                        If Not (Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") >= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromofrmdate.Value), Date), "yyyy/MM/dd") And Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") <= Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            txtfromDate.Text = ""
                            SetFocus(txtfromDate)
                            ValidateSave = False
                            Exit Function
                        End If

                        If Format(CType(txtfromDate.Text, Date), "yyyy/MM/dd") > ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' From Date Should belongs to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & "  ');", True)
                            txtfromDate.Text = ""
                            SetFocus(txtfromDate)
                            ValidateSave = False
                            Exit Function
                        End If


                        If (Format(CType(txtToDate.Text, Date), "yyyy/MM/dd") > Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(hdnpromotodate.Value), Date), "yyyy/MM/dd")) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' To Date Should belong to the Promotions Period   " & hdnpromofrmdate.Value & " to  " & hdnpromotodate.Value & " ');", True)
                            txtToDate.Text = ""
                            txtfromDate.Text = ""
                            SetFocus(txtfromDate)
                            ValidateSave = False
                            Exit Function
                        End If
                    Else
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



                End If

            End If
        Next
        If flg = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one  Exhibition :" & lnCnt & "');", True)
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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function

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
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub grdviewrates_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdviewrates.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If
            Dim lbltran As Label
            Dim lblcontract As Label, lblpromotionid As Label
            lbltran = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblplistcode")
            lblcontract = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblcontract")
            lblpromotionid = grdviewrates.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblpromotionid")
            If lbltran.Text.Trim = "" Then Exit Sub
            If e.CommandName = "Select" Then

                PanelMain.Visible = True
                ViewState("CopyFrom") = "CopyFrom"
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False

              
                lblstatus.Visible = False
                lblstatustext.Visible = False
                ShowRecordCopy(CType(lbltran.Text.Trim, String))
                ShowRoomdetailsoffers(CType(lbltran.Text.Trim, String))
               



                If Session("Calledfrom") = "Offers" Then
                    wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
                    wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
                    wucCountrygroup.sbShowCountry()


                    btnSave.Visible = True

                    btnSave.Text = "Save"
                    lblHeading.Text = "Copy Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Exhibition Supplements "

                    DisableControl()


                End If



            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractCommission.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btncopycontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopycontract.Click
        Dim myds As New DataSet

        Dim sqlstr As String = ""


        Try


            '   If Session("Calledfrom") = "Offers" Then


            '      strSqlQry = " select '' contractid,oh.tranid plistcode,h.promotionid,max(h.promotionname) promotionname , convert(varchar(10),min(d.fromdate),103) fromdate,convert(varchar(10),max(d.todate),103) todate  " _
            '& "  from view_offers_header h(nolock) join  view_partymaxacc_header oh on h.promotionid =oh.promotionid  join view_offers_detail d(nolock) on h.promotionid =d.promotionid where   h.promotionid<>'" & hdnpromotionid.Value & "' and h.partycode='" + hdnpartycode.Value.Trim + "' group by h.promotionid,oh.tranid  order by convert(varchar(10),min(d.fromdate),111),convert(varchar(10),max(d.todate),111) "


            strSqlQry = " select '' contractid,oh.exhibitionid plistcode,'' promotionid,'' promotionname , oh.applicableto  " _
            & "  from  view_contracts_exhibition_header oh  join view_contracts_search h  on oh.contractid  =h.contractid where isnull(h.withdraw,0)=0  and   h.partycode='" + hdnpartycode.Value.Trim + "'  and isnull(oh.promotionid,'')<>'" & hdnpromotionid.Value & "'  order by oh.exhibitionid "


            ' End If



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
            grdviewrates.Columns(2).Visible = False
            grdviewrates.Columns(4).Visible = False
            grdviewrates.Columns(5).Visible = False

            ModalViewrates.Show()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxOcc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow



        Dim strMsg As String = ""

        FindDatePeriod = True
        Try
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            Dim txtExhname As TextBox
            Dim txtRoomtype As TextBox
            Dim txtMealplan As TextBox
            Dim txtSuppamount As TextBox
            Dim txtMinstay As TextBox
            Dim chkwithdrawn As CheckBox
            Dim chkSelect As CheckBox
            Dim txtExhicode As TextBox

            '   CopyRow = 0

            Session("CountryList") = Nothing
            Session("AgentList") = Nothing

            Session("CountryList") = wucCountrygroup.checkcountrylist
            Session("AgentList") = wucCountrygroup.checkagentlist



            For Each GVRow In gv_Filldata.Rows
                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                chkSelect = GVRow.FindControl("chkSelect")
                txtExhicode = GVRow.FindControl("txtExhicode")


                Dim ds As DataSet
                Dim parms2 As New List(Of SqlParameter)
                Dim parm2(10) As SqlParameter


                If txtExhname.Text <> "" Then
                    parm2(0) = New SqlParameter("@contractid", IIf(Session("Calledfrom") = "Offers", "", CType(hdncontractid.Value, String)))
                    parm2(1) = New SqlParameter("@fromdate", Format(CType(dpFDate.Text, Date), "yyyy/MM/dd"))
                    parm2(2) = New SqlParameter("@todate", Format(CType(dpTDate.Text, Date), "yyyy/MM/dd"))
                    parm2(3) = New SqlParameter("@mode", CType(ViewState("State"), String))
                    parm2(4) = New SqlParameter("@tranid", CType(txttranid.Text.Trim, String))
                    parm2(5) = New SqlParameter("@country", IIf(chkctrygrp.Checked = True, CType(Session("CountryList"), String), ""))
                    parm2(6) = New SqlParameter("@agent", IIf(chkctrygrp.Checked = True, CType(Session("AgentList"), String), ""))
                    parm2(7) = New SqlParameter("@promotionid", IIf(Session("Calledfrom") = "Offers", CType(hdnpromotionid.Value, String), ""))
                    parm2(8) = New SqlParameter("@rmtypcode", CType(txtRoomtype.Text, String))
                    parm2(9) = New SqlParameter("@mealcode", CType(txtMealplan.Text, String))


                    'parms2.Add(parm2(0))
                    'parms2.Add(parm2(1))
                    'parms2.Add(parm2(2))
                    'parms2.Add(parm2(3))
                    'parms2.Add(parm2(4))
                    'parms2.Add(parm2(5))
                    'parms2.Add(parm2(6))
                    'parms2.Add(parm2(7))

                    For i = 0 To 9
                        parms2.Add(parm2(i))
                    Next

                    ds = New DataSet()
                    ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_duplicate_contractexhibition", parms2)


                    If ds.Tables.Count > 0 Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds.Tables(0).Rows(0)("exhibitionid")) = False Then
                                If Session("Calledfrom") = "Offers" Then
                                    strMsg = "Exhibition already exists For this Promotion  " + CType(hdnpromotionid.Value, String) + " -  " + ds.Tables(0).Rows(0)("exhibitionid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                Else
                                    strMsg = "Contract Exhibition already exists For this Contract  " + CType(hdncontractid.Value, String) + " -  " + ds.Tables(0).Rows(0)("exhibitionid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                                End If

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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
#Region "SetDate"

#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        Dim maxocccombination As String
        Dim hdncombination As HiddenField
        Dim sptypecode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" & hdnpartycode.Value & "'")
        Dim strMsg As String = ""

        Try
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


                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_Exhibition", mySqlConn, sqlTrans)
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
                        optionval = objUtils.GetAutoDocNo("CONEXHIBIT", mySqlConn, sqlTrans)
                        txttranid.Text = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_edit_contracts_exhibition_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = IIf(txtApplicableTo.Text = "", "", CType(Replace(txtApplicableTo.Text, ",  ", ","), String))
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)

                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                        Else

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
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
                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_exhibition_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0

                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroupsyesno", SqlDbType.Int)).Value = IIf(chkctrygrp.Checked = True, 1, 0)
                        If Session("Calledfrom") = "Offers" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = DBNull.Value

                        Else

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = ""
                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
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


                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_Events Where  EventList_ID='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_EventRates Where  eventid='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    ' '''''''
                    Dim sqlstr As String = ""
                    Dim txtExhicode As TextBox
                    Dim txtExhiname As TextBox
                    Dim txtfromDate As TextBox
                    Dim txtToDate As TextBox
                    Dim txtrmtypcode As TextBox
                    Dim txtmealcode As TextBox
                    Dim txtsuppamount As TextBox
                    Dim txtMinstay As TextBox
                    Dim chkwithdraw As CheckBox
                    Dim chkwithdrawamt As CheckBox
                    Dim ddleventtype As HtmlSelect
                    Dim arrOcpncy As String()

                    Dim x As Integer = 1
                    Dim kl As Integer
                    Dim ds1 As DataSet
                    Dim ix As Integer
                    Dim Exhicode As String = ""

                    Dim arrcountrygroup As String = ""
                    Dim Agentgroup As String = ""

                    If Session("AgentList") <> "" Then
                        Agentgroup = wucCountrygroup.checkagentlist.ToString.Trim
                    End If

                    If Session("CountryList") <> "" Then

                        'Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")

                        arrcountrygroup = wucCountrygroup.checkcountrylist.ToString.Trim
                        ' For kl = 0 To arrcountry.Length - 1

                    End If

                    If arrcountrygroup <> "" Then 'Or Agentgroup <> "" Then

                        Dim lineno As Integer = 1
                        For Each GVRow In gv_Filldata.Rows

                            txtExhicode = GVRow.FindControl("txtExhicode")
                            txtExhiname = GVRow.FindControl("txtExhiname")
                            txtfromDate = GVRow.FindControl("txtfromDate")
                            txtToDate = GVRow.FindControl("txtToDate")
                            txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                            txtmealcode = GVRow.FindControl("txtmealcode")
                            txtsuppamount = GVRow.FindControl("txtsuppamount")
                            txtMinstay = GVRow.FindControl("txtMinstay")
                            chkwithdraw = GVRow.Findcontrol("chkwithdraw")
                            chkwithdrawamt = GVRow.Findcontrol("chkwithdrawamt")
                            ddleventtype = GVRow.FindControl("ddleventtype")

                            Dim txtTV1 As TextBox = Nothing
                            Dim txtNTV1 As TextBox = Nothing
                            Dim txtVAT1 As TextBox = Nothing
                            txtTV1 = GVRow.FindControl("txtTV")
                            txtNTV1 = GVRow.FindControl("txtNTV")
                            txtVAT1 = GVRow.FindControl("txtVAT")



                            If txtExhicode.Text <> "" Then

                                If Exhicode <> txtExhicode.Text And Exhicode <> "" Then
                                    x = x + 1
                                End If


                                ' rosalin commented promotion line no  2019-11-04

                                ' ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                                ' If ds1.Tables(0).Rows.Count > 0 Then
                                ' For ix = 0 To ds1.Tables(0).Rows.Count - 1

                                ' mySqlCmd = New SqlCommand("sp_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                                mySqlCmd = New SqlCommand("New_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure


                                mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = lineno ' x
                                mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                                mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@minstay", SqlDbType.Int)).Value = CType(Val(txtMinstay.Text), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = CType(IIf(chkwithdrawamt.Checked = True, 1, 0), Integer)
                                mySqlCmd.Parameters.Add(New SqlParameter("@withdrawamt", SqlDbType.Int)).Value = CType(IIf(chkwithdrawamt.Checked = True, 1, 0), Integer)

                                mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)

                                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@eventname", SqlDbType.VarChar, 400)).Value = CType(txtExhiname.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@eventtype", SqlDbType.VarChar, 50)).Value = CType(ddleventtype.Value, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(Agentgroup, String) ' ""
                                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 8000)).Value = CType(arrcountrygroup, String)  'CType(arrcountry(kl), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed 
                                'Next Rosalin


                                'Rosalin 2019-11-04
                                'Else
                                '    'mySqlCmd = New SqlCommand("sp_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                                '    mySqlCmd = New SqlCommand("New_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                                '    mySqlCmd.CommandType = CommandType.StoredProcedure


                                '    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = lineno ' x
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@minstay", SqlDbType.Int)).Value = CType(Val(txtMinstay.Text), Integer)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = CType(IIf(chkwithdraw.Checked = True, 1, 0), Integer)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@withdrawamt", SqlDbType.Int)).Value = CType(IIf(chkwithdrawamt.Checked = True, 1, 0), Integer)

                                '    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)

                                '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@eventname", SqlDbType.VarChar, 400)).Value = CType(txtExhiname.Text.Trim, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@eventtype", SqlDbType.VarChar, 50)).Value = CType(ddleventtype.Value, String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(Agentgroup, String) ' ""
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 8000)).Value = CType(arrcountrygroup, String)  'CType(arrcountry(kl), String)
                                '    mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1

                                '    mySqlCmd.ExecuteNonQuery()
                                '    mySqlCmd.Dispose() 'command disposed
                                'End If  ' rosalin


                                Exhicode = txtExhicode.Text

                            End If

                            lineno = lineno + 1

                        Next



                    End If
                    x = 1
                    Exhicode = ""
                    ' Next
                    ' End If


                    Dim ik As Integer
                    ik = 1
                    For Each GVRow In gv_Filldata.Rows

                        txtExhicode = GVRow.FindControl("txtExhicode")
                        txtExhiname = GVRow.FindControl("txtExhiname")
                        txtfromDate = GVRow.FindControl("txtfromDate")
                        txtToDate = GVRow.FindControl("txtToDate")
                        txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                        txtmealcode = GVRow.FindControl("txtmealcode")
                        txtsuppamount = GVRow.FindControl("txtsuppamount")
                        txtMinstay = GVRow.FindControl("txtMinstay")
                        chkwithdraw = GVRow.Findcontrol("chkwithdraw")
                        chkwithdrawamt = GVRow.Findcontrol("chkwithdrawamt")
                        ddleventtype = GVRow.FindControl("ddleventtype")

                        Dim txtTV1 As TextBox = Nothing
                        Dim txtNTV1 As TextBox = Nothing
                        Dim txtVAT1 As TextBox = Nothing
                        txtTV1 = GVRow.FindControl("txtTV")
                        txtNTV1 = GVRow.FindControl("txtNTV")
                        txtVAT1 = GVRow.FindControl("txtVAT")

                        If txtExhicode.Text <> "" And txtrmtypcode.Text.ToString.Trim <> "" Then

                            If CType(txtrmtypcode.Text, String) = "All" Then
                                Dim rmtypcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + rmtypcode from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                                arrOcpncy = rmtypcode.ToString.Trim.Split(",")
                            Else
                                arrOcpncy = txtrmtypcode.Text.ToString.Trim.Split(",")
                            End If

                            ' arrOcpncy = txtrmtypcode.Text.ToString.Trim.Split(",")
                            For i = 0 To arrOcpncy.Length - 1

                                If arrOcpncy(i) <> "" Then
                                    Dim mealcode As String()

                                    If CType(txtmealcode.Text, String) = "All" Then
                                        Dim mealcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + mealcode from  partymeal(nolock) where   partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                                        mealcode = mealcode1.ToString.Trim.Split(",")
                                    Else
                                        mealcode = txtmealcode.Text.ToString.Trim.Split(",")
                                    End If


                                    ' Dim mealcode As String() = txtmealcode.Text.ToString.Trim.Split(",")
                                    For j = 0 To mealcode.Length - 1

                                        If mealcode(j) <> "" Then



                                            mySqlCmd = New SqlCommand("sp_add_new_edit_eventrates", mySqlConn, sqlTrans)
                                            mySqlCmd.CommandType = CommandType.StoredProcedure

                                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = ik
                                            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 100)).Value = CType(arrOcpncy(i), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = "" 'CType(rmcatcode(k), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(mealcode(j), String)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                                            mySqlCmd.Parameters.Add(New SqlParameter("@optiontype", SqlDbType.VarChar, 25)).Value = CType(ddleventtype.Value, String)


                                            mySqlCmd.ExecuteNonQuery()
                                            mySqlCmd.Dispose() 'command disposed


                                            '    End If

                                            'Next

                                        End If

                                    Next

                                End If


                            Next
                            ik += 1

                        End If


                    Next

                    ' Rosalin

                    'If Session("AgentList") <> "" Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For kl = 0 To arragents.Length - 1

                    '        If arragents(kl) <> "" Then

                    '            For Each GVRow In gv_Filldata.Rows

                    '                txtExhicode = GVRow.FindControl("txtExhicode")
                    '                txtExhiname = GVRow.FindControl("txtExhiname")
                    '                txtfromDate = GVRow.FindControl("txtfromDate")
                    '                txtToDate = GVRow.FindControl("txtToDate")
                    '                txtrmtypcode = GVRow.FindControl("txtrmtypcode")
                    '                txtmealcode = GVRow.FindControl("txtmealcode")
                    '                txtsuppamount = GVRow.FindControl("txtsuppamount")
                    '                txtMinstay = GVRow.FindControl("txtMinstay")
                    '                chkwithdraw = GVRow.Findcontrol("chkwithdraw")
                    '                chkwithdrawamt = GVRow.Findcontrol("chkwithdrawamt")
                    '                ddleventtype = GVRow.FindControl("ddleventtype")

                    '                Dim txtTV1 As TextBox = Nothing
                    '                Dim txtNTV1 As TextBox = Nothing
                    '                Dim txtVAT1 As TextBox = Nothing
                    '                txtTV1 = GVRow.FindControl("txtTV")
                    '                txtNTV1 = GVRow.FindControl("txtNTV")
                    '                txtVAT1 = GVRow.FindControl("txtVAT")

                    '                If txtExhicode.Text <> "" Then

                    '                    ds1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),d.Offer_Start_Date,111) Offer_Start_Date, convert(varchar(10),d.Offer_End_Date,111) Offer_End_Date  from New_edit_OffersCombinations d(nolock) where  d.Promotion_Mas_ID='" & hdnpromotionid.Value & "' and Promotion_Type='Special Rates' ")
                    '                    If ds1.Tables(0).Rows.Count > 0 Then

                    '                        For ix = 0 To ds1.Tables(0).Rows.Count - 1


                    '                            mySqlCmd = New SqlCommand("sp_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                    '                            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = x
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@minstay", SqlDbType.Int)).Value = CType(Val(txtMinstay.Text), Integer)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = CType(IIf(chkwithdraw.Checked = True, 1, 0), Integer)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@withdrawamt", SqlDbType.Int)).Value = CType(IIf(chkwithdrawamt.Checked = True, 1, 0), Integer)

                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)

                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@eventname", SqlDbType.VarChar, 400)).Value = CType(txtExhiname.Text.Trim, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@eventtype", SqlDbType.VarChar, 50)).Value = CType(ddleventtype.Value, String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(kl), String)
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(kl), String) & "'")
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""
                    '                            mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1

                    '                            mySqlCmd.ExecuteNonQuery()
                    '                            mySqlCmd.Dispose() 'command disposed
                    '                        Next
                    '                    Else

                    '                        mySqlCmd = New SqlCommand("sp_mod_edit_contracts_exhibition_detail", mySqlConn, sqlTrans)
                    '                        mySqlCmd.CommandType = CommandType.StoredProcedure


                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = x
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtfromDate.Text)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypcode.Text, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@minstay", SqlDbType.Int)).Value = CType(Val(txtMinstay.Text), Integer)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@withdraw", SqlDbType.Int)).Value = CType(IIf(chkwithdraw.Checked = True, 1, 0), Integer)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@withdrawamt", SqlDbType.Int)).Value = CType(IIf(chkwithdrawamt.Checked = True, 1, 0), Integer)

                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)

                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@eventname", SqlDbType.VarChar, 400)).Value = CType(txtExhiname.Text.Trim, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@eventtype", SqlDbType.VarChar, 50)).Value = CType(ddleventtype.Value, String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 100)).Value = CType(arragents(kl), String)
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast(nolock) where agentcode='" & CType(arragents(kl), String) & "'")
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@Ccode", SqlDbType.VarChar, 100)).Value = ""
                    '                        mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix + 1

                    '                        mySqlCmd.ExecuteNonQuery()
                    '                        mySqlCmd.Dispose() 'command disposed



                    '                    End If
                    '                End If


                    '                'If txtExhicode.Text <> "" And txtrmtypcode.Text.ToString.Trim <> "" Then

                    '                '    If CType(txtrmtypcode.Text, String) = "All" Then
                    '                '        Dim rmtypcode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + rmtypcode from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                    '                '        arrOcpncy = rmtypcode.ToString.Trim.Split(",")
                    '                '    Else
                    '                '        arrOcpncy = txtrmtypcode.Text.ToString.Trim.Split(",")
                    '                '    End If

                    '                '    ' arrOcpncy = txtrmtypcode.Text.ToString.Trim.Split(",")
                    '                '    For i = 0 To arrOcpncy.Length - 1

                    '                '        If arrOcpncy(i) <> "" Then

                    '                '            Dim mealcode As String()

                    '                '            If CType(txtmealcode.Text, String) = "All" Then
                    '                '                Dim mealcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select distinct ',' + mealcode from  partymeal(nolock) where   partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,'' ) ")

                    '                '                mealcode = mealcode1.ToString.Trim.Split(",")
                    '                '            Else
                    '                '                mealcode = txtmealcode.Text.ToString.Trim.Split(",")
                    '                '            End If

                    '                '            ' Dim mealcode As String() = txtmealcode.Text.ToString.Trim.Split(",")
                    '                '            For j = 0 To mealcode.Length - 1

                    '                '                If mealcode(j) <> "" Then

                    '                '                    'Dim rmcatcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + u.rmcatcode from view_partymaxaccomodation  u(nolock) where u.rmtypcode='" & CType(arrOcpncy(i), String) & "' and  u.partycode ='" & CType(hdnpartycode.Value.Trim, String) & "'  for xml path('')),1,1,'' ) ")

                    '                '                    'Dim rmcatcode As String() = rmcatcode1.ToString.Trim.Split(",")

                    '                '                    'For k = 0 To rmcatcode.Length - 1

                    '                '                    '    If rmcatcode(k) <> "" Then

                    '                '                    mySqlCmd = New SqlCommand("sp_add_new_edit_eventrates", mySqlConn, sqlTrans)
                    '                '                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 100)).Value = CType(hdncontractid.Value.Trim, String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 100)).Value = CType(hdnpromotionid.Value.Trim, String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid ", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@elineno", SqlDbType.Int)).Value = x
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitioncode", SqlDbType.VarChar, 20)).Value = CType(txtExhicode.Text.Trim, String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 100)).Value = CType(arrOcpncy(i), String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = "" 'CType(rmcatcode(k), String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(mealcode(j), String)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@supplementvalue", SqlDbType.Decimal)).Value = CType(Val(txtsuppamount.Text), Decimal)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@TaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtTV1.Text), Decimal)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@NonTaxableValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtNTV1.Text), Decimal)
                    '                '                    mySqlCmd.Parameters.Add(New SqlParameter("@VATValue", SqlDbType.Decimal, 20, 3)).Value = CType(Val(txtVAT1.Text), Decimal)
                    '                '                    'mySqlCmd.Parameters.Add(New SqlParameter("@eventname", SqlDbType.VarChar, 400)).Value = CType(txtExhiname.Text.Trim, String)
                    '                '                    'mySqlCmd.Parameters.Add(New SqlParameter("@eventtype", SqlDbType.VarChar, 50)).Value = CType(ddleventtype.Value, String)

                    '                '                    mySqlCmd.ExecuteNonQuery()
                    '                '                    mySqlCmd.Dispose() 'command disposed


                    '                '                    '    End If

                    '                '                    'Next

                    '                '                End If

                    '                '            Next

                    '                '        End If


                    '                '    Next

                    '                'End If





                    '                x = x + 1

                    '            Next

                    '        End If
                    '    Next


                    'End If
                    ' Rosalin











                    '''  User cotrol country saving
                    ''' 

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_exhibition_countries Where exhibitionid='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'mySqlCmd = New SqlCommand("DELETE FROM edit_contracts_exhibition_agents Where exhibitionid='" & CType(txttranid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'If wucCountrygroup.checkcountrylist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                    '    For i = 0 To arrcountry.Length - 1

                    '        If arrcountry(i) <> "" Then




                    '            mySqlCmd = New SqlCommand("sp_add_contracts_exhibition_countries", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@countrycode", SqlDbType.VarChar, 20)).Value = CType(arrcountry(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    'If wucCountrygroup.checkagentlist.ToString <> "" And chkctrygrp.Checked = True Then

                    '    ''Value in hdn variable , so splting to get string correctly
                    '    Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                    '    For i = 0 To arragents.Length - 1

                    '        If arragents(i) <> "" Then




                    '            mySqlCmd = New SqlCommand("sp_add_contracts_exhibition_agents", mySqlConn, sqlTrans)
                    '            mySqlCmd.CommandType = CommandType.StoredProcedure


                    '            mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(arragents(i), String)

                    '            mySqlCmd.ExecuteNonQuery()
                    '            mySqlCmd.Dispose() 'command disposed
                    '        End If
                    '    Next

                    'End If

                    '''' Insert into Minimum nights table 

                    'If Session("Calledfrom") <> "Offers" Then

                    '    mySqlCmd = New SqlCommand("sp_add_minimumnightstable", mySqlConn, sqlTrans)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure

                    '    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)

                    '    mySqlCmd.ExecuteNonQuery()
                    '    mySqlCmd.Dispose() 'command disposed
                    'End If


                    'mySqlCmd = New SqlCommand("sp_add_editpendforapprove", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 30)).Value = "edit_contracts_exhibition_header"
                    'mySqlCmd.Parameters.Add(New SqlParameter("@markets", SqlDbType.VarChar, 50)).Value = ""
                    'mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 30)).Value = CType(txttranid.Text.Trim, String)

                    'mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = txthotelname.Text
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 100)).Value = hdnpartycode.Value
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@moddate ", SqlDbType.DateTime)).Value = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "yyyy/MM/dd")
                    'mySqlCmd.Parameters.Add(New SqlParameter("@pricecode", SqlDbType.VarChar, 100)).Value = ""
                    'mySqlCmd.ExecuteNonQuery()



                    strMsg = "Saved Succesfully!!"

                ElseIf ViewState("State") = "Delete" Then

                    ''------------------------for delete contracts_exhibition_header
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    '''' Insert Main tables entry to Edit Table
                    'mySqlCmd = New SqlCommand("sp_insertrecords_edit_contracts_Exhibition", mySqlConn, sqlTrans)
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
                    mySqlCmd = New SqlCommand("sp_del_contracts_exhibition_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exhibitionid", SqlDbType.VarChar, 20)).Value = CType(txttranid.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Succesfully!!"
                End If



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                ' Divmain.Style("display") = "none"

                ViewState("State") = ""
                btnreset1_Click(sender, e)


                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & " ', please click return to search and refresh the search page );", True)
                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('MaxaccWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region






    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Private Sub getShowroomtypes()

        Try

            Dim MyDs As New DataTable
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            strSqlQry = "select rmtypcode,rmtypname from  partyrmtyp where  inactive=0 and partycode='" & hdnpartycode.Value & "'"

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

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then SqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Private Function getroomtypes() As String

        Dim roomtypes As String = ""
        Try

            Dim chk2 As CheckBox
            Dim txtrmtypcode As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode.Text + ";"

                End If
            Next

            If roomtypes.Length > 0 Then
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
            End If
            Return roomtypes
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("rptpricelistSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return "" 'mealplan
        End Try

    End Function
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            ' Dim txtbox As TextBox
            Dim roomtypes As String

            '   roomtypes = getroomtypes()

            Dim chkSelect As CheckBox
            Dim intAdult, intChild As Integer
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

            'If tickedornot = False Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Row');", True)
            '    ModalExtraPopup.Show()
            '    Exit Sub
            'End If



            Dim chk2 As CheckBox
            Dim txtrmtypcode1 As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode1 = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode1.Text + ","

                End If
            Next

            If roomtypes Is Nothing Then
                roomtypes = "All"

            Else
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
            End If

            If gv_Showroomtypes.HeaderRow.Cells(3).Text = "Meal Plan" Then
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtmealcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                    If roomtypes.ToString = "" And tickedornot = False Then
                        txtmealcode.Text = "All"
                    Else
                        txtmealcode.Text = roomtypes.ToString
                    End If

                End If
            Else
                If hdnMainGridRowid.Value <> "" Then
                    Dim txtrmtypcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                    If roomtypes.ToString = "" And tickedornot = False Then
                        txtrmtypcode.Text = "All"
                    Else
                        txtrmtypcode.Text = roomtypes.ToString
                    End If

                End If
            End If




            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    'Protected Sub imgbEditnew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim clickedRow As GridViewRow = TryCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)

    '    Dim txtrmtypcode As TextBox = DirectCast(clickedRow.FindControl("txtrmtypcode"), TextBox)






    'End Sub


#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Sub gv_FillData_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Filldata.RowDataBound
        Dim fromdate As TextBox
        Dim todate As TextBox
        Dim gvrow As GridViewRow
        Dim txtsuppamount As TextBox
        Dim txtminstay As TextBox
        Dim txtexhicode As TextBox
        Dim txtexhiname As TextBox
        gvrow = e.Row
        If e.Row.RowIndex = -1 Then

            Exit Sub
        End If
        fromdate = CType(e.Row.FindControl("txtfromdate"), TextBox)
        todate = CType(e.Row.FindControl("txttodate"), TextBox)
        txtexhicode = CType(e.Row.FindControl("txtExhicode"), TextBox)
        txtexhiname = CType(e.Row.FindControl("txtExhiname"), TextBox)

        txtsuppamount = CType(e.Row.FindControl("txtsuppamount"), TextBox)
        txtminstay = CType(e.Row.FindControl("txtMinstay"), TextBox)

        txtminstay.Text = "1"
        txtsuppamount.Attributes.Add("onkeypress", "return  checkNumber(event)")
        txtminstay.Attributes.Add("onkeypress", "return  checkNumber(event)")
        fromdate.Attributes.Add("onchange", "javascript:ChangeDate('" + CType(fromdate.ClientID, String) + "','" + CType(todate.ClientID, String) + "')")

        If Session("Calledfrom") = "Offers" Then
            fromdate.Attributes.Add("onchange", "setdate();")
            fromdate.Attributes.Add("onchange", "checkfromdatespromo('" & txtexhiname.ClientID & "','" & fromdate.ClientID & "','" & todate.ClientID & "');")
            todate.Attributes.Add("onchange", "checkdatespromo('" & fromdate.ClientID & "','" & todate.ClientID & "');")
        Else
            fromdate.Attributes.Add("onchange", "setdate();")
            fromdate.Attributes.Add("onchange", "checkfromdates('" & txtexhiname.ClientID & "','" & fromdate.ClientID & "','" & todate.ClientID & "');")
            todate.Attributes.Add("onchange", "checkdates('" & fromdate.ClientID & "','" & todate.ClientID & "');")
        End If

        '------------------------------------------------
        'changed by mohamed on 21/02/2018
        Dim txt1 As TextBox = e.Row.FindControl("txtsuppamount")

        Dim txtTV1 As TextBox = Nothing
        Dim txtNTV1 As TextBox = Nothing
        Dim txtVAT1 As TextBox = Nothing
        txtTV1 = e.Row.FindControl("txtTV")
        txtNTV1 = e.Row.FindControl("txtNTV")
        txtVAT1 = e.Row.FindControl("txtVAT")
        txt1.Attributes.Add("onchange", "javascript:calculateVAT('" & txt1.ClientID & "','" & txtTV1.ClientID & "','" & txtNTV1.ClientID & "','" & txtVAT1.ClientID & "');")
        '------------------------------------------------

        ''' Commented shahul 21/01/17 becoz its not working
        'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
        '    Dim strGridName As String = gv_FillData.ClientID
        '    Dim strRowId As String = e.Row.RowIndex
        '    Dim strFoucsColumnIndex = "2"
        '    e.Row.Attributes("onclick") = "javascript:SelectRow(this,'" + strRowId + "','" + strGridName + "','" + strFoucsColumnIndex + "');"
        '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event,'" + strGridName + "','" + strFoucsColumnIndex + "',this);"
        'End If

    End Sub

    Protected Sub btnrmtyp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        'btnmealok.Style("display") = "none"
        'btnOk1.Style("display") = "block"

        Dim strrmtypename As String = CType(gv_Filldata.Rows(rowid).FindControl("txtrmtypcode"), TextBox).Text


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

        Dim strmealname As String = CType(gv_Filldata.Rows(rowid).FindControl("txtmealcode"), TextBox).Text


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



    Private Sub ChkExistingMeal(ByVal prm_strChkmeals As String)
        'Dim chkSelect As CheckBox
        'Dim txtmealcode As Label


        'Dim arrString As String()


        'If prm_strChkmeals <> "" Then
        '    arrString = prm_strChkmeals.Split(",") 'spliting for ';' 1st

        '    For k = 0 To arrString.Length - 1
        '        'If arrString(k) <> "" Then

        '        '  Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

        '        For Each grdRow In gv_Showmeals.Rows
        '            chkSelect = CType(grdRow.FindControl("chkmeal"), CheckBox)
        '            txtmealcode = CType(grdRow.FindControl("txtmealcode"), Label)
        '            If arrString(k) = txtmealcode.Text Then
        '                chkSelect.Checked = True

        '            End If

        '        Next
        '        'End If
        '    Next
        '    'Else
        '    '    'first case when not selected , chk all
        '    '    For Each grdRow In gvOccupancy.Rows
        '    '        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
        '    '        chkSelect.Checked = True

        '    '    Next


        'End If
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








    Private Sub DisableControl()
        If ViewState("State") = "New" Or ViewState("State") = "Copy" Then

            gv_Filldata.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True


            wucCountrygroup.Disable(True)
            txtApplicableTo.Enabled = True
            txttranid.Text = ""
            btnclearamt.Enabled = True
            btncopyrow.Enabled = True
        ElseIf ViewState("State") = "View" Or ViewState("State") = "Delete" Then


            ' gv_DisableControl()


            gv_Filldata.Enabled = False
            btnAddrow.Enabled = False
            btndeleterow.Enabled = False
            btncopyrow.Enabled = False

            wucCountrygroup.Disable(False)
            txtApplicableTo.Enabled = False
            btnclearamt.Enabled = False


        ElseIf ViewState("State") = "Edit" Or ViewState("State") = "Copy" Then


            gv_Filldata.Enabled = True
            btnAddrow.Enabled = True
            btndeleterow.Enabled = True

            btnclearamt.Enabled = True
            wucCountrygroup.Disable(True)
            txtApplicableTo.Enabled = True
            btncopyrow.Enabled = True
        End If
    End Sub
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "moreless" Then
                Exit Sub
            End If

            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub

            Dim lbltran As Label
            Session("Maxid") = Nothing
            lbltran = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbltran")

            'If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
            '    lblpromotion.Style.Add("display", "none")
            '    txtpromotionid.Style.Add("display", "none")
            '    txtpromotionname.Style.Add("display", "none")


            'Else
            '    lblpromotion.Style.Add("display", "block")
            '    txtpromotionid.Style.Add("display", "block")
            '    txtpromotionname.Style.Add("display", "block")

            'End If
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
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()



                DisableControl()

                btnSave.Visible = True
                btnSave.Text = "Update"
                lblHeading.Text = "Edit Exhibition Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Contract Exhibition Supplements "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Edit Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Exhibition Supplements "
                End If
                gv_Filldata.Columns(9).Visible = False

            ElseIf e.CommandName = "View" Then
                ViewState("State") = "View"
                PanelMain.Visible = True
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                'ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = False
                lblHeading.Text = "View Exhibition Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Contract Exhibition Supplements "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "View Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Exhibition Supplements "
                End If
                gv_Filldata.Columns(9).Visible = False
            ElseIf e.CommandName = "DeleteRow" Then
                PanelMain.Visible = True
                ViewState("State") = "Delete"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                ' ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                btnSave.Text = "Delete"
                lblHeading.Text = "Delete Exhibition Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Contract Exhibition Supplements "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Delete Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Exhibition Supplements "
                End If
                gv_Filldata.Columns(9).Visible = False

            ElseIf e.CommandName = "Copy" Then
                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lbltran.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lbltran.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lbltran.Text.Trim, String))
                ShowRoomdetails(CType(lbltran.Text.Trim, String))
                ' ShowRoomdetails(hdnpartycode.Value, CType(lbltran.Text.Trim, String))

                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txttranid.Text = ""
                btnSave.Text = "Save"
                lblHeading.Text = "Copy Exhibition Supplements - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = "Contract Exhibition Supplements "

                If Session("Calledfrom") = "Offers" Then

                    lblHeading.Text = "Copy Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Exhibition Supplements "
                End If
                gv_Filldata.Columns(9).Visible = False
            End If



        Catch ex As Exception
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub ShowRoomdetailsoffers(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_Filldata.Visible = True
            strSqlQry = ""

            Dim strQry As String
            Dim cnt As Integer = 0


            Dim roomtypes As String = ""
            Dim mealplans As String = ""

            Dim contractid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(contractid,'')contractid from contracts_exhibition_header  where exhibitionid='" & CType(RefCode, String) & "'")
            Dim promoid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(promotion,'')contractid from contracts_exhibition_header  where exhibitionid='" & CType(RefCode, String) & "'")

            If Session("Calledfrom") = "Offers" Then
                strQry = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

                strQry = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
                mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
            End If


            If Session("Calledfrom") = "Offers" Then
                If contractid = "" Then
                    strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '','" & CType(promoid, String) & "' ,0"
                Else
                    strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '" & CType(contractid, String) & "','' ,0"
                End If


            Else
                If contractid = "" Then
                    strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '','" & CType(promoid, String) & "' ,0"
                Else
                    strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '" & CType(contractid, String) & "','' ,0"
                End If

                ' strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '" & CType(hdncontractid.Value, String) & "','',0"
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            ' gv_Filldata.DataSource = myDS

            cnt = myDS.Tables(0).Rows.Count


            'strQry = "select count( distinct elineno) from view_contracts_exhibition_detail where exhibitionid='" & RefCode & "'"

            'cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            fillDategrd(gv_Filldata, False, cnt)

            If cnt > 0 Then
                'strSqlQry = "select d.exhibitioncode,h.exhibitionname,d.fromdate,d.todate,d.roomtypes,d.mealplans,d.supplementvalue,d.minstay,d.withdraw,d.withdrawamt from view_contracts_exhibition_detail d,exhibition_master h where d.exhibitioncode=h.exhibitioncode  and  d.exhibitionid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                myCommand.CommandTimeout = 0
                mySqlReader = myCommand.ExecuteReader




                Dim txtExhicode As TextBox
                Dim txtfromDate As TextBox
                Dim txtToDate As TextBox
                Dim txtrmtypcode As TextBox
                Dim txtmealcode As TextBox
                Dim txtsuppamount As TextBox
                Dim txtMinstay As TextBox
                Dim chkwithdraw As CheckBox
                Dim txtExhiname As TextBox
                Dim lblExhicode As Label
                Dim chkwithdrawamt As CheckBox
                For Each GvRow In gv_Filldata.Rows

                    txtExhicode = GvRow.FindControl("txtExhicode")
                    txtExhiname = GvRow.FindControl("txtExhiname")
                    txtfromDate = GvRow.FindControl("txtfromDate")
                    txtToDate = GvRow.FindControl("txtToDate")
                    txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                    txtmealcode = GvRow.FindControl("txtmealcode")
                    txtsuppamount = GvRow.FindControl("txtsuppamount")
                    txtMinstay = GvRow.FindControl("txtMinstay")
                    chkwithdraw = GvRow.Findcontrol("chkwithdraw")
                    lblExhicode = GvRow.findcontrol("lblExhicode")
                    chkwithdrawamt = GvRow.Findcontrol("chkwithdrawamt")

                    If mySqlReader.Read = True Then

                        If IsDBNull(mySqlReader("exhibitioncode")) = False Then
                            txtExhicode.Text = mySqlReader("exhibitioncode")
                            txtExhiname.Text = mySqlReader("exhibitionname")
                            lblExhicode.Text = mySqlReader("exhibitioncode")

                        End If

                        'If IsDBNull(mySqlReader("roomtypes")) = False Then
                        txtrmtypcode.Text = roomtypes

                        'End If
                        'If IsDBNull(mySqlReader("mealplans")) = False Then
                        txtmealcode.Text = mealplans

                        'End If
                        If IsDBNull(mySqlReader("supplementvalue")) = False Then
                            txtsuppamount.Text = DecRound(mySqlReader("supplementvalue"))

                        End If
                        If IsDBNull(mySqlReader("minstay")) = False Then
                            txtMinstay.Text = mySqlReader("minstay")

                        End If

                        If IsDBNull(mySqlReader("withdraw")) = False Then

                            If mySqlReader("withdraw") = 1 Then

                                chkwithdraw.Checked = True

                            Else

                                chkwithdraw.Checked = False

                            End If
                        End If

                        If IsDBNull(mySqlReader("withdrawamt")) = False Then
                            If mySqlReader("withdrawamt") = 1 Then
                                chkwithdrawamt.Checked = True
                            Else
                                chkwithdrawamt.Checked = False
                            End If
                        End If


                        If IsDBNull(mySqlReader("fromdate")) = False Then


                            txtfromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then


                            txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                        End If

                        'changed by mohamed on 21/02/2018
                        Dim txtTV1 As TextBox = Nothing
                        Dim txtNTV1 As TextBox = Nothing
                        Dim txtVAT1 As TextBox = Nothing
                        txtTV1 = GvRow.FindControl("txtTV")
                        txtNTV1 = GvRow.FindControl("txtNTV")
                        txtVAT1 = GvRow.FindControl("txtVAT")
                        If txtsuppamount IsNot Nothing Then
                            Call assignVatValueToTextBox(txtsuppamount, txtTV1, txtNTV1, txtVAT1)
                        End If

                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub ShowRoomdetails(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_Filldata.Visible = True
            strSqlQry = ""



            Dim strQry As String
            Dim cnt As Integer = 0

            If Session("Calledfrom") = "Offers" Then
                strSqlQry = "sp_show_exhibition_details'" & CType(RefCode, String) & "' , '','" & CType(hdnpromotionid.Value, String) & "' ,0"
            Else
                strSqlQry = "sp_show_exhibition_details '" & CType(RefCode, String) & "' , '" & CType(hdncontractid.Value, String) & "','',0"
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)

            ' gv_Filldata.DataSource = myDS

            cnt = myDS.Tables(0).Rows.Count

            'strQry = "select count( distinct elineno) from view_contracts_exhibition_detail(nolock) where exhibitionid='" & RefCode & "'"
            'cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)

            fillDategrd(gv_Filldata, False, cnt)

            If cnt > 0 Then
                '  strSqlQry = "select  * from  (select distinct d.exhibitioncode,h.exhibitionname,d.fromdate,d.todate,d.roomtypes,d.mealplans,d.supplementvalue,d.minstay,d.withdraw,d.withdrawamt,d.line_no,d.eventtype from view_contracts_exhibition_detail d(nolock),exhibition_master h where d.exhibitioncode=h.exhibitioncode  and  d.exhibitionid='" & RefCode & "') rs order by rs.line_no"


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                myCommand.CommandTimeout = 0
                mySqlReader = myCommand.ExecuteReader


                Dim txtExhicode As TextBox
                Dim txtfromDate As TextBox
                Dim txtToDate As TextBox
                Dim txtrmtypcode As TextBox
                Dim txtmealcode As TextBox
                Dim txtsuppamount As TextBox
                Dim txtMinstay As TextBox
                Dim chkwithdraw As CheckBox
                Dim txtExhiname As TextBox
                Dim lblExhicode As Label
                Dim chkwithdrawamt As CheckBox
                Dim ddleventtype As HtmlSelect

                For Each GvRow In gv_Filldata.Rows

                    txtExhicode = GvRow.FindControl("txtExhicode")
                    txtExhiname = GvRow.FindControl("txtExhiname")
                    txtfromDate = GvRow.FindControl("txtfromDate")
                    txtToDate = GvRow.FindControl("txtToDate")
                    txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                    txtmealcode = GvRow.FindControl("txtmealcode")
                    txtsuppamount = GvRow.FindControl("txtsuppamount")
                    txtMinstay = GvRow.FindControl("txtMinstay")
                    chkwithdraw = GvRow.Findcontrol("chkwithdraw")
                    lblExhicode = GvRow.findcontrol("lblExhicode")
                    chkwithdrawamt = GvRow.Findcontrol("chkwithdrawamt")
                    ddleventtype = GvRow.FindControl("ddleventtype")

                    If mySqlReader.Read = True Then

                        If IsDBNull(mySqlReader("exhibitioncode")) = False Then
                            txtExhicode.Text = mySqlReader("exhibitioncode")
                            txtExhiname.Text = mySqlReader("exhibitionname")
                            lblExhicode.Text = mySqlReader("exhibitioncode")

                        End If

                        If IsDBNull(mySqlReader("roomtypes")) = False Then
                            txtrmtypcode.Text = mySqlReader("roomtypes")

                        End If
                        If IsDBNull(mySqlReader("mealplans")) = False Then
                            txtmealcode.Text = mySqlReader("mealplans")

                        End If

                        If IsDBNull(mySqlReader("eventtype")) = False Then
                            ddleventtype.Value = mySqlReader("eventtype")

                        End If

                        If IsDBNull(mySqlReader("supplementvalue")) = False Then
                            txtsuppamount.Text = DecRound(mySqlReader("supplementvalue"))

                        End If
                        If IsDBNull(mySqlReader("minstay")) = False Then
                            txtMinstay.Text = mySqlReader("minstay")

                        End If

                        If IsDBNull(mySqlReader("withdraw")) = False Then

                            If mySqlReader("withdraw") = 1 Then

                                chkwithdraw.Checked = True

                            Else

                                chkwithdraw.Checked = False

                            End If
                        End If

                        If IsDBNull(mySqlReader("withdrawamt")) = False Then
                            If mySqlReader("withdrawamt") = 1 Then
                                chkwithdrawamt.Checked = True
                            Else
                                chkwithdrawamt.Checked = False
                            End If
                        End If


                        If IsDBNull(mySqlReader("fromdate")) = False Then


                            txtfromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then


                            txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                        End If

                        'changed by mohamed on 21/02/2018
                        Dim txtTV1 As TextBox = Nothing
                        Dim txtNTV1 As TextBox = Nothing
                        Dim txtVAT1 As TextBox = Nothing
                        txtTV1 = GvRow.FindControl("txtTV")
                        txtNTV1 = GvRow.FindControl("txtNTV")
                        txtVAT1 = GvRow.FindControl("txtVAT")
                        If txtsuppamount IsNot Nothing Then
                            Call assignVatValueToTextBox(txtsuppamount, txtTV1, txtNTV1, txtVAT1)
                        End If
                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub
    Protected Sub btndeleterow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleterow.Click
        'Dim dtGridVal As New DataTable
        'Dim chkselect As New CheckBox



        'dtGridVal.Rows.Clear()

        'For Each grdRow In gv_FillData.Rows
        '    chkselect = CType(grdRow.FindControl("chkSelect"), CheckBox)
        '    If chkSelect.Checked = False Then
        '        dtGridVal.Rows.Add(grdRow.Cells(1).Text, grdRow.Cells(2).Text)
        '    End If
        'Next

        'gv_FillData.DataSource = dtGridVal
        'gv_FillData.DataBind()

        '  Createdatacolumns("delete")


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = gv_Filldata.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim Exhname(count) As String
        Dim Roomtype(count) As String
        Dim Mealplan(count) As String
        Dim Suppamount(count) As String
        Dim Minstay(count) As String
        Dim withdrawn(count) As String
        Dim withdrawnamt(count) As String
        Dim exhicode(count) As String
        Dim eventtype(count) As String
        Dim n As Integer = 0
        Dim dpFDate As TextBox
        Dim dpTDate As TextBox
        Dim txtExhname As TextBox
        Dim txtRoomtype As TextBox
        Dim txtMealplan As TextBox
        Dim txtSuppamount As TextBox
        Dim txtMinstay As TextBox
        Dim chkwithdrawn As CheckBox
        Dim chkwithdrawnamt As CheckBox
        Dim chkSelect As CheckBox
        Dim txtexhicode As TextBox
        Dim ddleventtype As HtmlSelect
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In gv_Filldata.Rows
                chkSelect = GVRow.FindControl("chkSelect")
                If chkSelect.Checked = False Then

                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    txtExhname = GVRow.FindControl("txtExhiname")
                    txtRoomtype = GVRow.FindControl("txtrmtypcode")
                    txtMealplan = GVRow.FindControl("txtmealcode")
                    txtSuppamount = GVRow.FindControl("txtsuppamount")
                    txtMinstay = GVRow.FindControl("txtMinstay")
                    chkwithdrawn = GVRow.FindControl("chkwithdraw")
                    txtexhicode = GVRow.FindControl("txtExhicode")
                    chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")
                    ddleventtype = GVRow.FindControl("ddleventtype")

                    fDate(n) = CType(dpFDate.Text, String)
                    tDate(n) = CType(dpTDate.Text, String)
                    Exhname(n) = CType(txtExhname.Text, String)
                    Roomtype(n) = CType(txtRoomtype.Text, String)
                    Mealplan(n) = CType(txtMealplan.Text, String)
                    Suppamount(n) = CType(txtSuppamount.Text, String)
                    Minstay(n) = CType(txtMinstay.Text, String)
                    exhicode(n) = CType(txtexhicode.Text, String)
                    eventtype(n) = CType(ddleventtype.Value, String)

                    If chkwithdrawn.Checked = True Then
                        withdrawn(n) = 1
                    Else
                        withdrawn(n) = 0
                    End If
                    If chkwithdrawnamt.Checked = True Then
                        withdrawnamt(n) = 1
                    Else
                        withdrawnamt(n) = 0
                    End If
                    n = n + 1

                Else
                    deletedrow = deletedrow + 1
                    'delrows(k) = GVRow.RowIndex
                    ' k = k + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(gv_Filldata, False, count)
            'If gv_Filldata.Rows.Count > 1 Then
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count - deletedrow)
            'Else
            '    fillDategrd(gv_Filldata, False, gv_Filldata.Rows.Count)
            'End If


            Dim i As Integer = n
            n = 0

            For Each GVRow In gv_Filldata.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then


                    dpFDate = GVRow.FindControl("txtfromDate")
                    dpTDate = GVRow.FindControl("txtToDate")
                    txtExhname = GVRow.FindControl("txtExhiname")
                    txtRoomtype = GVRow.FindControl("txtrmtypcode")
                    txtMealplan = GVRow.FindControl("txtmealcode")
                    txtSuppamount = GVRow.FindControl("txtsuppamount")
                    txtMinstay = GVRow.FindControl("txtMinstay")
                    chkwithdrawn = GVRow.FindControl("chkwithdraw")
                    txtexhicode = GVRow.FindControl("txtExhicode")
                    chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")
                    ddleventtype = GVRow.FindControl("ddleventtype")

                    dpFDate.Text = fDate(n)
                    dpTDate.Text = tDate(n)
                    txtExhname.Text = Exhname(n)
                    txtRoomtype.Text = Roomtype(n)
                    txtMealplan.Text = Mealplan(n)
                    txtSuppamount.Text = Suppamount(n)
                    txtMinstay.Text = Minstay(n)
                    txtexhicode.Text = exhicode(n)
                    ddleventtype.Value = eventtype(n)


                    If withdrawn(n) = 1 Then
                        chkwithdrawn.Checked = True
                    Else
                        chkwithdrawn.Checked = False
                    End If
                    If withdrawnamt(n) = 1 Then
                        chkwithdrawnamt.Checked = True
                    Else
                        chkwithdrawnamt.Checked = False
                    End If

                    'changed by mohamed on 21/02/2018
                    Dim txtTV1 As TextBox = Nothing
                    Dim txtNTV1 As TextBox = Nothing
                    Dim txtVAT1 As TextBox = Nothing
                    txtTV1 = GVRow.FindControl("txtTV")
                    txtNTV1 = GVRow.FindControl("txtNTV")
                    txtVAT1 = GVRow.FindControl("txtVAT")

                    If txtSuppamount IsNot Nothing Then
                        Call assignVatValueToTextBox(txtSuppamount, txtTV1, txtNTV1, txtVAT1)
                    End If
                    n = n + 1
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Private Sub fillheader()
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select c.applicableto,a.seasonname,c.fromdate,c.todate from view_contracts_search c(nolock),view_contractseasons a Where c.contractid=a.contractid and c.contractid='" & hdncontractid.Value & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                        txtApplicableTo.Text = CType(Replace(txtApplicableTo.Text, ",      ", ","), String)


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
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


    End Sub
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        ViewState("State") = "New"
        Session("RefCode") = Nothing

        PanelMain.Visible = True
        PanelMain.Style("display") = "block"
        Panelsearch.Enabled = False
        lblstatus.Visible = False
        lblstatustext.Visible = False

        If Session("Calledfrom") = "Offers" Then

            divoffer.Style.Add("display", "block")



            txtpromotionidnew.Text = hdnpromotionid.Value
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





            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))
            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")

            wucCountrygroup.sbSetPageState(hdnpromotionid.Value, Nothing, "Edit")
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()

            fillDategrd(gv_Filldata, True)
            '  fillDategrd(gv_Filldata, False, 1)
            'fillheader()
            gv_Filldata.Enabled = True
            btnSave.Visible = True
            DisableControl()
            btnSave.Text = "Save"
            lblHeading.Text = "New Exhibition Supplements - " + ViewState("hotelname") + "-" + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "New Exhibition Supplements -" + ViewState("hotelname") + "-" + hdnpromotionid.Value


            chkctrygrp.Checked = False
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If

            Dim roomtypes As String = ""
            Dim mealplans As String = ""
            Dim strQry As String = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)

            strQry = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)



            For Each GvRow In gv_Filldata.Rows

                Dim txtrmtypcode As TextBox = GvRow.FindControl("txtrmtypcode")
                Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")

                txtrmtypcode.Text = roomtypes
                txtmealcode.Text = mealplans

            Next

            gv_Filldata.Columns(9).Visible = False


        Else
            wucCountrygroup.sbSetPageState("", "CONTRACTMAIN", CType(Session("ContractState"), String))
            wucCountrygroup.sbSetPageState(hdncontractid.Value, Nothing, "Edit")
            'wucCountrygroup.sbSetPageState("", Nothing, ViewState("State"))

            fillDategrd(gv_Filldata, True)
            fillheader()
            gv_Filldata.Enabled = True
            btnSave.Visible = True
            DisableControl()
            btnSave.Text = "Save"
            lblHeading.Text = "New Exhibition Supplements - " + ViewState("hotelname")
            Page.Title = Page.Title + " " + "New Exhibition Supplements -" + ViewState("hotelname")


            chkctrygrp.Checked = False
            If chkctrygrp.Checked = True Then
                divuser.Style.Add("display", "block")
            Else
                divuser.Style.Add("display", "none")
            End If
            Session("isAutoTick_wuccountrygroupusercontrol") = 1
            wucCountrygroup.sbShowCountry()
        End If

        'If ViewState("Menucalling") = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1138") Then
        '    lblpromotion.Style.Add("display", "none")
        '    txtpromotionid.Style.Add("display", "none")
        '    txtpromotionname.Style.Add("display", "none")


        'Else
        '    lblpromotion.Style.Add("display", "block")
        '    txtpromotionid.Style.Add("display", "block")
        '    txtpromotionname.Style.Add("display", "block")

        'End If

        chkVATCalculationRequired.Checked = True
        Call sbFillTaxDetail() 'changed by mohamed on 21/02/2018
        gv_Filldata.Columns(9).Visible = False

    End Sub

    Protected Sub btnreset1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnreset1.Click
        '   Divmain.Style("display") = "none"
        Panelsearch.Enabled = True


        'gv_Filldata.Visible = False


        Session("RefCode") = Nothing
        Panelsearch.Enabled = True

        PanelMain.Style("display") = "none"
        Panelsearch.Style("display") = "block"
       
        wucCountrygroup.clearsessions()
        wucCountrygroup.sbSetPageState("", "CONTRACTEXHIBITION", CType(Session("ContractState"), String))
        ddlorder.SelectedIndex = 0
        ddlorderby.SelectedIndex = 1

        If Session("Calledfrom") = "Offers" Then
            lblHeading.Text = "Exhibition Supplements  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value
            Page.Title = "Promotion Exhibition Supplements "
            FillGrid("exhibitionid", hdnpromotionid.Value, "Desc")
        Else
            lblHeading.Text = "Exhibition Supplements  -" + ViewState("hotelname") + " - " + hdncontractid.Value
            Page.Title = "Contract Exhibition Supplements "
            FillGrid("exhibitionid", hdncontractid.Value, "Desc")

        End If


    End Sub


    'Protected Sub chkctrygrp_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkctrygrp.CheckedChanged
    '    'If chkctrygrp.Checked = True Then
    '    '    wucCountrygroup.Visible = True
    '    '    divuser.Style("display") = "block"
    '    '    dlList.Visible = True
    '    'Else
    '    '    wucCountrygroup.Visible = False

    '    '    divuser.Style("display") = "none"
    '    '    dlList.Visible = False

    '    'End If
    'End Sub



    Protected Sub btncopyrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrow.Click

        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = gv_Filldata.Rows.Count '+ 1


            Dim n As Integer = 0
            Dim dpFDate As TextBox
            Dim dpTDate As TextBox
            Dim txtExhname As TextBox
            Dim txtRoomtype As TextBox
            Dim txtMealplan As TextBox
            Dim txtSuppamount As TextBox
            Dim txtMinstay As TextBox
            Dim chkwithdrawn As CheckBox
            Dim txtExhicode As TextBox
            Dim chkwithdrawnamt As CheckBox



            Dim k As Integer = 0

            For Each GVRow In gv_Filldata.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                dpFDate = GVRow.FindControl("txtfromDate")
                dpTDate = GVRow.FindControl("txtToDate")
                txtExhname = GVRow.FindControl("txtExhiname")
                txtRoomtype = GVRow.FindControl("txtrmtypcode")
                txtMealplan = GVRow.FindControl("txtmealcode")
                txtSuppamount = GVRow.FindControl("txtsuppamount")
                txtMinstay = GVRow.FindControl("txtMinstay")
                chkwithdrawn = GVRow.FindControl("chkwithdraw")
                txtExhicode = GVRow.FindControl("txtExhicode")
                chkwithdrawnamt = GVRow.FindControl("chkwithdrawamt")

                If txtExhname.Text = "" Then
                    'For k = 0 To CopyRowlist.Count - 1


                    txtExhname.Text = Exhnamenew.Item(k)
                    txtRoomtype.Text = Roomtypenew.Item(k)
                    txtMealplan.Text = Mealplannew.Item(k)
                    txtSuppamount.Text = Suppamountnew.Item(k)
                    txtMinstay.Text = Minstaynew.Item(k)
                    txtExhicode.Text = Exhicodenew.Item(k)
                    If Val(withdrawnnew.Item(k)) = 1 Then
                        chkwithdrawn.Checked = True
                    Else
                        chkwithdrawn.Checked = False
                    End If
                    If Val(withdrawnamtnew.Item(k)) = 1 Then
                        chkwithdrawnamt.Checked = True
                    Else
                        chkwithdrawnamt.Checked = False
                    End If

                    If CType(fDatenew.Item(k), String) <> "" Then
                        dpFDate.Text = Format(CType(fDatenew.Item(k), Date), "dd/MM/yyyy")
                        dpTDate.Text = Format(CType(tDatenew.Item(k), Date), "dd/MM/yyyy")
                    End If

                   

                    'Exit For

                    'Next


                    k = k + 1
                    ' 

                End If

                'changed by mohamed on 21/02/2018
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = GVRow.FindControl("txtTV")
                txtNTV1 = GVRow.FindControl("txtNTV")
                txtVAT1 = GVRow.FindControl("txtVAT")

                If txtSuppamount IsNot Nothing Then
                    Call assignVatValueToTextBox(txtSuppamount, txtTV1, txtNTV1, txtVAT1)
                End If

                If k >= CopyRowlist.Count Then
                    Exit For
                End If
                n = n + 1
            Next
            Dim gridcopyrow As GridViewRow
            gridcopyrow = gv_Filldata.Rows(n)
            Dim strRowId As String = gridcopyrow.ClientID
            Dim strGridName As String = gv_Filldata.ClientID
            Dim strFoucsColumnIndex As String = "2"
            Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(n, String) + "','" + strGridName + "','" + strFoucsColumnIndex + "');")
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

            CopyClick = 0
            ClearArray()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Sub ClearArray()
        Exhnamenew.Clear()
        Roomtypenew.Clear()
        Mealplannew.Clear()
        Suppamountnew.Clear()
        Minstaynew.Clear()
        withdrawnnew.Clear()
        fDatenew.Clear()
        tDatenew.Clear()
        Exhicodenew.Clear()
        CopyRowlist.Clear()
        withdrawnamtnew.Clear()
    End Sub

    'Protected Sub btnClear1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear1.Click
    '    'ModalExtraPopup.Hide()
    'End Sub

    Protected Sub btnmealok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmealok.Click
        Try
            ' Dim txtbox As TextBox
            Dim mealname As String = ""

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
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Meal Plan');", True)
                ModalExtraPopup.Show()
                Exit Sub
            End If



            Dim chk2 As CheckBox
            Dim txtmealcode1 As Label

            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtmealcode1 = gvRow.FindControl("txtrmtypcode")

                If chk2.Checked = True Then
                    mealname = mealname + txtmealcode1.Text + ","

                End If
            Next

            If mealname.Length > 0 Then
                mealname = mealname.Substring(0, mealname.Length - 1)
            End If

            If hdnMainGridRowid.Value <> "" Then
                Dim txtmealcode As TextBox = gv_Filldata.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                txtmealcode.Text = mealname.ToString
            End If
            ModalExtraPopup.Hide()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    'Protected Sub btnmealclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnmealclose.Click
    '    ModalExtraMeal.Hide()
    'End Sub





    Protected Sub btnclearamt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclearamt.Click
        For Each GvRow In gv_Filldata.Rows

            Dim txtsuppamount As TextBox = GvRow.FindControl("txtsuppamount")

            txtsuppamount.Text = ""

            'changed by mohamed on 21/02/2018
            Dim txtTV1 As TextBox = Nothing
            Dim txtNTV1 As TextBox = Nothing
            Dim txtVAT1 As TextBox = Nothing
            txtTV1 = GvRow.FindControl("txtTV")
            txtNTV1 = GvRow.FindControl("txtNTV")
            txtVAT1 = GvRow.FindControl("txtVAT")

            If txtsuppamount IsNot Nothing Then
                Call assignVatValueToTextBox(txtsuppamount, txtTV1, txtNTV1, txtVAT1)
            End If
        Next

    End Sub

    Protected Sub btncopymeal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopymeal.Click
        Dim mealcode As String = ""
        For Each GvRow In gv_Filldata.Rows

            Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
            Dim chkSelect As CheckBox = GvRow.FindControl("chkSelect")

            If chkSelect.Checked = True Then
                mealcode = txtmealcode.Text
            End If

        Next

        For Each gvrow In gv_Filldata.Rows
            Dim txtmealcode As TextBox = gvrow.FindControl("txtmealcode")

            txtmealcode.Text = mealcode
        Next
    End Sub

    Protected Sub btnfillminnts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillminnts.Click
        For Each gvrow In gv_Filldata.Rows
            Dim ttxminstay As TextBox = gvrow.FindControl("txtMinstay")

            ttxminstay.Text = txtminnights.Text
        Next
    End Sub

    Private Sub sortgvsearch()

        Select Case ddlorder.SelectedIndex
            Case 0
                FillGrid("exhibitionid", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 1
                FillGrid("exhibitionid", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 2
                FillGrid("applicableto", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 3
                FillGrid("adddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 4
                FillGrid("adduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 5
                FillGrid("moddate", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
            Case 6
                FillGrid("moduser", hdnpartycode.Value, ddlorderby.SelectedItem.Text.Trim)
        End Select
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

                PanelMain.Visible = True
                ViewState("State") = "Copy"
                PanelMain.Style("display") = "block"
                Panelsearch.Enabled = False
                Session.Add("RefCode", CType(lblplistcode.Text.Trim, String))

                wucCountrygroup.sbSetPageState(lblplistcode.Text.Trim, Nothing, ViewState("State"))
                fillheader()
                ShowRecord(CType(lblplistcode.Text.Trim, String))
             
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                DisableControl()
                btnSave.Visible = True
                txttranid.Text = ""
                btnSave.Text = "Save"
         

                    lblHeading.Text = "Copy Exhibition Supplements - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                    Page.Title = "Promotion Exhibition Supplements "



                PanelMain.Visible = True
                ViewState("State") = "Copy"
                Session.Add("RefCode", CType(lblpromotionid.Text.Trim, String))
                PanelMain.Style("display") = "block"

                txtpromotionidnew.Text = CType(lblpromotionid.Text, String)
                txtpromoitonname.Text = CType(lblpromotionname.Text, String)
                'txtApplicableTo.Text = CType(lblapplicable.Text, String)

                fillofferdetails(lblplistcode.Text)




            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ContractExhiSupplements.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub fillofferdetails(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            gv_Filldata.Visible = True
            strSqlQry = ""





            Dim strQry As String
            Dim cnt As Integer = 0





            strQry = "select count( distinct elineno) from view_contracts_exhibition_detail where exhibitionid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            fillDategrd(gv_Filldata, False, cnt)

            Dim roomtypes As String = ""
            Dim mealplans As String = ""
            Dim strQry1 As String = "select distinct stuff((select  ',' + u.rmtypcode     from view_offers_rmtype u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            roomtypes = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry1)

            strQry1 = "select distinct stuff((select  ',' + u.mealcode     from view_offers_meal u(nolock) where promotionid ='" & hdnpromotionid.Value & "'    for xml path('')),1,1,'')"
            mealplans = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry1)

            If cnt > 0 Then
                strSqlQry = "select d.exhibitioncode,h.exhibitionname,d.fromdate,d.todate,d.supplementvalue,d.withdraw,d.withdrawamt from view_contracts_exhibition_detail d,exhibition_master h where d.exhibitioncode=h.exhibitioncode  and  d.exhibitionid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader


                Dim txtExhicode As TextBox
                Dim txtfromDate As TextBox
                Dim txtToDate As TextBox
                Dim txtrmtypcode As TextBox
                Dim txtmealcode As TextBox
                Dim txtsuppamount As TextBox
                Dim txtMinstay As TextBox
                Dim chkwithdraw As CheckBox
                Dim txtExhiname As TextBox
                Dim lblExhicode As Label
                Dim chkwithdrawamt As CheckBox
                For Each GvRow In gv_Filldata.Rows

                    txtExhicode = GvRow.FindControl("txtExhicode")
                    txtExhiname = GvRow.FindControl("txtExhiname")
                    txtfromDate = GvRow.FindControl("txtfromDate")
                    txtToDate = GvRow.FindControl("txtToDate")
                    txtrmtypcode = GvRow.FindControl("txtrmtypcode")
                    txtmealcode = GvRow.FindControl("txtmealcode")
                    txtsuppamount = GvRow.FindControl("txtsuppamount")
                    txtMinstay = GvRow.FindControl("txtMinstay")
                    chkwithdraw = GvRow.Findcontrol("chkwithdraw")
                    lblExhicode = GvRow.findcontrol("lblExhicode")
                    chkwithdrawamt = GvRow.Findcontrol("chkwithdrawamt")

                    If mySqlReader.Read = True Then

                        If IsDBNull(mySqlReader("exhibitioncode")) = False Then
                            txtExhicode.Text = mySqlReader("exhibitioncode")
                            txtExhiname.Text = mySqlReader("exhibitionname")
                            lblExhicode.Text = mySqlReader("exhibitioncode")

                        End If

                        'If IsDBNull(mySqlReader("roomtypes")) = False Then
                        txtrmtypcode.Text = roomtypes

                        'End If
                        'If IsDBNull(mySqlReader("mealplans")) = False Then
                        txtmealcode.Text = mealplans

                        'End If
                        If IsDBNull(mySqlReader("supplementvalue")) = False Then
                            txtsuppamount.Text = DecRound(mySqlReader("supplementvalue"))

                        End If
                        'If IsDBNull(mySqlReader("minstay")) = False Then
                        '    txtMinstay.Text = mySqlReader("minstay")

                        'End If

                        If IsDBNull(mySqlReader("withdraw")) = False Then
                            If mySqlReader("withdraw") = 1 Then
                                chkwithdraw.Checked = True
                            Else
                                chkwithdraw.Checked = False
                            End If
                        End If

                        If IsDBNull(mySqlReader("withdrawamt")) = False Then
                            If mySqlReader("withdrawamt") = 1 Then
                                chkwithdrawamt.Checked = True
                            Else
                                chkwithdrawamt.Checked = False
                            End If
                        End If


                        If IsDBNull(mySqlReader("fromdate")) = False Then


                            txtfromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")

                        End If
                        If IsDBNull(mySqlReader("todate")) = False Then


                            txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")

                        End If


                        'changed by mohamed on 21/02/2018
                        Dim txtTV1 As TextBox = Nothing
                        Dim txtNTV1 As TextBox = Nothing
                        Dim txtVAT1 As TextBox = Nothing
                        txtTV1 = GvRow.FindControl("txtTV")
                        txtNTV1 = GvRow.FindControl("txtNTV")
                        txtVAT1 = GvRow.FindControl("txtVAT")
                        If txtsuppamount IsNot Nothing Then
                            Call assignVatValueToTextBox(txtsuppamount, txtTV1, txtNTV1, txtVAT1)
                        End If
                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If





        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMaxocc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then
                mySqlConn.Close()
            End If

            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection
        End Try
    End Sub

    'changed by mohamed on 21/02/2018
    Function fnCalculateVATValue() As Boolean
        For Each grdRow As GridViewRow In gv_Filldata.Rows
            If (grdRow.RowType = DataControlRowType.DataRow) Then
                Dim txt1 As TextBox = grdRow.FindControl("txtsuppamount")
                Dim txtTV1 As TextBox = Nothing
                Dim txtNTV1 As TextBox = Nothing
                Dim txtVAT1 As TextBox = Nothing
                txtTV1 = grdRow.FindControl("txtTV")
                txtNTV1 = grdRow.FindControl("txtNTV")
                txtVAT1 = grdRow.FindControl("txtVAT")

                If txt1 IsNot Nothing Then
                    Call assignVatValueToTextBox(txt1, txtTV1, txtNTV1, txtVAT1)
                End If
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
        strSqlQry = "execute sp_get_taxdetail_frommaster '" & hdnpartycode.Value & "',5103" '"select * from partymast (nolock) where partycode='" & hdnpartycode.Value & "'"
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
