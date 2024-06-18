Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptgltrialbalReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim divcode As String
    Dim fromdate, todate, frmcode, fromname, closing, level, withmov, currflg, type, acccodefrom, acccodeto, rptval As String
    Dim month, lastday As String
    Dim day As Date
    Dim rpttype As Integer

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            ViewState.Add("Pageame", Request.QueryString("Pageame"))
            ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

            If Request.QueryString("divid") <> "" Then
                divcode = Trim(Request.QueryString("divid"))
            End If
            If Request.QueryString("rptype") <> "" Then
                rpttype = Trim(Request.QueryString("rptype"))
            End If


            If Request.QueryString("pagetype") = "C" Then

                If Request.QueryString("fromdate") <> "" Then
                    fromdate = Trim(Request.QueryString("fromdate"))
                End If
                If Request.QueryString("todate") <> "" Then
                    todate = Trim(Request.QueryString("todate"))
                End If
                If Request.QueryString("frmcode") <> "" Then
                    frmcode = Trim(Request.QueryString("frmcode"))
                End If
                If Request.QueryString("type") <> "" Then
                    type = Trim(Request.QueryString("type"))
                End If
              

                If Request.QueryString("currflg") <> "" Then
                    currflg = Trim(Request.QueryString("currflg"))
                End If

                'If Request.QueryString("fromname") <> "" Then
                '    fromname = Trim(Request.QueryString("fromname"))
                'End If

            Else

                If Request.QueryString("pagetype") = "B" Then
                    If Request.QueryString("todate") <> "" Then
                        todate = Trim(Request.QueryString("todate"))
                    End If

                    If Request.QueryString("level") <> "" Then
                        level = Trim(Request.QueryString("level"))
                    End If

                Else

                    If Request.QueryString("fromdate") <> "" Then
                        fromdate = Trim(Request.QueryString("fromdate"))
                    End If
                    If Request.QueryString("todate") <> "" Then
                        todate = Trim(Request.QueryString("todate"))
                    End If
                    If Request.QueryString("closing") <> "" Then
                        closing = Trim(Request.QueryString("closing"))
                    End If
                    If Request.QueryString("level") <> "" Then
                        level = Trim(Request.QueryString("level"))
                    End If

                    If Request.QueryString("withmov") <> "" Then
                        withmov = Trim(Request.QueryString("withmov"))
                    End If

                    If Request.QueryString("acccodefrom") <> "" Then
                        acccodefrom = Trim(Request.QueryString("acccodefrom"))
                    End If

                    If Request.QueryString("acccodeto") <> "" Then
                        acccodeto = Trim(Request.QueryString("acccodeto"))
                    End If

                    If Request.QueryString("rptval") <> "" Then
                        rptval = Request.QueryString("rptval")
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
        ' rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
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
        Dim accnamefrom, accnameto As String
        Dim ctry As Integer

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
        reportfilter = ""

        If Request.QueryString("pagetype") = "C" Then

            If Request.QueryString("type") = "0" Then
                rep.Load(Server.MapPath("..\Report\cashbank_brief.rpt"))
                rptreportname = "Cash/Bank book --Summary"
            Else
                rep.Load(Server.MapPath("..\Report\cashbank_detail.rpt"))
                rptreportname = "Cash/Bank book --Detailed"
            End If

            rptreportname = rptreportname + " From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")
            If Trim(Request.QueryString("frmcode")) <> "" Then
                fromname = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where bankyn='Y' and acctcode='" & Trim(Request.QueryString("frmcode")) & "' ")
                reportfilter = "For Bank " + fromname
            End If


        Else
            ctry = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)

            If Request.QueryString("pagetype") = "B" Then

                'rep.Load(Server.MapPath("..\Report\rptbalancesheetnew.rpt"))
                If Request.QueryString("newformat") = 1 Then

                    rep.Load(Server.MapPath("..\Report\balance.rpt"))

                Else
                    rep.Load(Server.MapPath("..\Report\rptbalancesheetnew_5.rpt"))
                End If


                rptreportname = "Balance Sheet"
                reportfilter = "As On Date " + Format(CDate(todate), "dd/MM/yyyy")
            Else
                If Request.QueryString("rptype") = 0 Then
                    'If withmov = "0" Then
                    '    'rep.Load(Server.MapPath("..\Report\rptGLTrialbal_withmovement.rpt"))

                    '    rep.Load(Server.MapPath("..\Report\rptGLTrialbal_withmovementnew.rpt"))

                    '    reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")
                    '    rptreportname = "GL Trial Balance --Transactions"
                    'Else

                    '    rep.Load(Server.MapPath("..\Report\rptGLTrialbal_withoutmovementnew.rpt"))
                    '    reportfilter = "As on " + Format(CDate(fromdate), "dd/MM/yyyy")
                    '    rptreportname = "GL Trial Balance --Balances"



                    'End If
                    If withmov = "0" Then
                        rep.Load(Server.MapPath("..\Report\gltbtree_move.rpt"))
                        reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")
                        rptreportname = "GL Trial Balance --Transactions"
                    Else
                        rep.Load(Server.MapPath("..\Report\gltbtree.rpt"))
                        reportfilter = "As on " + Format(CDate(fromdate), "dd/MM/yyyy")
                        rptreportname = "GL Trial Balance --Balances"
                    End If
                    rpttype = 1
                Else
                    If withmov = "0" Then
                        rep.Load(Server.MapPath("..\Report\gltbtree_move.rpt"))
                        reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + " To " + Format(CDate(todate), "dd/MM/yyyy")
                        rptreportname = "GL Trial Balance --Transactions"
                    Else
                        rep.Load(Server.MapPath("..\Report\gltbtree.rpt"))
                        reportfilter = "As on " + Format(CDate(fromdate), "dd/MM/yyyy")
                        rptreportname = "GL Trial Balance --Balances"
                    End If
                    rpttype = 1
                End If

            End If

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

        If Request.QueryString("newformat") = 0 Then
            'If Val(Request.QueryString("rptype")) <> 1 Then
            If Val(rpttype) <> 1 Then

                pname = pnames.Item("@todate")
                paramvalue.Value = Trim(todate)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
        End If

        If Request.QueryString("pagetype") = "B" Then

            If Request.QueryString("newformat") = 0 Then

                pname = pnames.Item("@div_code")
                paramvalue.Value = Trim(divcode) 'Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=511"), String))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("repfilter")
                paramvalue.Value = reportfilter
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("orderlevel")
                paramvalue.Value = level
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@curr")
                paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            Else
                pname = pnames.Item("repfilter")
                paramvalue.Value = reportfilter
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("aclevel")
                paramvalue.Value = level
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("ctry")
                paramvalue.Value = ctry
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


            End If


        Else

            If Request.QueryString("pagetype") = "C" Then

                pname = pnames.Item("@fromdate")
                paramvalue.Value = Trim(fromdate)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@div_id")
                paramvalue.Value = divcode ' Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=511"), String))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@bankcode")
                paramvalue.Value = Trim(frmcode)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@type")
                paramvalue.Value = Trim(type)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@currflg")
                paramvalue.Value = Trim(currflg)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("currname")
                If currflg = "0" Then

                    paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + frmcode + "'"), String))
                Else
                    paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

                End If
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("repfilter")
                paramvalue.Value = reportfilter
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            Else
                'If Val(Request.QueryString("rptype")) = 0 Then
                If Val(rpttype) = 0 Then
                    pname = pnames.Item("@frmdate")
                    paramvalue.Value = Trim(fromdate)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@div_code")
                    paramvalue.Value = divcode ' Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=511"), String))
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@closing")
                    paramvalue.Value = Trim(closing)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)


                    pname = pnames.Item("@rptval")
                    paramvalue.Value = Val(rptval)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@acccodefrom")
                    paramvalue.Value = Trim(acccodefrom)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@acccodeto")
                    paramvalue.Value = Trim(acccodeto)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    'pname = pnames.Item("@divcode")
                    'paramvalue.Value = Trim(divcode)
                    'param = pname.CurrentValues
                    'param.Add(paramvalue)
                    'pname.ApplyCurrentValues(param)



                    If Trim(Request.QueryString("acccodefrom")) <> "" Then
                        accnamefrom = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctgroup where acctcode='" & Trim(Request.QueryString("acccodefrom")) & "' ")
                        reportfilter = reportfilter + " " + "Account From :" + acccodefrom + " " + accnamefrom
                    End If

                    If Trim(Request.QueryString("acccodeto")) <> "" Then
                        accnameto = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctgroup where acctcode='" & Trim(Request.QueryString("acccodeto")) & "' ")
                        reportfilter = reportfilter + " " + " To :" + acccodeto + " " + accnameto
                    End If


                    month = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=521"), String)
                    lastday = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=529"), String)
                    If withmov = "0" Then
                        day = Format(CType(todate, Date), "yyyy/MM/dd")
                        If day.Month = month And day.Day = lastday Then
                            reportfilter = reportfilter + IIf(closing = 0, " Including Closing entry", " Excluding Closing entry")
                        End If
                    Else
                        day = Format(CType(fromdate, Date), "yyyy/MM/dd")
                        If day.Month = month And day.Day = lastday Then
                            reportfilter = reportfilter + IIf(closing = 0, " Including Closing entry", " Excluding Closing entry")
                        End If

                    End If
                Else
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


                    pname = pnames.Item("@movflg")
                    paramvalue.Value = IIf(Val(withmov) = 0, 1, 0)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@frmdiv")
                    paramvalue.Value = divcode
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@todiv")
                    paramvalue.Value = divcode
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@currflg")
                    paramvalue.Value = 1
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@ptype")
                    paramvalue.Value = "G"
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)


                    pname = pnames.Item("@frmac")
                    paramvalue.Value = Trim(acccodefrom)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@toac")
                    paramvalue.Value = Trim(acccodeto)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@tbord")
                    paramvalue.Value = 1
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@aclevel")
                    paramvalue.Value = level
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                    pname = pnames.Item("@closing")
                    paramvalue.Value = Val(closing)
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)




                End If
                pname = pnames.Item("level")
                paramvalue.Value = Trim(level)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("currency")
                paramvalue.Value = Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("reportfilter")
                paramvalue.Value = reportfilter
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If

        End If

        '  Me.CRVReport.ReportSource = rep

        Session.Add("ReportSource", rep)
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
        'If Request.QueryString("pagetype") = "C" Then
        '    Response.Redirect("RptCashBankBook.aspx?fromdate=" & fromdate & "&todate=" & todate & "&frmcode=" & frmcode & "&type=" & type & "&currflg=" & currflg, False)
        'Else
        '    If Request.QueryString("pagetype") = "B" Then
        '        Response.Redirect("RptBalancesheet.aspx?pagetype=B&todate=" & todate & "&level=" & level, False)
        '    Else
        '        Response.Redirect("RptTrialBalance.aspx?fromdate=" & fromdate & "&todate=" & todate & "&closing=" & closing & "&level=" & level & "&withmov=" & withmov, False)
        '    End If
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
