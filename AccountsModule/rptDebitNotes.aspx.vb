Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class rptDebitnotes
    Inherits System.Web.UI.Page
    Dim objDate As New clsDateTime
    Dim objDateTime As New clsDateTime

    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fdate As String, ldate As String, div_code As String, order As String, repfilter As String
    Dim flag As Integer
    Dim strReportTitle As String = ""
    Dim strSelectionFormula As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        If Request.QueryString("BackPageName") <> "" Then
            ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
        End If

        If Request.QueryString("Pageame") <> "" Then
            ViewState.Add("Pageame", Request.QueryString("Pageame"))
        End If

        Dim strReportName As String = ""
        If CType(ViewState("BackPageName"), String) = "" Then
            'Response.Redirect("~/Login.aspx", False)
            'Exit Sub
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Else
            If CType(ViewState("Pageame"), String) = "" Then
                'Response.Redirect(CType(Session("BackPageName"), String), False)
                'Exit Sub
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
            Else
                'txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                'txtToDate.Text = txtFromDate.Text

                If Request.QueryString("fdate") <> "" Then
                    fdate = Request.QueryString("fdate")
                End If

                If Request.QueryString("ldate") <> "" Then
                    ldate = Request.QueryString("ldate")
                End If
                If Request.QueryString("order") <> "" Then
                    order = Request.QueryString("order")
                End If
                If Request.QueryString("flag") <> "" Then
                    flag = Request.QueryString("flag")
                End If
                If Request.QueryString("div_code") <> "" Then
                    div_code = Request.QueryString("div_code")
                End If
                'If Request.QueryString("repfilter") <> "" Then
                '    repfilter = Request.QueryString("repfilter")
                'End If

                If Request.QueryString("DocNo") <> "" Then
                    If strSelectionFormula = "" Then
                        strReportTitle = "Doc No: " & Request.QueryString("DocNo")
                        strSelectionFormula = " upper(tran_id) = '" & Request.QueryString("DocNo") & "'"
                    Else
                        strReportTitle = strReportTitle & " ;Doc No: " & Request.QueryString("DocNo")
                        strSelectionFormula = strSelectionFormula & " AND upper(tran_id) = '" & Request.QueryString("DocNo") & "'"
                    End If
                End If
                If Request.QueryString("Customer") <> "" Then
                    If strSelectionFormula = "" Then
                        strReportTitle = "Customer: " & Request.QueryString("Customer")

                        strSelectionFormula = " upper(supcode) = '" & Request.QueryString("Customer") & "'"
                    Else
                        strReportTitle = strReportTitle & ";Customer: " & Request.QueryString("Customer")
                        strSelectionFormula = strSelectionFormula & " AND upper(supcode) = '" & Request.QueryString("Customer") & "'"
                    End If
                End If
                If Request.QueryString("FromAmount") <> "" And Request.QueryString("ToAmount") <> "" Then
                    If strSelectionFormula = "" Then
                        strReportTitle = "From Amount: " & Request.QueryString("FromAmount") & "To Amount:" & Request.QueryString("ToAmount")
                        strSelectionFormula = " (total between " & Val(Request.QueryString("FromAmount")) & " and " & Val(Request.QueryString("ToAmount")) & ") "
                    Else
                        strReportTitle = strReportTitle & ";From Amount: " & Request.QueryString("FromAmount") & "To Amount:" & Request.QueryString("ToAmount")
                        strSelectionFormula = strSelectionFormula & " AND (total between " & Val(Request.QueryString("FromAmount")) & " and " & Val(Request.QueryString("ToAmount")) & ") "
                    End If
                End If

                'If Request.QueryString("Type") <> "" Then
                '    If strSelectionFormula = "" Then
                '        strReportTitle = "Type: " & Request.QueryString("Type")
                '        strSelectionFormula = " acc_type = '" & Request.QueryString("Type") & "'"
                '    Else
                '        strReportTitle = strReportTitle & ";Type: " & Request.QueryString("Type")
                '        strSelectionFormula = strSelectionFormula & " AND acc_type = '" & Request.QueryString("Type") & "'"
                '    End If
                'End If

                If Request.QueryString("Status") <> "" Then
                    If strSelectionFormula = "" Then
                        strReportTitle = "Status: " & Request.QueryString("Status")
                        strSelectionFormula = " isnull(post_state,'')  = '" & Request.QueryString("Status") & "'"
                    Else
                        strReportTitle = ";Status: " & Request.QueryString("Status")
                        strSelectionFormula = strSelectionFormula & " AND isnull(post_state,'')  = '" & Request.QueryString("Status") & "'"
                    End If
                End If


                Select Case CType(ViewState("Pageame"), String)
                    Case "DebitNoteBrief"
                        strReportName = CType(Server.MapPath("~\Report\debit_summary.rpt"), String)
                        If flag = 2 Then
                            rptreportname = "Report - Debit Notes Summary"
                        Else
                            rptreportname = "Report - Credit Notes Summary"
                        End If
                        Exit Select
                    Case "DebitNoteDetail"
                        strReportName = CType(Server.MapPath("~\Report\debit_detail.rpt"), String)
                        If flag = 2 Then
                            rptreportname = "Report - Debit Notes Detail"
                        Else
                            rptreportname = "Report - Credit Notes Detail"
                        End If
                        Exit Select
                End Select
                If strReportName = "" Then
                    'Response.Redirect(CType(Session("BackPageName"), String), False)
                    'Exit Sub
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                Else
                    BindReport(strReportName, strSelectionFormula, strReportTitle)
                End If

            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)
        Try


            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue

            'If Session("Rep") Is Nothing Then
            Dim ConnInfo As New ConnectionInfo
            
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

            pname = pnames.Item("order")
            paramvalue.Value = Trim(order)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("flag")
            paramvalue.Value = Val(flag)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("div_code")
            paramvalue.Value = Trim(div_code)
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

            pname = pnames.Item("repfilter")
            paramvalue.Value = Trim(repfilter)
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
        'If CType(Session("BackPageName"), String) = "" Then
        '    Response.Redirect("MainPage.aspx", False)
        '    Exit Sub
        'Else
        '    Session("ColReportParams") = Nothing
        '    Response.Redirect(CType(Session("BackPageName"), String), False)
        'End If

        ViewState.Add("RepCalledFrom", 0)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    repDeocument.Close()
        '    repDeocument.Dispose()
        '    ' Session("ColReportParams") = Nothing
        'End If
        If Page.IsPostBack = True Then
            If ViewState("RepCalledFrom") <> 1 Then
                repDeocument.Close()
                repDeocument.Dispose()
            End If
        End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", repDeocument)
    '    Dim strpop As String = ""
    '    'strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    strpop = "<script language=""javascript"">var win=window.open('../PriceListModule/RptPrintPage.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    'ScriptStr = "<script language=""javascript"">var win=window.open('PrintDoc.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, False)
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", repDeocument)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PopUpDebitNote','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class

