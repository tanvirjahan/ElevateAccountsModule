'------------================--------------=======================------------------================
'   Module Name    :    rptiComplaint.aspx
'   Developer Name :    Jaffer
'   Date           :    18 Nov 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class reservation_rptagentinformation
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim agentcode As String
    Dim prntype As String
    Dim fromdate As String
    Dim todate As String
    Dim contact As String
    Dim reportfilter As String
    'Dim rptcompanyname As String
    'Dim rptreportname As String
    Dim objutils As New clsUtils
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If Request.QueryString("agentcode") Is Nothing = False Then
                agentcode = CType(Request.QueryString("agentcode"), String)
            End If
            If Request.QueryString("prntype") Is Nothing = False Then
                prntype = CType(Request.QueryString("prntype"), String)
            End If
            If Request.QueryString("fromdate") Is Nothing = False Then
                fromdate = CType(Request.QueryString("fromdate"), String)
            End If
            If Request.QueryString("todate") Is Nothing = False Then
                todate = CType(Request.QueryString("todate"), String)
            End If
            If Request.QueryString("contact") Is Nothing = False Then
                contact = CType(Request.QueryString("contact"), String)
            End If
            If Request.QueryString("reportfilter") Is Nothing = False Then
                reportfilter = CType(Request.QueryString("reportfilter"), String)
            End If

            BindReport()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport()
        Dim strReportTitle As String = "Reservation Allotment Details"
        Dim reportoption As String = ""

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo
        Dim CompanyName As String = ""
        Dim ReportName As String = ""


        With ConnInfo
            .ServerName = Session("dbServerName")
            .DatabaseName = Session("dbDatabaseName")
            .UserID = Session("dbUserName")
            .Password = Session("dbPassword")

            '.ServerName = ConfigurationManager.AppSettings("dbServerName")
            '.DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
            '.UserID = ConfigurationManager.AppSettings("dbUserName")
            '.Password = ConfigurationManager.AppSettings("dbPassword")
        End With

        CompanyName = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

        reportoption = Session("reportoption")
        If prntype = 1 Then
            rep.Load(Server.MapPath("~\Report\rptagentinformation.rpt"))
        Else
            If prntype = 2 Then
                rep.Load(Server.MapPath("~\Report\rptagentinforeport.rpt"))
                ReportName = "Agent Information Report"

            End If
        End If

        Me.CRVComplaint.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = strReportTitle
        pnames = rep.DataDefinition.ParameterFields

        pname = pnames.Item("agentcode")
        paramvalue.Value = agentcode
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If prntype = 2 Then
            pname = pnames.Item("fromdate")
            paramvalue.Value = fromdate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("todate")
            paramvalue.Value = todate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("user")
            paramvalue.Value = contact
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("CompanyName")
            paramvalue.Value = CompanyName
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ReportName")
            paramvalue.Value = ReportName
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("reportfilter")
            paramvalue.Value = reportfilter
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If


        Me.CRVComplaint.ReportSource = rep
        'Me.CRVReservationReqOther.DataBind()
        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
    End Sub
End Class
