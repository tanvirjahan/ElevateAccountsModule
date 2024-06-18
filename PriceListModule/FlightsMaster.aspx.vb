'------------================--------------=======================------------------================
'   Module Name    :    Flight Master .aspx
'   Developer Name :    Amit Survase
'   Date           :    2 July 2008
'   
''------------================--------------=======================------------------================

#Region "namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class FlightsMaster
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)


                'End If


                Pnldeparture.Visible = False
                PanelArrival.Visible = False

                chkapplytofuture.Visible = False
                lblapplytofuture.Visible = False
                chkapplytofuture.Enabled = True





                charcters(TxtFlightnumber)

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("FlightmastState", Request.QueryString("State"))
                ViewState.Add("FlightmastRefCode", Request.QueryString("RefCode"))
                '   objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlAirline, "partycode", "select partycode from partymast where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAirline, "partycode", "partyname", "select partycode,partyname from partymast where upper(sptypecode)='AIR' and active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlairlinename, "partyname", "partycode", "select partycode,partyname from partymast where upper(sptypecode)='AIR' and active=1 order by partycode", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAirBorCode, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAirBorName, "airportbordername", "airportbordercode", "select airportbordercode,airportbordername from airportbordersmaster where active=1", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlarvlairports, "airportbordercode", "airportbordername", "select airportbordercode,airportbordername from airportbordersmaster where active=1", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlarvlairportname, "airportbordername", "airportbordercode", "select airportbordercode,airportbordername from airportbordersmaster where active=1", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityDeparture, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityArrival, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)

                If ViewState("FlightmastState") = "New" Then

                    filldaysgrid()
                    'SetFocus(TxtFlightnumber)
                    SetFocus(ddlFlightType)
                    lblHeading.Text = "Add New Flight Master"
                    Page.Title = Page.Title + " " + "New Flight Master"
                    btnSave.Text = "Save"
                    chkapplytofuture.Checked = False


                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Flight Master?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("FlightmastState") = "Edit" Then
                    SetFocus(TxtFlightnumber)
                    lblHeading.Text = "Edit Flight Master"
                    Page.Title = Page.Title + " " + "Edit Flight Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    'ShowRecord1(CType(ViewState("FlightmastRefCode"), String))
                    ShowRecord(CType(ViewState("FlightmastRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Flight Master?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    Showdates(CType(ViewState("FlightmastRefCode"), String))
                    grdDates.Visible = True

                    chkapplytofuture.Visible = True
                    lblapplytofuture.Visible = True
                    gvweekdays.Visible = True


                    '  chkapplytofuture.Enabled = False








                ElseIf ViewState("FlightmastState") = "Copy" Then

                    SetFocus(ddlFlightType)
                    lblHeading.Text = "Add New Flight Master"
                    Page.Title = Page.Title + " " + "New Flight Master"
                    btnSave.Text = "Save"
                    chkapplytofuture.Checked = False

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Flight Master?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                    ShowRecord(CType(ViewState("FlightmastRefCode"), String))
                    'ShowRecord1(CType(ViewState("FlightmastRefCode"), String))
                    ViewState.Remove("FlightmastState")
                    ViewState.Remove("FlightmastRefCode")
                    ViewState.Add("FlightmastState", "New")
                    TxtFlighttranid.Value = ""
                    gvweekdays.Visible = True


                    'Dim dt1 As New DataTable

                    'dt1.Columns.Add("frmdate")
                    'dt1.Columns.Add("todate")



                    'dt1.Rows.Add("", "")
                    'grdDates.DataSource = dt1
                    'grdDates.DataBind()








                ElseIf ViewState("FlightmastState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Flight Master"
                    Page.Title = Page.Title + " " + "View Flight Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    chkapplytofuture.Checked = False
                    ShowRecord(CType(ViewState("FlightmastRefCode"), String))
                    'ShowRecord1(CType(ViewState("FlightmastRefCode"), String))
                ElseIf ViewState("FlightmastState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Flight Master"
                    Page.Title = Page.Title + " " + "Delete Flight Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    chkapplytofuture.Checked = False
                    ShowRecord(CType(ViewState("FlightmastRefCode"), String))
                    'ShowRecord1(CType(ViewState("FlightmastRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Flight Master?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                TxtFlightnumber.Attributes.Add("onchnge", "return keypress()")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlAirline.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlairlinename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    TxtFlightnumber.Attributes.Add("onchnage", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FlightsMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region




    Private Sub filldaysgrid()



        Dim dt As New DataTable
        dt.Columns.Add("days")
        dt.Columns.Add("origintime")
        dt.Columns.Add("destinationtime")




        dt.Rows.Add("SUNDAY", "", "")
        dt.Rows.Add("MONDAY", "", "")
        dt.Rows.Add("TUESDAY", "", "")
        dt.Rows.Add("WEDNESDAY", "", "")
        dt.Rows.Add("THURSDAY", "", "")
        dt.Rows.Add("FRIDAY", "", "")
        dt.Rows.Add("SATURDAY", "", "")


        gvweekdays.DataSource = dt

        gvweekdays.DataBind()

        Dim dt1 As New DataTable

        dt1.Columns.Add("frmdate")
        dt1.Columns.Add("todate")



        dt1.Rows.Add("", "")
        grdDates.DataSource = dt1
        grdDates.DataBind()






    End Sub

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("FlightmastState") = "View" Or ViewState("FlightmastState") = "Delete" Then
            TxtFlightnumber.Disabled = True
            ddlAirline.Disabled = True

            ddlairlinename.Disabled = True

            ddlFlightType.Enabled = False

            ddlAirBorCode.Disabled = True
            ddlAirBorName.Disabled = True
            btnAddLines.Enabled = False
            btndeleteLines.Enabled = False
        ElseIf ViewState("FlightmastState") = "Edit" Then
            TxtFlightnumber.Disabled = True
            ddlairlinename.Disabled = False
            ddlAirline.Disabled = False
        End If
    End Sub
#End Region



    Private Function ValidatePage() As Boolean
        Dim dpFDate As New TextBox
        Dim dpTDate As New TextBox
        Dim ObjDate As New clsDateTime
        Dim flag As Integer
        Try
            If ddlFlightType.SelectedItem.Text = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Flight Type should not be blank..')", True)
                ValidatePage = False
                Exit Function

            End If
            'If ddlAirline.Items(ddlAirline.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Airline should not be blank..')", True)
            '    ValidatePage = False
            '    Exit Function
            'End If

            If TxtFlightnumber.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Flight no. should not be blank..')", True)
                ValidatePage = False
                Exit Function
            End If

            If ddlFlightType.SelectedItem.Text = "Arrival" Then
                If txtarrvl.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Orgin Airport should not be blank..')", True)
                    ValidatePage = False
                    Exit Function
                End If

                If ddlCityArrival.Items(ddlCityArrival.SelectedIndex).Text = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('City should not be blank..')", True)
                    ValidatePage = False
                    Exit Function
                End If


                If ddlarvlairports.Items(ddlarvlairports.SelectedIndex).Text = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Destination  should not be blank..')", True)
                    ValidatePage = False
                    Exit Function
                End If

            ElseIf ddlFlightType.SelectedItem.Text = "Departure" Then
                If ddlAirBorCode.Items(ddlAirBorCode.SelectedIndex).Text = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Orgin Airport should not be blank..')", True)
                    ValidatePage = False
                    Exit Function
                End If

                If txtdep.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Destination should not be blank..')", True)
                    ValidatePage = False
                    Exit Function

                End If

                If ddlCityDeparture.Items(ddlCityDeparture.SelectedIndex).Text = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('City should not be blank..')", True)
                    ValidatePage = False
                    Exit Function

                End If


            End If

            flag = 0
            For Each GvRow In grdDates.Rows
                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    flag = 1
                    Exit For
                End If
            Next

            If flag = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Date cannot be blank..')", True)
                ValidatePage = False
                Exit Function
            End If

            For Each GvRow In grdDates.Rows


                dpFDate = GvRow.FindControl("txtfromDate")
                dpTDate = GvRow.FindControl("txtToDate")
                If dpFDate.Text <> "" And dpTDate.Text <> "" Then
                    If ObjDate.ConvertDateromTextBoxToDatabase(dpTDate.Text) < ObjDate.ConvertDateromTextBoxToDatabase(dpFDate.Text) Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('All the To Dates should be greater than From Dates.');", True)
                        '    SetFocus(dpTDate.txtDate)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            Next

            Dim chk As HtmlInputCheckBox, chkcount As Integer
            chkcount = 0
            For i = 0 To gvweekdays.Rows.Count - 1
                chk = CType(gvweekdays.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                If chk.Checked Then
                    chkcount = chkcount + 1
                End If
            Next
            If chkcount = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('At least one day of the week should be ticked');", True)
                ValidatePage = False
                Exit Function

            End If


            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function


    Private Sub Showdates(ByVal RefCode As String)
        Try
            Dim ds As DataSet
            ds = objUtils.ExecuteQuerySqlnew(Session("dbConnectionName"), "Select frmdate,todate from flightmast_dates Where flight_tranid ='" & RefCode & "'")
            For i As Integer = 0 To ds.Tables.Count - 1

                grdDates.DataSource = ds.Tables(i)
                For a As Integer = 0 To ds.Tables(i).Rows.Count

                    Dim dt2 As New DataTable

                    Dim txtfrmdate As TextBox
                    Dim txttodate As TextBox
                    Dim gvRow1 As GridViewRow


                    dt2.Columns.Add("frmdate")
                    dt2.Columns.Add("todate")


                    For Each gvRow1 In grdDates.Rows

                        txtfrmdate = gvRow1.FindControl("txtfromDate")
                        txttodate = gvRow1.FindControl("txtToDate")


                        dt2.Rows.Add(txtfrmdate.Text, txttodate.Text)
                    Next

                Next
                grdDates.DataBind()
            Next


            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            'Dim gvRow As GridViewRow
            'Dim dpFDate As New TextBox
            'Dim dpTDate As New TextBox

            'mySqlCmd = New SqlCommand("Select frmdatec,todatec from flightmast_dates Where flight_tranid ='" & RefCode & "'", mySqlConn)

            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            'If mySqlReader.HasRows Then
            '    While mySqlReader.Read()
            '        For Each gvRow In grdDates.Rows
            '            dpFDate = gvRow.FindControl("txtfromDate")
            '            dpTDate = gvRow.FindControl("txtToDate")
            '            '     Dim lblseason As Label = gvRow.FindControl("lblseason")
            '            If dpFDate IsNot Nothing And dpFDate IsNot Nothing Then
            '                If IsDBNull(mySqlReader("frmdatec")) = False Then
            '                    dpFDate.Text = CType(Format(CType(mySqlReader("frmdatec"), Date), "dd/MM/yyyy"), String)

            '                End If
            '                If IsDBNull(mySqlReader("todatec")) = False Then
            '                    dpTDate.Text = CType(Format(CType(mySqlReader("todatec"), Date), "dd/MM/yyyy"), String)
            '                End If
            '                'If IsDBNull(mySqlReader("seasonname")) = False Then
            '                '    lblseason.Text = CType(mySqlReader("seasonname"), String)
            '                '    txtseasonname.Text = CType(mySqlReader("seasonname"), String)
            '                'End If
            '                'Exit For
            '            End If
            '        Next
            '    End While
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightsMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click ' Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("FlightmastState") = "New" Or ViewState("FlightmastState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If

                    If checkDateOverlapping() = False Then
                        Exit Sub
                    End If





                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction


                    'SQL  Trans start

                    If ViewState("FlightmastState") = "New" Then

                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("FLTMAST", mySqlConn, sqlTrans)
                        TxtFlighttranid.Value = optionval.Trim
                        mySqlCmd = New SqlCommand("sp_add_flight", mySqlConn, sqlTrans)
                    ElseIf ViewState("FlightmastState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_flight", mySqlConn, sqlTrans)


                        Dim mydelmd As SqlCommand
                        mydelmd = New SqlCommand("sp_del_flightmast_days_dates", mySqlConn, sqlTrans)
                        mydelmd.CommandType = CommandType.StoredProcedure
                        mydelmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(TxtFlighttranid.Value.Trim, String)
                        mydelmd.ExecuteNonQuery()

                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure


                    mySqlCmd.Parameters.Add(New SqlParameter("@flightno", SqlDbType.VarChar, 20)).Value = CType(TxtFlightnumber.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(TxtFlighttranid.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@departtime1", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@arrivetime1", SqlDbType.VarChar, 10)).Value = DBNull.Value

                    mySqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 100)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 100)).Value = DBNull.Value


                    If ddlAirline.Value = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@airlinecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@airlinecode", SqlDbType.VarChar, 20)).Value = ddlairlinename.Value
                    End If
                    '  mySqlCmd.Parameters.Add(New SqlParameter("@airlinecode", SqlDbType.VarChar, 20)).Value = CType(ddlAirline.SelectedItem.Text, String)





                    If ddlFlightType.SelectedItem.Text = "Arrival" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Int)).Value = 1
                        If ddlarvlairports.Value = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar, 20)).Value = ddlarvlairportname.Value
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@airport", SqlDbType.VarChar, 20)).Value = txtarrvl.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@city", SqlDbType.VarChar, 20)).Value = ddlCityArrival.Value



                    ElseIf ddlFlightType.SelectedItem.Text = "Departure" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Int)).Value = 0
                        If ddlAirBorName.Value = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar, 20)).Value.Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar, 20)).Value = ddlAirBorName.Value
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@airport", SqlDbType.VarChar, 20)).Value = txtdep.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@city", SqlDbType.VarChar, 20)).Value = ddlCityDeparture.Value

                    End If
                    '   mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Int)).Value = CType(TxtAirlineNm.Value.Trim, Long)

                    mySqlCmd.Parameters.Add(New SqlParameter("@terminal", SqlDbType.Int)).Value = DBNull.Value


                    mySqlCmd.Parameters.Add(New SqlParameter("@depflightno", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@departtime2", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@arrivetime2", SqlDbType.VarChar, 10)).Value = DBNull.Value

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@roundtrip", SqlDbType.Int)).Value = DBNull.Value

                    mySqlCmd.Parameters.Add(New SqlParameter("@showinplist", SqlDbType.Int)).Value = DBNull.Value

                    If chkapplytofuture.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Applyfuture", SqlDbType.Int)).Value = 1
                    ElseIf chkapplytofuture.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Applyfuture", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.VarChar, 20)).Value = DBNull.Value



                    mySqlCmd.ExecuteNonQuery()


                    ' days Grid




                    Dim Cmd As SqlCommand

                    Dim chk As HtmlInputCheckBox
                    Dim origin, departure As HtmlInputText
                    For i = 0 To gvweekdays.Rows.Count - 1
                        chk = CType(gvweekdays.Rows(i).FindControl("chk"), HtmlInputCheckBox)
                        If chk.Checked Then
                            origin = CType(gvweekdays.Rows(i).FindControl("txtarrtime"), HtmlInputText)
                            departure = CType(gvweekdays.Rows(i).FindControl("txtdeptime"), HtmlInputText)
                            Cmd = New SqlCommand("sp_add_flightmast_days", mySqlConn, sqlTrans)
                            Cmd.CommandType = CommandType.StoredProcedure

                            Cmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(TxtFlighttranid.Value.Trim, String)
                            Cmd.Parameters.Add(New SqlParameter("@fldayofweek", SqlDbType.VarChar, 20)).Value = gvweekdays.Rows(i).Cells(1).Text
                            Cmd.Parameters.Add(New SqlParameter("@origintime", SqlDbType.VarChar, 10)).Value = origin.Value
                            Cmd.Parameters.Add(New SqlParameter("@destintime", SqlDbType.VarChar, 10)).Value = departure.Value
                            Cmd.ExecuteNonQuery()
                        End If
                    Next





                    ' dates Grid
                    Dim Dmd As SqlCommand
                    Dim gvRow1 As GridViewRow


                    Dim txtfrmdate As TextBox
                    Dim txttodate As TextBox

                    For Each gvRow1 In grdDates.Rows

                        txtfrmdate = gvRow1.FindControl("txtfromDate")
                        txttodate = gvRow1.FindControl("txtToDate")
                        Dmd = New SqlCommand("sp_add_flightmast_dates", mySqlConn, sqlTrans)
                        Dmd.CommandType = CommandType.StoredProcedure
                        Dmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(TxtFlighttranid.Value.Trim, String)
                        Dmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.DateTime)).Value = txtfrmdate.Text
                        Dmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = txttodate.Text


                        Dmd.ExecuteNonQuery()


                    Next

                    If chkapplytofuture.Checked = True Then

                        If ViewState("FlightmastState") = "Edit" Then


                            Dim ds As DataSet
                            Dim transferdate As DateTime
                            Dim frmdate, todate As TextBox
                            Dim frmdate_, todate_ As DateTime
                            Dim flighttype As Integer

                            Dim strQuery As String = ""
                            If ddlFlightType.SelectedItem.Text = "Arrival" Then
                                flighttype = 1
                                strQuery = "select  distinct arrdate transferdate from  booking_guest_flights where arrflightcode='" & TxtFlightnumber.Value & "' "
                            End If
                            If ddlFlightType.SelectedItem.Text = "Departure" Then
                                flighttype = 2
                                strQuery = "select  distinct depdate transferdate from  booking_guest_flights where depflightcode='" & TxtFlightnumber.Value & "' "
                            End If



                            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQuery)
                            If ds.Tables(0).Rows.Count > 0 Then
                                Dim j As Integer
                                For j = 0 To ds.Tables(0).Rows.Count - 1
                                    transferdate = ds.Tables(0).Rows(j).Item("transferdate").ToString
                                    Dim i As Integer
                                    For i = 0 To grdDates.Rows.Count - 1
                                        frmdate = CType(grdDates.Rows(i).FindControl("txtfromDate"), TextBox)
                                        todate = CType(grdDates.Rows(i).FindControl("txtToDate"), TextBox)
                                        If frmdate.Text <> "" And todate.Text <> "" Then



                                            frmdate_ = frmdate.Text
                                            todate_ = todate.Text


                                            If (transferdate >= frmdate_ And transferdate <= todate_) Then
                                                Dim day As String = transferdate.DayOfWeek.ToString.ToLower

                                                Dim k As Integer
                                                For k = 0 To gvweekdays.Rows.Count - 1
                                                    Dim day2 As String = gvweekdays.Rows(k).Cells(1).Text.ToLower
                                                    Dim chk_days As HtmlInputCheckBox = CType(gvweekdays.Rows(k).FindControl("chk"), HtmlInputCheckBox)
                                                    If day = day2 And chk_days.Checked Then
                                                        Dim txtorigin, txtdestin As HtmlInputText

                                                        txtorigin = CType(gvweekdays.Rows(k).FindControl("txtarrtime"), HtmlInputText)
                                                        txtdestin = CType(gvweekdays.Rows(k).FindControl("txtdeptime"), HtmlInputText)


                                                        Dim flighttime As String
                                                        If flighttype = 1 Then
                                                            flighttime = txtdestin.Value
                                                        Else
                                                            flighttime = txtorigin.Value

                                                        End If


                                                        Dim flightcmd As SqlCommand
                                                        Dim sqlstring As String = ""
                                                        If ddlFlightType.SelectedItem.Text = "Arrival" Then
                                                            sqlstring = "update  booking_guest_flights set arrflighttime='" & flighttime & "'where arrflightcode='" & TxtFlightnumber.Value & "' "
                                                        End If
                                                        If ddlFlightType.SelectedItem.Text = "Departure" Then
                                                            sqlstring = "update  booking_guest_flights set depflighttime='" & flighttime & "'where depflightcode='" & TxtFlightnumber.Value & "' "
                                                        End If

                                                        flightcmd = New SqlCommand(sqlstring, mySqlConn, sqlTrans)
                                                        flightcmd.CommandType = CommandType.Text
                                                        flightcmd.ExecuteNonQuery()

                                                        Exit For
                                                    End If
                                                    ' exit week

                                                Next


                                                Exit For 'exit griddates

                                            End If
                                        End If



                                    Next
                                Next

                            End If


                        End If
                    End If

                ElseIf ViewState("FlightmastState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction          'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_flight", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(TxtFlighttranid.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("FlightsMasterSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('FlightmastWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                If ViewState("FlightmastState") = "New" Then
                    Me.TxtFlighttranid.Value = ""
                End If



            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightsMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region
    'Private Sub ShowRecord1(ByVal RefCode As String)
    '    Try
    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '        mySqlCmd = New SqlCommand("Select frmdatec,todatec from flightmast_dates Where flight_tranid ='" & RefCode & "'", mySqlConn)
    '        mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
    '        If mySqlReader.HasRows Then
    '            If mySqlReader.Read() = True Then
    '                If IsDBNull(mySqlReader("frmdatec")) = False Then
    '                    Me.txtfromDate.Value = CType(mySqlReader("frmdatec"), String)
    '                Else
    '                    Me.txtfromDate.Value = ""
    '                End If

    '                If IsDBNull(mySqlReader("todatec")) = False Then
    '                    Me.txttoDate.Value = CType(mySqlReader("todatec"), String)
    '                Else
    '                    Me.txttoDate.Value = ""
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("FlightsMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    Finally
    '        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
    '        clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
    '        clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
    '    End Try
    'End Sub
    'Private Sub ShowRecord1(ByVal RefCode As String)
    ' '    Try
    'Then
    '                         If IsDBNull(mySqlReader("fromdate")) = False Then
    '                             dpFDate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)

    '                         End If
    '                         If IsDBNull(mySqlReader("todate")) = False Then
    '                             dpTDate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
    '                         End If
    ' 'If IsDBNull(mySqlReader("seasonname")) = False Then
    ' '    lblseason.Text = CType(mySqlReader("seasonname"), String)
    ' '    txtseasonname.Text = CType(mySqlReader("seasonname"), String)
    ' 'End If
    '                         Exit For
    '                     End If
    '                 Next
    '             End While
    '         End If



#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select flight_tranid,isnull(Applyfuture,0) Applyfuture,isnull(flightcode,'') flightcode,isnull(city,'') city,isnull(airport,'') airport,isnull(airportbordercode,'') airportbordercode,isnull(airlinecode,'') airlinecode,isnull(type,1) type,active from flightmast Where flight_tranid ='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("flight_tranid")) = False Then
                        Me.TxtFlighttranid.Value = CType(mySqlReader("flight_tranid"), String)
                    Else
                        Me.TxtFlighttranid.Value = ""
                    End If



                    If IsDBNull(mySqlReader("flightcode")) = False Then
                        Me.TxtFlightnumber.Value = CType(mySqlReader("flightcode"), String)
                    Else
                        Me.TxtFlightnumber.Value = ""
                    End If

                    If IsDBNull(mySqlReader("airlinecode")) = False Then
                        Me.ddlairlinename.Value = CType(mySqlReader("airlinecode"), String)
                        'Me.TxtAirlineNm.Value = Me.ddlAirline.SelectedValue
                        Me.ddlAirline.SelectedIndex = Me.ddlairlinename.SelectedIndex          ' objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"partymast", "partyname", "partycode", CType(mySqlReader("airlinecode"), String))
                    End If



                    If IsDBNull(mySqlReader("type")) = False Then


                        If CType(mySqlReader("type"), String) = "1" Then

                            Me.ddlFlightType.SelectedValue = "Arrival"
                            PanelArrival.Visible = True
                            Pnldeparture.Visible = False

                            ddlCityArrival.Value = CType(mySqlReader("city"), String).Trim
                            hdnarrcity.Value = CType(mySqlReader("city"), String).Trim
                            txtarrvl.Value = CType(mySqlReader("airport"), String).Trim
                            If CType(mySqlReader("airportbordercode"), String) <> "" Then
                                ddlarvlairportname.Value = CType(mySqlReader("airportbordercode"), String)
                                ddlarvlairports.SelectedIndex = Me.ddlarvlairportname.SelectedIndex

                            End If

                        ElseIf CType(mySqlReader("type"), String) = "0" Then

                            Me.ddlFlightType.SelectedValue = "Departure"
                            PanelArrival.Visible = False
                            Pnldeparture.Visible = True
                            ddlCityDeparture.Value = CType(mySqlReader("city"), String).Trim
                            hdndepcity.Value = CType(mySqlReader("city"), String).Trim
                            txtdep.Value = CType(mySqlReader("airport"), String).Trim
                            If CType(mySqlReader("airportbordercode"), String) <> "" Then
                                ddlAirBorName.Value = CType(mySqlReader("airportbordercode"), String)
                                ddlAirBorCode.SelectedIndex = Me.ddlAirBorName.SelectedIndex

                            End If

                        End If
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("f.active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("Applyfuture")) = False Then
                        If CType(mySqlReader("Applyfuture"), String) = "1" Then
                            chkapplytofuture.Checked = True
                        ElseIf CType(mySqlReader("Applyfuture"), String) = "0" Then
                            chkapplytofuture.Checked = False
                        End If
                    End If

                    fillgrid(RefCode)


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FlightsMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub

#End Region

    Private Sub fillgrid(ByVal RefCode As String)
        'fill days grid



        Dim dt As New DataTable
        dt.Columns.Add("days")
        dt.Columns.Add("origintime")
        dt.Columns.Add("destinationtime")

        dt.Rows.Add("SUNDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='sunday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='sunday'"))
        dt.Rows.Add("MONDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='monday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='monday'"))
        dt.Rows.Add("TUESDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='tuesday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='tuesday'"))
        dt.Rows.Add("WEDNESDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='wednesday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='wednesday'"))
        dt.Rows.Add("THURSDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='thursday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='thursday'"))
        dt.Rows.Add("FRIDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='friday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='friday'"))
        dt.Rows.Add("SATURDAY", objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(origintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='saturday'"), objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(destintime,'') from flightmast_days where flight_tranid='" & RefCode & "' and lower(fldayofweek)='saturday'"))


        gvweekdays.DataSource = dt

        gvweekdays.DataBind()
        Dim dt2 As DataSet
        dt2 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select lower(fldayofweek) from flightmast_days where flight_tranid='" & RefCode & "'")
        Dim chk As HtmlInputCheckBox

        Dim i As Integer
        For i = 0 To dt2.Tables(0).Rows.Count - 1

            Select Case dt2.Tables(0).Rows(i).Item(0).ToString
                Case "sunday"
                    chk = CType(gvweekdays.Rows(0).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "monday"
                    chk = CType(gvweekdays.Rows(1).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "tuesday"
                    chk = CType(gvweekdays.Rows(2).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "wednesday"
                    chk = CType(gvweekdays.Rows(3).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "thursday"
                    chk = CType(gvweekdays.Rows(4).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "friday"
                    chk = CType(gvweekdays.Rows(5).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

                Case "saturday"
                    chk = CType(gvweekdays.Rows(6).FindControl("chk"), HtmlInputCheckBox)
                    chk.Checked = True

            End Select
        Next



        'Dim dt1 As New DataSet

        'dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select frmdate,todate from flightmast_dates where flight_tranid='" & RefCode & "'")

        'fillDategrd(grdDates, False, 1)

        'grdDates.DataSource = dt1.Tables(0)
        'grdDates.DataBind()



    End Sub
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

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("FlightsMasterSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkOverlap() As Boolean"
    Public Function checkDateOverlapping() As Boolean
        ' If ViewState("FlightmastState") = "New" Then
        'If objUtils.isDuplicatenew(Session("dbconnectionName"), "flightmast ", "flightcode", CType(TxtFlightnumber.Value.Trim, String)) Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Flight Master Number is already present.');", True)
        '    SetFocus(TxtFlightnumber)
        '    checkForDuplicate = False
        '    Exit Function
        'End If
        ' End If

        Dim i As Integer
        Dim j As Integer
        Dim gvRow1 As GridViewRow


        Dim txtfrmdate As TextBox
        Dim txttodate As TextBox

        For Each gvRow1 In grdDates.Rows

            txtfrmdate = gvRow1.FindControl("txtfromDate")
            txttodate = gvRow1.FindControl("txtToDate")



            Dim parms As New List(Of SqlParameter)
            Dim parm(6) As SqlParameter
            Dim mySqlCmd1 As New SqlCommand("sp_chkflightdates", clsDBConnect.dbConnectionnew(Session("dbconnectionName")))


            mySqlCmd1.CommandType = CommandType.StoredProcedure

            parm(0) = New SqlParameter("@flight_tranid", IIf(TxtFlighttranid.Value = "", String.Empty, TxtFlighttranid.Value))
            parm(1) = New SqlParameter("@flightcode", TxtFlightnumber.Value)

            parm(2) = New SqlParameter("@frmdate", CType(txtfrmdate.Text, DateTime))
            parm(3) = New SqlParameter("@todate", CType(txttodate.Text, DateTime))


            parm(4) = New SqlParameter("@allowflg", SqlDbType.Int)
            parm(4).Direction = ParameterDirection.Output
            parm(4).Value = ""

            parm(5) = New SqlParameter("@errmsg", SqlDbType.VarChar, 50)
            parm(5).Direction = ParameterDirection.Output
            parm(5).Value = ""
            For j = 0 To 5
                mySqlCmd1.Parameters.Add(parm(j))
            Next
            mySqlCmd1.ExecuteNonQuery()





            Dim strError, strError2 As String
            strError = ""
            strError2 = ""

            strError = parm(4).Value.ToString()

            strError2 = parm(5).Value.ToString()


            If strError = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strError2 & "');", True)
                checkDateOverlapping = False
                Exit Function

            End If








        Next

        checkDateOverlapping = True
    End Function
#End Region


#Region " Protected Sub ddlAirline_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAirline.SelectedIndexChanged"

    'Protected Sub ddlAirline_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAirline.SelectedIndexChanged
    '    Try
    '        strSqlQry = ""
    '        TxtAirlineNm.Value = ddlAirline.SelectedValue
    '    Catch ex As Exception

    '    End Try
    'End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True















        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdnew", "flightcode", CType(TxtFlightnumber.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Flight is already used for a Details of TicketingPriceList, cannot delete this Flight');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplistdwknew", "flightcode", CType(TxtFlightnumber.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Flight is already used for a WeekEnd-TicketingPriceList, cannot delete this Flight');", True)
            checkForDeletion = False
            Exit Function

        End If






        'Dim ds As DataSet
        'Dim transferdate As DateTime
        'Dim frmdate, todate As TextBox
        'Dim frmdate_, todate_ As DateTime
        Dim flighttype As Integer

        If ddlFlightType.SelectedItem.Text = "Arrival" Then
            flighttype = 1
        End If
        If ddlFlightType.SelectedItem.Text = "Departure" Then
            flighttype = 2
        End If



        'ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select  distinct transfertype,transferdate from  reservation_online_hotels_arrdep where flightcode='" & TxtFlightnumber.Value & "' and transfertype='" & flighttype & "'")
      
        'If ds.Tables(0).Rows.Count > 0 Then
        '    Dim j As Integer
        '    For j = 0 To ds.Tables(0).Rows.Count - 1
        '        transferdate = ds.Tables(0).Rows(j).Item("transferdate").ToString
        '        Dim i As Integer
        '        For i = 0 To grdDates.Rows.Count - 1
        '            frmdate = CType(grdDates.Rows(i).FindControl("txtfromDate"), TextBox)
        '            todate = CType(grdDates.Rows(i).FindControl("txtToDate"), TextBox)
        '            'If frmdate.Text <> "" And todate.Text <> "" Then



        '            '    frmdate_ = frmdate.Text
        '            '    todate_ = todate.Text


        '            '    If (transferdate >= frmdate_ And transferdate <= todate_) Then

        '            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Flight already used in Reservation cannot Delete');", True)
        '            '        checkForDeletion = False
        '            '        Exit Function

        '            '    End If
        '            'End If



        '        Next
        '    Next

        'End If











        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=FlightsMaster','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnAddLines_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt1 As New DataTable

        Dim txtfrmdate As TextBox
        Dim txttodate As TextBox
        Dim gvRow1 As GridViewRow


        dt1.Columns.Add("frmdate")
        dt1.Columns.Add("todate")


        For Each gvRow1 In grdDates.Rows

            txtfrmdate = gvRow1.FindControl("txtfromDate")
            txttodate = gvRow1.FindControl("txtToDate")


            dt1.Rows.Add(txtfrmdate.Text, txttodate.Text)
        Next

        dt1.Rows.Add("", "")
        grdDates.DataSource = dt1
        grdDates.DataBind()




    End Sub




    Protected Sub ddlFlightType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFlightType.SelectedIndexChanged
        If ddlFlightType.SelectedIndex = 0 Then
            Pnldeparture.Visible = False
            PanelArrival.Visible = False
        End If
        If ddlFlightType.SelectedIndex = 1 Then
            Pnldeparture.Visible = False
            PanelArrival.Visible = True
        End If
        If ddlFlightType.SelectedIndex = 2 Then
            PanelArrival.Visible = False
            Pnldeparture.Visible = True

        End If

    End Sub

    Protected Sub Btnselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In gvweekdays.Rows
            chksel = gvRow1.FindControl("chk")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub Btnunselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnunselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In gvweekdays.Rows
            chksel = gvRow1.FindControl("chk")
            chksel.Checked = False
        Next
    End Sub

    Protected Sub Btncopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btncopy.Click

        Dim txtorigincopy As HtmlInputText
        Dim txtdestinationcopy As HtmlInputText
        Dim txtoriginto As HtmlInputText
        Dim txtdestinationto As HtmlInputText
        Dim txtorigin_old As HtmlInputText
        Dim txtdestination_old As HtmlInputText

        Dim strorigincopy As String = ""
        Dim strdestinationcopy As String = ""

        Dim chk As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim flag As Boolean = False

        For Each gvRow1 In gvweekdays.Rows
            chk = gvRow1.FindControl("chktime")
            If chk.Checked Then
                flag = True

                txtorigincopy = gvRow1.FindControl("txtarrtime")
                txtdestinationcopy = gvRow1.FindControl("txtdeptime")
                'cop
                strorigincopy = txtorigincopy.Value
                strdestinationcopy = txtdestinationcopy.Value
                Exit For
            End If

        Next


        If flag = False Then

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Please select one row to copy');", True)

        Else


            Dim chk1 As HtmlInputCheckBox
            For Each gvRow1 In gvweekdays.Rows
                chk = gvRow1.FindControl("chktime")
                chk1 = gvRow1.FindControl("chk")
                If chk.Checked = False Then
                    flag = True

                    txtoriginto = gvRow1.FindControl("txtarrtime")
                    txtdestinationto = gvRow1.FindControl("txtdeptime")

                    txtorigin_old = gvRow1.FindControl("txtarrtime_old")
                    txtdestination_old = gvRow1.FindControl("txtdeptime_old")
                    If ViewState("FlightmastState") = "Edit" Then


                        If txtorigin_old.Value <> strorigincopy Then
                            chkapplytofuture.Enabled = True
                            chkapplytofuture.Checked = True
                        End If


                        If txtdestination_old.Value <> strdestinationcopy Then


                            chkapplytofuture.Enabled = True
                            chkapplytofuture.Checked = True
                        End If
                    End If


                    If (chk.Checked = False) Or (chk1.Checked = True) Then
                        txtoriginto.Value = strorigincopy
                        txtdestinationto.Value = strdestinationcopy
                    End If
                    'cop

                End If

            Next

        End If









    End Sub

    Protected Sub gvweekdays_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvweekdays.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chk As HtmlInputCheckBox = CType(e.Row.FindControl("chktime"), HtmlInputCheckBox)
            chk.Attributes.Add("onclick", "checkselect(" & e.Row.RowIndex & ")")

            If ViewState("FlightmastState") = "Edit" Then
                Dim txtorigin, txtorigin_old As HtmlInputText
                Dim txtdestination, txtdestination_old As HtmlInputText



                txtorigin = e.Row.FindControl("txtarrtime")
                txtdestination = e.Row.FindControl("txtdeptime")

                txtorigin_old = e.Row.FindControl("txtarrtime_old")
                txtdestination_old = e.Row.FindControl("txtdeptime_old")

                txtorigin.Attributes.Add("onchange", "Keepchanges(" & txtorigin_old.ClientID & "," & txtorigin.ClientID & ");")
                txtdestination.Attributes.Add("onchange", "Keepchanges(" & txtdestination_old.ClientID & "," & txtdestination.ClientID & ");")




            End If






        End If
    End Sub

    Protected Sub grdDates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdDates.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chk As HtmlInputCheckBox = CType(e.Row.FindControl("chk"), HtmlInputCheckBox)
            chk.Attributes.Add("onclick", "checkselect_grddates(" & e.Row.RowIndex & ")")

            Dim fromdate As TextBox
            Dim todate As TextBox

            fromdate = CType(e.Row.FindControl("txtfromDate"), TextBox)
            todate = CType(e.Row.FindControl("txtToDate"), TextBox)
            If fromdate.Text = "" Then
                Exit Sub
            End If

            If todate.Text = "" Then
                Exit Sub
            End If

            Dim transferdate As DateTime
            Dim frmdate_ As DateTime = fromdate.Text
            Dim todate_ As DateTime = todate.Text





            'If ViewState("FlightmastState") = "Edit" Then

            '    Dim ds As DataSet
            '    ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select transferdate from  reservation_online_hotels_arrdep where flightcode='" & TxtFlightnumber.Value & "'")



            '    Dim ab As Integer = ds.Tables.Count


            '    If ab <> 0 Then

            '        If ds.Tables(0).Rows.Count > 0 Then

            '            Dim i As Integer
            '            For i = 0 To ds.Tables(0).Rows.Count - 1
            '                transferdate = ds.Tables(i).Rows(i).Item("transferdate").ToString
            '                If transferdate >= frmdate_ And transferdate <= todate_ Then
            '                    ' e.Row.Enabled = False
            '                    ddlFlightType.Enabled = False
            '                    Pnldeparture.Enabled = False
            '                    PanelArrival.Enabled = False

            '                    Exit Sub
            '                End If



            '            Next

            '        End If

            '    End If


            'End If
        End If
    End Sub

    Protected Sub btndeleteLines_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt1 As New DataTable
        Dim gvRow1 As GridViewRow


        Dim txtfrmdate As TextBox
        Dim txttodate As TextBox

        dt1.Columns.Add("frmdate")
        dt1.Columns.Add("todate")

        Dim i As Integer
        Dim chk As HtmlInputCheckBox



        If grdDates.Rows.Count = 1 Then

            txtfrmdate = grdDates.Rows(0).FindControl("txtfromDate")
            txttodate = grdDates.Rows(0).FindControl("txtToDate")
            If txtfrmdate.Text <> "" Then
                Exit Sub
            End If

            If txttodate.Text <> "" Then
                Exit Sub
            End If


            dt1.Rows.Add("", "")


        Else

            For Each gvRow1 In grdDates.Rows
                chk = gvRow1.FindControl("chk")
                txtfrmdate = gvRow1.FindControl("txtfromDate")
                txttodate = gvRow1.FindControl("txtToDate")


                If chk.Checked = False Then
                    dt1.Rows.Add(txtfrmdate.Text, txttodate.Text)
                End If
            Next

        End If


        grdDates.DataSource = dt1
        grdDates.DataBind()
    End Sub


End Class
