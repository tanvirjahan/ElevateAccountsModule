'------------================--------------=======================------------------================
'   Module Name    :    ProfitCenterSearch.aspx
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ProfitCenterSearch
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

        ServiceCategoryTCol = 0
        ServiceCategory = 1
        DisplayName = 2
        IncomeCode = 3
        CostofSaleCode = 4
        RefIncomeCode = 5
        RefCostofSaleCode = 6
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

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            lblCostcode.Visible = False
            lblCostname.Visible = False
            lblInccode.Visible = False
            lblIncname.Visible = False
            lblRefCostcode.Visible = False
            lblRefCostname.Visible = False
            lblRefInccode.Visible = False
            lblRefIncname.Visible = False
            ddlCostcode.Visible = False
            ddlCostname.Visible = False
            ddlIncomecode.Visible = False
            ddlIncomename.Visible = False
            ddlRefCostcode.Visible = False
            ddlRefCostname.Visible = False
            ddlRefIncomecode.Visible = False
            ddlRefIncomename.Visible = False

            Try
                SetFocus(txtServicecode)
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
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\ProfitCenterSearch.aspx", BtnAddNew, BtnExportToExcel, _
                                                       BtnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostcode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostname, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncomecode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlIncomename, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostcode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefCostname, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefIncomecode, "acctcode", "acctname", "select acctcode,acctname from acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefIncomename, "acctname", "acctcode", "select acctname,acctcode from acctmast order by acctname", True)



                Session.Add("strsortExpression", "servicecat")
                Session.Add("strsortdirection", SortDirection.Ascending)
                FillGrid("servicecat")




            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ProfitCenterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        Dim typ As Type
        typ = GetType(DropDownList)
        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlCostcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlIncomecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlIncomename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRefCostcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRefCostname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRefIncomecode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlRefIncomename.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
        BtnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ProfitCenterWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub
#End Region

#Region "  Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        Try
            strWhereCond = ""
            If txtServicecode.Value.Trim <> "" Then
                If Trim(strWhereCond) = "" Then

                    strWhereCond = " upper(servicecat) LIKE '" & Trim(txtServicecode.Value.Trim.ToUpper) & "%'"
                Else
                    strWhereCond = strWhereCond & " AND upper(servicecat) LIKE '" & Trim(txtServicecode.Value.Trim.ToUpper) & "%'"
                End If
            End If

            If txtDisplayname.Value.Trim <> "" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " upper(dispname)LIKE  '" & Trim(txtDisplayname.Value.Trim.ToUpper) & "%'"
                Else
                    strWhereCond = strWhereCond & " AND upper(dispname)LIKE  '" & Trim(txtDisplayname.Value.Trim.ToUpper) & "%'"
                End If
            End If
            If ddlCostcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " costcode = '" & Trim(ddlCostcode.Items(ddlCostcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND costcode  = '" & Trim(ddlCostcode.Items(ddlCostcode.SelectedIndex).Text) & "'"
                End If
            End If

            If ddlIncomecode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " incomecode = '" & Trim(ddlIncomecode.Items(ddlIncomecode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND incomecode  = '" & Trim(ddlIncomecode.Items(ddlIncomecode.SelectedIndex).Text) & "'"
                End If
            End If
            If ddlRefIncomecode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " refundincomecode = '" & Trim(ddlRefIncomecode.Items(ddlRefIncomecode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND refundincomecode = '" & Trim(ddlRefIncomecode.Items(ddlRefIncomecode.SelectedIndex).Text) & "'"
                End If
            End If
            If ddlRefCostcode.Value <> "[Select]" Then
                If Trim(strWhereCond) = "" Then
                    strWhereCond = " refundcostcode = '" & Trim(ddlRefCostcode.Items(ddlRefCostcode.SelectedIndex).Text) & "'"
                Else
                    strWhereCond = strWhereCond & " AND refundcostcode = '" & Trim(ddlRefCostcode.Items(ddlRefCostcode.SelectedIndex).Text) & "'"
                End If
            End If

            BuildCondition = strWhereCond
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            strSqlQry = "select servicecat,dispname,incomecode,costcode,refundincomecode,refundcostcode,[Active]=case active when 1 then 'Active' else 'In Active' end," & _
                        " AddDate,AddUser,ModDate,ModUser,acctmast.acctname" & _
                        " FROM profitcentremast INNER JOIN acctmast ON profitcentremast.incomecode = acctmast.acctcode "



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

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try

    End Sub
#End Region

#Region " Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gv_SearchResult.PageIndex = e.NewPageIndex
        FillGrid("servicecat")
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
                'Response.Redirect("ProfitCenter.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('ProfitCenter.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','ProfitCenter','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("ProfitCenter.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('ProfitCenter.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','ProfitCenter','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "DeleteRow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("ProfitCenter.aspx", False)
                Dim strpop As String = ""
                strpop = "window.open('ProfitCenter.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','ProfitCenter','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ProfitCenterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gv_SearchResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_SearchResult.RowDeleting

    End Sub

    Protected Sub gv_SearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_SearchResult.RowEditing

    End Sub

    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub

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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillGrid("servicecat")
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDisplayname.Value = ""
        txtServicecode.Value = ""
        ddlCostcode.Value = "[Select]"
        ddlCostname.Value = "[Select]"
        ddlIncomecode.Value = "[Select]"
        ddlIncomename.Value = "[Select]"
        ddlRefCostcode.Value = "[Select]"
        ddlRefCostname.Value = "[Select]"
        ddlRefIncomecode.Value = "[Select]"
        ddlRefIncomename.Value = "[Select]"
        FillGrid("servicecat")
    End Sub

#Region "Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click"
    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Dim DS As New DataSet
        Dim DA As SqlDataAdapter
        Dim con As SqlConnection
        Dim objcon As New clsDBConnect
        Try
            If gv_SearchResult.Rows.Count <> 0 Then

                strSqlQry = "select servicecat as [Service Category],dispname as [Display Name],incomecode as [Income Code],costcode as [Cost of Sale Code], refundincomecode as [Refund Income Code],refundcostcode as [Refund Cost Code]," & _
                            " [Active]=case active when 1 then 'Active' else 'In Active' end," & _
                            " (Convert(Varchar, Datepart(DD,adddate))+ '/'+ Convert(Varchar, Datepart(MM,adddate))+ '/'+ Convert(Varchar, Datepart(YY,adddate)) + ' ' + Convert(Varchar, Datepart(hh,adddate))+ ':' + Convert(Varchar, Datepart(m,adddate))+ ':'+ Convert(Varchar, Datepart(ss,adddate))) as [Date Created],AddUser as [User Created], " & _
                            " (Convert(Varchar, Datepart(DD,moddate))+ '/'+ Convert(Varchar, Datepart(MM,moddate))+ '/'+ Convert(Varchar, Datepart(YY,moddate)) + ' ' + Convert(Varchar, Datepart(hh,moddate))+ ':' + Convert(Varchar, Datepart(m,moddate))+ ':'+ Convert(Varchar, Datepart(ss,moddate))) as [Date Modified],moduser as [User Modified] " & _
                            " FROM profitcentremast  "


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " where " & BuildCondition() & " ORDER BY servicecat "
                Else
                    strSqlQry = strSqlQry & " ORDER BY servicecat"
                End If

                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "profit")

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

#Region " Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblCostcode.Visible = True
        lblCostname.Visible = True
        lblInccode.Visible = True
        lblIncname.Visible = True
        lblRefCostcode.Visible = True
        lblRefCostname.Visible = True
        lblRefInccode.Visible = True
        lblRefIncname.Visible = True
        ddlCostcode.Visible = True
        ddlCostname.Visible = True
        ddlIncomecode.Visible = True
        ddlIncomename.Visible = True
        ddlRefCostcode.Visible = True
        ddlRefCostname.Visible = True
        ddlRefIncomecode.Visible = True
        ddlRefIncomename.Visible = True
    End Sub
#End Region

#Region " Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblCostcode.Visible = False
        lblCostname.Visible = False
        lblInccode.Visible = False
        lblIncname.Visible = False
        lblRefCostcode.Visible = False
        lblRefCostname.Visible = False
        lblRefInccode.Visible = False
        lblRefIncname.Visible = False
        ddlCostcode.Visible = False
        ddlCostname.Visible = False
        ddlIncomecode.Visible = False
        ddlIncomename.Visible = False
        ddlRefCostcode.Visible = False
        ddlRefCostname.Visible = False
        ddlRefIncomecode.Visible = False
        ddlRefIncomename.Visible = False
    End Sub
#End Region


#Region " Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click"
    Protected Sub BtnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("ProfitCenter.aspx", False)
        Dim strpop As String = ""
        strpop = "window.open('ProfitCenter.aspx?State=New','ProfitCenter','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
#End Region


    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
        Try
            Dim strReportTitle As String = ""
            Dim strSelectionFormula As String = ""
            'Session("ColReportParams") = Nothing
            'Session.Add("Pageame", "Profit Center Master")
            'Session.Add("BackPageName", "ProfitCenterSearch.aspx")

            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Profit Center Master&BackPageName=ProfitCenterSearch.aspx&ScatCode=" & txtServicecode.Value.Trim & "&DispName=" & txtDisplayname.Value.Trim & "&IncomeCode=" & Trim(ddlIncomecode.Items(ddlIncomecode.SelectedIndex).Text) & "&CostCode=" & Trim(ddlCostcode.Items(ddlCostcode.SelectedIndex).Text) & "&RefIncomeCode=" & Trim(ddlRefIncomecode.Items(ddlRefIncomecode.SelectedIndex).Text) & "&RefCostCode=" & Trim(ddlRefCostcode.Items(ddlRefCostcode.SelectedIndex).Text) & "','ProfitCenterMast','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DefineGroupSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ProfitCenterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
