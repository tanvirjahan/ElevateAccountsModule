
'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceSellingTypes.aspx
'   Developer Name :    Amit Survase
'   Date           :    29 June 2008
'   
'
'-----------

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class Visa_Selling_Types_Search
    Inherits System.Web.UI.Page

#Region "Global Declarations"
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
        visasellcode = 1
        visasellname = 2
        Curency = 3
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


                SetFocus(txtSellingCode)
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    If Request.QueryString("Type") = "HF" Then
                        Lblselltypes.Text = "HandlingFees Selling Types List"

                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                    CType(strappname, String), "PriceListModule\VisaSellingTypesSearch.aspx?Type=HF", btnAddNew, btnExportToExcel, _
                                                              btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)

                    Else
                        Lblselltypes.Text = "Visa Selling Types List"
                        objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                    CType(strappname, String), "PriceListModule\VisaSellingTypesSearch.aspx", btnAddNew, btnExportToExcel, _
                                                    btnPrint, gv_SearchResult, GridCol.Edit, GridCol.Delete, GridCol.View)


                    End If
                End If

                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrCode, "currcode", "select currcode from currmast where active=1 order by currcode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrName, "currname", "select currname from currmast where active=1 order by currname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurCode, "currcode", "currname", "select currcode,currname from currmast where active=1 order by currcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurName, "currname", "currcode", "select currname,currcode from currmast where active=1 order by currname", True)

                Session.Add("strsortExpression", "visasellcode")
                Session.Add("strsortdirection", SortDirection.Ascending)

                'FillGrid("othsellcode")
                fillorderby()
                FillGrid("visasellmast.visasellname")
                Dim typ As Type
                typ = GetType(DropDownList)


                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCurCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCurName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                'btnPrint.Visible = True

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "OtherSerSelltypeWindowPostBack") Then
            btnSearch_Click(sender, e)
        End If
    End Sub


#End Region


#Region "  Private Function BuildCondition() As String"

    Private Function BuildCondition() As String
        strWhereCond = ""
        If txtSellingCode.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(visasellmast.visasellcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(visasellmast.visasellcode) LIKE '" & Trim(txtSellingCode.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If txtSellingName.Text.Trim <> "" Then
            If Trim(strWhereCond) = "" Then

                strWhereCond = " upper(visasellmast.visasellname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            Else
                strWhereCond = strWhereCond & " AND upper(visasellmast.visasellname) LIKE '" & Trim(txtSellingName.Text.Trim.ToUpper) & "%'"
            End If
        End If

        If ddlCurCode.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(currmast.currcode) = '" & Trim(CType(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(currmast.currcode) = '" & Trim(CType(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text, String)) & "'"
            End If
        End If


        If ddlCurName.Value <> "[Select]" Then
            If Trim(strWhereCond) = "" Then
                strWhereCond = " upper(currmast.currname) = '" & Trim(CType(ddlCurName.Items(ddlCurName.SelectedIndex).Text, String)) & "'"
            Else
                strWhereCond = strWhereCond & " AND upper(currmast.currname) = '" & Trim(CType(ddlCurName.Items(ddlCurName.SelectedIndex).Text, String)) & "'"
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

            strSqlQry = ""

            'strSqlQry = "SELECT othsellmast.othsellcode,case othsellmast.costyesno  when 1 then 'Yes' else 'No' end costyesno,othsellmast.othsellname, othsellmast.currcode, currmast.currname, othsellmast.active, " & _
            '            "othsellmast.adddate, othsellmast.adduser, othsellmast.moddate, othsellmast.moduser " & _
            '            "FROM currmast INNER JOIN " & _
            '            "othsellmast ON currmast.currcode = othsellmast.currcode "

            strSqlQry = "SELECT visasellmast.visasellcode,visasellmast.visasellname, visasellmast.currcode, currmast.currname,(select case when visasellmast.active=1 then 'Active' else 'Inactive' end) as Active, " & _
                      "visasellmast.adddate, visasellmast.adduser, visasellmast.moddate, visasellmast.moduser " & _
                      "FROM currmast INNER JOIN " & _
                      "visasellmast ON currmast.currcode = visasellmast.currcode "

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
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

#End Region


#Region "Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click"

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        'Session.Add("State", "New")
        'Response.Redirect("Other Services Selling Types.aspx", False)

        Dim strpop As String = ""
        'strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=New','VisaSellingTypesSearch','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=New','VisaSellingTypesSearch');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

#End Region

#Region "Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging"

    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        gv_SearchResult.PageIndex = e.NewPageIndex
        'FillGrid("othsellmast.othsellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("visasellmast.visasellname")
            Case 1
                FillGrid("visasellmast.visasellcode")
            Case 2
                FillGrid("currmast.currcode")

        End Select
    End Sub

#End Region


#Region "Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand"

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand

        Try
            If e.CommandName = "Page" Then Exit Sub
            If e.CommandName = "Sort" Then Exit Sub

            Dim lblId As Label
            lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblothsellcode")

            If e.CommandName = "Editrow" Then
                'Session.Add("State", "Edit")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Other Services Selling Types.aspx", False)


                Dim strpop As String = ""
                'strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=Edit&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "View" Then
                'Session.Add("State", "View")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Other Services Selling Types.aspx", False)

                Dim strpop As String = ""
                'strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=View&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf e.CommandName = "Deleterow" Then
                'Session.Add("State", "Delete")
                'Session.Add("RefCode", CType(lblId.Text.Trim, String))
                'Response.Redirect("Other Services Selling Types.aspx", False)
                Dim strpop As String = ""
                'strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes','width='+screen.availWidth+' height='+screen.availHeight+' left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('VisaSellingTypes.aspx?Type=visasell&State=Delete&RefCode=" + CType(lblId.Text.Trim, String) + "','OtherServicesSellingTypes');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'FillGrid("othsellcode")
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("visasellmast.visasellname")
            Case 1
                FillGrid("visasellmast.visasellcode")
            Case 2
                FillGrid("currmast.currcode")

        End Select

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
        Try
            If gv_SearchResult.Rows.Count <> 0 Then
                'Response.ContentType = "application/vnd.ms-excel"
                'Response.Charset = ""
                'Me.EnableViewState = False

                'Dim tw As New System.IO.StringWriter()
                'Dim hw As New System.Web.UI.HtmlTextWriter(tw)
                'Dim frm As HtmlForm = New HtmlForm()
                'Me.Controls.Add(frm)
                'frm.Controls.Add(gv_SearchResult)
                'frm.RenderControl(hw)
                'Response.Write(tw.ToString())
                'Response.End()
                'Response.Clear()

                '             strSqlQry = "SELECT othsellmast.othsellcode AS [Selling Code], othsellmast.othsellname AS [Selling Name], " & _
                '"othsellmast.currcode AS [Currency Code], [Active]=case when othsellmast.active =1 then 'Active'   when othsellmast.active=0 then 'InActive' end , " & _
                '"(Convert(Varchar, Datepart(DD,othsellmast.adddate))+'/'+ Convert(Varchar, Datepart(MM,othsellmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,othsellmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,othsellmast.adddate))+ ':' + Convert(Varchar, Datepart(m,othsellmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,othsellmast.adddate))) as [Date Created],othsellmast.adduser as [User Created],(Convert(Varchar, Datepart(DD,othsellmast.moddate))+ '/' + Convert(Varchar, Datepart(MM,othsellmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,othsellmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,othsellmast.moddate))+ ':'+ Convert(Varchar, Datepart(hh,othsellmast.moddate))+ ':' + Convert(Varchar, Datepart(ss,othsellmast.moddate))) as [Date Modified],othsellmast.moduser as [User Modified]  " & _
                ' "FROM currmast INNER JOIN othsellmast ON currmast.currcode = othsellmast.currcode"

                strSqlQry = "SELECT visasellmast.visasellcode AS [Visa Selling Code], visasellmast.visasellname AS [Visa Selling Name], " & _
"visasellmast.currcode AS [Currency Code], [Active]=case when visasellmast.active =1 then 'Active'   when visasellmast.active=0 then 'InActive' end , " & _
"(Convert(Varchar, Datepart(DD,visasellmast.adddate))+'/'+ Convert(Varchar, Datepart(MM,visasellmast.adddate))+ '/'+ Convert(Varchar, Datepart(YY,visasellmast.adddate)) + ' ' + Convert(Varchar, Datepart(hh,visasellmast.adddate))+ ':' + Convert(Varchar, Datepart(m,visasellmast.adddate))+ ':'+ Convert(Varchar, Datepart(ss,visasellmast.adddate))) as [Date Created],visasellmast.adduser as [User Created],(Convert(Varchar, Datepart(DD,visasellmast.moddate))+ '/' + Convert(Varchar, Datepart(MM,visasellmast.moddate))+ '/'+ Convert(Varchar, Datepart(YY,visasellmast.moddate)) + ' ' + Convert(Varchar, Datepart(hh,visasellmast.moddate))+ ':'+ Convert(Varchar, Datepart(hh,visasellmast.moddate))+ ':' + Convert(Varchar, Datepart(ss,visasellmast.moddate))) as [Date Modified],visasellmast.moduser as [User Modified]  " & _
 "FROM currmast INNER JOIN visasellmast ON currmast.currcode = visasellmast.currcode"



                If Trim(BuildCondition) <> "" Then
                    strSqlQry = strSqlQry & " WHERE " & BuildCondition() & " ORDER BY visasellcode "
                Else
                    strSqlQry = strSqlQry & " ORDER BY visasellcode"
                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))

                myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
                myDataAdapter.Fill(DS, "visasellmast")

                objUtils.ExportToExcel(DS, Response)
                clsDBConnect.dbConnectionClose(SqlConn)
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
            If Request.QueryString("Type") = "HF" Then
                ' strpop = "window.open('rptReportNew.aspx?Pageame=Handling Fees Selling Type&BackPageName=VisaSellingTypesSearch.aspx?Type=HF&SellCode=" & txtSellingCode.Text.Trim & "&SellName=" & txtSellingName.Text.Trim & "&CurrCode=" & Trim(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlCurName.Items(ddlCurName.SelectedIndex).Text) & "','OthSellType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=Handling Fees Selling Type&BackPageName=VisaSellingTypesSearch.aspx?Type=HF&SellCode=" & txtSellingCode.Text.Trim & "&SellName=" & txtSellingName.Text.Trim & "&CurrCode=" & Trim(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlCurName.Items(ddlCurName.SelectedIndex).Text) & "','OthSellType');"
            Else
                'strpop = "window.open('rptReportNew.aspx?Pageame=VisaSellingTypesSearch&BackPageName=VisaSellingTypesSearch.aspx&visasellcode=" & txtSellingCode.Text.Trim & "&visasellname=" & txtSellingName.Text.Trim & "&CurrCode=" & Trim(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlCurName.Items(ddlCurName.SelectedIndex).Text) & "','OthSellType','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptReportNew.aspx?Pageame=VisaSellingTypesSearch&BackPageName=VisaSellingTypesSearch.aspx&visasellcode=" & txtSellingCode.Text.Trim & "&visasellname=" & txtSellingName.Text.Trim & "&CurrCode=" & Trim(ddlCurCode.Items(ddlCurCode.SelectedIndex).Text) & "&CurrName=" & Trim(ddlCurName.Items(ddlCurName.SelectedIndex).Text) & "','OthSellType');"
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rbtnsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustSect.Visible = False
        lblcurrcode.Visible = False
        lblcurrname.Visible = False
        ddlCurCode.Visible = False
        ddlCurName.Visible = False
    End Sub

    Protected Sub rbtnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'PnlCustSect.Visible = True
        lblcurrcode.Visible = True
        lblcurrname.Visible = True
        ddlCurCode.Visible = True
        ddlCurName.Visible = True
    End Sub

    '#Region "Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)"

    '    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    '    End Sub
    '#End Region
#Region "Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs)"

    Protected Sub btnClear_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        txtSellingCode.Text = ""
        txtSellingName.Text = ""
        ddlCurCode.Value = "[Select]"
        ddlCurName.Value = "[Select]"
        ddlOrderBy.SelectedIndex = 0
        'FillGrid("othsellcode")
        FillGrid("visasellmast.visasellname")
    End Sub
#End Region

    'Protected Sub ddlCurrName_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)

    'End Sub
    Private Sub fillorderby()
        ddlOrderBy.Items.Clear()
        ddlOrderBy.Items.Add("Selling Name")
        ddlOrderBy.Items.Add("Selling Code")
        ddlOrderBy.Items.Add("Currency code")
        ddlOrderBy.SelectedIndex = 0
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOrderBy.SelectedIndexChanged
        Select Case ddlOrderBy.SelectedIndex
            Case 0
                FillGrid("visasellmast.visasellname")
            Case 1
                FillGrid("visasellmast.visasellcode")
            Case 2
                FillGrid("currmast.currcode")

        End Select
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesSellingTypesSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub






End Class
