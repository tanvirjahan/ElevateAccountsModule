Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptProfitabilityReportsearch
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String, todate As String
    Dim groupby, rpttype, booktype As Integer
    Dim supcode, supcodeto, plgrpcode, plgrpcodeto, acccode, acccodeto, agentcode, agentcodeto, Type, poststate, voucher, strrpttype1, strclosing As String

    Dim salescodefrom, salescodeto As String
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
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

            If Request.QueryString("supcode") <> "" Then
                supcode = Trim(Request.QueryString("supcode"))
            End If
            If Request.QueryString("supcodeto") <> "" Then
                supcodeto = Trim(Request.QueryString("supcodeto"))
            End If

            If Request.QueryString("plgrpcode") <> "" Then
                plgrpcode = Trim(Request.QueryString("plgrpcode"))
            End If

            If Request.QueryString("plgrpcodeto") <> "" Then
                plgrpcodeto = Trim(Request.QueryString("plgrpcodeto"))
            End If

            If Request.QueryString("acccode") <> "" Then
                acccode = Trim(Request.QueryString("acccode"))
            End If

            If Request.QueryString("acccodeto") <> "" Then
                acccodeto = Trim(Request.QueryString("acccodeto"))
            End If

            If Request.QueryString("agentcode") <> "" Then
                agentcode = Trim(Request.QueryString("agentcode"))
            End If
            If Request.QueryString("agentcodeto") <> "" Then
                agentcodeto = Trim(Request.QueryString("agentcodeto"))
            End If
            If Request.QueryString("groupby") <> "" Then
                groupby = Trim(Request.QueryString("groupby"))
            End If

            If Request.QueryString("rpttype") <> "" Then
                rpttype = Trim(Request.QueryString("rpttype"))
            End If

            If Request.QueryString("booktype") <> "" Then
                booktype = Trim(Request.QueryString("booktype"))
            End If

            If Request.QueryString("salescodefrom") <> "" Then
                salescodefrom = Trim(Request.QueryString("salescodefrom"))
            End If
            If Request.QueryString("salescodeto") <> "" Then
                salescodeto = Trim(Request.QueryString("salescodeto"))
            End If
           




            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'BindReport()
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""
        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        'rptreportname = "Arrival Report"

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo
        Dim reportfilter As String
        'With ConnInfo
        '    .ServerName = ConfigurationManager.AppSettings("dbServerName")
        '    .DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
        '    .UserID = ConfigurationManager.AppSettings("dbUserName")
        '    .Password = ConfigurationManager.AppSettings("dbPassword")
        'End With
        With ConnInfo
            .ServerName = Session("dbServerName")
            .DatabaseName = Session("dbDatabaseName")
            .UserID = Session("dbUserName")
            .Password = Session("dbPassword")
        End With


       
        rptreportname = " Profitablity Report"
        If rpttype = 0 Then
            rep.Load(Server.MapPath("..\Report\Profitablityreport.rpt"))
        ElseIf rpttype = 1 And groupby <> 0 Then
            rep.Load(Server.MapPath("..\Report\Profitablityreportdetail.rpt"))
        ElseIf rpttype = 1 And groupby = 0 Then
            rep.Load(Server.MapPath("..\Report\Profitablityreportdetailcustomerwise.rpt"))
        End If


        Me.CRVReport.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = rptreportname

        pnames = rep.DataDefinition.ParameterFields

        reportfilter = ""

        reportfilter = "From: " + Format(CType(fromdate, Date), "dd/MM/yyyy") + "  To: " + Format(CType(todate, Date), "dd/MM/yyyy")



        pname = pnames.Item("CompanyName")
        paramvalue.Value = rptcompanyname
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("ReportName")
        paramvalue.Value = rptreportname
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If Trim(plgrpcodeto) <> "" And plgrpcode <> "" Then
            reportfilter = reportfilter + " And  Market From " + plgrpcode + " To : " + Trim(plgrpcodeto)
        End If

        If Trim(acccode) <> "" And acccodeto <> "" Then
            reportfilter = reportfilter + " And  Account From " + acccode + " To : " + Trim(acccodeto)
        End If

        If Trim(acccode) <> "" And acccodeto <> "" Then
            reportfilter = reportfilter + " And  Agent From " + agentcode + " To : " + Trim(agentcodeto)
        End If

        If Trim(salescodefrom) <> "" And salescodeto <> "" Then
            reportfilter = reportfilter + " And  Sales From " + salescodefrom + " To : " + Trim(salescodeto)
        End If

        If rpttype = 1 Then
            pname = pnames.Item("groupby")
            paramvalue.Value = groupby
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("curr")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@booktype")
            paramvalue.Value = booktype
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            reportfilter = reportfilter + "  And Report Group by : " + IIf(groupby = 1, "Marketwise", "Customerwise")

        End If


        If rpttype = 1 Then

            If Val(booktype) = 1 Then
                reportfilter = reportfilter + " And Booking Type : Hotel "
            ElseIf Val(booktype) = 2 Then
                reportfilter = reportfilter + " And Booking Type : Transfers "
            ElseIf Val(booktype) = 3 Then
                reportfilter = reportfilter + " And Booking Type : Visa "
            ElseIf Val(booktype) = 4 Then
                reportfilter = reportfilter + " And Booking Type : Excursions "
            ElseIf Val(booktype) = 5 Then
                reportfilter = reportfilter + " And Booking Type : Others "
            ElseIf Val(booktype) = 6 Then
                reportfilter = reportfilter + " And Booking Type : Packages "
            End If
        End If


        pname = pnames.Item("@fromdate")
        paramvalue.Value = Trim(Format(CDate(fromdate), "yyyy/MM/dd"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@todate")
        paramvalue.Value = Trim(Format(CDate(todate), "yyyy/MM/dd"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        'reportfilter = "from: " + fromdate + "  To: " + todate




        pname = pnames.Item("@frmplgrpcode")
        paramvalue.Value = plgrpcode
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toplgrpcode")
        paramvalue.Value = Trim(plgrpcodeto)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@fromagent")
        paramvalue.Value = agentcode
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toagent")
        paramvalue.Value = agentcodeto
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@frmparty")
        paramvalue.Value = supcode
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toparty")
        paramvalue.Value = supcodeto
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@frmacct")
        paramvalue.Value = acccode
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toacct")
        paramvalue.Value = acccodeto
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@fromsales")
        paramvalue.Value = salescodefrom
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@tosales")
        paramvalue.Value = salescodeto
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)



        pname = pnames.Item("repfilter")
        paramvalue.Value = reportfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Session.Add("ReportSource", rep)
        '  Me.CRVReport.ReportSource = rep
        Me.CRVReport.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If

    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'If Request.QueryString("Type") = "Profit" Then

        '    Response.Redirect("rptProfitLoss.aspx?frmdate=" & fromdate & "&todate=" & todate & "&level=" & level, False)

        'Else
        '    Select Case accttype
        '        Case "RV"
        '            Response.Redirect("ReceiptsSearch.aspx?tran_type=RV")
        '        Case "PV"
        '            Response.Redirect("ReceiptsSearch.aspx?tran_type=PV")

        '    End Select
        'End If
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

End Class
