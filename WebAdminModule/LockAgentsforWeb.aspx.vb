'------------================--------------=======================------------------================
'   Module Name    :    LockAgentsforWeb.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
#End Region

Partial Class LockAgentsforWeb
    Inherits System.Web.UI.Page
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamic") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chkSel As CheckBox
    Dim txtRes As TextBox
#End Region
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
    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessCat As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "CUSTOMER"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMER", lsProcessCountry, "C")
                Case "CUSTOMERGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CUSTOMERGROUP", lsProcessCountry, "CSG")
                Case "COUNTRY"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRY", lsProcessCountry, "CTY")
                Case "COUNTRYGROUP"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("COUNTRYGROUP", lsProcessCountry, "CG")
                Case "SECTOR"
                    lsProcessCountry = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("SECTOR", lsProcessCountry, "S")

                Case "CITY"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CITY", lsProcessCity, "CT")
                Case "CATEGORY"
                    lsProcessCat = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("CATEGORY", lsProcessCat, "CTG")

                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 
    End Sub
    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
    Private Function BuildConditionNew(ByVal strCustomerGroupValue As String, ByVal strCountryGroupValue As String, ByVal strCustomerValue As String, ByVal strCountryValue As String, ByVal strCityValue As String, ByVal strCategoryValue As String, ByVal strsectorvalue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCountryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(ctrymast.ctryname) IN (" & Trim(strCountryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCustomerGroupValue <> "" Then

            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.agentcode in (select customergroup_detail.agentcode   from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            Else

                strWhereCond = strWhereCond & " AND agentmast.agentcode in (select customergroup_detail.agentcode  from customergroup ,customergroup_detail where customergroup.customergroupcode = customergroup_detail.customergroupcode and customergroupname in  (" & Trim(strCustomerGroupValue.Trim.ToUpper) & "))"
            End If
        End If
        If strCountryGroupValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " ctrymast.ctrycode in (select countrygroup_detail.ctrycode   from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            Else
                strWhereCond = strWhereCond & "  AND ctrymast.ctrycode in (select countrygroup_detail.ctrycode    from countrygroup ,countrygroup_detail where countrygroup.countrygroupcode = countrygroup_detail.countrygroupcode and countrygroupname in (" & Trim(strCountryGroupValue.Trim.ToUpper) & "))"
            End If
        End If

        If strCustomerValue <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            Else
                strWhereCond = strWhereCond & " and  upper(agentmast.agentname) in (" & Trim(strCustomerValue) & ")"
            End If
        End If
        If strCityValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(citymast. cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(citymast.cityname) IN (" & Trim(strCityValue.Trim.ToUpper) & ")"
            End If
        End If
        If strCategoryValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agentcatmast. agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agentcatmast.agentcatname) IN (" & Trim(strCategoryValue.Trim.ToUpper) & ")"
            End If
        End If
        If strsectorvalue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(agent_sectormaster. sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(agent_sectormaster.sectorname) IN (" & Trim(strsectorvalue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    Else

                        strWhereCond1 = strWhereCond1 & " OR upper(agentmast.agentname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.agentcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webusername) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(agentmast.webcontact) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or upper(agentmast.webemail) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
                    End If
                End If
            Next
            If Trim(strWhereCond) = "" Then
                strWhereCond = "(" & strWhereCond1 & ")"
            Else
                strWhereCond = strWhereCond & " AND (" & strWhereCond1 & ")"
            End If

        End If
        'If txtFromDate.Text.Trim <> "" And txtToDate.Text <> "" Then

        '    If ddlOrder.SelectedValue = "C" Then
        '        If Trim(strWhereCond) = "" Then

        '            strWhereCond = " (CONVERT(datetime, convert(varchar(10),partymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        Else
        '            strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),partymast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        End If
        '    ElseIf ddlOrder.SelectedValue = "M" Then
        '        If Trim(strWhereCond) = "" Then

        '            strWhereCond = " (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        Else
        '            strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),partymast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
        '        End If
        '    End If
        'End If


        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        Dim dtt As DataTable

        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        dtt = Session("sDtDynamic")
        Dim strCountryValue As String = ""
        Dim strCityValue As String = ""
        Dim strCategoryValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Dim strCustomerGroupValue As String = ""
        Dim strCustomerValue As String = ""
        Dim strCountryGroupValue As String = ""
        Dim strSectorValue As String = ""

        Try
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
                    If dtt.Rows(i)("Code").ToString = "CUSTOMERGROUP" Then
                        If strCustomerGroupValue <> "" Then
                            strCustomerGroupValue = strCustomerGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CUSTOMER" Then
                        If strCustomerValue <> "" Then
                            strCustomerValue = strCustomerValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCustomerValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "COUNTRYGROUP" Then
                        If strCountryGroupValue <> "" Then
                            strCountryGroupValue = strCountryGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCountryGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "CATEGORY" Then
                        If strCategoryValue <> "" Then
                            strCategoryValue = strCategoryValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCategoryValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "SECTOR" Then
                        If strSectorValue <> "" Then
                            strSectorValue = strSectorValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strSectorValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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

            strBindCondition = BuildConditionNew(strCustomerGroupValue, strCountryGroupValue, strCustomerValue, strCountryValue, strCityValue, strCategoryValue, strSectorValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            grdSupplier.Visible = True
            lblMsg.Visible = False
            If grdSupplier.PageIndex < 0 Then
                grdSupplier.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strappsortExpression")
            Dim strsortorder As String = "ASC"
            '  strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive'end  FROM plgrpmast"
            strSqlQry = "select agentmast.agentcode,agentname,reason  from agentmast left outer join agents_locked on agentmast.agentcode = agents_locked.agentcode left outer join ctrymast on agentmast.ctrycode= ctrymast.ctrycode " & _
                  " Left outer join  citymast on agentmast.citycode=citymast.citycode left outer join agent_Sectormaster on agentmast.sectorcode=agent_sectormaster.sectorcode " & _
            " left outer join agentcatmast on agentmast.catcode=agentcatmast.agentcatcode where agentmast.active=1"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " and " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            grdSupplier.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                '  grdSupplier.PageSize = pagevaluems
                grdSupplier.DataBind()
            Else
                grdSupplier.PageIndex = 0
                grdSupplier.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

            Dim cnt As Long
            For Each gvRow In grdSupplier.Rows
                cnt = Val(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agents_locked", "count(*)", "agentcode", gvRow.Cells(0).Text.ToString))
                chkSel = gvRow.FindControl("chkSelect")
                txtRes = gvRow.FindControl("txtReason")
                If cnt <> 0 Then
                    chkSel.Checked = True
                    txtRes.Text = myDS.Tables(0).Rows(gvRow.RowIndex)("reason").ToString
                Else
                    chkSel.Checked = False
                    txtRes.Text = ""
                End If
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BankTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then


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



            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                                    CType(strappname, String), "WebAdminModule\LockAgentsforWeb.aspx?appid=" + strappid, btnadd, BtnExportToExcel, _
                                                                    btnPrint, grdSupplier)

            End If


            Try
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic
                '--------end
                Session.Add("strappsortExpression", "agentcode")
                Session.Add("strLocksortdirection", SortDirection.Ascending)

                FillGridNew()





            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try


        Else

            Dim strstring As String = "'"

        End If
    End Sub





#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        lblMsg.Visible = False

        If grdSupplier.PageIndex < 0 Then
            grdSupplier.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = " select agentmast.agentcode,agentname,reason  from agentmast left outer join agents_locked on agentmast.agentcode = agents_locked.agentcode where agentmast.active=1 "

            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            'Else
            '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            grdSupplier.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                grdSupplier.DataBind()
            Else
                grdSupplier.PageIndex = 0
                grdSupplier.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection

            '----------------------------------Show Lock Status 
            Dim cnt As Long
            For Each gvRow In grdSupplier.Rows
                cnt = Val(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agents_locked", "count(*)", "agentcode", gvRow.Cells(0).Text.ToString))
                chkSel = gvRow.FindControl("chkSelect")
                txtRes = gvRow.FindControl("txtReason")
                If cnt <> 0 Then
                    chkSel.Checked = True
                    txtRes.Text = myDS.Tables(0).Rows(gvRow.RowIndex)("reason").ToString
                Else
                    chkSel.Checked = False
                    txtRes.Text = ""
                End If
            Next

        Catch ex As Exception
            objUtils.WritErrorLog("LockAgentsforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Validate Page()"
    Public Function ValidatePage() As Boolean
        Try

            For Each gvRow In grdSupplier.Rows
                chkSel = gvRow.FindControl("chkSelect")
                txtRes = gvRow.FindControl("txtReason")
                If chkSel.Checked = True Then
                    If txtRes.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter reason for locked customers.');", True)
                        SetFocus(grdSupplier)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            Next

            ValidatePage = True

        Catch ex As Exception

        End Try
    End Function
#End Region

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("agentmast.agentcode")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            If Page.IsValid = True Then
                If ValidatePage() = False Then
                    Exit Sub
                End If

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start


                '----------------------------------- Iserting Data To agents_locked Table
                For Each gvRow In grdSupplier.Rows
                    chkSel = gvRow.FindControl("chkSelect")
                    txtRes = gvRow.FindControl("txtReason")
                    myCommand = New SqlCommand("sp_add_agents_locked", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(0).Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 200)).Value = CType(Trim(txtRes.Text), String)
                    myCommand.Parameters.Add(New SqlParameter("@lockdate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@lockuser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                    If chkSel.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@locked", SqlDbType.Int, 9)).Value = 1
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@locked", SqlDbType.Int, 9)).Value = 0
                    End If
                    myCommand.ExecuteNonQuery()
                Next
                '-----------------------------------------------------------

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)              ' sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)             ' connection close

                FillGrid("agentmast.agentcode")
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully.');", True)
                ' SetFocus(ddlMarket)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("LockAgentsforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx", False)
    End Sub


    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        FillGrid("agentmast.agentcode")
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strappsortExpression"), "")

        myDS = grdSupplier.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strappsortExpression", objUtils.SwapSortDirection(Session("strappsortExpression")))
            dataView.Sort = Session("strappsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strappsortExpression"))
            grdSupplier.DataSource = dataView
            grdSupplier.DataBind()
        End If
    End Sub
#End Region

    Protected Sub grdSupplier_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdSupplier.Sorting
        Session.Add("strappsortExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
 

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=LockAgentsforWeb','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

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
                    Case 10
                        Me.MasterPageFile = "~/TransferMaster.master"
                    Case 11
                        Me.MasterPageFile = "~/ExcursionMaster.master"
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamic") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub

    Protected Sub BtnExportToExcel_Click1(sender As Object, e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If grdSupplier.Rows.Count <> 0 Then
                strSqlQry = " select agentmast.agentcode as [Customer Code],agentname as [Customer Name], " & _
                            " [Lock Status] = case when reason is null then 'No' else 'Yes' end  , reason as [Reason]" & _
                            " from agentmast inner join agents_locked on agentmast.agentcode = agents_locked.agentcode where agentmast.active=1 "

                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY agentmast.agentcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY agentmast.agentcode "
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "LockAgents")

                If DS.Tables(0).Rows.Count > 0 Then


                    objUtils.ExportToExcel(DS, Response)
                    con.Close()
                Else
                    objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
                End If
            End If


        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnPrint_Click1(sender As Object, e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=LockAgentsforWeb&BackPageName=LockAgentsforWeb.aspx&SellTypeCode=" & Trim(ddlSellingType.Items(ddlSellingType.SelectedIndex).Text) & "&SellTypeName=" & Trim(ddlSellingTypeName.Items(ddlSellingTypeName.SelectedIndex).Text) & "&CatCode=" & Trim(ddlCategory.Items(ddlCategory.SelectedIndex).Text) & "&CatName=" & Trim(ddlCategoryName.Items(ddlCategoryName.SelectedIndex).Text) & "&CtryCode=" & Trim(ddlCountry.Items(ddlCountry.SelectedIndex).Text) & "&CtryName=" & Trim(ddlCountryName.Items(ddlCountryName.SelectedIndex).Text) & "&MktCode=" & Trim(ddlMarket.Items(ddlMarket.SelectedIndex).Text) & "&MktName=" & Trim(ddlMarketName.Items(ddlMarketName.SelectedIndex).Text) & "&CityCode=" & Trim(ddlCity.Items(ddlCity.SelectedIndex).Text) & "&CityName=" & Trim(ddlCityName.Items(ddlCityName.SelectedIndex).Text) & "','ApprovedCust','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=LockAgentsforWeb&BackPageName=LockAgentsforWeb.aspx','ApprovedCust');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'If dd

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("LockAgentsforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
End Class
