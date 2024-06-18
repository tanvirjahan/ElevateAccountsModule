Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class AccountsModule_CreditNote_Report
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim frmdate, todate, custcode, reqid, invoiceno, type, poststate, repname, creditnoteno As String


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Request.QueryString("fromdate") <> "" Then
                frmdate = Trim(Request.QueryString("fromdate"))
            End If
            If Request.QueryString("todate") <> "" Then
                todate = Trim(Request.QueryString("todate"))
            End If

            If Request.QueryString("custcode") <> "" Then
                custcode = Trim(Request.QueryString("custcode"))
            End If

            If Request.QueryString("reqid") <> "" Then
                reqid = Trim(Request.QueryString("reqid"))
            End If

            If Request.QueryString("creditnoteno") <> "" Then
                creditnoteno = Trim(Request.QueryString("creditnoteno"))
            End If

            If Request.QueryString("invoiceno") <> "" Then
                invoiceno = Trim(Request.QueryString("invoiceno"))
            End If

            If Request.QueryString("type") <> "" Then
                type = Trim(Request.QueryString("type"))
            End If

            If Request.QueryString("poststate") <> "" Then
                poststate = Trim(Request.QueryString("poststate"))
            End If

            If Request.QueryString("repname") <> "" Then
                repname = Trim(Request.QueryString("repname"))
            End If

            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'BindReport()
            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")

        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub

    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""
        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
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



        Select Case type
            Case "0"
                If repname = "salesregister" Then
                    rptreportname = " Sales Register -Summary"
                Else
                    rptreportname = " Reseservation CreditNote -Summary"
                End If

                rep.Load(Server.MapPath("..\Report\rptCreditNotebrief.rpt"))

            Case "1"

                If repname = "salesregister" Then
                    rptreportname = " Sales Register -Detailed"
                Else
                    rptreportname = " Reseservation CreditNote -Detailed"
                End If

                rep.Load(Server.MapPath("..\Report\rptCreditNotedetail.rpt"))
        End Select

        rep.SummaryInfo.ReportTitle = rptreportname
        pnames = rep.DataDefinition.ParameterFields
        reportfilter = ""

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

        pname = pnames.Item("creditnoteno")
        paramvalue.Value = Trim(creditnoteno)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = reportfilter + IIf(creditnoteno = "", "", " For CreditNote  " + creditnoteno)


        pname = pnames.Item("frmdate")
        paramvalue.Value = Trim(frmdate)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("todate")
        paramvalue.Value = Trim(todate)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = "From Date " + Format(CType(frmdate, Date), "dd/MM/yyyy") + " To Date " + Format(CType(todate, Date), "dd/MM/yyyy") + " "

        pname = pnames.Item("custcode")
        paramvalue.Value = Trim(custcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        reportfilter = reportfilter + IIf(custcode = "", "", " For customer  " + custcode)
        pname = pnames.Item("reqid")
        paramvalue.Value = Trim(reqid)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = reportfilter + IIf(reqid = "", "", " For File Number  " + reqid)

        pname = pnames.Item("invoiceno")
        paramvalue.Value = Trim(invoiceno)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = reportfilter + IIf(invoiceno = "", "", " For Invoice  " + invoiceno)

        pname = pnames.Item("curr")
        paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=457"), String))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("poststate")
        paramvalue.Value = Trim(poststate)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If Trim(poststate) <> "" Then
            If Trim(poststate) = "P" Then
                reportfilter = reportfilter + IIf(poststate = "", "", "; State: Posted ")
            ElseIf Trim(poststate) = "U" Then
                reportfilter = reportfilter + IIf(poststate = "", "", "; State: UnPosted ")
            End If
        End If

        pname = pnames.Item("repfilter")
        paramvalue.Value = reportfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)




        Me.CRVReport.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next



        Session.Add("ReportSource", rep)
        '  Me.CRVReport.ReportSource = rep
        Me.CRVReport.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Page.IsPostBack = True Then
            If ViewState("RepCalledFrom") <> 1 Then
                rep.Close()
                rep.Dispose()
            End If
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Response.Redirect("ReservationInvoiceSearch.aspx")
        ViewState.Add("RepCalledFrom", 0)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('../PriceListModule/RptPrintPage.aspx','RptExpReservationReport','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
