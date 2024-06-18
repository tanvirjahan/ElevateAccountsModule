

Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class SupHotelAmenitiesSearch
    Inherits System.Web.UI.Page
#Region "Enum GridCol"
    Enum GridCol
        CodeTCol = 0
        Code = 1
        Name = 2
        Type = 3
        Active = 4
        DateCreated = 5
        UserCreated = 6
        DateModified = 7
        UserModified = 8
        Edit = 9
        View = 10
        Delete = 11
    End Enum
#End Region
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                RowsPerPageMPS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                'SetFocus(txtFromDate)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "PriceListModule\SupHotelAmenitiesSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                '' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)
                '' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSSPTypeCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypename", True)


                Session.Add("strsortExpression", "iCode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '' Create a Dynamic datatable ---- Start
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

                '' ''fillorderby()
                FillGrid("iCode")
                FillGridNew()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    '' ''ddlSSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '' ''ddlSSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        '' ''btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        '' ''If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "MealplanWindowPostBack") Then
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AminityWindowPostBack") Then
            '' ''btnSearch_Click(sender, e)
        End If
        '' ''Page.Title = "MealPlans Search"
    End Sub
#End Region
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lbliName As Label = e.Row.FindControl("lbliName")

            Dim lsiName As String = ""


            Dim dtDynamics As New DataTable
            dtDynamics = Session("sDtDynamic")
            If Session("sDtDynamic") IsNot Nothing Then
                If dtDynamics.Rows.Count > 0 Then
                    Dim j As Integer
                    For j = 0 To dtDynamics.Rows.Count - 1
                        lsiName = ""

                        If "AMENITYNAME" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsiName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "AMENITYTYPE" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsiName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If
                        If "TEXT" = dtDynamics.Rows(j)("code").ToString.Trim.ToUpper Then
                            lsiName = dtDynamics.Rows(j)("value").ToString.Trim.ToUpper
                        End If

                        If lsiName.Trim <> "" Then
                            lbliName.Text = Regex.Replace(lbliName.Text.Trim, lsiName.Trim(), _
                                Function(match As Match) String.Format("<span style = 'background-color:#ffcc99'>{0}</span>", match.Value), _
                                            RegexOptions.IgnoreCase)
                        End If

                    Next
                End If
            End If



        End If




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
            strSqlQry = "SELECT TB_HotelAmenitiesMaster.AmenityCode AS iCode,AmenityName As iName,TB_HotelAmenitiesMaster.AmenityTypeCode AmenityTypecode,[Active]=case when TB_HotelAmenitiesMaster.active=1 then 'Active' when TB_HotelAmenitiesMaster.active=0 then 'In Active' end," +
                            " TB_HotelAmenitiesMaster.adddate, TB_HotelAmenitiesMaster.adduser, TB_HotelAmenitiesMaster.moddate, TB_HotelAmenitiesMaster.moduser, TB_HotelAmenitiesMaster.rankorder" +
                            " FROM TB_HotelAmenitiesMaster INNER JOIN TB_AmenityTypeMaster ON TB_AmenityTypeMaster.AmenityTypeCode=TB_HotelAmenitiesMaster.AmenityTypeCode	left join  sptypemast on TB_HotelAmenitiesMaster.sptypecode=sptypemast.sptypecode"
            strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
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
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
        End Try

    End Sub
#End Region

    Protected Sub btnvsprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnvsprocess.Click
      
        Try
            FilterGrid()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('SupHotelAmenitiesAdd.aspx?State=New','Amenities');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid(Session("strsortexpression"), "")
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

            lblId.Text = lblId.Text.Trim().Replace("+", "%252b")

            If e.CommandName = "EditRow" Then
                
                Dim strpop As String = ""
                strpop = "window.open('SupHotelAmenitiesAdd.aspx?State=Edit&RefCode=" & CType(lblId.Text, String) & "','Amenityplans');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                Dim strpop As String = ""
                strpop = "window.open('SupHotelAmenitiesAdd.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Amenityplans');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                Dim strpop As String = ""
                strpop = "window.open('SupHotelAmenitiesAdd.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Amenityplans');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region


#Region " Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        ''FillGrid(e.SortExpression, direction)

        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), Session("strsortdirection"))

        myDS = gv_SearchResult.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression").ToString().Replace("TB_HotelAmenitiesMaster.", "") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
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
                
                strSqlQry = "SELECT TB_HotelAmenitiesMaster.AmenityCode AS iCode,AmenityName As iName,TB_HotelAmenitiesMaster.AmenityTypeCode AmenityTypecode,[Active]=case when TB_HotelAmenitiesMaster.active=1 then 'Active' when TB_HotelAmenitiesMaster.active=0 then 'In Active' end," +
                            " TB_HotelAmenitiesMaster.adddate, TB_HotelAmenitiesMaster.adduser, TB_HotelAmenitiesMaster.moddate, TB_HotelAmenitiesMaster.moduser, TB_HotelAmenitiesMaster.rankorder" +
                            " FROM TB_HotelAmenitiesMaster INNER JOIN TB_AmenityTypeMaster ON TB_AmenityTypeMaster.AmenityTypeCode=TB_HotelAmenitiesMaster.AmenityTypeCode	left join  sptypemast on TB_HotelAmenitiesMaster.sptypecode=sptypemast.sptypecode"
                strSqlQry = strSqlQry & " ORDER BY " & Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))


                'strSqlQry = "SELECT mealmast.mealcode AS [Meal Plan Code],mealmast.mealname AS [Meal Plan Name],[Active]=case when mealmast.active=1 then 'Active' when mealmast.active=0 then 'InActive' end,(Convert(Varchar, Datepart(DD,mealmast.adddate))+ '/'+ Convert(Varchar, Datepart(MM,mealmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,mealmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,mealmast.adddate))+ ':' + Convert(Varchar, Datepart(m,mealmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,mealmast.adddate))) as [Date Created],mealmast.moduser as [User Modified] ,(Convert(Varchar, Datepart(DD,mealmast.moddate))+ '/'+ Convert(Varchar, Datepart(MM,mealmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,mealmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,mealmast.moddate))+ ':' + Convert(Varchar, Datepart(m,mealmast.moddate))+ ':'+ Convert(Varchar, Datepart(ss,mealmast.moddate))) as [Date Modified] FROM mealmast left join sptypemast on mealmast.sptypecode =sptypemast.sptypecode"
                'strSqlQry = strSqlQry & " ORDER BY mealcode"
                '' ''If Trim(BuildCondition) <> "" Then
                '' ''    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY mealcode "
                '' ''Else
                '' ''    strSqlQry = strSqlQry & " ORDER BY mealcode"
                '' ''End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS)

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            con.Close()
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Try
            Session("ColReportParams") = Nothing
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            Dim strpop As String = ""
            '*** Danny NEED this LINE
            'strpop = "window.open('rptReportNew.aspx?Pageame=Meal Plan&BackPageName=SupHotelAmenitiesSearch.aspx&MealCode=" & txtCode.Text.Trim & "&MealName=" & txtName.Text.Trim & "&SuptypeCode=" & Trim(ddlSSPTypeCode.Items(ddlSSPTypeCode.SelectedIndex).Text) & "&SuptypeName=" & Trim(ddlSSPTypeName.Items(ddlSSPTypeName.SelectedIndex).Text) & "','RepMealPlan');"
            strpop = "window.open('rptReportNew.aspx?Pageame=Hotel Amenities&BackPageName=SupHotelAmenitiesSearch.aspx&AmenityCode=" & String.Empty & "&AmenityName=" & String.Empty & "&SuptypeCode=" & String.Empty & "&SuptypeName=" & String.Empty & "','RepAmenities');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

           

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region




    Private Sub FilterGrid()
        Dim lsSearchTxt As String = ""
        lsSearchTxt = txtvsprocesssplit.Text
        Dim lsProcessName As String = ""
        Dim lsProcessCountry As String = ""
        Dim lsProcessGroup As String = ""
        Dim lsProcessSector As String = ""
        Dim lsProcessAll As String = ""



        Dim lsMainArr As String()
        lsMainArr = objUtils.splitWithWords(lsSearchTxt, "|~,")
        For i = 0 To lsMainArr.GetUpperBound(0)
            Select Case lsMainArr(i).Split(":")(0).ToString.ToUpper.Trim
                Case "AMENITYNAME"
                    lsProcessName = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AMENITYNAME", lsProcessName, "AMENITYNAME")
                Case "AMENITYTYPE"
                    lsProcessName = lsMainArr(i).Split(":")(1)
                    sbAddToDataTable("AMENITYTYPE", lsProcessName, "AMENITYTYPE")

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

    Private Sub FillGridNew()
        ' Dim strRowsPerPage = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Dim dtt As DataTable
        dtt = Session("sDtDynamic")
        Dim strNameValue As String = ""
        Dim strTypeValue As String = ""
        Dim strTextValue As String = ""
        Dim strBindCondition As String = ""
        Try
            If dtt.Rows.Count > 0 Then
                For i As Integer = 0 To dtt.Rows.Count - 1
                    If dtt.Rows(i)("Code").ToString = "AMENITYNAME" Then
                        If strNameValue <> "" Then
                            strNameValue = strNameValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strNameValue = "'" + dtt.Rows(i)("Value").ToString + "'"
                        End If
                    End If
                    If dtt.Rows(i)("Code").ToString = "AMENITYTYPE" Then
                        If strTypeValue <> "" Then
                            strTypeValue = strTypeValue + ",'" + dtt.Rows(i)("Value").ToString + "'"
                        Else
                            strTypeValue = "'" + dtt.Rows(i)("Value").ToString + "'"
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
            Dim pagevaluestmps = RowsPerPageMPS.SelectedValue
            strBindCondition = BuildConditionNew(strNameValue, strTypeValue, strTextValue)
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
            strSqlQry = "SELECT TB_HotelAmenitiesMaster.AmenityCode AS iCode,AmenityName AS iName,TB_HotelAmenitiesMaster.AmenityTypeCode AS AmenityTypecode ,[Active]=case when TB_HotelAmenitiesMaster.active=1 then 'Active' when TB_HotelAmenitiesMaster.active=0 then 'In Active' end, " +
                        " TB_HotelAmenitiesMaster.adddate, TB_HotelAmenitiesMaster.adduser, TB_HotelAmenitiesMaster.moddate, TB_HotelAmenitiesMaster.moduser, TB_HotelAmenitiesMaster.rankorder" +
                        " FROM TB_HotelAmenitiesMaster	TB_HotelAmenitiesMaster INNER JOIN TB_AmenityTypeMaster ON TB_AmenityTypeMaster.AmenityTypeCode=TB_HotelAmenitiesMaster.AmenityTypeCode	left join  sptypemast on TB_HotelAmenitiesMaster.sptypecode=sptypemast.sptypecode"

            If strBindCondition <> "" Then
                strSqlQry = strSqlQry & " WHERE " & strBindCondition & " ORDER BY " & strorderby & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            'Session("SSqlQuery") = strSqlQry
            'myDS = clsUtils.GetDetailsPageWise(1, 10, strSqlQry)
            gv_SearchResult.DataSource = myDS
            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.PageSize = pagevaluestmps
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
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




    Private Function BuildConditionNew(ByVal strNameValue As String, ByVal strTypeValue As String, ByVal strTextValue As String) As String
        strWhereCond = ""
        If strNameValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(TB_HotelAmenitiesMaster.AmenityName) IN (" & Trim(strNameValue.Trim.ToUpper) & ")"
                'Else
                '    strWhereCond = strWhereCond & " AND upper(sectormaster.sectorname) IN (" & Trim(strSectorValue.Trim.ToUpper) & ")"
            End If
        End If
        If strTypeValue.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = "upper(TB_HotelAmenitiesMaster.AmenityTypecode) IN (" & Trim(strTypeValue.Trim.ToUpper) & ")"
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
                        strWhereCond1 = "(upper(TB_HotelAmenitiesMaster.AmenityName) LIKE '%" & Trim(strValue.Trim.ToUpper) & "%')"
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

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),TB_HotelAmenitiesMaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),TB_HotelAmenitiesMaster.adddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            ElseIf ddlOrder.SelectedValue = "M" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " (CONVERT(datetime, convert(varchar(10),TB_HotelAmenitiesMaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                Else
                    strWhereCond = strWhereCond & "  and (CONVERT(datetime, convert(varchar(10),TB_HotelAmenitiesMaster.moddate,103),103)  between CONVERT(datetime, '" + txtFromDate.Text + "',103) and CONVERT(datetime,  '" + txtToDate.Text + "',103)) "
                End If
            End If
        End If
        BuildConditionNew = strWhereCond
    End Function


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AmenitySearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function getRowpage() As String
        Dim rowpagestmps As String
        If RowsPerPageMPS.SelectedValue = "20" Then
            rowpagestmps = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
        Else
            rowpagestmps = RowsPerPageMPS.SelectedValue

        End If
        Return rowpagestmps
    End Function
    Protected Sub RowsPerPageSTS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageMPS.SelectedIndexChanged
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
    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            FillGridNew()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupHotelAmenitiesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnClearDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDate.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        FillGridNew()
    End Sub
    '*** Follwing code was commented because was not used >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    '' ''#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    '' ''    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '' ''        'FillGrid("mealcode")
    '' ''        Select Case ddlOrderBy.SelectedIndex
    '' ''            Case 0
    '' ''                FillGrid("mealname")
    '' ''            Case 1
    '' ''                FillGrid("mealcode")

    '' ''            Case 2
    '' ''                FillGrid("mealmast.rankorder")

    '' ''        End Select
    '' ''    End Sub

    '' ''#End Region

    '' ''#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    '' ''    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    '' ''        txtCode.Text = ""
    '' ''        txtName.Text = ""
    '' ''        ddlSSPTypeCode.Value = "[Select]"
    '' ''        ddlSSPTypeName.Value = "[Select]"
    '' ''        ddlOrderBy.SelectedIndex = 0
    '' ''        FillGrid("mealname")
    '' ''    End Sub
    '' ''#End Region
    '' ''Protected Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
    '' ''    FillGrid("mealmast.mealname")
    '' ''End Sub

    '' ''Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
    '' ''    Select Case ddlOrderBy.SelectedIndex
    '' ''        Case 0
    '' ''            FillGrid("mealname")
    '' ''        Case 1
    '' ''            FillGrid("mealcode")


    '' ''    End Select
    '' ''End Sub

    '' ''Private Sub fillorderby()
    '' ''    ddlOrderBy.Items.Clear()
    '' ''    ddlOrderBy.Items.Add("Amenity Name")
    '' ''    ddlOrderBy.Items.Add("Amenity Code")
    '' ''    ddlOrderBy.Items.Add("Rank Order")
    '' ''    ddlOrderBy.SelectedIndex = 0
    '' ''End Sub

    '' ''<System.Web.Script.Services.ScriptMethod()> _
    '' '' <System.Web.Services.WebMethod()> _
    '' ''Public Shared Function GetMeals(ByVal prefixText As String) As List(Of String)

    '' ''    Dim strSqlQry As String = ""
    '' ''    Dim myDS As New DataSet
    '' ''    Dim MealNames As New List(Of String)
    '' ''    Try

    '' ''        strSqlQry = "select mealname from mealmast where mealname like  " & "'%" & prefixText & "%'"
    '' ''        Dim SqlConn As New SqlConnection
    '' ''        Dim myDataAdapter As New SqlDataAdapter
    '' ''        ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
    '' ''        SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
    '' ''        'Open connection
    '' ''        myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '' ''        myDataAdapter.Fill(myDS)

    '' ''        If myDS.Tables(0).Rows.Count > 0 Then
    '' ''            For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
    '' ''                MealNames.Add(myDS.Tables(0).Rows(i)("mealname").ToString())
    '' ''            Next

    '' ''        End If

    '' ''        Return MealNames
    '' ''    Catch ex As Exception
    '' ''        Return MealNames
    '' ''    End Try

    '' ''End Function

    '' ''#Region "  Private Function BuildCondition() As String"
    '' ''    Private Function BuildCondition() As String
    '' ''        strWhereCond = ""
    '' ''        If txtCode.Text.Trim <> "" Then
    '' ''            If Trim(strWhereCond) = "" Then

    '' ''                strWhereCond = " upper(mealmast.mealcode) LIKE '" & Trim(txtCode.Text.Trim.ToUpper) & "%'"
    '' ''            Else
    '' ''                strWhereCond = strWhereCond & " AND upper(mealmast.mealcode) LIKE '" & Trim(txtCode.Text.Trim.ToUpper) & "%'"
    '' ''            End If
    '' ''        End If

    '' ''        If txtName.Text.Trim <> "" Then
    '' ''            If Trim(strWhereCond) = "" Then

    '' ''                strWhereCond = " upper(mealmast.mealname) LIKE '" & Trim(txtName.Text.Trim.ToUpper) & "%'"
    '' ''            Else
    '' ''                strWhereCond = strWhereCond & " AND upper(mealmast.mealname) LIKE '" & Trim(txtName.Text.Trim.ToUpper) & "%'"
    '' ''            End If
    '' ''        End If


    '' ''        BuildCondition = strWhereCond
    '' ''    End Function
    '' ''#End Region



    '*** Follwing code was commented because was not used <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
End Class

