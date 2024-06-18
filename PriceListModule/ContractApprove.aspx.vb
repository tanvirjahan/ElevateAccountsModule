Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.ArrayList
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports Microsoft.Office.Interop
Imports System.Data.OleDb
Imports System.Diagnostics
Imports ADODB

Partial Class ContractApprove
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private conn1 As New ADODB.Connection


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

    Dim objEmail As New clsEmail  ''' Added shahul
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
                lblHeading.Text = " Promotion Approval  - " + hotelname1 + " - " + hdnpromotionid.Value


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

                lblHeading.Text = " Contract Approval  - " + ViewState("hotelname") + " - " + hdncontractid.Value
            End If



        Else

        End If

        btnApprove.Style.Add("display", "none")
        btncalculaterate.Style.Add("display", "none")  '' Added shahul 21/06/18
        If Session("Calledfrom") = "Offers" Then
            btnchecking.Attributes.Add("onclick", "return CheckPromotion('" & hdnpromotionid.Value & "')")
        Else
            btnchecking.Attributes.Add("onclick", "return CheckContract('" & hdncontractid.Value & "')")
        End If


        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


        End If
        Session.Add("submenuuser", "ContractsSearch.aspx")
    End Sub
#End Region


#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex


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
            'strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            'strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
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

    Protected Sub ReadMoreLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim readmore As LinkButton = CType(sender, LinkButton)
            Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
            Dim lbtext As Label = CType(row.FindControl("lblerrormsg"), Label)
            Dim strtemp As String = ""
            strtemp = lbtext.Text
            If readmore.Text.ToUpper = UCase("More") Then

                lbtext.Text = lbtext.ToolTip
                lbtext.ToolTip = strtemp
                readmore.Text = "less"
            Else
                readmore.Text = "More"
                lbtext.ToolTip = lbtext.Text
                lbtext.Text = lbtext.Text.Substring(0, 50)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractValidate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub











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
    Sub saverates()

        Try
            Dim dt As New DataTable
            Dim strSqlQry As String = ""

            If Session("Calledfrom") = "Offers" Then

                'strSqlQry = " execute sp_finalcalculated_rates_offers '" & CType(hdnpromotionid.Value, String) & "'"

                'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                'myDataAdapter.SelectCommand.CommandTimeout = 0
                'myDataAdapter.Fill(dt)
                'SqlConn.Close()


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                mySqlCmd = New SqlCommand("sp_finalcalculated_rates_offers", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                mySqlCmd.ExecuteNonQuery()
                SqlConn.Close()


                'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
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


                ModalPopupDays.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Offer !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else

                ' strSqlQry = " execute sp_finalcalculated_rates '" & CType(hdncontractid.Value, String) & "'"

                'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                'myDataAdapter.SelectCommand.CommandTimeout = 0
                'myDataAdapter.Fill(dt)
                'SqlConn.Close()

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                mySqlCmd = New SqlCommand("sp_finalcalculated_rates", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)

                mySqlCmd.ExecuteNonQuery()
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


                ModalPopupDays.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Contract !.');", True)

                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If

            ViewState("CheckState") = "Open"
            btnApprove.Style.Add("display", "block")
            btnchecking.Style.Add("display", "none")


        Catch ex As Exception
            ModalPopupDays.Hide()
            ModalPopupError.Hide()
            'If SqlConn.State = ConnectionState.Open Then
            '    sqlTrans.Rollback()
            'End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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
        Try
            Dim ds As New DataSet

            If Validatepage() = False Then
                Exit Sub
            End If

            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = " Promotion Approval  - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Page.Title = Page.Title + " " + "Promotion Approval  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Dim approveexistpromo As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_Offer_search(nolock) where partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "' and isnull(status,'')='Yes'")

                If approveexistpromo <> "" Then
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Promotion Already Approved Please check.');", True)
                    'Exit Sub
                End If

                ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & hdnpromotionid.Value & "'")

            Else
                lblHeading.Text = " Contract Approval  - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + "Contract Approval  -" + ViewState("hotelname") + " - " + hdncontractid.Value

                Dim approveexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_search(nolock) where partycode='" & hdnpartycode.Value & "' and contractid='" & hdncontractid.Value & "' and isnull(status,'')='Yes'")

                If approveexist <> "" Then
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Contract Already Approved Please check.');", True)
                    'Exit Sub
                End If

              



                ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_contract " & "'" & hdncontractid.Value & "'")
            End If

            If ds.Tables(0).Rows.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    Session("ContractErrors") = ds.Tables(0)
                End If

                If ds.Tables(1).Rows.Count > 0 Then
                    Session("Missingdates") = ds.Tables(1)
                End If
                If ds.Tables(2).Rows.Count > 0 Then
                    Session("MissingCountryagent") = ds.Tables(2)
                End If


                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('ContractValidateWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                ModalPopupDays.Hide()
                Dim strpop As String = ""
                strpop = "window.open('ContractValidate.aspx?appid=1','Contract Validate');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            Else
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Contracts Now calculating the Rates !.');", True)

                Dim dependentid As DataSet
                If Session("Calledfrom") = "Offers" Then

                    strSqlQry = " execute sp_get_dependentoffers '" & DBNull.Value & "','" & CType(hdnpromotionid.Value, String) & "'"

                    dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)
                    If dependentid.Tables(0).Rows.Count > 0 Then

                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        sqlTrans = mySqlConn.BeginTransaction

                        mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdnpromotionid.Value, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.Text
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

                        ModalPopupDays.Hide()
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Promotion, please go to Dependent Offers recalculation option !.');", True)
                        Exit Sub
                    Else
                        ModalPopupError.Show()
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
                            mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = dependentid.Tables(0).Rows(i)("promotionid")
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

                        ModalPopupDays.Hide()

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Contract, please go to Dependent Offers recalculation option !.');", True)
                        Exit Sub
                    Else
                        ModalPopupError.Show()
                    End If
                End If

                'ModalPopupError.Show()

            End If

        Catch ex As Exception
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbConnectionClose(mySqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Protected Sub btncheckingnew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncheckingnew.Click
        Try
            Dim ds As New DataSet

            If Validatepage() = False Then
                Exit Sub
            End If

            If Session("Calledfrom") = "Offers" Then

                lblHeading.Text = " Promotion Approval  - " + ViewState("hotelname") + " - " + hdnpromotionid.Value
                Page.Title = Page.Title + " " + "Promotion Approval  -" + ViewState("hotelname") + " - " + hdnpromotionid.Value

                Dim approveexistpromo As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_Offer_search(nolock) where partycode='" & hdnpartycode.Value & "' and promotionid='" & hdnpromotionid.Value & "' and isnull(status,'')='Yes'")

                If approveexistpromo <> "" Then
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Promotion Already Approved Please check.');", True)
                    'Exit Sub
                End If

                ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_offers  " & "'" & hdnpromotionid.Value & "'")

            Else
                lblHeading.Text = " Contract Approval  - " + ViewState("hotelname") + " - " + hdncontractid.Value
                Page.Title = Page.Title + " " + "Contract Approval  -" + ViewState("hotelname") + " - " + hdncontractid.Value

                Dim approveexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_search(nolock) where partycode='" & hdnpartycode.Value & "' and contractid='" & hdncontractid.Value & "' and isnull(status,'')='Yes'")

                If approveexist <> "" Then
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Contract Already Approved Please check.');", True)
                    'Exit Sub
                End If


                ds = objUtils.GetDataFromDatasetnew(Session("DbConnectionName"), "Exec sp_validate_contract " & "'" & hdncontractid.Value & "'")
            End If

            If ds.Tables(0).Rows.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    Session("ContractErrors") = ds.Tables(0)
                End If

                If ds.Tables(1).Rows.Count > 0 Then
                    Session("Missingdates") = ds.Tables(1)
                End If
                If ds.Tables(2).Rows.Count > 0 Then
                    Session("MissingCountryagent") = ds.Tables(2)
                End If


                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('ContractValidateWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                ModalPopupDays.Hide()
                Dim strpop As String = ""
                strpop = "window.open('ContractValidate.aspx?appid=1','Contract Validate');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            Else
               
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Contracts/Offers Now calculating the Rates !.');", True)
                '' btncalculaterate.Style.Add("display", "block")
                '' btncheckingnew.Style.Add("display", "none")

                'ViewState("CheckState") = "Open"
                'btnApprove.Style.Add("display", "block")
                'btncheckingnew.Style.Add("display", "none")
                'btncalculaterate.Style.Add("display", "none")

                If ds.Tables(0).Rows.Count > 0 Then

                    If ds.Tables(0).Rows.Count > 0 Then
                        Session("ContractErrors") = ds.Tables(0)
                    End If

                    If ds.Tables(1).Rows.Count > 0 Then
                        Session("Missingdates") = ds.Tables(1)
                    End If
                    If ds.Tables(2).Rows.Count > 0 Then
                        Session("MissingCountryagent") = ds.Tables(2)
                    End If


                    'Dim strscript As String = ""
                    'strscript = "window.opener.__doPostBack('ContractValidateWindowPostBack', '');window.opener.focus();window.close();"
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                    ModalPopupDays.Hide()
                    Dim strpop As String = ""
                    strpop = "window.open('ContractValidate.aspx?appid=1','Contract Validate');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                Else
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Contracts Now calculating the Rates !.');", True)

                    Dim dependentid As DataSet
                    If Session("Calledfrom") = "Offers" Then

                        strSqlQry = " execute sp_get_dependentoffers '" & DBNull.Value & "','" & CType(hdnpromotionid.Value, String) & "'"

                        dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)
                        If dependentid.Tables(0).Rows.Count > 0 Then

                            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                            sqlTrans = mySqlConn.BeginTransaction

                            mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdnpromotionid.Value, String) & "'", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.Text
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

                            ModalPopupDays.Hide()
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Promotion, please go to Dependent Offers recalculation option !.');", True)
                            Exit Sub
                        Else
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Contracts/Offers Now calculating the Rates !.');", True)
                          
                            ViewState("CheckState") = "Open"
                            btnApprove.Style.Add("display", "block")
                            btncheckingnew.Style.Add("display", "none")
                            btncalculaterate.Style.Add("display", "none")
                            ModalPopupDays.Hide()

                            ' Rosalin 2019-10-14 for xml & RateSheet
                            'ViewState("CheckState") = "Open"
                            'btnApprove.Style.Add("display", "none")
                            'btncheckingnew.Style.Add("display", "none")
                            'btncalculaterate.Style.Add("display", "block")
                            'ModalPopupDays.Hide()


                            ' ModalPopupError.Show()
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
                                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = dependentid.Tables(0).Rows(i)("promotionid")
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

                            ModalPopupDays.Hide()

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Contract, please go to Dependent Offers recalculation option !.');", True)
                            Exit Sub
                        Else

                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Errors in Selected Contracts/Offers Now calculating the Rates !.');", True)

                            ViewState("CheckState") = "Open"
                            btnApprove.Style.Add("display", "block")
                            btncheckingnew.Style.Add("display", "none")
                            btncalculaterate.Style.Add("display", "none")
                            ModalPopupDays.Hide()

                            '' Rosalin 2019-10-14 for xml & RateSheet
                            'ViewState("CheckState") = "Open"
                            'btnApprove.Style.Add("display", "none")
                            'btncheckingnew.Style.Add("display", "none")
                            'btncalculaterate.Style.Add("display", "block")
                            'ModalPopupDays.Hide()


                            '  ModalPopupError.Show()
                        End If
                    End If
                End If

            End If

        Catch ex As Exception
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbConnectionClose(mySqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        btnchecking.Style.Add("display", "block")
        btnApprove.Style.Add("display", "none")

        ''' Added shhaul 23/06/18
        btncheckingnew.Style.Add("display", "block")
        btncheckingnew.Enabled = True
        btncheckingnew.Text = "Start Checking"

        btnchecking.Enabled = True
        btnchecking.Text = "Start Checking"
    End Sub



   


    Public Sub KillUnusedExcelProcess()
        Dim oXlProcess As Process() = Process.GetProcessesByName("Excel")
        For Each oXLP As Process In oXlProcess
            If Len(oXLP.MainWindowTitle) = 0 Then
                oXLP.Kill()
            End If
        Next
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
            objUtils.WritErrorLog("ContractHotelConst.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Return Nothing
        End Try
    End Function
    Public Sub closeconnection()
        If conn1.State = ConnectionState.Open Then conn1.Close()
    End Sub


    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        Dim dt As New DataTable
        Dim strSqlQry As String = ""
        Try
            Dim strFromEmailID As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text where emailtextfor=1")
            Dim strSubject1 As String = IIf(Session("Calledfrom") = "Offers", " Approval Error for this Promotion - " + hdnpromotionid.Value, " Approval Error for this Contract - " + hdncontractid.Value)
            Dim toemail As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select option_selected from reservation_parameters where param_id=2011")

            If Session("Calledfrom") = "Offers" Then



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = mySqlConn.BeginTransaction

                'Added by Rosalin 9th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( priceOffer adults etc...)

                'mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_approve_promotion", mySqlConn, sqlTrans)
                'mySqlCmd.CommandType = CommandType.StoredProcedure
                'mySqlCmd.CommandTimeout = 0
                'mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                'mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                'mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                'mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1

                'mySqlCmd.ExecuteNonQuery()



                mySqlCmd = New SqlCommand("sp_approve_promotion", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
                mySqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)

                'Added by Rosalin 9th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( priceOffer adults etc...)
                'Try

                '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                '    'Open connection

                '    mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_PreFinalcalculateMain_offers", mySqlConn)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.CommandTimeout = 0
                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                '    mySqlCmd.ExecuteNonQuery()
                '    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                '    clsDBConnect.dbConnectionClose(mySqlConn)


                '    ModalPopupDays.Hide()

                'Catch ex As Exception
                '    ModalPopupDays.Hide()
                '    Dim strEmailText1 As String = ex.Message.ToString
                '    ' If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then

                '    ' Delete  from main table

                '    strSqlQry = " execute ColumbusRpts.dbo.sp_delete_markuprates_offers '" & CType(hdnpromotionid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                '    myDataAdapter.SelectCommand.CommandTimeout = 0
                '    myDataAdapter.Fill(dt)
                '    mySqlConn.Close()

                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Promotion not able to Approved - ' " + strEmailText1 + ");", True)

                '    ' Else
                '    ' objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                '    ' End If
                'End Try




                ''' Added shahul 15/05/18
                'Try

                '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                '    mySqlCmd = New SqlCommand("sp_approve_markuprates_offers", SqlConn)
                '    mySqlCmd.CommandType = CommandType.StoredProcedure
                '    mySqlCmd.CommandTimeout = 0
                '    mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                '    mySqlCmd.ExecuteNonQuery()
                '    SqlConn.Close()


                '    'strSqlQry = " execute sp_approve_markuprates_offers '" & CType(hdnpromotionid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                '    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                '    'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                '    'myDataAdapter.SelectCommand.CommandTimeout = 0
                '    'myDataAdapter.Fill(dt)
                '    'mySqlConn.Close()


                '    ModalPopupDays.Hide()

                'Catch ex As Exception
                '    ModalPopupDays.Hide()
                '    Dim strEmailText1 As String = ex.Message.ToString
                '    If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then
                '        ''' Delete  from main table

                '        strSqlQry = " execute sp_delete_markuprates_offers '" & CType(hdnpromotionid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                '        myDataAdapter.SelectCommand.CommandTimeout = 0
                '        myDataAdapter.Fill(dt)
                '        mySqlConn.Close()

                '    Else
                '        objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                '    End If
                'End Try


                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Promotion  Approved  !.');", True)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OfferMainWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else
                '' Added Shahul 15/04/18
                Dim withdrawexist As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from view_contracts_search(nolock) where partycode='" & hdnpartycode.Value & "' and contractid='" & hdncontractid.Value & "' and isnull(withdraw,'')=1")

                If withdrawexist <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('In This Contract Already WithDrawn No need to Approve.');", True)
                    Exit Sub
                End If

                If ViewState("CheckState") = "Open" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction

                    ''Added by Rosalin 9th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( priceOffer adults etc...) 
                    'mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_approve_contract", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.StoredProcedure
                    'mySqlCmd.CommandTimeout = 0
                    'mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
                    'mySqlCmd.ExecuteNonQuery()


                    mySqlCmd = New SqlCommand("sp_approve_contract", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                    mySqlCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.Int, 4)).Value = 1
                    mySqlCmd.ExecuteNonQuery()


                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)

                    '' ''' Added by Rosalin 9th Oct 2019 for Rate Sheet and XML Pushing data into ColumbusRPTS main tables ( price adults etc...)
                    'Try


                    '    ' SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                    '    mySqlCmd = New SqlCommand("ColumbusRpts.dbo.New_PreFinalcalculateMain_contracts", mySqlConn)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure
                    '    mySqlCmd.CommandTimeout = 0
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                    '    mySqlCmd.ExecuteNonQuery()
                    '    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    '    clsDBConnect.dbConnectionClose(mySqlConn)


                    '    ModalPopupDays.Hide()

                    'Catch ex As Exception

                    '    ModalPopupDays.Hide()

                    '    Dim strEmailText1 As String = ex.Message.ToString
                    '    ' If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then

                    '    ' ''' Delete  from main table
                    '    strSqlQry = " execute ColumbusRpts.dbo.sp_delete_markuprates_contracts '" & CType(hdncontractid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                    '    myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    '    myDataAdapter.SelectCommand.CommandTimeout = 0
                    '    myDataAdapter.Fill(dt)
                    '    mySqlConn.Close()

                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Contract not able to Approved - ' " + strEmailText1 + ");", True)

                    '    '  Else
                    '    ' objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                    '    ' End If
                    'End Try

                    '#############


                    ' ''' Added shahul 15/05/18
                    'Try


                    '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    '    mySqlCmd = New SqlCommand("sp_approve_markuprates_contracts", SqlConn)
                    '    mySqlCmd.CommandType = CommandType.StoredProcedure
                    '    mySqlCmd.CommandTimeout = 0
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)

                    '    mySqlCmd.ExecuteNonQuery()
                    '    SqlConn.Close()


                    '    'strSqlQry = " execute sp_approve_markuprates_contracts '" & CType(hdncontractid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                    '    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                    '    'myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    '    'myDataAdapter.SelectCommand.CommandTimeout = 0
                    '    'myDataAdapter.Fill(dt)
                    '    'mySqlConn.Close()
                    '    ModalPopupDays.Hide()

                    'Catch ex As Exception

                    '    ModalPopupDays.Hide()

                    '    Dim strEmailText1 As String = ex.Message.ToString
                    '    If objEmail.SendEmailCCust(strFromEmailID, toemail, toemail, strSubject1, strEmailText1, Server.MapPath("~/images/logo.png")) Then


                    '        ''' Delete  from main table

                    '        strSqlQry = " execute sp_delete_markuprates_contracts '" & CType(hdncontractid.Value, String) & "','" & CType(Session("GlobalUserName"), String) & "'"

                    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                    '        myDataAdapter = New SqlDataAdapter(strSqlQry, mySqlConn)
                    '        myDataAdapter.SelectCommand.CommandTimeout = 0
                    '        myDataAdapter.Fill(dt)
                    '        mySqlConn.Close()

                    '    Else
                    '        objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), "Sending Approval Fail Email " + strSubject1, Session("GlobalUserName"))
                    '    End If
                    'End Try


                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Selected Contract are Approved  !.');", True)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ContractMainWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If



        Catch ex As Exception


            ModalPopupDays.Hide()

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            'If mySqlConn.State = ConnectionState.Open Then
            '    sqlTrans.Rollback()
            'End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            'objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub

    Protected Sub btnOk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk1.Click

        saverates()

    End Sub

    Protected Sub btncalculaterate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncalculaterate.Click
        Dim sqltranstarted As Boolean = False
        Try
            Dim dt As New DataTable
            Dim strSqlQry As String = ""

            If Session("Calledfrom") = "Offers" Then


                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection

                ' Push the data into the columbusRPTS edit tables -- Added By rosalin 9th Oct  
                mySqlCmd = New SqlCommand("New_XMLMove_PromotionEdit", SqlConn)   ' Moving data to Edit tables
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                mySqlCmd.ExecuteNonQuery()


                ' SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                mySqlCmd = New SqlCommand("ColumbusRpts.dbo.sp_finalcalculated_rates_offers", SqlConn)   ' Moving data to price Edit tables
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = CType(hdnpromotionid.Value, String)

                mySqlCmd.ExecuteNonQuery()
                SqlConn.Close()

                ModalPopupDays.Hide()

                ' added Rosalin 9th Oct 2019
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Offer !.');", True)

            Else


                ' Push the data into the columbusRPTS edit tables -- Added By rosalin 9th Oct
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                'Open connection
                mySqlCmd = New SqlCommand("New_XMLMove_contractEdit", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(hdnpartycode.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approveuser", SqlDbType.VarChar, 50)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@approvedate", SqlDbType.DateTime)).Value = CType(Now, Date)
                mySqlCmd.ExecuteNonQuery()
                '-----

                'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                mySqlCmd = New SqlCommand("ColumbusRpts.dbo.sp_finalcalculated_rates", SqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@contractid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)

                mySqlCmd.ExecuteNonQuery()
                SqlConn.Close()


                ModalPopupDays.Hide()

                ' Added Rosalin 9th Oct
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Contract !.');", True)

            End If

            Dim dependentid As DataSet
            If Session("Calledfrom") = "Offers" Then

                strSqlQry = " execute sp_get_dependentoffers '" & DBNull.Value & "','" & CType(hdnpromotionid.Value, String) & "'"

                dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)
                If dependentid.Tables(0).Rows.Count > 0 Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction
                    sqltranstarted = True
                    mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdnpromotionid.Value, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
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
                    sqltranstarted = False
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Promotion, please go to Dependent Offers recalculation option !.');", True)
                    ModalPopupDays.Hide()
                    Exit Sub
                Else
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Offer !.');", True)
                    '  btnApprove.Style.Add("display", "block")
                End If
            Else
                strSqlQry = " execute sp_get_dependentoffers '" & CType(hdncontractid.Value, String) & "','" & DBNull.Value & "'"

                dependentid = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)

                If dependentid.Tables(0).Rows.Count > 0 Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                    sqlTrans = mySqlConn.BeginTransaction
                    sqltranstarted = True
                    mySqlCmd = New SqlCommand("DELETE FROM offers_recalculate Where  contpromid='" & CType(hdncontractid.Value, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()

                    For i As Integer = 0 To dependentid.Tables(0).Rows.Count - 1

                        mySqlCmd = New SqlCommand("sp_save_recalculateoffers", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0
                        mySqlCmd.Parameters.Add(New SqlParameter("@contpromid", SqlDbType.VarChar, 20)).Value = CType(hdncontractid.Value, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@promotionid", SqlDbType.VarChar, 20)).Value = dependentid.Tables(0).Rows(i)("promotionid")
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
                    sqltranstarted = False
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)

                    ModalPopupDays.Hide()

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There are dependent offers for this Contract, please go to Dependent Offers recalculation option !.');", True)
                    Exit Sub
                Else
                    ModalPopupDays.Hide()
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Rates Are Calculated you can Approve the Contract !.');", True)

                End If
            End If


            ViewState("CheckState") = "Open"
            btnApprove.Style.Add("display", "block")
            btnchecking.Style.Add("display", "none")
            btncalculaterate.Style.Add("display", "none")


        Catch ex As Exception
            ModalPopupDays.Hide()

            If SqlConn.State = ConnectionState.Open And sqltranstarted = True Then
                sqlTrans.Rollback()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ContractApprove.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub
End Class
