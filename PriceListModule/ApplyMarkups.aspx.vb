'------------================--------------=======================------------------================
'   Module Name    :    Apply Markups.aspx
'   Developer Name :    Abin Paul
'   Date           :    19 Dec 2016
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Generic


#End Region

Partial Class ApplyMarkups
    Inherits System.Web.UI.Page
#Region "Enum GridCol"
    Enum GridLogCol
        View = 12
        Copy = 13
        Edit = 14
        Delete = 15

    End Enum
    Enum GridPendingCol
        SelectItem = 12
        Copy = 13
        Edit = 14
        Delete = 15

    End Enum
#End Region
    Protected Sub btnResetSelection2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection2.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamicSearch") = dtt
        dlList2.DataSource = dtt
        dlList2.DataBind()
        'FilterGridSEARCH()
        bindsearchgrid(False)

  
    End Sub
#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnExportToExcel.Click
        Try
            If gvSearch.Rows.Count <> 0 Then
                Dim lsFormulaId As String = ""
                Dim lsInvType As String = ""
                Dim lsRoomClscode As String = ""
                Dim lsRoomCls As String = ""
                Dim lsText As String = ""
                Dim dttSess As DataTable

                dttSess = Session("sDtDynamicSearch")
                If dttSess IsNot Nothing Then
                    If dttSess.Rows.Count > 0 Then
                        For Each dtR As DataRow In dttSess.Rows
                            Select Case UCase(Trim(dtR("code")))
                                Case "FORMULA-ID"
                                    lsFormulaId += IIf(lsFormulaId.Trim = "", "", ",") + dtR("value")
                                Case "INVENTORYTYPE"
                                    lsInvType += IIf(lsInvType.Trim = "", "", ",") + dtR("value")

                                Case "ROOMCLASSIFICATION"
                                    lsRoomCls += IIf(lsRoomCls.Trim = "", "", ",") + objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ROOM_CLASSIFICATION", "ROOMCLASScode", "ROOMCLASSname", dtR("value"))

                                    ' lsRoomClscode = 
                                Case "TEXT"
                                    lsText += IIf(lsText.Trim = "", "", ",") + dtR("value")
                            End Select

                        Next
                    End If
                End If
                Dim constring As String = ConfigurationManager.ConnectionStrings(Session("dbconnectionName")).ConnectionString
                Dim dtt As New DataSet
                '   gvSearch.DataSource = Nothing
                Using con As New SqlConnection(constring)
                    Using cmd As New SqlCommand("[sp_search_markup]")
                        cmd.CommandType = CommandType.StoredProcedure
                        'FilterGridSEARCH()
                        cmd.Parameters.AddWithValue("@inventorytype", lsInvType)
                        cmd.Parameters.AddWithValue("@partycode", "")
                        cmd.Parameters.AddWithValue("@roomclasscode ", lsRoomCls)
                        cmd.Parameters.AddWithValue("@formulaid ", lsFormulaId)
                        cmd.Parameters.AddWithValue("@text ", lsText)
                        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                            cmd.Parameters.AddWithValue("@fromdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
                            cmd.Parameters.AddWithValue("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))
                        End If
                        Using sda As New SqlDataAdapter()
                            cmd.Connection = con
                            sda.SelectCommand = cmd
                            sda.Fill(dtt, "markups")
                            'gvSearch.DataSource = dtt.Tables(0)
                            'gvSearch.DataBind()
                            objUtils.ExportToExcel(dtt, Response)
                        End Using
                    End Using
                End Using



            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "Protected Sub btnExportToExcel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Try
            ' Session.Add("export", "1")
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls")
            Response.Charset = ""
            Response.Cache.SetCacheability(HttpCacheability.NoCache)

            Response.ContentType = "application/vnd.ms-excel"
            Using sw As New IO.StringWriter()
                Dim hw As New HtmlTextWriter(sw)

                'To Export all pages
                'gvSearch.AllowPaging = False
                bindsearchgrid(True)

                Dim lsURLStr As String = ""
                lsURLStr = "window.open('ApplyMarkupsExcel.aspx','_blank','fullscreen=yes,height=400px,width=800px,top=0,left=0')"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", lsURLStr, True)

                'gvSearch.HeaderRow.BackColor = Color.White
                'For Each cell As TableCell In gvSearch.HeaderRow.Cells
                '    cell.BackColor = gvSearch.HeaderStyle.BackColor
                'Next
                'For Each row As GridViewRow In gvSearch.Rows
                '    row.BackColor = Color.White
                '    For Each cell As TableCell In row.Cells
                '        If row.RowIndex Mod 2 = 0 Then
                '            cell.BackColor = gvSearch.AlternatingRowStyle.BackColor
                '        Else
                '            cell.BackColor = gvSearch.RowStyle.BackColor
                '        End If
                '        cell.CssClass = "textmode"
                '    Next
                'Next

                'gvSearch.RenderControl(hw)
                ''style to format numbers to string
                'Dim style As String = "<style> .textmode { } </style>"
                'Response.Write(style)
                'Response.Output.Write(sw.ToString())
                'Response.Flush()
                'Response.[End]()

            End Using
        Catch exa As Threading.ThreadAbortException

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLogGrid.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub



#End Region
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Verifies that the control is rendered 
    End Sub


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim objUser As New clsUser
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                hdOPMode.Value = "N"
                hdFillByGrid.Value = "N"
                hdLinkButtonValue.Value = ""
                dvNew.Visible = True
                dvLog.Visible = False
                dvSearch.Visible = False
                dvPendingApproval.Visible = False
                'txtFromDate.Text = DateTime.Now()
                'txtToDate.Text = DateTime.Now()



                Session("ApplyMarkupState") = "New"
                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                ucMarkup.sbSetPageState("", "ApplyMarkup", CType(Session("ApplyMarkupState"), String))

                ucMarkup.clearsessions() 'changed by mohamed on 14/08/2018
                ucMarkup.sbSetPageState("", "ApplyMarkup", CType(Session("ApplyMarkupState"), String))

                'If CType(Session("ApplyMarkupState"), String) <> "New" Then
                '    'Session("ContractRefCode") = CType(Request.QueryString("contractid"), String)
                '    'Session("contractid") = CType(Request.QueryString("contractid"), String)
                '    ucMarkup.sbSetPageState(Session("contractid"), Nothing, Nothing)
                'End If
                Session("isAutoTick_wuccountrygroupusercontrol") = 1
                ucMarkup.sbShowCountry()

                sbGClearExhibitionAndHolidayGridColumns()
                FillDaysOfWeek()
                FillInventoryType()
                btnNew.Enabled = True

                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")


                Page.Title = "Apply Markup"



                If hdOPMode.Value = "N" Then
                    lblHeading.Text = "Apply Markups"
                    Page.Title = Page.Title + " " + "- New "
                    btnSave.Text = "Save"
                    ' txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from citymast") + 1
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then

                    lblHeading.Text = "Edit Applied Markups"
                    Page.Title = Page.Title + " " + "Edit Applied Markups"
                    btnSave.Text = "Update"

                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Applied Markups"
                    Page.Title = Page.Title + " " + "Delete Applied Markups"
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete markup?')==false)return false;")

                ElseIf hdOPMode.Value = "V" Then
                    lblHeading.Text = "Apply Markups"
                    Page.Title = Page.Title + " " + "Apply Markups"
                    ' btnSave.Text = "Delete"
                End If

                Dim AppId As String = CType(Request.QueryString("appid"), String)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)

                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(strappname, String), "PriceListModule\ApplyMarkups.aspx?appid=" + strappid, btnNew, btnEdit, _
                                                   btnDelete, btnView, btnExcel, btnHotelReport)
                    'objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                               CType(strappname, String), "PriceListModule\ApplyMarkups.aspx?appid=" + strappid, btnNew, btnEdit, _
                    '                               btnDelete, btnView, btnExcel, btnHotelReport)
                    '   Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ContractsSearch.aspx?appid=1", String), CType(Request.QueryString("appid"), Integer))
                    ' Dim CalledfromValue As String = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\ApplyMarkups.aspx?appid=" + strappid, String), CType(Request.QueryString("appid"), Integer))

                    '*** Danny 24-04-2018
                    ''Dim CalledfromValue As String = "22"
                    Dim CalledfromValue As String = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "7")
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\ApplyMarkups.aspx?appid=" + strappid, CType(CalledfromValue, String), btnNew, btnEdit, _
                 btnDelete, gvLog, GridLogCol.Edit, GridLogCol.Delete, GridLogCol.View, 0, GridLogCol.Copy)
                    objUser.CheckUserSubRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                     CType(strappname, String), "PriceListModule\ApplyMarkups.aspx?appid=" + strappid, CType(CalledfromValue, String), btnNew, btnEdit, _
               btnDelete, gvPendingApproval, GridPendingCol.Edit, GridPendingCol.Delete, GridPendingCol.SelectItem, 0, GridPendingCol.Copy)

                    btnEdit.Visible = False
                    btnDelete.Visible = False
                    btnView.Visible = False
                    '   btnsearch.Visible = False
                End If

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    '' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcCountry = New DataColumn("Country", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcCountry)
                    Session("sDtHotelDynamic") = dtDynamic
                    '--------end

                    '' Create a Dynamic datatable ---- Start
                    Session("sDtDynamicSearch") = Nothing
                    Dim dtDynamicSearch = New DataTable()
                    Dim dcSearchCode = New DataColumn("Code", GetType(String))
                    Dim dcSearchValue = New DataColumn("Value", GetType(String))
                    Dim dcSearchCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                    dtDynamicSearch.Columns.Add(dcSearchCode)
                    dtDynamicSearch.Columns.Add(dcSearchValue)
                    dtDynamicSearch.Columns.Add(dcSearchCodeAndValue)
                    Session("sDtDynamicSearch") = dtDynamicSearch
                    '--------end

                    '' Create a Dynamic datatable ---- Start
                    Dim dtHotelGroupDetails = New DataTable()
                    Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                    Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailsCountry = New DataColumn("Country", GetType(String))
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsType)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsTypeName)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsCode)
                    dtHotelGroupDetails.Columns.Add(dcGroupDetailsCountry)
                    Session("sDtHotelGroupDetails") = dtHotelGroupDetails
                    '--------end
                    Session("strsortexpression") = "partymast.partyname"
                    ' FillGridNew()
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Session("strsortexpression") = "partymast.partyname"
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SuppliersWindowPostBack") Then
            FilterGrid()
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ApplyMarkupWindowPostBack") Then
            If hdMarkUp.Value = "Y" Then
                FillMarkups()
                ModalSelectMarkup.Show()
            End If

        End If

        'txtFromDate.Attributes.Add("onchange", "setdate();")
        btnFilter.Attributes.Add("onclick", "return datesValidation()")
        txtFromDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")
        txtToDate.Attributes.Add("onchange", "checkdates('" & txtFromDate.ClientID & "','" & txtToDate.ClientID & "');")

    End Sub
#End Region

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case 13
                        Me.MasterPageFile = "~/VisaMaster.master"      'Added by Archana on 05/06/2015 for VisaModule
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"

                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        'Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        'Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then

                    Dim strSelectedCountriesList As String = ""
                    strSelectedCountriesList = ucMarkup.checkcountrylist.ToString

                    Dim strSelectedAgentList As String = ""
                    strSelectedAgentList = ucMarkup.checkagentlist.ToString()

                    Dim lsCustomerGroupList As String = ""
                    lsCustomerGroupList = ucMarkup.checkcustomerGroupFitlerList.ToString()


                    If hdMissing.Value <> "1" Then
                        If strSelectedCountriesList = "" And strSelectedAgentList = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any country or agent.');", True)
                            Exit Sub
                        End If


                        If checkForMissingDate() = False Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter markup dates.');", True)
                            Exit Sub
                        End If
                        If checkForDateOverlap() = False Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Inavlid date range.');", True)
                            Exit Sub
                        End If

                        If checkForDayOfWeek() = False Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any day of week.');", True)
                            Exit Sub
                        End If
                        If checkForInventoryType() = False Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any Inventory Type.');", True)
                            Exit Sub
                        End If
                        If txtMarkupFormula.Text = "" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any markup formula.');", True)
                            Exit Sub
                        End If
                    End If
                    Dim dtsHotelGroupDetails As DataTable
                    dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                    If dtsHotelGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The hotels are not selected.');", True)
                        Exit Sub
                    End If

                    Dim dt As New DataTable
                    Dim strSeachCriteria As String = ""
                    dt = Session("sDtHotelDynamic")
                    If dt.Rows.Count > 0 Then
                        For i As Integer = 0 To dt.Rows.Count - 1
                            If strSeachCriteria = "" Then
                                strSeachCriteria = dt.Rows(i)("Country").ToString
                            Else
                                strSeachCriteria = strSeachCriteria & "," & dt.Rows(i)("Country").ToString
                            End If
                        Next
                    End If



                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        frmmode = 1

                    ElseIf hdOPMode.Value = "E" Then
                        frmmode = 2
                    End If

                    Dim strBufferDates As New System.Text.StringBuilder
                    Dim mRow As Integer = 0
                    strBufferDates.Append("<ApplyMarkup_Dates>")
                    For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows

                        Dim txtFrom As TextBox = CType(row.FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
                        Dim txtTo As TextBox = CType(row.FindControl("txtExhibitionAndHolidayToDate"), TextBox)
                        If txtFrom.Text <> "" And txtTo.Text <> "" Then
                            mRow = mRow + 1
                            strBufferDates.Append("<ApplyMarkup_Date>")
                            strBufferDates.Append(" <LineNo>" & mRow.ToString & " </LineNo>")
                            strBufferDates.Append(" <FromDate>" & txtFrom.Text.ToString & " </FromDate>")
                            strBufferDates.Append(" <ToDate>" & txtTo.Text.ToString & " </ToDate>")
                            strBufferDates.Append("</ApplyMarkup_Date>")
                        End If
                    Next
                    strBufferDates.Append("</ApplyMarkup_Dates>")

                    Dim dtHotelDynamic As DataTable
                    dtHotelDynamic = Session("sDtHotelDynamic")
                    Dim strRoomClassName As String = ""
                    If dtHotelDynamic.Rows.Count > 0 Then
                        For i As Integer = 0 To dtHotelDynamic.Rows.Count - 1
                            If dtHotelDynamic.Rows(i)("Code").ToString = "RC" Then
                                If strRoomClassName = "" Then
                                    strRoomClassName = dtHotelDynamic.Rows(i)("Country").ToString.Replace("(RC)", "")
                                Else
                                    strRoomClassName = strRoomClassName & "," & dtHotelDynamic.Rows(i)("Country").ToString.Replace("(RC)", "")
                                End If
                            End If
                        Next
                    End If



                    Dim strBuffer As New System.Text.StringBuilder
                    If dtsHotelGroupDetails.Rows.Count > 0 Then

                        strBuffer.Append("<ApplyMarkup_Hotels>")
                        For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                            strBuffer.Append("<ApplyMarkup_Hotel>")
                            strBuffer.Append(" <PartyCode>" & dtsHotelGroupDetails.Rows(i)("Code").ToString & " </PartyCode>")
                            strBuffer.Append("</ApplyMarkup_Hotel>")
                        Next
                        strBuffer.Append("</ApplyMarkup_Hotels>")
                    End If
                    Dim strDaysOfTheWeek As String = ""
                    Dim chkWSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkWSelectAll"), CheckBox)
                    If chkWSelectAll.Checked = True Then
                        strDaysOfTheWeek = "ALL"
                    Else
                        For Each gvRow As GridViewRow In gvWeekOfDays.Rows
                            Dim chkWSelect As CheckBox = CType(gvRow.FindControl("chkWSelect"), CheckBox)
                            If chkWSelect.Checked = True Then
                                Dim lblWeekDaysCode As Label = CType(gvRow.FindControl("lblWeekDaysCode"), Label)
                                If strDaysOfTheWeek = "" Then
                                    strDaysOfTheWeek = lblWeekDaysCode.Text
                                Else
                                    strDaysOfTheWeek = strDaysOfTheWeek & "," & lblWeekDaysCode.Text
                                End If
                            End If
                        Next
                    End If


                    Dim strInventoryTypes As String = ""
                    ' Dim chkInvSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkInvSelectAll"), CheckBox)
                    'If chkWSelectAll.Checked = True Then
                    '    strInventoryTypes = "ALL"
                    'Else
                    For Each gvRow As GridViewRow In gvInventoryType.Rows
                        Dim chkInvSelect As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
                        If chkInvSelect.Checked = True Then
                            Dim lblInventoryType As Label = CType(gvRow.FindControl("lblInventoryType"), Label)
                            If strInventoryTypes = "" Then
                                strInventoryTypes = lblInventoryType.Text
                            Else
                                strInventoryTypes = strInventoryTypes & "," & lblInventoryType.Text
                            End If
                        End If
                    Next
                    ' End If


                    If hdMissing.Value <> "1" Then



                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        mySqlCmd = New SqlCommand()
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.CommandTimeout = 0 'changed by mohamed on 17/05/2018
                        mySqlCmd.CommandText = "sp_ApplyMarkup_Validate"
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMrkpId", SqlDbType.VarChar, 20)).Value = hdApplyMarkupId.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@DaysOfTheWeek", SqlDbType.VarChar, 100)).Value = strDaysOfTheWeek
                        mySqlCmd.Parameters.Add(New SqlParameter("@InventoryTypes", SqlDbType.VarChar, 5000)).Value = strInventoryTypes
                        mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = txtmarkupCode.Text
                        mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 20)).Value = txtMarkupFormulaName.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplicableTo", SqlDbType.VarChar, 5000)).Value = txtApplicableTo.Text.Trim
                        mySqlCmd.Parameters.Add(New SqlParameter("@SearchCriteria", SqlDbType.VarChar, 1000)).Value = strSeachCriteria
                        If chkActive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 1
                        ElseIf chkActive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 0
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@Userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Hotels_XML", SqlDbType.Xml)).Value = strBuffer.ToString
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Countries", SqlDbType.VarChar, 8000)).Value = strSelectedCountriesList
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Agents", SqlDbType.VarChar, 8000)).Value = strSelectedAgentList ''' modified by shahul 20/03/18
                        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Dates_XML", SqlDbType.Xml)).Value = strBufferDates.ToString
                        mySqlCmd.Parameters.Add(New SqlParameter("@RoomClassName", SqlDbType.VarChar, 500)).Value = strRoomClassName
                        mySqlCmd.Parameters.Add(New SqlParameter("@OPMode", SqlDbType.Int)).Value = frmmode
                        myDataAdapter = New SqlDataAdapter()
                        mySqlCmd.Connection = mySqlConn
                        myDataAdapter.SelectCommand = mySqlCmd
                        Dim dsValidate As New DataSet
                        myDataAdapter.Fill(dsValidate)

                        If dsValidate.Tables(0).Rows.Count > 0 Then
                            If dsValidate.Tables(0).Rows.Count > 0 Then
                                Dim lsMessageExists As String = "alert('The same Markup already applied"
                                If dsValidate.Tables.Count >= 2 Then
                                    If dsValidate.Tables(1).Rows.Count > 0 Then
                                        lsMessageExists += " in " & dsValidate.Tables(1).Rows(0)("ApplyMarkExistIDs")
                                    End If
                                End If
                                lsMessageExists += ".');"
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", lsMessageExists, True)
                                Exit Sub
                            End If
                        End If

                        'If dsValidate.Tables(0).Rows.Count > 0 Then
                        '    Dim strMessage As String = ""
                        '    hdMissing.Value = "1"
                        '    For ss As Integer = 0 To dsValidate.Tables(0).Rows.Count - 1
                        '        If strMessage = "" Then
                        '            strMessage = dsValidate.Tables(0).Rows(ss)("FROM_DATE").ToString & " to " & dsValidate.Tables(0).Rows(ss)("TO_DATE").ToString
                        '        Else
                        '            strMessage = strMessage & ", " & dsValidate.Tables(0).Rows(ss)("FROM_DATE").ToString & " to " & dsValidate.Tables(0).Rows(ss)("TO_DATE").ToString
                        '        End If
                        '    Next
                        '    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Markup already applied in the date range " & strMessage & ".');", True)
                        '    Dim strmsg3 As String = "Markup already applied in the date range " & strMessage & " Do you want to continue ?"
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "ConfirmMarkup('" + strmsg3 + "','" + btnSave.ClientID + "');", True)
                        '    Exit Sub
                        'End If

                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_Add_Mod_ApplyMarkup_Hotel_New", mySqlConn, sqlTrans)

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMrkpId", SqlDbType.VarChar, 20)).Value = hdApplyMarkupId.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@DaysOfTheWeek", SqlDbType.VarChar, 100)).Value = strDaysOfTheWeek
                    mySqlCmd.Parameters.Add(New SqlParameter("@InventoryTypes", SqlDbType.VarChar, 5000)).Value = strInventoryTypes
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = txtmarkupCode.Text
                    mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 20)).Value = txtMarkupFormulaName.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplicableTo", SqlDbType.VarChar, 5000)).Value = txtApplicableTo.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@SearchCriteria", SqlDbType.VarChar, 1000)).Value = strSeachCriteria
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@Userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Hotels_XML", SqlDbType.Xml)).Value = strBuffer.ToString
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Countries", SqlDbType.VarChar, 8000)).Value = strSelectedCountriesList '1000 'changed by mohamed on 17/05/2018
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Agents", SqlDbType.VarChar, 8000)).Value = strSelectedAgentList '' modified by shahul 20/03/18
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Dates_XML", SqlDbType.Xml)).Value = strBufferDates.ToString
                    mySqlCmd.Parameters.Add(New SqlParameter("@RoomClassName", SqlDbType.VarChar, 500)).Value = strRoomClassName
                    mySqlCmd.Parameters.Add(New SqlParameter("@OPMode", SqlDbType.Int)).Value = frmmode
                    mySqlCmd.Parameters.Add(New SqlParameter("@SelectedCustomerGroupFilter", SqlDbType.VarChar, 8000)).Value = lsCustomerGroupList 'changed by mohamed on 15/08/2018
                    mySqlCmd.CommandTimeout = 0 ' 100000  'changed by mohamed on 17/05/2018
                    mySqlCmd.ExecuteNonQuery()


                ElseIf hdOPMode.Value = "D" Then


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_Delete_ApplyMarkup_Hotel_New", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMrkpId", SqlDbType.VarChar, 20)).Value = hdApplyMarkupId.Value
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                ucMarkup.clearsessions()   '' ADDED shahul 20/03/18
                ModalPopupDays.Hide()   '' Added shahul 22/03/18

                If hdOPMode.Value = "N" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Markup Applied Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Markup Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Markup Deleted Successfully..');", True)
                End If
                ClearFields()
                hdOPMode.Value = "N"
                hdMissing.Value = ""
                Dim col As System.Drawing.Color
                col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
                btnCancel_new.BackColor = col
                btnCancel_new.ForeColor = Drawing.Color.White

                Dim col1 As System.Drawing.Color
                col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
                btnNew.BackColor = col1
                btnEdit.BackColor = col1
                btnDelete.BackColor = col1
                btnView.BackColor = col1
                btnNew.ForeColor = col
                btnEdit.ForeColor = col
                btnView.ForeColor = col
                btnDelete.ForeColor = col


                dlList.Visible = True

                lblHeading.Text = "Apply Markup"

                dvNew.Visible = False
                dvLog.Visible = True
                dvPendingApproval.Visible = False
                dvSearch.Visible = False
                BindLogs()
                btnSave.Text = "Save"

                '  FillGridNew()

            End If
        Catch ex As Exception
            '' Added shahul 22/03/18
            ModalPopupDays.Hide()
            If Not mySqlConn Is Nothing Then
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CntryGrpWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
    End Function
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ApplyMarkupHotels','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



    ''' <summary>
    ''' GetHotelGroup 
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetHotelGroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelNames As New List(Of String)
        Try

            strSqlQry = "select rtrim(ltrim(hotelgroupname))+'(HG)' name from hotelgroup where  hotelgroupname like  " & "'" & prefixText & "%'  order by name"
            'and grouptype<>'C'
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotelNames.Add(myDS.Tables(0).Rows(i)("name").ToString())
                Next

            End If

            Return hotelNames
        Catch ex As Exception
            Return hotelNames
        End Try

    End Function


    ''' <summary>
    ''' btnNew_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' btnCancel.Attributes.Add("class", "btnExampleHold")
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False

        hdOPMode.Value = "N"
        hdFillByGrid.Value = "N"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        lblHeading.Text = "Apply Markup"

        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnNew.BackColor = col
        btnNew.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnEdit.ForeColor = col
        btnDelete.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col
        btnSave.Visible = True

    End Sub

    ''' <summary>
    ''' btnEdit_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True

        hdOPMode.Value = "E"

        btnSave.Text = "Update"
        lblHeading.Text = "Edit Applied markup"


        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnEdit.BackColor = col
        btnEdit.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnDelete.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col

        btnSave.Visible = True

    End Sub

    ''' <summary>
    ''' btnDelete_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        hdOPMode.Value = "D"

        btnSave.Text = "Delete"
        lblHeading.Text = "Delete Applied Markup"



        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnDelete.BackColor = col
        btnDelete.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col


        dlList.Visible = False

        btnSave.Visible = True

    End Sub

    ''' <summary>
    ''' btnCancel_new_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancel_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel_new.Click

        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        lblHeading.Text = "Apply Markup"
        btnSave.Visible = True

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True


        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnCancel_new.BackColor = col
        btnCancel_new.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnView.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnView.ForeColor = col
        btnDelete.ForeColor = col


        dlList.Visible = True


        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")
        dtt.Rows.Clear()
        dlList.DataSource = dtt
        dlList.DataBind()
        Session("sDtDynamicSearch") = dtt
        ' FillGridNew()
        btnSave.Visible = False
        chkActive.Checked = False
        ClearFields()
        DisableControl()
        hdOPMode.Value = "V"
        ucMarkup.sbSetPageState("", "ApplyMarkup", CType(Session("ApplyMarkupState"), String))
        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        ucMarkup.sbShowCountry()


    End Sub

    ''' <summary>
    ''' ClearFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearFields()
        hdOPMode.Value = "V"

        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        btnSave.Text = "Save"

        txtmarkupCode.Text = ""
        txtMarkupFormula.Text = ""
        txtMarkupFormulaName.Text = ""
        txtNameNew.Text = ""
        txtApplicableTo.Text = ""

        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
        dtsHotelGroupDetails.Rows.Clear()
        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
        gv_SearchResult.DataSource = dtsHotelGroupDetails
        gv_SearchResult.DataBind()


        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtHotelDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtHotelDynamic") = dtDynamic
        dlList1.DataSource = dtDynamic
        dlList1.DataBind()

        sbGClearExhibitionAndHolidayGridColumns()
        FillDaysOfWeek()
        FillInventoryType()
        'txtvsprocesssplit.Text = ""
        'ucMarkup.fnbtnVsProcess(txtvsprocesssplit, dlList)
        'Session("isAutoTick_wuccountrygroupusercontrol") = 1
        'ucMarkup.sbShowCountry()
        Session("ApplyMarkupState") = "New"
        ucMarkup.clearsessions()
        chkActive.Checked = False

    End Sub

    ''' <summary>
    ''' btnView_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        lblHeading.Text = "View Apply Markup"


        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnView.BackColor = col
        btnView.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnNew.BackColor = col1
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnNew.ForeColor = col
        btnEdit.ForeColor = col
        btnCancel_new.ForeColor = col
        btnDelete.ForeColor = col

        ClearFields()

        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtHotelDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtHotelDynamic") = dtDynamic
        dlList1.DataSource = dtDynamic
        dlList1.DataBind()
        Session("ApplyMarkupState") = "View"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False

        hdOPMode.Value = "V"
        EnableControls()
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True

        btnSave.Visible = False
        btnCheckExist.Visible = False
        chkActive.Enabled = False

        btnSelectMarkup.Visible = False

        txtMarkupFormulaName.Visible = False
        txtMarkupFormula.Visible = False
        txtApplicableTo.Visible = False
        Label15.Visible = False
        Label16.Visible = False
        Label6.Visible = False
        chkActive.Visible = False


    End Sub
    Private Sub FilterGridSEARCH()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit2.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAgent As String = ""
        Dim lsProcessHotel As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "FORMULA-ID"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("FORMULA-ID", lsProcessCity, "F")
                Case "INVENTORYTYPE"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("INVENTORYTYPE", lsProcessCity, "I")
                Case "ROOMCLASSIFICATION"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("ROOMCLASSIFICATION", lsProcessCity, "R")
                Case "HOTEL"
                    lsProcessHotel = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("HOTEL", lsProcessHotel, "H")
                Case "AGENT"
                    lsProcessAgent = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("AGENT", lsProcessAgent, "A")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("COUNTRY", lsProcessCountry, "C")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable2("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")
        dlList2.DataSource = dtt
        dlList2.DataBind()
        bindsearchgrid(False)
        ' FillGridNewsearch() 'Bind Gird based selection 

    End Sub

    Function sbAddToDataTable2(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        ' FilterGrid()
        If hdOPMode.Value = "D" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select new or edit option for selection.');", True)
            Exit Sub
        Else
            ucMarkup.fnbtnVsProcess(txtvsprocesssplit, dlList)
        End If



    End Sub


    Private Sub FillGridNew()

        Dim dtt As DataTable
        dtt = Session("sDtDynamicSearch")

        Dim strCountryValue As String = ""
        Dim strSectorValue As String = ""
        Dim strCityValue As String = ""
        Dim strHotelGroupValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strHotelstatusValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strHotelchainValue As String = ""


        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "HOTELNAME" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If

                If dtt.Rows(i)("Code").ToString = "COUNTRYGROUP" Then
                    If strCountryGroupValue <> "" Then
                        strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                    If strSectorValue <> "" Then
                        strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CITY" Then
                    If strCityValue <> "" Then
                        strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                    If strCategoryValue <> "" Then
                        strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTELGROUP" Then
                    If strHotelGroupValue <> "" Then
                        strHotelGroupValue = strHotelGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTELSTATUS" Then
                    If strHotelstatusValue <> "" Then
                        strHotelstatusValue = strHotelstatusValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelstatusValue = "'" + dtt.Rows(i)("Value").ToString + "'"

                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "HOTELCHAIN" Then
                    If strHotelchainValue <> "" Then
                        strHotelchainValue = strHotelchainValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strHotelchainValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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



        Dim myDS As New DataSet
        Dim strorderby As String = Session("strsortexpression")
        Dim strsortorder As String = "ASC"
        Dim strWhereCond As String = ""

        strSqlQry = ""
        Try

            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1 FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode  where partymast.sptypecode='HOT'"
            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"

                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue) & ")"
                End If
            End If

            'If strCountryGroupValue <> "" Then
            '    If Trim(strWhereCond) = "" Then
            '        strWhereCond = "  ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strCountryGroupValue) & ")"
            '    Else
            '        strWhereCond = strWhereCond & " and ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and countrygroupname=" & Trim(strCountryGroupValue) & ")"
            '    End If
            'End If

            If strSectorValue <> "" Then

                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectormaster.sectorname) IN( " & Trim(strSectorValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue) & ")"
                End If
            End If

            If strCityValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue) & ")"
                End If
            End If

            If strCategoryValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(catmast.catname) IN (" & Trim(strCategoryValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(catmast.catname) IN (" & Trim(strCategoryValue) & ")"
                End If
            End If

            If strHotelstatusValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelstatus.hotelstatusname) IN ( " & Trim(strHotelstatusValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelstatus.hotelstatusname) IN (" & Trim(strHotelstatusValue) & ")"
                End If
            End If
            If strHotelchainValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelchainmaster.hotelchainname) IN ( " & Trim(strHotelchainValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelchainmaster.hotelchainname) IN (" & Trim(strHotelchainValue) & ")"
                End If
            End If

            If strHotelGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(strHotelGroupValue) & "))"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(strHotelGroupValue) & "))"
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
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or   ctrymast.ctrycode in (select cgd.ctrycode   from countrygroup cg,countrygroup_detail cgd where cg.countrygroupcode = cgd.countrygroupcode and upper(countrygroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                End If

            End If



            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            'gvSearchGrid.DataBind()
            'gvSearchGrid.DataSource = myDS

            'If myDS.Tables(0).Rows.Count > 0 Then
            '    gvSearchGrid.DataBind()
            'Else
            '    gvSearchGrid.PageIndex = 0
            '    gvSearchGrid.DataBind()
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Records not found, Please redefine search criteria."
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try

    End Sub

    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit1.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessCity As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessCountryGroup As String = ""
        Dim lsProcessHotelGroup As String = ""
        Dim lsProcessCategory As String = ""
        Dim lsProcessHotels As String = ""
        Dim lsMainArr As String()
        Dim IsProcessTypes As String = ""
        Dim IsProcessValue As String = ""
        Dim IsProcessValues As String = ""
        Dim lsProcessChain As String = ""
        Dim lsProcessStatus As String = ""
        Dim lsRoomClassification As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")

        If hdOPMode.Value = "D" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select new or edit option for selection.');", True)
            Exit Sub
        ElseIf hdOPMode.Value = "N" Or hdOPMode.Value = "E" Or hdOPMode.Value = "V" Then
            'If txtGroupTagName.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
            '    txtGroupTagName.Focus()
            '    Exit Sub
            'End If
            hdFillByGrid.Value = "N"
            hdLinkButtonValue.Value = ""
            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtHotelDynamic")

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1) & "(CT)"
                        txtNameNew.Text = lsProcessCity
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCity Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CT", lsProcessCity)

                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt

                    Case "COUNTRYGROUP"
                        lsProcessCountryGroup = lsMainArr(i).Split(":")(1) & "(G)"
                        txtNameNew.Text = lsProcessCountryGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountryGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("G", lsProcessCountryGroup)
                            End If

                        End If

                        Session("sDtHotelDynamic") = dtt
                    Case "HOTELSTATUS"
                        lsProcessStatus = lsMainArr(i).Split(":")(1) & "(HS)"
                        txtNameNew.Text = lsProcessStatus

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessStatus Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HS", lsProcessStatus)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                    Case "HOTELCHAIN"
                        lsProcessChain = lsMainArr(i).Split(":")(1) & "(HC)"
                        txtNameNew.Text = lsProcessChain

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessChain Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HC", lsProcessChain)
                            End If

                        End If

                        Session("sDtHotelDynamic") = dtt
                        ' FillGridByType("CG", lsProcessCountryGroup)
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessText Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("T", lsProcessText)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                        'FillGridByType("T", lsProcessText)
                    Case "SECTOR"
                        lsProcessSector = lsMainArr(i).Split(":")(1) & "(S)"
                        txtNameNew.Text = lsProcessSector

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessSector Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("S", lsProcessSector)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                        ' FillGridByType("S", lsProcessSector)
                    Case "CATEGORY"
                        lsProcessCategory = lsMainArr(i).Split(":")(1) & "(CTG)"
                        txtNameNew.Text = lsProcessCategory
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCategory Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CTG", lsProcessCategory)
                            End If
                        End If
                        'If IsProcessType = "" Then
                        '    IsProcessType = "CTG"
                        'Else
                        '    IsProcessType = IsProcessType + "," + "CTG"
                        'End If
                        'If IsProcessValue = "" Then
                        '    IsProcessValue = lsProcessCategory
                        'Else
                        '    IsProcessValue = IsProcessValue + "," + lsProcessCategory
                        'End If
                        Session("sDtHotelDynamic") = dtt
                        ' FillGridByType("CTG", lsProcessCategory)
                    Case "HOTELGROUP"
                        lsProcessHotelGroup = lsMainArr(i).Split(":")(1) & "(HG)"
                        txtNameNew.Text = lsProcessHotelGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessHotelGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("HG", lsProcessHotelGroup)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                        'FillGridByType("HG", lsProcessHotelGroup)

                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(C)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("C", lsProcessCountry)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                    Case "HOTELNAME"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(H)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("H", lsProcessCountry)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt

                        '  FillGridByType("C", lsProcessCountry)
                    Case "HOTELS"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = "Hotels" Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("H", "Hotels")
                            End If
                        End If
                        Session("sDtHotelDynamic") = dtt
                        'FillGridByType("H", lsProcessCountry)
                    Case "ROOM CLASSIFICATION"
                        lsRoomClassification = lsMainArr(i).Split(":")(1) & "(RC)"
                        txtNameNew.Text = lsRoomClassification

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Country").ToString = lsRoomClassification Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("RC", lsRoomClassification)
                            End If

                        End If
                        Session("sDtHotelDynamic") = dtt
                End Select
            Next



            'Dim oldprocessvalue As String = ""
            'For i = 0 To dtt.Rows.Count - 1
            '    If IsProcessType <> "" Then
            '        If oldprocessvalue <> dtt.Rows(i).Item("Code") Then
            '            IsProcessType = IsProcessType + ":" + dtt.Rows(i).Item("Code")
            '            IsProcessValue = IsProcessValue + ":" + "'" + dtt.Rows(i).Item("Country") + "'"


            '        Else
            '            IsProcessType = IsProcessType
            '            IsProcessValue = IsProcessValue + "," + "'" + dtt.Rows(i).Item("Country") + "'"
            '        End If
            '    Else
            '        IsProcessType = dtt.Rows(i).Item("Code")
            '        IsProcessValue = "'" + dtt.Rows(i).Item("Country") + "'"
            '    End If
            '    oldprocessvalue = IsProcessType

            '    'If IsProcessValue <> "" Then

            '    'Else
            '    '    IsProcessValue = dtt.Rows(i).Item("Country")
            '    'End If
            'Next

            Dim arCode As New ArrayList
            For k = 0 To dtt.Rows.Count - 1
                If Not arCode.Contains(dtt.Rows(k).Item("Code")) Then
                    arCode.Add(dtt.Rows(k).Item("Code"))
                End If
            Next
            If arCode.Count > 0 Then
                For j = 0 To arCode.Count - 1
                    If IsProcessTypes <> "" Then
                        IsProcessTypes = IsProcessTypes & ":" & arCode(j).ToString
                    Else
                        IsProcessTypes = arCode(j).ToString
                    End If

                    For l = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(l)("Country").ToString.Contains("(" & arCode(j).ToString.Trim & ")") Then
                            If IsProcessValue <> "" Then
                                IsProcessValue = IsProcessValue & ":'" & dtt.Rows(l)("Country").ToString & "'" '*** Danny 09/06/2018
                            Else
                                IsProcessValue = "'" & dtt.Rows(l)("Country").ToString & "'"
                            End If

                        End If
                    Next
                    If IsProcessValues <> "" Then
                        IsProcessValues = IsProcessValues & ":" & IsProcessValue
                    Else
                        IsProcessValues = IsProcessValue
                    End If
                    IsProcessValue = ""
                Next
            End If
            FillGridByType(IsProcessTypes, IsProcessValues)
            If dtt.Rows.Count >= 0 Then
                Dim dtsHotelGroupDetails As DataTable
                dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
                If dtsHotelGroupDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
                        Session("sDtHotelDynamic") = dtt
                    End If
                End If
            End If
            dlList1.DataSource = dtt
            dlList1.DataBind()

        ElseIf hdOPMode.Value = "D" Then
            'If txtGroupTagName.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
            '    txtGroupTagName.Focus()
            '    Exit Sub
            'End If
        End If

    End Sub

    ''' <summary>
    ''' btnResetSelection_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click



        If hdOPMode.Value = "V" Then
            Dim dtt As DataTable
            dtt = Session("sDtDynamicSearch")
            dtt.Rows.Clear()
            dlList.DataSource = dtt
            dlList.DataBind()
            Session("sDtDynamicSearch") = dtt
            ' FillGridNew()
        Else
            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
            dtsHotelGroupDetails.Rows.Clear()
            Session("sDtHotelGroupDetails") = dtsHotelGroupDetails



            Dim dtDynamic As DataTable
            dtDynamic = Session("sDtHotelDynamic")
            dtDynamic.Rows.Clear()
            Session("sDtHotelDynamic") = dtDynamic

            txtvsprocesssplit.Text = ""
            ucMarkup.fnbtnVsProcess(txtvsprocesssplit, dlList)

        End If



    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCloseSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            ucMarkup.fnCloseButtonClick(sender, e, dlList)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        'Try
        '    Dim myButton As Button = CType(sender, Button)
        '    Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
        '    Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
        '    Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

        '    Dim dtDynamics As New DataTable
        '    dtDynamics = Session("sDtDynamicSearch")
        '    If dtDynamics.Rows.Count > 0 Then
        '        Dim j As Integer
        '        For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
        '            If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
        '                dtDynamics.Rows.Remove(dtDynamics.Rows(j))
        '            End If
        '        Next
        '    End If
        '    Session("sDtDynamicSearch") = dtDynamics
        '    dlListSearch.DataSource = dtDynamics
        '    dlListSearch.DataBind()
        '    FillGridNew()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        'End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Edit")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "View")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""

        If CType(Request.QueryString("appid"), String) = "2" Or CType(Request.QueryString("appid"), String) = "3" Or CType(Request.QueryString("appid"), String) = "9" Then
            strpop = "window.open('SupMain.aspx?appid=" + CType("1", String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        Else
            strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Delete")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim myButton As LinkButton = CType(sender, LinkButton)
        Dim row As GridViewRow = CType((CType(sender, LinkButton)).NamingContainer, GridViewRow)
        Dim lblId As Label = CType(row.FindControl("lblHotelCode"), Label)
        Session.Add("SupState", "Copy")
        Session.Add("SupRefCode", CType(lblId.Text.Trim, String))
        Dim strpop As String = ""
        strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=Copy&RefCode=" + CType(lblId.Text.Trim, String) + "','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAddHotel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHotel.Click
        Session.Add("SupState", "New")
        Dim strpop As String = ""
        strpop = "window.open('SupMain.aspx?appid=" + CType(Request.QueryString("appid"), String) + "&State=New','Suppliers');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnHotelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHotelReport.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Supplier&BackPageName=SupplierSearch.aspx&SupCode=&SupName=&SuptypeCode=[Select]&SuptypeName=[Select]&SupCatCode=[Select]&SupcatName=[Select]&SellcatCode=[Select]&SellcatName=[Select]&CtryCode=[Select]&CtryName=[Select]&SectCode=[Select]&SectName=[Select]&CityCode=[Select]&CityName=[Select]&CtrlaccCode=[Select]','RepSupplier');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnAddRowGVs_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            sbGenerateExhibitionAndHolidayGridColumns()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Sub sbGenerateExhibitionAndHolidayGridColumns()



        Dim dtExhibitionAndHoliday As New DataTable
        Dim drExhibitionAndHoliday As DataRow
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("FromDate", GetType(String)))
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("ToDate", GetType(String)))
        drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()

        If gvExhibitionAndHolidayInput.Rows.Count > 0 Then
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows

                txtFrom = row.FindControl("txtExhibitionAndHolidayfromDate")
                txtTo = row.FindControl("txtExhibitionAndHolidayToDate")
                drExhibitionAndHoliday("FromDate") = txtFrom.Text
                drExhibitionAndHoliday("ToDate") = txtTo.Text
                dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
                drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
            Next
        End If

        drExhibitionAndHoliday("FromDate") = ""
        drExhibitionAndHoliday("ToDate") = ""
        dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
        gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
        gvExhibitionAndHolidayInput.DataBind()
        Dim txtfromDate As TextBox
        txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
        txtfromDate.Focus()


        Dim gridNewrow As GridViewRow
        gridNewrow = gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1)
        Dim strRowId As String = gridNewrow.ClientID
        Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(gvExhibitionAndHolidayInput.Rows.Count - 1, String) + "');")
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

    End Sub

    Sub sbGClearExhibitionAndHolidayGridColumns()

        Dim dtExhibitionAndHoliday As New DataTable
        Dim drExhibitionAndHoliday As DataRow
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("FromDate", GetType(String)))
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("ToDate", GetType(String)))
        drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()

        drExhibitionAndHoliday("FromDate") = ""
        drExhibitionAndHoliday("ToDate") = ""
        dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
        gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
        gvExhibitionAndHolidayInput.DataBind()
        'Dim txtfromDate As TextBox
        'txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
        'txtfromDate.Focus()


        'Dim gridNewrow As GridViewRow
        'gridNewrow = gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1)
        'Dim strRowId As String = gridNewrow.ClientID
        'Dim str As String = String.Format("javascript:LastSelectRow('" + strRowId + "','" + CType(gvExhibitionAndHolidayInput.Rows.Count - 1, String) + "');")
        'ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script", str, True)

    End Sub

    Protected Sub imgUpdateToNextGrid_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub btnDeleteRow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteRow.Click
        sbDeleteExhibitionAndHolidayGridColumns()
    End Sub

    Private Sub sbDeleteExhibitionAndHolidayGridColumns()

        'If gvExhibitionAndHolidayInput.Rows.Count > 0 Then
        '    Dim chkSelect As CheckBox
        '    For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
        '        chkSelect = row.FindControl("chkSelect")
        '        If chkSelect.Checked = True Then
        '            gvExhibitionAndHolidayInput.DeleteRow(row.RowIndex)
        '        End If
        '    Next
        'End If


        Dim dtExhibitionAndHoliday As New DataTable
        Dim drExhibitionAndHoliday As DataRow
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("FromDate", GetType(String)))
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("ToDate", GetType(String)))
        ' drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()

        If gvExhibitionAndHolidayInput.Rows.Count > 0 Then
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim chkSelect As CheckBox
            For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
                txtFrom = row.FindControl("txtExhibitionAndHolidayfromDate")
                txtTo = row.FindControl("txtExhibitionAndHolidayToDate")
                chkSelect = row.FindControl("chkSelect")
                If chkSelect.Checked = False Then
                    drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
                    drExhibitionAndHoliday("FromDate") = txtFrom.Text
                    drExhibitionAndHoliday("ToDate") = txtTo.Text
                    dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)

                End If

            Next
        End If

        'drExhibitionAndHoliday("FromDate") = ""
        'drExhibitionAndHoliday("ToDate") = ""
        'dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
        gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
        gvExhibitionAndHolidayInput.DataBind()
        If gvExhibitionAndHolidayInput.Rows.Count <= 0 Then
            drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
            drExhibitionAndHoliday("FromDate") = ""
            drExhibitionAndHoliday("ToDate") = ""
            dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
            gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
            gvExhibitionAndHolidayInput.DataBind()
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        Else
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        End If

    End Sub

    Protected Sub btnExhibition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExhibition.Click
        Try
            hdEx.Value = "Y"
            Dim dt As New DataTable
            Dim strSqlQry As String = "select m.exhibitioncode,m.exhibitionname,convert(varchar(10),fromdate,103)fromdate,convert(varchar(10),todate,103)todate from exhibition_master m,exhibition_detail d where m.exhibitioncode=d.exhibitioncode and active=1 and fromdate>=convert(datetime,convert(varchar(10),GETDATE(),101),101) and todate>=convert(datetime,convert(varchar(10),GETDATE(),101),101) order by convert(datetime,fromdate)"


            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)

            gvExhibition.DataBind()
            gvExhibition.DataSource = dt

            If dt.Rows.Count > 0 Then
                gvExhibition.DataBind()
            Else
                gvExhibition.PageIndex = 0
                gvExhibition.DataBind()
            End If
            ModalExhibitionPopup.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub chkSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim chkSelectAll As CheckBox = CType(gvExhibition.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        Dim strFlag As String = ""
        For Each gvRow In gvExhibition.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkSelect"), CheckBox)
            If chkSelect1.Checked = False Then
                chkSelectAll.Checked = False
                Exit For
            End If
        Next
        For Each gvRows In gvExhibition.Rows
            Dim chkSelect2 As CheckBox = CType(gvRows.FindControl("chkSelect"), CheckBox)
            If chkSelect2.Checked = True Then
                strFlag = "1"
            Else
                strFlag = "0"
                Exit For
            End If
        Next
        If strFlag = "1" Then
            chkSelectAll.Checked = True
        Else
            chkSelectAll.Checked = False
        End If
        If hdEx.Value = "Y" Then
            ModalExhibitionPopup.Show()
        Else
            ModalExhibitionPopup.Hide()
        End If
    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelectAll As CheckBox = CType(sender, CheckBox)

        For Each gvRow In gvExhibition.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkSelect"), CheckBox)
            If chkSelectAll.Checked = True Then
                chkSelect.Checked = True
            Else
                chkSelect.Checked = False
            End If
        Next
        If hdEx.Value = "Y" Then
            ModalExhibitionPopup.Show()
        Else
            ModalExhibitionPopup.Hide()
        End If
    End Sub

    Protected Sub btnFillExhibitions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillExhibitions.Click
        Dim dtExhibitionAndHoliday As New DataTable
        Dim drExhibitionAndHoliday As DataRow
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("FromDate", GetType(String)))
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("ToDate", GetType(String)))
        ' drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()

        If gvExhibitionAndHolidayInput.Rows.Count > 0 Then
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim chkSelect As CheckBox
            For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
                txtFrom = row.FindControl("txtExhibitionAndHolidayfromDate")
                txtTo = row.FindControl("txtExhibitionAndHolidayToDate")
                chkSelect = row.FindControl("chkSelect")
                If chkSelect.Checked = False Then
                    If txtFrom.Text <> "" And txtTo.Text <> "" Then
                        drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
                        drExhibitionAndHoliday("FromDate") = txtFrom.Text
                        drExhibitionAndHoliday("ToDate") = txtTo.Text
                        dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
                    End If
                End If

            Next
        End If

        For Each gvRow In gvExhibition.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkSelect"), CheckBox)
            Dim lblFromDate As Label = CType(gvRow.FindControl("lblFromDate"), Label)
            Dim lblToDate As Label = CType(gvRow.FindControl("lblToDate"), Label)
            If chkSelect.Checked = True Then
                drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
                ' dtExhibitionAndHoliday.Columns("FromDate").Container(lblFromDate.Text)
                drExhibitionAndHoliday("FromDate") = lblFromDate.Text
                drExhibitionAndHoliday("ToDate") = lblToDate.Text
                dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
            End If
        Next
        gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
        gvExhibitionAndHolidayInput.DataBind()
        If gvExhibitionAndHolidayInput.Rows.Count <= 0 Then
            drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
            drExhibitionAndHoliday("FromDate") = ""
            drExhibitionAndHoliday("ToDate") = ""
            dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
            gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
            gvExhibitionAndHolidayInput.DataBind()
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        Else
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        End If

    End Sub

    Protected Sub chkHSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim chkSelectAll As CheckBox = CType(gvHoliday.HeaderRow.FindControl("chkHSelectAll"), CheckBox)
        Dim strFlag As String = ""
        For Each gvRow In gvHoliday.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkHSelect"), CheckBox)
            If chkSelect1.Checked = False Then
                chkSelectAll.Checked = False
                Exit For
            End If
        Next
        For Each gvRows In gvHoliday.Rows
            Dim chkSelect2 As CheckBox = CType(gvRows.FindControl("chkHSelect"), CheckBox)
            If chkSelect2.Checked = True Then
                strFlag = "1"
            Else
                strFlag = "0"
                Exit For
            End If
        Next
        If strFlag = "1" Then
            chkSelectAll.Checked = True
        Else
            chkSelectAll.Checked = False
        End If
        If hdHoliday.Value = "Y" Then
            ModalHolidayPopup.Show()
        Else
            ModalHolidayPopup.Hide()
        End If
    End Sub

    Protected Sub chkHSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelectAll As CheckBox = CType(sender, CheckBox)
        For Each gvRow In gvHoliday.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkHSelect"), CheckBox)
            If chkSelectAll.Checked = True Then
                chkSelect.Checked = True
            Else
                chkSelect.Checked = False
            End If
        Next
        If hdHoliday.Value = "Y" Then
            ModalHolidayPopup.Show()
        Else
            ModalHolidayPopup.Hide()
        End If
    End Sub

    Protected Sub btnHFillDates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHFillDates.Click
        Dim dtExhibitionAndHoliday As New DataTable
        Dim drExhibitionAndHoliday As DataRow
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("FromDate", GetType(String)))
        dtExhibitionAndHoliday.Columns.Add(New DataColumn("ToDate", GetType(String)))
        ' drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()

        If gvExhibitionAndHolidayInput.Rows.Count > 0 Then
            Dim txtFrom As TextBox
            Dim txtTo As TextBox
            Dim chkSelect As CheckBox
            For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
                txtFrom = row.FindControl("txtExhibitionAndHolidayfromDate")
                txtTo = row.FindControl("txtExhibitionAndHolidayToDate")
                chkSelect = row.FindControl("chkSelect")
                If chkSelect.Checked = False Then
                    If txtFrom.Text <> "" And txtTo.Text <> "" Then
                        drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
                        drExhibitionAndHoliday("FromDate") = txtFrom.Text
                        drExhibitionAndHoliday("ToDate") = txtTo.Text
                        dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
                    End If
                End If

            Next
        End If

        For Each gvRow In gvHoliday.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkHSelect"), CheckBox)
            Dim lblFromDate As Label = CType(gvRow.FindControl("lblFromDate"), Label)
            Dim lblToDate As Label = CType(gvRow.FindControl("lblToDate"), Label)
            If chkSelect.Checked = True Then
                drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
                ' dtExhibitionAndHoliday.Columns("FromDate").Container(lblFromDate.Text)
                drExhibitionAndHoliday("FromDate") = lblFromDate.Text
                drExhibitionAndHoliday("ToDate") = lblToDate.Text
                dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
            End If
        Next
        gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
        gvExhibitionAndHolidayInput.DataBind()
        If gvExhibitionAndHolidayInput.Rows.Count <= 0 Then
            drExhibitionAndHoliday = dtExhibitionAndHoliday.NewRow()
            drExhibitionAndHoliday("FromDate") = ""
            drExhibitionAndHoliday("ToDate") = ""
            dtExhibitionAndHoliday.Rows.Add(drExhibitionAndHoliday)
            gvExhibitionAndHolidayInput.DataSource = dtExhibitionAndHoliday
            gvExhibitionAndHolidayInput.DataBind()
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        Else
            Dim txtfromDate As TextBox
            txtfromDate = TryCast(gvExhibitionAndHolidayInput.Rows(gvExhibitionAndHolidayInput.Rows.Count - 1).FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            txtfromDate.Focus()
        End If
    End Sub

    Protected Sub btnHoliday_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHoliday.Click
        Try
            Dim strSelectedCounties As String = ucMarkup.fnGetSelectedCountriesList()
            If strSelectedCounties = "" Then
                strSelectedCounties = "''"
            End If
            hdHoliday.Value = "Y"
            Dim dt As New DataTable
            Dim strSqlQry As String = "select m.holidaycode,m.holidayname,convert(varchar(10),fromdate,103)fromdate,convert(varchar(10),todate,103)todate from holidaycalendar m,holidaycalendar_detail d where m.holidaycode=d.holidaycode and active=1 and fromdate>=convert(datetime,convert(varchar(10),GETDATE(),101),101) and todate>=convert(datetime,convert(varchar(10),GETDATE(),101),101) and d.ctrycode in (" + strSelectedCounties + ")  order by convert(datetime,fromdate)"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)

            gvHoliday.DataBind()
            gvHoliday.DataSource = dt

            If dt.Rows.Count > 0 Then
                gvHoliday.DataBind()
            Else
                gvHoliday.PageIndex = 0
                gvHoliday.DataBind()
            End If
            ModalHolidayPopup.Show()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub FillDaysOfWeek()
        Dim dtDaysOfWeek As New DataTable
        Dim drDaysOfWeek As DataRow
        dtDaysOfWeek.Columns.Add(New DataColumn("DaysOfWeek", GetType(String)))
        dtDaysOfWeek.Columns.Add(New DataColumn("Code", GetType(String)))
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Sunday"
        drDaysOfWeek("Code") = "SUN"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Monday"
        drDaysOfWeek("Code") = "MON"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Tuesday"
        drDaysOfWeek("Code") = "TUE"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Wednsday"
        drDaysOfWeek("Code") = "WED"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Thursday"
        drDaysOfWeek("Code") = "THU"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Friday"
        drDaysOfWeek("Code") = "FRI"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        drDaysOfWeek = dtDaysOfWeek.NewRow()
        drDaysOfWeek("DaysOfWeek") = "Saturday"
        drDaysOfWeek("Code") = "SAT"
        dtDaysOfWeek.Rows.Add(drDaysOfWeek)
        gvWeekOfDays.DataSource = dtDaysOfWeek
        gvWeekOfDays.DataBind()
    End Sub

    Protected Sub chkInvSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim chkSelectAll As CheckBox = CType(gvInventoryType.HeaderRow.FindControl("chkInvSelectAll"), CheckBox)
        Dim strFlag As String = ""
        For Each gvRow In gvInventoryType.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
            If chkSelect1.Checked = False Then
                chkSelectAll.Checked = False
                Exit For
            End If
        Next
        For Each gvRows In gvInventoryType.Rows
            Dim chkSelect2 As CheckBox = CType(gvRows.FindControl("chkInvSelect"), CheckBox)
            If chkSelect2.Checked = True Then
                strFlag = "1"
            Else
                strFlag = "0"
                Exit For
            End If
        Next
        If strFlag = "1" Then
            chkSelectAll.Checked = True
        Else
            chkSelectAll.Checked = False
        End If

        Dim gvInvRow As GridViewRow = CType(chkSelect.NamingContainer, GridViewRow)
        Dim lblInventoryTypes As Label = CType(gvInvRow.FindControl("lblInventoryType"), Label)
        If lblInventoryTypes.Text = "Free Sale" Then
            For Each gvRow In gvInventoryType.Rows
                Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
                Dim lblInventoryType As Label = CType(gvRow.FindControl("lblInventoryType"), Label)
                If lblInventoryType.Text = "General" Then
                    If chkSelect.Checked = True Then
                        chkSelect1.Checked = True
                    Else
                        chkSelect1.Checked = False
                    End If

                End If

            Next
        End If
        If lblInventoryTypes.Text = "General" Then
            For Each gvRow In gvInventoryType.Rows
                Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
                Dim lblInventoryType As Label = CType(gvRow.FindControl("lblInventoryType"), Label)
                If lblInventoryType.Text = "Free Sale" Then
                    If chkSelect.Checked = True Then
                        chkSelect1.Checked = True
                    Else
                        chkSelect1.Checked = False
                    End If
                End If
            Next
        End If

        'lblInventoryType
    End Sub

    Protected Sub chkInvSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelectAll As CheckBox = CType(sender, CheckBox)
        For Each gvRow In gvInventoryType.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
            If chkSelectAll.Checked = True Then
                chkSelect.Checked = True
            Else
                chkSelect.Checked = False
            End If
        Next



    End Sub

    Private Sub FillInventoryType()
        Dim dtInventoryType As New DataTable
        Dim drInventoryType As DataRow
        dtInventoryType.Columns.Add(New DataColumn("InventoryType", GetType(String)))
        drInventoryType = dtInventoryType.NewRow()
        drInventoryType("InventoryType") = "General"
        dtInventoryType.Rows.Add(drInventoryType)
        drInventoryType = dtInventoryType.NewRow()
        drInventoryType("InventoryType") = "B2B"
        dtInventoryType.Rows.Add(drInventoryType)
        drInventoryType = dtInventoryType.NewRow()
        drInventoryType("InventoryType") = "Financial"
        dtInventoryType.Rows.Add(drInventoryType)
        drInventoryType = dtInventoryType.NewRow()
        drInventoryType("InventoryType") = "Free Sale"
        dtInventoryType.Rows.Add(drInventoryType)
        drInventoryType = dtInventoryType.NewRow()
        drInventoryType("InventoryType") = "Dummy Booking"
        dtInventoryType.Rows.Add(drInventoryType)

        gvInventoryType.DataSource = dtInventoryType
        gvInventoryType.DataBind()
    End Sub

    Protected Sub chkWSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        Dim chkSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkWSelectAll"), CheckBox)
        Dim strFlag As String = ""
        For Each gvRow In gvWeekOfDays.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkWSelect"), CheckBox)
            If chkSelect1.Checked = False Then
                chkSelectAll.Checked = False
                Exit For
            End If
        Next
        For Each gvRows In gvWeekOfDays.Rows
            Dim chkSelect2 As CheckBox = CType(gvRows.FindControl("chkWSelect"), CheckBox)
            If chkSelect2.Checked = True Then
                strFlag = "1"
            Else
                strFlag = "0"
                Exit For
            End If
        Next
        If strFlag = "1" Then
            chkSelectAll.Checked = True
        Else
            chkSelectAll.Checked = False
        End If
    End Sub

    Protected Sub chkWSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelectAll As CheckBox = CType(sender, CheckBox)
        For Each gvRow In gvWeekOfDays.Rows
            Dim chkSelect As CheckBox = CType(gvRow.FindControl("chkWSelect"), CheckBox)
            If chkSelectAll.Checked = True Then
                chkSelect.Checked = True
            Else
                chkSelect.Checked = False
            End If
        Next
    End Sub

    Protected Sub chkmarkupSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        For Each gvRow In gvMarkupFormulas.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkmarkupSelect"), CheckBox)
            If Not chkSelect1.ClientID = chkSelect.ClientID Then
                chkSelect1.Checked = False
            End If
        Next
        If hdMarkUp.Value = "Y" Then
            ModalSelectMarkup.Show()
        Else
            ModalSelectMarkup.Hide()
        End If
    End Sub

    Protected Sub btnSelectMarkup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectMarkup.Click
        hdMarkUp.Value = "Y"
        FillMarkups()
        ModalSelectMarkup.Show()
    End Sub

    Private Sub FillMarkups()
        Try


            Dim dt As New DataTable
            'Dim strSqlQry As String = " SELECT formulaid,formulaname,MarkupFormula,currname from V_GET_MARKUP_FORMULA where Active=1 and FormulaType='Hotel'"
            Dim strSqlQry As String = "select formulaid,formulaname,MarkupFormulaString MarkupFormula,(select currmast.currname from currmast where currmast.currcode=markupformula_header.currcode)currname from markupformula_header(nolock) where Active=1 and FormulaType='Hotel'  ORDER BY FormulaID ASC" '*** Danny 13/05/2018
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)

            gvMarkupFormulas.DataBind()
            gvMarkupFormulas.DataSource = dt

            If dt.Rows.Count > 0 Then
                gvMarkupFormulas.DataBind()

            Else
                gvMarkupFormulas.PageIndex = 0
                gvMarkupFormulas.DataBind()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnSelectedMarkup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectedMarkup.Click
        txtMarkupFormulaName.Text = ""
        txtMarkupFormula.Text = ""
        For Each gvRow In gvMarkupFormulas.Rows
            Dim chkSelect1 As CheckBox = CType(gvRow.FindControl("chkmarkupSelect"), CheckBox)
            Dim lblFormulaName As Label
            Dim lblFormula As Label
            Dim lblCode As Label
            If chkSelect1.Checked = True Then
                lblFormulaName = CType(gvRow.FindControl("lblFormulaName"), Label)
                lblFormula = CType(gvRow.FindControl("lblFormula"), Label)
                lblCode = CType(gvRow.FindControl("lblCode"), Label)
                txtMarkupFormulaName.Text = lblFormulaName.Text
                txtMarkupFormula.Text = lblFormula.Text
                txtmarkupCode.Text = lblCode.Text
            End If
        Next
    End Sub

    Protected Sub btnAddNewMarkup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNewMarkup.Click
        Dim strpop As String = ""
        strpop = "window.open('MarkupFormula.aspx?State=New&page=applymarkup','ApplyMarkupFormula');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim dtsHotelGroupDetails As New DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsHotelGroupDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsHotelGroupDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsHotelGroupDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsHotelGroupDetails.AcceptChanges()
                Next
            End If
            Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtHotelDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Country").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtHotelDynamic") = dtDynamics
            dlList1.DataSource = dtDynamics
            dlList1.DataBind()
            '------------------------------------
            If dtDynamics.Rows.Count > 0 Then

                Dim IsProcessTypes As String = ""
                Dim IsProcessValue As String = ""
                Dim IsProcessValues As String = ""
                Dim arCode As New ArrayList
                For k = 0 To dtDynamics.Rows.Count - 1
                    If Not arCode.Contains(dtDynamics.Rows(k).Item("Code")) Then
                        arCode.Add(dtDynamics.Rows(k).Item("Code"))
                    End If
                Next
                If arCode.Count > 0 Then
                    For j = 0 To arCode.Count - 1
                        If IsProcessTypes <> "" Then
                            IsProcessTypes = IsProcessTypes & ":" & arCode(j).ToString
                        Else
                            IsProcessTypes = arCode(j).ToString
                        End If

                        For l = 0 To dtDynamics.Rows.Count - 1
                            If dtDynamics.Rows(l)("Country").ToString.Contains("(" & arCode(j).ToString.Trim & ")") Then
                                If IsProcessValue <> "" Then

                                    IsProcessValue = IsProcessValue & ":'" & dtDynamics.Rows(l)("Country").ToString & "'" '*** Danny 09/06/2018
                                Else
                                    IsProcessValue = "'" & dtDynamics.Rows(l)("Country").ToString & "'"
                                End If

                            End If
                        Next
                        If IsProcessValues <> "" Then
                            IsProcessValues = IsProcessValues & ":" & IsProcessValue
                        Else
                            IsProcessValues = IsProcessValue
                        End If
                        IsProcessValue = ""
                    Next
                End If
                FillGridByType(IsProcessTypes, IsProcessValues)
                '----------------------------------------
            Else

                '' Create a Dynamic datatable ---- Start
                Dim ClearDataTable = New DataTable()
                Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                Dim dcGroupDetailsCountry = New DataColumn("Country", GetType(String))
                ClearDataTable.Columns.Add(dcGroupDetailsType)
                ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
                ClearDataTable.Columns.Add(dcGroupDetailsCode)
                ClearDataTable.Columns.Add(dcGroupDetailsCountry)
                gv_SearchResult.DataSource = ClearDataTable
                gv_SearchResult.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub lbCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdFillByGrid.Value = "Y"
        hdLinkButtonValue.Value = ""
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "Hotels" Then
                strlbValue = "%"
                If gv_SearchResult.Rows.Count > 0 Then

                    Dim chkSelectAll As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
                    chkSelectAll.Visible = False

                End If
            Else
                If gv_SearchResult.Rows.Count > 0 Then

                    Dim chkSelectAll As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
                    chkSelectAll.Visible = True

                End If
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()

                If Not myButton Is Nothing Then
                    strlbValue = myButton.Text
                    If strlbValue = "Hotels" Then
                        strlbValue = "%"
                        If gv_SearchResult.Rows.Count > 0 Then

                            Dim chkSelectAll As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
                            chkSelectAll.Visible = False

                        End If
                    Else
                        If gv_SearchResult.Rows.Count > 0 Then

                            Dim chkSelectAll As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
                            chkSelectAll.Visible = True


                        End If
                    End If
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("HotelGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If
    End Sub

    Private Sub FillCheckbox()
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsHotelGroupDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSSelect"), CheckBox)

                Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
                Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)


                For i As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
                    If dtsHotelGroupDetails.Rows(i)("Code").ToString.Trim = lblHotelCode.Text.Trim Then
                        ChkBoxRows.Checked = True
                        Exit For
                    Else
                        ChkBoxRows.Checked = False
                    End If

                Next
            Next

        End If
    End Sub

    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "partymast.partyname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""

        strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode  left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode  where partymast.sptypecode='HOT'"

        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(C)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(S)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(CT)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(CTG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(catmast.catname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(catmast.catname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(H)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(partymast.partyname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(partymast.partyname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(HG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
                Else

                    strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
                End If
            End If

            If strlbValue.Contains("(HC)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelchainmaster.hotelchainname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND upper(hotelchainmaster.hotelchainname) = " & Trim(strlbValue) & ""
                End If
            End If
            If strlbValue.Contains("(HS)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(hotelstatus.hotelstatusname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(hotelstatus.hotelstatusname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(T)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " ( upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR   (upper(partymast.partyname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Trim.ToUpper) & "%'  or   upper(sectormaster.sectorname)  LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' )) "
                        End If
                    End If
                Next
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "(" & strWhereCond1 & ")"
                Else
                    strWhereCond = strWhereCond & " or (" & strWhereCond1 & ")"
                End If

            End If
        End If


        If Trim(strWhereCond) <> "" Then
            strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
        Else
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(myDS)
        gv_SearchResult.DataBind()


        Dim dtsHotelGroupDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("partycode").ToString
                If dtsHotelGroupDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If
        'End If


        Dim dataView As DataView = New DataView(myDS.Tables(0))
        dataView.Sort = "sortorder desc, partyname asc"
        gv_SearchResult.DataSource = dataView
        If myDS.Tables(0).Rows.Count > 0 Then
            gv_SearchResult.DataBind()
        Else
            gv_SearchResult.PageIndex = 0
            gv_SearchResult.DataBind()
            'lblMsg.Visible = True
            'lblMsg.Text = "Records not found, Please redefine search criteria."
        End If
    End Sub

    Protected Sub btnvsprocess1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess1.Click
        FilterGrid()
    End Sub

    Private Sub FillGridByType(ByVal strType As String, ByVal lsProcessValue As String)
        Dim strorderby As String = "partymast.partyname"
        Dim strsortorder As String = "ASC"
        Dim Type() As String
        Dim value() As String
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        '  lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            lsProcessValue = lsProcessValue.ToUpper
            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode=partymast.hotelstatuscode   where partymast.sptypecode='HOT' "
            Type = strType.Split(":")
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = (((((((Trim(lsProcessValue.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "").Replace("(RC)", "")
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        If value.Length > 1 Then   '' Added shahul 12/0/18
                            lsProcessValue = Trim(lsProcessValue.Trim).Replace(":", ",")
                        Else
                            lsProcessValue = "'" & lsProcessValue & "'"
                        End If
                    End If


                    If Type(k) = "C" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(ctrymast.ctryname) IN(" & Trim(value(k).Replace("(C)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(value(k).Replace("(C)", "")) & ")"
                        End If
                    End If


                    If Type(k) = "S" Then

                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(sectormaster.sectorname) IN ( " & Trim(value(k).Replace("(S)", "")) & ")"
                        Else

                            strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(value(k).Replace("(S)", "")) & ")"
                        End If
                    End If
                    If Type(k) = "CT" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(citymast.cityname) IN (" & Trim(value(k).Replace("(CT)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(value(k).Replace("(CT)", "")) & ")"
                        End If
                    End If

                    If Type(k) = "CTG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(catmast.catname) IN (" & Trim(value(k).Replace("(CTG)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND upper(catmast.catname) IN (" & Trim(value(k).Replace("(CTG)", "")) & ")"
                        End If
                    End If

                    If Type(k) = "HG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname In (" & Trim(value(k).Replace("(HG)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname IN (" & Trim(value(k).Replace("(HG)", "")) & "))"
                        End If
                    End If
                    If Type(k) = "HS" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "upper(hotelstatus.hotelstatusname) IN (" & Trim(value(k).Replace("(HS)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(hotelstatus.hotelstatusname) IN (" & Trim(value(k).Replace("(HS)", "")) & ")"
                        End If
                    End If
                    If Type(k) = "HC" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "upper(hotelchainmaster.hotelchainname) IN (" & Trim(value(k).Replace("(HC)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(hotelchainmaster.hotelchainname) IN (" & Trim(value(k).Replace("(HC)", "")) & ")"
                        End If
                    End If
                    If Type(k) = "RC" Then
                        If Trim(strWhereCond) = "" Then
                            If value.Length > 1 Then   '' Added shahul 12/0/18
                                strWhereCond = " partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname in (" & Trim(lsProcessValue) & ")))"
                            Else
                                strWhereCond = " partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname in (" & Trim(value(k).Replace("(RC)", "")) & ")))"
                            End If

                        Else
                            '  strWhereCond = strWhereCond & " AND partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname in (" & Trim(value(k).Replace("(RC)", "")) & ")))"

                            If value.Length > 1 Then   '' Added shahul 12/0/18
                                strWhereCond = strWhereCond & " AND partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname in (" & Trim(lsProcessValue) & ")))"
                            Else
                                strWhereCond = strWhereCond & " AND  partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname in (" & Trim(value(k).Replace("(RC)", "")) & ")))"
                            End If

                        End If
                    End If
                    '   select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode in ('')
                    If Type(k) = "T" Then
                        Dim lsMainArr As String()
                        Dim limitArray As String()
                        Dim strValue As String = ""
                        Dim strWhereCond1 As String = ""
                        lsMainArr = lsProcessValue.Split(",")
                        limitArray = value(k).Split(",")
                        value(k) = value(k).Replace("'", "")

                        For i = 0 To limitArray.GetUpperBound(0)
                            strValue = ""
                            strValue = limitArray(i).Replace("'", "")

                            If strValue <> "" Then
                                If Trim(strWhereCond1) = "" Then
                                    strWhereCond1 = " (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )  or (partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname like ('%" & Trim(strValue.Replace("(T)", "")) & "%')))) ) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR (upper(partymast.partyname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'   or upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelstatus.hotelstatusname) like '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(hotelchainmaster.hotelchainname) like '%" & Trim(strValue.Replace("(T)", "")) & "%'   or   upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(catmast.catname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or   partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )  or (partymast.partycode in (select distinct partycode from  partyrmtyp where inactive=0 and roomclasscode  IN  (select distinct roomclasscode from room_classification where roomclassname like ('%" & Trim(strValue.Replace("(T)", "")) & "%')))) ) "
                                End If

                            End If

                        Next
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "(" & strWhereCond1 & ")"
                        Else
                            strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
                        End If
                    End If
                Next
            End If
            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_SearchResult.DataBind()
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub

    Protected Sub btnResetSelection1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection1.Click

        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
        dtsHotelGroupDetails.Rows.Clear()
        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
        gv_SearchResult.DataSource = dtsHotelGroupDetails
        gv_SearchResult.DataBind()


        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtHotelDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtHotelDynamic") = dtDynamic
        dlList1.DataSource = dtDynamic
        dlList1.DataBind()

    End Sub

    Protected Sub chkSSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSSelect"), CheckBox)
            Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
            Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
                iFlag = 0
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(S)") Then
                            dtsHotelGroupDetails.Rows.Add("S", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(G)") Then
                            dtsHotelGroupDetails.Rows.Add("G", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsHotelGroupDetails.Rows.Add("C", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsHotelGroupDetails.Rows.Add("CT", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsHotelGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(H)") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("Hotels") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HG)") Then
                            dtsHotelGroupDetails.Rows.Add("HG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HS)") Then
                            dtsHotelGroupDetails.Rows.Add("HS", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HC)") Then
                            dtsHotelGroupDetails.Rows.Add("HC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(RC)") Then
                            dtsHotelGroupDetails.Rows.Add("RC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsHotelGroupDetails.Rows.Add("T", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        End If
                        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

                    End If

                End If

                'If fnValidateDuplication(lblHotelCode.Text) = False Then
                '    row.BackColor = Drawing.Color.SkyBlue
                'End If
            Else
                ChkBoxRows.Checked = False
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                row.BackColor = Drawing.Color.Empty
                Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtHotelDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsHotelGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
                        Session("sDtHotelDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtHotelDynamic") = dtt
            dlList1.DataSource = dtt
            dlList1.DataBind()


        Next
    End Sub

    Protected Sub chkSSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim ChkBoxRows As CheckBox = CType(sender, CheckBox)
            Dim lblHotelCode As Label = CType(ChkBoxRows.FindControl("lblHotelCode"), Label)
            Dim lblHotelName As Label = CType(ChkBoxRows.FindControl("lblHotelName"), Label)
            Dim row As GridViewRow = CType(ChkBoxRows.NamingContainer, GridViewRow)
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")

            If ChkBoxRows.Checked = True Then


                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").ToString = lblHotelCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If iFlag = 0 Then
                        dtsHotelGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(S)") Then
                            dtsHotelGroupDetails.Rows.Add("S", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(G)") Then
                            dtsHotelGroupDetails.Rows.Add("G", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(C)") Then
                            dtsHotelGroupDetails.Rows.Add("C", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsHotelGroupDetails.Rows.Add("CT", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTG)") Then
                            dtsHotelGroupDetails.Rows.Add("CTG", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(H)") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("Hotels") Then
                            dtsHotelGroupDetails.Rows.Add("H", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HC)") Then
                            dtsHotelGroupDetails.Rows.Add("HC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(HS)") Then
                            dtsHotelGroupDetails.Rows.Add("HS", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(RC)") Then
                            dtsHotelGroupDetails.Rows.Add("RC", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsHotelGroupDetails.Rows.Add("T", txtNameNew.Text, lblHotelCode.Text, lblHotelName.Text)
                        End If

                        Session("sDtHotelGroupDetails") = dtsHotelGroupDetails

                    End If

                End If

                'If fnValidateDuplication(lblHotelCode.Text) = False Then
                '    row.BackColor = Drawing.Color.SkyBlue
                'End If

            Else

                ChkBoxRows.Checked = False
                If dtsHotelGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                        If dtsHotelGroupDetails.Rows(i)("Code").Trim.ToString = lblHotelCode.Text.Trim Then
                            dtsHotelGroupDetails.Rows.Remove(dtsHotelGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                row.BackColor = Drawing.Color.Empty
                Session("sDtHotelGroupDetails") = dtsHotelGroupDetails
            End If

            ' Check all check box is checked  or not.
            Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
            Dim row1 As GridViewRow
            For Each row1 In gv_SearchResult.Rows
                Dim ChkBoxRows1 As CheckBox = CType(row1.FindControl("chkSSelect"), CheckBox)
                If ChkBoxRows1.Checked = True Then
                    iFlagCheckedAll = 0
                Else
                    iFlagCheckedAll = 1
                    Exit For
                End If

            Next

            If iFlagCheckedAll = 0 And ChkBoxHeader.Checked = False Then
                ChkBoxHeader.Checked = True
            End If
            If iFlagCheckedAll = 1 And ChkBoxHeader.Checked = True Then
                ChkBoxHeader.Checked = False
            End If


            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtHotelDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsHotelGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Country").ToString = "Hotels" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("H", "Hotels")
                        Session("sDtHotelDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtHotelDynamic") = dtt
            dlList1.DataSource = dtt
            dlList1.DataBind()
            ChkBoxRows.Focus()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub imgbRoomType_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim imgbRoomType As ImageButton = CType(sender, ImageButton)
        Dim lblHotelCode As Label = CType(imgbRoomType.FindControl("lblHotelCode"), Label)
        hdnhotelcode.value = lblHotelCode.Text

        Dim dtHotelDynamic As New DataTable
        dtHotelDynamic = Session("sDtHotelDynamic")
        Dim strRC As String = ""
        If dtHotelDynamic.Rows.Count > 0 Then
            For i As Integer = 0 To dtHotelDynamic.Rows.Count - 1
                If dtHotelDynamic.Rows(i)("Code").ToString() = "RC" Then
                    If strRC = "" Then
                        strRC = "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    Else
                        strRC = strRC & "," & "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    End If
                End If
            Next
        End If

        Dim dt As New DataTable
        Dim strSqlQry As String = ""
        If strRC = "" Then
            strSqlQry = "select partyrmtyp.*,room_classification.roomclassname from  partyrmtyp,room_classification where inactive=0 and partyrmtyp.roomclasscode=room_classification.roomclasscode and partycode='" + lblHotelCode.Text + "' order by rankord"
        Else
            strSqlQry = "select partyrmtyp.*,room_classification.roomclassname from  partyrmtyp,room_classification where inactive=0 and partyrmtyp.roomclasscode=room_classification.roomclasscode and partycode='" + lblHotelCode.Text + "' and partyrmtyp.roomclasscode in (select distinct roomclasscode from room_classification where roomclassname in (" & strRC & ")) order by rankord"
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(dt)

        gvRoomType.DataBind()
        gvRoomType.DataSource = dt

        gvRoomType.PageIndex = 0
        If dt.Rows.Count > 0 Then
            gvRoomType.DataBind()

        Else

            gvRoomType.DataBind()
        End If
        ModalRoomType.Show()
    End Sub
    Sub Fillroomtypesgrid(ByVal hotelcode As String)


        Dim dtHotelDynamic As New DataTable
        dtHotelDynamic = Session("sDtHotelDynamic")
        Dim strRC As String = ""
        If dtHotelDynamic.Rows.Count > 0 Then
            For i As Integer = 0 To dtHotelDynamic.Rows.Count - 1
                If dtHotelDynamic.Rows(i)("Code").ToString() = "RC" Then
                    If strRC = "" Then
                        strRC = "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    Else
                        strRC = strRC & "," & "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    End If
                End If
            Next
        End If

        Dim dt As New DataTable
        Dim strSqlQry As String = ""
        If strRC = "" Then
            strSqlQry = "select partyrmtyp.*,room_classification.roomclassname from  partyrmtyp,room_classification where inactive=0 and partyrmtyp.roomclasscode=room_classification.roomclasscode and partycode='" + hotelcode + "' order by rankord"
        Else
            strSqlQry = "select partyrmtyp.*,room_classification.roomclassname from  partyrmtyp,room_classification where inactive=0 and partyrmtyp.roomclasscode=room_classification.roomclasscode and partycode='" + hotelcode + "' and partyrmtyp.roomclasscode in (select distinct roomclasscode from room_classification where roomclassname in (" & strRC & ")) order by rankord"
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(dt)

        gvRoomType.DataBind()
        gvRoomType.DataSource = dt

        If dt.Rows.Count > 0 Then
            gvRoomType.DataBind()

        Else
            gvRoomType.PageIndex = 0
            gvRoomType.DataBind()
        End If
        ModalRoomType.Show()
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)

            Dim lblPartyName As Label = e.Row.FindControl("lblPartyName")
            Dim lblCatName As Label = e.Row.FindControl("lblCatName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectName")
            Dim lblHotelGroup As Label = e.Row.FindControl("lblHotelGroup")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextCG As String = ""
            Dim lsSearchTextHC As String = ""
            Dim lsSearchTextHG As String = ""
            Dim lsSearchTextRC As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtHotelDynamic")
            If Session("sDtHotelDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "CTG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Country").ToString.ToString.Replace("(CTG)", "").Trim.ToUpper
                        End If
                        If "CT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("Country").ToString.Replace("(CT)", "").Trim.ToUpper
                        End If
                        If "G" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCG = dtDynamics.Rows(j)("Country").ToString.Replace("(G)", "").Trim.ToUpper
                        End If
                        If "HC" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextHC = dtDynamics.Rows(j)("Country").ToString.Replace("(HC)", "").Trim.ToUpper
                        End If
                        If "S" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("Country").ToString.Replace("(S)", "").Trim.ToUpper
                        End If
                        If "HG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextHotelGroup = dtDynamics.Rows(j)("Country").ToString.Replace("(HG)", "").Trim.ToUpper
                        End If
                        If "RC" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextRC = dtDynamics.Rows(j)("Country").ToString.Replace("(RC)", "").Trim.ToUpper
                        End If

                        'If "HOTELGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                        '    lsSearchTextHotelGroup = dtDynamics.Rows(j)("Country").ToString.Trim.ToUpper
                        'End If
                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCat = dtDynamics.Rows(j)("Country").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCat
                            lsSearchTextSector = lsSearchTextCat
                            lsSearchTextHotelGroup = lsSearchTextCat
                            lsSearchTextPartyName = lsSearchTextCat
                            lsSearchTextCG = lsSearchTextCat
                            lsSearchTextHC = lsSearchTextCat
                            lsSearchTextHG = lsSearchTextCat
                            lsSearchTextRC = lsSearchTextCat
                        End If

                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblCityName.Text = Regex.Replace(lblCityName.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextSector.Trim <> "" Then
                            lblSectName.Text = Regex.Replace(lblSectName.Text.Trim, lsSearchTextSector.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCat.Trim <> "" Then
                            lblCatName.Text = Regex.Replace(lblCatName.Text.Trim, lsSearchTextCat.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextHotelGroup.Trim <> "" Then
                            lblHotelGroup.Text = Regex.Replace(lblHotelGroup.Text.Trim, lsSearchTextHotelGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If

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

    Protected Sub lnkCodeAndValue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Try
        '    ucMarkup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        '    objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        'End Try
    End Sub

    Protected Sub lnkCodeAndValue_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ucMarkup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub lnkCodeAndValue2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ucMarkup.fnCodeAndValue_ButtonClick(sender, e, dlList, Nothing, Nothing)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Function checkForDateOverlap() As Boolean
        For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
            Dim txtFrom As TextBox = CType(row.FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            Dim txtTo As TextBox = CType(row.FindControl("txtExhibitionAndHolidayToDate"), TextBox)
            Dim dtFrom As DateTime = CType(txtFrom.Text, DateTime)
            Dim dtTo As DateTime = CType(txtTo.Text, DateTime)

            For Each rowInner As GridViewRow In gvExhibitionAndHolidayInput.Rows
                If row.RowIndex <> rowInner.RowIndex Then
                    Dim txtFromInner As TextBox = CType(rowInner.FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
                    Dim txtToInner As TextBox = CType(rowInner.FindControl("txtExhibitionAndHolidayToDate"), TextBox)
                    Dim dtFromInner As DateTime = CType(txtFromInner.Text, DateTime)
                    Dim dtToInner As DateTime = CType(txtToInner.Text, DateTime)
                    'If (((DateTime.Compare(dtFrom, dtFromInner) > 0) And (DateTime.Compare(dtFrom, dtToInner) > 0)) Or ((DateTime.Compare(dtTo, dtFromInner) > 0) And (DateTime.Compare(dtTo, dtToInner) > 0))) Then ' which means ("date1 > date2") 
                    '    Return False
                    'End If
                    If (dtFrom >= dtFromInner And dtFrom <= dtToInner) Or (dtTo >= dtFromInner And dtTo <= dtToInner) Then
                        Return False
                    End If

                End If
            Next

        Next

        Return True
    End Function

    Protected Sub gvExhibitionAndHolidayInput_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExhibitionAndHolidayInput.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow AndAlso (e.Row.RowState = DataControlRowState.Normal OrElse e.Row.RowState = DataControlRowState.Alternate) Then
            e.Row.Attributes("onclick") = String.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex)
            e.Row.Attributes("onkeydown") = "javascript:return SelectSibling(event);"
            e.Row.Attributes("onselectstart") = "javascript:return false;"
        End If
    End Sub

    Private Function checkForDayOfWeek() As Boolean
        For Each row As GridViewRow In gvWeekOfDays.Rows
            Dim chkWSelect As CheckBox = CType(row.FindControl("chkWSelect"), CheckBox)
            If chkWSelect.Checked = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function checkForInventoryType() As Boolean
        For Each row As GridViewRow In gvInventoryType.Rows
            Dim chkInvSelect As CheckBox = CType(row.FindControl("chkInvSelect"), CheckBox)
            If chkInvSelect.Checked = True Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function checkForMissingDate() As Boolean
        For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows
            Dim txtFrom As TextBox = CType(row.FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            Dim txtTo As TextBox = CType(row.FindControl("txtExhibitionAndHolidayToDate"), TextBox)
            If txtFrom.Text = "" Then
                txtFrom.Focus()
                Return False
            End If
            If txtTo.Text = "" Then
                txtTo.Focus()
                Return False
            End If
        Next
        Return True
    End Function

    Protected Sub btnLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLog.Click
        'dvNew.Visible = False
        'dvLog.Visible = True
        'dvPendingApproval.Visible = False
        'dvSearch.Visible = True
        'gvLog.Visible = False
        'BindLogs()
        'btnSave.Text = "Save"
        'ucMarkup.clearsessions()   '' ADDED shahul 20/03/18



        dvNew.Visible = False
        dvLog.Visible = True
        dvPendingApproval.Visible = False
        dvSearch.Visible = True
        gvLog.Visible = True
        gvSearch.Visible = False
        BindLogs()
        btnSave.Text = "Save"
        ucMarkup.clearsessions()   '' ADDED shahul 20/03/18

    End Sub
    Protected Sub lbEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Edit"
        hdOPMode.Value = "E"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)

        EnableControls()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
        btnCheckExist.Visible = True
        btnSave.Text = "Update"

    End Sub
    Protected Sub lbPEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Edit"
        hdOPMode.Value = "E"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)

        EnableControls()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
        btnCheckExist.Visible = True
        btnSave.Text = "Update"

    End Sub
    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Delete"
        hdOPMode.Value = "D"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        DisableControl()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True

        btnCheckExist.Visible = False
        btnSave.Enabled = True
        btnSave.Visible = True
        btnSave.Text = "Delete"
        btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete markup?')==false)return false;")
    End Sub
    Protected Sub lbPDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Delete"
        hdOPMode.Value = "D"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        DisableControl()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True

        btnCheckExist.Visible = False
        btnSave.Enabled = True
        btnSave.Visible = True
        btnSave.Text = "Delete"
        btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete markup?')==false){HideProgess();return false;}")
    End Sub
    Protected Sub lbView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "View"
        hdOPMode.Value = "V"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        DisableControl()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
        btnCheckExist.Visible = False

    End Sub

    Protected Sub lbSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Edit"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        hdOPMode.Value = "E"
        EnableControls()

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
        btnCheckExist.Visible = True
        btnSave.Text = "Save"
    End Sub

    Private Sub bindsearchgrid(ByVal abExcelExport As Boolean)
        Dim lsFormulaId As String = ""
        Dim lsInvType As String = ""
        Dim lsRoomClscode As String = ""
        Dim lsRoomCls As String = ""
        Dim lsText As String = ""
        Dim lsHotel As String = ""
        Dim lsCountry As String = ""
        Dim lsAgent As String = ""
        Dim dttSess As DataTable

        dttSess = Session("sDtDynamicSearch")
        If dttSess IsNot Nothing Then
            If dttSess.Rows.Count > 0 Then
                For Each dtR As DataRow In dttSess.Rows
                    Select Case UCase(Trim(dtR("code")))
                        Case "FORMULA-ID"
                            lsFormulaId += IIf(lsFormulaId.Trim = "", "", ",") + dtR("value")
                        Case "INVENTORYTYPE"
                            lsInvType += IIf(lsInvType.Trim = "", "", ",") + dtR("value")

                        Case "ROOMCLASSIFICATION"
                            lsRoomCls += IIf(lsRoomCls.Trim = "", "", ",") + objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ROOM_CLASSIFICATION", "ROOMCLASScode", "ROOMCLASSname", dtR("value"))
                        Case "HOTEL"
                            lsHotel += IIf(lsHotel.Trim = "", "", ",") + objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "partymast", "partycode", "partyname", dtR("value"))
                        Case "AGENT"
                            lsAgent += IIf(lsAgent.Trim = "", "", ",") + objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", dtR("value"))
                        Case "COUNTRY"
                            lsCountry += IIf(lsCountry.Trim = "", "", ",") + objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctrycode", "ctryname", dtR("value"))
                        Case "TEXT"
                            lsText += IIf(lsText.Trim = "", "", ",") + dtR("value")
                    End Select

                Next
            End If
        End If

        If gvSearch.PageIndex < 0 Then

            gvSearch.PageIndex = 0
        End If
        '    Dim strConnName As String = (HttpContext.Current.Session("dbconnectionName")).ToString
        Dim constring As String = ConfigurationManager.ConnectionStrings(Session("dbconnectionName")).ConnectionString
        Dim dtt As New DataSet
        '   gvSearch.DataSource = Nothing
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("[sp_search_markup]")
                cmd.CommandType = CommandType.StoredProcedure
                'FilterGridSEARCH()
                cmd.Parameters.AddWithValue("@inventorytype", lsInvType)
                cmd.Parameters.AddWithValue("@partycode", "")
                cmd.Parameters.AddWithValue("@roomclasscode ", lsRoomCls)
                cmd.Parameters.AddWithValue("@formulaid ", lsFormulaId)
                cmd.Parameters.AddWithValue("@Hotel", lsHotel)
                cmd.Parameters.AddWithValue("@Country", lsCountry)
                cmd.Parameters.AddWithValue("@Agent", lsAgent)
                cmd.Parameters.AddWithValue("@text ", lsText)
                If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                    cmd.Parameters.AddWithValue("@fromdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
                    cmd.Parameters.AddWithValue("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))
                End If
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    sda.Fill(dtt)
                    If abExcelExport = True Then
                        Session("ViewMarkupsExcelExport") = dtt
                    End If
                    If dtt.Tables(0).Rows.Count > 0 Then

                        gvSearch.PageSize = RowsPerPageCS.SelectedValue
                        gvSearch.DataSource = dtt.Tables(0)
                        gvSearch.DataBind()


                    Else

                        gvSearch.PageIndex = 0
                        gvSearch.DataSource = dtt.Tables(0)
                        gvSearch.DataBind()
                        lblMsg.Visible = True
                        lblMsg.Text = "Records not found, Please redefine search criteria."
                    End If

                End Using
            End Using
        End Using

        Try

            'Using con As New SqlConnection(constring)
            '    Using cmd As New SqlCommand("[SP_SelectApplyMarkup]")
            '        cmd.CommandType = CommandType.StoredProcedure
            '        'FilterGridSEARCH()
            '        cmd.Parameters.AddWithValue("@inventorytype", lsInvType)
            '        'cmd.Parameters.AddWithValue("@partycode", "")
            '        cmd.Parameters.AddWithValue("@roomclasscode ", lsRoomCls)
            '        cmd.Parameters.AddWithValue("@formulaid ", lsFormulaId)
            '        cmd.Parameters.AddWithValue("@Hotel", lsHotel)
            '        cmd.Parameters.AddWithValue("@Country", lsCountry)
            '        cmd.Parameters.AddWithValue("@Agent", lsAgent)
            '        cmd.Parameters.AddWithValue("@text ", lsText)
            '        'If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            '        '    cmd.Parameters.AddWithValue("@fromdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
            '        '    cmd.Parameters.AddWithValue("@todate", Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"))
            '        'End If
            '        Using sda As New SqlDataAdapter()
            '            cmd.Connection = con
            '            sda.SelectCommand = cmd
            '            sda.Fill(dtt1, "markups1")
            '            'If abExcelExport = True Then
            '            '    'Session("ViewMarkupsExcelExport") = dtt
            '            'End If
            'If dtt.Tables(1).Rows.Count > 0 Then

            gvLog.PageSize = RowsPerPageCS.SelectedValue
            gvLog.DataSource = dtt.Tables(1)
            gvLog.DataBind()


            'Else

            'gvLog.PageIndex = 0
            'gvLog.DataSource = dtt.Tables(1)
            'gvLog.DataBind()
            'lblMsg.Visible = True
            'lblMsg.Text = "Records not found, Please redefine search criteria."
            'End If

            '        End Using
            '    End Using
            'End Using


        Catch ex As Exception

        End Try
        'Dim dt As New DataTable
        'Dim strSqlQry As String = ""

        'strSqlQry = "select ApplyMarkupId,case when Approved =1 then 'Yes' else 'No' end ApprovedStatus,FormulaId,FormulaName,SearchCriteria,DaysOfTheWeek,InventoryTypes, convert(varchar(10), AddDate,103)+ ' '+  convert(varchar(5), AddDate,108) AddDate,AddUser,convert(varchar(10), ModDate,103)+ ' '+  convert(varchar(5), ModDate,108) ModDate,ModUser,ApplicableTo from ApplyMarkup_Header"


        'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        'myDataAdapter.Fill(dt)

        'gvLog.DataBind()
        'gvLog.DataSource = dt

        'If dt.Rows.Count > 0 Then
        '    gvLog.DataBind()

        'Else
        '    gvLog.PageIndex = 0
        '    gvLog.DataBind()
        'End If
    End Sub
    Private Sub BindLogs()
        Dim dt As New DataTable
        Dim strSqlQry As String = ""

        strSqlQry = "select ApplyMarkupId,case when Approved =1 then 'Yes' else 'No' end ApprovedStatus,FormulaId,FormulaName,SearchCriteria,DaysOfTheWeek,InventoryTypes, convert(varchar(10), AddDate,103)+ ' '+  convert(varchar(5), AddDate,108) AddDate,AddUser,convert(varchar(10), ModDate,103)+ ' '+  convert(varchar(5), ModDate,108) ModDate,ModUser,ApplicableTo from ApplyMarkup_Header"


        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(dt)

        gvLog.DataBind()
        gvLog.DataSource = dt
        gvLog.PageSize = RowsPerPageCS.SelectedValue
        If dt.Rows.Count > 0 Then
            gvLog.DataBind()

        Else
            gvLog.PageIndex = 0
            gvLog.DataBind()
        End If
    End Sub

    Protected Sub btnPending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPending.Click
        dvNew.Visible = False
        dvLog.Visible = False
        dvSearch.Visible = False
        dvPendingApproval.Visible = True
        BindPendingApproval()
    End Sub

    Private Sub BindPendingApproval()
        Dim dt As New DataTable
        Dim strSqlQry As String = ""

        strSqlQry = "select ApplyMarkupId,case when Approved =1 then 'Yes' else 'No' end ApprovedStatus,FormulaId,FormulaName,SearchCriteria,DaysOfTheWeek,InventoryTypes, convert(varchar(10), AddDate,103)+ ' '+  convert(varchar(5), AddDate,108) AddDate,AddUser,convert(varchar(10), ModDate,103)+ ' '+  convert(varchar(5), ModDate,108) ModDate,ModUser,ApplicableTo from ApplyMarkup_Header where Approved =0 order by AddDate desc"


        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(dt)

        gvPendingApproval.DataBind()
        gvPendingApproval.DataSource = dt

        If dt.Rows.Count > 0 Then
            gvPendingApproval.DataBind()

        Else
            gvPendingApproval.PageIndex = 0
            gvPendingApproval.DataBind()
        End If
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        dvSearch.Visible = False
        hdFillByGrid.Value = "N"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        lblHeading.Text = "Apply Markup"

        Dim col As System.Drawing.Color
        col = System.Drawing.ColorTranslator.FromHtml("#2D7C8A")
        btnNew.BackColor = col
        btnNew.ForeColor = Drawing.Color.White

        Dim col1 As System.Drawing.Color
        col1 = System.Drawing.ColorTranslator.FromHtml("#e7e7e7")
        btnEdit.BackColor = col1
        btnDelete.BackColor = col1
        btnCancel_new.BackColor = col1
        btnView.BackColor = col1
        btnEdit.ForeColor = col
        btnDelete.ForeColor = col
        btnView.ForeColor = col
        btnCancel_new.ForeColor = col

        dlList.Visible = True

        btnSave.Visible = True
        ClearFields()
        EnableControls()

        ucMarkup.sbSetPageState("", "ApplyMarkup", CType(Session("ApplyMarkupState"), String))
        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        ucMarkup.sbShowCountry()
        hdOPMode.Value = "N"

        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
        btnCheckExist.Visible = True
    End Sub

    Private Sub FillMarkupDetails(ByVal strApplyMarkupId As String)

        ucMarkup.sbSetPageState(strApplyMarkupId.Trim, "APPLY_MARKUP", Session("ApplyMarkupState"))

        Dim dtDynamics As New DataTable
        dtDynamics = Session("sDtDynamic")
        dtDynamics.Rows.Clear()
        Session("sDtDynamic") = dtDynamics
        Session.Add("RefCode", CType(strApplyMarkupId.Trim, String))
        'ShowRecord(CType(lbltran.Text.Trim, String))
        'Showdetailsgrid(CType(lbltran.Text.Trim, String))
        ucMarkup.sbSetPageState(strApplyMarkupId.Trim, Nothing, ViewState("State"))
        Session("isAutoTick_wuccountrygroupusercontrol") = 1
        ucMarkup.sbShowCountry()



        Dim ds As New DataSet
        Dim strSqlQry As String = ""
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        mySqlCmd = New SqlCommand()
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.CommandText = "SP_GET_APPLY_MARKUP_DETAILS"
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkupId", strApplyMarkupId))
        myDataAdapter = New SqlDataAdapter()
        mySqlCmd.Connection = SqlConn
        myDataAdapter.SelectCommand = mySqlCmd
        myDataAdapter.Fill(ds)

        If ds.Tables(0).Rows.Count > 0 Then 'ApplyMarkup_Header
            txtmarkupCode.Text = ds.Tables(0).Rows(0)("FormulaId").ToString
            txtMarkupFormulaName.Text = ds.Tables(0).Rows(0)("FormulaName").ToString
            txtMarkupFormula.Text = ds.Tables(0).Rows(0)("MarkupFormula").ToString
            txtApplicableTo.Text = ds.Tables(0).Rows(0)("ApplicableTo").ToString
            If ds.Tables(0).Rows(0)("Approved").ToString = "1" Then
                chkActive.Checked = True
            Else
                chkActive.Checked = False
            End If

        End If
        If ds.Tables(1).Rows.Count > 0 Then 'DaysOfTheWeek
            For k As Integer = 0 To ds.Tables(1).Rows.Count - 1
                Dim lblWeekDaysCode As Label
                Dim chkWSelect As CheckBox
                If ds.Tables(1).Rows(0)("DaysOfTheWeek").ToString = "ALL" Then
                    Dim chkWSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkWSelectAll"), CheckBox)
                    chkWSelectAll.Checked = True
                    For Each gvR As GridViewRow In gvWeekOfDays.Rows
                        chkWSelect = CType(gvR.FindControl("chkWSelect"), CheckBox)
                        chkWSelect.Checked = True
                    Next
                Else
                    For Each gvR As GridViewRow In gvWeekOfDays.Rows
                        lblWeekDaysCode = CType(gvR.FindControl("lblWeekDaysCode"), Label)
                        chkWSelect = CType(gvR.FindControl("chkWSelect"), CheckBox)
                        If lblWeekDaysCode.Text = ds.Tables(1).Rows(k)("DaysOfTheWeek").ToString Then
                            chkWSelect.Checked = True
                            Exit For
                        End If
                    Next
                End If

            Next
        End If
        If ds.Tables(2).Rows.Count > 0 Then 'InventoryType
            For k As Integer = 0 To ds.Tables(2).Rows.Count - 1
                Dim lblInventoryType As Label
                Dim chkInvSelect As CheckBox

                For Each gvR As GridViewRow In gvInventoryType.Rows
                    lblInventoryType = CType(gvR.FindControl("lblInventoryType"), Label)
                    chkInvSelect = CType(gvR.FindControl("chkInvSelect"), CheckBox)
                    If lblInventoryType.Text = ds.Tables(2).Rows(k)("InventoryType").ToString Then
                        chkInvSelect.Checked = True
                        Exit For
                    End If
                Next
            Next
        End If

        If ds.Tables(3).Rows.Count > 0 Then 'ApplyMarkup_Dates
            gvExhibitionAndHolidayInput.DataSource = ds.Tables(3)
            gvExhibitionAndHolidayInput.DataBind()
        End If

        If ds.Tables(4).Rows.Count > 0 Then 'Search Critieria
            Dim dtHotelDynamic As New DataTable
            dtHotelDynamic = Session("sDtHotelDynamic")
            dtHotelDynamic.Rows.Clear()
            If ds.Tables(4).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(4).Rows.Count - 1
                    Dim strValue As String = ""
                    strValue = ds.Tables(4).Rows(i)("SearchCriteria").ToString
                    dtHotelDynamic.NewRow()
                    If strValue.Contains("(S)") Then
                        dtHotelDynamic.Rows.Add("S", strValue)
                    ElseIf strValue.Contains("(G)") Then
                        dtHotelDynamic.Rows.Add("G", strValue)
                    ElseIf strValue.Contains("(C)") Then
                        dtHotelDynamic.Rows.Add("C", strValue)
                    ElseIf strValue.Contains("(CT)") Then
                        dtHotelDynamic.Rows.Add("CT", strValue)
                    ElseIf strValue.Contains("(CTG)") Then
                        dtHotelDynamic.Rows.Add("CTG", strValue)
                    ElseIf strValue.Contains("(HC)") Then
                        dtHotelDynamic.Rows.Add("HC", strValue)
                    ElseIf strValue.Contains("(HS)") Then
                        dtHotelDynamic.Rows.Add("HS", strValue)
                    ElseIf strValue.Contains("(HG)") Then
                        dtHotelDynamic.Rows.Add("HG", strValue)
                    ElseIf strValue.Contains("(RC)") Then
                        dtHotelDynamic.Rows.Add("RC", strValue)
                    ElseIf strValue.Contains("(T)") Then
                        dtHotelDynamic.Rows.Add("T", strValue)
                    ElseIf strValue = "Hotels" Then
                        dtHotelDynamic.Rows.Add("H", strValue)
                    End If

                Next
                Session("sDtHotelDynamic") = dtHotelDynamic
                dlList1.DataSource = dtHotelDynamic
                dlList1.DataBind()
            End If
        End If

        If ds.Tables(5).Rows.Count > 0 Then 'ApplyMarkup_Hotels_Detail

            Dim dtsHotelGroupDetails As DataTable
            dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
            dtsHotelGroupDetails.Rows.Clear()
            If ds.Tables(5).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(5).Rows.Count - 1
                    dtsHotelGroupDetails.NewRow()
                    dtsHotelGroupDetails.Rows.Add("H", "Hotels", ds.Tables(5).Rows(i)("partycode").ToString, ds.Tables(5).Rows(i)("Hotel").ToString)
                Next
                txtNameNew.Text = "Hotels"
                hdLinkButtonValue.Value = "%"
                FillGridByLinkButton()
                FillCheckbox()

            End If

        End If

    End Sub

    ''' <summary>
    ''' FillGridForEdit
    ''' </summary>
    ''' <param name="strorderby"></param>
    ''' <param name="strsortorder"></param>
    ''' <remarks></remarks>
    ''' 

    Private Sub FillGridForEdit(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        '  lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = "SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='HOT' "

            'If txtGroupTagName.Text.Contains("(HG)") Then
            Dim strlbValue As String = ""
            '  strlbValue = (((((((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(HG)", "")).Replace("(H)", "")).Replace("(CTG)", "")
            strlbValue = "'" & strlbValue & "'"
            If Trim(strWhereCond) = "" Then
                strWhereCond = " partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
            Else

                strWhereCond = strWhereCond & " AND partymast.partycode in (select cgd.partycode   from hotelgroup cg,hotelgroup_detail cgd where cg.hotelgroupcode = cgd.hotelgroupcode and hotelgroupname=" & Trim(strlbValue) & ")"
            End If
            'End If

            If Trim(strWhereCond) <> "" Then
                strSqlQry = strSqlQry & " and " & strWhereCond & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            gv_SearchResult.DataBind()
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub

    Private Sub DisableControl()
        gvExhibitionAndHolidayInput.Enabled = False
        gvWeekOfDays.Enabled = False
        gvInventoryType.Enabled = False
        dlList.Enabled = False
        btnDeleteRow.Enabled = False
        btnExhibition.Enabled = False
        btnHoliday.Enabled = False
        btnSelectMarkup.Enabled = False
        txtMarkupFormula.Enabled = False
        txtMarkupFormulaName.Enabled = False
        txtApplicableTo.Enabled = False
        dlList1.Enabled = False
        chkActive.Enabled = False
        ucMarkup.Disable(False)
        btnSave.Visible = False
    End Sub

    Private Sub EnableControls()
        gvExhibitionAndHolidayInput.Enabled = True
        gvWeekOfDays.Enabled = True
        gvInventoryType.Enabled = True
        dlList.Enabled = True
        btnDeleteRow.Enabled = True
        btnExhibition.Enabled = True
        btnHoliday.Enabled = True
        btnSelectMarkup.Enabled = True
        txtMarkupFormula.Enabled = True
        txtMarkupFormulaName.Enabled = True
        txtApplicableTo.Enabled = True
        dlList1.Enabled = True
        chkActive.Enabled = True
        ucMarkup.Disable(True)
        btnSave.Visible = True
    End Sub



    Function fnValidateDuplication(ByVal strhotelCode As String) As Boolean
        Dim txtFromDate As TextBox
        Dim txtToDate As TextBox
        Dim strQuery As String = ""
        Dim strQuery1 As String = ""
        Dim strSelectedCountriesList As String = ""
        Dim strSelectedCountriesListMod As String = ""
        strSelectedCountriesList = ucMarkup.checkcountrylist.ToString
        Dim strSelectedCountriesLists As String() = strSelectedCountriesList.Split(",")
        For i As Integer = 0 To strSelectedCountriesLists.Length - 1
            If strSelectedCountriesListMod = "" Then
                strSelectedCountriesListMod = "'" & strSelectedCountriesLists(i) & "'"
            Else
                strSelectedCountriesListMod = strSelectedCountriesListMod & ",'" & strSelectedCountriesLists(i) & "'"
            End If
        Next

        Dim strSelectedAgentList As String = ""
        Dim strSelectedAgentListMod As String = ""
        strSelectedAgentList = ucMarkup.checkagentlist.ToString()
        Dim strSelectedAgentLists As String() = strSelectedAgentList.Split(",")
        For i As Integer = 0 To strSelectedAgentLists.Length - 1
            If strSelectedAgentListMod = "" Then
                strSelectedAgentListMod = "'" & strSelectedAgentLists(i) & "'"
            Else
                strSelectedAgentListMod = strSelectedAgentListMod & ",'" & strSelectedAgentLists(i) & "'"
            End If
        Next

        If strSelectedCountriesList = "" And strSelectedAgentList = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any country or agent.');", True)

        End If


        If checkForMissingDate() = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter markup dates.');", True)

        End If

        For Each gvRow As GridViewRow In gvExhibitionAndHolidayInput.Rows
            txtFromDate = gvRow.FindControl("txtExhibitionAndHolidayfromDate")
            txtToDate = gvRow.FindControl("txtExhibitionAndHolidayToDate")
            strQuery = ""
            strQuery1 = ""
            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                ' strQuery = "select count(ctrycode)cnt from ApplyMarkup_Countries where ApplyMarkupId in (select distinct ApplyMarkupId from Markup_Hotels where convert(datetime,MarkupDate,101)  between convert(datetime,'" & txtFromDate.Text & "',103) and convert(datetime,'" & txtToDate.Text & "',103) and PartyCode='" & strhotelCode & "') and ctrycode in (" & strSelectedCountriesListMod & ")"
                strQuery = "select count(ctrycode)cnt from ApplyMarkup_Countries where ApplyMarkupId in (select distinct ApplyMarkupId from Markup_Hotels where ((convert(datetime,MarkupFromDate,101)  between convert(datetime,'" & txtFromDate.Text & "',103) and   convert(datetime,'" & txtToDate.Text & "',103)) or  ( convert(datetime,MarkupToDate,101)  between convert(datetime,'" & txtFromDate.Text & "',103)and convert(datetime,'" & txtToDate.Text & "',103))   or (convert(datetime,'" & txtFromDate.Text & "',103) between convert(datetime,MarkupFromDate,101)  and convert(datetime,MarkupToDate,101)) or (convert(datetime,'" & txtToDate.Text & "',103) between convert(datetime,MarkupFromDate,101)  and convert(datetime,MarkupToDate,101))) and PartyCode='" & strhotelCode & "') and ctrycode in (" & strSelectedCountriesListMod & ")"
                Dim strCountryCount As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQuery)


                strQuery1 = "select count(AgentCode)cnt from ApplyMarkup_Agents where ApplyMarkupId in (select distinct ApplyMarkupId from Markup_Hotels where  ((convert(datetime,MarkupFromDate,101)  between convert(datetime,'" & txtFromDate.Text & "',103) and   convert(datetime,'" & txtToDate.Text & "',103)) or  ( convert(datetime,MarkupToDate,101)  between convert(datetime,'" & txtFromDate.Text & "',103)and convert(datetime,'" & txtToDate.Text & "',103)) or (convert(datetime,'" & txtFromDate.Text & "',103) between convert(datetime,MarkupFromDate,101)  and convert(datetime,MarkupToDate,101)) or (convert(datetime,'" & txtToDate.Text & "',103) between convert(datetime,MarkupFromDate,101)  and convert(datetime,MarkupToDate,101)))  and PartyCode='" & strhotelCode & "') and AgentCode in (" & strSelectedAgentListMod & ")"
                Dim strAgentCount As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQuery1)

                If strCountryCount > 0 Or strAgentCount > 0 Then
                    gvRow.BackColor = Drawing.Color.SkyBlue
                    Return False
                End If

            End If
        Next
        Return True
    End Function

    Protected Sub btnCheckExist_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckExist.Click
        Try


            If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
                If gv_SearchResult.Rows.Count > 0 Then
                    'Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSSelectAll"), CheckBox)
                    'Dim row As GridViewRow

                    'For Each row In gv_SearchResult.Rows

                    '    Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSSelect"), CheckBox)
                    '    Dim lblHotelCode As Label = CType(row.FindControl("lblHotelCode"), Label)
                    '    Dim lblHotelName As Label = CType(row.FindControl("lblHotelName"), Label)
                    '    If ChkBoxRows.Checked = True Then

                    '        If fnValidateDuplication(lblHotelCode.Text) = False Then
                    '            row.BackColor = Drawing.Color.SkyBlue
                    '        End If
                    '    Else
                    '        row.BackColor = Drawing.Color.Empty
                    '    End If

                    'Next

                    fnShowExistingAppliedMarkups()

                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any item for checking.');", True)
                    Exit Sub
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select New or Edit option for checking.');", True)
                Exit Sub
            End If

        Catch ex As Exception
            ModalPopupDays.Hide()  '' Added shahul 13/06/18
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ApplyMarkups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamic") = dtt
            End If
        End If
        Return True
    End Function
    Private Sub FillGridNewsearch()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCurrencyValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "INVENTORYTYPE" Then
                        If strCountryValue <> "" Then
                            strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "FORMULA-ID" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "ROOMCLASSIFICATION" Then
                        If strCityValue <> "" Then
                            strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If

                    If dtt.Rows(i)("Code").ToString = "TEXT" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If


                Next
            End If
            'Dim pagevaluecs = RowsPerPageCS.SelectedValue
            '  strBindCondition = BuildConditionNew(strCountryValue, strCityValue, strCurrencyValue, strTextValue)
            'Dim myDS As New DataSet
            'Dim strValue As String
            'gv_SearchResult.Visible = True
            'lblMsg.Visible = False
            'If gv_SearchResult.PageIndex < 0 Then

            '    gv_SearchResult.PageIndex = 0
            'End If
            'strSqlQry = "select citycode,cityname,ctryname,rankorder,showweb,citymast.active,citymast.adddate,citymast.adduser,citymast.moddate,citymast.moduser  ,case when isnull(dbo.ctrymast.active,0)=1 then 'Active'   when isnull(dbo.ctrymast.active,0)=0 then 'InActive' else 'InActive' end isactive from citymast inner join ctrymast on citymast.ctrycode=ctrymast.ctrycode"
            'Dim strorderby As String = Session("strsortexpression")
            'Dim strsortorder As String = "ASC"

            'If strBindCondition <> "" Then
            '    strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If
            'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            'myDataAdapter.Fill(myDS)
            ''Session("SSqlQuery") = strSqlQry
            ''myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            'gv_SearchResult.DataSource = myDS
            'If myDS.Tables(0).Rows.Count > 0 Then
            '    gv_SearchResult.PageSize = pagevaluecs
            '    gv_SearchResult.DataBind()
            'Else
            '    gv_SearchResult.PageIndex = 0
            '    gv_SearchResult.DataBind()
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Records not found, Please redefine search criteria."
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Protected Sub lbClose2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicSearch")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicSearch") = dtDynamics
            dlList2.DataSource = dtDynamics
            dlList2.DataBind()

            bindsearchgrid(False)
            'FillGridNewsearch()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Protected Sub lbCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtHotelDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtHotelDynamic") = dtDynamic
        dlList1.DataSource = dtDynamic
        dlList1.DataBind()

        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Copy"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        hdOPMode.Value = "N"
        EnableControls()
        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
    End Sub

    Protected Sub lbPcopy_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtHotelDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtHotelDynamic") = dtDynamic
        dlList1.DataSource = dtDynamic
        dlList1.DataBind()

        Dim lbView As LinkButton = CType(sender, LinkButton)
        Dim lblApplyMarkupId As Label = CType(lbView.FindControl("lblApplyMarkupId"), Label)
        hdApplyMarkupId.Value = lblApplyMarkupId.Text.Trim
        Session("ApplyMarkupState") = "Copy"
        dvNew.Visible = True
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        FillMarkupDetails(lblApplyMarkupId.Text)
        hdOPMode.Value = "N"
        EnableControls()
        btnSelectMarkup.Visible = True
        txtMarkupFormulaName.Visible = True
        txtMarkupFormula.Visible = True
        txtApplicableTo.Visible = True
        Label15.Visible = True
        Label16.Visible = True
        Label6.Visible = True
        chkActive.Visible = True
    End Sub
    ''' <summary>
    ''' btnhidden_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnhidden_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdMissing.Value = ""
    End Sub
    ''' <summary>
    ''' lbViewMarkup_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbViewMarkup_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim imgbRoomType As LinkButton = CType(sender, LinkButton)
        Dim lblHotelCode As Label = CType(imgbRoomType.FindControl("lblHotelCode"), Label)

        Dim dtHotelDynamic As New DataTable
        dtHotelDynamic = Session("sDtHotelDynamic")
        Dim strRC As String = ""
        If dtHotelDynamic.Rows.Count > 0 Then
            For i As Integer = 0 To dtHotelDynamic.Rows.Count - 1
                If dtHotelDynamic.Rows(i)("Code").ToString() = "RC" Then
                    If strRC = "" Then
                        strRC = "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    Else
                        strRC = strRC & "," & "'" & dtHotelDynamic.Rows(i)("Country").ToString().Replace("(RC)", "") & "'"
                    End If
                End If
            Next
        End If

        Dim dt As New DataTable
        Dim strSqlQry As String = ""
        If strRC = "" Then
            strSqlQry = "SELECT distinct convert(varchar(10), MarkupFromDate,103)MarkupFromDate,convert(varchar(10), MarkupToDate,103)MarkupToDate,(select distinct  r.roomclassname  from room_classification r where r.roomclasscode=Markup_Hotels.roomclasscode)RoomClassName,FormulaId,FormulaString,ApplicableTo,Markup_Hotels.RoomClassCode,InventoryTypes,DaysOfTheWeek  FROM Markup_Hotels where  partycode='" + lblHotelCode.Text + "'  order by RoomClassCode,MarkupFromDate asc ,MarkupToDate desc"
        Else
            strSqlQry = "SELECT distinct convert(varchar(10), MarkupFromDate,103)MarkupFromDate,convert(varchar(10), MarkupToDate,103)MarkupToDate,(select distinct  r.roomclassname  from room_classification r where r.roomclasscode=Markup_Hotels.roomclasscode)RoomClassName,FormulaId,FormulaString,ApplicableTo,Markup_Hotels.RoomClassCode,InventoryTypes,DaysOfTheWeek  FROM Markup_Hotels where  partycode='" + lblHotelCode.Text + "' and Markup_Hotels.roomclasscode in (select distinct roomclasscode from room_classification where roomclassname in (" & strRC & ")) order by RoomClassCode,MarkupFromDate asc ,MarkupToDate desc"
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
        myDataAdapter.Fill(dt)
        gvViewMarkup.DataBind()
        gvViewMarkup.DataSource = dt

        If dt.Rows.Count > 0 Then
            gvViewMarkup.DataBind()

        Else
            gvViewMarkup.PageIndex = 0
            gvViewMarkup.DataBind()
        End If
        ModalViewMarkup.Show()
    End Sub

    Protected Sub gvMarkupFormulas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMarkupFormulas.PageIndexChanging
        gvMarkupFormulas.PageIndex = e.NewPageIndex
        hdMarkUp.Value = "Y"
        FillMarkups()
        ModalSelectMarkup.Show()

    End Sub

    Protected Sub gvLog_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLog.PageIndexChanging
        gvLog.PageIndex = e.NewPageIndex
        BindLogs()
    End Sub

    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        'dvSearch.Visible = True
        'dvNew.Visible = False
        'dvLog.Visible = False
        'dvPendingApproval.Visible = False
        ''bindsearchgrid(False)
        '' BindLogs()


        dvSearch.Visible = True
        dvNew.Visible = False
        dvLog.Visible = False
        dvPendingApproval.Visible = False
        gvLog.Visible = False
        gvSearch.Visible = True
        'bindsearchgrid(False)
        ' BindLogs()

    End Sub

    Protected Sub btnvsprocess2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess2.Click
        FilterGridSEARCH()
    End Sub
#Region "Public Function checkForvalidate() As Boolean"
    Public Function checkForvalidate() As Boolean
        'If ViewState("CitiesState") = "New" Then
        If txtFromDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select From Date.');", True)
            checkForvalidate = False
            Exit Function
        End If
        If txtToDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select To Date.');", True)
            checkForvalidate = False
            Exit Function
        End If

        'End If
        checkForvalidate = True
    End Function
#End Region
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Session("export") = Nothing
        lblMsg.Visible = False
        If checkForvalidate() = False Then
            Exit Sub
        End If
        bindsearchgrid(False)

    End Sub



    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
    End Sub


    Protected Sub gvSearch_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearch.PageIndexChanging
        gvSearch.PageIndex = e.NewPageIndex
        bindsearchgrid(False)
    End Sub

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCS.SelectedIndexChanged
        bindsearchgrid(False)
    End Sub

    Protected Sub gvRoomType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRoomType.PageIndexChanging
        gvRoomType.PageIndex = e.NewPageIndex

        Fillroomtypesgrid(hdnhotelcode.Value)
        ModalRoomType.Show()

    End Sub

    Private Sub fnShowExistingAppliedMarkups()
        Dim strSelectedCountriesList As String = ""
        strSelectedCountriesList = ucMarkup.checkcountrylist.ToString

        Dim strSelectedAgentList As String = ""
        strSelectedAgentList = ucMarkup.checkagentlist.ToString()
        If hdMissing.Value <> "1" Then
            If strSelectedCountriesList = "" And strSelectedAgentList = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any country or agent.');", True)
                Exit Sub
            End If


            If checkForMissingDate() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter markup dates.');", True)
                Exit Sub
            End If
            If checkForDateOverlap() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Inavlid date range.');", True)
                Exit Sub
            End If

            If checkForDayOfWeek() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any day of week.');", True)
                Exit Sub
            End If
            If checkForInventoryType() = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select any Inventory Type.');", True)
                Exit Sub
            End If
            If txtMarkupFormula.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select any markup formula.');", True)
                Exit Sub
            End If
        End If
        Dim dtsHotelGroupDetails As DataTable
        dtsHotelGroupDetails = Session("sDtHotelGroupDetails")
        If dtsHotelGroupDetails.Rows.Count <= 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The hotels are not selected.');", True)
            Exit Sub
        End If

        Dim dt As New DataTable
        Dim strSeachCriteria As String = ""
        dt = Session("sDtHotelDynamic")
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                If strSeachCriteria = "" Then
                    strSeachCriteria = dt.Rows(i)("Country").ToString
                Else
                    strSeachCriteria = strSeachCriteria & "," & dt.Rows(i)("Country").ToString
                End If
            Next
        End If


        Dim frmmode As Integer
        Dim lastno As String
        If hdOPMode.Value = "N" Then
            frmmode = 1

        ElseIf hdOPMode.Value = "E" Then
            frmmode = 2
        End If

        Dim strBufferDates As New System.Text.StringBuilder
        Dim mRow As Integer = 0
        strBufferDates.Append("<ApplyMarkup_Dates>")
        For Each row As GridViewRow In gvExhibitionAndHolidayInput.Rows

            Dim txtFrom As TextBox = CType(row.FindControl("txtExhibitionAndHolidayfromDate"), TextBox)
            Dim txtTo As TextBox = CType(row.FindControl("txtExhibitionAndHolidayToDate"), TextBox)
            If txtFrom.Text <> "" And txtTo.Text <> "" Then
                mRow = mRow + 1
                strBufferDates.Append("<ApplyMarkup_Date>")
                strBufferDates.Append(" <LineNo>" & mRow.ToString & " </LineNo>")
                strBufferDates.Append(" <FromDate>" & txtFrom.Text.ToString & " </FromDate>")
                strBufferDates.Append(" <ToDate>" & txtTo.Text.ToString & " </ToDate>")
                strBufferDates.Append("</ApplyMarkup_Date>")
            End If
        Next
        strBufferDates.Append("</ApplyMarkup_Dates>")

        Dim dtHotelDynamic As DataTable
        dtHotelDynamic = Session("sDtHotelDynamic")
        Dim strRoomClassName As String = ""
        If dtHotelDynamic.Rows.Count > 0 Then
            For i As Integer = 0 To dtHotelDynamic.Rows.Count - 1
                If dtHotelDynamic.Rows(i)("Code").ToString = "RC" Then
                    If strRoomClassName = "" Then
                        strRoomClassName = dtHotelDynamic.Rows(i)("Country").ToString.Replace("(RC)", "")
                    Else
                        strRoomClassName = strRoomClassName & "," & dtHotelDynamic.Rows(i)("Country").ToString.Replace("(RC)", "")
                    End If
                End If
            Next
        End If



        Dim strBuffer As New System.Text.StringBuilder
        If dtsHotelGroupDetails.Rows.Count > 0 Then

            strBuffer.Append("<ApplyMarkup_Hotels>")
            For i = 0 To dtsHotelGroupDetails.Rows.Count - 1
                strBuffer.Append("<ApplyMarkup_Hotel>")
                strBuffer.Append(" <PartyCode>" & dtsHotelGroupDetails.Rows(i)("Code").ToString & " </PartyCode>")
                strBuffer.Append("</ApplyMarkup_Hotel>")
            Next
            strBuffer.Append("</ApplyMarkup_Hotels>")
        End If
        Dim strDaysOfTheWeek As String = ""
        Dim chkWSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkWSelectAll"), CheckBox)
        If chkWSelectAll.Checked = True Then
            strDaysOfTheWeek = "ALL"
        Else
            For Each gvRow As GridViewRow In gvWeekOfDays.Rows
                Dim chkWSelect As CheckBox = CType(gvRow.FindControl("chkWSelect"), CheckBox)
                If chkWSelect.Checked = True Then
                    Dim lblWeekDaysCode As Label = CType(gvRow.FindControl("lblWeekDaysCode"), Label)
                    If strDaysOfTheWeek = "" Then
                        strDaysOfTheWeek = lblWeekDaysCode.Text
                    Else
                        strDaysOfTheWeek = strDaysOfTheWeek & "," & lblWeekDaysCode.Text
                    End If
                End If
            Next
        End If


        Dim strInventoryTypes As String = ""
        ' Dim chkInvSelectAll As CheckBox = CType(gvWeekOfDays.HeaderRow.FindControl("chkInvSelectAll"), CheckBox)
        'If chkWSelectAll.Checked = True Then
        '    strInventoryTypes = "ALL"
        'Else
        For Each gvRow As GridViewRow In gvInventoryType.Rows
            Dim chkInvSelect As CheckBox = CType(gvRow.FindControl("chkInvSelect"), CheckBox)
            If chkInvSelect.Checked = True Then
                Dim lblInventoryType As Label = CType(gvRow.FindControl("lblInventoryType"), Label)
                If strInventoryTypes = "" Then
                    strInventoryTypes = lblInventoryType.Text
                Else
                    strInventoryTypes = strInventoryTypes & "," & lblInventoryType.Text
                End If
            End If
        Next
        ' End If


        '  If hdMissing.Value <> "1" Then



        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        mySqlCmd = New SqlCommand()
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.CommandText = "sp_ApplyMarkup_Validate"
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMrkpId", SqlDbType.VarChar, 20)).Value = hdApplyMarkupId.Value
        mySqlCmd.Parameters.Add(New SqlParameter("@DaysOfTheWeek", SqlDbType.VarChar, 100)).Value = strDaysOfTheWeek
        mySqlCmd.Parameters.Add(New SqlParameter("@InventoryTypes", SqlDbType.VarChar, 5000)).Value = strInventoryTypes
        mySqlCmd.Parameters.Add(New SqlParameter("@FormulaId", SqlDbType.VarChar, 20)).Value = txtmarkupCode.Text
        mySqlCmd.Parameters.Add(New SqlParameter("@FormulaName", SqlDbType.VarChar, 20)).Value = txtMarkupFormulaName.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplicableTo", SqlDbType.VarChar, 5000)).Value = txtApplicableTo.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@SearchCriteria", SqlDbType.VarChar, 1000)).Value = strSeachCriteria
        If chkActive.Checked = True Then
            mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 1
        ElseIf chkActive.Checked = False Then
            mySqlCmd.Parameters.Add(New SqlParameter("@Approved", SqlDbType.Int)).Value = 0
        End If
        mySqlCmd.Parameters.Add(New SqlParameter("@Userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Hotels_XML", SqlDbType.Xml)).Value = strBuffer.ToString
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Countries", SqlDbType.VarChar, 8000)).Value = strSelectedCountriesList
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Agents", SqlDbType.VarChar, 8000)).Value = strSelectedAgentList
        mySqlCmd.Parameters.Add(New SqlParameter("@ApplyMarkup_Dates_XML", SqlDbType.Xml)).Value = strBufferDates.ToString
        mySqlCmd.Parameters.Add(New SqlParameter("@RoomClassName", SqlDbType.VarChar, 500)).Value = strRoomClassName
        mySqlCmd.Parameters.Add(New SqlParameter("@OPMode", SqlDbType.Int)).Value = frmmode
        myDataAdapter = New SqlDataAdapter()
        mySqlCmd.Connection = mySqlConn
        myDataAdapter.SelectCommand = mySqlCmd
        Dim dsValidate As New DataSet
        myDataAdapter.Fill(dsValidate)

        If dsValidate.Tables(0).Rows.Count > 0 Then
            If dsValidate.Tables(0).Rows.Count > 0 Then
                gvShowExistingAppliedMarkup.DataSource = dsValidate.Tables(0)
                gvShowExistingAppliedMarkup.DataBind()
                mpShowExistAppliedMarkup.Show()
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No duplication found.');", True)
            Exit Sub
        End If
    End Sub

End Class
