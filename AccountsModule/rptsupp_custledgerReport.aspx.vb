Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptsupp_custledgerReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim divcode As String
    Dim fromdate As String, todate As String
    Dim accttype, fromctry, ststement, fromacct, toacct, ageing, toctry, fromcity, tocity, fromcode, tocode, fromsptype, fromtosptype, fromcat, tocat, fromglcode, toglcode, currtype, ledgertype, pdcyesno, custgroup_sp_type As String
    Dim frommarket As String, tomarket As String

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

            If Request.QueryString("actype") <> "" Then
                accttype = Trim(Request.QueryString("actype"))
            End If
            If Request.QueryString("divid") <> "" Then
                divcode = Trim(Request.QueryString("divid"))
            End If


            If Request.QueryString("custgroup_sp_type") <> "" Then
                custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
            End If



            If accttype = "G" Then
                If Request.QueryString("fromcode") <> "" Then
                    fromcode = Trim(Request.QueryString("fromcode"))
                End If
                If Request.QueryString("tocode") <> "" Then
                    tocode = Trim(Request.QueryString("tocode"))
                End If

                If Request.QueryString("reptype") <> "" Then
                    ledgertype = Trim(Request.QueryString("reptype"))
                End If

                If Request.QueryString("pdcyesno") <> "" Then
                    pdcyesno = Trim(Request.QueryString("pdcyesno"))
                End If


            Else

                If Request.QueryString("fromctry") <> "" Then
                    fromctry = Trim(Request.QueryString("fromctry"))
                End If
                If Request.QueryString("toctry") <> "" Then
                    toctry = Trim(Request.QueryString("toctry"))
                End If

                If Request.QueryString("fromcity") <> "" Then
                    fromcity = Trim(Request.QueryString("fromcity"))
                End If
                If Request.QueryString("tocity") <> "" Then
                    tocity = Trim(Request.QueryString("tocity"))
                End If

                If Request.QueryString("fromcode") <> "" Then
                    fromcode = Trim(Request.QueryString("fromcode"))
                End If
                If Request.QueryString("tocode") <> "" Then
                    tocode = Trim(Request.QueryString("tocode"))
                End If

                If Request.QueryString("fromcat") <> "" Then
                    fromcat = Trim(Request.QueryString("fromcat"))
                End If

                If Request.QueryString("tocat") <> "" Then
                    tocat = Trim(Request.QueryString("tocat"))
                End If
                'If accttype = "S" Then
                '    If Request.QueryString("fromglcode") <> "" Then
                '        fromcode = Trim(Request.QueryString("fromglcode"))
                '    End If
                '    If Request.QueryString("toglcode") <> "" Then
                '        tocode = Trim(Request.QueryString("toglcode"))
                '    End If
                'Else

                If Request.QueryString("fromglcode") <> "" Then
                    fromglcode = Trim(Request.QueryString("fromglcode"))
                End If
                If Request.QueryString("toglcode") <> "" Then
                    toglcode = Trim(Request.QueryString("toglcode"))
                End If
                'End If
                If Request.QueryString("currtype") <> "" Then
                    currtype = Trim(Request.QueryString("currtype"))
                End If
                If Request.QueryString("pdcyesno") <> "" Then
                    pdcyesno = Trim(Request.QueryString("pdcyesno"))
                End If

                If Request.QueryString("ledgertype") <> "" Then
                    ledgertype = Trim(Request.QueryString("ledgertype"))
                End If
                If Request.QueryString("frommarketcode") <> "" Then
                    frommarket = Request.QueryString("frommarketcode")
                End If
                If Request.QueryString("tomarketcode") <> "" Then
                    tomarket = Request.QueryString("tomarketcode")
                End If
                ststement = 0
                If Request.QueryString("ststement") <> "" Then
                    ststement = Request.QueryString("ststement")
                End If

                If ststement = 1 Then


                    If Request.QueryString("ageing") <> "" Then
                        ageing = Request.QueryString("ageing")
                    End If
                End If


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
        ' rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        'rptreportname = "Arrival Report"

        If divcode <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

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
        Dim led As String


        led = IIf(ledgertype = 0, "Summary", "Detailed")


        Select Case accttype
            Case "S"
                rptreportname = led + " Report- Supplier Ledger"
                If ledgertype = "0" Then
                    rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger.rpt"))
                Else
                    rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger_det.rpt"))
                End If

            Case "A"
                rptreportname = led + " Report- Supplier Agent Ledger"
                If ledgertype = "0" Then
                    rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger.rpt"))
                Else
                    rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger_det.rpt"))
                End If

            Case "C"
                rptreportname = led + "Report- Customer Ledger"
                If ledgertype = "0" Then
                    If ststement = 1 Then
                        rep.Load(Server.MapPath("..\Report\rptsupcust_stkledgerStatement.rpt"))
                        rptreportname = "Customer Ledger Statement"
                    Else
                        rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger.rpt"))
                    End If
                Else
                    If ststement = 1 Then
                        rep.Load(Server.MapPath("..\Report\rptsupcust_stkledgerStatement.rpt"))
                        rptreportname = "Customer Ledger Statement"
                    Else
                        rep.Load(Server.MapPath("..\Report\rptsupcust_stkledger_det.rpt"))
                    End If

                End If

            Case "G"
                led = IIf(ledgertype = 1, "Summary", "Detailed")
                rptreportname = led + "Report- General Ledger"

                rep.Load(Server.MapPath("..\Report\rptGLledger.rpt"))

        End Select


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

        If ststement = 1 Then
            pname = pnames.Item("@summdet")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("@web")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("@agingtype")
            paramvalue.Value = ageing
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If
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
        pname = pnames.Item("addrLine1")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("addrLine2")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("addrLine3")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("addrLine4")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("addrLine5")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If accttype = "G" Then

            pname = pnames.Item("@frmdate")
            paramvalue.Value = Trim(fromdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = Trim(todate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")

            pname = pnames.Item("@frmac")
            paramvalue.Value = Trim(fromcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toac")
            paramvalue.Value = Trim(tocode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            reportfilter = reportfilter + IIf((fromcode <> "" And tocode <> ""), "Account Code from " + fromcode + " To " + tocode, "")



            pname = pnames.Item("@actype")
            paramvalue.Value = Trim(accttype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("@div_code")
            paramvalue.Value = Trim(divcode) 'Trim(CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=511"), String))
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@reptype")
            paramvalue.Value = Trim(ledgertype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@pdctype")
            paramvalue.Value = Trim(pdcyesno)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        Else


            pname = pnames.Item("@fromdate")
            paramvalue.Value = Trim(fromdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = Trim(todate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")


       

            Select Case accttype
                Case "S"
                    reportfilter = reportfilter + IIf((fromcode <> "" And tocode <> ""), "Supplier Code From " + fromcode + " To " + tocode, "")

                Case "A"
                    reportfilter = reportfilter + IIf((fromcode <> "" And tocode <> ""), "Supplier Agent Code From " + fromcode + " To " + tocode, "")

                Case "C"
                    reportfilter = reportfilter + IIf((fromcode <> "" And tocode <> ""), "Customer Code From " + fromcode + " To " + tocode, "")

            End Select




            pname = pnames.Item("@type")
            paramvalue.Value = Trim(accttype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)




            pname = pnames.Item("@custgroup_sp_type")
            paramvalue.Value = Trim(custgroup_sp_type)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)





            ''If customer statement then will pass the Market in the city 
            If Trim(accttype) = "C" Then
                pname = pnames.Item("@fromcity")
                paramvalue.Value = Trim(frommarket)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@tocity")
                paramvalue.Value = Trim(tomarket)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                If rptreportname = "Customer Ledger Statement" Then
                    pname = pnames.Item("cmb")
                    paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1050"), String)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)
                End If

                reportfilter = reportfilter + IIf((Trim(frommarket) <> "" And Trim(tomarket) <> ""), " : Market Code from " + Trim(frommarket) + " To " + Trim(tomarket), "")

            Else
                pname = pnames.Item("@fromcity")
                paramvalue.Value = Trim(fromcity)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@tocity")
                paramvalue.Value = Trim(tocity)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            reportfilter = reportfilter + IIf((fromcity <> "" And tocity <> ""), " Supplier city  From " + fromcity + " To " + tocity, "")


            Select Case accttype
                Case "S"
                    reportfilter = reportfilter + IIf((fromcity <> "" And tocity <> ""), " Supplier city  From " + fromcity + " To " + tocity, "")

                Case "A"
                    reportfilter = reportfilter + IIf((fromcity <> "" And tocity <> ""), " Supplier city  From " + fromcity + " To " + tocity, "")

                Case "C"
                    reportfilter = reportfilter + IIf((fromcity <> "" And tocity <> ""), " Market  From " + fromcity + " To " + tocity, "")

            End Select



            pname = pnames.Item("@fromctry")
            paramvalue.Value = Trim(fromctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toctry")
            paramvalue.Value = Trim(toctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            reportfilter = reportfilter + IIf((fromctry <> "" And toctry <> ""), " Supplier country  From " + fromctry + " To " + toctry, "")


            pname = pnames.Item("@fromcat")
            paramvalue.Value = Trim(fromcat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocat")
            paramvalue.Value = Trim(tocat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            reportfilter = reportfilter + IIf((fromcat <> "" And tocat <> ""), " Category Code From " + fromcat + " To " + tocat, "")

            'If accttype = "S" Then

            'pname = pnames.Item("@fromcontrol")
            'paramvalue.Value = Trim(fromcode)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@tocontrol")
            'paramvalue.Value = Trim(tocode)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@fromacct")
            'paramvalue.Value = Trim(fromglcode)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@toacct")
            'paramvalue.Value = Trim(toglcode)
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)


            pname = pnames.Item("@fromcontrol")
            paramvalue.Value = Trim(fromglcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocontrol")
            paramvalue.Value = Trim(toglcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromacct")
            paramvalue.Value = Trim(fromcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toacct")
            paramvalue.Value = Trim(tocode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            ' End If

            reportfilter = reportfilter + IIf((fromglcode <> "" And toglcode <> ""), " Control Account Code From " + fromglcode + " To " + toglcode, "")


            pname = pnames.Item("@currflg")
            paramvalue.Value = Trim(currtype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@ledgertype")
            paramvalue.Value = Trim(ledgertype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@pdcyesno")
            paramvalue.Value = IIf(pdcyesno = "Yes", 1, 0)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@divcode")
            paramvalue.Value = Trim(divcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        pname = pnames.Item("reportfilter")
        paramvalue.Value = reportfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Session.Add("ReportSource", rep)
        '  Me.CRVReport.ReportSource = rep
        Me.CRVReport.DataBind()

        'Response.Buffer = False
        'Response.ClearContent()
        'Response.ClearHeaders()
        'rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'If Request.QueryString("type") = "S" Then
        '    Response.Redirect("RptSupplierLedger.aspx?fromdate=" & fromdate & "&todate=" & todate _
        '                      & "&fromctry=" & fromctry & "&toctry=" & toctry & "&fromcity=" & fromcity & "&tocity=" & tocity & "&acctype=" & accttype & "&fromcode=" & fromcode & "&tocode=" & tocode _
        '                      & "&fromcat=" & fromcat & "&tocat=" & tocat & "&fromglcode=" & fromglcode & "&toglcode=" & toglcode & "&frommarketcode=" & frommarket & "&tomarketcode=" & tomarket _
        '                      & "&currtype=" & currtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)
        'End If
        'If Request.QueryString("type") = "C" Then
        '    Response.Redirect("RptCustomerLedger.aspx?fromdate=" & fromdate & "&todate=" & todate _
        '                     & "&fromctry=" & fromctry & "&toctry=" & toctry & "&fromcity=" & fromcity & "&tocity=" & tocity & "&acctype=" & accttype & "&fromcode=" & fromcode & "&tocode=" & tocode _
        '                     & "&fromcat=" & fromcat & "&tocat=" & tocat & "&fromglcode=" & fromglcode & "&toglcode=" & toglcode & "&frommarketcode=" & frommarket & "&tomarketcode=" & tomarket _
        '                     & "&currtype=" & currtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)

        'End If

        'If accttype = "G" Then
        '    Response.Redirect("RptGeneralLedger.aspx?fromdate=" & fromdate & "&todate=" & todate & "&actype=G &fromac=" & fromcode & "&toac=" & tocode & "&reptype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)

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
