'------------================--------------=======================------------------================
'   Module Name    :    DepartmentMasterSearch.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DepartmentMasterSearch
    Inherits System.Web.UI.Page
#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region
#Region "Enum GridCol"
    Enum GridCol
        DepartmentCodeTCol = 0
        DepartmentCode = 1
        DepartmentName = 2
        DeptHead = 3
        Phone = 4
        Fax = 5
        EmailID = 6
        url = 7
        ShowinReservation = 8
        Active = 9
        DateCreated = 10
        UserCreated = 11
        DateModified = 12
        UserModified = 13
        Edit = 14
        View = 15
        Delete = 16
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


#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            lblcode.Visible = False
            lblname.Visible = False
            ddlDeptheadcode.Visible = False
            ddlDeptheadname.Visible = False
            Try

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


                RowsPerPageCS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub

                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(Session("AppName"), String), "UserAdminModule\DepartmentMasterSearch.aspx?appid=" + strappid, BtnAddNew, BtnExportToExcel, _
                                                       BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)


                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptheadcode, "usercode", "username", "select usercode,username from usermaster  where active=1 order by usercode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptheadname, "username", "usercode", "select username,usercode from usermaster  where active=1 order by username", True)

                Session.Add("DepartmentstrsortExpression", "Deptcode")
                Session.Add("Departmentstrsortdirection", SortDirection.Ascending)
                Session("sDtDynamic") = Nothing
                Dim dtDynamic = New DataTable()
                Dim dcCode = New DataColumn("Code", GetType(String))
                Dim dcValue = New DataColumn("Value", GetType(String))
                Dim dcCodeAndValue = New DataColumn("CodeAndValue", GetType(String))
                dtDynamic.Columns.Add(dcCode)
                dtDynamic.Columns.Add(dcValue)
                dtDynamic.Columns.Add(dcCodeAndValue)
                Session("sDtDynamic") = dtDynamic

                FillGridNew()

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlDeptheadcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlDeptheadname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        FillGridNew()

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "DeptWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
#End Region
#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Try
            strWhereCond = ""
            If txtcode.Value.Trim <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(DeptMaster.Deptcode) LIKE '" & Trim(txtcode.Value.Trim.ToUpper) & "%'"
                Else
                    strWhereCond = strWhereCond & " AND upper(DeptMaster.Deptcode) LIKE '" & Trim(txtcode.Value.Trim.ToUpper) & "%'"
                End If
            End If

            If txtName.Value.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(DeptMaster.DeptName) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
                Else
                    strWhereCond = strWhereCond & " AND upper(DeptMaster.DeptName) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
                End If
            End If
            If ddlDeptheadcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " UserMaster.UserCode = '" & Trim(ddlDeptheadcode.Items(ddlDeptheadcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND UserMaster.UserCode  = '" & Trim(ddlDeptheadcode.Items(ddlDeptheadcode.SelectedIndex).Text) & "'"
                End If
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
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
            strSqlQry = " SELECT  DeptMaster.Deptcode,DeptMaster.DeptName,[Active]=case DeptMaster.active when 1 then 'Active' else 'In Active' end ," & _
                        " DeptMaster.AddDate, DeptMaster.AddUser,DeptMaster.ModDate, DeptMaster.ModUser," & _
                        " [ShowinReservation]=case DeptMaster.showinresn when 1 then 'Yes' else 'No' end ,DeptMaster.email, UserMaster.username as depthead, " & _
                         "  DeptMaster.phone, DeptMaster.fax, DeptMaster.URL " & _
                         " FROM DeptMaster left outer JOIN UserMaster ON DeptMaster.depthead = UserMaster.UserCode "

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " Where " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region


    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "EditRow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DepartmentMaster.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('DepartmentMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Departments','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DepartmentMaster.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Departments');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DepartmentMaster.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('DepartmentMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Departments','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DepartmentMaster.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("DepartmentMaster.aspx", False)
                Dim strpop As String = ""
                ' strpop = "window.open('DepartmentMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Departments','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('DepartmentMaster.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("DepartmentstrsortExpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("DepartmentstrsortExpression"), "")

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("Departmentstrsortdirection", objUtils.SwapSortDirection(Session("Departmentstrsortdirection")))
            dataView.Sort = Session("DepartmentstrsortExpression") & " " & objUtils.ConvertSortDirectionToSql(Session("Departmentstrsortdirection"))
            gv_SearchResult.DataSource = dataView
            gv_SearchResult.DataBind()
        End If
    End Sub
#End Region

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("Deptcode")
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtcode.Value = ""
        txtName.Value = ""
        ddlDeptheadcode.Value = "[Select]"
        ddlDeptheadname.Value = "[Select]"
        FillGrid("Deptcode")
    End Sub

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As New SqlConnection

        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "SELECT  DeptMaster.Deptcode as [Department Code],DeptMaster.DeptName as [Department Name]," & _
                            " UserMaster.username as [Dept Head], DeptMaster.phone as [Phone],DeptMaster.fax as [Fax]," & _
                            " DeptMaster.email as [Email ID],DeptMaster.URL as [URL],[Show In Reservation]=case when DeptMaster.showinresn=1 then 'Yes' when DeptMaster.showinresn=0 then 'No' end," & _
                            " [Active]=case DeptMaster.active when 1 then 'Active' else 'In Active' end ,(Convert(Varchar, Datepart(DD,DeptMaster.adddate))+ '/'+ Convert(Varchar, Datepart(MM,DeptMaster.adddate))+ '/'+ Convert(Varchar, Datepart(YY,DeptMaster.adddate)) + ' ' + Convert(Varchar, Datepart(hh,DeptMaster.adddate))+ ':' + Convert(Varchar, Datepart(m,DeptMaster.adddate))+ ':'+ Convert(Varchar, Datepart(ss,DeptMaster.adddate))) as [Date Created],DeptMaster.AddUser as [User Created], " & _
                            " (Convert(Varchar, Datepart(DD,DeptMaster.moddate))+ '/'+ Convert(Varchar, Datepart(MM,DeptMaster.moddate))+ '/'+ Convert(Varchar, Datepart(YY,DeptMaster.moddate)) + ' ' + Convert(Varchar, Datepart(hh,DeptMaster.moddate))+ ':' + Convert(Varchar, Datepart(m,DeptMaster.moddate))+ ':'+ Convert(Varchar, Datepart(ss,DeptMaster.moddate))) as [Date Modified],DeptMaster.moduser as [User Modified]" & _
                            " FROM DeptMaster left outer JOIN UserMaster ON DeptMaster.depthead = UserMaster.UserCode "


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " Where " & BuildCondition() & "ORDER BY DeptMaster.Deptcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY DeptMaster.Deptcode"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "dept")

                objUtils.ExportToExcel(DS, Response)

            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        Finally
            clsDBConnect.dbConnectionClose(con)
        End Try
    End Sub
#End Region

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblcode.Visible = True
        lblname.Visible = True
        ddlDeptheadcode.Visible = True
        ddlDeptheadname.Visible = True

    End Sub

    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblcode.Visible = False
        lblname.Visible = False
        ddlDeptheadcode.Visible = False
        ddlDeptheadname.Visible = False

    End Sub

#Region " Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click"
    Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("DepartmentMaster.aspx", False)
        Dim strpop As String = ""
        'strpop = "window.open('DepartmentMaster.aspx?State=New','Departments','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('DepartmentMaster.aspx?State=New');"

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try

            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            'Session.Add("Pageame", "Department Master")
            'Session.Add("BackPageName", "DepartmentMasterSearch.aspx")

            Dim strpop As String = ""
            'strpop = "window.open('rptReportNew.aspx?Pageame=Department Master&BackPageName=DepartmentMasterSearch.aspx&DeptCode=" & txtcode.Value.Trim & "&DeptName=" & txtName.Value.Trim & "&DeptHead=" & Trim(ddlDeptheadcode.Items(ddlDeptheadcode.SelectedIndex).Text) & "','DeptMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Department Master&BackPageName=DepartmentMasterSearch.aspx&DeptCode=" & txtcode.Value.Trim & "&DeptName=" & txtName.Value.Trim & "&DeptHead=" & Trim(ddlDeptheadcode.Items(ddlDeptheadcode.SelectedIndex).Text) & "');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DepartmentMasterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click


        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

    Private Sub FillGridNew()
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")

        Dim strDeptHeadValue As String = ""
        Dim strDeptValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "DEPARTMENTNAME" Then
                        If strDeptValue <> "" Then
                            strDeptValue = strDeptValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDeptValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                   
                    If dtt.Rows(i)("Code").ToString = "DEPARTMENTHEAD" Then
                        If strDeptHeadValue <> "" Then
                            strDeptHeadValue = strDeptHeadValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strDeptHeadValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                            If dtt.Rows(i)("Code").ToString = "TEXT" Then
                                If strTextValue <> "" Then
                                    strTextValue = strTextValue + "," + dtt.Rows(i)("Value").ToString + ""
                                Else
                                    strTextValue = "" + dtt.Rows(i)("Value").ToString + ""
                                End If
                            End If
                        Next
                    End If

            Dim pagevaluecs = RowsPerPageCS.SelectedValue

            strBindCondition = BuildConditionNew(strDeptValue, strDeptHeadValue, strTextValue)
                    Dim myDS As New DataSet

                    gv_SearchResult.Visible = True
                    lblMsg.Visible = False
                    If gv_SearchResult.PageIndex < 0 Then

                        gv_SearchResult.PageIndex = 0
                    End If
                    strSqlQry = " SELECT  DeptMaster.Deptcode,DeptMaster.DeptName,[Active]=case DeptMaster.active when 1 then 'Active' else 'In Active' end ," & _
                                " DeptMaster.AddDate, DeptMaster.AddUser,DeptMaster.ModDate, DeptMaster.ModUser," & _
                                " [ShowinReservation]=case DeptMaster.showinresn when 1 then 'Yes' else 'No' end ,DeptMaster.email, UserMaster.username as depthead, " & _
                                 " DeptMaster.phone, DeptMaster.fax, DeptMaster.URL " & _
                                 " FROM DeptMaster left outer JOIN UserMaster ON DeptMaster.depthead = UserMaster.UserCode "

                    Dim strorderby As String = Session("DepartmentstrsortExpression")
                    Dim strsortorder As String = "ASC"

                    If strBindCondition <> "" Then
                        strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & strsortorder
                    Else
                        strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                    End If
                    SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                    myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                    myDataAdapter.Fill(myDS)

                    gv_SearchResult.DataSource = myDS
                    If myDS.Tables(0).Rows.Count > 0 Then
                        gv_SearchResult.PageSize = pagevaluecs
                        gv_SearchResult.DataBind()
                    Else
                        gv_SearchResult.PageIndex = 0
                        gv_SearchResult.DataBind()
                        lblMsg.Visible = True
                        lblMsg.Text = "Records not found, Please redefine search criteria."
                    End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub

    Private Function BuildConditionNew(ByVal strDeptValue As String, ByVal strDeptHeadValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strDeptValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(DeptMaster.DeptName) IN (" & Trim(strDeptValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(DeptMaster.DeptName) IN (" & Trim(strDeptValue.Trim.ToUpper) & ")"
            End If

        End If
        If strDeptHeadValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(UserMaster.username) IN (" & Trim(strDeptHeadValue.Trim.ToUpper) & ")"
            Else
                strWhereCond = strWhereCond & " AND upper(UserMaster.username) IN (" & Trim(strDeptHeadValue.Trim.ToUpper) & ")"
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

                        strWhereCond1 = "upper(DeptMaster.Deptcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(DeptMaster.DeptName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(UserMaster.username) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
                    Else
                        strWhereCond1 = strWhereCond1 & " OR  upper(DeptMaster.Deptcode) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(DeptMaster.DeptName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%' or upper(UserMaster.username) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%'"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),DeptMaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),DeptMaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),DeptMaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),DeptMaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If

            End If
        End If


        BuildConditionNew = strWhereCond
    End Function

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





    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub

    Public Function getRowpage() As String
        Dim rowpagecs As String
        If RowsPerPageCS.SelectedValue = "20" Then
            rowpagecs = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagecs = RowsPerPageCS.SelectedValue

        End If
        Return rowpagecs
    End Function

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                Case "DEPARTMENTNAME"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DEPARTMENTNAME", lsProcessCity, "DEPARTMENTNAME")
                Case "DEPARTMENTHEAD"
                    lsProcessCity = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("DEPARTMENTHEAD", lsProcessCity, "DEPARTMENTHEAD")


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

    Protected Sub RowsPerPageCS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCS.SelectedIndexChanged
        FillGridNew()
    End Sub
    Protected Sub btnResetSelection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetSelection.Click
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
End Class

