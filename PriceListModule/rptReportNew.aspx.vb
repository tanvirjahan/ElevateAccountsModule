Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class rptReportNew
    Inherits System.Web.UI.Page

    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strSelectionFormula As String = ""
    Dim strReportTitle As String = ""
    Dim repfilter As String = ""
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        Dim strReportName As String = ""
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
        ViewState.Add("suptype", Request.QueryString("suptype"))
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
                        strReportName = CType(Server.MapPath("~\Report\rptGuidemaster.rpt"), String)
                        rptreportname = "Report - Guides"
                        Exit Select
                    Case "Currency"
                        strReportName = CType(Server.MapPath("~\Report\rptCurrencies.rpt"), String)
                        rptreportname = "Report - Currencies"
                        Exit Select
                    Case "Hotel Remarks"
                        strReportName = CType(Server.MapPath("~\Report\rptHotelRemarks.rpt"), String)
                        rptreportname = "Report - Hotel Remarks Template"
                        Exit Select
                    Case "Market"
                        If Request.QueryString("MktCode") <> "" Then
                            strReportTitle = "Region Code : " & Request.QueryString("MktCode")
                            strSelectionFormula = "{plgrpmast.plgrpcode} LIKE '" & Request.QueryString("MktCode") & "*'"
                        End If
                        If Request.QueryString("MktName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Region Name : " & Request.QueryString("MktName")
                                strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} LIKE '" & Request.QueryString("MktName") & "*'"
                            Else
                                strReportTitle = "Region Name : " & Request.QueryString("MktName")
                                strSelectionFormula = "{plgrpmast.plgrpname} LIKE '" & Request.QueryString("MktName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptMarkets.rpt"), String)
                        rptreportname = "Report - Regions"
                        Exit Select

                    Case "Room Classification"
                        If Request.QueryString("RoomclassCode") <> "" Then
                            strReportTitle = "Roomclass Code : " & Request.QueryString("RoomclassCode")
                            strSelectionFormula = "{plgrpmast.roomclasscode} LIKE '" & Request.QueryString("RoomclassCode") & "*'"
                        End If
                        If Request.QueryString("RoomclassName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Roomclass Name: " & Request.QueryString("RoomclassName")
                                strSelectionFormula = strSelectionFormula & " and {plgrpmast.roomclassname} LIKE '" & Request.QueryString("RoomclassName") & "*'"
                            Else
                                strReportTitle = " Roomclass Name : " & Request.QueryString("RoomclassName")
                                strSelectionFormula = "{plgrpmast.roomclassname} LIKE '" & Request.QueryString("RoomclassName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptroomclassification.rpt"), String)
                        rptreportname = "Report - Room Classification"
                        Exit Select

                    Case "Hotelchain"
                        If Request.QueryString("HotelchainCode") <> "" Then
                            strReportTitle = "Hotelchain Code : " & Request.QueryString("HotelchainCode")
                            strSelectionFormula = "{plgrpmast.hotelchaincode} LIKE '" & Request.QueryString("HotelchainCode") & "*'"
                        End If
                        If Request.QueryString("HotelchainName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Hotelchain Name: " & Request.QueryString("HotelchainName")
                                strSelectionFormula = strSelectionFormula & " and {plgrpmast.hotelchainname} LIKE '" & Request.QueryString("HotelchainName") & "*'"
                            Else
                                strReportTitle = "Hotelchain Name : " & Request.QueryString("HotelchainName")
                                strSelectionFormula = "{plgrpmast.hotelchainname} LIKE '" & Request.QueryString("HotelchainName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rpthotelchain.rpt"), String)
                        rptreportname = "Report - Hotelchain Master"
                        Exit Select

                    Case ("HotelStatus")
                        If Request.QueryString("HotelstatusCode") <> "" Then
                            strReportTitle = "Hotelstatus Code : " & Request.QueryString("HotelstatusCode")
                            strSelectionFormula = "{hotelstatus.hotelstatuscode} LIKE '" & Request.QueryString("HotelstatusCode") & "*'"
                        End If
                        If Request.QueryString("HotelstatusName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Hotel  Status: " & Request.QueryString("HotelstatusName")
                                strSelectionFormula = strSelectionFormula & " and {hotelstatus.hotelstatusname} LIKE '" & Request.QueryString("HotelstatusName") & "*'"
                            Else
                                strReportTitle = "Hotel Status : " & Request.QueryString("HotelstatusName")
                                strSelectionFormula = "{hotelstatus.hotelstatusname} LIKE '" & Request.QueryString("Hotelstatusname") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rpthotelstatus.rpt"), String)
                        rptreportname = "Report - Hotel Status "
                        Exit Select
                            
                    Case ("PropertyType")
                        If Request.QueryString("PropertytypeCode") <> "" Then
                            strReportTitle = "PropertyType Code : " & Request.QueryString("PropertyTypecode")
                            strSelectionFormula = "{hotel_propertytype.propertytypecode} LIKE '" & Request.QueryString("PropertyTypeCode") & "*'"
                        End If
                        If Request.QueryString("PropertyTypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Property Type: " & Request.QueryString("ProperTytypeName")
                                strSelectionFormula = strSelectionFormula & " and {hotel_propertytype.propertytypename} LIKE '" & Request.QueryString("propertytypename") & "*'"
                            Else
                                strReportTitle = "Property Type: " & Request.QueryString("ProperTytypeName")
                                strSelectionFormula = "{hotel_propertytype.propertytypename} LIKE '" & Request.QueryString("PropertyTypeName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptpropertytype.rpt"), String)
                        rptreportname = "Report -Property Type "
                        Exit Select

                    Case "ExhibitionMaster"
                        If Request.QueryString("ExhibitionCode") <> "" Then
                            strReportTitle = "Hotelchain Code : " & Request.QueryString("ExhibitionCode")
                            strSelectionFormula = "{exhibition_master.exhibitioncode} LIKE '" & Request.QueryString("ExhibitionCode") & "*'"
                        End If
                        If Request.QueryString("ExhibitionName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Exhibition Name: " & Request.QueryString("ExhibitionName")
                                strSelectionFormula = strSelectionFormula & " and {exhibition_master.exhibitionname} LIKE '" & Request.QueryString("ExhibitionName") & "*'"
                            Else
                                strReportTitle = "Exhibition Name : " & Request.QueryString("ExhibitionName")
                                strSelectionFormula = "{exhibition_master.exhibitionname} LIKE '" & Request.QueryString("ExhibitionName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptExhibitionMaster.rpt"), String)
                        rptreportname = "Report - Exhibition Master"
                        Exit Select





                    Case "Holiday"
                        If Request.QueryString("Holidaycode") <> "" Then
                            strReportTitle = "Holiday Code : " & Request.QueryString("holidaycode")
                            strSelectionFormula = "{holidaycalendar.holidaycode} LIKE '" & Request.QueryString("Holidaycode") & "*'"
                        End If
                        If Request.QueryString("HolidayName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Hotelchain Name: " & Request.QueryString("HotelchainName")
                                strSelectionFormula = strSelectionFormula & " and {holidaycalendar.holidayame} LIKE '" & Request.QueryString("HotelchainName") & "*'"
                            Else
                                strReportTitle = "Holiday Name : " & Request.QueryString("holidayname")
                                strSelectionFormula = "{holidaycalendar.holidayname} LIKE '" & Request.QueryString("HolidayName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptholidaycalendar.rpt"), String)
                        rptreportname = "Report -Holiday Calendar"
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

                    Case "Country"
                        If Request.QueryString("CtryCode") <> "" Then
                            strReportTitle = "Country Code : " & Request.QueryString("CtryCode")
                            strSelectionFormula = "{ctrymast.ctrycode} LIKE '" & Request.QueryString("CtryCode") & "*'"
                        End If
                        If Request.QueryString("CtryName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & Request.QueryString("CtryName")
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} LIKE '" & Request.QueryString("CtryName") & "*'"
                            Else
                                strReportTitle = "Country Name : " & Request.QueryString("CtryName")
                                strSelectionFormula = "{ctrymast.ctryname} LIKE '" & Request.QueryString("CtryName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & Request.QueryString("CurrCode")
                                strSelectionFormula = "{ctrymast.currcode} = '" & Request.QueryString("CurrCode") & "'"
                            End If
                        End If

                        If Request.QueryString("CurrName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Name : " & CType(Request.QueryString("CurrName"), String)
                                strSelectionFormula = strSelectionFormula & " and {currmast.currname} = '" & Request.QueryString("CurrName") & "'"
                            Else
                                strReportTitle = "Currency Name: " & CType(Request.QueryString("CurrName"), String)
                                strSelectionFormula = "{currmast.currname} = '" & CType(Request.QueryString("CurrName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Region Code : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Region Code: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = "{ctrymast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("MktName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Region Name : " & CType(Request.QueryString("MktName"), String)
                                strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & CType(Request.QueryString("MktName"), String) & "'"
                            Else
                                strReportTitle = "Region Name: " & CType(Request.QueryString("MktName"), String)
                                strSelectionFormula = "{plgrpmast.plgrpname} = '" & CType(Request.QueryString("MktName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCountries.rpt"), String)
                        rptreportname = "Report - Countries"
                        Exit Select
                    Case "City"
                        If Request.QueryString("CityCode") <> "" Then
                            strReportTitle = "City Code : " & Request.QueryString("CityCode")
                            strSelectionFormula = "{citymast.citycode} LIKE '" & Request.QueryString("CityCode") & "*'"
                        End If
                        If Request.QueryString("CityName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Name : " & Request.QueryString("CityName")
                                strSelectionFormula = strSelectionFormula & " and {citymast.cityname} LIKE '" & Request.QueryString("CityName") & "*'"
                            Else
                                strReportTitle = "City Name : " & Request.QueryString("CityName")
                                strSelectionFormula = "{citymast.cityname} LIKE '" & Request.QueryString("CityName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Code : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {citymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country Code: " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{citymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            Else
                                strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCities.rpt"), String)
                        rptreportname = "Report - Cities"
                        Exit Select

                    Case "Sector"
                        If Request.QueryString("SectorCode") <> "" Then
                            strReportTitle = "Sector Code : " & Request.QueryString("SectorCode")
                            strSelectionFormula = "{sectormaster.sectorcode} LIKE '" & Request.QueryString("SectorCode") & "*'"
                        End If
                        If Request.QueryString("SectorName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Name : " & Request.QueryString("SectorName")
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorname} LIKE '" & Request.QueryString("SectorName") & "*'"
                            Else
                                strReportTitle = "Sector Name : " & Request.QueryString("SectorName")
                                strSelectionFormula = "{sectormaster.sectorname} LIKE '" & Request.QueryString("SectorName") & "*'"
                            End If
                        End If

                        If Request.QueryString("SGroupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Group Code : " & CType(Request.QueryString("SGroupCode"), String) & " ; Sector Group Name:" & CType(Request.QueryString("SGroupName"), String) & ""
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorgroupcode} = '" & CType(Request.QueryString("SGroupCode"), String) & "'"
                            Else
                                strReportTitle = "Sector Group Code : " & CType(Request.QueryString("SGroupCode"), String) & " ; Sector Group Name:" & CType(Request.QueryString("SGroupName"), String) & ""
                                strSelectionFormula = "{sectormaster.sectorgroupcode}  = '" & CType(Request.QueryString("SGroupCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            Else
                                strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Code : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country Code: " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{sectormaster.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            Else
                                strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CityCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Code : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            Else
                                strReportTitle = "City Code: " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = "{sectormaster.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CityName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Name : " & CType(Request.QueryString("CityName"), String)
                                strSelectionFormula = strSelectionFormula & " and {citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                            Else
                                strReportTitle = "City Name: " & CType(Request.QueryString("CityName"), String)
                                strSelectionFormula = "{citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSector.rpt"), String)
                        rptreportname = "Report - Sectors"
                        Exit Select



                    Case "Location"
                        If Request.QueryString("AreaCode") <> "" Then
                            strReportTitle = "Area Code : " & Request.QueryString("AreaCode")
                            strSelectionFormula = "{locationmaster.areacode} LIKE '" & Request.QueryString("AreaCode") & "*'"
                        End If
                        If Request.QueryString("AreaName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Area Name : " & Request.QueryString("AreaName")
                                strSelectionFormula = strSelectionFormula & " and {locationmaster.areaname} LIKE '" & Request.QueryString("AreaName") & "*'"
                            Else
                                strReportTitle = "Area Name : " & Request.QueryString("AreaName")
                                strSelectionFormula = "{locationmaster.arearname} LIKE '" & Request.QueryString("AreaName") & "*'"
                            End If
                        End If


                        strReportName = CType(Server.MapPath("~\Report\rptLocation.rpt"), String)
                        rptreportname = "Report - Locations"
                        Exit Select

                        'Added case "Location" by Archana on 18/05/2015 for menu Location Master 

                    Case "Sectorgroup"
                        If Request.QueryString("othtypcode") <> "" Then
                            strReportTitle = "Sector Group Code : " & Request.QueryString("othtypcode")
                            strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("othtypcode") & "*'"
                        End If
                        If Request.QueryString("othtypname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Group Name : " & Request.QueryString("othtypname")
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                                'Else
                                '    strReportTitle = "Sector Group Name : " & Request.QueryString("othtypname")
                                '    strSelectionFormula = "{othtypmast.othtypname} LIKE '" & Request.QueryString("othtypname") & "*'"
                            End If
                        End If

                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Code : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                                'Else
                                '    strReportTitle = "Country Code: " & CType(Request.QueryString("CtryCode"), String)
                                '    strSelectionFormula = "{othtypmast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                                'Else
                                '    strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                '    strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CityCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Code : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                                'Else
                                '    strReportTitle = "City Code: " & CType(Request.QueryString("CityCode"), String)
                                '    strSelectionFormula = "{othtypmast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CityName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Name : " & CType(Request.QueryString("CityName"), String)
                                strSelectionFormula = strSelectionFormula & " and {citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                                'Else
                                '    strReportTitle = "City Name: " & CType(Request.QueryString("CityName"), String)
                                '    strSelectionFormula = "{citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSectorgroup.rpt"), String)
                        rptreportname = "Report - Sector Group"
                        Exit Select






                    Case "Supplier Type"
                        If Request.QueryString("SuptypeCode") <> "" Then
                            strReportTitle = "Supplier Type Code: " & Request.QueryString("SuptypeCode")
                            strSelectionFormula = "{sptypemast.sptypecode} LIKE '" & Request.QueryString("SuptypeCode") & "*'"
                        End If
                        If Request.QueryString("SuptypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name: " & Request.QueryString("SuptypeName")
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} LIKE '" & Request.QueryString("SuptypeName") & "*'"
                            Else
                                strReportTitle = "Supplier Type Name: " & Request.QueryString("SuptypeName")
                                strSelectionFormula = "{sptypemast.sptypename} LIKE '" & Request.QueryString("SuptypeName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierType.rpt"), String)
                        rptreportname = "Report -Supplier Types"
                        Exit Select

                    Case "Supplier"
                        If ViewState("suptype") = "EXC" Then
                            strReportName = CType(Server.MapPath("~\Report\rptexcsupplierGroup.rpt"), String)
                            rptreportname = "Report -Excursion Supplier Group"

                        Else
                            strReportName = CType(Server.MapPath("~\Report\rptsupplierGroup.rpt"), String)
                            rptreportname = "Report -Supplier Details"
                            strSelectionFormula = "{partymast.sptypecode} LIKE '" & Request.QueryString("suptype") & "*'"
                        End If
                        Exit Select


                    Case "GPRICETERMS"

                        strReportName = CType(Server.MapPath("~\Report\rptpriceterms.rpt"), String)
                        rptreportname = "Report -Price Terms"
                        Exit Select

                    Case "Supplier Category"
                        If Request.QueryString("SupcatCode") <> "" Then
                            strReportTitle = "Supplier Category Code : " & Request.QueryString("SupcatCode")
                            strSelectionFormula = "{catmast.catcode} LIKE '" & Request.QueryString("SupcatCode") & "*'"
                        End If
                        If Request.QueryString("SupcatName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Category Name : " & Request.QueryString("SupcatName")
                                strSelectionFormula = strSelectionFormula & " and {catmast.catname} LIKE '" & Request.QueryString("SupcatName") & "*'"
                            Else
                                strReportTitle = "Supplier Category Name : " & Request.QueryString("SupcatName")
                                strSelectionFormula = "{catmast.catname} LIKE '" & Request.QueryString("SupcatName") & "*'"
                            End If
                        End If

                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {catmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{catmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If


                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("appid") <> "" Then
                            If Request.QueryString("appid") = "1" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                    strSelectionFormula = strSelectionFormula & " and {catmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                                Else
                                    strReportTitle = "Supplier Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                    strSelectionFormula = "{catmast.sptypecode} ='HOT' or  {catmast.sptypecode} ='AGT'"
                                End If
                            ElseIf Request.QueryString("appid") = "11" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                    strSelectionFormula = strSelectionFormula & " and {catmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                                Else
                                    strReportTitle = "Supplier Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                    strSelectionFormula = "{catmast.sptypecode} ='EXC'"
                                End If
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptSupplierCategories.rpt"), String)
                        rptreportname = "Report -Supplier Categories"
                        Exit Select

                    Case "Supplier Selling Category"
                        If Request.QueryString("SupsellcatCode") <> "" Then
                            strReportTitle = "Supplier Selling Category Code : " & CType(Request.QueryString("SupsellcatCode"), String)
                            strSelectionFormula = "{sellcatmast.scatcode} LIKE '" & CType(Request.QueryString("SupsellcatCode"), String) & "*'"
                        End If
                        If Request.QueryString("SupsellcatName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Selling Category Name : " & CType(Request.QueryString("SupsellcatName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellcatmast.scatname} LIKE '" & CType(Request.QueryString("SupsellcatName"), String) & "*'"
                            Else
                                strReportTitle = "Supplier Selling Category Name : " & CType(Request.QueryString("SupsellcatName"), String)
                                strSelectionFormula = "{sellcatmast.scatname} LIKE '" & CType(Request.QueryString("SupsellcatName"), String) & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SupplierType Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellcatmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "SupplierType Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{sellcatmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SupplierType Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "SupplierType Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierSellingCategories.rpt"), String)
                        rptreportname = "Report -Supplier Selling Categories"
                        Exit Select

                    Case "Meal Plan"
                        If Request.QueryString("MealCode") <> "" Then
                            strReportTitle = "Meal Plan Code : " & Request.QueryString("MealCode")
                            strSelectionFormula = "{mealmast.mealcode} LIKE '" & Request.QueryString("MealCode") & "*'"
                        End If
                        If Request.QueryString("MealName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Meal Plan Name : " & Request.QueryString("MealName")
                                strSelectionFormula = strSelectionFormula & " and {mealmast.mealname} LIKE '" & Request.QueryString("MealName") & "*'"
                            Else
                                strReportTitle = "Meal Plan Name : " & Request.QueryString("MealName")
                                strSelectionFormula = "{mealmast.mealname} LIKE '" & Request.QueryString("MealName") & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; S.Type Code : " & CType(Request.QueryString("SuptypeCode").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {mealmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode").ToUpper, String) & "'"
                            Else
                                strReportTitle = "S.Type Code: " & CType(Request.QueryString("SuptypeCode").ToUpper, String)
                                strSelectionFormula = "{mealmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode").ToUpper, String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; S.Type Name : " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                            Else
                                strReportTitle = "S.Type Name: " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptMealPlans.rpt"), String)
                        rptreportname = "Report - Meal Plans"
                        Exit Select
                    Case "Hotel Amenities"
                        strReportName = CType(Server.MapPath("~\Report\rptAmenities.rpt"), String)
                        rptreportname = "Report - Amenities"
                        Exit Select
                    Case "Other Service Group"
                        If Request.QueryString("OthgrpCode") <> "" Then
                            strReportTitle = "Group Code : " & Request.QueryString("OthgrpCode")
                            strSelectionFormula = "{othgrpmast.othgrpcode} LIKE '" & Request.QueryString("OthgrpCode") & "*'"
                        End If
                        If Request.QueryString("OthgrpName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Group Name : " & Request.QueryString("OthgrpName")
                                strSelectionFormula = strSelectionFormula & " and {othgrpmast.othgrpname} LIKE '" & Request.QueryString("OthgrpName") & "*'"
                            Else
                                strReportTitle = "Group Name : " & Request.QueryString("OthgrpName")
                                strSelectionFormula = "{othgrpmast.othgrpname} LIKE '" & Request.QueryString("OthgrpName") & "*'"
                            End If
                        End If
                        'If Request.QueryString("DeptCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Department Code : " & CType(Request.QueryString("DeptCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {othgrpmast.deptcode} = '" & CType(Request.QueryString("DeptCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Department Code: " & CType(Request.QueryString("DeptCode"), String)
                        '        strSelectionFormula = "{othgrpmast.deptcode} = '" & CType(Request.QueryString("DeptCode"), String) & "'"
                        '    End If
                        'End If

                        'If Request.QueryString("DeptName") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Department Name : " & CType(Request.QueryString("DeptName"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {DeptMaster.DeptName} = '" & CType(Request.QueryString("DeptName"), String) & "'"
                        '    Else
                        '        strReportTitle = "Department Name: " & CType(Request.QueryString("DeptName"), String)
                        '        strSelectionFormula = "{DeptMaster.DeptName} = '" & CType(Request.QueryString("DeptName"), String) & "'"
                        '    End If
                        'End If
                        Dim othtypcode1 As String
                        Dim othtypcode2 As String
                        othtypcode1 = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1104", String))
                        othtypcode2 = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1105", String))
                        If Request.QueryString("Type") = "OTH" Then

                            'If strSelectionFormula <> "" Then
                            '    strSelectionFormula = strSelectionFormula & " and not({othgrpmast.othmaingrpcode} in ['" & othtypcode1 & "','" & othtypcode2 & "'])"
                            'Else
                            '    strSelectionFormula = "not({othgrpmast.othmaingrpcode}  in ['" & othtypcode1 & "','" & othtypcode2 & "'])"
                            'End If
                        Else
                            If strSelectionFormula <> "" Then
                                strSelectionFormula = strSelectionFormula & " and ({othgrpmast.othmaingrpcode} in ['" & othtypcode1 & "','" & othtypcode2 & "'])"

                            Else
                                strSelectionFormula = "({othgrpmast.othmaingrpcode}  in ['" & othtypcode1 & "' ,'" & othtypcode2 & "'])"
                            End If


                        End If





                        'If Request.QueryString("MainGroupCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Excursion Main Group Code : " & CType(Request.QueryString("MainGroupCode"), String) & " ; Excursion Main Group Name : " & CType(Request.QueryString("MainGroupName"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {othgrpmast.othmaingrpcode }= '" & CType(Request.QueryString("MainGroupCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Excursion Main Group Code : " & CType(Request.QueryString("MainGroupCode"), String) & " ; Excursion Main Group Name : " & CType(Request.QueryString("MainGroupName"), String)
                        '        strSelectionFormula = "{othgrpmast.othmaingrpcode} = '" & CType(Request.QueryString("MainGroupCode"), String) & "'"
                        '    End If
                        'End If


                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceGroups.rpt"), String)
                        If Request.QueryString("Type") = "OTH" Then

                            rptreportname = "Report -Other Service Groups"
                            strReportName = CType(Server.MapPath("~\Report\rptOtherServiceGroupsforothers.rpt"), String)

                        Else
                            rptreportname = "Report -Excursion Groups"
                            strReportName = CType(Server.MapPath("~\Report\rptOtherServiceGroups.rpt"), String)
                        End If
                        Exit Select

                    Case "Other service Type"
                        'If Request.QueryString("OthtypeCode") <> "" Then
                        '    strReportTitle = "Type Code : " & Request.QueryString("OthtypeCode")
                        '    strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("OthtypeCode") & "*'"
                        'End If
                        'If Request.QueryString("OthtypeName") <> "" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Type Name : " & Request.QueryString("OthtypeName")
                        '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                        '    Else
                        '        strReportTitle = "Type Name : " & Request.QueryString("OthtypeName")
                        '        strSelectionFormula = "{othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                        '    End If
                        'End If

                        'If Request.QueryString("OthgrpCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    Else
                        '        strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = "{othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    End If
                        'End If

                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceTypes.rpt"), String)
                        rptreportname = "Report - Other Service Types"
                        Exit Select

                    Case "Car Rental Type"
                        If Request.QueryString("OthtypeCode") <> "" Then
                            strReportTitle = "Type Code : " & Request.QueryString("OthtypeCode")
                            strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("OthtypeCode") & "*'"
                        End If
                        If Request.QueryString("OthtypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            Else
                                strReportTitle = "Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = "{othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            End If
                        End If
                        Dim rptName As String = ""
                        If Request.QueryString("GrpName") <> "[Select]" Then
                            rptName = Request.QueryString("GrpName")
                        Else
                            rptName = "Other Service"
                        End If
                        If Request.QueryString("OthgrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                'strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            Else
                                'strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = "{othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            End If
                        Else
                            If strSelectionFormula <> "" Then
                                strSelectionFormula = strSelectionFormula & "and  {othtypmast.othgrpcode} <> ""CAR RENTAL"" And {othtypmast.othgrpcode}<> ""VISA""" & _
                                            " And {othtypmast.othgrpcode} <> ""EXC"" And {othtypmast.othgrpcode} <> ""MEALS"" And {othtypmast.othgrpcode} <> ""GUIDES"" " & _
                                    " And {othtypmast.othgrpcode} <> ""TRFS""  And {othtypmast.othgrpcode} <> ""ENTRANCE""  And {othtypmast.othgrpcode} <> ""JEEPWADI"""
                            Else
                                strSelectionFormula = strSelectionFormula & " {othtypmast.othgrpcode} <> ""CAR RENTAL"" And {othtypmast.othgrpcode}<> ""VISA""" & _
                                            " And {othtypmast.othgrpcode} <> ""EXC"" And {othtypmast.othgrpcode} <> ""MEALS"" And {othtypmast.othgrpcode} <> ""GUIDES"" " & _
                                    " And {othtypmast.othgrpcode} <> ""TRFS""  And {othtypmast.othgrpcode} <> ""ENTRANCE""  And {othtypmast.othgrpcode} <> ""JEEPWADI"""
                            End If


                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCarRentalTypes.rpt"), String)
                        rptreportname = "Report - " + rptName + " Types"
                        Exit Select


                    Case "AirPortMeet"
                        If Request.QueryString("OthtypeCode") <> "" Then
                            strReportTitle = "Type Code : " & Request.QueryString("OthtypeCode")
                            strSelectionFormula = "{othtypmast.othtypcode} LIKE '" & Request.QueryString("OthtypeCode") & "*'"
                        End If
                        If Request.QueryString("OthtypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = strSelectionFormula & " and {othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            Else
                                strReportTitle = "Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = "{othtypmast.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            End If
                        End If
                        Dim rptName As String = "Airport Meet&Assit"
                        'If Request.QueryString("GrpName") <> "[Select]" Then
                        '    rptName = Request.QueryString("GrpName")

                        'End If
                        'If Request.QueryString("OthgrpCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        'strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    Else
                        '        'strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = "{othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    End If
                        'Else
                        '    If strSelectionFormula <> "" Then
                        '        strSelectionFormula = strSelectionFormula & "and  {othtypmast.othgrpcode} <> ""CAR RENTAL"" And {othtypmast.othgrpcode}<> ""VISA""" & _
                        '                    " And {othtypmast.othgrpcode} <> ""EXC"" And {othtypmast.othgrpcode} <> ""MEALS"" And {othtypmast.othgrpcode} <> ""GUIDES"" " & _
                        '            " And {othtypmast.othgrpcode} <> ""TRFS""  And {othtypmast.othgrpcode} <> ""ENTRANCE""  And {othtypmast.othgrpcode} <> ""JEEPWADI"""
                        '    Else
                        '        strSelectionFormula = strSelectionFormula & " {othtypmast.othgrpcode} <> ""CAR RENTAL"" And {othtypmast.othgrpcode}<> ""VISA""" & _
                        '                    " And {othtypmast.othgrpcode} <> ""EXC"" And {othtypmast.othgrpcode} <> ""MEALS"" And {othtypmast.othgrpcode} <> ""GUIDES"" " & _
                        '            " And {othtypmast.othgrpcode} <> ""TRFS""  And {othtypmast.othgrpcode} <> ""ENTRANCE""  And {othtypmast.othgrpcode} <> ""JEEPWADI"""
                        '    End If


                        'End If
                        strReportName = CType(Server.MapPath("~\Report\RptAirPortMeet&Assit.rpt"), String)
                        rptreportname = "Report - " + rptName + " Types"
                        Exit Select



                    Case "Routes"
                        If Request.QueryString("OthtypeCode") <> "" Then
                            strReportTitle = "Type Code : " & Request.QueryString("OthtypeCode")
                            strSelectionFormula = "{othtypmastTransfers.othtypcode} LIKE '" & Request.QueryString("OthtypeCode") & "*'"
                        End If
                        If Request.QueryString("OthtypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = strSelectionFormula & " and {othtypmastTransfers.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            Else
                                strReportTitle = "Type Name : " & Request.QueryString("OthtypeName")
                                strSelectionFormula = "{othtypmastTransfers.othtypname} LIKE '" & Request.QueryString("OthtypeName") & "*'"
                            End If
                        End If

                        'If Request.QueryString("OthgrpCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = strSelectionFormula & " and {othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    Else
                        '        strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                        '        strSelectionFormula = "{othtypmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                        '    End If
                        'End If
                        strReportName = CType(Server.MapPath("~\Report\rptRoutes.rpt"), String)
                        rptreportname = "Report - Routes"
                        Exit Select

                    Case "Other Service Category"
                        If Request.QueryString("OthcatCode") <> "" Then
                            strReportTitle = "Category Code : " & Request.QueryString("OthcatCode")
                            strSelectionFormula = "{othcatmast.othcatcode} LIKE '" & Request.QueryString("OthcatCode") & "*'"
                        End If
                        If Request.QueryString("OthcatName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Name : " & Request.QueryString("OthcatName")
                                strSelectionFormula = strSelectionFormula & " and {othcatmast.othcatname} LIKE '" & Request.QueryString("OthcatName") & "*'"
                            Else
                                strReportTitle = "Category Name : " & Request.QueryString("OthcatName")
                                strSelectionFormula = "{othcatmast.othcatname} LIKE '" & Request.QueryString("OthcatName") & "*'"
                            End If
                        End If

                        If Request.QueryString("OthgrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {othcatmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            Else
                                strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = "{othcatmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceCategories.rpt"), String)
                        rptreportname = "Report - Other Service Categories"
                        Exit Select

                    Case "Vehicle Type"
                        If Request.QueryString("OthcatCode") <> "" Then
                            strReportTitle = " Code : " & Request.QueryString("OthcatCode")
                            strSelectionFormula = "{othcatmast.othcatcode} LIKE '" & Request.QueryString("OthcatCode") & "*'"
                        End If
                        If Request.QueryString("OthcatName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;  Name : " & Request.QueryString("OthcatName")
                                strSelectionFormula = strSelectionFormula & " and {othcatmast.othcatname} LIKE '" & Request.QueryString("OthcatName") & "*'"
                            Else
                                strReportTitle = " Name : " & Request.QueryString("OthcatName")
                                strSelectionFormula = "{othcatmast.othcatname} LIKE '" & Request.QueryString("OthcatName") & "*'"
                            End If
                        End If

                        If Request.QueryString("OthgrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle
                                '& " ; Group Code : " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {othcatmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            Else
                                'strReportTitle = "Group Code: " & CType(Request.QueryString("OthgrpCode").ToUpper, String)
                                strSelectionFormula = "{othcatmast.othgrpcode} = '" & CType(Request.QueryString("OthgrpCode").ToUpper, String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptVehicleType.rpt"), String)
                        rptreportname = "Report - Vehicle Type"
                        Exit Select



                    Case "Special Event Extras"
                        If Request.QueryString("SpleventCode") <> "" Then
                            strReportTitle = "Spl Events/ Extra Code : " & Request.QueryString("SpleventCode")
                            strSelectionFormula = "{spleventsmast.spleventcode} LIKE '" & Request.QueryString("SpleventCode") & "*'"
                        End If
                        If Request.QueryString("SpleventName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Spl Events/ Extra Name : " & Request.QueryString("SpleventName")
                                strSelectionFormula = strSelectionFormula & " and {spleventsmast.spleventname} LIKE '" & Request.QueryString("SpleventName") & "*'"
                            Else
                                strReportTitle = "Spl Events/ Extra Name : " & Request.QueryString("SpleventName")
                                strSelectionFormula = "{spleventsmast.spleventname} LIKE '" & Request.QueryString("SpleventName") & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SP Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {spleventsmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "SP Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{spleventsmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SP Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "SP Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptSpecialEventsorExtras.rpt"), String)
                        rptreportname = "Report - Special Events/Extras"
                        Exit Select

                    Case "Season"
                        If Request.QueryString("SeasonCode") <> "" Then
                            strReportTitle = "Season Code: " & Request.QueryString("SeasonCode")
                            strSelectionFormula = "{seasmast.seascode} LIKE '" & Request.QueryString("SeasonCode") & "*'"
                        End If
                        If Request.QueryString("SeasonName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Season Name: " & Request.QueryString("SeasonName")
                                strSelectionFormula = strSelectionFormula & " AND {seasmast.seasname} LIKE '" & Request.QueryString("SeasonName") & "*'"
                            Else
                                strReportTitle = "Season Name: " & Request.QueryString("SeasonName")
                                strSelectionFormula = "{seasmast.seasname} LIKE '" & Request.QueryString("SeasonName") & "*'"
                            End If
                        End If
                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({seasmast.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({seasmast.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({seasmast.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {seasmast.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({seasmast.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({seasmast.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({seasmast.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {seasmast.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSeasons.rpt"), String)
                        rptreportname = "Report - Seasons"
                        Exit Select

                    Case "Sub Season"
                        If Request.QueryString("SubSeasonCode") <> "" Then
                            strReportTitle = "Sub Season Code : " & Request.QueryString("SubSeasonCode")
                            strSelectionFormula = "{subseasmast.subseascode} LIKE '" & Request.QueryString("SubSeasonCode") & "*'"
                        End If
                        If Request.QueryString("SubSeasonName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sub Season Name : " & Request.QueryString("SubSeasonName")
                                strSelectionFormula = strSelectionFormula & " and {subseasmast.subseasname} LIKE '" & Request.QueryString("SubSeasonName") & "*'"
                            Else
                                strReportTitle = "Sub Season Name : " & Request.QueryString("SubSeasonName")
                                strSelectionFormula = "{subseasmast.subseasname} LIKE '" & Request.QueryString("SubSeasonName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSubSeasons.rpt"), String)
                        rptreportname = "Report - Sub Seasons"
                        Exit Select

                    Case "Cancellation Type"
                        If Request.QueryString("CanctypeCode") <> "" Then
                            strReportTitle = "Cancellation Type Code : " & Request.QueryString("CanctypeCode")
                            strSelectionFormula = "{cancellation_types.ctypecode} LIKE '" & Request.QueryString("CanctypeCode") & "*'"
                        End If
                        If Request.QueryString("CanctypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Cancellation Type Name : " & Request.QueryString("CanctypeName")
                                strSelectionFormula = strSelectionFormula & " and {cancellation_types.ctypename} LIKE '" & Request.QueryString("CanctypeName") & "*'"
                            Else
                                strReportTitle = "Cancellation Type Name : " & Request.QueryString("CanctypeName")
                                strSelectionFormula = "{cancellation_types.ctypename} LIKE '" & Request.QueryString("CanctypeName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCancellationTypes.rpt"), String)
                        rptreportname = "Report - Cancellation Types"
                        Exit Select

                    Case "Selling Price Type"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{sellmast.sellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Market : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = "{sellmast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If

                        End If
                        If Request.QueryString("DispName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; DisplayName: " & CType(Request.QueryString("DispName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.dispname} = '" & CType(Request.QueryString("DispName"), String) & "'"
                            Else
                                strReportTitle = " DisplayName: " & CType(Request.QueryString("DispName"), String)
                                strSelectionFormula = "{sellmast.dispname} = '" & CType(Request.QueryString("DispName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSellPriceType.rpt"), String)
                        rptreportname = "Report - Selling Price Type"
                        Exit Select

                    Case "Other Service Selling Type"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{othsellmast.othsellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        Dim default_group As String
                        default_group = ""
                        default_group = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1109", String))
                        If strSelectionFormula <> "" Then

                            strSelectionFormula = strSelectionFormula & " and {othsellmast.othertype}='" & default_group & "'"
                        Else

                            strSelectionFormula = "{othsellmast.othertype}='" & default_group & "' "
                        End If

                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesSellingTypes.rpt"), String)
                        rptreportname = "Report - Other Service Selling Types"
                        Exit Select
                    Case "VisaSellingTypesSearch"
                        If Request.QueryString("visasellcode") <> "" Then
                            strReportTitle = "Visa Selling Code : " & Request.QueryString("visasellcode")
                            strSelectionFormula = "{visasellmast.visasellcode} LIKE '" & Request.QueryString("visasellcode") & "*'"
                        End If
                        If Request.QueryString("visasellname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Visa Selling Name : " & Request.QueryString("visasellname")
                                strSelectionFormula = strSelectionFormula & " and {visasellmast.visasellname} LIKE '" & Request.QueryString("visasellname") & "*'"
                            Else
                                strReportTitle = "Visa Selling Name : " & Request.QueryString("visasellname")
                                strSelectionFormula = "{visasellmast.visasellname} LIKE '" & Request.QueryString("visasellname") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {visasellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{visasellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptvisaSellingTypesReport.rpt"), String)
                        rptreportname = "Report - Visa  Selling Types"
                        Exit Select

                    Case "TransferSellingTypesSearch"
                        If Request.QueryString("trfsellcode") <> "" Then
                            strReportTitle = "Transfer Selling Code : " & Request.QueryString("trfsellcode")
                            strSelectionFormula = "{trfsellmast.trfsellcode} LIKE '" & Request.QueryString("trfsellcode") & "*'"
                        End If
                        If Request.QueryString("trfsellname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Transfer Selling Name : " & Request.QueryString("trfsellname")
                                strSelectionFormula = strSelectionFormula & " and {trfsellmast.trfsellname} LIKE '" & Request.QueryString("trfsellname") & "*'"
                            Else
                                strReportTitle = "Transfer Selling Name : " & Request.QueryString("trfsellname")
                                strSelectionFormula = "{trfsellmast.trfsellname} LIKE '" & Request.QueryString("trfsellname") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {trfsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{trfsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rpttransferSellingTypes.rpt"), String)
                        rptreportname = "Report - Transfer  Selling Types"
                        Exit Select



                    Case "Handling Fees Selling Type"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{othsellmast.othsellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If


                        'If Request.QueryString("TypeValue") <> "" Then
                        Dim default_group As String
                        default_group = ""
                        default_group = objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1108", String))
                        If strSelectionFormula <> "" Then

                            strSelectionFormula = strSelectionFormula & " and {othsellmast.othertype}='" & default_group & "' "
                        Else

                            strSelectionFormula = "{othsellmast.othertype}='" & default_group & "'"
                        End If
                        'End If




                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency Code : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency Code: " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesSellingTypes.rpt"), String)
                        rptreportname = "Report - Handling FeesSelling Types"
                        Exit Select

                    Case "Complusary Remark"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Trans ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{compulsory_header.compulsoryid} = '" & Request.QueryString("TranID") & "'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Party Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {compulsory_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Party Code: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{compulsory_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If


                        If Request.QueryString("SupName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Party Code : " & CType(Request.QueryString("SupName"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            Else
                                strReportTitle = "Party Code: " & CType(Request.QueryString("SupName"), String)
                                strSelectionFormula = "{partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            End If
                        End If


                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market Code : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {compulsory_remarks_market.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market Code: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = "{compulsory_remarks_market.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {compulsory_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{compulsory_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SP Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "SP Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCompulsoryRemarks.rpt"), String)
                        rptreportname = "Report - Complusary Remarks"
                        Exit Select

                    Case "Minimum Nights"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Trans ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{minnights_header.minnights_id} = '" & Request.QueryString("TranID") & "'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Party Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {minnights_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Party Code: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{minnights_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If



                        If Request.QueryString("MktCode") <> "" Then
                            If Request.QueryString("MktCode") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Market Code : " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = strSelectionFormula & " and {minnights_market.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                Else
                                    strReportTitle = "Market Code: " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = "{minnights_market.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                End If
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {minnights_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{minnights_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; SP Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "SP Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptMinimumNights 1.rpt"), String)
                        rptreportname = "Report - Minimum Nights"
                        Exit Select

                    Case "Customer Category"
                        If Request.QueryString("CustCatCode") <> "" Then
                            strReportTitle = "Customer Category Code : " & Request.QueryString("CustCatCode")
                            strSelectionFormula = "{agentcatmast.agentcatcode} LIKE '" & Request.QueryString("CustCatCode") & "*'"
                        End If
                        If Request.QueryString("CustCatName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Customer Category Name : " & Request.QueryString("CustCatName")
                                strSelectionFormula = strSelectionFormula & " and {agentcatmast.agentcatname} LIKE '" & Request.QueryString("CustCatName") & "*'"
                            Else
                                strReportTitle = "Customer Category Name : " & Request.QueryString("CustCatName")
                                strSelectionFormula = "{agentcatmast.agentcatname} LIKE '" & Request.QueryString("CustCatName") & "*'"
                            End If
                        End If

                        'If Request.QueryString("SellCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Selling Type Code : " & CType(Request.QueryString("SellCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {agentcatmast.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Selling Type Code: " & CType(Request.QueryString("SellCode"), String)
                        '        strSelectionFormula = "{agentcatmast.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("SellName") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Selling Type Code : " & CType(Request.QueryString("SellName"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {sellmast.sellname} = '" & CType(Request.QueryString("SellName"), String) & "'"
                        '    Else
                        '        strReportTitle = "Selling Type Code: " & CType(Request.QueryString("SellName"), String)
                        '        strSelectionFormula = "{sellmast.sellname} = '" & CType(Request.QueryString("SellName"), String) & "'"
                        '    End If
                        'End If
                        strReportName = CType(Server.MapPath("~\Report\rptCustomerCategories.rpt"), String)
                        rptreportname = "Report - Customer Categories"
                        Exit Select

                    Case "Customer Sector"

                        strReportName = CType(Server.MapPath("~\Report\rptSect.rpt"), String)
                        rptreportname = "Report - Customer Sectors"
                        Exit Select

                    Case "Ticket Selling Type"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Ticket Sell Type Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{tktsellmast.tktsellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Ticket Sell Type Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {tktsellmast.tktsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Ticket Sell Type Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{tktsellmast.tktsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Currency: " & Request.QueryString("CurrCode")
                                strSelectionFormula = strSelectionFormula & " and {tktsellmast.currcode} = '" & Request.QueryString("CurrCode") & "'"
                            Else
                                strReportTitle = "Currency: " & Request.QueryString("CurrCode")
                                strSelectionFormula = "{tktsellmast.currcode} = '" & Request.QueryString("CurrCode") & "'"
                            End If
                        End If

                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market: " & Request.QueryString("MktCode")
                                strSelectionFormula = strSelectionFormula & " and {tktsellmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                            Else
                                strReportTitle = "Market: " & Request.QueryString("MktCode")
                                strSelectionFormula = "{tktsellmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptTicketSellingTypes.rpt"), String)
                        rptreportname = "Report - Ticket Selling Types"
                        Exit Select

                    Case ("Flight")
                        If Request.QueryString("flight_tranid") <> "" Then
                            strReportTitle = "FLIGHT  TRANID : " & Request.QueryString("flight_tranid")
                            strSelectionFormula = "{flightmast.flight_tranid} LIKE '" & Request.QueryString("flight_tranid") & "*'"

                        End If
                        If Request.QueryString("type") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; type: " & Request.QueryString("type")
                                strSelectionFormula = strSelectionFormula & " and {flightmast.type} LIKE '" & Request.QueryString("{flightmast.type") & "*'"
                            Else
                                strReportTitle = "type: " & Request.QueryString("type")
                                strSelectionFormula = "{flightmast.type} LIKE '" & Request.QueryString("Hotelstatusname") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\flightmast_new.rpt"), String)
                        rptreportname = "Report - Flight "
                        Exit Select



                        'Case "Flights Master"
                        '    If Request.QueryString("FlightNo") <> "" Then
                        '        strReportTitle = "Flight No. : " & Request.QueryString("FlightNo")
                        '        strSelectionFormula = "{sp_rep_flightmast;1.flightcode} LIKE '" & Request.QueryString("FlightNo") & "*'"
                        '    End If

                        '    '***********
                        '    If Request.QueryString("AirCode") <> "[Select]" Then
                        '        If strSelectionFormula <> "" Then
                        '            strReportTitle = strReportTitle & " Airline : " & Request.QueryString("AirCode")
                        '            strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.airlinecode} = '" & Request.QueryString("AirCode") & "'"
                        '        Else
                        '            strReportTitle = "Airline : " & Request.QueryString("AirCode")
                        '            strSelectionFormula = "{sp_rep_flightmast;1.airlinecode} = '" & Request.QueryString("AirCode") & "'"
                        '        End If
                        '    End If

                        '    'Arrival
                        '    If Request.QueryString("flighttype") <> "0" Then
                        '        If Request.QueryString("flighttype") = "1" Then
                        '            If strSelectionFormula <> "" Then
                        '                strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.flighttype} = 1"
                        '            Else
                        '                strSelectionFormula = "{sp_rep_flightmast;1.flighttype} = 1"
                        '            End If


                        '            If Request.QueryString("airborcode") <> "[Select]" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.airportbordercode} = '" + Request.QueryString("airborcode") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.airportbordercode} = '" + Request.QueryString("airborcode") + "'"
                        '                End If
                        '            End If

                        '            If Request.QueryString("CityArrival") <> "[Select]" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.city} = '" + Request.QueryString("CityArrival") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.city} = '" + Request.QueryString("CityArrival") + "'"
                        '                End If
                        '            End If

                        '            If Request.QueryString("Orginarr") <> "" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.airport} = '" + Request.QueryString("Orginarr") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.airport} = '" + Request.QueryString("Orginarr") + "'"
                        '                End If
                        '            End If


                        '        End If
                        '    End If

                        '    'Departure
                        '    If Request.QueryString("flighttype") <> "0" Then
                        '        If Request.QueryString("flighttype") = "2" Then
                        '            If strSelectionFormula <> "" Then
                        '                strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.flighttype} = 0"
                        '            Else
                        '                strSelectionFormula = "{sp_rep_flightmast;1.flighttype} = 0"
                        '            End If


                        '            If Request.QueryString("depairborcode") <> "[Select]" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.airportbordercode} = '" + Request.QueryString("depairborcode") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.airportbordercode} = '" + Request.QueryString("depairborcode") + "'"
                        '                End If
                        '            End If

                        '            If Request.QueryString("CityDeparture") <> "[Select]" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.city} = '" + Request.QueryString("CityDeparture") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.city} = '" + Request.QueryString("CityDeparture") + "'"
                        '                End If
                        '            End If

                        '            If Request.QueryString("Orgindep") <> "" Then
                        '                If strSelectionFormula <> "" Then
                        '                    strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.airport} = '" + Request.QueryString("Orgindep") + "'"
                        '                Else
                        '                    strSelectionFormula = "  {sp_rep_flightmast;1.airport} = '" + Request.QueryString("Orgindep") + "'"
                        '                End If
                        '            End If


                        '        End If
                        '    End If



                        '    If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                        '        If strSelectionFormula <> "" Then
                        '            strReportTitle = strReportTitle & " From Date: " & Request.QueryString("FromDate")
                        '            strReportTitle = strReportTitle & "To Date: " & Request.QueryString("ToDate")
                        '            strSelectionFormula = strSelectionFormula & " AND  ((datevalue({sp_rep_flightmast;1.frmdate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                        '            strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                        '            strSelectionFormula = strSelectionFormula & " OR (datevalue({sp_rep_flightmast;1.todate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                        '            strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                        '            strSelectionFormula = strSelectionFormula & "  OR (datevalue({sp_rep_flightmast;1.frmdate}) < Date('" & Request.QueryString("FromDate") & "') "
                        '            strSelectionFormula = strSelectionFormula & "  AND datevalue({sp_rep_flightmast;1.todate}) > Date('" & Request.QueryString("ToDate") & "'))) "
                        '        Else
                        '            strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                        '            strReportTitle = strReportTitle & " To Date: " & Request.QueryString("ToDate")
                        '            strSelectionFormula = strSelectionFormula & " ((datevalue({sp_rep_flightmast;1.frmdate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                        '            strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                        '            strSelectionFormula = strSelectionFormula & " OR (datevalue({sp_rep_flightmast;1.todate}) IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                        '            strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                        '            strSelectionFormula = strSelectionFormula & " OR (datevalue({sp_rep_flightmast;1.frmdate}) < Date('" & Request.QueryString("FromDate") & "') "
                        '            strSelectionFormula = strSelectionFormula & " AND datevalue({sp_rep_flightmast;1.todate}) > Date('" & Request.QueryString("ToDate") & "'))) "
                        '        End If
                        '    End If

                        '    'If Request.QueryString("active") <> "[Select]" Then
                        '    '    If strSelectionFormula <> "" Then
                        '    '        If Request.QueryString("active") = "0" Then
                        '    '            strReportTitle = strReportTitle & " ; All  flights : "                                 
                        '    '        ElseIf Request.QueryString("active") = "1" Then
                        '    '            strReportTitle = strReportTitle & " ; Inactive  flights : "
                        '    '            strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.active} = '0'"
                        '    '        ElseIf Request.QueryString("active") = "2" Then
                        '    '            strReportTitle = strReportTitle & " ; Active  flights : "
                        '    '            strSelectionFormula = strSelectionFormula & " and {sp_rep_flightmast;1.active} = '1'"
                        '    '        End If


                        '    '    Else
                        '    '        If Request.QueryString("active") = "0" Then
                        '    '            strReportTitle = strReportTitle & "All  flights : "
                        '    '        ElseIf Request.QueryString("active") = "1" Then
                        '    '            strReportTitle = strReportTitle & " ; Inactive  flights : "
                        '    '            strSelectionFormula = "  {sp_rep_flightmast;1.active} = '0'"
                        '    '        ElseIf Request.QueryString("active") = "2" Then
                        '    '            strReportTitle = strReportTitle & " ; Active  flights : "
                        '    '            strSelectionFormula = "  totext({sp_rep_flightmast;1.active}) = '1'"
                        '    '        End If
                        '    '    End If
                        '    'End If



                        '    strReportName = CType(Server.MapPath("~\Report\flightmast_new.rpt"), String)
                        '    rptreportname = "Report - Flights Masters"
                        '    Exit Select

                    Case "Flight Class Master"
                        If Request.QueryString("ClassCode") <> "" Then
                            strReportTitle = "Flight Class Code : " & Request.QueryString("ClassCode")
                            strSelectionFormula = "{flightclsmast.flightclscode} LIKE '" & Request.QueryString("ClassCode") & "*'"
                        End If
                        If Request.QueryString("ClassName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Flight Class Name : " & Request.QueryString("ClassName")
                                strSelectionFormula = strSelectionFormula & " and {flightclsmast.flightclsname} LIKE '" & Request.QueryString("ClassName") & "*'"
                            Else
                                strReportTitle = "Flight Class Name : " & Request.QueryString("ClassName")
                                strSelectionFormula = "{flightclsmast.flightclsname} LIKE '" & Request.QueryString("ClassName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptFlightClassMaster.rpt"), String)
                        rptreportname = "Report - Flight Class Masters"
                        Exit Select

                    Case "AirportBorder"
                        If Request.QueryString("ClassCode") <> "" Then
                            strReportTitle = "Airport Border Code : " & Request.QueryString("ClassCode")
                            strSelectionFormula = "{airportbordersmaster.airportbordercode} LIKE '" & Request.QueryString("ClassCode") & "*'"
                        End If
                        If Request.QueryString("ClassName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Airport Border Name : " & Request.QueryString("ClassName")
                                strSelectionFormula = strSelectionFormula & " and {airportbordersmaster.airportbordername} LIKE '" & Request.QueryString("ClassName") & "*'"
                            Else
                                strReportTitle = "Airport Border Name : " & Request.QueryString("ClassName")
                                strSelectionFormula = "{airportbordersmaster.airportbordername}  LIKE  '" & Request.QueryString("ClassName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptAirportBorderMaster.rpt"), String)
                        rptreportname = "Report - Airport Border Masters"
                        Exit Select

                    Case "Flight Sector Master"
                        If Request.QueryString("SectorCode") <> "" Then
                            strReportTitle = "Flight Sector Code : " & Request.QueryString("SectorCode")
                            strSelectionFormula = "{flightsectormast.flightsectorcode} LIKE '" & Request.QueryString("ClassCode") & "*'"
                        End If
                        If Request.QueryString("SectorName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Flight Sector Name : " & Request.QueryString("ClassName")
                                strSelectionFormula = strSelectionFormula & " and {flightsectormast.flightsectorname} LIKE '" & Request.QueryString("SectorName") & "*'"
                            Else
                                strReportTitle = "Flight Sector Name : " & Request.QueryString("SectorName")
                                strSelectionFormula = "{flightsectormast.flightsectorname} LIKE '" & Request.QueryString("SectorName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptFlightSectorMaster.rpt"), String)
                        rptreportname = "Report - Flight Sector Masters"
                        Exit Select

                    Case "Fare Type"
                        strReportName = CType(Server.MapPath("~\Report\rptFareTypes.rpt"), String)
                        rptreportname = "Report - Fare Types"
                        Exit Select

                    Case "Hotel Construction"
                        If Request.QueryString("ConstID") <> "" Then
                            strReportTitle = "Construction ID : " & Request.QueryString("ConstID")
                            strSelectionFormula = "{hotels_construction.constructionid} LIKE '" & Request.QueryString("ConstID") & "*'"
                        End If

                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} like'" & CType(Request.QueryString("SupCode"), String) & "*'"
                            Else
                                strReportTitle = "Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{partymast.partyname} like '" & CType(Request.QueryString("SupCode"), String) & "*'"
                            End If
                        End If

                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CityCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {citymast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            Else
                                strReportTitle = "City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = "{citymast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CatCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Code : " & CType(Request.QueryString("CatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {catmast.catcode} = '" & CType(Request.QueryString("CatCode"), String) & "'"
                            Else
                                strReportTitle = "Category Code : " & CType(Request.QueryString("CatCode"), String)
                                strSelectionFormula = "{catmast.catcode} = '" & CType(Request.QueryString("CatCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptHotelsConstruction.rpt"), String)
                        rptreportname = "Report - Hotel Constructions"
                        Exit Select
                    Case "Room Type"
                        If Request.QueryString("RmtypeCode") <> "" Then
                            strReportTitle = "Room Type Code : " & Request.QueryString("RmtypeCode")
                            strSelectionFormula = "{rmtypmast.rmtypcode} LIKE '" & Request.QueryString("RmtypeCode") & "*'"
                        End If
                        If Request.QueryString("RmtypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Room Type Name : " & Request.QueryString("RmtypeName")
                                strSelectionFormula = strSelectionFormula & " and {rmtypmast.rmtypname} LIKE '" & Request.QueryString("RmtypeName") & "*'"
                            Else
                                strReportTitle = "Room Type Name : " & Request.QueryString("RmtypeName")
                                strSelectionFormula = "{rmtypmast.rmtypname} LIKE '" & Request.QueryString("RmtypeName") & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; S.Type Code : " & CType(Request.QueryString("SuptypeCode").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {rmtypmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode").ToUpper, String) & "'"
                            Else
                                strReportTitle = "S.Type Code: " & CType(Request.QueryString("SuptypeCode").ToUpper, String)
                                strSelectionFormula = "{rmtypmast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode").ToUpper, String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; S.Type Name : " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                            Else
                                strReportTitle = "S.Type Name: " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptRoomTypes.rpt"), String)
                        rptreportname = "Report - Room Types"
                        Exit Select

                    Case "Room Category"
                        If Request.QueryString("RmcatCode") <> "" Then
                            strReportTitle = "Room Category Code : " & Request.QueryString("RmcatCode")
                            strSelectionFormula = "{rmcatmast.rmcatcode} LIKE '" & Request.QueryString("RmcatCode") & "*'"
                        End If
                        If Request.QueryString("Rmcatname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Room Category Name : " & Request.QueryString("Rmcatname")
                                strSelectionFormula = strSelectionFormula & " and {rmcatmast.rmcatname} LIKE '" & Request.QueryString("Rmcatname") & "*'"
                            Else
                                strReportTitle = "Room Category Name : " & Request.QueryString("Rmcatname")
                                strSelectionFormula = "{rmcatmast.rmcatname} LIKE '" & Request.QueryString("Rmcatname") & "*'"
                            End If
                        End If

                        If Request.QueryString("CategoryTypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Type : " & CType(Request.QueryString("CategoryTypeName").ToUpper, String)
                                strSelectionFormula = strSelectionFormula & " and {rmcatmast.accom_extra} = '" & CType(Request.QueryString("CategoryType").ToUpper, String) & "'"
                            Else
                                strReportTitle = "Category Type: " & CType(Request.QueryString("CategoryTypeName").ToUpper, String)
                                strSelectionFormula = "{rmcatmast.accom_extra} = '" & CType(Request.QueryString("CategoryType").ToUpper, String) & "'"
                            End If
                        End If

                        'If Request.QueryString("SuptypeName") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; S.Type Name : " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                        '        strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                        '    Else
                        '        strReportTitle = "S.Type Name: " & CType(Request.QueryString("SuptypeName").ToUpper, String)
                        '        strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName").ToUpper, String) & "'"
                        '    End If
                        'End If
                        If Request.QueryString("Typevalue") <> "" Then
                            If Request.QueryString("Typevalue") = "Acc" Then
                                rptreportname = "Report - Accommodation Categories"
                            ElseIf Request.QueryString("Typevalue") = "Supp" Then
                                rptreportname = "Report - Supplement Categories"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptRoomCategories.rpt"), String)
                        'rptreportname = "Report - Room Categories"
                        Exit Select

                    Case "General Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Trans Id : " & Request.QueryString("TranID")
                            strSelectionFormula = "{sparty_policy.tranid} LIKE '" & Request.QueryString("TranID") & "*'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sparty_policy.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Code: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{sparty_policy.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Name : " & CType(Request.QueryString("SupName"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Name: " & CType(Request.QueryString("SupName"), String)
                                strSelectionFormula = "{partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "" Then
                            If Request.QueryString("MktCode") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Market Code : " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = strSelectionFormula & " and {sparty_policy_market.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                Else
                                    strReportTitle = "Market Code: " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = "{sparty_policy_market.plgrpcode} ='" & CType(Request.QueryString("MktCode"), String) & "'"
                                End If
                            End If
                        End If

                        If Request.QueryString("MktName") <> "" Then
                            If Request.QueryString("MktName") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ;Market Name : " & CType(Request.QueryString("MktName"), String)
                                    strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} ='" & CType(Request.QueryString("MktName"), String) & "'"
                                Else
                                    strReportTitle = "Market Name: " & CType(Request.QueryString("MktName"), String)
                                    strSelectionFormula = "{plgrpmast.plgrpname} ='" & CType(Request.QueryString("MktName"), String) & "'"
                                End If
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sparty_policy.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Code: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{sparty_policy.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupagentName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Supplier Agent Name : " & CType(Request.QueryString("SupagentName"), String)
                                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & CType(Request.QueryString("SupagentName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Name: " & CType(Request.QueryString("SupagentName"), String)
                                strSelectionFormula = "{supplier_agents.supagentname} = '" & CType(Request.QueryString("SupagentName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptGe.rpt"), String)
                        rptreportname = "Report - General Policies"
                        Exit Select

                    Case "Max Accomodation"
                        If Request.QueryString("SupCode") <> "" Then
                            strReportTitle = "Supplier Code : " & Request.QueryString("SupCode")
                            strSelectionFormula = "{partymaxacc_header.partycode} LIKE '" & Request.QueryString("SupCode") & "*'"
                        End If
                        If Request.QueryString("SupName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Name : " & Request.QueryString("SupName")
                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} LIKE '" & Request.QueryString("SupName") & "*'"
                            Else
                                strReportTitle = "Supplier Name : " & Request.QueryString("SupName")
                                strSelectionFormula = "{partymast.partyname} LIKE '" & Request.QueryString("SupName") & "*'"
                            End If
                        End If

                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymaxacc_header.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{partymaxacc_header.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptMaxAccomodation.rpt"), String)
                        rptreportname = "Report -Max Occupancy"
                        Exit Select

                    Case "Customers"
                        If Request.QueryString("CustCode") <> "" Then
                            strReportTitle = "Customer Code : " & Request.QueryString("CustCode")
                            strSelectionFormula = "{agentmast.agentcode} LIKE '" & Request.QueryString("CustCode") & "*'"
                        End If
                        If Request.QueryString("CustName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Customer Name : " & Request.QueryString("CustName")
                                strSelectionFormula = strSelectionFormula & " and {agentmast.agentname} LIKE '" & Request.QueryString("CustName") & "*'"
                            Else
                                strReportTitle = "Customer Name : " & Request.QueryString("CustName")
                                strSelectionFormula = "{agentmast.agentname} LIKE '" & Request.QueryString("CustName") & "*'"
                            End If
                        End If

                        'If Request.QueryString("SellType") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Selling Type  : " & CType(Request.QueryString("SellType"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {sellmast.sellcode} = '" & CType(Request.QueryString("SellType"), String) & "'"
                        '    Else
                        '        strReportTitle = "Selling Type : " & CType(Request.QueryString("SellType"), String)
                        '        strSelectionFormula = "{sellmast.sellcode} = '" & CType(Request.QueryString("SellType"), String) & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("OthSellType") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Other Selling Type : " & CType(Request.QueryString("OthSellType"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {othsellmast.othsellcode} = '" & CType(Request.QueryString("OthSellType"), String) & "'"
                        '    Else
                        '        strReportTitle = "Other Selling Type : " & CType(Request.QueryString("OthSellType"), String)
                        '        strSelectionFormula = "{othsellmast.othsellcode} = '" & CType(Request.QueryString("OthSellType"), String) & "'"
                        '    End If
                        'End If
                        If Request.QueryString("TktSellType") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Ticket Selling Type : " & CType(Request.QueryString("TktSellType"), String)
                                strSelectionFormula = strSelectionFormula & " and {agentmast.tktsellcode} = '" & CType(Request.QueryString("TktSellType"), String) & "'"
                            Else
                                strReportTitle = "Ticket Selling Type : " & CType(Request.QueryString("TktSellType"), String)
                                strSelectionFormula = "{agentmast.tktsellcode} = '" & CType(Request.QueryString("TktSellType"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {agentmast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{agentmast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CityCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {agentmast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " ; City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = "{agentmast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If
                        'If Request.QueryString("MktCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Market : " & CType(Request.QueryString("MktCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Market : " & CType(Request.QueryString("MktCode"), String)
                        '        strSelectionFormula = "{plgrpmast.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                        '    End If
                        'End If
                        If Request.QueryString("CatCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category : " & CType(Request.QueryString("CatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {agentmast.catcode} = '" & CType(Request.QueryString("CatCode"), String) & "'"
                            Else
                                strReportTitle = "Category : " & CType(Request.QueryString("CatCode"), String)
                                strSelectionFormula = "{agentmast.catcode} = '" & CType(Request.QueryString("CatCode"), String) & "'"
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptCustomers.rpt"), String)
                        rptreportname = "Report - Customers"
                        Exit Select

                    Case "Block Full Sales"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Block Sale ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{blocksale_header.blocksaleid} LIKE '" & Request.QueryString("TranID") & "*'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {blocksale_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{blocksale_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Name: " & CType(Request.QueryString("SupName"), String)


                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Code : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{partymast.partyname} = '" & CType(Request.QueryString("SupName"), String) & "'"
                            End If
                        End If


                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {blocksale_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Code : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{blocksale_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupagentName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent Name : " & CType(Request.QueryString("SupagentName"), String)
                                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} = '" & CType(Request.QueryString("SupagentName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent Name : " & CType(Request.QueryString("SupagentName"), String)
                                strSelectionFormula = "{supplier_agents.supagentname} = '" & CType(Request.QueryString("SupagentName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market Code : " & CType(Request.QueryString("MktCode"), String)
                                ' strSelectionFormula = strSelectionFormula & " and {view_blocksale_header.markets} ='" & CType(Request.QueryString("MktCode"), String) & "'"
                                strSelectionFormula = strSelectionFormula & " and {blocksale_market.plgrpcode} ='" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market Code : " & CType(Request.QueryString("MktCode"), String)
                                ' strSelectionFormula = "{view_blocksale_header.markets} ='" & CType(Request.QueryString("MktCode"), String) & "'"
                                strSelectionFormula = strSelectionFormula & "  {blocksale_market.plgrpcode} ='" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        'If Request.QueryString("MktName") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Market Name : " & CType(Request.QueryString("MktName"), String)
                        '        'strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} = '" & CType(Request.QueryString("MktName"), String) & "'"
                        '    Else
                        '        strReportTitle = "Market Name : " & CType(Request.QueryString("MktName"), String)
                        '        strSelectionFormula = "{plgrpmast.plgrpname} = '" & CType(Request.QueryString("MktName"), String) & "'"
                        '    End If
                        'End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{sptypemast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptBlockFullSales.rpt"), String)
                        rptreportname = "Report - Block Full Sales"
                        Exit Select

                    Case "Supplier Agent"
                        If Request.QueryString("SupagentCode") <> "" Then
                            strReportTitle = "Agent Code : " & Request.QueryString("SupagentCode")
                            strSelectionFormula = "{supplier_agents.supagentcode} LIKE '" & Request.QueryString("SupagentCode") & "*'"
                        End If
                        If Request.QueryString("SupagentName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Agent Name : " & Request.QueryString("SupagentName")
                                strSelectionFormula = strSelectionFormula & " and {supplier_agents.supagentname} LIKE '" & Request.QueryString("SupagentName") & "*'"
                            Else
                                strReportTitle = "Agent Name : " & Request.QueryString("SupagentName")
                                strSelectionFormula = "{supplier_agents.supagentname} LIKE '" & Request.QueryString("SupagentName") & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {supplier_agents.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type  Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{supplier_agents.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupcatCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Code : " & CType(Request.QueryString("SupcatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {supplier_agents.catcode} = '" & CType(Request.QueryString("SupcatCode"), String) & "'"
                            Else
                                strReportTitle = "Category Code: " & CType(Request.QueryString("SupcatCode"), String)
                                strSelectionFormula = "{supplier_agents.catcode} = '" & CType(Request.QueryString("SupcatCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupcatName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Name : " & CType(Request.QueryString("SupcatName"), String)
                                strSelectionFormula = strSelectionFormula & " and {catmast.catname} = '" & CType(Request.QueryString("SupcatName"), String) & "'"
                            Else
                                strReportTitle = "Category Name: " & CType(Request.QueryString("SupcatName"), String)
                                strSelectionFormula = "{catmast.catname} = '" & CType(Request.QueryString("SupcatName"), String) & "'"
                            End If
                        End If

                        'If Request.QueryString("SellcatCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Selling Category Code : " & CType(Request.QueryString("SellcatCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {supplier_Agents.scatcode} = '" & CType(Request.QueryString("SellcatCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Selling Category Code: " & CType(Request.QueryString("SellcatCode"), String)
                        '        strSelectionFormula = "{supplier_Agents.scatcode} = '" & CType(Request.QueryString("SellcatCode"), String) & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("SellcatName") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Selling Category Name : " & CType(Request.QueryString("SellcatName"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {sellcatmast.scatname} = '" & CType(Request.QueryString("SellcatName"), String) & "'"
                        '    Else
                        '        strReportTitle = "Selling Category Name: " & CType(Request.QueryString("SellcatName"), String)
                        '        strSelectionFormula = "{sellcatmast.scatname} = '" & CType(Request.QueryString("SellcatName"), String) & "'"
                        '    End If
                        'End If
                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Code : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country Code: " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            Else
                                strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SectCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Code : " & CType(Request.QueryString("SectCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorcode} = '" & CType(Request.QueryString("SectCode"), String) & "'"
                            Else
                                strReportTitle = "Sector Code: " & CType(Request.QueryString("SectCode"), String)
                                strSelectionFormula = "{sectormaster.sectorcode} = '" & CType(Request.QueryString("SectCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SectName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Name : " & CType(Request.QueryString("SectName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorname} = '" & CType(Request.QueryString("SectName"), String) & "'"
                            Else
                                strReportTitle = "Sector Name: " & CType(Request.QueryString("SectName"), String)
                                strSelectionFormula = "{sectormaster.sectorname} = '" & CType(Request.QueryString("SectName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtrlaccCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Control Code : " & CType(Request.QueryString("CtrlaccCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {supplier_Agents.controlacctcode} = '" & CType(Request.QueryString("CtrlaccCode"), String) & "'"
                            Else
                                strReportTitle = "Control Code: " & CType(Request.QueryString("CtrlaccCode"), String)
                                strSelectionFormula = "{supplier_Agents.controlacctcode} = '" & CType(Request.QueryString("CtrlaccCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierAgents.rpt"), String)
                        rptreportname = "Report - Supplier Agents"
                        Exit Select

                    Case "Supplier"
                        If Request.QueryString("SupCode") <> "" Then
                            strReportTitle = "Supplier Code : " & Request.QueryString("SupCode")
                            strSelectionFormula = "{partymast.partycode} LIKE '" & Request.QueryString("SupCode") & "*'"
                        End If
                        If Request.QueryString("SupName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Name : " & Request.QueryString("SupName")
                                strSelectionFormula = strSelectionFormula & " and {partymast.partyname} LIKE '" & Request.QueryString("SupName") & "*'"
                            Else
                                strReportTitle = "Supplier Name : " & Request.QueryString("SupName")
                                strSelectionFormula = "{partymast.partyname} LIKE '" & Request.QueryString("SupName") & "*'"
                            End If
                        End If
                        If Request.QueryString("SuptypeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Code : " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type  Code: " & CType(Request.QueryString("SuptypeCode"), String)
                                strSelectionFormula = "{partymast.sptypecode} = '" & CType(Request.QueryString("SuptypeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SuptypeName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Type Name : " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            Else
                                strReportTitle = "Supplier Type Name: " & CType(Request.QueryString("SuptypeName"), String)
                                strSelectionFormula = "{sptypemast.sptypename} = '" & CType(Request.QueryString("SuptypeName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupCatCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Code : " & CType(Request.QueryString("SupCatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.catcode} = '" & CType(Request.QueryString("SupCatCode"), String) & "'"
                            Else
                                strReportTitle = "Category Code: " & CType(Request.QueryString("SupCatCode"), String)
                                strSelectionFormula = "{partymast.catcode} = '" & CType(Request.QueryString("SupCatCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SupCatName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category Name : " & CType(Request.QueryString("SupCatName"), String)
                                strSelectionFormula = strSelectionFormula & " and {catmast.catname} = '" & CType(Request.QueryString("SupCatName"), String) & "'"
                            Else
                                strReportTitle = "Category Name: " & CType(Request.QueryString("SupCatName"), String)
                                strSelectionFormula = "{catmast.catname} = '" & CType(Request.QueryString("SupCatName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SellcatCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Selling Category Code : " & CType(Request.QueryString("SellcatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.hotelchaincode} = '" & CType(Request.QueryString("SellcatCode"), String) & "'"
                            Else
                                strReportTitle = "Selling Category Code: " & CType(Request.QueryString("SellcatCode"), String)
                                strSelectionFormula = "{partymast.hotelchaincode} = '" & CType(Request.QueryString("SellcatCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SellcatName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Selling Category Name : " & CType(Request.QueryString("SellcatName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellcatmast.hotelchainname} = '" & CType(Request.QueryString("SellcatName"), String) & "'"
                            Else
                                strReportTitle = "Selling Category Name: " & CType(Request.QueryString("SellcatName"), String)
                                strSelectionFormula = "{sellcatmast.hotelchainname} = '" & CType(Request.QueryString("SellcatName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtryCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Code : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country Code: " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{ctrymast.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtryName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country Name : " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = strSelectionFormula & " and {ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            Else
                                strReportTitle = "Country Name: " & CType(Request.QueryString("CtryName"), String)
                                strSelectionFormula = "{ctrymast.ctryname} = '" & CType(Request.QueryString("CtryName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SectCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Code : " & CType(Request.QueryString("SectCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorcode} = '" & CType(Request.QueryString("SectCode"), String) & "'"
                            Else
                                strReportTitle = "Sector Code: " & CType(Request.QueryString("SectCode"), String)
                                strSelectionFormula = "{sectormaster.sectorcode} = '" & CType(Request.QueryString("SectCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SectName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sector Name : " & CType(Request.QueryString("SectName"), String)
                                strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorname} = '" & CType(Request.QueryString("SectName"), String) & "'"
                            Else
                                strReportTitle = "Sector Name: " & CType(Request.QueryString("SectName"), String)
                                strSelectionFormula = "{sectormaster.sectorname} = '" & CType(Request.QueryString("SectName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CityCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Code : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            Else
                                strReportTitle = "City Code: " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = "{partymast.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CityName") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City Name : " & CType(Request.QueryString("CityName"), String)
                                strSelectionFormula = strSelectionFormula & " and {citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                            Else
                                strReportTitle = "City Name: " & CType(Request.QueryString("CityName"), String)
                                strSelectionFormula = "{citymast.cityname} = '" & CType(Request.QueryString("CityName"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CtrlaccCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Control A/C Code : " & CType(Request.QueryString("CtrlaccCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {partymast.controlacctcode} = '" & CType(Request.QueryString("CtrlaccCode"), String) & "'"
                            Else
                                strReportTitle = "Control A/C Code: " & CType(Request.QueryString("CtrlaccCode"), String)
                                strSelectionFormula = "{partymast.controlacctcode} = '" & CType(Request.QueryString("CtrlaccCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSuppliers.rpt"), String)
                        rptreportname = "Report - Suppliers"
                        Exit Select

                    Case "SellingFormulaForSuppliersNew"
                        If Request.QueryString("Code") <> "" Then
                            strReportTitle = "Selling Code For Suppliers : " & Request.QueryString("Code")
                            'strSelectionFormula = "{sellsph.code} LIKE '" & Request.QueryString("Code") & "*'"
                        End If

                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier: " & CType(Request.QueryString("SupName"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier: " & CType(Request.QueryString("SupName"), String)
                                'strSelectionFormula = "{sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SellCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ;Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            Else
                                strReportTitle = "Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                ' strSelectionFormula = "{sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ;Market Code : " & CType(Request.QueryString("MktCode"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market Code : " & CType(Request.QueryString("MktCode"), String)
                                'strSelectionFormula = "{sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If



                        strReportName = CType(Server.MapPath("~\Report\rptSellingFormulasuppliers.rpt"), String)
                        rptreportname = "Report - Selling Formula for Suppliers"
                        Exit Select





                    Case "SellingFormulaForCategories"


                        If Request.QueryString("Code") <> "" Then
                            strReportTitle = "Selling Code For Categories : " & Request.QueryString("Code")
                            'strSelectionFormula = "{sellsph.code} LIKE '" & Request.QueryString("Code") & "*'"
                        End If


                        If Request.QueryString("SupCode") <> "" Then
                            If Request.QueryString("SupCode") <> "[Select]" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ; Supplier: " & CType(Request.QueryString("SupName"), String)
                                    'strSelectionFormula = strSelectionFormula & "and {sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                                Else
                                    strReportTitle = "Supplier: " & CType(Request.QueryString("SupName"), String)
                                    'strSelectionFormula = "{sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                                End If
                            End If
                        End If
                        If Request.QueryString("SellCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ;Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            Else
                                strReportTitle = "Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                ' strSelectionFormula = "{sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("MktCode") <> "" Then
                            If Request.QueryString("MktCode") <> "[Select]" Then
                                If strReportTitle <> "" Then
                                    strReportTitle = strReportTitle & " ;Market Code : " & CType(Request.QueryString("MktCode"), String)
                                    'strSelectionFormula = strSelectionFormula & "and {sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                Else
                                    strReportTitle = "Market Code : " & CType(Request.QueryString("MktCode"), String)
                                    'strSelectionFormula = "{sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                End If
                            End If
                        End If


                        strReportName = CType(Server.MapPath("~\Report\rptSellingFormulacategories.rpt"), String)
                        rptreportname = "Report - Selling Formula for Categories"
                        Exit Select









                    Case "Selling Price Formulas"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{sellformulas.sellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("Approved") <> "" Then

                            If Request.QueryString("Approved") <> "2" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & "; Status: " & CType(Request.QueryString("Status"), String)
                                    strSelectionFormula = strSelectionFormula & " and {view_sellformula.approved} = " & CType(Request.QueryString("Approved"), Integer) & ""
                                Else
                                    strReportTitle = "Status: " & CType(Request.QueryString("Status"), String)
                                    strSelectionFormula = "{view_sellformula.approved} = " & CType(Request.QueryString("Approved"), Integer) & ""
                                End If
                            End If
                        End If
                        'strReportName = CType(Server.MapPath("~\Report\rptSellingpriceformula.rpt"), String)
                        strReportName = CType(Server.MapPath("~\Report\rptsellingprice.rpt"), String)
                        rptreportname = "Report - Selling Price Formulas"
                        Exit Select

                    Case "Transfer Selling Formulas"
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{trf_sellformulas.sellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptTrfSellingFormula.rpt"), String)
                        rptreportname = "Report -Transfer Selling Formulas"
                        Exit Select

                    Case "Other Selling Formulas"
                        Dim strRptName As String = ""
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{oth_sellformulas.sellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{sellmast.sellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{sellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("GrpName") <> "[Select]" Then
                            strRptName = Request.QueryString("GrpName")
                        Else
                            strRptName = "Other"
                        End If
                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                If Session("QueryString") = "OTH" Then
                                    strReportTitle = strReportTitle & "; Group : " & CType(Request.QueryString("GrpCode"), String)
                                    '    strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA" & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""TRFS"""
                                    'Else

                                End If
                                strSelectionFormula = strSelectionFormula & " and {oth_sellformulas.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"

                            Else
                                If Session("QueryString") = "OTH" Then
                                    strReportTitle = "Group : " & CType(Request.QueryString("GrpCode"), String)
                                    '    strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA" & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""TRFS"""
                                    'Else

                                End If
                                strSelectionFormula = "{oth_sellformulas.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            End If
                        Else
                            'implies its "OTH" 
                            strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA""" & _
                                            " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    " And {oth_sellformulas.othgrpcode} <> ""TRFS""  And {oth_sellformulas.othgrpcode} <> ""ENTRANCE""  And {oth_sellformulas.othgrpcode} <> ""JEEPWADI"""
                        End If

                        ' strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""HFEES"

                        strReportName = CType(Server.MapPath("~\Report\rptOtherSellingFormula.rpt"), String)
                        rptreportname = "Report -" + strRptName + " Selling Formulas"
                        Exit Select





                    Case "HandlingFees Selling Formulas"
                        Dim strRptName As String = ""
                        If Request.QueryString("SellCode") <> "" Then
                            strReportTitle = "Selling Code : " & Request.QueryString("SellCode")
                            strSelectionFormula = "{oth_sellformulas.sellcode} LIKE '" & Request.QueryString("SellCode") & "*'"
                        End If
                        If Request.QueryString("SellName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            Else
                                strReportTitle = "Selling Name : " & Request.QueryString("SellName")
                                strSelectionFormula = "{othsellmast.othsellname} LIKE '" & Request.QueryString("SellName") & "*'"
                            End If
                        End If
                        If Request.QueryString("CurrCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            Else
                                strReportTitle = "Currency : " & CType(Request.QueryString("CurrCode"), String)
                                strSelectionFormula = "{othsellmast.currcode} = '" & CType(Request.QueryString("CurrCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("GrpName") <> "[Select]" Then
                            strRptName = Request.QueryString("GrpName")
                        Else
                            strRptName = "Other"
                        End If
                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                If Session("QueryString") = "OTH" Then
                                    strReportTitle = strReportTitle & "; Group : " & CType(Request.QueryString("GrpCode"), String)
                                    '    strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA" & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""TRFS"""
                                    'Else

                                End If
                                strSelectionFormula = strSelectionFormula & " and {oth_sellformulas.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"

                            Else
                                If Session("QueryString") = "OTH" Then
                                    strReportTitle = "Group : " & CType(Request.QueryString("GrpCode"), String)
                                    '    strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA" & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    '        " And {oth_sellformulas.othgrpcode} <> ""TRFS"""
                                    'Else

                                End If
                                strSelectionFormula = "{oth_sellformulas.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            End If
                        Else
                            'implies its "OTH" 
                            strSelectionFormula = strSelectionFormula & " {oth_sellformulas.othgrpcode} <> ""CAR RENTAL"" And {oth_sellformulas.othgrpcode}<> ""VISA""" & _
                                            " And {oth_sellformulas.othgrpcode} <> ""EXC"" And {oth_sellformulas.othgrpcode} <> ""MEALS"" And {oth_sellformulas.othgrpcode} <> ""GUIDES"" " & _
                                    " And {oth_sellformulas.othgrpcode} <> ""TRFS""  And {oth_sellformulas.othgrpcode} <> ""ENTRANCE""  And {oth_sellformulas.othgrpcode} <> ""JEEPWADI"""
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptHandlingSellingFormula.rpt"), String)
                        rptreportname = "Report -" + strRptName + " Selling Formulas"
                        Exit Select
                    Case ("HotelGroup")
                        If Request.QueryString("HotelGroupCode") <> "" Then
                            strReportTitle = "HotelGroup Code : " & Request.QueryString("HotelgroupCode")
                            strSelectionFormula = "{Hotelgroup.Hotelgroupcode} LIKE '" & Request.QueryString("HotelgroupCode") & "*'"
                        End If
                        If Request.QueryString("HotelgroupName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Hotel  Group: " & Request.QueryString("HotelgroupName")
                                strSelectionFormula = strSelectionFormula & " and {Hotelgroup.Hotelgroupname} LIKE '" & Request.QueryString("HotelgroupName") & "*'"
                            Else
                                strReportTitle = "Hotel Group : " & Request.QueryString("HotelstatusName")
                                strSelectionFormula = "{hotelHotelgroup.hotelgroupname} LIKE '" & Request.QueryString("Hotelgroupname") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rpthotelgroup.rpt"), String)
                        rptreportname = "Report - Hotel Group"
                        Exit Select

                    Case "Cancellation Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{cancel_header.tranid} LIKE '" & Request.QueryString("TranID") & "*'"
                        End If


                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {cancel_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "; Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{cancel_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {cancel_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "; Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{cancel_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SubSeasCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Sub Season: " & CType(Request.QueryString("SubSeasCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {cancel_header.subseascode} = '" & CType(Request.QueryString("SubSeasCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "; Sub Season: " & CType(Request.QueryString("SubSeasCode"), String)
                                strSelectionFormula = "{cancel_header.subseascode} = '" & CType(Request.QueryString("SubSeasCode"), String) & "'"
                            End If
                        End If



                        If Request.QueryString("MktCode") <> "" Then
                            If Request.QueryString("MktCode") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Market Code : " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = strSelectionFormula & " and {subcanmarket_detail.marketcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                Else
                                    strReportTitle = "Market Code: " & CType(Request.QueryString("MktCode"), String)
                                    strSelectionFormula = "{subcanmarket_detail.marketcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                                End If
                            End If
                        End If

                        If Request.QueryString("MktName") <> "" Then
                            If Request.QueryString("MktName") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ;Market Name : " & CType(Request.QueryString("MktName"), String)
                                    strSelectionFormula = strSelectionFormula & " and {plgrpmast.plgrpname} like '" & CType(Request.QueryString("MktName"), String) & "'"
                                Else
                                    strReportTitle = "Market Name: " & CType(Request.QueryString("MktName"), String)
                                    strSelectionFormula = "{plgrpmast.plgrpname} like '" & CType(Request.QueryString("MktName"), String) & "'"
                                End If
                            End If
                        End If
                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & "; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({cancel_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({cancel_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({cancel_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {cancel_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & "; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({cancel_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({cancel_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({cancel_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {cancel_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptcancellationplcy.rpt"), String)
                        rptreportname = "Report - Cancellation Policies"
                        Exit Select

                    Case "Child Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{child_header.tranid} LIKE '" & Request.QueryString("TranID") & "*'"
                        End If

                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "  Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & "and {child_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{child_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "  Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & "and {child_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{child_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("market") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " Markets: " & CType(Request.QueryString("Market"), String)
                                strSelectionFormula = strSelectionFormula & "and {subchildmarket_detail.marketcode} = '" & CType(Request.QueryString("market"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " Markets: " & CType(Request.QueryString("Market"), String)
                                strSelectionFormula = "{subchildmarket_detail.marketcode} = '" & CType(Request.QueryString("market"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({child_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {child_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({child_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {child_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptchildpolicy.rpt"), String)
                        rptreportname = "Report - Child Policies"
                        Exit Select

                    Case "Child PolicyNew"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{child_header.tranid} LIKE '" & Request.QueryString("TranID") & "*'"
                        End If

                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "  Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & "and {child_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{child_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "  Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & "and {child_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  Supplier Agent: " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{child_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("market") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " Markets: " & CType(Request.QueryString("Market"), String)
                                strSelectionFormula = strSelectionFormula & "and {subchildmarket_detail.marketcode} = '" & CType(Request.QueryString("market"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " Markets: " & CType(Request.QueryString("Market"), String)
                                strSelectionFormula = "{subchildmarket_detail.marketcode} = '" & CType(Request.QueryString("market"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({child_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {child_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({child_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({child_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {child_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If


                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptChildPolicyNew.rpt"), String)
                        rptreportname = "Report - Child Policies"
                        Exit Select

                    Case "SellingFormulaforSuppliers"
                        If Request.QueryString("Code") <> "" Then
                            strReportTitle = "Selling Code For Suppliers : " & Request.QueryString("Code")
                            'strSelectionFormula = "{sellsph.code} LIKE '" & Request.QueryString("Code") & "*'"
                        End If

                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier: " & CType(Request.QueryString("SupName"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier: " & CType(Request.QueryString("SupName"), String)
                                'strSelectionFormula = "{sellsph.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("SellCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ;Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            Else
                                strReportTitle = "Sell Code : " & CType(Request.QueryString("SellCode"), String)
                                ' strSelectionFormula = "{sellsph.sellcode} = '" & CType(Request.QueryString("SellCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strReportTitle <> "" Then
                                strReportTitle = strReportTitle & " ;Market Code : " & CType(Request.QueryString("MktCode"), String)
                                'strSelectionFormula = strSelectionFormula & "and {sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market Code : " & CType(Request.QueryString("MktCode"), String)
                                'strSelectionFormula = "{sellsph.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptSellingFormulasuppliers.rpt"), String)
                        rptreportname = "Report - Selling Formula for Suppliers"
                        Exit Select

                    Case "Promotion"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Promotion ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{promotion_header.promotionid} = '" & Request.QueryString("TranID") & "'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {promotion_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{promotion_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {promotion_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{promotion_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {promotionmarket_detail.marketcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = "{promotionmarket_detail.marketcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("promotionname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Promotion Name : " & CType(Request.QueryString("promotionname"), String)
                                strSelectionFormula = strSelectionFormula & " and {promotion_header.pricecode} = '" & CType(Request.QueryString("promotionname"), String) & "'"
                            Else
                                strReportTitle = "Promotion Name : " & CType(Request.QueryString("promotionname"), String)
                                strSelectionFormula = "{promotion_header.pricecode} = '" & CType(Request.QueryString("promotionname"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("Hotelpromoname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Hotel Promotion Name: " & CType(Request.QueryString("Hotelpromoname"), String)
                                strSelectionFormula = strSelectionFormula & " and {promotion_header.hotelpromcode} = '" & CType(Request.QueryString("Hotelpromoname"), String) & "'"
                            Else
                                strReportTitle = "Hotel Promotion Name : " & CType(Request.QueryString("Hotelpromoname"), String)
                                strSelectionFormula = "{promotion_header.hotelpromcode} = '" & CType(Request.QueryString("Hotelpromoname"), String) & "'"
                            End If
                        End If








                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({promotion_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({promotion_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({promotion_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {promotion_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({promotion_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({promotion_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({promotion_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {promotion_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptpromotions1.rpt"), String)
                        rptreportname = "Report - Promotions"
                        Exit Select

                    Case "Early Promotion"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Early Promotion ID : " & Request.QueryString("TranID")
                            strSelectionFormula = "{earlypromotion_header.earlypromotionid} = '" & Request.QueryString("TranID") & "'"
                        End If
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {earlypromotion_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier : " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = "{earlypromotion_header.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Supplier Agent : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {earlypromotion_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            Else
                                strReportTitle = "Supplier Agent : " & CType(Request.QueryString("SupagentCode"), String)
                                strSelectionFormula = "{earlypromotion_header.supagentcode} = '" & CType(Request.QueryString("SupagentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {earlypromotion_header.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = "{earlypromotion_header.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("FromDate") <> "" And Request.QueryString("ToDate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " AND (({earlypromotion_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({earlypromotion_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({earlypromotion_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {earlypromotion_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "From Date: " & Request.QueryString("FromDate")
                                strReportTitle = strReportTitle & " ; To Date: " & Request.QueryString("ToDate")
                                strSelectionFormula = strSelectionFormula & " (({earlypromotion_header.frmdate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({earlypromotion_header.todate} IN Date('" & Format(CType(Request.QueryString("FromDate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd 00:00:00 ") & "')) "
                                strSelectionFormula = strSelectionFormula & " OR ({earlypromotion_header.frmdate} < Date('" & Request.QueryString("FromDate") & "') "
                                strSelectionFormula = strSelectionFormula & " AND {earlypromotion_header.todate} > Date('" & Request.QueryString("ToDate") & "'))) "
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptearlybirdpromotion.rpt"), String)
                        rptreportname = "Report - Early Bird Promotions"
                        Exit Select
                    Case "Currency Conversion Rates"

                        If Request.QueryString("FromCurr") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Currency : " & Request.QueryString("FromCurr")

                                'repfilter = "From Currency:" & Request.QueryString("FromCurr")
                                strSelectionFormula = strSelectionFormula & " and {currrates.currcode} = '" & Request.QueryString("FromCurr") & "'"
                            Else
                                strReportTitle = "From Currency : " & Request.QueryString("FromCurr")
                                'repfilter = "From Currency:" & Request.QueryString("FromCurr")
                                strSelectionFormula = "{currrates.currcode} = '" & Request.QueryString("FromCurr") & "'"
                            End If

                        End If

                        If Request.QueryString("ToCurr") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; To Currency : " & Request.QueryString("ToCurr")
                                'repfilter = repfilter & ",To Currency :" & Request.QueryString("ToCurr")
                                strSelectionFormula = strSelectionFormula & " and {currrates.tocurr} = '" & Request.QueryString("ToCurr") & "'"
                            Else
                                strReportTitle = "To Currency : " & Request.QueryString("ToCurr")
                                'repfilter = repfilter & ",To Currency :" & Request.QueryString("ToCurr")
                                strSelectionFormula = "{currrates.tocurr} = '" & Request.QueryString("ToCurr") & "'"
                            End If

                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptcurrencyconversionrates.rpt"), String)
                        rptreportname = "Report - Currency Conversion Rates"
                        Exit Select
                    Case "Competitor Rates"
                        strReportName = CType(Server.MapPath("~\Report\rptCompetitorRates.rpt"), String)
                        rptreportname = "Report - Competitors Rates"
                        Exit Select

                    Case "ChildMealMapping"
                        strReportName = CType(Server.MapPath("~\Report\rptChildmealMapping.rpt"), String)
                        rptreportname = "Report - Child Meal Mapping"
                        Exit Select
                    Case "Other Services Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction Code: " & Request.QueryString("TranID")
                            strSelectionFormula = " {othserv_policy.tranid} = '" & UCase(Request.QueryString("TranID")) & "'"
                        End If
                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "  Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            Else
                                strReportTitle = "Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = " {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " Market  : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = " {othserv_policy.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesPolicy.rpt"), String)
                        rptreportname = "Report - Other Services Policy"
                        Exit Select

                    Case "Transfers Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction Code: " & Request.QueryString("TranID")
                            strSelectionFormula = " {othserv_policy.tranid} = '" & UCase(Request.QueryString("TranID")) & "'"
                        End If
                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                'strReportTitle = strReportTitle & "  Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            Else
                                'strReportTitle = "Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = " {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " Market  : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy_markets.plgrpcode}=  '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = " {othserv_policy_markets.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptTransfersPolicy.rpt"), String)
                        rptreportname = "Report - Transfers Policy"
                        Exit Select

                    Case "Other Service Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction Code: " & Request.QueryString("TranID")
                            strSelectionFormula = " {othserv_policy.tranid} = '" & UCase(Request.QueryString("TranID")) & "'"
                        End If

                        Dim rptName As String = ""
                        If Request.QueryString("GrpName") <> "[Select]" Then
                            rptName = Request.QueryString("GrpName")
                        Else
                            rptName = "Other Service Policy"
                        End If



                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                'strReportTitle = strReportTitle & "  Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            Else
                                'strReportTitle = "Group : " & CType(Request.QueryString("GrpCode"), String)
                                strSelectionFormula = " {othserv_policy.othgrpcode} = '" & CType(Request.QueryString("GrpCode"), String) & "'"
                            End If
                        Else
                            If strSelectionFormula <> "" Then
                                strSelectionFormula = strSelectionFormula & "and  {othserv_policy.othgrpcode} <> ""CAR RENTAL"" And {othserv_policy.othgrpcode}<> ""VISA""" & _
                                            " And {othserv_policy.othgrpcode} <> ""EXC"" And {othserv_policy.othgrpcode} <> ""MEALS"" And {othserv_policy.othgrpcode} <> ""GUIDES"" " & _
                                    " And {othserv_policy.othgrpcode} <> ""TRFS""  And {othserv_policy.othgrpcode} <> ""ENTRANCE""  And {othserv_policy.othgrpcode} <> ""JEEPWADI"""
                            Else
                                strSelectionFormula = strSelectionFormula & " {othserv_policy.othgrpcode} <> ""CAR RENTAL"" And {othserv_policy.othgrpcode}<> ""VISA""" & _
                                            " And {othserv_policy.othgrpcode} <> ""EXC"" And {othserv_policy.othgrpcode} <> ""MEALS"" And {othserv_policy.othgrpcode} <> ""GUIDES"" " & _
                                    " And {othserv_policy.othgrpcode} <> ""TRFS""  And {othserv_policy.othgrpcode} <> ""ENTRANCE""  And {othserv_policy.othgrpcode} <> ""JEEPWADI"""
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " Market  : " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {othserv_policy_markets.plgrpcode}=  '" & CType(Request.QueryString("MktCode"), String) & "'"
                            Else
                                strReportTitle = "Market: " & CType(Request.QueryString("MktCode"), String)
                                strSelectionFormula = " {othserv_policy_markets.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptTransfersPolicy.rpt"), String)
                        rptreportname = "Report - " + rptName
                        Exit Select

                    Case "Ticketing Policy"
                        If Request.QueryString("TranID") <> "" Then
                            strReportTitle = "Transaction Code: " & Request.QueryString("TranID")
                            strSelectionFormula = " { ticket_policy.tranid} = '" & UCase(Request.QueryString("TranID")) & "'"
                        End If
                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Airline  : " & Request.QueryString("GrpCode")
                                strSelectionFormula = strSelectionFormula & " and {ticket_policy.airlinecode} = '" & Request.QueryString("GrpCode") & "'"
                            Else
                                strReportTitle = "Airline : " & Request.QueryString("GrpCode")
                                strSelectionFormula = " {ticket_policy.airlinecode} = '" & Request.QueryString("GrpCode") & "'"
                            End If
                        End If
                        If Request.QueryString("MktCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Market  : " & Request.QueryString("MktCode")
                                strSelectionFormula = strSelectionFormula & " and {ticket_policy.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                            Else
                                strReportTitle = "Market: " & Request.QueryString("MktCode")
                                strSelectionFormula = " {ticket_policy.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                            End If
                        End If
                        If Request.QueryString("SupagentCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & "; Supplier Agent  : " & Request.QueryString("SupagentCode")
                                strSelectionFormula = strSelectionFormula & " and {ticket_policy.supagentcode} = '" & Request.QueryString("SupagentCode") & "'"
                            Else
                                strReportTitle = "Supplier Agent: " & Request.QueryString("SupagentCode")
                                strSelectionFormula = " {ticket_policy.supagentcode} = '" & Request.QueryString("SupagentCode") & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptTicketingPolicy.rpt"), String)
                        rptreportname = "Report - Ticketing Policy"
                        Exit Select

                    Case "Other Services Price List"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesPriceList.rpt"), String)
                        rptreportname = "Report - Other Services Price List"
                        Exit Select

                    Case "CustomerSector"
                        If Request.QueryString("SectCode") <> "" Then
                            strReportTitle = "Customer Sector Code : " & Request.QueryString("SectCode")
                            strSelectionFormula = "{agent_sectormaster.sectorcode} LIKE '" & Request.QueryString("SectCode") & "*'"
                        End If
                        If Request.QueryString("SectName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Customer Sector Name : " & Request.QueryString("SectName")
                                strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.sectorname} LIKE '" & Request.QueryString("SectName") & "*'"
                            Else
                                strReportTitle = "Customer Sector Name : " & Request.QueryString("SectName")
                                strSelectionFormula = "{agent_sectormaster.sectorname} LIKE '" & Request.QueryString("SectName") & "*'"
                            End If
                        End If

                        'If Request.QueryString("CtryCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Country : " & CType(Request.QueryString("CtryCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Country : " & CType(Request.QueryString("CtryCode"), String)
                        '        strSelectionFormula = "{agent_sectormaster.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                        '    End If
                        'End If

                        'If Request.QueryString("MktCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Market : " & CType(Request.QueryString("MktCode"), String)
                        '        strSelectionFormula = strSelectionFormula & " and {agent_sectormaster.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                        '    Else
                        '        strReportTitle = "Market : " & CType(Request.QueryString("MktCode"), String)
                        '        strSelectionFormula = "{agent_sectormaster.plgrpcode} = '" & CType(Request.QueryString("MktCode"), String) & "'"
                        '    End If
                        'End If
                        strReportName = CType(Server.MapPath("~\Report\rptCustomerSector.rpt"), String)
                        rptreportname = "Report - Customer Sector"
                        Exit Select
                    Case ("CustomerGroup")
                        If Request.QueryString("customergroupcode") <> "" Then
                            strReportTitle = "CustomerGroup Code : " & Request.QueryString("customergroupcode")
                            strSelectionFormula = "{customergroup.customergroupcode} LIKE '" & Request.QueryString("customergroupCode") & "*'"
                        End If
                        If Request.QueryString("customergroupname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Customer  Group: " & Request.QueryString("customergroupname")
                                strSelectionFormula = strSelectionFormula & " and {customergroup.customergroupname} LIKE '" & Request.QueryString("customergroupname") & "*'"
                            Else
                                strReportTitle = "Customer Group : " & Request.QueryString("customergroupname")
                                strSelectionFormula = "{customergroup.customergroupname} LIKE '" & Request.QueryString("customergroup") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptcustomergroup.rpt"), String)
                        rptreportname = "Report -Customer Group"
                        Exit Select

                    Case "CountryGroups"
                        If Request.QueryString("countrygroupcode") <> "" Then
                            strReportTitle = "CountryGroup Code : " & Request.QueryString("countrygroupcode")
                            strSelectionFormula = "{countrygroup.countrygroupcode} LIKE '" & Request.QueryString("countrygroupcode") & "*'"
                        End If
                        If Request.QueryString("countrygroupname") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Countrygroup Name: " & Request.QueryString("countrygroupname")
                                strSelectionFormula = strSelectionFormula & " and {countrygroup.countrygroupname} LIKE '" & Request.QueryString("countrygroupname") & "*'"
                            Else
                                strReportTitle = "CountryGroup Name : " & Request.QueryString("countrygroupname")
                                strSelectionFormula = "{countrygroup.countrygroupname} LIKE '" & Request.QueryString("countrygroup") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptcountrygroup.rpt"), String)
                        rptreportname = "Report -Country Group"
                        Exit Select


                    Case "Compliment"

                        If Request.QueryString("Code") <> "" Then
                            strReportTitle = "Compliment Code : " & Request.QueryString("Code")
                            strSelectionFormula = "{complgroup_master.complgroupcode} LIKE '" & Request.QueryString("Code") & "*'"
                        End If
                        If Request.QueryString("Name") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Compliment Name : " & Request.QueryString("Name")
                                strSelectionFormula = strSelectionFormula & " and {complgroup_master.complgroupname} LIKE '" & Request.QueryString("Name") & "*'"
                            Else
                                strReportTitle = "Compliment Name : " & Request.QueryString("Name")
                                strSelectionFormula = "{complgroup_master.complgroupname} LIKE '" & Request.QueryString("Name") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptComplimentGroup.rpt"), String)
                        rptreportname = "Report - Compliments"
                        Exit Select
                        ' Admin Module
                    Case "Define Group"
                        strReportName = CType(Server.MapPath("~\Report\rptUserGroups.rpt"), String)
                        rptreportname = "Report - Define User Groups"
                        Exit Select
                    Case "Application Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupApplicationRights.rpt"), String)
                        rptreportname = "Report - Application Rights for User Groups"
                        Exit Select
                    Case "Group Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupMenuRights.rpt"), String)
                        rptreportname = "Report - Group Rights for Menus"
                        Exit Select

                    Case "Department Master"
                        If Request.QueryString("DeptCode") <> "" Then
                            strReportTitle = "Department Code: " & Request.QueryString("DeptCode")
                            'strSelectionFormula = "{nationality_master.Nationalitycode} LIKE '" & Request.QueryString("Nationalitycode") & "*'"
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptDeptMaster2.rpt"), String)
                        rptreportname = "Report -  Master"
                        Exit Select
                    Case "User Master"
                        strReportName = CType(Server.MapPath("~\Report\rptUserMaster.rpt"), String)
                        rptreportname = "Report - User Master"
                        Exit Select
                    Case "Privilege Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupPrivilegeRights.rpt"), String)
                        rptreportname = "Report - Privilege Rights"
                        Exit Select
                        ' Accounts
                    Case "Account Group"
                        strReportName = CType(Server.MapPath("~\Report\rptAcctGroup.rpt"), String)
                        rptreportname = "Report - Account Group Master"
                        Exit Select
                    Case "Bank Type"
                        strReportName = CType(Server.MapPath("~\Report\bank_master_type.rpt"), String)
                        rptreportname = "Report - Bank Master Type"
                        Exit Select
                    Case "Profit Center Master"
                        strReportName = CType(Server.MapPath("~\Report\rptProfitCenterMaster.rpt"), String)
                        rptreportname = "Report - Profit Center Master"
                        Exit Select
                    Case "Cost Center Group"
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterGroupMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Group Master"
                        Exit Select
                    Case "Cost Center"
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Master"
                        Exit Select
                    Case "Narration Template"
                        strReportName = CType(Server.MapPath("`~\Report\rptNarrationTemplate.rpt"), String)
                        rptreportname = "Report - Narration Template"
                        Exit Select
                    Case "OpeningTrailBalance"
                        strReportName = CType(Server.MapPath("~\Report\rptOpeningTrialbalance.rpt"), String)
                        rptreportname = "Report - Opening Trail Balance"
                        Exit Select
                    Case "SupplierOpeningTrailBalance"
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierOpeningbalance.rpt"), String)
                        If Session("OpenType") = "S" Then
                            rptreportname = "Report - Supplier Opening  Balance"
                        ElseIf Session("OpenType") = "C" Then
                            rptreportname = "Report - Customer Opening  Balance"
                        ElseIf Session("OpenType") = "A" Then
                            rptreportname = "Report - Supplier Agent Opening Balance"
                        End If
                    Case "DefineControlAccountsSupplierAgents"
                        strReportName = CType(Server.MapPath("~\Report\rptDefAccSupplieragent.rpt"), String)
                        rptreportname = "Report - SupplierAgents Control Accounts "
                        Exit Select
                    Case "DebitNoteDoc"
                        strReportName = CType(Server.MapPath("~\Report\rptDebitNoteDoc.rpt"), String)
                        If Session("CNDNOpen_type") = "DN" Then
                            rptreportname = "Report - Debit Note"
                        ElseIf Session("CNDNOpen_type") = "CN" Then
                            rptreportname = "Report - Credit Note"
                        End If
                    Case "DebitNoteBrief"
                        strReportName = CType(Server.MapPath("~\Report\rptDebitNoteBrief.rpt"), String)
                        If Session("CNDNOpen_type") = "DN" Then
                            rptreportname = "Report - Debit Note"
                        ElseIf Session("CNDNOpen_type") = "CN" Then
                            rptreportname = "Report - Credit Note"
                        End If
                    Case "DebitNoteDetail"
                        strReportName = CType(Server.MapPath("~\Report\rptDebitNoteDetail.rpt"), String)
                        If Session("CNDNOpen_type") = "DN" Then
                            rptreportname = "Report - Debit Note"
                        ElseIf Session("CNDNOpen_type") = "CN" Then
                            rptreportname = "Report - Credit Note"
                        End If
                    Case "Approve"
                        strReportName = CType(Server.MapPath("~\Report\Approve.rpt"), String)
                        rptreportname = "Report - Approval Required for the following Entries"
                        Exit Select
                    Case "VisaApplied"
                        strReportName = CType(Server.MapPath("~\Report\rptVisaApplied.rpt"), String)
                        rptreportname = "Report - Applied Visa Detail."
                        strReportTitle = " Applied Visa Detail."
                        Exit Select
                    Case "CommissionFormula"
                        strReportName = CType(Server.MapPath("~\Report\rptCommissionFormula.rpt"), String)
                        rptreportname = "Report - Commission Formula"
                        strReportTitle = ""
                        Exit Select
                    Case "MarkupFormula"
                        strReportName = CType(Server.MapPath("~\Report\rptMarkupFormula.rpt"), String)
                        rptreportname = "Report - Markup Formula"
                        strReportTitle = ""
                        Exit Select

                    Case "MinMarkup"
                        strReportName = CType(Server.MapPath("~\Report\rptMinMarkupAndDiscount.rpt"), String)
                        rptreportname = "Report - Minimum Markup and Discount"
                        strReportTitle = ""
                        Exit Select

                    Case "VisaOnArrival"
                        strReportName = CType(Server.MapPath("~\Report\rptVisaOnArrival.rpt"), String)
                        rptreportname = "Report - Visa On Arrival Countries"
                        strReportTitle = ""
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue

        'If Session("Rep") Is Nothing Then
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


        repDeocument.Load(ReportName)

        Me.CRVReport.ReportSource = repDeocument
        Dim RepTbls As Tables = repDeocument.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        If CType(ViewState("Pageame"), String) <> "Supplier" And CType(ViewState("Pageame"), String) <> "Country" And CType(ViewState("Pageame"), String) <> "Sector" And CType(ViewState("Pageame"), String) <> "Location" And CType(ViewState("Pageame"), String) <> "Supplier Category" And CType(ViewState("Pageame"), String) <> "Supplier Selling Category" And CType(ViewState("Pageame"), String) <> "General Policy" And CType(ViewState("Pageame"), String) <> "Supplier Agent" And CType(ViewState("Pageame"), String) <> "Room Category" And CType(ViewState("Pageame"), String) <> "Hotel Construction" And CType(ViewState("Pageame"), String) <> "Cancellation Policy" And CType(ViewState("Pageame"), String) <> "Block Full Sales" And CType(ViewState("Pageame"), String) <> "Complusary Remark" And CType(ViewState("Pageame"), String) <> "Promotion" And CType(ViewState("Pageame"), String) <> "Customers" And CType(ViewState("Pageame"), String) <> "SectorGroup" And CType(ViewState("Pageame"), String) <> "Other Service Group" And CType(ViewState("Pageame"), String) <> "Child PolicyNew" Then

            '
            repDeocument.SummaryInfo.ReportTitle = strReportTitle 'was giving error in 'supplier' , for long texts , so added a parm for the same
        End If

        'Added "Location" by Archana on 18/05/2015 for menu Location Master 
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



        'pname = pnames.Item("repfilter")
        'paramvalue.Value = strReportTitle
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        If CType(ViewState("Pageame"), String) = "Country" Or CType(ViewState("Pageame"), String) = "Sector" Or CType(ViewState("Pageame"), String) = "Location" Or CType(ViewState("Pageame"), String) = "Supplier Category" Or CType(ViewState("Pageame"), String) = "Supplier Selling Category" Or CType(ViewState("Pageame"), String) = "General Policy" Or CType(ViewState("Pageame"), String) = "Supplier Agent" Or CType(ViewState("Pageame"), String) = "Room Category" Or CType(ViewState("Pageame"), String) = "Hotel Construction" Or CType(ViewState("Pageame"), String) = "Cancellation Policy" Or CType(ViewState("Pageame"), String) = "Block Full Sales" Or CType(ViewState("Pageame"), String) = "Complusary Remark" Or CType(ViewState("Pageame"), String) = "Promotion" Or CType(ViewState("Pageame"), String) = "Customers" Or CType(ViewState("Pageame"), String) = "Sectorgroup" Or CType(ViewState("Pageame"), String) = "Other Service Group" Or CType(ViewState("Pageame"), String) = "Child PolicyNew" Then

            pname = pnames.Item("ReportTitle_text")
            paramvalue.Value = strReportTitle ' Request.QueryString("ReportTitle_text")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


        End If
        'Added case "Location" by Archana on 18/05/2015 for menu Location Master 
        If CType(ViewState("Pageame"), String) = "Block Full Sales" Or CType(ViewState("Pageame"), String) = "Complusary Remark" Or CType(ViewState("Pageame"), String) = "Minimum Nights" Or CType(ViewState("Pageame"), String) = "Promotion" Then
            pname = pnames.Item("market")
            paramvalue.Value = Request.QueryString("MktCode") ' Request.QueryString("ReportTitle_text")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If

        If CType(ViewState("Pageame"), String) = "Flights Master" Then

            pname = pnames.Item("detail")
            paramvalue.Value = Request.QueryString("detail")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


        End If

        If CType(ViewState("Pageame"), String) = "SellingFormulaForSuppliersNew" Then

            pname = pnames.Item("@supcode")
            paramvalue.Value = IIf(Request.QueryString("SupCode") <> "[Select]", Request.QueryString("SupCode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@codevalue")
            paramvalue.Value = Request.QueryString("Code")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@marketcode")
            paramvalue.Value = IIf(Request.QueryString("MktCode") <> "[Select]", Request.QueryString("MktCode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@selltypcode")
            paramvalue.Value = IIf(Request.QueryString("SellCode") <> "[Select]", Request.QueryString("SellCode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@frmdate")
            If (Request.QueryString("FrmDate")) = "" Then
                paramvalue.Value = String.Empty

            Else
                paramvalue.Value = Format(CType(Request.QueryString("FrmDate"), Date), "yyyy/MM/dd")

            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")


            If (Request.QueryString("ToDate")) = "" Then
                paramvalue.Value = String.Empty

            Else
                paramvalue.Value = Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd")

            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@approved")
            paramvalue.Value = IIf(Request.QueryString("Approved") = 2, DBNull.Value, Request.QueryString("Approved"))
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If
        If CType(ViewState("Pageame"), String) = "Room Category" Then
            pname = pnames.Item("TypeValue")
            paramvalue.Value = Request.QueryString("Typevalue")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If





        If CType(ViewState("Pageame"), String) = "SellingFormulaForCategories" Then

            pname = pnames.Item("@frmdate")
            If (Request.QueryString("FrmDate")) = "" Then
                paramvalue.Value = String.Empty

            Else
                paramvalue.Value = Format(CType(Request.QueryString("FrmDate"), Date), "yyyy/MM/dd")

            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            If (Request.QueryString("ToDate")) = "" Then
                paramvalue.Value = String.Empty

            Else
                paramvalue.Value = Format(CType(Request.QueryString("ToDate"), Date), "yyyy/MM/dd")

            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@pspfmlcode")
            paramvalue.Value = Request.QueryString("Code")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@sellcode")
            paramvalue.Value = IIf(Request.QueryString("SellCode") <> "[Select]", Request.QueryString("SellCode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@currcode")
            paramvalue.Value = IIf(Request.QueryString("Currcode") <> "[Select]", Request.QueryString("Currcode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@suptypcode")
            paramvalue.Value = IIf(Request.QueryString("SuptypeCode") <> "[Select]", Request.QueryString("SuptypeCode"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        If CType(ViewState("Pageame"), String) = "VisaApplied" Then

            pname = pnames.Item("@fromdate")
            paramvalue.Value = Format(CType(Request.QueryString("frmdt"), Date), "yyyy/MM/dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = Format(CType(Request.QueryString("todt"), Date), "yyyy/MM/dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@chkdate")
            paramvalue.Value = Request.QueryString("chkdt")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@cmbsearch")
            paramvalue.Value = IIf(Request.QueryString("seltyp") <> "[Select]", Request.QueryString("seltyp"), "")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@txtsearch")
            paramvalue.Value = Request.QueryString("txtSrc")
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
