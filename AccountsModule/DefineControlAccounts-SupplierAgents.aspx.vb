'------------================--------------=======================------------------================
'   Module Name    :  DefineControlAccounts-SupplierAgents.aspx  
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DefineControlAccounts_SupplierAgents
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
                    'objUser.CheckUserRight(Session("dbconnectionName"),CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(Session("AppName"), String), "DefineControlAccounts-SupplierAgents.aspx", BtnAddNew, BtnExportToExcel, _
                    '                                   BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If

                Page.Title = Page.Title + "-" + "Define Control Accounts -Supplier Agents"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppleiAgentcode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active=1 order by supagentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSuppleiAgentname, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active=1 order by supagentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSptypecode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast where active=1 order by sptypecode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSptypename, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast  where active=1 order by sptypename", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlacctcode, "acctcode", "acctcode", "select acctcode from acctmast where controlyn='Y' and cust_supp='S'", True)
                'Session.Add("strsortExpression", "supagentcode")
                'Session.Add("strsortdirection", SortDirection.Ascending)
                FillGrid(ddlOrderby.Value)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSuppleiAgentcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSuppleiAgentname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlSptypecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSptypename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlControlacctcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

            btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            BtnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Clear Search Criteria?')==false)return false;")
            btnSaveselected.Attributes.Add("onclick", "javascript:if(confirm('Are you sure to save in all rows, this will change in the database directly and cannot be reverted? ')==false)return false;")

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

        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand(strqry, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("supagentcode")) = False Then
                            If CType(mySqlReader("supagentcode"), String) = gvRow.Cells(3).Text Then
                                ddlInccode = gvRow.FindControl("ddlIncomecode")
                                ddlCostCode = gvRow.FindControl("ddlCostcode")
                                ddlIncname = gvRow.FindControl("ddlIncomename")
                                ddlCstname = gvRow.FindControl("ddlCostname")

                                If IsDBNull(mySqlReader("accrualacctcode")) = False Then
                                    ddlInccode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("accrualacctcode"), String))
                                    ddlIncname.Value = CType(mySqlReader("accrualacctcode"), String)
                                End If

                                If IsDBNull(mySqlReader("controlacctcode")) = False Then
                                    ddlCostCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("controlacctcode"), String))
                                    ddlCstname.Value = CType(mySqlReader("controlacctcode"), String)
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
            If ddlSuppleiAgentcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " supagentcode = '" & Trim(ddlSuppleiAgentcode.Items(ddlSuppleiAgentcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND supagentcode = '" & Trim(ddlSuppleiAgentcode.Items(ddlSuppleiAgentcode.SelectedIndex).Text) & "'"
                End If
            End If
            If ddlSptypecode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " supplier_agents.sptypecode  = '" & Trim(ddlSptypecode.Items(ddlSptypecode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND supplier_agents.sptypecode  = '" & Trim(ddlSptypecode.Items(ddlSptypecode.SelectedIndex).Text) & "'"
                End If
            End If

            BuildCondition = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = " select supplier_agents.*,sptypename from supplier_agents inner join " & _
                        " sptypemast on supplier_agents.sptypecode = sptypemast.sptypecode where supplier_agents.active=1"

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
            '--------------------------------------------------------------
            showgrid(strSqlQry)
            '----------------------------------------------------------------------------------------------------
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        'gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid(ddlOrderby.Value)
        If Session("SaveChanges") = "Yes" Then
            If SaveGrid() Then
                gv_SearchResult.PageIndex = e.NewPageIndex
                FillGrid(ddlOrderby.Value)
            End If
        Else
            gv_SearchResult.PageIndex = e.NewPageIndex
            FillGrid(ddlOrderby.Value)
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
        FillGrid(ddlOrderby.Value)
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

                strSqlQry = " SELECT  supplier_agents.sptypecode as [SPType Code],sptypename as [SPType Name],supagentcode as [SupplierAgent Code],supagentname as [SupplierAgent Name]," & _
                            " acctmast_1.acctcode AS [Accrual A/C Code]," & _
                            " acctmast_1.acctname AS [Accrual A/C  Name], acctmast.acctcode AS [Control A/C Code], acctmast.acctname AS [Control A/C Name] " & _
                            " FROM supplier_agents inner join sptypemast on supplier_agents.sptypecode = sptypemast.sptypecode " & _
                            " LEFT OUTER JOIN " & _
                            " acctmast ON supplier_agents.controlacctcode = acctmast.acctcode LEFT OUTER JOIN " & _
                            " acctmast AS acctmast_1 ON supplier_agents.accrualacctcode = acctmast_1.acctcode "


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " Where " & BuildCondition() & " ORDER BY supagentcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY supagentcode"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "SupAge")

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
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "SaveGrid()"
    Public Function SaveGrid() As Boolean
        Try
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each grdRow In gv_SearchResult.Rows
                ddlInccode = grdRow.FindControl("ddlIncomecode")
                ddlCostCode = grdRow.FindControl("ddlCostcode")

                myCommand = New SqlCommand("sp_update_supplieragent", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(grdRow.Cells(3).Text.Trim, String)
                If ddlInccode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlInccode.Items(ddlInccode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If
                If ddlCostCode.Value <> "[Select]" Then
                    myCommand.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlCostCode.Items(ddlCostCode.SelectedIndex).Text, String)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                End If
                'myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                'myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                myCommand.ExecuteNonQuery()

            Next

            myCommand = New SqlCommand("sp_update_othtypmastDet", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@optionname", SqlDbType.VarChar, 100)).Value = "DefineControlAccounts_SupplierAgents"
            myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)          'connection close
            objUtils.MessageBox("Record Saved Successfully", Page)
            ddlAcType.Value = "[Select]"
            ddlControlacctcode.Value = "[Select]"

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

            Dim flg As Boolean = False

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlCostCode = gvRow.FindControl("ddlCostcode")

                If ddlInccode.Value <> "[Select]" Or ddlCostCode.Value <> "[Select]" Then
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
        Response.Redirect("~\MainPage.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnfillcontrolAC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnfillcontrolAC.Click"
    Protected Sub BtnfillcontrolAC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnfillcontrolAC.Click
        Try
            Dim ddlCostCode As HtmlSelect
            Dim ddlCostname As HtmlSelect
            Dim ddlInccode As HtmlSelect
            Dim ddlIncname As HtmlSelect

            For Each gvRow In gv_SearchResult.Rows
                ddlInccode = gvRow.FindControl("ddlIncomecode")
                ddlIncname = gvRow.FindControl("ddlIncomename")
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlCostname = gvRow.FindControl("ddlCostname")

                If ddlAcType.SelectedIndex = 1 Then
                    If ddlInccode.Value = "[Select]" Then
                        ddlInccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where controlyn='Y' and cust_supp='S'and acctcode='" & ddlControlacctcode.Value.Trim & "'")
                        ddlIncname.Value = ddlControlacctcode.Value.Trim
                    End If
                ElseIf ddlAcType.SelectedIndex = 2 Then
                    If ddlCostCode.Value = "[Select]" Then
                        ddlCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where controlyn='Y' and cust_supp='S'and acctcode='" & ddlControlacctcode.Value.Trim & "'")
                        ddlCostname.Value = ddlControlacctcode.Value.Trim
                    End If
                End If
                '     objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCostCode, "incomecode", "acctname", "select acctcode as [incomecode],acctname from acctmast where controlyn='Y' and cust_supp='S'and acctcode='" & ddlControlacctcode.Value.Trim & "'", True)
                '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCostname, "acctname", "incomecode", "select acctcode as [incomecode],acctname from acctmast where controlyn='Y' and cust_supp='S'and acctcode='" & ddlControlacctcode.Value.Trim & "'", True)

                'showgrid(strSqlQry)
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid(ddlOrderby.Value)
    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound"
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try

            If e.Row.RowIndex = -1 Then
                Exit Sub
            End If

            Dim ddlInccode As HtmlSelect
            Dim ddlIncname As HtmlSelect
            Dim ddlCostCode As HtmlSelect
            Dim ddlCostname As HtmlSelect
            gvRow = e.Row

            ddlInccode = gvRow.FindControl("ddlIncomecode")
            ddlIncname = gvRow.FindControl("ddlIncomename")
            ddlCostCode = gvRow.FindControl("ddlCostcode")
            ddlCostname = gvRow.FindControl("ddlCostname")

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlInccode, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='Y' and cust_supp='S'  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncname, "acctname", "acctcode", "select acctcode,acctname from acctmast where controlyn='Y' and cust_supp='S'  order by acctname", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='Y' and cust_supp='S'  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostname, "acctname", "acctcode", "select acctcode,acctname from acctmast where controlyn='Y' and cust_supp='S'  order by acctname", True)


            ddlInccode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlInccode.ClientID, String) + "','" + CType(ddlIncname.ClientID, String) + "' )")
            ddlIncname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlInccode.ClientID, String) + "','" + CType(ddlIncname.ClientID, String) + "' )")

            ddlCostCode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")
            ddlCostname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")


            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ddlInccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlIncname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCostCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlSptypecode.Value = "[Select]"
        ddlSptypename.Value = "[Select]"
        ddlSuppleiAgentcode.Value = "[Select]"
        ddlSuppleiAgentname.Value = "[Select]"
        FillGrid(ddlOrderby.Value)
    End Sub
#End Region

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            'Session.Add("Pageame", "DefineControlAccountsSupplierAgents")
            'Session.Add("BackPageName", "~\AccountsModule\DefineControlAccounts-SupplierAgents.aspx")

            'strSelectionFormula = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=DefineControlAccountsSupplierAgents&BackPageName=DefineControlAccounts-SupplierAgents.aspx&SPtypeCode=" & Trim(ddlSptypecode.Items(ddlSptypecode.SelectedIndex).Text) & "&SupAgentCode=" & Trim(ddlSuppleiAgentcode.Items(ddlSuppleiAgentcode.SelectedIndex).Text) & "','ProfitCenterMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("~\PriceListModule\rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DefineControlAccounts-SupplierAgents','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

#Region "ValidateSelectGrids()"
    Public Function ValidateSelectGrids() As Boolean
        Try
            Dim chkSelect As HtmlInputCheckBox
            Dim ddlInccode As HtmlSelect
            Dim ddlCostCode As HtmlSelect
          
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
                    

                    If ddlInccode.Value <> "[Select]" And ddlCostCode.Value <> "[Select]" Then
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

        Dim ddlInccodeNew As HtmlSelect
        Dim ddlIncnameNew As HtmlSelect
        Dim ddlCostCodeNew As HtmlSelect
        Dim ddlCostnameNew As HtmlSelect
        Dim chkSelectNew As HtmlInputCheckBox
       
        If ValidateSelectGrids() = True Then


            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then

                    ddlInccode = grdRow.FindControl("ddlIncomecode")
                    ddlIncname = grdRow.FindControl("ddlIncomename")
                    ddlCostCode = grdRow.FindControl("ddlCostcode")
                    ddlCostname = grdRow.FindControl("ddlCostname")

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

                 

                    ddlInccodeNew.Value = ddlInccode.Value
                    ddlIncnameNew.Value = ddlIncname.Value
                    ddlCostCodeNew.Value = ddlCostCode.Value
                    ddlCostnameNew.Value = ddlCostname.Value

                  
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

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then

                    ddlInccode = grdRow.FindControl("ddlIncomecode")
                    ddlCostCode = grdRow.FindControl("ddlCostcode")

                    Exit For
                End If

            Next

            ' Not looping thru grid since , it is pageing enabled and will not save all rows (only curent page row ), so updateing table fully

            'For Each grdRow In gv_SearchResult.Rows
            'chkSelect = grdRow.FindControl("chkSelect")
            'If chkSelect.Checked = False Then
            myCommand = New SqlCommand("sp_copy_supplieragent", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            If ddlInccode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlInccode.Items(ddlInccode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If
            If ddlCostCode.Value <> "[Select]" Then
                myCommand.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlCostCode.Items(ddlCostCode.SelectedIndex).Text, String)
            Else
                myCommand.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
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
                FillGrid(ddlOrderby.Value)
            End If
        End If
    End Sub
End Class
