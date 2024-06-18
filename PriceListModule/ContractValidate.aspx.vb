Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
'Imports Microsoft.Office.Interop
Imports System.Data.OleDb
'Imports System.Diagnostics
Imports ADODB



Partial Class ContractValidate
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private conn1 As New ADODB.Connection
    Dim objutil As New clsUtils


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
        If IsPostBack = False Then
            txtconnection.Value = Session("dbconnectionName")
            If Session("Calledfrom") = "Offers" Then
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\OfferSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))

                hdnpartycode.Value = CType(Session("Offerparty"), String)
                hdncontractid.Value = CType(Session("contractid"), String)

                If Not Session("OfferRefCode") Is Nothing Then
                    hdnpromotionid.Value = Session("OfferRefCode")
                End If

                Session("partycode") = hdnpartycode.Value
                Dim hotelname1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" & hdnpartycode.Value & "'")
                ViewState("hotelname") = hotelname1
                lblHeading.Text = " Promotion Validate Approval  - " + hotelname1 + " - " + hdnpromotionid.Value

                btnContractPrint.Visible = False
                btnOfferPrint.Visible = True
                btnReport.Style.Add("display", "none")
            Else
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))

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
                btnContractPrint.Visible = True
                btnOfferPrint.Visible = False
                btnReport.Style.Add("display", "none")
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


    Protected Sub btnchecking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchecking.Click
        'ViewState("State") = "New"
        'PanelMain.Visible = True
        'PanelMain.Style("display") = "block"
        '' Panelsearch.Enabled = False



        If Session("Calledfrom") = "Offers" Then
            lblHeading.Text = " Promotion Validate For Approval  - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
            Page.Title = Page.Title + " " + "Promotion Validate For Approval  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value
        Else
            lblHeading.Text = " Validate For Approval  - " + ViewState("hotelname") + " - " + hdncontractid.Value
            Page.Title = Page.Title + " " + "Validate For Approval  -" + ViewState("hotelname") + " - " + hdncontractid.Value
        End If

        Session("ContractErrors") = Nothing

        FillGrid()

        'If gv_SearchResult.Rows.Count = 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Records  Now calculating the Rates !.');", True)

        '    ModalPopupError.Show()
        '    btnOk1_Click(sender, e)
        '    ModalPopupError.Hide()
        'End If

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


                btnchecking.Text = "Checking Completed"
                btnchecking.Enabled = False
                btnReport.Style.Add("display", "block")

                If ds.Tables(0).Rows.Count = 0 Then

                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Records. You can Approve the selected !.');", True)

                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Records  Now calculating the Rates !.');", True)
                    ' mp1.Show()
                    ModalPopupError.Show()
                    'btnOk1_Click(sender, e)
                    'ModalPopupError.Hide()
                    
                End If
            End If




        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Sub saverates()

        Try
            If Session("Calledfrom") = "Offers" Then



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction


                mySqlCmd = New SqlCommand("sp_finalcalculated_rates_offers", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                mySqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Offer !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction


                mySqlCmd = New SqlCommand("sp_finalcalculated_rates", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                mySqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can continue To Approve the Contract !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If



        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidateNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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

        'Try
        '    Dim xlApp
        '    Dim xlBook
        '    Dim xlSheet



        '    xlApp = Server.CreateObject("Excel.Application")
        '    xlApp.visible = False
        '    Dim FolderPath As String = "..\ExcelTemplates\"
        '    Dim FileName As String = "OfferPrint.xlsx"
        '    Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        '    Dim strConn As String = Session("dbconnectionName")
        '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & FilePath & "');", True)
        '    Try
        '        xlBook = xlApp.Workbooks.Open(FilePath, True, False)


        '    Catch ex As Exception
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    End Try

        '    Dim activestate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(activestate,'') activestate  from view_Offer_search where promotionid='" & hdnpromotionid.Value & "'")


        '    xlSheet = xlBook.Worksheets(1) ' Sheet 1
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdnpromotionid.Value
        '    xlSheet.Range("B5").Value = activestate



        '    xlSheet = xlBook.Worksheets(2)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")

        '    '    xlSheet.Range("B5").Value = hdncontractid.Value
        '    '    xlSheet.Range("A6").Value = "From Date"







        '    'xlSheet.Range("B" & iLineNo1m.ToString).CopyFromRecordset(rs)



        '    Dim dt31 As New DataTable

        '    strSqlQry = "select distinct q.item1 as promotiontypes FROM view_offers_header cross apply dbo.SplitString1colsWithOrderField(promotiontypes,',') q  where  promotionid= '" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt31)

        '    Dim iLineNo1m As Integer = 6
        '    strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto from view_offers_header h where  h.partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'"
        '    Dim rsi As New ADODB.Recordset
        '    rsi = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsi)
        '    'y = rsi.RecordCount

        '    xlSheet.Range("A" & iLineNo1m - 1.ToString) = "PromotionId"
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString) = "Promotion Name"
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString).WRAPTEXT = True
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Applicable To"


        '    xlSheet.Range("C" & iLineNo1m - 1.ToString).WRAPTEXT = True

        '    iLineNo1m = iLineNo1m + 3
        '    xlSheet.Range("A8") = "Countries"
        '    xlSheet.Range("A8").font.bold = True
        '    Dim dto As New DataTable
        '    strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_offers_countries v where v.ctrycode =p.ctrycode  and v.promotionid ='" & hdnpromotionid.Value & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dto)
        '    If dto.Rows.Count > 0 Then
        '        For i As Integer = 0 To dto.Rows.Count - 1

        '            'xlSheet.Range("A12").Value = "Countries"
        '            'xlSheet.Range("A14:I14").Merge()
        '            'xlSheet.Range("A14").WrapText = True
        '            'xlSheet.Range("A14").rowheight = 400
        '            'xlSheet.Range("A14").Value = dt.Rows(i)

        '            Dim remark As String = ""
        '            Dim exap As New Excel.Application
        '            Dim Range As Excel.Range
        '            Dim remarklength As Integer
        '            Dim r As Integer = 9
        '            remark = dto.Rows(i)("Country").ToString
        '            remark = remark.Replace(Chr(13), "")
        '            Dim remarks As String() = remark.Split(Chr(10))
        '            If (remarks.Length > 0) Then
        '                Dim re As String
        '                For Each re In remarks


        '                    With xlSheet.Range("A" & iLineNo1m & ":" & "D" & iLineNo1m)
        '                        .MergeCells = True
        '                        .WrapText = True
        '                        'If re.Length > 152 Then 'longtext and wrap- but not displayed completely..so added this--need to chk
        '                        '    .Cells.RowHeight = calCellHeightForPromotion(re.Length)
        '                        'End If
        '                    End With
        '                    xlSheet.Range("A9").Value = re
        '                    Range = xlSheet.Range("A" & iLineNo1m & ":" & "D" & iLineNo1m)
        '                    autoheight(Range, exap)
        '                    iLineNo1m = iLineNo1m + 1
        '                Next
        '            End If



        '            '''commented
        '            'xlSheet.Range("A9:D9").Merge()
        '            'xlSheet.Range("A9").Value = dto.Rows(i)("Country").ToString
        '            'xlSheet.Range("A9").WrapText = True
        '            'xlSheet.Range("A9").rowheight = 200
        '            ''''''''''''''''''


        '            '    Next

        '            'Else
        '            '    For i As                              Integer = 0 To dt.Rows.Count - 1

        '            '        xlSheet.Range("A14").Value = dt.Rows(i)("Country").ToString
        '            '    Next
        '            'End If

        '        Next

        '    End If
        '    Dim rt As Integer
        '    iLineNo1m = iLineNo1m + 2
        '    xlSheet.Range("A" & iLineNo1m) = "Promotion Types"
        '    xlSheet.Range("A" & iLineNo1m).font.bold = True
        '    strSqlQry = "select promotiontypes from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
        '    Dim rso As ADODB.Recordset
        '    rso = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("B" & iLineNo1m.ToString).CopyFromRecordset(rso)
        '    xlSheet.Range("B" & iLineNo1m).WrapText = True

        '    xlSheet.Range("C" & iLineNo1m) = "Days Of The Week"
        '    xlSheet.Range("C" & iLineNo1m).font.bold = True
        '    strSqlQry = "SELECT daysoftheweek from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
        '    Dim rsdd As ADODB.Recordset
        '    rsdd = GetResultAsRecordSet(strSqlQry)
        '    rt = rsdd.RecordCount

        '    If rt > 0 Then
        '        xlSheet.Range("D" & iLineNo1m.ToString).CopyFromRecordset(rsdd)
        '        xlSheet.Range("D" & iLineNo1m.ToString).WrapText = True
        '    End If
        '    iLineNo1m = iLineNo1m + 2

        '    Dim e2 As Integer
        '    Dim rsdd222 As ADODB.Recordset
        '    Dim rsdd2221 As ADODB.Recordset
        '    Dim xg As Integer
        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1

        '            If dt31.Rows(i)("promotiontypes") = "Early Bird Discount" Then
        '                e2 = 1

        '            ElseIf dt31.Rows(i)("promotiontypes") = "Free Nights" Then
        '                e2 = e2 + 1


        '            Else
        '                e2 = e2
        '            End If
        '        Next
        '    End If

        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If e2 = 1 And dt31.Rows(i)("promotiontypes") = "Early Bird Discount" Then
        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "From Date"
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString) = "To Date"
        '                xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Booking Code"
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Discount Type"
        '                xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Discount % or Value"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString) = "Additional Discount % Or Value"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString).wraptext = True
        '                xlSheet.Range("G" & iLineNo1m - 1.ToString) = "Booking Validity Options"
        '                xlSheet.Range("H" & iLineNo1m - 1.ToString) = "Booking Validity From/Book By"
        '                xlSheet.Range("I" & iLineNo1m - 1.ToString) = "Booking Validity To"
        '                xlSheet.Range("J" & iLineNo1m - 1.ToString) = "Booking Validity Days/Month"

        '                xlSheet.Range("K" & iLineNo1m - 1.ToString) = "Min.Nights"

        '                xlSheet.Range("M" & iLineNo1m - 1.ToString) = "Max.Nights "


        '                strSqlQry = "select fromdate,todate,isnull(bookingcode,''),discounttype,discountamount,additionalamount,bookingvalidityoptions,isnull(bookingvalidityfromdate,''),isnull(bookingvaliditytodate,''),bookingvaliditydaysmonths,minnights,isnull(maxnights,'') from view_offers_DETAIL  where promotionid='" & hdnpromotionid.Value & "'"
        '                rsdd222 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdd222)
        '                xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xg = rsdd222.RecordCount
        '            ElseIf e2 = 2 Then
        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "From Date"
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString) = "To Date"
        '                xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Booking Code"
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Discount Type"
        '                xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Discount % or Value"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString) = "Additional Discount % Or Value"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString).wraptext = True
        '                xlSheet.Range("G" & iLineNo1m - 1.ToString) = "Booking Validity Options"
        '                xlSheet.Range("H" & iLineNo1m - 1.ToString) = "Booking Validity From/Book By"
        '                xlSheet.Range("I" & iLineNo1m - 1.ToString) = "Booking Validity To"
        '                xlSheet.Range("J" & iLineNo1m - 1.ToString) = "Booking Validity Days/Month"

        '                xlSheet.Range("K" & iLineNo1m - 1.ToString) = "Min.Nights"
        '                xlSheet.Range("M" & iLineNo1m - 1.ToString) = "Apply To"
        '                xlSheet.Range("L" & iLineNo1m - 1.ToString) = "Max.Nights "
        '                xlSheet.Range("N" & iLineNo1m - 1.ToString) = "Allow Multi Stay"
        '                xlSheet.Range("O" & iLineNo1m - 1.ToString) = "Stay For"
        '                xlSheet.Range("P" & iLineNo1m - 1.ToString) = "Pay For"
        '                xlSheet.Range("Q" & iLineNo1m - 1.ToString) = "Max FreeNights"
        '                xlSheet.Range("R" & iLineNo1m - 1.ToString) = "Max Multiples"


        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "From Date"
        '                strSqlQry = "select fromdate,todate,isnull(bookingcode,''),discounttype,discountamount,additionalamount,bookingvalidityoptions,isnull(bookingvalidityfromdate,''),isnull(bookingvaliditytodate,''),bookingvaliditydaysmonths,minnights,isnull(maxnights,''),applyto,allowmultistay,stayfor,payfor,isnull(maxfeenights,''),isnull(maxmultiples,'') from view_offers_DETAIL  where promotionid='" & hdnpromotionid.Value & "'"
        '                rsdd222 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdd222)

        '                xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xg = rsdd222.RecordCount


        '            ElseIf e2 = 1 And dt31.Rows(i)("promotiontypes") = "Free nights" Then

        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "From Date"
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString) = "To Date"
        '                xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Booking Code"

        '                xlSheet.Range("F" & iLineNo1m - 1.ToString).wraptext = True
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Booking Validity Options"
        '                xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Booking Validity From/Book By"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString) = "Booking Validity To"
        '                xlSheet.Range("G" & iLineNo1m - 1.ToString) = "Booking Validity Days/Month"
        '                xlSheet.Range("H" & iLineNo1m - 1.ToString) = "Min.Nights"
        '                xlSheet.Range("J" & iLineNo1m - 1.ToString) = "Apply To"
        '                xlSheet.Range("I" & iLineNo1m - 1.ToString) = "Max.Nights "
        '                xlSheet.Range("K" & iLineNo1m - 1.ToString) = "Allow Multi Stay"
        '                xlSheet.Range("L" & iLineNo1m - 1.ToString) = "Stay For"
        '                xlSheet.Range("M" & iLineNo1m - 1.ToString) = "Pay For"
        '                xlSheet.Range("N" & iLineNo1m - 1.ToString) = "Max FreeNights"
        '                xlSheet.Range("O" & iLineNo1m - 1.ToString) = "Max Multiples"
        '                strSqlQry = "select fromdate,todate,isnull(bookingcode,''),discounttype,discountamount,additionalamount,bookingvalidityoptions,isnull(bookingvalidityfromdate,''),isnull(bookingvaliditytodate,''),bookingvaliditydaysmonths,minnights,isnull(maxnights,''),allowmultistay,stayfor,payfor,isnull(maxfeenights,''),isnull(maxmultiples,'') from view_offers_detail  where promotionid='" & hdnpromotionid.Value & "'"
        '                rsdd222 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdd222)
        '                xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xg = rsdd222.RecordCount

        '            Else
        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "From Date"
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString) = "To Date"
        '                xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Booking Code"

        '                xlSheet.Range("F" & iLineNo1m - 1.ToString).wraptext = True
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Booking Validity Options"
        '                xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Booking Validity From/Book By"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString) = "Booking Validity To"
        '                xlSheet.Range("G" & iLineNo1m - 1.ToString) = "Booking Validity Days/Month"
        '                xlSheet.Range("H" & iLineNo1m - 1.ToString) = "Min.Nights"
        '                xlSheet.Range("I" & iLineNo1m - 1.ToString) = "Max.Nights"
        '                strSqlQry = "select fromdate,todate,isnull(bookingcode,''),bookingvalidityoptions,isnull(bookingvalidityfromdate,''),isnull(bookingvaliditytodate,''),bookingvaliditydaysmonths,minnights,maxnights,isnull(maxnights,'') from view_offers_detail  where promotionid='" & hdnpromotionid.Value & "'"
        '                rsdd222 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdd222)
        '                xlSheet.Range("A" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xlSheet.Range("B" & iLineNo1m.ToString).NumberFormat = "dd/mm/yyyy;@"
        '                xg = rsdd222.RecordCount
        '            End If
        '        Next
        '    End If
        '    Dim xr As Integer
        '    Dim xu As Integer
        '    Dim xm1 As Integer
        '    Dim xm As Integer
        '    Dim xc As Integer
        '    Dim xms As Integer
        '    Dim x2m As Integer
        '    Dim xa1 As Integer

        '    iLineNo1m = iLineNo1m + xg + 3
        '    strSqlQry = "select  p.rmtypname from view_offers_rmtype d  cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.promotionid='" & hdnpromotionid.Value & "' and p.partycode ='" & hdnpartycode.Value & "'"
        '    rsdd2221 = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdd2221)
        '    xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True

        '    xr = rsdd2221.RecordCount
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Room Type"
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True


        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If dt31.Rows(i)("promotiontypes") = "Room Upgrade" Then

        '                Dim rsdd22212 As ADODB.Recordset
        '                strSqlQry = "select  isnull(p.rmtypname,'') rmtypname from view_offers_rmtype d  cross apply dbo.SplitString1colsWithOrderField(d.rmtypeupgrade,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.promotionid='" & hdnpromotionid.Value & "' and p.partycode ='" & hdnpartycode.Value & "'"

        '                rsdd22212 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("B" & iLineNo1m.ToString).CopyFromRecordset(rsdd22212)
        '                xlSheet.Range("B" & iLineNo1m.ToString).wraptext = True
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString) = "Room Upgrade"
        '                xlSheet.Range("B" & iLineNo1m - 1.ToString).font.bold = True
        '                xu = rsdd22212.RecordCount
        '            End If
        '        Next
        '    End If
        '    Dim rsdmm As ADODB.Recordset
        '    strSqlQry = "select  mealcode from view_offers_meal where  promotionid='" & hdnpromotionid.Value & "'"
        '    rsdmm = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("C" & iLineNo1m.ToString).CopyFromRecordset(rsdmm)

        '    xm1 = rsdmm.RecordCount
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Meal"
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True
        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If dt31.Rows(i)("promotiontypes") = "Meal Upgrade" Then

        '                Dim rsdmm1 As ADODB.Recordset
        '                strSqlQry = " select isnull(mealupgrade,'') mealupgrade from view_offers_meal where promotionid='" & hdnpromotionid.Value & "'"
        '                rsdmm1 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("D" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1)
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Meal Upgrade"
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString).font.bold = True
        '                xm = rsdmm1.RecordCount
        '            End If
        '        Next
        '    End If
        '    Dim rsdmma As ADODB.Recordset
        '    strSqlQry = "select rmcatcode from view_offers_accomodation where  promotionid='" & hdnpromotionid.Value & "'"

        '    rsdmma = GetResultAsRecordSet(strSqlQry)

        '    xlSheet.Range("E" & iLineNo1m.ToString).CopyFromRecordset(rsdmma)
        '    xlSheet.Range("E" & iLineNo1m.ToString).wraptext = True
        '    xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Accomodation"
        '    xlSheet.Range("E" & iLineNo1m - 1.ToString).font.bold = True
        '    xc = rsdmma.RecordCount
        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If dt31.Rows(i)("promotiontypes") = "Accomodation Upgrade" Then

        '                Dim rsdmm1a As ADODB.Recordset
        '                strSqlQry = "select isnull(rmcatupgrade,'') rmcatupgrade from view_offers_accomodation where promotionid='" & hdnpromotionid.Value & "'"

        '                rsdmm1a = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("F" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1a)
        '                xlSheet.Range("F" & iLineNo1m.ToString).wraptext = True
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString) = "Accomodation Upgrade"
        '                xlSheet.Range("F" & iLineNo1m - 1.ToString).font.bold = True
        '                x2m = rsdmm1a.RecordCount
        '            End If
        '        Next
        '    End If
        '    Dim rsdmmam As ADODB.Recordset
        '    strSqlQry = "Select rmcatcode FROM view_offers_supplement where  promotionid='" & hdnpromotionid.Value & "'"
        '    rsdmmam = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("G" & iLineNo1m.ToString).CopyFromRecordset(rsdmmam)
        '    xlSheet.Range("G" & iLineNo1m.ToString).wraptext = True
        '    xlSheet.Range("G" & iLineNo1m - 1.ToString) = "Meal Supplement"
        '    xlSheet.Range("G" & iLineNo1m - 1.ToString).font.bold = True
        '    xms = rsdmmam.RecordCount
        '    'Select max(recordcount) from xms,xr,xu,xc
        '    'case when ISNULL(nonrefundable,0)=0 then 'No' else 'Yes' end  status
        '    Dim dt2o1 As New DataTable
        '    strSqlQry = "select isnull(max(rmcount),0) as rmcount  from view_maxvariable  where partycode= '" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt2o1)
        '    If dt2o1.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt2o1.Rows.Count - 1
        '            iLineNo1m = iLineNo1m + dt2o1.Rows(i)("rmcount")
        '        Next
        '    End If

        '    iLineNo1m = iLineNo1m + 1
        '    Dim dt2o As New DataTable
        '    strSqlQry = "select inventorytype,combinetype,commissiontype,isnull(specialoccassion,'') specialoccassion ,remarks,arrivaltransfer,departuretransfer,isnull(applydiscounttype,'') applydiscounttype,isnull(applydiscountids,'') applydiscountids  from view_offers_header where promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt2o)
        '    Dim xd2 As Integer

        '    If dt2o.Rows.Count > 0 Then
        '        xlSheet.Range("A" & iLineNo1m.ToString) = "Inventory Type"
        '        xlSheet.Range("A" & iLineNo1m.ToString).font.bold = True

        '        For i As Integer = 0 To dt2o.Rows.Count - 1
        '            xlSheet.Range("B" & iLineNo1m.ToString) = dt2o.Rows(i)("inventorytype")
        '            xlSheet.Range("C" & iLineNo1m.ToString) = "Apply Discount Type"
        '            xlSheet.Range("C" & iLineNo1m.ToString).font.bold = True
        '            xlSheet.Range("D" & iLineNo1m.ToString) = dt2o.Rows(i)("applydiscounttype")
        '            xlSheet.Range("E" & iLineNo1m.ToString) = "Apply DiscountID"
        '            xlSheet.Range("E" & iLineNo1m.ToString).font.bold = True
        '            xlSheet.Range("F" & iLineNo1m.ToString).wraptext = True
        '            xlSheet.Range("F" & iLineNo1m & ":" & "G" & iLineNo1m).Merge()
        '            xlSheet.Range("F" & iLineNo1m.ToString) = dt2o.Rows(i)("applydiscountids")

        '            iLineNo1m = iLineNo1m + 1

        '            Dim c As Integer
        '            xlSheet.Range("A" & iLineNo1m.ToString) = "Combine"
        '            xlSheet.Range("A" & iLineNo1m.ToString).font.bold = True
        '            xlSheet.Range("B" & iLineNo1m.ToString) = dt2o.Rows(i)("combinetype")
        '            If dt2o.Rows(i)("combinetype") = "Combinable with specific" Then
        '                'xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Hotel"
        '                strSqlQry = "Select promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.promotionid and c.promotionid='" & hdnpromotionid.Value & "'"
        '                Dim rsdmm1ac As ADODB.Recordset
        '                rsdmm1ac = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("C" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1ac)

        '                xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True
        '                c = 1
        '                xd2 = rsdmm1ac.RecordCount

        '            ElseIf dt2o.Rows(i)("combinetype") = "Combinable Mandatory with" Then
        '                'xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Hotel"
        '                strSqlQry = "Select promotionname  FROM view_offers_header h, view_offers_combinable c where h.promotionid=c.promotionid and c.promotionid='" & hdnpromotionid.Value & "'"
        '                Dim rsdmm1ac As ADODB.Recordset
        '                rsdmm1ac = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("C" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1ac)
        '                xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True

        '                xd2 = rsdmm1ac.RecordCount
        '                c = 2

        '            End If


        '            If c <> 2 And c <> 1 Then
        '                xlSheet.Range("C" & iLineNo1m.ToString) = "Commission"
        '                xlSheet.Range("C" & iLineNo1m.ToString).font.bold = True
        '                xlSheet.Range("D" & iLineNo1m.ToString) = dt2o.Rows(i)("commissiontype")
        '                xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True

        '            Else
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Commission"
        '                xlSheet.Range("D" & iLineNo1m - 1.ToString).font.bold = True
        '                xlSheet.Range("E" & iLineNo1m.ToString) = dt2o.Rows(i)("commissiontype")
        '                xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True

        '            End If

        '        Next
        '    End If

        '    iLineNo1m = iLineNo1m + xd2 + 3
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Non Refundable"
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString) = "Apply Discount to Exhibition supplement"
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString).wraptext = True
        '    xlSheet.Range("B" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Arrival Transfer"
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True
        '    xlSheet.Range("C" & iLineNo1m - 1.ToString).wraptext = True
        '    xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Departure Transfer "
        '    xlSheet.Range("D" & iLineNo1m - 1.ToString).wraptext = True
        '    xlSheet.Range("D" & iLineNo1m - 1.ToString).font.bold = True



        '    Dim rsdmm1a1 As ADODB.Recordset
        '    strSqlQry = "select  case when ISNULL(nonrefundable,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(applytdiscountoexhibition,0)=0 then 'No' else 'Yes' end  status, case when ISNULL(arrivaltransfer,0)=0 then 'No' else 'Yes' end  status,case when ISNULL(departuretransfer,0)=0 then 'No' else 'Yes' end  status   from view_offers_header h where  promotionid='" & hdnpromotionid.Value & "'"
        '    rsdmm1a1 = GetResultAsRecordSet(strSqlQry)
        '    xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1a1)

        '    iLineNo1m = iLineNo1m + 2
        '    Dim xa As Integer
        '    Dim xd As Integer
        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If dt2o.Rows.Count > 0 Then
        '                For c As Integer = 0 To dt2o.Rows.Count - 1
        '                    If dt31.Rows(i)("promotiontypes") = "Complimentary Airport Transfer" And dt2o.Rows(c)("arrivaltransfer") = 1 And dt2o.Rows(c)("departuretransfer") = 1 Then
        '                        xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Arrivals"
        '                        xlSheet.Range("D" & iLineNo1m - 1.ToString).font.bold = True
        '                        Dim rsdmm1a1a As ADODB.Recordset

        '                        strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Arrival' and promotionid='" & hdnpromotionid.Value & "'"
        '                        rsdmm1a1a = GetResultAsRecordSet(strSqlQry)
        '                        xlSheet.Range("D" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1a1a)
        '                        xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True
        '                        xa = rsdmm1a1a.RecordCount
        '                        Dim rsdmm1a1ad As ADODB.Recordset
        '                        strSqlQry = "select a.airportbordername  from view_offers_transfers t,airportbordersmaster a  where t.airportcode=a.airportbordercode and transfertype= 'Departure' and promotionid='" & hdnpromotionid.Value & "'"
        '                        xlSheet.Range("E" & iLineNo1m - 1.ToString) = "Departure"
        '                        xlSheet.Range("E" & iLineNo1m - 1.ToString).font.bold = True

        '                        rsdmm1a1ad = GetResultAsRecordSet(strSqlQry)
        '                        xlSheet.Range("E" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1a1ad)
        '                        xlSheet.Range("E" & iLineNo1m.ToString).wraptext = True
        '                        xd = rsdmm1a1a.RecordCount

        '                    End If
        '                Next
        '            End If

        '        Next
        '    End If

        '    If xd <> 0 Or xa <> 0 Then
        '        If xd > xa Then
        '            iLineNo1m = iLineNo1m + 2 + xd
        '        Else
        '            iLineNo1m = iLineNo1m + 2 + xa
        '        End If
        '    Else
        '        iLineNo1m = iLineNo1m + 2

        '    End If
        '    Dim d As Integer

        '    Dim xb2 As Integer

        '    If dt31.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt31.Rows.Count - 1
        '            If dt31.Rows(i)("promotiontypes") = "Select flights only" Then
        '                Dim rsdmm1a1f As ADODB.Recordset
        '                xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Flight"
        '                xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '                strSqlQry = "select flightcode from view_offers_flight where  promotionid='" & hdnpromotionid.Value & "'"
        '                rsdmm1a1f = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rsdmm1a1f)

        '                xa = rsdmm1a1f.RecordCount
        '            End If

        '            If dt31.Rows(i)("promotiontypes") = "Inter Hotels" Then
        '                If xa >= 1 Then

        '                    xlSheet.Range("B" & iLineNo1m - 1.ToString) = "Hotel Name"
        '                    xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Min.Stay"
        '                    xlSheet.Range("B" & iLineNo1m - 1.ToString).font.bold = True
        '                    xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True
        '                    Dim rs1h As ADODB.Recordset

        '                    strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & hdnpromotionid.Value & "'"
        '                    rs1h = GetResultAsRecordSet(strSqlQry)
        '                    xlSheet.Range("B" & iLineNo1m.ToString).CopyFromRecordset(rs1h)
        '                    xlSheet.Range("B" & iLineNo1m.ToString).wraptext = True
        '                    xa1 = rs1h.RecordCount
        '                    d = 1


        '                Else
        '                    xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Hotel Name"
        '                    xlSheet.Range("B" & iLineNo1m - 1.ToString) = "Min.Stay"
        '                    xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '                    xlSheet.Range("B" & iLineNo1m - 1.ToString).font.bold = True
        '                    Dim rs1h As ADODB.Recordset

        '                    strSqlQry = "select p.partyname,i.minstay FROM view_offers_interhotel i,partymast p where p.partycode=i.partycode and  i.promotionid='" & hdnpromotionid.Value & "'"
        '                    rs1h = GetResultAsRecordSet(strSqlQry)
        '                    xlSheet.Range("A" & iLineNo1m.ToString).CopyFromRecordset(rs1h)
        '                    xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True
        '                    xa1 = rs1h.RecordCount
        '                    d = 1
        '                End If

        '            End If

        '            If dt31.Rows(i)("promotiontypes") = "Special Occasion" Then


        '                xb2 = 1
        '                If dt2o.Rows.Count > 0 Then

        '                    For c As Integer = 0 To dt2o.Rows.Count - 1
        '                        If xa >= 1 And xa1 >= 1 Then
        '                            xlSheet.Range("D" & iLineNo1m - 1.ToString) = "Special Occasion"
        '                            xlSheet.Range("D" & iLineNo1m - 1.ToString).font.bold = True
        '                            xlSheet.Range("D" & iLineNo1m.ToString) = dt2o.Rows(c)("specialoccassion")
        '                            xlSheet.Range("D" & iLineNo1m.ToString).wraptext = True

        '                        ElseIf xa = 0 And xa1 >= 1 Then
        '                            xlSheet.Range("C" & iLineNo1m - 1.ToString) = "Special Occasion"
        '                            xlSheet.Range("C" & iLineNo1m - 1.ToString).font.bold = True
        '                            xlSheet.Range("C" & iLineNo1m.ToString) = dt2o.Rows(c)("specialoccassion")
        '                            xlSheet.Range("C" & iLineNo1m.ToString).wraptext = True
        '                        Else
        '                            xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Special Occasion"
        '                            xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '                            xlSheet.Range("A" & iLineNo1m.ToString) = dt2o.Rows(c)("specialoccassion")
        '                            xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True
        '                        End If

        '                    Next

        '                End If
        '            End If




        '        Next
        '    End If

        '    If xa1 <> 0 Then
        '        iLineNo1m = iLineNo1m + xa1 + 2

        '    ElseIf xa = 1 Or xb2 = 1 Then

        '        iLineNo1m = iLineNo1m + 3

        '    Else

        '        iLineNo1m = iLineNo1m

        '    End If

        '    xlSheet.Range("A" & iLineNo1m - 1.ToString) = "Remarks"
        '    xlSheet.Range("A" & iLineNo1m - 1.ToString).font.bold = True
        '    For c As Integer = 0 To dt2o.Rows.Count - 1
        '        xlSheet.Range("A" & iLineNo1m & ":" & "D" & iLineNo1m).Merge()
        '        xlSheet.Range("A" & iLineNo1m.ToString).wraptext = True
        '        xlSheet.Range("A" & iLineNo1m.ToString) = dt2o.Rows(c)("remarks")
        '        xlSheet.Range("A" & iLineNo1m.ToString).rowheight = 200

        '    Next

        '    xlSheet = xlBook.Worksheets(3)

        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    Dim iLineNo As Integer = 6
        '    Dim y As Integer
        '    Dim x As Integer
        '    Dim z As Integer

        '    Dim dt3 As New DataTable
        '    strSqlQry = "select tranid from view_contracts_commission_header where promotionid = '" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt3)
        '    If dt3.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt3.Rows.Count - 1
        '            xlSheet.Range("A" & iLineNo - 1.ToString) = "Promotionid"
        '            xlSheet.Range("B" & iLineNo - 1.ToString) = "Promotionname"
        '            xlSheet.Range("A" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLineNo - 1.ToString).FONT.BOLD = True

        '            strSqlQry = "select h.promotionid,h.promotionname   from view_offers_header h WHERE h.partycode='" & CType(Session("Contractparty"), String) & "' and h.promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rsic As New ADODB.Recordset
        '            rsic = GetResultAsRecordSet(strSqlQry)
        '            xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rsic)
        '            xlSheet.Range("B" & iLineNo.ToString).wraptext = True
        '            iLineNo = iLineNo + 3
        '            xlSheet.Range("A" & iLineNo - 1.ToString) = "From Date"
        '            xlSheet.Range("B" & iLineNo - 1.ToString) = "To Date"
        '            xlSheet.Range("B" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("C" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("C" & iLineNo - 1.ToString) = "Room Classification"
        '            xlSheet.Range("A" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("C" & iLineNo - 1.ToString).FONT.BOLD = True
        '            strSqlQry = "select isnull(d.fromdate,''),isnull(d.todate,'') from view_contracts_commission_detail d where d.tranid='" & dt3.Rows(i)("tranid").ToString & "'"
        '            Dim rsic1 As New ADODB.Recordset
        '            rsic1 = GetResultAsRecordSet(strSqlQry)
        '            xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rsic1)
        '            y = rsic1.RecordCount
        '            strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & hdnpromotionid.Value & "' and isnull(h.roomcategory,'')<>'' for xml path('')),1,1,''),'')"

        '            Dim rs2 As New ADODB.Recordset
        '            rs2 = GetResultAsRecordSet(strSqlQry)
        '            z = rs2.RecordCount
        '            xlSheet.Range("C" & iLineNo.ToString).CopyFromRecordset(rs2)
        '            xlSheet.Range("C" & iLineNo.ToString).wraptext = True
        '            If y > z Then
        '                iLineNo = iLineNo + y + 2
        '            Else
        '                iLineNo = iLineNo + z + 2
        '            End If
        '            xlSheet.Range("A" & iLineNo - 1.ToString) = "Room Type"
        '            xlSheet.Range("B" & iLineNo - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("A" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLineNo - 1.ToString).FONT.BOLD = True
        '            strSqlQry = "select roomType=isnull(stuff((select ',' + prm.rmtypname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and partycode='RI13' and isnull(h.roomtypes,'')<>'' for xml path('')),1,1,''),''), h.mealplans from view_contracts_commission_header h where h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rs1 As New ADODB.Recordset
        '            rs1 = GetResultAsRecordSet(strSqlQry)

        '            xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rs1)
        '            xlSheet.Range("A" & iLineNo.ToString).wraptext = True
        '            xlSheet.Range("B" & iLineNo.ToString).wraptext = True
        '            'xlSheet.Range("C" & iLineNo2.ToString).wraptext = True
        '            x = rs1.RecordCount
        '            iLineNo = iLineNo + x + 2
        '            xlSheet.Range("A" & iLineNo - 1.ToString) = "Formulaname"
        '            xlSheet.Range("A" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLineNo - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLineNo - 1.ToString) = "Formulastring"
        '            strSqlQry = "select distinct v.formulaname,formulastring=isnull(stuff((select ';' + c.term1+','+ltrim(rtrim(convert(varchar(20),c.value)))  from view_contracts_commissions c,commissionformula_header h where c.formulaid=h.formulaid  and  c.tranid ='" & dt3.Rows(i)("tranid").ToString & "'    order by c.clineno for xml path('')),1,1,''),'') from view_contracts_commission_header h, view_contracts_commissions c, commissionformula_header v  where h.tranid = c.tranid And c.formulaid = v.formulaid and h.tranid ='" & dt3.Rows(i)("tranid").ToString & "'"
        '            Dim rs3 As New ADODB.Recordset
        '            rs3 = GetResultAsRecordSet(strSqlQry)
        '            ' Dim iLineNo1 As Integer = 10
        '            xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rs3)
        '            xlSheet.Range("A" & iLineNo.ToString).wraptext = True
        '            xlSheet.Range("B" & iLineNo.ToString).wraptext = True
        '            'x = rs1.RecordCount
        '        Next
        '    End If
        '    xlSheet = xlBook.Worksheets(4)

        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value
        '    Dim dtmx As New DataTable


        '    strSqlQry = "select distinct tranid from view_partymaxacc_header where partycode= '" & CType(Session("Contractparty"), String) & "' and promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtmx)


        '    Dim iLinmx As Integer = 6

        '    Dim em1 As Integer
        '    Dim em As Integer
        '    Dim em2 As Integer



        '    Dim dt23 As New DataTable
        '    If dtmx.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtmx.Rows.Count - 1

        '            strSqlQry = "select h.promotionid,h.promotionname ,applicableto from view_offers_header h WHERE h.partycode='" & CType(Session("Contractparty"), String) & "' and h.promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rsicm As New ADODB.Recordset
        '            rsicm = GetResultAsRecordSet(strSqlQry)
        '            xlSheet.Range("A" & iLinmx.ToString).CopyFromRecordset(rsicm)
        '            xlSheet.Range("B" & iLinmx.ToString).wraptext = True
        '            xlSheet.Range("A" & iLinmx - 1.ToString) = "Promotionid"
        '            xlSheet.Range("B" & iLinmx - 1.ToString) = "Promotionname"
        '            xlSheet.Range("B" & iLinmx - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("C" & iLinmx - 1.ToString) = "Applicable To"
        '            xlSheet.Range("C" & iLinmx - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("C" & iLinmx - 1.ToString).FONT.BOLD = True

        '            xlSheet.Range("A" & iLinmx - 1.ToString).FONT.BOLD = True
        '            xlSheet.Range("B" & iLinmx - 1.ToString).FONT.BOLD = True
        '            iLinmx = iLinmx + 3

        '            xlSheet.Range("A" & iLinmx - 1.ToString) = "Max Occ.ID"
        '            xlSheet.Range("B" & iLinmx - 1.ToString) = "Room Name"
        '            xlSheet.Range("A" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinmx - 1.ToString).font.bold = True

        '            xlSheet.Range("C" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinmx - 1.ToString).font.bold = True

        '            xlSheet.Range("F" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("J" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("K" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("L" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("M" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("N" & iLinmx - 1.ToString).font.bold = True


        '            xlSheet.Range("C" & iLinmx - 1.ToString) = "Room Classification"
        '            xlSheet.Range("D" & iLinmx - 1.ToString) = "unit yes/no	"
        '            xlSheet.Range("E" & iLinmx - 1.ToString) = "Price Adult Occupancy only for Unit"
        '            xlSheet.Range("E" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("F" & iLinmx - 1.ToString) = "Price Pax"
        '            xlSheet.Range("G" & iLinmx - 1.ToString) = "Max Adults"
        '            xlSheet.Range("H" & iLinmx - 1.ToString) = "Max Child"
        '            xlSheet.Range("I" & iLinmx - 1.ToString) = "Max Infant"
        '            xlSheet.Range("J" & iLinmx - 1.ToString) = "Max EB"

        '            xlSheet.Range("K" & iLinmx - 1.ToString) = "Max Total Occupancy without infant"
        '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("L" & iLinmx - 1.ToString) = "Rank Order"
        '            xlSheet.Range("N" & iLinmx - 1.ToString) = "Start with 0 based"
        '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("M" & iLinmx - 1.ToString) = "Occupancy Combinations"
        '            xlSheet.Range("M" & iLinmx - 1.ToString).wraptext = True



        '            Dim dtmx1 As New DataTable
        '            strSqlQry = "select rmtypcode from view_partymaxaccomodation where partycode= '" & CType(Session("Contractparty"), String) & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dtmx1)


        '            strSqlQry = "select distinct m.tranid,prm.rmtypname,rc.roomclassname, prm.unityesno,m.pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb,m.noofextraperson, m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            Dim rsmx As New ADODB.Recordset
        '            rsmx = GetResultAsRecordSet(strSqlQry)
        '            em = rsmx.RecordCount

        '            xlSheet.Range("A" & iLinmx.ToString).CopyFromRecordset(rsmx)
        '            xlSheet.Range("M" & iLinmx.ToString).wraptext = True
        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If
        '            Dim rsmx1 As New ADODB.Recordset
        '            If dtmx1.Rows.Count > 0 Then
        '                For idt As Integer = 0 To dtmx1.Rows.Count - 1



        '                    strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "

        '                    rsmx1 = GetResultAsRecordSet(strSqlQry)
        '                    em1 = rsmx1.RecordCount
        '                    xlSheet.Range("M" & iLinmx.ToString).CopyFromRecordset(rsmx1)

        '                    iLinmx = iLinmx + 1

        '                Next

        '            End If



        '            strSqlQry = "select  start0based from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            Dim rsmx2 As New ADODB.Recordset
        '            rsmx2 = GetResultAsRecordSet(strSqlQry)
        '            em2 = rsmx2.RecordCount
        '            iLinmx = iLinmx - dtmx1.Rows.Count

        '            xlSheet.Range("N" & iLinmx.ToString).CopyFromRecordset(rsmx2)


        '            Dim Maxint As Integer = Math.Max(em, Math.Max(em1, em2))
        '            iLinmx = iLinmx + 2 + Maxint

        '        Next

        '    End If
        '    xlSheet = xlBook.Worksheets(5)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    Dim dtrr2 As New DataTable
        '    strSqlQry = "select plistcode from view_cplisthnew  where promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtrr2)
        '    Dim iLine2 As Integer = 6
        '    Dim ei2 As Integer
        '    Dim ei3 As Integer
        '    Dim ei4 As Integer
        '    Dim ei4r As Integer
        '    If dtrr2.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtrr2.Rows.Count - 1

        '            strSqlQry = "select h.plistcode,h.promotionid,p.promotionname,h.applicableto  from view_offers_header p, view_cplisthnew  h WHERE p.promotionid=h.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "' and h.promotionid='" & hdnpromotionid.Value & "' and plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
        '            Dim rse2 As New ADODB.Recordset
        '            rse2 = GetResultAsRecordSet(strSqlQry)
        '            ei2 = rse2.RecordCount
        '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse2)
        '            xlSheet.Range("A" & iLine2 - 1.ToString) = "PriceList Code"
        '            xlSheet.Range("B" & iLine2 - 1.ToString) = "Promotionid"
        '            xlSheet.Range("C" & iLine2 - 1.ToString) = "Promotionname"
        '            xlSheet.Range("C" & iLine2.ToString).WRAPTEXT = True

        '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine2 - 1.ToString) = "Aplicable to"
        '            xlSheet.Range("D" & iLine2.ToString).WRAPTEXT = True

        '            iLine2 = iLine2 + 3
        '            strSqlQry = "select isnull(fromdate,''),isnull(todate,'') from view_cplisthnew_offerdates WHERE plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
        '            Dim rse32 As New ADODB.Recordset
        '            rse32 = GetResultAsRecordSet(strSqlQry)
        '            ei4 = rse32.RecordCount
        '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse32)



        '            xlSheet.Range("A" & iLine2 - 1.ToString) = "From Date"
        '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine2 - 1.ToString) = "To Date"
        '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True

        '            xlSheet.Range("B" & iLine2.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("A" & iLine2.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            iLine2 = iLine2 + ei4 + 1
        '            Dim fromrange As Integer, torange As Integer
        '            fromrange = iLine2
        '            torange = IIf(rse32.RecordCount > 0, iLine2 + rse32.RecordCount, iLine2)

        '            'xlSheet.Range("B" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            'xlSheet.Range("C" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"

        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_cplisthnew_weekdays   where  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' for xml path('')),1,1,''),'') "

        '            Dim rsw2 As New ADODB.Recordset
        '            rsw2 = GetResultAsRecordSet(strSqlQry)
        '            xlSheet.Range("B" & iLine2.ToString).CopyFromRecordset(rsw2)
        '            xlSheet.Range("B" & iLine2.ToString).wraptext = True
        '            xlSheet.Range("B" & iLine2 & ":" & "C" & iLine2).Merge()
        '            xlSheet.Range("B" & iLine2.ToString).rowheight = 30
        '            xlSheet.Range("A" & iLine2.ToString) = "Days of the week"
        '            xlSheet.Range("A" & iLine2.ToString).font.bold = True

        '            'strSqlQry = "select  c.mealplans,m.mealname from view_cplisthnew c,mealmast m where m.mealcode=c.mealplans and  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and  c.promotionid='" & hdnpromotionid.Value & "'"
        '            'Dim rse32r As New ADODB.Recordset
        '            'rse32r = GetResultAsRecordSet(strSqlQry)
        '            'ei4r = rse32r.RecordCount
        '            'xlSheet.Range("A" & iLine2 - 1.ToString).CopyFromRecordset(rse32r)
        '            'xlSheet.Range("A" & iLine2 - 1.ToString) = "Meal Code"
        '            'xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True
        '            'xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
        '            'xlSheet.Range("B" & iLine2 - 1.ToString) = "Meal Name"

        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If



        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_roomrates]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@plistcode", dtrr2.Rows(i)("plistcode").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)

        '                    End Using
        '                End Using
        '            End Using


        '            Dim rssp72 As New ADODB.Recordset
        '            rssp72 = convertToADODB(dtt)
        '            Dim ii3 As Integer = 65
        '            'For Each column As DataColumn In dt.Columns
        '            'name(ii) = column.ColumnName
        '            iLine2 = iLine2 + 2
        '            Dim sss3 = Chr(ii3).ToString
        '            For OO As Integer = 0 To dtt.Columns.Count - 1
        '                xlSheet.Range(sss3.ToString() + iLine2.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                ii3 += 1
        '                xlSheet.Range(sss3.ToString() + (iLine2).ToString).FONT.BOLD = True
        '                sss3 = Chr(ii3).ToString
        '            Next

        '            If rssp72.RecordCount > 0 Then
        '                iLine2 = iLine2 + ei4r + 2
        '                xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rssp72)
        '                ei3 = rssp72.RecordCount

        '                fromrange = iLine2
        '                torange = IIf(rssp72.RecordCount > 0, iLine2 + rssp72.RecordCount, iLine2)
        '                xlSheet.Range("C" & fromrange.ToString & ":" & "C" & torange.ToString).NumberFormat = "####"
        '            End If

        '            iLine2 = iLine2 + 2

        '        Next

        '    End If

        '    xlSheet = xlBook.Worksheets(6)

        '    xlSheet.Range("B3").Value = ViewState("hotelname")


        '    Dim dte As New DataTable
        '    strSqlQry = "select exhibitionid from view_contracts_exhibition_header  where promotionid= '" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dte)
        '    Dim iLinee As Integer = 6
        '    Dim ei As Integer
        '    Dim ze As Integer
        '    Dim chkc As Integer

        '    If dte.Rows.Count > 0 Then
        '        For i As Integer = 0 To dte.Rows.Count - 1



        '            strSqlQry = "select c.exhibitionid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d, view_contracts_exhibition_header c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "'  and  h.promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rscpcr As New ADODB.Recordset
        '            rscpcr = GetResultAsRecordSet(strSqlQry)
        '            chkc = rscpcr.RecordCount
        '            xlSheet.Range("A" & iLinee - 1.ToString) = "Exhibition Id"
        '            xlSheet.Range("B" & iLinee - 1.ToString) = "PromotionId"
        '            xlSheet.Range("C" & iLinee - 1.ToString) = "Promotion Name"
        '            xlSheet.Range("D" & iLinee - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinee - 1.ToString).font.bold = True

        '            xlSheet.Range("A" & iLinee.ToString).CopyFromRecordset(rscpcr)
        '            xlSheet.Range("C" & iLinee.ToString).WRAPTEXT = True

        '            iLinee = iLinee + 3




        '            xlSheet.Range("A" & iLinee - 1.ToString) = "Exhibition Name"

        '            xlSheet.Range("B" & iLinee - 1.ToString) = "From Date"
        '            xlSheet.Range("C" & iLinee - 1.ToString) = "To Date"
        '            xlSheet.Range("D" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinee - 1.ToString) = "Room Type"
        '            xlSheet.Range("E" & iLinee - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("F" & iLinee - 1.ToString) = "Supplement Amount"
        '            xlSheet.Range("F" & iLinee - 1.ToString).wraptext = True
        '            xlSheet.Range("G" & iLinee - 1.ToString) = "With Drawn"


        '            xlSheet.Range("B" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLinee - 1.ToString).font.bold = True


        '            strSqlQry = "select  e.exhibitionname,isnull(d.fromdate,''),isnull(d.todate,'') from view_contracts_exhibition_detail d join exhibition_master e on d.exhibitioncode=e.exhibitioncode join  view_contracts_exhibition_header h on d.exhibitionid=h.exhibitionid and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            Dim rse As New ADODB.Recordset
        '            rse = GetResultAsRecordSet(strSqlQry)
        '            ei = rse.RecordCount
        '            xlSheet.Range("A" & iLinee.ToString).CopyFromRecordset(rse)
        '            xlSheet.Range("B" & iLinee.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("C" & iLinee.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            strSqlQry = "select distinct mealplans,supplementvalue,isnull(withdraw,'') from view_contracts_exhibition_detail where exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            Dim rsr As New ADODB.Recordset
        '            rsr = GetResultAsRecordSet(strSqlQry)
        '            ze = rsr.RecordCount
        '            xlSheet.Range("E" & iLinee.ToString).CopyFromRecordset(rsr)
        '            xlSheet.Range("E" & iLinee.ToString).wraptext = True
        '            Dim dter As New DataTable
        '            strSqlQry = "select distinct exhibitioncode from view_contracts_exhibition_detail  where  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dter)



        '            Dim xee As Integer
        '            Dim yee As Integer
        '            If dter.Rows.Count > 0 Then
        '                For er As Integer = 0 To dter.Rows.Count - 1





        '                    strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "' and d.exhibitioncode='" & dter.Rows(er)("exhibitioncode").ToString & "' for xml path('')),1,1,''),'') "
        '                    Dim rser As New ADODB.Recordset
        '                    rser = GetResultAsRecordSet(strSqlQry)
        '                    yee = rser.RecordCount
        '                    iLinee = iLinee
        '                    xlSheet.Range("D" & iLinee.ToString).CopyFromRecordset(rser)
        '                    xlSheet.Range("D" & iLinee.ToString).wraptext = True
        '                    'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
        '                    'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                    'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                    iLinee = iLinee + y


        '                Next

        '            End If

        '            Dim Maxint As Integer = Math.Max(ze, Math.Max(ei, y))

        '            iLinee = iLinee + Maxint + 2
        '        Next
        '    End If

        '    xlSheet = xlBook.Worksheets(7)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")



        '    Dim dtmr As New DataTable
        '    strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header where promotionid= '" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtmr)
        '    Dim iLinmr As Integer = 8
        '    Dim m7 As Integer
        '    Dim s7 As Integer
        '    Dim e7 As Integer
        '    Dim mn7 As Integer


        '    If dtmr.Rows.Count > 0 Then
        '        ' Dim conn As New ADODB.Connection
        '        For i As Integer = 0 To dtmr.Rows.Count - 1
        '            xlSheet.Range("C" & iLinmr - 1.ToString) = "Promotion Name"
        '            xlSheet.Range("D" & iLinmr - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Supplement ID"
        '            xlSheet.Range("B" & iLinmr - 1.ToString) = "PromotionId"
        '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString).font.bold = True



        '            strSqlQry = "select mealsupplementid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d ,view_contracts_mealsupp_header c where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "'  and  h.promotionid='" & hdnpromotionid.Value & "'  and  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsm7 As New ADODB.Recordset
        '            rsm7 = GetResultAsRecordSet(strSqlQry)
        '            m7 = rsm7.RecordCount
        '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rsm7)

        '            xlSheet.Range("C" & iLinmr.ToString).WRAPTEXT = True


        '            iLinmr = iLinmr + m7 + 2



        '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Manual From Date not linked to Seasons"
        '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("B" & iLinmr - 1.ToString) = "Manual To Date not linked to Seasons"
        '            xlSheet.Range("B" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinmr - 1.ToString) = "Excluded From Date"
        '            xlSheet.Range("C" & iLinmr - 1.ToString).font.bold = True
        '            'xlSheet.Range("G" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString) = "Excluded To Date"
        '            xlSheet.Range("D" & iLinmr - 1.ToString).font.bold = True

        '            strSqlQry = "select  isnull(fromdate,'')  ,isnull(todate,'') from  view_contracts_mealsupp_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsmc As New ADODB.Recordset
        '            rsmc = GetResultAsRecordSet(strSqlQry)
        '            mn7 = rsmc.RecordCount



        '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rsmc)
        '            xlSheet.Range("A" & iLinmr.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("B" & iLinmr.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            strSqlQry = " select  isnull(fromdate,'')  ,isnull(todate,'') from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsed As New ADODB.Recordset
        '            rsed = GetResultAsRecordSet(strSqlQry)
        '            e7 = rsed.RecordCount

        '            xlSheet.Range("C" & iLinmr.ToString).CopyFromRecordset(rsed)
        '            xlSheet.Range("D" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("C" & iLineNo.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_mealsupp_weekdays  where  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' for xml path('')),1,1,''),'') "

        '            Dim rsw As New ADODB.Recordset
        '            rsw = GetResultAsRecordSet(strSqlQry)


        '            xlSheet.Range("E" & iLinmr.ToString).CopyFromRecordset(rsw)
        '            xlSheet.Range("E" & iLinmr.ToString).wraptext = True

        '            xlSheet.Range("E" & iLinmr - 1.ToString) = "Days of the week"
        '            xlSheet.Range("E" & iLinmr - 1.ToString).font.bold = True
        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If


        '            If mn7 > e7 Then
        '                iLinmr = iLinmr + mn7
        '            Else
        '                iLinmr = iLinmr + e7
        '            End If

        '            'strSqlQry = "select rmcat=isnull(stuff((select ',' + prm.rmcatname  from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join rmcatmast prm on s.Item1=prm.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and h.promotionid='" & hdnpromotionid.Value & "' and isnull(h.roomcategory,'')<>'' for xml path('')),1,1,''),'')"

        '            'Dim rs2 As New ADODB.Recordset
        '            'rs2 = GetResultAsRecordSet(strSqlQry)
        '            'z = rs2.RecordCount
        '            'xlSheet.Range("C" & iLineNo.ToString).CopyFromRecordset(rs2)
        '            'xlSheet.Range("C" & iLineNo.ToString).wraptext = True
        '            'strSqlQry = " select  convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105) from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            'Dim rsedq As New ADODB.Recordset
        '            'rsedq = GetResultAsRecordSet(strSqlQry)
        '            'e17 = rsed.RecordCount

        '            'xlSheet.Range("C" & iLinmr.ToString).CopyFromRecordset(rsedq)

        '            'If conn.State = ConnectionState.Open Then
        '            '    conn.Close()
        '            'End If

        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_mealsupplements]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@mealsupplementid", dtmr.Rows(i)("mealsupplementid").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)
        '                        'If dtt.Rows(i)(i) = "-3" Then
        '                        '    "Free"

        '                        '    "Incl"
        '                        '    txt.Text = "-1"
        '                        '    Case "N.Incl"
        '                        '    txt.Text = "-2"
        '                        '    Case "N/A"
        '                        '    txt.Text = "-4"
        '                        '    Case "On Request"
        '                        '    txt.Text = "-5"

        '                    End Using
        '                End Using
        '            End Using




        '            Dim rssp7 As New ADODB.Recordset
        '            rssp7 = convertToADODB(dtt)
        '            iLinmr = iLinmr + 1


        '            'Dim name(dtt.Columns.Count) As String
        '            Dim ii As Integer = 65
        '            'For Each column As DataColumn In dt.Columns
        '            'name(ii) = column.ColumnName

        '            Dim sss = Chr(ii).ToString
        '            For OO As Integer = 0 To dtt.Columns.Count - 1
        '                xlSheet.Range(sss.ToString() + iLinmr.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                ii += 1
        '                xlSheet.Range(sss.ToString() + (iLinmr).ToString).FONT.BOLD = True
        '                sss = Chr(ii).ToString
        '            Next





        '            'Next


        '            If rssp7.RecordCount > 0 Then
        '                iLinmr = iLinmr + 1
        '                xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rssp7)
        '                s7 = rssp7.RecordCount

        '            End If


        '            iLinmr = iLinmr + s7 + 3

        '        Next

        '    End If
        '    xlSheet = xlBook.Worksheets(8)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")


        '    Dim dtcpi As New DataTable
        '    strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtcpi)
        '    Dim iLine8 As Integer = 7

        '    If dtcpi.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtcpi.Rows.Count - 1
        '            xlSheet.Range("B" & iLine8 - 1.ToString) = "PromotionId"
        '            xlSheet.Range("C" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine8 - 1.ToString) = "Promotion Name"
        '            xlSheet.Range("A" & iLine8 - 1.ToString) = "ChildPolicy Id"
        '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine8 - 1.ToString) = "Applicable To"
        '            xlSheet.Range("C" & iLine8 - 1.ToString).WRAPTEXT = True


        '            Dim chk8 As Integer
        '            Dim ei31 As Integer
        '            strSqlQry = "select c.childpolicyid,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d ,view_contracts_childpolicy_header c where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "'  and  h.promotionid='" & hdnpromotionid.Value & "'  and  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
        '            Dim rs8 As New ADODB.Recordset
        '            rs8 = GetResultAsRecordSet(strSqlQry)
        '            chk8 = rs8.RecordCount
        '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rs8)
        '            xlSheet.Range("C" & iLine8.ToString).WRAPTEXT = True

        '            iLine8 = iLine8 + 3


        '            xlSheet.Range("B" & iLine8 - 1.ToString) = "Manual To Date not linked to Seasons"
        '            xlSheet.Range("B" & iLine8 - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & iLine8 - 1.ToString) = "Manual From Date not linked to Seasons"
        '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True

        '            xlSheet.Range("A" & iLine8 - 1.ToString).WRAPTEXT = True


        '            strSqlQry = "select  isnull(fromdate,'') ,isnull(todate,'') from  view_contracts_childpolicy_dates  where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
        '            Dim rsmc As New ADODB.Recordset
        '            rsmc = GetResultAsRecordSet(strSqlQry)
        '            mn7 = rsmc.RecordCount



        '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rsmc)
        '            xlSheet.Range("A" & iLine8.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("B" & iLine8.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_childpolicy_weekdays where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' for xml path('')),1,1,''),'') "
        '            Dim rss8 As New ADODB.Recordset
        '            rss8 = GetResultAsRecordSet(strSqlQry)


        '            xlSheet.Range("C" & iLine8 - 1.ToString) = "Days of the week"
        '            xlSheet.Range("C" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine8.ToString).WrapText = True
        '            xlSheet.Range("C" & iLine8.ToString).CopyFromRecordset(rss8)
        '            xlSheet.Range("C" & iLine8 - 1 & ":" & "D" & iLine8 - 1).Merge()
        '            xlSheet.Range("C" & iLine8 & ":" & "D" & iLine8).Merge()
        '            iLine8 = iLine8 + mn7 + 1






        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If



        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_childpolicy]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@childpolicyid", dtcpi.Rows(i)("childpolicyid").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)

        '                    End Using
        '                End Using
        '            End Using


        '            Dim rssp721 As New ADODB.Recordset
        '            rssp721 = convertToADODB(dtt)
        '            If rssp721.RecordCount > 0 Then

        '                Dim ii2 As Integer = 65
        '                'For Each column As DataColumn In dt.Columns
        '                'name(ii) = column.ColumnName

        '                Dim sss2 = Chr(ii2).ToString

        '                For OO As Integer = 0 To dtt.Columns.Count - 1
        '                    xlSheet.Range(sss2.ToString() + iLine8.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                    ii2 += 1
        '                    xlSheet.Range(sss2.ToString() + (iLine8).ToString).FONT.BOLD = True
        '                    sss2 = Chr(ii2).ToString

        '                Next


        '                iLine8 = iLine8 + 1

        '                xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rssp721)
        '                ei31 = rssp721.RecordCount

        '            End If

        '            iLine8 = iLine8 + ei31 + 2

        '        Next

        '    End If


        '    xlSheet = xlBook.Worksheets(9)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")


        '    Dim dtcn As New DataTable
        '    strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where  promotionid= '" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtcn)
        '    Dim iLinecr2 As Integer = 6


        '    If dtcn.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtcn.Rows.Count - 1

        '            Dim rm2 As Integer
        '            Dim ml2 As Integer
        '            Dim chk1cc As Integer
        '            Dim co2 As Integer
        '            Dim cocc As Integer
        '            Dim ns As Integer
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Cancellation ID"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "PromotionId"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "Promotion Name"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True

        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True

        '            strSqlQry = "select c.cancelpolicyid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d, view_contracts_cancelpolicy_header   c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "'  and  h.promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rscp As New ADODB.Recordset
        '            rscp = GetResultAsRecordSet(strSqlQry)
        '            chk1cc = rscp.RecordCount

        '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)
        '            xlSheet.Range("C" & iLinecr2.ToString).WRAPTEXT = True
        '            iLinecr2 = iLinecr2 + 3



        '            strSqlQry = "select isnull(fromdate,'')  ,isnull(todate,'') from view_contracts_cancelpolicy_offerdates where cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '            Dim rsc As New ADODB.Recordset
        '            rsc = GetResultAsRecordSet(strSqlQry)
        '            cocc = rsc.RecordCount

        '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rsc)
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Promotion From Date"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Promotion To Date"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("A" & iLinecr2.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            iLinecr2 = iLinecr2 + cocc + 2



        '            '        strSqlQry = "select distinct d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & hdncontractid.Value & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and h.promotionid= '" & hdnpromotionid.Value & "'"
        '            '        Dim rscp As New ADODB.Recordset
        '            '        rscp = GetResultAsRecordSet(strSqlQry)
        '            '        co2 = rscp.RecordCount
        '            '        xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)

        '            '        strSqlQry = "select h.promotionid,h.promotionname ,h.applicableto,d.fromdate,d.todate,case when ISNULL(h.approved,0)=0 then 'No' else 'Yes' end  status    from view_offers_header h,view_offers_detail d, view_contracts_cancelpolicy_header c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='RI13' and  h.promotionid= '" & hdnpromotionid.Value & "'"
        '            '        Dim rs2e As New ADODB.Recordset
        '            '        rs2e = GetResultAsRecordSet(strSqlQry)
        '            '        ce2 = rs2e.RecordCount
        '            '        xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rs2e)


        '            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_header  h join view_contracts_cancelpolicy_detail d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode  and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and h.promotionid= '" & hdnpromotionid.Value & "' and p.partycode='RI13' for xml path('')),1,1,''),'') "

        '            Dim rs2r As New ADODB.Recordset
        '            rs2r = GetResultAsRecordSet(strSqlQry)
        '            rm2 = rs2r.RecordCount

        '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rs2r)
        '            xlSheet.Range("A" & iLinecr2.ToString).wraptext = True
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True

        '            strSqlQry = "select mealplans,nodayshours, dayshours,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_detail where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

        '            Dim rsr2 As New ADODB.Recordset
        '            rsr2 = GetResultAsRecordSet(strSqlQry)
        '            ml2 = rsr2.RecordCount
        '            '    
        '            xlSheet.Range("B" & iLinecr2.ToString).CopyFromRecordset(rsr2)
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "No.of Days or Hours"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Unit -Days/Hours"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "No. OfNights to charge"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString) = "Percentage to charge"
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString) = "Value to charge"
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString).font.bold = True

        '            If rm2 > ml2 Then
        '                iLinecr2 = iLinecr2 + rm2 + 2
        '            Else
        '                iLinecr2 = iLinecr2 + ml2 + 2
        '            End If
        '            '        'strSqlQry = "select  distinct p.rmtypname from view_contracts_cancelpolicy_noshowearly  h  join view_contracts_cancelpolicy_detail  d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =d.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '            '        'Dim rsr4 As New ADODB.Recordset
        '            '        'rsr4 = GetResultAsRecordSet(strSqlQry)
        '            '        'rm2 = rsr4.RecordCount
        '            '        'If ce2 Or co2 <> 0 Then
        '            '        '    If ce2 > co2 Then
        '            '        '        iLinecr2 = iLinecr2 + ce2 + 2
        '            '        '    ElseIf co2 > ce2 Then
        '            '        '        iLinecr2 = iLinecr2 + co2 + 2
        '            '        '    End If
        '            '        'Else
        '            '        '    iLinecr2 = iLinecr2 + 3
        '            '        'End If
        '            '        'xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rsr4)

        '            strSqlQry = "select noshowearly from view_contracts_cancelpolicy_noshowearly  where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"


        '            Dim dtns As New DataTable
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dtns)
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "No Show/Early Checkout"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).wraptext = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Charge Basis"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Percentage to charge"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "No.of Nights to charge"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString) = "Percentage to charge"
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString) = "Value to charge"
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString).font.bold = True

        '            If dtns.Rows.Count > 0 Then
        '                For i2 As Integer = 0 To dtns.Rows.Count - 1

        '                    strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),''), mealplans,d.noshowearly,chargebasis,nightstocharge, percentagetocharge,valuetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '                    Dim rsrns4 As New ADODB.Recordset
        '                    rsrns4 = GetResultAsRecordSet(strSqlQry)
        '                    ns = rsrns4.RecordCount
        '                    xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rsrns4)
        '                    xlSheet.Range("A" & iLinecr2.ToString).WrapText = True
        '                    iLinecr2 = iLinecr2 + 1
        '                Next
        '            End If



        '            iLinecr2 = iLinecr2 + ns + 3

        '        Next
        '    End If


        '    xlSheet = xlBook.Worksheets(10)

        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    Dim dtc As New DataTable
        '    strSqlQry = "select checkinoutpolicyid FROM view_contracts_checkinout_header where promotionid='" & hdnpromotionid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtc)
        '    Dim linelbct As Integer = 6

        '    If dtc.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtc.Rows.Count - 1

        '            Dim rm As Integer
        '            Dim ml As Integer
        '            Dim tm As Integer
        '            Dim co As Integer
        '            Dim de As Integer
        '            Dim chk As Integer



        '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/OutPolicyId"
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "PromotionId"
        '            xlSheet.Range("C" & linelbct - 1.ToString) = "Promotion Name"
        '            xlSheet.Range("D" & linelbct - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True

        '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True

        '            strSqlQry = "select c.checkinoutpolicyid ,h.promotionid,h.promotionname ,h.applicableto from view_offers_header h,view_offers_detail d, view_contracts_checkinout_header  c(nolock)  where h.promotionid=c.promotionid and  h.promotionid= d.promotionid and h.partycode='" & CType(Session("Contractparty"), String) & "'  and  h.promotionid='" & hdnpromotionid.Value & "'"
        '            Dim rscp As New ADODB.Recordset
        '            rscp = GetResultAsRecordSet(strSqlQry)
        '            chk = rscp.RecordCount

        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rscp)
        '            xlSheet.Range("C" & linelbct.ToString).WRAPTEXT = True
        '            linelbct = linelbct + 3

        '            strSqlQry = "select isnull(fromdate,''),isnull(todate,'') from view_contracts_checkinout_offerdates where checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rsc As New ADODB.Recordset
        '            rsc = GetResultAsRecordSet(strSqlQry)
        '            co = rsc.RecordCount
        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsc)
        '            xlSheet.Range("A" & linelbct - 1.ToString) = "Promotion From Date"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "Promotion To Date"
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("A" & linelbct.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            linelbct = linelbct + co + 1

        '            strSqlQry = "select isnull(stuff((select ',' + mealcode from view_contracts_checkinout_mealplans where  checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' for xml path('')),1,1,''),'') "
        '            Dim rscm As New ADODB.Recordset
        '            rscm = GetResultAsRecordSet(strSqlQry)
        '            ml = rscm.RecordCount

        '            xlSheet.Range("D" & linelbct.ToString).CopyFromRecordset(rscm)
        '            'xlSheet.Range("D" & linelbct & ":" & "D" & linelbct).Merge()
        '            'xlSheet.Range("B:C").Merge()

        '            xlSheet.Range("C" & linelbct.ToString) = "Meal Plan"
        '            xlSheet.Range("C" & linelbct.ToString).font.bold = True

        '            strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "
        '            Dim rscr As New ADODB.Recordset
        '            rscr = GetResultAsRecordSet(strSqlQry)
        '            rm = rscr.RecordCount



        '            xlSheet.Range("B" & linelbct.ToString).CopyFromRecordset(rscr)
        '            'xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
        '            'xlSheet.Range("e:C").Merge()
        '            xlSheet.Range("B" & linelbct.ToString).wraptext = True
        '            xlSheet.Range("A" & linelbct.ToString) = "Room Type"
        '            xlSheet.Range("A" & linelbct.ToString).font.bold = True

        '            If ml > rm Then
        '                linelbct = linelbct + ml + 2
        '            Else
        '                linelbct = linelbct + rm + 2
        '            End If

        '            strSqlQry = "select checkinouttype,	fromhours,tohours,case when ISNULL(chargeyesno,0)=0 then 'No' else 'Yes' end,chargetype,percentage,value,condition,isnull(requestbeforedays,'') from view_contracts_checkinout_detail where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rst As New ADODB.Recordset
        '            rst = GetResultAsRecordSet(strSqlQry)
        '            tm = rst.RecordCount
        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rst)



        '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/CheckoutType"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "From"
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & linelbct - 1.ToString) = "To"
        '            xlSheet.Range("D" & linelbct - 1.ToString) = "Charge Y/N"
        '            xlSheet.Range("E" & linelbct - 1.ToString) = "Charge Type"
        '            xlSheet.Range("F" & linelbct - 1.ToString) = "Percentage"
        '            xlSheet.Range("G" & linelbct - 1.ToString) = "Value"
        '            xlSheet.Range("H" & linelbct - 1.ToString) = "Conditions"
        '            xlSheet.Range("I" & linelbct - 1.ToString) = "Requestbeforedays"
        '            xlSheet.Range("I" & linelbct - 1.ToString).wraptext = True
        '            xlSheet.Range("I" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & linelbct - 1.ToString).font.bold = True

        '            strSqlQry = "select ISNULL(datetype,''),ISNULL(restrictdate,'') from view_contracts_checkinout_restricted where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rsd As New ADODB.Recordset
        '            rsd = GetResultAsRecordSet(strSqlQry)
        '            de = rsd.RecordCount

        '            linelbct = linelbct + tm + 2


        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsd)
        '            xlSheet.Range("A" & linelbct - 1.ToString) = "Date Type"
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "No CheckIn-/Out"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            linelbct = linelbct + de + 3


        '        Next
        '    End If




        '    FolderPath = "~\ExcelTemp\"
        '    Dim FileNameNew As String = "OfferPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        '    Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)
        '    'Delete all files from Temp folder
        '    Try

        '        Dim outputPath1 As String = Server.MapPath(FolderPath)
        '        If Directory.Exists(outputPath1) Then

        '            Dim files() As String
        '            files = Directory.GetFileSystemEntries(outputPath1)

        '            For Each element As String In files
        '                If (Not Directory.Exists(element)) Then
        '                    File.Delete(Path.Combine(outputPath1, Path.GetFileName(element)))
        '                End If
        '            Next
        '        End If
        '    Catch ex As Exception

        '    End Try
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


        '    ' Response.Clear()
        '    ' Response.ContentType = "application/vnd.ms-excel"
        '    ' Response.AddHeader("content-disposition", "attachment;filename=" & FileNameNew)
        '    ' Response.WriteFile(outputPath)
        '    ' Response.End()




        '    objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Exported succesfully : ", Session("GlobalUserName"))

        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try

    End Sub
    'Protected Sub btnContractPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContractPrint.Click
    '    Dim xlApp
    '    Dim xlBook
    '    Dim xlSheet



    '    xlApp = Server.CreateObject("Excel.Application")
    '    xlApp.visible = False
    '    Dim FolderPath As String = "..\ExcelTemplates\"
    '    Dim FileName As String = "ContractPrint.xlsx"
    '    Dim FilePath As String = Server.MapPath(FolderPath + FileName)
    '    Dim strConn As String = Session("dbconnectionName")
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & FilePath & "');", True)



    '    Try
    '        xlBook = xlApp.Workbooks.Open(FilePath, True, False)


    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    End Try



    '    xlSheet = xlBook.Worksheets(1) ' Sheet 1
    '    xlSheet.Range("C3").Value = ViewState("hotelname")
    '    xlSheet.Range("C5").Value = hdncontractid.Value

    '    xlSheet = xlBook.Worksheets(2) ' Sheet 2

    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value
    '    xlSheet.Range("A6").Value = "From Date"




    '    Dim dt1 As New DataTable
    '    strSqlQry = "select convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105),applicableto,0 countrygroups from  view_contracts_search where contractid ='" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dt1)

    '    If dt1.Rows.Count > 0 Then
    '        For i As Integer = 0 To dt1.Rows.Count - 1
    '            xlSheet.Range("B6").Value = dt1.Rows(0)(0).ToString
    '            xlSheet.Range("B7").Value = dt1.Rows(0)(1).ToString
    '            xlSheet.Range("B8").Value = dt1.Rows(0)(2).ToString
    '        Next
    '    End If

    '    Dim dt2 As New DataTable
    '    strSqlQry = "select contractid from view_contractcountry where contractid ='" & hdncontractid.Value & "' "
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dt2)

    '    Dim dt As New DataTable
    '    strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_contractcountry  v where v.ctrycode =p.ctrycode  and v.contractid ='" & hdncontractid.Value & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dt)
    '    If dt2.Rows.Count > 0 Then
    '        For i As Integer = 0 To dt.Rows.Count - 1
    '            xlSheet.Range("A12").Value = "Countries"
    '            xlSheet.Range("A14:I14").Merge()
    '            xlSheet.Range("A14").WrapText = True
    '            'xlSheet.Range("A14").rowheight = 400
    '            xlSheet.Range("A14").Value = dt.Rows(i)("Country").ToString
    '        Next

    '    Else
    '        For i As Integer = 0 To dt.Rows.Count - 1

    '            xlSheet.Range("A14").Value = dt.Rows(i)("Country").ToString
    '        Next
    '    End If

    '    strSqlQry = "select seasonname,convert(varchar(10),fromdate , 105)fromdate,convert(varchar(10),todate , 105)todate,MinNight from view_contractseasons where contractid='" & hdncontractid.Value & "' "
    '    'Dim conn As New ADODB.Connection
    '    Dim rs As ADODB.Recordset
    '    'Dim oledbcon As String = ConfigurationManager.ConnectionStrings("strADODBConnection").ConnectionString
    '    'conn.Open(oledbcon)
    '    'rs = conn.Execute(strSqlQry)
    '    rs = GetResultAsRecordSet(strSqlQry)
    '    Dim iLineNo As Integer = 19
    '    xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rs)

    '    xlSheet = xlBook.Worksheets(3)
    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value

    '    strSqlQry = "select tranid from view_contracts_commission_header where contractid = '" & hdncontractid.Value & "'"
    '    Dim rsi As New ADODB.Recordset
    '    rsi = GetResultAsRecordSet(strSqlQry)
    '    Dim iLineNoI As Integer = 10

    '    Dim dt3 As New DataTable
    '    strSqlQry = "select tranid,seasons from view_contracts_commission_header where contractid = '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dt3)
    '    Dim iLine As Integer = 11

    '    Dim iLineNo2 As Integer = 10

    '    If dt3.Rows.Count > 0 Then
    '        For i As Integer = 0 To dt3.Rows.Count - 1
    '            Dim x As Integer
    '            Dim y As Integer
    '            Dim ex As Integer



    '            xlSheet.Range("A" & iLineNo2 - 1.ToString) = "Commission Id"
    '            xlSheet.Range("A" & iLineNo2 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLineNo2 - 1.ToString) = "Formula Name"
    '            xlSheet.Range("B" & iLineNo2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLineNo2 - 1.ToString) = "Formula String"
    '            xlSheet.Range("C" & iLineNo2 - 1.ToString).font.bold = True

    '            If dt3.Rows(i)("seasons") <> "" Then
    '                xlSheet.Range("D" & iLineNo2 - 1.ToString) = "Season Name	"
    '                xlSheet.Range("D" & iLineNo2 - 1.ToString).font.bold = True
    '                xlSheet.Range("E" & iLineNo2 - 1.ToString) = "From Date"
    '                xlSheet.Range("E" & iLineNo2 - 1.ToString).font.bold = True
    '                xlSheet.Range("F" & iLineNo2 - 1.ToString) = "To Date"
    '                xlSheet.Range("F" & iLineNo2 - 1.ToString).font.bold = True
    '            Else
    '                xlSheet.Range("E" & iLineNo2 - 1.ToString) = "From Date"
    '                xlSheet.Range("E" & iLineNo2 - 1.ToString).font.bold = True
    '                xlSheet.Range("F" & iLineNo2 - 1.ToString) = "To Date"
    '                xlSheet.Range("F" & iLineNo2 - 1.ToString).font.bold = True
    '            End If
    '            xlSheet.Range("G" & iLineNo2 - 1.ToString) = "Room Categories"
    '            xlSheet.Range("G" & iLineNo2 - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLineNo2 - 1.ToString) = "Room Types"
    '            xlSheet.Range("H" & iLineNo2 - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLineNo2 - 1.ToString) = "Meal Plans"
    '            xlSheet.Range("I" & iLineNo2 - 1.ToString).font.bold = True
    '            xlSheet.Range("J" & iLineNo2 - 1.ToString) = "Applicable To"
    '            xlSheet.Range("J" & iLineNo2 - 1.ToString).font.bold = True



    '            strSqlQry = "select distinct h.tranid, v.formulaname,formulastring=isnull(stuff((select ';' + c.term1+','+ltrim(rtrim(convert(varchar(20),c.value)))  from view_contracts_commissions c,commissionformula_header h where c.formulaid=h.formulaid  and  c.tranid ='" & dt3.Rows(i)("tranid").ToString & "'    order by c.clineno for xml path('')),1,1,''),'') from view_contracts_commission_header h, view_contracts_commissions c, commissionformula_header v  where h.tranid = c.tranid And c.formulaid = v.formulaid and h.tranid ='" & dt3.Rows(i)("tranid").ToString & "'"
    '            Dim rs1 As New ADODB.Recordset
    '            rs1 = GetResultAsRecordSet(strSqlQry)
    '            ' Dim iLineNo1 As Integer = 10
    '            xlSheet.Range("A" & iLineNo2.ToString).CopyFromRecordset(rs1)
    '            xlSheet.Range("B" & iLineNo2.ToString).wraptext = True
    '            xlSheet.Range("C" & iLineNo2.ToString).wraptext = True
    '            x = rs1.RecordCount

    '            If dt3.Rows(i)("seasons") <> "" Then

    '                strSqlQry = "select cs.Item1 seascode,convert(varchar(10),s.fromdate , 105)fromdate,convert(varchar(10),s.todate , 105)todate from view_contracts_commission_header h"
    '                strSqlQry += " cross apply dbo.SplitString1colsWithOrderField(h.seasons,',') cs "
    '                strSqlQry += " join view_contractseasons s on h.contractid=s.contractid and cs.Item1=s.SeasonName "
    '                strSqlQry += " where h.contractid ='" & hdncontractid.Value & "' and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(cs.Item1,'')<>'' order by seascode,fromdate "


    '                Dim rs2 As New ADODB.Recordset
    '                rs2 = GetResultAsRecordSet(strSqlQry)
    '                xlSheet.Range("D" & iLineNo2.ToString).CopyFromRecordset(rs2)
    '                y = rs2.RecordCount
    '            Else


    '                strSqlQry = "SELECT fromdate,todate from view_contracts_commission_detail WHERE tranid='" & dt3.Rows(i)("tranid").ToString & "' "
    '                Dim rssd As ADODB.Recordset
    '                rssd = GetResultAsRecordSet(strSqlQry)

    '                ex = rssd.RecordCount
    '                xlSheet.Range("E" & iLineNo2.ToString).CopyFromRecordset(rssd)
    '            End If
    '            strSqlQry = "select distinct prm.rmtypname,h.mealplans,h.applicableto from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s  join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>''"
    '            Dim rs3 As ADODB.Recordset
    '            rs3 = GetResultAsRecordSet(strSqlQry)

    '            xlSheet.Range("H" & iLineNo2.ToString).CopyFromRecordset(rs3)
    '            Dim z As Integer
    '            z = rs3.RecordCount


    '            strSqlQry = "select r.rmcatname   from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join view_contracts_search ch on h.contractid=ch.contractid join rmcatmast r on s.Item1=r.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>'' "
    '            Dim rs4 As ADODB.Recordset
    '            rs4 = GetResultAsRecordSet(strSqlQry)

    '            xlSheet.Range("G" & iLineNo2.ToString).CopyFromRecordset(rs4)
    '            Dim d As Integer
    '            d = rs4.RecordCount



    '            If x > y And x > z And x > d And x > ex Then
    '                iLineNo2 = iLineNo2 + x + 2
    '            ElseIf y > x And y > z And y > d And y > ex Then
    '                iLineNo2 = iLineNo2 + y + 2
    '            ElseIf z > x And z > y And z > d And z > ex Then
    '                iLineNo2 = iLineNo2 + z + 2
    '            ElseIf d > x And d > y And d > z And d > ex Then
    '                iLineNo2 = iLineNo2 + d + 2
    '            ElseIf ex > x And ex > y And ex > z And ex > d Then
    '                iLineNo2 = iLineNo2 + ex + 2
    '            Else
    '                iLineNo2 = iLineNo2 + d + 2
    '            End If



    '        Next
    '    End If

    '    xlSheet = xlBook.Worksheets(4)

    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value
    '    Dim dtmx As New DataTable


    '    strSqlQry = "select distinct tranid from view_partymaxacc_header where partycode= '" & CType(Session("Contractparty"), String) & "' "
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtmx)


    '    Dim iLinmx As Integer = 9

    '    Dim em1 As Integer
    '    Dim em As Integer
    '    Dim em2 As Integer



    '    Dim dt23 As New DataTable
    '    If dtmx.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtmx.Rows.Count - 1

    '            xlSheet.Range("A" & iLinmx - 1.ToString) = "Max Occ.ID"
    '            xlSheet.Range("B" & iLinmx - 1.ToString) = "Room Name"
    '            xlSheet.Range("A" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLinmx - 1.ToString).font.bold = True

    '            xlSheet.Range("C" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLinmx - 1.ToString).font.bold = True

    '            xlSheet.Range("F" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("J" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("K" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("L" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("M" & iLinmx - 1.ToString).font.bold = True
    '            xlSheet.Range("N" & iLinmx - 1.ToString).font.bold = True


    '            xlSheet.Range("C" & iLinmx - 1.ToString) = "Room Classification"
    '            xlSheet.Range("D" & iLinmx - 1.ToString) = "unit yes/no	"
    '            xlSheet.Range("E" & iLinmx - 1.ToString) = "Price Adult Occupancy only for Unit"
    '            xlSheet.Range("E" & iLinmx - 1.ToString).wraptext = True
    '            xlSheet.Range("F" & iLinmx - 1.ToString) = "Price Pax"
    '            xlSheet.Range("G" & iLinmx - 1.ToString) = "Max Adults"
    '            xlSheet.Range("H" & iLinmx - 1.ToString) = "Max Child"
    '            xlSheet.Range("I" & iLinmx - 1.ToString) = "Max Infant"
    '            xlSheet.Range("J" & iLinmx - 1.ToString) = "Max EB"

    '            xlSheet.Range("K" & iLinmx - 1.ToString) = "Max Total Occupancy without infant"
    '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
    '            xlSheet.Range("L" & iLinmx - 1.ToString) = "Rank Order"
    '            xlSheet.Range("N" & iLinmx - 1.ToString) = "Start with 0 based"
    '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
    '            xlSheet.Range("M" & iLinmx - 1.ToString) = "Occupancy Combinations"




    '            Dim dtmx1 As New DataTable
    '            strSqlQry = "select rmtypcode from view_partymaxaccomodation where partycode= '" & CType(Session("Contractparty"), String) & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(dtmx1)


    '            strSqlQry = "select distinct m.tranid,prm.rmtypname,rc.roomclassname, prm.unityesno,m.pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb,m.noofextraperson, m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
    '            Dim rsmx As New ADODB.Recordset
    '            rsmx = GetResultAsRecordSet(strSqlQry)
    '            em = rsmx.RecordCount

    '            xlSheet.Range("A" & iLinmx.ToString).CopyFromRecordset(rsmx)
    '            If conn1.State = ConnectionState.Open Then
    '                conn1.Close()
    '            End If
    '            Dim rsmx1 As New ADODB.Recordset
    '            If dtmx1.Rows.Count > 0 Then
    '                For idt As Integer = 0 To dtmx1.Rows.Count - 1



    '                    strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "

    '                    rsmx1 = GetResultAsRecordSet(strSqlQry)
    '                    em1 = rsmx1.RecordCount
    '                    xlSheet.Range("M" & iLinmx.ToString).CopyFromRecordset(rsmx1)

    '                    iLinmx = iLinmx + 1
    '                    '        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '                    '        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)

    '                    '        iLinmx += 1
    '                    '        myDataAdapter.Fill(dt23)
    '                    '    Next


    '                    'End If
    '                    'Dim rsmx1 As New ADODB.Recordset
    '                    'rsmx1 = convertToADODB(dt23)
    '                    'iLinmx = iLinmx - dtmx1.Rows.Count
    '                    ''Dim name(dtt.Columns.Count) As String
    '                    'Dim ii As Integer = 65
    '                    ''For Each column As DataColumn In dt.Columns
    '                    ''name(ii) = column.ColumnName

    '                    'Dim sss = Chr(ii).ToString
    '                    'For OO As Integer = 0 To dtt.Columns.Count - 1
    '                    '    xlSheet.Range(sss.ToString() + iLinmr.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
    '                    '    ii += 1
    '                    '    xlSheet.Range(sss.ToString() + (iLinmr).ToString).FONT.BOLD = True
    '                    '    sss = Chr(ii).ToString

    '                Next

    '            End If



    '            strSqlQry = "select  start0based from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
    '            Dim rsmx2 As New ADODB.Recordset
    '            rsmx2 = GetResultAsRecordSet(strSqlQry)
    '            em2 = rsmx2.RecordCount
    '            iLinmx = iLinmx - dtmx1.Rows.Count

    '            xlSheet.Range("N" & iLinmx.ToString).CopyFromRecordset(rsmx2)




    '            If em1 > em And em1 > em2 Then
    '                iLinmx = iLinmx + em1 + 4
    '            ElseIf em > em1 And em > em2 Then
    '                iLinmx = iLinmx + em + 4
    '            Else
    '                iLinmx = iLinmx + em2 + 4
    '            End If







    '        Next

    '    End If

    '    xlSheet = xlBook.Worksheets(5)

    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value

    '    Dim dtrr2 As New DataTable
    '    strSqlQry = "select plistcode from view_cplisthnew  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtrr2)
    '    Dim iLine2 As Integer = 8
    '    Dim ei2 As Integer
    '    Dim ei3 As Integer
    '    Dim ei4 As Integer
    '    If dtrr2.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtrr2.Rows.Count - 1


    '            strSqlQry = "select plistcode,applicableto from  view_cplisthnew where plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
    '            Dim rse2 As New ADODB.Recordset
    '            rse2 = GetResultAsRecordSet(strSqlQry)
    '            ei2 = rse2.RecordCount
    '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse2)
    '            xlSheet.Range("A" & iLine2 - 1.ToString) = "PriceList Code"
    '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine2 - 1.ToString) = "Aplicable to"
    '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True


    '            strSqlQry = "select  c.subseascode,d.fromdate fromdate,d.todate todate from view_cplisthnew c,view_contractseasons d  where c.subseascode=d.SeasonName and c.plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and d.contractid='" & hdncontractid.Value & "'"
    '            Dim rse32 As New ADODB.Recordset
    '            rse32 = GetResultAsRecordSet(strSqlQry)
    '            ei4 = rse32.RecordCount

    '            iLine2 = iLine2 + 1

    '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse32)
    '            xlSheet.Range("A" & iLine2 - 1.ToString) = "Season"
    '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine2 - 1.ToString) = "From Date"
    '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLine2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLine2 - 1.ToString) = "To Date"

    '            Dim fromrange As Integer, torange As Integer
    '            fromrange = iLine2
    '            torange = IIf(rse32.RecordCount > 0, iLine2 + rse32.RecordCount, iLine2)

    '            xlSheet.Range("B" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
    '            xlSheet.Range("C" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"

    '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_cplisthnew_weekdays   where  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' for xml path('')),1,1,''),'') "

    '            Dim rsw2 As New ADODB.Recordset
    '            rsw2 = GetResultAsRecordSet(strSqlQry)
    '            iLine2 = iLine2 + ei4 + 2
    '            xlSheet.Range("B" & iLine2.ToString).CopyFromRecordset(rsw2)
    '            xlSheet.Range("B" & iLine2.ToString).wraptext = True
    '            xlSheet.Range("B" & iLine2 & ":" & "C" & iLine2).Merge()
    '            xlSheet.Range("B" & iLine2.ToString).rowheight = 30
    '            xlSheet.Range("A" & iLine2.ToString) = "Days of the week"
    '            xlSheet.Range("A" & iLine2.ToString).font.bold = True
    '            If conn1.State = ConnectionState.Open Then
    '                conn1.Close()
    '            End If



    '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
    '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
    '            Dim dtt As New DataTable
    '            Using con As New SqlConnection(constring)
    '                Using cmd1 As New SqlCommand("[sp_print_roomrates]")
    '                    cmd1.CommandType = CommandType.StoredProcedure
    '                    cmd1.Parameters.AddWithValue("@plistcode", dtrr2.Rows(i)("plistcode").ToString)

    '                    Using sda As New SqlDataAdapter()
    '                        cmd1.Connection = con
    '                        sda.SelectCommand = cmd1

    '                        sda.Fill(dtt)

    '                    End Using
    '                End Using
    '            End Using


    '            Dim rssp72 As New ADODB.Recordset
    '            rssp72 = convertToADODB(dtt)
    '            Dim ii3 As Integer = 65
    '            'For Each column As DataColumn In dt.Columns
    '            'name(ii) = column.ColumnName
    '            iLine2 = iLine2 + 2
    '            Dim sss3 = Chr(ii3).ToString
    '            For OO As Integer = 0 To dtt.Columns.Count - 1
    '                xlSheet.Range(sss3.ToString() + iLine2.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
    '                ii3 += 1
    '                xlSheet.Range(sss3.ToString() + (iLine2).ToString).FONT.BOLD = True
    '                sss3 = Chr(ii3).ToString
    '            Next

    '            If rssp72.RecordCount > 0 Then
    '                iLine2 = iLine2 + 1
    '                xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rssp72)
    '                ei3 = rssp72.RecordCount

    '                fromrange = iLine2
    '                torange = IIf(rssp72.RecordCount > 0, iLine2 + rssp72.RecordCount, iLine2)
    '                xlSheet.Range("C" & fromrange.ToString & ":" & "C" & torange.ToString).NumberFormat = "####"
    '            End If

    '            iLine2 = iLine2 + ei3 + 3

    '        Next

    '    End If

    '    xlSheet = xlBook.Worksheets(6)
    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value

    '    Dim dte As New DataTable
    '    strSqlQry = "select exhibitionid from view_contracts_exhibition_header  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dte)
    '    Dim iLinee As Integer = 8
    '    Dim ei As Integer
    '    Dim ze As Integer
    '    If dte.Rows.Count > 0 Then
    '        For i As Integer = 0 To dte.Rows.Count - 1
    '            xlSheet.Range("A" & iLinee - 1.ToString) = "Exhibition Id"
    '            xlSheet.Range("B" & iLinee - 1.ToString) = "Applicable To"
    '            xlSheet.Range("C" & iLinee - 1.ToString) = "Exhibition Name"
    '            xlSheet.Range("D" & iLinee - 1.ToString) = "From Date"
    '            xlSheet.Range("E" & iLinee - 1.ToString) = "To Date"
    '            xlSheet.Range("A" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLinee - 1.ToString) = "Room Type"
    '            xlSheet.Range("G" & iLinee - 1.ToString) = "Meal Plan"
    '            xlSheet.Range("H" & iLinee - 1.ToString) = "Supplement Amount"
    '            xlSheet.Range("H" & iLinee - 1.ToString).wraptext = True
    '            xlSheet.Range("I" & iLinee - 1.ToString) = "Min Stay"
    '            xlSheet.Range("B" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLinee - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLinee - 1.ToString).font.bold = True


    '            strSqlQry = "select h.exhibitionid,h.applicableto, e.exhibitionname,d.fromdate,d.todate from view_contracts_exhibition_detail d join exhibition_master e on d.exhibitioncode=e.exhibitioncode join  view_contracts_exhibition_header h on d.exhibitionid=h.exhibitionid and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
    '            Dim rse As New ADODB.Recordset
    '            rse = GetResultAsRecordSet(strSqlQry)
    '            ei = rse.RecordCount
    '            xlSheet.Range("A" & iLinee.ToString).CopyFromRecordset(rse)

    '            strSqlQry = "select distinct mealplans,supplementvalue,minstay from view_contracts_exhibition_detail where exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
    '            Dim rsr As New ADODB.Recordset
    '            rsr = GetResultAsRecordSet(strSqlQry)
    '            ze = rsr.RecordCount
    '            xlSheet.Range("G" & iLinee.ToString).CopyFromRecordset(rsr)

    '            Dim dter As New DataTable
    '            strSqlQry = "select distinct roomtypes,exhibitioncode from view_contracts_exhibition_detail  where  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(dter)



    '            Dim x As Integer
    '            Dim y As Integer
    '            If dter.Rows.Count > 0 Then
    '                For er As Integer = 0 To dter.Rows.Count - 1


    '                    If dter.Rows(er)("roomtypes").ToString = "All" Then


    '                        strSqlQry = "select roomtypes from view_contracts_exhibition_detail  where  roomtypes='All' and exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
    '                        xlSheet.Range("F" & iLinee.ToString) = "All"
    '                        iLinee = iLinee + 1

    '                    ElseIf dter.Rows(er)("roomtypes").ToString <> "All" Then



    '                        strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "' and d.exhibitioncode='" & dter.Rows(er)("exhibitioncode").ToString & "' for xml path('')),1,1,''),'') "
    '                        Dim rser As New ADODB.Recordset
    '                        rser = GetResultAsRecordSet(strSqlQry)
    '                        y = rser.RecordCount
    '                        iLinee = iLinee
    '                        xlSheet.Range("F" & iLinee.ToString).CopyFromRecordset(rser)
    '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        iLinee = iLinee + y
    '                    End If

    '                Next

    '            End If



    '            If ei > ze And ei > y And ei > x Then

    '                iLinee = iLinee + ei + 1
    '            ElseIf y > ze And y > ei And y > x Then

    '                iLinee = iLinee + y + 1
    '            ElseIf ze > y And ze > ei And ze > x Then
    '                iLinee = iLinee + ze + 1
    '            ElseIf x > y And x > ei And x > ze Then
    '                iLinee = iLinee + x + 2

    '            Else
    '                iLinee = iLinee + ze + 1

    '            End If
    '        Next
    '    End If
    '    xlSheet = xlBook.Worksheets(7)


    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value

    '    Dim dtmr As New DataTable
    '    strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtmr)
    '    Dim iLinmr As Integer = 8
    '    Dim m7 As Integer
    '    Dim s7 As Integer
    '    Dim e7 As Integer
    '    Dim w7 As Integer
    '    Dim mn7 As Integer
    '    Dim tm7 As Integer
    '    If dtmr.Rows.Count > 0 Then
    '        ' Dim conn As New ADODB.Connection
    '        For i As Integer = 0 To dtmr.Rows.Count - 1

    '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Supplement ID"
    '            xlSheet.Range("B" & iLinmr - 1.ToString) = "Applicable To"
    '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
    '            strSqlQry = "select mealsupplementid,applicableto from view_contracts_mealsupp_header where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
    '            Dim rsm7 As New ADODB.Recordset
    '            rsm7 = GetResultAsRecordSet(strSqlQry)
    '            m7 = rsm7.RecordCount
    '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rsm7)

    '            strSqlQry = "select distinct q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_mealsupp_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"

    '            Dim rscm As New ADODB.Recordset
    '            rscm = GetResultAsRecordSet(strSqlQry)
    '            tm7 = rscm.RecordCount


    '            iLinmr = iLinmr + m7 + 3


    '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rscm)
    '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Season"
    '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLinmr - 1.ToString) = "From Date"
    '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLinmr - 1.ToString) = "To Date"
    '            xlSheet.Range("C" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinmr - 1.ToString) = "Manual From Date not linked to Seasons"
    '            xlSheet.Range("E" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinmr - 1.ToString).WRAPTEXT = True
    '            xlSheet.Range("E" & iLinmr - 1.ToString) = "Manual To Date not linked to Seasons"
    '            xlSheet.Range("E" & iLinmr - 1.ToString).WRAPTEXT = True
    '            xlSheet.Range("F" & iLinmr - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLinmr - 1.ToString) = "Excluded From Date"
    '            xlSheet.Range("G" & iLinmr - 1.ToString).font.bold = True
    '            'xlSheet.Range("G" & iLinmr - 1.ToString).WRAPTEXT = True
    '            xlSheet.Range("E" & iLinmr - 1.ToString).WRAPTEXT = True
    '            xlSheet.Range("G" & iLinmr - 1.ToString) = "Excluded To Date"


    '            strSqlQry = "select  convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105) from  view_contracts_mealsupp_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
    '            Dim rsmc As New ADODB.Recordset
    '            rsmc = GetResultAsRecordSet(strSqlQry)
    '            mn7 = rsmc.RecordCount

    '            iLinmr = iLinmr

    '            xlSheet.Range("D" & iLinmr.ToString).CopyFromRecordset(rsmc)

    '            strSqlQry = " select  convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105) from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
    '            Dim rsed As New ADODB.Recordset
    '            rsed = GetResultAsRecordSet(strSqlQry)
    '            e7 = rsed.RecordCount
    '            iLinmr = iLinmr
    '            xlSheet.Range("F" & iLinmr.ToString).CopyFromRecordset(rsed)

    '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_mealsupp_weekdays  where  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' for xml path('')),1,1,''),'') "

    '            Dim rsw As New ADODB.Recordset
    '            rsw = GetResultAsRecordSet(strSqlQry)

    '            w7 = rsw.RecordCount
    '            If tm7 > m7 And tm7 > mn7 Then
    '                iLinmr = iLinmr + tm7 + 1
    '            ElseIf m7 > tm7 And m7 > mn7 Then
    '                iLinmr = iLinmr + m7 + 1
    '            ElseIf mn7 > m7 And mn7 > mn7 Then
    '                iLinmr = iLinmr + mn7 + 1
    '            Else
    '                iLinmr = iLinmr + tm7 + 1
    '            End If
    '            xlSheet.Range("B" & iLinmr.ToString).CopyFromRecordset(rsw)
    '            xlSheet.Range("B" & iLinmr.ToString).wraptext = True
    '            xlSheet.Range("B" & iLinmr & ":" & "C" & iLinmr).Merge()
    '            xlSheet.Range("A" & iLinmr.ToString) = "Days of the week"
    '            xlSheet.Range("A" & iLinmr.ToString).font.bold = True
    '            If conn1.State = ConnectionState.Open Then
    '                conn1.Close()
    '            End If


    '            'If conn.State = ConnectionState.Open Then
    '            '    conn.Close()
    '            'End If

    '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
    '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
    '            Dim dtt As New DataTable
    '            Using con As New SqlConnection(constring)
    '                Using cmd1 As New SqlCommand("[sp_print_mealsupplements]")
    '                    cmd1.CommandType = CommandType.StoredProcedure
    '                    cmd1.Parameters.AddWithValue("@mealsupplementid", dtmr.Rows(i)("mealsupplementid").ToString)

    '                    Using sda As New SqlDataAdapter()
    '                        cmd1.Connection = con
    '                        sda.SelectCommand = cmd1

    '                        sda.Fill(dtt)
    '                        'If dtt.Rows(i)(i) = "-3" Then
    '                        '    "Free"

    '                        '    "Incl"
    '                        '    txt.Text = "-1"
    '                        '    Case "N.Incl"
    '                        '    txt.Text = "-2"
    '                        '    Case "N/A"
    '                        '    txt.Text = "-4"
    '                        '    Case "On Request"
    '                        '    txt.Text = "-5"

    '                    End Using
    '                End Using
    '            End Using




    '            Dim rssp7 As New ADODB.Recordset
    '            rssp7 = convertToADODB(dtt)
    '            iLinmr = iLinmr + w7 + 1


    '            'Dim name(dtt.Columns.Count) As String
    '            Dim ii As Integer = 65
    '            'For Each column As DataColumn In dt.Columns
    '            'name(ii) = column.ColumnName

    '            Dim sss = Chr(ii).ToString
    '            For OO As Integer = 0 To dtt.Columns.Count - 1
    '                xlSheet.Range(sss.ToString() + iLinmr.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
    '                ii += 1
    '                xlSheet.Range(sss.ToString() + (iLinmr).ToString).FONT.BOLD = True
    '                sss = Chr(ii).ToString
    '            Next





    '            'Next


    '            If rssp7.RecordCount > 0 Then
    '                iLinmr = iLinmr + 1
    '                xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rssp7)
    '                s7 = rssp7.RecordCount

    '            End If


    '            iLinmr = iLinmr + s7 + 3

    '        Next

    '    End If
    '    xlSheet = xlBook.Worksheets(8)
    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value

    '    Dim dtcpi As New DataTable
    '    strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtcpi)
    '    Dim iLine8 As Integer = 7

    '    If dtcpi.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtcpi.Rows.Count - 1

    '            xlSheet.Range("A" & iLine8 - 1.ToString) = "ChildPolicy Id"
    '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine8 - 1.ToString) = "Applicable To"

    '            Dim ml8 As Integer
    '            Dim tm8 As Integer
    '            Dim co8 As Integer
    '            Dim d8 As Integer
    '            Dim chk8 As Integer
    '            Dim ei31 As Integer

    '            strSqlQry = "select childpolicyid,applicableto from view_contracts_childpolicy_header  where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
    '            Dim rs8 As New ADODB.Recordset
    '            rs8 = GetResultAsRecordSet(strSqlQry)
    '            chk8 = rs8.RecordCount
    '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rs8)
    '            'xlSheet.Range("A" & iLine8.ToString) = ""

    '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_childpolicy_weekdays where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' for xml path('')),1,1,''),'') "
    '            Dim rss8 As New ADODB.Recordset
    '            rss8 = GetResultAsRecordSet(strSqlQry)

    '            iLine8 = iLine8 + chk8 + 1
    '            xlSheet.Range("A" & iLine8.ToString) = "Days of the week"
    '            xlSheet.Range("A" & iLine8.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine8.ToString).CopyFromRecordset(rss8)
    '            xlSheet.Range("B" & iLine8.ToString).WrapText = True
    '            xlSheet.Range("B" & iLine8 & ":" & "D" & iLine8).Merge()
    '            xlSheet.Range("A" & iLine8.ToString).rowheight = "35"

    '            strSqlQry = "select distinct q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_childpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
    '            Dim rscp As New ADODB.Recordset
    '            rscp = GetResultAsRecordSet(strSqlQry)
    '            tm8 = rscp.RecordCount

    '            iLine8 = iLine8 + 2




    '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rscp)
    '            xlSheet.Range("A" & iLine8 - 1.ToString) = "Season"
    '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine8 - 1.ToString) = "From Date"
    '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLine8 - 1.ToString) = "To Date"
    '            xlSheet.Range("C" & iLine8 - 1.ToString).font.bold = True

    '            xlSheet.Range("D" & iLine8 - 1.ToString) = "Date Manual From Date not linked to Seasons"
    '            xlSheet.Range("D" & iLine8 - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLine8 - 1.ToString).WrapText = True


    '            xlSheet.Range("E" & iLine8 - 1.ToString) = "Manual To Date not linked to Seasons"
    '            xlSheet.Range("E" & iLine8 - 1.ToString).WrapText = True
    '            xlSheet.Range("E" & iLine8 - 1.ToString).font.bold = True

    '            strSqlQry = "select convert(varchar(10),fromdate , 105) ,convert(varchar(10),todate , 105) from  view_contracts_childpolicy_dates where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
    '            Dim rsm8 As New ADODB.Recordset
    '            rsm8 = GetResultAsRecordSet(strSqlQry)
    '            co8 = rsm8.RecordCount
    '            iLine8 = iLine8 + 1
    '            xlSheet.Range("D" & iLine8.ToString).CopyFromRecordset(rsm8)

    '            If conn1.State = ConnectionState.Open Then
    '                conn1.Close()
    '            End If



    '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
    '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
    '            Dim dtt As New DataTable
    '            Using con As New SqlConnection(constring)
    '                Using cmd1 As New SqlCommand("[sp_print_childpolicy]")
    '                    cmd1.CommandType = CommandType.StoredProcedure
    '                    cmd1.Parameters.AddWithValue("@childpolicyid", dtcpi.Rows(i)("childpolicyid").ToString)

    '                    Using sda As New SqlDataAdapter()
    '                        cmd1.Connection = con
    '                        sda.SelectCommand = cmd1

    '                        sda.Fill(dtt)

    '                    End Using
    '                End Using
    '            End Using


    '            Dim rssp721 As New ADODB.Recordset
    '            rssp721 = convertToADODB(dtt)
    '            If rssp721.RecordCount > 0 Then
    '                If chk8 > tm8 And chk8 > co8 Then
    '                    iLine8 = iLine8 + chk8 + 1
    '                ElseIf co8 > tm8 And co8 > chk8 Then
    '                    iLine8 = iLine8 + co8 + 1
    '                Else
    '                    iLine8 = iLine8 + tm8 + 1
    '                End If
    '                Dim ii2 As Integer = 65
    '                'For Each column As DataColumn In dt.Columns
    '                'name(ii) = column.ColumnName

    '                Dim sss2 = Chr(ii2).ToString

    '                For OO As Integer = 0 To dtt.Columns.Count - 1
    '                    xlSheet.Range(sss2.ToString() + iLine8.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
    '                    ii2 += 1
    '                    xlSheet.Range(sss2.ToString() + (iLine8).ToString).FONT.BOLD = True
    '                    sss2 = Chr(ii2).ToString

    '                Next


    '                iLine8 = iLine8 + 1

    '                xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rssp721)
    '                ei31 = rssp721.RecordCount

    '            End If

    '            iLine8 = iLine8 + ei31 + 4

    '        Next

    '    End If

    '    xlSheet = xlBook.Worksheets(9)
    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value

    '    Dim dtcn As New DataTable
    '    strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtcn)
    '    Dim iLinecr2 As Integer = 8


    '    If dtcn.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtcn.Rows.Count - 1

    '            Dim rm2 As Integer
    '            Dim ml2 As Integer

    '            Dim co2 As Integer
    '            Dim ce2 As Integer

    '            Dim ns As Integer
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Cancellation ID"
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Applicable To"
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "Season"
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Pricelist From Date"
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Pricelist To Date"
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & iLinecr2 - 1.ToString) = "Exhibition Name"
    '            xlSheet.Range("G" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLinecr2 - 1.ToString) = "From"
    '            xlSheet.Range("H" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLinecr2 - 1.ToString) = "To"
    '            xlSheet.Range("I" & iLinecr2 - 1.ToString).font.bold = True


    '            strSqlQry = "select distinct d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & hdncontractid.Value & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
    '            Dim rscp As New ADODB.Recordset
    '            rscp = GetResultAsRecordSet(strSqlQry)
    '            co2 = rscp.RecordCount
    '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)

    '            strSqlQry = "select d.exhibitioncode,m.exhibitionname,d.fromdate,d.todate from view_contracts_cancelpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.exhibitions,',') e inner join view_contracts_exhibition_detail d on e.Item1=d.exhibitioncode inner join view_contracts_exhibition_header eh on d.exhibitionid=eh.exhibitionid and eh.contractid=h.contractid inner join exhibition_master m on d.exhibitioncode=m.exhibitioncode where h.contractid='" & hdncontractid.Value & "' and h.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
    '            Dim rs2e As New ADODB.Recordset
    '            rs2e = GetResultAsRecordSet(strSqlQry)
    '            ce2 = rs2e.RecordCount
    '            xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rs2e)


    '            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_header  h join view_contracts_search v on h.contractid =v.contractid join view_contracts_cancelpolicy_detail d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'  for xml path('')),1,1,''),'') "
    '            Dim rs2r As New ADODB.Recordset
    '            rs2r = GetResultAsRecordSet(strSqlQry)
    '            rm2 = rs2r.RecordCount
    '            If ce2 Or co2 <> 0 Then
    '                If ce2 > co2 Then
    '                    iLinecr2 = iLinecr2 + ce2 + 2
    '                ElseIf co2 > ce2 Then
    '                    iLinecr2 = iLinecr2 + co2 + 2
    '                End If
    '            Else

    '                iLinecr2 = iLinecr2 + 4
    '            End If
    '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rs2r)
    '            xlSheet.Range("A" & iLinecr2.ToString).wraptext = True
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True

    '            strSqlQry = "select mealplans,nodayshours, dayshours,chargebasis, percentagetocharge from view_contracts_cancelpolicy_detail where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

    '            Dim rsr2 As New ADODB.Recordset
    '            rsr2 = GetResultAsRecordSet(strSqlQry)
    '            ml2 = rsr2.RecordCount
    '            If rm2 <> 0 Then

    '                'If ce2 Or co2 <> 0 Then
    '                '    If ce2 > co2 Then
    '                '        iLinecr2 = iLinecr2
    '                '    ElseIf co2 > ce2 Then
    '                '        iLinecr2 = iLinecr2
    '                '    End If
    '                'Else

    '                iLinecr2 = iLinecr2

    '            ElseIf ce2 Or co2 <> 0 Then
    '                If ce2 > co2 Then
    '                    iLinecr2 = iLinecr2 + ce2 + 2
    '                ElseIf co2 > ce2 Then
    '                    iLinecr2 = iLinecr2 + co2 + 2
    '                End If
    '            Else
    '                iLinecr2 = iLinecr2 + 2
    '            End If
    '            xlSheet.Range("B" & iLinecr2.ToString).CopyFromRecordset(rsr2)
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Meal Plan"
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "No.of Days or Hours"
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Unit -Days/Hours"
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Charge Basis"
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLinecr2 - 1.ToString) = "Percentage to charge"
    '            xlSheet.Range("F" & iLinecr2 - 1.ToString).font.bold = True
    '            'strSqlQry = "select  distinct p.rmtypname from view_contracts_cancelpolicy_noshowearly  h  join view_contracts_cancelpolicy_detail  d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =d.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
    '            'Dim rsr4 As New ADODB.Recordset
    '            'rsr4 = GetResultAsRecordSet(strSqlQry)
    '            'rm2 = rsr4.RecordCount
    '            'If ce2 Or co2 <> 0 Then
    '            '    If ce2 > co2 Then
    '            '        iLinecr2 = iLinecr2 + ce2 + 2
    '            '    ElseIf co2 > ce2 Then
    '            '        iLinecr2 = iLinecr2 + co2 + 2
    '            '    End If
    '            'Else
    '            '    iLinecr2 = iLinecr2 + 3
    '            'End If
    '            'xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rsr4)

    '            strSqlQry = "select noshowearly from view_contracts_cancelpolicy_noshowearly  where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"


    '            Dim dtns As New DataTable
    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(dtns)
    '            iLinecr2 = iLinecr2 + 3
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
    '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "No Show/Early Checkout"
    '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Charge Basis"
    '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Percentage to charge"
    '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "Meal Plan"
    '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True


    '            If dtns.Rows.Count > 0 Then
    '                For i2 As Integer = 0 To dtns.Rows.Count - 1

    '                    strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),''),d.noshowearly, mealplans,chargebasis, percentagetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
    '                    Dim rsrns4 As New ADODB.Recordset
    '                    rsrns4 = GetResultAsRecordSet(strSqlQry)
    '                    ns = rsrns4.RecordCount
    '                    xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rsrns4)
    '                    xlSheet.Range("A" & iLinecr2.ToString).WrapText = True
    '                    iLinecr2 = iLinecr2 + 1
    '                Next
    '            End If
    '            If ml2 Or rm2 <> 0 Then
    '                If ml2 > rm2 Then
    '                    iLinecr2 = iLinecr2 + ml2 + 2

    '                Else
    '                    iLinecr2 = iLinecr2 + rm2 + 2
    '                End If
    '            Else

    '                iLinecr2 = iLinecr2 + 2 + rm2

    '            End If


    '            iLinecr2 = iLinecr2 + ns + 3

    '        Next
    '    End If




    '    xlSheet = xlBook.Worksheets(10)
    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value



    '    Dim dtc As New DataTable
    '    strSqlQry = "select checkinoutpolicyid from view_contracts_checkinout_header  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtc)
    '    Dim linelbct As Integer = 8

    '    If dtc.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtc.Rows.Count - 1

    '            Dim rm As Integer
    '            Dim ml As Integer
    '            Dim tm As Integer
    '            Dim co As Integer
    '            Dim de As Integer
    '            Dim chk As Integer



    '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/OutPolicyId"
    '            xlSheet.Range("C" & linelbct - 1.ToString) = "Season"
    '            xlSheet.Range("B" & linelbct - 1.ToString) = "Applicable To"
    '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & linelbct - 1.ToString) = "Pricelist From Date"
    '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & linelbct - 1.ToString) = "Pricelist To Date	"
    '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & linelbct - 1.ToString).font.bold = True

    '            strSqlQry = "select distinct d.checkinoutpolicyid,d.applicableto,q.Item1 seasons, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105)  from view_contracts_checkinout_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q  join  view_contractseasons s on s.SeasonName=q.Item1 and  s.contractid= '" & hdncontractid.Value & "' and  d.checkinoutpolicyid  ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' "
    '            Dim rscp As New ADODB.Recordset
    '            rscp = GetResultAsRecordSet(strSqlQry)
    '            chk = rscp.RecordCount

    '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rscp)



    '            strSqlQry = "select checkintime,checkouttime from view_contracts_checkinout_header where checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
    '            Dim rsc As New ADODB.Recordset
    '            rsc = GetResultAsRecordSet(strSqlQry)
    '            co = rsc.RecordCount
    '            If chk = 0 Then
    '                linelbct = linelbct + 2
    '            Else
    '                linelbct = linelbct + chk + 2
    '            End If

    '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsc)
    '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn Time"
    '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & linelbct - 1.ToString) = "CheckOut Time"
    '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True


    '            strSqlQry = "select isnull(stuff((select ',' + mealcode from view_contracts_checkinout_mealplans where  checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' for xml path('')),1,1,''),'') "
    '            Dim rscm As New ADODB.Recordset
    '            rscm = GetResultAsRecordSet(strSqlQry)
    '            ml = rscm.RecordCount
    '            If co = 0 Then
    '                linelbct = linelbct + 1
    '            Else
    '                linelbct = linelbct + co + 1
    '            End If
    '            xlSheet.Range("B" & linelbct.ToString).CopyFromRecordset(rscm)
    '            xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
    '            'xlSheet.Range("B:C").Merge()

    '            xlSheet.Range("A" & linelbct.ToString) = "Meal Plan"
    '            xlSheet.Range("A" & linelbct.ToString).font.bold = True
    '            strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "
    '            Dim rscr As New ADODB.Recordset
    '            rscr = GetResultAsRecordSet(strSqlQry)
    '            rm = rscr.RecordCount

    '            linelbct = linelbct + 1

    '            xlSheet.Range("B" & linelbct.ToString).CopyFromRecordset(rscr)
    '            'xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
    '            'xlSheet.Range("e:C").Merge()
    '            xlSheet.Range("B" & linelbct.ToString).wraptext = True
    '            xlSheet.Range("A" & linelbct.ToString) = "Room Type"
    '            xlSheet.Range("A" & linelbct.ToString).font.bold = True

    '            strSqlQry = "select checkinouttype,	fromhours,tohours,chargeyesno,chargetype,percentage,condition,requestbeforedays from view_contracts_checkinout_detail where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
    '            Dim rst As New ADODB.Recordset
    '            rst = GetResultAsRecordSet(strSqlQry)
    '            tm = rst.RecordCount

    '            linelbct = linelbct + 3

    '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/CheckoutType"
    '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & linelbct - 1.ToString) = "From"
    '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & linelbct - 1.ToString) = "To"
    '            xlSheet.Range("D" & linelbct - 1.ToString) = "Charge Y/N"
    '            xlSheet.Range("E" & linelbct - 1.ToString) = "Charge Type"
    '            xlSheet.Range("F" & linelbct - 1.ToString) = "Percentage"
    '            xlSheet.Range("G" & linelbct - 1.ToString) = "Conditions"
    '            xlSheet.Range("H" & linelbct - 1.ToString) = "Requestbeforedays"
    '            xlSheet.Range("H" & linelbct - 1.ToString).wraptext = True
    '            xlSheet.Range("H" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rst)

    '            strSqlQry = "select datetype,convert(varchar(10),restrictdate, 105)restrictdate from view_contracts_checkinout_restricted where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
    '            Dim rsd As New ADODB.Recordset
    '            rsd = GetResultAsRecordSet(strSqlQry)
    '            de = rsd.RecordCount
    '            If tm = 0 Then
    '                linelbct = linelbct + 2
    '            Else
    '                linelbct = linelbct + tm + 2

    '            End If
    '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsd)
    '            xlSheet.Range("A" & linelbct - 1.ToString) = "Date Type"
    '            xlSheet.Range("B" & linelbct - 1.ToString) = "Restrict Date"
    '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
    '            linelbct = linelbct + de + 3


    '        Next
    '    End If

    '    xlSheet = xlBook.Worksheets(11)
    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value


    '    Dim iLineg As Integer = 9


    '    Dim dtg As New DataTable
    '    strSqlQry = "select genpolicyid from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtg)
    '    Dim g As Integer

    '    If dtg.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtg.Rows.Count - 1
    '            xlSheet.Range("A" & iLineg.ToString) = "Genpolicy Id"
    '            xlSheet.Range("A" & iLineg.ToString).font.bold = True
    '            xlSheet.Range("B" & iLineg.ToString).font.bold = True
    '            xlSheet.Range("C" & iLineg.ToString).font.bold = True
    '            xlSheet.Range("D" & iLineg.ToString).font.bold = True
    '            xlSheet.Range("B" & iLineg.ToString) = "Applicable To"
    '            xlSheet.Range("C" & iLineg.ToString) = "To Date"
    '            xlSheet.Range("D" & iLineg.ToString) = "From Date "



    '            strSqlQry = "select distinct genpolicyid,applicableto,fromdate,todate from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
    '            Dim rsg As New ADODB.Recordset
    '            rsg = GetResultAsRecordSet(strSqlQry)
    '            g = rsg.RecordCount
    '            xlSheet.Range("A" & iLineg.ToString).CopyFromRecordset(rsg)

    '            Dim y As Integer
    '            strSqlQry = "select policytext from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "' and genpolicyid= '" & dtg.Rows(i)("genpolicyid").ToString & "' "
    '            Dim rsp As New ADODB.Recordset
    '            rsp = GetResultAsRecordSet(strSqlQry)
    '            y = rsp.RecordCount

    '            iLineg = iLineg + g + 2
    '            xlSheet.Range("A" & iLineg.ToString).CopyFromRecordset(rsp)
    '            xlSheet.Range("A" & iLineg - 1.ToString) = "Policy"
    '            xlSheet.Range("A" & iLineg - 1.ToString).font.bold = True
    '            xlSheet.Range("A" & iLineg - 1.ToString).WrapText = True
    '            'xlSheet.Range("A14").WrapText = True
    '            '("A"iLineg&char(70)iLineg).Merge()

    '            xlSheet.Range("A" & iLineg & ":" & "J" & iLineg).Merge()



    '            'xlSheet.Range("A14:I14").Merge()

    '            If g > y Then
    '                iLineg = iLineg + g + 3
    '            Else

    '                iLineg = iLineg + y + 3
    '            End If



    '        Next
    '    End If



    '    xlSheet = xlBook.Worksheets(12)
    '    xlSheet.Range("B3").Value = ViewState("hotelname")
    '    xlSheet.Range("B4").Value = hdncontractid.Value


    '    Dim dtm As New DataTable
    '    strSqlQry = "select minnightsid from view_contracts_minnights_header where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dtm)



    '    Dim iLiner As Integer = 8


    '    If dtm.Rows.Count > 0 Then
    '        For i As Integer = 0 To dtm.Rows.Count - 1
    '            xlSheet.Range("A" & iLiner - 1.ToString) = "Minnight Id"
    '            xlSheet.Range("A" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLiner - 1.ToString) = "Applicable To"
    '            xlSheet.Range("B" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & iLiner - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLiner - 1.ToString) = "RoomType"
    '            xlSheet.Range("D" & iLiner - 1.ToString) = "Meal Plan"
    '            xlSheet.Range("E" & iLiner - 1.ToString) = "From Date"
    '            xlSheet.Range("F" & iLiner - 1.ToString) = "To Date"

    '            xlSheet.Range("G" & iLiner - 1.ToString) = "Min.Nights"
    '            xlSheet.Range("H" & iLiner - 1.ToString) = " Options"

    '            Dim dtm2 As New DataTable
    '            strSqlQry = "select clineno from view_contracts_minnights_detail where minnightsid= '" & dtm.Rows(i)("minnightsid").ToString & "'"
    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(dtm2)

    '            Dim x As Integer
    '            Dim x1 As Integer

    '            If dtm2.Rows.Count > 0 Then
    '                For i2 As Integer = 0 To dtm2.Rows.Count - 1

    '                    strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_minnights_header h join view_contracts_search v on h.contractid =v.contractid join view_contracts_minnights_detail d on h.minnightsid =d.minnightsid  cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode where d.minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'  for xml path('')),1,1,''),'') "
    '                    Dim rsm1 As New ADODB.Recordset
    '                    rsm1 = GetResultAsRecordSet(strSqlQry)
    '                    x1 = rsm1.RecordCount
    '                    xlSheet.Range("C" & iLiner.ToString).CopyFromRecordset(rsm1)

    '                    strSqlQry = "select distinct D.minnightsid ,H.applicableto from view_contracts_minnights_detail d,view_contracts_minnights_header h where d.minnightsid ='" & dtm.Rows(i)("minnightsid").ToString & "' and d.clineno='" & dtm2.Rows(i2)("clineno").ToString & "' and contractid= '" & hdncontractid.Value & "'"
    '                    Dim rsm As New ADODB.Recordset
    '                    rsm = GetResultAsRecordSet(strSqlQry)
    '                    x = rsm.RecordCount
    '                    xlSheet.Range("A" & iLiner.ToString).CopyFromRecordset(rsm)


    '                    Dim y As Integer
    '                    strSqlQry = "select distinct mealplans,convert(datetime,fromdate),convert(datetime,todate),minnights,nightsoption from view_contracts_minnights_detail where minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'"
    '                    Dim rsr As New ADODB.Recordset
    '                    rsr = GetResultAsRecordSet(strSqlQry)
    '                    y = rsm.RecordCount
    '                    xlSheet.Range("D" & iLiner.ToString).CopyFromRecordset(rsr)

    '                    iLiner = iLiner + 1
    '                Next

    '            End If
    '            iLiner = iLiner + 2
    '        Next
    '    End If

    '    xlSheet = xlBook.Worksheets(13)

    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value




    '    Dim dts As New DataTable
    '    strSqlQry = "select splistcode from  view_contracts_specialevents_header where contractid= '" & hdncontractid.Value & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dts)
    '    Dim iLines As Integer = 9
    '    If dts.Rows.Count > 0 Then
    '        For i As Integer = 0 To dts.Rows.Count - 1
    '            xlSheet.Range("A" & iLines - 1.ToString) = "Splistcode"
    '            xlSheet.Range("A" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("E" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("F" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("G" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("H" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("I" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("J" & iLines - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLines - 1.ToString) = "Applicable To"
    '            xlSheet.Range("C" & iLines - 1.ToString) = "From Date"
    '            xlSheet.Range("D" & iLines - 1.ToString) = "To Date"
    '            xlSheet.Range("E" & iLines - 1.ToString) = "SplEvents Name"
    '            xlSheet.Range("F" & iLines - 1.ToString) = "RoomType"
    '            xlSheet.Range("G" & iLines - 1.ToString) = "Meal Plans"
    '            xlSheet.Range("H" & iLines - 1.ToString) = "Room Occupancy"
    '            xlSheet.Range("I" & iLines - 1.ToString) = "Adult Rate"
    '            xlSheet.Range("J" & iLines - 1.ToString) = "Child Age From,Child Age To,Child Rate"
    '            xlSheet.Range("J" & iLines - 1.ToString).wraptext = True
    '            xlSheet.Range("J" & iLines - 1.ToString).font.bold = True

    '            Dim x As Integer
    '            strSqlQry = "select distinct h.splistcode,h.applicableto,d.fromdate,d.todate,p.spleventname from view_contracts_specialevents_detail d join party_splevents  p on d.spleventcode= p.spleventcode join view_contracts_specialevents_header h on h.splistcode=d.splistcode and p.partycode= '" & CType(Session("Contractparty"), String) & "'  and d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '            '" & dtm.Rows(i)("minnightsid").ToString & "'"
    '            Dim rsm As New ADODB.Recordset
    '            rsm = GetResultAsRecordSet(strSqlQry)
    '            x = rsm.RecordCount
    '            xlSheet.Range("A" & iLines.ToString).CopyFromRecordset(rsm)


    '            Dim dters As New DataTable
    '            strSqlQry = "select distinct roomtypes,mealplans,splineno,roomcategory from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(dters)

    '            Dim ys2 As Integer
    '            Dim xs As Integer
    '            Dim ys As Integer
    '            If dters.Rows.Count > 0 Then
    '                For ers As Integer = 0 To dters.Rows.Count - 1


    '                    If dters.Rows(ers)("roomtypes").ToString = "All" Then


    '                        strSqlQry = "select roomtypes from view_contracts_specialevents_detail  where  roomtypes='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '                        xlSheet.Range("F" & iLines.ToString) = "All"
    '                        iLines = iLines

    '                    Else

    '                        strSqlQry = "select isnull(stuff((select ',' +   p.rmtypname from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),'') "
    '                        Dim rsers As New ADODB.Recordset
    '                        rsers = GetResultAsRecordSet(strSqlQry)
    '                        ys = rsers.RecordCount
    '                        iLines = iLines

    '                        xlSheet.Range("F" & iLines.ToString).CopyFromRecordset(rsers)
    '                        xlSheet.Range("F" & iLines.ToString).WrapText = True
    '                        xlSheet.Range("F" & iLines.ToString).rowheight = 100
    '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        iLines = iLines
    '                    End If


    '                    If dters.Rows(ers)("mealplans").ToString = "All" Then


    '                        strSqlQry = "select mealplans from view_contracts_specialevents_detail  where  mealplans='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '                        xlSheet.Range("G" & iLines.ToString) = "All"
    '                        iLines = iLines

    '                    Else

    '                        strSqlQry = "select isnull(stuff((select ',' +  mealplans from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and splineno ='" & dters.Rows(ers)("splineno").ToString & "' for xml path('')),1,1,''),'') "
    '                        Dim rserss As New ADODB.Recordset
    '                        rserss = GetResultAsRecordSet(strSqlQry)
    '                        ys2 = rserss.RecordCount
    '                        xlSheet.Range("G" & iLines.ToString).CopyFromRecordset(rserss)
    '                        xlSheet.Range("G" & iLines.ToString).WrapText = True
    '                        xlSheet.Range("G" & iLines.ToString).rowheight = 100
    '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        iLines = iLines
    '                    End If

    '                    If dters.Rows(ers)("roomcategory").ToString = "All" Then


    '                        strSqlQry = "select roomcategory from view_contracts_specialevents_detail  where  roomcategory='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '                        xlSheet.Range("H" & iLines.ToString) = "All"
    '                        iLines = iLines

    '                    Else

    '                        strSqlQry = "select isnull(stuff((select ',' +   p.rmcatcode from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomcategory,',') q join partyrmcat p on q.Item1=p.rmcatcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),'') "
    '                        Dim rsersr As New ADODB.Recordset
    '                        rsersr = GetResultAsRecordSet(strSqlQry)

    '                        iLines = iLines
    '                        xlSheet.Range("H" & iLines.ToString).CopyFromRecordset(rsersr)
    '                        xlSheet.Range("H" & iLines.ToString).rowheight = 100
    '                        xlSheet.Range("H" & iLines.ToString).WrapText = True
    '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
    '                        iLines = iLines
    '                    End If


    '                Next

    '            End If


    '            Dim y As Integer
    '            strSqlQry = "select distinct adultrate,childdetails from view_contracts_specialevents_detail where splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
    '            Dim rsr As New ADODB.Recordset
    '            rsr = GetResultAsRecordSet(strSqlQry)
    '            iLines = iLines
    '            y = rsr.RecordCount
    '            xlSheet.Range("I" & iLines.ToString).CopyFromRecordset(rsr)

    '            If x > y Then
    '                iLines = iLines + x + 2
    '            Else
    '                iLines = iLines + y + 2
    '            End If

    '        Next
    '    End If

    '    xlSheet = xlBook.Worksheets(14)
    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value



    '    Dim iLine5 As Integer = 10
    '    Dim dt4 As New DataTable
    '    strSqlQry = "select h.constructionid from hotels_construction h join partymast p on p.partycode=h.partycode and p.partyname='" & ViewState("hotelname") & "'"
    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
    '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '    myDataAdapter.Fill(dt4)

    '    If dt4.Rows.Count > 0 Then
    '        For i As Integer = 0 To dt4.Rows.Count - 1

    '            xlSheet.Range("A" & iLine5 - 1.ToString) = "Construction Id"
    '            xlSheet.Range("A" & iLine5 - 1.ToString).font.bold = True
    '            xlSheet.Range("B" & iLine5 - 1.ToString) = "From Date"
    '            xlSheet.Range("B" & iLine5 - 1.ToString).font.bold = True
    '            xlSheet.Range("C" & iLine5 - 1.ToString) = "To Date"
    '            xlSheet.Range("C" & iLine5 - 1.ToString).font.bold = True
    '            xlSheet.Range("D" & iLine5 - 1.ToString) = "Reason"
    '            xlSheet.Range("D" & iLine5 - 1.ToString).font.bold = True
    '            Dim x As Integer
    '            strSqlQry = "select constructionid,fromdate,todate,Reason from hotels_construction where constructionid  ='" & dt4.Rows(i)("constructionid").ToString & "'"
    '            Dim rs5 As New ADODB.Recordset
    '            rs5 = GetResultAsRecordSet(strSqlQry)

    '            x = rs5.RecordCount
    '            xlSheet.Range("A" & iLine5.ToString).CopyFromRecordset(rs5)
    '            iLine5 = iLine5 + x + 2
    '        Next
    '    End If

    '    xlSheet = xlBook.Worksheets(15)
    '    xlSheet.Range("B4").Value = ViewState("hotelname")
    '    xlSheet.Range("B5").Value = hdncontractid.Value


    '    strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"
    '    Dim rsrf As New ADODB.Recordset
    '    Dim yf As Integer
    '    rsrf = GetResultAsRecordSet(strSqlQry)
    '    Dim iLinesf As Integer = 8


    '    yf = rsrf.RecordCount

    '    xlSheet.Range("A" & iLinesf - 1.ToString) = "plistcode"
    '    xlSheet.Range("B" & iLinesf - 1.ToString) = "Roomtype"
    '    xlSheet.Range("C" & iLinesf - 1.ToString) = "Roomcategory"
    '    xlSheet.Range("D" & iLinesf - 1.ToString) = "Mealplan"
    '    xlSheet.Range("E" & iLinesf - 1.ToString) = "Accommodationid"
    '    xlSheet.Range("E" & iLinesf - 1.ToString).wraptext = True
    '    xlSheet.Range("F" & iLinesf - 1.ToString) = "Agecombination"
    '    xlSheet.Range("F" & iLinesf - 1.ToString).wraptext = True
    '    xlSheet.Range("G" & iLinesf - 1.ToString) = "Adults"

    '    'xlSheet.Range("I" & iLinesf - 1.ToString) = "Child"
    '    'xlSheet.Range("J" & iLinesf - 1.ToString) = "Totalpaxwithinpricepax"
    '    xlSheet.Range("J" & iLinesf - 1.ToString).wraptext = True
    '    'xlSheet.Range("K" & iLinesf - 1.ToString) = "Maxeb"
    '    'xlSheet.Range("L" & iLinesf - 1.ToString) = "Noofadulteb"
    '    'xlSheet.Range("M" & iLinesf - 1.ToString) = "pfromdate"
    '    'xlSheet.Range("N" & iLinesf - 1.ToString) = "ptodate"
    '    'xlSheet.Range("O" & iLinesf - 1.ToString) = "Grr"
    '    'xlSheet.Range("P" & iLinesf - 1.ToString) = "Npr"
    '    'xlSheet.Range("Q" & iLinesf - 1.ToString) = "Exhibitionprice"
    '    'xlSheet.Range("R" & iLinesf - 1.ToString) = "Adultebprice"
    '    'xlSheet.Range("S" & iLinesf - 1.ToString) = "Extrapaxprice"
    '    'xlSheet.Range("T" & iLinesf - 1.ToString) = "Totalsharingcharge"
    '    'xlSheet.Range("T" & iLinesf - 1.ToString).wraptext = True
    '    'xlSheet.Range("U" & iLinesf - 1.ToString) = "Totalebcharge"
    '    'xlSheet.Range("V" & iLinesf - 1.ToString) = "Totalprice"
    '    'xlSheet.Range("W" & iLinesf - 1.ToString) = "Nights"
    '    'xlSheet.Range("X" & iLinesf - 1.ToString) = "Minstay"
    '    'xlSheet.Range("Y" & iLinesf - 1.ToString) = "Minstayoption"
    '    'xlSheet.Range("Z" & iLinesf - 1.ToString) = "Commissionformulaid"
    '    'xlSheet.Range("AA" & iLinesf - 1.ToString) = "Commissionformulastring"
    '    xlSheet.Range("AA" & iLinesf - 1.ToString).wraptext = True
    '    'xlSheet.Range("AB" & iLinesf - 1.ToString) = "Pricepax"
    '    'xlSheet.Range("AC" & iLinesf - 1.ToString) = "Childpolicyid"
    '    'xlSheet.Range("AD" & iLinesf - 1.ToString) = "rmtyporder"
    '    'xlSheet.Range("AE" & iLinesf - 1.ToString) = "rmcatorder"
    '    xlSheet.Range("A" & iLinesf.ToString).CopyFromRecordset(rsrf)








    '    FolderPath = "~\ExcelTemp\"
    '    Dim FileNameNew As String = "ContractPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
    '    Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)
    '    'Delete all files from Temp folder
    '    Try

    '        Dim outputPath1 As String = Server.MapPath(FolderPath)
    '        If Directory.Exists(outputPath1) Then

    '            Dim files() As String
    '            files = Directory.GetFileSystemEntries(outputPath1)

    '            For Each element As String In files
    '                If (Not Directory.Exists(element)) Then
    '                    File.Delete(Path.Combine(outputPath1, Path.GetFileName(element)))
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception

    '    End Try
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

    '    ExcelOpen(outputPath)
    '    ' Collect the unreferenced objects
    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()

    'End Sub



    Protected Sub btnContractPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContractPrint.Click
        'Dim xlApp
        'Dim xlBook
        'Dim xlSheet



        'xlApp = Server.CreateObject("Excel.Application")
        'xlApp.visible = False
        'Dim FolderPath As String = "..\ExcelTemplates\"
        'Dim FileName As String = "ContractPrint.xlsx"
        'Dim FilePath As String = Server.MapPath(FolderPath + FileName)
        'Dim strConn As String = Session("dbconnectionName")




        'Try
        '    xlBook = xlApp.Workbooks.Open(FilePath, True, False)

        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try
        'Try



        '    xlSheet = xlBook.Worksheets(1) ' Sheet 1
        '    xlSheet.Range("C3").Value = ViewState("hotelname")
        '    xlSheet.Range("C5").Value = hdncontractid.Value

        '    xlSheet = xlBook.Worksheets(2) ' Sheet 2

        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value
        '    xlSheet.Range("A6").Value = "From Date"




        '    Dim dt1 As New DataTable
        '    strSqlQry = "select isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),applicableto,0 countrygroups, isnull(activestate,'') activestate  from  view_contracts_search where contractid ='" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt1)

        '    If dt1.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt1.Rows.Count - 1
        '            xlSheet.Range("B6").Value = dt1.Rows(0)(0).ToString
        '            xlSheet.Range("B7").Value = dt1.Rows(0)(1).ToString
        '            xlSheet.Range("B8").Value = dt1.Rows(0)(2).ToString
        '            xlSheet.Range("B10").Value = dt1.Rows(0)(4).ToString
        '        Next
        '    End If

        '    Dim dt2 As New DataTable
        '    strSqlQry = "select contractid from view_contractcountry where contractid ='" & hdncontractid.Value & "' "
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt2)

        '    Dim dt As New DataTable
        '    strSqlQry = "select isnull(stuff((select ',' + p.ctryname  from ctrymast p ,view_contractcountry  v where v.ctrycode =p.ctrycode  and v.contractid ='" & hdncontractid.Value & "'  order by ISNULL(p.ctryname,'') for xml path('')),1,1,''),'') Country"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt)

        '    Dim r As Integer = 14
        '    If dt2.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt.Rows.Count - 1

        '            Dim remark As String = ""
        '            Dim exap As New Excel.Application
        '            Dim Range As Excel.Range
        '            Dim remarklength As Integer

        '            remark = dt.Rows(i)("Country").ToString
        '            remark = remark.Replace(Chr(13), "")
        '            Dim remarks As String() = remark.Split(Chr(10))
        '            If (remarks.Length > 0) Then
        '                Dim re As String
        '                For Each re In remarks


        '                    With xlSheet.Range("A" & r & ":" & "F" & r)
        '                        .MergeCells = True
        '                        .WrapText = True
        '                        'If re.Length > 152 Then 'longtext and wrap- but not displayed completely..so added this--need to chk
        '                        '    .Cells.RowHeight = calCellHeightForPromotion(re.Length)
        '                        'End If
        '                    End With
        '                    xlSheet.Range("A14").Value = re
        '                    Range = xlSheet.Range("A" & r & ":" & "F" & r)
        '                    autoheight(Range, exap)
        '                    r = r + 1
        '                Next
        '            End If
        '        Next

        '    End If

        '    'If dt2.Rows.Count > 0 Then
        '    '    For i As Integer = 0 To dt.Rows.Count - 1
        '    '        xlSheet.Range("A12").Value = "Countries"
        '    '        xlSheet.Range("A14:I14").Merge()
        '    '        xlSheet.Range("A14").WrapText = True
        '    '        'xlSheet.Range("A14").rowheight = 400
        '    '        xlSheet.Range("A14").Value = dt.Rows(i)("Country").ToString
        '    '    Next

        '    'Else
        '    '    For i As Integer = 0 To dt.Rows.Count - 1

        '    '        xlSheet.Range("A14").Value = dt.Rows(i)("Country").ToString
        '    '    Next
        '    'End If

        '    strSqlQry = "select isnull(seasonname,''),isnull(convert(varchar(10),fromdate , 105),'')fromdate,isnull(convert(varchar(10),todate , 105),'')todate,isnull(MinNight,'') from view_contractseasons where contractid='" & hdncontractid.Value & "' "
        '    'Dim conn As New ADODB.Connection
        '    Dim rs As ADODB.Recordset
        '    'Dim oledbcon As String = ConfigurationManager.ConnectionStrings("strADODBConnection").ConnectionString
        '    'conn.Open(oledbcon)
        '    'rs = conn.Execute(strSqlQry)
        '    rs = GetResultAsRecordSet(strSqlQry)
        '    Dim iLineNo As Integer = 19
        '    xlSheet.Range("A" & iLineNo.ToString).CopyFromRecordset(rs)

        '    xlSheet = xlBook.Worksheets(3)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value

        '    strSqlQry = "select tranid from view_contracts_commission_header where contractid = '" & hdncontractid.Value & "'"
        '    Dim rsi As New ADODB.Recordset
        '    rsi = GetResultAsRecordSet(strSqlQry)
        '    Dim iLineNoI As Integer = 10

        '    Dim dt3 As New DataTable
        '    strSqlQry = "select tranid,isnull(seasons,'')seasons from view_contracts_commission_header where contractid = '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt3)
        '    Dim iLine As Integer = 11

        '    Dim iLineNo2 As Integer = 10

        '    If dt3.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt3.Rows.Count - 1
        '            Dim x As Integer
        '            Dim y As Integer
        '            Dim ex As Integer



        '            xlSheet.Range("A" & iLineNo2 - 1.ToString) = "Commission Id"
        '            xlSheet.Range("A" & iLineNo2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLineNo2 - 1.ToString) = "Formula Name"
        '            xlSheet.Range("B" & iLineNo2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLineNo2 - 1.ToString) = "Formula String"
        '            xlSheet.Range("C" & iLineNo2 - 1.ToString).font.bold = True

        '            If dt3.Rows(i)("seasons") <> "" Then
        '                xlSheet.Range("D" & iLineNo2 - 1.ToString) = "Season Name	"
        '                xlSheet.Range("D" & iLineNo2 - 1.ToString).font.bold = True
        '                xlSheet.Range("E" & iLineNo2 - 1.ToString) = "From Date"
        '                xlSheet.Range("E" & iLineNo2 - 1.ToString).font.bold = True
        '                xlSheet.Range("F" & iLineNo2 - 1.ToString) = "To Date"
        '                xlSheet.Range("F" & iLineNo2 - 1.ToString).font.bold = True
        '            Else
        '                xlSheet.Range("E" & iLineNo2 - 1.ToString) = "From Date"
        '                xlSheet.Range("E" & iLineNo2 - 1.ToString).font.bold = True
        '                xlSheet.Range("F" & iLineNo2 - 1.ToString) = "To Date"
        '                xlSheet.Range("F" & iLineNo2 - 1.ToString).font.bold = True
        '            End If
        '            xlSheet.Range("G" & iLineNo2 - 1.ToString) = "Room Categories"
        '            xlSheet.Range("G" & iLineNo2 - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLineNo2 - 1.ToString) = "Room Types"
        '            xlSheet.Range("H" & iLineNo2 - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLineNo2 - 1.ToString) = "Meal Plans"
        '            xlSheet.Range("I" & iLineNo2 - 1.ToString).font.bold = True
        '            xlSheet.Range("J" & iLineNo2 - 1.ToString) = "Applicable To"
        '            xlSheet.Range("J" & iLineNo2 - 1.ToString).font.bold = True



        '            strSqlQry = "select distinct h.tranid, v.formulaname,formulastring=isnull(stuff((select ';' + c.term1+','+ltrim(rtrim(convert(varchar(20),c.value)))  from view_contracts_commissions c,commissionformula_header h where c.formulaid=h.formulaid  and  c.tranid ='" & dt3.Rows(i)("tranid").ToString & "'    order by c.clineno for xml path('')),1,1,''),'') from view_contracts_commission_header h, view_contracts_commissions c, commissionformula_header v  where h.tranid = c.tranid And c.formulaid = v.formulaid and h.tranid ='" & dt3.Rows(i)("tranid").ToString & "'"
        '            Dim rs1 As New ADODB.Recordset
        '            rs1 = GetResultAsRecordSet(strSqlQry)
        '            ' Dim iLineNo1 As Integer = 10
        '            xlSheet.Range("A" & iLineNo2.ToString).CopyFromRecordset(rs1)
        '            xlSheet.Range("B" & iLineNo2.ToString).wraptext = True
        '            xlSheet.Range("C" & iLineNo2.ToString).wraptext = True
        '            x = rs1.RecordCount

        '            If dt3.Rows(i)("seasons") <> "" Then

        '                strSqlQry = "select isnull(cs.Item1,'') seascode,isnull(convert(varchar(10),s.fromdate , 105),'')fromdate,isnull(convert(varchar(10),s.todate , 105),'')todate from view_contracts_commission_header h"
        '                strSqlQry += " cross apply dbo.SplitString1colsWithOrderField(h.seasons,',') cs "
        '                strSqlQry += " join view_contractseasons s on h.contractid=s.contractid and cs.Item1=s.SeasonName "
        '                strSqlQry += " where h.contractid ='" & hdncontractid.Value & "' and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(cs.Item1,'')<>'' order by seascode,fromdate "


        '                Dim rs2 As New ADODB.Recordset
        '                rs2 = GetResultAsRecordSet(strSqlQry)
        '                xlSheet.Range("D" & iLineNo2.ToString).CopyFromRecordset(rs2)
        '                y = rs2.RecordCount
        '            Else


        '                strSqlQry = "SELECT isnull(fromdate,''),isnull(todate,'') from view_contracts_commission_detail WHERE tranid='" & dt3.Rows(i)("tranid").ToString & "' "
        '                Dim rssd As ADODB.Recordset
        '                rssd = GetResultAsRecordSet(strSqlQry)

        '                ex = rssd.RecordCount
        '                xlSheet.Range("E" & iLineNo2.ToString).CopyFromRecordset(rssd)
        '            End If
        '            strSqlQry = "select distinct isnull(prm.rmtypname,''),isnull(h.mealplans,''),h.applicableto from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomtypes,',') s  join partyrmtyp prm on s.Item1=prm.rmtypcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>''"
        '            Dim rs3 As ADODB.Recordset
        '            rs3 = GetResultAsRecordSet(strSqlQry)

        '            xlSheet.Range("H" & iLineNo2.ToString).CopyFromRecordset(rs3)
        '            Dim z As Integer
        '            z = rs3.RecordCount


        '            strSqlQry = "select isnull(r.rmcatname,'')   from view_contracts_commission_header h cross apply dbo.SplitString1colsWithOrderField(h.roomcategory,',') s join view_contracts_search ch on h.contractid=ch.contractid join rmcatmast r on s.Item1=r.rmcatcode and h.tranid='" & dt3.Rows(i)("tranid").ToString & "' and isnull(h.roomtypes,'')<>'' "
        '            Dim rs4 As ADODB.Recordset
        '            rs4 = GetResultAsRecordSet(strSqlQry)

        '            xlSheet.Range("G" & iLineNo2.ToString).CopyFromRecordset(rs4)
        '            Dim d As Integer
        '            d = rs4.RecordCount
        '            Dim Maxint As Integer = Math.Max(x, Math.Max(y, Math.Max(z, Math.Max(ex, d))))


        '            iLineNo2 = iLineNo2 + Maxint + 2




        '        Next
        '    End If

        '    xlSheet = xlBook.Worksheets(4)

        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value
        '    Dim dtmx As New DataTable


        '    strSqlQry = "select distinct tranid from view_partymaxaccomodation where partycode= '" & CType(Session("Contractparty"), String) & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtmx)


        '    Dim iLinmx As Integer = 9

        '    Dim em1 As Integer
        '    Dim em As Integer
        '    Dim em2 As Integer



        '    Dim dt23 As New DataTable
        '    If dtmx.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtmx.Rows.Count - 1

        '            xlSheet.Range("A" & iLinmx - 1.ToString) = "Max Occ.ID"
        '            xlSheet.Range("B" & iLinmx - 1.ToString) = "Room Name"
        '            xlSheet.Range("A" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinmx - 1.ToString).font.bold = True

        '            xlSheet.Range("C" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinmx - 1.ToString).font.bold = True

        '            xlSheet.Range("F" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("J" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("K" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("L" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("M" & iLinmx - 1.ToString).font.bold = True
        '            xlSheet.Range("N" & iLinmx - 1.ToString).font.bold = True


        '            xlSheet.Range("C" & iLinmx - 1.ToString) = "Room Classification"
        '            xlSheet.Range("D" & iLinmx - 1.ToString) = "unit yes/no	"
        '            xlSheet.Range("E" & iLinmx - 1.ToString) = "Price Adult Occupancy only for Unit"
        '            xlSheet.Range("E" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("F" & iLinmx - 1.ToString) = "Price Pax"
        '            xlSheet.Range("G" & iLinmx - 1.ToString) = "Max Adults"
        '            xlSheet.Range("H" & iLinmx - 1.ToString) = "Max Child"
        '            xlSheet.Range("I" & iLinmx - 1.ToString) = "Max Infant"
        '            xlSheet.Range("J" & iLinmx - 1.ToString) = "Max EB"

        '            xlSheet.Range("K" & iLinmx - 1.ToString) = "Max Total Occupancy without infant"
        '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("L" & iLinmx - 1.ToString) = "Rank Order"
        '            xlSheet.Range("N" & iLinmx - 1.ToString) = "Start with 0 based"
        '            xlSheet.Range("K" & iLinmx - 1.ToString).wraptext = True
        '            xlSheet.Range("M" & iLinmx - 1.ToString) = "Occupancy Combinations"




        '            Dim dtmx1 As New DataTable
        '            strSqlQry = "select rmtypcode from view_partymaxaccomodation where partycode= '" & CType(Session("Contractparty"), String) & "' and tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dtmx1)


        '            strSqlQry = "select distinct m.tranid,prm.rmtypname,rc.roomclassname, case when isnull(prm.unityesno,0)=0 then 'No' else 'yes' end status,m.pricepax,m.maxadults,m.maxchilds,maxinfant,m.maxeb,isnull(m.noofextraperson,''), m.maxoccpancy,prm.rankord from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            Dim rsmx As New ADODB.Recordset
        '            rsmx = GetResultAsRecordSet(strSqlQry)
        '            em = rsmx.RecordCount

        '            xlSheet.Range("A" & iLinmx.ToString).CopyFromRecordset(rsmx)
        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If
        '            Dim rsmx1 As New ADODB.Recordset
        '            If dtmx1.Rows.Count > 0 Then
        '                For idt As Integer = 0 To dtmx1.Rows.Count - 1



        '                    strSqlQry = "select distinct isnull(stuff((select ',' + ltrim(STR(ltrim(maxadults)))+'/'+ltrim(STR(ltrim(maxchilds)))+'/'+rmcatcode  from view_maxaccom_details where  tranid='" & dtmx.Rows(i)("tranid").ToString & "' and rmtypcode='" & dtmx1.Rows(idt)("rmtypcode").ToString & "' and  partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "

        '                    rsmx1 = GetResultAsRecordSet(strSqlQry)
        '                    em1 = rsmx1.RecordCount
        '                    xlSheet.Range("M" & iLinmx.ToString).CopyFromRecordset(rsmx1)

        '                    iLinmx = iLinmx + 1

        '                Next

        '            End If



        '            strSqlQry = "select  isnull(start0based,'') from view_partymaxaccomodation m, partyrmtyp prm,view_partymaxacc_header h,room_classification rc where m.partycode=prm.partycode and m.rmtypcode=prm.rmtypcode and prm.roomclasscode=rc.roomclasscode  and m.tranid=h.tranid and h.partycode='" & CType(Session("Contractparty"), String) & "' and m.tranid='" & dtmx.Rows(i)("tranid").ToString & "'"
        '            Dim rsmx2 As New ADODB.Recordset
        '            rsmx2 = GetResultAsRecordSet(strSqlQry)
        '            em2 = rsmx2.RecordCount
        '            iLinmx = iLinmx - dtmx1.Rows.Count

        '            xlSheet.Range("N" & iLinmx.ToString).CopyFromRecordset(rsmx2)

        '            Dim Maxint As Integer = Math.Max(em1, Math.Max(em2, em))



        '            iLinmx = iLinmx + Maxint + 4



        '        Next

        '    End If

        '    xlSheet = xlBook.Worksheets(5)

        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value

        '    Dim dtrr2 As New DataTable
        '    strSqlQry = "select plistcode from view_cplisthnew  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtrr2)
        '    Dim iLine2 As Integer = 8
        '    Dim ei2 As Integer
        '    Dim ei3 As Integer
        '    Dim ei4 As Integer
        '    If dtrr2.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtrr2.Rows.Count - 1


        '            strSqlQry = "select plistcode,applicableto from  view_cplisthnew where plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "'"
        '            Dim rse2 As New ADODB.Recordset
        '            rse2 = GetResultAsRecordSet(strSqlQry)
        '            ei2 = rse2.RecordCount
        '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse2)
        '            xlSheet.Range("A" & iLine2 - 1.ToString) = "PriceList Code"
        '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine2 - 1.ToString) = "Aplicable to"
        '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True


        '            strSqlQry = "select  isnull(c.subseascode,''),isnull(d.fromdate,'') fromdate,isnull(d.todate,'') todate from view_cplisthnew c,view_contractseasons d  where c.subseascode=d.SeasonName and c.plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' and d.contractid='" & hdncontractid.Value & "'"
        '            Dim rse32 As New ADODB.Recordset
        '            rse32 = GetResultAsRecordSet(strSqlQry)
        '            ei4 = rse32.RecordCount

        '            iLine2 = iLine2 + 1

        '            xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rse32)
        '            xlSheet.Range("A" & iLine2 - 1.ToString) = "Season"
        '            xlSheet.Range("A" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine2 - 1.ToString) = "From Date"
        '            xlSheet.Range("B" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine2 - 1.ToString) = "To Date"

        '            Dim fromrange As Integer, torange As Integer
        '            fromrange = iLine2
        '            torange = IIf(rse32.RecordCount > 0, iLine2 + rse32.RecordCount, iLine2)

        '            xlSheet.Range("B" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"
        '            xlSheet.Range("C" & fromrange.ToString & ":" & "B" & torange.ToString).NumberFormat = "dd/mm/yyyy;@"

        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_cplisthnew_weekdays   where  plistcode='" & dtrr2.Rows(i)("plistcode").ToString & "' for xml path('')),1,1,''),'') "

        '            Dim rsw2 As New ADODB.Recordset
        '            rsw2 = GetResultAsRecordSet(strSqlQry)
        '            iLine2 = iLine2 + ei4 + 2
        '            xlSheet.Range("B" & iLine2.ToString).CopyFromRecordset(rsw2)
        '            xlSheet.Range("B" & iLine2.ToString).wraptext = True
        '            xlSheet.Range("B" & iLine2 & ":" & "C" & iLine2).Merge()
        '            xlSheet.Range("B" & iLine2.ToString).rowheight = 30
        '            xlSheet.Range("A" & iLine2.ToString) = "Days of the week"
        '            xlSheet.Range("A" & iLine2.ToString).font.bold = True
        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If



        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_roomrates]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@plistcode", dtrr2.Rows(i)("plistcode").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)

        '                    End Using
        '                End Using
        '            End Using


        '            Dim rssp72 As New ADODB.Recordset
        '            rssp72 = convertToADODB(dtt)
        '            Dim ii3 As Integer = 65
        '            'For Each column As DataColumn In dt.Columns
        '            'name(ii) = column.ColumnName
        '            iLine2 = iLine2 + 2
        '            Dim sss3 = Chr(ii3).ToString
        '            For OO As Integer = 0 To dtt.Columns.Count - 1
        '                xlSheet.Range(sss3.ToString() + iLine2.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                ii3 += 1
        '                xlSheet.Range(sss3.ToString() + (iLine2).ToString).FONT.BOLD = True
        '                sss3 = Chr(ii3).ToString
        '            Next

        '            If rssp72.RecordCount > 0 Then
        '                iLine2 = iLine2 + 1
        '                xlSheet.Range("A" & iLine2.ToString).CopyFromRecordset(rssp72)
        '                ei3 = rssp72.RecordCount

        '                fromrange = iLine2
        '                torange = IIf(rssp72.RecordCount > 0, iLine2 + rssp72.RecordCount, iLine2)
        '                xlSheet.Range("C" & fromrange.ToString & ":" & "C" & torange.ToString).NumberFormat = "####"
        '            End If

        '            iLine2 = iLine2 + ei3 + 3

        '        Next

        '    End If

        '    xlSheet = xlBook.Worksheets(6)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value

        '    Dim dte As New DataTable
        '    strSqlQry = "select exhibitionid from view_contracts_exhibition_header  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dte)
        '    Dim iLinee As Integer = 8
        '    Dim ei As Integer
        '    Dim ze As Integer
        '    If dte.Rows.Count > 0 Then
        '        For i As Integer = 0 To dte.Rows.Count - 1
        '            xlSheet.Range("A" & iLinee - 1.ToString) = "Exhibition Id"
        '            xlSheet.Range("B" & iLinee - 1.ToString) = "Applicable To"
        '            xlSheet.Range("C" & iLinee - 1.ToString) = "Exhibition Name"
        '            xlSheet.Range("D" & iLinee - 1.ToString) = "From Date"
        '            xlSheet.Range("E" & iLinee - 1.ToString) = "To Date"
        '            xlSheet.Range("A" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinee - 1.ToString) = "Room Type"
        '            xlSheet.Range("G" & iLinee - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("H" & iLinee - 1.ToString) = "Supplement Amount"
        '            xlSheet.Range("H" & iLinee - 1.ToString).wraptext = True
        '            xlSheet.Range("I" & iLinee - 1.ToString) = "Min Stay"
        '            xlSheet.Range("B" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLinee - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLinee - 1.ToString).font.bold = True


        '            strSqlQry = "select h.exhibitionid,h.applicableto, e.exhibitionname,d.fromdate,d.todate from view_contracts_exhibition_detail d join exhibition_master e on d.exhibitioncode=e.exhibitioncode join  view_contracts_exhibition_header h on d.exhibitionid=h.exhibitionid and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            Dim rse As New ADODB.Recordset
        '            rse = GetResultAsRecordSet(strSqlQry)
        '            ei = rse.RecordCount
        '            xlSheet.Range("A" & iLinee.ToString).CopyFromRecordset(rse)

        '            strSqlQry = "select distinct isnull(mealplans,''),supplementvalue,isnull(minstay,'') from view_contracts_exhibition_detail where exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            Dim rsr As New ADODB.Recordset
        '            rsr = GetResultAsRecordSet(strSqlQry)
        '            ze = rsr.RecordCount
        '            xlSheet.Range("G" & iLinee.ToString).CopyFromRecordset(rsr)

        '            Dim dter As New DataTable
        '            strSqlQry = "select distinct roomtypes,exhibitioncode from view_contracts_exhibition_detail  where  exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dter)



        '            Dim x As Integer
        '            Dim y As Integer
        '            If dter.Rows.Count > 0 Then
        '                For er As Integer = 0 To dter.Rows.Count - 1


        '                    If dter.Rows(er)("roomtypes").ToString = "All" Then


        '                        strSqlQry = "select roomtypes from view_contracts_exhibition_detail  where  roomtypes='All' and exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'"
        '                        xlSheet.Range("F" & iLinee.ToString) = "All"
        '                        iLinee = iLinee + 1

        '                    ElseIf dter.Rows(er)("roomtypes").ToString <> "All" Then



        '                        strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname from view_contracts_exhibition_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.exhibitionid='" & dte.Rows(i)("exhibitionid").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "' and d.exhibitioncode='" & dter.Rows(er)("exhibitioncode").ToString & "' for xml path('')),1,1,''),'') "
        '                        Dim rser As New ADODB.Recordset
        '                        rser = GetResultAsRecordSet(strSqlQry)
        '                        y = rser.RecordCount
        '                        iLinee = iLinee
        '                        xlSheet.Range("F" & iLinee.ToString).CopyFromRecordset(rser)
        '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        iLinee = iLinee + y
        '                    End If

        '                Next

        '            End If


        '            Dim Maxint As Integer = Math.Max(x, Math.Max(y, Math.Max(ei, ze)))


        '            iLinee = iLinee + Maxint + 1

        '        Next
        '    End If
        '    xlSheet = xlBook.Worksheets(7)


        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value

        '    Dim dtmr As New DataTable
        '    strSqlQry = "select mealsupplementid from view_contracts_mealsupp_header  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtmr)
        '    Dim iLinmr As Integer = 8
        '    Dim m7 As Integer
        '    Dim s7 As Integer
        '    Dim e7 As Integer
        '    Dim w7 As Integer
        '    Dim mn7 As Integer
        '    Dim tm7 As Integer
        '    If dtmr.Rows.Count > 0 Then
        '        ' Dim conn As New ADODB.Connection
        '        For i As Integer = 0 To dtmr.Rows.Count - 1

        '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Supplement ID"
        '            xlSheet.Range("B" & iLinmr - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
        '            strSqlQry = "select mealsupplementid,applicableto from view_contracts_mealsupp_header where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsm7 As New ADODB.Recordset
        '            rsm7 = GetResultAsRecordSet(strSqlQry)
        '            m7 = rsm7.RecordCount
        '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rsm7)

        '            strSqlQry = "select distinct isnull(q.Item1,''), isnull(convert(varchar(10),s.fromdate , 105),''),isnull(convert(varchar(10),s.todate , 105),'') from view_contracts_mealsupp_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"

        '            Dim rscm As New ADODB.Recordset
        '            rscm = GetResultAsRecordSet(strSqlQry)
        '            tm7 = rscm.RecordCount


        '            iLinmr = iLinmr + m7 + 3


        '            xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rscm)
        '            xlSheet.Range("A" & iLinmr - 1.ToString) = "Season"
        '            xlSheet.Range("A" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinmr - 1.ToString) = "From Date"
        '            xlSheet.Range("B" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinmr - 1.ToString) = "To Date"
        '            xlSheet.Range("C" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString) = "Manual From Date not linked to Seasons"
        '            xlSheet.Range("E" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("E" & iLinmr - 1.ToString) = "Manual To Date not linked to Seasons"
        '            xlSheet.Range("E" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("F" & iLinmr - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinmr - 1.ToString) = "Excluded From Date"
        '            xlSheet.Range("G" & iLinmr - 1.ToString).font.bold = True
        '            'xlSheet.Range("G" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("E" & iLinmr - 1.ToString).WRAPTEXT = True
        '            xlSheet.Range("G" & iLinmr - 1.ToString) = "Excluded To Date"


        '            strSqlQry = "select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from  view_contracts_mealsupp_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsmc As New ADODB.Recordset
        '            rsmc = GetResultAsRecordSet(strSqlQry)
        '            mn7 = rsmc.RecordCount

        '            iLinmr = iLinmr

        '            xlSheet.Range("D" & iLinmr.ToString).CopyFromRecordset(rsmc)

        '            strSqlQry = " select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from view_contracts_mealsupp_excl_dates where mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "'"
        '            Dim rsed As New ADODB.Recordset
        '            rsed = GetResultAsRecordSet(strSqlQry)
        '            e7 = rsed.RecordCount
        '            iLinmr = iLinmr
        '            xlSheet.Range("F" & iLinmr.ToString).CopyFromRecordset(rsed)

        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_mealsupp_weekdays  where  mealsupplementid='" & dtmr.Rows(i)("mealsupplementid").ToString & "' for xml path('')),1,1,''),'') "

        '            Dim rsw As New ADODB.Recordset
        '            rsw = GetResultAsRecordSet(strSqlQry)

        '            w7 = rsw.RecordCount
        '            Dim Maxint As Integer = Math.Max(tm7, Math.Max(mn7, m7))


        '            iLinmr = iLinmr + Maxint + 1

        '            xlSheet.Range("B" & iLinmr.ToString).CopyFromRecordset(rsw)
        '            xlSheet.Range("B" & iLinmr.ToString).wraptext = True
        '            xlSheet.Range("B" & iLinmr & ":" & "C" & iLinmr).Merge()
        '            xlSheet.Range("A" & iLinmr.ToString) = "Days of the week"
        '            xlSheet.Range("A" & iLinmr.ToString).font.bold = True
        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If


        '            'If conn.State = ConnectionState.Open Then
        '            '    conn.Close()
        '            'End If

        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_mealsupplements]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@mealsupplementid", dtmr.Rows(i)("mealsupplementid").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)
        '                        'If dtt.Rows(i)(i) = "-3" Then
        '                        '    "Free"

        '                        '    "Incl"
        '                        '    txt.Text = "-1"
        '                        '    Case "N.Incl"
        '                        '    txt.Text = "-2"
        '                        '    Case "N/A"
        '                        '    txt.Text = "-4"
        '                        '    Case "On Request"
        '                        '    txt.Text = "-5"

        '                    End Using
        '                End Using
        '            End Using




        '            Dim rssp7 As New ADODB.Recordset
        '            rssp7 = convertToADODB(dtt)
        '            iLinmr = iLinmr + w7 + 1


        '            'Dim name(dtt.Columns.Count) As String
        '            Dim ii As Integer = 65
        '            'For Each column As DataColumn In dt.Columns
        '            'name(ii) = column.ColumnName

        '            Dim sss = Chr(ii).ToString
        '            For OO As Integer = 0 To dtt.Columns.Count - 1
        '                xlSheet.Range(sss.ToString() + iLinmr.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                ii += 1
        '                xlSheet.Range(sss.ToString() + (iLinmr).ToString).FONT.BOLD = True
        '                sss = Chr(ii).ToString
        '            Next





        '            'Next


        '            If rssp7.RecordCount > 0 Then
        '                iLinmr = iLinmr + 1
        '                xlSheet.Range("A" & iLinmr.ToString).CopyFromRecordset(rssp7)
        '                s7 = rssp7.RecordCount

        '            End If


        '            iLinmr = iLinmr + s7 + 3

        '        Next

        '    End If
        '    xlSheet = xlBook.Worksheets(8)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value

        '    Dim dtcpi As New DataTable
        '    strSqlQry = "select childpolicyid from view_contracts_childpolicy_header where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtcpi)
        '    Dim iLine8 As Integer = 7

        '    If dtcpi.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtcpi.Rows.Count - 1

        '            xlSheet.Range("A" & iLine8 - 1.ToString) = "ChildPolicy Id"
        '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine8 - 1.ToString) = "Applicable To"

        '            Dim ml8 As Integer
        '            Dim tm8 As Integer
        '            Dim co8 As Integer
        '            Dim d8 As Integer
        '            Dim chk8 As Integer
        '            Dim ei31 As Integer

        '            strSqlQry = "select childpolicyid,applicableto from view_contracts_childpolicy_header  where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
        '            Dim rs8 As New ADODB.Recordset
        '            rs8 = GetResultAsRecordSet(strSqlQry)
        '            chk8 = rs8.RecordCount
        '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rs8)
        '            'xlSheet.Range("A" & iLine8.ToString) = ""

        '            strSqlQry = "select isnull(stuff((select ',' +  dayoftheweek from view_contracts_childpolicy_weekdays where  childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "' for xml path('')),1,1,''),'') "
        '            Dim rss8 As New ADODB.Recordset
        '            rss8 = GetResultAsRecordSet(strSqlQry)

        '            iLine8 = iLine8 + chk8 + 1
        '            xlSheet.Range("A" & iLine8.ToString) = "Days of the week"
        '            xlSheet.Range("A" & iLine8.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine8.ToString).CopyFromRecordset(rss8)
        '            xlSheet.Range("B" & iLine8.ToString).WrapText = True
        '            xlSheet.Range("B" & iLine8 & ":" & "D" & iLine8).Merge()
        '            xlSheet.Range("A" & iLine8.ToString).rowheight = "35"

        '            strSqlQry = "select distinct isnull(q.Item1,''), isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from view_contracts_childpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1  and s.contractid= '" & hdncontractid.Value & "' and h.childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
        '            Dim rscp As New ADODB.Recordset
        '            rscp = GetResultAsRecordSet(strSqlQry)
        '            tm8 = rscp.RecordCount

        '            iLine8 = iLine8 + 2




        '            xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rscp)
        '            xlSheet.Range("A" & iLine8 - 1.ToString) = "Season"
        '            xlSheet.Range("A" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine8 - 1.ToString) = "From Date"
        '            xlSheet.Range("B" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine8 - 1.ToString) = "To Date"
        '            xlSheet.Range("C" & iLine8 - 1.ToString).font.bold = True

        '            xlSheet.Range("D" & iLine8 - 1.ToString) = "Date Manual From Date not linked to Seasons"
        '            xlSheet.Range("D" & iLine8 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine8 - 1.ToString).WrapText = True


        '            xlSheet.Range("E" & iLine8 - 1.ToString) = "Manual To Date not linked to Seasons"
        '            xlSheet.Range("E" & iLine8 - 1.ToString).WrapText = True
        '            xlSheet.Range("E" & iLine8 - 1.ToString).font.bold = True

        '            strSqlQry = "select  isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),'') from  view_contracts_childpolicy_dates where childpolicyid='" & dtcpi.Rows(i)("childpolicyid").ToString & "'"
        '            Dim rsm8 As New ADODB.Recordset
        '            rsm8 = GetResultAsRecordSet(strSqlQry)
        '            co8 = rsm8.RecordCount
        '            iLine8 = iLine8 + 1
        '            xlSheet.Range("D" & iLine8.ToString).CopyFromRecordset(rsm8)

        '            If conn1.State = ConnectionState.Open Then
        '                conn1.Close()
        '            End If



        '            Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        '            Dim constring As String = ConfigurationManager.ConnectionStrings(strConnName).ConnectionString
        '            Dim dtt As New DataTable
        '            Using con As New SqlConnection(constring)
        '                Using cmd1 As New SqlCommand("[sp_print_childpolicy]")
        '                    cmd1.CommandType = CommandType.StoredProcedure
        '                    cmd1.Parameters.AddWithValue("@childpolicyid", dtcpi.Rows(i)("childpolicyid").ToString)

        '                    Using sda As New SqlDataAdapter()
        '                        cmd1.Connection = con
        '                        sda.SelectCommand = cmd1

        '                        sda.Fill(dtt)

        '                    End Using
        '                End Using
        '            End Using


        '            Dim rssp721 As New ADODB.Recordset
        '            rssp721 = convertToADODB(dtt)
        '            If rssp721.RecordCount > 0 Then
        '                Dim Maxint As Integer = Math.Max(chk8, Math.Max(co8, tm8))


        '                iLine8 = iLine8 + Maxint + 1

        '                Dim ii2 As Integer = 65
        '                'For Each column As DataColumn In dt.Columns
        '                'name(ii) = column.ColumnName

        '                Dim sss2 = Chr(ii2).ToString

        '                For OO As Integer = 0 To dtt.Columns.Count - 1
        '                    xlSheet.Range(sss2.ToString() + iLine8.ToString).Value() = dtt.Columns(OO).ColumnName.ToString()
        '                    ii2 += 1
        '                    xlSheet.Range(sss2.ToString() + (iLine8).ToString).FONT.BOLD = True
        '                    sss2 = Chr(ii2).ToString

        '                Next


        '                iLine8 = iLine8 + 1

        '                xlSheet.Range("A" & iLine8.ToString).CopyFromRecordset(rssp721)
        '                ei31 = rssp721.RecordCount

        '            End If

        '            iLine8 = iLine8 + ei31 + 4

        '        Next

        '    End If

        '    xlSheet = xlBook.Worksheets(9)
        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value

        '    Dim dtcn As New DataTable
        '    strSqlQry = "select cancelpolicyid from view_contracts_cancelpolicy_header  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtcn)
        '    Dim iLinecr2 As Integer = 8


        '    If dtcn.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtcn.Rows.Count - 1

        '            Dim rm2 As Integer
        '            Dim ml2 As Integer

        '            Dim co2 As Integer
        '            Dim ce2 As Integer

        '            Dim ns As Integer
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Cancellation ID"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "Season"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Pricelist From Date"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Pricelist To Date"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString) = "Exhibition Name"
        '            xlSheet.Range("G" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLinecr2 - 1.ToString) = "From"
        '            xlSheet.Range("H" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLinecr2 - 1.ToString) = "To"
        '            xlSheet.Range("I" & iLinecr2 - 1.ToString).font.bold = True


        '            strSqlQry = "select distinct d.cancelpolicyid,d.applicableto,q.Item1, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105) from view_contracts_cancelpolicy_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q inner join  view_contractseasons s on s.SeasonName=q.Item1 and s.contractid='" & hdncontractid.Value & "' and  d.cancelpolicyid ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '            Dim rscp As New ADODB.Recordset
        '            rscp = GetResultAsRecordSet(strSqlQry)
        '            co2 = rscp.RecordCount
        '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rscp)

        '            strSqlQry = "select d.exhibitioncode,m.exhibitionname,isnull(d.fromdate,''),isnull(d.todate,'') from view_contracts_cancelpolicy_header h cross apply dbo.SplitString1colsWithOrderField(h.exhibitions,',') e inner join view_contracts_exhibition_detail d on e.Item1=d.exhibitioncode inner join view_contracts_exhibition_header eh on d.exhibitionid=eh.exhibitionid and eh.contractid=h.contractid inner join exhibition_master m on d.exhibitioncode=m.exhibitioncode where h.contractid='" & hdncontractid.Value & "' and h.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '            Dim rs2e As New ADODB.Recordset
        '            rs2e = GetResultAsRecordSet(strSqlQry)
        '            ce2 = rs2e.RecordCount
        '            xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rs2e)


        '            strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_header  h join view_contracts_search v on h.contractid =v.contractid join view_contracts_cancelpolicy_detail d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'  for xml path('')),1,1,''),'') "
        '            Dim rs2r As New ADODB.Recordset
        '            rs2r = GetResultAsRecordSet(strSqlQry)
        '            rm2 = rs2r.RecordCount
        '            If ce2 Or co2 <> 0 Then
        '                If ce2 > co2 Then
        '                    iLinecr2 = iLinecr2 + ce2 + 2
        '                ElseIf co2 > ce2 Then
        '                    iLinecr2 = iLinecr2 + co2 + 2
        '                End If
        '            Else

        '                iLinecr2 = iLinecr2 + 4
        '            End If
        '            xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rs2r)
        '            xlSheet.Range("A" & iLinecr2.ToString).wraptext = True
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True

        '            strSqlQry = "select mealplans,nodayshours, dayshours,isnull(chargebasis,'')chargebasis, percentagetocharge from view_contracts_cancelpolicy_detail where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"

        '            Dim rsr2 As New ADODB.Recordset
        '            rsr2 = GetResultAsRecordSet(strSqlQry)
        '            ml2 = rsr2.RecordCount
        '            If rm2 <> 0 Then

        '                'If ce2 Or co2 <> 0 Then
        '                '    If ce2 > co2 Then
        '                '        iLinecr2 = iLinecr2
        '                '    ElseIf co2 > ce2 Then
        '                '        iLinecr2 = iLinecr2
        '                '    End If
        '                'Else

        '                iLinecr2 = iLinecr2

        '            ElseIf ce2 Or co2 <> 0 Then
        '                If ce2 > co2 Then
        '                    iLinecr2 = iLinecr2 + ce2 + 2
        '                ElseIf co2 > ce2 Then
        '                    iLinecr2 = iLinecr2 + co2 + 2
        '                End If
        '            Else
        '                iLinecr2 = iLinecr2 + 2
        '            End If
        '            xlSheet.Range("B" & iLinecr2.ToString).CopyFromRecordset(rsr2)
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "No.of Days or Hours"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Unit -Days/Hours"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Charge Basis"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString) = "Percentage to charge"
        '            xlSheet.Range("F" & iLinecr2 - 1.ToString).font.bold = True
        '            'strSqlQry = "select  distinct p.rmtypname from view_contracts_cancelpolicy_noshowearly  h  join view_contracts_cancelpolicy_detail  d on h.cancelpolicyid =d.cancelpolicyid cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =d.partycode  where d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '            'Dim rsr4 As New ADODB.Recordset
        '            'rsr4 = GetResultAsRecordSet(strSqlQry)
        '            'rm2 = rsr4.RecordCount
        '            'If ce2 Or co2 <> 0 Then
        '            '    If ce2 > co2 Then
        '            '        iLinecr2 = iLinecr2 + ce2 + 2
        '            '    ElseIf co2 > ce2 Then
        '            '        iLinecr2 = iLinecr2 + co2 + 2
        '            '    End If
        '            'Else
        '            '    iLinecr2 = iLinecr2 + 3
        '            'End If
        '            'xlSheet.Range("H" & iLinecr2.ToString).CopyFromRecordset(rsr4)

        '            strSqlQry = "select noshowearly from view_contracts_cancelpolicy_noshowearly  where cancelpolicyid  ='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"


        '            Dim dtns As New DataTable
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dtns)
        '            iLinecr2 = iLinecr2 + 3
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString) = "Room Type"
        '            xlSheet.Range("A" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString) = "No Show/Early Checkout"
        '            xlSheet.Range("B" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString) = "Charge Basis"
        '            xlSheet.Range("D" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString) = "Percentage to charge"
        '            xlSheet.Range("E" & iLinecr2 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("C" & iLinecr2 - 1.ToString).font.bold = True


        '            If dtns.Rows.Count > 0 Then
        '                For i2 As Integer = 0 To dtns.Rows.Count - 1

        '                    strSqlQry = "select  distinct roomtypes=isnull(stuff((select ',' +  p.rmtypname  from view_contracts_cancelpolicy_noshowearly d cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q  inner join partyrmtyp p on p.rmtypcode= q.item1  and d.cancelpolicyid= '" & dtcn.Rows(i)("cancelpolicyid").ToString & "' and d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'  and p.partycode='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),''),d.noshowearly, mealplans,chargebasis, percentagetocharge from view_contracts_cancelpolicy_noshowearly d where d.noshowearly='" & dtns.Rows(i2)("noshowearly").ToString & "'   and d.cancelpolicyid='" & dtcn.Rows(i)("cancelpolicyid").ToString & "'"
        '                    Dim rsrns4 As New ADODB.Recordset
        '                    rsrns4 = GetResultAsRecordSet(strSqlQry)
        '                    ns = rsrns4.RecordCount
        '                    xlSheet.Range("A" & iLinecr2.ToString).CopyFromRecordset(rsrns4)
        '                    xlSheet.Range("A" & iLinecr2.ToString).WrapText = True
        '                    iLinecr2 = iLinecr2 + 1
        '                Next
        '            End If
        '            If ml2 Or rm2 <> 0 Then
        '                If ml2 > rm2 Then
        '                    iLinecr2 = iLinecr2 + ml2 + 2

        '                Else
        '                    iLinecr2 = iLinecr2 + rm2 + 2
        '                End If
        '            Else

        '                iLinecr2 = iLinecr2 + 2 + rm2

        '            End If


        '            iLinecr2 = iLinecr2 + ns + 3

        '        Next
        '    End If




        '    xlSheet = xlBook.Worksheets(10)
        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value



        '    Dim dtc As New DataTable
        '    strSqlQry = "select checkinoutpolicyid from view_contracts_checkinout_header  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtc)
        '    Dim linelbct As Integer = 8

        '    If dtc.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtc.Rows.Count - 1

        '            Dim rm As Integer
        '            Dim ml As Integer
        '            Dim tm As Integer
        '            Dim co As Integer
        '            Dim de As Integer
        '            Dim chk As Integer



        '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/OutPolicyId"
        '            xlSheet.Range("C" & linelbct - 1.ToString) = "Season"
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "Applicable To"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & linelbct - 1.ToString) = "Pricelist From Date"
        '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & linelbct - 1.ToString) = "Pricelist To Date	"
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & linelbct - 1.ToString).font.bold = True

        '            strSqlQry = "select distinct d.checkinoutpolicyid,d.applicableto,q.Item1 seasons, convert(varchar(10),s.fromdate , 105),convert(varchar(10),s.todate , 105)  from view_contracts_checkinout_header d cross apply dbo.SplitString1colsWithOrderField(d.seasons,',')q  join  view_contractseasons s on s.SeasonName=q.Item1 and  s.contractid= '" & hdncontractid.Value & "' and  d.checkinoutpolicyid  ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' "
        '            Dim rscp As New ADODB.Recordset
        '            rscp = GetResultAsRecordSet(strSqlQry)
        '            chk = rscp.RecordCount

        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rscp)



        '            strSqlQry = "select checkintime,checkouttime from view_contracts_checkinout_header where checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rsc As New ADODB.Recordset
        '            rsc = GetResultAsRecordSet(strSqlQry)
        '            co = rsc.RecordCount
        '            If chk = 0 Then
        '                linelbct = linelbct + 2
        '            Else
        '                linelbct = linelbct + chk + 2
        '            End If

        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsc)
        '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn Time"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "CheckOut Time"
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True


        '            strSqlQry = "select isnull(stuff((select ',' + mealcode from view_contracts_checkinout_mealplans where  checkinoutpolicyid ='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' for xml path('')),1,1,''),'') "
        '            Dim rscm As New ADODB.Recordset
        '            rscm = GetResultAsRecordSet(strSqlQry)
        '            ml = rscm.RecordCount
        '            If co = 0 Then
        '                linelbct = linelbct + 1
        '            Else
        '                linelbct = linelbct + co + 1
        '            End If
        '            xlSheet.Range("B" & linelbct.ToString).CopyFromRecordset(rscm)
        '            xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
        '            'xlSheet.Range("B:C").Merge()

        '            xlSheet.Range("A" & linelbct.ToString) = "Meal Plan"
        '            xlSheet.Range("A" & linelbct.ToString).font.bold = True
        '            strSqlQry = "select  distinct isnull(stuff((select ',' +  p.rmtypname  from view_contracts_checkinout_roomtypes d cross apply dbo.SplitString1colsWithOrderField(d.rmtypcode,',') q  inner join partyrmtyp p on p.rmtypcode= d.rmtypcode  and d.checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "' and partycode='" & CType(Session("Contractparty"), String) & "' for xml path('')),1,1,''),'') "
        '            Dim rscr As New ADODB.Recordset
        '            rscr = GetResultAsRecordSet(strSqlQry)
        '            rm = rscr.RecordCount

        '            linelbct = linelbct + 1

        '            xlSheet.Range("B" & linelbct.ToString).CopyFromRecordset(rscr)
        '            'xlSheet.Range("B" & linelbct & ":" & "D" & linelbct).Merge()
        '            'xlSheet.Range("e:C").Merge()
        '            xlSheet.Range("B" & linelbct.ToString).wraptext = True
        '            xlSheet.Range("A" & linelbct.ToString) = "Room Type"
        '            xlSheet.Range("A" & linelbct.ToString).font.bold = True

        '            strSqlQry = "select checkinouttype,	isnull(fromhours,''),isnull(tohours,''),chargeyesno,chargetype,percentage,condition,isnull(requestbeforedays,'') from view_contracts_checkinout_detail where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rst As New ADODB.Recordset
        '            rst = GetResultAsRecordSet(strSqlQry)
        '            tm = rst.RecordCount

        '            linelbct = linelbct + 3

        '            xlSheet.Range("A" & linelbct - 1.ToString) = "CheckIn/CheckoutType"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "From"
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & linelbct - 1.ToString) = "To"
        '            xlSheet.Range("D" & linelbct - 1.ToString) = "Charge Y/N"
        '            xlSheet.Range("E" & linelbct - 1.ToString) = "Charge Type"
        '            xlSheet.Range("F" & linelbct - 1.ToString) = "Percentage"
        '            xlSheet.Range("G" & linelbct - 1.ToString) = "Conditions"
        '            xlSheet.Range("H" & linelbct - 1.ToString) = "Requestbeforedays"
        '            xlSheet.Range("H" & linelbct - 1.ToString).wraptext = True
        '            xlSheet.Range("H" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rst)

        '            strSqlQry = "select isnull(datetype,''),isnull(convert(varchar(10),restrictdate, 105),'')restrictdate from view_contracts_checkinout_restricted where checkinoutpolicyid='" & dtc.Rows(i)("checkinoutpolicyid").ToString & "'"
        '            Dim rsd As New ADODB.Recordset
        '            rsd = GetResultAsRecordSet(strSqlQry)
        '            de = rsd.RecordCount
        '            If tm = 0 Then
        '                linelbct = linelbct + 2
        '            Else
        '                linelbct = linelbct + tm + 2

        '            End If
        '            xlSheet.Range("A" & linelbct.ToString).CopyFromRecordset(rsd)
        '            xlSheet.Range("A" & linelbct - 1.ToString) = "Date Type"
        '            xlSheet.Range("B" & linelbct - 1.ToString) = "Restrict Date"
        '            xlSheet.Range("A" & linelbct - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & linelbct - 1.ToString).font.bold = True
        '            linelbct = linelbct + de + 3


        '        Next
        '    End If

        '    xlSheet = xlBook.Worksheets(11)
        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value


        '    Dim iLineg As Integer = 9


        '    Dim dtg As New DataTable
        '    strSqlQry = "select genpolicyid from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtg)
        '    Dim g As Integer

        '    If dtg.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtg.Rows.Count - 1
        '            xlSheet.Range("A" & iLineg.ToString) = "Genpolicy Id"
        '            xlSheet.Range("A" & iLineg.ToString).font.bold = True
        '            xlSheet.Range("B" & iLineg.ToString).font.bold = True
        '            xlSheet.Range("C" & iLineg.ToString).font.bold = True
        '            xlSheet.Range("D" & iLineg.ToString).font.bold = True
        '            xlSheet.Range("B" & iLineg.ToString) = "Applicable To"
        '            xlSheet.Range("C" & iLineg.ToString) = "To Date"
        '            xlSheet.Range("D" & iLineg.ToString) = "From Date "



        '            strSqlQry = "select distinct genpolicyid,applicableto,isnull(fromdate,''),isnull(todate,'') from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "'"
        '            Dim rsg As New ADODB.Recordset
        '            rsg = GetResultAsRecordSet(strSqlQry)
        '            g = rsg.RecordCount
        '            xlSheet.Range("A" & iLineg.ToString).CopyFromRecordset(rsg)

        '            Dim y As Integer
        '            strSqlQry = "select isnull(policytext,'') from view_contracts_genpolicy_header  where contractid= '" & hdncontractid.Value & "' and genpolicyid= '" & dtg.Rows(i)("genpolicyid").ToString & "' "
        '            Dim rsp As New ADODB.Recordset
        '            rsp = GetResultAsRecordSet(strSqlQry)
        '            y = rsp.RecordCount

        '            iLineg = iLineg + g + 2
        '            xlSheet.Range("A" & iLineg.ToString).CopyFromRecordset(rsp)
        '            xlSheet.Range("A" & iLineg - 1.ToString) = "Policy"
        '            xlSheet.Range("A" & iLineg - 1.ToString).font.bold = True
        '            xlSheet.Range("A" & iLineg - 1.ToString).WrapText = True
        '            'xlSheet.Range("A14").WrapText = True
        '            '("A"iLineg&char(70)iLineg).Merge()

        '            xlSheet.Range("A" & iLineg & ":" & "J" & iLineg).Merge()



        '            'xlSheet.Range("A14:I14").Merge()

        '            If g > y Then
        '                iLineg = iLineg + g + 3
        '            Else

        '                iLineg = iLineg + y + 3
        '            End If



        '        Next
        '    End If



        '    xlSheet = xlBook.Worksheets(12)
        '    xlSheet.Range("B3").Value = ViewState("hotelname")
        '    xlSheet.Range("B4").Value = hdncontractid.Value


        '    Dim dtm As New DataTable
        '    strSqlQry = "select minnightsid from view_contracts_minnights_header where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dtm)



        '    Dim iLiner As Integer = 8


        '    If dtm.Rows.Count > 0 Then
        '        For i As Integer = 0 To dtm.Rows.Count - 1
        '            xlSheet.Range("A" & iLiner - 1.ToString) = "Minnight Id"
        '            xlSheet.Range("A" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLiner - 1.ToString) = "Applicable To"
        '            xlSheet.Range("B" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLiner - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLiner - 1.ToString) = "RoomType"
        '            xlSheet.Range("D" & iLiner - 1.ToString) = "Meal Plan"
        '            xlSheet.Range("E" & iLiner - 1.ToString) = "From Date"
        '            xlSheet.Range("F" & iLiner - 1.ToString) = "To Date"

        '            xlSheet.Range("G" & iLiner - 1.ToString) = "Min.Nights"
        '            xlSheet.Range("H" & iLiner - 1.ToString) = " Options"

        '            Dim dtm2 As New DataTable
        '            strSqlQry = "select clineno from view_contracts_minnights_detail where minnightsid= '" & dtm.Rows(i)("minnightsid").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dtm2)

        '            Dim x As Integer
        '            Dim x1 As Integer

        '            If dtm2.Rows.Count > 0 Then
        '                For i2 As Integer = 0 To dtm2.Rows.Count - 1

        '                    strSqlQry = "select isnull(stuff((select ',' +  p.rmtypname  from view_contracts_minnights_header h join view_contracts_search v on h.contractid =v.contractid join view_contracts_minnights_detail d on h.minnightsid =d.minnightsid  cross apply dbo.splitallotmkt(d.roomtypes,',') dm inner join partyrmtyp p on p.rmtypcode =dm.mktcode and p.partycode =v.partycode where d.minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'  for xml path('')),1,1,''),'') "
        '                    Dim rsm1 As New ADODB.Recordset
        '                    rsm1 = GetResultAsRecordSet(strSqlQry)
        '                    x1 = rsm1.RecordCount
        '                    xlSheet.Range("C" & iLiner.ToString).CopyFromRecordset(rsm1)

        '                    strSqlQry = "select distinct D.minnightsid ,H.applicableto from view_contracts_minnights_detail d,view_contracts_minnights_header h where d.minnightsid ='" & dtm.Rows(i)("minnightsid").ToString & "' and d.clineno='" & dtm2.Rows(i2)("clineno").ToString & "' and contractid= '" & hdncontractid.Value & "'"
        '                    Dim rsm As New ADODB.Recordset
        '                    rsm = GetResultAsRecordSet(strSqlQry)
        '                    x = rsm.RecordCount
        '                    xlSheet.Range("A" & iLiner.ToString).CopyFromRecordset(rsm)


        '                    Dim y As Integer
        '                    strSqlQry = "select distinct mealplans, isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),isnull(minnights,''),nightsoption from view_contracts_minnights_detail where minnightsid='" & dtm.Rows(i)("minnightsid").ToString & "' and clineno='" & dtm2.Rows(i2)("clineno").ToString & "'"
        '                    Dim rsr As New ADODB.Recordset
        '                    rsr = GetResultAsRecordSet(strSqlQry)
        '                    y = rsm.RecordCount
        '                    xlSheet.Range("D" & iLiner.ToString).CopyFromRecordset(rsr)

        '                    iLiner = iLiner + 1
        '                Next

        '            End If
        '            iLiner = iLiner + 2
        '        Next
        '    End If

        '    xlSheet = xlBook.Worksheets(13)

        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value




        '    Dim dts As New DataTable
        '    strSqlQry = "select splistcode from  view_contracts_specialevents_header where contractid= '" & hdncontractid.Value & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dts)
        '    Dim iLines As Integer = 9
        '    If dts.Rows.Count > 0 Then
        '        For i As Integer = 0 To dts.Rows.Count - 1
        '            xlSheet.Range("A" & iLines - 1.ToString) = "Splistcode"
        '            xlSheet.Range("A" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("E" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("F" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("G" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("H" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("I" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("J" & iLines - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLines - 1.ToString) = "Applicable To"
        '            xlSheet.Range("C" & iLines - 1.ToString) = "From Date"
        '            xlSheet.Range("D" & iLines - 1.ToString) = "To Date"
        '            xlSheet.Range("E" & iLines - 1.ToString) = "SplEvents Name"
        '            xlSheet.Range("F" & iLines - 1.ToString) = "RoomType"
        '            xlSheet.Range("G" & iLines - 1.ToString) = "Meal Plans"
        '            xlSheet.Range("H" & iLines - 1.ToString) = "Room Occupancy"
        '            xlSheet.Range("I" & iLines - 1.ToString) = "Adult Rate"
        '            xlSheet.Range("J" & iLines - 1.ToString) = "Child Age From,Child Age To,Child Rate"
        '            xlSheet.Range("J" & iLines - 1.ToString).wraptext = True
        '            xlSheet.Range("J" & iLines - 1.ToString).font.bold = True

        '            Dim x As Integer
        '            strSqlQry = "select distinct h.splistcode,h.applicableto,d.fromdate,d.todate,p.spleventname from view_contracts_specialevents_detail d join party_splevents  p on d.spleventcode= p.spleventcode join view_contracts_specialevents_header h on h.splistcode=d.splistcode and p.partycode= '" & CType(Session("Contractparty"), String) & "'  and d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '            '" & dtm.Rows(i)("minnightsid").ToString & "'"
        '            Dim rsm As New ADODB.Recordset
        '            rsm = GetResultAsRecordSet(strSqlQry)
        '            x = rsm.RecordCount
        '            xlSheet.Range("A" & iLines.ToString).CopyFromRecordset(rsm)


        '            Dim dters As New DataTable
        '            strSqlQry = "select distinct roomtypes,mealplans,splineno,roomcategory from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '            myDataAdapter.Fill(dters)

        '            Dim ys2 As Integer
        '            Dim xs As Integer
        '            Dim ys As Integer
        '            If dters.Rows.Count > 0 Then
        '                For ers As Integer = 0 To dters.Rows.Count - 1


        '                    If dters.Rows(ers)("roomtypes").ToString = "All" Then


        '                        strSqlQry = "select roomtypes from view_contracts_specialevents_detail  where  roomtypes='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '                        xlSheet.Range("F" & iLines.ToString) = "All"
        '                        iLines = iLines

        '                    Else

        '                        strSqlQry = "select isnull(stuff((select ',' +   p.rmtypname from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomtypes,',') q join partyrmtyp p on q.Item1=p.rmtypcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),'') "
        '                        Dim rsers As New ADODB.Recordset
        '                        rsers = GetResultAsRecordSet(strSqlQry)
        '                        ys = rsers.RecordCount
        '                        iLines = iLines

        '                        xlSheet.Range("F" & iLines.ToString).CopyFromRecordset(rsers)
        '                        xlSheet.Range("F" & iLines.ToString).WrapText = True
        '                        xlSheet.Range("F" & iLines.ToString).rowheight = 100
        '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        iLines = iLines
        '                    End If


        '                    If dters.Rows(ers)("mealplans").ToString = "All" Then


        '                        strSqlQry = "select mealplans from view_contracts_specialevents_detail  where  mealplans='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '                        xlSheet.Range("G" & iLines.ToString) = "All"
        '                        iLines = iLines

        '                    Else

        '                        strSqlQry = "select isnull(stuff((select ',' +  mealplans from view_contracts_specialevents_detail where  splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and splineno ='" & dters.Rows(ers)("splineno").ToString & "' for xml path('')),1,1,''),'') "
        '                        Dim rserss As New ADODB.Recordset
        '                        rserss = GetResultAsRecordSet(strSqlQry)
        '                        ys2 = rserss.RecordCount
        '                        xlSheet.Range("G" & iLines.ToString).CopyFromRecordset(rserss)
        '                        xlSheet.Range("G" & iLines.ToString).WrapText = True
        '                        xlSheet.Range("G" & iLines.ToString).rowheight = 100
        '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        iLines = iLines
        '                    End If

        '                    If dters.Rows(ers)("roomcategory").ToString = "All" Then


        '                        strSqlQry = "select roomcategory from view_contracts_specialevents_detail  where  roomcategory='All' and  splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '                        xlSheet.Range("H" & iLines.ToString) = "All"
        '                        iLines = iLines

        '                    Else

        '                        strSqlQry = "select isnull(stuff((select ',' +   p.rmcatcode from view_contracts_specialevents_detail d  cross apply dbo.SplitString1colsWithOrderField(d.roomcategory,',') q join partyrmcat p on q.Item1=p.rmcatcode and  d.splistcode='" & dts.Rows(i)("splistcode").ToString & "'  and p.partycode ='" & CType(Session("Contractparty"), String) & "'  for xml path('')),1,1,''),'') "
        '                        Dim rsersr As New ADODB.Recordset
        '                        rsersr = GetResultAsRecordSet(strSqlQry)

        '                        iLines = iLines
        '                        xlSheet.Range("H" & iLines.ToString).CopyFromRecordset(rsersr)
        '                        xlSheet.Range("H" & iLines.ToString).rowheight = 100
        '                        xlSheet.Range("H" & iLines.ToString).WrapText = True
        '                        'xlSheet.Range("F" & iLinee.ToString).rowwidth = "100"
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        'xlSheet.Range("F" & iLinee.ToString & ":F" & (iLine + y).ToString).Merge()
        '                        iLines = iLines
        '                    End If


        '                Next

        '            End If


        '            Dim y As Integer
        '            strSqlQry = "select distinct adultrate,childdetails from view_contracts_specialevents_detail where splistcode='" & dts.Rows(i)("splistcode").ToString & "'"
        '            Dim rsr As New ADODB.Recordset
        '            rsr = GetResultAsRecordSet(strSqlQry)
        '            iLines = iLines
        '            y = rsr.RecordCount
        '            xlSheet.Range("I" & iLines.ToString).CopyFromRecordset(rsr)


        '            'Dim intmax, intmin As Integer
        '            'intmax = x
        '            'intmin=
        '            If x > y Then
        '                iLines = iLines + x + 2
        '            Else
        '                iLines = iLines + y + 2

        '            End If
        '            'Dim z As Integer = Maxcount(x, y)
        '        Next
        '    End If

        '    xlSheet = xlBook.Worksheets(14)
        '    xlSheet.Range("B4").Value = ViewState("hotelname")
        '    xlSheet.Range("B5").Value = hdncontractid.Value



        '    Dim iLine5 As Integer = 10
        '    Dim dt4 As New DataTable
        '    strSqlQry = "select h.constructionid from hotels_construction h join partymast p on p.partycode=h.partycode and p.partyname='" & ViewState("hotelname") & "'"
        '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        '    myDataAdapter.Fill(dt4)

        '    If dt4.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt4.Rows.Count - 1

        '            xlSheet.Range("A" & iLine5 - 1.ToString) = "Construction Id"
        '            xlSheet.Range("A" & iLine5 - 1.ToString).font.bold = True
        '            xlSheet.Range("B" & iLine5 - 1.ToString) = "From Date"
        '            xlSheet.Range("B" & iLine5 - 1.ToString).font.bold = True
        '            xlSheet.Range("C" & iLine5 - 1.ToString) = "To Date"
        '            xlSheet.Range("C" & iLine5 - 1.ToString).font.bold = True
        '            xlSheet.Range("D" & iLine5 - 1.ToString) = "Reason"
        '            xlSheet.Range("D" & iLine5 - 1.ToString).font.bold = True
        '            Dim x As Integer
        '            strSqlQry = "select constructionid, isnull(convert(varchar(10),fromdate , 105),'') ,isnull(convert(varchar(10),todate , 105),''),isnull(Reason,'') from hotels_construction where constructionid  ='" & dt4.Rows(i)("constructionid").ToString & "'"
        '            Dim rs5 As New ADODB.Recordset
        '            rs5 = GetResultAsRecordSet(strSqlQry)

        '            x = rs5.RecordCount
        '            xlSheet.Range("A" & iLine5.ToString).CopyFromRecordset(rs5)
        '            iLine5 = iLine5 + x + 2
        '        Next
        '    End If

        '    '''' Final Calculated Rate
        '    '''' Final Calculated Rate

        '    '''' Final Calculated Rate
        '    Dim sellingexists As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_final_contracted_rates where contractid='" & hdncontractid.Value & "'")
        '    If sellingexists <> "" Then

        '        xlSheet = xlBook.Worksheets(15)
        '        xlSheet.Range("B4").Value = ViewState("hotelname")
        '        xlSheet.Range("B5").Value = hdncontractid.Value


        '        strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan,accommodationid,agecombination,adults,child,totalpaxwithinpricepax,maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,totalprice,nights,minstay,minstayoption,commissionformulaid,commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder from view_final_contracted_rates where contractid = '" & hdncontractid.Value & "' order by rmtyporder,rmcatorder,agecombination,pfromdate"
        '        Dim rsrf As New ADODB.Recordset
        '        Dim yf As Integer
        '        rsrf = GetResultAsRecordSet(strSqlQry)
        '        Dim iLinesf As Integer = 8


        '        yf = rsrf.RecordCount

        '        xlSheet.Range("A" & iLinesf - 1.ToString) = "plistcode"
        '        xlSheet.Range("B" & iLinesf - 1.ToString) = "Roomtype"
        '        xlSheet.Range("C" & iLinesf - 1.ToString) = "Roomcategory"
        '        xlSheet.Range("D" & iLinesf - 1.ToString) = "Mealplan"
        '        xlSheet.Range("E" & iLinesf - 1.ToString) = "Accommodationid"
        '        xlSheet.Range("E" & iLinesf - 1.ToString).wraptext = True
        '        xlSheet.Range("F" & iLinesf - 1.ToString) = "Agecombination"
        '        xlSheet.Range("F" & iLinesf - 1.ToString).wraptext = True
        '        xlSheet.Range("G" & iLinesf - 1.ToString) = "Adults"


        '        xlSheet.Range("J" & iLinesf - 1.ToString).wraptext = True

        '        xlSheet.Range("AA" & iLinesf - 1.ToString).wraptext = True

        '        xlSheet.Range("A" & iLinesf.ToString).CopyFromRecordset(rsrf)


        '        ''''''''End Final Calculated Rate

        '        '--- Contract Rates for Other Meal Plan

        '        xlSheet = xlBook.Worksheets(16)
        '        xlSheet.Range("B4").Value = ViewState("hotelname")
        '        xlSheet.Range("B5").Value = hdncontractid.Value


        '        strSqlQry = "select plistcode,rmtypname as roomtype , rmcatcode as roomcategory,mealcode as mealplan, basemeal ,accommodationid,agecombination,adults,child,totalpaxwithinpricepax, " _
        '            & "maxeb,noofadulteb,pfromdate,ptodate,grr,npr,exhibitionprice,adultebprice,extrapaxprice,totalsharingcharge,totalebcharge,mealsupplementid,adultmealprice,adultmealrmcatdetails, " _
        '            & " totalchildmealcharge,childmealdetails,totalprice,nights,minstay,minstayoption,commissionformulaid, " _
        '            & " commissionformulastring,exhibitionids,pricepax,childpolicyid,rmtyporder,rmcatorder,othmealcode from view_final_contracted_rates_othmeal where contractid = '" & hdncontractid.Value & "' order by rmtyporder, " _
        '            & " rmcatorder,agecombination,pfromdate"
        '        Dim rsrfo As New ADODB.Recordset
        '        Dim yfo As Integer
        '        rsrfo = GetResultAsRecordSet(strSqlQry)
        '        Dim iLinesfo As Integer = 8


        '        yf = rsrfo.RecordCount


        '        If rsrfo.RecordCount > 0 Then

        '            xlSheet.Range("A" & iLinesfo.ToString).CopyFromRecordset(rsrfo)
        '        End If
        '    End If

        '    '''''''''''





        '    FolderPath = "~\ExcelTemp\"
        '    Dim FileNameNew As String = "ContractPrint_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
        '    Dim outputPath As String = Server.MapPath(FolderPath + FileNameNew)
        '    'Delete all files from Temp folder
        '    Try

        '        Dim outputPath1 As String = Server.MapPath(FolderPath)
        '        If Directory.Exists(outputPath1) Then

        '            Dim files() As String
        '            files = Directory.GetFileSystemEntries(outputPath1)

        '            For Each element As String In files
        '                If (Not Directory.Exists(element)) Then
        '                    File.Delete(Path.Combine(outputPath1, Path.GetFileName(element)))
        '                End If
        '            Next
        '        End If
        '    Catch ex As Exception

        '    End Try
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


        '    ' Response.Clear()
        '    ' Response.ContentType = "application/vnd.ms-excel"
        '    ' Response.AddHeader("content-disposition", "attachment;filename=" & FileNameNew)
        '    ' Response.WriteFile(outputPath)
        '    ' Response.End()




        '    objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Exported succesfully : ", Session("GlobalUserName"))

        'Catch ex As Exception
        '    objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), "Full : " & ex.Message.ToString, Session("GlobalUserName"))
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try
    End Sub

    Public Function convertToADODB(ByRef table As DataTable) As ADODB.Recordset


        Dim result As New ADODB.Recordset
        result.CursorLocation = CursorLocationEnum.adUseClient
        Dim resultFields As ADODB.Fields = result.Fields
        Dim col As DataColumn
        For Each col In table.Columns
            resultFields.Append(col.ColumnName, TranslateType(col.DataType),
                    col.MaxLength, col.AllowDBNull = ADODB.FieldAttributeEnum.adFldIsNullable)
        Next
        'result.Open(System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)
        result.Open(System.Reflection.Missing.Value, System.Reflection.Missing.Value, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, 0)

        For Each row As DataRow In table.Rows
            result.AddNew(System.Reflection.Missing.Value, System.Reflection.Missing.Value)

            For i As Integer = 0 To table.Columns.Count - 1
                resultFields(i).Value = row(i)
            Next
        Next
        Return (result)
    End Function
    Public Function TranslateType(ByRef type As Type) As ADODB.DataTypeEnum
        Try


            Select Case type.UnderlyingSystemType.ToString
                Case "System.Boolean"
                    Return ADODB.DataTypeEnum.adBoolean
                Case "System.Byte"
                    Return ADODB.DataTypeEnum.adUnsignedTinyInt
                Case "System.Char"
                    Return ADODB.DataTypeEnum.adChar
                Case "System.DateTime"
                    Return ADODB.DataTypeEnum.adDate
                Case "System.Decimal"
                    Return ADODB.DataTypeEnum.adCurrency

                Case "System.Double"


                    Return ADODB.DataTypeEnum.adDouble
                Case "System.Int16"
                    Return ADODB.DataTypeEnum.adSmallInt
                Case "System.Int32"
                    Return ADODB.DataTypeEnum.adInteger
                Case "System.Int64"
                    Return ADODB.DataTypeEnum.adBigInt
                Case "System.SByte"
                    Return ADODB.DataTypeEnum.adTinyInt
                Case "System.Single"
                    Return ADODB.DataTypeEnum.adSingle
                Case ("System.UInt16")
                    Return ADODB.DataTypeEnum.adUnsignedSmallInt
                Case "System.UInt32"
                    Return ADODB.DataTypeEnum.adUnsignedInt
                Case "System.UInt64"
                    Return ADODB.DataTypeEnum.adUnsignedBigInt
                Case "System.String"
                    '  Case default
                    Return ADODB.DataTypeEnum.adVarWChar
            End Select
        Catch ex As Exception

        End Try
    End Function




    Public Sub KillUnusedExcelProcess()
        'Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")
        'For Each oXLP As Process In oXlProcess
        '    If Len(oXLP.MainWindowTitle) = 0 Then
        '        oXLP.Kill()
        '    End If
        'Next
    End Sub
    Private Sub ExcelOpen(ByVal openpath As String)
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Dim oExcel As Object = Nothing
        Try
            If Dir(Trim(openpath)) = "" Then
                MsgBox("No Such Record ", , "Excel Export")
                Exit Sub
            End If
            oExcel = CreateObject("Excel.Application")
            oExcel.Workbooks.Open(Trim(openpath))
            oExcel.Visible = True
            oExcel.UserControl = True
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch excc As Exception
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Finally
            If oExcel IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub
    Public Function RecordSetToDataTable(ByVal objRS As ADODB.Recordset) As DataTable
        '****************************************
        '*** Code from VisibleVisual.com ********
        '****************************************

        Dim objDA As New OleDbDataAdapter()
        Dim objDT As New DataTable()
        objDA.Fill(objDT, objRS)
        Return objDT
    End Function

    Public Function DataTableToRecordSet(ByVal objDT As DataTable) As ADODB.Recordset
        '****************************************
        '*** Code from VisibleVisual.com ********
        '****************************************
        Dim objDA As New OleDbDataAdapter()
        Dim objRS As New ADODB.Recordset()
        '   Dim objDT As New DataTable()
        objDA.Fill(objRS, objDT)
        Return objRS
    End Function


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

    Public Function GetResultAsRecordSet(ByVal strSqlQry As String) As ADODB.Recordset
        Try
            If conn1.State = ConnectionState.Open Then
                Call closeconnection()
            End If

            Dim rsNew As New ADODB.Recordset
            Dim oledbcon1 As String = ConfigurationManager.ConnectionStrings("strADODBConnection").ConnectionString
            conn1.Open(oledbcon1)
            conn1.CursorLocation = CursorLocationEnum.adUseClient
            rsNew = conn1.Execute(strSqlQry)
            ' conn1.Close()
            Return rsNew
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return Nothing
        End Try
    End Function
    Public Sub closeconnection()
        If conn1.State = ConnectionState.Open Then conn1.Close()
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

    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click


        saverates()
        ' ModalPopupError.Hide()
    End Sub
End Class
