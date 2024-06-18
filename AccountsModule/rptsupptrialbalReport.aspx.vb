Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptsupptrialbalReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String, todate As String, movflg As String
    Dim divcode As String
    Dim accttype, gpby, frommarkcode, tomarkcode, fromctry, toctry, fromcity, tocity, fromcode, tocode, fromsptype, fromtosptype, fromcat, tocat, fromglcode, toglcode, currtype, orderby, includezero, withCredit, custgroup_sp_type As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If


            If Request.QueryString("fromdate") <> "" Then
                fromdate = Trim(Request.QueryString("fromdate"))
            End If
            If Request.QueryString("todate") <> "" Then
                todate = Trim(Request.QueryString("todate"))
            End If

            If Request.QueryString("divid") <> "" Then
                divcode = Trim(Request.QueryString("divid"))
            End If

            If Request.QueryString("fromctry") <> "" Then
                fromctry = Trim(Request.QueryString("fromctry"))
            End If
            If Request.QueryString("toctry") <> "" Then
                toctry = Trim(Request.QueryString("toctry"))
            End If
            If Request.QueryString("custgroup_sp_type") <> "" Then
                custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
            End If
            If Request.QueryString("type") = "S" Then
                If Request.QueryString("fromcity") <> "" Then
                    fromcity = Trim(Request.QueryString("fromcity"))
                End If
                If Request.QueryString("tocity") <> "" Then
                    tocity = Trim(Request.QueryString("tocity"))
                End If
                If Request.QueryString("acctype") <> "" Then
                    accttype = Trim(Request.QueryString("acctype"))
                End If
                If Request.QueryString("fromsptype") <> "" Then
                    fromsptype = Trim(Request.QueryString("fromsptype"))
                End If
                If Request.QueryString("fromtosptype") <> "" Then
                    fromtosptype = Trim(Request.QueryString("fromtosptype"))
                End If
            End If

            If Request.QueryString("type") = "C" Then
                If Request.QueryString("frommarkcode") <> "" Then
                    frommarkcode = Trim(Request.QueryString("frommarkcode"))
                End If
                If Request.QueryString("tomarkcode") <> "" Then
                    tomarkcode = Trim(Request.QueryString("tomarkcode"))
                End If
            End If


            If Request.QueryString("movflg") <> "" Then
                movflg = Trim(Request.QueryString("movflg"))
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
            If Request.QueryString("fromglcode") <> "" Then
                fromglcode = Trim(Request.QueryString("fromglcode"))
            End If
            If Request.QueryString("toglcode") <> "" Then
                toglcode = Trim(Request.QueryString("toglcode"))
            End If
            If Request.QueryString("currtype") <> "" Then
                currtype = Trim(Request.QueryString("currtype"))
            End If
            If Request.QueryString("orderby") <> "" Then
                orderby = Trim(Request.QueryString("orderby"))
            End If
            If Request.QueryString("includezero") <> "" Then
                includezero = Trim(Request.QueryString("includezero"))
            End If
            If Request.QueryString("gpby") <> "" Then
                gpby = Trim(Request.QueryString("gpby"))
            End If
            If Request.QueryString("withCredit") <> "" Then
                withCredit = Trim(Request.QueryString("withCredit"))
            End If


            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
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
        'rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
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
        If Request.QueryString("type") = "S" Then
            If accttype = "S" Then

                rptreportname = IIf(Trim(withCredit) = 1, "Supplier  with Debit Balance", "Supplier Trial Balance")

            Else
                rptreportname = IIf(Trim(withCredit) = 1, "Supplier Agent  with Debit Balance", "Supplier Agent Trial Balance")

            End If


            If movflg = "0" Then
                rep.Load(Server.MapPath("..\Report\rptsupTrialbal_withmovement.rpt"))
            Else
                rep.Load(Server.MapPath("..\Report\rptsupTrialbal_withoutmovement.rpt"))
            End If


        End If

        If Request.QueryString("type") = "C" Then
            If Request.QueryString("trialtype") = "TB" Then
                rptreportname = IIf(Trim(withCredit) = 1, "Customer with Credit Balance", "Customer Trial Balance")
                If movflg = "0" Then
                    rep.Load(Server.MapPath("..\Report\rptcustTrialbal_withmovement.rpt"))
                ElseIf movflg = "1" Then
                    rep.Load(Server.MapPath("..\Report\rptcustTrialbal_withoutmovement.rpt"))
                ElseIf movflg = "2" Then
                    rep.Load(Server.MapPath("..\Report\rptcustTrialbal_balance.rpt"))
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

        pname = pnames.Item("gpby")
        paramvalue.Value = gpby
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

        pname = pnames.Item("@movflg")
        paramvalue.Value = Trim(movflg)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)




        pname = pnames.Item("@fromcode")
        paramvalue.Value = Trim(fromcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@tocode")
        paramvalue.Value = Trim(tocode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@custgroup_sp_type")
        paramvalue.Value = Trim(custgroup_sp_type)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        If Request.QueryString("type") = "S" Then

            reportfilter = IIf((fromcode <> "" And tocode <> ""), IIf(accttype = "S", "Supplier", "Supplier Agent") + " Code from " + fromcode + " to " + tocode, "")

            pname = pnames.Item("@acctype")
            paramvalue.Value = Trim(accttype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromsptype")
            paramvalue.Value = Trim(fromsptype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tosptype")
            paramvalue.Value = Trim(fromtosptype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            reportfilter = reportfilter + IIf((fromsptype <> "" And fromtosptype <> ""), " Supplier Type  from " + fromsptype + " to " + fromtosptype, "")

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

            reportfilter = reportfilter + IIf((fromcity <> "" And tocity <> ""), " Supplier city  from " + fromcity + " to " + tocity, "")

        End If

        If Request.QueryString("type") = "C" Then

            reportfilter = IIf((fromcode <> "" And tocode <> ""), " Customer Code from " + fromcode + " to " + tocode, "")

            pname = pnames.Item("@frommarkcode")
            paramvalue.Value = Trim(frommarkcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tomarkcode")
            paramvalue.Value = Trim(tomarkcode)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            reportfilter = reportfilter + IIf((frommarkcode <> "" And tomarkcode <> ""), " Market from " + frommarkcode + " to " + tomarkcode, "")

        End If
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
        reportfilter = reportfilter + IIf((fromctry <> "" And toctry <> ""), " Supplier country  from " + fromctry + " to " + toctry, "")

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

        reportfilter = reportfilter + IIf((fromcat <> "" And tocat <> ""), " Category code from " + fromcat + " to " + tocat, "")


        pname = pnames.Item("@fromglcode")
        paramvalue.Value = Trim(fromglcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toglcode")
        paramvalue.Value = Trim(toglcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = reportfilter + IIf((fromglcode <> "" And toglcode <> ""), " Control account code from " + fromglcode + " to " + toglcode, "")


        pname = pnames.Item("@currtype")
        paramvalue.Value = Trim(currtype)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@divcode")
        paramvalue.Value = Trim(divcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@orderby")
        paramvalue.Value = Trim(orderby)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@includezero")
        paramvalue.Value = Trim(includezero)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        ' If Request.QueryString("type") = "C" Then
        pname = pnames.Item("@withCredit")
        paramvalue.Value = Trim(withCredit)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        'End If


        pname = pnames.Item("reportfilter")
        paramvalue.Value = reportfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Session.Add("ReportSource", rep)
        '  Me.CRVReport.ReportSource = rep
        Me.CRVReport.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    rep.Close()
        '    rep.Dispose()
        'End If
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub
    
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        'If Request.QueryString("type") = "S" Then
        '    Response.Redirect("RptSupplierTrialBalance.aspx?fromdate=" & fromdate & "&todate=" & todate _
        '   & "&fromctry=" & fromctry & "&toctry=" & toctry & "&fromcity=" & fromcity & "&tocity=" & tocity & "&movflg=" & movflg & "&acctype=" & accttype & "&fromcode=" & fromcode & "&tocode =" & tocode _
        '   & "&fromsptype=" & fromsptype & "&tosptype=" & fromtosptype & "&fromcat=" & fromcat _
        '   & "&tocat=" & tocat & "&fromglcode=" & fromglcode & "&toglcode=" & toglcode _
        '   & "&currtype=" & currtype & "&orderby=" & orderby & "&includezero=" & includezero & "&gpby=" & gpby)

        'End If
        'If Request.QueryString("type") = "C" Then
        '    Response.Redirect("RptCustomerTrialBalance.aspx?fromdate=" & fromdate & "&todate=" & todate _
        ' & "&fromctry=" & fromctry & "&toctry=" & toctry & "&movflg=" & movflg & "&fromcode=" & fromcode & "&tocode =" & tocode _
        ' & "&frommarkcode=" & frommarkcode & "&tomarkcode=" & tomarkcode & "&fromcat=" & fromcat _
        ' & "&tocat=" & tocat & "&fromglcode=" & fromglcode & "&toglcode=" & toglcode _
        ' & "&currtype=" & currtype & "&orderby=" & orderby & "&includezero=" & includezero & "&gpby=" & gpby, False)

        'End If
    End Sub

     
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
