Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Partial Class PriceListModule_RptPromotionExpiredReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim skip As String, sptypecode As String, partycodef As String, partycodet As String
    Dim citycodef As String, citycodet As String, asondate As String, plgrpcodef As String, plgrpcodet As String
    Dim repfilter As String, reportoption As String, reporttitle As String, frompartyname As String, topartyname As String, fromcityname As String, tocityname As String, fromcategoryname As String, tocategoryname As String, marketname As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try
            If Request.QueryString("datatype") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("datatype", Request.QueryString("datatype"))

                If Request.QueryString("datatype") = 1 Then
                    If Request.QueryString("frmdate") <> "" Then
                        repfilter = "Promotion Validity-" + "From Date:" & Format("U", CType(Request.QueryString("frmdate"), Date))
                    End If

                    If Request.QueryString("todate") <> "" Then
                        repfilter = repfilter & ";To Date:" & Format("U", CType(Request.QueryString("todate"), Date))
                    End If
                End If

                If Request.QueryString("datatype") = 2 Then
                    If Request.QueryString("todate") <> "" Then
                        repfilter = "Book By- " & Format("U", CType(Request.QueryString("todate"), Date))
                    End If
                End If







                If Request.QueryString("datatype") = 3 Then
                    If Request.QueryString("frmdate") <> "" Then
                        repfilter = "Range Of Dates-" + "From Date:" & Format("U", CType(Request.QueryString("frmdate"), Date))
                    End If

                    If Request.QueryString("todate") <> "" Then
                        repfilter = repfilter & ";To Date:" & Format("U", CType(Request.QueryString("todate"), Date))
                    End If
                End If




                If Request.QueryString("datatype") = 4 Then
                    If Request.QueryString("todate") <> "" Then
                        repfilter = "Book Before Days from Checkin-" & Format("U", CType(Request.QueryString("todate"), Date))
                    End If
                End If

                If Request.QueryString("datatype") = 5 Then
                    If Request.QueryString("todate") <> "" Then
                        repfilter = "Book Before Months from Check In-" & Format("U", CType(Request.QueryString("todate"), Date))
                    End If
                End If



            Else
                ViewState.Add("datatype", String.Empty)
                ViewState.Add("frmdate", String.Empty)
                ViewState.Add("todate", String.Empty)

            End If




            




            If Request.QueryString("frompartycode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")

                ViewState.Add("frompartycode", Request.QueryString("frompartycode"))
                frompartyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" + ViewState("frompartycode") + "'"), String)
                repfilter = repfilter & ";From Hotel Name:" & frompartyname
            Else
                ViewState.Add("frompartycode", String.Empty)
            End If


            If Request.QueryString("topartycode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("topartycode", Request.QueryString("topartycode"))
                topartyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" + ViewState("topartycode") + "'"), String)
                repfilter = repfilter & ";To Hotel Name:" & topartyname
            Else
                ViewState.Add("topartycode", String.Empty)
            End If

            If Request.QueryString("fromcitycode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("fromcitycode", Request.QueryString("fromcitycode"))
                fromcityname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cityname from citymast where citycode='" + ViewState("fromcitycode") + "'"), String)
                repfilter = repfilter & ";From City Name:" & fromcityname
            Else
                ViewState.Add("fromcitycode", String.Empty)
            End If

            If Request.QueryString("tocitycode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("tocitycode", Request.QueryString("tocitycode"))

                tocityname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cityname from citymast where citycode='" + ViewState("tocitycode") + "'"), String)
                repfilter = repfilter & ";To City Name:" & tocityname
            Else
                ViewState.Add("tocitycode", String.Empty)
            End If

            If Request.QueryString("fromcatcode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("fromcatcode", Request.QueryString("fromcatcode"))
                fromcategoryname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select catname from catmast where catcode='" + ViewState("fromcatcode") + "'"), String)
                repfilter = repfilter & ";From Hotel Category Name:" & fromcategoryname
            Else
                ViewState.Add("fromcatcode", String.Empty)
            End If

            If Request.QueryString("tocatcode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("tocatcode", Request.QueryString("tocatcode"))
                tocategoryname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select catname from catmast where catcode='" + ViewState("tocatcode") + "'"), String)
                repfilter = repfilter & ";To Hotel Category Name:" & tocategoryname

            Else
                ViewState.Add("tocatcode", String.Empty)
            End If


            If Trim(Request.QueryString("plgrpcode")) <> "[Select]" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("plgrpcode", Request.QueryString("plgrpcode"))
                marketname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select plgrpname from plgrpmast where plgrpcode='" + ViewState("plgrpcode") + "'"), String)
                repfilter = repfilter & ";Market Name:" & marketname

            Else
                ViewState.Add("plgrpcode", String.Empty)
            End If








            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub






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
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Report - Expired Promotions "

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



        'If Session("skip") = "Y" Then

        rep.Load(Server.MapPath("~\Report\rptExpiredPromotions.rpt"))



        ' Me.CRVPriceExpiry.ReportSource = rep
        Me.CRVPriceExpiry.ReportSource = rep

        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = ViewState("reporttitle")

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


        pname = pnames.Item("repfilter")
        'paramvalue.Value = "Updated as on : " + Session("reportoption")
        paramvalue.Value = repfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@frmdate")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = Request.QueryString("frmdate") 'ViewState("frmdate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@todate")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = Request.QueryString("todate") 'ViewState("todate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@frompartycode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("frompartycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@topartycode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("topartycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@fromcitycode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("fromcitycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@tocitycode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("tocitycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@fromcatcode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("fromcatcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@tocatcode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("tocatcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@plgrpcode")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("plgrpcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@datetype")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("datatype")

        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        Me.CRVPriceExpiry.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Session.Add("ReportSource", rep)
        Me.CRVPriceExpiry.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'Session("skip") = ""
        'Session("sptypecode") = ""
        'Session("partycodef") = ""
        'Session("partycodet") = ""
        'Session("plgrpcodef") = ""
        'Session("plgrpcodet") = ""
        'Session("citycodef") = ""
        'Session("citycodet") = ""
        'Session("asondate") = ""

        'Session("repfilter") = ""
        'Session("reportoption") = ""
        'Session("ReportTitle") = ""
        ''        Response.Redirect("rptPriceExpirySearch.aspx", False)
        'Response.Redirect("rptPriceExpirySearch.aspx?skip=" & skip & "&sptypecode=" & sptypecode _
        '& "&partycodef=" & partycodef & "&partycodet=" & partycodet & "&plgrpcodef=" & plgrpcodef & "&plgrpcodet=" & plgrpcodet _
        '& "&citycodef=" & citycodef & "&citycodet=" & citycodet & "&asondate=" & asondate _
        '& "&repfilter=" & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", rep)
    '    Dim strpop As String = ""
    '    strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUppPromotionsDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

End Class
