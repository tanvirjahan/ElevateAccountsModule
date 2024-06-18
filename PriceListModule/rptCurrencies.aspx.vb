Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource


Partial Class rptCurrencies
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.IsPostBack = False Then
            Try
                Dim strReportTitle As String = ""
                Dim objReport As New ReportDocument
                objReport = New ReportDocument()
                ViewState.Add("RepCurrencyCode", Request.QueryString("CurrencyCode"))
                ViewState.Add("RepCurrencyName", Request.QueryString("CurrencyName"))

                Dim str As String = ""
                If CType(ViewState("RepCurrencyCode"), String) <> "" Then
                    strReportTitle = "Currency Code : " & CType(ViewState("RepCurrencyCode"), String)
                    str = "{currmast.currcode} LIKE '" & ViewState("RepCurrencyCode") & "*'"
                End If
                If CType(ViewState("RepCurrencyName"), String) <> "" Then
                    If str <> "" Then
                        strReportTitle = strReportTitle & " ; Currency Name : " & CType(ViewState("RepCurrencyName"), String)
                        str = str & " and {currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
                    Else
                        strReportTitle = "Currency Name : " & CType(ViewState("RepCurrencyName"), String)
                        str = "{currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
                    End If
                End If
                objReport.Load(Server.MapPath("~\Report\rptCurrencies.rpt"))
                objReport.SetDatabaseLogon(Session("dbUserName"), Session("dbPassword"), Session("dbServerName"), Session("dbDatabaseName"))
               
        

                objReport.SummaryInfo.ReportTitle = strReportTitle

                CRVCurrency.ReportSource = objReport
                If str <> "" Then
                    CRVCurrency.SelectionFormula = str

                End If

                CRVCurrency.Visible = True
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            End Try
        End If


        Try
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' If Page.IsPostBack = False Then
        'Try
        '    Dim strReportTitle As String = ""
        '    Dim objReport As New ReportDocument
        '    objReport = New ReportDocument()

        '    Dim str As String = ""
        '    If CType(ViewState("RepCurrencyCode"), String) <> "" Then
        '        strReportTitle = "Currency Code : " & CType(ViewState("RepCurrencyCode"), String)
        '        str = "{currmast.currcode} LIKE '" & ViewState("RepCurrencyCode") & "*'"
        '    End If
        '    If CType(ViewState("RepCurrencyName"), String) <> "" Then
        '        If str <> "" Then
        '            strReportTitle = strReportTitle & " ; Currency Name : " & CType(ViewState("RepCurrencyName"), String)
        '            str = str & " and {currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
        '        Else
        '            strReportTitle = "Currency Name : " & CType(ViewState("RepCurrencyName"), String)
        '            str = "{currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
        '        End If
        '    End If

        '    objReport.Load(Server.MapPath("Report\rptCurrencies.rpt"))
        '    objReport.SetDatabaseLogon(ConfigurationManager.AppSettings("dbUserName"), ConfigurationManager.AppSettings("dbPassword"), ConfigurationManager.AppSettings("dbServerName"), ConfigurationManager.AppSettings("dbDatabaseName"))
        '    objReport.SummaryInfo.ReportTitle = strReportTitle

        '    CRVCurrency.ReportSource = objReport
        '    If str <> "" Then
        '        CRVCurrency.SelectionFormula = str

        '    End If

        '    CRVCurrency.Visible = True
        'Catch ex As Exception

        'End Try
        ''End If
        ViewState.Add("RepCalledFrom", 0)
        btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")

        Try
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub
    Private Sub BindReport()
        Dim strReportTitle As String = ""
        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Report - Currencies"

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo

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

        rep.Load(Server.MapPath("~\Report\rptCurrencies.rpt"))
        Me.CRVCurrency.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next
        'Session("Rep") = rep
        'End If
        Dim strSelectionFormula As String = ""
        If CType(ViewState("RepCurrencyCode"), String) <> "" Then
            strReportTitle = "Currency Code : " & CType(ViewState("RepCurrencyCode"), String)
            strSelectionFormula = "{currmast.currcode} LIKE '" & ViewState("RepCurrencyCode") & "*'"
        End If
        If CType(ViewState("RepCurrencyName"), String) <> "" Then
            If strSelectionFormula <> "" Then
                strReportTitle = strReportTitle & " ; Currency Name : " & CType(ViewState("RepCurrencyName"), String)
                strSelectionFormula = strSelectionFormula & " and {currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
            Else
                strReportTitle = "Currency Name : " & CType(ViewState("RepCurrencyName"), String)
                strSelectionFormula = "{currmast.currname} LIKE '" & ViewState("RepCurrencyName") & "*'"
            End If
        End If
        rep.SummaryInfo.ReportTitle = strReportTitle

        pnames = rep.DataDefinition.ParameterFields

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

        Me.CRVCurrency.ReportSource = rep
        If strSelectionFormula <> "" Then
            CRVCurrency.SelectionFormula = strSelectionFormula
        End If
        Session.Add("ReportSource", rep)
        Me.CRVCurrency.DataBind()

    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Response.Redirect("CurrenciesSearch.aspx", False)
        ViewState.Add("RepCalledFrom", 0)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
        End If
        'End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    'Session.Add("ReportSource", rep)
    '    'Dim strpop As String = ""
    '    'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub

    Protected Sub Button1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.ServerClick
        '        BindReport()
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
