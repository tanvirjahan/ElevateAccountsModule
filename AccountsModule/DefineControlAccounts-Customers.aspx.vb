'------------================--------------=======================------------------================
'   Module Name    :    DefineControlAccounts-Customers.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class DefineControlAccounts_Customers
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
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                SetFocus(ddlOrderby)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    'objUser.CheckUserRight(Session("dbconnectionName"),CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                    '                                   CType(Session("AppName"), String), "DefineControlAccounts-Customers.aspx", BtnAddNew, BtnExportToExcel, _
                    '                                   BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                Page.Title = Page.Title + "-" + "Define Control Accounts -Customers "

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomercode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomername, "agentname", "agentcode", "select agentname,agentcode from agentmast  where active=1 order by agentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategorycode, "agentcatcode", "agentcatname", "select agentcatcode,agentcatname from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryname, "agentcatname", "agentcatcode", "select agentcatname,agentcatcode from agentcatmast  where active=1 order by agentcatname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlacctcode, "acctcode", "acctcode", "select acctcode from acctmast where controlyn='Y' and cust_supp='C'", True)

                FillGrid(ddlOrderby.Value)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCustomercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCustomername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlCategorycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategoryname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlControlacctcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

            btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Save?')==false)return false;")
            btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            BtnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
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

        Dim ddlCostCode As HtmlSelect
        Dim ddlCstname As HtmlSelect

        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand(strqry, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In gv_SearchResult.Rows
                        If IsDBNull(mySqlReader("agentcode")) = False Then
                            If CType(mySqlReader("agentcode"), String) = gvRow.Cells(3).Text Then
                                ddlCostCode = gvRow.FindControl("ddlCostcode")
                                ddlCstname = gvRow.FindControl("ddlCostname")

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
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Try
            strWhereCond = ""
            If ddlCustomercode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " agentcode = '" & Trim(ddlCustomercode.Items(ddlCustomercode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND agentcode = '" & Trim(ddlCustomercode.Items(ddlCustomercode.SelectedIndex).Text) & "'"
                End If
            End If

            If ddlCategorycode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " agentcatcode = '" & Trim(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND agentcatcode = '" & Trim(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) & "'"
                End If
            End If

            BuildCondition = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            strSqlQry = " SELECT  agentcode,agentname, agentmast.controlacctcode,agentcatcode,agentcatname " & _
                        " FROM  agentmast INNER JOIN " & _
                        " agentcatmast ON agentmast.catcode = agentcatmast.agentcatcode WHERE agentmast.active = 1 "

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
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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

                strSqlQry = " SELECT  agentcatcode as [Category Code],agentcatname as [Category Name],agentcode as [Customer Code],agentname as [Customer Name]," & _
                            " acctmast.acctcode AS [Control A/C Code], acctmast.acctname AS [Control A/C Name] " & _
                            " FROM agentmast inner join agentcatmast on agentmast.catcode = agentcatmast.agentcatcode LEFT OUTER JOIN " & _
                            " acctmast ON agentmast.controlacctcode = acctmast.acctcode  "


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " Where " & BuildCondition() & " ORDER BY agentcode"
                Else
                    strSqlQry = strSqlQry & " ORDER BY agentcode"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "Customer")

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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region

#Region "SaveGrid()"
    Public Function SaveGrid() As Boolean
        Try

            Dim ddlCostCode As HtmlSelect

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each gvRow In gv_SearchResult.Rows

                ddlCostCode = gvRow.FindControl("ddlCostcode")

                myCommand = New SqlCommand("sp_update_agentmast", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(3).Text.Trim, String)

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
            myCommand.Parameters.Add(New SqlParameter("@optionname", SqlDbType.VarChar, 100)).Value = "DefineControlAccounts_Customers"
            myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)          'connection close
            objUtils.MessageBox("Record Saved Successfully", Page)
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
            Dim ddlCostCode As HtmlSelect
            Dim flg As Boolean = False

            For Each gvRow In gv_SearchResult.Rows
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                If ddlCostCode.Value <> "[Select]" Then
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
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            For Each gvRow In gv_SearchResult.Rows
                ddlCostCode = gvRow.FindControl("ddlCostcode")
                ddlCostname = gvRow.FindControl("ddlCostname")
                If ddlCostCode.Value = "[Select]" Then
                    ddlCostCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where controlyn='Y' and cust_supp='C'and acctcode='" & ddlControlacctcode.Value.Trim & "'")
                    ddlCostname.Value = ddlControlacctcode.Value.Trim
                End If

                '                showgrid(strSqlQry)
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

            Dim ddlCostCode As HtmlSelect
            Dim ddlCostname As HtmlSelect
            gvRow = e.Row

            ddlCostCode = gvRow.FindControl("ddlCostcode")
            ddlCostname = gvRow.FindControl("ddlCostname")

           
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostCode, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='Y'  order by acctcode", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostname, "acctname", "acctcode", "select acctcode,acctname from acctmast where controlyn='Y'  order by acctname", True)


            ddlCostCode.Attributes.Add("onchange", "javascript:FillIncomepCode('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")
            ddlCostname.Attributes.Add("onchange", "javascript:FillIncomeName('" + CType(ddlCostCode.ClientID, String) + "','" + CType(ddlCostname.ClientID, String) + "' )")


            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ddlCostCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlCategorycode.Value = "[Select]"
        ddlCategoryname.Value = "[Select]"
        ddlCustomercode.Value = "[Select]"
        ddlCustomername.Value = "[Select]"
        FillGrid(ddlOrderby.Value)
    End Sub
#End Region

    
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DefineControlAccounts-Customers','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""

            'Session.Add("Pageame", "DefineControlAccountsSupplierAgents")
            'Session.Add("BackPageName", "~\AccountsModule\DefineControlAccounts-SupplierAgents.aspx")

            'strSelectionFormula = ""
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=DefineControlAccountsCustomers&BackPageName=DefineControlAccounts-Customers.aspx&CustCode=" & Trim(ddlCustomercode.Items(ddlCustomercode.SelectedIndex).Text) & "&CustCat=" & Trim(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) & "','DefCtrlacctCust','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("~\PriceListModule\rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineControlAccounts-Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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


                    If ddlCostCode.Value <> "[Select]" Then
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
        
        Dim ddlCostCode As HtmlSelect
        Dim ddlCostname As HtmlSelect
        Dim chkSelect As HtmlInputCheckBox
       
        Dim ddlCostCodeNew As HtmlSelect
        Dim ddlCostnameNew As HtmlSelect
        Dim chkSelectNew As HtmlInputCheckBox

        If ValidateSelectGrids() = True Then

            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then
                    ddlCostCode = grdRow.FindControl("ddlCostcode")
                    ddlCostname = grdRow.FindControl("ddlCostname")

                    Exit For
                End If
            Next

            For Each grdRow In gv_SearchResult.Rows
                chkSelectNew = grdRow.FindControl("chkSelect")
                If chkSelectNew.Checked = False Then

                    ddlCostCodeNew = grdRow.FindControl("ddlCostcode")
                    ddlCostnameNew = grdRow.FindControl("ddlCostname")
                    ddlCostCodeNew.Value = ddlCostCode.Value
                    ddlCostnameNew.Value = ddlCostname.Value
                End If
            Next
        End If
    End Sub

#Region "SaveSelectedToGrid()"
    Public Function SaveSelectedToGrid() As Boolean
        Try

            Dim ddlCostCode As HtmlSelect
            Dim chkSelect As HtmlInputCheckBox

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

            For Each grdRow In gv_SearchResult.Rows
                chkSelect = grdRow.FindControl("chkSelect")
                If chkSelect.Checked = True Then
                    ddlCostCode = grdRow.FindControl("ddlCostcode")

                    Exit For
                End If

            Next

            ' Not looping thru grid since , it is pageing enabled and will not save all rows (only curent page row ), so updateing table fully

            'For Each grdRow In gv_SearchResult.Rows
            'chkSelect = grdRow.FindControl("chkSelect")
            'If chkSelect.Checked = False Then
            myCommand = New SqlCommand("sp_copy_agentmast", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
           
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
