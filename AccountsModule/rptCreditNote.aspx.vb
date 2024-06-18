Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Partial Class AccountsModule_rptCreditNote
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim strReportTitle As String = ""
            Dim objReport As New ReportDocument
            objReport = New ReportDocument()
            ViewState.Add("CreditNoteNo", Request.QueryString("CreditNo"))

            CRVReport.Visible = True
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(n'" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try

        'Try
        '    BindReport()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        'End Try

    End Sub
    Private Sub BindReport()
        Try
            Dim strReportTitle As String = ""

            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
            ' rptreportname = "Report - Currencies"

            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue
            Dim ConnInfo As New ConnectionInfo

            With ConnInfo
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With

            rep.Load(Server.MapPath("~\Report\rptCreditNote.rpt"))
            Me.CRVReport.ReportSource = rep
            Dim RepTbls As Tables = rep.Database.Tables
            Me.CRVReport.ReportSource = rep

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            rep.SummaryInfo.ReportTitle = strReportTitle
            pnames = rep.DataDefinition.ParameterFields

            pname = pnames.Item("@requestid")
            paramvalue.Value = ViewState("CreditNoteNo")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            'For Each RepTbl As Table In RepTbls
            '    Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            '    RepTblLogonInfo.ConnectionInfo = ConnInfo
            '    RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            'Next


            'Dim strSelectionFormula As String = ""
            'If CType(ViewState("RefundRequestId"), String) <> "" Then
            '    strSelectionFormula = "{refund_request_header.refundreqid} = '" & ViewState("RefundRequestId") & "'"
            'End If

            ' rep.SummaryInfo.ReportTitle = strReportTitle

            'pnames = rep.DataDefinition.ParameterFields

            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("currency")
            'paramvalue.Value = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select option_selected from reservation_parameters where param_id=457")
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            '****************added for report footer- common for above  reports****************************
            pname = pnames.Item("rptComId")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine1")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1051)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine2")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1052)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine3")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            '************************************************************************************************






            Me.CRVReport.ReportSource = rep
            'If strSelectionFormula <> "" Then
            '    CRVReport.SelectionFormula = strSelectionFormula
            'End If

            Me.CRVReport.DataBind()
            Session.Add("ReportSource", rep)
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
        End If
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Session.Add("ReportSource", rep)
            ViewState.Add("RepCalledFrom", 1)
            Dim strpop As String = ""
            strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PrintRefundRequest','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ViewState.Add("RepCalledFrom", 0)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
End Class
