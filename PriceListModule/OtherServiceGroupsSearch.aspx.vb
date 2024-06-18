'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceGroupSearch.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class OtherServiceGroupsSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
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
        GroupCodeTCol = 0
        GroupCode = 1
        GroupName = 2
        Department = 3
        PrintGroup = 4
        PrintType = 5
        PrintCategory = 6
        Active = 7
        DateCreated = 8
        UserCreated = 9
        DateModified = 10
        UserModified = 11
        Edit = 12
        View = 13
        Delete = 14

    End Enum
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
    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            Dim dlItem As DataListItem = CType((CType(sender, Button)).NamingContainer, DataListItem)
            Dim lnkcode As Button = CType(dlItem.FindControl("lnkcode"), Button)
            Dim lnkvalue As Button = CType(dlItem.FindControl("lnkvalue"), Button)
            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicOthGrps")
            If dtDynamics.Rows.Count > 0 Then
                Dim j As Integer
                For j = dtDynamics.Rows.Count - 1 To 0 Step j - 1
                    If lnkcode.Text.Trim.ToUpper = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper And lnkvalue.Text.Trim.ToUpper = dtDynamics.Rows(j)("Value").ToString.Trim.ToUpper Then
                        dtDynamics.Rows.Remove(dtDynamics.Rows(j))
                    End If
                Next
            End If
            Session("sDtDynamicOthGrps") = dtDynamics
            dlList.DataSource = dtDynamics
            dlList.DataBind()

            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Function sbAddToDataTable(ByVal lsName As String, ByVal lsValue As String, ByVal lsShortCode As String) As Boolean
        Dim iFlag As Integer = 0
        Dim dtt As DataTable
        dtt = Session("sDtDynamicOthGrps")
        If dtt.Rows.Count >= 0 Then
            For i = 0 To dtt.Rows.Count - 1
                If dtt.Rows(i)("Code").ToString.Trim.ToUpper = lsName.Trim.ToUpper And dtt.Rows(i)("Value").ToString.Trim.ToUpper = lsValue.Trim.ToUpper Then
                    iFlag = 1
                End If
            Next
            If iFlag = 0 Then
                dtt.NewRow()
                dtt.Rows.Add(lsName.Trim.ToUpper, lsValue.Trim.ToUpper, lsShortCode.Trim.ToUpper & ": " & lsValue.Trim)
                Session("sDtDynamicOthGrps") = dtt
            End If
        End If
        Return True
    End Function
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("OthTypeFilter") = Request.Params("Type")
        If Page.IsPostBack = False Then
            Try
                '  SetFocus(txtOtherCode)


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


                    If Session("OthTypeFilter") = "EXU" Then
                        Lblselltypes.Text = "Excursion Group List"

                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                    CType(strappname, String), "PriceListModule\OtherServiceGroupsSearch.aspx?appid=" & strappid & "&Type=" & Session("OthTypeFilter") & "", btnAddNew, btnExportToExcel, _
                                                              btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                    Else
                        Lblselltypes.Text = "Other Services Group List"
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                    CType(strappname, String), "PriceListModule\OtherServiceGroupsSearch.aspx?appid=" & strappid & "&Type=" & Session("OthTypeFilter") & "", btnAddNew, btnExportToExcel, _
                                                    btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                      

                    End If


                End If

                ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddldeptnm, "Deptname", "select Deptname from DeptMaster where active=1 order by Deptname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddldept, "Deptcode", "select Deptcode from DeptMaster where active=1 order by Deptcode", True)

                Session.Add("strsortExpressionOthgrp", "othgrpcode")
                Session.Add("strsortdirectionOthgrp", SortDirection.Ascending)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlDepartment, "Deptcode", "select Deptcode from DeptMaster where active=1 order by Deptcode", True)

                Dim otypecode1 As String
                Dim otypecode2 As String

                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
                If Session("OthTypeFilter") = "OTH" Then
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupCode, "othmaingrpcode", "othmaingrpname", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 and othmaingrpcode not in('" & otypecode1 & "'" & ",'" & otypecode2 & "')  order by othmaingrpcode", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupName, "othmaingrpname", "othmaingrpcode", "select othmaingrpname,othmaingrpcode from othmaingrpmast where active=1 and othmaingrpcode not in('" & otypecode1 & "'" & ",'" & otypecode2 & "') order by othmaingrpname", True)

                Else
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupCode, "othmaingrpcode", "othmaingrpname", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 and othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "') order by othmaingrpcode", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupName, "othmaingrpname", "othmaingrpcode", "select othmaingrpname,othmaingrpcode from othmaingrpmast where active=1  and othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "') order by othmaingrpname", True)

                End If
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptCode, "deptcode", "deptname", "select deptcode,deptname from DeptMaster where active=1 order by deptcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptName, "deptname", "deptcode", "select deptname,deptcode from DeptMaster where active=1 order by deptname", True)

                'FillGrid("othgrpcode")
                fillorderby()
                ' FillGrid("othgrpname")
                'charcters(txtOtherCode)
                'charcters(txtOtherName)

                Session("sDtDynamicOthGrps") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamicOthGrps") = dtDynamic
                '--------end
                Session.Add("strsortExpressionOthgrp", "Othgrpcode")
                Session.Add("strsortdirectionOthgrp", SortDirection.Ascending)

                'FillGrid("currname")
                FillGridNew()

                If Session("OthTypeFilter") = "OTH" Then
                    'ddlMainGroupCode.Visible = False
                    'ddlMainGroupName.Visible = False

                End If




                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                '    ddlDeptCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlDeptName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServiceGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else



            'Post back handling


        End If
        '  btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OthgrpWindowPostBack") Then
            ' btnSearch_Click(sender, e)
            btnResetSelection_Click(sender, e)
        End If

    End Sub
    Protected Sub btnResetSelection_Click(sender As Object, e As System.EventArgs) Handles btnResetSelection.Click
        Dim dtt As DataTable
        dtt = Session("sDtDynamicOthGrps")
        If dtt.Rows.Count > 0 Then
            dtt.Clear()
        End If
        Session("sDtDynamicOthGrps") = dtt
        dlList.DataSource = dtt
        dlList.DataBind()
        FillGridNew()

    End Sub
    Private Function BuildConditionNew(ByVal strGroupValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strGroupValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(othgrpmast.othgrpname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(othgrpmast.othgrpname) IN (" & Trim(strGroupValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "upper(othgrpmast.othgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(othgrpmast.othgrpcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'  or  upper(othgrpmast.othgrpcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'    "
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(othgrpmast.othgrpname) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or  upper(othgrpmast.othgrpcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' "
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),othgrpmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),othgrpmast.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),othgrpmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),othgrpmast.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OthServiceGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    '#End Region
    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamicOthGrps")
        Dim strGroupValue As String = ""

        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "GROUP" Then
                        If strGroupValue <> "" Then
                            strGroupValue = strGroupValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strGroupValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            strBindCondition = BuildConditionNew(strGroupValue, strTextValue)
            Dim myDS As New DataSet
            Dim strValue As String
            gv_SearchResult.Visible = True
            lblMsg.Visible = False
            If gv_SearchResult.PageIndex < 0 Then
                gv_SearchResult.PageIndex = 0
            End If
            strSqlQry = ""
            Dim strorderby As String = Session("strsortexpressionOthgrp")
            Dim strsortorder As String = "ASC"
            Dim otypecode1 As String
            Dim otypecode2 As String
            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
            If Session("OthTypeFilter") = "OTH" Then
                strSqlQry = "SELECT othgrpcode,othgrpname,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast where othgrpcode not in ( select * from view_system_othgrp)"

            Else
                strSqlQry = "SELECT othgrpcode,othgrpname,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast not in  ( select * from view_system_othgrp) "
            End If
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
            objUtils.WritErrorLog("BankdetailsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
    '#Region "  Private Function BuildCondition() As String"
    '    Private Function BuildCondition() As String
    '        strWhereCond = ""
    '        If txtOtherCode.Value.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then

    '                strWhereCond = " upper(othgrpcode) LIKE '" & Trim(txtOtherCode.Value.Trim.ToUpper) & "%'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(othgrpcode) LIKE '" & Trim(txtOtherCode.Value.Trim.ToUpper) & "%'"
    '            End If
    '        End If

    '        If txtOtherName.Value.Trim <> "" Then
    '            If Trim(strWhereCond) = "" Then

    '                strWhereCond = " upper(othgrpname) LIKE '" & Trim(txtOtherName.Value.Trim.ToUpper) & "%'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(othmaingrpmast.othgrpname) LIKE '" & Trim(txtOtherName.Value.Trim.ToUpper) & "%'"
    '            End If
    '        End If

    '        If ddlMainGroupName.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = "upper(othmaingrpmast.othmaingrpcode)='" & Trim(ddlMainGroupCode.Items(ddlMainGroupCode.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            Else
    '                strWhereCond = strWhereCond & "And upper(othmaingrpmast.othmaingrpcode)='" & Trim(ddlMainGroupCode.Items(ddlMainGroupCode.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            End If



    '        End If
    '        If ddlDeptCode.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then

    '                strWhereCond = " upper(deptcode) ='" & Trim(ddlDeptCode.Items(ddlDeptCode.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(deptcode) ='" & Trim(ddlDeptCode.Items(ddlDeptCode.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            End If
    '        End If
    '        If ddlDeptName.Value <> "[Select]" Then
    '            If Trim(strWhereCond) = "" Then
    '                strWhereCond = " upper(deptcode) ='" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "DeptMaster", "Deptcode", "DeptName", ddlDeptName.Items(ddlDeptName.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            Else
    '                strWhereCond = strWhereCond & " AND upper(deptcode) ='" & objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "DeptMaster", "Deptcode", "DeptName", ddlDeptName.Items(ddlDeptName.SelectedIndex).Text.Trim.ToUpper) & "'"
    '            End If
    '        End If


    '        BuildCondition = strWhereCond
    '    End Function
    '#End Region

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
            'strSqlQry = "SELECT *,[IsActive]=case when active=1 then 'Active' when active=0 then 'InActive' end FROM othgrpmast"
            Dim otypecode1 As String
            Dim otypecode2 As String

            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
            If Session("OthTypeFilter") = "OTH" Then
                ' strSqlQry = "SELECT othgrpcode,othgrpname,othmaingrpname,deptcode,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast,othmaingrpmast where  othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode not in('" & otypecode1 & "'" & ",'" & otypecode2 & "')"
                strSqlQry = "SELECT othgrpcode,othgrpname,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast where othgrpcode not in (select * from view_system_othgrp)"
            Else
                ' strSqlQry = "SELECT othgrpcode,othgrpname,othmaingrpname,deptcode,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast,othmaingrpmast where  othgrpmast.othmaingrpcode =othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "')"
                strSqlQry = "SELECT othgrpcode,othgrpname,case ratesbasedonpax when 1 then 'Yes' else'No' end  as ratesbasedonpax ,case paxcalcreqd  when 1 then 'Yes' else'No' end  as paxcalcreqd,incomecode,expensecode,profitcode,othgrpmast.adddate,othgrpmast.adduser,othgrpmast.moddate,othgrpmast.moduser,case when printcat=1 then 'Yes' when printcat=0 then 'No' end as printcat,case when printtype=1 then 'Yes' when printtype=0 then 'No' end as printtype,case when printgrp=1 then 'Yes' when printgrp=0 then 'No' end as printgrp,case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end as IsActive FROM othgrpmast "
            End If
            'If Trim(BuildCondition) <> "" Then
            '    strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

            If Session("OthTypeFilter") = "OTH" Then
                gv_SearchResult.Columns(3).Visible = False


            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherServiceGroups.aspx", False)
        Dim strpop As String = ""
        ' strpop = "window.open('OtherServiceGroups.aspx?State=New&Type=" + Session("OthTypeFilter") + "','Othgrp','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('OtherServiceGroups.aspx?State=New&Type=" + Session("OthTypeFilter") + "','Othgrp');"
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
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblOtherCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceGroups.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('OtherServiceGroups.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherServiceGroups.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceGroups.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('OtherServiceGroups.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherServiceGroups.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("OtherServiceGroups.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('OtherServiceGroups.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('OtherServiceGroups.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "&Type=" + Session("OthTypeFilter") + " ','Othgrp');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    'FillGrid("othgrpcode")
    '    Select Case ddlOrderBy.SelectedIndex
    '        Case 0
    '            FillGrid("othgrpname")
    '        Case 1
    '            FillGrid("othgrpcode")
    '        Case 2
    '            FillGrid("deptcode")

    '    End Select
    'End Sub

#End Region

    '#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    '    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    '        txtOtherCode.Value = ""
    '        txtOtherName.Value = ""
    '        ddlMainGroupCode.Value = "[Select]"
    '        ddlMainGroupName.Value = "[Select]"
    '        ddlDeptCode.Value = "[Select]"
    '        ddlDeptName.Value = "[Select]"
    '        ddlOrderBy.SelectedIndex = 0

    '        FillGrid("othgrpname")
    '    End Sub
    '#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"

    Protected Sub gv_SearchResult_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim lblothgrpname As Label = e.Row.FindControl("lblothgrpname")


            Dim lsCurrencyName As String = ""

            Dim lsTextName As String = ""
            Dim lsGroupName As String = ""

            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamicOthGrps")
            If Session("sDtDynamicOthGrps") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsCurrencyName = ""

                        If "GROUP" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsGroupName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If


                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsTextName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If


                        If lsGroupName.Trim <> "" Then
                            lblothgrpname.Text = Regex.Replace(lblothgrpname.Text.Trim, lsGroupName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                        If lsTextName.Trim <> "" Then
                            lblothgrpname.Text = Regex.Replace(lblothgrpname.Text.Trim, lsTextName.Trim(), _
                              Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                          RegexOptions.IgnoreCase)

                        End If


                    Next
                End If
            End If



        End If
    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        '  FillGrid(e.SortExpression, direction)
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

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try

            Dim otypecode1 As String
            Dim otypecode2 As String

            otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
            otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")
            If gv_SearchResult.Rows.Count <> 0 Then


                If Session("OthTypeFilter") = "OTH" Then
                    strSqlQry = "SELECT othgrpcode AS [Group Code],othgrpname AS [Group Name],othmaingrpmast.othmaingrpname as [Excursion Main Group Name],deptcode as [Department],[Active]=case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,othgrpmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othgrpmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othgrpmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othgrpmast.adddate))+ ':' + Convert(Varchar, Datepart(m,othgrpmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othgrpmast.adddate))) as [Date Created],othgrpmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,othgrpmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othgrpmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othgrpmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othgrpmast.moddate))+ ':' + Convert(Varchar, Datepart(m,othgrpmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,othgrpmast.moddate))) as [Date Modified] FROM othgrpmast,othmaingrpmast where othgrpmast.othmaingrpcode=othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode not in('" & otypecode1 & "'" & ",'" & otypecode2 & "')"
                Else

                    strSqlQry = "SELECT othgrpcode AS [Group Code],othgrpname AS [Group Name],othmaingrpmast.othmaingrpname as [Excursion Main Group Name],deptcode as [Department],[Active]=case when othgrpmast.active=1 then 'Active' when othgrpmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,othgrpmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othgrpmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othgrpmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othgrpmast.adddate))+ ':' + Convert(Varchar, Datepart(m,othgrpmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othgrpmast.adddate))) as [Date Created],othgrpmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,othgrpmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othgrpmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othgrpmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othgrpmast.moddate))+ ':' + Convert(Varchar, Datepart(m,othgrpmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,othgrpmast.moddate))) as [Date Modified] FROM othgrpmast,othmaingrpmast where othgrpmast.othmaingrpcode=othmaingrpmast.othmaingrpcode and othmaingrpmast.othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "')"

                End If
                'If Trim(BuildCondition) <> "" Then
                '    strSqlQry = strSqlQry & " and " & BuildCondition() & " ORDER BY othgrpcode "
                'Else
                '    strSqlQry = strSqlQry & " ORDER BY othgrpcode"
                'End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "othgrpmast")

                objUtils.ExportToExcel(DS, Response)
                con.Close()

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'Session("ColReportParams") = Nothing
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Other Service Group&BackPageName=OtherServiceGroupsSearch.aspx&Type=" + Session("OthTypeFilter") + "','RepOthGrp');"
            '' strpop = "window.open('rptReportNew.aspx?Pageame=Other Service Group&BackPageName=OtherServiceGroupsSearch.aspx&OthgrpCode=" & txtOtherCode.Value.Trim & "&OthgrpName=" & txtOtherName.Value.Trim & "&DeptCode=" & Trim(ddlDeptCode.Items(ddlDeptCode.SelectedIndex).Text) & "&DeptName=" & Trim(ddlDeptName.Items(ddlDeptName.SelectedIndex).Text) & "&MainGroupCode=" & Trim(ddlMainGroupCode.Items(ddlMainGroupCode.SelectedIndex).Text) & "&MainGroupName=" & Trim(ddlMainGroupName.Items(ddlMainGroupName.SelectedIndex).Text) & "&Type=" + Session("OthTypeFilter") + "','RepOthGrp');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceGroupsSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'lblMainGroupCode.Visible = False
        'lblMainGroupName.Visible = False
        'ddlMainGroupCode.Visible = False
        'ddlMainGroupName.Visible = False
        'lblDeptName.Visible = False
        'ddlDeptName.Visible = False
        'lblDeptCode.Visible = False
        'ddlDeptCode.Visible = False


    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'lblMainGroupCode.Visible = True
        'lblMainGroupName.Visible = True
        'ddlMainGroupCode.Visible = True
        'ddlMainGroupName.Visible = True
        'lblDeptName.Visible = True
        'ddlDeptName.Visible = True

        'lblDeptCode.Visible = True
        'ddlDeptCode.Visible = True
        'If Session("OthTypeFilter") = "OTH" Then
        '    ddlMainGroupCode.Visible = False
        '    ddlMainGroupName.Visible = False
        '    lblMainGroupCode.Visible = False
        '    lblMainGroupName.Visible = False
        'End If
    End Sub
    Private Sub fillorderby()
        'ddlOrderBy.Items.Clear()
        'ddlOrderBy.Items.Add("Group Name")
        'ddlOrderBy.Items.Add("Group Code")
        'ddlOrderBy.Items.Add("Department Code")
        'ddlOrderBy.SelectedIndex = 0
    End Sub

    'Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
    '    'Select Case ddlOrderBy.SelectedIndex
    '    '    Case 0
    '    '        FillGrid("othgrpname")
    '    '    Case 1
    '    '        FillGrid("othgrpcode")
    '    '    Case 2
    '    '        FillGrid("deptcode")

    '    'End Select

    'End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceGroupsSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click

        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CostCenterCodeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub
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

                Case "GROUP"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("GROUP", lsProcessCity, "GRP")
         
                Case "TEXT"
                    lsProcessAll = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("TEXT", lsProcessAll, "TEXT")
            End Select

        Next

        Dim dtt As DataTable
        dtt = Session("sDtDynamicOthGrps")
        dlList.DataSource = dtt
        dlList.DataBind()

        FillGridNew() 'Bind Gird based selection 

    End Sub

    Protected Sub RowsPerPageCUS_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
End Class
