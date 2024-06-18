
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource




Partial Class ExcursionModule_rptReportNew
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strSelectionFormula As String = ""
    Dim strReportTitle As String = ""
    Dim repfilter As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbConnectionName"), "select conm from columbusmaster"), String)
        Dim strReportName As String = ""
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
        If CType(ViewState("BackPageName"), String) = "" Then
            'Response.Redirect("Login.aspx", False)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        Else
            If CType(ViewState("Pageame"), String) = "" Then
                'Response.Redirect(CType(Session("BackPageName"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                Exit Sub
            Else
                Select Case CType(ViewState("Pageame"), String)

                    Case "salesexperts"
                        If Request.QueryString("code") <> "" Then
                            strReportTitle = "S.Person Code : " & Request.QueryString("code")
                            strSelectionFormula = "{spersonmast.spersoncode} like '" & Request.QueryString("code") & "*'"
                        End If
                        If Request.QueryString("name") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle + "S.Person Name : " & Request.QueryString("name")
                                strSelectionFormula = strSelectionFormula & " and {spersonmast.spersonname} like '" & Request.QueryString("name") & "*'"
                            Else
                                strReportTitle = "S.Person Name : " & Request.QueryString("name")
                                strSelectionFormula = "{spersonmast.spersonname} like '" & Request.QueryString("name") & "*'"
                            End If
                        End If

                        If Request.QueryString("deptcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle + "Dept Code : " & Request.QueryString("deptcode")
                                strSelectionFormula = strSelectionFormula & " and {spersonmast_office.deptcode} = '" & Request.QueryString("deptcode") & "'"
                            Else
                                strReportTitle = "Dept Code : " & Request.QueryString("deptcode")
                                strSelectionFormula = "{spersonmast.deptcode} = '" & Request.QueryString("deptcode") & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\salesexperts.rpt"), String)
                        rptreportname = "Report - Sales Experts"
                        Exit Select
                    Case "concierge"
                        If Request.QueryString("code") <> "" Then
                            strReportTitle = "Concierge Code : " & Request.QueryString("code")
                            strSelectionFormula = "{spersonmast.spersoncode} like '" & Request.QueryString("code") & "*'"
                        End If
                        If Request.QueryString("name") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle + "Concierge Name : " & Request.QueryString("name")
                                strSelectionFormula = strSelectionFormula & " and {spersonmast.spersonname} like '" & Request.QueryString("name") & "*'"
                            Else
                                strReportTitle = "Concierge Name : " & Request.QueryString("name")
                                strSelectionFormula = "{spersonmast.spersonname} like '" & Request.QueryString("name") & "*'"
                            End If
                        End If

                        If Request.QueryString("deptcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle + "Dept Code : " & Request.QueryString("deptcode")
                                strSelectionFormula = strSelectionFormula & " and {spersonmast.deptcode} = '" & Request.QueryString("deptcode") & "'"
                            Else
                                strReportTitle = "Dept Code : " & Request.QueryString("deptcode")
                                strSelectionFormula = "{spersonmast.deptcode} = '" & Request.QueryString("deptcode") & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\concierge.rpt"), String)
                        rptreportname = "Report - Concierge"
                        Exit Select

                    Case "ExcMainGroup"
                        If Request.QueryString("GroupCode") <> "" Then
                            strReportTitle = "Main Group Code : " & Request.QueryString("GroupCode")
                            strSelectionFormula = "{othmaingrpmast.othmaingrpcode} like '" & Request.QueryString("GroupCode") & "*'"
                        End If
                        If Request.QueryString("GroupName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Guide Name : " & Request.QueryString("GroupName")
                                strSelectionFormula = strSelectionFormula & " and {othmaingrpmast.othmaingrpname} like '" & Request.QueryString("GroupName") & "*'"
                            Else
                                strReportTitle = "Guide Name : " & Request.QueryString("GroupName")
                                strSelectionFormula = "{othmaingrpmast.othmaingrpname} like '" & Request.QueryString("GroupName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptExcMainGroup.rpt"), String)
                        rptreportname = "Report - Guides"
                        Exit Select
                    Case "Nationality"
                        If Request.QueryString("NationalityCode") <> "" Then
                            strReportTitle = "Nationality Code : " & Request.QueryString("NationalityCode")
                            strSelectionFormula = "{nationality_master.Nationalitycode} LIKE '" & Request.QueryString("Nationalitycode") & "*'"
                        End If
                        If Request.QueryString("NationalityName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Nationality Name : " & Request.QueryString("NationalityName")
                                strSelectionFormula = strSelectionFormula & " and {nationality_master.Nationalityname} LIKE '" & Request.QueryString("NationalityName") & "*'"
                            Else
                                strReportTitle = "Nationality Name : " & Request.QueryString("NationalityName")
                                strSelectionFormula = "{nationality_master.Nationalityname} LIKE '" & Request.QueryString("NationalityName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptNationality.rpt"), String)
                        rptreportname = "Report - Nationality"
                        Exit Select

                    Case "ExcursionSellingTypesSearch"
                        If Request.QueryString("excsellcode") <> "" Then
                            strReportTitle = "Excursion Selling Code : " & Request.QueryString("excsellcode")
                            strSelectionFormula = "{excsellmast.excsellcode} LIKE '" & Request.QueryString("excsellcode") & "*'"
                        End If
                        If Request.QueryString("excsellname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Excursion Selling Name : " & Request.QueryString("excsellname")
                                strSelectionFormula = strSelectionFormula & " and {excsellmast.excsellname} LIKE '" & Request.QueryString("excsellname") & "*'"
                            Else
                                strReportTitle = "Excursion Selling Name : " & Request.QueryString("excsellname")
                                strSelectionFormula = "{excsellmast.excsellname} LIKE '" & Request.QueryString("excsellname") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {excsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{excsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptExcursionSellingTypes.rpt"), String)
                        rptreportname = "Report - Excursion  Selling Types"
                        Exit Select
                    Case "ExcursionDaysoftheweek"
                        If Request.QueryString("othtypcode") <> "" Then
                            strReportTitle = "Excursion  Code : " & Request.QueryString("othtypcode")
                            strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("othtypcode") & "*'"
                        End If
                        If Request.QueryString("othtypname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Excursion  Name : " & Request.QueryString("othtypname")
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                            Else
                                strReportTitle = "Excursion  Name : " & Request.QueryString("othtypname")
                                strSelectionFormula = "{othtypmast.othtypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                            End If
                        End If

                        If Request.QueryString("othgrpcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & ",Excursion Group Code : " & Request.QueryString("othgrpcode")
                                strSelectionFormula = strSelectionFormula & " and {othgrpmast.othgrpcode} LIKE '" & Request.QueryString("othgrpcode") & "*'"
                            Else
                                strReportTitle = "Excursion Group Code : " & Request.QueryString("othgrpcode")
                                strSelectionFormula = "{othgrpmast.othgrpcode} LIKE '" & Request.QueryString("othgrpcode") & "*'"
                            End If
                        End If
                        'If Request.QueryString("othgrpname") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Excursion  Group Name : " & Request.QueryString("othgrpname")
                        '        strSelectionFormula = strSelectionFormula & " and {othgrpmast.othgrpname} LIKE '" & Request.QueryString("othgrpname") & "*'"
                        '    Else
                        '        strReportTitle = "Excursion  Group Name : " & Request.QueryString("othgrpname")
                        '        strSelectionFormula = "{othgrpmast.othgrpname} LIKE '" & Request.QueryString("othgrpname") & "*'"
                        '    End If
                        'End If

                        strReportName = CType(Server.MapPath("~\Report\rptExcursiondaysofweek.rpt"), String)
                        rptreportname = "Report - Excursion  Days of the Week"
                        Exit Select
                    Case "ExcursionTypesSearch"
                        If Request.QueryString("othtypcode") <> "" Then
                            strReportTitle = "Excursion  Code : " & Request.QueryString("othtypcode")
                            strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("othtypcode") & "*'"
                        End If
                        If Request.QueryString("othtypname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Excursion  Name : " & Request.QueryString("othtypname")
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                            Else
                                strReportTitle = "Excursion  Name : " & Request.QueryString("othtypname")
                                strSelectionFormula = "{othtypmast.othypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                            End If
                        End If
                        If Request.QueryString("othgrpcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("othgrpcode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & CType(Request.QueryString("othgrpcode"), String) & "'"
                            Else
                                strReportTitle = "Group Code: " & CType(Request.QueryString("othgrpcode"), String)
                                strSelectionFormula = "{othtypmast.othgrpcode} = '" & CType(Request.QueryString("othgrpcode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("ticketsreqd") <> "All" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Tickets Required : " & CType(Request.QueryString("ticketsreqd"), String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.ticketsreqd} = " & IIf(CType(Request.QueryString("ticketsreqd"), String) = "Yes", "1", "0")
                            Else
                                strReportTitle = "Tickets Required : " & CType(Request.QueryString("ticketsreqd"), String)
                                strSelectionFormula = "{othtypmast.ticketsreqd} = " & IIf(CType(Request.QueryString("ticketsreqd"), String) = "Yes", "1", "0")
                            End If
                        End If

                        If Request.QueryString("uponrequest") <> "All" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Upon Request : " & CType(Request.QueryString("uponrequest"), String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.uponrequest} = " & IIf(CType(Request.QueryString("uponrequest"), String) = "Yes", "1", "0")
                            Else
                                strReportTitle = " Upon Request : " & CType(Request.QueryString("uponrequest"), String)
                                strSelectionFormula = "{othtypmast.uponrequest} = " & IIf(CType(Request.QueryString("uponrequest"), String) = "Yes", "1", "0")
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptExcursionTypes.rpt"), String)
                        rptreportname = "Report - Excursion   Types"
                        Exit Select
                    Case "Payment"
                        If Request.QueryString("PayCode") <> "" Then
                            strReportTitle = "Payment Mode Code : " & Request.QueryString("PayCode")
                            strSelectionFormula = "{paymentmodemaster.paycode} like '" & Request.QueryString("PayCode") & "*'"
                        End If
                        If Request.QueryString("PayName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Payment Mode Name : " & Request.QueryString("PayName")
                                strSelectionFormula = strSelectionFormula & " and {paymentmodemaster.payname} like '" & Request.QueryString("PayName") & "*'"
                            Else
                                strReportTitle = "Payment Mode Name : " & Request.QueryString("PayName")
                                strSelectionFormula = "{paymentmodemaster.paycode} like '" & Request.QueryString("PayName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptExcPayment.rpt"), String)
                        rptreportname = "Report - Payment Modes"
                        Exit Select

                    Case "Guide"
                        If Request.QueryString("GuideCode") <> "" Then
                            strReportTitle = "Guide Code : " & Request.QueryString("GuideCode")
                            strSelectionFormula = "{guide_master.guidecode} LIKE '" & Request.QueryString("GuideCode") & "*'"
                        End If
                        If Request.QueryString("GuideName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Guide Name : " & Request.QueryString("GuideName")
                                strSelectionFormula = strSelectionFormula & " and {guide_master.guidename} LIKE '" & Request.QueryString("GuideName") & "*'"
                            Else
                                strReportTitle = "Guide Name : " & Request.QueryString("GuideName")
                                strSelectionFormula = "{guide_master.guidename} LIKE '" & Request.QueryString("GuideName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptexcursiongroup.rpt"), String)
                        rptreportname = "Report - Excursion Groups"
                        Exit Select

                    Case "ManageTicketsReceived"

                        If Request.QueryString("TicketID") <> "" Then
                            strReportTitle = "TicketID : " & Request.QueryString("TicketID")
                            strSelectionFormula = "{excursion_tickets_received.ticketid} LIKE '" & Request.QueryString("TicketID") & "*'"
                        End If


                        If Request.QueryString("othgrpcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("othgrpcode"), String)
                                strSelectionFormula = strSelectionFormula & " and {excursion_tickets_received.othgrpcode} LIKE '" & CType(Request.QueryString("othgrpcode"), String) & "*'"
                            Else
                                strReportTitle = "Group Code: " & CType(Request.QueryString("othgrpcode"), String)
                                strSelectionFormula = "{excursion_tickets_received.othgrpcode}  LIKE '" & CType(Request.QueryString("othgrpcode"), String) & "*'"
                            End If
                        End If


                        If Request.QueryString("othtypcode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Type Code : " & CType(Request.QueryString("othtypcode"), String)
                                strSelectionFormula = strSelectionFormula & " and {excursion_tickets_received.othtypcode} LIKE '" & CType(Request.QueryString("othtypcode"), String) & "*'"
                            Else
                                strReportTitle = "Group Code: " & CType(Request.QueryString("othtypcode"), String)
                                strSelectionFormula = "{excursion_tickets_received.othtypcode} LIKE '" & CType(Request.QueryString("othtypcode"), String) & "*'"
                            End If
                        End If


                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " and  (datevalue({excursion_tickets_subdetail.ticketdate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")

                                strSelectionFormula = strSelectionFormula & " datevalue({excursion_tickets_subdetail.ticketdate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "

                            End If
                        End If

                        If Request.QueryString("FromTicketNo") <> "" And Request.QueryString("ToTicketNo") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Ticket No: " & Request.QueryString("FromTicketNo")
                                strReportTitle = strReportTitle & " ; To Ticket No: " & Request.QueryString("ToTicketNo")
                                strSelectionFormula = strSelectionFormula & " and ({excursion_tickets_subdetail.ticketno} IN  '" & CType(Request.QueryString("FromTicketNo"), String)
                                strSelectionFormula = strSelectionFormula & "' TO '" & CType(Request.QueryString("ToTicketNo"), String) & "')"
                            Else
                                strReportTitle = strReportTitle & " From Ticket No: " & Request.QueryString("FromTicketNo")
                                strReportTitle = strReportTitle & " ; To Ticket No: " & Request.QueryString("ToTicketNo")

                                strSelectionFormula = strSelectionFormula & " {excursion_tickets_subdetail.ticketno} IN  '" & CType(Request.QueryString("FromTicketNo"), String)
                                strSelectionFormula = strSelectionFormula & "' TO '" & CType(Request.QueryString("ToTicketNo"), String) & "')"

                            End If
                        End If





                        strReportName = CType(Server.MapPath("~\Report\rptExcursionManageTickets.rpt"), String)
                        rptreportname = "Report - Manage Excursion Tickets"
                        Exit Select


                    Case "Ex_NormalRequest"
                       
                        strReportName = CType(Server.MapPath("~\Report\exrequest_det.rpt"), String)

                        Exit Select

                    Case "Ex_BurjAlArabRequest"

                        strReportName = CType(Server.MapPath("~\Report\fax1_det.rpt"), String)

                        Exit Select

                    Case "Ex_DowCruiseRequest"

                        strReportName = CType(Server.MapPath("~\Report\dc_exrequest_det.rpt"), String)

                        Exit Select
                    Case "Ex_RentCarRequest"

                        strReportName = CType(Server.MapPath("~\Report\rc_exrequest_det_new.rpt"), String)

                        Exit Select

                    Case "Ex_Ticket"

                        strReportName = CType(Server.MapPath("~\Report\ticket_det.rpt"), String)

                        Exit Select

                    Case "Ex_Ticket1"

                        strReportName = CType(Server.MapPath("~\Report\deskticket_det.rpt"), String)

                        Exit Select

                    Case "Ex_request"

                        strReportName = CType(Server.MapPath("~\Report\exrequest_det.rpt"), String)

                    Case "Ex_Invoice"

                        strReportName = CType(Server.MapPath("~\Report\wl_sales_invoice.rpt"), String)
                    Case "Ex_det"
                        strReportName = CType(Server.MapPath("~\Report\exc_det_worate.rpt"), String)
                        Exit Select
                    Case "Ex_ProformaInvoice"
                        strReportName = CType(Server.MapPath("~\Report\exc_Proforma.rpt"), String)
                        Exit Select
                    Case "Ex_Email"
                        strReportName = CType(Server.MapPath("~\Report\exc_Email.rpt"), String)
                        Exit Select

                End Select
                If strReportName = "" Then
                    'Response.Redirect(CType(Session("BackPageName"), String), False)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                    Exit Sub
                Else
                    ViewState.Add("RepCalledFrom", 0)
                    btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
                    BindReport(strReportName, strSelectionFormula, strReportTitle)
                End If

            End If
        End If
    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue

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


        If CType(ViewState("Pageame"), String) = "Ex_NormalRequest" Or CType(ViewState("Pageame"), String) = "Ex_request" Then

            pname = pnames.Item("conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excreqid")
            paramvalue.Value = Request.QueryString("tranid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            If CType(ViewState("Pageame"), String) = "Ex_request" Then
                pname = pnames.Item("rlineno")
                paramvalue.Value = Request.QueryString("rlineno")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

        ElseIf CType(ViewState("Pageame"), String) = "Ex_BurjAlArabRequest" Or CType(ViewState("Pageame"), String) = "Ex_DowCruiseRequest" Or CType(ViewState("Pageame"), String) = "Ex_RentCarRequest" Then

            pname = pnames.Item("conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excreqid")
            paramvalue.Value = Request.QueryString("tranid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcotel")
            paramvalue.Value = Request.QueryString("pcotel")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcofax")
            paramvalue.Value = Request.QueryString("pcofax")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoemail")
            paramvalue.Value = Request.QueryString("pcoemail")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoweb")
            paramvalue.Value = Request.QueryString("pcoweb")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoadd2")
            paramvalue.Value = Request.QueryString("pcoadd2")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcocmt")
            paramvalue.Value = Request.QueryString("pcocmt")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        ElseIf CType(ViewState("Pageame"), String) = "Ex_Ticket" Or CType(ViewState("Pageame"), String) = "Ex_Ticket1" Then

            pname = pnames.Item("conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excreqid")
            paramvalue.Value = Request.QueryString("Refcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rlineno")
            paramvalue.Value = Request.QueryString("rlineno")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcotel")
            paramvalue.Value = Request.QueryString("pcotel")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcofax")
            paramvalue.Value = Request.QueryString("pcofax")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoemail")
            paramvalue.Value = Request.QueryString("pcoemail")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoweb")
            paramvalue.Value = Request.QueryString("pcoweb")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcopobox")
            paramvalue.Value = Request.QueryString("pcopobox")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoadd1")
            paramvalue.Value = Request.QueryString("pcoadd1")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoadd2")
            paramvalue.Value = Request.QueryString("pcoadd2")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        ElseIf CType(ViewState("Pageame"), String) = "Ex_Invoice" Or CType(ViewState("Pageame"), String) = "Ex_ProformaInvoice" Then

            pname = pnames.Item("conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("tranid")
            paramvalue.Value = Request.QueryString("tranid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("trantype")
            paramvalue.Value = Request.QueryString("trantype")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("amtinwrds")
            paramvalue.Value = Request.QueryString("amtinwrds")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("divid")
            paramvalue.Value = Request.QueryString("divid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rephead")
            paramvalue.Value = Request.QueryString("rephead")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("repfilter")
            paramvalue.Value = Request.QueryString("repfilter")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoweb")
            paramvalue.Value = Request.QueryString("pcoweb")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcoemail")
            paramvalue.Value = Request.QueryString("pcoemail")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcofax")
            paramvalue.Value = Request.QueryString("pcofax")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcotel")
            paramvalue.Value = Request.QueryString("pcotel")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        ElseIf CType(ViewState("Pageame"), String) = "Ex_det" Then
            pname = pnames.Item("conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rephead")
            paramvalue.Value = "Excursions Detail" ''Request.QueryString("rephead")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("repfilter")
            paramvalue.Value = "" ''Request.QueryString("repfilter")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excid")
            paramvalue.Value = Request.QueryString("Refcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rlineno")
            paramvalue.Value = Request.QueryString("rlineno")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        Else

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

        End If




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
        Try


        Catch ex As Exception

        End Try


        ' CRVReport.HasToggleGroupTreeButton = False
    End Sub

    

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If CType(ViewState("BackPageName"), String) = "" Then
            'Response.Redirect("MainPage.aspx", False)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
            Exit Sub
        Else
            'Session("ColReportParams") = Nothing
            'Response.Redirect(CType(Session("BackPageName"), String), False)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        '        If Page.IsPostBack = True Then
        If ViewState("RepCalledFrom") <> 1 Then
            repDeocument.Close()
            repDeocument.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
        'End If
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", repDeocument)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub




End Class
