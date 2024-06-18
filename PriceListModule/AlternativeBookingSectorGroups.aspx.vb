


#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class AlternativeBookingSectorGroups
    Inherits System.Web.UI.Page


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
        '
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                ViewState.Add("CitiesState", Request.QueryString("State"))
                ViewState.Add("CitiesRefCode", Request.QueryString("RefCode"))
                Session.Add("trfgroupcode", objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1001'"))
                ' Session.Remove("sectgrps_citycode")

                ' txtGroupCode.Attributes.Add("onkeydown", "fnReadOnly(event)")
                txtGroupCode.Enabled = False
                txtGroupTagName.Visible = False
                TxtCityName.Visible = False
                lblsectgrp.Visible = False
                lblcity0.Visible = False
                hdOPMode.Value = "S"
                hdFillByGrid.Value = "S"
                Session.Add("hdOPMode", hdOPMode.Value)
                hdLinkButtonValue.Value = ""
                btnSave.Visible = False
                txtGroupTagName.Visible = False
                lblcity0.Visible = False
                lblsectgrp.Visible = False
                ' lblsectgroup.Visible = False
                btnNew.Enabled = True
                txtName.ReadOnly = True
                ' txtGroupCode.Enabled = False
                txtGroupTagName.ReadOnly = True
                'txtGroupCode.ReadOnly = True
                btnEdit.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
                trNameAndCode.Visible = False
                '   lblcity.Visible = False
                TxtCityName.Visible = False
                btnSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                If hdOPMode.Value = "N" Then
                    lblHeading.Text = "Add Alternative Booking Sector Groups"
                    Page.Title = Page.Title + " " + "New "
                    btnSave.Text = "Save"
                    TxtCityName.ReadOnly = False

                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf hdOPMode.Value = "E" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Sector Group"
                    Page.Title = Page.Title + " " + "Edit Alternative Booking Sector Group"
                    btnSave.Text = "Update"

                    TxtCityName.ReadOnly = True

                ElseIf hdOPMode.Value = "D" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Alternative Booking Sector Group"
                    Page.Title = Page.Title + " " + "Delete Sector Group"
                    btnSave.Text = "Delete"
                    TxtCityName.Enabled = False
                    TxtCityName.ReadOnly = True


                ElseIf hdOPMode.Value = "S" Then
                    TxtCityName.ReadOnly = False

                    lblHeading.Text = "Alternative Booking Sector Groups"
                    Page.Title = Page.Title + " " + "SectorGroups"
                    btnSave.Text = "Save"
                    'TxtCityName.ReadOnly = True
                End If


                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckNewFormatUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                CType(strappname, String), "PriceListModule\AlternativeBookingSectorGroups.aspx?appid=" + strappid, btnNew, btnEdit, _
                                                btnDelete, btnView, btnExcel, btnPrint)

                End If

                btnPrint.Visible = False


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ' Create a Dynamic datatable ---- Start
                    Dim dtDynamic = New DataTable()
                    Dim dcCode = New DataColumn("Code", GetType(String))
                    Dim dcCountry = New DataColumn("Sector", GetType(String))
                    dtDynamic.Columns.Add(dcCode)
                    dtDynamic.Columns.Add(dcCountry)
                    Session("sDtDynamic") = dtDynamic
                    ' --------end

                    ' Create a Dynamic datatable ---- Start
                    Session("sDtAlternateDynamicSearch") = Nothing
                    Dim dtDynamicSearch = New DataTable()
                    Dim dcSearchCode = New DataColumn("Code", GetType(String))
                    Dim dcSearchValue = New DataColumn("Value", GetType(String))
                    Dim dcSearchCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                    dtDynamicSearch.Columns.Add(dcSearchCode)
                    dtDynamicSearch.Columns.Add(dcSearchValue)
                    dtDynamicSearch.Columns.Add(dcSearchCodeAndValue)
                    Session("sDtAlternateDynamicSearch") = dtDynamicSearch


                    ' Create a Dynamic datatable ---- Start
                    Dim dtSectorGroupDetails = New DataTable()
                    Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
                    Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
                    Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
                    Dim dcGroupDetailssector = New DataColumn("SECGRP", GetType(String))
                    dtSectorGroupDetails.Columns.Add(dcGroupDetailsType)
                    dtSectorGroupDetails.Columns.Add(dcGroupDetailsTypeName)
                    dtSectorGroupDetails.Columns.Add(dcGroupDetailsCode)
                    dtSectorGroupDetails.Columns.Add(dcGroupDetailssector)
                    Session("sDtSectorGroupDetails") = dtSectorGroupDetails

                    Session("strsortexpression") = "sectormaster.sectorgroupcode"
                    FillGridNew()
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Session("strsortexpression") = "sectormaster.sectorgroupcode"
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SuppliersWindowPostBack") Then
            FilterGrid()
        End If
        Page.Title = "Alternative Booking Sector Group"

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
                    Case 10 'changed by mohamed on 31/05/2018
                        Me.MasterPageFile = "~/TransferMaster.master"
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


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitynames(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Citynames As New List(Of String)
        Try



            strSqlQry = "select cityname,citycode from citymast where active=1  and  cityname like  " & "'" & prefixText & "%'  order by cityname"


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Citynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Citynames
        Catch ex As Exception
            Return Citynames
        End Try
    End Function

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Flight Class Master") Pageame=hotelstatus&BackPageName=HotelStatusSearch.aspx&HotelstatusCode=" + txtGroupCode.Text.Trim + "&HotelstatusName=" + txtName.Text.Trim + "','rpthotelstatus');"
            'Session.Add("BackPageName", "FlightClassMasterSearch.aspx")
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=SectorGroup&BackPageName=ExcSectorGroups.aspx&.othtypcode=" + txtGroupCode.Text.Trim + "&othtypname=" + txtGroupTagName.Text.Trim + "','rptSectorgroup');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "exctypes_sectorgrp", "sectorgrpcode", CType(txtGroupCode.Text.Trim("SG", ""), String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Sector Group is already used in excursions, cannot delete this SectorGroup');", True)
            checkForDeletion = False
            Exit Function
        End If



        checkForDeletion = True
    End Function
#End Region
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strBuffer As New Text.StringBuilder
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"

        Try

            If Page.IsValid = True Then

                Dim dtsSectorGroupDetails As DataTable
                dtsSectorGroupDetails = Session("sDtSectorGroupDetails")

                If dtsSectorGroupDetails.Rows.Count > 0 Then

                    strBuffer.Append("<SectorGroups>")
                    For i = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        strBuffer.Append("<SectorGroup>")
                        strBuffer.Append(" <SectorCode>" & dtsSectorGroupDetails.Rows(i)("Code").ToString & " </SectorCode>")
                        strBuffer.Append(" <SectorGrpCode>" & txtGroupCode.Text.Trim & "</SectorGrpCode>")
                        strBuffer.Append("</SectorGroup>")
                    Next
                    strBuffer.Append("</SectorGroups>")
                End If


                If hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then


                    If dtsSectorGroupDetails.Rows.Count <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The Sectors are not selected.');", True)
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter  Group name.');", True)
                        Exit Sub
                    End If




                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If hdOPMode.Value = "N" Then
                        mySqlCmd = New SqlCommand("sp_add_alternative_booking_sectorgrps", mySqlConn, sqlTrans)

                        lastno = objUtils.ExecuteQueryReturnSingleValuenew(CType(Session("dbconnectionName"), String), "select lastno from docgen where optionname='ALTSECGRP'")
                        optionval = objUtils.GetAutoDocNo("ALTSECGRP", mySqlConn, sqlTrans)
                        txtGroupCode.Text = optionval.Trim

                    ElseIf hdOPMode.Value = "E" Then
                        mySqlCmd = New SqlCommand("sp_add_alternative_booking_sectorgrps", mySqlConn, sqlTrans)

                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgrpname", SqlDbType.VarChar, 150)).Value = CType(txtGroupTagName.Text.Replace("(SG)", ""), String)
           
                    mySqlCmd.Parameters.Add(New SqlParameter("@ValidXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                ElseIf hdOPMode.Value = "D" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    If txtGroupTagName.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
                        txtGroupTagName.Focus()
                        Exit Sub
                    End If

                    strBuffer = New StringBuilder("")

                    strBuffer.Append("<SectorGroups>")
                    strBuffer.Append("</SectorGroups>")

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_add_alternative_booking_sectorgrps", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtGroupCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgrpname", SqlDbType.VarChar, 150)).Value = CType(txtGroupTagName.Text.Replace("(SG)", ""), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@ValidXMLInput", SqlDbType.Xml)).Value = strBuffer.ToString
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

                If hdOPMode.Value = "N" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector Group Created Successfully..');", True)
                ElseIf hdOPMode.Value = "E" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector Group Modified Successfully..');", True)
                ElseIf hdOPMode.Value = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sector Group  Deleted Successfully..');", True)
                End If
                ClearFields()
                hdOPMode.Value = "S"

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

                gvsearchgrid.Visible = True
                gv_SearchResult.Visible = False
                dlList.Visible = False
                dlListSearch.Visible = True
                trNameAndCode.Visible = False
                lblHeading.Text = "Alternative Booking Sector Group"

                FillGridNew()
                btnSave.Visible = False

                TxtCityName.Visible = False
                txtGroupTagName.Visible = False
                lblsectgrp.Visible = False
                lblcity0.Visible = False
                TxtCityName.Text = ""

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("ExcSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub




#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("CitiesSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CntryGrpWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region


#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If hdOPMode.Value = "N" Then

            If objUtils.isDuplicatenew(Session("dbconnectionName"), "AlternativeBookingSectorGroup", "sectorgroupname", ((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(SG)", "")).Replace("(G)", "")) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf hdOPMode.Value = "E" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "AlternativeBookingSectorGroup", "sectorgroupcode", "sectorgroupname", CType(((Trim(txtGroupTagName.Text.Trim).Replace("(C)", "")).Replace("(SG)", "")).Replace("(HG)", ""), String), txtGroupCode.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    '#Region "Public Function checkForDeletion() As Boolean"
    '    Public Function checkForDeletion() As Boolean
    '    End Function
    '#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcSector','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    ''' <summary>
    ''' chkSelectAll_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        Dim dtsSectorGroupDetails As DataTable
        dtsSectorGroupDetails = Session("sDtSectorGroupDetails")

        For Each row In gv_SearchResult.Rows
            Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)
            Dim lblSectorCode As Label = CType(row.FindControl("lblSectorCode"), Label)
            Dim lblSectorName As Label = CType(row.FindControl("lblSectorName"), Label)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
                iFlag = 0
                If dtsSectorGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        If dtsSectorGroupDetails.Rows(i)("Code").ToString = lblSectorCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If TxtCityName.Text <> "" Then
                        txtNameNew.Text = TxtCityName.Text + "(CT)"
                    End If
                    If iFlag = 0 Then
                        dtsSectorGroupDetails.NewRow()

                        If txtNameNew.Text.Contains("(S)") Then
                            dtsSectorGroupDetails.Rows.Add("S", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTY)") Then
                            dtsSectorGroupDetails.Rows.Add("CTY", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsSectorGroupDetails.Rows.Add("CT", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(SG)") Then
                            dtsSectorGroupDetails.Rows.Add("SG", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsSectorGroupDetails.Rows.Add("T", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        End If

                        Session("sDtSectorGroupDetails") = dtsSectorGroupDetails

                    End If

                End If

            Else
                ChkBoxRows.Checked = False
                If dtsSectorGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        If dtsSectorGroupDetails.Rows(i)("Code").ToString = lblSectorCode.Text Then
                            dtsSectorGroupDetails.Rows.Remove(dtsSectorGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtSectorGroupDetails") = dtsSectorGroupDetails
            End If

            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsSectorGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Sector").ToString = "SECTOR" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("SE", "SECTOR")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()


        Next
    End Sub

    ''' <summary>
    ''' btnNew_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        btnCancel.Attributes.Add("class", "btnExampleHold")
        hdOPMode.Value = "N"
        hdFillByGrid.Value = "N"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        lblsectgrp.Visible = True
        btnCancel.Enabled = True
        txtName.ReadOnly = False
        txtGroupTagName.ReadOnly = False
        'txtGroupCode.ReadOnly = False
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        Session.Add("hdOPMode", hdOPMode.Value)
        lblsectgrp.Visible = True
        txtGroupTagName.Visible = True
        lblcity0.Visible = True
        ' lblsectgroup.Visible = True
        txtcityName.Visible = True
        'txtGroupCode.Enabled = False
        txtGroupTagName.Focus()
        lblHeading.Text = "Add Alternative Booking Sector Group"
        btnSave.Text = "Save"
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


        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True

        trNameAndCode.Visible = True

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
        txtName.ReadOnly = False
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""

        hdOPMode.Value = "E"
        Session.Add("hdOPMode", hdOPMode.Value)
        txtGroupTagName.ReadOnly = False
        txtGroupTagName.Focus()
        btnSave.Text = "Update"
        lblHeading.Text = "Edit Alternative Booking Sector Groups"
        TxtCityName.Visible = True
        txtGroupTagName.Visible = True
        lblsectgrp.Visible = True
        lblcity0.Visible = True
        chkActive.Visible = True

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

        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True
        trNameAndCode.Visible = True
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
        txtGroupTagName.ReadOnly = False
        lblsectgrp.Visible = True
        txtGroupTagName.Visible = True
        lblcity0.Visible = True
        TxtCityName.Visible = True
        btnSave.Text = "Delete"
        lblHeading.Text = "Delete Alternative Booking Sector Group"
        txtGroupTagName.Focus()

        TxtCityName.ReadOnly = True

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

        gvSearchGrid.Visible = False
        dlListSearch.Visible = False

        gv_SearchResult.Visible = True
        dlList.Visible = True
        btnSave.Visible = True
        trNameAndCode.Visible = True
    End Sub
    ''' <summary>
    ''' btnCancel_new_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancel_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel_new.Click
        hdOPMode.Value = "S"
        txtName.ReadOnly = True
        txtGroupTagName.ReadOnly = True
        'txtGroupCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtGroupCode.Text = ""
        Dim dtsSectorGroupDetails As DataTable
        dtsSectorGroupDetails = Session("sDtSectorGroupDetails")
        dtsSectorGroupDetails.Rows.Clear()
        Session("sDtSectorGroupDetails") = dtsSectorGroupDetails
        gv_SearchResult.DataSource = dtsSectorGroupDetails
        gv_SearchResult.DataBind()

        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtDynamic") = dtDynamic
        dlList.DataSource = dtDynamic
        dlList.DataBind()
        ClearFields()
        txtName.Enabled = True
        gv_SearchResult.Enabled = True
        lblHeading.Text = "Alternative Booking Sector Group"
        btnSave.Visible = True

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

        gvSearchGrid.Visible = True
        dlListSearch.Visible = True

        gv_SearchResult.Visible = False
        dlList.Visible = False

        Dim dtt As DataTable
        dtt = Session("sDtAlternateDynamicSearch")
        dtt.Rows.Clear()
        dlListSearch.DataSource = dtt
        dlListSearch.DataBind()
        Session("sDtAlternateDynamicSearch") = dtt
        FillGridNew()
        btnSave.Visible = False

        trNameAndCode.Visible = False
        txtGroupTagName.ReadOnly = False
        txtGroupTagName.Enabled = True
        txtGroupTagName.Visible = False
        lblsectgrp.Visible = False
        TxtCityName.Visible = False
        lblcity0.Visible = False
    End Sub



    ''' <summary>
    ''' lbCountry_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdFillByGrid.Value = "Y"
        hdLinkButtonValue.Value = ""
        Dim strlbValue As String = ""

        Dim myButton As LinkButton = CType(sender, LinkButton)

        Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
        Dim lb As Label = CType(dlItem.FindControl("lblType"), Label)

        If Not myButton Is Nothing Then
            strlbValue = myButton.Text
            If strlbValue = "Sector" Then
                strlbValue = "%"
            End If

            hdLinkButtonValue.Value = strlbValue
            Try
                FillGridByLinkButton()
                FillCheckbox()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            Finally


            End Try

        End If



    End Sub
    ''' <summary>
    ''' gv_SearchResult_PageIndexChanging
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        If hdFillByGrid.Value = "Y" Then
            FillGridByLinkButton()
            FillCheckbox()
        Else
            FilterGrid()
            FillCheckbox()
        End If

    End Sub
    ''' <summary>
    ''' FillCheckbox
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillCheckbox()
        Dim dtsSectorGroupDetails As DataTable
        dtsSectorGroupDetails = Session("sDtSectorGroupDetails")

        Dim row As GridViewRow
        Dim iFlag As Integer = 0
        If dtsSectorGroupDetails.Rows.Count > 0 Then

            For Each row In gv_SearchResult.Rows

                Dim ChkBoxRows As CheckBox = CType(row.FindControl("chkSelect"), CheckBox)

                Dim lblSectorCode As Label = CType(row.FindControl("lblSectorCode"), Label)
                Dim lblSectorName As Label = CType(row.FindControl("lblSectorName"), Label)


                For i As Integer = 0 To dtsSectorGroupDetails.Rows.Count - 1
                    If dtsSectorGroupDetails.Rows(i)("Code").ToString.Trim = lblSectorCode.Text.Trim Then
                        ChkBoxRows.Checked = True
                        Exit For
                    Else
                        ChkBoxRows.Checked = False
                    End If

                Next
            Next

        End If
    End Sub
    ''' <summary>
    ''' chkSelect_CheckedChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub chkSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim ChkBoxRows As CheckBox = CType(sender, CheckBox)
            Dim lblSectorCode As Label = CType(ChkBoxRows.FindControl("lblSectorCode"), Label)
            Dim lblSectorName As Label = CType(ChkBoxRows.FindControl("lblSectorName"), Label)
            Dim row As GridViewRow
            Dim iFlag As Integer = 0
            Dim iFlagCheckedAll As Integer = 0
            Dim iFlagUnCheckedAll As Integer = 0
            Dim dtsSectorGroupDetails As DataTable
            dtsSectorGroupDetails = Session("sDtSectorGroupDetails")


            If ChkBoxRows.Checked = True Then
                ChkBoxRows.Checked = True



                If dtsSectorGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        If dtsSectorGroupDetails.Rows(i)("Code").ToString = lblSectorCode.Text Then
                            iFlag = 1
                        End If
                    Next
                    If TxtCityName.Text <> "" Then
                        txtNameNew.Text = TxtCityName.Text + "(CT)"
                    End If
                    If iFlag = 0 Then

                        dtsSectorGroupDetails.NewRow()
                        If txtNameNew.Text.Contains("(S)") Then
                            dtsSectorGroupDetails.Rows.Add("S", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(CTY)") Then
                            dtsSectorGroupDetails.Rows.Add("CTY", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(CT)") Then
                            dtsSectorGroupDetails.Rows.Add("CT", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(SG)") Then
                            dtsSectorGroupDetails.Rows.Add("SG", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        ElseIf txtNameNew.Text.Contains("(T)") Then
                            dtsSectorGroupDetails.Rows.Add("T", txtNameNew.Text, lblSectorCode.Text, lblSectorName.Text)
                        End If


                        Session("dtsSectorGroupDetails") = dtsSectorGroupDetails


                    End If

                End If

            Else

                ChkBoxRows.Checked = False
                If dtsSectorGroupDetails.Rows.Count >= 0 Then
                    For i = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        If dtsSectorGroupDetails.Rows(i)("Code").Trim.ToString = lblSectorCode.Text.Trim Then
                            dtsSectorGroupDetails.Rows.Remove(dtsSectorGroupDetails.Rows(i))
                            Exit For
                        End If
                    Next

                End If
                Session("sDtSectorGroupDetails") = dtsSectorGroupDetails
            End If


            Dim ChkBoxHeader As CheckBox = CType(gv_SearchResult.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            Dim row1 As GridViewRow
            For Each row1 In gv_SearchResult.Rows
                Dim ChkBoxRows1 As CheckBox = CType(row1.FindControl("chkSelect"), CheckBox)
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
            dtt = Session("sDtDynamic")
            If dtt.Rows.Count >= 0 Then

                If dtsSectorGroupDetails.Rows.Count > 0 Then

                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Sector").ToString = "SECTOR" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("SE", "SECTOR")
                        Session("sDtDynamic") = dtt
                    End If

                End If
            End If
            Session("sDtDynamic") = dtt
            dlList.DataSource = dtt
            dlList.DataBind()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    ''' <summary>
    ''' FillGridByLinkButton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillGridByLinkButton()
        Dim strorderby As String = "sectorname"
        Dim strsortorder As String = "ASC"
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True

        Dim strlbValue As String = hdLinkButtonValue.Value
        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""

        strSqlQry = "with ctee as( SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'') as othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1,0 sortorder FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode   and sectormaster.citycode='" & TxtCityCode.Text & "' )select * from ctee where othtypname ='' "


        If strlbValue.Trim <> "" Then

            If strlbValue.Contains("(CTY)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"

                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                End If
            End If


            If strlbValue.Contains("(S)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(sectorname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(sectorname) = " & Trim(strlbValue) & ""
                End If
            End If

            If strlbValue.Contains("(CTY)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(ctrymast.ctryname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname)= " & Trim(strlbValue) & ""
                End If
            End If
            If strlbValue.Contains("(SG)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(S)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = "  upper(othtypname)  = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(othtypmast.othtypname) = " & Trim(strlbValue) & ""
                End If
            End If



            If strlbValue.Contains("(CT)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")
                strlbValue = "'" & strlbValue & "'"
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) = " & Trim(strlbValue) & ""
                Else

                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) = " & Trim(strlbValue) & ""
                End If
            End If






            If strlbValue.Contains("(T)") Then
                strlbValue = (((((((Trim(strlbValue.Trim).Replace("(CTY)", "")).Replace("(ST)", "")).Replace("(G)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", "")).Replace("(HC)", "")).Replace("(HS)", "").Replace("(CTG)", "")

                Dim lsMainArr As String()
                Dim strValue As String = ""
                Dim strWhereCond1 As String = ""
                lsMainArr = objUtils.splitWithWords(strlbValue, ",")
                For i = 0 To lsMainArr.GetUpperBound(0)
                    strValue = ""
                    strValue = lsMainArr(i)
                    If strValue <> "" Then
                        If Trim(strWhereCond1) = "" Then
                            strWhereCond1 = " (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or othtypmast.othtypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR (upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or othtypmast.othtypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' )) "
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


        Dim dtsSectorGroupDetails As DataTable
        Dim strValues As String = ""
        Dim strQuery As String = ""
        dtsSectorGroupDetails = Session("sDtSectorGroupDetails")

        If myDS.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                strValues = ""
                strValues = myDS.Tables(0).Rows(i)("sectorcode").ToString
                If dtsSectorGroupDetails.Rows.Count > 0 Then
                    For j As Integer = 0 To dtsSectorGroupDetails.Rows.Count - 1
                        If dtsSectorGroupDetails.Rows(j)("Code").ToString().Trim = strValues.Trim Then
                            myDS.Tables(0).Rows(i)("sortorder") = 1
                            Exit For
                        End If
                    Next

                End If


            Next
        End If
        'End If


        Dim dataView As DataView = New DataView(myDS.Tables(0))
        'dataView.Sort = "sortorder desc, sectorname asc"
        dataView.Sort = "sectorname asc"
        gv_SearchResult.DataSource = dataView
        If myDS.Tables(0).Rows.Count > 0 Then
            gv_SearchResult.DataBind()
        Else
            gv_SearchResult.PageIndex = 0
            gv_SearchResult.DataBind()
            lblMsg.Visible = True
            lblMsg.Text = "Records not found, Please redefine search criteria."
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

            'If hdOPMode.Value = "D" Then
            '    strSqlQry = "with ctee as (SELECT sectormaster.sectorcode,sectormaster.sectorname,sectormaster.ctrycode,ctrymast.ctryname,sectormaster.citycode, citymast.cityname,isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'')as othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate,sectormaster.adduser,sectormaster.moduser FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode and sectormaster.citycode ='" & TxtCityCode.Text.Trim & "') select * from ctee where othtypname='" & txtGroupTagName.Text.Replace("(SG)", "") & "'"
            'Else
            '    strSqlQry = "with ctee as (SELECT sectormaster.sectorcode,sectormaster.sectorname,sectormaster.ctrycode,ctrymast.ctryname,sectormaster.citycode, citymast.cityname,isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'')as othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate,sectormaster.adduser,sectormaster.moduser FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode and sectormaster.citycode ='" & TxtCityCode.Text.Trim & "') select * from ctee where othtypname='" & txtGroupTagName.Text.Replace("(SG)", "") & "' or othtypname = '' "
            'End If

            ' strSqlQry = "select sectormaster.sectorcode,sectormaster.sectorname,isnull(othtypmast.othtypcode,'')othtypcode,isnull(dbo.fn_get_sectorgroup( sectormaster.sectorgroupcode),'') othtypname,ctrymast.ctryname,citymast.cityname, [IsActive]=case when sectormaster.active=1 then 'Yes' when sectormaster.active=0 then 'No' end ,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate,sectormaster.adduser,sectormaster.moduser from sectormaster left join othtypmast on  othtypmast.othtypname= isnull(dbo.fn_get_sectorgroup( sectormaster.sectorgroupcode),'')  inner join  ctrymast on ctrymast.ctrycode=sectormaster.ctrycode  inner join citymast on citymast.citycode=sectormaster.citycode and othtypmast.othtypname='" & txtGroupTagName.Text.Replace("(SG)", "") & "' and  sectormaster.citycode ='" & TxtCityCode.Text.Trim & "' "
            strSqlQry = "select sectormaster.sectorcode,sectormaster.sectorname,isnull(ab.sectorgroupcode,'')sectorgroupcode,ab.sectorgroupname,ctrymast.ctryname,citymast.cityname, [IsActive]=case when sectormaster.active=1 then 'Yes' when ab.active=0 then 'No' end ,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.adddate,108) as adddate,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.moddate,108) moddate,ab.adduser,ab.moduser from  AlternativeBookingSectorGroup(nolock) ab left join sectormaster on  ab.sectorcode=sectormaster.sectorcode   inner join  ctrymast on ctrymast.ctrycode=sectormaster.ctrycode  inner join citymast on citymast.citycode=sectormaster.citycode where ab.sectorgroupcode='" & txtGroupCode.Text.Trim & "'"

            strorderby = "sectorname"
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
                lblMsg.Text = ""
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try

    End Sub

    ''' <summary>
    ''' ClearFields
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearFields()
        hdOPMode.Value = "S"
        txtName.ReadOnly = True
        txtGroupTagName.ReadOnly = True
        '  txtGroupCode.ReadOnly = True
        btnNew.Enabled = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
        btnCancel.Enabled = True
        txtGroupTagName.Text = ""
        txtName.Text = ""
        txtNameNew.Text = ""
        txtGroupCode.Text = ""
        btnSave.Text = "Save"

        Dim dtsSectorGroupDetails As DataTable
        dtsSectorGroupDetails = Session("sDtSectorGroupDetails")
        dtsSectorGroupDetails.Rows.Clear()
        Session("sDtSectorGroupDetails") = dtsSectorGroupDetails
        gv_SearchResult.DataSource = dtsSectorGroupDetails
        gv_SearchResult.DataBind()


        Dim dtDynamic As DataTable
        dtDynamic = Session("sDtDynamic")
        dtDynamic.Rows.Clear()
        Session("sDtDynamic") = dtDynamic
        dlList.DataSource = dtDynamic
        dlList.DataBind()

        txtName.Enabled = True
        gv_SearchResult.Enabled = True
    End Sub
    ''' <summary>
    ''' lbClose_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim dtsSectorGroupDetails As New DataTable
            dtsSectorGroupDetails = Session("sDtSectorGroupDetails")

            'Dim dtsCountryGroupDetailsNew As New DataTable
            'dtsCountryGroupDetailsNew = Session("sDtHotelGroupDetails")
            'dtsCountryGroupDetailsNew.Clear()

            Dim myButton As LinkButton = CType(sender, LinkButton)
            Dim dlItem As DataListItem = CType((CType(sender, LinkButton)).NamingContainer, DataListItem)
            Dim lb As LinkButton = CType(dlItem.FindControl("lbCountry"), LinkButton)

            If dtsSectorGroupDetails.Rows.Count > 0 Then

                Dim i As Integer
                For i = dtsSectorGroupDetails.Rows.Count - 1 To 0 Step i - 1
                    If lb.Text.Trim = dtsSectorGroupDetails.Rows(i)("TypeName").ToString.Trim Then
                        dtsSectorGroupDetails.Rows.Remove(dtsSectorGroupDetails.Rows(i))
                    End If
                    ' dtsCountryGroupDetails.Rows(i).Delete()
                    dtsSectorGroupDetails.AcceptChanges()
                Next
            End If
            Session("sDtSectorGroupDetails") = dtsSectorGroupDetails

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")

            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    ' For j As Integer = 0 To dtDynamics.Rows.Count - 1
                    If lb.Text.Trim = dtDynamics.Rows(j)("Sector").ToString.Trim Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next

            End If

            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()



            '' Create a Dynamic datatable ---- Start
            Dim ClearDataTable = New DataTable()
            Dim dcGroupDetailsType = New DataColumn("Type", GetType(String))
            Dim dcGroupDetailsTypeName = New DataColumn("TypeName", GetType(String))
            Dim dcGroupDetailsCode = New DataColumn("Code", GetType(String))
            Dim dcGroupDetailsSector = New DataColumn("Sector", GetType(String))
            ClearDataTable.Columns.Add(dcGroupDetailsType)
            ClearDataTable.Columns.Add(dcGroupDetailsTypeName)
            ClearDataTable.Columns.Add(dcGroupDetailsCode)
            ClearDataTable.Columns.Add(dcGroupDetailsSector)
            gv_SearchResult.DataSource = ClearDataTable
            gv_SearchResult.DataBind()

        Catch ex As Exception

        End Try


    End Sub
    ' ("TypeName", GetType(String))

    ''' btnView_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        lblHeading.Text = "View Alternative Booking Sector Group"
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnDelete.Enabled = False
        btnCancel.Enabled = True
        hdOPMode.Value = "S"
        txtGroupTagName.ReadOnly = False
        txtName.ReadOnly = True
        txtGroupTagName.Focus()
        btnSave.Visible = False

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

        gvsearchgrid.Visible = True
        dlListSearch.Visible = True

        gv_SearchResult.Visible = False
        dlList.Visible = False
        txtGroupTagName.Enabled = False
        trNameAndCode.Visible = False
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        FilterGrid()

    End Sub

    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtAlternateDynamicSearch")

        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ":" & lsValue.Trim)
                Session("sDtAlternateDynamicSearch") = dtt
            End If
        End If
        Return True
    End Function
    ''' <summary>
    ''' txtGroupTagName_TextChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtGroupTagName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGroupTagName.TextChanged

      
    End Sub



    ''' <summary>
    ''' GetHotelGroup 
    ''' </summary>
    ''' <param name="prefixText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetSectorGroups(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim sectorgrpnames As New List(Of String)
        Try

            strSqlQry = "select distinct sectorgroupcode,sectorgroupname from AlternativeBookingSectorGroup(nolock) where  sectorgroupname like  " & "'" & prefixText & "%'  order by sectorgroupname"
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
                    ' sectorgrpnames.Add(myDS.Tables(0).Rows(i)("sectorgroupname").ToString())
                    sectorgrpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sectorgroupname").ToString(), myDS.Tables(0).Rows(i)("sectorgroupcode").ToString()))
                Next

            End If

            Return sectorgrpnames
        Catch ex As Exception
            Return sectorgrpnames
        End Try

    End Function

    Private Sub FillGridNew()

        Dim dtt As DataTable
        dtt = Session("sDtAlternateDynamicSearch")


        Dim strCountryGroupValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strHotelchainValue As String = ""
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strSectorValue As String = ""
        Dim strSectorGroupValue As String = ""

        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString = "COUNTRY" Then
                    If strCountryValue <> "" Then
                        strCountryValue = strCountryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCountryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "CITY" Then
                    If strCityValue <> "" Then
                        strCityValue = strCityValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strCityValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                    If strSectorValue <> "" Then
                        strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                    End If
                End If
                If dtt.Rows(i)("Code").ToString = "SECTORGROUP" Then
                    If strSectorGroupValue <> "" Then
                        strSectorGroupValue = strSectorGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                    Else
                        strSectorGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
        Dim strsortorder As String = "DESC"
        Dim strWhereCond As String = ""
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try



            ' strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode left JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode "


            strSqlQry = "select sectormaster.sectorcode,sectormaster.sectorname,isnull(ab.sectorgroupcode,'')sectorgroupcode,ab.sectorgroupname,ctrymast.ctryname,citymast.cityname, [IsActive]=case when sectormaster.active=1 then 'Yes' when ab.active=0 then 'No' end ,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.adddate,108) as adddate,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.moddate,108) moddate,ab.adduser,ab.moduser from  AlternativeBookingSectorGroup(nolock) ab left join sectormaster on  ab.sectorcode=sectormaster.sectorcode   inner join  ctrymast on ctrymast.ctrycode=sectormaster.ctrycode  inner join citymast on citymast.citycode=sectormaster.citycode "

            If strSectorValue.Trim <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
                End If
            End If

            If strSectorGroupValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(ab.sectorgroupname) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & " AND  upper(ab.sectorgroupname) IN ( " & Trim(strSectorGroupValue.Trim.ToUpper) & ")"
                End If
            End If
            If strCountryValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
                End If
            End If


            If strCityValue <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(citymast.cityname) IN ( " & Trim(strCityValue.Trim.ToUpper) & ")"
                Else
                    strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
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
                            strWhereCond1 = " (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ab.sectorgroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
                        Else
                            strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ab.sectorgroupname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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

            gvsearchgrid.DataBind()
            gvsearchgrid.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gvsearchgrid.DataBind()
            Else
                gvsearchgrid.PageIndex = 0
                gvsearchgrid.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally


        End Try

    End Sub





#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet

        FillGridNew()
        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvsearchgrid.DataSource = dataView
            gvsearchgrid.DataBind()
        End If
    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSearchGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvsearchgrid.PageIndexChanging
        gvsearchgrid.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub


    ''' <summary>
    ''' txtGroupTagName_TextChanged
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtCityName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCityName.TextChanged

        If hdOPMode.Value = "N" Then
            Dim myDS As New DataSet
            Dim dt As New DataTable
            Session("sectgrps_citycode") = TxtCityCode.Text

            strSqlQry = "with ctee as( SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'') as othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode   and sectormaster.citycode='" & TxtCityCode.Text & "' )select * from ctee where othtypname ='' "
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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            'ElseIf hdOPMode.Value = "E" Then
            '    Dim myDS As New DataSet
            '    Dim dt As New DataTable
            '    Session("sectgrps_citycode") = TxtCityCode.Text

            '    strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode join othtypmast on sectormaster.sectorgroupcode=othtypmast.othtypcode and sectormaster.citycode='" & TxtCityCode.Text & "'  "
            '    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            '    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            '    myDataAdapter.Fill(myDS)

            '    gv_SearchResult.DataBind()
            '    gv_SearchResult.DataSource = myDS

            '    If myDS.Tables(0).Rows.Count > 0 Then
            '        gv_SearchResult.DataBind()
            '    Else
            '        gv_SearchResult.PageIndex = 0
            '        gv_SearchResult.DataBind()
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Records not found, Please redefine search criteria."
            '    End If
        End If

    End Sub

    Private Sub FillGridByType(ByVal strType As String, ByVal lsProcessValue As String)
        Dim strorderby As String = "sectormaster.sectorname"
        Dim strsortorder As String = "DESC"
        Dim Type() As String
        Dim value() As String
        Dim myDS As New DataSet
        Dim strWhereCond As String = ""
        hdFillByGrid.Value = "N"
        hdLinkButtonValue.Value = ""
        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            lsProcessValue = lsProcessValue.ToUpper
            'strSqlQry = "select othtypcode,othtypname,othtypmast.othgrpcode,isnull(dbo.fn_get_excursiongroup( othtypmast.othtypcode),'') Excursiongroup ,(select case when othtypmast.active='1' then 'Yes' else 'No'end) as Active,(select case when othtypmast.ticketsreqd='1' then 'Yes' else 'No'end) as ticketsreqd,(select case when othtypmast.uponrequest='1' then 'Yes' else 'No'end) as uponrequest,othtypmast.daysofweek,othtypmast.adddate,othtypmast.adduser,othtypmast.moddate,othtypmast.moduser from othtypmast inner join " & _
            '" othgrpmast on othtypmast.othgrpcode=othgrpmast.othgrpcode and othgrpmast.othmaingrpcode in ('EXU','SAFARI') "
            ' strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode"

            strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,'' sectorgroupcode,'' sectorgroupname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode   " 'and sectormaster.citycode='" & TxtCityCode.Text & "'
            Type = strType.Split(":")
            lsProcessValue = lsProcessValue.ToUpper
            value = lsProcessValue.Split(":")
            If lsProcessValue.Trim <> "" Then
                lsProcessValue = (((((Trim(lsProcessValue.Trim).Replace("(CTY)", "")).Replace("(S)", "")).Replace("(T)", "")).Replace("(CT)", "")).Replace("(SG)", ""))
                '  lsProcessValue = ((Trim(lsProcessValue.Trim).Replace("(ET)", "")).Replace("(EG)", ""))
                For k = 0 To Type.GetUpperBound(0)
                    If Type(k) <> "T" Then
                        lsProcessValue = "'" & lsProcessValue & "'"
                        ' If hdOPMode.Value = "E" Then
                        value(k) = "'" & value(k) & "'"
                        '  End If
                    End If


                    If Type(k) = "CTY" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = " upper(ctrymast.ctryname) IN(" & Trim(value(k).Replace("(CTY)", "")) & ")"
                        Else
                            strWhereCond = strWhereCond & " AND  upper(ctrymast.ctryname) IN (" & Trim(value(k).Replace("(CTY)", "")) & ")"
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
                    If Type(k) = "SG" Then
                        If Trim(strWhereCond) = "" Then
                            strWhereCond = "upper(othtypmast.othtypname) in (" & Trim(value(k).Replace("(SG)", "")) & ")"
                            ' strWhereCond = "othtypmast.othtypcode in (select egd.othtypcode   from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname In (" & Trim(value(k).Replace("(EG)", "")) & "))"
                        Else
                            strWhereCond = strWhereCond & "upper(othtypmast.othtypname) in (" & Trim(value(k).Replace("(SG)", "")) & ")"
                            'strWhereCond = strWhereCond & " AND othtypmast.othtypcode in (select egd.othtypcode from excursiongroup eg,excursiongroup_detail egd where eg.excursiongroupcode = egd.excursiongroupcode and excursiongroupname IN (" & Trim(value(k).Replace("(EG)", "")) & "))"
                        End If
                    End If


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


                            If Type(k) = "T" Then

                                If Trim(strWhereCond1) = "" Then
                                    strWhereCond1 = " (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or   upper(isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'') ) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' ) "
                                Else
                                    strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(isnull(dbo.fn_get_groupname(sectormaster.sectorgroupcode),'') ) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Replace("(T)", "")) & "%' ) "
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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExSectorGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

        End Try
    End Sub








    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text '.Replace(": """, ":""")
        Dim lsProcessText As String = ""

        Dim lsProcessCity As String = ""

        Dim lsProcessCountryGroup As String = ""

        Dim lsMainArr As String()
        Dim IsProcessTypes As String = ""
        Dim IsProcessValue As String = ""
        Dim IsProcessValues As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        If hdOPMode.Value = "N" Then
            TxtCityName.ReadOnly = False

        End If
        If hdOPMode.Value = "E" Then
            TxtCityName.ReadOnly = True
        End If
        If hdOPMode.Value = "S" Then

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("CITY", lsProcessCity, "CITY")
                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("COUNTRY", lsProcessCountry, "CTY")
                    Case "SECTORGROUP"
                        lsProcessGroup = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTORGROUP", lsProcessGroup, "SG")
                    Case "SECTOR"
                        lsProcessSector = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("SECTOR", lsProcessSector, "S")
                    Case "TEXT"
                        lsProcessAll = lsMainArr(i).Split(":")(1)
                        sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
                        'If lsProcessAll.Trim = """" Then
                        '    lsProcessAll = ""
                        'End If
                End Select
            Next

            Dim dttDyn As DataTable
            dttDyn = Session("sDtAlternateDynamicSearch")
            dlListSearch.DataSource = dttDyn
            dlListSearch.DataBind()
            FillGridNew()

        ElseIf hdOPMode.Value = "N" Or hdOPMode.Value = "E" Then
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter group name.');", True)
                txtGroupTagName.Focus()
                Exit Sub
            End If
            hdFillByGrid.Value = "N"
            hdLinkButtonValue.Value = ""
            Dim iFlagS As Integer = 0
            Dim dtt As DataTable
            dtt = Session("sDtDynamic")

            For i = 0 To lsMainArr.GetUpperBound(0)
                Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim


                    Case "CITY"
                        lsProcessCity = lsMainArr(i).Split(":")(1) & "(CT)"
                        txtNameNew.Text = lsProcessCity
                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Sector").ToString = lsProcessCity Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CT", lsProcessCity)

                            End If

                        End If
                        Session("sDtDynamic") = dtt

                        FillGridByType("CT", lsProcessCity)

                    Case "COUNTRY"
                        lsProcessCountry = lsMainArr(i).Split(":")(1) & "(CTY)"
                        txtNameNew.Text = lsProcessCountry

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Sector").ToString = lsProcessCountry Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("CTY", lsProcessCountry)
                            End If

                        End If
                        Session("sDtDynamic") = dtt


                        FillGridByType("CTY", lsProcessCountry)
                    Case "SECTOR"
                        lsProcessSector = lsMainArr(i).Split(":")(1) & "(S)"
                        txtNameNew.Text = lsProcessSector

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Sector").ToString = lsProcessSector Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("S", lsProcessSector)
                            End If

                        End If
                        Session("sDtDynamic") = dtt


                        FillGridByType("S", lsProcessSector)
                    Case "SECTORGROUP"
                        lsProcessGroup = lsMainArr(i).Split(":")(1) & "(SG)"
                        txtNameNew.Text = lsProcessGroup

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Sector").ToString = lsProcessGroup Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("SG", lsProcessGroup)
                            End If

                        End If
                        Session("sDtDynamic") = dtt

                        FillGridByType("SG", lsProcessGroup)
                    Case "TEXT"
                        lsProcessText = lsMainArr(i).Split(":")(1) & "(T)"
                        txtNameNew.Text = lsProcessText

                        Dim iFlag As Integer = 0
                        If dtt.Rows.Count >= 0 Then
                            For j = 0 To dtt.Rows.Count - 1
                                If dtt.Rows(j)("Sector").ToString = lsProcessText Then
                                    iFlag = 1
                                End If
                            Next
                            If iFlag = 0 Then
                                dtt.NewRow()
                                dtt.Rows.Add("T", lsProcessText)
                            End If

                        End If

                        Session("sDtDynamic") = dtt
                        FillGridByType("T", lsProcessText)

                End Select
            Next

            If dtt.Rows.Count >= 0 Then
                Dim dtsSectorGroupDetails As DataTable
                dtsSectorGroupDetails = Session("sDtSectorGroupDetails")
                If dtsSectorGroupDetails.Rows.Count > 0 Then
                    For j = 0 To dtt.Rows.Count - 1
                        If dtt.Rows(j)("Sector").ToString = "SECTOR" Then
                            iFlagS = 1
                        End If
                    Next
                    If iFlagS = 0 Then
                        dtt.NewRow()
                        dtt.Rows.Add("SE", "SECTOR")
                        Session("sDtDynamic") = dtt
                    End If
                End If
            End If
            dlList.DataSource = dtt
            dlList.DataBind()

            txtName.Text = ""
        ElseIf hdOPMode.Value = "D" Then
            If txtGroupTagName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group name.');", True)
                txtGroupTagName.Focus()
                Exit Sub
            End If
        End If

    End Sub
    Protected Sub btnGroupName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGroupName.Click
        If hdOPMode.Value = "E" Or hdOPMode.Value = "D" Or hdOPMode.Value = "S" Then

            Dim dtsSectorGroupDetails As DataTable
            dtsSectorGroupDetails = Session("sDtSectorGroupDetails")
            'SELECT partymast.partycode, partymast.partyname,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1,0 sortorder FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode INNER  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode  where partymast.sptypecode='HOT' 
            Dim dt As New DataTable

            'TxtCityCode.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "citycode", "othtypname", txtGroupTagName.Text.Replace("(SG)", ""))
            'TxtCityName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", TxtCityCode.Text.Trim)
            'txtGroupCode.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", txtGroupTagName.Text.Trim.Replace("(SG)", ""))
            ' strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,othtypmast.othtypname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster INNER JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode INNER JOIN citymast ON sectormaster.citycode = citymast.citycode INNER JOIN othtypmast ON sectormaster.sectorgroupcode  = othtypmast.othtypcode and  sectormaster.citycode='" & TxtCityCode.Text.Trim & "'  and othtypmast.othtypname ='" & txtGroupTagName.Text.Replace("(SG)", "") & "'  " ' and sectormaster.citycode= and othtypmast.othtypname ='" & txtGroupTagName.Text.Replace("(SG)", "") & "'  """
            '  strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,'' sectorgroupcode,'' sectorgroupname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode   " 'and sectormaster.citycode='" & TxtCityCode.Text & "'
            '  strSqlQry = "select sectormaster.sectorcode,sectormaster.sectorname,isnull(ab.sectorgroupcode,'')sectorgroupcode,ab.sectorgroupname,ctrymast.ctryname,citymast.cityname, [IsActive]=case when sectormaster.active=1 then 'Yes' when ab.active=0 then 'No' end ,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.adddate,108) as adddate,convert(varchar(16),ab.adddate,101)+ ' ' + convert(varchar(16),ab.moddate,108) moddate,ab.adduser,ab.moduser from  AlternativeBookingSectorGroup(nolock) ab left join sectormaster on  ab.sectorcode=sectormaster.sectorcode   inner join  ctrymast on ctrymast.ctrycode=sectormaster.ctrycode  inner join citymast on citymast.citycode=sectormaster.citycode where ab.sectorgroupcode='" & txtGroupTagName.Text.Replace("(SG)", "") & "'"
            strSqlQry = "SELECT sectormaster.*,ctrymast.ctryname, citymast.cityname,'' sectorgroupcode,'' sectorgroupname,[IsActive]=case when sectormaster.active=1 then 'Active' when sectormaster.active=0 then 'InActive' end,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.adddate,108) as adddate1,convert(varchar(16),sectormaster.adddate,101)+ ' ' + convert(varchar(16),sectormaster.moddate,108) moddate1 FROM sectormaster  JOIN ctrymast ON sectormaster.ctrycode = ctrymast.ctrycode  JOIN citymast ON sectormaster.citycode = citymast.citycode left join AlternativeBookingSectorGroup ab on ab.sectorcode=sectormaster.sectorcode   where ab.sectorgroupcode='" & txtGroupCode.Text.Replace("(SG)", "") & "' " 'and sectormaster.citycode='" & TxtCityCode.Text & "'
 

            Session.Add("sectgrps_citycode", TxtCityCode.Text)


            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    'txtGroupCode.Text = dt.Rows(i)("sectorgroupcode").ToString


                    dtsSectorGroupDetails.NewRow()
                    dtsSectorGroupDetails.Rows.Add("SG", txtGroupTagName.Text, dt.Rows(i)("sectorcode").ToString, dt.Rows(i)("sectorname").ToString)
                    '  dtsExcSuppClsDetails.Rows.Add("EC", txtExcClsTagName.Text, dt.Rows(i)("othtypcode").ToString, dt.Rows(i)("othtypname").ToString)

                    If dt.Rows(i)("active").ToString = "0" Then
                        chkActive.Checked = False
                    Else
                        chkActive.Checked = True
                    End If

                Next
                FillGridForEdit("sectormaster.sectorname")

                If txtGroupTagName.Text.Contains("(SG)") Then
                    Dim dtt As DataTable
                    Dim iFlag As Integer = 0
                    dtt = Session("sDtDynamic")
                    If dtt.Rows.Count >= 0 Then
                        For i = 0 To dtt.Rows.Count - 1
                            If dtt.Rows(i)("Sector").ToString = txtGroupTagName.Text Then
                                iFlag = 1
                            End If
                        Next
                        If iFlag = 0 Then
                            dtt.NewRow()
                            dtt.Rows.Add("SG", txtGroupTagName.Text)
                            Session("sDtDynamic") = dtt
                            dlList.DataSource = dtt
                            dlList.DataBind()
                        End If

                    End If

                End If


            End If

            FillCheckbox()
            If hdOPMode.Value = "D" Then
                gv_SearchResult.Enabled = False
                txtName.Enabled = False
            End If
            If hdOPMode.Value = "S" Then
                gv_SearchResult.Enabled = False
                txtName.Enabled = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' btnResetSelection_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click



        If hdOPMode.Value = "S" Then
            Dim dtt As DataTable
            dtt = Session("sDtAlternateDynamicSearch")
            dtt.Rows.Clear()
            dlListSearch.DataSource = dtt
            dlListSearch.DataBind()
            Session("sDtAlternateDynamicSearch") = dtt
            FillGridNew()
        Else
            Dim dtsSectorGroupDetails As DataTable
            dtsSectorGroupDetails = Session("sDtSectorGroupDetails")
            dtsSectorGroupDetails.Rows.Clear()
            Session("sDtSectorGroupDetails") = dtsSectorGroupDetails
            gv_SearchResult.DataSource = dtsSectorGroupDetails
            gv_SearchResult.DataBind()


            Dim dtDynamic As DataTable
            dtDynamic = Session("sDtDynamic")
            dtDynamic.Rows.Clear()
            Session("sDtDynamic") = dtDynamic
            dlList.DataSource = dtDynamic
            dlList.DataBind()
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
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtAlternateDynamicSearch")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtAlternateDynamicSearch") = dtDynamics
            dlListSearch.DataSource = dtDynamics
            dlListSearch.DataBind()
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub




    Protected Sub gvSearchGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvsearchgrid.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            '   Dim lblSupplierName As Label = e.Row.FindControl("lblSupplierName")
            Dim lblCtryName As Label = e.Row.FindControl("lblCountryName")
            Dim lblCityName As Label = e.Row.FindControl("lblCityName")
            Dim lblSectName As Label = e.Row.FindControl("lblSectorName")
            Dim lblSectorGroup As Label = e.Row.FindControl("lblSectorGroupName")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextSectorGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtAlternateDynamicSearch")
            If Session("sDtAlternateDynamicSearch") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""
                        If "COUNTRY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "CITY" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTOR" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSector = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "SECTORGROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextSectorGroup = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCtry = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCtry
                            lsSearchTextSector = lsSearchTextCtry
                            lsSearchTextSectorGroup = lsSearchTextCtry
                        End If

                        If lsSearchTextCtry.Trim <> "" Then
                            lblCtryName.Text = Regex.Replace(lblCtryName.Text.Trim, lsSearchTextCtry.Trim(), _
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

                        If lsSearchTextSectorGroup.Trim <> "" Then
                            lblSectorGroup.Text = Regex.Replace(lblSectorGroup.Text.Trim, lsSearchTextSectorGroup.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If

    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Dim chkCtry2 As CheckBox = e.Row.FindControl("chkCtry2")
            'Dim dtRow As DataRow = objUtils.fnGridViewRowToDataRow(e.Row)
            'chkCtry2.Checked = IIf(dtRow("chkselect") = 1, True, False)


            Dim lblPartyName As Label = e.Row.FindControl("lblSectorName")
            Dim lblexcursiongroup As Label = e.Row.FindControl("lblSectorGroupName")

            Dim lsSearchTextCtry As String = ""
            Dim lsSearchTextCity As String = ""
            Dim lsSearchTextSector As String = ""
            Dim lsSearchTextCat As String = ""
            Dim lsSearchTextHotelGroup As String = ""
            Dim lsSearchTextPartyName As String = ""
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsSearchTextCat = ""

                        If "ET" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("Sector").ToString.Trim.ToUpper
                            lsSearchTextPartyName = lsSearchTextPartyName.Replace("(ET)", "")
                        End If
                        If "EG" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextCity = dtDynamics.Rows(j)("Sector").ToString.Trim.ToUpper
                            lsSearchTextCity = lsSearchTextCity.Replace("(EG)", "")
                        End If


                        If "T" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsSearchTextPartyName = dtDynamics.Rows(j)("Sector").ToString.Replace("(T)", "").Trim.ToUpper
                            lsSearchTextCity = lsSearchTextPartyName

                        End If
                        If lsSearchTextPartyName.Trim <> "" Then
                            lblPartyName.Text = Regex.Replace(lblPartyName.Text.Trim, lsSearchTextPartyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                        If lsSearchTextCity.Trim <> "" Then
                            lblexcursiongroup.Text = Regex.Replace(lblexcursiongroup.Text.Trim, lsSearchTextCity.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)


                        End If
                    Next
                End If
            End If

        End If
    End Sub


    Protected Sub chkActive_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkActive.CheckedChanged

    End Sub
End Class



