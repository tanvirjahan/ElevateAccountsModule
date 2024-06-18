Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Linq
Imports System.Drawing
Imports System.IO
Imports System.IO.File
Imports System.Text
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
'Imports Microsoft.Office.Interop
'Imports System.Diagnostics
Imports System



Partial Class ContractValidatenew
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private wbPart As WorkbookPart = Nothing
    Private document As SpreadsheetDocument = Nothing
    Dim objutil As New clsUtils
    Private wbPart1 As WorkbookPart = Nothing
    Private document1 As SpreadsheetDocument = Nothing


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
    Dim fDatenew As New ArrayList
    Dim tDatenew As New ArrayList


#End Region
#Region "Enum GridCol"
    Enum GridCol
        MaxidTCol = 0
        Maxid = 1
        Fromdate = 2
        Todate = 3
        Countrygroup = 4
        Promotionid = 5
        Promotionname = 6
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        Session("Calledfrom") = CType(Request.QueryString("Calledfrom"), String)

        If IsPostBack = False Then



            Dim CalledfromValue As String = ""

            Dim ConValappid As String = ""
            Dim ConValappname As String = ""
            Dim Count As Integer
            Dim lngCount As Int16
            Dim strTempUserFunctionalRight As String()
            Dim strRights As String
            Dim functionalrights As String = ""


            ConValappid = 1
            ConValappname = objUser.GetAppName(Session("dbconnectionName"), ConValappid)

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else

                If Session("Calledfrom") = "Contracts" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(ConValappname, String), "ContractValidatenew.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                btnContractPrint, gv_SearchResult)


                ElseIf Session("Calledfrom") = "Offers" Then
                    Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    CalledfromValue = Me.SubMenuUserControl1.menuidval
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                          CType(ConValappname, String), "ContractValidatenew.aspx", CType(CalledfromValue, String), btnAddNew, btnExportToExcel, _
                    btnOfferPrint, gv_SearchResult)
                End If

            End If




            Dim intGroupID As Integer = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
            Dim intMenuID As Integer = objUser.GetCotractofferMenuId(Session("dbconnectionName"), "ContractValidatenew.aspx", ConValappid, CalledfromValue)

            functionalrights = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, ConValappid, intMenuID)

            If functionalrights <> "" Then
                strTempUserFunctionalRight = functionalrights.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strRights = strTempUserFunctionalRight.GetValue(lngCount)

                    If strRights = "06" Then
                        Count = 1
                    End If
                Next

                If CalledfromValue = 1030 Then
                    btnOfferPrint.Visible = False


                    If Count = 1 Then
                        btnContractPrint.Visible = True
                        btnofferrates.Visible = True
                        btnothmealofferrates.Visible = True
                    Else
                        btnContractPrint.Visible = False
                        btnofferrates.Visible = False
                        btnothmealofferrates.Visible = False
                    End If

                ElseIf CalledfromValue = 1200 Then
                    btnContractPrint.Visible = False
                    If Count = 1 Then
                        btnOfferPrint.Visible = True
                        btnofferrates.Visible = True
                        btnothmealofferrates.Visible = True

                    Else
                        btnOfferPrint.Visible = False
                        btnofferrates.Visible = False
                        btnothmealofferrates.Visible = False
                    End If
                End If

            Else
                btnOfferPrint.Visible = False
                btnofferrates.Visible = False
                btnothmealofferrates.Visible = False
                btnContractPrint.Visible = False




            End If


            txtconnection.Value = Session("dbconnectionName")
            If Session("Calledfrom") = "Offers" Then
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                End If

                Session("partycode") = hdnpartycode.Value
                Dim hotelname1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = hotelname1
                lblHeading.Text = " Promotion Validate Approval  - " + hotelname1 + " - " + hdnpromotionid.Value


                btnReport.Style.Add("display", "none")
                btnofferrates.Style.Add("display", "block")
                btnothmealofferrates.Style.Add("disp    lay", "block")

            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                Me.SubMenuUserControl1.Calledfromval = CType(Request.QueryString("Calledfrom"), String)

                hdnpartycode.Value = CType(Session("Contractparty"), String)
                SubMenuUserControl1.partyval = hdnpartycode.Value
                SubMenuUserControl1.contractval = CType(Session("contractid"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                Dim hotelname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = hotelname
                Session("partycode") = hdnpartycode.Value

                '  Session("contractid") = CType(Request.QueryString("contractid"), String)
                lblHeading.Text = lblHeading.Text + " - " + hotelname + " - " + hdncontractid.Value

                ' btnContractPrint.Style.Add("display", "block")
                'btnContractPrint.Visible = True
                ' btnOfferPrint.Visible = False
                btnReport.Style.Add("display", "none")

                'btnofferrates.Style.Add("display", "none")
                'btnothmealofferrates.Style.Add("display", "none")
            End If



        Else

        End If

        If Session("Calledfrom") = "Offers" Then
            btnchecking.Attributes.Add("onclick", "return Checkoffers('" & hdnpromotionid.Value & "')")
            gv_SearchResult.Columns(1).HeaderText = "Promotion Id"
        Else
            btnchecking.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
            gv_SearchResult.Columns(1).HeaderText = "Contract No"
        End If


        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ContractValidateWindowPostBack") Then
        '    FillGrid()
        'End If
        If Session("ContractErrors") IsNot Nothing Then
            FillGrid()
        End If
        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex

        FillGrid()
        '  FillGrid("partymaxacc_header.tranid", hdnpartycode.Value, "Desc")


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

    End Sub
#End Region
    Private Sub FillGrid(ByVal strorderby As String, ByVal partycode As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = "" ' "select  '' constructionid , '' fromdate , '' todate , '' reason ,'' Miscellaneous ,'' adddate, '' adduser, ''  moddate ,'' moduser"
        Try
            strSqlQry = "SELECT constructionid ,convert(varchar(10),fromdate,103) fromdate,convert(varchar(10),todate,103) todate," & _
                "ISNULL(reason,'') reason,ISNULL(Miscellaneous,'') Miscellaneous, " & _
                "adddate, adduser,moddate,moduser FROM hotels_construction where partycode='" & partycode & "'"

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
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
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
    'Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        Dim readmore As LinkButton = CType(sender, LinkButton)
    '        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
    '        Dim lbtext As Label = CType(row.FindControl("lblerrormsg"), Label)
    '        Dim strtemp As String = ""
    '        strtemp = lbtext.Text
    '        If readmore.Text.ToUpper = UCase("More") Then

    '            lbtext.Text = lbtext.ToolTip
    '            lbtext.ToolTip = strtemp
    '            readmore.Text = "less"
    '        Else
    '            readmore.Text = "More"
    '            lbtext.ToolTip = lbtext.Text
    '            lbtext.Text = lbtext.Text.Substring(0, 50)
    '        End If

    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub







    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region







    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim dsmissing As DataSet
            Dim dtmissing As DataTable
            Dim lblTranid As Label = Nothing
            Dim lbloptions As Label = Nothing
            Dim lblErrType As Label

            Dim dvmissingdates As DataView

            Session("tranid") = Nothing
            lblTranid = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblTranid")
            lbloptions = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbloptions")
            lblErrType = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblErrType")

            If e.CommandName = "Details" Then
                gvErrorlist.Columns(1).Visible = True
                gvErrorlist.Columns(2).Visible = True
                gvErrorlist.Columns(3).Visible = True
                gvErrorlist.Columns(4).Visible = True
                gvErrorlist.Columns(3).HeaderText = "Country"

                If Not Session("Missingdates") Is Nothing And lblErrType.Text = 1 Then


                    dtmissing = Session("Missingdates")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "'  and optionname='" & lbloptions.Text & "'")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()



                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = True
                            gvErrorlist.Columns(2).Visible = True
                            gvErrorlist.Columns(3).Visible = False
                            gvErrorlist.Columns(4).Visible = False

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If


                End If
                If Not Session("MissingCountryagent") Is Nothing And lblErrType.Text = 2 Then

                    dtmissing = Session("MissingCountryagent")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "' and errtype=" & lblErrType.Text & " and optionname='" & lbloptions.Text & "' ")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()

                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = False
                            gvErrorlist.Columns(2).Visible = False
                            gvErrorlist.Columns(3).Visible = True
                            gvErrorlist.Columns(4).Visible = True

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If

                End If

                If Not Session("MissingCountryagent") Is Nothing And lblErrType.Text = 3 Then
                    gvErrorlist.Columns(3).HeaderText = "Room Type"


                    dtmissing = Session("MissingCountryagent")
                    dvmissingdates = dtmissing.DefaultView
                    dvmissingdates.RowFilter = ("tranid='" & lblTranid.Text & "' and errtype=" & lblErrType.Text & " and optionname='" & lbloptions.Text & "'")

                    gvErrorlist.DataSource = dvmissingdates
                    gvErrorlist.DataBind()

                    If Not dtmissing Is Nothing Then
                        If dtmissing.Rows.Count > 0 Then
                            gvErrorlist.Visible = True
                            gvErrorlist.Columns(1).Visible = False
                            gvErrorlist.Columns(2).Visible = False
                            gvErrorlist.Columns(3).Visible = True
                            gvErrorlist.Columns(4).Visible = False

                        End If
                    Else
                        gvErrorlist.Visible = False
                    End If

                End If





            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "BindGv", "BindGrid();", True)



        Catch ex As Exception


            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Private Function Validatepage() As Boolean
        Dim chkcontract As String = ""
        If Session("Calledfrom") = "Offers" Then
            chkcontract = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from edit_Offers_header(nolock) where partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'")

            If chkcontract = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Offer  Already Approved Please check.');", True)
                Validatepage = False
                Exit Function

            End If
        Else
            chkcontract = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from edit_contracts(nolock) where partycode='" & hdnpartycode.Value & "' and contractid='" & hdncontractid.Value & "'")

            If chkcontract = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Contract  Already Approved Please check.');", True)
                Validatepage = False
                Exit Function

            End If
        End If




        Validatepage = True
    End Function
    Protected Sub btnchecking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchecking.Click
        'ViewState("State") = "New"
        'PanelMain.Visible = True
        'PanelMain.Style("display") = "block"
        ' Panelsearch.Enabled = False

        If Validatepage() = False Then
            Exit Sub
        End If


        If Session("Calledfrom") = "Offers" Then
            lblHeading.Text = " Promotion Validate For Approval  - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "Promotion Validate For Approval  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value
        Else
            lblHeading.Text = " Validate For Approval  - " + ViewState("hotelname") + " - " + hdncontractid.Value
            Page.Title = Page.Title + " " + "Validate For Approval  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        End If

        Session("ContractErrors") = Nothing

        FillGrid()

        'Dim ds As New DataSet

        'ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_contract " & "'" & hdncontractid.Value & "'")

        'gv_SearchResult.DataSource = ds.Tables(0)
        'gv_SearchResult.DataBind()

        'If ds.Tables(1).Rows.Count > 0 Then
        '    Session("Missingdates") = ds.Tables(1)
        'End If
        'If ds.Tables(2).Rows.Count > 0 Then
        '    Session("MissingCountryagent") = ds.Tables(2)
        'End If

        'btnchecking.Text = "Checking Completed"
        'btnchecking.Enabled = False

        'If ds.Tables(0).Rows.Count = 0 Then

        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Records. You can Approve the selected !.');", True)

        'End If


    End Sub

    Private Sub FillGrid()
        Dim myDS As New DataSet
        Try
            gv_SearchResult.Visible = True


            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            Dim strSqlQry As String = ""

            Dim ds As New DataSet

            If Session("ContractErrors") IsNot Nothing Then
                gv_SearchResult.DataSource = Session("ContractErrors")
                gv_SearchResult.DataBind()

                Session("ContractErrors") = Nothing

            Else
                If Session("Calledfrom") = "Offers" Then
                    ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers " & "'" & hdnpromotionid.Value & "'")
                Else
                    ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_contract " & "'" & hdncontractid.Value & "'")
                End If


                gv_SearchResult.DataSource = ds.Tables(0)
                gv_SearchResult.DataBind()

                If ds.Tables(3).Rows.Count > 0 Then
                    Session("ErrorList") = ds.Tables(3)
                End If

                If ds.Tables(1).Rows.Count > 0 Then
                    Session("Missingdates") = ds.Tables(1)
                End If
                If ds.Tables(2).Rows.Count > 0 Then
                    Session("MissingCountryagent") = ds.Tables(2)
                End If

                If ds.Tables(0).Rows.Count = 0 Then
                    btnchecking.Text = "Checking Completed"
                    btnchecking.Enabled = False
                    btnReport.Style.Add("display", "block")

                    '  ModalPopupError.Show()
                End If
            End If




        Catch ex As Exception
            ModalPopupError.Hide()
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbConnectionClose(mySqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description : " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx::FillGrid :: ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub







    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lnkDetails As LinkButton = e.Row.FindControl("lnkDetails")
            Dim lblErrType As Label = e.Row.FindControl("lblErrType")

            If lblErrType.Text = "0" Then
                lnkDetails.Enabled = False
            End If


        End If

    End Sub

    Protected Sub btnrefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnrefresh.Click
        gv_SearchResult.DataSource = Nothing
        gv_SearchResult.DataBind()
        gvErrorlist.DataSource = Nothing
        gvErrorlist.DataBind()

        Session("ContractErrors") = Nothing
        Session("Missingdates") = Nothing
        Session("MissingCountryagent") = Nothing
        Session("ErrorList") = Nothing

        btnchecking.Enabled = True
        btnchecking.Text = "Start Checking"
        btnReport.Style.Add("display", "none")
    End Sub
    Private Function calCellHeightForPromotion(ByVal strlen As Integer) As Double
        Dim a As Integer
        If ((strlen > 90) And (strlen < 180)) Then
            a = 2
        Else
            a = strlen / 90
        End If
        'a = strlen / 132
        a = a * 12.75
        If a > 409 Then a = 409
        If a < 1 Then
            Return 12.75
        Else
            Return a
        End If
    End Function
    'Private Sub autoheight(ByVal Target As Excel.Range, ByVal xlapp As Excel.Application)
    '    Dim NewRwHt As Single
    '    Dim cWdth As Single, MrgeWdth As Single
    '    Dim c As Excel.Range, cc As Excel.Range
    '    Dim ma As Excel.Range

    '    With Target
    '        If .MergeCells And .WrapText Then
    '            c = Target.Cells(1, 1)
    '            cWdth = c.ColumnWidth
    '            ma = c.MergeArea
    '            For Each cc In ma.Cells
    '                MrgeWdth = MrgeWdth + cc.ColumnWidth
    '            Next
    '            xlapp.ScreenUpdating = False
    '            ma.MergeCells = False
    '            c.ColumnWidth = MrgeWdth
    '            c.EntireRow.AutoFit()
    '            NewRwHt = c.RowHeight
    '            c.ColumnWidth = cWdth
    '            ma.MergeCells = True
    '            ma.RowHeight = NewRwHt
    '            cWdth = 0 : MrgeWdth = 0
    '            xlapp.ScreenUpdating = True
    '        End If
    '    End With
    'End Sub
    Protected Sub btnofferPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOfferPrint.Click

        Try
            Dim xlApp
            Dim xlBook
            Dim xlSheet

            Dim FolderPath As String = "..\ExcelTemplates\"
            Dim FileName As String = "OfferPrint.xlsx"
            Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            FolderPath = "~\ExcelTemp\"
            Dim FileNameNew As String = "OfferPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            File.Copy(FilePath, outputPath, True)
            document = SpreadsheetDocument.Open(outputPath, True)
            wbPart = document.WorkbookPart




            Dim activestate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(activestate,'') activestate  from view_Offer_search where promotionid='" & hdnpromotionid.Value & "'")

            Dim wsName As String = "Index"

            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", hdnpromotionid.Value, 3, True, False)
            UpdateValue(wsName, "B5", activestate, 3, True, False)


            wsName = "Main Detail"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 2, True, False)
            'UpdateValue(wsName, "B5", hdnpromotionid.Value, 3, True, False)
            'UpdateValue(wsName, "A6", "From Date", 3, True, False)



            Dim dt31 As New DataTable

            strSqlQry = "select distinct q.item1 as promotiontypes FROM view_offers_header cross apply dbo.SplitString1colsWithOrderField(promotiontypes,',') q  where  promotionid= '" & hdnpromotionid.Value & "'"
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

            strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto from view_offers_header h where  h.partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'"
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
            strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_offers_countries v where v.ctrycode =p.ctrycode  and v.promotionid ='" & hdnpromotionid.Value & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
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
            strSqlQry = "select promotiontypes from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rso)
            SqlConn.Close()
            UpdateTableValue(wsName, rso, 1, iLineNo1m, 2, True)



            'xlSheet.Range("B" & iLineNo1m).WrapText = True
            UpdateValue(wsName, "C" & iLineNo1m.ToString, "Days Of The Week", 3, True, False)

            strSqlQry = "SELECT daysoftheweek from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
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
                              & " where promotionid='" & hdnpromotionid.Value & "'"
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
                                & " isnull(cast(nullif(maxfeenights, 0) as varchar(10)),'') maxfeenights ,isnull(cast(nullif(maxmultiples, 0) as varchar(10)),'') maxmultiples  from view_offers_DETAIL  where promotionid='" & hdnpromotionid.Value & "'"
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
                            & " isnull(cast(nullif(stayfor, 0) as varchar(10)),'') stayfor,isnull(cast(nullif(payfor, 0) as varchar(10)),'') payfor ,isnull(cast(nullif(maxfeenights, 0) as varchar(10)),'') maxfeenights ,isnull(cast(nullif(maxmultiples, 0) as varchar(10)),'')  maxmultiples  from view_offers_DETAIL  where promotionid='" & hdnpromotionid.Value & "'"
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
                            & " isnull(cast(nullif(minnights, 0) as varchar(10)),'')  minnights ,isnull(cast(nullif(maxnights, 0) as varchar(10)),'') maxnights  from view_offers_detail  where promotionid='" & hdnpromotionid.Value & "'"
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

            strSqlQry = "select  p.rmtypname from view_offers_rmtype d  cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.promotionid='" & hdnpromotionid.Value & "' and p.partycode ='" & hdnpartycode.Value & "' order by rankord"
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
                        strSqlQry = " select r.rmtypname from view_offers_rmtype s join partyrmtyp r on s.rmtypeupgrade=r.rmtypcode and  ( isnull(s.rmtypeupgrade,'')<>'[Select]' and  isnull(s.rmtypeupgrade,'')<>'') and  s.promotionid='" & hdnpromotionid.Value & "' and r.partycode ='" & hdnpartycode.Value & "' order by rankord"

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


            strSqlQry = "select  p.mealname from view_offers_meal d  cross apply dbo.SplitString1colsWithOrderField(d.mealcode,',') q join mealmast p on q.Item1=p.mealcode and  d.promotionid='" & hdnpromotionid.Value & "' order by rankorder "
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
                        strSqlQry = "select r.mealname from view_offers_meal s join mealmast r on r.mealcode=s.mealupgrade and ( isnull(s.mealupgrade,'')<>'[Select]' and  isnull(s.mealupgrade,'')<>'') and promotionid='" & hdnpromotionid.Value & "' order by rankorder"
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




            strSqlQry = "select  p.rmcatname from view_offers_accomodation d  cross apply dbo.SplitString1colsWithOrderField(d.rmcatcode,',') q join rmcatmast p on q.Item1=p.rmcatcode and  d.promotionid='" & hdnpromotionid.Value & "' order by rankorder"

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

                        strSqlQry = " select r.rmcatname from view_offers_accomodation s join rmcatmast on r.rmcatcode=s.rmcatupgrade and  ( isnull(s.rmcatupgrade,'')<>'[Select]' and  isnull(s.rmcatupgrade,'')<>'') and promotionid='" & hdnpromotionid.Value & "' order by rankorder"


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

            strSqlQry = "Select d.rmcatcode from view_offers_supplement d,rmcatmast r   where r.rmcatcode=d.rmcatcode and promotionid='" & hdnpromotionid.Value & "' order by rankorder"
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
            'strSqlQry = "select isnull(max(rmcount),0) as rmcount  from view_maxvariable  where partycode= '" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'"
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(dt2o1)
            'SqlConn.Close()

            iLineNo1m = iLineNo1m + Maxint + 2



            Dim dt2o As New DataTable
            strSqlQry = "select inventorytype,combinetype,commissiontype,isnull(specialoccassion,'') specialoccassion ,remarks,arrivaltransfer,departuretransfer,isnull(applydiscounttype,'') applydiscounttype,isnull(applydiscountids,'') applydiscountids  from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
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
                        strSqlQry = "Select h.promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.combinableid and c.promotionid='" & hdnpromotionid.Value & "'"
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
                        strSqlQry = "Select h.promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.combinableid and c.promotionid='" & hdnpromotionid.Value & "'"
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
            strSqlQry = "select  case when ISNULL(nonrefundable,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(applytdiscountoexhibition,0)=0 then 'No' else 'Yes' end  status, case when ISNULL(arrivaltransfer,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(departuretransfer,0)=0 then 'No' else 'Yes' end  status   from view_offers_header h where  promotionid='" & hdnpromotionid.Value & "'"
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
                                strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Arrival' and promotionid='" & hdnpromotionid.Value & "'"
                                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                                myDataAdapter.Fill(rsdmm1a1a)
                                SqlConn.Close()
                                UpdateTableValue(wsName, rsdmm1a1a, 3, iLineNo1m, 2, True)


                                'xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True
                                xa = rsdmm1a1a.Rows.Count

                                Dim rsdmm1a1ad As New DataTable
                                strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Departure' and promotionid='" & hdnpromotionid.Value & "'"
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
                        strSqlQry = "select flightcode from view_offers_flight where  promotionid='" & hdnpromotionid.Value & "'"
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

                            strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & hdnpromotionid.Value & "'"

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

                            strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & hdnpromotionid.Value & "'"
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
            strSqlQry = "select tranid from view_contracts_commission_header where promotionid = '" & hdnpromotionid.Value & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt3)
            SqlConn.Close()
            If dt3.Rows.Count > 0 Then
                For i As Integer = 0 To dt3.Rows.Count - 1


                    UpdateValue(wsName, "A" & iLineNo - 1.ToString, "Promotionid", 3, True, False)
                    UpdateValue(wsName, "B" & iLineNo - 1.ToString, "Promotionname", 3, True, False)
                    UpdateValue(wsName, "C" & iLineNo - 1.ToString, "ApplicableTo", 3, True, False)

                    strSqlQry = "select h.promotionid,h.promotionname,applicableto   from view_offers_header h WHERE h.partycode='" & hdnpartycode.Value & "' and h.promotionid='" & hdnpromotionid.Value & "'"
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

                    strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & hdnpromotionid.Value & "' and isnull(h.roomcategory,'')<>'' order by prm.rankorder for xml path('')),1,1,''),'') "

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


                    strSqlQry = "select roomType=isnull(stuff((select ',' + prm.rmtypname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and partycode='" & hdnpartycode.Value & "' and isnull(h.roomtypes,'')<>''  order by rankord for xml path('')),1,1,''),'')"
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
                    strSqlQry = "select  p.mealname from view_contracts_commission_header d  cross apply dbo.SplitString1colsWithOrderField(d.mealplans,',') q join mealmast p on q.Item1=p.mealcode  and d.tranid='" & dt3.Rows(i)("tranid").ToString & "' and d.promotionid='" & hdnpromotionid.Value & "' order by rankorder "
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


            strSqlQry = "select distinct tranid from view_partymaxacc_header where partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'"
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

                    strSqlQry = "select h.promotionid,h.promotionname ,applicableto from view_offers_header h WHERE h.partycode='" & hdnpartycode.Value & "' and h.promotionid='" & hdnpromotionid.Value & "'"
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
                    strSqlQry = "select v.rmtypcode from view_partymaxaccomodation v ,partyrmtyp  p  where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and    v.partycode= '" & hdnpartycode.Value & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  p.rankord"
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
                             & "  And h.partycode='" & hdnpartycode.Value & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

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
                    strSqlQry = "select  start0based from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & hdnpartycode.Value & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord "
                    Dim rsmx2 As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx2)
                    SqlConn.Close()
                    em2 = rsmx2.Rows.Count

                    UpdateTableValue(wsName, rsmx2, 13, iLinmx, 2, True)


                    If dtmx1.Rows.Count > 0 Then
                        For idt As Integer = 0 To dtmx1.Rows.Count - 1



                            strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & hdnpartycode.Value & "' order by maxadults for xml path('')),1,1,''),'') "

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
            strSqlQry = "select plistcode from view_cplisthnew  where promotionid='" & hdnpromotionid.Value & "'"
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

                    strSqlQry = "select h.plistcode,h.promotionid,p.promotionname,h.applicableto  from view_offers_header p, view_cplisthnew  h WHERE p.promotionid=h.promotionid and h.partycode='" & hdnpartycode.Value & "' and h.promotionid='" & hdnpromotionid.Value & "' and plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
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

                    'strSqlQry = "select  c.mealplans,m.mealname from view_cplisthnew c,mealmast m where m.mealcode=c.mealplans and  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and  c.promotionid='" & hdnpromotionid.Value & "'"
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
            strSqlQry = "select exhibitionid from view_contracts_exhibition_header  where promotionid= '" & hdnpromotionid.Value & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dte)
            Dim iLinee As Integer = 6
            Dim ei As Integer
            Dim ze As Integer
            Dim chkc As Integer

            If dte.Rows.Count > 0 Then
                For i As Integer = 0 To dte.Rows.Count - 1



                    strSqlQry = "select c.exhibitionid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h, view_contracts_exhibition_header c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "' and  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
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





                            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'  and p.partycode ='" & hdnpartycode.Value & "' and d.exhibitioncode='" & dter.Rows(er)("exhibitioncode").ToString & "' for xml path('')),1,1,''),'') "
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
            strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header where promotionid= '" & hdnpromotionid.Value & "'"
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
                        & " where h.promotionid=c.promotionid and   h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "'  and  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"

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

                    'strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & hdnpromotionid.Value & "' and isnull(h.roomcategory,'')<>'' for xml path('')),1,1,''),'')"

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
            strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where promotionid='" & hdnpromotionid.Value & "'"
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

                    'strSqlQry = "select c.childpolicyid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d ,view_contracts_childpolicy_header c where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "'  and  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"

                    strSqlQry = "select c.childpolicyid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_contracts_childpolicy_header c  " _
                        & " where h.promotionid=c.promotionid and   h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "'  and  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"

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
            strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where  promotionid= '" & hdnpromotionid.Value & "'"
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


                    strSqlQry = "select c.cancelpolicyid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h, view_contracts_cancelpolicy_header   c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "' AND  cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

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



                    '        strSqlQry = "select distinct d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & hdncontractid.Value & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and h.promotionid= '" & hdnpromotionid.Value & "'"
                    '        Dim rscp As New ADODB.Recordset
                    '        rscp = GetResultAsRecordSet(strSqlQry)
                    '        co2 = rscp.RecordCount
                    '        xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)

                    '            '        strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto,d.fromdate,d.todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status    from view_offers_header h,view_offers_detail d, view_contracts_cancelpolicy_header c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='RI13' and  h.promotionid= '" & hdnpromotionid.Value & "'"
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

                            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from  view_contracts_cancelpolicy_detail d  cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and clineno='" & dtcl.Rows(cl)("clineno").ToString & "' and p.partycode='" & hdnpartycode.Value & "' for xml path('')),1,1,''),'') "

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
                            strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,''),''), mealplans,d.noshowearly,chargebasis,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

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
            strSqlQry = "select checkinoutpolicyid FROM view_contracts_checkinout_header where promotionid='" & hdnpromotionid.Value & "'"
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

                    strSqlQry = "select c.checkinoutpolicyid ,h.promotionid,h.promotionname ,h.applicableto,isnull(c.checkintime,0) checkintime,isnull(c.checkouttime,0) checkouttime  from view_offers_header h, view_contracts_checkinout_header  c(nolock)  where h.promotionid=c.promotionid  and h.partycode='" & hdnpartycode.Value & "'  and  h.promotionid='" & hdnpromotionid.Value & "'"
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


                    strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & hdnpartycode.Value & "' for xml path('')),1,1,''),'') "
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
            strpop = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)




            ''''


            ''''''''''''29/03/17 as per Madam Commented taking time to load 

            'wsName = "Final Offer Rates"
            'UpdateValue(wsName, "B4", ViewState("hotelname"), 2, True, False)


            ''Dim sellingexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_final_contracted_rates where promotionid='" & hdnpromotionid.Value & "'")
            ''If sellingexists <> "" Then


            '' strSqlQry = "select distinct plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"

            'strSqlQry = "select promotionid,calculatedid,autoid,plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb, " _
            '    & " noofadulteb,noofchildeb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,discounttype,discount,addldiscount,exhibitionprice,adultebprice,adultebpricedisc,extrapaxprice,extrapaxpricedisc,totalsharingcharge,totalsharingdiscount,totalebcharge,totalebdiscount,totalprice,pricewithfreenight,nights,minstay,minstayoption,stayfor,freenights,rmtypupgradefrom,rmtypupgradefromname,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_offer_rates where promotionid = '" & hdnpromotionid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"


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
            '    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_offer_rates_othmeal where promotionid ='" & hdnpromotionid.Value & "' order by rmtyporder, " _
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
    Public Shared Function CreateExcelDocument(ByVal ds As DataSet, ByVal excelFilename As String) As Boolean
        Try
            Using document As SpreadsheetDocument = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook)

                Dim workbook As WorkbookPart = document.AddWorkbookPart

                '                document.AddWorkbookPart()
                document.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()

                '  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                document.WorkbookPart.Workbook.Append(New BookViews(New WorkbookView()))

                '  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                Dim workbookStylesPart As WorkbookStylesPart = document.WorkbookPart.AddNewPart(Of WorkbookStylesPart)("rIdStyles")

                Dim stylesheet As New Stylesheet
                workbookStylesPart.Stylesheet = stylesheet
                workbookStylesPart.Stylesheet.Save()

                '                Dim sp As WorkbookStylesPart = document.WorkbookPart.AddNewPart(Of WorkbookStylesPart)()

                CreateParts(ds, document)

            End Using
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Shared Sub CreateParts(ByVal ds As DataSet, ByVal spreadsheet As SpreadsheetDocument)

        '  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
        Dim worksheetNumber As UInt64 = 1
        For Each dt As DataTable In ds.Tables
            '  For each worksheet you want to create
            Dim workSheetID As String = "rId" + worksheetNumber.ToString()
            Dim worksheetName As String = dt.TableName

            Dim newWorksheetPart As WorksheetPart = spreadsheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
            newWorksheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet()

            '  If you want to define the Column Widths, you need to do this *before* appending the SheetData
            '  http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/
            '
            '  If you want to calculate the column width, it's not easy.  Have a read of this article:
            '  http://polymathprogrammer.com/2010/01/11/custom-column-widths-in-excel-open-xml/
            '

            Dim columnWidthSize As Int32 = 20     ' Replace the following line with your desired Column Width for column # col
            Dim columns As New Columns

            For colInx As Integer = 0 To dt.Columns.Count
                Dim column As Column = CustomColumnWidth(colInx, columnWidthSize)
                columns.Append(column)
            Next
            newWorksheetPart.Worksheet.Append(columns)

            ' create sheet data
            newWorksheetPart.Worksheet.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.SheetData())

            ' save worksheet
            WriteDataTableToExcelWorksheet(dt, newWorksheetPart)
            newWorksheetPart.Worksheet.Save()

            ' create the worksheet to workbook relation
            If (worksheetNumber = 1) Then
                spreadsheet.WorkbookPart.Workbook.AppendChild(New DocumentFormat.OpenXml.Spreadsheet.Sheets())
            End If

            Dim sheet As DocumentFormat.OpenXml.Spreadsheet.Sheet = New DocumentFormat.OpenXml.Spreadsheet.Sheet
            sheet.Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart)
            sheet.SheetId = worksheetNumber
            sheet.Name = dt.TableName
            '            Sheets.Append(sheet)

            spreadsheet.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets).Append(sheet)
            ' AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
        Next
    End Sub

    Private Shared Sub WriteDataTableToExcelWorksheet(ByVal dt As DataTable, ByVal worksheetPart As WorksheetPart)

        Dim worksheet As Worksheet = worksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()

        Dim cellValue As String = ""

        '  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        '
        '  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        '  cells of data, we'll know if to write Text values or Numeric cell values.
        Dim numberOfColumns As Integer = dt.Columns.Count
        Dim IsNumericColumn(numberOfColumns) As Boolean

        Dim excelColumnNames([numberOfColumns]) As String

        For n As Integer = 0 To numberOfColumns
            excelColumnNames(numberOfColumns) = GetExcelColumnName(n)
        Next n

        '
        '  Create the Header row in our Excel Worksheet
        '
        Dim rowIndex As UInt32 = 1

        Dim headerRow As Row = New Row
        headerRow.RowIndex = rowIndex            ' add a row at the top of spreadsheet
        sheetData.Append(headerRow)

        For colInx As Integer = 0 To numberOfColumns - 1
            Dim col As DataColumn = dt.Columns(colInx)
            AppendTextCell(excelColumnNames(colInx) + "1", col.ColumnName, headerRow)
            IsNumericColumn(colInx) = (col.DataType.FullName = "System.Decimal") Or (col.DataType.FullName = "System.Int32")
        Next

        '
        '  Now, step through each row of data in our DataTable...
        '
        Dim cellNumericValue As Double = 0

        For Each dr As DataRow In dt.Rows
            ' ...create a new row, and append a set of this row's data to it.
            rowIndex = rowIndex + 1
            Dim newExcelRow As New Row
            newExcelRow.RowIndex = rowIndex         '  add a row at the top of spreadsheet
            sheetData.Append(newExcelRow)

            For colInx As Integer = 0 To numberOfColumns - 1
                cellValue = dr.ItemArray(colInx).ToString()

                ' Create cell with data
                If (IsNumericColumn(colInx)) Then
                    '  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                    '  If this numeric value is NULL, then don't write anything to the Excel file.
                    cellNumericValue = 0
                    If (Double.TryParse(cellValue, cellNumericValue)) Then
                        cellValue = cellNumericValue.ToString()
                        AppendNumericCell(excelColumnNames(colInx) + rowIndex.ToString(), cellValue, newExcelRow)
                    End If
                Else
                    '  For text cells, just write the input data straight out to the Excel file.
                    AppendTextCell(excelColumnNames(colInx) + rowIndex.ToString(), cellValue, newExcelRow)
                End If
            Next

        Next

    End Sub

    Private Sub callexcel(ByVal ds As DataSet, ByVal excelFilename As String, ByVal spreadsheet As SpreadsheetDocument, ByVal workbook1 As WorkbookPart)

        Using workbook = SpreadsheetDocument.Open(excelFilename, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook)
            Dim workbookPart = workbook1 ' workbook.AddWorkbookPart()
            workbook.WorkbookPart.Workbook = New DocumentFormat.OpenXml.Spreadsheet.Workbook()
            workbook.WorkbookPart.Workbook.Sheets = New DocumentFormat.OpenXml.Spreadsheet.Sheets()

            Dim sheetId As UInteger = 10



            For Each table As DataTable In ds.Tables
                Dim sheetPart = workbook.WorkbookPart.AddNewPart(Of WorksheetPart)()
                Dim sheetData = New DocumentFormat.OpenXml.Spreadsheet.SheetData()
                sheetPart.Worksheet = New DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData)


                Dim ws As Worksheet = DirectCast(workbook1.GetPartById(sheetId), WorksheetPart).Worksheet




                Dim sheets As DocumentFormat.OpenXml.Spreadsheet.Sheets = workbook.WorkbookPart.Workbook.GetFirstChild(Of DocumentFormat.OpenXml.Spreadsheet.Sheets)()
                Dim relationshipId As String = workbook.WorkbookPart.GetIdOfPart(ws.WorksheetPart)

                If sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().Count() > 0 Then
                    sheetId = sheets.Elements(Of DocumentFormat.OpenXml.Spreadsheet.Sheet)().[Select](Function(s) s.SheetId.Value).Max() + 1
                End If

                Dim sheet1 As New DocumentFormat.OpenXml.Spreadsheet.Sheet()


                sheets.Append(sheet1)

                Dim headerRow As New DocumentFormat.OpenXml.Spreadsheet.Row()

                Dim columns As List(Of [String]) = New List(Of String)()
                For Each column As DataColumn In table.Columns
                    columns.Add(column.ColumnName)

                    Dim cell As New DocumentFormat.OpenXml.Spreadsheet.Cell()
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.[String]
                    cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName)
                    headerRow.AppendChild(cell)
                Next

                ws.AppendChild(headerRow)

                For Each dsrow As DataRow In table.Rows
                    Dim newRow As New DocumentFormat.OpenXml.Spreadsheet.Row()
                    For Each col As [String] In columns
                        Dim cell As New DocumentFormat.OpenXml.Spreadsheet.Cell()
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.[String]
                        cell.CellValue = New DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow(col).ToString())
                        '
                        newRow.AppendChild(cell)
                    Next

                    ws.AppendChild(newRow)
                Next
            Next
        End Using
    End Sub
    Public Shared Function ListToDataTable(Of T)(ByVal list As List(Of T)) As DataTable

        Dim dt As New DataTable
        Dim row As DataRow
        For Each info As System.Reflection.PropertyInfo In list.GetType().GetProperties()
            dt.Columns.Add(New DataColumn(info.Name, GetNullableType(info.PropertyType)))
        Next

        For Each tValue As T In list

            row = dt.NewRow()
            For Each info As System.Reflection.PropertyInfo In list.GetType().GetProperties()

                If Not IsNullableType(info.PropertyType) Then
                    row(info.Name) = info.GetValue(tValue, Nothing)
                Else
                    row(info.Name) = info.GetValue(tValue, Nothing)
                End If
            Next
            dt.Rows.Add(row)
        Next
        Return dt
    End Function
    Public Shared Function GetNullableType(ByVal t As Type) As Type

        Dim returnType As Type = t

        If (t.IsGenericType Or t.GetGenericTypeDefinition() Is GetType(Nullable(Of ))) Then
            returnType = Nullable.GetUnderlyingType(t)
        End If

        Return returnType

    End Function
    Public Shared Function IsNullableType(ByVal type As Type) As Boolean

        Return (type Is GetType(String) Or
                type.IsArray Or
                (type.IsGenericType And type.GetGenericTypeDefinition() Is GetType(Nullable(Of ))))
    End Function
    Public Shared Function GetExcelColumnName(ByVal columnIndex As Integer) As String
        If (columnIndex < 26) Then
            Return Chr(Asc("A") + columnIndex)
        End If

        Dim firstChar As Char,
            secondChar As Char

        firstChar = Chr(Asc("A") + (columnIndex \ 26) - 1)
        secondChar = Chr(Asc("A") + (columnIndex Mod 26))

        Return firstChar + secondChar
    End Function
    Public Shared Sub AppendTextCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = CellValues.String

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub
    Public Shared Sub AppendNumericCell(ByVal cellReference As String, ByVal cellStringValue As String, ByVal excelRow As Row)
        '/  Add a new Excel Cell to our Row 
        Dim cell As New Cell
        cell.CellReference = cellReference
        cell.DataType = CellValues.Number

        Dim cellValue As New CellValue
        cellValue.Text = cellStringValue

        cell.Append(cellValue)

        excelRow.Append(cell)
    End Sub
    Private Shared Function CustomColumnWidth(ByVal columnIndex As Integer, ByVal columnWidth As Double) As Column
        ' This creates a Column variable for a zero-based column-index (eg 0 = Excel Column A), with a particular column width.
        Dim column As New Column
        column.Min = columnIndex + 1
        column.Max = columnIndex + 1
        column.Width = columnWidth
        column.CustomWidth = True
        Return column
    End Function


    Protected Sub btnContractPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContractPrint.Click
        Try
            Dim FolderPath As String = "..\ExcelTemplates\"
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
            UpdateValue(wsName, "C5", hdncontractid.Value, 3, True, False)

            wsName = "Main Details"
            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)
            UpdateValue(wsName, "A6", "From Date", 2, True, False)

            Dim dt1 As New DataTable
            strSqlQry = "select isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),applicableto,0 countrygroups, isnull(activestate,'') activestate  from  view_contracts_search where contractid ='" & hdncontractid.Value & "' order by convert(varchar(10),fromdate , 111)"
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
            strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_contractcountry  v where v.ctrycode =p.ctrycode  and v.contractid ='" & hdncontractid.Value & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
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


            strSqlQry = "select isnull(seasonname,''),isnull(convert(varchar(10),fromdate , 105),'')fromdate,isnull(convert(varchar(10),todate , 105),'')todate,isnull(MinNight,'') from view_contractseasons where contractid='" & hdncontractid.Value & "' order by convert(varchar(10),fromdate , 111) "
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(rs)

            UpdateTableValue(wsName, rs, 0, 19, 3, True)

            SqlConn.Close()
            wsName = "Commission"

            UpdateValue(wsName, "B3", ViewState("hotelname"), 3, True, False)
            UpdateValue(wsName, "B4", hdncontractid.Value, 3, True, False)


            Dim dt3 As New DataTable
            strSqlQry = "select tranid,isnull(seasons,'')seasons from view_contracts_commission_header where contractid = '" & hdncontractid.Value & "'"
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
                        strSqlQry += " where h.contractid ='" & hdncontractid.Value & "' and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(cs.Item1,'')<>'' order by convert(varchar(10),s.fromdate , 111) "

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
            UpdateValue(wsName, "B4", hdncontractid.Value, 3, True, False)



            Dim dtmx As New DataTable


            strSqlQry = "select distinct tranid from view_partymaxaccomodation where partycode= '" & hdnpartycode.Value & "'"
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
                    '  strSqlQry = "select rmtypcode from view_partymaxaccomodation where partycode= '" & hdnpartycode.Value & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
                    strSqlQry = "select v.rmtypcode from view_partymaxaccomodation v ,partyrmtyp  p  where v.partycode=p.partycode and v.rmtypcode=p.rmtypcode and    v.partycode= '" & hdnpartycode.Value & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  p.rankord"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(dtmx1)

                    Dim rsmx As New DataTable
                    '  strSqlQry = "select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status,case when isnull(prm.unityesno,0)=0 then 0 else m.pricepax  end  pricepaxunit,case when isnull(prm.unityesno,0)=0 then 2 else m.pricepax  end pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb,isnull(m.noofextraperson,''), m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & hdnpartycode.Value & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

                    strSqlQry = "  select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status," _
                                            & " case when isnull(prm.unityesno,0)=0 then 0 else m.pricepax  end  pricepaxunit, " _
                                            & " case when isnull(prm.unityesno,0)=0 then 2 else m.pricepax  end pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb, " _
                                            & " isnull(m.noofextraperson,'') noofextraperson, m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm, " _
                                            & " view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode  " _
                                            & " and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid  " _
                                            & "  And h.partycode='" & hdnpartycode.Value & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "' order by  prm.rankord"

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsmx)
                    UpdateTableValue(wsName, rsmx, 0, iLinmx, 3, True)
                    SqlConn.Close()
                    em = rsmx.Rows.Count


                    Dim rsmx2 As New DataTable
                    strSqlQry = "select  isnull(start0based,'') from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & hdnpartycode.Value & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
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
                            strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & hdnpartycode.Value & "' for xml path('')),1,1,''),'') "
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)

            Dim dtrr2 As New DataTable
            strSqlQry = "select plistcode from view_cplisthnew  where contractid= '" & hdncontractid.Value & "'"
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
                    strSqlQry = "select  isnull(c.subseascode,''),isnull(convert(varchar(10),d.fromdate , 103),'')fromdate,isnull(convert(varchar(10),d.todate , 103),'')todate from view_cplisthnew c,view_contractseasons d  where c.subseascode=d.SeasonName and c.plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and d.contractid='" & hdncontractid.Value & "' order by convert(varchar(10),d.fromdate , 111)"
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
            UpdateValue(wsName, "B4", hdncontractid.Value, 3, True, False)

            Dim dte As New DataTable
            strSqlQry = "select d.exhibitionid,d.elineno from view_contracts_exhibition_detail d, view_contracts_exhibition_header h   where h.exhibitionid=d.exhibitionid  and  h.contractid= '" & hdncontractid.Value & "'"
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
                            & " d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "' and d.elineno=" & dte.Rows(i)("elineno").ToString & "  and p.partycode ='" & hdnpartycode.Value & "' and  " _
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
            UpdateValue(wsName, "B4", hdncontractid.Value, 3, True, False)


            Dim dtmr As New DataTable
            strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header  where contractid= '" & hdncontractid.Value & "'"
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
                    strSqlQry = "select  isnull(q.Item1,''), isnull(convert(varchar(10),s.fromdate , 105),''),isnull(convert(varchar(10),s.todate , 105),'') from view_contracts_mealsupp_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' order by convert(varchar(10),s.fromdate , 111) "
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
            UpdateValue(wsName, "B4", hdncontractid.Value, 3, True, False)



            Dim dtcpi As New DataTable
            strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where contractid= '" & hdncontractid.Value & "'"
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
                        & " cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' order by convert(varchar(10),fromdate , 111) "
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)


            Dim dtcn As New DataTable
            strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where contractid= '" & hdncontractid.Value & "'"
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
                    strSqlQry = "select  d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & hdncontractid.Value & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' order by convert(varchar(10),s.fromdate , 111)"
                    Dim rscp As New DataTable
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rscp)
                    SqlConn.Close()
                    iLinecr2 = iLinecr2 + 1
                    co2 = rscp.Rows.Count
                    UpdateTableValue(wsName, rscp, 0, iLinecr2, 3, True)

                    ''' shahul 21/03/17
                    strSqlQry = "select m.exhibitionname,isnull(convert(varchar(10),d.fromdate,105),''),isnull(convert(varchar(10),d.todate,105),'') from view_contracts_cancelpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.exhibitions,',') e inner join view_contracts_exhibition_detail d on e.Item1=d.exhibitioncode inner join view_contracts_exhibition_header eh on d.exhibitionid=eh.exhibitionid and eh.contractid=h.contractid inner join exhibition_master m on d.exhibitioncode=m.exhibitioncode where h.contractid='" & hdncontractid.Value & "' and h.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
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

                            strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & hdnpartycode.Value & "'  for xml path('')),1,1,''),''), mealplans,d.noshowearly,chargebasis,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)





            Dim dtc As New DataTable
            strSqlQry = "select checkinoutpolicyid from view_contracts_checkinout_header  where contractid= '" & hdncontractid.Value & "'"
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


                    strSqlQry = "select  d.checkinoutpolicyid,d.applicableto,q.Item1 seasons, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105)  from view_contracts_checkinout_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q  join  view_contractseasons s on s.SeasonName=q.Item1 and  s.contractid= '" & hdncontractid.Value & "' and  d.checkinoutpolicyid  ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' order by convert(varchar(10),s.fromdate , 111) "
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




                    strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & hdnpartycode.Value & "' for xml path('')),1,1,''),'') "
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)




            Dim iLineg As Integer = 7


            Dim dtg As New DataTable
            strSqlQry = "select genpolicyid from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
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
                    strSqlQry = "select distinct genpolicyid,applicableto,isnull(fromdate,''),isnull(todate,'') from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(rsg)
                    SqlConn.Close()
                    g = rsg.Rows.Count

                    UpdateTableValue(wsName, rsg, 0, iLineg, 3, True)

                    Dim rsp As New DataTable
                    Dim y As Integer
                    strSqlQry = "select isnull(policytext,'') from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "' and genpolicyid= '" & dtg.Rows(i)("genpolicyid").ToString & "' "
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)


            Dim dtm As New DataTable
            strSqlQry = "select minnightsid from view_contracts_minnights_header where contractid= '" & hdncontractid.Value & "'"
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
                            strSqlQry = "select distinct D.minnightsid ,H.applicableto from view_contracts_minnights_detail d,view_contracts_minnights_header h where d.minnightsid ='" & dtm.Rows(i)("minnightsid").ToString & "' and d.clineno='" & dtm2.Rows(i2)("clineno").ToString & "' and contractid= '" & hdncontractid.Value & "'"
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)



            Dim iLines As Integer = 7


            Dim dts As New DataTable
            strSqlQry = ""
            strSqlQry = "select splistcode from  view_contracts_specialevents_header where contractid= '" & hdncontractid.Value & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dts)

            If dts.Rows.Count > 0 Then
                For i As Integer = 0 To dts.Rows.Count - 1

                    Dim rsp1 As New DataTable
                    strSqlQry = ""
                    strSqlQry = "select isnull(remarks,'') remarks,case when isnull(compulsory,0)=0 then 'All Compulsory' else case when  isnull(compulsory,0)=1 then 'Any One Compulsory' else 'Optional in Special Events' end end " _
                        & " compulsory from view_contracts_specialevents_header  where contractid= '" & hdncontractid.Value & "' and splistcode= '" & dts.Rows(i)("splistcode").ToString & "' "
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
                                & " and p.partycode= '" & hdnpartycode.Value & "'  and d.splistcode='" & dts.Rows(i)("splistcode").ToString & "' and  splineno=" & dters.Rows(ers)("splineno").ToString & " order by splineno"
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
                                    & " p.partycode ='" & hdnpartycode.Value & "'  for xml path('')),1,1,''),'') "
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
                                    & " p.partycode ='" & hdnpartycode.Value & "'   for xml path('')),1,1,''),'') "
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
            UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)





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
            'UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)

            'Dim sellingexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_final_contracted_rates where contractid='" & hdncontractid.Value & "'")
            'If sellingexists <> "" Then


            '    ' strSqlQry = "select distinct plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"
            '    ''' shahul 21/03/17
            '    strSqlQry = "select  plistcode,rmtypname as roomtype , rmcatcode as roomcategory,view_final_contracted_rates.mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb, " _
            '        & " noofadulteb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,  " _
            '        & " totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates,mealmast m where   " _
            '        & " view_final_contracted_rates.mealcode=m.mealcode and contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate,m.rankorder"


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
            'UpdateValue(wsName, "B5", hdncontractid.Value, 3, True, False)
            'Dim iLinesfo As Integer = 8
            ' ''' shahul 21/03/17
            'strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,othmealcode as mealplan,mealcode basemeal ,accommodationid,agecombination,adults,child,totalpaxwithinpricepax, " _
            '    & "maxeb,noofadulteb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,mealsupplementid,adultmealprice,adultmealrmcatdetails, " _
            '    & " totalchildmealcharge,childmealdetails,totalprice,nights,minstay,minstayoption,commissionformulaid, " _
            '    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder,othmealcode from view_final_contracted_rates_othmeal where contractid = '" & hdncontractid.Value & "' order by rmtyporder, " _
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
            strpop = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)



            objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Exported succesfully : ", Session("GlobalUserName"))

        Catch ex As Exception
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub




    Public Sub KillUnusedExcelProcess()
        'Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")
        'For Each oXLP As Process In oXlProcess
        '    If Len(oXLP.MainWindowTitle) = 0 Then
        '        oXLP.Kill()
        '    End If
        'Next
    End Sub


    Private Sub FillCountry()
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If



        strSqlQry = "" ' "select  '' constructionid , '' fromdate , '' todate , '' reason ,'' Miscellaneous ,'' adddate, '' adduser, ''  moddate ,'' moduser"
        Try
            strSqlQry = "SELECT ctrycode 'Country Code',ctryname 'Country Name' from ctrymast"


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
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub



    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click



        'Dim xlApp
        'Dim xlBook
        'Dim xlSheet

        'If Session("ErrorList") Is Nothing = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Start Checking and Generate Error Report !.');", True)
        '    Exit Sub
        'End If



        'xlApp = Server.CreateObject("Excel.Application")
        'xlApp.visible = False
        'Dim FolderPath As String = "..\ExcelTemplates\"
        'Dim FileName As String = "ErrorList.xlsx"
        'Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        'Dim strConn As String = Session("dbconnectionName")




        'Try
        '    xlBook = xlApp.Workbooks.Open(FilePath, True, False)

        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try


        'Dim iLine2 As Integer = 11



        'xlSheet = xlBook.Worksheets(1) ' Sheet 1
        'xlSheet.Range("B4").Value = ViewState("hotelname")
        'If Session("Calledfrom") = "Offers" Then
        '    xlSheet.Range("A5".ToString) = "Promotion Id"
        '    xlSheet.Range("B5").Value = hdnpromotionid.Value
        'Else
        '    xlSheet.Range("A5".ToString) = "Contract Id"
        '    xlSheet.Range("B5").Value = hdncontractid.Value
        'End If


        'Dim rsrates As New ADODB.Recordset
        'rsrates = objutil.convertToADODB(Session("ErrorList"))

        'If rsrates.RecordCount > 0 Then
        '    xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rsrates)
        'End If

        'Dim contno As String()
        'If Session("Calledfrom") = "Offers" Then
        '    contno = hdnpromotionid.Value.Split("/")
        'Else
        '    contno = hdncontractid.Value.Split("/")
        'End If


        'FolderPath = "~\ExcelTemp\"
        'Dim FileNameNew As String = "ErrorList" & Now.Year & Now.Month & Now.Day & contno(0) + contno(1) & ".xlsx"
        'Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

        'Try

        '    Dim outputPath1 As String = Server.MapPath(FolderPath)
        '    If Directory.Exists(outputPath1) Then

        '        Dim files() As String
        '        files = Directory.GetFileSystemEntries(outputPath1)

        '        For Each element As String In files
        '            If (Not Directory.Exists(element)) Then
        '                If Path.GetFileName(outputPath) = Path.GetFileName(element) Then
        '                    File.Delete(Path.Combine(outputPath1, Path.GetFileName(element)))
        '                    Exit For
        '                End If
        '                ' File.Delete(Path.Combine(outputPath1, Path.GetFileName(element)))

        '            End If
        '        Next
        '    End If


        '    ' Set active as first sheet
        '    xlSheet = xlBook.Worksheets(1)
        '    xlSheet.Activate()
        '    xlSheet.Range("A1").Activate()
        '    ' Save and Close the Workbook
        '    xlBook.SaveAs(outputPath)
        '    xlBook.Close(True, Type.Missing, Type.Missing)

        '    ' Release the Application object
        '    xlApp.Quit()
        '    xlBook = Nothing
        '    xlApp = Nothing

        '    ' ExcelOpen(outputPath)
        '    ' Collect the unreferenced objects
        '    GC.Collect()
        '    GC.WaitForPendingFinalizers()

        '    'DownloadFiles.aspx

        '    Dim strpop As String
        '    strpop = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        'Catch ex As Exception

        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        'End Try



    End Sub
    ' Given a Worksheet and an address (like "AZ254"), either return a cell reference, or 
    ' create the cell reference and return it.
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
    ' Given the main workbook part, and a text value, insert the text into the shared
    ' string table. Create the table if necessary. If the value already exists, return
    ' its index. If it doesn't exist, insert it and return its new index.
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

    ' Used to force a recalc of cells containing formulas. The
    ' CellValue has a cached value of the evaluated formula. This
    ' will prevent Excel from recalculating the cell even if 
    ' calculation is set to automatic.
    Private Function RemoveCellValue(ByVal sheetName As String, ByVal addressName As String) As Boolean
        Dim returnValue As Boolean = False

        Dim sheet As Sheet = wbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = sheetName).FirstOrDefault()
        If sheet IsNot Nothing Then
            Dim ws As Worksheet = DirectCast(wbPart.GetPartById(sheet.Id), WorksheetPart).Worksheet
            Dim cell As Cell = InsertCellInWorksheet(ws, addressName)

            ' If there is a cell value, remove it to force a recalc
            ' on this cell.
            If cell.CellValue IsNot Nothing Then
                cell.CellValue.Remove()
            End If

            ' Save the worksheet.
            ws.Save()
            returnValue = True
        End If

        Return returnValue
    End Function
    Public Function GenerateCellFormats() As CellFormats
        Dim cellFormats1 As New CellFormats() With { _
            .Count = CType(2, UInt32Value) _
           }
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
        cellFormats1.Append(cellFormat1)

        Return cellFormats1
    End Function

    Public Function InsertCellFormat(ByVal workbookpart As WorkbookPart, ByVal cellformat As CellFormat) As UInteger
        Dim cellFormats As CellFormats = workbookpart.WorkbookStylesPart.Stylesheet.Elements(Of CellFormats)().First()
        cellFormats.Append(cellformat)
        Dim indexcount As UInteger
        indexcount = cellFormats.Count
        If indexcount > 0 Then indexcount = indexcount - 1
        Return indexcount
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
    Sub saverates()

        Try

            Dim dt As New DataTable
            Dim strSqlQry As String = ""

            If Session("Calledfrom") = "Offers" Then

                strSqlQry = " execute sp_finalcalculated_rates_offers '" & CType(hdnpromotionid.Value, String) & "'"

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.SelectCommand.CommandTimeout = 0
                myDataAdapter.Fill(dt)
                SqlConn.Close()




                'sqlTrans = mySqlConn.BeginTransaction
                'mySqlCmd = New SqlCommand("sp_finalcalculated_rates_offers", mySqlConn, sqlTrans)
                'mySqlCmd.CommandType = CommandType.StoredProcedure
                'mySqlCmd.CommandTimeout = 0
                'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                'mySqlCmd.ExecuteNonQuery()
                'sqlTrans.Commit()    'SQl Tarn Commit



                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 

                'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                'clsDBConnect.dbConnectionClose(mySqlConn)



                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Offer !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else




                strSqlQry = " execute sp_finalcalculated_rates '" & CType(hdncontractid.Value, String) & "'"

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.SelectCommand.CommandTimeout = 0
                myDataAdapter.Fill(dt)
                SqlConn.Close()


                'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                'sqlTrans = mySqlConn.BeginTransaction


                'mySqlCmd = New SqlCommand("sp_finalcalculated_rates", mySqlConn, sqlTrans)
                'mySqlCmd.CommandType = CommandType.StoredProcedure
                'mySqlCmd.CommandTimeout = 0
                'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                'mySqlCmd.ExecuteNonQuery()


                'sqlTrans.Commit()    'SQl Tarn Commit
                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                'clsDBConnect.dbConnectionClose(mySqlConn)


                'btnrefresh.Enabled = True
                'btnofferrates.Enabled = True
                'btnothmealofferrates.Enabled = True
                'btnContractPrint.Enabled = True

                '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Contract !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If


            '' ADDED shahul dependent offer rate calcualte first 23/06/18
            Dim dependentid As DataSet
            If Session("Calledfrom") = "Offers" Then

                strSqlQry = " execute sp_get_dependentoffers '" & DBNull.Value & "','" & CType(hdnpromotionid.Value, String) & "'"

                dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)

                If dependentid.Tables(0).Rows.Count > 0 Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction

                    mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdnpromotionid.Value, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.ExecuteNonQuery()



                    For i As Integer = 0 To dependentid.Tables(0).Rows.Count - 1


                        mySqlCmd = New SqlCommand("sp_save_recalculateoffers", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@contpromid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = dependentid.Tables(0).Rows(i)("promotionid")
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()

                    Next

                    mySqlCmd = New SqlCommand("sp_save_recalculateoffers", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@contpromid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Promotion, please go to Dependent Offers recalculation option !.');", True)
                    Exit Sub
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Offer !.');", True)
                End If
            Else

                strSqlQry = " execute sp_get_dependentoffers '" & CType(hdncontractid.Value, String) & "','" & DBNull.Value & "'"

                dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)

                If dependentid.Tables(0).Rows.Count > 0 Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction

                    mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdncontractid.Value, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For i As Integer = 0 To dependentid.Tables(0).Rows.Count - 1


                        mySqlCmd = New SqlCommand("sp_save_recalculateoffers", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@contpromid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(dependentid.Tables(0).Rows(i)("promotionid"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()

                    Next

                    mySqlCmd = New SqlCommand("sp_save_recalculateoffers", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@contpromid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Contract, please go to Dependent Offers recalculation option !.');", True)
                    Exit Sub
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Contracts !.');", True)
                End If
            End If






        Catch ex As Exception
            'If mySqlConn.State = ConnectionState.Open Then
            '    sqlTrans.Rollback()
            'End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description- " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Contractvalidatenew.aspx :: saverates :: ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click
        Try
            saverates()
        Catch ex As Exception
            ModalPopupError.Hide()
            ModalPopupDays.Hide()
        End Try
        ' ModalPopupError.Hide()
    End Sub

    Protected Sub btnofferrates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnofferrates.Click

        Try

            ''''' Final Rates

            Dim xlApp
            Dim xlBook
            Dim xlSheet
            Dim wsName As String = ""
            Dim FolderPath As String = ""
            Dim FileName As String = ""
            Dim FilePath As String = ""
            Dim FileNameNew As String = ""
            Dim outputPath As String = ""

            Dim dt1 As New DataTable
            Dim ds As New DataSet

            If Session("Calledfrom") = "Contracts" Then
                wsName = "Final Contracted Rates"
                FolderPath = "..\ExcelTemplates\"
                FileName = "ContractRatePrint.xlsx"
                FilePath = Server.MapPath(FolderPath + FileName)

                FolderPath = "~\ExcelTemp\"

                FileNameNew = "ContractRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                outputPath = Server.MapPath(FolderPath + FileNameNew)

                File.Copy(FilePath, outputPath, True)

                wsName = "Final Contracted Rates"

                strSqlQry = "select  plistcode,rmtypname as roomtype , rmcatcode as roomcategory,view_final_contracted_rates.mealcode as mealplan," _
                           & " accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,  noofadulteb,convert(varchar(10),convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr, npr,  " _
                           & " exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,   totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,  " _
                           & " exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder, vatperc VatPercentage, costtaxablevalue, costnontaxablevalue, costvatvalue  from view_final_contracted_rates,mealmast m where view_final_contracted_rates.mealcode=m.mealcode and contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate,m.rankorder"

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(dt1)
                SqlConn.Close()

                dt1.TableName = "Final Contracted Rates"
                ds.Tables.Add(dt1)



                CreateExcelDocument(ds, outputPath)



            Else
                wsName = "Final Offer Rates"
                FolderPath = "..\ExcelTemplates\"
                FileName = "OfferRatePrint.xlsx"
                FilePath = Server.MapPath(FolderPath + FileName)

                FolderPath = "~\ExcelTemp\"

                FileNameNew = "OfferRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                outputPath = Server.MapPath(FolderPath + FileNameNew)

                File.Copy(FilePath, outputPath, True)


                wsName = "Final Offer Rates"

                ''Pricewithfreenight removed this column as per madam producton notes 29/06/17

                strSqlQry = "select Promotionid,Calculatedid,Autoid,Plistcode,rmtypname as Roomtype , rmcatcode as Roomcategory,view_final_offer_rates.mealcode as Mealplan,Accommodationid,Agecombination,Adults,Child,Totalpaxwithinpricepax,Maxeb, " _
          & " Noofadulteb,Noofchildeb,convert(varchar(10),convert(date,pfromdate),105) Fromdate,convert(varchar(10),convert(date,ptodate),105) Todate,GRR,NPR,Discounttype,Discount,Addldiscount,Exhibitionprice,Adultebprice,Adultebpricedisc," _
          & "Extrapaxprice,Extrapaxpricedisc,Totalsharingcharge,Totalsharingdiscount,Totalebcharge,Totalebdiscount,Totalprice,Nights,Minstay,Minstayoption,Stayfor,Freenights,Rmtypupgradefrom,Rmtypupgradefromname," _
          & "Commissionformulaid,Commissionformulastring,Exhibitionids,Pricepax,Childpolicyid,Rmtyporder,rmcatorder, vatperc VatPercentage, costtaxablevalue, costnontaxablevalue, costvatvalue from view_final_offer_rates,mealmast  where view_final_offer_rates.mealcode=mealmast.mealcode and  promotionid = '" & hdnpromotionid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate,mealmast.rankorder"


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(dt1)
                SqlConn.Close()

                dt1.TableName = "Final Offer Rates"
                ds.Tables.Add(dt1)



                CreateExcelDocument(ds, outputPath)

            End If




            'Dim FolderPath As String = "..\ExcelTemplates\"
            'Dim FileName As String = "OfferRatePrint.xlsx"
            'Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            'FolderPath = "~\ExcelTemp\"
            'Dim FileNameNew As String = "OfferRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            'Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            'File.Copy(FilePath, outputPath, True)




            'wsName = "Final Offer Rates"



            'strSqlQry = "select Promotionid,Calculatedid,Autoid,Plistcode,rmtypname as Roomtype , rmcatcode as Roomcategory,view_final_offer_rates.mealcode as Mealplan,Accommodationid,Agecombination,Adults,Child,Totalpaxwithinpricepax,Maxeb, " _
            '& " Noofadulteb,Noofchildeb,convert(varchar(10),convert(date,pfromdate),105) Fromdate,convert(varchar(10),convert(date,ptodate),105) Todate,GRR,NPR,Discounttype,Discount,Addldiscount,Exhibitionprice,Adultebprice,Adultebpricedisc," _
            '& "Extrapaxprice,Extrapaxpricedisc,Totalsharingcharge,Totalsharingdiscount,Totalebcharge,Totalebdiscount,Totalprice,Pricewithfreenight,Nights,Minstay,Minstayoption,Stayfor,Freenights,Rmtypupgradefrom,Rmtypupgradefromname," _
            '& "Commissionformulaid,Commissionformulastring,Exhibitionids,Pricepax,Childpolicyid,Rmtyporder,rmcatorder from view_final_offer_rates,mealmast  where view_final_offer_rates.mealcode=mealmast.mealcode and  promotionid = '" & hdnpromotionid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate,mealmast.rankorder"

            'Dim dt1 As New DataTable
            'Dim ds As New DataSet

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(dt1)
            'SqlConn.Close()

            'dt1.TableName = "Final Offer Rates"
            'ds.Tables.Add(dt1)






            'CreateExcelDocument(ds, outputPath)



            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)


        Catch ex As Exception
            objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub

    Protected Sub btnothmealofferrates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnothmealofferrates.Click
        Try

            ''''' Final Rates

            Dim xlApp
            Dim xlBook
            Dim xlSheet
            Dim wsName As String = ""

            Dim FolderPath As String = ""
            Dim FileName As String = ""
            Dim FilePath As String = ""
            Dim FileNameNew As String = ""
            Dim outputPath As String = ""

            Dim dt1 As New DataTable
            Dim ds As New DataSet



            If Session("Calledfrom") = "Contracts" Then
                wsName = "Contract Rates – Other Meal Plan"

                FolderPath = "..\ExcelTemplates\"
                FileName = "ContractRatePrint.xlsx"
                FilePath = Server.MapPath(FolderPath + FileName)


                FolderPath = "~\ExcelTemp\"

                FileNameNew = "ContractRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                outputPath = Server.MapPath(FolderPath + FileNameNew)

                File.Copy(FilePath, outputPath, True)

                wsName = "Contract Rates – Other Meal Plan"



                strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,othmealcode as mealplan,mealcode basemeal ,accommodationid, " _
                    & " agecombination,adults,child,totalpaxwithinpricepax, maxeb,noofadulteb,convert(varchar(10), " _
                    & " convert(date,pfromdate),105),convert(varchar(10),convert(date,ptodate),105),grr,npr,exhibitionprice, " _
                    & " adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,mealsupplementid,adultmealprice,adultmealrmcatdetails, totalchildmealcharge,childmealdetails,totalprice,nights,minstay,minstayoption,commissionformulaid,  " _
                    & "  commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder,othmealcode, vatperc VatPercentage, costtaxablevalue, costnontaxablevalue, costvatvalue   from view_final_contracted_rates_othmeal where contractid = '" & hdncontractid.Value & "'  order by rmtyporder,  rmcatorder,agecombination,pfromdate "


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(dt1)
                SqlConn.Close()

                dt1.TableName = "Contract Rates – Other Meal Plan"
                ds.Tables.Add(dt1)

                CreateExcelDocument(ds, outputPath)





            Else
                wsName = "Offer Rates – Other Meal Plan"

                FolderPath = "..\ExcelTemplates\"
                FileName = "OfferRatePrint.xlsx"
                FilePath = Server.MapPath(FolderPath + FileName)

                FolderPath = "~\ExcelTemp\"

                FileNameNew = "OfferRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                outputPath = Server.MapPath(FolderPath + FileNameNew)

                File.Copy(FilePath, outputPath, True)

                wsName = "Offer Rates – Other Meal Plan"

                ''Pricewithfreenight removed this column as per madam producton notes 29/06/17

                strSqlQry = "select Promotionid,Calculatedid,Autoid,Plistcode,rmtypname as Roomtype , rmcatcode as Roomcategory,view_final_offer_rates_othmeal.mealcode as Mealplan, Basemeal ,Accommodationid,Agecombination,Adults,Child,Totalpaxwithinpricepax, " _
                    & "Maxeb,Noofadulteb,Noofchildeb,convert(varchar(10),convert(date,pfromdate),105) Fromdate,convert(varchar(10),convert(date,ptodate),105) Todate,GRR,NPR,Discounttype,Discount,Addldiscount,Exhibitionprice,Adultebprice,Adultebpricedisc,Extrapaxprice,Extrapaxpricedisc,Totalsharingcharge,totalsharingdiscount,totalebcharge,totalebdiscount,mealsupplementid,adultmealprice,adultmealdisc,adultmealrmcatdetails, " _
                    & " totalchildmealcharge,totalchildmealdisc,childmealdetails,totalprice,nights,minstay,minstayoption,stayfor,freenights,rmtypupgradefrom,rmtypupgradefromname,commissionformulaid, " _
                    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder, vatperc VatPercentage, costtaxablevalue, costnontaxablevalue, costvatvalue from view_final_offer_rates_othmeal,mealmast where view_final_offer_rates_othmeal.mealcode=mealmast.mealcode and promotionid ='" & hdnpromotionid.Value & "' order by rmtyporder, " _
                    & " rmcatorder,agecombination,pfromdate,mealmast.rankorder"


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(dt1)
                SqlConn.Close()

                dt1.TableName = "Offer Rates – Other Meal Plan"
                ds.Tables.Add(dt1)


                CreateExcelDocument(ds, outputPath)


            End If

            'Dim wsName As String = "Offer Rates – Other Meal Plan"


            'Dim FolderPath As String = "..\ExcelTemplates\"
            'Dim FileName As String = "OfferRatePrint.xlsx"
            'Dim FilePath As String = Server.MapPath(FolderPath + FileName)


            'FolderPath = "~\ExcelTemp\"
            'Dim FileNameNew As String = "OfferRatePrint" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            'Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)

            'File.Copy(FilePath, outputPath, True)






            'wsName = "Offer Rates – Other Meal Plan"



            'strSqlQry = "select Promotionid,Calculatedid,Autoid,Plistcode,rmtypname as Roomtype , rmcatcode as Roomcategory,view_final_offer_rates_othmeal.mealcode as Mealplan, Basemeal ,Accommodationid,Agecombination,Adults,Child,Totalpaxwithinpricepax, " _
            '    & "Maxeb,Noofadulteb,Noofchildeb,convert(varchar(10),convert(date,pfromdate),105) Fromdate,convert(varchar(10),convert(date,ptodate),105) Todate,GRR,NPR,Discounttype,Discount,Addldiscount,Exhibitionprice,Adultebprice,Adultebpricedisc,Extrapaxprice,Extrapaxpricedisc,Totalsharingcharge,totalsharingdiscount,totalebcharge,totalebdiscount,mealsupplementid,adultmealprice,adultmealdisc,adultmealrmcatdetails, " _
            '    & " totalchildmealcharge,totalchildmealdisc,childmealdetails,totalprice,pricewithfreenight,nights,minstay,minstayoption,stayfor,freenights,rmtypupgradefrom,rmtypupgradefromname,commissionformulaid, " _
            '    & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_offer_rates_othmeal,mealmast where view_final_offer_rates_othmeal.mealcode=mealmast.mealcode and promotionid ='" & hdnpromotionid.Value & "' order by rmtyporder, " _
            '    & " rmcatorder,agecombination,pfromdate,mealmast.rankorder"

            'Dim dt1 As New DataTable
            'Dim ds As New DataSet

            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(dt1)
            'SqlConn.Close()

            'dt1.TableName = "Offer Rates – Other Meal Plan"
            'ds.Tables.Add(dt1)






            'CreateExcelDocument(ds, outputPath)





            GC.Collect()
            GC.WaitForPendingFinalizers()



            Dim strpop1 As String
            strpop1 = "window.open('DownloadFiles.aspx?filename=" & FileNameNew & " &FileLoc=ExcelTemp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)


        Catch ex As Exception
            objUtils.WritErrorLog("ContractValidatenew.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
End Class

