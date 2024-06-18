Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.IO

Partial Class ImportBookingSearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Enum GridCol"
    Enum GridCol
        bookingCode = 0
        bookingDate = 1
        status = 2
        arrivalDate = 3
        departureDate = 4
        agentName = 5
        agentBookingRef = 6
        salecurrcode = 7
        saleValue = 8
        costcurrcode = 9
        costValue = 10
        addDate = 11
        addUser = 12
        modDate = 13
        modUser = 14
        view = 15
    End Enum
#End Region

#Region "Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
    End Sub
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                txtDivcode.Value = ViewState("divcode")

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\ImportBookingSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gvSearch:=gvImportBooking, ViewColumnNo:=GridCol.view)

                btnRecalculate.Visible = btnAddNew.Visible 'changed by mohamed on 09/06/2021

                Session("DtImportBookingDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("DtImportBookingDynamic") = dtDynamic
                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)
                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("strsortExpression", "addDate")
                Session.Add("strsortdirection", SortDirection.Descending)
                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ImportBookingPostBack") Then
            FillGridNew()
        End If
    End Sub
#End Region

#Region "Protected Sub btnvsprocess_Click(sender As Object, e As System.EventArgs) Handles btnvsprocess.Click"
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Sub FilterGrid()"
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessBooking As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "BOOKING CODE"
                    lsProcessBooking = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BOOKING CODE", lsProcessBooking, "BOOKING CODE")
                Case "AGENCY"
                    lsProcessBooking = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AGENCY", lsProcessBooking, "AGENCY")
                Case "STATUS"
                    lsProcessBooking = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("STATUS", lsProcessBooking, "STATUS")
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select
        Next

        Dim dtt As DataTable
        dtt = Session("DtImportBookingDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 
    End Sub
#End Region

#Region " Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean"
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("DtImportBookingDynamic")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("DtImportBookingDynamic") = dtt
            End If
        End If
        Return True
    End Function
#End Region

#Region "Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click"
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("DtImportBookingDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("DtImportBookingDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("DtImportBookingDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("DtImportBookingDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnResetSearch_Click(sender As Object, e As System.EventArgs) Handles btnResetSearch.Click"
    Protected Sub btnResetSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSearch.Click
        ddlOrder.SelectedIndex = 0
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
#End Region

#Region "Private Sub FillGridNew()"
    Private Sub FillGridNew()
        Try
            Dim strBindCondition As String = ""
            strBindCondition = BuildConditionNew()
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            Dim myDS As New DataSet
            lblMsg.Visible = False
            If gvImportBooking.PageIndex < 0 Then gvImportBooking.PageIndex = 0
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "Desc"
            strSqlQry = "select h.bookingCode,h.bookingDate, case when isnull(amended,0)=1 then 'Amended' else case when isnull(cancelled,0)=1 then 'Cancelled' " & _
            "else 'New' end end status,h.agentCode,a.agentname, h.agentBookingRef,h.arrivalDate,h.departureDate," & _
            "h.salecurrcode,h.saleValue,h.costcurrcode,h.costValue,h.adddate,h.adduser,h.moddate,h.moduser from " & _
            "import_Bookingelements_header h(nolock) inner join agentmast a(nolock) on h.agentCode=a.agentcode"
            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                gvImportBooking.DataSource = myDS.Tables(0)
                gvImportBooking.PageSize = pagevaluecus
                gvImportBooking.DataBind()
            Else
                gvImportBooking.PageIndex = 0
                gvImportBooking.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Private Function BuildConditionNew() As String"
    Private Function BuildConditionNew() As String
        Dim dtt As DataTable
        dtt = Session("DtImportBookingDynamic")
        Dim strBookingCodeValue As String = ""
        Dim strStatusValue As String = ""
        Dim strAgentValue As String = ""
        Dim strTextValue As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "BOOKING CODE" Then
                        If strBookingCodeValue <> "" Then
                            strBookingCodeValue = strBookingCodeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strBookingCodeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "AGENCY" Then
                        If strAgentValue <> "" Then
                            strAgentValue = strAgentValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strAgentValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "STATUS" Then
                        If strStatusValue <> "" Then
                            strStatusValue = strStatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strStatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strTextValue <> "" Then
                            strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString
                        Else
                            strTextValue = dtt.Rows(i)("Value").ToString
                        End If
                    End If
                Next
            End If
            strWhereCond = ""
            If strBookingCodeValue.Trim <> "" Then
                strWhereCond = "h.bookingCode IN (" & Trim(strBookingCodeValue.Trim.ToUpper) & ")"
            End If
            If strStatusValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "(case when isnull(h.amended,0)=1 then 'Amended' else case when isnull(h.cancelled,0)=1 then 'Cancelled' " & _
                                  "Else 'New' end end) in (" & Trim(strStatusValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and (case when isnull(h.amended,0)=1 then 'Amended' else case when isnull(h.cancelled,0)=1 then 'Cancelled' " & _
                                  "Else 'New' end end) in (" & Trim(strStatusValue.Trim.ToUpper) & ")"
                End If
            End If
            If strAgentValue.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "a.agentName in (" & Trim(strAgentValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & "and a.agentName in (" & Trim(strAgentValue.Trim.ToUpper) & ")"
                End If
            End If

            If strTextValue <> "" Then
                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strTextValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = "h.bookingcode like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                            "or a.agentName like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        Else
                            strWhereCond1 = strWhereCond1 & " or h.bookingcode like '%" & Trim(strValue.Trim.ToUpper) & "%' " &
                            "or a.agentName like '%" & Trim(strValue.Trim.ToUpper) & "%'"
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                End If
            End If
            If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then
                If ddlOrder.SelectedValue = "B" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),h.bookingDate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),h.bookingDate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "A" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),h.arrivalDate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),h.arrivalDate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "D" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),h.departureDate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),h.departureDate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "C" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10),h.adddate,103),103) between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime, '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),h.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,'" + txtToDate.Text + "',103)) "
                    End If
                ElseIf ddlOrder.SelectedValue = "M" Then
                    If Trim(strWhereCond) = "" Then
                        strWhereCond = " (CONVERT(datetime, convert(varchar(10), h.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    Else
                        strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10), h.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                    End If
                End If
            End If
            BuildConditionNew = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            BuildConditionNew = ""
        End Try
    End Function
#End Region

#Region "Protected Sub btnHelp_Click(sender As Object, e As System.EventArgs) Handles btnHelp.Click"
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(sender As Object, e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        Dim ExcelUploadMethod As String = "0"  ' 1 for old page, 2 for new page
        ExcelUploadMethod = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =5753")
        If ExcelUploadMethod = "2" Then
            strpop = "window.open('ImportBookingNew.aspx?State=New&divid=" & ViewState("divcode") & "' ,'ImportBooking');"
        Else
            strpop = "window.open('ImportBooking.aspx?State=New&divid=" & ViewState("divcode") & "' ,'ImportBooking');"
        End If
        ' strpop = "window.open('ImportBooking.aspx?State=New&divid=" & ViewState("divcode") & "' ,'ImportBooking');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

#Region "Protected Sub btnRecalculate_Click(sender As Object, e As System.EventArgs) Handles btnRecalculate.Click"
    Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecalculate.Click
        Dim appid As String = CType(Request.QueryString("appid"), String)
        Dim strpop As String = ""
        strpop = "window.open('ImportBookingRecalculate.aspx?appid=" & appid & "&divid=" & ViewState("divcode") & "' ,'ImportBookingRecalculate');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
#End Region

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvImportBooking_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvImportBooking.PageIndexChanging"
    Protected Sub gvImportBooking_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvImportBooking.PageIndexChanging
        gvImportBooking.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvImportBooking_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvImportBooking.RowCommand"
    Protected Sub gvImportBooking_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvImportBooking.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblBookingCode As Label
            lblBookingCode = gvImportBooking.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblBookingCode")
            Dim strpop As String = ""
            If e.CommandName = "View" Then
                Try
                    lblTitle.Text = "View Booking --" + lblBookingCode.Text
                    Dim myDS As New DataSet

                    strSqlQry = ""

                    strSqlQry = "select serviceType,startdate,enddate,noNights,agentCode,agent,agentBookingRef,guestName,servDescription,ProductGroup,salescurrcode,salesPrice,convrate," & _
                    "SupplierID, SupplierName, costCurrCode, Costprice, bookingDate, Linebookingdate, Nationality, NoofRooms, SalesTax, CostTax, agencyCtry" & _
                    "  from import_Bookingelements_detail where bookingcode='" & lblBookingCode.Text & "' order by startdate,enddate"

                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(myDS)
                    If myDS.Tables(0).Rows.Count > 0 Then
                        GrdviewBook.DataSource = myDS.Tables(0)
                        GrdviewBook.DataBind()
                    Else
                        GrdviewBook.DataSource = Nothing
                        GrdviewBook.DataBind()
                    End If
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                Catch ex As Exception
                    If Not SqlConn Is Nothing Then
                        If SqlConn.State = ConnectionState.Open Then
                            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                            clsDBConnect.dbConnectionClose(SqlConn)
                        End If
                    End If
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                    objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                End Try
                ModalExtraPopup.Show()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvImportBooking_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImportBooking.RowDataBound"
    Protected Sub gvImportBooking_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvImportBooking.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
            Dim lblCostAmount As Label = CType(e.Row.FindControl("lblCostAmount"), Label)
            If IsNumeric(lblCostAmount.Text) Then
                lblCostAmount.Text = Math.Round(Convert.ToDecimal(lblCostAmount.Text), decimalPlaces)
            End If

        End If
    End Sub
#End Region

#Region "Protected Sub gvImportBooking_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvImportBooking.Sorting"
    Protected Sub gvImportBooking_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvImportBooking.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColumn()
    End Sub
#End Region

#Region "Public Sub SortGridColumn()"
    Public Sub SortGridColumn()
        Dim DataTable As DataTable
        FillGridNew()
        DataTable = gvImportBooking.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvImportBooking.DataSource = dataView
            gvImportBooking.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strBindCondition As String = ""
        Try
            If gvImportBooking.Rows.Count > 0 Then
                strBindCondition = BuildConditionNew()
                Dim strorderby As String = Session("strsortexpression")
                Dim strsortorder As String = IIf(Session("strsortdirection") = "0", "Asc", "Desc")
                Dim myDS As New DataSet
                strSqlQry = "select h.bookingCode,h.bookingDate, case when isnull(amended,0)=1 then 'Amended' else case when isnull(cancelled,0)=1 then 'Cancelled' " & _
                "else 'New' end end status,h.agentCode,a.agentname, h.agentBookingRef,h.arrivalDate,h.departureDate," & _
                "h.salecurrcode,h.saleValue,h.costcurrcode,h.costValue,h.adddate,h.adduser,h.moddate,h.moduser from " & _
                "import_Bookingelements_header h(nolock) inner join agentmast a(nolock) on h.agentCode=a.agentcode"
                If strBindCondition <> "" Then
                    strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                Else
                    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(myDS, "ImportHeader")
                objUtils.ExportToExcel(myDS, Response)
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete"
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Dim lbltitle As Label = CType(Me.Master.FindControl("title"), Label)
        Dim strTitle As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code ='" & ViewState("divcode") & "'")
        lbltitle.Text = strTitle
        Me.Page.Title = strTitle
    End Sub
#End Region

#Region "Protected Sub GrdviewBook_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdviewBook.RowDataBound"
    Protected Sub GrdviewBook_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdviewBook.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
            Dim lblCostAmount As Label = CType(e.Row.FindControl("lblCostAmount"), Label)
            If IsNumeric(lblCostAmount.Text) Then
                lblCostAmount.Text = Math.Round(Convert.ToDecimal(lblCostAmount.Text), decimalPlaces)
            End If

        End If
    End Sub
#End Region

End Class
