Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptRV_PVreport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String, todate As String, divcode As String
    Dim accttype, level, fromctry, toctry, tranid, C_Btype, accfrm, bank, Type, poststate, voucher, strrpttype1, strclosing As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            ViewState.Add("Pageame", Request.QueryString("Pageame"))
            ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

            If Request.QueryString("frmdate") <> "" Then
                fromdate = Trim(Request.QueryString("frmdate"))
            End If
            If Request.QueryString("todate") <> "" Then
                todate = Trim(Request.QueryString("todate"))
            End If

            If Request.QueryString("divid") <> "" Then
                divcode = Trim(Request.QueryString("divid"))
            End If

            If Request.QueryString("trantype") <> "" Then
                accttype = Trim(Request.QueryString("trantype"))
            End If
            If Request.QueryString("tranid") <> "" Then
                tranid = Trim(Request.QueryString("tranid"))
            End If

            If Request.QueryString("C_Btype") <> "" Then
                C_Btype = Trim(Request.QueryString("C_Btype"))
            End If

            If Request.QueryString("accfrm") <> "" Then
                accfrm = Trim(Request.QueryString("accfrm"))
            End If

            If Request.QueryString("bank") <> "" Then
                bank = Trim(Request.QueryString("bank"))
            End If

            If Request.QueryString("type") <> "" Then
                Type = Trim(Request.QueryString("type"))
            End If

            If Request.QueryString("level") <> "" Then
                level = Trim(Request.QueryString("level"))
            End If
            If Request.QueryString("poststate") <> "" Then
                poststate = Trim(Request.QueryString("poststate"))
            End If

            If Request.QueryString("voucher") <> "" Then
                voucher = Trim(Request.QueryString("voucher"))
            End If

            If Request.QueryString("strrpttype1") <> "" Then
                strrpttype1 = Trim(Request.QueryString("strrpttype1"))
            End If

            If Request.QueryString("closing") <> "" Then
                strclosing = Trim(Request.QueryString("closing"))
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


        If Type = "Profit" Then
            If strrpttype1 = 0 Then
                rptreportname = " Report- Income Statement"
                ' rep.Load(Server.MapPath("..\Report\rptProfitLoss.rpt"))
                'rep.Load(Server.MapPath("..\Report\rptProfitLossnew.rpt"))
                If Request.QueryString("rptype") = 0 Then
                    rep.Load(Server.MapPath("..\Report\rptProfitLossnew5.rpt"))
                Else
                    rep.Load(Server.MapPath("..\Report\pf.rpt"))
                End If
            Else
                rptreportname = " Report- Income Statement Month wise"
                rep.Load(Server.MapPath("..\Report\rptProfitLossmonthly.rpt"))

            End If
        Else
            Select Case accttype
                Case "CPV"
                    rptreportname = " Report- Cash Payments"
                    If Type = "0" Then
                        rep.Load(Server.MapPath("..\Report\acctPV_brief.rpt"))
                    Else
                        If voucher = "All" Then
                            rep.Load(Server.MapPath("..\Report\rptPaymentVoucherdetail.rpt"))
                        Else
                            rep.Load(Server.MapPath("..\Report\acctPV_detail.rpt"))
                        End If
                    End If
                Case "BPV"
                    rptreportname = " Report- Bank Payments"
                    If Type = "0" Then
                        rep.Load(Server.MapPath("..\Report\acctPV_brief.rpt"))
                    Else
                        If voucher = "All" Then
                            rep.Load(Server.MapPath("..\Report\rptPaymentVoucherdetail.rpt"))
                        Else
                            rep.Load(Server.MapPath("..\Report\acctPV_detail.rpt"))
                        End If
                    End If
                Case "DEP"
                    rptreportname = " Report- Deposit"
                    If Type = "0" Then
                        rep.Load(Server.MapPath("..\Report\acctPV_brief.rpt"))
                    Else
                        If voucher = "All" Then
                            rep.Load(Server.MapPath("..\Report\rptPaymentVoucherdetail.rpt"))
                        Else
                            rep.Load(Server.MapPath("..\Report\acctPV_detail.rpt"))
                        End If
                    End If
                Case "RV"
                    rptreportname = " Report- Reciepts"
                    If Type = "0" Then
                        rep.Load(Server.MapPath("..\Report\acctRV_brief.rpt"))
                    Else
                        If voucher = "All" Then
                            rep.Load(Server.MapPath("..\Report\rptReceiptVoucherDetail.rpt"))
                        Else
                            rep.Load(Server.MapPath("..\Report\acctRV_detail.rpt"))
                        End If
                    End If



            End Select
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







        If Request.QueryString("Type") = "Profit" Then
            If Request.QueryString("rptype") = 1 Then

                pname = pnames.Item("@date")
                paramvalue.Value = Trim(Format(CDate(fromdate), "yyyy/MM/dd"))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@range")
                paramvalue.Value = 3
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("@division")
                paramvalue.Value = divcode
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                'pname = pnames.Item("@division")
                'paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select   option_selected  from reservation_parameters where param_id=511"), String))
                'param = pname.CurrentValues
                'param.Add(paramvalue)
                'pname.ApplyCurrentValues(param)




                pname = pnames.Item("RANGE")
                paramvalue.Value = 3
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("aclevel")
                paramvalue.Value = 5
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            Else

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
                reportfilter = "From: " + Format(CType(fromdate, Date), "dd/MM/yyyy") + "  To: " + Format(CType(todate, Date), "dd/MM/yyyy")

                pname = pnames.Item("@div_code")
                paramvalue.Value = Trim(divcode) ' Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select   option_selected  from reservation_parameters where param_id=511"), String))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@closing")
                paramvalue.Value = Trim(strclosing)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("level")
                paramvalue.Value = level
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                If strrpttype1 = 0 Then
                    pname = pnames.Item("cmb")
                    paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1050"), String)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)
                End If
            End If
        Else

            pname = pnames.Item("frmdate")
            paramvalue.Value = Trim(fromdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("todate")
            paramvalue.Value = Trim(todate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            'reportfilter = "from: " + fromdate + "  To: " + todate
            reportfilter = "From: " + Format(CType(fromdate, Date), "dd/MM/yyyy") + "  To: " + Format(CType(todate, Date), "dd/MM/yyyy")
            pname = pnames.Item("tran_type")
            paramvalue.Value = Trim(accttype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("tranid")
            paramvalue.Value = Trim(tranid)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("C_Btype")
            paramvalue.Value = Trim(C_Btype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            reportfilter = reportfilter + IIf(C_Btype <> "", "  Only For " + IIf(C_Btype = "B", "Banks", "Cash"), "")

            pname = pnames.Item("accfrm")
            paramvalue.Value = Trim(accfrm)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            reportfilter = reportfilter + IIf(accfrm <> "", "   For Account  " + accfrm, "")


            pname = pnames.Item("bank")
            paramvalue.Value = Trim(bank)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("divcode")
            paramvalue.Value = Trim(divcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            reportfilter = reportfilter + IIf(bank <> "", "   For Bank  " + bank, "")


            pname = pnames.Item("Curr")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from  reservation_parameters where param_id=457"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            If accttype = "CPV" Or accttype = "BPV" Or accttype = "DEP" Or accttype = "RV" Then
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
                    ElseIf Trim(poststate) = "Y" Then
                        reportfilter = reportfilter + IIf(poststate = "", "", "; State: Cancelled ")
                    End If
                End If

            End If

        End If

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
