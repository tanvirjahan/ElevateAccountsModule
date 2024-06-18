Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.IO
Partial Class rptFLMReport
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strSelectionFormula As String = ""
    Dim strReportTitle As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            Dim str As String = ""
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
                        Case "Driver"
                            'Dim str As String = ""


                            ViewState("RepCode") = Request.QueryString("drivercode")
                            ViewState("RepName") = Request.QueryString("drivername")

                            If CType(Request.QueryString("drivercode"), String) <> "" Then
                                strReportTitle = "Driver Code : " & CType(ViewState("RepCode"), String)
                                str = "{drivermaster.drivercode} = '" & ViewState("RepCode") & "'"
                            End If
                            If CType(Request.QueryString("drivername"), String) <> "" Then
                                If str <> "" Then
                                    strReportTitle = strReportTitle & " ; Driver Name : " & CType(ViewState("RepName"), String)
                                    str = str & " and {drivermaster.drivername} LIKE '" & ViewState("RepName") & "*'"
                                Else
                                    strReportTitle = "Driver Name : " & CType(ViewState("RepName"), String)
                                    str = "{drivermaster.drivername} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            strSelectionFormula = str

                            'strReportName = CType(Server.MapPath("~\Report\rptDriverDetails.rpt"), String)
                            strReportName = CType(Server.MapPath("~\Report\rptDriverDet1.rpt"), String)
                            rptreportname = "Report - Driver Details"
                            Exit Select
                        Case "VehicleMake"
                            'Dim str As String = ""


                            ViewState("RepCode") = Request.QueryString("vehiclemakecode")
                            ViewState("RepName") = Request.QueryString("vehiclemakename")

                            If CType(Request.QueryString("vehiclemakecode"), String) <> "" Then
                                strReportTitle = "VehicleMake Code : " & CType(ViewState("RepCode"), String)
                                str = "{vehiclemakemaster.vehiclemakecode} = '" & ViewState("RepCode") & "'"
                            End If
                            If CType(Request.QueryString("vehiclemakename"), String) <> "" Then
                                If str <> "" Then
                                    strReportTitle = strReportTitle & " ; VehicleMake Name : " & CType(ViewState("RepName"), String)
                                    str = str & " and {drivermaster.vehiclemakename} LIKE '" & ViewState("RepName") & "*'"
                                Else
                                    strReportTitle = "VehicleMake Name : " & CType(ViewState("RepName"), String)
                                    str = "{vehiclemakemaster.vehiclemakename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            strSelectionFormula = str

                            'strReportName = CType(Server.MapPath("~\Report\rptDriverDetails.rpt"), String)
                            strReportName = CType(Server.MapPath("~\Report\rptVehicleMakeMaster11.rpt"), String)
                            rptreportname = "Report - Vehicle Make Master Details"
                            Exit Select

                        Case "Vehicle"
                            'Dim str As String = ""


                            ViewState("RepCode") = Request.QueryString("vehiclecode")
                            ViewState("RepName") = Request.QueryString("vehiclename")

                            ViewState("VMCode") = Request.QueryString("vehiclemakecode")
                            ViewState("VType") = Request.QueryString("othcatcode")


                            If CType(Request.QueryString("vehiclecode"), String) <> "" Then
                                strReportTitle = "Vehicle Reg No. : " & CType(ViewState("RepCode"), String)
                                str = "{vehiclemaster.vehiclecode} LIKE  '" & ViewState("RepCode") & "*'"
                            End If
                            If CType(Request.QueryString("vehiclename"), String) <> "" Then
                                If str <> "" Then
                                    strReportTitle = strReportTitle & " ; Vehicle Name : " & CType(ViewState("RepName"), String)
                                    str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                Else
                                    strReportTitle = "Vehicle Name : " & CType(ViewState("RepName"), String)
                                    str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("VMCode"), String) <> "[Select]" Then
                                If str <> "" Then
                                    strReportTitle = strReportTitle & " ; Vehicle MaKe Code : " & CType(ViewState("VMCode"), String)
                                    str = str & " and {vehiclemaster.vehiclemakecode} LIKE '" & ViewState("VMCode") & "*'"
                                Else
                                    strReportTitle = "Vehicle MaKe Code : " & CType(ViewState("VMCode"), String)
                                    str = "{vehiclemaster.vehiclemakecode} LIKE '" & ViewState("VMCode") & "*'"
                                End If
                            End If
                            If CType(ViewState("VType"), String) <> "[Select]" Then
                                If str <> "" Then
                                    strReportTitle = strReportTitle & " ; Vehicle Type code : " & CType(ViewState("VType"), String)
                                    str = str & " and {vehiclemaster.othcatcode} LIKE '" & ViewState("VType") & "*'"
                                Else
                                    strReportTitle = "Vehicle Type Code : " & CType(ViewState("VType"), String)
                                    str = "{vehiclemaster.othcatcode} LIKE '" & ViewState("VType") & "*'"
                                End If
                            End If
                            strSelectionFormula = str

                            'strReportName = CType(Server.MapPath("~\Report\rptDriverDetails.rpt"), String)
                            strReportName = CType(Server.MapPath("~\Report\rptVehicle.rpt"), String)
                            rptreportname = "Report - Vehicle  Details"
                            Exit Select

                        Case "ArrDepsummary"

                            ViewState("fromdate") = Request.QueryString("fromdate")
                            ViewState("todate") = Request.QueryString("todate")
                            ViewState("bookingtype") = Request.QueryString("bookingtype")
                            ViewState("plgrpcode") = Request.QueryString("plgrpcode")
                            ViewState("agentcode") = Request.QueryString("agentcode")
                            ViewState("flightcode") = Request.QueryString("flightcode")
                            ViewState("transfertype") = Request.QueryString("transfertype")
                            ViewState("filtertransfer") = Request.QueryString("filtertransfer")
                            ViewState("filterassigned") = Request.QueryString("filterassigned")
                            ViewState("requestid") = Request.QueryString("requestid")
                            ViewState("appid") = Request.QueryString("appid")
                            ViewState("strfilter") = Request.QueryString("strfilter")
                            ViewState("filterguesttype") = Request.QueryString("filterguesttype")



                            If CType(ViewState("fromdate"), String) <> "" Then
                                strReportTitle = "From : " & CType(ViewState("fromdate"), String)
                                ' str = "{vehiclemaster.vehiclecode} LIKE  '" & ViewState("RepCode") & "*'"
                            End If
                            If CType(ViewState("todate"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & "    To : " & CType(ViewState("todate"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("bookingtype"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Booking Type : " & CType(ViewState("bookingtype"), String)

                                Else
                                    strReportTitle = "Booking Type : " & CType(ViewState("bookingtype"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("plgrpcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Market : " & CType(ViewState("plgrpcode"), String)

                                Else
                                    strReportTitle = "Market : " & CType(ViewState("plgrpcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("agentcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Agent : " & CType(ViewState("agentcode"), String)

                                Else
                                    strReportTitle = "Agent : " & CType(ViewState("agentcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("flightcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Flight : " & CType(ViewState("flightcode"), String)

                                Else
                                    strReportTitle = "Flight : " & CType(ViewState("flightcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("transfertype"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Transfer Type : " & CType(ViewState("transfertype"), String)

                                Else
                                    strReportTitle = "Transfer Type : " & CType(ViewState("transfertype"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("filtertransfer"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Filter Transfer : " & CType(ViewState("filtertransfer"), String)

                                Else
                                    strReportTitle = "Filter Transfer : " & CType(ViewState("filtertransfer"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("filterassigned"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Filter Assigned : " & CType(ViewState("filterassigned"), String)

                                Else
                                    strReportTitle = "Filter Assigned : " & CType(ViewState("filterassigned"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("requestid"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; File No. : " & CType(ViewState("requestid"), String)

                                Else
                                    strReportTitle = "File No. : " & CType(ViewState("requestid"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("filterguesttype"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Filter Guest Type : " & CType(ViewState("filterguesttype"), String)

                                Else
                                    strReportTitle = "Filter Guest Type : " & CType(ViewState("filterguesttype"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            ViewState("reportoption") = CType(ViewState("strfilter"), String)
                            strReportName = CType(Server.MapPath("~\Report\ArrDepSummary.rpt"), String)
                            rptreportname = "Arrival And Departure Summary"
                            Exit Select

                        Case "DriverVoucher"

                            ViewState("fromdate") = Request.QueryString("fromdate")
                            ViewState("todate") = Request.QueryString("todate")
                            ViewState("bookingtype") = Request.QueryString("bookingtype")
                            ViewState("plgrpcode") = Request.QueryString("plgrpcode")
                            ViewState("agentcode") = Request.QueryString("agentcode")
                            ViewState("flightcode") = Request.QueryString("flightcode")
                            ViewState("transfertype") = Request.QueryString("transfertype")
                            ViewState("filtertransfer") = Request.QueryString("filtertransfer")
                            ViewState("filterassigned") = Request.QueryString("filterassigned")
                            ViewState("requestid") = Request.QueryString("requestid")
                            ViewState("drivercode") = Request.QueryString("drivercode")

                            If CType(ViewState("fromdate"), String) <> "" Then
                                strReportTitle = "From : " & CType(ViewState("fromdate"), String)
                                ' str = "{vehiclemaster.vehiclecode} LIKE  '" & ViewState("RepCode") & "*'"
                            End If
                            If CType(ViewState("todate"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; To : " & CType(ViewState("todate"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("bookingtype"), String) <> "" Then
                                Dim booktype As String = ""
                                Select Case ViewState("bookingtype")
                                    Case 0
                                        booktype = "All"
                                    Case 1
                                        booktype = "Confirmed "
                                    Case 2
                                        booktype = "Reconfirmed"

                                End Select
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Booking Type : " & CType(booktype, String)

                                Else
                                    strReportTitle = "Booking Type : " & CType(booktype, String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("plgrpcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Market : " & CType(ViewState("plgrpcode"), String)

                                Else
                                    strReportTitle = "Market : " & CType(ViewState("plgrpcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("agentcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Agent : " & CType(ViewState("agentcode"), String)

                                Else
                                    strReportTitle = "Agent : " & CType(ViewState("agentcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("flightcode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Flight : " & CType(ViewState("flightcode"), String)

                                Else
                                    strReportTitle = "Flight : " & CType(ViewState("flightcode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("transfertype"), String) <> "" Then

                                Dim transfertype As String = ""
                                Select Case ViewState("transfertype")
                                    Case 0
                                        transfertype = "All"
                                    Case 1
                                        transfertype = "Arrival Border"
                                    Case 2
                                        transfertype = "Departure Border"
                                    Case 3
                                        transfertype = "Internal Transfer/Excursion"
                                    Case 4
                                        transfertype = "Arrival/ Departure Transfer Border"
                                End Select

                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Transfer Type : " & CType(transfertype, String)

                                Else
                                    strReportTitle = "Transfer Type : " & CType(transfertype, String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("filtertransfer"), String) <> "" Then
                                Dim filltrans As String = ""
                                Select Case ViewState("filtertransfer")
                                    Case 0
                                        filltrans = "All"
                                    Case 1
                                        filltrans = "Private"
                                    Case 2
                                        filltrans = "Shuttle"

                                End Select
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Filter Transfer : " & CType(filltrans, String)

                                Else
                                    strReportTitle = "Filter Transfer : " & CType(filltrans, String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("filterassigned"), String) <> "" Then

                                Dim fillassign As String = ""
                                Select Case ViewState("filterassigned")
                                    Case 0
                                        fillassign = "All"
                                    Case 1
                                        fillassign = "Assigned"
                                    Case 2
                                        fillassign = "Pending To Assign"

                                End Select
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Filter Assigned : " & CType(fillassign, String)

                                Else
                                    strReportTitle = "Filter Assigned : " & CType(fillassign, String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("requestid"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; File No. : " & CType(ViewState("requestid"), String)

                                Else
                                    strReportTitle = "File No. : " & CType(ViewState("requestid"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("drivercode"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Drivercode : " & CType(ViewState("drivercode"), String)

                                Else
                                    strReportTitle = "Drivercode : " & CType(ViewState("drivercode"), String)
                                    ' str = "{vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            ViewState("reportoption") = strReportTitle
                            strReportName = CType(Server.MapPath("~\Report\DriverVoucher.rpt"), String)
                            rptreportname = "Driver Voucher"
                            Exit Select
                        Case "SupplierRequest"

                            ViewState.Add("Filter", Request.QueryString("strfilter"))

                            ViewState("fromdate") = Request.QueryString("fromdate")
                            ViewState("todate") = Request.QueryString("todate")
                            ViewState("partycode") = Request.QueryString("partycode")
                            ViewState("transfertype") = Request.QueryString("transfertype")


                            If Request.QueryString("bookingtype") <> "" Then
                                ViewState.Add("bookingtype", Request.QueryString("bookingtype"))
                            Else
                                ViewState.Add("bookingtype", "0")
                            End If

                            If Request.QueryString("strfiltertransfer") <> "" Then
                                ViewState.Add("strfiltertransfer", Request.QueryString("strfiltertransfer"))
                            Else
                                ViewState.Add("strfiltertransfer", "0")
                            End If

                            If Request.QueryString("strfilterassigned") <> "" Then
                                ViewState.Add("strfilterassigned", Request.QueryString("strfilterassigned"))
                            Else
                                ViewState.Add("strfilterassigned", "0")
                            End If


                            If Request.QueryString("requestid") <> "" Then
                                ViewState.Add("strrequestid", Request.QueryString("requestid"))
                            Else
                                ViewState.Add("strrequestid", "")
                            End If


                            strReportName = CType(Server.MapPath("~\Report\rptSupplierRequestfleetnew2.rpt"), String)

                            Exit Select
                        Case "TransportInvoice"
                            ViewState("Requestid") = Request.QueryString("Requestid")
                            strReportName = CType(Server.MapPath("~\Report\TransportationInvoice.rpt"), String)
                            Exit Select
                        Case "TransportInvoicestat"
                            ViewState("Requestid") = Request.QueryString("Requestid")
                            ViewState("fromdate") = Request.QueryString("fromdate")
                            ViewState("todate") = Request.QueryString("todate")
                            ViewState("market") = Request.QueryString("market")
                            ViewState("customer") = Request.QueryString("customer")
                            ViewState("customername") = Request.QueryString("customername")
                            strReportName = CType(Server.MapPath("~\Report\TransportationInvoiceDate.rpt"), String)

                            If CType(ViewState("fromdate"), String) <> "" Then
                                strReportTitle = "From : " & Trim(CType(ViewState("fromdate"), Date).ToString("dd/MM/yyyy"))
                                ' str = "{vehiclemaster.vehiclecode} LIKE  '" & ViewState("RepCode") & "*'"
                            End If
                            If CType(ViewState("todate"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; To : " & Trim(CType(ViewState("todate"), Date).ToString("dd/MM/yyyy"))
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("Requestid"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; File No. : " & CType(ViewState("Requestid"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("market"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Market : " & CType(ViewState("market"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("customername"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Customer : " & CType(ViewState("customername"), String)
                                End If
                            End If
                            ViewState("reportoption") = strReportTitle

                            Exit Select
                        Case "TransportInvoicestat1"
                            ViewState("Requestid") = Request.QueryString("Requestid")
                            ViewState("fromdate") = Request.QueryString("fromdate")
                            ViewState("todate") = Request.QueryString("todate")
                            ViewState("market") = Request.QueryString("market")
                            ViewState("customer") = Request.QueryString("customer")
                            ViewState("customername") = Request.QueryString("customername")
                            strReportName = CType(Server.MapPath("~\Report\TransportationInvoiceDate.rpt"), String)

                            If CType(ViewState("fromdate"), String) <> "" Then
                                strReportTitle = "From : " & Trim(CType(ViewState("fromdate"), Date).ToString("dd/MM/yyyy"))
                                ' str = "{vehiclemaster.vehiclecode} LIKE  '" & ViewState("RepCode") & "*'"
                            End If
                            If CType(ViewState("todate"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; To : " & Trim(CType(ViewState("todate"), Date).ToString("dd/MM/yyyy"))
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If

                            If CType(ViewState("Requestid"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; File No. : " & CType(ViewState("Requestid"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("market"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Market : " & CType(ViewState("market"), String)
                                    '  str = str & " and {vehiclemaster.vehiclename} LIKE '" & ViewState("RepName") & "*'"
                                End If
                            End If
                            If CType(ViewState("customername"), String) <> "" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Customer : " & CType(ViewState("customername"), String)
                                End If
                            End If
                            ViewState("reportoption") = strReportTitle

                            Exit Select
                        Case "DriverItinerary"
                            ViewState("requestid") = Request.QueryString("Requestid")
                            ViewState("Drivercode") = Request.QueryString("drivercode")
                            strReportName = CType(Server.MapPath("~\Report\DriverItinerary.rpt"), String)
                    End Select



                    If strReportName = "" Then
                        'Response.Redirect(CType(Session("BackPageName"), String), False)
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                        Exit Sub
                    Else
                        ViewState.Add("RepCalledFrom", 0)
                        btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
                        If ViewState("Pageame") = "ArrDepsummary" Or ViewState("Pageame") = "DriverVoucher" Or ViewState("Pageame") = "DriverItinerary" Then
                            BindSummaryReport(strReportName, strReportTitle)
                        ElseIf ViewState("Pageame") = "SupplierRequest" Then
                            BindsupplierRequest(ViewState("partycode"), ViewState("fromdate"), ViewState("todate"), CType(ViewState("transfertype"), Integer))
                        ElseIf ViewState("Pageame") = "TransportInvoice" Or ViewState("Pageame") = "TransportInvoicestat" Or ViewState("Pageame") = "TransportInvoicestat1" Then

                            BindTransportInvoice(ViewState("Requestid"))
                        Else

                            BindReport(strReportName, strSelectionFormula, strReportTitle)
                        End If

                    End If
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BindTransportInvoice(ByVal reqid As String)
        Try
            Dim reportoption As String = ""

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

            If ViewState("Pageame") = "TransportInvoice" Then
                repDeocument.Load(Server.MapPath("~\Report\TransportationInvoice.rpt"))
            ElseIf ViewState("Pageame") = "TransportInvoicestat" Then
                repDeocument.Load(Server.MapPath("~\Report\TransportationInvoiceDate.rpt"))
            ElseIf ViewState("Pageame") = "TransportInvoicestat1" Then
                repDeocument.Load(Server.MapPath("~\Report\TransportationInvoiceDate1.rpt"))
            End If
           


            CRVflmReport.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            'repDeocument.SummaryInfo.ReportTitle = strReportTitle

            pnames = repDeocument.DataDefinition.ParameterFields


            pname = pnames.Item("@requestid")
            paramvalue.Value = reqid
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("cmb")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine1")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
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
            If ViewState("Pageame") = "TransportInvoicestat" Or ViewState("Pageame") = "TransportInvoicestat1" Then
                pname = pnames.Item("@fromdate")
                paramvalue.Value = ViewState("fromdate")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@todate")
                paramvalue.Value = ViewState("todate")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@plgrpcode")
                paramvalue.Value = ViewState("market")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@agentcode")
                paramvalue.Value = ViewState("customer")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("rptFilter")

                paramvalue.Value = ViewState("reportoption")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            Me.CRVflmReport.ReportSource = repDeocument
            Me.CRVflmReport.DataBind()
            'Response.Buffer = False
            'Response.ClearContent()
            'Response.ClearHeaders()
            'repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")


        Catch ex As Exception

        Finally

        End Try
    End Sub
    Private Sub BindsupplierRequest(ByVal partycode As String, ByVal fromdate As String, ByVal todate As String, ByVal transfertype As Integer)
        Try
            Dim reportoption As String = ""

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


            ' repDeocument.Load(Server.MapPath("~\Report\rptSupplierRequestfleet1.rpt"))
            'repDeocument.Load(Server.MapPath("~\Report\rptSupplierRequestfleetnew1.rpt"))
            repDeocument.Load(Server.MapPath("~\Report\rptSupplierRequestfleetnew2.rpt"))


            CRVflmReport.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            'repDeocument.SummaryInfo.ReportTitle = strReportTitle

            pnames = repDeocument.DataDefinition.ParameterFields


            pname = pnames.Item("@partycode")
            paramvalue.Value = partycode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("repfilter")
            paramvalue.Value = ViewState("Filter")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@transfertype")
            paramvalue.Value = transfertype
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@bookingtype")
            paramvalue.Value = ViewState("bookingtype")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@filtertransfer")
            paramvalue.Value = ViewState("strfiltertransfer")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@filterassigned")
            paramvalue.Value = ViewState("strfilterassigned")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@requestid")
            paramvalue.Value = ViewState("strrequestid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)





            pname = pnames.Item("@fromdate")
            paramvalue.Value = fromdate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@todate")
            paramvalue.Value = todate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("cmb")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine1")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
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



            Me.CRVflmReport.ReportSource = repDeocument
            Me.CRVflmReport.DataBind()

            'Response.Buffer = False
            'Response.ClearContent()
            'Response.ClearHeaders()
            'repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")


        Catch ex As Exception

        Finally

        End Try
    End Sub


    Private Sub BindSummaryReport(ByVal ReportName As String, ByVal strReportTitle As String)
        Try



            Dim reportoption As String = ""

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

            If ViewState("Pageame") = "DriverVoucher" Then
                ReportName = CType(Server.MapPath("~\Report\DriverVoucher.rpt"), String)
            ElseIf ViewState("Pageame") = "DriverItinerary" Then
                ReportName = CType(Server.MapPath("~\Report\DriverItinerary.rpt"), String)
            Else
                If ViewState("appid") = 1 Then

                    ReportName = CType(Server.MapPath("~\Report\ArrDepSummary.rpt"), String)
                Else

                    ReportName = CType(Server.MapPath("~\Report\ArrDepSummaryNew.rpt"), String)
                End If

            End If

            repDeocument.Load(ReportName)


            CRVflmReport.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            'repDeocument.SummaryInfo.ReportTitle = strReportTitle

            pnames = repDeocument.DataDefinition.ParameterFields

            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            If ViewState("Pageame") = "ArrDepsummary" Or ViewState("Pageame") = "DriverVoucher" Then

                If ViewState("appid") = 1 Then
                    pname = pnames.Item("ReportName")
                    paramvalue.Value = rptreportname
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                End If

            End If

            If ViewState("Pageame") = "ArrDepsummary" Or ViewState("Pageame") = "DriverVoucher" Or ViewState("Pageame") = "DriverItinerary" Then
                pname = pnames.Item("cmb")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
                'paramvalue.Value = rptreportname
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
            If ViewState("Pageame") = "ArrDepsummary" Or ViewState("Pageame") = "DriverVoucher" Then


                pname = pnames.Item("rptFilter")

                paramvalue.Value = ViewState("reportoption")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("@fromdate")
                paramvalue.Value = Format(CType(ViewState("fromdate"), Date), "yyyy/MM/dd")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@todate")
                paramvalue.Value = Format(CType(ViewState("todate"), Date), "yyyy/MM/dd")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@bookingtype")
                paramvalue.Value = ViewState("bookingtype")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("@plgrpcode")
                paramvalue.Value = ViewState("plgrpcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@agentcode")
                paramvalue.Value = ViewState("agentcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@flightcode")
                paramvalue.Value = ViewState("flightcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("@transfertype")
                paramvalue.Value = ViewState("transfertype")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@filtertransfer")
                paramvalue.Value = ViewState("filtertransfer")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@filterassigned")
                paramvalue.Value = ViewState("filterassigned")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@requestid")
                paramvalue.Value = ViewState("requestid")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                If ViewState("Pageame") = "ArrDepsummary" Then
                    pname = pnames.Item("@filterguesttype")
                    paramvalue.Value = ViewState("filterguesttype")
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)
                End If

            End If
            If ViewState("Pageame") = "DriverVoucher" Then
                pname = pnames.Item("@drivercode")
                paramvalue.Value = ViewState("drivercode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("frmdate")
                paramvalue.Value = ViewState("fromdate")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("Todate")
                paramvalue.Value = ViewState("todate")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


            End If

            If ViewState("Pageame") = "DriverItinerary" Then

                pname = pnames.Item("@requestid")
                paramvalue.Value = ViewState("requestid")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@drivercode")
                paramvalue.Value = ViewState("Drivercode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
                pname = pnames.Item("addrLine1")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
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


            End If

            Me.CRVflmReport.ReportSource = repDeocument
            'If ViewState("Pageame") = "ArrDepsummary" Or ViewState("Pageame") = "DriverVoucher" Or ViewState("Pageame") = "DriverItinerary" Then
            '    Response.Buffer = False
            '    Response.ClearContent()
            '    Response.ClearHeaders()
            '    repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
            'Else
            Me.CRVflmReport.DataBind()
            'CRVflmReport.HasCrystalLogo = False
            'End If

        Catch ex As Exception

        End Try
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

        Me.CRVflmReport.ReportSource = repDeocument
        Dim RepTbls As Tables = repDeocument.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next


        repDeocument.SummaryInfo.ReportTitle = strReportTitle
        pnames = repDeocument.DataDefinition.ParameterFields

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
        Me.CRVflmReport.ReportSource = repDeocument
        If strSelectionFormula <> "" Then
            CRVflmReport.SelectionFormula = strSelectionFormula
        End If
        Session.Add("ReportSource", repDeocument)
        Me.CRVflmReport.DataBind()
        CRVflmReport.HasCrystalLogo = False
        Try
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ViewState.Add("RepCalledFrom", 0)
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
        Try
            If ViewState("RepCalledFrom") <> 1 Then
                repDeocument.Close()
                repDeocument.Dispose()

            End If
        Catch ex As Exception

        End Try
      
    End Sub

    Protected Sub btnprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnprint.Click
        Session.Add("ReportSource", repDeocument)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
