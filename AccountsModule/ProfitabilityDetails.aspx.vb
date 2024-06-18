Imports System.Data
Imports System.Data.SqlClient

Partial Class AccountsModule_ProfitabilityDetails
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter    
    Dim sqlTrans As SqlTransaction
    Dim fromdate As String = String.Empty
    Dim todate As String = String.Empty
    Dim frmplgrpcode As String = String.Empty
    Dim toplgrpcode As String = String.Empty
    Dim fromagent As String = String.Empty
    Dim toagent As String = String.Empty
    Dim frmparty As String = String.Empty
    Dim toparty As String = String.Empty
    Dim frmacct As String = String.Empty
    Dim toacct As String = String.Empty
    Dim booktype As Integer = 0 '0 --All ,1 --Hotel,2 --Transfer,3 --Visa ,4 --Excursions , 6--Packages,5 --others
    Dim fromsales As String = String.Empty
    Dim tosales As String = String.Empty
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                ViewState.Add("Pageame", Request.QueryString("Pageame"))
                ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

                If Request.QueryString("fromdate") <> "" Then
                    fromdate = Trim(Request.QueryString("fromdate"))
                End If
                If Request.QueryString("todate") <> "" Then
                    todate = Trim(Request.QueryString("todate"))
                End If

                If Request.QueryString("booktype") <> "" Then
                    booktype = Trim(Request.QueryString("booktype"))
                End If

                If Request.QueryString("plgrpcode") <> "" Then
                    frmplgrpcode = Trim(Request.QueryString("plgrpcode"))
                End If

                If Request.QueryString("plgrpcodeto") <> "" Then
                    toplgrpcode = Trim(Request.QueryString("plgrpcodeto"))
                End If

                If Request.QueryString("agentcode") <> "" Then
                    fromagent = Trim(Request.QueryString("agentcode"))
                End If
                If Request.QueryString("agentcodeto") <> "" Then
                    toagent = Trim(Request.QueryString("agentcodeto"))
                End If

                If Request.QueryString("supcode") <> "" Then
                    frmparty= Trim(Request.QueryString("supcode"))
                End If
                If Request.QueryString("supcodeto") <> "" Then
                    toparty = Trim(Request.QueryString("supcodeto"))
                End If

                If Request.QueryString("acccode") <> "" Then
                    frmacct = Trim(Request.QueryString("acccode"))
                End If

                If Request.QueryString("acccodeto") <> "" Then
                    toacct = Trim(Request.QueryString("acccodeto"))
                End If

                If Request.QueryString("salescodefrom") <> "" Then
                    fromsales = Trim(Request.QueryString("salescodefrom"))
                End If
                If Request.QueryString("salescodeto") <> "" Then
                    tosales = Trim(Request.QueryString("salescodeto"))
                End If

                Dim decpoints As Integer = Val(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT option_selected FROM reservation_parameters Where param_id = 509"))
                ViewState("DecPoints") = decpoints
                BindGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindGrid()
        Try
            
            Dim ds As New DataSet
            Dim strQry As String = "EXEC [dbo].[sp_show_GPdetailed] '" & fromdate & "','" & todate & "','" & frmplgrpcode & "','" & toplgrpcode & "','" & fromagent & "','" & toagent & "','" & frmparty & "','" & toparty & "','" & frmacct & "','" & toacct & "'," & booktype & ",'" & fromsales & "','" & tosales & "'"
            If ViewState("dsData") Is Nothing Then
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strQry)
                ViewState("dsData") = ds
            Else
                ds = ViewState("dsData")
            End If

            Dim dt As New DataTable
            Dim view As New DataView(ds.Tables(0))
            view.Sort = "plgrpcode"
            dt = view.ToTable
            'f ds.Tables.Count > 0 Then
            If dt.Rows.Count > 0 Then
                gv_SearchResult.DataSource = dt
                gv_SearchResult.DataBind()
            End If
            'BindTable(ds.Tables(0), "plgrpname")
            'End If

            GridGrouping()
            CalculateTotals()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub CalculateTotals()

    End Sub


    Private Sub BindTable(ByVal dt As DataTable, ByVal filtername As String)
        Try
            Dim mStringBuilder As New StringBuilder
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "SELECT plgrpname FROM plgrpmast Where active = 1")
            Dim newtbl As New Table
            Dim newtr As TableRow
            Dim newtd As TableCell
            Dim gv As GridView = gv_SearchResult


            Dim htmlhr As New TableRow            
            For j As Integer = 0 To dt.Columns.Count - 1
                Dim newhtd As New TableCell
                newhtd.Text = dt.Columns(j).ColumnName
                htmlhr.Cells.Add(newhtd)
            Next
            newtbl.Rows.Add(htmlhr)

            Dim view As New DataView(dt)
            view.Sort = "plgrpcode"
            dt = view.ToTable
            For j As Integer = dt.Rows.Count - 1 To 1 Step -1
                Dim prevcolval As String = ""
                Dim curval As String = ""
                prevcolval = dt.Rows(j - 1).Item(filtername)
                curval = dt.Rows(j).Item(filtername)
                newtr = New TableRow
                If prevcolval = curval Then
                    newtd = New TableCell
                    newtd.Attributes.Add("colspan", "10")
                    newtd.Text = curval
                    newtr.Cells.Add(newtd)
                    newtbl.Rows.Add(newtr)
                End If
                newtr = New TableRow                
            Next

        Catch ex As Exception

        End Try
    End Sub

    'Added By Riswan -- GROUPING GRID
    Private Sub GridGrouping()
        Try

            For i As Integer = gv_SearchResult.Rows.Count - 1 To 1 Step -1
                Dim row As GridViewRow = gv_SearchResult.Rows(i)
                Dim previousRow As GridViewRow = gv_SearchResult.Rows(i - 1)
                Dim hdnCurRowMkt As Label = row.FindControl("lblmarket")
                Dim hdnPrevRowMkt As Label = previousRow.FindControl("lblmarket")
                If hdnCurRowMkt.Text = hdnPrevRowMkt.Text Then
                    If previousRow.Cells(1).RowSpan = 0 Then
                        If row.Cells(1).RowSpan = 0 Then
                            previousRow.Cells(1).RowSpan += 2
                        Else
                            previousRow.Cells(1).RowSpan = row.Cells(1).RowSpan + 1
                        End If
                        row.Cells(1).Visible = False
                    End If
                End If
                previousRow.Attributes.Add("style", "border-bottom:1px solid")
                row.Attributes.Add("style", "border-bottom:1px solid")
            Next
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub gv_SearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_SearchResult.PageIndexChanging
        Try
            gv_SearchResult.PageIndex = e.NewPageIndex
            BindGrid()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        If e.CommandName = "Page" Then Exit Sub
        If e.CommandName = "Sort" Then Exit Sub
        Dim lblId As Label
        lblId = gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblId")

        If e.CommandName = "plreport" Then
            Dim rowindex As Integer = CInt(e.CommandArgument)
            Dim row As GridViewRow = gv_SearchResult.Rows(rowindex)
            Dim lblReqid As Label = DirectCast(row.FindControl("lblreqid"), Label)
            Dim strpop As String = ""
            strpop = "window.open('../Reservation/rptConfirmaton.aspx?reqid=" + lblReqid.Text + "&typ=printpl','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        End If
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try

            Dim lblProfit As Label
            Dim lblPercentage As Label
            Dim lblCredit As Label
            Dim lblDebit As Label
            Dim decpoints As Integer = 0
            If (e.Row.RowType = DataControlRowType.Header) Then
                Exit Sub
            End If            
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                lblProfit = e.Row.FindControl("lblProfit")
                lblPercentage = e.Row.FindControl("lblPercentage")
                lblCredit = e.Row.FindControl("lblCredit")
                lblDebit = e.Row.FindControl("lblDebit")
                If ViewState("DecPoints") Is Nothing Then
                    decpoints = Val(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "SELECT option_selected FROM reservation_parameters Where param_id = 509"))
                    ViewState("DecPoints") = decpoints
                Else
                    decpoints = ViewState("DecPoints")
                End If
                lblCredit.Text = Math.Round(Val(lblCredit.Text), decpoints)
                lblDebit.Text = Math.Round(Val(lblDebit.Text), decpoints)
                lblProfit.Text = Math.Round(Val(lblCredit.Text) - Val(lblDebit.Text), decpoints)
                If Val(lblCredit.Text) > 0 Then
                    Dim val1 As Decimal = Val(lblCredit.Text) - Val(lblDebit.Text)
                    lblPercentage.Text = Math.Round((val1 / Val(lblCredit.Text)) * 100, decpoints)
                Else
                    lblPercentage.Text = "0.00"
                End If

                Dim totper As Decimal = 0
                txtSaleVale.Text = Math.Round((Val(txtSaleVale.Text) + Val(lblCredit.Text)), decpoints)
                txtCostValue.Text = Math.Round((Val(txtCostValue.Text) + Val(lblDebit.Text)), decpoints)
                txtProfit.Text = Math.Round((Val(txtProfit.Text) + Val(lblProfit.Text)), decpoints)

                totper = Val(txtSaleVale.Text) - Val(txtCostValue.Text)
                totper = Math.Round((totper / Val(txtSaleVale.Text)) * 100, decpoints)
                txtPercentage.Text = totper
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim strReportTitle As String = ""

        Dim strfromdate As String = ""
        Dim strtodate As String = ""

        Dim strplgrpcode As String = ""
        Dim strplgrpcodeto As String = ""

        Dim stragentcode As String = ""
        Dim stragentcodeto As String = ""

        Dim strsupcode As String = ""
        Dim strsupcodeto As String = ""

        Dim stracccode As String = ""
        Dim stracccodeto As String = ""

        Dim strsalescode As String = ""
        Dim strsalescodeto As String = ""


        Dim strrepfilter As String = ""
        Dim strrpttype As Integer = 0


        strfromdate = Mid(Format(CType(Request.QueryString("fromdate"), Date), "yyyy/MM/dd"), 1, 10)
        strtodate = Mid(Format(CType(Request.QueryString("todate"), Date), "yyyy/MM/dd"), 1, 10)

        strrepfilter = "From Date: " & Format(CType(Request.QueryString("fromdate"), Date), "dd/MM/yyyy")
        strrepfilter = strrepfilter & " - To Date: " & Format(CType(Request.QueryString("todate"), Date), "dd/MM/yyyy")


        strplgrpcode = Request.QueryString("plgrpcode")
        strplgrpcodeto = Request.QueryString("plgrpcodeto")
        strsalescode = Request.QueryString("salescodefrom")
        strsalescodeto = Request.QueryString("salescodeto")
        stragentcode = Request.QueryString("agentcode")
        stragentcodeto = Request.QueryString("agentcodeto")        
        strsupcode = Request.QueryString("supcode")
        strsupcodeto = Request.QueryString("supcodeto")
        stracccode = Request.QueryString("acccode")
        stracccodeto = Request.QueryString("acccodeto")
        

        Dim strpop As String = "window.open('rptProfitabilityReportsearch.aspx?fromdate=" & strfromdate & "&todate=" & strtodate _
                 & "&supcode=" & strsupcode & "&supcodeto=" & strsupcodeto _
                 & "&plgrpcode=" & strplgrpcode & "&plgrpcodeto=" & strplgrpcodeto _
                 & "&salescodefrom=" & strsalescode & "&salescodeto=" & strsalescodeto _
                 & "&acccode=" & stracccode & "&acccodeto=" & stracccodeto _
                 & "&compliment=" & stragentcode & "&booktype=" & booktype & "&rpttype=1&groupby=1&agentcode=" & stragentcode & "&agentcodeto=" & stragentcodeto & "','Profitability','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    
End Class
