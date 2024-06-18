Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class chequeprint
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String, todate As String
    Dim accttype, level, fromctry, toctry, tranid, C_Btype, accfrm, bank, Type, poststate, voucher, strrpttype1, strclosing As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try



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
        Dim ctry As String = ""

        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

        ctry = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from  reservation_parameters where param_id=459"), String)

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

        rep.Load(Server.MapPath("..\Report\cheque.rpt"))
        'If ctry = "UAE" Then
        '    rep.Load(Server.MapPath("..\Report\cheque.rpt"))
        'Else
        '    rep.Load(Server.MapPath("..\Report\chequeoman.rpt"))
        'End If

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



        pname = pnames.Item("tranid")
        paramvalue.Value = Request.QueryString("tranid")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("trantype")
        paramvalue.Value = Request.QueryString("type")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("divid")
        paramvalue.Value = Request.QueryString("divid")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        'Me.CRVConfirmation.ReportSource = rep
        'Me.CRVReservationReqOther.DataBind()
        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")


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
