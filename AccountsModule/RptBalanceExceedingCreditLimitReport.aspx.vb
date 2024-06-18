Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class RptBalanceExceedingCreditLimitReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim todate As String
    Dim accttype, gpby, frommarkcode, tomarkcode, fromctry, toctry, fromcode, tocode, fromcat, tocat, orderby As String
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If Request.QueryString("type") <> "" Then
                accttype = Request.QueryString("type")
            End If
            If Request.QueryString("todate") <> "" Then
                todate = Request.QueryString("todate")
            End If

            If Request.QueryString("fromctry") <> "" Then
                fromctry = Request.QueryString("fromctry")
            End If
            If Request.QueryString("toctry") <> "" Then
                toctry = Request.QueryString("toctry")
            End If

            If Request.QueryString("frommarkcode") <> "" Then
                frommarkcode = Request.QueryString("frommarkcode")
            End If
            If Request.QueryString("tomarkcode") <> "" Then
                tomarkcode = Request.QueryString("tomarkcode")
            End If


            If Request.QueryString("fromcode") <> "" Then
                fromcode = Request.QueryString("fromcode")
            End If
            If Request.QueryString("tocode") <> "" Then
                tocode = Request.QueryString("tocode")
            End If
            If Request.QueryString("fromcat") <> "" Then
                fromcat = Request.QueryString("fromcat")
            End If
            If Request.QueryString("tocat") <> "" Then
                tocat = Request.QueryString("tocat")
            End If
            If Request.QueryString("orderby") <> "" Then
                orderby = Request.QueryString("orderby")
            End If
            If Request.QueryString("gpby") <> "" Then
                gpby = Request.QueryString("gpby")
            End If
            ViewState.Add("RepCalledFrom", 0)
            BindReport()
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
        With ConnInfo
            .ServerName = Session("dbServerName")
            .DatabaseName = Session("dbDatabaseName")
            .UserID = Session("dbUserName")
            .Password = Session("dbPassword")
        End With
        ' If Request.QueryString("type") = "C" Then
        rptreportname = "Balance Exceeding Credit limit"
        rep.Load(Server.MapPath("..\Report\rptcustBalanceExceedingCreditLimit.rpt"))
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
        paramvalue.Value = Trim(gpby)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@todate")
        paramvalue.Value = Trim(todate)
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


        'If Request.QueryString("type") = "C" Then
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
        ' End If
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
        reportfilter = reportfilter + IIf((fromctry <> "" And toctry <> ""), " country  from " + fromctry + " to " + toctry, "")

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


        pname = pnames.Item("@orderby")
        paramvalue.Value = Trim(orderby)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


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
