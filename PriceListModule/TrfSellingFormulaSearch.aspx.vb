Imports System.Data
Imports System.Data.SqlClient

Partial Class PriceListModule_TrfSellingFormulaSearch
    Inherits System.Web.UI.Page

#Region "Global Declaration"
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
        SellingCodeTCol = 0
        SellingCode = 1
        SellingName = 2
        CurrencyCode = 3
        FormulaFrom = 4
        FormulaCurrency = 5
        Formula = 6
        DateCreated = 7
        UserCreated = 8
        DateModified = 9
        UserModified = 10
        Edit = 11
        View = 12
        Delete = 13

    End Enum
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                SetFocus(txtSellingCode)
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
                                                       CType(strappname, String), "PriceListModule\TrfSellingFormulaSearch.aspx", btnAddNew, btnExportToExcel, _
                                                       btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                End If
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "select currcode from sellmast where active=1 order by currcode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrencyName, "currname", "select currname from currmast where active=1 order by currname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrencyName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currcode", True)

                Session.Add("strsortExpression", "sellmast.currcode")
                Session.Add("strsortdirection", SortDirection.Ascending)
                '  FillDDL()
                fillorderby()
                FillGrid("sellmast.sellname")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TrfSellingFormulaSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


        End If



        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "TrfSellingPriceFormulaeWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If


    End Sub
#End Region

#Region "Private Function BuildCondition() As String"
    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtSellingCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(sellmast.sellcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(sellmast.sellcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtSellingName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(sellmast.sellname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(sellmast.sellname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            End If
        End If
        If ddlCurrencyCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(sellmast.currcode) = '" & Trim(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(sellmast.currcode) = '" & Trim(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text.Trim.ToUpper) & "'"
            End If
        End If

        BuildCondition = strWhereCond
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
            'strSqlQry = " SELECT sellmast.sellcode,sellmast.sellname,sellmast.currcode,sellformulas.calcfrom, " & _
            '" case  when isnull(sellformulas.fmlacurr,0)=0 then 'Supplier' when isnull(sellformulas.fmlacurr,0)=1 " & _
            '" then 'Selling Type' end as fmlacurr, sellformulas.sellstring," & _
            '"sellformulas.adddate, sellformulas.adduser, sellformulas.moddate, sellformulas.moduser" & _
            '" FROM sellmast INNER JOIN sellformulas ON  sellmast.sellcode = sellformulas.sellcode"


            strSqlQry = "SELECT sellmast.sellcode,sellmast.sellname,sellmast.currcode,trf_sellformulas.calcfrom," & _
                        "case  when isnull(trf_sellformulas.fmlacurr,0)=0 then 'Selling Type' when isnull(trf_sellformulas.fmlacurr,0)=1" & _
                        " then 'Supplier' end as fmlacurr, trf_sellformulas.sellstring," & _
                        " trf_sellformulas.adddate, trf_sellformulas.adduser, trf_sellformulas.moddate, trf_sellformulas.moduser" & _
                        " FROM sellmast INNER JOIN trf_sellformulas ON  sellmast.sellcode = trf_sellformulas.sellcode"



            If Trim(BuildCondition) <> "" Then
                strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
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
                lblMsg.Text = "Records not found. Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfSellingFormulaSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
#End Region

#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("SellingPriceFormulas.aspx", False)

        Dim strpop As String = ""
        strpop = "window.open('TrfSellingFormulas.aspx?State=New','Trfsellformulae','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"
    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("sellmast.sellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("sellmast.sellname")
            Case 1
                FillGrid("sellmast.sellcode")
            Case 2
                FillGrid("sellmast.currcode")
        End Select
    End Sub
#End Region

#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"
    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub
            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblCode")

            If e.CommandName = "Editrow" Then


                Dim strpop As String = ""
                strpop = "window.open('TrfSellingFormulas.aspx?State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','Trfsellformulae','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SellingPriceFormulas.aspx", False)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SellingPriceFormulas.aspx", False)



                Dim strpop As String = ""
                strpop = "window.open('TrfSellingFormulas.aspx?State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','Trfsellformulae','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("SellingPriceFormulas.aspx", False)


                Dim strpop As String = ""
                strpop = "window.open('TrfSellingFormulas.aspx?State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','Trfsellformulae','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TrfSellingFormulas.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("sellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("sellmast.sellname")
            Case 1
                FillGrid("sellmast.sellcode")
            Case 2
                FillGrid("sellmast.currcode")
        End Select
    End Sub
#End Region

#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click"
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtSellingCode.Text = ""
        txtSellingName.Text = ""
        ddlCurrencyCode.Value = "[Select]"
        ddlCurrencyName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        'FillGrid("sellcode")
        FillGrid("sellmast.sellname")
    End Sub
#End Region


#Region "Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting"
    Protected Sub gv_SearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_SearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColoumn_click()
    End Sub
#End Region

#Region " Public Sub SortGridColoumn_click()"
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
                
                'strSqlQry = "SELECT sellformulas.sellcode as [Selling Code],sellmast.sellname as [Selling Name], " & _
                ' "  sellmast.currcode as [Curremcy Code],sellformulas.calcfrom as [Formula From],  " & _
                ' " case  when isnull(sellformulas.fmlacurr,0)=0 then 'Supplier' when isnull(sellformulas.fmlacurr,0)=1 " & _
                ' " then 'Selling Type' end  as [Formula Currency] ," & _
                ' " sellformulas.sellstring as [Formula]," & _
                ' "(Convert(Varchar, Datepart(DD,sellformulas.adddate))+ '/'+ Convert(Varchar, Datepart(MM,sellformulas.adddate))+ '/'+ Convert(Varchar, Datepart(YY,sellformulas.adddate)) + ' ' + Convert(Varchar, Datepart(hh,sellformulas.adddate))+ ':' + Convert(Varchar, Datepart(m,sellformulas.adddate))+ ':'+ Convert(Varchar, Datepart(ss,sellformulas.adddate))) as [Date Created]," & _
                '"sellformulas.adduser as [User Created],sellformulas.moduser as [User Modified],(Convert(Varchar, Datepart(DD,sellformulas.moddate))+ '/'+ Convert(Varchar, Datepart(MM,sellformulas.moddate))+ '/'+ Convert(Varchar, Datepart(YY,sellformulas.moddate)) + ' ' + Convert(Varchar, Datepart(hh,sellformulas.moddate))+ ':' + Convert(Varchar, Datepart(m,sellformulas.moddate))+ ':'+ Convert(Varchar, Datepart(ss,sellformulas.moddate))) as [Date Modified]" & _
                '" FROM sellmast INNER JOIN sellformulas ON  sellmast.sellcode = sellformulas.sellcode"


                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY sellcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY sellmast.sellcode"
                End If
                con = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                DA = New SqlDataAdapter(strSqlQry, con)
                DA.Fill(DS, "sellformulas")

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

            'Session.Add("Pageame", "Selling Price Formulas")
            'Session.Add("BackPageName", "SellingPriceFormulasSearch.aspx")
            Dim strpop As String = ""
            strpop = "window.open('rptReportNew.aspx?Pageame=Transfer Selling Formulas&BackPageName=TrfSellingFormulaSearch.aspx&SellCode=" & txtSellingCode.Text.Trim & "&Sellname=" & txtSellingName.Text.Trim & "&CurrCode=" & Trim(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text) & "','RepSellFormula','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            'If txtSellingCode.Text.Trim <> "" Then
            '    strReportTitle = "Selling Code : " & txtSellingCode.Text.Trim
            '    strSelectionFormula = "{sellformulas.sellcode} LIKE '" & txtSellingCode.Text.Trim & "*'"
            'End If
            'If txtSellingName.Text.Trim <> "" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & "  Selling Name : " & txtSellingName.Text.Trim
            '        strSelectionFormula = strSelectionFormula & " and {sellformulas.sellname} LIKE '" & txtSellingName.Text.Trim & "*'"
            '    Else
            '        strReportTitle = "Selling Name : " & txtSellingName.Text.Trim
            '        strSelectionFormula = "{sellformulas.sellname} LIKE '" & txtSellingName.Text.Trim & "*'"
            '    End If
            'End If
            'If ddlCurrencyCode.Value.Trim <> "[Select]" Then
            '    If strSelectionFormula <> "" Then
            '        strReportTitle = strReportTitle & "  Currency   : " & Trim((CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Value, String)))
            '        strSelectionFormula = strSelectionFormula & " and {sellmast.currcode} = '" & Trim((CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String))) & "'"
            '    Else
            '        strReportTitle = "Currency  : " & Trim((CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Value, String)))
            '        strSelectionFormula = "{sellmast.currcode} = '" & Trim((CType(ddlCurrencyCode.Items(ddlCurrencyCode.SelectedIndex).Text, String))) & "'"
            '    End If
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Response.Redirect("rptReport.aspx", False)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SellingPriceFormulasSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSellingFormula.Visible = False

        lblcurrcode.Visible = False
        ddlCurrencyCode.Visible = False
        ddlCurrencyName.Visible = False
        lblcurrname.Visible = False
    End Sub
#End Region

#Region "Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'pnlSellingFormula.Visible = True

        lblcurrcode.Visible = True
        ddlCurrencyCode.Visible = True
        ddlCurrencyName.Visible = True
        lblcurrname.Visible = True
    End Sub
#End Region
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Selling Name")
        ddlOrderBy.Items.Add("Selling Code")
        ddlOrderBy.Items.Add("Currency Code")

        ddlOrderBy.SelectedIndex = 0
    End Sub
    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("sellmast.sellname")
            Case 1
                FillGrid("sellmast.sellcode")
            Case 2
                FillGrid("sellmast.currcode")
        End Select
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=TrfSellingFormulaSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
