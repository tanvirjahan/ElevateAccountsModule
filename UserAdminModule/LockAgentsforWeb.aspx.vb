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

            txtconnection.Value = Session("dbconnectionName")

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("Login.aspx", False)
                Exit Sub
            End If

            Try

                SetFocus(ddlMarket)
                lblHeading.Text = "Lock Agents for Web"

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarket, "plgrpcode", "plgrpname", "select * from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMarketName, "plgrpname", "plgrpcode", "select * from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCity, "citycode", "cityname", "select * from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select * from citymast where active=1 order by cityname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingType, "sellcode", "sellname", "select * from sellmast where active=1 order by sellcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSellingTypeName, "sellname", "sellcode", "select * from sellmast where active=1 order by sellname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategory, "agentcatcode", "agentcatname", "select * from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCategoryName, "agentcatname", "agentcatcode", "select * from agentcatmast where active=1 order by agentcatname", True)

                'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save?')==false)return false;")

                FillGrid("agentmast.agentcode")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSellingTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If
    End Sub

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""

        If ddlMarket.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.plgrpcode = '" & ddlMarket.Items(ddlMarket.SelectedIndex).Text & "'"
            End If
        End If

        If ddlSellingType.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.sellcode = '" & ddlSellingType.Items(ddlSellingType.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCategory.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.catcode = '" & ddlCategory.Items(ddlCategory.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCountry.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.ctrycode = '" & ddlCountry.Items(ddlCountry.SelectedIndex).Text & "'"
            End If
        End If

        If ddlCity.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            Else
                strWhereCond = strWhereCond & " AND agentmast.citycode = '" & ddlCity.Items(ddlCity.SelectedIndex).Text & "'"
            End If
        End If

        BuildCondition = strWhereCond
    End Function
#End Region

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

            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
            Else
                strSqlQry = strSqlQry & " ORDER BY " & strorderby & " " & strsortorder
            End If

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
                SetFocus(ddlMarket)

            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("LockAgentsforWeb.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("MainPage.aspx", False)
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect

        Try
            If grdSupplier.Rows.Count <> 0 Then
                strSqlQry = " select agentmast.agentcode as [Customer Code],agentname as [Customer Name], " & _
                            " [Lock] = case when reason is null then 0 else 1 end, reason as [Reason]" & _
                            " from agentmast left outer join agents_locked on agentmast.agentcode = agents_locked.agentcode where agentmast.active=1 "

                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " AND " & BuildCondition() & " ORDER BY agentmast.agentcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY agentmast.agentcode "
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "LockAgents")

                objUtils.ExportToExcel(DS, Response)
                con.Close()
            Else
                objUtils.MessageBox("Sorry , No data availabe for export to excel.", Page)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddlMarket.Value = "[Select]"
        ddlMarketName.Value = "[Select]"
        ddlSellingType.Value = "[Select]"
        ddlSellingTypeName.Value = "[Select]"
        ddlCountry.Value = "[Select]"
        ddlCountryName.Value = "[Select]"
        ddlCity.Value = "[Select]"
        ddlCityName.Value = "[Select]"
        ddlCategory.Value = "[Select]"
        ddlCategoryName.Value = "[Select]"
        FillGrid("agentmast.agentcode")
    End Sub

#Region "Public Sub SortGridColoumn_click()"
    Public Sub SortGridColoumn_click()
        Dim DataTable As DataTable
        Dim myDS As New DataSet
        FillGrid(Session("strsortexpression"), "")

        myDS = grdSupplier.DataSource
        DataTable = myDS.Tables(0)
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            grdSupplier.DataSource = dataView
            grdSupplier.DataBind()
        End If
    End Sub
#End Region

    Protected Sub grdSupplier_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdSupplier.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

End Class
