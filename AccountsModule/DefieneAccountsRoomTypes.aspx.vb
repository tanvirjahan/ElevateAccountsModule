
'------------================--------------=======================------------------================
'   Module Name    :    Define Accounts Room Types.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DefieneAccountsRoomTypes
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
    Dim sqlTrans As SqlTransaction
    Dim mySqlReader As SqlDataReader
    Dim gvRow As GridViewRow
    Dim objdate As New clsDateTime
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            chkSaveChanges.Checked = True
            Session.Add("SaveChanges", "Yes")
            Try
                SetFocus(ddlOrderby)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(Session("AppName"), String), "DefieneAccountsRoomTypes.aspx", BtnAddNew, BtnExportToExcel, _
                    '                                   BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If


                Page.Title = Page.Title + "-" + "Define Accounts - Room Types"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomcode, "rmtypcode", "rmtypname", "select rmtypcode,rmtypname from rmtypmast  where active=1 order by rmtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRoomname, "rmtypname", "rmtypcode", "select rmtypname,rmtypcode from rmtypmast  where active=1 order by rmtypname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpcode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast  where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPname, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast  where active=1 order by sptypename", True)

                Session.Add("strsortExpression", "rmtypcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillGrid("rmtypcode")



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefieneAccountsRoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
            btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            btnSaveselected.Attributes.Add("onclick", "javascript:if(confirm('Are you sure to save in all rows, this will change in the database directly and cannot be reverted? ')==false)return false;")
        Else
            If chkSaveChanges.Checked Then
                Session.Add("SaveChanges", "Yes")
            Else
                Session.Add("SaveChanges", "No")
            End If
        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlRoomcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRoomname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSpcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSPname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

        End If
    End Sub
#End Region

#Region "showgrid"
    Private Sub showgrid(ByVal strqry As String)
        Dim ddlInccode As HtmlSelect
        Dim ddlCostCode As HtmlSelect
        Dim ddlIncname As HtmlSelect
        Dim ddlCstname As HtmlSelect

        Dim ddlRefInccode As HtmlSelect
        Dim ddlRefCostCode As HtmlSelect
        Dim ddlRefIncname As HtmlSelect
        Dim ddlRefCstname As HtmlSelect

        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand(strqry, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("rmtypcode")) = False Then
                            If CType(mySqlReader("rmtypcode"), String) = gvRow.Cells(3).Text Then
                                ddlInccode = gvRow.FindControl("ddlIncomecode")
                                ddlCostCode = gvRow.FindControl("ddlCostcode")
                                ddlIncname = gvRow.FindControl("ddlIncomename")
                                ddlCstname = gvRow.FindControl("ddlCostname")

                                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")
                                ddlRefIncname = gvRow.FindControl("ddlRefIncomename")
                                ddlRefCstname = gvRow.FindControl("ddlRefCostname")

                                If IsDBNull(mySqlReader("incomecode")) = False Then
                                    ddlInccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("incomecode"), String))
                                    ddlIncname.Value = CType(mySqlReader("incomecode"), String)
                                End If

                                If IsDBNull(mySqlReader("expensecode")) = False Then
                                    ddlCostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("expensecode"), String))
                                    ddlCstname.Value = CType(mySqlReader("expensecode"), String)
                                End If

                                If IsDBNull(mySqlReader("refundincomecode")) = False Then
                                    ddlRefInccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("refundincomecode"), String))
                                    ddlRefIncname.Value = CType(mySqlReader("refundincomecode"), String)
                                End If

                                If IsDBNull(mySqlReader("refundcostcode")) = False Then
                                    ddlRefCostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("refundcostcode"), String))
                                    ddlRefCstname.Value = CType(mySqlReader("refundcostcode"), String)
                                End If
                            End If
                        End If
                    Next
                End While
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Try
            strWhereCond = ""
            If ddlRoomcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " rmtypmast.rmtypcode = '" & Trim(ddlRoomcode.Items(ddlRoomcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND rmtypmast.rmtypcode  = '" & Trim(ddlRoomcode.Items(ddlRoomcode.SelectedIndex).Text) & "'"
                End If
            End If
            If ddlSpcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " rmtypmast.sptypecode = '" & Trim(ddlSpcode.Items(ddlSpcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND rmtypmast.sptypecode = '" & Trim(ddlSpcode.Items(ddlSpcode.SelectedIndex).Text) & "'"
                End If
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefieneAccountsRoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = " SELECT *,sptypemast.sptypename FROM  rmtypmast  INNER JOIN " & _
                        " sptypemast  ON  rmtypmast.sptypecode = sptypemast.sptypecode where rmtypmast.active=1 "

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
            '--------------------------fill dropdown in grid
            showgrid(strSqlQry)
            '----------------------------------------------------------------------------------------------------
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefieneAccountsRoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

    '#Region " Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    '    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
    '        gv_SearchResult.PageIndex = e.NewPageIndex
    '        ' FillGrid("rmtypcode")
    '        If ddlOrderby.Value = "Room/Service Type Code" Then
    '            FillGrid(" rmtypmast.rmtypcode,sptypemast.sptypecode", "ASC")
    '        ElseIf ddlOrderby.Value = "Room/Service Type Name" Then
    '            FillGrid("rmtypmast.rmtypname,sptypemast.sptypename", "ASC")
    '        ElseIf ddlOrderby.Value = "Select" Then
    '            FillGrid(" rmtypmast.rmtypcode")
    '        End If
    '    End Sub
    '#End Region

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

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("rmtypcode")
    End Sub
#End Region

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = " SELECT  rmtypmast.sptypecode AS [SPType Code],sptypemast.sptypename AS [SPType Name], rmtypmast.rmtypcode AS [Room Type Code], rmtypmast.rmtypname AS [Room Type Name], " & _
                            " acctmast_1.acctcode AS [Income Code], acctmast_1.acctname AS [Income Account Name], " & _
                              " acctmast_2.acctcode AS [Refund Income Code], acctmast_2.acctname AS [Refund Income Account Name], " & _
                              " acctmast_3.acctcode AS [Refund Income Code], acctmast_3.acctname AS [Refund Income Account Name], " & _
                            " acctmast.acctcode AS [Cost of Sales A/C Code], acctmast.acctname AS [Cost of Sales A/C Name] " & _
                            " FROM  rmtypmast INNER JOIN " & _
                            " sptypemast ON rmtypmast.sptypecode = sptypemast.sptypecode LEFT OUTER JOIN " & _
                            " acctmast ON rmtypmast.expensecode = acctmast.acctcode LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_1 ON rmtypmast.incomecode = acctmast_1.acctcode LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_2 ON rmtypmast.refundincomecode = acctmast_2.acctcode LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_3 ON rmtypmast.refundcostcode = acctmast_3.acctcode "



                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " Where " & BuildCondition() & " ORDER BY rmtypmast.rmtypcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY rmtypmast.rmtypcode"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "rmtype")

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

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ValidateGrids() = False Then
                    Exit Sub
                End If
                If SaveGrid() Then
                    objUtils.MessageBox("Record Saved Successfully", Page)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefieneAccountsRoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "SaveGrid()"
    Public Function SaveGrid() As Boolean
        Try
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect

            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlCostCode = gvRow.FindControl("ddlCostcode")

                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")

                myCommand = New SqlCommand("sp_update_rmtypmast", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(3).Text.Trim, String)
                If ddlInccode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(ddlInccode.Items(ddlInccode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If
                If ddlCostCode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = CType(ddlCostCode.Items(ddlCostCode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If

                If ddlRefInccode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@refundincomecode", SqlDbType.VarChar, 20)).Value = CType(ddlRefInccode.Items(ddlRefInccode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@refundincomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If
                If ddlRefCostCode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@refundcostcode", SqlDbType.VarChar, 20)).Value = CType(ddlRefCostCode.Items(ddlRefCostCode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@refundcostcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If

                'myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                'myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                myCommand.ExecuteNonQuery()

            Next

            myCommand = New SqlCommand("sp_update_othtypmastDet", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@optionname", SqlDbType.VarChar, 100)).Value = "DefienAccounts_RoomTypes"
            myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()


            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)          'connection close

            SaveGrid = True
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            SaveGrid = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

#Region "ValidateGrids()"
    Public Function ValidateGrids() As Boolean
        Try
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect
            Dim flg As Boolean = False

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")

                If ddlInccode.Value <> "[Select]" Or ddlCostCode.Value <> "[Select]" Or ddlRefInccode.Value <> "[Select]" Or ddlRefCostCode.Value <> "[Select]" Then
                    flg = True
                    Exit For
                End If
            Next

            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Atleast One Row .');", True)
                SetFocus(gv_SearchResult)
                ValidateGrids = False
                Exit Function
            End If

            ValidateGrids = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnProfitcenter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProfitcenter.Click"
    Protected Sub BtnProfitcenter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnProfitcenter.Click
        Try
            Dim ddlInccode As HtmlSelect
            Dim ddlIncname As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim ddlCostname As HtmlSelect

            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefIncname As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect
            Dim ddlRefCostname As HtmlSelect

            For Each Me.gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlIncname = gvRow.FindControl("ddlIncomename")
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlCostname = gvRow.FindControl("ddlCostname")

                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                ddlRefIncname = gvRow.FindControl("ddlRefIncomename")
                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")
                ddlRefCostname = gvRow.FindControl("ddlRefCostname")

                If ddlInccode.Value = "[Select]" Then
                    ddlInccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.incomecode = acctmast.acctcode where servicecat = 'Rooms'")
                    ddlIncname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.incomecode  FROM profitcentremast  where servicecat = 'Rooms'")
                End If
                If ddlCostCode.Value = "[Select]" Then
                    ddlCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.costcode = acctmast.acctcode where servicecat = 'Rooms'")
                    ddlCostname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.costcode FROM profitcentremast where servicecat = 'Rooms'")
                End If

                If ddlRefInccode.Value = "[Select]" Then
                    ddlRefInccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.refundincomecode = acctmast.acctcode where servicecat = 'Rooms'")
                    ddlRefIncname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.refundincomecode  FROM profitcentremast  where servicecat = 'Rooms'")
                End If
                If ddlRefCostCode.Value = "[Select]" Then
                    ddlRefCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.refundcostcode = acctmast.acctcode where servicecat = 'Rooms'")
                    ddlRefCostname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.refundcostcode FROM profitcentremast where servicecat = 'Rooms'")
                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefieneAccountsRoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region " Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlOrderby.Value = "Room/Service Type Code" Then
            FillGrid(" rmtypmast.rmtypcode,sptypemast.sptypecode", "ASC")
        ElseIf ddlOrderby.Value = "Room/Service Type Name" Then
            FillGrid("rmtypmast.rmtypname,sptypemast.sptypename", "ASC")
        End If
    End Sub
#End Region

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try
            If e.Row.RowIndex = -1 Then
                Exit Sub
            End If
            Dim ddlInccode As HtmlSelect
            Dim ddlIncname As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim ddlCostname As HtmlSelect

            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefIncname As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect
            Dim ddlRefCostname As HtmlSelect

            gvRow = e.Row
            '  For Each gvRow In gv_SearchResult.Rows
            ddlInccode = gvRow.FindControl("ddlIncomecode")
            ddlIncname = gvRow.FindControl("ddlIncomename")
            ddlCostCode = gvRow.FindControl("ddlCostcode")
            ddlCostname = gvRow.FindControl("ddlCostname")

            ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
            ddlRefIncname = gvRow.FindControl("ddlRefIncomename")
            ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")
            ddlRefCostname = gvRow.FindControl("ddlRefCostname")

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlInccode, "acctcode", "acctname", "select acctcode,acctname from acctmast  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncname, "acctname", "acctcode", "select acctcode,acctname from acctmast  order by acctname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostCode, "acctcode", "acctname", "select acctcode,acctname from acctmast  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostname, "acctname", "acctcode", "select acctcode,acctname from acctmast  order by acctname", True)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefInccode, "acctcode", "acctname", "select acctcode,acctname from acctmast  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefIncname, "acctname", "acctcode", "select acctcode,acctname from acctmast  order by acctname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostCode, "acctcode", "acctname", "select acctcode,acctname from acctmast  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostname, "acctname", "acctcode", "select acctcode,acctname from acctmast  order by acctname", True)


            ddlInccode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlInccode.ClientID, String) + "','" + CType(ddlIncname.ClientID, String) + "' )")
            ddlIncname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlInccode.ClientID, String) + "','" + CType(ddlIncname.ClientID, String) + "' )")

            ddlCostCode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")
            ddlCostname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")

            ddlRefInccode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlRefInccode.ClientID, String) + "','" + CType(ddlRefIncname.ClientID, String) + "' )")
            ddlRefIncname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlRefInccode.ClientID, String) + "','" + CType(ddlRefIncname.ClientID, String) + "' )")

            ddlRefCostCode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlRefCostCode.ClientID, String) + "','" + CType(ddlRefCostname.ClientID, String) + "' )")
            ddlRefCostname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlRefCostCode.ClientID, String) + "','" + CType(ddlRefCostname.ClientID, String) + "' )")

            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlInccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlIncname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCostCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlRefInccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlRefIncname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlRefCostCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlRefCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If
            ' Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlRoomcode.Value = "[Select]"
        ddlRoomname.Value = "[Select]"
        ddlSpcode.Value = "[Select]"
        ddlSPname.Value = "[Select]"
        If ddlOrderby.Value = "Room/Service Type Code" Then
            FillGrid(" rmtypmast.rmtypcode,sptypemast.sptypecode", "ASC")
        ElseIf ddlOrderby.Value = "Room/Service Type Name" Then
            FillGrid("rmtypmast.rmtypname,sptypemast.sptypename", "ASC")
        ElseIf ddlOrderby.Value = "Select" Then
            FillGrid(" rmtypmast.rmtypcode")
        End If
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DefineAccountsRoomTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        If Session("SaveChanges") = "Yes" Then
            If SaveGrid() Then
                gv_SearchResult.PageIndex = e.NewPageIndex
                FillGrid("rmtypcode")
            End If
        Else
            gv_SearchResult.PageIndex = e.NewPageIndex
            FillGrid("rmtypcode")
        End If
    End Sub

#Region "ValidateSelectGrids()"
    Public Function ValidateSelectGrids() As Boolean
        Try
            Dim chkSelect As HtmlInputCheckBox
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect
            Dim flg As Boolean = True
            Dim chkcount As Integer = 0

            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then
                    chkcount += 1
                End If
            Next
            If chkcount > 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Only One row should be selected for copying');", True)
                SetFocus(gv_SearchResult)
                ValidateSelectGrids = False
                Exit Function
            ElseIf chkcount = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select atleast one row for copying');", True)
                SetFocus(gv_SearchResult)
                ValidateSelectGrids = False
                Exit Function
            End If



            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then
                    ddlInccode = grdRow.FindControl("ddlIncomecode")
                    ddlCostCode = grdRow.FindControl("ddlCostcode")
                    ddlRefInccode = grdRow.FindControl("ddlRefIncomecode")
                    ddlRefCostCode = grdRow.FindControl("ddlRefCostcode")

                    If ddlInccode.Value <> "[Select]" And ddlCostCode.Value <> "[Select]" And ddlRefInccode.Value <> "[Select]" And ddlRefCostCode.Value <> "[Select]" Then
                        flg = True
                        Exit For
                    End If
                End If
            Next

            If flg = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select all fields in the row  .');", True)
                SetFocus(gv_SearchResult)
                ValidateSelectGrids = False
                Exit Function
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ValidateSelectGrids = True
    End Function
#End Region

    Protected Sub btnCopySelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopySelected.Click

        Dim ddlInccode As HtmlSelect
        Dim ddlIncname As HtmlSelect
        Dim ddlCostCode As HtmlSelect
        Dim ddlCostname As HtmlSelect
        Dim chkSelect As HtmlInputCheckBox
        Dim ddlRefInccode As HtmlSelect
        Dim ddlRefIncname As HtmlSelect
        Dim ddlRefCostCode As HtmlSelect
        Dim ddlRefCostname As HtmlSelect


        Dim ddlInccodeNew As HtmlSelect
        Dim ddlIncnameNew As HtmlSelect
        Dim ddlCostCodeNew As HtmlSelect
        Dim ddlCostnameNew As HtmlSelect
        Dim chkSelectNew As HtmlInputCheckBox
        Dim ddlRefInccodeNew As HtmlSelect
        Dim ddlRefIncnameNew As HtmlSelect
        Dim ddlRefCostCodeNew As HtmlSelect
        Dim ddlRefCostnameNew As HtmlSelect

        If ValidateSelectGrids() = True Then


            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then

                    ddlInccode = grdRow.FindControl("ddlIncomecode")
                    ddlIncname = grdRow.FindControl("ddlIncomename")
                    ddlCostCode = grdRow.FindControl("ddlCostcode")
                    ddlCostname = grdRow.FindControl("ddlCostname")

                    ddlRefInccode = grdRow.FindControl("ddlRefIncomecode")
                    ddlRefIncname = grdRow.FindControl("ddlRefIncomename")
                    ddlRefCostCode = grdRow.FindControl("ddlRefCostcode")
                    ddlRefCostname = grdRow.FindControl("ddlRefCostname")
                    Exit For
                End If
            Next

            For Each grdRow In gv_SearchResult.Rows
                chkSelectNew = grdRow.FindControl("chkSelect")
                If chkSelectNew.Checked = False Then
                    ddlInccodeNew = grdRow.FindControl("ddlIncomecode")
                    ddlIncnameNew = grdRow.FindControl("ddlIncomename")
                    ddlCostCodeNew = grdRow.FindControl("ddlCostcode")
                    ddlCostnameNew = grdRow.FindControl("ddlCostname")

                    ddlRefInccodeNew = grdRow.FindControl("ddlRefIncomecode")
                    ddlRefIncnameNew = grdRow.FindControl("ddlRefIncomename")
                    ddlRefCostCodeNew = grdRow.FindControl("ddlRefCostcode")
                    ddlRefCostnameNew = grdRow.FindControl("ddlRefCostname")

                    ddlInccodeNew.Value = ddlInccode.Value
                    ddlIncnameNew.Value = ddlIncname.Value
                    ddlCostCodeNew.Value = ddlCostCode.Value
                    ddlCostnameNew.Value = ddlCostname.Value

                    ddlRefInccodeNew.Value = ddlRefInccode.Value
                    ddlRefIncnameNew.Value = ddlRefIncname.Value
                    ddlRefCostCodeNew.Value = ddlRefCostCode.Value
                    ddlRefCostnameNew.Value = ddlRefCostname.Value
                End If
            Next
        End If

    End Sub

#Region "SaveSelectedToGrid()"
    Public Function SaveSelectedToGrid() As Boolean
        Try
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim chkSelect As HtmlInputCheckBox
            Dim ddlRefInccode As HtmlSelect
            Dim ddlRefCostCode As HtmlSelect
            'Dim strRmType As String
          

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then
                    'strRmType = CType(grdRow.Cells(3).Text.Trim, String)
                    ddlInccode = grdRow.FindControl("ddlIncomecode")
                    ddlCostCode = grdRow.FindControl("ddlCostcode")

                    ddlRefInccode = grdRow.FindControl("ddlRefIncomecode")
                    ddlRefCostCode = grdRow.FindControl("ddlRefCostcode")
                    Exit For
                End If

            Next

            ' Not looping thru grid since , it is pageing enabled and will not save all rows (only curent page row ), so updateing table fully

            'For Each grdRow In gv_SearchResult.Rows
            'chkSelect = grdRow.FindControl("chkSelect")
            'If chkSelect.Checked = False Then
            myCommand = New SqlCommand("sp_copy_rmtypmast", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            'myCommand.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(grdRow.Cells(3).Text.Trim, String)
            If ddlInccode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(ddlInccode.Items(ddlInccode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If
            If ddlCostCode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = CType(ddlCostCode.Items(ddlCostCode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If

            If ddlRefInccode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@refundincomecode", SqlDbType.VarChar, 20)).Value = CType(ddlRefInccode.Items(ddlRefInccode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@refundincomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If
            If ddlRefCostCode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@refundcostcode", SqlDbType.VarChar, 20)).Value = CType(ddlRefCostCode.Items(ddlRefCostCode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@refundcostcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If

            myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()

            'End If

            'Next
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)          'connection close


            SaveSelectedToGrid = True
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            SaveSelectedToGrid = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
#End Region

    Protected Sub btnSaveselected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveselected.Click


        If ValidateSelectGrids() Then
            If SaveSelectedToGrid() Then
                'gv_SearchResult.AllowPaging = True
                'gv_SearchResult.DataBind()
                FillGrid("rmtypcode")
            End If
        End If


    End Sub
End Class
