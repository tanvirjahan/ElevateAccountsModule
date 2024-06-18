Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class AccountsModule_rptPuchaseInvoice
    Inherits System.Web.UI.Page
    Dim objDate As New clsDateTime
    Dim objDateTime As New clsDateTime

    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fdate As String, ldate As String, div_code As String, order As String, repfilter As String
    Dim strReportTitle As String = ""
    Dim strSelectionFormula As String = ""
    'Dim strReportName As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'If Page.IsPostBack = False Then

        If Request.QueryString("BackPageName") <> "" Then
            ViewState("BackPageName") = Request.QueryString("BackPageName")
        End If
        If Request.QueryString("Pageame") <> "" Then
            ViewState("Pageame") = Request.QueryString("Pageame")
        End If
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

        If CType(ViewState("BackPageName"), String) = "" Then
            'Response.Redirect("~/Login.aspx", False)
            'Exit Sub
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Else
            If CType(ViewState("Pageame"), String) = "" Then
                'Response.Redirect(CType(ViewState("BackPageName"), String), False)
                'Exit Sub
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
            Else
                If Request.QueryString("fdate") <> "" Then
                    fdate = Request.QueryString("fdate")
                End If

                If Request.QueryString("ldate") <> "" Then
                    ldate = Request.QueryString("ldate")
                End If

                If Request.QueryString("reporttitle") <> "" Then
                    repfilter = Request.QueryString("reporttitle")
                End If

                If Request.QueryString("InvoiceNo") <> "" Then
                    If strSelectionFormula <> "" Then
                        strReportTitle = strReportTitle & " ;Purchase Invoice No: " & Request.QueryString("InvoiceNo")
                        strSelectionFormula = strSelectionFormula & " and {providerinv_header.tran_id} = '" & Request.QueryString("InvoiceNo") & "'"
                    Else
                        strReportTitle = "Purchase Invoice No: " & Request.QueryString("InvoiceNo")
                        strSelectionFormula = "{providerinv_header.tran_id} = '" & Request.QueryString("InvoiceNo") & "'"
                    End If
                End If
                If Request.QueryString("Type") <> "[Select]" Then
                    If strSelectionFormula <> "" Then
                        strReportTitle = strReportTitle & " ;Type: " & Request.QueryString("Type")
                        strSelectionFormula = strSelectionFormula & " and {providerinv_header.acc_type} = '" & Request.QueryString("Type") & "'"
                    Else
                        strReportTitle = "Type: " & Request.QueryString("Type")
                        strSelectionFormula = "{providerinv_header.acc_type} ='" & Request.QueryString("Type") & "'"
                    End If
                End If
                If Request.QueryString("SupplierCode") <> "[Select]" Then
                    If strSelectionFormula <> "" Then
                        strReportTitle = strReportTitle & " ;Supplier Code: " & Request.QueryString("SupplierCode")
                        strSelectionFormula = strSelectionFormula & " and {providerinv_header.acc_code} = '" & Request.QueryString("SupplierCode") & "'"
                    Else
                        strReportTitle = "Supplier Code: " & Request.QueryString("SupplierCode")
                        strSelectionFormula = "{providerinv_header.acc_code} = '" & Request.QueryString("SupplierCode") & "'"
                    End If
                End If

                If Request.QueryString("PostToCode") <> "[Select]" Then
                    If strSelectionFormula <> "" Then
                        strReportTitle = strReportTitle & " ;Post Account Code: " & Request.QueryString("PostToCode")
                        strSelectionFormula = strSelectionFormula & " and {providerinv_header.postaccount} = '" & Request.QueryString("PostToCode") & "'"
                    Else
                        strReportTitle = "Post Account Code: " & Request.QueryString("PostToCode")
                        strSelectionFormula = "{providerinv_header.postaccount} = '" & Request.QueryString("PostToCode") & "'"
                    End If
                End If

                Select Case CType(ViewState("Pageame"), String)
                    Case "PurchaseInvoiceBrief"
                        ViewState.Add("strReportName", CType(Server.MapPath("~\Report\rptPurchaseInvoiceBrief.rpt"), String))
                        rptreportname = "Report - Purchase Invoice Brief"
                        Exit Select
                    Case "PurchaseInvoiceDetail"
                        ViewState.Add("strReportName", CType(Server.MapPath("~\Report\rptPurchaseInvoiceDetails.rpt"), String))
                        rptreportname = "Report - Purchase Invoice Detail"
                        Exit Select
                End Select
            End If
        End If
        'End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ViewState.Add("RepCalledFrom", 0)
        btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")

        BindReport(ViewState("strReportName"), CType(strSelectionFormula, String), CType(strReportTitle, String))

    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)
        Try


            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue

            'If Session("Rep") Is Nothing Then
            Dim ConnInfo As New ConnectionInfo
            'With ConnInfo
            '    .ServerName = ConfigurationManager.AppSettings("dbServerName")
            '    .DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
            '    .UserID = ConfigurationManager.AppSettings("dbUserName")
            '    .Password = ConfigurationManager.AppSettings("dbPassword")
            ''End With
            With ConnInfo
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With


            repDeocument.Load(ReportName)

            Me.CRVReport.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            repDeocument.SummaryInfo.ReportTitle = strReportTitle
            pnames = repDeocument.DataDefinition.ParameterFields


            pname = pnames.Item("fdate")
            paramvalue.Value = Trim(fdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ldate")
            paramvalue.Value = Trim(ldate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("order")
            'paramvalue.Value = Trim(order)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)


            'pname = pnames.Item("div_code")
            'paramvalue.Value = Trim(div_code)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            pname = pnames.Item("repfilter")
            paramvalue.Value = Trim(repfilter)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

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

            pname = pnames.Item("currency")
            paramvalue.Value = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            If Not Session("ColReportParams") Is Nothing Then
                Dim p As Integer
                Dim colreport As New Collection
                colreport = Session("ColReportParams")
                Dim creport As New clsReportParam
                For p = 1 To colreport.Count
                    creport = colreport.Item(p)
                    pname = pnames.Item(creport.rep_parametername)
                    paramvalue.Value = creport.rep_parametervalue
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                Next

            End If
            Me.CRVReport.ReportSource = repDeocument
            If strSelectionFormula <> "" Then
                CRVReport.SelectionFormula = strSelectionFormula
            End If
            Session.Add("ReportSource", repDeocument)
            Me.CRVReport.DataBind()
            CRVReport.HasCrystalLogo = False
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ViewState.Add("RepCalledFrom", 0)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        'If CType(Session("BackPageName"), String) = "" Then
        '    Response.Redirect("MainPage.aspx", False)
        '    Exit Sub
        'Else
        '    Session("ColReportParams") = Nothing
        '    Response.Redirect(CType(Session("BackPageName"), String), False)
        'End If
    End Sub
   
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    repDeocument.Close()
        '    repDeocument.Dispose()
        'End If
        '  If Page.IsPostBack = True Then
        If ViewState("RepCalledFrom") <> 1 Then
            repDeocument.Close()
            repDeocument.Dispose()
        End If
        ' End If
    End Sub

    'Protected Sub btnprint_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnprint.ServerClick
    '    Session.Add("ReportSource", repDeocument)
    '    ViewState.Add("RepCalledFrom", 1)
    '    Dim strpop As String = ""
    '    strpop = "window.open('../RptPrintPage.aspx','PopUpPurchaseInvoice','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", repDeocument)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PopUpPurchaseInvoice','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
