'------------================--------------=======================------------------================
'   Page Name       :   OtherBankMasterSearch.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient

Partial Class OtherBankMasterSearch
    Inherits System.Web.UI.Page



#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Enum GridCol"
    Enum GridCol
        OtherBankCodeTCol = 0
        OtherBankCode = 1
        OtherBankName = 2
        Active = 3
        DateCreated = 4
        UserCreated = 5
        DateModified = 6
        UserModified = 7
        Edit = 8
        View = 9
        Delete = 10
    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try

                Dim appidnew As String = CType(Request.QueryString("appid"), String)

                Dim strappid As String = ""
                Dim strappname As String = ""
                Dim AppName As String = objUser.GetAppName(Session("dbconnectionName"), appidnew)

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")

                If AppName Is Nothing = False Then
                    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                    strappname = Session("AppName")
                    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                End If
                If appidnew Is Nothing = False Then
                    strappid = appidnew

                End If
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                ViewState.Add("divcode", divid)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                         CType(strappname, String), "AccountsModule\OtherBankMasterSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                         btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
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
     
                Session.Add("strsortExpression", "other_bank_master_code")
                Session.Add("strsortdirection", SortDirection.Ascending)
'
                FillGridNew()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        ' btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CustBankWindowPostBack") Then
            btnResetSelection_Click(sender, e)
        End If
    End Sub
#End Region

    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessCity As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""

        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "BANKNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("BankName", lsProcessCity, "BankName")
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

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#Region "Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = 'ASC')"
    Private Sub FillGrid(ByVal strorderby As String, Optional ByVal strsortorder As String = "ASC")
        Dim myDS As New DataSet

        gv_SearchResult.Visible = True
        lblMsg.Visible = False

        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try
            strSqlQry = " select other_bank_master_code,other_bank_master_des,adddate,adduser,moddate,moduser," & _
                        " [Active]=case when active=1 then 'Active' else 'In Active' end " & _
                        " from customer_bank_master"

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
                lblMsg.Visible = True
                lblMsg.Text = "Records not found. Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherBankMaster.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('OtherBankMaster.aspx?State=New','OtherBankMaster','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('OtherBankMaster.aspx?State=New','OtherBankMaster');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('OtherBankMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherBankMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                'strpop = "window.open('OtherBankMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherBankMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                'strpop = "window.open('OtherBankMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherBankMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherBankMaster');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region




#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblOthbBankName As Label = e.Row.FindControl("lblOthbBankName")

            Dim lsCurrencyName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCurrencyName = ""

                        If "BANKNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCurrencyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsCurrencyName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsCurrencyName.Trim <> "" Then
                            lblOthbBankName.Text = Regex.Replace(lblOthbBankName.Text.Trim, lsCurrencyName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If
                    Next
                End If
            End If



        End If



    End Sub
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub

    Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting

    End Sub

    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region
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
    Private Function BuildConditionNew(ByVal strCurrencyValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strCurrencyValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(customer_bank_master.other_bank_master_des) IN (" & Trim(strCurrencyValue.Trim.ToUpper) & ")"
                'Else
                '    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "(upper(customer_bank_master.other_bank_master_des) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
                        'Else
                        'strWhereCond1 = strWhereCond1 & " OR  (upper(sectormaster.sectorname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othtypmast.othtypname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(ctrymast.ctryname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(citymast.cityname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' ) "
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

            If ddlOrder.SelectedValue = "C" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),customer_bank_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),customer_bank_master.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),customer_bank_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),customer_bank_master.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function
    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strCurrencyValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "BANKNAME" Then
                        If strCurrencyValue <> "" Then
                            strCurrencyValue = strCurrencyValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strCurrencyValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            strBindCondition = BuildConditionNew(strCurrencyValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpression")
            Dim strsortorder As String = "ASC"
            strSqlQry = " select other_bank_master_code,other_bank_master_des,adddate,adduser,moddate,moduser," & _
                                   " [Active]=case when active=1 then 'Active' else 'In Active' end " & _
                                   " from customer_bank_master"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluecus
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = " SELECT other_bank_master_code AS [Customer Bank Code],other_bank_master_des AS [Customer Bank Name]," & _
                            " [Active]=case when active=1 then 'Active' when active=0 then 'In Active' end,(Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created]," & _
                            " adduser as [User Created],(Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified] " & _
                            " FROM customer_bank_master "

                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & ExportWithOrderByValues()
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY " & ExportWithOrderByValues()
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "otherbank")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            Dim strpop As String = ""

            strpop = "window.open('rptReportNew.aspx?Pageame=OtherBankMaster&BackPageName=OtherBankMasterSearch.aspx','OtherBankMaster');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            objUtils.WritErrorLog("OtherBankMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


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


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherBankMasterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

   
End Class
