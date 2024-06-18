#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class PriceListModule_VehicletypeSearch
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim objUser As New clsUser
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    'Dim FindPage As String
#End Region

#Region "Enum GridCol"
    Enum GridCol
        VTCodeTCol = 0
        Code = 1
        Name = 2
        GroupCode = 3
        GroupName = 4
        RankOrder = 5
        MinPax = 6
        MaxPax = 7
        'UnitName = 8
        'PrintRemarks = 9
        PaxCheckRequired = 8
        Capacity = 9
        Options = 10
        Active = 11
        'CalculateByPaxUnits = 12
        DateCreated = 12
        UserCreated = 13
        DateModified = 14
        UserModified = 15
        Edit = 16
        View = 17
        Delete = 18

    End Enum
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '    Session("VehicleType") = "Yes"
        '    Response.Redirect("OtherservicecategoriesSearch.aspx?VehiclePage=Yes", False)
        If Page.IsPostBack = False Then
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
               
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)

                Dim strQry As String = "select othgrpcode from othcatmast where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters " & _
                       "   Where Param_Id='1001') "
                hdnGrpCode.Value = (objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry))
                SetFocus(txtCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(strappname, String),
                                           "PriceListModule\VehicleTypeSearch.aspx?appid=1", btnAddNew, btnExportToExcel, btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete,
                                           GridCol.View)

                End If
                Session.Add("strsortExpression", "othcatmast.othcatcode") 'for vehicle type, saving value to same 'othcatmast' table
                Session.Add("strsortdirection", SortDirection.Ascending)

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGrpCode, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
                fillorderby()
                FillGrid("othcatmast.othcatname")
                charcters(txtCode)
                charcters(txtName)
                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                '    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("VehicleTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OthcatWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If

    End Sub

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtCode.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(othcatmast.othcatcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othcatmast.othcatcode) LIKE '" & Trim(txtCode.Value.Trim.ToUpper) & "%'"
            End If
        End If

        If txtName.Value.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(othcatmast.othcatname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(othcatmast.othcatname) LIKE '" & Trim(txtName.Value.Trim.ToUpper) & "%'"
            End If
        End If
        
        'If ddlOtherGrpCode.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " upper(othgrpmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND upper(othgrpmast.othgrpcode)= '" & Trim(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If

        'If ddlOtherGrpName.Value <> "[Select]" Then
        '    If Trim(strWhereCond) = "" Then

        '        strWhereCond = " upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    Else
        '        strWhereCond = strWhereCond & " AND  upper(othgrpmast.othgrpname)= '" & Trim(ddlOtherGrpName.Items(ddlOtherGrpName.SelectedIndex).Text.Trim.ToUpper) & "'"
        '    End If
        'End If
        BuildCondition = strWhereCond
    End Function
#End Region

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
            ' strSqlQry = "SELECT * FROM othtypmast"
            'strSqlQry = " SELECT     dbo.othcatmast.othcatcode, dbo.othcatmast.othcatname,case isnull(dbo.othcatmast.adultchild,'k') when 'A' then 'adult' when 'C' then 'child' when 'K' then 'nil' end adultchild , dbo.othcatmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othcatmast.grporder,  " & _
            '          "dbo.othcatmast.minpax, dbo.othcatmast.maxpax, dbo.othcatmast.unitname,case when isnull(dbo.othcatmast.printremarks,0)=1 then 'Yes' else 'No' end as printremarks, case when isnull(dbo.othcatmast.paxcheckreqd,0)=1 then 'Yes' when isnull(dbo.othcatmast.paxcheckreqd,0)=0 then 'No' end as paxcheckreqd, " & _
            '          "case when isnull(dbo.othcatmast.active,0)=1 then 'Active' when isnull(dbo.othcatmast.active,0)=0 then 'InActive' else 'InActive' end Active , " & _
            '          "dbo.othcatmast.calcyn, dbo.othcatmast.adddate, dbo.othcatmast.adduser, dbo.othcatmast.moddate, " & _
            '           "dbo.othcatmast.moduser FROM         dbo.othcatmast INNER JOIN   dbo.othgrpmast ON dbo.othcatmast.othgrpcode = dbo.othgrpmast.othgrpcode "



            'strSqlQry = " SELECT  dbo.othcatmast.othcatcode, dbo.othcatmast.othcatname, " & _
            '"  dbo.othcatmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othcatmast.grporder,  dbo.othcatmast.minpax, " & _
            '"  dbo.othcatmast.maxpax,  case when isnull(dbo.othcatmast.paxcheckreqd,0)=1 then 'Yes' when isnull(dbo.othcatmast.paxcheckreqd,0)=0 then 'No' end as paxcheckreqd," & _
            '"  dbo.othcatmast.capacity ,dbo.othcatmast.options ,case when isnull(dbo.othcatmast.active,0)=1 then 'Active' when isnull(dbo.othcatmast.active,0)=0 then " & _
            '" 'InActive' else 'InActive' end Active ,  dbo.othcatmast.adddate, dbo.othcatmast.adduser, dbo.othcatmast.moddate, dbo.othcatmast.moduser FROM dbo.othcatmast " & _
            '" INNER JOIN   dbo.othgrpmast ON dbo.othcatmast.othgrpcode =    dbo.othgrpmast.othgrpcode "

            strSqlQry = " SELECT  dbo.othcatmast.othcatcode, dbo.othcatmast.othcatname, " & _
            "  dbo.othcatmast.othgrpcode, dbo.othgrpmast.othgrpname, dbo.othcatmast.grporder,  dbo.othcatmast.minpax, " & _
            "  dbo.othcatmast.maxpax,  case when isnull(dbo.othcatmast.paxcheckreqd,0)=1 then 'Yes' when isnull(dbo.othcatmast.paxcheckreqd,0)=0 then 'No' end as paxcheckreqd," & _
            "  dbo.othcatmast.capacity ,dbo.othcatmast.options ,case when isnull(dbo.othcatmast.active,0)=1 then 'Active' when isnull(dbo.othcatmast.active,0)=0 then " & _
            " 'InActive' else 'InActive' end Active ,  dbo.othcatmast.adddate, dbo.othcatmast.adduser, dbo.othcatmast.moddate, dbo.othcatmast.moduser," & _
            "  case when othcatmast.shuttle=1 then 'Yes' else 'No' end shuttle FROM dbo.othcatmast " & _
            " INNER JOIN   dbo.othgrpmast ON dbo.othcatmast.othgrpcode =    dbo.othgrpmast.othgrpcode "




            If Trim(BuildCondition) <> "" Then
                ' strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                'If FindPage = "Yes" Then
                strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                'Else
                '    strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder

                'End If
            Else
                '  strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>'TRFS' "
                'If FindPage = "Yes" Then
                strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                'Else
                '    strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                '    strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
                'End If

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
            objUtils.WritErrorLog("VehicleTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("OtherServiceCategories.aspx", False)
        Dim strpop As String = ""
        strpop = "window.open('VehicleType.aspx?State=New','Othcat','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"


        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("othcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
                'Case 2
                '    FillGrid("othgrpmast.othgrpname")
                'Case 3
                '    FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblothtypcode")

            If e.CommandName = "EditRow" Then
                Dim strpop As String = ""
                strpop = "window.open('VehicleType.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','VehicleType','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                strpop = "window.open('VehicleType.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','VehicleType','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                strpop = "window.open('VehicleType.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','VehicleType','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VehicleTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("othcatcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
                'Case 2
                '    FillGrid("othgrpmast.othgrpname")
                'Case 3
                '    FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCode.Value = ""
        txtName.Value = ""
        ''ddlGrpCode.SelectedValue = "[Select]"
        ''ddlGrpNm.SelectedValue = "[Select]"
        'ddlOtherGrpCode.Value = "[Select]"
        'ddlOtherGrpName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        FillGrid("othcatmast.othcatname")
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
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

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                strSqlQry = "SELECT othcatcode AS [Category Code],othcatname AS [Category Name],othcatmast.othgrpcode as [Group Code],othgrpmast.othgrpname as [Group Name]," & _
                    " othcatmast.grporder as [Rank Order],othcatmast.minpax as [Min Pax],othcatmast.maxpax as [Max Pax],othcatmast.unitname as [Unit Name]," & _
                    " othcatmast.printremarks as [Print Remark],othcatmast.paxcheckreqd as [Pax Check Required],[shuttle]=case when othcatmast.shuttle=1 then 'Yes' else 'No' end ,[Active]=case when othcatmast.active=1 then 'Active' when " & _
                    " othcatmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,othcatmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,othcatmast.adddate))+ '/'+ " & _
                    " Convert(Varchar, Datepart(YY,othcatmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othcatmast.adddate))+ ':' + Convert(Varchar, " & _
                    " Datepart(m,othcatmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othcatmast.adddate))) as [Date Created],othcatmast.moduser as [User Modified] ," & _
                    " (Convert(Varchar, Datepart(DD,othcatmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,othcatmast.moddate))+ '/'+ " & _
                    " Convert(Varchar, Datepart(YY,othcatmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othcatmast.moddate))+ ':' + " & _
                    " Convert(Varchar, Datepart(m,othcatmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,othcatmast.moddate))) as [Date Modified]FROM othcatmast INNER JOIN " & _
                    " othgrpmast ON othcatmast.othgrpcode = othgrpmast.othgrpcode"
                
                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY  othcatcode"

                Else
                    ''  strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode<>'TRFS' "
                    'If FindPage = "Yes" Then
                    strSqlQry = strSqlQry & " Where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') "

                    strSqlQry = strSqlQry & " ORDER BY othcatcode"
                End If
            
            con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

            DA = New SqlDataAdapter(strSqlQry, con)
            DA.Fill(DS, "othcatmast")

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
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Vehicle Type&BackPageName=VehicleTypeSearch.aspx&OthcatCode=" & txtCode.Value.Trim & "&OthcatName=" & txtName.Value.Trim & "&OthgrpCode=" & hdnGrpCode.Value.Trim & "','RepVehicleType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlOtherCat.Visible = False
        'lblgrpcode.Visible = False
        'ddlOtherGrpCode.Visible = False
        'lblgrpname.Visible = False
        'ddlOtherGrpName.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlOtherCat.Visible = True
        'lblgrpcode.Visible = True
        'ddlOtherGrpCode.Visible = True
        'lblgrpname.Visible = True
        'ddlOtherGrpName.Visible = True
    End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Name")
        ddlOrderBy.Items.Add("Code")
        'ddlOrderBy.Items.Add("Group Name")
        'ddlOrderBy.Items.Add("Group Code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("othcatmast.othcatname")
            Case 1
                FillGrid("othcatmast.othcatcode")
                'Case 2
                '    FillGrid("othgrpmast.othgrpname")
                'Case 3
                '    FillGrid("othgrpmast.othgrpcode")

        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=VehicleTypeSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
