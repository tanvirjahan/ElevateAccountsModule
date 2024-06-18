'------------================--------------=======================------------------================
'   Module Name    :    DefineAccounts-FlightType.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DefineAccounts_FlightTypes
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
            Try
                SetFocus(ddlOrderby)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"),CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(Session("AppName"), String), "DefineAccounts-FlightTypes.aspx", BtnAddNew, BtnExportToExcel, _
                    '                                   BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAirlinecode, "airlinecode", "partyname", "select distinct airlinecode,partyname FROM   flightmast ,partymast where flightmast.airlinecode =partymast.partycode and  sptypecode='air' and flightmast.active=1 order by airlinecode ", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAirlinename, "partyname", "airlinecode", "select distinct partyname,airlinecode FROM   flightmast ,partymast where flightmast.airlinecode =partymast.partycode and  sptypecode='air' and flightmast.active=1 order by partyname ", True)

                Session.Add("strsortExpression", "flightcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillGrid("flightcode")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlAirlinecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAirlinename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

            btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            btnclear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        Else
            If chkSaveChanges.Checked Then
                Session.Add("SaveChanges", "Yes")
            Else
                Session.Add("SaveChanges", "No")
            End If
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
                        If IsDBNull(mySqlReader("flightcode")) = False Then
                            If CType(mySqlReader("flightcode"), String) = gvRow.Cells(2).Text Then
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
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Try
            strWhereCond = ""
            If ddlAirlinecode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " airlinecode = '" & Trim(ddlAirlinecode.Items(ddlAirlinecode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND airlinecode = '" & Trim(ddlAirlinecode.Items(ddlAirlinecode.SelectedIndex).Text) & "'"
                End If
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = " SELECT flightmast.*,partymast.partyname " & _
                        " FROM   flightmast ,partymast where flightmast.airlinecode =partymast.partycode " & _
                        " and  sptypecode='air' and flightmast.active=1 "

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
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        If Session("SaveChanges") = "Yes" Then
            If SaveGrid() Then
                gv_SearchResult.PageIndex = e.NewPageIndex
                FillGrid("flightcode")
            End If
        Else
            gv_SearchResult.PageIndex = e.NewPageIndex
            FillGrid("flightcode")
        End If
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

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("flightcode")
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

                strSqlQry = " SELECT  flightmast.airlinecode as [Airline Code],partymast.partyname AS [Airline Name]," & _
                            " flightcode as [Flight Code],acctmast_1.acctcode AS [Income Code]," & _
                            " acctmast_1.acctname AS [Income Account Name], acctmast.acctcode AS [Cost of Sales A/C Code], acctmast.acctname AS [Cost of Sales A/C Name] ,acctmast_2.acctcode AS [Refund Income Code]," & _
                            " acctmast_2.acctname AS [Refund Income Account Name], acctmast_3.acctcode AS [Refund Cost Code], acctmast_3.acctname AS [Refund Cost Name] " & _
                            " FROM flightmast LEFT OUTER JOIN partymast on " & _
                            " flightmast.airlinecode = partymast.partycode and  sptypecode='air'  LEFT OUTER JOIN " & _
                            " acctmast ON flightmast.expensecode = acctmast.acctcode LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_1 ON flightmast.incomecode = acctmast_1.acctcode  LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_2 ON flightmast.refundincomecode = acctmast_2.acctcode  LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_3 ON flightmast.refundcostcode = acctmast_3.acctcode "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " Where " & BuildCondition() & " ORDER BY flightcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY flightcode"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "flight")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
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
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "SaveGrid()"
    Public Function SaveGrid() As Boolean
        Dim ddlInccode As HtmlSelect
        Dim ddlCostCode As HtmlSelect
        Dim ddlRefInccode As HtmlSelect
        Dim ddlRefCostCode As HtmlSelect
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")

                myCommand = New SqlCommand("sp_update_flightmast", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@flightcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(2).Text.Trim, String)
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
            Next
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close
            SaveGrid = True
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            SaveGrid = True
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
            ValidateGrids = False
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

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlIncname = gvRow.FindControl("ddlIncomename")
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlCostname = gvRow.FindControl("ddlCostname")
                ddlRefInccode = gvRow.FindControl("ddlRefIncomecode")
                ddlRefIncname = gvRow.FindControl("ddlRefIncomename")
                ddlRefCostCode = gvRow.FindControl("ddlRefCostcode")
                ddlRefCostname = gvRow.FindControl("ddlRefCostname")

                If ddlInccode.Value = "[Select]" Then
                    ddlInccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.incomecode = acctmast.acctcode where servicecat = 'Ticketing'")
                    ddlIncname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.incomecode  FROM profitcentremast  where servicecat = 'Ticketing'")
                End If

                If ddlCostCode.Value = "[Select]" Then
                    ddlCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.costcode = acctmast.acctcode where servicecat = 'Ticketing'")
                    ddlCostname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.costcode  FROM profitcentremast  where servicecat = 'Ticketing'")
                End If

                If ddlRefInccode.Value = "[Select]" Then
                    ddlRefInccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.refundincomecode = acctmast.acctcode where servicecat = 'Ticketing'")
                    ddlRefIncname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.refundincomecode  FROM profitcentremast  where servicecat = 'Ticketing'")
                End If
                If ddlRefCostCode.Value = "[Select]" Then
                    ddlRefCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  acctmast.acctname  FROM profitcentremast INNER JOIN acctmast ON profitcentremast.refundcostcode = acctmast.acctcode where servicecat = 'Ticketing'")
                    ddlRefCostname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT  profitcentremast.refundcostcode  FROM profitcentremast  where servicecat = 'Ticketing'")
                End If
            Next
        Catch ex As Exception
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlOrderby.Value = "Flight Code" Then
            FillGrid("flightcode", "ASC")
        ElseIf ddlOrderby.Value = "Airline Code" Then
            FillGrid("airlinecode", "ASC")
        End If
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound "
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
        Catch ex As Exception
            objUtils.WritErrorLog("DefineAccounts-FlightTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnclear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlAirlinecode.Value = "[Select]"
        ddlAirlinename.Value = "[Select]"
        FillGrid("flightcode")
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DefineAccountsFlightTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
