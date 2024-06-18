#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Generic
#End Region
Partial Class ExcursionModule_AssignDriversExcursionsSearch
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objDate As New clsDateTime
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim index As String
    Dim lblid As Label
    Dim objuser As New clsUser

    Dim gvRow1 As GridViewRow
#End Region

#Region "Enum GridCol"
    Enum GridCol
        SectorCodeTCol = 0
        SectorCode = 1
        SectorName = 2
        CountryCode = 3
        CountryName = 4
        CityCode = 5
        CityName = 6
        Active = 7
        DateCreated = 8
        UserCreated = 9
        DateModified = 10
        UserModified = 11
        Assign = 11

        RemoveAssign = 12

    End Enum
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim pagedec As String = ""

                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                If Request.QueryString("menudesc") <> "" Then
                    pagedec = CType(Request.QueryString("menudesc"), String)
                End If
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub

                Else
                    'objuser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(strappname, String), "ExcursionModule\AssignDriversExcursionsSearch.aspx?menudesc=" + pagedec, btnAddNew, btnExportToExcel, _
                    '                                   btnPrint, gv_SearchResult, , , , , , , , , , GridCol.Assign, GridCol.RemoveAssign)

                End If

                txtRemarks.Text = "Pending Excursions to Confirm"

                btnAddNew.Visible = False
                btnExportToExcel.Visible = False
                btnPrint.Visible = False
                Dim default_group As String
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1106", String))
                fromdate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtodate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                fromdate.Enabled = False
                txtodate.Enabled = False
                txtTransferTodate.Enabled = False
                txtTransFrmDate.Enabled = False

                'chkDate.Checked = True
                txtTransFrmDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtTransferTodate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbConnectionName")), Date), "dd/MM/yyyy")
                'chkTransferDate.Checked = True

                Dim otypecode1 As String
                Dim otypecode2 As String

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                divassigntransfer.Style("display") = "none"
                divassigntransfer.Style("visibility") = "hidden"
                btnCancel.Attributes.Add("onclick", "return hidediv()")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDriverName, "drivername", "drivercode", "select drivername,drivercode from drivermaster where active=1", True)
                objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlTransferType, "othgrpname", "othgrpcode", "select rtrim(ltrim(othgrpname))othgrpname,rtrim(ltrim(othgrpcode))othgrpcode from othgrpmast where othmaingrpcode in('" & otypecode1 & "'" & ",'" & otypecode2 & "')  order by othgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbConnectionName"), ddlHotelName, "partyname", "partycode", "select partyname,partycode from partymast where active=1", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbConnectionName"), ddlAgent, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1", True)

                'If default_group <> "" Then
                '    ddlTransferType.Text = CType(default_group, String)
                'End If

                'If ddlTransferType.Text = "[Select]" Then
                '    FillGridall()
                'Else
                '    FillGrid()
                'End If
                FillGridall()



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("AssignDriversSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            grdDriverName()
            If ddlDriverName.Value <> "[Select]" Then
                txtTelephone.Text = hdndritelep.Value
            End If
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ddlTransferType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlDriverName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlAgent.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlHotelName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ExcursionsRequestWindowPostBack") Then
            btnResult_Click(sender, e)
        End If

    End Sub
#End Region

#Region "Private Sub FillGrid()"
    Private Sub FillGrid()
        Dim myDS As New DataSet
        gv_SearchResult.Visible = True
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try



            Dim parms As New List(Of SqlParameter)
            Dim parm(12) As SqlParameter
            parm(12) = New SqlParameter
            parm(0) = New SqlParameter("@searchbytourdate", IIf(chkDate.Checked = True, 1, 0))
            parm(1) = New SqlParameter("@fromtourdate", Format(CType((fromdate.Text), Date), "yyyy/MM/dd"))
            parm(2) = New SqlParameter("@totourdate", Format(CType(txtodate.Text, Date), "yyyy/MM/dd"))
            parm(3) = New SqlParameter("@searchbyrequestdate", IIf(chkTransferDate.Checked = True, 1, 0))
            parm(4) = New SqlParameter("@fromrequestdate", Format(CType((txtTransFrmDate.Text), Date), "yyyy/MM/dd"))
            parm(5) = New SqlParameter("@torequestdate", Format(CType((txtTransferTodate.Text), Date), "yyyy/MM/dd"))
            parm(6) = New SqlParameter("@othgrpcode", ddlTransferType.Items(ddlTransferType.SelectedIndex).Value)
            parm(7) = New SqlParameter("@requestid", CType(txtExcursionId.Text.Trim, String))
            parm(8) = New SqlParameter("@hotelcode", IIf(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value <> "[Select]", CType(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value, String), ""))
            parm(9) = New SqlParameter("@agentcode", IIf(ddlAgent.Items(ddlAgent.SelectedIndex).Value <> "[Select]", CType(ddlAgent.Items(ddlAgent.SelectedIndex).Value, String), ""))
            parm(10) = New SqlParameter("@guestname", CType(txtGuestname.Text.Trim, String))
            parm(11) = New SqlParameter("@ticketno", CType(txtTicketNo.Text.Trim, String))


            parms.Add(parm(0))
            parms.Add(parm(1))
            parms.Add(parm(2))
            parms.Add(parm(3))
            parms.Add(parm(4))
            parms.Add(parm(5))
            parms.Add(parm(6))
            parms.Add(parm(7))
            parms.Add(parm(8))
            parms.Add(parm(9))
            parms.Add(parm(10))
            parms.Add(parm(11))



            ' Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_excursions_assign_pending", parms)
            Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), " sp_get_excursions_pendingtoconfirm", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    gv_SearchResult.DataSource = ds.Tables(0)
                    gv_SearchResult.DataBind()
                    gv_SearchResult.Visible = True
                    lblMsg.Visible = False
                Else
                    gv_SearchResult.Visible = False
                    lblMsg.Visible = True
                End If
            Else
                gv_SearchResult.DataBind()
                gv_SearchResult.Visible = True
                lblMsg.Visible = False
            End If

            'strSqlQry = "select d.vlineno,t.requestid,am.agentname,[transfers_Type] =case when t.transfertype=0 then 'Arrival' else case when t.transfertype=1 then 'Departure'else case when t.transfertype=2 then 'Shifting' end end end,t.transferdate,t.requestid,pm.partyname as hotelname,pm.tel1 ,case when t.transfertype = 2 then pm.partyname else '' end shifthotelname,t.guestname,t.flightcode,t.startingpoint,t.endingpoint,[airportname]=case when t.transfertype =0 then v1.airportbordername else case when t.transfertype=1 then v2.airportbordername end end,t.flighttime,t.remarks, t.cartype, d.drivercode, dm.drivername, dm.mobileno, d.pickupdate, d.pickuptime, d.vehiclecode, d.suppliercode,pm.partyname as suppliername, d.suppliercurr, d.costprice, d.complementsup,trfstatus= case d.[status] when 0 then 'Confirm' when 1 then 'Cancel' else 'On Request' end, transfermode=case d.transfermode when 0 then 'Own Transport' else 'From Supplier' end, d.incominginvno from transfers_booking t(nolock) join transfers_booking_details d(nolock) on t.requestid = d.requestid  left outer join partymast pm on t.hotelcode =pm.partycode left outer join  view_airportsectormast v1(nolock) on t.startingpoint=v1.airportbordercode  left outer join view_airportsectormast v2(nolock) on t.endingpoint=v2.airportbordercode left outer join drivermaster dm on d.drivercode =dm.drivercode left outer join agentmast am on t.agentcode =am.agentcode"
            'strValue = Trim(BuildCondition())
            'If strValue <> "" Then
            '    strSqlQry = strSqlQry & " where " & strValue & " order by t.flightcode,t.flighttime,t.transferdate,t.requestid"
            'Else
            '    strSqlQry = strSqlQry & " order by t.flightcode,t.flighttime,t.transferdate,t.requestid"
            'End If
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)
            'gv_SearchResult.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    gv_SearchResult.DataBind()
            'Else
            '    gv_SearchResult.PageIndex = 0
            '    gv_SearchResult.DataBind()

            'End If










        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AssignDriversSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Private Sub FillGridall()
        Dim myDS As New DataSet
        gv_SearchResult.Visible = True
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try



            Dim parms As New List(Of SqlParameter)
            Dim parm(13) As SqlParameter
            parm(13) = New SqlParameter
            parm(0) = New SqlParameter("@searchbytourdate", IIf(chkDate.Checked = True, 1, 0))
            parm(1) = New SqlParameter("@fromtourdate", Format(CType((fromdate.Text), Date), "yyyy/MM/dd"))
            parm(2) = New SqlParameter("@totourdate", Format(CType(txtodate.Text, Date), "yyyy/MM/dd"))
            parm(3) = New SqlParameter("@searchbyrequestdate", IIf(chkTransferDate.Checked = True, 1, 0))
            parm(4) = New SqlParameter("@fromrequestdate", Format(CType((txtTransFrmDate.Text), Date), "yyyy/MM/dd"))
            parm(5) = New SqlParameter("@torequestdate", Format(CType((txtTransferTodate.Text), Date), "yyyy/MM/dd"))
            parm(6) = New SqlParameter("@othgrpcode", IIf(ddlTransferType.Items(ddlTransferType.SelectedIndex).Value = "[Select]", "", ddlTransferType.Items(ddlTransferType.SelectedIndex).Value))
            parm(7) = New SqlParameter("@requestid", CType(txtExcursionId.Text.Trim, String))
            parm(8) = New SqlParameter("@hotelcode", IIf(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value <> "[Select]", CType(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value, String), ""))
            parm(9) = New SqlParameter("@agentcode", IIf(ddlAgent.Items(ddlAgent.SelectedIndex).Value <> "[Select]", CType(ddlAgent.Items(ddlAgent.SelectedIndex).Value, String), ""))
            parm(10) = New SqlParameter("@guestname", CType(txtGuestname.Text.Trim, String))
            parm(11) = New SqlParameter("@ticketno", CType(txtTicketNo.Text.Trim, String))
            parm(12) = New SqlParameter("@tourguidereq", IIf(chkTourGuideReq.Checked = True, "1", "0"))


            parms.Add(parm(0))
            parms.Add(parm(1))
            parms.Add(parm(2))
            parms.Add(parm(3))
            parms.Add(parm(4))
            parms.Add(parm(5))
            parms.Add(parm(6))
            parms.Add(parm(7))
            parms.Add(parm(8))
            parms.Add(parm(9))
            parms.Add(parm(10))
            parms.Add(parm(11))
            parms.Add(parm(12))



            ' Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_excursions_assign_pending_all", parms)

            Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_excursions_pendingtoconfirm", parms)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    gv_SearchResult.DataSource = ds.Tables(0)
                    gv_SearchResult.DataBind()
                    gv_SearchResult.Visible = True
                    lblMsg.Visible = False
                Else
                    gv_SearchResult.Visible = False
                    lblMsg.Visible = True
                End If
            Else
                gv_SearchResult.DataBind()
                gv_SearchResult.Visible = True
                lblMsg.Visible = False
            End If

            'strSqlQry = "select d.vlineno,t.requestid,am.agentname,[transfers_Type] =case when t.transfertype=0 then 'Arrival' else case when t.transfertype=1 then 'Departure'else case when t.transfertype=2 then 'Shifting' end end end,t.transferdate,t.requestid,pm.partyname as hotelname,pm.tel1 ,case when t.transfertype = 2 then pm.partyname else '' end shifthotelname,t.guestname,t.flightcode,t.startingpoint,t.endingpoint,[airportname]=case when t.transfertype =0 then v1.airportbordername else case when t.transfertype=1 then v2.airportbordername end end,t.flighttime,t.remarks, t.cartype, d.drivercode, dm.drivername, dm.mobileno, d.pickupdate, d.pickuptime, d.vehiclecode, d.suppliercode,pm.partyname as suppliername, d.suppliercurr, d.costprice, d.complementsup,trfstatus= case d.[status] when 0 then 'Confirm' when 1 then 'Cancel' else 'On Request' end, transfermode=case d.transfermode when 0 then 'Own Transport' else 'From Supplier' end, d.incominginvno from transfers_booking t(nolock) join transfers_booking_details d(nolock) on t.requestid = d.requestid  left outer join partymast pm on t.hotelcode =pm.partycode left outer join  view_airportsectormast v1(nolock) on t.startingpoint=v1.airportbordercode  left outer join view_airportsectormast v2(nolock) on t.endingpoint=v2.airportbordercode left outer join drivermaster dm on d.drivercode =dm.drivercode left outer join agentmast am on t.agentcode =am.agentcode"
            'strValue = Trim(BuildCondition())
            'If strValue <> "" Then
            '    strSqlQry = strSqlQry & " where " & strValue & " order by t.flightcode,t.flighttime,t.transferdate,t.requestid"
            'Else
            '    strSqlQry = strSqlQry & " order by t.flightcode,t.flighttime,t.transferdate,t.requestid"
            'End If
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)
            'gv_SearchResult.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    gv_SearchResult.DataBind()
            'Else
            '    gv_SearchResult.PageIndex = 0
            '    gv_SearchResult.DataBind()

            'End If










        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AssignDriversSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

    Protected Sub btnResult_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResult.Click


        divassigntransfer.Style("display") = "none"
        divassigntransfer.Style("visibility") = "hidden"

        'If ddlTransferType.Text = "[Select]" Then
        FillGridall()
        'Else
        'FillGrid()
        'End If

    End Sub


    
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            Dim lbldrivercode As Label
            'Dim requestid As String

            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub

            Dim lblstatus As Label
            Dim hotelcode As Label

            ' hotelcode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(12).Text

            'Dim rowindex As Integer = CInt(e.CommandArgument)
            'Dim row As GridViewRow = gv_SearchResult.Rows(rowindex)



            lblid = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")
            index = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(1).Text
            lblstatus = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblstatus")
            hotelcode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblhotel")

            Session("Excursion_ID") = index
            Session("Rlineno") = lblid.Text

            'requestid = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(2).Text
            hdnvlineno.Value = CType(lblid.Text, String)
            hdntransaction.Value = index



            Dim flag As Boolean
            flag = False
            'If e.CommandName = "Assign" Then
            '    Dim parms As New List(Of SqlParameter)
            '    Dim parm(2) As SqlParameter
            '    parm(2) = New SqlParameter
            '    parm(0) = New SqlParameter("@requestid", index)
            '    parm(1) = New SqlParameter("@vlineno", lblid.Text.Trim)


            '    parms.Add(parm(0))
            '    parms.Add(parm(1))

            '    Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_getdrivervalue", parms)
            '    If ds.Tables(0).Rows.Count > 0 Then
            '        'drivercode = IIf(gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(12).Text = "&nbsp;", "", gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(12).Text)
            '        lbldrivercode = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblsdrivercode")

            '        '  Dim drvtelephone As String = IIf(gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(16).Text = "&nbsp;", "", Val(IIf(gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).Cells(16).Text = "Own Transport", 0, 1)))

            '        If (lbldrivercode.Text <> ds.Tables(0).Rows(0)(0).ToString) Then
            '            'Or b <> ds.Tables(0).Rows(0)(1).ToString Or c <> ds.Tables(0).Rows(0)(2).ToString Or d <> ds.Tables(0).Rows(0)(3).ToString Or g <> ds.Tables(0).Rows(0)(5).ToString Or h <> ds.Tables(0).Rows(0)(6).ToString Or i <> ds.Tables(0).Rows(0)(7).ToString Or k <> ds.Tables(0).Rows(0)(8).ToString Or l <> ds.Tables(0).Rows(0)(9).ToString Or m <> ds.Tables(0).Rows(0)(10).ToString) Then

            '            flag = True
            '        End If
            '    End If





            '    If flag = True Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Assigned values have been changed,refreshing record');", True)
            '        divassigntransfer.Style("display") = "none"
            '        divassigntransfer.Style("visibility") = "hidden"
            '        Exit Sub
            '    Else
            '        'divassigntransfer.Style("display") = "block"

            '        'divassigntransfer.Style("visibility") = "visibile"
            '        'divassigntransfer.Style("position") = "absolute"
            '        'divassigntransfer.Style("top") = "100px"
            '        'divassigntransfer.Style("left") = "170px"
            '        'divassigntransfer.Style("z-index") = "200px"

            '        divassigntransfer.Style("display") = "block"
            '        divassigntransfer.Style("visibility") = "visibile"
            '        ModalPopupDays.Show()

            '        fillDategrd(Gv_DriverName, True, 5)
            '        showrecord(index, lblid.Text.Trim)

            '    End If


            'End If

            'If e.CommandName = "Remove Assign" Then

            '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            '    sqlTrans = SqlConn.BeginTransaction
            '    myCommand = New SqlCommand("sp_update_excursions_detail", SqlConn, sqlTrans)
            '    myCommand.CommandType = CommandType.StoredProcedure


            '    myCommand.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = hdntransaction.Value
            '    myCommand.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = hdnvlineno.Value
            '    myCommand.Parameters.Add(New SqlParameter("@drivercode", SqlDbType.VarChar, 50)).Value = DBNull.Value

            '    myCommand.Parameters.Add(New SqlParameter("@additionaldrivers", SqlDbType.VarChar, 2000)).Value = DBNull.Value









            '    myCommand.ExecuteNonQuery()

            '    sqlTrans.Commit()    'SQl Tarn Commit
            '    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            '    clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            '    clsDBConnect.dbConnectionClose(SqlConn)

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Assigned values are removed');", True)
            '    btnResult_Click(sender, e)
            'End If

            Dim mindate1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convert(varchar(10),min(datein),111) from view_bookingmindatein where requestid='" & CType(index, String) & "'")

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master(nolock)")

            If mindate1 <> "" Then
                If Convert.ToDateTime(mindate1) <= Convert.ToDateTime(sealdate) Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Month Already Sealed');", True)
                    Return
                End If
            End If

            If e.CommandName = "Confirm" Then
                InsertExcursionCostTemp(Session("Excursion_ID"))
                ' If lblstatus.Text = "Confirmed" Then Exit Sub
                Session("TempMultiCostGrid") = Nothing
                Dim strpop As String = ""
                'strpop = "window.open('ExcursionRequestSubEntry.aspx?State=AddRow&RefCode=" + CType(Session("ExcursionRequestRefCode"), String) + "&LineNo=" + CType(lblid.Text, String) + "&SellingType=" + CType(ddlSellingType.Value, String) + "&SpersonCode=" + CType(ddlConcierge.Value, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                'strpop = "window.open('ExcursionRequestSubEntry1.aspx?State=EditRow&RefCode=" + CType(index, String) + "&LineNo=" + CType(lblid.Text, String) + "&hotel=" + CType(hotelcode.Text, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('ExcursionRequestSubEntry1.aspx?State=EditRow&RefCode=" + CType(index, String) + "&LineNo=" + CType(lblid.Text, String) + "&hotel=" + CType(hotelcode.Text, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                Exit Sub
            End If

            If e.CommandName = "CancelRow" Then
                '  If lblstatus.Text = "Cancelled" Then Exit Sub
                Dim strpop1 As String = ""
                'strpop = "window.open('ExcursionRequestSubEntry.aspx?State=AddRow&RefCode=" + CType(Session("ExcursionRequestRefCode"), String) + "&LineNo=" + CType(lblid.Text, String) + "&SellingType=" + CType(ddlSellingType.Value, String) + "&SpersonCode=" + CType(ddlConcierge.Value, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                'strpop1 = "window.open('ExcursionRequestSubEntry1.aspx?State=CancelRow&RefCode=" + CType(index, String) + "&LineNo=" + CType(lblid.Text, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop1 = "window.open('ExcursionRequestSubEntry1.aspx?State=CancelRow&RefCode=" + CType(index, String) + "&LineNo=" + CType(lblid.Text, String) + "&TicketNo=" + CType(txtTicketNo.Text, String) + "','ExcursionRequestSubEntry');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop1, True)
                Exit Sub
            End If






        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TransfersRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Private Sub InsertExcursionCostTemp(ByVal excID As String)
        Try
            Dim lsTempInsertQry As String
            lsTempInsertQry = "exec sp_edit_saveExcursion_Cost_Detail_ToTempTable '" & excID & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand(lsTempInsertQry, SqlConn)
            myCommand.CommandType = CommandType.Text
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionsRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub


    Private Sub showrecord(ByVal refcode As String, ByVal line As String)

        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("select * from excursions_detail,excursions_header where excursions_header.excid=excursions_detail.excid  and excursions_detail.excid='" & refcode & "' and rlineno='" & line & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("excid")) = False Then
                        Me.txtTransferIDvalue.Text = CType(mySqlReader("excid"), String)
                    Else
                        Me.txtTransferIDvalue.Text = ""
                    End If
                    If IsDBNull(mySqlReader("tourdate")) = False Then
                        txtTransferDatevalue.Text = Format(CType((mySqlReader("tourdate")), Date), "dd/MM/yyyy")
                    Else
                        txtTransferDatevalue.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                    End If


                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtagentName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "agentname", "agentcode", CType(mySqlReader("agentcode"), String))
                    Else
                        Me.txtagentName.Text = ""
                    End If

                    'If IsDBNull(mySqlReader("agentref")) = False Then

                    '    Me.txtclientRefvalue.Text = CType(mySqlReader("agentref"), String)
                    'Else
                    '    Me.txtclientRefvalue.Text = ""
                    'End If

                    If IsDBNull(mySqlReader("ticketno")) = False Then

                        Me.txtclientRefvalue.Text = CType(mySqlReader("ticketno"), String)
                    Else
                        Me.txtclientRefvalue.Text = ""
                    End If

                    If IsDBNull(mySqlReader("hotel")) = False Then
                        Me.txtHotelNameValue.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partyname", "partycode", CType(mySqlReader("hotel"), String))
                    Else
                        Me.txtHotelNameValue.Text = ""
                    End If



                    If IsDBNull(mySqlReader("guestname")) = False Then
                        txtGuestNameValue.Text = mySqlReader("guestname")
                    Else
                        txtGuestNameValue.Text = ""
                    End If
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        txtTransferTypevalue.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", CType(mySqlReader("othtypcode"), String))
                    Else
                        txtTransferTypevalue.Text = ""
                    End If

                    If IsDBNull(mySqlReader("drivercode")) = False Then
                        ddlDriverName.Value = CType(mySqlReader("drivercode"), String)
                        txtTelephone.Text = GetDriverTelephoneNo(CType(mySqlReader("drivercode"), String))
                    Else
                        ddlDriverName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("additionaldrivers")) = False Then
                        AssignDriverDetailsToGrid(CType(mySqlReader("additionaldrivers"), String))
                    End If


                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AssignDriversSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub


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
        'txtgridrows.Value = grd.Rows.Count
    End Sub
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("drivercode", GetType(Integer)))

        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function



    Protected Sub Gv_DriverName_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_DriverName.RowDataBound

        Dim ddldrivername As HtmlSelect

        Dim txttele As TextBox
        Dim lbldrivercode As Label
        Dim hdndrivercode As HiddenField
        Dim hdntrelephone As HiddenField

        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowIndex <> -1 Then
            lbldrivercode = e.Row.FindControl("lblDriverCode")
            ddldrivername = e.Row.FindControl("ddldrivernamevalue")
            txttele = e.Row.FindControl("txttelephonevalue")
            hdndrivercode = e.Row.FindControl("hdndrivercode")
            hdntrelephone = e.Row.FindControl("hdntelephone")
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldrivername, "drivername", "drivercode", "select drivername,drivercode from drivermaster where active=1", True)
            ddldrivername.Attributes.Add("onchange", "javascript:gettelehponeforgrid('" + CType(ddldrivername.ClientID, String) + "','" + CType(txttele.ClientID, String) + "','" + CType(hdndrivercode.ClientID, String) + "','" + CType(hdntrelephone.ClientID, String) + "')")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddldrivername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
        End If


    End Sub

    Public Sub grdDriverName()
        Dim hdnDriveCode As HiddenField
        Dim ddldrivername As HtmlSelect
        Dim hdntrelephone As HiddenField
        Dim txttele As TextBox

        For Each GvRow In Gv_DriverName.Rows



            ddldrivername = CType(GvRow.FindControl("ddldrivernamevalue"), HtmlSelect)
            txttele = CType(GvRow.FindControl("txttelephonevalue"), TextBox)

            hdnDriveCode = CType(GvRow.FindControl("hdndrivercode"), HiddenField)
            hdntrelephone = CType(GvRow.FindControl("hdntelephone"), HiddenField)

            If ddldrivername.Value <> "[Select]" Then
                If hdnDriveCode.Value <> "" And hdnDriveCode.Value <> "[Select]" Then
                    ddldrivername.DataValueField = hdnDriveCode.Value
                    txttele.Text = hdntrelephone.Value
                End If
            End If


        Next
    End Sub

    Protected Sub btnAddRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = Gv_DriverName.Rows.Count + 1
        Dim n As Integer = 0

        Dim driverName(count) As String
        Dim drivtele(count) As String
        Dim chkselect(count) As String
        Dim chkactivevalue As CheckBox
        Dim ddldrivername As HtmlSelect
        Dim txttelephone As TextBox


        Try
            For Each GVRow In Gv_DriverName.Rows


                ddldrivername = CType(GVRow.FindControl("ddldrivernamevalue"), HtmlSelect)
                driverName(n) = CType(ddldrivername.Value, String)
                txttelephone = CType(GVRow.FindControl("txttelephonevalue"), TextBox)
                drivtele(n) = CType(txttelephone.Text, String)
                chkactivevalue = CType(GVRow.FindControl("ChkSelect"), CheckBox)


                If chkactivevalue.Checked = True Then
                    chkselect(n) = 1
                Else
                    chkselect(n) = 0
                End If
                n = n + 1
            Next
            fillDategrd(Gv_DriverName, False, count)
            Dim i As Integer = n
            n = 0
            For Each GVRow In Gv_DriverName.Rows
                If n = i Then
                    Exit For
                End If

                ddldrivername = CType(GVRow.FindControl("ddldrivernamevalue"), HtmlSelect)
                ddldrivername.Value = driverName(n)


                txttelephone = CType(GVRow.FindControl("txttelephonevalue"), TextBox)
                txttelephone.Text = drivtele(n)
                'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlroomname, "rmtypname", "rmtypcode", "select rmtypname, rmtypcode from rmtypmast where rmtypcode='" + txtroomcode.Text + "' order by rmtypname", True)
                'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlroomname, "rmtypname", "rmtypcode", "select  rmtypname,rmtypcode from rmtypmast where rmtypcode='" + txtroomcode.Text + "' order by rmtypcode", True)

                chkactivevalue = CType(GVRow.FindControl("ChkSelect"), CheckBox)
                'chkactivevalue.Text = chkactive(n)
                If chkselect(n) = 1 Then
                    chkactivevalue.Checked = True
                Else
                    chkactivevalue.Checked = False
                End If


                n = n + 1
            Next


            ModalPopupDays.Show()
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRow.Click
        Dim count As Integer
        Dim GVRow As GridViewRow
        count = Gv_DriverName.Rows.Count + 1
        Dim n As Integer = 0

        Dim driverName(count) As String
        Dim drivtele(count) As String
        Dim chkselect(count) As String
        Dim chkactivevalue As CheckBox
        Dim ddldrivername As HtmlSelect
        Dim txttelephone As TextBox


        Try
            For Each GVRow In Gv_DriverName.Rows
                chkactivevalue = GVRow.FindControl("ChkSelect")
                If chkactivevalue.Checked = False Then
                    ddldrivername = CType(GVRow.FindControl("ddldrivernamevalue"), HtmlSelect)
                    driverName(n) = CType(ddldrivername.Value, String)
                    txttelephone = CType(GVRow.FindControl("txttelephonevalue"), TextBox)
                    drivtele(n) = CType(txttelephone.Text, String)
                    chkactivevalue = CType(GVRow.FindControl("ChkSelect"), CheckBox)


                    If chkactivevalue.Checked = True Then
                        chkselect(n) = 1
                    Else
                        chkselect(n) = 0
                    End If
                    n = n + 1
                End If
            Next
            count = n
            If count = 0 Then
                count = 1
            End If
            fillDategrd(Gv_DriverName, False, count)
            Dim i As Integer = n
            n = 0
            For Each GVRow In Gv_DriverName.Rows
                If n = i Then
                    Exit For
                End If

                ddldrivername = CType(GVRow.FindControl("ddldrivernamevalue"), HtmlSelect)
                ddldrivername.Value = driverName(n)


                txttelephone = CType(GVRow.FindControl("txttelephonevalue"), TextBox)
                txttelephone.Text = drivtele(n)

                chkactivevalue = CType(GVRow.FindControl("ChkSelect"), CheckBox)

                If chkselect(n) = 1 Then
                    chkactivevalue.Checked = True
                Else
                    chkactivevalue.Checked = False
                End If


                n = n + 1
            Next

            ModalPopupDays.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            If Page.IsValid = True Then

                If ValidateSave() = False Then
                    ModalPopupDays.Show()
                    Exit Sub
                End If
                Dim drivercode As String
                drivercode = getdriver()
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction
                myCommand = New SqlCommand("sp_update_excursions_detail", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure


                myCommand.Parameters.Add(New SqlParameter("@excid", SqlDbType.VarChar, 20)).Value = txtTransferIDvalue.Text
                myCommand.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = hdnvlineno.Value
                myCommand.Parameters.Add(New SqlParameter("@drivercode", SqlDbType.VarChar, 50)).Value = ddlDriverName.Items(ddlDriverName.SelectedIndex).Value
                If drivercode <> "" Then
                    myCommand.Parameters.Add(New SqlParameter("@additionaldrivers", SqlDbType.VarChar, 2000)).Value = drivercode
                Else
                    myCommand.Parameters.Add(New SqlParameter("@additionaldrivers", SqlDbType.VarChar, 2000)).Value = ""
                End If



                myCommand.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)

                clearfields()
            End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
            divassigntransfer.Style("display") = "none"
            divassigntransfer.Style("visibility") = "hidden"
            btnResult_Click(sender, e)
        Catch ex As Exception

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AssignDriversSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try






    End Sub

    Private Function getdriver() As String

        Dim ddldriname As HtmlSelect
        Dim drivercode As String = ""

        For Each Me.gvRow1 In Gv_DriverName.Rows
            ddldriname = gvRow1.FindControl("ddldrivernamevalue")
            'lblcode = gvRow1.FindControl("lblDriverCode")
            If ddldriname.Value <> "[Select]" Then
                drivercode = drivercode + "'" + ddldriname.Value + "'" + ","
            End If
        Next
        If drivercode.Length > 0 Then
            drivercode = drivercode.Substring(0, drivercode.Length - 1)
        End If

        Return drivercode
    End Function

    Private Function ValidateSave() As Boolean
        ValidateSave = True
        If ddlDriverName.Items(ddlDriverName.SelectedIndex).Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Driver');", True)
            SetFocus(ddlDriverName)
            ValidateSave = False
            Exit Function
        End If

    End Function

    Private Sub clearfields()


        ddlDriverName.Value = "[Select]"
        txtTelephone.Text = ""
        Dim ddldrivernameg As HtmlSelect
        Dim txttelephoneg As TextBox

        For Each gvRow In Gv_DriverName.Rows
            ddldrivernameg = CType(gvRow.FindControl("ddldrivernamevalue"), HtmlSelect)
            txttelephoneg = CType(gvRow.FindControl("txttelephonevalue"), TextBox)
            ddldrivernameg.Value = "[Select]"
            txttelephoneg.Text = ""
        Next





    End Sub

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        If ddlTransferType.Text = "[Select]" Then
            FillGridall()
        Else
            FillGridall()
        End If

        'gv_SearchResult.PageIndex = e.NewPageIndex
        'If ddlTransferType.Text = "[Select]" Then
        '    FillGridall()
        'Else
        '    FillGrid()
        'End If
        'FillGridall()
        'divassigntransfer.Style("display") = "none"
        'divassigntransfer.Style("visibility") = "hidden"

    End Sub

    Protected Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked = True Then
            fromdate.Enabled = True
            txtodate.Enabled = True
        Else
            fromdate.Enabled = False
            txtodate.Enabled = False
            ImgPBtnFrmDt.Enabled = False
            ImgPBtnToDt.Enabled = False

        End If
    End Sub



    Protected Sub chkTransferDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTransferDate.CheckedChanged
        If chkTransferDate.Checked = True Then
            txtTransFrmDate.Enabled = True
            txtTransferTodate.Enabled = True

        Else

            txtTransFrmDate.Enabled = False
            txtTransferTodate.Enabled = False
            ImgPBtnTransFrmDt.Enabled = False
            ImgPBtnTransToDt.Enabled = False


        End If
    End Sub

#End Region


    Private Function GetDriverTelephoneNo(ByVal driverCode As String) As String
        Try
            Dim driverTelephone As String
            driverTelephone = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(mobileno,'')mobileno from drivermaster where drivercode='" & driverCode & "'")
            Return driverTelephone

        Catch ex As Exception
            Return ""
        End Try

    End Function

    Private Sub AssignDriverDetailsToGrid(ByVal additionalDriver As String)
        Try
            Dim i, j As Integer
            i = 0
            j = 0
            Dim driverCode As String()
            Dim ddldrivernamevalue As HtmlSelect
            Dim txttelephonevalue As TextBox
            driverCode = additionalDriver.Split(",")

            For i = 0 To driverCode.Length - 1
                GetDriverTelephoneNo(driverCode(i))
            Next
            Dim MyArrayList As New ArrayList
            For i = 0 To Gv_DriverName.Rows.Count - 1
                ddldrivernamevalue = Gv_DriverName.Rows(i).FindControl("ddldrivernamevalue")
                txttelephonevalue = Gv_DriverName.Rows(i).FindControl("txttelephonevalue")

                For j = 0 To driverCode.Length - 1
                    If driverCode(j) <> "" Then
                        If MyArrayList.Contains(driverCode(j).Replace("'", "")) Then
                            GoTo NextRow
                        Else
                            MyArrayList.Add(driverCode(j).Replace("'", ""))
                            ddldrivernamevalue.Value = driverCode(j).Replace("'", "")
                            txttelephonevalue.Text = GetDriverTelephoneNo(driverCode(j).Replace("'", ""))
                            Exit For
                        End If
                       
NextRow:
                    End If
                Next
            Next


        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Dim lblstatus As Label
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            lblstatus = CType(e.Row.FindControl("lblstatus"), Label)
            'lblstatus = gv_SearchResult.Rows(CType(e.Row.FindControl("lblstatus"), Label),string)
            'lblstatus = gv_SearchResult.Rows(CType(e.Row.FindControl("lblstatus"), Label))
            Dim lnlUpdate As LinkButton = CType(e.Row.FindControl("lnkConfirm"), LinkButton)
            Dim lnlcancel As LinkButton = CType(e.Row.FindControl("lnlCancel"), LinkButton)

            If lblstatus.Text = "Confirmed" Then
                '   lnlUpdate.Enabled = False
            ElseIf lblstatus.Text = "Cancelled" Then
                lnlcancel.Enabled = True
                lnlUpdate.Enabled = True
            Else
                lnlUpdate.Enabled = True
                lnlcancel.Enabled = True
            End If
            '    '
            '    'lnkConfirm.Visible = False

        End If

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim strpop As String = ""

        Dim parms As New List(Of SqlParameter)
        Dim parm(13) As SqlParameter
        parm(13) = New SqlParameter
        parm(0) = New SqlParameter("@searchbytourdate", IIf(chkDate.Checked = True, 1, 0))
        parm(1) = New SqlParameter("@fromtourdate", Format(CType((fromdate.Text), Date), "yyyy/MM/dd"))
        parm(2) = New SqlParameter("@totourdate", Format(CType(txtodate.Text, Date), "yyyy/MM/dd"))
        parm(3) = New SqlParameter("@searchbyrequestdate", IIf(chkTransferDate.Checked = True, 1, 0))
        parm(4) = New SqlParameter("@fromrequestdate", Format(CType((txtTransFrmDate.Text), Date), "yyyy/MM/dd"))
        parm(5) = New SqlParameter("@torequestdate", Format(CType((txtTransferTodate.Text), Date), "yyyy/MM/dd"))
        parm(6) = New SqlParameter("@othgrpcode", IIf(ddlTransferType.Items(ddlTransferType.SelectedIndex).Value = "[Select]", "", ddlTransferType.Items(ddlTransferType.SelectedIndex).Value))
        parm(7) = New SqlParameter("@requestid", CType(txtExcursionId.Text.Trim, String))
        parm(8) = New SqlParameter("@hotelcode", IIf(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value <> "[Select]", CType(ddlHotelName.Items(ddlHotelName.SelectedIndex).Value, String), ""))
        parm(9) = New SqlParameter("@agentcode", IIf(ddlAgent.Items(ddlAgent.SelectedIndex).Value <> "[Select]", CType(ddlAgent.Items(ddlAgent.SelectedIndex).Value, String), ""))
        parm(10) = New SqlParameter("@guestname", CType(txtGuestname.Text.Trim, String))
        parm(11) = New SqlParameter("@ticketno", CType(txtTicketNo.Text.Trim, String))
        parm(12) = New SqlParameter("@tourguidereq", IIf(chkTourGuideReq.Checked = True, "1", "0"))


        parms.Add(parm(0))
        parms.Add(parm(1))
        parms.Add(parm(2))
        parms.Add(parm(3))
        parms.Add(parm(4))
        parms.Add(parm(5))
        parms.Add(parm(6))
        parms.Add(parm(7))
        parms.Add(parm(8))
        parms.Add(parm(9))
        parms.Add(parm(10))
        parms.Add(parm(11))
        parms.Add(parm(12))
        Session("ParametersTourGuideReq") = parms
        'strpop = "window.open('rptVehicleExpenseReport.aspx?PageName=tourguiderequired&searchbytourdate=','TourGuideRequired','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('rptVehicleExpenseReport.aspx?PageName=tourguiderequired&searchbytourdate=','TourGuideRequired');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
