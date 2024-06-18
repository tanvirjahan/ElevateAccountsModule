Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Partial Class OfferMain
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim myCommand As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim ObjDate As New clsDateTime
    Dim myDataAdapter As SqlDataAdapter
    Dim chknew As CheckBox
    Dim seasonListnew As New List(Of String)
    Dim CopyRow As Integer = 0
    Dim CopyClick As Integer = 0
    Dim n As Integer = 0
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList
    Dim bookingcodenew As New ArrayList
    Dim discounttypenew As New ArrayList
    Dim discountpercnew As New ArrayList
    Dim adddiscountpercnew As New ArrayList
    Dim noofroomsnew As New ArrayList
    Dim multiyesnew As New ArrayList
    Dim bookingvaliditynew As New ArrayList
    Dim bookfdatenew As New ArrayList
    Dim booktdatenew As New ArrayList
    Dim bookdaysnew As New ArrayList
    Dim minnightsnew As New ArrayList
    Dim maxnightsnew As New ArrayList

    Dim rmtypnamenew As New ArrayList
    Dim uprmtypnamenew As New ArrayList
    Dim rmcombinationnew As New ArrayList

    Dim mealcodenew As New ArrayList
    Dim upmealcodenew As New ArrayList
    Dim mealcombinationnew As New ArrayList

    Dim rmcatcodenew As New ArrayList
    Dim uprmcatcodenew As New ArrayList

    Dim mealrmcatcodenew As New ArrayList
    Dim upmealrmcatcodenew As New ArrayList


    Dim staynew As New ArrayList
    Dim payfornew As New ArrayList
    Dim maxfreentsnew As New ArrayList
    Dim maxmultiplesnew As New ArrayList
    Dim multiplesnew As New ArrayList
    Dim applynew As New ArrayList

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
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            wucCountrygroup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                'If HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl") IsNot Nothing Then 'changed by mohamed on 03/10/2016 - instead of selected, used all
                'lsCountryList = HttpContext.Current.Session("AllCountriesList_WucCountryGroupUserControl").ToString.Trim
                If lsCountryList <> "" Then
                    'strSqlQry += " and a.ctrycode in (" & lsCountryList & ")" 'changed by mohamed on 03/10/2016 -'commented this line to show all the agents.
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
    '*** Danny 19/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    Private Function ShowHide(ByVal ddlapply As DropDownList) As DropDownList
        Try
            If IsNothing(Session("1002")) Then ''*** Danny 1002 is Excel Document number 
                Session("1002") = String.Empty
            End If
            If Session("1002").ToString() <> "SHOW" Then
                If ddlapply.Items.Count = 4 Then
                    ddlapply.Items.RemoveAt(3)
                End If
            End If
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractMinNights.aspx=>ShowHide()", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ShowHide = ddlapply
    End Function
    '*** Danny 19/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RefCode As String
        If IsPostBack = False Then

            '   PanelMain.Visible = True
            'charcters(txtCode)
            'charcters(txtName)



            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
            Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

            If Session("OfferState") Is Nothing Then
                Session("OfferState") = CType(Request.QueryString("State"), String)

            End If


            If CType(Session("OfferState"), String) <> "New" Then
                'Session("OfferRefCode") = CType(Request.QueryString("promotionid"), String)
                'Session("promotionid") = CType(Request.QueryString("promotionid"), String)
                wucCountrygroup.sbSetPageState(Session("OfferRefCode"), Nothing, Nothing)
            End If

            txtconnection.Value = Session("dbconnectionName")

            hdCurrentDate.Value = Now.ToString("dd/MM/yyyy")

            Session("partycode") = CType(Request.QueryString("partycode"), String)
            hdnpartycode.Value = CType(Request.QueryString("partycode"), String)
            txthotelname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast(nolock) where partycode='" & Session("partycode") & "'")
            txthotelname.Enabled = False


            lblstatustext.Visible = False
            lblstatus.Visible = False

            gridrmtype.Columns(3).Visible = False
            grdmealplan.Columns(3).Visible = False
            grdrmcat.Columns(3).Visible = False
            'grdpromotiondetail.Columns(4).Visible = False
            'grdpromotiondetail.Columns(5).Visible = False
            'grdpromotiondetail.Columns(6).Visible = False

            'grdpromotiondetail.Columns(7).Visible = False
            'grdpromotiondetail.Columns(8).Visible = False
            'grdpromotiondetail.Columns(9).Visible = False

            grdpromotiondetail.Columns(1).Visible = True
            grdpromotiondetail.Columns(2).Visible = True

            grdpromotiondetail.Columns(15).Visible = False
            grdpromotiondetail.Columns(16).Visible = False
            grdpromotiondetail.Columns(17).Visible = False
            grdpromotiondetail.Columns(18).Visible = False
            grdpromotiondetail.Columns(19).Visible = False
            grdpromotiondetail.Columns(20).Visible = False

            btnselectcontract.Style.Add("display", "none")
            divflight.Style.Add("display", "none")
            divsplocc.Style.Add("display", "none")
            divinter.Style.Add("display", "none")
            divstay.Style.Add("display", "none")
            divcomptrf.Style.Add("display", "none")
            divArrivalTransfer.Style.Add("display", "none")
            divDepartureTransfer.Style.Add("display", "none")
            ddlcombine.Attributes.Add("onchange", "showpromotion()")
            ddlapplydiscount.Attributes.Add("onchange", "showcontract()")
            '  divapplydiscount.Style.Add("display", "none")
            Session("Checks") = ""


            wucCountrygroup.sbSetPageState("", "OFFERSMAIN", CType(Session("OfferState"), String))

            If CType(Session("OfferState"), String) = "New" Then
                txtpromotionname.Focus()
                Page.Title = Page.Title + " " + "New Offer"
                txtCode.Disabled = True
                ' wucCountrygroup.clearsessions()
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                ddlapplydiscount.SelectedIndex = 0

                lblHeading.Text = "Add New Offer - " + txthotelname.Text
                ' fillDategrd(grdpromodates, True)

                filldaysgrid()
                '   fillDategrd(gridrmtype, True)
                FillroomsGrids()
                fillDategrd(grdpromotiondetail, True)
                fillDategrd(grdinterhotel, True)
                fillDategrd(grdflight, True)
                fillDategrd(grdcombinable, True)
                fillDategrd(grdArrivalTransfer, True)
                fillDategrd(grdDepartureTransfer, True)
                'txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")

                Bookingoptionchange()
                For Each gvrow In grdpromotiondetail.Rows
                    Dim txtminnights As TextBox = CType(gvrow.FindControl("txtminnights"), TextBox)
                    txtminnights.Text = 1
                Next

            ElseIf CType(Session("OfferState"), String) = "Copy" Then
                txtpromotionname.Focus()

                Page.Title = Page.Title + " " + "Copy Offer"
                txtCode.Disabled = True
                RefCode = CType(Session("OfferRefCode"), String)
                ShowRecord(RefCode)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("OfferState"), String))
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                filldaysgrid()

                FillroomsGrids()
                Showdetailsgrid(CType(RefCode, String))
                '  MakeFilterGridSelected()
                Fillothergrids(RefCode)
                lblHeading.Text = "Copy Offer - " + txthotelname.Text
                txtpromotionid.Text = ""
                'txtCode.Value = ""
                'txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rnkorder),0) from partymast") + 1
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")

            ElseIf CType(Session("OfferState"), String) = "Edit" Then
                txtpromotionname.Focus()
                btnSave.Text = "Update"

                RefCode = CType(Session("OfferRefCode"), String)

                ShowRecord(RefCode)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("OfferState"), String))
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                filldaysgrid() ' 

                FillroomsGrids()
                Showdetailsgrid(CType(RefCode, String))
                ' MakeFilterGridSelected()
                Fillothergrids(RefCode)
                txtpromotionid.Enabled = False
                txthotelname.Enabled = False
                lblHeading.Text = "Edit Offer - " + txthotelname.Text

                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")


                '  btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Contracts?')==false)return false;")
            ElseIf CType(Session("OfferState"), String) = "View" Then

                RefCode = CType(Session("OfferRefCode"), String)
                txtpromotionname.Focus()
                txtCode.Disabled = True
                txtName.Disabled = True
                RefCode = CType(Session("OfferRefCode"), String)
                ShowRecord(RefCode)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("OfferState"), String))
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                filldaysgrid() '

                FillroomsGrids()
                Showdetailsgrid(CType(RefCode, String))
                ' MakeFilterGridSelected()
                Fillothergrids(RefCode)
                Page.Title = Page.Title + " " + "View Offer "
                btnSave.Visible = False
                btnCancel.Text = "Return to Search"
                btnCancel.Focus()
                txtpromotionid.Enabled = False
                txthotelname.Enabled = False
                DisableControl()
                lblHeading.Text = "View Offer - " + txthotelname.Text
            ElseIf CType(Session("OfferState"), String) = "PendingView" Then 'Added by abin on 20190219


                Session.Remove("OfferState")
                Session.Add("OfferState", "View")

                Dim offerid As String = Request.QueryString("offerid")

                Dim partycode As String = Request.QueryString("partycode")
                Session.Add("OfferRefCode", CType(offerid, String))



                Session("Offerparty") = partycode




                RefCode = CType(Session("OfferRefCode"), String)
                txtpromotionname.Focus()
                txtCode.Disabled = True
                txtName.Disabled = True
                RefCode = CType(Session("OfferRefCode"), String)
                ShowRecord(RefCode)


                If Session("sDtDynamic") Is Nothing Then
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcValue = New DataColumn("Value", GetType(String))
                    Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcValue)
                    dtDynamic.Columns.Add(dcCodeAndValue)
                    Session("sDtDynamic") = dtDynamic
                End If
     

                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("OfferState"), String))
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                filldaysgrid() '

                FillroomsGrids()
                Showdetailsgrid(CType(RefCode, String))
                '  MakeFilterGridSelected()
                Fillothergrids(RefCode)
                Page.Title = Page.Title + " " + "View Offer "
                btnSave.Visible = False
                btnCancel.Text = "Return to Search"
                btnCancel.Focus()
                txtpromotionid.Enabled = False
                txthotelname.Enabled = False
                DisableControl()
                lblHeading.Text = "View Offer - " + txthotelname.Text
            ElseIf CType(Session("OfferState"), String) = "Delete" Then
                txtpromotionname.Focus()
                RefCode = CType(Session("OfferRefCode"), String)
                RefCode = CType(Session("OfferRefCode"), String)

                ShowRecord(RefCode)
                wucCountrygroup.sbSetPageState(RefCode, Nothing, CType(Session("OfferState"), String))
                wucCountrygroup.sbShowCountry()
                promotiontypefill()
                filldaysgrid() ' 

                FillroomsGrids()
                Showdetailsgrid(CType(RefCode, String))
                ' MakeFilterGridSelected()
                Fillothergrids(RefCode)

                DisableControl()

                Page.Title = Page.Title + " " + "Delete Offer"
                lblHeading.Text = "Delete Offer - " + txthotelname.Text
                btnSave.Text = "Delete"
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Contracts?')==false)return false;")

            End If

            Session.Add("1002", objUtils.GetString("strDBConnection", "select option_selected from reservation_parameters where param_id=4")) '*** Danny Added 19/03/2018
           
        Else
            Try
                showDiv()



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If ddlcombine.SelectedIndex <> 2 And ddlcombine.SelectedIndex <> 4 Then
            divcombine.Style.Add("display", "none")
        Else
            divcombine.Style.Add("display", "block")
        End If
        'chkinter.Attributes.Add("onChange", "showintergrid('" & chkinter.ClientID & "')")

        btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
        txtstayfor.Attributes.Add("onkeypress", "return  checkNumber(event)")
        txtpayfor.Attributes.Add("onkeypress", "return  checkNumber(event)")
        txtmaxfreents.Attributes.Add("onkeypress", "return  checkNumber(event)")
        txtmaxmultiples.Attributes.Add("onkeypress", "return  checkNumber(event)")

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If


       
        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region
    Private Sub Fillothergrids(ByVal refcode As String)



        '----------------------------------Show Records In Promotion Inter hotels Grid 

        Dim strqry As String = ""
        Dim cnt As Integer = 0
        strqry = "select count( partycode)  from view_offers_interhotel(nolock) where promotionid='" & refcode & "'"
        cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strqry)
        fillDategrd(grdinterhotel, False, cnt)

        strqry = "select count( flightcode)  from view_offers_flight(nolock) where promotionid='" & refcode & "'"
        cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strqry)
        fillDategrd(grdflight, False, cnt)

        strqry = "select count( airportcode)  from view_offers_transfers(nolock) where  transfertype='Arrival'  and  promotionid='" & refcode & "'"
        cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strqry)
        fillDategrd(grdArrivalTransfer, False, cnt)

        strqry = "select count( airportcode)  from view_offers_transfers(nolock) where transfertype='Departure'  and promotionid='" & refcode & "'"
        cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strqry)
        fillDategrd(grdDepartureTransfer, False, cnt)

        strqry = "select count( combinableid)  from view_offers_combinable(nolock) where  promotionid='" & refcode & "'"
        cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strqry)
        fillDategrd(grdcombinable, False, cnt)




        Dim txtpartycode As TextBox

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        myCommand = New SqlCommand("Select v.*,p.partyname from view_offers_interhotel v(nolock), partymast p(nolock) Where v.partycode=p.partycode and v.promotionid='" & refcode & "'", mySqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        If mySqlReader.HasRows Then
            divinter.Style.Add("display", "block")
            While mySqlReader.Read()
                If IsDBNull(mySqlReader("partycode")) = False Then
                    For Each gvRow In grdinterhotel.Rows
                        txtpartycode = CType(gvRow.FindControl("txtinterhotelcode"), TextBox)
                        Dim txtinterhotelname As TextBox = CType(gvRow.FindControl("txtinterhotelname"), TextBox)
                        Dim txtinterminnights As TextBox = CType(gvRow.FindControl("txtinterminnights"), TextBox)
                        If txtpartycode.Text = "" Then

                            txtpartycode.Text = CType(mySqlReader("partycode"), String)
                            txtinterhotelname.Text = CType(mySqlReader("partyname"), String) 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName").ToString, "select partyname from  partymast where partycode='" & txtpartycode.Text & "'")
                            txtinterminnights.Text = CType(mySqlReader("minstay"), String)

                            Exit For
                        End If

                    Next
                End If
            End While
        Else
            fillDategrd(grdinterhotel, True)
        End If
        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 


        '----------------------------------Show Records In Promotion Flights Grid 


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        myCommand = New SqlCommand("Select * from view_offers_flight v(nolock) Where  v.promotionid='" & refcode & "'", mySqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        If mySqlReader.HasRows Then
            divflight.Style.Add("display", "block")
            While mySqlReader.Read()
                If IsDBNull(mySqlReader("flightcode")) = False Then
                    For Each gvRow In grdflight.Rows
                        Dim txtflightcode = CType(gvRow.FindControl("txtflightcode"), TextBox)

                        If txtflightcode.Text = "" Then

                            txtflightcode.Text = CType(mySqlReader("flightcode"), String)


                            Exit For
                        End If

                    Next
                End If
            End While
        Else
            fillDategrd(grdflight, True)
        End If
        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 


        '----------------------------------Show Records In Promotion Arrival Transfer Grid 


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        myCommand = New SqlCommand("Select * from view_offers_transfers v(nolock) Where v.transfertype='Arrival' and  v.promotionid='" & refcode & "'", mySqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        If mySqlReader.HasRows Then
            divArrivalTransfer.Style.Add("display", "block")
            While mySqlReader.Read()
                If IsDBNull(mySqlReader("airportcode")) = False Then
                    For Each gvRow In grdArrivalTransfer.Rows
                        Dim txtarrivalterminal = CType(gvRow.FindControl("txtarrivalterminal"), TextBox)
                        Dim txtarrivalAirportName = CType(gvRow.FindControl("txtarrivalAirportName"), TextBox)
                        Dim txtflightcode = CType(gvRow.FindControl("txtflightcode"), TextBox)

                        If txtarrivalterminal.Text = "" Then

                            txtarrivalterminal.Text = CType(mySqlReader("airportcode"), String)
                            txtarrivalAirportName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName").ToString, "select airportbordername from  airportbordersmaster where airportbordercode='" & txtarrivalterminal.Text & "'")
                            txtflightcode.Text = CType(mySqlReader("flightcode"), String)

                            Exit For
                        End If

                    Next
                End If
            End While
        Else
            fillDategrd(grdArrivalTransfer, True)
        End If
        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 


        '----------------------------------Show Records In Promotion Departure Transfer Grid 


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        myCommand = New SqlCommand("Select * from view_offers_transfers v(nolock) Where v.transfertype='Departure' and  v.promotionid='" & refcode & "'", mySqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        If mySqlReader.HasRows Then
            divDepartureTransfer.Style.Add("display", "block")
            While mySqlReader.Read()
                If IsDBNull(mySqlReader("airportcode")) = False Then
                    For Each gvRow In grdDepartureTransfer.Rows
                        Dim txtdepartureterminal = CType(gvRow.FindControl("txtdepartureterminal"), TextBox)
                        Dim txtdepartureAirportName = CType(gvRow.FindControl("txtdepartureAirportName"), TextBox)
                        Dim txtdepflightcode = CType(gvRow.FindControl("txtdepflightcode"), TextBox)

                        If txtdepartureterminal.Text = "" Then

                            txtdepartureterminal.Text = CType(mySqlReader("airportcode"), String)
                            txtdepartureAirportName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName").ToString, "select airportbordername from  airportbordersmaster where airportbordercode='" & txtdepartureterminal.Text & "'")
                            txtdepflightcode.Text = CType(mySqlReader("flightcode"), String)
                            Exit For
                        End If

                    Next
                End If
            End While
        Else
            fillDategrd(grdDepartureTransfer, True)
        End If
        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 



        '----------------------------------Show Records In Promotion Combinable


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open 
        myCommand = New SqlCommand("Select * from view_offers_combinable v(nolock) Where  v.promotionid='" & refcode & "'", mySqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
        If mySqlReader.HasRows Then
            divcombine.Style.Add("display", "block")
            While mySqlReader.Read()
                If IsDBNull(mySqlReader("combinableid")) = False Then
                    For Each gvRow In grdcombinable.Rows
                        Dim txtpromotioncode = CType(gvRow.FindControl("txtpromotioncode"), TextBox)
                        Dim txtpromotionname = CType(gvRow.FindControl("txtpromotionname"), TextBox)

                        If txtpromotioncode.Text = "" Then

                            txtpromotioncode.Text = CType(mySqlReader("combinableid"), String)
                            txtpromotionname.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName").ToString, "select promotionname from  view_offers_header where promotionid='" & txtpromotioncode.Text & "'")

                            Exit For
                        End If

                    Next
                End If
            End While
        Else
            fillDategrd(grdcombinable, True)
        End If
        clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
        clsDBConnect.dbReaderClose(mySqlReader)              'sql reader disposed    
        clsDBConnect.dbConnectionClose(mySqlConn)              'connection close 


    End Sub
    Protected Sub btnmealrmcatshow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtmealrmcatcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        'strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

        strSqlQry = "select prc.rmcatcode rmtypcode,rc.rmcatname rmtypname,isnull(rc.units,0) rankord  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra in ('M','L') and prc.partycode='" _
                      & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"


        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Mealsupp.Category"
        gv_Showroomtypes.Columns(4).Visible = False

        'ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)

        ViewState("noshowclick") = 5
        ModalPopupNoshow.Show()

        'Enablegrid()
    End Sub
    Protected Sub btnaccrmcatcode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtrmcatcode"), TextBox).Text


        Dim strmeal As String = CType(strmealname, String)
        fillControls()
        ViewState("noshowclick") = 4

        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        'strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"

        strSqlQry = "select prc.rmcatcode as rmtypcode,prc.rmcatcode rmtypname,isnull(rc.units,0) rankord  from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra in ('A','C') and prc.partycode='" _
                         & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"


        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Accom.Category"

        gv_Showroomtypes.Columns(4).Visible = False

        'ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)


        ModalPopupNoshow.Show()

        'Enablegrid()
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

        Dim strmealname As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtmealcode"), TextBox).Text

        Dim strChkdValnew As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtmealcombination"), TextBox).Text


        Dim checkedrow As Integer = 0
        Dim promoList As String = String.Empty
        Dim count As Integer = 0

        For Each item As DataListItem In dlPromotionType.Items
            Dim chkRow As CheckBox = TryCast(item.FindControl("chkpromotiontype"), CheckBox)
            Dim txtbox As Label = TryCast(item.FindControl("txtpromtoiontype"), Label)
            If chkRow.Checked = True Then
                Dim lbltype As String = txtbox.Text
                'item.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
                Dim strpop As String = ""
                checkedrow = checkedrow + 1
                If count = 0 Then
                    promoList = lbltype
                    count = 1
                Else
                    promoList &= "|" & lbltype
                End If
            End If
        Next


        Dim strmeal As String = CType(strmealname, String)
        fillControls()
        ViewState("noshowclick") = 3
        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

        'strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"
      
        strSqlQry = "select p.mealcode as rmtypcode,m.mealname rmtypname,m.rankorder rankord from  partymeal p(nolock),mealmast m(nolock) where p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"


        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
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

        Dim strCondition As String = ""
        If promoList.Length <> 0 Then

            Dim mString As String() = promoList.Split("|")

            If promoList.Contains("Meal Upgrade") = True Then
                ViewState("MealUpgrade") = 1
                gv_Showroomtypes.Columns(4).Visible = True
            Else
                gv_Showroomtypes.Columns(4).Visible = False
            End If

            'For i As Integer = 0 To mString.Length - 1

            '    If UCase(mString(i)) = UCase("Meal Upgrade") Then
            '        ViewState("MealUpgrade") = 1
            '        gv_Showroomtypes.Columns(4).Visible = True
            '    Else
            '        gv_Showroomtypes.Columns(4).Visible = False
            '    End If
            'Next
        End If

        If ViewState("MealUpgrade") = 1 Then
            ChkExistingRoomtypes(strChkdValnew)
        Else
            ChkExistingRoomtypes(strmeal)
        End If



        ModalPopupNoshow.Show()

        'Enablegrid()
    End Sub
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

            Dim chkpromotiontype As CheckBox
            Dim txtpromtoiontype As Label
            Dim promotypes As String = ""

            For Each gvRow As DataListItem In dlPromotionType.Items
                chkpromotiontype = gvRow.FindControl("chkpromotiontype")
                txtpromtoiontype = gvRow.FindControl("txtpromtoiontype")
                ' chkAll = dlWeekDays.Controls(0).Controls(0).FindControl("chkAll")

                If chkpromotiontype.Checked = True Then
                    promotypes = promotypes + txtpromtoiontype.Text + ","
                End If


            Next

            If promotypes.Length > 0 Then
                promotypes = promotypes.Substring(0, promotypes.Length - 1)
            End If





            For Each gvRow As GridViewRow In gv_Showroomtypes.Rows
                chk2 = gvRow.FindControl("chkrmtype")
                txtrmtypcode1 = gvRow.FindControl("txtrmtypcode")
                lblrmtypname = gvRow.FindControl("lblrmtypname")
                Dim ddlUpgrade As HtmlSelect = gvRow.FindControl("ddlUpgrade")

                If chk2.Checked = True Then
                    roomtypes = roomtypes + txtrmtypcode1.Text + ","

                    If CType(promotypes, String).Contains("Upgrade") = True Then


                        If ddlUpgrade.Value <> "[Select]" Then
                            strChkdStringVal.AppendFormat("{0};{1},", txtrmtypcode1.Text, ddlUpgrade.Value)
                            rmtypname = rmtypname + ddlUpgrade.Value + ","
                        Else
                            strChkdStringVal.AppendFormat("{0};{1},", txtrmtypcode1.Text, txtrmtypcode1.Text)
                            rmtypname = ""
                        End If
                    Else
                        strChkdStringVal.AppendFormat("{0};{1},", txtrmtypcode1.Text, txtrmtypcode1.Text)
                        rmtypname = ""
                    End If

                End If
            Next

            If roomtypes Is Nothing Then
                roomtypes = " "

            Else
                roomtypes = roomtypes.Substring(0, roomtypes.Length - 1)
                If rmtypname.Length > 0 Then
                    rmtypname = rmtypname.Substring(0, rmtypname.Length - 1)
                End If
            End If

                If gv_Showroomtypes.HeaderRow.Cells(3).Text = "Flight Code" Then
                    If hdnMainGridRowid.Value <> "" Then

                        If ViewState("noshowclick") = 1 Then
                            Dim txtmealcode As TextBox = grdDepartureTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtdepflightcode")
                            txtmealcode.Text = roomtypes.ToString
                        ElseIf ViewState("noshowclick") = 2 Then
                            Dim txtmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                            txtmealcode.Text = roomtypes.ToString
                            Dim txtrmcombination As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmcombination")
                            txtrmcombination.Text = strChkdStringVal.ToString
                            Dim txtuprmtypname As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtuprmtypname")
                            txtuprmtypname.Text = rmtypname.ToString

                        ElseIf ViewState("noshowclick") = 3 Then
                            Dim txtmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                            txtmealcode.Text = roomtypes.ToString
                            Dim txtmealcombination As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealcombination")
                            txtmealcombination.Text = strChkdStringVal.ToString
                            Dim txtupmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtupmealcode")
                            txtupmealcode.Text = rmtypname.ToString

                        ElseIf ViewState("noshowclick") = 4 Then
                            Dim txtmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmcatcode")
                            txtmealcode.Text = roomtypes.ToString
                        ElseIf ViewState("noshowclick") = 5 Then
                            Dim txtmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealrmcatcode")
                            txtmealcode.Text = roomtypes.ToString
                        Else
                            Dim txtmealcode As TextBox = grdArrivalTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtflightcode")
                            txtmealcode.Text = roomtypes.ToString
                        End If

                    End If
                Else
                    If hdnMainGridRowid.Value <> "" Then
                        If ViewState("noshowclick") = 1 Then
                            Dim txtrmtypname As TextBox = grdDepartureTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                            Dim txtrmtypcode As TextBox = grdDepartureTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                            txtrmtypname.Text = rmtypname.ToString
                            txtrmtypcode.Text = roomtypes.ToString
                        ElseIf ViewState("noshowclick") = 2 Then
                            Dim txtrmtypcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                            txtrmtypcode.Text = roomtypes.ToString
                            Dim txtrmcombination As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmcombination")
                            txtrmcombination.Text = strChkdStringVal.ToString
                            Dim txtuprmtypname As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtuprmtypname")
                            txtuprmtypname.Text = rmtypname.ToString

                        ElseIf ViewState("noshowclick") = 3 Then
                            Dim txtrmtypcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealcode")
                            txtrmtypcode.Text = roomtypes.ToString

                            Dim txtmealcombination As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealcombination")
                            txtmealcombination.Text = strChkdStringVal.ToString
                            Dim txtupmealcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtupmealcode")
                            txtupmealcode.Text = rmtypname.ToString

                        ElseIf ViewState("noshowclick") = 4 Then

                            Dim txtrmtypcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtrmcatcode")
                            txtrmtypcode.Text = roomtypes.ToString
                        ElseIf ViewState("noshowclick") = 5 Then

                            Dim txtrmtypcode As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtmealrmcatcode")
                            txtrmtypcode.Text = roomtypes.ToString
                        Else
                            Dim txtrmtypname As TextBox = grdArrivalTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypname")
                            Dim txtrmtypcode As TextBox = grdArrivalTransfer.Rows(hdnMainGridRowid.Value).FindControl("txtrmtypcode")
                            txtrmtypname.Text = rmtypname.ToString
                            txtrmtypcode.Text = roomtypes.ToString
                        End If




                    End If
                End If



                ViewState("noshowclick") = Nothing
            ModalExtraPopup.Hide()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnflight_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdArrivalTransfer.Rows(rowid).FindControl("txtflightcode"), TextBox).Text

        Dim strarrterminal As String = CType(grdArrivalTransfer.Rows(rowid).FindControl("txtarrivalterminal"), TextBox).Text
        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

      

        If strarrterminal = "" Then
            strSqlQry = "select  flightcode rmtypcode,flightcode rmtypname, 0 rankord from flightmast where type=1 and active=1   order by flightcode"
        Else
            strSqlQry = "select  flightcode rmtypcode,flightcode rmtypname, 0 rankord from flightmast where type=1 and active=1  and airportbordercode='" & strarrterminal & "' order by flightcode"
        End If



        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Flight Code"

        'ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)

        '   Enablegrid()
        ModalExtraPopup.Show()


    End Sub

    Protected Sub btndepflight_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        'btnmealok.Style("display") = "block"
        'btnOk1.Style("display") = "none"

        Dim strmealname As String = CType(grdDepartureTransfer.Rows(rowid).FindControl("txtdepflightcode"), TextBox).Text

        Dim strarrterminal As String = CType(grdDepartureTransfer.Rows(rowid).FindControl("txtdepartureterminal"), TextBox).Text
        Dim strmeal As String = CType(strmealname, String)


        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


        If strarrterminal = "" Then
            strSqlQry = "select  flightcode rmtypcode,flightcode rmtypname,0 rankord from flightmast where type=1 and active=0   order by flightcode"
        Else


            strSqlQry = "select  flightcode rmtypcode,flightcode rmtypname,0 rankord from flightmast where type=1 and active=0  and airportbordercode='" & strarrterminal & "' order by flightcode"
        End If


        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
        myDataAdapter.Fill(MyDs)

        If MyDs.Rows.Count > 0 Then
            gv_Showroomtypes.DataSource = MyDs
            gv_Showroomtypes.DataBind()
            gv_Showroomtypes.Visible = True
            hdnMainGridRowid.Value = rowid
        Else

            gv_Showroomtypes.Visible = False

        End If
        gv_Showroomtypes.HeaderRow.Cells(3).Text = "Flight Code"

        'ChkExistingMeal(strmeal)
        ChkExistingRoomtypes(strmeal)
        ViewState("noshowclick") = 1
        '   Enablegrid()
        ModalExtraPopup.Show()


    End Sub
    Private Sub ChkExistingRoomtypes(ByVal prm_strChkRoomtypes As String)
        Dim chkSelect As CheckBox
        Dim txtrmtypcode As Label


        Dim arrString As String()
        Dim arrlist As String()


        If prm_strChkRoomtypes <> "" Then
            arrString = prm_strChkRoomtypes.Split(",") 'spliting for ';' 1st

            If ViewState("MealUpgrade") = 1 Or ViewState("RoomUpgrade") = 1 Then

                Dim i As Integer = 0

                For k = 0 To arrString.Length - 1
                    For Each grdRow In gv_Showroomtypes.Rows
                        Dim ddlUpgrade As HtmlSelect = grdRow.FindControl("ddlUpgrade")
                        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                        txtrmtypcode = CType(grdRow.FindControl("txtrmtypcode"), Label)


                        If arrString(k) <> "" Then
                            arrlist = arrString(k).Split(";")
                            If arrlist(0) = txtrmtypcode.Text Then
                                chkSelect.Checked = True
                                ddlUpgrade.Value = CType(arrlist(1), String)
                            End If

                        End If


                        i += 1
                    Next
                Next

            Else
                For k = 0 To arrString.Length - 1
                    'If arrString(k) <> "" Then

                    '  Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                    For Each grdRow In gv_Showroomtypes.Rows
                        Dim ddlUpgrade As HtmlSelect = grdRow.FindControl("ddlUpgrade")
                        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
                        txtrmtypcode = CType(grdRow.FindControl("txtrmtypcode"), Label)

                        If arrString(k) = txtrmtypcode.Text Then
                            chkSelect.Checked = True
                            ' ddlUpgrade.Value = ""
                        End If

                    Next
                    'End If
                Next
            End If
            'Else
            '    'first case when not selected , chk all
            '    For Each grdRow In gvOccupancy.Rows
            '        chkSelect = CType(grdRow.FindControl("chkrmtype"), CheckBox)
            '        chkSelect.Checked = True

            '    Next


        End If

        ViewState("MealUpgrade") = Nothing
        ViewState("RoomUpgrade") = Nothing
    End Sub
    Private Sub MakeFilterGridSelected()
        Dim dt As New DataTable
        Dim chksel As CheckBox
        Dim gvRow As GridViewRow
        Dim i As Integer
        dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmtypcode from view_offers_rmtype where promotionid='" & txtpromotionid.Text & "'").Tables(0)
        For Each gvRow In gridrmtype.Rows
            chksel = gvRow.FindControl("chkrmtype")
            Dim txtrmtypcode As Label = gvRow.FindControl("txtrmtypcode")
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)(0).ToString = txtrmtypcode.Text Then
                    chksel.Checked = True
                End If
            Next
        Next

        dt = New DataTable
        dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmcatcode from view_offers_accomodation where promotionid='" & txtpromotionid.Text & "'").Tables(0)
        For Each gvRow In grdrmcat.Rows
            chksel = gvRow.FindControl("chkrmcat")
            Dim txtrmcatcode As Label = gvRow.FindControl("txtrmcatcode")
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)(0).ToString = txtrmcatcode.Text Then
                    chksel.Checked = True
                End If
            Next
        Next

        dt = New DataTable
        dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select mealcode from view_offers_meal where promotionid='" & txtpromotionid.Text & "'").Tables(0)
        For Each gvRow In grdmealplan.Rows
            chksel = gvRow.FindControl("chkmeal")
            Dim txtmealcode As Label = gvRow.FindControl("txtmealcode")
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)(0).ToString = txtmealcode.Text Then
                    chksel.Checked = True
                End If
            Next
        Next


        dt = New DataTable
        dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select rmcatcode from view_offers_supplement where promotionid='" & txtpromotionid.Text & "'").Tables(0)
        For Each gvRow In grdsupplement.Rows
            chksel = gvRow.FindControl("chksuprmcat")
            Dim lblrmcatcode As Label = gvRow.FindControl("lblrmcatcode")
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)(0).ToString = lblrmcatcode.Text Then
                    chksel.Checked = True
                End If
            Next
        Next


        Dim strval As String = ""
        Dim strval1 As String = ""
        Dim gvRow1 As GridViewRow
        Dim ddl As HtmlSelect
        Dim ddl1 As HtmlSelect
        For Each gvRow1 In gridrmtype.Rows
            Dim txtrmtypcode As Label = gvRow1.FindControl("txtrmtypcode")
            Dim hdnrmtypcode As HiddenField = gvRow1.FindControl("hdnrmtypcode")
            strval = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 rmtypeupgrade from view_offers_rmtype(nolock) where promotionid='" & txtpromotionid.Text & "' and rmtypcode='" & txtrmtypcode.Text & "'")
            ddl = gvRow1.FindControl("ddlUpgradermtyp")
            If strval <> "" Then
                ddl.Value = strval
                hdnrmtypcode.Value = strval
            End If
        Next


        For Each gvRow1 In grdmealplan.Rows
            Dim txtmealcode As Label = gvRow1.FindControl("txtmealcode")
            Dim hdnmealcode As HiddenField = gvRow1.FindControl("hdnmealcode")
            strval = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 mealupgrade from view_offers_meal(nolock) where promotionid='" & txtpromotionid.Text & "' and mealcode='" & txtmealcode.Text & "'")
            ddl = gvRow1.FindControl("ddlUpgrademeal")
            If strval <> "" Then
                ddl.Value = strval
                hdnmealcode.Value = strval
            End If
        Next

        For Each gvRow1 In grdrmcat.Rows
            Dim txtrmcatcode As Label = gvRow1.FindControl("txtrmcatcode")
            Dim hdnrmcatcode As HiddenField = gvRow1.FindControl("hdnrmcatcode")
            strval = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 rmcatupgrade from view_offers_accomodation(nolock) where promotionid='" & txtpromotionid.Text & "' and rmcatcode='" & txtrmcatcode.Text & "'")
            ddl = gvRow1.FindControl("ddlUpgradermcat")
            If strval <> "" Then
                ddl.Value = strval
                hdnrmcatcode.Value = strval
            End If
        Next


        'Dim mealcodestr As String = ""
        'For Each gvRow1 In grdmealplan.Rows
        '    mealcodestr = gvRow1.Cells(1).Text
        '    strval = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 mealstay from vw_promotion_detail where promotionid='" & txtPramotionId.Text & "' and mealcode='" & mealcodestr & "'")
        '    strval1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 mealextra from vw_promotion_detail where promotionid='" & txtPramotionId.Text & "' and mealcode='" & mealcodestr & "'")
        '    ddl = gvRow1.FindControl("ddlMealStayPromo")
        '    If strval <> "" Then
        '        ddl.Value = strval
        '    End If
        '    ddl1 = gvRow1.FindControl("ddlMealExtraPromo")
        '    If strval1 <> "" Then
        '        ddl1.Value = strval1
        '    End If
        'Next

    End Sub
    Private Sub Showdetailsgrid(ByVal RefCode As String)
        Try
            Dim myDS As New DataSet
            grdpromotiondetail.Visible = True
            strSqlQry = ""


            Dim strQry As String = ""
            Dim cnt As Integer = 0


            strQry = "select count( distinct plineno) from view_offers_detail(nolock) where promotionid='" & RefCode & "'"

            cnt = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry)
            fillDategrd(grdpromotiondetail, False, cnt)

            If cnt > 0 Then
                strSqlQry = "select * from view_offers_detail d(nolock)  where   d.promotionid='" & RefCode & "'"
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand(strSqlQry, mySqlConn)
                mySqlReader = myCommand.ExecuteReader



                For Each GvRow In grdpromotiondetail.Rows

                    Dim txtfromdate As TextBox = GvRow.FindControl("txtfromDate")
                    Dim txtToDate As TextBox = GvRow.FindControl("txtToDate")
                    Dim txthotelbookingcode As TextBox = GvRow.FindControl("txthotelbookingcode")
                    Dim ddloptions As DropDownList = GvRow.FindControl("ddloptions")
                    Dim txtdiscount As TextBox = GvRow.FindControl("txtdiscount")
                    Dim txtadddiscount As TextBox = GvRow.FindControl("txtadddiscount")
                    Dim txtnoofrooms As TextBox = GvRow.FindControl("txtnoofrooms")
                    Dim chkmultiyes As CheckBox = GvRow.FindControl("chkmultiyes")
                    Dim ddlbookoptions As DropDownList = GvRow.FindControl("ddlbookoptions")
                    Dim txtbookfromDate As TextBox = GvRow.FindControl("txtbookfromDate")
                    Dim txtBookToDate As TextBox = GvRow.FindControl("txtBookToDate")
                    Dim txtbookdays As TextBox = GvRow.FindControl("txtbookdays")
                    Dim txtminnights As TextBox = GvRow.FindControl("txtminnights")
                    Dim ddlapply As DropDownList = GvRow.FindControl("ddlapply")
                    ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                    Dim chkmultiples As CheckBox = GvRow.FindControl("chkmultiples")
                    Dim txtstayfor As TextBox = GvRow.FindControl("txtstayfor")
                    Dim txtpayfor As TextBox = GvRow.FindControl("txtpayfor")
                    Dim txtmaxfreents As TextBox = GvRow.FindControl("txtmaxfreents")
                    Dim txtmaxmultiples As TextBox = GvRow.FindControl("txtmaxmultiples")
                    Dim txtmaxnights As TextBox = GvRow.FindControl("txtmaxnights")

                    Dim txtrmtypname As TextBox = GvRow.FindControl("txtrmtypname")
                    Dim txtuprmtypname As TextBox = GvRow.FindControl("txtuprmtypname")

                    Dim txtmealcode As TextBox = GvRow.FindControl("txtmealcode")
                    Dim txtupmealcode As TextBox = GvRow.FindControl("txtupmealcode")

                    Dim txtrmcatcode As TextBox = GvRow.FindControl("txtrmcatcode")
                    Dim txtuprmcatcode As TextBox = GvRow.FindControl("txtuprmcatcode")

                    Dim txtmealrmcatcode As TextBox = GvRow.FindControl("txtmealrmcatcode")
                    Dim txtupmealrmcatcode As TextBox = GvRow.FindControl("txtupmealrmcatcode")
                    Dim txtrmcombination As TextBox = GvRow.FindControl("txtrmcombination")
                    Dim txtmealcombination As TextBox = GvRow.FindControl("txtmealcombination")

                    If mySqlReader.Read = True Then


                        If IsDBNull(mySqlReader("todate")) = False Then
                            txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                        End If

                        If IsDBNull(mySqlReader("fromdate")) = False Then
                            txtfromdate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")
                        End If

                        If IsDBNull(mySqlReader("bookingcode")) = False Then
                            txthotelbookingcode.Text = mySqlReader("bookingcode")
                        End If

                        If IsDBNull(mySqlReader("discounttype")) = False Then
                            ddloptions.SelectedValue = mySqlReader("discounttype")
                        End If
                        If IsDBNull(mySqlReader("discountamount")) = False Then
                            txtdiscount.Text = mySqlReader("discountamount")
                        End If
                        If IsDBNull(mySqlReader("additionalamount")) = False Then
                            txtadddiscount.Text = mySqlReader("additionalamount")
                        End If
                        If IsDBNull(mySqlReader("roomstring")) = False Then
                            txtnoofrooms.Text = mySqlReader("roomstring")
                        End If
                        If IsDBNull(mySqlReader("multiples")) = False Then
                            chkmultiyes.Checked = mySqlReader("multiples")
                        End If
                        If IsDBNull(mySqlReader("bookingvalidityoptions")) = False Then
                            If mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("Range of dates") Then
                                ddlbookoptions.SelectedIndex = 3
                            ElseIf mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("Book By") Then
                                ddlbookoptions.SelectedIndex = 0
                            ElseIf mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("Book before days from check in") Then
                                ddlbookoptions.SelectedIndex = 1
                            ElseIf mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("Book before months from check in") Then
                                ddlbookoptions.SelectedIndex = 2
                            ElseIf mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("No Booking Validity") Then
                                ddlbookoptions.SelectedIndex = 4
                            ElseIf mySqlReader("bookingvalidityoptions").ToString.ToUpper = UCase("Book By and book before days from check in") Then
                                ddlbookoptions.SelectedIndex = 5

                            End If
                            'ddlbookoptions.SelectedIndex = mySqlReader("bookingvalidityoptions")
                        End If

                        If mySqlReader("bookingvalidityfromdate") <> "" Then
                            txtbookfromDate.Text = IIf(mySqlReader("bookingvalidityfromdate") = "", DBNull.Value, Format(CType(mySqlReader("bookingvalidityfromdate"), Date), "dd/MM/yyyy"))
                        End If

                        '   If IsDBNull(mySqlReader("bookingvaliditytodate")) = False Then
                        If mySqlReader("bookingvaliditytodate") <> "" Then
                            txtBookToDate.Text = IIf(mySqlReader("bookingvaliditytodate") = "", DBNull.Value, Format(CType(mySqlReader("bookingvaliditytodate"), Date), "dd/MM/yyyy"))
                        End If

                        If IsDBNull(mySqlReader("bookingvaliditydaysmonths")) = False Then
                            txtbookdays.Text = IIf(mySqlReader("bookingvaliditydaysmonths") = 0, "", mySqlReader("bookingvaliditydaysmonths"))
                        End If

                        If IsDBNull(mySqlReader("minnights")) = False Then
                            txtminnights.Text = IIf(mySqlReader("minnights") = 0, "", mySqlReader("minnights"))
                        End If
                        If IsDBNull(mySqlReader("maxnights")) = False Then
                            txtmaxnights.Text = IIf(mySqlReader("maxnights") = 0, "", mySqlReader("maxnights"))
                        End If

                        If IsDBNull(mySqlReader("applyto")) = False Then
                            ' ddlapply.SelectedValue = mySqlReader("applyto")

                            If mySqlReader("applyto").ToString.ToUpper = UCase("Every slab of stay") Then
                                ddlapply.SelectedIndex = 0
                            ElseIf mySqlReader("applyto").ToString.ToUpper = UCase("Beginning of stay") Then
                                ddlapply.SelectedIndex = 1
                            ElseIf mySqlReader("applyto").ToString.ToUpper = UCase("End of stay") Then
                                ddlapply.SelectedIndex = 2
                            ElseIf mySqlReader("applyto").ToString.ToUpper = UCase("Cheapest Rate") Then '*** Danny 18/03/2018
                                ddlapply.SelectedIndex = 3

                            End If

                        End If
                        If IsDBNull(mySqlReader("allowmultistay")) = False Then
                            chkmultiples.Checked = mySqlReader("allowmultistay")
                        End If
                        If IsDBNull(mySqlReader("stayfor")) = False Then
                            txtstayfor.Text = IIf(mySqlReader("stayfor") = 0, "", mySqlReader("stayfor"))
                        End If
                        If IsDBNull(mySqlReader("payfor")) = False Then
                            txtpayfor.Text = IIf(mySqlReader("payfor") = 0, "", mySqlReader("payfor"))
                        End If
                        If IsDBNull(mySqlReader("maxfeenights")) = False Then
                            txtmaxfreents.Text = IIf(mySqlReader("maxfeenights") = 0, "", mySqlReader("maxfeenights"))
                        End If
                        If IsDBNull(mySqlReader("maxmultiples")) = False Then
                            txtmaxmultiples.Text = IIf(mySqlReader("maxmultiples") = 0, "", mySqlReader("maxmultiples"))
                        End If

                        If IsDBNull(mySqlReader("roomtypes")) = False Then
                            txtrmtypname.Text = (mySqlReader("roomtypes"))
                        End If
                        If IsDBNull(mySqlReader("updgroomtypes")) = False Then
                            txtuprmtypname.Text = (mySqlReader("updgroomtypes"))
                        End If
                        If IsDBNull(mySqlReader("rmtypcombine")) = False Then
                            txtrmcombination.Text = (mySqlReader("rmtypcombine"))
                        End If

                        If IsDBNull(mySqlReader("mealplan")) = False Then
                            txtmealcode.Text = (mySqlReader("mealplan"))
                        End If
                        If IsDBNull(mySqlReader("updmealplan")) = False Then
                            txtupmealcode.Text = (mySqlReader("updmealplan"))
                        End If
                        If IsDBNull(mySqlReader("mealcombine")) = False Then
                            txtmealcombination.Text = (mySqlReader("mealcombine"))
                        End If

                        If IsDBNull(mySqlReader("rmcatcodes")) = False Then
                            txtrmcatcode.Text = (mySqlReader("rmcatcodes"))
                        End If
                        If IsDBNull(mySqlReader("updrmcatcodes")) = False Then
                            txtuprmcatcode.Text = (mySqlReader("updrmcatcodes"))
                        End If

                        If IsDBNull(mySqlReader("suppcodes")) = False Then
                            txtmealrmcatcode.Text = (mySqlReader("suppcodes"))
                        End If
                        If IsDBNull(mySqlReader("updsuppcodes")) = False Then
                            txtupmealrmcatcode.Text = (mySqlReader("updsuppcodes"))
                        End If








                    End If
                Next


                mySqlReader.Close()
                mySqlConn.Close()
            End If

            Bookingoptionchange()
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
    Protected Sub grdpromotiondetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdpromotiondetail.RowDataBound
        Try



            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dtrow As DataRow = objUtils.fnGridViewRowToDataRow(CType(e.Row, GridViewRow))

                Dim txtfromDate As TextBox
                Dim txtToDate As TextBox



                txtfromDate = e.Row.FindControl("txtfromDate")



                txtToDate = e.Row.FindControl("txtToDate")

                txtToDate.Attributes.Add("onchange", "checkdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")
                txtfromDate.Attributes.Add("onchange", "checkfromdates('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "');")

                txtfromDate.Attributes.Add("onchange", "fillSeasontodate('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "')")
                txtToDate.Attributes.Add("onchange", "ValidateSeasonChkInDate('" & txtfromDate.ClientID & "','" & txtToDate.ClientID & "')")






                If dtrow.ItemArray.Length > 1 Then
                    If Not dtrow.IsNull("FromDate") Then txtfromDate.Text = dtrow("FromDate")
                    If Not dtrow.IsNull("ToDate") Then txtToDate.Text = dtrow("ToDate")
                End If




                Dim ddloptions As DropDownList
                Dim ddlindex As String
                Dim txtminstay As TextBox
                Dim txtdiscount As TextBox
                Dim txtadddiscount As TextBox
                Dim txtbookdays As TextBox

                ddloptions = e.Row.FindControl("ddloptions")
                txtminstay = e.Row.FindControl("txtminnights")
                ddlindex = ddloptions.SelectedIndex
                txtdiscount = e.Row.FindControl("txtdiscount")
                txtadddiscount = e.Row.FindControl("txtadddiscount")
                txtbookdays = e.Row.FindControl("txtbookdays")

                Dim txtmaxmultiples As TextBox = e.Row.FindControl("txtmaxmultiples")
                Dim txtmaxfreents As TextBox = e.Row.FindControl("txtmaxfreents")
                Dim txtpayfor As TextBox = e.Row.FindControl("txtpayfor")
                Dim txtstayfor As TextBox = e.Row.FindControl("txtstayfor")
                Dim txtmaxnights As TextBox = e.Row.FindControl("txtmaxnights")

                Numberssrvctrl(txtstayfor)
                Numberssrvctrl(txtpayfor)
                Numberssrvctrl(txtmaxfreents)
                Numberssrvctrl(txtmaxmultiples)

                Numberssrvctrl(txtminstay)
                Numberssrvctrl(txtdiscount)
                Numberssrvctrl(txtadddiscount)
                Numberssrvctrl(txtbookdays)
                Numberssrvctrl(txtmaxnights)

            End If

            'If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            '    e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
            '    e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
            '    e.Row.Attributes("onselectstart") = "javascript:return false;"
            'End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub




    Private Sub FillroomsGrids()
        Dim myDS As New DataSet
        Dim strsql As String = ""
        'fill roomtypes grid

        gridrmtype.Visible = True

        strsql = "select rmtypcode,rmtypname,rankord from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        gridrmtype.DataSource = myDS
        gridrmtype.DataBind()


        myDS = New DataSet
        strsql = "select p.mealcode as mealcode,m.mealname mealname,mp.rankorder from  partymeal p(nolock),mealmast m(nolock),mainmealplans mp where m.mainmealcode=mp.mainmealcode and  p.mealcode=m.mealcode and p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)"


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        grdmealplan.DataSource = myDS
        grdmealplan.DataBind()


        myDS = New DataSet
        strsql = "select prc.rmcatcode,isnull(rc.units,0) units from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra in ('A','C') and prc.partycode='" _
                        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        grdrmcat.DataSource = myDS
        grdrmcat.DataBind()
        myDS = New DataSet
        strsql = "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and rc.accom_extra in ('M','L') and prc.partycode='" _
                        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)"


        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        myDataAdapter.Fill(myDS)
        grdsupplement.DataSource = myDS
        grdsupplement.DataBind()


        'myDS = New DataSet
        'strsql = "select flightcode from flightmast  where active=1 order by flightcode "

        'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        'myDataAdapter = New SqlDataAdapter(strsql, mySqlConn)
        'myDataAdapter.Fill(myDS)
        'grdflight.DataSource = myDS
        'grdflight.DataBind()

    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getflightlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim flightlist As New List(Of String)

        Try

            ' strSqlQry = "select flightcode,flightcode as flightcodenew from flightmast(nolock)  where active=1 and flightcode like  '" & Trim(prefixText) & "%' order by flightcode "

            strSqlQry = "select flightcode from flightmast(nolock)  where isnull(type,0)=1 and  active=1 and flightcode like  '" & Trim(prefixText) & "%' order by flightcode "




            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter

            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    flightlist.Add(myDS.Tables(0).Rows(i)("flightcode").ToString())
                    'flightlist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("flightcode").ToString(), myDS.Tables(0).Rows(i)("flightcodenew").ToString()))
                Next

            End If

            Return flightlist
        Catch ex As Exception
            Return flightlist
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getpromotionlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim promotionlistnew As New List(Of String)
        Dim maxstate As String
        Dim partycode As String = ""
        Dim promoid As String = ""
        Try

            maxstate = Convert.ToString(HttpContext.Current.Session("OfferState").ToString())
            partycode = Convert.ToString(HttpContext.Current.Session("partycode").ToString())





            If CType(maxstate, String) = "Edit" Then
                promoid = Convert.ToString(HttpContext.Current.Session("OfferRefCode").ToString())
                strSqlQry = "select  promotionname ,promotionid from view_offers_header where  promotionid <>'" & promoid & "' and  partycode='" & partycode & "' and promotionname like  '" & prefixText & "%'"
            Else
                strSqlQry = "select  promotionname ,promotionid from view_offers_header where     partycode='" & partycode & "' and promotionname like  '" & prefixText & "%'"
                ' strSqlQry = "select  promotionname ,promotionid from view_offers_header where   active=1 and convert(varchar(10),fromdate,111) >=GETDATE() and partycode='" & partycode & "' and promotionname like  '" & prefixText & "%'"
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
                    promotionlistnew.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("promotionname").ToString(), myDS.Tables(0).Rows(i)("promotionid").ToString()))
                Next

            End If

            Return promotionlistnew
        Catch ex As Exception
            Return promotionlistnew
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getairportlist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim promotionlist As New List(Of String)
        Dim maxstate As String
        Dim partycode As String = ""
        Try
            ' contextKey = Convert.ToString(HttpContext.Current.Session("partycode").ToString())
            'maxstate = Convert.ToString(HttpContext.Current.Session("OfferState").ToString())
            'partycode = Convert.ToString(HttpContext.Current.Session("partycode").ToString())

            'If CType(maxstate, String) = "Edit" Then
            '    strSqlQry = "select  case when isnull(pricecode,'')='' then isnull(webdesc,'') else pricecode end pricecode ,promotionid from vw_promotion_header where     partycode='" & partycode & "' and case when isnull(pricecode,'')='' then isnull(webdesc,'') else pricecode end like  '" & prefixText & "%'"
            'Else
            '    strSqlQry = "select  case when isnull(pricecode,'')='' then isnull(webdesc,'') else pricecode end pricecode ,promotionid from vw_promotion_header where   active=1 and convert(varchar(10),frmdate,111) >=GETDATE() and partycode='" & partycode & "' and case when isnull(pricecode,'')='' then isnull(webdesc,'') else pricecode end like  '" & prefixText & "%'"
            'End If
            strSqlQry = "select airportbordername,airportbordercode from airportbordersmaster where  airportbordername like  '" & Trim(prefixText) & "%'"
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
                    promotionlist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("airportbordername").ToString(), myDS.Tables(0).Rows(i)("airportbordercode").ToString()))
                Next
            End If
            Return promotionlist
        Catch ex As Exception
            Return promotionlist
        End Try

    End Function





    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getinterhotellist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotellist As New List(Of String)

        Try



            strSqlQry = "select partyname,partycode from  partymast where active=1  and sptypecode=(select option_selected from reservation_parameters where param_id=458) and  partyname like  '" & Trim(prefixText) & "%' order by partyname "


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
                    hotellist.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next

            End If

            Return hotellist
        Catch ex As Exception
            Return hotellist
        End Try

    End Function
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
    Sub promotiontypefill()
        Try
            Dim myDS As New DataSet
            'gv_promotype.Visible = True commented by Elsitta on 13.11.2016
            dlPromotionType.Visible = True
            strSqlQry = ""


            strSqlQry = "select  promtoiontype from promotion_types(nolock)  order by autoid "

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(myDS)
            'gv_promotype.DataSource = myDS 'commented by Elsitta on 13.11.2016
            dlPromotionType.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                ' gv_promotype.DataBind()
                dlPromotionType.DataBind()

            Else
                ' gv_promotype.DataBind()
                dlPromotionType.DataBind()

            End If


            Dim txtpromtoiontype As Label
            Dim chkpromotiontype As CheckBox
            For Each gvRow As DataListItem In dlPromotionType.Items
                chkpromotiontype = gvRow.FindControl("chkpromotiontype")

                txtpromtoiontype = gvRow.FindControl("txtpromtoiontype")

                If ViewState("promotiontypes") Is Nothing = True Then

                    chkpromotiontype.Checked = False

                    If txtpromtoiontype.Text = "Accomodation Upgrade" Then
                        chkpromotiontype.Enabled = False
                    Else
                        chkpromotiontype.Enabled = True
                    End If

                Else

                    If Convert.ToString(ViewState("promotiontypes")).ToUpper.Trim.Contains(txtpromtoiontype.Text.ToUpper.Trim) = False Then

                        chkpromotiontype.Checked = False
                    Else
                        chkpromotiontype.Checked = True
                    End If

                    If txtpromtoiontype.Text = "Accomodation Upgrade" Then
                        chkpromotiontype.Enabled = False
                    Else
                        chkpromotiontype.Enabled = True
                    End If

                End If

            Next

            fillControls()
          
         



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(mySqlConn)                          'Close connection       
        End Try
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Gethotelslist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select partyname, partycode from  partymast where isnull(sptypecode,'') in (select option_selected from reservation_parameters where param_id=458) and active=1 and partyname like  '" & prefixText & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function
    Protected Sub imgStayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim count As Integer
        Dim GVRow As GridViewRow
        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

        count = gvchildage.Rows.Count + 1

        ''  count = grdpromotiondetail.Rows.Count + 1
        Dim flightcode(count) As String

        Dim excl(count) As String
        Dim n As Integer = 0
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim bookingcode(count) As String
        Dim discounttype(count) As String
        Dim discountperc(count) As String
        Dim adddiscountperc(count) As String
        Dim noofrooms(count) As String
        Dim multiyes(count) As String
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim minnights(count) As String
        Dim maxnights(count) As String

        Dim rmtypname(count) As String
        Dim uprmtypname(count) As String
        Dim rmcombination(count) As String

        Dim mealcode(count) As String
        Dim upmealcode(count) As String
        Dim mealcombination(count) As String

        Dim rmcatcode(count) As String
        Dim uprmcatcode(count) As String

        Dim mealrmcatcode(count) As String
        Dim upmealrmcatcode(count) As String

        Dim txtfromdate As TextBox
        Dim txttodate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim txtmaxnights As TextBox

        Dim txtrmtypname As TextBox
        Dim txtuprmtypname As TextBox
        Dim txtrmcombination As TextBox

        Dim txtmealcode As TextBox
        Dim txtupmealcode As TextBox
        Dim txtmealcombination As TextBox

        Dim txtrmcatcode As TextBox
        Dim txtuprmcatcode As TextBox

        Dim txtmealrmcatcode As TextBox
        Dim txtupmealrmcatcode As TextBox




        Dim txtstayfor As TextBox
        Dim txtpayfor As TextBox
        Dim txtmaxfreents As TextBox
        Dim txtmaxmultiples As TextBox
        Dim chkmultiples As CheckBox
        Dim ddlapply As DropDownList

        Dim stay(count) As String
        Dim payfor(count) As String
        Dim maxfreents(count) As String
        Dim maxmultiples(count) As String
        Dim multiples(count) As String
        Dim apply(count) As String






        Try

            For Each GVRow In grdpromotiondetail.Rows
                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")

                txtstayfor = GVRow.FindControl("txtstayfor")
                txtpayfor = GVRow.FindControl("txtpayfor")
                txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                ddlapply = GVRow.FindControl("ddlapply")
                chkmultiples = GVRow.FindControl("chkmultiples")

                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                txtrmcombination = GVRow.FindControl("txtrmcombination")

                txtmealcode = GVRow.FindControl("txtmealcode")
                txtupmealcode = GVRow.FindControl("txtupmealcode")
                txtmealcombination = GVRow.FindControl("txtmealcombination")

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")



                rmtypname(n) = CType(txtrmtypname.Text, String)
                uprmtypname(n) = CType(txtuprmtypname.Text, String)
                rmcombination(n) = CType(txtrmcombination.Text, String)

                mealcode(n) = CType(txtmealcode.Text, String)
                upmealcode(n) = CType(txtupmealcode.Text, String)
                mealcombination(n) = CType(txtmealcombination.Text, String)

                rmcatcode(n) = CType(txtrmcatcode.Text, String)
                uprmcatcode(n) = CType(txtuprmcatcode.Text, String)

                mealrmcatcode(n) = CType(txtmealrmcatcode.Text, String)
                upmealrmcatcode(n) = CType(txtupmealrmcatcode.Text, String)


                stay(n) = CType(txtstayfor.Text, String)
                payfor(n) = CType(txtpayfor.Text, String)
                maxfreents(n) = CType(txtmaxfreents.Text, String)
                maxmultiples(n) = CType(txtmaxmultiples.Text, String)
                apply(n) = CType(ddlapply.SelectedValue, String)

                If chkmultiples.Checked = True Then
                    multiples(n) = 1
                Else
                    multiples(n) = 0
                End If



                fDate(n) = CType(txtfromdate.Text, String)
                tDate(n) = CType(txttodate.Text, String)
                bookingcode(n) = CType(txthotelbookingcode.Text, String)
                discounttype(n) = CType(ddloptions.SelectedValue, String)
                discountperc(n) = CType(txtdiscount.Text, String)
                adddiscountperc(n) = CType(txtadddiscount.Text, String)
                noofrooms(n) = CType(txtnoofrooms.Text, String)
                If chkmultiyes.Checked = True Then
                    multiyes(n) = 1
                Else
                    multiyes(n) = 0
                End If
                bookingvalidity(n) = CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text, String) 'CType(ddlbookoptions.SelectedValue, String)
                bookfdate(n) = CType(txtbookfromDate.Text, String)
                booktdate(n) = CType(txtBookToDate.Text, String)
                bookdays(n) = CType(txtbookdays.Text, String)
                minnights(n) = CType(txtminnights.Text, String)
                maxnights(n) = CType(txtmaxnights.Text, String)

                n = n + 1

            Next

            fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count + 1)

            txtfromdate = grdpromotiondetail.Rows(grdpromotiondetail.Rows.Count - 1).FindControl("txtfromdate")
            txtfromdate.Focus()

            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromotiondetail.Rows
                If n = i Then
                    Exit For
                End If
                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")

                txtstayfor = GVRow.FindControl("txtstayfor")
                txtpayfor = GVRow.FindControl("txtpayfor")
                txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                ddlapply = GVRow.FindControl("ddlapply")

                chkmultiples = GVRow.FindControl("chkmultiples")

                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                txtrmcombination = GVRow.FindControl("txtrmcombination")

                txtmealcode = GVRow.FindControl("txtmealcode")
                txtupmealcode = GVRow.FindControl("txtupmealcode")
                txtmealcombination = GVRow.FindControl("txtmealcombination")

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")



                txtrmtypname.Text = rmtypname(n)
                txtuprmtypname.Text = uprmtypname(n)
                txtrmcombination.Text = rmcombination(n)

                txtmealcode.Text = mealcode(n)
                txtupmealcode.Text = upmealcode(n)
                txtmealcombination.Text = mealcombination(n)

                txtrmcatcode.Text = rmcatcode(n)
                txtuprmcatcode.Text = uprmcatcode(n)

                txtmealrmcatcode.Text = mealrmcatcode(n)
                txtupmealrmcatcode.Text = upmealrmcatcode(n)

                txtfromdate.Text = fDate(n)
                txttodate.Text = tDate(n)
                txthotelbookingcode.Text = bookingcode(n)
                ddloptions.SelectedValue = discounttype(n)
                txtdiscount.Text = discountperc(n)
                txtadddiscount.Text = adddiscountperc(n)
                txtnoofrooms.Text = noofrooms(n)
                If multiyes(n) = 1 Then
                    chkmultiyes.Checked = True
                Else
                    chkmultiyes.Checked = False
                End If
                If bookingvalidity(n).ToString.ToUpper = UCase("Range of dates") Then
                    ddlbookoptions.SelectedIndex = 3
                ElseIf bookingvalidity(n).ToString.ToUpper = UCase("Book By") Then
                    ddlbookoptions.SelectedIndex = 0
                ElseIf bookingvalidity(n).ToString.ToUpper = UCase("Book before days from check in") Then
                    ddlbookoptions.SelectedIndex = 1
                ElseIf bookingvalidity(n).ToString.ToUpper = UCase("Book before months from check in") Then
                    ddlbookoptions.SelectedIndex = 2
                ElseIf bookingvalidity(n).ToString.ToUpper = UCase("No Booking Validity") Then
                    ddlbookoptions.SelectedIndex = 4

                End If

                '  ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text = bookingvalidity(n)
                txtbookfromDate.Text = bookfdate(n)
                txtBookToDate.Text = booktdate(n)
                txtbookdays.Text = bookdays(n)
                txtminnights.Text = minnights(n)
                txtmaxnights.Text = maxnights(n)


                txtstayfor.Text = stay(n)
                txtpayfor.Text = payfor(n)
                txtmaxfreents.Text = maxfreents(n)
                txtmaxmultiples.Text = maxmultiples(n)
                ddlapply.SelectedValue = apply(n)

                If multiples(n) = 1 Then
                    chkmultiples.Checked = True
                Else
                    chkmultiples.Checked = False
                End If



                n = n + 1

            Next

            Bookingoptionchange()

            For Each GVRow In grdpromotiondetail.Rows
                Dim txtminnights1 As TextBox = CType(GVRow.FindControl("txtminnights"), TextBox)

                ddlapply = GVRow.FindControl("ddlapply")
                ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                If Val(txtminnights1.Text) = 0 Then
                    txtminnights1.Text = 1
                End If
            Next


            'Dim gridNewrow As GridViewRow
            'gridNewrow = grdpromotiondetail.Rows(grdpromotiondetail.Rows.Count - 1)
            'Dim strRowId As String = gridNewrow.ClientID
            'Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdpromotiondetail.Rows.Count - 1, String) + "');")
            'ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


            'Dim count As Integer
            'Dim GVRow As GridViewRow

            'Dim gvchildage As GridView

            'gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)

            'count = gvchildage.Rows.Count + 1

            ''count = grdDates.Rows.Count + 1
            'Dim fDate(count) As String
            'Dim tDate(count) As String
            'Dim excl(count) As String
            'Dim n As Integer = 0
            'Dim dpFDate As TextBox
            'Dim dpTDate As TextBox
            'Dim lblseason As Label

            'Try
            '    For Each GVRow In grdDates.Rows
            '        dpFDate = GVRow.FindControl("txtfromDate")
            '        fDate(n) = CType(dpFDate.Text, String)
            '        dpTDate = GVRow.FindControl("txtToDate")
            '        tDate(n) = CType(dpTDate.Text, String)
            '        'lblseason = GVRow.FindControl("lblseason")
            '        'excl(n) = CType(lblseason.Text, String)
            '        n = n + 1
            '    Next
            '    fillDategrd(grdDates, False, grdDates.Rows.Count + 1)
            '    Dim i As Integer = n
            '    n = 0
            '    For Each GVRow In grdDates.Rows
            '        If n = i Then
            '            Exit For
            '        End If
            '        dpFDate = GVRow.FindControl("txtfromDate")
            '        dpFDate.Text = fDate(n)
            '        dpTDate = GVRow.FindControl("txtToDate")
            '        dpTDate.Text = tDate(n)
            '        'lblseason = GVRow.FindControl("lblseason")
            '        'lblseason.Text = excl(n)

            '        n = n + 1
            '    Next


            '    Dim txtStayFromDt As TextBox
            '    txtStayFromDt = TryCast(grdDates.Rows(grdDates.Rows.Count - 1).FindControl("txtfromDate"), TextBox)
            '    txtStayFromDt.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub imgSclose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim count As Integer
        Dim GVRow As GridViewRow



        Dim gvchildage As GridView

        gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        count = gvchildage.Rows.Count + 1

        ' count = grdpromotiondetail.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim bookingcode(count) As String
        Dim discounttype(count) As String
        Dim discountperc(count) As String
        Dim adddiscountperc(count) As String
        Dim noofrooms(count) As String
        Dim multiyes(count) As String
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim minnights(count) As String
        Dim maxnights(count) As String

        Dim rmtypname(count) As String
        Dim uprmtypname(count) As String
        Dim rmcombination(count) As String

        Dim mealcode(count) As String
        Dim upmealcode(count) As String
        Dim mealcombination(count) As String

        Dim rmcatcode(count) As String
        Dim uprmcatcode(count) As String

        Dim mealrmcatcode(count) As String
        Dim upmealrmcatcode(count) As String

        Dim txtfromdate As TextBox
        Dim txttodate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim txtmaxnights As TextBox

        Dim txtrmtypname As TextBox
        Dim txtuprmtypname As TextBox
        Dim txtrmcombination As TextBox

        Dim txtmealcode As TextBox
        Dim txtupmealcode As TextBox
        Dim txtmealcombination As TextBox

        Dim txtrmcatcode As TextBox
        Dim txtuprmcatcode As TextBox

        Dim txtmealrmcatcode As TextBox
        Dim txtupmealrmcatcode As TextBox


        Dim txtstayfor As TextBox
        Dim txtpayfor As TextBox
        Dim txtmaxfreents As TextBox
        Dim txtmaxmultiples As TextBox
        Dim chkmultiples As CheckBox

        Dim stay(count) As String
        Dim payfor(count) As String
        Dim maxfreents(count) As String
        Dim maxmultiples(count) As String
        Dim multiples(count) As String
        Dim apply(count) As String

        Dim ddlapply As DropDownList


        Dim n As Integer = 0


        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdpromotiondetail.Rows
                chkSelect = GVRow.FindControl("chkpromdet")
                '  If chkSelect.Checked = False Then
                If k <> row.RowIndex Then
                    txtfromdate = GVRow.FindControl("txtfromdate")
                    txttodate = GVRow.FindControl("txttodate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    txtmaxnights = GVRow.FindControl("txtmaxnights")

                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                    txtrmcombination = GVRow.FindControl("txtrmcombination")

                    txtmealcode = GVRow.FindControl("txtmealcode")
                    txtupmealcode = GVRow.FindControl("txtupmealcode")
                    txtmealcombination = GVRow.FindControl("txtmealcombination")

                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                    txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                    txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                    txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")

                    txtstayfor = GVRow.FindControl("txtstayfor")
                    txtpayfor = GVRow.FindControl("txtpayfor")
                    txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                    txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                    ddlapply = GVRow.FindControl("ddlapply")
                    ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                    chkmultiples = GVRow.FindControl("chkmultiples")

                    stay(n) = CType(txtstayfor.Text, String)
                    payfor(n) = CType(txtpayfor.Text, String)
                    maxfreents(n) = CType(txtmaxfreents.Text, String)
                    maxmultiples(n) = CType(txtmaxmultiples.Text, String)
                    apply(n) = CType(ddlapply.SelectedValue, String)

                    If chkmultiples.Checked = True Then
                        multiples(n) = 1
                    Else
                        multiples(n) = 0
                    End If


                    rmtypname(n) = CType(txtrmtypname.Text, String)
                    uprmtypname(n) = CType(txtuprmtypname.Text, String)
                    rmcombination(n) = CType(txtrmcombination.Text, String)

                    mealcode(n) = CType(txtmealcode.Text, String)
                    upmealcode(n) = CType(txtupmealcode.Text, String)
                    mealcombination(n) = CType(txtmealcombination.Text, String)

                    rmcatcode(n) = CType(txtrmcatcode.Text, String)
                    uprmcatcode(n) = CType(txtuprmcatcode.Text, String)

                    mealrmcatcode(n) = CType(txtmealrmcatcode.Text, String)
                    upmealrmcatcode(n) = CType(txtupmealrmcatcode.Text, String)


                    fDate(n) = CType(txtfromdate.Text, String)
                    tDate(n) = CType(txttodate.Text, String)
                    bookingcode(n) = CType(txthotelbookingcode.Text, String)
                    discounttype(n) = CType(ddloptions.SelectedValue, String)
                    discountperc(n) = CType(txtdiscount.Text, String)
                    adddiscountperc(n) = CType(txtadddiscount.Text, String)
                    noofrooms(n) = CType(txtnoofrooms.Text, String)
                    If chkmultiyes.Checked = True Then
                        multiyes(n) = 1
                    Else
                        multiyes(n) = 0
                    End If
                    bookingvalidity(n) = CType(ddlbookoptions.SelectedValue, String)
                    bookfdate(n) = CType(txtbookfromDate.Text, String)
                    booktdate(n) = CType(txtBookToDate.Text, String)
                    bookdays(n) = CType(txtbookdays.Text, String)
                    minnights(n) = CType(txtminnights.Text, String)
                    maxnights(n) = CType(txtmaxnights.Text, String)


                    stay(n) = CType(txtstayfor.Text, String)
                    payfor(n) = CType(txtpayfor.Text, String)
                    maxfreents(n) = CType(txtmaxfreents.Text, String)
                    maxmultiples(n) = CType(txtmaxmultiples.Text, String)
                    apply(n) = CType(ddlapply.SelectedValue, String)

                    If chkmultiples.Checked = True Then
                        multiples(n) = 1
                    Else
                        multiples(n) = 0
                    End If

                    n = n + 1


                Else
                    deletedrow = deletedrow + 1
                End If

                k = k + 1
                ' End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If
            'fillDategrd(grdflight, False, grdflight.Rows.Count - deletedrow)

            If grdpromotiondetail.Rows.Count > 1 Then
                fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count - deletedrow)
            Else
                fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromotiondetail.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then

                    txtfromdate = GVRow.FindControl("txtfromdate")
                    txttodate = GVRow.FindControl("txttodate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    txtmaxnights = GVRow.FindControl("txtmaxnights")

                    txtrmtypname = GVRow.FindControl("txtrmtypname")
                    txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                    txtrmcombination = GVRow.FindControl("txtrmcombination")

                    txtmealcode = GVRow.FindControl("txtmealcode")
                    txtupmealcode = GVRow.FindControl("txtupmealcode")
                    txtmealcombination = GVRow.FindControl("txtmealcombination")

                    txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                    txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                    txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                    txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")


                    txtstayfor = GVRow.FindControl("txtstayfor")
                    txtpayfor = GVRow.FindControl("txtpayfor")
                    txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                    txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                    ddlapply = GVRow.FindControl("ddlapply")
                    ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                    chkmultiples = GVRow.FindControl("chkmultiples")



                    txtfromdate.Text = fDate(n)
                    txttodate.Text = tDate(n)
                    txthotelbookingcode.Text = bookingcode(n)
                    ddloptions.SelectedValue = discounttype(n)
                    txtdiscount.Text = discountperc(n)
                    txtadddiscount.Text = adddiscountperc(n)
                    txtnoofrooms.Text = noofrooms(n)
                    If multiyes(n) = 1 Then
                        chkmultiyes.Checked = True
                    Else
                        chkmultiyes.Checked = False
                    End If
                    ddlbookoptions.SelectedValue = bookingvalidity(n)
                    txtbookfromDate.Text = bookfdate(n)
                    txtBookToDate.Text = booktdate(n)
                    txtbookdays.Text = bookdays(n)
                    txtminnights.Text = minnights(n)
                    txtmaxnights.Text = maxnights(n)


                    txtstayfor.Text = stay(n)
                    txtpayfor.Text = payfor(n)
                    txtmaxfreents.Text = maxfreents(n)
                    txtmaxmultiples.Text = maxmultiples(n)
                    ddlapply.SelectedValue = apply(n)
                    If multiples(n) = 1 Then
                        chkmultiples.Checked = True
                    Else
                        chkmultiples.Checked = False
                    End If

                    txtrmtypname.Text = rmtypname(n)
                    txtuprmtypname.Text = uprmtypname(n)
                    txtrmcombination.Text = rmcombination(n)

                    txtmealcode.Text = mealcode(n)
                    txtupmealcode.Text = upmealcode(n)
                    txtmealcombination.Text = mealcombination(n)

                    txtrmcatcode.Text = rmcatcode(n)
                    txtuprmcatcode.Text = uprmcatcode(n)

                    txtmealrmcatcode.Text = mealrmcatcode(n)
                    txtupmealrmcatcode.Text = upmealrmcatcode(n)


                    n = n + 1
                End If
            Next

            Bookingoptionchange()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

        'Dim count As Integer

        'Dim gvchildage As GridView

        'gvchildage = DirectCast(DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow).NamingContainer, GridView)
        'Dim row As GridViewRow = CType((CType(sender, ImageButton)).NamingContainer, GridViewRow)
        'count = gvchildage.Rows.Count + 1

        'Dim GVRow As GridViewRow
        ''  count = grdDates.Rows.Count + 1
        'Dim fDate(count) As String
        'Dim tDate(count) As String
        'Dim excl(count) As String
        'Dim n As Integer = 0
        'Dim dpFDate As TextBox
        'Dim dpTDate As TextBox
        'Dim lblseason As Label

        'Dim delrows(count) As String
        'Dim deletedrow As Integer = 0
        'Dim k As Integer = 0

        'Try
        '    For Each GVRow In grdDates.Rows
        '        '  chkSelect = GVRow.FindControl("chkSelect")
        '        'If chkSelect.Checked = False Then
        '        If k <> row.RowIndex Then
        '            dpFDate = GVRow.FindControl("txtfromDate")
        '            fDate(n) = CType(dpFDate.Text, String)
        '            dpTDate = GVRow.FindControl("txtToDate")
        '            tDate(n) = CType(dpTDate.Text, String)
        '            'lblseason = GVRow.FindControl("lblseason")
        '            'excl(n) = CType(lblseason.Text, String)
        '        Else
        '            deletedrow = deletedrow + 1
        '        End If
        '        n = n + 1
        '        k = k + 1
        '    Next

        '    count = n
        '    If count = 0 Then
        '        count = 1
        '    End If

        '    If grdDates.Rows.Count > 1 Then
        '        fillDategrd(grdDates, False, grdDates.Rows.Count - deletedrow)
        '    Else
        '        fillDategrd(grdDates, False, grdDates.Rows.Count)
        '    End If


        '    Dim i As Integer = n
        '    n = 0
        '    For Each GVRow In grdDates.Rows
        '        If GVRow.RowIndex < count Then
        '            dpFDate = GVRow.FindControl("txtfromDate")
        '            dpFDate.Text = fDate(n)
        '            dpTDate = GVRow.FindControl("txtToDate")
        '            dpTDate.Text = tDate(n)
        '            'lblseason = GVRow.FindControl("lblseason")
        '            'lblseason.Text = excl(n)
        '            n = n + 1
        '        End If
        '    Next

        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        'End Try


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


        dlWeekDays.DataSource = dt

        dlWeekDays.DataBind()

        Dim lblDaysOfWeek As Label
        For Each gvRow As DataListItem In dlWeekDays.Items
            chkSelect = gvRow.FindControl("chkSelect")
            chkAll = dlWeekDays.Controls(0).Controls(0).FindControl("chkAll")
            lblDaysOfWeek = gvRow.FindControl("lblDaysOfWeek")

            If ViewState("daysofweek") Is Nothing = True Then

                chkSelect.Checked = True
                chkAll.Checked = True
            Else

                If Convert.ToString(ViewState("daysofweek")).ToUpper.Trim.Contains(lblDaysOfWeek.Text.ToUpper.Trim) = False Then

                    chkSelect.Checked = False
                Else
                    chkSelect.Checked = True
                End If
            End If

        Next





    End Sub
    Protected Sub btndelflight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelflight.Click

        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdflight.Rows.Count + 1
        Dim flightcode(count) As String


        Dim n As Integer = 0
        Dim txtflightcode As TextBox


        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdflight.Rows
                chkSelect = GVRow.FindControl("chkflight")
                If chkSelect.Checked = False Then
                    txtflightcode = GVRow.FindControl("txtflightcode")
                    flightcode(n) = CType(txtflightcode.Text, String)

                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If
            'fillDategrd(grdflight, False, grdflight.Rows.Count - deletedrow)

            If grdflight.Rows.Count > 1 Then
                fillDategrd(grdflight, False, grdflight.Rows.Count - deletedrow)
            Else
                fillDategrd(grdflight, False, grdflight.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdflight.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then

                    txtflightcode = GVRow.FindControl("txtflightcode")
                    txtflightcode.Text = flightcode(n)


                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btndelcombine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelcombine.Click

        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdcombinable.Rows.Count + 1
        Dim promotioncode(count) As String
        Dim promotionname(count) As String

        Dim n As Integer = 0
        Dim txtpromotioncode As TextBox
        Dim txtpromotionname As TextBox
        Dim ddlExcl As HtmlSelect
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdcombinable.Rows
                chkSelect = GVRow.FindControl("chkcombine")
                If chkSelect.Checked = False Then
                    txtpromotioncode = GVRow.FindControl("txtpromotioncode")
                    promotioncode(n) = CType(txtpromotioncode.Text, String)
                    txtpromotionname = GVRow.FindControl("txtpromotionname")
                    promotionname(n) = CType(txtpromotionname.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdcombinable.Rows.Count > 1 Then
                fillDategrd(grdcombinable, False, grdcombinable.Rows.Count - deletedrow)
            Else
                fillDategrd(grdcombinable, False, grdcombinable.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdcombinable.Rows
                If GVRow.RowIndex < count Then
                    txtpromotioncode = GVRow.FindControl("txtpromotioncode")
                    txtpromotioncode.Text = promotioncode(n)
                    txtpromotionname = GVRow.FindControl("txtpromotionname")
                    txtpromotionname.Text = promotionname(n)

                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btndeletehotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeletehotel.Click

        'Createdatacolumns("delete")
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdinterhotel.Rows.Count + 1
        Dim interhotelcode(count) As String
        Dim interhotelname(count) As String
        Dim interminnights(count) As String

        Dim n As Integer = 0
        Dim txtinterhotelcode As TextBox
        Dim txtinterhotelname As TextBox
        Dim txtinterminnights As TextBox

        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdinterhotel.Rows
                chkSelect = GVRow.FindControl("chkinterSelect")
                If chkSelect.Checked = False Then
                    txtinterhotelcode = GVRow.FindControl("txtinterhotelcode")
                    interhotelcode(n) = CType(txtinterhotelcode.Text, String)
                    txtinterhotelname = GVRow.FindControl("txtinterhotelname")
                    interhotelname(n) = CType(txtinterhotelname.Text, String)
                    txtinterminnights = GVRow.FindControl("txtinterminnights")
                    interminnights(n) = CType(txtinterminnights.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If

            If grdinterhotel.Rows.Count > 1 Then
                fillDategrd(grdinterhotel, False, grdinterhotel.Rows.Count - deletedrow)
            Else
                fillDategrd(grdinterhotel, False, grdinterhotel.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdinterhotel.Rows
                If GVRow.RowIndex < count Then
                    txtinterhotelcode = GVRow.FindControl("txtinterhotelcode")
                    txtinterhotelcode.Text = interhotelcode(n)
                    txtinterhotelname = GVRow.FindControl("txtinterhotelname")
                    txtinterhotelname.Text = interhotelname(n)
                    txtinterminnights = GVRow.FindControl("txtinterminnights")
                    txtinterminnights.Text = interminnights(n)

                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    'Protected Sub btndeletedates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeletedates.Click

    '    'Createdatacolumns("delete")
    '    Dim count As Integer
    '    Dim GVRow As GridViewRow
    '    count = grdpromodates.Rows.Count + 1
    '    Dim fDate(count) As String
    '    Dim tDate(count) As String
    '    Dim excl(count) As String
    '    Dim n As Integer = 0
    '    Dim dpFDate As TextBox
    '    Dim dpTDate As TextBox
    '    Dim ddlExcl As HtmlSelect
    '    Dim chkSelect As CheckBox
    '    Dim delrows(count) As String
    '    Dim deletedrow As Integer = 0
    '    Dim k As Integer = 0

    '    Try
    '        For Each GVRow In grdpromodates.Rows
    '            chkSelect = GVRow.FindControl("chkSelect")
    '            If chkSelect.Checked = False Then
    '                dpFDate = GVRow.FindControl("txtfromDate")
    '                fDate(n) = CType(dpFDate.Text, String)
    '                dpTDate = GVRow.FindControl("txtToDate")
    '                tDate(n) = CType(dpTDate.Text, String)
    '                n = n + 1
    '            Else
    '                deletedrow = deletedrow + 1
    '            End If

    '        Next

    '        count = n
    '        If count = 0 Then
    '            count = 1
    '        End If

    '        If grdpromodates.Rows.Count > 1 Then
    '            fillDategrd(grdpromodates, False, grdpromodates.Rows.Count - deletedrow)
    '        Else
    '            fillDategrd(grdpromodates, False, grdpromodates.Rows.Count)
    '        End If


    '        Dim i As Integer = n
    '        n = 0
    '        For Each GVRow In grdpromodates.Rows
    '            If GVRow.RowIndex < count Then
    '                dpFDate = GVRow.FindControl("txtfromDate")
    '                dpFDate.Text = fDate(n)
    '                dpTDate = GVRow.FindControl("txtToDate")
    '                dpTDate.Text = tDate(n)

    '                n = n + 1
    '            End If
    '        Next

    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
    '    End Try
    'End Sub

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSeasonlist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select distinct subseasname from contracts_seasons"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString())
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function
    Protected Sub btnaddflight_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddflight.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdflight.Rows.Count + 1
        Dim flightcode(count) As String

        Dim excl(count) As String
        Dim n As Integer = 0
        Dim txtflightcode As TextBox
        Dim txtinterhotelname As TextBox
        Dim ddlExcl As HtmlSelect

        Try
            For Each GVRow In grdflight.Rows
                txtflightcode = GVRow.FindControl("txtflightcode")
                flightcode(n) = CType(txtflightcode.Text, String)


                n = n + 1
            Next
            fillDategrd(grdflight, False, grdflight.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdflight.Rows
                If n = i Then
                    Exit For
                End If
                txtflightcode = GVRow.FindControl("txtflightcode")
                txtflightcode.Text = flightcode(n)


                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btnaddcombine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddcombine.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdcombinable.Rows.Count + 1
        Dim promotioncode(count) As String
        Dim promotionname(count) As String

        Dim n As Integer = 0
        Dim txtpromotioncode As TextBox
        Dim txtpromotionname As TextBox


        Try
            For Each GVRow In grdcombinable.Rows
                txtpromotioncode = GVRow.FindControl("txtpromotioncode")
                promotioncode(n) = CType(txtpromotioncode.Text, String)
                txtpromotionname = GVRow.FindControl("txtpromotionname")
                promotionname(n) = CType(txtpromotionname.Text, String)

                n = n + 1
            Next
            fillDategrd(grdcombinable, False, grdcombinable.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdcombinable.Rows
                If n = i Then
                    Exit For
                End If
                txtpromotioncode = GVRow.FindControl("txtpromotioncode")
                txtpromotioncode.Text = promotioncode(n)
                txtpromotionname = GVRow.FindControl("txtpromotionname")
                txtpromotionname.Text = promotionname(n)

                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btnaddhotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddhotel.Click


        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdinterhotel.Rows.Count + 1
        Dim interhotelcode(count) As String
        Dim interhotelname(count) As String
        Dim interminnights(count) As String

        Dim excl(count) As String
        Dim n As Integer = 0
        Dim txtinterhotelcode As TextBox
        Dim txtinterhotelname As TextBox
        Dim txtinterminnights As TextBox
        Dim ddlExcl As HtmlSelect

        Try
            For Each GVRow In grdinterhotel.Rows
                txtinterhotelcode = GVRow.FindControl("txtinterhotelcode")
                interhotelcode(n) = CType(txtinterhotelcode.Text, String)
                txtinterhotelname = GVRow.FindControl("txtinterhotelname")
                interhotelname(n) = CType(txtinterhotelname.Text, String)
                txtinterminnights = GVRow.FindControl("txtinterminnights")
                interminnights(n) = CType(txtinterminnights.Text, String)

                n = n + 1
            Next
            fillDategrd(grdinterhotel, False, grdinterhotel.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdinterhotel.Rows
                If n = i Then
                    Exit For
                End If
                txtinterhotelcode = GVRow.FindControl("txtinterhotelcode")
                txtinterhotelcode.Text = interhotelcode(n)
                txtinterhotelname = GVRow.FindControl("txtinterhotelname")
                txtinterhotelname.Text = interhotelname(n)
                txtinterminnights = GVRow.FindControl("txtinterminnights")
                txtinterminnights.Text = interminnights(n)

                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    'Protected Sub btnAddLinesDates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLinesDates.Click
    '    '  Createdatacolumns("add")

    '    Dim count As Integer
    '    Dim GVRow As GridViewRow
    '    count = grdpromodates.Rows.Count + 1
    '    Dim fDate(count) As String
    '    Dim tDate(count) As String
    '    Dim excl(count) As String
    '    Dim n As Integer = 0
    '    Dim dpFDate As TextBox
    '    Dim dpTDate As TextBox
    '    Dim ddlExcl As HtmlSelect

    '    Try
    '        For Each GVRow In grdpromodates.Rows
    '            dpFDate = GVRow.FindControl("txtfromDate")
    '            fDate(n) = CType(dpFDate.Text, String)
    '            dpTDate = GVRow.FindControl("txtToDate")
    '            tDate(n) = CType(dpTDate.Text, String)

    '            n = n + 1
    '        Next
    '        fillDategrd(grdpromodates, False, grdpromodates.Rows.Count + 1)
    '        Dim i As Integer = n
    '        n = 0
    '        For Each GVRow In grdpromodates.Rows
    '            If n = i Then
    '                Exit For
    '            End If
    '            dpFDate = GVRow.FindControl("txtfromDate")
    '            dpFDate.Text = fDate(n)
    '            dpTDate = GVRow.FindControl("txtToDate")
    '            dpTDate.Text = tDate(n)

    '            n = n + 1
    '        Next

    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
    '    End Try
    'End Sub

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        If CType(Session("OfferState"), String) = "View" Or CType(Session("OfferState"), String) = "Delete" Then
            txtpromotionid.Enabled = False
            txthotelname.Enabled = False
            txtApplicableTo.Enabled = False


            wucCountrygroup.Disable(False)

            grdpromotiondetail.Enabled = False
            dlPromotionType.Enabled = False
            dlWeekDays.Enabled = False
            gridrmtype.Enabled = False
            grdmealplan.Enabled = False
            grdrmcat.Enabled = False
            grdsupplement.Enabled = False
            grdinterhotel.Enabled = False
            grdArrivalTransfer.Enabled = False
            grdDepartureTransfer.Enabled = False
            grdflight.Enabled = False
            txtremarks.Disabled = True
            txtsplocc.Disabled = True
            chkactive.Enabled = False
            chkarrival.Enabled = False
            chkdeparture.Enabled = False
            chkdiscount.Enabled = False
            chkrefund.Enabled = False
            ddlinventorytype.Disabled = True
            ddlcombine.Disabled = True
            txtpromotionname.Enabled = False


        Else
            txthotelname.Enabled = False
            txtApplicableTo.Enabled = True
            wucCountrygroup.Disable(True)


            grdpromotiondetail.Enabled = True
            dlPromotionType.Enabled = True
            dlWeekDays.Enabled = True
            gridrmtype.Enabled = True
            grdmealplan.Enabled = True
            grdrmcat.Enabled = True
            grdsupplement.Enabled = True
            grdinterhotel.Enabled = True
            grdArrivalTransfer.Enabled = True
            grdDepartureTransfer.Enabled = True
            grdflight.Enabled = True
            txtremarks.Disabled = False
            txtsplocc.Disabled = False

            chkactive.Enabled = True
            chkarrival.Enabled = True
            chkdeparture.Enabled = True
            chkdiscount.Enabled = True
            chkrefund.Enabled = True

            ddlinventorytype.Disabled = False
            ddlcombine.Disabled = False
            txtpromotionname.Enabled = True

        End If


    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select a.partyname,c.promotionname,c.applicableto,c.promotionid,c.partycode,c.approved,c.daysoftheweek,c.promotiontypes,c.remarks, " _
                                      & " c.active,c.nonrefundable,c.applytdiscountoexhibition,c.arrivaltransfer,c.departuretransfer,c.specialoccassion,c.inventorytype,c.combinetype,c.commissiontype,isnull(c.applydiscounttype,'') applydiscounttype, " _
                                      & " isnull(c.applydiscountids,'') applydiscountids,isnull(c.shortname,'') shortname from view_offers_header c(nolock),partymast a  Where c.partycode=a.partycode and c.promotionid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("partyname")) = False Then
                        txthotelname.Text = mySqlReader("partyname")
                        hdnpartycode.Value = mySqlReader("partycode")
                        SubMenuUserControl1.partyval = hdnpartycode.Value
                       
                        If Session("Contractparty") = "" Or Session("Contractparty") Is Nothing Then
                            Session("Contractparty") = hdnpartycode.Value

                        End If
                        If Session("partycode") = "" Or Session("partycode") Is Nothing Then
                            Session("partycode") = hdnpartycode.Value
                        End If

                    End If


                    If IsDBNull(mySqlReader("applicableto")) = False Then
                        txtApplicableTo.Text = mySqlReader("applicableto")
                    End If
                    If IsDBNull(mySqlReader("daysoftheweek")) = False Then
                        ViewState("daysofweek") = mySqlReader("daysoftheweek")
                    End If
                    If IsDBNull(mySqlReader("promotiontypes")) = False Then
                        ViewState("promotiontypes") = mySqlReader("promotiontypes")
                    End If

                    If IsDBNull(mySqlReader("promotionname")) = False Then
                        txtpromotionname.Text = mySqlReader("promotionname")
                    End If

                    If IsDBNull(mySqlReader("shortname")) = False Then
                        txtshortname.Text = mySqlReader("shortname")
                    End If

                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtremarks.Value = mySqlReader("remarks")
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        chkactive.Checked = mySqlReader("active")
                    End If


                    If IsDBNull(mySqlReader("nonrefundable")) = False Then
                        chkrefund.Checked = mySqlReader("nonrefundable")
                    End If

                    If IsDBNull(mySqlReader("applytdiscountoexhibition")) = False Then
                        chkdiscount.Checked = mySqlReader("applytdiscountoexhibition")
                    End If

                    If IsDBNull(mySqlReader("arrivaltransfer")) = False Then
                        chkarrival.Checked = mySqlReader("arrivaltransfer")
                    End If

                    If IsDBNull(mySqlReader("departuretransfer")) = False Then
                        chkdeparture.Checked = mySqlReader("departuretransfer")
                    End If

                    If IsDBNull(mySqlReader("specialoccassion")) = False Then
                        txtsplocc.Value = mySqlReader("specialoccassion")
                    End If

                    If IsDBNull(mySqlReader("inventorytype")) = False Then
                        ddlinventorytype.Value = mySqlReader("inventorytype")
                    End If

                    If IsDBNull(mySqlReader("combinetype")) = False Then
                        ddlcombine.Value = mySqlReader("combinetype")
                    End If


                    If IsDBNull(mySqlReader("commissiontype")) = False Then
                        ddlcommission.Value = mySqlReader("commissiontype")
                    End If

                    If IsDBNull(mySqlReader("applydiscounttype")) = False Then
                        ddlapplydiscount.Value = mySqlReader("applydiscounttype")
                    End If

                    If IsDBNull(mySqlReader("applydiscountids")) = False Then
                        hdnapplydiscount.Value = mySqlReader("applydiscountids")
                    End If
                    If hdnapplydiscount.Value <> "" Then
                        btnselectcontract.Style.Add("display", "block")
                        divapplydiscount.Style.Add("display", "block")
                        'Else
                        '    btnselectcontract.Style.Add("display", "none")
                        '    divapplydiscount.Style.Add("display", "none")
                    End If

                    If IsDBNull(mySqlReader("promotionid")) = False Then
                        txtpromotionid.Text = mySqlReader("promotionid")
                        SubMenuUserControl1.contractval = txtpromotionid.Text


                    End If
                    If IsDBNull(mySqlReader("approved")) = False Then
                        lblstatustext.Visible = True
                        lblstatus.Visible = True
                        lblstatus.Text = IIf(mySqlReader("approved") = 1, "APPROVED", "UNAPPROVED")


                    End If


                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region
    Private Function validatepromotionEntry() As Boolean

        Dim MyAdapter As SqlDataAdapter
        Try

            ''' To check room Types
            strSqlQry = "select * from view_promotionroom(nolock) where promotionid='" & txtpromotionid.Text & "'"


            Dim MyGrpDS As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.SelectCommand.CommandTimeout = 0
            MyAdapter.Fill(MyGrpDS, "view_romtypes")

            Dim chkrmtype As CheckBox

            If MyGrpDS.Tables(0).Rows.Count > 0 Then

                For Each GVRow1 In gridrmtype.Rows
                    chkrmtype = GVRow1.FindControl("chkrmtype")
                    Dim txtrmtypcode As Label = GVRow1.FindControl("txtrmtypcode")
                    If chkrmtype.Checked = False Then
                        For i As Integer = 0 To MyGrpDS.Tables(0).Rows.Count - 1

                            If txtrmtypcode.Text = MyGrpDS.Tables(0).Rows(i).Item("rmtypcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + GVRow1.Cells(2).Text + " already used in  " + MyGrpDS.Tables(0).Rows(i).Item("Page") + " ')", True)

                                validatepromotionEntry = False
                                Exit Function


                            End If
                        Next

                    End If
                Next
            Else
                validatepromotionEntry = True
            End If

            ''''''''''''''''''''''''''

            ''' To check meal plans
            strSqlQry = "select * from view_promotionmeals(nolock) where promotionid='" & txtpromotionid.Text & "'"


            Dim MyGrpDSmeal As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.SelectCommand.CommandTimeout = 0
            MyAdapter.Fill(MyGrpDSmeal, "viewmeals")

            Dim chkrmtype1 As CheckBox

            If MyGrpDSmeal.Tables(0).Rows.Count > 0 Then




              

                For Each gvrow1 In grdmealplan.Rows
                    Dim chkmeal As CheckBox = gvrow1.FindControl("chkmeal")
                    Dim txtmealcode As Label = gvrow1.FindControl("txtmealcode")
                    If chkmeal.Checked = False Then
                        For i As Integer = 0 To MyGrpDSmeal.Tables(0).Rows.Count - 1

                            If txtmealcode.Text = MyGrpDSmeal.Tables(0).Rows(i).Item("mealcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + gvrow1.Cells(2).Text + " already used in  " + MyGrpDSmeal.Tables(0).Rows(i).Item("Page") + " ')", True)

                                validatepromotionEntry = False
                                Exit Function


                            End If
                        Next

                    End If
                Next
            Else
                validatepromotionEntry = True
            End If

            ''''''''''''''''''''''''''

            ''' To check accomodation
            strSqlQry = "select * from view_promotionaccomodation(nolock) where promotionid='" & txtpromotionid.Text & "'"


            Dim MyGrpDSaccom As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.SelectCommand.CommandTimeout = 0
            MyAdapter.Fill(MyGrpDSaccom, "viewaccom")



            If MyGrpDSaccom.Tables(0).Rows.Count > 0 Then





                For Each gvrow1 In grdrmcat.Rows
                    Dim chkrmcat As CheckBox = gvrow1.FindControl("chkrmcat")
                    Dim txtrmcatcode As Label = gvrow1.FindControl("txtrmcatcode")
                    If chkrmcat.Checked = False Then
                        For i As Integer = 0 To MyGrpDSaccom.Tables(0).Rows.Count - 1

                            If txtrmcatcode.Text = MyGrpDSaccom.Tables(0).Rows(i).Item("rmcatcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + gvrow1.Cells(2).Text + " already used in  " + MyGrpDSaccom.Tables(0).Rows(i).Item("Page") + " ')", True)

                                validatepromotionEntry = False
                                Exit Function


                            End If
                        Next

                    End If
                Next
            Else
                validatepromotionEntry = True
            End If

            ''''''''''''''''''''''''''

            ''' To check meal supplements
            strSqlQry = "select * from view_promotionmealcategory(nolock) where promotionid='" & txtpromotionid.Text & "'"


            Dim MyGrpDSsupp As New DataSet
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            MyAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            MyAdapter.Fill(MyGrpDSsupp, "viewsupp")



            If MyGrpDSsupp.Tables(0).Rows.Count > 0 Then

                For Each gvrow1 In grdsupplement.Rows
                    Dim chksuprmcat As CheckBox = gvrow1.FindControl("chksuprmcat")
                    Dim lblrmcatcode As Label = gvrow1.FindControl("lblrmcatcode")
                    If chksuprmcat.Checked = False Then
                        For i As Integer = 0 To MyGrpDSsupp.Tables(0).Rows.Count - 1

                            If lblrmcatcode.Text = MyGrpDSsupp.Tables(0).Rows(i).Item("rmcatcode") Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' You are not allowed to uncheck " + lblrmcatcode.Text + " already used in  " + MyGrpDSsupp.Tables(0).Rows(i).Item("Page") + " ')", True)

                                validatepromotionEntry = False
                                Exit Function


                            End If
                        Next

                    End If
                Next
            Else
                    validatepromotionEntry = True
            End If

            ''''''''''''''''''''''''''


            validatepromotionEntry = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Private Function validatepromotiondetails() As Boolean


        Dim lnCnt As Integer = 0
        Dim txtfromdate As TextBox
        Dim txtToDate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim ddlapply As DropDownList
        Dim chkmultiples As CheckBox
        Dim txtstayfor As TextBox
        Dim txtpayfor As TextBox
        Dim txtmaxfreents As TextBox
        Dim txtmaxmultiples As TextBox
        Dim txtmaxnights As TextBox

        Dim txtrmtypname As TextBox

        Dim txtmealcode As TextBox
        Dim txtrmcatcode As TextBox
        Dim txtuprmtypname As TextBox
        Dim txtupmealcode As TextBox

        Dim checkedrow As Integer = 0
        Dim promoList As String = String.Empty
        Dim count As Integer = 0
        For Each item As DataListItem In dlPromotionType.Items
            Dim chkRow As CheckBox = TryCast(item.FindControl("chkpromotiontype"), CheckBox)
            Dim txtbox As Label = TryCast(item.FindControl("txtpromtoiontype"), Label)
            If chkRow.Checked = True Then
                Dim lbltype As String = txtbox.Text
                'item.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
                Dim strpop As String = ""
                checkedrow = checkedrow + 1
                If count = 0 Then
                    promoList = lbltype
                    count = 1
                Else
                    promoList &= "|" & lbltype
                End If
            End If
        Next

        If promoList.Length = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast Promotion Type.');", True)
            SetFocus(gridrmtype)
            validatepromotiondetails = False
            Exit Function

        End If


        If ddlapplydiscount.SelectedIndex = 0 And UCase(promoList).ToUpper.Trim.Contains(UCase("Special Rates").ToUpper.Trim) = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Apply Offer To Options ');", True)

            validatepromotiondetails = False
            Exit Function
        End If

        If ddlapplydiscount.SelectedIndex <> 0 And hdnapplydiscount.Value = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Apply Offer to Contracts or Offers ');", True)
            btnselectcontract.Style.Add("display", "block")
            validatepromotiondetails = False
            Exit Function
        End If

        If UCase(promoList).ToUpper.Trim.Contains(UCase("Special Rates").ToUpper.Trim) = True Then
            If ddlapplydiscount.SelectedIndex <> 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Special Rates Promotion selected  Apply offer should be None ');", True)
                btnselectcontract.Style.Add("display", "block")
                validatepromotiondetails = False
                Exit Function
            End If
        End If

        If UCase(promoList).ToUpper.Trim.Contains(UCase("Special Rates").ToUpper.Trim) = False Then

            Dim ratesexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_cplisthnew(nolock) where promotionid='" & txtpromotionid.Text & "'")

            If ratesexists <> "" And txtpromotionid.Text <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Special Rates already Entered For this Promotion Please select Special Rates');", True)

                validatepromotiondetails = False
                Exit Function
            End If

          

        End If
        'If UCase(promoList).ToUpper.Trim.Contains(UCase("Early Bird Discount").ToUpper.Trim) = True Then
        '    'If ddlapplydiscount.SelectedIndex = 0 Then
        '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Apply discount Options ');", True)
        '    '    validatepromotiondetails = False
        '    '    Exit Function
        '    'End If


        'End If

        If ddlcommission.SelectedIndex <> 2 Then

            Dim ratesexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_commission_header(nolock) where promotionid='" & txtpromotionid.Text & "'")

            If ratesexists <> "" And txtpromotionid.Text <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Commission already Entered For this Promotion Please select Special Commissionable');", True)

                validatepromotiondetails = False
                Exit Function
            End If


        End If





        For Each GVRow In grdpromotiondetail.Rows
            lnCnt += 1

            txtfromdate = GVRow.FindControl("txtfromDate")
            txtToDate = GVRow.FindControl("txtToDate")
            txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
            ddloptions = GVRow.FindControl("ddloptions")
            txtdiscount = GVRow.FindControl("txtdiscount")
            txtadddiscount = GVRow.FindControl("txtadddiscount")
            txtnoofrooms = GVRow.FindControl("txtnoofrooms")
            chkmultiyes = GVRow.FindControl("chkmultiyes")
            ddlbookoptions = GVRow.FindControl("ddlbookoptions")
            txtbookfromDate = GVRow.FindControl("txtbookfromDate")
            txtBookToDate = GVRow.FindControl("txtBookToDate")
            txtbookdays = GVRow.FindControl("txtbookdays")
            txtminnights = GVRow.FindControl("txtminnights")
            ddlapply = GVRow.FindControl("ddlapply")

            chkmultiples = GVRow.FindControl("chkmultiples")
            txtstayfor = GVRow.FindControl("txtstayfor")
            txtpayfor = GVRow.FindControl("txtpayfor")
            txtmaxfreents = GVRow.FindControl("txtmaxfreents")
            txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
            txtmaxnights = GVRow.FindControl("txtmaxnights")

            txtrmtypname = GVRow.FindControl("txtrmtypname")
            txtuprmtypname = GVRow.FindControl("txtuprmtypname")

            txtmealcode = GVRow.FindControl("txtmealcode")

            txtupmealcode = GVRow.FindControl("txtupmealcode")
            txtrmcatcode = GVRow.FindControl("txtrmcatcode")


            '*** Danny 19/03/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            If IsNothing(Session("1002")) Then ''*** Danny 1002 is Excel Document number 
                Session("1002") = String.Empty
            End If
            If Session("1002").ToString() <> "SHOW" Then
                If ddlapply.SelectedValue = 3 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Apply To value is invalid.  ');", True)
                    validatepromotiondetails = False
                    SetFocus(ddlapply)
                    Exit Function
                End If
            End If
            '*** Danny 19/03/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<




            If txtfromdate.Text <> "" And txtToDate.Text <> "" Then
                'If txthotelbookingcode.Text = "" Then

                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Booking Code Row No :" & lnCnt & "');", True)
                '    txthotelbookingcode.Focus()
                '    validatepromotiondetails = False
                '    Exit Function

                'End If

                If txtrmtypname.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Room Type Row No :" & lnCnt & "');", True)
                    validatepromotiondetails = False
                    Exit Function
                End If

                If txtmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Meal  Row No :" & lnCnt & "');", True)
                    validatepromotiondetails = False
                    Exit Function
                End If

                If txtrmcatcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Accomodation Category  Row No :" & lnCnt & "');", True)
                    validatepromotiondetails = False
                    Exit Function
                End If

                If Left(Right(txtfromdate.Text, 4), 2) <> "20" Or Left(Right(txtToDate.Text, 4), 2) <> "20" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter From Date and To Date Belongs to 21 st century  ');", True)
                    validatepromotiondetails = False
                    SetFocus(txtfromdate)
                    Exit Function
                End If

                If UCase(promoList).ToUpper.Trim.Contains(UCase("Room Upgrade").ToUpper.Trim) = True And txtuprmtypname.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Upgrade Room Type Row No :" & lnCnt & "');", True)
                    validatepromotiondetails = False
                    Exit Function
                End If

                If UCase(promoList).ToUpper.Trim.Contains(UCase("Meal Upgrade").ToUpper.Trim) = True And txtupmealcode.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Upgrade Meal Plan Row No :" & lnCnt & "');", True)
                    validatepromotiondetails = False
                    Exit Function
                End If


                If CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text.ToUpper, String) = ("Book By").ToUpper And txtbookfromDate.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Book By Date  Row No :" & lnCnt & "');", True)
                    txtbookfromDate.Focus()
                    validatepromotiondetails = False
                    Exit Function
                ElseIf CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text.ToUpper, String) = ("Book before days from check in").ToUpper And txtbookdays.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Book By Days  Row No :" & lnCnt & "');", True)
                    txtbookfromDate.Focus()
                    validatepromotiondetails = False
                    Exit Function
                ElseIf CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text.ToUpper, String) = ("Book before months from check in").ToUpper And txtbookdays.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Book By Months  Row No :" & lnCnt & "');", True)
                    txtbookfromDate.Focus()
                    validatepromotiondetails = False
                    Exit Function
                ElseIf CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text.ToUpper, String) = ("Range of dates").ToUpper And txtbookfromDate.Text = "" And txtBookToDate.Text = "" Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select  Range Of Dates Row No :" & lnCnt & "');", True)
                    txtbookfromDate.Focus()
                    validatepromotiondetails = False
                    Exit Function
                End If
            Else

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Promotion From and To Dates Row No :" & lnCnt & "');", True)
                txtfromdate.Focus()
                validatepromotiondetails = False
                Exit Function
            End If

        Next




        Dim flgCheck As Boolean = False
        Dim chkrmtype As CheckBox
        'For Each gvRow In gridrmtype.Rows
        '    chkrmtype = gvRow.FindControl("chkrmtype")
        '    If chkrmtype.Checked = True Then
        '        flgCheck = True
        '        Exit For
        '    End If
        'Next
        'If flgCheck = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one Room type.');", True)
        '    SetFocus(gridrmtype)
        '    validatepromotiondetails = False
        '    Exit Function
        'End If

        'flgCheck = False
        'For Each gvRow In grdmealplan.Rows
        '    chkrmtype = gvRow.FindControl("chkmeal")
        '    If chkrmtype.Checked = True Then
        '        flgCheck = True
        '        Exit For
        '    End If
        'Next
        'If flgCheck = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one Meal Plan.');", True)
        '    SetFocus(grdmealplan)
        '    validatepromotiondetails = False
        '    Exit Function
        'End If

        'flgCheck = False
        'For Each gvRow In grdrmcat.Rows
        '    chkrmtype = gvRow.FindControl("chkrmcat")
        '    If chkrmtype.Checked = True Then
        '        flgCheck = True
        '        Exit For
        '    End If
        'Next
        'If flgCheck = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one Accoomodations.');", True)
        '    SetFocus(grdrmcat)
        '    validatepromotiondetails = False
        '    Exit Function
        'End If








        Session("promolist") = Nothing

        Dim strCondition As String = ""
        If promoList.Length <> 0 Then
            Session("promolist") = promoList
            Dim mString As String() = promoList.Split("|")
            '  For i As Integer = 0 To mString.Length - 1

            'If UCase(mString(i)).ToUpper.Trim = UCase("Room Upgrade") Then

            If UCase(promoList).ToUpper.Trim.Contains(UCase("Early Bird Discount").ToUpper.Trim) = True And UCase(promoList).ToUpper.Trim.Contains(UCase("Special Rates").ToUpper.Trim) = True Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Early Discount and Special Rates are Selected Please select any one.');", True)
                SetFocus(gridrmtype)
                validatepromotiondetails = False
                Exit Function
            End If


            If UCase(promoList).ToUpper.Trim.Contains(UCase("Room Upgrade").ToUpper.Trim) = True Then


                'flgCheck = False
                'For Each gvRow In gridrmtype.Rows
                '    chkrmtype = gvRow.FindControl("chkrmtype")
                '    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgradermtyp")

                '    If (ddlUpgradermtyp.Value <> "" And ddlUpgradermtyp.Value <> "[Select]") Then
                '        flgCheck = True
                '        Exit For
                '    End If
                'Next

                'If flgCheck = False Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Room Upgrade Promotion Selected  Please select Room Type to upgrade.');", True)
                '    SetFocus(gridrmtype)
                '    validatepromotiondetails = False
                '    Exit Function
                'End If
                'For Each gvRow In gridrmtype.Rows
                '    chkrmtype = gvRow.FindControl("chkrmtype")
                '    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgradermtyp")

                '    If (ddlUpgradermtyp.Value = "[Select]") And chkrmtype.Checked = True Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Please select Room Type to upgrade.');", True)
                '        SetFocus(gridrmtype)
                '        validatepromotiondetails = False
                '        Exit Function
                '    End If
                'Next


                'ElseIf UCase(mString(i)) = UCase("Meal Upgrade") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Meal Upgrade").ToUpper.Trim) = True Then
                'flgCheck = False
                'For Each gvRow In grdmealplan.Rows
                '    Dim chkmeal1 As CheckBox = gvRow.FindControl("chkmeal")
                '    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgrademeal")

                '    If (ddlUpgradermtyp.Value <> "" And ddlUpgradermtyp.Value <> "[Select]") Then
                '        flgCheck = True
                '        Exit For
                '    End If
                'Next

                'If flgCheck = False Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Meal Upgrade Promotion Selected  Please select Meal Plan to upgrade.');", True)
                '    SetFocus(grdmealplan)
                '    validatepromotiondetails = False
                '    Exit Function
                'End If



                'For Each gvRow In grdmealplan.Rows
                '    Dim chkmeal1 As CheckBox = gvRow.FindControl("chkmeal")
                '    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgrademeal")

                '    If (ddlUpgradermtyp.Value = "[Select]") And chkmeal1.Checked = True Then
                '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Meal Plan to upgrade.');", True)
                '        SetFocus(grdmealplan)
                '        validatepromotiondetails = False
                '        Exit Function
                '    End If
                'Next

                'ElseIf UCase(mString(i)) = UCase("Accomodation Upgrade") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Accomodation Upgrade").ToUpper.Trim) = True Then
                flgCheck = False
                For Each gvRow In grdrmcat.Rows
                    Dim chkmeal1 As CheckBox = gvRow.FindControl("chkrmcat")
                    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgradermcat")

                    If (ddlUpgradermtyp.Value <> "" And ddlUpgradermtyp.Value <> "[Select]") Then
                        flgCheck = True
                        Exit For
                    End If
                Next

                If flgCheck = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accomodation Upgrade Promotion Selected  Please select Accomodation  to upgrade.');", True)
                    SetFocus(grdrmcat)
                    validatepromotiondetails = False
                    Exit Function
                End If

                For Each gvRow In grdrmcat.Rows
                    Dim chkmeal1 As CheckBox = gvRow.FindControl("chkrmcat")
                    Dim ddlUpgradermtyp As HtmlSelect = gvRow.FindControl("ddlUpgradermcat")

                    If (ddlUpgradermtyp.Value = "[Select]") And chkmeal1.Checked = True Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Please select Accomodation  to upgrade.');", True)
                        SetFocus(grdrmcat)
                        validatepromotiondetails = False
                        Exit Function
                    End If
                Next


                'ElseIf UCase(mString(i)) = UCase("Select flights only") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Select flights only").ToUpper.Trim) = True Then
                'divflight.Style.Add("display", "block")
                flgCheck = False
                For Each gvRow In grdflight.Rows
                    Dim txtflightcode As TextBox = gvRow.FindControl("txtflightcode")


                    If txtflightcode.Text <> "" Then
                        flgCheck = True
                        Exit For
                    End If
                Next
                If flgCheck = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Flight Only Promotion Selected  Please select Flight.');", True)
                    SetFocus(grdflight)
                    validatepromotiondetails = False
                    Exit Function
                End If

                ' ElseIf UCase(mString(i)) = UCase("Special Occasion") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Special Occasion").ToUpper.Trim) = True Then

                If txtsplocc.Value.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Special Occasion.');", True)
                    txtsplocc.Focus()
                    validatepromotiondetails = False
                    Exit Function
                End If


                ' ElseIf UCase(mString(i)) = UCase("Inter Hotels") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Inter Hotels").ToUpper.Trim) = True Then
                flgCheck = False
                For Each gvRow In grdinterhotel.Rows
                    Dim txtinterhotelname As TextBox = gvRow.FindControl("txtinterhotelname")


                    If txtinterhotelname.Text <> "" Then
                        flgCheck = True
                        Exit For
                    End If
                Next
                If flgCheck = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Inter Hotel Promotion Selected  Please select Inter Hotel');", True)
                    SetFocus(grdinterhotel)
                    validatepromotiondetails = False
                    Exit Function
                End If

                'ElseIf UCase(mString(i)) = UCase("Free Nights") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Free Nights").ToUpper.Trim) = True Then

                lnCnt = 0
                For Each GVRow In grdpromotiondetail.Rows
                    lnCnt += 1

                    txtfromdate = GVRow.FindControl("txtfromDate")
                    txtToDate = GVRow.FindControl("txtToDate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    ddlapply = GVRow.FindControl("ddlapply")
                    ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                    chkmultiples = GVRow.FindControl("chkmultiples")
                    txtstayfor = GVRow.FindControl("txtstayfor")
                    txtpayfor = GVRow.FindControl("txtpayfor")
                    txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                    txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")

                    If txtfromdate.Text <> "" And txtToDate.Text <> "" Then

                        If txtstayfor.Text = "" And txtpayfor.Text = "" And txtmaxfreents.Text = "" And txtmaxmultiples.Text = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Free nights Promotion selected Please Enter Stay For  and Pay For.');", True)
                            txtstayfor.Focus()
                            validatepromotiondetails = False
                            Exit Function
                        End If

                    End If
                Next

            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Early Bird Discount").ToUpper.Trim) = True Then
                lnCnt = 0
                For Each GVRow In grdpromotiondetail.Rows
                    lnCnt += 1
                    txtfromdate = GVRow.FindControl("txtfromDate")
                    txtToDate = GVRow.FindControl("txtToDate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    ddlapply = GVRow.FindControl("ddlapply")
                    ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                    chkmultiples = GVRow.FindControl("chkmultiples")
                    txtstayfor = GVRow.FindControl("txtstayfor")
                    txtpayfor = GVRow.FindControl("txtpayfor")
                    txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                    txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")

                    If txtfromdate.Text <> "" And txtToDate.Text <> "" Then

                        If CType(ddloptions.Items(ddloptions.SelectedIndex).Text.ToUpper, String) = ("Percentage").ToUpper And txtdiscount.Text = "" Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Discount  Row No :" & lnCnt & "');", True)
                            txtdiscount.Focus()
                            validatepromotiondetails = False
                            Exit Function
                        ElseIf CType(ddloptions.Items(ddloptions.SelectedIndex).Text.ToUpper, String) = ("Value").ToUpper And txtdiscount.Text = "" Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Value  Row No :" & lnCnt & "');", True)
                            txtdiscount.Focus()
                            validatepromotiondetails = False
                            Exit Function
                        ElseIf CType(ddloptions.Items(ddloptions.SelectedIndex).Text.ToUpper, String) = ("Mulitiple Rooms").ToUpper And txtnoofrooms.Text = "" Then

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select No.Of Rooms  Adults & child Combination  Row No :" & lnCnt & "');", True)
                            txtnoofrooms.Focus()
                            validatepromotiondetails = False
                            Exit Function
                        End If
                    End If

                Next


                'ElseIf UCase(mString(i)) = UCase("Complimentary Airport Transfer") Then
            ElseIf UCase(promoList).ToUpper.Trim.Contains(UCase("Complimentary Airport Transfer").ToUpper.Trim) = True Then

                If chkarrival.Checked = False And chkdeparture.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Complimentary Airport Transfer Promotion Selected  Please select Transfer');", True)
                    'SetFocus(grdinterhotel)
                    validatepromotiondetails = False
                    Exit Function
                End If


            End If
            '  Next
        End If


        If chkarrival.Checked = True Then
            flgCheck = False
            For Each gvRow In grdArrivalTransfer.Rows
                Dim txtarrivalAirportName As TextBox = gvRow.FindControl("txtarrivalAirportName")
                If txtarrivalAirportName.Text <> "" Then
                    flgCheck = True
                    Exit For
                End If
            Next


            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Arrival Transfer Select atleast one Transfer.');", True)
                SetFocus(grdArrivalTransfer)
                validatepromotiondetails = False
                Exit Function
            End If

        End If


        If chkdeparture.Checked = True Then
            flgCheck = False
            For Each gvRow In grdDepartureTransfer.Rows
                Dim txtdepartureAirportName As TextBox = gvRow.FindControl("txtdepartureAirportName")
                If txtdepartureAirportName.Text <> "" Then
                    flgCheck = True
                    Exit For
                End If
            Next


            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Departure Transfer Select atleast one Transfer.');", True)
                SetFocus(grdDepartureTransfer)
                validatepromotiondetails = False
                Exit Function
            End If

        End If

        If ddlcombine.SelectedIndex = 2 Or ddlcombine.SelectedIndex = 4 Then

            flgCheck = False
            For Each gvRow In grdcombinable.Rows
                Dim txtpromotionname As TextBox = gvRow.FindControl("txtpromotionname")
                If txtpromotionname.Text <> "" Then
                    flgCheck = True
                    Exit For
                End If
            Next

            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Combinable Promotions.');", True)
                SetFocus(grdcombinable)
                validatepromotiondetails = False
                Exit Function
            End If
        End If
        validatepromotiondetails = True
    End Function

    Private Function ValidateSave() As Boolean

        If txtApplicableTo.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Applicable To.');", True)
            txtApplicableTo.Focus()
            ValidateSave = False
            Exit Function
        End If


        If txtpromotionname.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Promotion Name.');", True)
            txtpromotionname.Focus()
            ValidateSave = False
            Exit Function
        End If


        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from partymast where partycode='" & hdnpartycode.Value & "'") = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Hotel Not belongs to the Supplier List.');", True)

            ValidateSave = False
            Exit Function
        End If

        '' Madam asked to to chnage optional to for arun request 15/03/17
        'If txtremarks.Value = "" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Remarks.');", True)
        '    txtremarks.Focus()
        '    ValidateSave = False
        '    Exit Function
        'End If


        '  Dim GvRow As GridViewRow
        Dim chkSelect As CheckBox
        Dim lblDaysOfWeek As Label
        Dim weekdays As String = ""
        Session("daysoftheweek") = Nothing
        For Each gvRow As DataListItem In dlWeekDays.Items
            chkSelect = gvRow.FindControl("chkSelect")
            lblDaysOfWeek = gvRow.FindControl("lblDaysOfWeek")
            ' chkAll = dlWeekDays.Controls(0).Controls(0).FindControl("chkAll")

            If chkSelect.Checked = True Then
                weekdays = weekdays + lblDaysOfWeek.Text + ","
            End If


        Next

        If weekdays.Length > 0 Then
            weekdays = weekdays.Substring(0, weekdays.Length - 1)
        End If

        Session("daysoftheweek") = weekdays


        Dim chkpromotiontype As CheckBox
        Dim txtpromtoiontype As Label
        Dim promotypes As String = ""
        Session("promotypes") = Nothing
        For Each gvRow As DataListItem In dlPromotionType.Items
            chkpromotiontype = gvRow.FindControl("chkpromotiontype")
            txtpromtoiontype = gvRow.FindControl("txtpromtoiontype")
            ' chkAll = dlWeekDays.Controls(0).Controls(0).FindControl("chkAll")

            If chkpromotiontype.Checked = True Then
                promotypes = promotypes + txtpromtoiontype.Text + ","
            End If


        Next

        If promotypes.Length > 0 Then
            promotypes = promotypes.Substring(0, promotypes.Length - 1)
        End If

        Session("promotypes") = promotypes



        ValidateSave = True
    End Function
    Public Function FindDatePeriod() As Boolean
        Dim GVRow As GridViewRow

        Dim strMsg As String = ""
        FindDatePeriod = True
        Try



            Dim ds As DataSet
            Dim parms3 As New List(Of SqlParameter)
            Dim parm3(7) As SqlParameter
            parm3(0) = New SqlParameter("@partycode", CType(hdnpartycode.Value, String))

            parm3(3) = New SqlParameter("@mode", CType(Session("OfferState"), String))
            parm3(4) = New SqlParameter("@tranid", CType(txtpromotionid.Text, String))
            parm3(5) = New SqlParameter("@country", CType(wucCountrygroup.checkcountrylist, String))
            parm3(6) = New SqlParameter("@agent", CType(wucCountrygroup.checkagentlist, String))

            parms3.Add(parm3(0))
            parms3.Add(parm3(1))
            parms3.Add(parm3(2))
            parms3.Add(parm3(3))
            parms3.Add(parm3(4))
            parms3.Add(parm3(5))
            parms3.Add(parm3(6))



            ds = New DataSet()
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_chkduplicate_contract", parms3)


            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("contractid")) = False Then
                        strMsg = "Contract already exists For this Hotel " + ds.Tables(0).Rows(0)("contractid") + " - Country " + ds.Tables(0).Rows(0)("ctryname") + " - Agent - " + ds.Tables(0).Rows(0)("agentname")
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)
                        ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Max accomodation already exists For this Supplier Please check this transaction   ');", True)
                        FindDatePeriod = False
                        Exit Function
                    End If
                End If
            End If


            FindDatePeriod = True

        Catch ex As Exception
            FindDatePeriod = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Function
    Public Function checkForDuplicatepromotionname() As String

        Dim promoname As String
        Dim pricecode1 As String
        Dim strmsg1 As String = ""

        If (Session("OfferState") = "New" Or Session("OfferState") = "Copy") And txtpromotionname.Text.Trim <> "" Then


            promoname = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select 't' from view_offers_header(nolock) where  partycode ='" & CType(hdnpartycode.Value, String) & "' and  promotionname='" & CType(txtpromotionname.Text.Trim, String) & "'")

            If promoname <> "" Then
                strmsg1 = "This Promotion Name is already present Please Add hotel Promo code and continue"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Promotion Name is already present Please Add hotel Promo code');", True)
                'SetFocus(txtpromotionname)
                checkForDuplicatepromotionname = strmsg1
                Exit Function
            End If

        End If

        If (Session("OfferState") = "Edit") And txtpromotionname.Text.Trim <> "" Then


            promoname = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select 't' from view_offers_header(nolock) where  promotionid<>'" & txtpromotionid.Text & "' and partycode ='" & CType(hdnpartycode.Value, String) & "' and  promotionname='" & CType(txtpromotionname.Text.Trim, String) & "'")

            If promoname <> "" Then
                strmsg1 = "This Promotion Name is already present Please Add hotel Promo code and continue"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Promotion Name is already present Please Add hotel Promo code');", True)
                'SetFocus(txtpromotionname)
                checkForDuplicatepromotionname = strmsg1
                Exit Function
            End If

        End If


        '  checkForDuplicatepromotionname = True
    End Function
    Protected Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs)
     
        ViewState("checkPromotionMissing") = Nothing
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strMsg As String = ""
            Dim txtSeasonName As TextBox
            Dim txtSeasonfromDate As TextBox
            Dim txtSeasonToDate As TextBox
            Dim txtMinNight As TextBox
            Dim GvSeasonShowSub As GridView
            Dim lblRowId As Label
            Dim liRowId As Integer = 0
            Dim liRowIdCurr As Integer = -1

            If Page.IsValid = True Then
                If Session("OfferState") = "New" Or Session("OfferState") = "Edit" Or Session("OfferState") = "Copy" Then
                    If ValidateSave() = False Then
                        Exit Sub
                    End If

                    If validatepromotiondetails() = False Then
                        Exit Sub
                    End If

                    'If validatepromotionEntry() = False Then
                    '    Exit Sub
                    'End If

                

                    If ViewState("checkPromotionMissing") <> 1 Then
                        Dim strmsg3 As String = checkForDuplicatepromotionname()

                        If strmsg3 <> "" Then

                            ViewState("checkPromotionMissing") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "confirmapplicable('" + strmsg3 + "','" + btnSave.ClientID + "');", True)
                            Exit Sub

                        End If
                    End If

                    Session("CountryList") = Nothing
                    Session("AgentList") = Nothing

                    Session("CountryList") = wucCountrygroup.checkcountrylist
                    Session("AgentList") = wucCountrygroup.checkagentlist

                    If Session("CountryList") = Nothing And Session("AgentList") = Nothing Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country and Agent Should not be Empty Please select .');", True)
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("OfferState") = "New" Or Session("OfferState") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("PROMOTION", mySqlConn, sqlTrans)
                        txtpromotionid.Text = optionval.Trim

                        If txtpromotionid.Text.Trim = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The promotion id should not be blank');", True)
                            Exit Sub
                        End If

                        mySqlCmd = New SqlCommand("sp_add_edit_offers_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionname", SqlDbType.VarChar, 1000)).Value = CType(txtpromotionname.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = IIf(chkactive.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontypes", SqlDbType.VarChar, 8000)).Value = CType(Session("promotypes"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@daysoftheweek", SqlDbType.VarChar, 1000)).Value = CType(Session("daysoftheweek"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@inventorytype", SqlDbType.VarChar, 200)).Value = CType(ddlinventorytype.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@combinetype", SqlDbType.VarChar, 200)).Value = CType(ddlcombine.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@nonrefundable", SqlDbType.Int)).Value = IIf(chkrefund.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applytdiscountoexhibition", SqlDbType.Int)).Value = IIf(chkdiscount.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@arrivaltransfer", SqlDbType.Int)).Value = IIf(chkarrival.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@departuretransfer", SqlDbType.Int)).Value = IIf(chkdeparture.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@specialoccassion", SqlDbType.VarChar, 8000)).Value = CType(txtsplocc.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 8000)).Value = CType(txtremarks.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@commissiontype", SqlDbType.VarChar, 100)).Value = CType(ddlcommission.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@applydiscounttype", SqlDbType.VarChar, 100)).Value = CType(ddlapplydiscount.Value, String)
                        If ddlapplydiscount.SelectedIndex = 0 Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = ""
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = CType(hdnapplydiscount.Value, String)
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@activestate", SqlDbType.VarChar, 20)).Value = IIf(chkactive.Checked = False, "Active", "With Drawn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 100)).Value = CType(txtshortname.Text, String)


                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()

                    ElseIf Session("OfferState") = "Edit" Then

                        If txtpromotionid.Text.Trim = "" Then
                            'Dim optionval As String
                            'optionval = objUtils.GetAutoDocNo("PROMOTION", mySqlConn, sqlTrans)
                            'txtpromotionid.Text = optionval.Trim

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The promotion id should not be blank');", True)
                            Exit Sub
                        End If
                        ''Added by abin on 20190403
                        UpdateApplyDiscountIds()

                        mySqlCmd = New SqlCommand("sp_mod_edit_offers_header", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionname", SqlDbType.VarChar, 1000)).Value = CType(txtpromotionname.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applicableto", SqlDbType.VarChar, 5000)).Value = CType(Replace(txtApplicableTo.Text, ",  ", ","), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = IIf(chkactive.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotiontypes", SqlDbType.VarChar, 8000)).Value = CType(Session("promotypes"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@daysoftheweek", SqlDbType.VarChar, 1000)).Value = CType(Session("daysoftheweek"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@inventorytype", SqlDbType.VarChar, 200)).Value = CType(ddlinventorytype.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@combinetype", SqlDbType.VarChar, 200)).Value = CType(ddlcombine.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@nonrefundable", SqlDbType.Int)).Value = IIf(chkrefund.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applytdiscountoexhibition", SqlDbType.Int)).Value = IIf(chkdiscount.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@arrivaltransfer", SqlDbType.Int)).Value = IIf(chkarrival.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@departuretransfer", SqlDbType.Int)).Value = IIf(chkdeparture.Checked = True, 1, 0)
                        mySqlCmd.Parameters.Add(New SqlParameter("@specialoccassion", SqlDbType.VarChar, 8000)).Value = CType(txtsplocc.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 8000)).Value = CType(txtremarks.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@approved", SqlDbType.Int)).Value = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@commissiontype", SqlDbType.VarChar, 100)).Value = CType(ddlcommission.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@applydiscounttype", SqlDbType.VarChar, 100)).Value = CType(ddlapplydiscount.Value, String)
                        If ddlapplydiscount.SelectedIndex = 0 Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = ""
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = CType(hdnapplydiscount.Value, String)
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@activestate", SqlDbType.VarChar, 20)).Value = IIf(chkactive.Checked = False, "Active", "With Drawn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@shortname", SqlDbType.VarChar, 100)).Value = CType(txtshortname.Text, String)


                        mySqlCmd.ExecuteNonQuery()
                        mySqlCmd.Dispose()

                    End If

                    '''  User cotrol country saving
                    ''' 

                   

                    'If CType(Session("promotypes"), String).Contains("Special Rates") = False Then
                    If CType(Session("promotypes"), String) <> "Special Rates" Then

                        mySqlCmd = New SqlCommand("DELETE FROM new_edit_offersmain Where promotion_mas_id='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()


                    End If

                    If CType(Session("promotypes"), String).Contains("Special Rates") = True Then

                        mySqlCmd = New SqlCommand("DELETE FROM new_edit_offersmain_spl Where promotion_mas_id='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()


                    End If



                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersCombinations Where promotion_mas_id='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    mySqlCmd = New SqlCommand("DELETE FROM edit_offers_detail Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    mySqlCmd = New SqlCommand("DELETE FROM new_edit_offersroomrates Where  left(Promotion_ID,10)='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()



                    Dim k As Integer = 1
                    For Each GVRow In grdpromotiondetail.Rows

                        Dim txtfromdate As TextBox = GVRow.FindControl("txtfromDate")
                        Dim txtToDate As TextBox = GVRow.FindControl("txtToDate")
                        Dim txthotelbookingcode As TextBox = GVRow.FindControl("txthotelbookingcode")
                        Dim ddloptions As DropDownList = GVRow.FindControl("ddloptions")
                        Dim txtdiscount As TextBox = GVRow.FindControl("txtdiscount")
                        Dim txtadddiscount As TextBox = GVRow.FindControl("txtadddiscount")
                        Dim txtnoofrooms As TextBox = GVRow.FindControl("txtnoofrooms")
                        Dim chkmultiyes As CheckBox = GVRow.FindControl("chkmultiyes")
                        Dim ddlbookoptions As DropDownList = GVRow.FindControl("ddlbookoptions")
                        Dim txtbookfromDate As TextBox = GVRow.FindControl("txtbookfromDate")
                        Dim txtBookToDate As TextBox = GVRow.FindControl("txtBookToDate")
                        Dim txtbookdays As TextBox = GVRow.FindControl("txtbookdays")
                        Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                        Dim ddlapply As DropDownList = GVRow.FindControl("ddlapply")

                        Dim chkmultiples As CheckBox = GVRow.FindControl("chkmultiples")
                        Dim txtstayfor As TextBox = GVRow.FindControl("txtstayfor")
                        Dim txtpayfor As TextBox = GVRow.FindControl("txtpayfor")
                        Dim txtmaxfreents As TextBox = GVRow.FindControl("txtmaxfreents")
                        Dim txtmaxmultiples As TextBox = GVRow.FindControl("txtmaxmultiples")
                        Dim txtmaxnights As TextBox = GVRow.FindControl("txtmaxnights")

                        Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                        Dim txtuprmtypname As TextBox = GVRow.FindControl("txtuprmtypname")

                        Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                        Dim txtupmealcode As TextBox = GVRow.FindControl("txtupmealcode")

                        Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                        Dim txtuprmcatcode As TextBox = GVRow.FindControl("txtuprmcatcode")

                        Dim txtmealrmcatcode As TextBox = GVRow.FindControl("txtmealrmcatcode")
                        Dim txtupmealrmcatcode As TextBox = GVRow.FindControl("txtupmealrmcatcode")
                        Dim txtrmcombination As TextBox = GVRow.FindControl("txtrmcombination")
                        Dim txtmealcombination As TextBox = GVRow.FindControl("txtmealcombination")


                        If txtfromdate.Text <> "" And txtToDate.Text <> "" Then
                            ' mySqlCmd = New SqlCommand("sp_mod_edit_offers_detail", mySqlConn, sqlTrans)
                            mySqlCmd = New SqlCommand("sp_mod_edit_offers_detail_New", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid ", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = k
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 200)).Value = CType(txthotelbookingcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@discounttype", SqlDbType.VarChar, 200)).Value = CType(ddloptions.Items(ddloptions.SelectedIndex).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@discountamount", SqlDbType.Decimal)).Value = CType(Val(txtdiscount.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@additionalamount", SqlDbType.Decimal)).Value = CType(Val(txtadddiscount.Text), Decimal)
                            mySqlCmd.Parameters.Add(New SqlParameter("@roomstring", SqlDbType.VarChar, 200)).Value = CType(txtnoofrooms.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@multiples", SqlDbType.Int)).Value = IIf(chkmultiyes.Checked = True, 1, 0)

                            mySqlCmd.Parameters.Add(New SqlParameter("@bookingvalidityoptions", SqlDbType.VarChar, 100)).Value = CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text, String)
                            If txtbookfromDate.Text <> "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@bookingvalidityfromdate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtbookfromDate.Text, Date), "yyyy/MM/dd"), String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@bookingvalidityfromdate", SqlDbType.VarChar, 10)).Value = ""
                            End If
                            If txtBookToDate.Text <> "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@bookingvaliditytodate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtBookToDate.Text, Date), "yyyy/MM/dd"), String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@bookingvaliditytodate", SqlDbType.VarChar, 10)).Value = ""
                            End If

                            mySqlCmd.Parameters.Add(New SqlParameter("@bookingvaliditydaysmonths", SqlDbType.Int)).Value = CType(Val(txtbookdays.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@minnights", SqlDbType.Int)).Value = CType(Val(txtminnights.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@applyto", SqlDbType.VarChar, 100)).Value = CType(ddlapply.Items(ddlapply.SelectedIndex).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@allowmultistay", SqlDbType.Int)).Value = IIf(chkmultiples.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@stayfor", SqlDbType.Int)).Value = CType(Val(txtstayfor.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@payfor", SqlDbType.Int)).Value = CType(Val(txtpayfor.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxfeenights", SqlDbType.Int)).Value = CType(Val(txtmaxfreents.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxmultiples", SqlDbType.Int)).Value = CType(Val(txtmaxmultiples.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxnights", SqlDbType.Int)).Value = CType(Val(txtmaxnights.Text), Integer)

                            mySqlCmd.Parameters.Add(New SqlParameter("@roomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtrmtypname.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealplans", SqlDbType.VarChar, 8000)).Value = CType(txtmealcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcodes", SqlDbType.VarChar, 8000)).Value = CType(txtrmcatcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@suppcodes", SqlDbType.VarChar, 8000)).Value = CType(txtmealrmcatcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@updgroomtypes", SqlDbType.VarChar, 8000)).Value = CType(txtuprmtypname.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@updmealplans", SqlDbType.VarChar, 8000)).Value = CType(txtupmealcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@updrmcatcodes", SqlDbType.VarChar, 8000)).Value = CType(txtuprmcatcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@updsuppcodes", SqlDbType.VarChar, 8000)).Value = CType(txtupmealrmcatcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcombine", SqlDbType.VarChar, 8000)).Value = CType(txtrmcombination.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcombine", SqlDbType.VarChar, 8000)).Value = CType(txtmealcombination.Text.Trim, String)

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                            If CType(Session("promotypes"), String).Contains("Special Rates") = True Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 1
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 0
                            End If

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed


                            '''' Save New Offer Main only for non spl

                            Dim countrygroup As String = ""
                            Dim agentGroup As String = ""

                            'If CType(Session("promotypes"), String) <> "Special Rates" Then

                            If Session("CountryList") <> "" Then ' And CType(Session("promotypes"), String).Contains("Special Rates") = False Then

                                ''Value in hdn variable , so splting to get string correctly
                                countrygroup = wucCountrygroup.checkcountrylist.ToString.Trim
                            End If
                                If Session("AgentList") <> "" Then

                                agentGroup = wucCountrygroup.checkagentlist.ToString.Trim
                                End If

                                '  Dim arrcountry As String() = wucCountrygroup.checkcountrylist.ToString.Trim.Split(",")
                                ' For i = 0 To arrcountry.Length - 1

                                'If arrcountry(i) <> "" Then
                            If countrygroup <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain_New", mySqlConn, sqlTrans)
                                'mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.CommandTimeout = 0

                                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentGroup, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = ""
                                'mySqlCmd.Parameters.Add(New SqlParameter("@Cccode", SqlDbType.VarChar, 100)).Value =  ' CType(arrcountry(i), String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@Cccode", SqlDbType.VarChar, 8000)).Value = CType(countrygroup, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@createdby", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                                If CType(Session("promotypes"), String).Contains("Special Rates") = True Then

                                    ' If CType(Session("promotypes"), String).Trim = "Special Rates" Then
                                    mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 1

                                Else
                                    mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 0
                                End If

                                mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = k


                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed
                                'End If
                                ' Next

                            End If

                            'If Session("AgentList") <> "" Then  'And CType(Session("promotypes"), String).Contains("Special Rates") = False Then

                            '    ''Value in hdn variable , so splting to get string correctly
                            '    'Dim arragents As String() = wucCountrygroup.checkagentlist.ToString.Trim.Split(",")
                            '    ' For i = 0 To arragents.Length - 1

                            '    Dim agentGroup As String = wucCountrygroup.checkagentlist.ToString.Trim

                            '    If agentGroup <> "" Then


                            '        mySqlCmd = New SqlCommand("sp_add_new_edit_offersmain", mySqlConn, sqlTrans)
                            '        mySqlCmd.CommandType = CommandType.StoredProcedure
                            '        mySqlCmd.CommandTimeout = 0

                            '        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"), String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 8000)).Value = CType(agentGroup, String)   'CType(arragents(i), String)
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 100)).Value = "" 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ctrycode from agentmast where agentcode='" & CType(arragents(i), String) & "'")
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@Cccode", SqlDbType.VarChar, 100)).Value = ""
                            '        mySqlCmd.Parameters.Add(New SqlParameter("@createdby", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                            '        If CType(Session("promotypes"), String).Contains("Special Rates") = True Then

                            '            mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 1

                            '        ElseIf CType(Session("promotypes"), String).Trim <> "Special Rates" Then
                            '            mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 0
                            '        End If

                            '        mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = k


                            '        mySqlCmd.ExecuteNonQuery()
                            '        mySqlCmd.Dispose() 'command disposed
                            '    End If
                            '    ' Next

                            'End If

                            ' End If   ' NonSpl


                        End If
                        k += 1
                    Next

                    k = 1

                    For Each GVRow In grdpromotiondetail.Rows

                        Dim txtfromdate As TextBox = GVRow.FindControl("txtfromDate")
                        Dim txtToDate As TextBox = GVRow.FindControl("txtToDate")
                        Dim txthotelbookingcode As TextBox = GVRow.FindControl("txthotelbookingcode")
                        Dim ddloptions As DropDownList = GVRow.FindControl("ddloptions")
                        Dim txtdiscount As TextBox = GVRow.FindControl("txtdiscount")
                        Dim txtadddiscount As TextBox = GVRow.FindControl("txtadddiscount")
                        Dim txtnoofrooms As TextBox = GVRow.FindControl("txtnoofrooms")
                        Dim chkmultiyes As CheckBox = GVRow.FindControl("chkmultiyes")
                        Dim ddlbookoptions As DropDownList = GVRow.FindControl("ddlbookoptions")
                        Dim txtbookfromDate As TextBox = GVRow.FindControl("txtbookfromDate")
                        Dim txtBookToDate As TextBox = GVRow.FindControl("txtBookToDate")
                        Dim txtbookdays As TextBox = GVRow.FindControl("txtbookdays")
                        Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                        Dim ddlapply As DropDownList = GVRow.FindControl("ddlapply")

                        Dim chkmultiples As CheckBox = GVRow.FindControl("chkmultiples")
                        Dim txtstayfor As TextBox = GVRow.FindControl("txtstayfor")
                        Dim txtpayfor As TextBox = GVRow.FindControl("txtpayfor")
                        Dim txtmaxfreents As TextBox = GVRow.FindControl("txtmaxfreents")
                        Dim txtmaxmultiples As TextBox = GVRow.FindControl("txtmaxmultiples")
                        Dim txtmaxnights As TextBox = GVRow.FindControl("txtmaxnights")

                        Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                        Dim txtuprmtypname As TextBox = GVRow.FindControl("txtuprmtypname")

                        Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                        Dim txtupmealcode As TextBox = GVRow.FindControl("txtupmealcode")

                        Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                        Dim txtuprmcatcode As TextBox = GVRow.FindControl("txtuprmcatcode")

                        Dim txtmealrmcatcode As TextBox = GVRow.FindControl("txtmealrmcatcode")
                        Dim txtupmealrmcatcode As TextBox = GVRow.FindControl("txtupmealrmcatcode")
                        Dim txtrmcombination As TextBox = GVRow.FindControl("txtrmcombination")
                        Dim txtmealcombination As TextBox = GVRow.FindControl("txtmealcombination")


                        If txtfromdate.Text <> "" And txtToDate.Text <> "" Then
                            mySqlCmd = New SqlCommand("sp_add_new_edit_offercombinations", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid ", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = k
                            If ddlapplydiscount.SelectedIndex = 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = ""
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountids", SqlDbType.VarChar, 8000)).Value = CType(hdnapplydiscount.Value, String)
                            End If

                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotion_name", SqlDbType.VarChar, 200)).Value = CType(txtpromotionname.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotion_type", SqlDbType.VarChar, 100)).Value = CType(Session("promotypes"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"), String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                            If ddloptions.Text = 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@discountperc", SqlDbType.Decimal)).Value = CType(Val(txtdiscount.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@discountvalue", SqlDbType.Decimal)).Value = 0
                                mySqlCmd.Parameters.Add(New SqlParameter("@adddiscountperc", SqlDbType.Decimal)).Value = CType(Val(txtadddiscount.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@adddiscountvalue", SqlDbType.Decimal)).Value = 0
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@discountperc", SqlDbType.Decimal)).Value = 0
                                mySqlCmd.Parameters.Add(New SqlParameter("@discountvalue", SqlDbType.Decimal)).Value = CType(Val(txtdiscount.Text), Decimal)
                                mySqlCmd.Parameters.Add(New SqlParameter("@adddiscountperc", SqlDbType.Decimal)).Value = 0
                                mySqlCmd.Parameters.Add(New SqlParameter("@adddiscountvalue", SqlDbType.Decimal)).Value = CType(Val(txtadddiscount.Text), Decimal)

                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@bookingvaliditytext", SqlDbType.VarChar, 100)).Value = CType(ddlbookoptions.Items(ddlbookoptions.SelectedIndex).Text, String)

                            If txtbookfromDate.Text <> "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@BookingValidity_From", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtbookfromDate.Text, Date), "yyyy/MM/dd"), String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@BookingValidity_From", SqlDbType.VarChar, 10)).Value = ""
                            End If
                            If txtBookToDate.Text <> "" Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@BookingValidity_to", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtBookToDate.Text, Date), "yyyy/MM/dd"), String)
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@BookingValidity_to", SqlDbType.VarChar, 10)).Value = ""
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@BookDays_BeforeCheckIn", SqlDbType.Int)).Value = CType(Val(txtbookdays.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@BookingValidity", SqlDbType.Int)).Value = CType(Val(txtbookdays.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@minnight", SqlDbType.Int)).Value = CType(Val(txtminnights.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxnight", SqlDbType.Int)).Value = CType(Val(txtmaxnights.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 200)).Value = CType(txthotelbookingcode.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@non_refundable", SqlDbType.Int)).Value = IIf(chkrefund.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@combinetype", SqlDbType.VarChar, 200)).Value = CType(ddlcombine.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@inventorytype", SqlDbType.VarChar, 200)).Value = CType(ddlinventorytype.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@allow_multistay", SqlDbType.Int)).Value = IIf(chkmultiples.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@stayfor", SqlDbType.Int)).Value = CType(Val(txtstayfor.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@payfor", SqlDbType.Int)).Value = CType(Val(txtpayfor.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@max_freenights", SqlDbType.Int)).Value = CType(Val(txtmaxfreents.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@max_multiples", SqlDbType.Int)).Value = CType(Val(txtmaxmultiples.Text), Integer)
                            mySqlCmd.Parameters.Add(New SqlParameter("@applyto", SqlDbType.VarChar, 100)).Value = CType(ddlapply.Items(ddlapply.SelectedIndex).Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@commissiontype", SqlDbType.VarChar, 100)).Value = CType(ddlcommission.Value, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscounttype", SqlDbType.VarChar, 100)).Value = CType(ddlapplydiscount.Value, String)

                            mySqlCmd.Parameters.Add(New SqlParameter("@applydiscountexhi", SqlDbType.Int)).Value = ddlapplydiscount.SelectedIndex
                            mySqlCmd.Parameters.Add(New SqlParameter("@arrivaltransfer", SqlDbType.Int)).Value = IIf(chkarrival.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@departuretransfer", SqlDbType.Int)).Value = IIf(chkdeparture.Checked = True, 1, 0)
                            mySqlCmd.Parameters.Add(New SqlParameter("@activestatus", SqlDbType.VarChar, 20)).Value = IIf(chkactive.Checked = False, "Active", "With Drawn")


                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed
                        End If
                        k += 1
                    Next


                    ' ''''''' start                      ' rosalin on 2019-10-28
                    '' Select special rates 

                    '' If CType(Session("promotypes"), String).Contains("Special Rates") = False Then  ' only non special rate offers
                    '' Meal Upgrade,Special Rates

                    'If CType(Session("promotypes"), String) <> "Special Rates" Then

                    '    Dim arrOcpncy As String()
                    '    k = 1
                    '    Dim ix As Integer
                    '    ix = 1
                    '    For Each GVRow In grdpromotiondetail.Rows

                    '        Dim txtfromdate As TextBox = GVRow.FindControl("txtfromDate")
                    '        Dim txtToDate As TextBox = GVRow.FindControl("txtToDate")
                    '        Dim txthotelbookingcode As TextBox = GVRow.FindControl("txthotelbookingcode")
                    '        Dim ddloptions As DropDownList = GVRow.FindControl("ddloptions")
                    '        Dim txtdiscount As TextBox = GVRow.FindControl("txtdiscount")
                    '        Dim txtadddiscount As TextBox = GVRow.FindControl("txtadddiscount")
                    '        Dim txtnoofrooms As TextBox = GVRow.FindControl("txtnoofrooms")
                    '        Dim chkmultiyes As CheckBox = GVRow.FindControl("chkmultiyes")
                    '        Dim ddlbookoptions As DropDownList = GVRow.FindControl("ddlbookoptions")
                    '        Dim txtbookfromDate As TextBox = GVRow.FindControl("txtbookfromDate")
                    '        Dim txtBookToDate As TextBox = GVRow.FindControl("txtBookToDate")
                    '        Dim txtbookdays As TextBox = GVRow.FindControl("txtbookdays")
                    '        Dim txtminnights As TextBox = GVRow.FindControl("txtminnights")
                    '        Dim ddlapply As DropDownList = GVRow.FindControl("ddlapply")

                    '        Dim chkmultiples As CheckBox = GVRow.FindControl("chkmultiples")
                    '        Dim txtstayfor As TextBox = GVRow.FindControl("txtstayfor")
                    '        Dim txtpayfor As TextBox = GVRow.FindControl("txtpayfor")
                    '        Dim txtmaxfreents As TextBox = GVRow.FindControl("txtmaxfreents")
                    '        Dim txtmaxmultiples As TextBox = GVRow.FindControl("txtmaxmultiples")
                    '        Dim txtmaxnights As TextBox = GVRow.FindControl("txtmaxnights")

                    '        Dim txtrmtypname As TextBox = GVRow.FindControl("txtrmtypname")
                    '        Dim txtuprmtypname As TextBox = GVRow.FindControl("txtuprmtypname")

                    '        Dim txtmealcode As TextBox = GVRow.FindControl("txtmealcode")
                    '        Dim txtupmealcode As TextBox = GVRow.FindControl("txtupmealcode")

                    '        Dim txtrmcatcode As TextBox = GVRow.FindControl("txtrmcatcode")
                    '        Dim txtuprmcatcode As TextBox = GVRow.FindControl("txtuprmcatcode")

                    '        Dim txtmealrmcatcode As TextBox = GVRow.FindControl("txtmealrmcatcode")
                    '        Dim txtupmealrmcatcode As TextBox = GVRow.FindControl("txtupmealrmcatcode")
                    '        Dim txtrmcombination As TextBox = GVRow.FindControl("txtrmcombination")
                    '        Dim txtmealcombination As TextBox = GVRow.FindControl("txtmealcombination")


                    '        If txtfromdate.Text <> "" And txtToDate.Text <> "" Then

                    '            arrOcpncy = txtrmtypname.Text.ToString.Trim.Split(",")

                    '            For i = 0 To arrOcpncy.Length - 1

                    '                If arrOcpncy(i) <> "" Then
                    '                    Dim mealcode As String() = txtmealcode.Text.ToString.Trim.Split(",")
                    '                    For j = 0 To mealcode.Length - 1

                    '                        If mealcode(j) <> "" Then
                    '                            Dim rmcatcode As String() = txtrmcatcode.Text.Trim.Split(",")

                    '                            For k = 0 To rmcatcode.Length - 1

                    '                                If rmcatcode(k) <> "" Then

                    '                                    Dim upgradroom As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmtypeupgrade from view_offers_rmtype(nolock) where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "' and rmtypcode='" & CType(arrOcpncy(i), String) & "'")
                    '                                    Dim upgradrmcat As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select rmcatupgrade from view_offers_accomodation(nolock) where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "' and rmcatcode='" & CType(rmcatcode(k), String) & "'")
                    '                                    Dim upgradmeal As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select mealupgrade from view_offers_meal(nolock) where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "' and mealcode='" & CType(mealcode(j), String) & "'")

                    '                                    mySqlCmd = New SqlCommand("sp_add_new_editofferroomrates", mySqlConn, sqlTrans)
                    '                                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    '                                    mySqlCmd.CommandTimeout = 0

                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value.Trim, String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid ", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd"), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = CType(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 100)).Value = CType(arrOcpncy(i), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(rmcatcode(k), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(mealcode(j), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@upgraderoom", SqlDbType.VarChar, 100)).Value = CType(upgradroom, String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@upgraderoomcat", SqlDbType.VarChar, 100)).Value = CType(upgradrmcat, String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@upgrademeal", SqlDbType.VarChar, 100)).Value = CType(upgradmeal, String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcode", SqlDbType.VarChar, 200)).Value = CType(txthotelbookingcode.Text.Trim, String)
                    '                                    If ddloptions.Text = 0 Then
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@discountperc", SqlDbType.Decimal)).Value = CType(Val(txtdiscount.Text), Decimal)
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Discount_Value", SqlDbType.Decimal)).Value = 0
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Add_Discount_Per", SqlDbType.Decimal)).Value = CType(Val(txtadddiscount.Text), Decimal)
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Add_Discount_Value", SqlDbType.Decimal)).Value = 0
                    '                                    Else
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@discountperc", SqlDbType.Decimal)).Value = 0
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Discount_Value", SqlDbType.Decimal)).Value = CType(Val(txtdiscount.Text), Decimal)
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Add_Discount_Per", SqlDbType.Decimal)).Value = 0
                    '                                        mySqlCmd.Parameters.Add(New SqlParameter("@Add_Discount_Value", SqlDbType.Decimal)).Value = CType(Val(txtadddiscount.Text), Decimal)
                    '                                    End If
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@plineno", SqlDbType.Int)).Value = ix

                    '                                    'If CType(Session("promotypes"), String).Contains("Special Rates") = True Then
                    '                                    '    mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 1
                    '                                    'Else
                    '                                    mySqlCmd.Parameters.Add(New SqlParameter("@offertype", SqlDbType.Int)).Value = 0
                    '                                    ' End If

                    '                                    mySqlCmd.ExecuteNonQuery()
                    '                                    mySqlCmd.Dispose() 'command disposed

                    '                                End If

                    '                            Next


                    '                        End If
                    '                    Next

                    '                End If


                    '            Next




                    '        End If
                    '        k += 1
                    '        ix += 1
                    '    Next
                    'End If ' non special rate
                    ' ''''''' Rosalin


                    Dim fromdate As String = ""
                    Dim todate As String = ""

                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select convert(varchar(10),min(convert(datetime,d.fromdate,111)),103) fromdate, convert(varchar(10),max(convert(datetime,d.todate,111)),103) todate  from edit_offers_detail d(nolock) where  d.promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'")
                    If ds1.Tables(0).Rows.Count > 0 Then
                        fromdate = ds1.Tables(0).Rows(0).Item("fromdate")
                        todate = ds1.Tables(0).Rows(0).Item("todate")
                    End If










                    ''' Room tYpes
                    ''' 
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_offers_rmtype Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    Dim promolistnew As String = ""
                    If Not Session("promolist") Is Nothing Then
                        promolistnew = Session("promolist")
                        Dim mString As String() = promolistnew.Split("|")

                    End If




                    'For Each gvrow In gridrmtype.Rows

                    '    Dim txtrmtypcode As Label = gvrow.FindControl("txtrmtypcode")
                    '    Dim hdnrmtypcode As HiddenField = gvrow.FindControl("hdnrmtypcode")
                    '    Dim chkrmtype As CheckBox = gvrow.FindControl("chkrmtype")
                    '    Dim ddlUpgradermtyp As HtmlSelect = gvrow.FindControl("ddlUpgradermtyp")

                    '    If chkrmtype.Checked = True Then

                    '        mySqlCmd = New SqlCommand("sp_add_offers_rmtype", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure


                    '        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(txtrmtypcode.Text, String)
                    '        If UCase(promolistnew).ToUpper.Trim.Contains(UCase("Room Upgrade").ToUpper.Trim) = True Then
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypeupgrade", SqlDbType.VarChar, 20)).Value = CType(ddlUpgradermtyp.Items(ddlUpgradermtyp.SelectedIndex).Value, String) 'CType(hdnrmtypcode.Value, String)
                    '        Else
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@rmtypeupgrade", SqlDbType.VarChar, 20)).Value = ""
                    '        End If


                    '        mySqlCmd.ExecuteNonQuery()
                    '        mySqlCmd.Dispose() 'command disposed

                    '    End If
                    'Next

                    ''' Meal Plan
                    ''' 
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_offers_meal Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    'For Each gvrow In grdmealplan.Rows

                    '    Dim txtmealcode As Label = gvrow.FindControl("txtmealcode")
                    '    Dim hdnmealcode As HiddenField = gvrow.FindControl("hdnmealcode")
                    '    Dim chkmeal As CheckBox = gvrow.FindControl("chkmeal")
                    '    Dim ddlUpgrademeal As HtmlSelect = gvrow.FindControl("ddlUpgrademeal")

                    '    If chkmeal.Checked = True Then

                    '        mySqlCmd = New SqlCommand("sp_add_offers_meal", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure


                    '        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtmealcode.Text, String)
                    '        If UCase(promolistnew).ToUpper.Trim.Contains(UCase("Meal Upgrade").ToUpper.Trim) = True Then
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@mealupgrade", SqlDbType.VarChar, 20)).Value = CType(ddlUpgrademeal.Items(ddlUpgrademeal.SelectedIndex).Value, String) '  CType(hdnmealcode.Value, String)
                    '        Else
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@mealupgrade", SqlDbType.VarChar, 20)).Value = ""
                    '        End If


                    '        mySqlCmd.ExecuteNonQuery()
                    '        mySqlCmd.Dispose() 'command disposed

                    '    End If
                    'Next


                    ''' Accomodation
                    ''' 
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_offers_accomodation Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()


                    'For Each gvrow In grdrmcat.Rows

                    '    Dim txtrmcatcode As Label = gvrow.FindControl("txtrmcatcode")
                    '    Dim hdnrmcatcode As HiddenField = gvrow.FindControl("hdnrmcatcode")
                    '    Dim chkrmcat As CheckBox = gvrow.FindControl("chkrmcat")
                    '    Dim ddlUpgradermcat As HtmlSelect = gvrow.FindControl("ddlUpgradermcat")

                    '    If chkrmcat.Checked = True Then

                    '        mySqlCmd = New SqlCommand("sp_add_offers_accomodation", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure


                    '        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(txtrmcatcode.Text, String)
                    '        If UCase(promolistnew).ToUpper.Trim.Contains(UCase("Accomodation Upgrade").ToUpper.Trim) = True Then
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatupgrade", SqlDbType.VarChar, 20)).Value = CType(ddlUpgradermcat.Items(ddlUpgradermcat.SelectedIndex).Value, String) 'CType(hdnrmcatcode.Value, String)
                    '        Else
                    '            mySqlCmd.Parameters.Add(New SqlParameter("@rmcatupgrade", SqlDbType.VarChar, 20)).Value = ""
                    '        End If


                    '        mySqlCmd.ExecuteNonQuery()
                    '        mySqlCmd.Dispose() 'command disposed

                    '    End If
                    'Next


                    ''' Supplement
                    ''' 
                    'mySqlCmd = New SqlCommand("DELETE FROM edit_offers_supplement Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()

                    'For Each gvrow In grdsupplement.Rows

                    '    Dim lblrmcatcode As Label = gvrow.FindControl("lblrmcatcode")

                    '    Dim chksuprmcat As CheckBox = gvrow.FindControl("chksuprmcat")

                    '    If chksuprmcat.Checked = True Then

                    '        mySqlCmd = New SqlCommand("sp_add_offers_supplement", mySqlConn, sqlTrans)
                    '        mySqlCmd.CommandType = CommandType.StoredProcedure


                    '        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(lblrmcatcode.Text, String)


                    '        mySqlCmd.ExecuteNonQuery()
                    '        mySqlCmd.Dispose() 'command disposed

                    '    End If
                    'Next


                    ''' Arrival Transfers
                    ''' 
                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersTransfers Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "' and transfertype='Arrival'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    For Each gvrow In grdArrivalTransfer.Rows

                        Dim txtarrivalterminal As TextBox = gvrow.FindControl("txtarrivalterminal")
                        Dim txtflightcode As TextBox = gvrow.FindControl("txtflightcode")

                        If txtarrivalterminal.Text <> "" And chkarrival.Checked = True And UCase(promolistnew).ToUpper.Trim.Contains(UCase("Complimentary Airport Transfer").ToUpper.Trim) = True Then

                            mySqlCmd = New SqlCommand("sp_add_offers_transfers", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 20)).Value = IIf(chkarrival.Checked = True, "Arrival", "")
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportcode", SqlDbType.VarChar, 20)).Value = CType(txtarrivalterminal.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@flightcode", SqlDbType.VarChar, 8000)).Value = CType(txtflightcode.Text, String)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If
                    Next

                    ''' Departure Transfers
                    ''' 
                    mySqlCmd = New SqlCommand("DELETE FROM New_edit_OffersTransfers Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "' and transfertype='Departure'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    For Each gvrow In grdDepartureTransfer.Rows

                        Dim txtdepartureterminal As TextBox = gvrow.FindControl("txtdepartureterminal")
                        Dim txtdepflightcode As TextBox = gvrow.FindControl("txtdepflightcode")

                        If txtdepartureterminal.Text <> "" And chkdeparture.Checked = True And UCase(promolistnew).ToUpper.Trim.Contains(UCase("Complimentary Airport Transfer").ToUpper.Trim) = True Then

                            mySqlCmd = New SqlCommand("sp_add_offers_transfers", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 20)).Value = IIf(chkdeparture.Checked = True, "Departure", "")
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportcode", SqlDbType.VarChar, 20)).Value = CType(txtdepartureterminal.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@flightcode", SqlDbType.VarChar, 8000)).Value = CType(txtdepflightcode.Text, String)

                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If
                    Next

                    ''' Flights
                    ''' 
                    mySqlCmd = New SqlCommand("DELETE FROM edit_offers_flight Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    For Each gvrow In grdflight.Rows

                        Dim txtflightcode As TextBox = gvrow.FindControl("txtflightcode")

                        If txtflightcode.Text <> "" And UCase(promolistnew).ToUpper.Trim.Contains(UCase("Select flights only").ToUpper.Trim) = True Then

                            mySqlCmd = New SqlCommand("sp_add_offers_flight", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@flightcode", SqlDbType.VarChar, 20)).Value = CType(txtflightcode.Text, String)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If
                    Next

                    '''' Combinable
                    If ddlcombine.Value.ToUpper = "Combinable with Specific".ToUpper Or ddlcombine.Value.ToUpper = "Combine Mandatory With".ToUpper Then

                        mySqlCmd = New SqlCommand("DELETE FROM edit_offers_combinable Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
                        mySqlCmd.ExecuteNonQuery()

                        For Each gvrow In grdcombinable.Rows

                            Dim txtpromotioncode As TextBox = gvrow.FindControl("txtpromotioncode")

                            If txtpromotioncode.Text <> "" Then

                                mySqlCmd = New SqlCommand("sp_add_offers_combinable", mySqlConn, sqlTrans)
                                mySqlCmd.CommandType = CommandType.StoredProcedure
                                mySqlCmd.CommandTimeout = 0

                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                                mySqlCmd.Parameters.Add(New SqlParameter("@combinableid", SqlDbType.VarChar, 20)).Value = CType(txtpromotioncode.Text, String)

                                mySqlCmd.ExecuteNonQuery()
                                mySqlCmd.Dispose() 'command disposed

                            End If

                        Next

                    End If


                    ''''''


                    ''' Inter Hotel
                    ''' 
                    mySqlCmd = New SqlCommand("DELETE FROM edit_offers_interhotel Where promotionid='" & CType(txtpromotionid.Text.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()


                    For Each gvrow In grdinterhotel.Rows

                        Dim txtinterhotelcode As TextBox = gvrow.FindControl("txtinterhotelcode")
                        Dim txtinterminnights As TextBox = gvrow.FindControl("txtinterminnights")

                        If txtinterhotelcode.Text <> "" And UCase(promolistnew).ToUpper.Trim.Contains(UCase("Inter Hotels").ToUpper.Trim) = True Then

                            mySqlCmd = New SqlCommand("sp_add_offers_interhotel", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.CommandTimeout = 0

                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(txtinterhotelcode.Text, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@minstay", SqlDbType.Int)).Value = CType(Val(txtinterminnights.Text), Integer)
                            mySqlCmd.ExecuteNonQuery()
                            mySqlCmd.Dispose() 'command disposed

                        End If
                    Next

                    strMsg = "Saved Succesfully!!"

                ElseIf Session("OfferState") = "Delete" Then

                    If txtpromotionid.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Promotion ID Should not be Empty .');", True)
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    'delete for row tables present in sp
                    mySqlCmd = New SqlCommand("sp_del_offers_header", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(txtpromotionid.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    strMsg = "Delete  Succesfully!!"
                End If


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed

                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                Session("OfferState") = ""
                wucCountrygroup.clearsessions()


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "' );", True)

                ModalPopupDays.Hide()  '' Added shahul 08/08/18

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region


#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session("OfferState") = Nothing
        ' wucCountrygroup.clearsessions()
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub gridrmtype_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridrmtype.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlUpgradermtyp As HtmlSelect
                Dim hdnrmtypcode As HiddenField
                Dim txtrmtypcode As Label
                Dim lblrankorder As Label

                ddlUpgradermtyp = e.Row.FindControl("ddlUpgradermtyp")
                hdnrmtypcode = e.Row.FindControl("hdnrmtypcode")
                txtrmtypcode = e.Row.FindControl("txtrmtypcode")
                lblrankorder = e.Row.FindControl("lblrankorder")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermtyp, "rmtypname", "rmtypcode", "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and rmtypcode<>'" & txtrmtypcode.Text & "'   and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermtyp, "rmtypname", "rmtypcode", "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and rankord >'" & lblrankorder.Text & "'   and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)", True)

                Dim rowid As String
                rowid = CType(e.Row.RowIndex, String)
                ddlUpgradermtyp.Attributes.Add("Onchange", "Javascript:selectrmtype('" + ddlUpgradermtyp.ClientID + "','" + hdnrmtypcode.ClientID + "')")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdmealplan_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdmealplan.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlUpgrademeal As HtmlSelect
                Dim hdnmealcode As HiddenField
                Dim lblmealcode As Label
                Dim lblrankorder As Label

                ddlUpgrademeal = e.Row.FindControl("ddlUpgrademeal")
                hdnmealcode = e.Row.FindControl("hdnmealcode")
                lblmealcode = e.Row.FindControl("txtmealcode")
                lblrankorder = e.Row.FindControl("lblrankorder")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgrademeal, "mealname", "mealcode", "select p.mealcode as mealcode,m.mealname mealname from  partymeal p(nolock),mealmast m(nolock),mainmealplans mp where m.mainmealcode=mp.mainmealcode and   p.mealcode=m.mealcode and mp.rankorder >'" & lblrankorder.Text & "' and  p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)", True)
                Dim rowid As String
                rowid = CType(e.Row.RowIndex, String)
                ddlUpgrademeal.Attributes.Add("Onchange", "Javascript:selectmeal('" + ddlUpgrademeal.ClientID + "','" + hdnmealcode.ClientID + "')")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdrmcat_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdrmcat.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlUpgradermcat As HtmlSelect
                Dim hdnrmcatcode As HiddenField
                Dim lblrmcatcode As Label
                Dim lblrankorder As Label

                ddlUpgradermcat = e.Row.FindControl("ddlUpgradermcat")
                hdnrmcatcode = e.Row.FindControl("hdnrmcatcode")
                lblrmcatcode = e.Row.FindControl("txtrmcatcode")
                lblrankorder = e.Row.FindControl("lblrankorder")

                ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermcat, "rmcatcode", "rmcatcode", "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and prc.rmcatcode<>'" & lblrmcatcode.Text & "' and rc.accom_extra='A' and prc.partycode='" _
                '        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermcat, "rmcatcode", "rmcatcode", "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and isnull(rc.units,0) >'" & lblrankorder.Text & "' and rc.accom_extra='A' and prc.partycode='" _
                      & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)", True)

                Dim rowid As String
                rowid = CType(e.Row.RowIndex, String)
                ddlUpgradermcat.Attributes.Add("Onchange", "Javascript:selectrmcat('" + ddlUpgradermcat.ClientID + "','" + hdnrmcatcode.ClientID + "')")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub grdsupplement_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdsupplement.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlUpgradermcatsup As HtmlSelect
                Dim hdnrmcatcodesup As HiddenField
                Dim lblrmcatcode As Label

                ddlUpgradermcatsup = e.Row.FindControl("ddlUpgradermcatsup")
                hdnrmcatcodesup = e.Row.FindControl("hdnrmcatcodesup")
                lblrmcatcode = e.Row.FindControl("lblrmcatcode")



                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermcatsup, "rmcatcode", "rmcatcode", "select prc.rmcatcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and prc.rmcatcode<>'" & lblrmcatcode.Text & "' and rc.accom_extra='M' and prc.partycode='" _
                        & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)", True)
                Dim rowid As String
                rowid = CType(e.Row.RowIndex, String)
                ddlUpgradermcatsup.Attributes.Add("Onchange", "Javascript:selectrmcatsup('" + ddlUpgradermcatsup.ClientID + "','" + hdnrmcatcodesup.ClientID + "')")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btncombination_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex


        Dim strroomno As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtnoofrooms"), TextBox).Text
        hdnMainGridRowid.Value = rowid



        ChkExistingVal(strroomno)

        ModalRoomPopup.PopupControlID = divcombinations.ClientID
        ModalRoomPopup.Show()

    End Sub

    Private Sub ChkExistingVal(ByVal prm_strChkdVal As String)
        Dim chkSelect As HtmlInputCheckBox
        Dim intAdult, intChild, intRoomno As Integer

        Dim arrString As String()
        Dim j As Integer = 0 'string in the form "a,b;c,d;...."

        If prm_strChkdVal <> "" Then
            prm_strChkdVal = prm_strChkdVal.Substring(0, prm_strChkdVal.Length - 1)

            arrString = prm_strChkdVal.Split(",") 'spliting for ';' 1st


            fillDategrd(grdcombinations, False, arrString.Length)


            Dim i As Integer = 0
            For Each grdRow In grdcombinations.Rows
                Dim txtroomno As TextBox = grdRow.FindControl("txtroomno")
                Dim txtadults As TextBox = grdRow.FindControl("txtadults")
                Dim txtchild As TextBox = grdRow.FindControl("txtchild")

                Dim arrAdultChild As String() = arrString(i).Split("/") 'spliting for ',' 2nd
                If arrAdultChild(i) <> "" Then

                    txtroomno.Text = arrAdultChild(0)
                    txtadults.Text = arrAdultChild(1)
                    txtchild.Text = arrAdultChild(2)

                End If

                i += 1

            Next

            'For k = 0 To arrString.Length - 1

            '    Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

            '    If arrAdultChild(k) <> "" Then
            '        For Each grdRow In grdcombinations.Rows
            '            Dim txtroomno As TextBox = grdRow.FindControl("txtroomno")
            '            Dim txtadults As TextBox = grdRow.FindControl("txtadults")
            '            Dim txtchild As TextBox = grdRow.FindControl("txtchild")
            '            'Get  adult n child for comparing string
            '            txtroomno.Text = arrAdultChild(0)
            '            txtadults.Text = arrAdultChild(1)
            '            txtchild.Text = arrAdultChild(2)

            '        Next
            '    End If
            'Next
        Else
            fillDategrd(grdcombinations, False)


        End If
    End Sub
    Protected Sub btngAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim reqstr As String = ""


            gridrmtype.Columns(3).Visible = False
            gv_Showroomtypes.Columns(4).Visible = False
            grdmealplan.Columns(3).Visible = False
            grdrmcat.Columns(3).Visible = False

            grdpromotiondetail.Columns(4).Visible = False
            grdpromotiondetail.Columns(5).Visible = False
            grdpromotiondetail.Columns(6).Visible = False
            grdpromotiondetail.Columns(1).Visible = True
            grdpromotiondetail.Columns(2).Visible = True



            divflight.Style.Add("display", "none")
            divsplocc.Style.Add("display", "none")
            divinter.Style.Add("display", "none")
            divstay.Style.Add("display", "none")
            'divpromodates.Style.Add("display", "block")
            divcomptrf.Style.Add("display", "none")
            divArrivalTransfer.Style.Add("display", "none")
            divDepartureTransfer.Style.Add("display", "none")

            ' divapplydiscount.Style.Add("display", "none")

            Dim checkedrow As Integer = 0
            Dim promoList As String = String.Empty
            Dim count As Integer = 0

            'For Each row As GridViewRow In gv_promotype.Rows
            '    'commented by Elsitta on 13.11.2016
            '    'For Each row As datalistrow In GridView

            '    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkpromotiontype"), CheckBox)

            '    If chkRow.Checked = True Then

            '        Dim lbltype As String = row.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text



            '        Dim strpop As String = ""
            '        checkedrow = checkedrow + 1
            '        If count = 0 Then
            '            promoList = lbltype
            '            count = 1
            '        Else
            '            promoList &= "|" & lbltype
            '        End If

            '    End If
            'Next


            'below code added by Elsitta on 14.11.2016 because gridiew design changed into datalist
            For Each item As DataListItem In dlPromotionType.Items
                Dim chkRow As CheckBox = TryCast(item.FindControl("chkpromotiontype"), CheckBox)
                Dim txtbox As Label = TryCast(item.FindControl("txtpromtoiontype"), Label)
                If chkRow.Checked = True Then
                    Dim lbltype As String = txtbox.Text
                    'item.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
                    Dim strpop As String = ""
                    checkedrow = checkedrow + 1
                    If count = 0 Then
                        promoList = lbltype
                        count = 1
                    Else
                        promoList &= "|" & lbltype
                    End If
                End If
            Next

            'above code added by Elsitta on 14.11.2016 because gridview design changed to datalist













            Dim strCondition As String = ""
            If promoList.Length <> 0 Then

                Dim mString As String() = promoList.Split("|")
                For i As Integer = 0 To mString.Length - 1

                    If UCase(mString(i)) = UCase("Room Upgrade") Then

                        gridrmtype.Columns(3).Visible = True
                        gv_Showroomtypes.Columns(4).Visible = True

                    ElseIf UCase(mString(i)) = UCase("Meal Upgrade") Then

                        grdmealplan.Columns(3).Visible = True

                    ElseIf UCase(mString(i)) = UCase("Accomodation Upgrade") Then
                        grdrmcat.Columns(3).Visible = True
                    ElseIf UCase(mString(i)) = UCase("Early Bird Discount") Then
                        grdpromotiondetail.Columns(4).Visible = True
                        grdpromotiondetail.Columns(5).Visible = True
                        grdpromotiondetail.Columns(6).Visible = True
                        grdpromotiondetail.Columns(1).Visible = True
                        grdpromotiondetail.Columns(2).Visible = True

                        '  divapplydiscount.Style.Add("display", "block")

                        ' divpromodates.Style.Add("display", "none")


                    ElseIf UCase(mString(i)) = UCase("Select flights only") Then
                        divflight.Style.Add("display", "block")
                    ElseIf UCase(mString(i)) = UCase("Special Occasion") Then
                        divsplocc.Style.Add("display", "block")
                    ElseIf UCase(mString(i)) = UCase("Inter Hotels") Then
                        divinter.Style.Add("display", "block")
                    ElseIf UCase(mString(i)) = UCase("Free Nights") Then
                        divstay.Style.Add("display", "block")
                    ElseIf UCase(mString(i)) = UCase("Complimentary Airport Transfer") Then
                        divcomptrf.Style.Add("display", "block")

                    End If

                Next
            End If




        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (hdnpartycode.Value.Trim <> "") Then
            Dim myDataAdapter As SqlDataAdapter
            gvShowdates.Visible = True
            chkSeason.Checked = False
            chkApplicable.Checked = False
            Dim MyDs As New DataTable
            Dim countryList As String = ""
            Dim agentList As String = ""
            Dim filterCond As String = ""
            If wucCountrygroup.checkcountrylist.ToString().Trim <> "" Then
                countryList = wucCountrygroup.checkcountrylist.ToString().Trim.Replace(",", "','")
                filterCond = "c.contractid  in (select contractid from vw_edit_contracts_countries where ctrycode in (' " + countryList + "'))"
            End If
            If wucCountrygroup.checkagentlist.ToString().Trim <> "" Then
                agentList = wucCountrygroup.checkagentlist.ToString().Trim.Replace(",", "','")
                If filterCond <> "" Then
                    filterCond = filterCond + " or c.contractid  in (select contractid from vw_contracts_agents where agentcode in ( '" + agentList + "'))"
                Else
                    filterCond = "c.contractid  in (select contractid from vw_contracts_agents where agentcode in ( '" + agentList + "'))"
                End If
            End If
            If filterCond <> "" Then
                filterCond = " and (" + filterCond + ")"
            End If
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = " select c.contractid ,c.applicableto,v.SeasonName ,convert(varchar(10),v.fromdate,103) fromdate ,convert(varchar(10),v.todate,103) todate  from view_contractseasons v ,view_contracts_search c " _
                          & " where v.contractid =c.contractid  and c.partycode ='" + hdnpartycode.Value.Trim + "' and convert(varchar(10),v.fromdate,111) >=GETDATE() " + filterCond + " order by convert(varchar(10),v.fromdate,111),convert(varchar(10),v.todate,111)"
            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            myDataAdapter.Fill(MyDs)
            If MyDs.Rows.Count > 0 Then
                gvShowdates.DataSource = MyDs
                gvShowdates.DataBind()
                gvShowdates.Visible = True
            Else
                gvShowdates.Visible = False
            End If

            ModalExtraPopup1.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Hotel Name' );", True)
            Exit Sub
        End If
    End Sub

    Protected Sub btnSelectDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectDate.Click

        Try
            Dim dtStayPeriod As New DataTable
            Dim drStayPeriod As DataRow
            dtStayPeriod.Columns.Add(New DataColumn("FromDate", GetType(Date)))
            dtStayPeriod.Columns.Add(New DataColumn("ToDate", GetType(Date)))
            Dim chkSelect As CheckBox
            Dim txtFrDat As TextBox
            Dim txtToDat As TextBox

            For Each sdRow As GridViewRow In grdpromotiondetail.Rows
                txtFrDat = CType(sdRow.FindControl("txtfromDate"), TextBox)
                txtToDat = CType(sdRow.FindControl("txtToDate"), TextBox)
                'If (chkSelect.Checked = True) Then
                drStayPeriod = dtStayPeriod.NewRow
                If txtFrDat.Text.Trim().Length > 0 Then
                    drStayPeriod("FromDate") = CType(txtFrDat.Text.Trim(), Date)
                    'drStayPeriod("FromDate") = CType(sdRow.Cells(1).Text, Date)
                    drStayPeriod("ToDate") = CType(txtToDat.Text, Date)
                    dtStayPeriod.Rows.Add(drStayPeriod)

                End If
            Next

            For Each sdRow As GridViewRow In gvShowdates.Rows
                chkSelect = CType(sdRow.FindControl("chkdateselect"), CheckBox)
                If (chkSelect.Checked = True) Then
                    drStayPeriod = dtStayPeriod.NewRow
                    drStayPeriod("FromDate") = CType(sdRow.Cells(5).Text, Date)
                    drStayPeriod("ToDate") = CType(sdRow.Cells(6).Text, Date)
                    dtStayPeriod.Rows.Add(drStayPeriod)
                End If
            Next


            If dtStayPeriod.Rows.Count > 0 Then
                grdpromotiondetail.DataSource = dtStayPeriod
                grdpromotiondetail.DataBind()
            End If
            Session("GridVal") = ""
            Session("Checks") = ""
            Session("frmdate") = ""
            Session("tdate") = ""
            ModalExtraPopup1.Hide()

            Bookingoptionchange()
        Catch ex As Exception

        End Try


    End Sub
    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click

        Session("GridVal") = ""
        Session("Checks") = ""
        ViewState("Upgrade") = Nothing

        ModalExtraPopup1.Hide()

    End Sub



    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)




        ModalExtraPopup1.Show()
        If Session("Checks") = "" Then
            Session("GridVal") = ""
            For Each gr As GridViewRow In gvShowdates.Rows
                If gr.RowType = DataControlRowType.DataRow Then
                    chknew = CType(gr.FindControl("chkdateselect"), CheckBox)
                    If chknew.Checked = True Then
                        'If Not seasonListnew.Contains(gr.Cells(3).Text.Trim) Or seasonListnew.Contains(gr.Cells(4).Text.Trim) Then
                        '    'seasonListnew.Add(gr.Cells(3).Text.Trim)

                        If Session("GridVal").ToString().Length > 0 Then
                            Session("GridVal") = Session("GridVal") + ","
                        End If
                        Session("GridVal") = Session("GridVal") + gr.Cells(3).Text.Trim() + " ! " + gr.Cells(4).Text.Trim()
                        'End If
                    End If
                End If
            Next
        End If

        'Dim chknew As CheckBox
        'If chkApplicable.Checked = True Then

        '    For Each gr As GridViewRow In gvShowdates.Rows
        '        If gr.RowType = DataControlRowType.DataRow Then
        '            chknew = CType(gr.FindControl("chkdateselect"), CheckBox)
        '            If chknew.Checked = True Then
        '                If Not seasonListnew.Contains(gr.Cells(3).Text.Trim) Then
        '            seasonListnew.Add(gr.Cells(3).Text.Trim)
        '                End If
        '            End If
        '        End If
        '    Next


        'ElseIf chkSeason.Checked = True Then
        '    For Each gr As GridViewRow In gvShowdates.Rows
        '        If gr.RowType = DataControlRowType.DataRow Then
        '            chknew = CType(gr.FindControl("chkdateselect"), CheckBox)
        '            If chknew.Checked = True Then
        '                If Not seasonListnew.Contains(gr.Cells(4).Text.Trim) Then
        '                    seasonListnew.Add(gr.Cells(4).Text.Trim)
        '                End If
        '            End If
        '        End If
        '    Next
        'Else
        '    For Each gr As GridViewRow In gvShowdates.Rows
        '        If gr.RowType = DataControlRowType.DataRow Then
        '            chknew = CType(gr.FindControl("chkdateselect"), CheckBox)
        '            If chknew.Checked = True Then
        '                If Not seasonListnew.Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then
        '                    seasonListnew.Add(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim)
        '                End If
        '            End If
        '        End If
        '    Next
        'End If

    End Sub



    Protected Sub AvailableSeason(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        If chkApplicable.Checked = True And chkSeason.Checked = True Then
            Session("Checks") = "true"

            For Each gr As GridViewRow In gvShowdates.Rows
                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
                If Session("GridVal").ToString().Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then

                    If chkSeason.Checked = True Then
                        chk.Checked = True
                    Else
                        chk.Checked = False
                    End If
                Else
                    chk.Checked = False
                End If
            Next
        ElseIf chkSeason.Checked = True Then

            Session("Checks") = "true"
            'If seasonListnew.Count > 0 Then
            For Each gr As GridViewRow In gvShowdates.Rows
                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
                If Session("GridVal").ToString().Contains(gr.Cells(4).Text.Trim) Then

                    If chkSeason.Checked = True Then
                        chk.Checked = True
                    Else
                        chk.Checked = False
                    End If
                Else
                    chk.Checked = False
                End If
            Next

        ElseIf chkApplicable.Checked = True Then
            Session("Checks") = "true"
            For Each gr As GridViewRow In gvShowdates.Rows
                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
                If Session("GridVal").ToString().Contains(gr.Cells(3).Text.Trim) Then

                    If chkApplicable.Checked = True Then
                        chk.Checked = True
                    Else
                        chk.Checked = False
                    End If
                Else
                    chk.Checked = False
                End If
            Next

        Else
            For Each gr As GridViewRow In gvShowdates.Rows
                chk = CType(gr.FindControl("chkdateselect"), CheckBox)


                chk.Checked = False

            Next
            Session("GridVal") = ""
            Session("Checks") = ""
        End If
        ModalExtraPopup1.Show()
    End Sub
    'Protected Sub chkApplicable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkApplicable.CheckedChanged
    '    Dim chk As CheckBox
    '    Dim ApplicableToList As New List(Of String)

    '    If chkSeason.Checked = True Then


    '        For Each gr As GridViewRow In gvShowdates.Rows
    '            If gr.RowType = DataControlRowType.DataRow Then
    '                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                If chk.Checked = True Then
    '                    If Not ApplicableToList.Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then
    '                        ApplicableToList.Add(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim)
    '                    End If
    '                End If
    '            End If
    '        Next
    '        If ApplicableToList.Count > 0 Then
    '            For Each gr As GridViewRow In gvShowdates.Rows
    '                If ApplicableToList.Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then
    '                    chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                    If chkApplicable.Checked = True Then
    '                        chk.Checked = True
    '                    Else
    '                        chk.Checked = False
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Else

    '        For Each gr As GridViewRow In gvShowdates.Rows
    '            If gr.RowType = DataControlRowType.DataRow Then
    '                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                If chk.Checked = True Then
    '                    If Not ApplicableToList.Contains(gr.Cells(3).Text.Trim) Then
    '                        ApplicableToList.Add(gr.Cells(3).Text.Trim)
    '                    End If
    '                End If
    '            End If
    '        Next
    '        If ApplicableToList.Count > 0 Then
    '            For Each gr As GridViewRow In gvShowdates.Rows
    '                If ApplicableToList.Contains(gr.Cells(3).Text.Trim) Then
    '                    chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                    If chkApplicable.Checked = True Then
    '                        chk.Checked = True
    '                    Else
    '                        chk.Checked = False
    '                    End If
    '                End If
    '            Next
    '        End If
    '    End If

    '    ModalExtraPopup1.Show()
    'End Sub
    'Protected Sub chkApplicable_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkApplicable.CheckedChanged
    '    Dim chk As CheckBox
    '    Dim ApplicableToList As New List(Of String)

    '    If chkSeason.Checked = True Then


    '        'For Each gr As GridViewRow In gvShowdates.Rows
    '        '    If gr.RowType = DataControlRowType.DataRow Then
    '        '        chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '        '        If chk.Checked = True Then
    '        '            If Not ApplicableToList.Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then
    '        '                ApplicableToList.Add(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim)
    '        '            End If
    '        '        End If
    '        '    End If
    '        'Next
    '        If seasonListnew.Count > 0 Then
    '            For Each gr As GridViewRow In gvShowdates.Rows
    '                If seasonListnew.Contains(gr.Cells(3).Text.Trim + " ! " + gr.Cells(4).Text.Trim) Then
    '                    chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                    If chkApplicable.Checked = True Then
    '                        chk.Checked = True
    '                    Else
    '                        chk.Checked = False
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Else

    '        'For Each gr As GridViewRow In gvShowdates.Rows
    '        '    If gr.RowType = DataControlRowType.DataRow Then
    '        '        chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '        '        If chk.Checked = True Then
    '        '            If Not ApplicableToList.Contains(gr.Cells(3).Text.Trim) Then
    '        '                ApplicableToList.Add(gr.Cells(3).Text.Trim)
    '        '            End If
    '        '        End If
    '        '    End If
    '        'Next
    '        'If Session("GrdVal").ToString().Length > 0 Then

    '        'End If
    '        Session("Checks") = "true"

    '        For Each gr As GridViewRow In gvShowdates.Rows
    '            If Session("GridVal").ToString().Contains(gr.Cells(3).Text.Trim) Then
    '                chk = CType(gr.FindControl("chkdateselect"), CheckBox)
    '                If chkApplicable.Checked = True Then
    '                    chk.Checked = True
    '                Else
    '                    chk.Checked = False
    '                End If
    '            End If
    '        Next

    '        Session("Checks") = ""
    '    End If

    '    ModalExtraPopup1.Show()
    'End Sub
    'Protected Sub gv_promotype_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_promotype.RowDataBound
    '    Try
    '        Dim rowid As String
    '        Dim promotiontype As String = ""
    '        If e.Row.RowType = DataControlRowType.DataRow Then

    '            Dim chkprmtype As CheckBox
    '            Dim chkAll As CheckBox
    '            chkprmtype = e.Row.FindControl("chkpromotiontype")
    '            chkAll = gv_promotype.HeaderRow.FindControl("chkAll")
    '            promotiontype = e.Row.Cells(2).Text

    '            rowid = CType(e.Row.RowIndex, String)
    '            chkprmtype.Attributes.Add("onChange", "showcontrolfill('" & chkprmtype.ClientID & "')")
    '            chkAll.Attributes.Add("onChange", "showcontrolfill('" & chkprmtype.ClientID & "')")

    '            'chkprmtype.Attributes.Add("onChange", "hidecolumn('" & CType(promotiontype, String) & "', '" + CType(e.Row.RowIndex, String) + "')")

    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click

    '    gv_Showdates.Visible = True
    '    Dim MyDs As New DataTable
    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

    '    strSqlQry = " select c.contractid ,c.applicableto,v.SeasonName ,convert(varchar(10),v.fromdate,103) fromdate ,convert(varchar(10),v.todate,103) todate  from view_contractseasons v ,view_contracts_search c " _
    '                  & " where v.contractid =c.contractid  and c.partycode ='CR02' and convert(varchar(10),v.fromdate,111) >=GETDATE()   order by convert(varchar(10),v.fromdate,111),convert(varchar(10),v.todate,111)"




    '    mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
    '    myDataAdapter.Fill(MyDs)

    '    If MyDs.Rows.Count > 0 Then
    '        gv_Showdates.DataSource = MyDs
    '        gv_Showdates.DataBind()
    '        gv_Showdates.Visible = True

    '    Else

    '        gv_Showdates.Visible = False

    '    End If


    '    ModalExtraPopup.Show()
    'End Sub


    'Protected Sub btnsavedate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsavedate.Click
    '    Try

    '        Dim chkSelect As CheckBox
    '        Dim tickedornot As Boolean

    '        tickedornot = False
    '        For Each grdRow In gv_Showdates.Rows
    '            chkSelect = CType(grdRow.FindControl("chkdateselect"), CheckBox)
    '            chkSelect = grdRow.FindControl("chkdateselect")
    '            If chkSelect.Checked = True Then
    '                tickedornot = True
    '                Exit For
    '            End If
    '        Next

    '        If tickedornot = False Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one Row');", True)
    '            ModalExtraPopup.Show()
    '            Exit Sub
    '        End If


    '        Dim chk2 As CheckBox

    '        Dim seasondates As String = ""
    '        Dim fromdate As String = ""
    '        Dim todate As String = ""

    '        For Each gvRow As GridViewRow In gv_Showdates.Rows
    '            chk2 = gvRow.FindControl("chkdateselect")
    '            fromdate = gvRow.Cells(5).Text
    '            todate = gvRow.Cells(6).Text

    '            fromdate = fromdate + "," + todate

    '            If chk2.Checked = True Then
    '                seasondates = seasondates + fromdate + "|"

    '            End If
    '        Next
    '        seasondates = seasondates.Substring(0, seasondates.Length - 1)

    '        Dim dpFDate As TextBox
    '        Dim dpTDate As TextBox

    '        Dim promodates As String() = seasondates.ToString.Trim.Split("|")
    '        'fillDategrd(grdpromodates, False, promodates.Length)
    '        For i = 0 To promodates.Length - 1

    '            Dim fromtodate As String() = promodates(i).Split(",")

    '            'For Each gvRow In grdpromodates.Rows
    '            '    dpFDate = gvRow.FindControl("txtfromdate")
    '            '    dpTDate = gvRow.FindControl("txttodate")
    '            '    If dpFDate.Text = "" And dpFDate.Text = "" Then

    '            '        dpFDate.Text = CType(Format(CType(fromtodate(0), Date), "dd/MM/yyyy"), String)
    '            '        dpTDate.Text = CType(Format(CType(fromtodate(1), Date), "dd/MM/yyyy"), String)

    '            '        Exit For
    '            '    End If
    '            'Next
    '        Next


    '        ModalExtraPopup.Hide()

    '    Catch ex As Exception

    '    End Try
    'End Sub
    Protected Sub BookingoptionSelectedchange(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ddloptions1 As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType((CType(sender, DropDownList)).NamingContainer, GridViewRow)

        Dim ddlselect As DropDownList = CType(row.FindControl("ddlbookoptions"), DropDownList)
        Dim txtbookfromDate As TextBox = CType(row.FindControl("txtbookfromDate"), TextBox)
        Dim txtBookToDate As TextBox = CType(row.FindControl("txtBookToDate"), TextBox)
        Dim txtbookdays As TextBox = CType(row.FindControl("txtbookdays"), TextBox)

        Dim ImgBtnBookFrmDt As ImageButton = CType(row.FindControl("ImgBtnBookFrmDt"), ImageButton)
        Dim ImgBtnBookToDt As ImageButton = CType(row.FindControl("ImgBtnBookToDt"), ImageButton)

        If ddlselect.SelectedIndex = 1 Or ddlselect.SelectedIndex = 2 Then

            grdpromotiondetail.Columns(14).Visible = True
            grdpromotiondetail.Columns(15).Visible = True
            grdpromotiondetail.Columns(16).Visible = True

            txtbookdays.Enabled = True
            txtbookfromDate.Enabled = False
            txtBookToDate.Enabled = False

            txtbookfromDate.Text = ""
            txtBookToDate.Text = ""

        ElseIf ddlselect.SelectedIndex = 3 Then
            grdpromotiondetail.Columns(14).Visible = True
            grdpromotiondetail.Columns(15).Visible = True
            grdpromotiondetail.Columns(16).Visible = False

            txtbookdays.Enabled = False
            txtbookfromDate.Enabled = True
            txtBookToDate.Enabled = True

            txtbookdays.Text = ""
            ImgBtnBookFrmDt.Enabled = True
            ImgBtnBookToDt.Enabled = True

        ElseIf ddlselect.SelectedIndex = 1 Then
            grdpromotiondetail.Columns(14).Visible = True
            grdpromotiondetail.Columns(15).Visible = False
            grdpromotiondetail.Columns(16).Visible = False

            txtbookdays.Enabled = False
            txtbookfromDate.Enabled = True
            txtBookToDate.Enabled = False

            txtBookToDate.Text = ""
            txtbookdays.Text = ""

            ImgBtnBookFrmDt.Enabled = True
            ImgBtnBookToDt.Enabled = False

        ElseIf ddlselect.SelectedIndex = 0 Then
            grdpromotiondetail.Columns(15).Visible = True
            grdpromotiondetail.Columns(16).Visible = False
            grdpromotiondetail.Columns(14).Visible = True

            txtbookdays.Enabled = False
            txtbookfromDate.Enabled = True
            txtBookToDate.Enabled = False

            txtBookToDate.Text = ""
            txtbookdays.Text = ""

            ImgBtnBookFrmDt.Enabled = True
            ImgBtnBookToDt.Enabled = False

        ElseIf ddlselect.SelectedIndex = 4 Then

            txtbookdays.Enabled = False
            txtbookfromDate.Enabled = False
            txtBookToDate.Enabled = False

            txtbookfromDate.Text = ""
            txtBookToDate.Text = ""
            txtbookdays.Text = ""

            ImgBtnBookFrmDt.Enabled = False
            ImgBtnBookToDt.Enabled = False

        ElseIf ddlselect.SelectedIndex = 5 Then

            grdpromotiondetail.Columns(15).Visible = True
            grdpromotiondetail.Columns(16).Visible = True
            grdpromotiondetail.Columns(14).Visible = True

            txtbookdays.Enabled = True
            txtbookfromDate.Enabled = True
            txtBookToDate.Enabled = False

            txtBookToDate.Text = ""
            txtbookdays.Text = ""

            ImgBtnBookFrmDt.Enabled = True
            ImgBtnBookToDt.Enabled = False

        End If


    End Sub
    Private Sub Bookingoptionchange()

        'Dim ddloptions1 As DropDownList = CType(sender, DropDownList)
        'Dim row As GridViewRow = CType((CType(sender, DropDownList)).NamingContainer, GridViewRow)



        For Each gvrow In grdpromotiondetail.Rows

            Dim ddlselect As DropDownList = CType(gvrow.FindControl("ddlbookoptions"), DropDownList)
            Dim txtbookfromDate As TextBox = CType(gvrow.FindControl("txtbookfromDate"), TextBox)
            Dim txtBookToDate As TextBox = CType(gvrow.FindControl("txtBookToDate"), TextBox)
            Dim txtbookdays As TextBox = CType(gvrow.FindControl("txtbookdays"), TextBox)
            Dim ImgBtnBookFrmDt As ImageButton = CType(gvrow.FindControl("ImgBtnBookFrmDt"), ImageButton)
            Dim ImgBtnBookToDt As ImageButton = CType(gvrow.FindControl("ImgBtnBookToDt"), ImageButton)
           

            If ddlselect.SelectedIndex = 1 Then


                txtbookdays.Enabled = True
                txtbookfromDate.Enabled = False
                txtBookToDate.Enabled = False

                ' txtbookfromDate.Text = ""
                txtBookToDate.Text = ""

                '    txtbookdays.Text = ""
                txtbookdays.Enabled = True
                txtbookfromDate.Enabled = False
                txtBookToDate.Enabled = False

                ImgBtnBookFrmDt.Enabled = False
                ImgBtnBookToDt.Enabled = False

            ElseIf ddlselect.SelectedIndex = 2 Then


                txtbookdays.Enabled = True
                txtbookfromDate.Enabled = False
                txtBookToDate.Enabled = False

                txtbookfromDate.Text = ""
                txtBookToDate.Text = ""

            ElseIf ddlselect.SelectedIndex = 3 Then

                txtbookdays.Enabled = False
                txtbookfromDate.Enabled = True
                txtBookToDate.Enabled = True

                txtbookdays.Text = ""
                ImgBtnBookFrmDt.Enabled = True
                ImgBtnBookToDt.Enabled = True



            ElseIf ddlselect.SelectedIndex = 0 Then

                'txtbookdays.Enabled = False
                'txtbookfromDate.Enabled = True
                'txtBookToDate.Enabled = False

                'txtbookfromDate.Text = ""

                txtbookdays.Enabled = False
                txtbookfromDate.Enabled = True
                txtBookToDate.Enabled = False

                txtBookToDate.Text = ""
                txtbookdays.Text = ""

                ImgBtnBookFrmDt.Enabled = True
                ImgBtnBookToDt.Enabled = False


            ElseIf ddlselect.SelectedIndex = 4 Then

                txtbookdays.Enabled = False
                txtbookfromDate.Enabled = False
                txtBookToDate.Enabled = False

                txtbookfromDate.Text = ""
                txtBookToDate.Text = ""
                txtbookdays.Text = ""

                ImgBtnBookFrmDt.Enabled = False
                ImgBtnBookToDt.Enabled = False

            ElseIf ddlselect.SelectedIndex = 5 Then


                txtbookdays.Enabled = True
                txtbookfromDate.Enabled = True
                txtBookToDate.Enabled = False

                txtBookToDate.Text = ""
                '  txtbookdays.Text = ""
                ' txtbookdays.Text = ""

                ImgBtnBookFrmDt.Enabled = True
                ImgBtnBookToDt.Enabled = False
            End If

        Next




    End Sub
    Protected Sub SelectedItemchange(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ddlbookoptions1 As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType((CType(sender, DropDownList)).NamingContainer, GridViewRow)

        Dim ddlbookoptions As DropDownList = CType(row.FindControl("ddloptions"), DropDownList)


        ''' Commented As per madam email 06/03/17
        'If ddlbookoptions.SelectedIndex = 2 Then

        '    grdpromotiondetail.Columns(7).Visible = True
        '    grdpromotiondetail.Columns(8).Visible = True
        'Else
        '    grdpromotiondetail.Columns(7).Visible = False
        '    grdpromotiondetail.Columns(8).Visible = False

        'End If


    End Sub

#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region

    'Protected Sub grdpromotiondetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdpromotiondetail.RowDataBound
    '    Try

    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim ddloptions As DropDownList
    '            Dim ddlindex As String
    '            Dim txtminstay As TextBox
    '            Dim txtdiscount As TextBox
    '            Dim txtadddiscount As TextBox
    '            Dim txtbookdays As TextBox

    '            ddloptions = e.Row.FindControl("ddloptions")
    '            txtminstay = e.Row.FindControl("txtminnights")
    '            ddlindex = ddloptions.SelectedIndex
    '            txtdiscount = e.Row.FindControl("txtdiscount")
    '            txtadddiscount = e.Row.FindControl("txtadddiscount")
    '            txtbookdays = e.Row.FindControl("txtbookdays")



    '            Numberssrvctrl(txtminstay)
    '            Numberssrvctrl(txtdiscount)
    '            Numberssrvctrl(txtadddiscount)
    '            Numberssrvctrl(txtbookdays)


    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Protected Sub btnaddrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddrow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1
        Dim flightcode(count) As String

        Dim excl(count) As String
        Dim n As Integer = 0
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim bookingcode(count) As String
        Dim discounttype(count) As String
        Dim discountperc(count) As String
        Dim adddiscountperc(count) As String
        Dim noofrooms(count) As String
        Dim multiyes(count) As String
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim minnights(count) As String
        Dim maxnights(count) As String

        Dim txtfromdate As TextBox
        Dim txttodate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim txtmaxnights As TextBox



        Try

            For Each GVRow In grdpromotiondetail.Rows
                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")


                fDate(n) = CType(txtfromdate.Text, String)
                tDate(n) = CType(txttodate.Text, String)
                bookingcode(n) = CType(txthotelbookingcode.Text, String)
                discounttype(n) = CType(ddloptions.SelectedValue, String)
                discountperc(n) = CType(txtdiscount.Text, String)
                adddiscountperc(n) = CType(txtadddiscount.Text, String)
                noofrooms(n) = CType(txtnoofrooms.Text, String)
                If chkmultiyes.Checked = True Then
                    multiyes(n) = 1
                Else
                    multiyes(n) = 0
                End If
                bookingvalidity(n) = CType(ddlbookoptions.SelectedValue, String)
                bookfdate(n) = CType(txtbookfromDate.Text, String)
                booktdate(n) = CType(txtBookToDate.Text, String)
                bookdays(n) = CType(txtbookdays.Text, String)
                minnights(n) = CType(Val(txtminnights.Text), String)
                maxnights(n) = CType(Val(txtmaxnights.Text), String)

                n = n + 1

            Next

            fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count + 1)

            txtfromdate = grdpromotiondetail.Rows(grdpromotiondetail.Rows.Count - 1).FindControl("txtfromdate")
            txtfromdate.Focus()

            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromotiondetail.Rows
                If n = i Then
                    Exit For
                End If
                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")

                txtfromdate.Text = fDate(n)
                txttodate.Text = tDate(n)
                txthotelbookingcode.Text = bookingcode(n)
                ddloptions.SelectedValue = discounttype(n)
                txtdiscount.Text = discountperc(n)
                txtadddiscount.Text = adddiscountperc(n)
                txtnoofrooms.Text = noofrooms(n)
                If multiyes(n) = 1 Then
                    chkmultiyes.Checked = True
                Else
                    chkmultiyes.Checked = False
                End If
                ddlbookoptions.SelectedValue = bookingvalidity(n)
                txtbookfromDate.Text = bookfdate(n)
                txtBookToDate.Text = booktdate(n)
                txtbookdays.Text = bookdays(n)
                txtminnights.Text = minnights(n)
                txtmaxnights.Text = maxnights(n)




                n = n + 1

            Next
            'Dim gridNewrow As GridViewRow
            'gridNewrow = grdpromotiondetail.Rows(grdpromotiondetail.Rows.Count - 1)
            'Dim strRowId As String = gridNewrow.ClientID
            'Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(grdpromotiondetail.Rows.Count - 1, String) + "');")
            'ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Protected Sub btndelrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelrow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1
        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim bookingcode(count) As String
        Dim discounttype(count) As String
        Dim discountperc(count) As String
        Dim adddiscountperc(count) As String
        Dim noofrooms(count) As String
        Dim multiyes(count) As String
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim minnights(count) As String
        Dim maxnights(count) As String

        Dim txtfromdate As TextBox
        Dim txttodate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim txtmaxnights As TextBox


        Dim n As Integer = 0


        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdpromotiondetail.Rows
                chkSelect = GVRow.FindControl("chkpromdet")
                If chkSelect.Checked = False Then

                    txtfromdate = GVRow.FindControl("txtfromdate")
                    txttodate = GVRow.FindControl("txttodate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    txtmaxnights = GVRow.FindControl("txtmaxnights")


                    fDate(n) = CType(txtfromdate.Text, String)
                    tDate(n) = CType(txttodate.Text, String)
                    bookingcode(n) = CType(txthotelbookingcode.Text, String)
                    discounttype(n) = CType(ddloptions.SelectedValue, String)
                    discountperc(n) = CType(txtdiscount.Text, String)
                    adddiscountperc(n) = CType(txtadddiscount.Text, String)
                    noofrooms(n) = CType(txtnoofrooms.Text, String)
                    If chkmultiyes.Checked = True Then
                        multiyes(n) = 1
                    Else
                        multiyes(n) = 0
                    End If
                    bookingvalidity(n) = CType(ddlbookoptions.SelectedValue, String)
                    bookfdate(n) = CType(txtbookfromDate.Text, String)
                    booktdate(n) = CType(txtBookToDate.Text, String)
                    bookdays(n) = CType(txtbookdays.Text, String)
                    minnights(n) = CType(txtminnights.Text, String)
                    maxnights(n) = CType(txtmaxnights.Text, String)

                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If
            'fillDategrd(grdflight, False, grdflight.Rows.Count - deletedrow)

            If grdpromotiondetail.Rows.Count > 1 Then
                fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count - deletedrow)
            Else
                fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdpromotiondetail.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then

                    txtfromdate = GVRow.FindControl("txtfromdate")
                    txttodate = GVRow.FindControl("txttodate")
                    txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                    ddloptions = GVRow.FindControl("ddloptions")
                    txtdiscount = GVRow.FindControl("txtdiscount")
                    txtadddiscount = GVRow.FindControl("txtadddiscount")
                    txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                    chkmultiyes = GVRow.FindControl("chkmultiyes")
                    ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                    txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                    txtBookToDate = GVRow.FindControl("txtBookToDate")
                    txtbookdays = GVRow.FindControl("txtbookdays")
                    txtminnights = GVRow.FindControl("txtminnights")
                    txtmaxnights = GVRow.FindControl("txtmaxnights")

                    txtfromdate.Text = fDate(n)
                    txttodate.Text = tDate(n)
                    txthotelbookingcode.Text = bookingcode(n)
                    ddloptions.SelectedValue = discounttype(n)
                    txtdiscount.Text = discountperc(n)
                    txtadddiscount.Text = adddiscountperc(n)
                    txtnoofrooms.Text = noofrooms(n)
                    If multiyes(n) = 1 Then
                        chkmultiyes.Checked = True
                    Else
                        chkmultiyes.Checked = False
                    End If
                    ddlbookoptions.SelectedValue = bookingvalidity(n)
                    txtbookfromDate.Text = bookfdate(n)
                    txtBookToDate.Text = booktdate(n)
                    txtbookdays.Text = bookdays(n)
                    txtminnights.Text = minnights(n)
                    txtmaxnights.Text = maxnights(n)


                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btncopyrow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyrow.Click
        CopyClick = 2
        ' Addlines()
        copylines()
        n = 0
        Try
            Dim count As Integer
            Dim GVRow As GridViewRow
            count = grdpromotiondetail.Rows.Count '+ 1


            Dim n As Integer = 0
            Dim txtfromdate As TextBox
            Dim txttodate As TextBox
            Dim txthotelbookingcode As TextBox
            Dim ddloptions As DropDownList
            Dim txtdiscount As TextBox
            Dim txtadddiscount As TextBox
            Dim txtnoofrooms As TextBox
            Dim chkmultiyes As CheckBox
            Dim ddlbookoptions As DropDownList
            Dim txtbookfromDate As TextBox
            Dim txtBookToDate As TextBox
            Dim txtbookdays As TextBox
            Dim txtminnights As TextBox
            Dim txtmaxnights As TextBox

            Dim txtstayfor As TextBox
            Dim txtpayfor As TextBox
            Dim txtmaxfreents As TextBox
            Dim txtmaxmultiples As TextBox
            Dim chkmultiples As CheckBox
            Dim ddlapply As DropDownList

            Dim txtrmtypname As TextBox
            Dim txtuprmtypname As TextBox
            Dim txtrmcombination As TextBox

            Dim txtmealcode As TextBox
            Dim txtupmealcode As TextBox
            Dim txtmealcombination As TextBox

            Dim txtrmcatcode As TextBox
            Dim txtuprmcatcode As TextBox

            Dim txtmealrmcatcode As TextBox
            Dim txtupmealrmcatcode As TextBox




            For Each GVRow In grdpromotiondetail.Rows
                ' If n = CopyRow + 1 Then ' gv_Filldata.Rows.Count - 1 Then

                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")


                txtstayfor = GVRow.FindControl("txtstayfor")
                txtpayfor = GVRow.FindControl("txtpayfor")
                txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                ddlapply = GVRow.FindControl("ddlapply")
                ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                chkmultiples = GVRow.FindControl("chkmultiples")



                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                txtrmcombination = GVRow.FindControl("txtrmcombination")

                txtmealcode = GVRow.FindControl("txtmealcode")
                txtupmealcode = GVRow.FindControl("txtupmealcode")
                txtmealcombination = GVRow.FindControl("txtmealcombination")

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")


                If n > CopyRow And txthotelbookingcode.Text = "" Then

                    txthotelbookingcode.Text = bookingcodenew.Item(CopyRow)
                    ddloptions.SelectedValue = discounttypenew.Item(CopyRow)
                    txtdiscount.Text = discountpercnew.Item(CopyRow)
                    txtadddiscount.Text = adddiscountpercnew.Item(CopyRow)
                    txtnoofrooms.Text = noofroomsnew.Item(CopyRow)
                    ddlbookoptions.SelectedValue = bookingvaliditynew.Item(CopyRow)
                    txtbookdays.Text = bookdaysnew.Item(CopyRow)
                    txtminnights.Text = minnightsnew.Item(CopyRow)
                    txtmaxnights.Text = maxnightsnew.Item(CopyRow)

                    If Val(multiyesnew.Item(CopyRow)) = 1 Then
                        chkmultiyes.Checked = True
                    Else
                        chkmultiyes.Checked = False
                    End If
                    If CType(fDatenew.Item(CopyRow), String) <> "" Then
                        txtfromdate.Text = Format(CType(fDatenew.Item(CopyRow), Date), "dd/MM/yyyy")
                        txttodate.Text = Format(CType(tDatenew.Item(CopyRow), Date), "dd/MM/yyyy")
                    End If

                    If CType(bookfdatenew.Item(CopyRow), String) <> "" Then
                        txtbookfromDate.Text = Format(CType(bookfdatenew.Item(CopyRow), Date), "dd/MM/yyyy")

                    End If
                    If CType(booktdatenew.Item(CopyRow), String) <> "" Then
                        txtBookToDate.Text = Format(CType(booktdatenew.Item(CopyRow), Date), "dd/MM/yyyy")

                    End If

                    txtrmtypname.Text = rmtypnamenew.Item(CopyRow)
                    txtuprmtypname.Text = uprmtypnamenew.Item(CopyRow)
                    txtrmcombination.Text = rmcombinationnew.Item(CopyRow)

                    txtmealcode.Text = mealcodenew.Item(CopyRow)
                    txtupmealcode.Text = upmealcodenew.Item(CopyRow)
                    txtmealcombination.Text = mealcombinationnew.Item(CopyRow)

                    txtrmcatcode.Text = rmcatcodenew.Item(CopyRow)
                    txtuprmcatcode.Text = uprmcatcodenew.Item(CopyRow)

                    txtmealrmcatcode.Text = mealrmcatcodenew.Item(CopyRow)
                    txtupmealrmcatcode.Text = upmealrmcatcodenew.Item(CopyRow)

                    txtstayfor.Text = staynew.Item(CopyRow)
                    txtpayfor.Text = payfornew.Item(CopyRow)
                    txtmaxfreents.Text = maxfreentsnew.Item(CopyRow)
                    txtmaxmultiples.Text = maxmultiplesnew.Item(CopyRow)
                    ddlapply.SelectedValue = applynew.Item(CopyRow)
                    If Val(multiplesnew.Item(CopyRow)) = 1 Then
                        chkmultiples.Checked = True
                    Else
                        chkmultiples.Checked = False
                    End If

                  


                    Exit For

                End If
                n = n + 1
            Next
            CopyClick = 0
            ClearArray()
            Bookingoptionchange()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Sub ClearArray()
        bookingcodenew.Clear()
        discounttypenew.Clear()
        discountpercnew.Clear()
        adddiscountpercnew.Clear()
        noofroomsnew.Clear()
        multiyesnew.Clear()
        bookingvaliditynew.Clear()
        bookfdatenew.Clear()
        booktdatenew.Clear()
        bookdaysnew.Clear()
        minnightsnew.Clear()
        fDatenew.Clear()
        tDatenew.Clear()

        staynew.Clear()
        payfornew.Clear()
        maxfreentsnew.Clear()
        applynew.Clear()
        maxmultiplesnew.Clear()
        multiplesnew.Clear()
        maxnightsnew.Clear()

        rmtypnamenew.Clear()
        uprmtypnamenew.Clear()
        rmcombinationnew.Clear()
        mealcodenew.Clear()
        upmealcodenew.Clear()

        mealcombinationnew.Clear()
        rmcatcodenew.Clear()
        uprmcatcodenew.Clear()
        mealrmcatcodenew.Clear()
        upmealrmcatcodenew.Clear()


    End Sub
    Private Sub copylines()
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1

        Dim fDate(count) As String
        Dim tDate(count) As String
        Dim bookingcode(count) As String
        Dim discounttype(count) As String
        Dim discountperc(count) As String
        Dim adddiscountperc(count) As String
        Dim noofrooms(count) As String
        Dim multiyes(count) As String
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim minnights(count) As String
        Dim maxnights(count) As String

        Dim rmtypname(count) As String
        Dim uprmtypname(count) As String
        Dim rmcombination(count) As String

        Dim mealcode(count) As String
        Dim upmealcode(count) As String
        Dim mealcombination(count) As String

        Dim rmcatcode(count) As String
        Dim uprmcatcode(count) As String

        Dim mealrmcatcode(count) As String
        Dim upmealrmcatcode(count) As String

        Dim n As Integer = 0

        Dim txtfromdate As TextBox
        Dim txttodate As TextBox
        Dim txthotelbookingcode As TextBox
        Dim ddloptions As DropDownList
        Dim txtdiscount As TextBox
        Dim txtadddiscount As TextBox
        Dim txtnoofrooms As TextBox
        Dim chkmultiyes As CheckBox
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        Dim txtminnights As TextBox
        Dim chkSelect As CheckBox
        Dim txtmaxnights As TextBox

        Dim txtrmtypname As TextBox
        Dim txtuprmtypname As TextBox
        Dim txtrmcombination As TextBox

        Dim txtmealcode As TextBox
        Dim txtupmealcode As TextBox
        Dim txtmealcombination As TextBox

        Dim txtrmcatcode As TextBox
        Dim txtuprmcatcode As TextBox

        Dim txtmealrmcatcode As TextBox
        Dim txtupmealrmcatcode As TextBox


        Dim txtstayfor As TextBox
        Dim txtpayfor As TextBox
        Dim txtmaxfreents As TextBox
        Dim txtmaxmultiples As TextBox
        Dim chkmultiples As CheckBox
        Dim ddlapply As DropDownList

        Dim stay(count) As String
        Dim payfor(count) As String
        Dim maxfreents(count) As String
        Dim maxmultiples(count) As String
        Dim multiples(count) As String
        Dim apply(count) As String

        '   CopyRow = 0


        Try

            For Each GVRow In grdpromotiondetail.Rows

                txtfromdate = GVRow.FindControl("txtfromdate")
                txttodate = GVRow.FindControl("txttodate")
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                ddloptions = GVRow.FindControl("ddloptions")
                txtdiscount = GVRow.FindControl("txtdiscount")
                txtadddiscount = GVRow.FindControl("txtadddiscount")
                txtnoofrooms = GVRow.FindControl("txtnoofrooms")
                chkmultiyes = GVRow.FindControl("chkmultiyes")
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                txtminnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")

                chkSelect = GVRow.FindControl("chkpromdet")

                txtstayfor = GVRow.FindControl("txtstayfor")
                txtpayfor = GVRow.FindControl("txtpayfor")
                txtmaxfreents = GVRow.FindControl("txtmaxfreents")
                txtmaxmultiples = GVRow.FindControl("txtmaxmultiples")
                ddlapply = GVRow.FindControl("ddlapply")
                ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                chkmultiples = GVRow.FindControl("chkmultiples")

                txtrmtypname = GVRow.FindControl("txtrmtypname")
                txtuprmtypname = GVRow.FindControl("txtuprmtypname")
                txtrmcombination = GVRow.FindControl("txtrmcombination")

                txtmealcode = GVRow.FindControl("txtmealcode")
                txtupmealcode = GVRow.FindControl("txtupmealcode")
                txtmealcombination = GVRow.FindControl("txtmealcombination")

                txtrmcatcode = GVRow.FindControl("txtrmcatcode")
                txtuprmcatcode = GVRow.FindControl("txtuprmcatcode")

                txtmealrmcatcode = GVRow.FindControl("txtmealrmcatcode")
                txtupmealrmcatcode = GVRow.FindControl("txtupmealrmcatcode")

               

                If chkSelect.Checked = True Then

                    CopyRow = n
                End If


               


                fDate(n) = CType(txtfromdate.Text, String)
                tDate(n) = CType(txttodate.Text, String)
                bookingcode(n) = CType(txthotelbookingcode.Text, String)
                discounttype(n) = CType(ddloptions.SelectedValue, String)
                discountperc(n) = CType(txtdiscount.Text, String)
                adddiscountperc(n) = CType(txtadddiscount.Text, String)
                noofrooms(n) = CType(txtnoofrooms.Text, String)
                If chkmultiyes.Checked = True Then
                    multiyes(n) = 1
                Else
                    multiyes(n) = 0
                End If
                bookingvalidity(n) = CType(ddlbookoptions.SelectedValue, String)
                bookfdate(n) = CType(txtbookfromDate.Text, String)
                booktdate(n) = CType(txtBookToDate.Text, String)
                bookdays(n) = CType(txtbookdays.Text, String)
                minnights(n) = CType(txtminnights.Text, String)

                stay(n) = CType(txtstayfor.Text, String)
                payfor(n) = CType(txtpayfor.Text, String)
                maxfreents(n) = CType(txtmaxfreents.Text, String)
                maxmultiples(n) = CType(txtmaxmultiples.Text, String)
                apply(n) = CType(ddlapply.SelectedValue, String)

                If chkmultiples.Checked = True Then
                    multiples(n) = 1
                Else
                    multiples(n) = 0
                End If

                rmtypname(n) = CType(txtrmtypname.Text, String)
                uprmtypname(n) = CType(txtuprmtypname.Text, String)
                rmcombination(n) = CType(txtrmcombination.Text, String)

                mealcode(n) = CType(txtmealcode.Text, String)
                upmealcode(n) = CType(txtupmealcode.Text, String)
                mealcombination(n) = CType(txtmealcombination.Text, String)

                rmcatcode(n) = CType(txtrmcatcode.Text, String)
                uprmcatcode(n) = CType(txtuprmcatcode.Text, String)

                mealrmcatcode(n) = CType(txtmealrmcatcode.Text, String)
                upmealrmcatcode(n) = CType(txtupmealrmcatcode.Text, String)



                staynew.Add(CType(txtstayfor.Text, String))
                payfornew.Add(CType(txtpayfor.Text, String))
                maxfreentsnew.Add(CType(txtmaxfreents.Text, String))
                maxmultiplesnew.Add(CType(txtmaxmultiples.Text, String))
                applynew.Add(CType(ddlapply.SelectedValue, String))

                If chkmultiples.Checked = True Then
                    multiplesnew.Add("1")
                Else
                    multiplesnew.Add("0")

                End If

                rmtypnamenew.Add(CType(txtrmtypname.Text, String))
                uprmtypnamenew.Add(CType(txtuprmtypname.Text, String))
                rmcombinationnew.Add(CType(txtrmcombination.Text, String))

                mealcodenew.Add(CType(txtmealcode.Text, String))
                upmealcodenew.Add(CType(txtupmealcode.Text, String))
                mealcombinationnew.Add(CType(txtmealcombination.Text, String))

                rmcatcodenew.Add(CType(txtrmcatcode.Text, String))
                uprmcatcodenew.Add(CType(txtuprmcatcode.Text, String))
                mealrmcatcodenew.Add(CType(txtmealrmcatcode.Text, String))
                upmealrmcatcodenew.Add(CType(txtupmealrmcatcode.Text, String))


                bookingcodenew.Add(CType(txthotelbookingcode.Text, String))
                discounttypenew.Add(CType(ddloptions.SelectedValue, String))
                discountpercnew.Add(CType(txtdiscount.Text, String))
                adddiscountpercnew.Add(CType(txtadddiscount.Text, String))
                noofroomsnew.Add(CType(txtnoofrooms.Text, String))

                If chkmultiyes.Checked = True Then
                    multiyesnew.Add("1")
                Else
                    multiyesnew.Add("0")

                End If


                fDatenew.Add(CType(txtfromdate.Text, String))
                tDatenew.Add(CType(txttodate.Text, String))
                bookingvaliditynew.Add(CType(ddlbookoptions.SelectedValue, String))
                bookfdatenew.Add(CType(txtbookfromDate.Text, String))
                booktdatenew.Add(CType(txtBookToDate.Text, String))
                bookdaysnew.Add(CType(txtbookdays.Text, String))
                minnightsnew.Add(CType(txtminnights.Text, String))
                maxnightsnew.Add(CType(txtmaxnights.Text, String))



                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try

    End Sub

    Protected Sub grdcombinations_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdcombinations.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim txtroomno As TextBox
                Dim txtadults As TextBox
                Dim txtchild As TextBox


                txtroomno = e.Row.FindControl("txtroomno")
                txtadults = e.Row.FindControl("txtadults")
                txtchild = e.Row.FindControl("txtadults")



                Numberssrvctrl(txtadults)
                Numberssrvctrl(txtchild)
                Numberssrvctrl(txtroomno)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btnaddcomb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddcomb.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdcombinations.Rows.Count + 1
        Dim roomno(count) As String
        Dim adults(count) As String
        Dim child(count) As String

        Dim excl(count) As String
        Dim n As Integer = 0
        Dim txtadults As TextBox
        Dim txtroomno As TextBox
        Dim txtchild As TextBox


        Try
            For Each GVRow In grdcombinations.Rows
                txtroomno = GVRow.FindControl("txtroomno")
                roomno(n) = CType(txtroomno.Text, String)

                txtadults = GVRow.FindControl("txtadults")
                adults(n) = CType(txtadults.Text, String)

                txtchild = GVRow.FindControl("txtchild")
                child(n) = CType(txtchild.Text, String)


                n = n + 1
            Next
            fillDategrd(grdcombinations, False, grdcombinations.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdcombinations.Rows
                If n = i Then
                    Exit For
                End If
                txtroomno = GVRow.FindControl("txtroomno")
                txtadults = GVRow.FindControl("txtadults")
                txtchild = GVRow.FindControl("txtchild")

                txtroomno.Text = roomno(n)
                txtadults.Text = adults(n)
                txtchild.Text = child(n)


                n = n + 1
            Next
            ModalRoomPopup.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndelcomb_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelcomb.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdcombinations.Rows.Count + 1
        Dim roomno(count) As String
        Dim adults(count) As String
        Dim child(count) As String


        Dim n As Integer = 0
        Dim txtroomno As TextBox
        Dim txtadults As TextBox
        Dim txtchild As TextBox


        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdcombinations.Rows
                chkSelect = GVRow.FindControl("chkcombineselect")
                If chkSelect.Checked = False Then
                    txtroomno = GVRow.FindControl("txtroomno")
                    roomno(n) = CType(txtroomno.Text, String)

                    txtadults = GVRow.FindControl("txtadults")
                    adults(n) = CType(txtadults.Text, String)

                    txtchild = GVRow.FindControl("txtchild")
                    child(n) = CType(txtchild.Text, String)

                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next

            count = n
            If count = 0 Then
                count = 1
            End If
            'fillDategrd(grdflight, False, grdflight.Rows.Count - deletedrow)

            If grdcombinations.Rows.Count > 1 Then
                fillDategrd(grdcombinations, False, grdcombinations.Rows.Count - deletedrow)
            Else
                fillDategrd(grdcombinations, False, grdcombinations.Rows.Count)
            End If


            Dim i As Integer = n
            n = 0
            For Each GVRow In grdcombinations.Rows
                If n = i Then
                    Exit For
                End If
                If GVRow.RowIndex < count Then

                    txtroomno = GVRow.FindControl("txtroomno")
                    txtroomno.Text = roomno(n)

                    txtadults = GVRow.FindControl("txtadults")
                    txtadults.Text = adults(n)
                    txtchild = GVRow.FindControl("txtchild")
                    txtchild.Text = child(n)
                    n = n + 1
                End If
            Next

            ModalRoomPopup.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub grdinterhotel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdinterhotel.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtinterminnights As TextBox

                txtinterminnights = e.Row.FindControl("txtinterminnights")

                Numberssrvctrl(txtinterminnights)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub dlPromotionType_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlPromotionType.ItemDataBound
        'Try
        '    Dim rowid As String
        '    Dim promotiontype As String = ""
        '    'If e.Row.RowType = DataControlRowType.DataRow Then
        '    If e.Item.ItemType = DataControlCellType.DataCell Then
        '        Dim chkprmtype As CheckBox
        '        Dim chkAll As CheckBox
        '        Dim txtbox As Label
        '        chkprmtype = e.Item.FindControl("chkpromotiontype")
        '        chkAll = dlPromotionType.Controls(0).Controls(0).FindControl("chkAll")
        '        txtbox = TryCast(e.Item.FindControl("txtpromtoiontype"), Label)
        '        promotiontype = txtbox.Text
        '        rowid = CType(e.Item.ItemIndex, String)
        '        chkprmtype.Attributes.Add("onChange", "showcontrolfill('" & chkprmtype.ClientID & "')")
        '        chkAll.Attributes.Add("onChange", "showcontrolfill('" & chkprmtype.ClientID & "')")

        '        'chkprmtype.Attributes.Add("onChange", "hidecolumn('" & CType(promotiontype, String) & "', '" + CType(e.Row.RowIndex, String) + "')")

        '    End If
        'Catch ex As Exception

        'End Try
    End Sub
    Protected Sub check_changed(ByVal sender As Object, ByVal e As System.EventArgs)
        fillControls()
    End Sub

    Protected Sub btnrmtypnoshow_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As DataSet
        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex





        Dim strrmtypename As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtrmtypname"), TextBox).Text

        Dim strChkdValnew As String = CType(grdpromotiondetail.Rows(rowid).FindControl("txtrmcombination"), TextBox).Text


        Dim strRoomType As String = CType(strrmtypename, String)
        fillControls()
        ViewState("noshowclick") = 2

        Dim checkedrow As Integer = 0
        Dim promoList As String = String.Empty
        Dim count As Integer = 0

        For Each item As DataListItem In dlPromotionType.Items
            Dim chkRow As CheckBox = TryCast(item.FindControl("chkpromotiontype"), CheckBox)
            Dim txtbox As Label = TryCast(item.FindControl("txtpromtoiontype"), Label)
            If chkRow.Checked = True Then
                Dim lbltype As String = txtbox.Text
                'item.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
                Dim strpop As String = ""
                checkedrow = checkedrow + 1
                If count = 0 Then
                    promoList = lbltype
                    count = 1
                Else
                    promoList &= "|" & lbltype
                End If
            End If
        Next

       


        Dim MyDs As New DataTable
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))


        strSqlQry = "select rmtypcode,rmtypname,rankord from  partyrmtyp(nolock) where  inactive=0 and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)"



        myCommand = New SqlCommand(strSqlQry, mySqlConn)
        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
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

        Dim strCondition As String = ""
        If promoList.Length <> 0 Then

            Dim mString As String() = promoList.Split("|")

            If promoList.Contains("Room Upgrade") = True Then
                ViewState("RoomUpgrade") = 1
                gv_Showroomtypes.Columns(4).Visible = True
                For Each grdRow In gv_Showroomtypes.Rows
                    Dim ddlUpgrade As HtmlSelect = grdRow.FindControl("ddlUpgrade")

                    ddlUpgrade.Disabled = False
                Next
            Else
                For Each grdRow In gv_Showroomtypes.Rows
                    Dim ddlUpgrade As HtmlSelect = grdRow.FindControl("ddlUpgrade")
                    ddlUpgrade.Value = "[Select]"
                    ddlUpgrade.Disabled = True
                Next

                gv_Showroomtypes.Columns(4).Visible = False
            End If
            'For i As Integer = 0 To mString.Length - 1

            '    If UCase(mString(i)) = UCase("Room Upgrade") Then
            '        ViewState("RoomUpgrade") = 1
            '        gv_Showroomtypes.Columns(4).Visible = True
            '    Else
            '        gv_Showroomtypes.Columns(4).Visible = False
            '    End If
            'Next
        End If

        If ViewState("RoomUpgrade") = 1 Then
            ChkExistingRoomtypes(strChkdValnew)
        Else
            ChkExistingRoomtypes(strRoomType)
        End If




        ' Enablegrid()
        ModalPopupNoshow.Show()


    End Sub
    Protected Sub fillControls()
        Try
            Dim reqstr As String = ""
            gridrmtype.Columns(3).Visible = False
            grdmealplan.Columns(3).Visible = False
            grdrmcat.Columns(3).Visible = False
            gv_Showroomtypes.Columns(4).Visible = False
            grdpromotiondetail.Columns(8).Visible = False
            grdpromotiondetail.Columns(9).Visible = False
            grdpromotiondetail.Columns(10).Visible = False
            grdpromotiondetail.Columns(1).Visible = True
            grdpromotiondetail.Columns(2).Visible = True


            ''' free nights

            grdpromotiondetail.Columns(14).Visible = True
            grdpromotiondetail.Columns(15).Visible = True
            grdpromotiondetail.Columns(16).Visible = True
            grdpromotiondetail.Columns(17).Visible = True
            grdpromotiondetail.Columns(18).Visible = True
            'grdpromotiondetail.Columns(19).Visible = False
            'grdpromotiondetail.Columns(20).Visible = False

            grdpromotiondetail.Columns(19).Visible = False
            grdpromotiondetail.Columns(20).Visible = False
            grdpromotiondetail.Columns(21).Visible = False
            grdpromotiondetail.Columns(22).Visible = False
            grdpromotiondetail.Columns(23).Visible = False
            grdpromotiondetail.Columns(24).Visible = False



            '''''




            divflight.Style.Add("display", "none")
            divsplocc.Style.Add("display", "none")
            divinter.Style.Add("display", "none")
            divstay.Style.Add("display", "none")
            'divpromodates.Style.Add("display", "block")
            divcomptrf.Style.Add("display", "none")
            chkdiscount.Style.Add("display", "none")

            '  divapplydiscount.Style.Add("display", "none")

            Dim checkedrow As Integer = 0
            Dim promoList As String = String.Empty
            Dim count As Integer = 0
            'For Each row As GridViewRow In gv_promotype.Rows
            '    'commented by Elsitta on 13.11.2016
            '    'For Each row As datalistrow In GridView

            '    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkpromotiontype"), CheckBox)

            '    If chkRow.Checked = True Then

            '        Dim lbltype As String = row.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
            '        Dim strpop As String = ""
            '        checkedrow = checkedrow + 1
            '        If count = 0 Then
            '            promoList = lbltype
            '            count = 1
            '        Else
            '            promoList &= "|" & lbltype
            '        End If

            '    End If
            'Next


            'below code added by Elsitta on 14.11.2016 because gridiew design changed into datalist
            For Each item As DataListItem In dlPromotionType.Items
                Dim chkRow As CheckBox = TryCast(item.FindControl("chkpromotiontype"), CheckBox)
                Dim txtbox As Label = TryCast(item.FindControl("txtpromtoiontype"), Label)
                If chkRow.Checked = True Then
                    Dim lbltype As String = txtbox.Text
                    'item.Cells(2).Text     'TryCast(row.Cells(2).FindControl("lblreqid"), Label).Text
                    Dim strpop As String = ""
                    checkedrow = checkedrow + 1
                    If count = 0 Then
                        promoList = lbltype
                        count = 1
                    Else
                        promoList &= "|" & lbltype
                    End If
                End If
            Next

            'above code added by Elsitta on 14.11.2016 because gridview design changed to datalist





            If CType(Session("OfferState"), String) = "New" Then
                ddlapplydiscount.SelectedIndex = 0
                ddlapplydiscount.Disabled = False
            Else
                ddlapplydiscount.Disabled = False

            End If

            Dim strCondition As String = ""
            If promoList.Length <> 0 Then

                Dim mString As String() = promoList.Split("|")
                For i As Integer = 0 To mString.Length - 1

                    If UCase(mString(i)) = UCase("Room Upgrade") Then

                        gridrmtype.Columns(3).Visible = True
                        gv_Showroomtypes.Columns(4).Visible = True
                        divcomm.Style.Add("display", "none")


                    ElseIf UCase(mString(i)) = UCase("Meal Upgrade") Then

                        grdmealplan.Columns(3).Visible = True
                        gv_Showroomtypes.Columns(4).Visible = True
                        divcomm.Style.Add("display", "none")
                    ElseIf UCase(mString(i)) = UCase("Kids Go Free") Then
                        divcomm.Style.Add("display", "none")

                    ElseIf UCase(mString(i)) = UCase("Accomodation Upgrade") Then
                        grdrmcat.Columns(3).Visible = True
                    ElseIf UCase(mString(i)) = UCase("Special Rates") Then

                        ddlapplydiscount.Disabled = True

                        ddlapplydiscount.SelectedIndex = 0
                        divcomm.Style.Add("display", "block")

                    ElseIf UCase(mString(i)) = UCase("Early Bird Discount") Then

                        'grdpromotiondetail.Columns(4).Visible = True
                        'grdpromotiondetail.Columns(5).Visible = True
                        'grdpromotiondetail.Columns(6).Visible = True

                        grdpromotiondetail.Columns(8).Visible = True
                        grdpromotiondetail.Columns(9).Visible = True
                        grdpromotiondetail.Columns(10).Visible = True

                        grdpromotiondetail.Columns(1).Visible = True
                        grdpromotiondetail.Columns(2).Visible = True
                        chkdiscount.Style.Add("display", "block")
                        divcomm.Style.Add("display", "block")

                        'divapplydiscount.Style.Add("display", "block")
                        'If CType(Session("OfferState"), String) = "New" Then
                        '    
                        'End If

                        ' divpromodates.Style.Add("display", "none")
                    ElseIf UCase(mString(i)) = UCase("Select flights only") Then
                        divflight.Style.Add("display", "block")

                    ElseIf UCase(mString(i)) = UCase("Special Occasion") Then
                        divstay.Style.Add("display", "block")
                        divcomm.Style.Add("display", "none")

                    ElseIf UCase(mString(i)) = UCase("Inter Hotels") Then
                        divinter.Style.Add("display", "block")

                    ElseIf UCase(mString(i)) = UCase("Free Nights") Then

                        'ddlapply = ShowHide(ddlapply) '*** Danny added for show hile a value
                     


                        grdpromotiondetail.Columns(15).Visible = True
                        grdpromotiondetail.Columns(16).Visible = True
                        grdpromotiondetail.Columns(17).Visible = True
                        grdpromotiondetail.Columns(18).Visible = True
                        grdpromotiondetail.Columns(19).Visible = True
                        grdpromotiondetail.Columns(20).Visible = True

                        grdpromotiondetail.Columns(21).Visible = True
                        grdpromotiondetail.Columns(22).Visible = True
                        grdpromotiondetail.Columns(23).Visible = True
                        grdpromotiondetail.Columns(24).Visible = True


                        If Not IsNothing(Session("1002")) Then ''*** Danny 1002 is Excel Document number 
                            If Session("1002").ToString() <> "SHOW" Then
                                Dim ddlapply As DropDownList
                                For Each GVRow In grdpromotiondetail.Rows
                                    ddlapply = GVRow.FindControl("ddlapply")
                                    ddlapply.Items.RemoveAt(3)
                                Next
                            End If
                        End If

                        'grdpromotiondetail.Columns(19).Visible = False
                        'grdpromotiondetail.Columns(20).Visible = False
                        'grdpromotiondetail.Columns(21).Visible = False
                        'grdpromotiondetail.Columns(22).Visible = False
                        'grdpromotiondetail.Columns(23).Visible = False
                        'grdpromotiondetail.Columns(24).Visible = False

                    ElseIf UCase(mString(i)) = UCase("Complimentary Airport Transfer") Then

                        divcomptrf.Style.Add("display", "block")


                    End If
                Next
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnaddArrival_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddArrival.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdArrivalTransfer.Rows.Count + 1
        Dim airportcode(count) As String
        Dim airportnamename(count) As String
        Dim flightcode(count) As String

        Dim n As Integer = 0
        Dim txtairportcode As TextBox
        Dim txtairportname As TextBox
        Dim txtflightcode As TextBox
        Try
            For Each GVRow In grdArrivalTransfer.Rows
                txtairportcode = GVRow.FindControl("txtarrivalterminal")
                airportcode(n) = CType(txtairportcode.Text, String)
                txtflightcode = GVRow.FindControl("txtflightcode")
                flightcode(n) = CType(txtflightcode.Text, String)
                txtairportname = GVRow.FindControl("txtarrivalAirportName")
                airportnamename(n) = CType(txtairportname.Text, String)
                n = n + 1
            Next
            fillDategrd(grdArrivalTransfer, False, grdArrivalTransfer.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdArrivalTransfer.Rows
                If n = i Then
                    Exit For
                End If
                txtairportcode = GVRow.FindControl("txtarrivalterminal")
                txtairportcode.Text = airportcode(n)
                txtairportname = GVRow.FindControl("txtarrivalAirportName")
                txtairportname.Text = airportnamename(n)
                txtflightcode = GVRow.FindControl("txtflightcode")
                txtflightcode.Text = flightcode(n)

                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndelArrival_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelArrival.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdArrivalTransfer.Rows.Count + 1
        Dim airportcode(count) As String
        Dim airportnamename(count) As String
        Dim flightcode(count) As String

        Dim n As Integer = 0
        Dim txtairportcode As TextBox
        Dim txtairportname As TextBox
        Dim txtflightcode As TextBox

        Dim ddlExcl As HtmlSelect
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdArrivalTransfer.Rows
                chkSelect = GVRow.FindControl("chkArrivalTerminal")
                If chkSelect.Checked = False Then
                    txtairportcode = GVRow.FindControl("txtarrivalterminal")
                    airportcode(n) = CType(txtairportcode.Text, String)
                    txtflightcode = GVRow.FindControl("txtflightcode")
                    flightcode(n) = CType(txtflightcode.Text, String)
                    txtairportname = GVRow.FindControl("txtarrivalAirportName")
                    airportnamename(n) = CType(txtairportname.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            If grdArrivalTransfer.Rows.Count > 1 Then
                fillDategrd(grdArrivalTransfer, False, grdArrivalTransfer.Rows.Count - deletedrow)
            Else
                fillDategrd(grdArrivalTransfer, False, grdArrivalTransfer.Rows.Count)
            End If
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdArrivalTransfer.Rows
                If GVRow.RowIndex < count Then
                    txtairportcode = GVRow.FindControl("txtarrivalterminal")
                    txtairportcode.Text = airportcode(n)
                    txtflightcode = GVRow.FindControl("txtflightcode")
                    txtflightcode.Text = flightcode(n)
                    txtairportname = GVRow.FindControl("txtarrivalAirportName")
                    txtairportname.Text = airportnamename(n)

                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub chkarrival_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkarrival.CheckedChanged
        If chkarrival.Checked = True Then
            divArrivalTransfer.Style.Add("display", "block")
        Else
            divArrivalTransfer.Style.Add("display", "none")
        End If
    End Sub

    Protected Sub chkdeparture_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkdeparture.CheckedChanged
        If chkdeparture.Checked = True Then
            divDepartureTransfer.Style.Add("display", "block")
        Else
            divDepartureTransfer.Style.Add("display", "none")
        End If
    End Sub

    Protected Sub btnaddDeparture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddDeparture.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDepartureTransfer.Rows.Count + 1
        Dim airportcode(count) As String
        Dim airportnamename(count) As String
        Dim flightcode(count) As String

        Dim n As Integer = 0
        Dim txtairportcode As TextBox
        Dim txtairportname As TextBox
        Dim txtflightcode As TextBox
        Try
            For Each GVRow In grdDepartureTransfer.Rows
                txtairportcode = GVRow.FindControl("txtdepartureterminal")
                airportcode(n) = CType(txtairportcode.Text, String)
                txtflightcode = GVRow.FindControl("txtdepflightcode")
                flightcode(n) = CType(txtflightcode.Text, String)

                txtairportname = GVRow.FindControl("txtdepartureAirportName")
                airportnamename(n) = CType(txtairportname.Text, String)
                n = n + 1
            Next
            fillDategrd(grdDepartureTransfer, False, grdDepartureTransfer.Rows.Count + 1)
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDepartureTransfer.Rows
                If n = i Then
                    Exit For
                End If
                txtairportcode = GVRow.FindControl("txtdepartureterminal")
                txtairportcode.Text = airportcode(n)
                txtflightcode = GVRow.FindControl("txtdepflightcode")
                txtflightcode.Text = flightcode(n)
                txtairportname = GVRow.FindControl("txtdepartureAirportName")
                txtairportname.Text = airportnamename(n)
                n = n + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btndelDeparture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelDeparture.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdDepartureTransfer.Rows.Count + 1
        Dim airportcode(count) As String
        Dim flightcode(count) As String
        Dim airportnamename(count) As String

        Dim n As Integer = 0
        Dim txtairportcode As TextBox
        Dim txtairportname As TextBox
        Dim txtflightcode As TextBox

        Dim ddlExcl As HtmlSelect
        Dim chkSelect As CheckBox
        Dim delrows(count) As String
        Dim deletedrow As Integer = 0
        Dim k As Integer = 0

        Try
            For Each GVRow In grdDepartureTransfer.Rows
                chkSelect = GVRow.FindControl("chkDepartureTerminal")
                If chkSelect.Checked = False Then
                    txtairportcode = GVRow.FindControl("txtdepartureterminal")
                    airportcode(n) = CType(txtairportcode.Text, String)
                    txtflightcode = GVRow.FindControl("txtdepflightcode")
                    flightcode(n) = CType(txtflightcode.Text, String)

                    txtairportname = GVRow.FindControl("txtdepartureAirportName")
                    airportnamename(n) = CType(txtairportname.Text, String)
                    n = n + 1
                Else
                    deletedrow = deletedrow + 1
                End If

            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            If grdDepartureTransfer.Rows.Count > 1 Then
                fillDategrd(grdDepartureTransfer, False, grdDepartureTransfer.Rows.Count - deletedrow)
            Else
                fillDategrd(grdDepartureTransfer, False, grdDepartureTransfer.Rows.Count)
            End If
            Dim i As Integer = n
            n = 0
            For Each GVRow In grdDepartureTransfer.Rows
                If GVRow.RowIndex < count Then
                    txtairportcode = GVRow.FindControl("txtdepartureterminal")
                    txtairportcode.Text = airportcode(n)
                    txtflightcode = GVRow.FindControl("txtdepflightcode")
                    txtflightcode.Text = flightcode(n)
                    txtairportname = GVRow.FindControl("txtdepartureAirportName")
                    txtairportname.Text = airportnamename(n)
                    n = n + 1
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
#Region "Private Sub showDiv()"
    Private Sub showDiv()
        If ddlBookingValidity.Value = "2" Then
            DivFrom.Style("visibility") = "hidden"
            DivTo.Style("visibility") = "hidden"
            DivDays.Style("visibility") = "visible"
            'lblBookVal.Text = "Book Before Days"
            lblBookVal.Text = "Booking Validity Days/Months" 'This is changed by Elsitta on 160415 for changing the hotel label
        ElseIf ddlBookingValidity.Value = "3" Then
            DivFrom.Style("visibility") = "hidden"
            DivTo.Style("visibility") = "hidden"
            DivDays.Style("visibility") = "visible"
            lblBookVal.Text = "Booking Validity Days/Months"
        ElseIf ddlBookingValidity.Value = "4" Then
            DivFrom.Style("visibility") = "visible"
            DivTo.Style("visibility") = "visible"
            DivDays.Style("visibility") = "hidden"
            lblFrom.Text = "From"
            lblBookVal.Text = "From"
        ElseIf ddlBookingValidity.Value = "1" Then
            DivFrom.Style("visibility") = "visible"
            DivTo.Style("visibility") = "hidden"
            DivDays.Style("visibility") = "hidden"
            ' lblFrom.Text = "From"
            lblBookVal.Text = "Booking Validity To/Book By"
        End If
    End Sub
#End Region

    Protected Sub btnminFill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnminFill.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1
        Dim minnights(count) As String
        Dim maxnights(count) As String
        Dim n As Integer = 0
        Dim txtminmumnights As TextBox
        Dim txtmaxnights As TextBox
        Dim minimumnights As String = CType(txtminnights.Value, String)
        Dim ddlvalue As String = CType(ddlfillnights.Value, String)

        Try
            'For Each GVRow In grdpromotiondetail.Rows
            'txtminnights = GVRow.FindControl("txtminnights")
            minnights(1) = CType(txtminnights.Value, String)
            'n = n + 1
            'Next
            'fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count)
            'Dim i As Integer = n
            ' n = 0

            For Each GVRow In grdpromotiondetail.Rows
               
                txtminmumnights = GVRow.FindControl("txtminnights")
                txtmaxnights = GVRow.FindControl("txtmaxnights")

                If ddlvalue = 2 Then
                    txtmaxnights.Text = minnights(1)
                Else
                    txtminmumnights.Text = minnights(1)
                End If


                ' n = n + 1
            Next
            Bookingoptionchange()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub

    Protected Sub btnBookingCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBookingCode.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1
        Dim bookingcode(count) As String
        Dim n As Integer = 0
        Dim txthotelbookingcode As TextBox
        Dim hotelbookingcode As String = CType(txtbookingcode.Value, String)
        Try
            'For Each GVRow In grdpromotiondetail.Rows
            'txtminnights = GVRow.FindControl("txtminnights")
            bookingcode(1) = CType(hotelbookingcode, String)
            ' n = n + 1
            ' Next
            ' fillDategrd(grdpromotiondetail, False, grdpromotiondetail.Rows.Count)
            'Dim i As Integer = n
            'n = 0
            For Each GVRow In grdpromotiondetail.Rows
                'If n = i Then
                '    Exit For
                'End If
                txthotelbookingcode = GVRow.FindControl("txthotelbookingcode")
                txthotelbookingcode.Text = bookingcode(1)
                'n = n + 1
            Next
            Bookingoptionchange()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ' .writerrorlog(Server.MapPath("Errorlog.txt"), ex.ToString, 1, 1)
        End Try
    End Sub
    Protected Sub btnfillgrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillgrid.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = grdpromotiondetail.Rows.Count + 1
        Dim bookingvalidity(count) As String
        Dim bookfdate(count) As String
        Dim booktdate(count) As String
        Dim bookdays(count) As String
        Dim n As Integer = 0
        Dim ddlbookoptions As DropDownList
        Dim txtbookfromDate As TextBox
        Dim txtBookToDate As TextBox
        Dim txtbookdays As TextBox
        'dpFrom.Text = ""
        'dpTo.Text = ""
        'txtDays.Value = ""
        Try
            'For Each GVRow In grdpromotiondetail.Rows
            'ddlbookoptions = GVRow.FindControl("ddlbookoptions")
            'txtbookfromDate = GVRow.FindControl("txtbookfromDate")
            'txtBookToDate = GVRow.FindControl("txtBookToDate")
            'txtbookdays = GVRow.FindControl("txtbookdays")
            'txtminnights = GVRow.FindControl("txtminnights")
            bookingvalidity(1) = ddlBookingValidity.SelectedIndex
            bookfdate(1) = CType(dpFrom.Text, String)
            booktdate(1) = CType(dpTo.Text, String)
            bookdays(1) = CType(txtDays.Value, String)
         
            '  showControls()
            For Each GVRow In grdpromotiondetail.Rows
                'If n = i Then
                '    Exit For
                'End If
                ddlbookoptions = GVRow.FindControl("ddlbookoptions")
                txtbookfromDate = GVRow.FindControl("txtbookfromDate")
                txtBookToDate = GVRow.FindControl("txtBookToDate")
                txtbookdays = GVRow.FindControl("txtbookdays")
                ddlbookoptions.SelectedIndex = bookingvalidity(1)

                txtbookfromDate.Text = ""
                txtBookToDate.Text = ""
                txtbookdays.Text = ""

                If ddlbookoptions.SelectedIndex = 0 Then
                    ' txtBookToDate.Text = bookfdate(1)
                    txtbookfromDate.Text = bookfdate(1)
                End If
                If ddlbookoptions.SelectedIndex = 3 Then
                    txtBookToDate.Text = booktdate(1)
                    txtbookfromDate.Text = bookfdate(1)
                End If



                txtbookdays.Text = bookdays(1)
                'n = n + 1
            Next
            '' '' '' ''For Each GVRow In grdpromotiondetail.Rows
            '' '' '' ''    'If n = i Then
            '' '' '' ''    '    Exit For
            '' '' '' ''    'End If
            '' '' '' ''    txtBookToDate = GVRow.FindControl("txtBookToDate")
            '' '' '' ''    txtBookToDate.Text = "28/11/2016"

            '' '' '' ''    'n = n + 1
            '' '' '' ''Next

            Bookingoptionchange()


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Private Function showControls()
        If ddlBookingValidity.SelectedIndex = 1 Or ddlBookingValidity.SelectedIndex = 2 Then

            grdpromotiondetail.Columns(10).Visible = False
            grdpromotiondetail.Columns(11).Visible = False
            grdpromotiondetail.Columns(12).Visible = True

            
        ElseIf ddlBookingValidity.SelectedIndex = 3 Then

            grdpromotiondetail.Columns(10).Visible = True
            grdpromotiondetail.Columns(11).Visible = True
            grdpromotiondetail.Columns(12).Visible = False

        ElseIf ddlBookingValidity.SelectedIndex = 1 Then
            grdpromotiondetail.Columns(10).Visible = True
            grdpromotiondetail.Columns(11).Visible = False
            grdpromotiondetail.Columns(12).Visible = False

        ElseIf ddlBookingValidity.SelectedIndex = 0 Then
            grdpromotiondetail.Columns(11).Visible = False
            grdpromotiondetail.Columns(12).Visible = False
            grdpromotiondetail.Columns(10).Visible = True
        End If
    End Function




    Protected Sub btncombinationok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncombinationok.Click
        Dim chkSelect As HtmlInputCheckBox
        Dim intAdult, intChild, intRoomno As Integer
        Dim strChkdStringVal As StringBuilder = New StringBuilder()
        Dim tickedornot As Boolean

        tickedornot = False
        For Each grdRow In grdcombinations.Rows
            Dim txtroomno As TextBox = grdRow.FindControl("txtroomno")
            Dim txtadults As TextBox = grdRow.FindControl("txtadults")

            If txtroomno.Text <> "" And txtadults.Text <> "" Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter combination');", True)
            ModalRoomPopup.Show()
            Exit Sub
        End If

        For Each grdRow In grdcombinations.Rows
            Dim txtroomno As TextBox = grdRow.FindControl("txtroomno")
            Dim txtadults As TextBox = grdRow.FindControl("txtadults")
            Dim txtchild As TextBox = grdRow.FindControl("txtchild")
            If txtroomno.Text <> "" And txtadults.Text <> "" Then
                intRoomno = txtroomno.Text
                intAdult = txtadults.Text
                intChild = txtchild.Text

                strChkdStringVal.AppendFormat("{0}/{1}/{2},", intRoomno, intAdult, intChild)
            End If
        Next

        If hdnMainGridRowid.Value <> "" Then
            Dim txtnoofrooms As TextBox = grdpromotiondetail.Rows(hdnMainGridRowid.Value).FindControl("txtnoofrooms")
            txtnoofrooms.Text = strChkdStringVal.ToString
        End If
        ModalRoomPopup.Hide()


    End Sub
    Private Function FillContracts() As Boolean '*** Danny cnaged Sub to function with a return value 19/03/2018
        Try

            FillContracts = False '*** Danny 19/03/2018
            Dim dt As New DataTable
            Dim strSqlQry As String = ""

            ViewState("CountryList") = wucCountrygroup.checkcountrylist

            Dim strMealPlans As String = ""
            Dim strCondition As String = ""
            If ViewState("CountryList").ToString.Length > 0 Then

                strMealPlans = Right(ViewState("CountryList"), Len(ViewState("CountryList")) - 1)


            Else
                strMealPlans = ""
            End If

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

            ''' Min date and Max date
            Dim dtdatesnew As New DataTable
            Dim dsdates As New DataSet
            Dim dr As DataRow

            dtdatesnew.Columns.Add(New DataColumn("fromdate", GetType(String)))
            dtdatesnew.Columns.Add(New DataColumn("todate", GetType(String)))

            For Each gvRow1 In grdpromotiondetail.Rows
                Dim txtfromdate As TextBox = gvRow1.Findcontrol("txtfromdate")
                Dim txttodate As TextBox = gvRow1.Findcontrol("txttodate")

                If txtfromdate.Text <> "" And txttodate.Text <> "" Then

                    dr = dtdatesnew.NewRow

                    dr("fromdate") = Format(CType(txtfromdate.Text, Date), "yyyy/MM/dd")
                    dr("todate") = Format(CType(txttodate.Text, Date), "yyyy/MM/dd")
                    dtdatesnew.Rows.Add(dr)

                End If

            Next
            Dim mindate As Date
            Dim maxdate As Date
            If dtdatesnew.Rows.Count > 0 Then
                mindate = Convert.ToDateTime(dtdatesnew.Compute("min(fromdate)", ""))
                maxdate = Convert.ToDateTime(dtdatesnew.Compute("max(todate)", ""))
            End If

            '''''''''''

            If ddlapplydiscount.SelectedIndex = 1 Then
                gvMarkupFormulas.Columns(3).Visible = False
                gvMarkupFormulas.Columns(4).Visible = False
                gvMarkupFormulas.Columns(2).Visible = True
                gvMarkupFormulas.Columns(6).Visible = True

                'strSqlQry = " select distinct c.contractid,'' promotionid,'' promotionname,convert(varchar(10),c.fromdate,103) fromdate,convert(varchar(10),c.todate,103) todate,c.applicableto from view_contracts_search c, view_contractcountry ct where c.contractid=ct.contractid and  " _
                '                   & " c.partycode='" & hdnpartycode.Value & "' and ct.ctrycode  IN (" & strCondition & " )  "

                'strSqlQry = " select distinct c.contractid,'' promotionid,'' promotionname,convert(varchar(10),c.fromdate,103) fromdate ,convert(varchar(10),c.todate,103) todate,c.applicableto from view_contracts_search c, view_contractcountry ct where c.contractid=ct.contractid and  " _
                '                  & " c.partycode='" & hdnpartycode.Value & "' and ct.ctrycode in (select ctrycode from view_offers_countries where promotionid='" & txtpromotionid.Text & "') "

            ElseIf ddlapplydiscount.SelectedIndex = 2 Then
                gvMarkupFormulas.Columns(2).Visible = False
                gvMarkupFormulas.Columns(4).Visible = True
                gvMarkupFormulas.Columns(3).Visible = True
                ' gvMarkupFormulas.Columns(6).Visible = False

                'strSqlQry = "  select distinct '' contractid , h.promotionid,h.promotionname, h.applicableto ,    STUFF( (SELECT ','+convert(varchar(10),(convert(datetime,t1.fromdate,111)),103) +'-'+ convert(varchar(10),(convert(datetime,t1.todate,111)),103)+char(10) FROM view_offers_detail t1(nolock)   " _
                '& " WHERE(t1.promotionid = t2.promotionid) FOR XML PATH ('')) , 1, 1, '')  as fromdate, '' todate from view_offers_header h, view_offers_detail t2,view_offers_countries vc  where h.promotionid =t2.promotionid  and h.promotionid =vc.promotionid    " _
                '& "  and   vc.ctrycode  IN (" & strCondition & " ) and t2.promotionid<>'" & txtpromotionid.Text & "' "


            ElseIf ddlapplydiscount.SelectedIndex = 3 Then
                gvMarkupFormulas.Columns(2).Visible = True
                gvMarkupFormulas.Columns(4).Visible = True
                gvMarkupFormulas.Columns(3).Visible = True
                '   gvMarkupFormulas.Columns(6).Visible = True

                'strSqlQry = " select distinct c.contractid,'' promotionid,'' promotionname,convert(varchar(10),c.fromdate,103) fromdate,convert(varchar(10),c.todate,103) todate,c.applicableto from view_contracts_search c, view_contractcountry ct where c.contractid=ct.contractid and  " _
                '                  & " c.partycode='" & hdnpartycode.Value & "' and ct.ctrycode  IN (" & strCondition & " )  union all " _
                '               & " select distinct '' contractid , h.promotionid,h.promotionname,    STUFF( (SELECT ','+convert(varchar(10),(convert(datetime,t1.fromdate,111)),103) +'-'+ convert(varchar(10),(convert(datetime,t1.todate,111)),103)+char(10) FROM view_offers_detail t1(nolock)   " _
                '            & " WHERE(t1.promotionid = t2.promotionid) FOR XML PATH ('')) , 1, 1, '')  as fromdate, '' todate, h.applicableto  from view_offers_header h, view_offers_detail t2,view_offers_countries vc  where h.promotionid =t2.promotionid  and h.promotionid =vc.promotionid    " _
                '            & "  and   vc.ctrycode  IN (" & strCondition & " ) and t2.promotionid<>'" & txtpromotionid.Text & "' "
            End If

            Dim myDS As New DataSet
            If ddlapplydiscount.SelectedIndex <> 0 Then
                strSqlQry = "sp_showapplydiscount'" & CType(hdnpartycode.Value, String) & "' , '" & Format(CType(mindate, Date), "yyyy/MM/dd") & "' ,'" & Format(CType(maxdate, Date), "yyyy/MM/dd") & "','" & ViewState("CountryList") & "','" & txtpromotionid.Text & "', '" & ddlapplydiscount.Value & "'"

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                myDataAdapter.Fill(myDS)
                gvMarkupFormulas.DataSource = myDS

                If myDS.Tables(0).Rows.Count > 0 Then
                    gvMarkupFormulas.DataBind()

                Else
                    gvMarkupFormulas.PageIndex = 0
                    gvMarkupFormulas.DataBind()

                End If

                'gvMarkupFormulas.DataBind()
                'gvMarkupFormulas.DataSource = dt

                'If dt.Rows.Count > 0 Then
                '    gvMarkupFormulas.DataBind()

                'Else
                '    gvMarkupFormulas.PageIndex = 0
                '    gvMarkupFormulas.DataBind()
                'End If
                btnselectcontract.Style.Add("display", "block")

                ChkExistingValdiscount(hdnapplydiscount.Value)

            End If





            FillContracts = True '*** Danny 19/03/2018
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Private Sub ChkExistingValdiscount(ByVal prm_strChkdVal As String)


        Dim arrString As String()
        Dim arrlist As String()
        Dim j As Integer = 0 'string in the form "a,b;c,d;...."

        If prm_strChkdVal <> "" Then
            arrString = prm_strChkdVal.Split(",") 'spliting for ';' 1st



            'Dim i As Integer = 0
            'For Each grdRow In gvMarkupFormulas.Rows
            '    Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
            '    If arrString.Length = i Then
            '        Exit For
            '    Else
            '        If arrString(i) <> "" Then
            '            arrlist = arrString(i).Split("/")

            '            ddlRmcat.Value = CType(arrlist(2), String)

            '        End If
            '    End If

            '    i += 1
            'Next


            For k = 0 To arrString.Length - 1
                If arrString(k) <> "" Then

                    'Dim arrAdultChild As String() = arrString(k).Split("/") 'spliting for ',' 2nd

                    For Each grdRow In gvMarkupFormulas.Rows
                        Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
                        Dim lblcontract As Label = grdRow.FindControl("lblcontract")
                        Dim lblpromotionid As Label = grdRow.FindControl("lblpromotionid")


                        If (arrString(k) = lblcontract.Text) Or (arrString(k) = lblpromotionid.Text) Then
                            chkmarkupSelect.Checked = True

                        End If

                    Next
                End If
            Next


        End If
    End Sub
    Protected Sub btnselectcontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselectcontract.Click
        If ddlapplydiscount.SelectedIndex = 0 Then Exit Sub
        If FillContracts() = True Then '*** Danny applied if condition 19/03/2018
            ModalSelectMarkup.Show()
        End If
    End Sub
    'Protected Sub ReadMoreLinkButtoncopycont_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim readmore As LinkButton = CType(sender, LinkButton)
    '        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
    '        Dim lbtext As Label = CType(row.FindControl("lblapplicable"), Label)
    '        Dim strtemp As String = ""
    '        strtemp = lbtext.Text
    '        If readmore.Text.ToUpper = UCase("More") Then

    '            lbtext.Text = lbtext.ToolTip
    '            lbtext.ToolTip = strtemp
    '            readmore.Text = "less"
    '        Else
    '            readmore.Text = "More"
    '            lbtext.ToolTip = lbtext.Text
    '            lbtext.Text = lbtext.Text.Substring(0, 10)
    '        End If
    '        ModalSelectMarkup.Show()
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("OfferMain.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub
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
    'Protected Sub chkmarkupSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim chkSelect As CheckBox = CType(sender, CheckBox)
    '    'For Each gvRow In gvMarkupFormulas.Rows
    '    '    Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkmarkupSelect"), CheckBox)
    '    '    If Not chkSelect1.ClientID = chkSelect.ClientID Then
    '    '        chkSelect1.Checked = False
    '    '    End If
    '    'Next

    '    ModalSelectMarkup.Show()

    'End Sub
   
    Protected Sub btnokcontract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnokcontract.Click
        Dim applies As String = ""
        Dim tickedornot As Boolean
        tickedornot = False
        For Each grdRow In gvMarkupFormulas.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
            chkmarkupSelect = grdRow.FindControl("chkmarkupSelect")
            If chkmarkupSelect.Checked = True Then
                tickedornot = True
                Exit For
            End If
        Next

        If tickedornot = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any ');", True)
            ModalSelectMarkup.Show()
            Exit Sub
        End If
        
        For Each grdRow In gvMarkupFormulas.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
            Dim lblcontract As Label = grdRow.FindControl("lblcontract")
            Dim lblpromotionid As Label = grdRow.FindControl("lblpromotionid")
            If chkmarkupSelect.Checked = True Then
                If lblcontract.Text <> "" Then
                    applies = applies + "," + lblcontract.Text
                End If
                If lblpromotionid.Text <> "" Then
                    applies = applies + "," + lblpromotionid.Text
                End If

            End If
        Next

        If applies.Length > 0 Then
            hdnapplydiscount.Value = Right(applies, Len(applies) - 1)

        End If

        ModalSelectMarkup.Hide()


    End Sub



    Private Sub setdata()
        Dim applies As String = ""
        For Each grdRow In gvMarkupFormulas.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
            Dim lblcontract As Label = grdRow.FindControl("lblcontract")
            Dim lblpromotionid As Label = grdRow.FindControl("lblpromotionid")
            If chkmarkupSelect.Checked = True Then
                If lblcontract.Text <> "" Then
                    applies = applies + "," + lblcontract.Text
                End If
                If lblpromotionid.Text <> "" Then
                    applies = applies + "," + lblpromotionid.Text
                End If

            End If
        Next

        If applies.Length > 0 Then
            ViewState("SelectedRecords") = ViewState("SelectedRecords") + Right(applies, Len(applies) - 1)

        End If
    End Sub
    Protected Sub gvMarkupFormulas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMarkupFormulas.PageIndexChanging
        gvMarkupFormulas.PageIndex = e.NewPageIndex
        FillContracts()
        '  setdata()
        ModalSelectMarkup.Show()
    End Sub

    Private Sub UpdateApplyDiscountIds()
        Dim applies As String = ""
        For Each grdRow In gvMarkupFormulas.Rows
            Dim chkmarkupSelect As CheckBox = CType(grdRow.FindControl("chkmarkupSelect"), CheckBox)
            Dim lblcontract As Label = grdRow.FindControl("lblcontract")
            Dim lblpromotionid As Label = grdRow.FindControl("lblpromotionid")
            If chkmarkupSelect.Checked = True Then
                If lblcontract.Text <> "" Then
                    applies = applies + "," + lblcontract.Text
                End If
                If lblpromotionid.Text <> "" Then
                    applies = applies + "," + lblpromotionid.Text
                End If

            End If
        Next

        If applies.Length > 0 Then
            hdnapplydiscount.Value = Right(applies, Len(applies) - 1)

        End If
    End Sub

    Protected Sub gv_Showroomtypes_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Showroomtypes.RowDataBound
        Try

            Dim rowid As String

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim ddlUpgrade As HtmlSelect
                Dim hdnupgradecode As HiddenField
                Dim txtrmtypcode As Label
                Dim lblrankorder As Label

                ddlUpgrade = e.Row.FindControl("ddlUpgrade")
                hdnupgradecode = e.Row.FindControl("hdnupgradecode")
                txtrmtypcode = e.Row.FindControl("txtrmtypcode")
                lblrankorder = e.Row.FindControl("lblrankorder")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgradermtyp, "rmtypname", "rmtypcode", "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and rmtypcode<>'" & txtrmtypcode.Text & "'   and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)", True)
                If ViewState("noshowclick") = 2 Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgrade, "rmtypname", "rmtypcode", "select rmtypcode,rmtypname from  partyrmtyp(nolock) where  inactive=0 and rankord >'" & lblrankorder.Text & "'   and partycode='" & hdnpartycode.Value & "' order by isnull(rankord,999)", True)


                    rowid = CType(e.Row.RowIndex, String)
                    ddlUpgrade.Attributes.Add("Onchange", "Javascript:selectrmtype('" + ddlUpgrade.ClientID + "','" + hdnupgradecode.ClientID + "')")

                ElseIf ViewState("noshowclick") = 3 Then

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgrade, "rmtypname", "rmtypcode", "select p.mealcode as rmtypcode,m.mealname rmtypname from  partymeal p(nolock),mealmast m(nolock),mainmealplans mp where m.mainmealcode=mp.mainmealcode and   p.mealcode=m.mealcode and mp.rankorder >'" & lblrankorder.Text & "' and  p.partycode='" & hdnpartycode.Value & "' order by isnull(m.rankorder,999)", True)

                    rowid = CType(e.Row.RowIndex, String)
                    ddlUpgrade.Attributes.Add("Onchange", "Javascript:selectmeal('" + ddlUpgrade.ClientID + "','" + hdnupgradecode.ClientID + "')")

                ElseIf ViewState("noshowclick") = 4 Then

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUpgrade, "rmtypname", "rmtypcode", "select prc.rmcatcode rmtypcode from partyrmcat prc,rmcatmast rc where prc.rmcatcode=rc.rmcatcode and isnull(rc.units,0) >'" & lblrankorder.Text & "' and rc.accom_extra='A' and prc.partycode='" _
                    & hdnpartycode.Value & "' order by isnull(rc.rankorder,999)", True)


                    rowid = CType(e.Row.RowIndex, String)
                    ddlUpgrade.Attributes.Add("Onchange", "Javascript:selectrmcat('" + ddlUpgrade.ClientID + "','" + hdnupgradecode.ClientID + "')")

                End If

            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
