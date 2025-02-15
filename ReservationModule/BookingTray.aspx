﻿<%@ Page Title="" Language="VB" MasterPageFile="~/ReservationMaster.master" AutoEventWireup="false"
    CodeFile="BookingTray.aspx.vb" Inherits="BookingTray" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript">


        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtTravelFromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=travelFromDt_CalendarExtender.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtTravelToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=TravelToDate_CalendarExtender.ClientID %>");
                var date2 = calendarBehavior2._selectedDate;

                var dp2 = txtfromDate2.value.split("/");
                var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
                newDt2 = getFormatedDate(newDt2);
                newDt2 = new Date(newDt2);
                calendarBehavior2.set_selectedDate(newDt2);
            }

        }
        function getFormatedDate(chkdate) {
            var dd = chkdate.getDate();
            var mm = chkdate.getMonth() + 1; //January is 0!
            var yyyy = chkdate.getFullYear();
            if (dd < 10) { dd = '0' + dd };
            if (mm < 10) { mm = '0' + mm };
            chkdate = mm + '/' + dd + '/' + yyyy;
            return chkdate;
        }

        function filltodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtTravelFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtTravelToDate.ClientID%>");

            if ((txtToDate.value != null) && (txtToDate.value != '')) {

                var dpFrom = txtfromDate.value.split("/");
                var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                var dpTo = txtToDate.value.split("/");
                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                newDtTo = getFormatedDate(newDtTo);
                newDtTo = new Date(newDtTo);
                if (newDt > newDtTo) {

                    txtToDate.value = txtfromDate.value;
                    DateSelectCalExt();
                    return;
                }
            }
        }

        function ValidateDate() {

            var txtfromDate = document.getElementById("<%=txtTravelFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtTravelToDate.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtfromDate.value = txtToDate.value;
            }

            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);

            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);
            if (newChkInDt > newChkOutDt) {
                txtToDate.value = txtfromDate.value;
                alert("To date should be greater than From date");
            }
        }
        function fnConfirmSave() {


        }

        function BookingDateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtBookingFromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=BookingFromDate_CalendarExtender.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtBookingToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=BookingToDate_CalendarExtender.ClientID %>");
                var date2 = calendarBehavior2._selectedDate;

                var dp2 = txtfromDate2.value.split("/");
                var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
                newDt2 = getFormatedDate(newDt2);
                newDt2 = new Date(newDt2);
                calendarBehavior2.set_selectedDate(newDt2);
            }

        }


        function fillBookingTodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtBookingFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtBookingToDate.ClientID%>");

            if ((txtToDate.value != null) && (txtToDate.value != '')) {

                var dpFrom = txtfromDate.value.split("/");
                var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                var dpTo = txtToDate.value.split("/");
                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                newDtTo = getFormatedDate(newDtTo);
                newDtTo = new Date(newDtTo);
                if (newDt > newDtTo) {

                    txtToDate.value = txtfromDate.value;
                    DateSelectCalExt();
                    return;
                }
            }
        }

        function ValidateBookingDate() {

            var txtfromDate = document.getElementById("<%=txtBookingFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtBookingToDate.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtfromDate.value = txtToDate.value;
            }

            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);

            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);
            if (newChkInDt > newChkOutDt) {
                txtToDate.value = txtfromDate.value;
                alert("To date should be greater than From date");
            }
        }



        function DestinationNameautocompleteselected(source, eventArgs) {

            if (eventArgs != null) {

                document.getElementById('<%=txtDestinationcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtDestinationcode.ClientID%>').value = '';
            }
            SetHotelContextkey();
        }

        function SetHotelContextkey() {
            var dc = document.getElementById('<%=txtDestinationcode.ClientID%>').value;
            var contxt = '';
            if (dc != '') {
                if (contxt != '') {
                    contxt = contxt + '||' + 'DC:' + dc;
                }
                else {
                    contxt = 'DC:' + dc;
                }

            }
            $find('<%=AutoCompleteExtender_txtHotelName.ClientID%>').set_contextKey(contxt);
        }

        function HotelNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {

                document.getElementById('<%=txtHotelCode.ClientID%>').value = eventArgs.get_value();
                GetHotelsDetails(document.getElementById('<%=txtHotelCode.ClientID%>').value);
            }
            else {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';

            }

            SetHotelContextkey();
        }

        function customer_OnClientPopulating(sender, args) {
            var ddlDivision = document.getElementById("<%=ddlDivision.ClientID %>");

            sender.set_contextKey(ddlDivision.value);
        }

        function Customersautocompleteselected(source, eventArgs) {

            if (eventArgs != null) {
                document.getElementById('<%=txtCustomerCode.ClientID%>').value = eventArgs.get_value();

                $find('<%=AutoCompleteExtender_txtCountry.ClientID%>').set_contextKey(eventArgs.get_value());
                GetCountryDetails(eventArgs.get_value());
            }
            else {
                document.getElementById('<%=txtCustomerCode.ClientID%>').value = '';
            }
        }
        function GetCountryDetails(CustCode) {

            $.ajax({
                type: "POST",
                url: "BookingTray.aspx/GetCountryDetails",
                data: '{CustCode:  "' + CustCode + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert('failure');
                    alert(response.d);
                },
                error: function (response) {
                    alert('error');
                    alert(response.d);
                }
            });
        }

        function OnSuccess(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var Countries = xml.find("Countries");
            var rowCount = Countries.length;

            if (rowCount == 1) {
                $.each(Countries, function () {
                    document.getElementById('<%=txtCountry.ClientID%>').value = ''
                    document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
                    document.getElementById('<%=txtCountry.ClientID%>').value = $(this).find("ctryname").text();
                    document.getElementById('<%=txtCountryCode.ClientID%>').value = $(this).find("ctrycode").text();
                    document.getElementById('<%=txtCountry.ClientID%>').setAttribute("readonly", true);
                    $find('AutoCompleteExtender_txtCountry').setAttribute("Enabled", false);
                });
            }
            else {
                document.getElementById('<%=txtCountry.ClientID%>').value = ''
                document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
                document.getElementById('<%=txtCountry.ClientID%>').removeAttribute("readonly");
                $find('AutoCompleteExtender_txtCountry').setAttribute("Enabled", true);
            }
        };

        function Countryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCountryCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
            }
        }
        function ROautocompleteselected(source, eventArgs) {

            if (eventArgs != null) {
                document.getElementById('<%=txtROCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtROCode.ClientID%>').value = '';
            }

        }


        function AutoCompleteExtender_Destination_KeyUp() {
            var destName = document.getElementById('<%=txtDestinationName.ClientID%>');
            if (destName.value == '') {
                document.getElementById('<%=txtDestinationCode.ClientID%>').value = '';

            }
        }

        function AutoCompleteExtender_HotelName_KeyUp() {

            var hotelName = document.getElementById('<%=txtHotelName.ClientID%>');
            if (hotelName.value == '') {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
            }
        }

        function AutoCompleteExtender_Customer_KeyUp() {
            var customerName = document.getElementById('<%=txtCustomer.ClientID%>');
            if (customerName.value == '') {
                document.getElementById('<%=txtCustomerCode.ClientID%>').value = '';
                document.getElementById('<%=txtCountry.ClientID%>').value = '';
                document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
                $find('<%=AutoCompleteExtender_txtCountry.ClientID%>').set_contextKey('');
            }

        }

        function AutoCompleteExtender_RO_KeyUp() {
            var ro = document.getElementById('<%=txtRO.ClientID%>');
            if (ro.value == '') {
                document.getElementById('<%=txtROCode.ClientID%>').value = '';
            }

        }


        function AutoCompleteExtender_txtFlightCode_OnClientPopulating(sender, args) {

            var cntxt = document.getElementById('<%=lblFlightServiceType.ClientID%>').innerHTML + '|' + document.getElementById('<%=lblAirportbordercodePopup.ClientID%>').innerHTML;
            sender.set_contextKey(cntxt);

        }

        function ArrivalflightAutocompleteSelected(source, eventArgs) {
            if (source != null) {

                var flightcode = eventArgs.get_value();
                document.getElementById('<%=txtTranflightCodePopup.ClientID%>').value = eventArgs.get_value();
                // var TranflightCode = document.getElementById('<%=txtTranflightCodePopup.ClientID%>').value;
                var txtFlightTime = document.getElementById('<%=txtFlightTime.ClientID%>');
         
               GetAirportAndTimeDetails(flightcode, txtFlightTime)
            }
            else {
                //  alert('test1');

            }

        }

        function AutoCompleteExtender_Arrivalflight_KeyUp(txt) {

                                    if (txt.value == '') {
                                        document.getElementById('<%=txtTranflightCodePopup.ClientID%>').value = '';
                                        document.getElementById('<%=txtFlightTime.ClientID%>').value = '';
                                    }

        }

        function GetAirportAndTimeDetails(flightcode, hiddenfieldIDTime) {

            $.ajax({
                type: "POST",
                url: "BookingTray.aspx/GetAirportAndTimeDetails",
                data: '{flightcode:  "' + flightcode + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var xmlDoc = $.parseXML(response.d);
                    var xml = $(xmlDoc);
                    var customers = xml.find("Customers");
                   
                    $.each(customers, function () {
                        var customer = $(this);
       
                        document.getElementById('<%=txtFlightTime.ClientID%>').value = $(this).find("destintime").text();
                    });

                },
                failure: function (response) {
                    alert('failure');
                    alert(response.d);
                },
                error: function (response) {
                    alert('error');
                    alert(response.d);
                }
            });
        }

    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function InitializeRequestUserControl(sender, args) {
          //  alert('InitializeRequestUserControl');
        }
        function EndRequestUserControl(sender, args) {
           // alert('EndRequestUserControl');
        }
              
    </script>
    <script type="text/javascript" charset="utf-8">
        function ShowProgress() {

            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.show();
                        $.removeCookie('Downloaded', { path: '/' });
                        //Check if receive cookie from server by second
                        intervalProgress = setInterval("$.checkDownloadFileCompletely()", 1000);
        }

        function HideProgess() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.hide(500);
        }

//                $.checkDownloadFileCompletely = function () {
//                    var cookieValue = $.getCookie('Downloaded');
//                    console.log(cookieValue + "---> Cookie Value;");
//                    if (cookieValue == 'True') {
//                        $.removeCookie('Downloaded');
//                        clearInterval(intervalProgress);
//                        HideProgess();
//                    }
//                }

//                /* get cookie from document.cookie */
//                $.getCookie = function (cookieName) {
//                    var cookieValue = document.cookie;
//                    var c_start = cookieValue.indexOf(" " + cookieName + "=");
//                    if (c_start == -1) {
//                        c_start = cookieValue.indexOf(cookieName + "=");
//                    }
//                    if (c_start == -1) {
//                        cookieValue = null;
//                    }
//                    else {
//                        c_start = cookieValue.indexOf("=", c_start) + 1;
//                        var c_end = cookieValue.indexOf(";", c_start);
//                        if (c_end == -1) {
//                            c_end = cookieValue.length;
//                        }
//                        cookieValue = unescape(cookieValue.substring(c_start, c_end));
//                    }
//                    return cookieValue;
//                }

//                /* Remove cookie in document.cookie */
//                $.removeCookie = function (cookieName) {
//                    var cookies = document.cookie.split(";");

//                    for (var i = 0; i < cookies.length; i++) {
//                        var cookie = cookies[i];
//                        var eqPos = cookie.indexOf("=");
//                        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
//                        if (name == cookieName) {
//                            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
//                        }
//                    }
//                }

    </script>
    <style type="text/css">
        .divCol
        {
            padding-left: 7px;
            padding-top: 15px;
            float: left;
            width: 31%;
        }
        
        .divCaption
        {
            width: 30%;
            float: left;
        }
        
        .divInput
        {
            width: 70%;
            float: left;
        }
        
        .gv_Title
        {
            font-family: Arial,Verdana, Geneva, ms sans serif;
            font-size: 12pt;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            border-width: 1px;
            border-color: #06788B;
            color: #06788B;
            margin-left: 0px;
        }
        
        .ChildGrid
        {
            border: 1px none #999999;
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            background: White;
            color: #DDD9CF;
            font-weight: normal;
            margin-top: 0px;
        }
        .ChildGridHeader
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            background-color: #0A9CB1;
            border-color: Gray;
            color: White;
            font-size: 12px;
            line-height: 150%;
        }
        
        .ChildgrdAternaterow
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            font-weight: normal;
            background: #CFF9FA; /*#FFDDCC; */
            color: black;
            line-height: 150%;
        }
        .ChildgrdRowstyle
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            background: white;
            font-weight: normal;
            background: #FFFFF0;
            color: black;
            font-size: 12px;
            line-height: 150%;
        }
        .Childgrdfooter
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            font-weight: bold;
            background: #DDD9CF;
            color: #06788B;
            height: 25px;
        }
        .fbcolor
        {
            border: 0px;
        }
        
        
          /* AutoComplete highlighted item */
    .autocomplete_completionListElement
    {
        margin: 0px !important;
        z-index:99999 !important;
        background-color: ivory;
        color: windowtext;
        border: buttonshadow;
        border-width: 1px;
        border-style: solid;
        cursor: 'default';
        overflow: auto;
        height: 200px;
        text-align: left;
        left: 0px;
        list-style-type: none;
    }
    /* AutoComplete highlighted item */
    .autocomplete_highlightedListItem
    {
        z-index:99999 !important;
        background-color: #ffff99;
        color: black;
        padding: 1px;
        cursor:hand;
    }
    /* AutoComplete item */
    .autocomplete_listItem
    {
        z-index:99999 !important;
        background-color: window;
        color: windowtext;
        padding: 1px;
        cursor:hand;
    }
    
    
    </style>
    <asp:UpdatePanel ID="UpdatePanel2"   runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%">
                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>  --%>
                <table style="border: gray 2px solid; width: 100%" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Booking Tray" CssClass="field_heading"
                                Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                            <asp:Button ID="btnHelp" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"
                                Style="display: none"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False"
                                CssClass="btn" Style="display: none"></asp:Button>
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Style="display: none" Text="Report" />
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False"
                                Text="Export To Excel" Style="display: none" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; min-width: 1260px;">
                            <%--       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                            <div style="width: 100%; float: left;">
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label id="lblBookingRef" class="field_caption">
                                                    Booking Reference</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtBookingRef" TabIndex="1" runat="server"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Service Type</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlServiceType" TabIndex="2" class="field_input" Width="95%"
                                                    Style="text-transform: uppercase;" AutoPostBack="true" runat="server">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Accommodation</asp:ListItem>
                                                    <asp:ListItem>Transfers</asp:ListItem>
                                                    <asp:ListItem>Airport Services</asp:ListItem>
                                                    <asp:ListItem>Visa</asp:ListItem>
                                                    <asp:ListItem>Tours</asp:ListItem>
                                                    <asp:ListItem>Other Services</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvTransferType" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Transfer Type</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTransferType" TabIndex="2" class="field_input" Width="95%"
                                                    Style="text-transform: uppercase;" runat="server">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Arrival</asp:ListItem>
                                                    <asp:ListItem>Departure</asp:ListItem>
                                                    <asp:ListItem>InterHotel/Transit</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Destination/Location</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtDestinationName" TabIndex="3" runat="server"
                                                    onKeyUp="AutoCompleteExtender_Destination_KeyUp()" Width="95%"></asp:TextBox>
                                                <asp:TextBox ID="txtDestinationCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="txtDestinationName_AutoCompleteExtender" runat="server"
                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                    FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetDestinationList"
                                                    TargetControlID="txtDestinationName" OnClientItemSelected="DestinationNameautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Agent Reference</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtAgentRef" runat="server" TabIndex="4"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dvGFName" class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Guest First Name</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtGuestFirstName" TabIndex="5" runat="server"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dvGSName" class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Guest Second Name</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtGuestSecondName" TabIndex="6" runat="server"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Travel Date</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTravelDate" TabIndex="7" class="field_input" Width="95%"
                                                    Style="text-transform: uppercase;" runat="server" AutoPostBack="true">
                                                    <asp:ListItem>Any Dates</asp:ListItem>
                                                    <asp:ListItem>Future bookings</asp:ListItem>
                                                    <asp:ListItem>Past bookings</asp:ListItem>
                                                    <asp:ListItem>Check In or Check Out</asp:ListItem>
                                                    <asp:ListItem>Checkin date</asp:ListItem>
                                                    <asp:ListItem>Checkout date</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvTravelFromDate" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Travel From date</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtTravelFromDate" class="field_input" runat="server"
                                                    Width="75" onchange="filltodate(this);"></asp:TextBox>
                                                <asp:CalendarExtender ID="travelFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                    OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnTravelFromDt"
                                                    PopupPosition="Right" TargetControlID="txtTravelFromDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="travelFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtTravelFromDate">
                                                </asp:MaskedEditExtender>
                                                <asp:ImageButton ID="ImgBtnTravelFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="-1" />
                                                <asp:MaskedEditValidator ID="MevTravelFromDt" runat="server" ControlExtender="travelFromDt_MaskedEditExtender"
                                                    ControlToValidate="txtTravelFromDate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="travelFromDt_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvTravelToDate" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Travel To Date</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtTravelToDate" class="field_input" runat="server"
                                                    Width="75" onchange="ValidateDate();"></asp:TextBox>
                                                <asp:CalendarExtender ID="TravelToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                    OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnTravelToDate"
                                                    PopupPosition="Right" TargetControlID="txtTravelToDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="TravelToDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtTravelToDate">
                                                </asp:MaskedEditExtender>
                                                <asp:ImageButton ID="imgBtnTravelToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="-1" />
                                                <asp:MaskedEditValidator ID="MevTravelToDate" runat="server" ControlExtender="TravelToDate_MaskedEditExtender"
                                                    ControlToValidate="txtTravelToDate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="TravelToDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label id="lblBookingDate" class="field_caption">
                                                    Booking Date</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBookingDate" TabIndex="8" class="field_input" Width="95%"
                                                    Style="text-transform: uppercase;" runat="server" AutoPostBack="true">
                                                    <asp:ListItem>Any Dates</asp:ListItem>
                                                    <asp:ListItem>Specific date</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvBookingFromDate" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label id="lblBookingFromDate" class="field_caption">
                                                    Booking From date</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtBookingFromDate" class="field_input" runat="server"
                                                    Width="75" onchange="fillBookingTodate(this);"></asp:TextBox>
                                                <asp:CalendarExtender ID="BookingFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="ImgBtnBookingFromDate" OnClientDateSelectionChanged="BookingDateSelectCalExt"
                                                    PopupPosition="Right" TargetControlID="txtBookingFromDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="BookingFromDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtBookingFromDate">
                                                </asp:MaskedEditExtender>
                                                <asp:ImageButton ID="ImgBtnBookingFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="-1" />
                                                <asp:MaskedEditValidator ID="BookingFromDate_mev" runat="server" ControlExtender="BookingFromDate_MaskedEditExtender"
                                                    ControlToValidate="txtBookingFromDate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="BookingFromDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvBookingToDate" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label id="lblBookingToDate" class="field_caption">
                                                    Booking To Date</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtBookingToDate" class="field_input" runat="server"
                                                    Width="75" onchange="ValidateBookingDate();"></asp:TextBox>
                                                <asp:CalendarExtender ID="BookingToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                    PopupButtonID="ImgBtnBookingToDate" OnClientDateSelectionChanged="BookingDateSelectCalExt"
                                                    PopupPosition="Right" TargetControlID="txtBookingToDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="BookingToDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtBookingToDate">
                                                </asp:MaskedEditExtender>
                                                <asp:ImageButton ID="ImgBtnBookingToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                    TabIndex="-1" />
                                                <asp:MaskedEditValidator ID="BookingToDate_mev" runat="server" ControlExtender="BookingToDate_MaskedEditExtender"
                                                    ControlToValidate="txtBookingToDate" CssClass="field_error" Display="Dynamic"
                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                    ErrorMessage="BookingToDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                    InvalidValueMessage="Invalid Date" TooltipMessage="Date/Month/Year">
                                                </asp:MaskedEditValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dvBookingStatus" class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Booking Status</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBookingStatus" TabIndex="9" class="field_input" Style="text-transform: uppercase;"
                                                    runat="server" Width="95%">
                                                    <asp:ListItem>All</asp:ListItem>
                                                    <asp:ListItem>Confirmed bookings</asp:ListItem>
                                                    <asp:ListItem>On request bookings</asp:ListItem>
                                                    <asp:ListItem>Amended bookings</asp:ListItem>
                                                    <asp:ListItem>Cancelled bookings</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divDivision" class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Division</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDivision" TabIndex="10" class="field_input" runat="server"
                                                    Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Hotels</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtHotelName" TabIndex="11" runat="server"
                                                    onkeyup=" AutoCompleteExtender_HotelName_KeyUp()" Width="95%"></asp:TextBox>
                                                <asp:TextBox ID="txtHotelCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtHotelName" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                                    ServiceMethod="GetHotelName" TargetControlID="txtHotelName" UseContextKey="true"
                                                    OnClientItemSelected="HotelNameautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="dvHotelConfNo" class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Hotel Conf No</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtHotelConfNo" TabIndex="12" runat="server"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvForAgent">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Agent</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtCustomer" runat="server" TabIndex="13"
                                                    onKeyup="AutoCompleteExtender_Customer_KeyUp()" Width="95%"></asp:TextBox>
                                                <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtCustomer" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                                    ServiceMethod="GetCustomers" TargetControlID="txtCustomer" ContextKey="true"
                                                    OnClientPopulating="customer_OnClientPopulating" OnClientItemSelected="Customersautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    Source Country</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtCountry" runat="server" TabIndex="14"
                                                    Width="95%"></asp:TextBox>
                                                <asp:TextBox ID="txtCountryCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtCountry" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                                    ServiceMethod="GetCountry" TargetControlID="txtCountry" UseContextKey="true"
                                                    OnClientItemSelected="Countryautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="divCol" id="dvForRO" runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%">
                                                <label class="field_caption">
                                                    RO</label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="field_input" ID="txtRO" onkeyup="AutoCompleteExtender_RO_KeyUp()"
                                                    TabIndex="15" runat="server" Width="95%"></asp:TextBox>
                                                <asp:TextBox ID="txtROCode" runat="server" Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="aceRO" runat="server" CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                    FirstRowSelected="True" MinimumPrefixLength="-1" ServiceMethod="GetRODetails"
                                                    TargetControlID="txtRO" UseContextKey="true" OnClientItemSelected="ROautocompleteselected">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--             </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 8px;">
                            <div style="padding-left: 250px;">
                                <div style="width: 150px; float: left;">
                                    <%--            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn" TabIndex="16" Text="Search"
                                        OnClientClick="ShowProgress()" />&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" TabIndex="17" Text="Reset"
                                        OnClientClick="return fnReset();" />
                                    <%--     </ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </div>
                                <div style="width: 150px; float: left;">

                                    <asp:Button ID="btnLoadReport" runat="server" CssClass="btn"   UseSubmitBehavior="true"
                                        TabIndex="16" Text="Load Report To Excel" /></div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <%--     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>--%>
                            <table width="100%;">
                                <tr>
                                    <td align="left">
                                        <label class="gv_Title">
                                            Search Result
                                        </label>
                                    </td>
                                    <td align="right" style="padding-right: 10px;">
                                        <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                        <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true" TabIndex="18">
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <%--     </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>--%>
                            <div id="divGrid" style="min-height: 370px; max-height: 1870px; min-width: 1260px;
                                max-width: 96vw; overflow: auto">
                                <asp:GridView ID="gvSearchResult" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    AllowPaging="true" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S No">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnSequenceNo" runat="server" Text='<%# Bind("sequenceNo") %>'
                                                    OnClick="lbtnSequenceNo_Click" CommandArgument="Show"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestDt" runat="server" Text='<%# Bind("requestDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hotel/Service Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblService" runat="server" Text='<%# Bind("servicename") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CheckIn Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcheckindate" runat="server" Text='<%# Bind("checkindate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CheckOut Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcheckoutdate" runat="server" Text='<%# Bind("checkoutdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArrivalDt" runat="server" Text='<%# Bind("Servicedate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgentName" runat="server" Text='<%# Bind("agentName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Ref.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgentRef" runat="server" Text='<%# Bind("agentref") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Guest Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuestName" runat="server" Text='<%# Bind("guestName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Booking Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookingStatus" runat="server" Text='<%# Bind("bookingstatus") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sale Value" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSaleValue" runat="server" Text='<%# Bind("salevaluebase") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collected Value" SortExpression="collectedamt" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCollectedamt" runat="server" Text='<%# Bind("collectedamt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance Amount" SortExpression="balanceamt" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceAmt" runat="server" Text='<%# Bind("balanceamt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Modified Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastModifiedDt" runat="server" Text='<%# Bind("lastmodified","{0:dd/MM/yyyy HH:mm:ss }") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Modified By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastModified" runat="server" Text='<%# Bind("modifiedby") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Update">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbUpdateGuest" runat="server" OnClick="lbUpdateGuest_Click" Text="Update Hotel Room No and Mobile"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View Details">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:LinkButton ID="lbViewFlight" runat="server" OnClick="lbViewFlight_Click" Text="Flight Details"></asp:LinkButton>
                                                </div>
                                                <div style="padding-top: 10px;">
                                                    <asp:LinkButton ID="lbViewBookingRemarks" runat="server" OnClick="lbViewBookingRemarks_Click"
                                                        Text="Booking Remarks"></asp:LinkButton>
                                                </div>
                                                <div style="padding-top: 10px;">
                                                    <asp:LinkButton ID="lbGRNote" runat="server" OnClick="lbGRNote_Click" Text="GR Note"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("Invoiceno") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnProforma" Text="Proforma" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="ProformaPrint" ForeColor="Blue" runat="server"></asp:LinkButton>
                                                <div style="padding-top: 10px;">
                                                    <asp:LinkButton ID="lbtnItinerary" Text="Itinerary" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                        CommandName="ItineraryPrint" ForeColor="Blue" runat="server"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnVoucher" Text="Voucher" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="VoucherPrint" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnPrint" Text="Invoice" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="InvoicePrint" ForeColor="Blue" runat="server"></asp:LinkButton>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <tr id="trClass" runat="server" style="border-width: 0px">
                                                            <td colspan="14" style="border-width: 0px">
                                                                <asp:Panel ID="panHotelDetail" runat="server" Visible="false" Style="width: 100%;">
                                                                    <div style="overflow-x: scroll; max-width: 100%">
                                                                        <asp:GridView ID="gvHotelDetail" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid"
                                                                            Width="100%" OnRowDataBound="gvHotelDetail_RowDataBound" ShowFooter="true">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Service Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblServiceType" runat="server" Text='<%# Bind("serviceType") %>'></asp:Label>
                                                                                        <asp:TextBox ID="txtRequestId" runat="server" Text='<%# Bind("requestid") %>' Style="display: none"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtRlineNo" runat="server" Text='<%# Bind("rlineno") %>' Style="display: none"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtRoomno" runat="server" Text='<%# Bind("roomno") %>' Style="display: none"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="From Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# Bind("fromdate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="To Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Bind("todate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Hotel Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                                                                                        <asp:TextBox ID="txtPartyCode" runat="server" Text='<%# Bind("partycode") %>' Style="display: none"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Particulars">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("particulars") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Supplier Agent">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSupAgentCode" runat="server" Text='<%# Bind("supagentcode") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Hotel Conf No">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblConfNo" runat="server" Text='<%# Bind("confno") %>'></asp:Label>
                                                                                        <asp:LinkButton ID="lbConfirm" runat="server" OnClick="lbConfirm_Click" Text="UpdateConfirm"></asp:LinkButton>
                                                                                        <asp:TextBox ID="txtConfirmNo" placeholder="Confirm No" runat="server" Text=""></asp:TextBox>
                                                                                        <asp:TextBox ID="txtConfirmTimeLimit" CssClass="field_input" placeholder="Cancel Time Limit"
                                                                                            runat="server" onchange="ValidateBookingDate();" Text=""></asp:TextBox>
                                                                                        <asp:CalendarExtender ID="txtConfirmTimeLimit_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                                                            PopupButtonID="ImgBtnConfirmTimeLimit" OnClientDateSelectionChanged="BookingDateSelectCalExt"
                                                                                            PopupPosition="Right" TargetControlID="txtConfirmTimeLimit">
                                                                                        </asp:CalendarExtender>
                                                                                        <asp:MaskedEditExtender ID="txtConfirmTimeLimit_MaskedEditExtender" runat="server"
                                                                                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtConfirmTimeLimit">
                                                                                        </asp:MaskedEditExtender>
                                                                                        <asp:ImageButton ID="ImgBtnConfirmTimeLimit" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                                            TabIndex="-1" />
                                                                                        <asp:MaskedEditValidator ID="txtConfirmTimeLimit_mev" runat="server" ControlExtender="txtConfirmTimeLimit_MaskedEditExtender"
                                                                                            ControlToValidate="txtConfirmTimeLimit" CssClass="field_error" Display="Dynamic"
                                                                                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                                            ErrorMessage="txtConfirmTimeLimit_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Date/Month/Year">
                                                                                        </asp:MaskedEditValidator>
                                                                                        <asp:Button ID="btnConfirmSave" OnClick="btnConfirmSave_Click" runat="server" Text="Save" />
                                                                                        <asp:Button ID="btnConfirmCancel" OnClick="btnConfirmCancel_Click" runat="server"
                                                                                            Text="Cancel" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Pax Details">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPaxDetails" runat="server" Text='<%# Bind("paxdetails") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalText" runat="server" Text="Total"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Sale Nontaxable" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSaleNontaxValue" runat="server" Text='<%# Bind("salenontaxablebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalSaleNontaxValue" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Sale Taxable" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSalevalue" runat="server" Text='<%# Bind("saletaxablebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalSaleValue" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="VAT Payable" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblVatPayable" runat="server" Text='<%# Bind("vatpayablebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalVatPayable" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total Sale">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSaleValueBase" runat="server" Text='<%# Bind("salevaluebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalSaleValueBase" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Cost Nontaxable" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCostNontaxable" runat="server" Text='<%# Bind("costnontaxablebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalCostNontaxable" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Cost Taxable" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCost" runat="server" Text='<%# Bind("costtaxablebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalCost" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="VAT Input" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblVatInputBase" runat="server" Text='<%# Bind("vatinputbase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalVatInputBase" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total Cost">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCostValueBase" runat="server" Text='<%# Bind("costvaluebase") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalCostValueBase" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Gross Profit" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGrossProfit" runat="server" Text='<%# Bind("grossprofit") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalGrossProfit" runat="server"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLineStatus" runat="server" Text='<%# Bind("linestatus") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Free To Customer">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblComplimentaryCust" runat="server" Text='<%# Bind("complimentarycust") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Free From Supplier">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblComplimentarySupp" runat="server" Text='<%# Bind("complimentarysupp") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Update Flight and Time">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAirportbordercode" runat="server" Style="display: none;" Text='<%# Bind("airportbordercode") %>'></asp:Label>
                                                                                        <asp:Label ID="lblFlight" runat="server" Text='<%#Eval("flightNo")+ " " + Eval("flighttime")%>'></asp:Label>
                                                                                        <asp:Label ID="lblTflightNo" runat="server" Style="display: none;" Text='<%#Eval("flightNo")%>'></asp:Label>
                                                                                        <asp:Label ID="lblTflighttime" runat="server" Style="display: none;" Text='<%#Eval("flighttime")%>'></asp:Label>
                                                                                        <asp:TextBox ID="txtTranflightCode" Style="display: none" Text='<%#Eval("flight_tranid")%>'
                                                                                            runat="server"></asp:TextBox>
                                                                                        <asp:LinkButton ID="lbUpdateFlight" runat="server" Text="Change" OnClick="lbUpdateFlight_Click"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Update Pickup Time">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lbUpdatePickupTime" runat="server" Text="Update" OnClick="lbUpdatePickupTime_Click"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Excusrion Ticket">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lbExcusrionTicket" runat="server" Text="Download" OnClick="lbExcusrionTicket_Click"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="ChildGridHeader" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle CssClass="ChildgrdRowstyle" HorizontalAlign="left"></RowStyle>
                                                                            <AlternatingRowStyle CssClass="ChildgrdAternaterow" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                            <FooterStyle CssClass="Childgrdfooter" HorizontalAlign="Right" VerticalAlign="Middle" />
                                                                        </asp:GridView>
                                                                        <asp:Label ID="lblMsgHotelDetail" runat="server" Text="Records not found, Please redefine search criteria"
                                                                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                                                            Visible="False"></asp:Label>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle"
                                        ForeColor="White" BorderColor="LightGray" />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                </asp:GridView>
                                <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                    Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                    Visible="False"></asp:Label>
                            </div>
                            <asp:HiddenField ID="hdguest" runat="server" />
                            <asp:ModalPopupExtender ID="modalSelectGuest" runat="server" BehaviorID="modalSelectGuest"
                                CancelControlID="TdGuestClose" TargetControlID="hdguest" PopupControlID="dvGuestDetail"
                                PopupDragHandleControlID="PopupGuestHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <div id="dvGuestDetail" runat="server" style="min-height: 250px; max-height: 450px;
                                width: 950px; border: 3px solid #06788B; background-color: White;">
                                <table style="width: 99%; padding: 5px 5px 5px 5px">
                                    <tr>
                                        <td id="PopupGuestHeader" bgcolor="#06788B">
                                            <asp:Label ID="Label3" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                Text="Update Hotel Room No and Mobile" Width="205px"></asp:Label>
                                        </td>
                                        <td align="center" id="TdGuestClose" bgcolor="#06788B">
                                            <asp:Label ID="Label4" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                Font-Size="Large" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="min-height: 200px; max-height: 400px; overflow: auto;">
                                                <asp:GridView ID="gvGuestDetails" TabIndex="9" runat="server" Font-Size="10px" Width="100%"
                                                    CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="Solid"
                                                    AutoGenerateColumns="False" AllowSorting="True">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Request Id">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
                                                                <asp:Label ID="lblRlineno" runat="server" Style="display: none" Text='<%# Bind("rlineno") %>'></asp:Label>
                                                                <asp:Label ID="lblGuestType" runat="server" Style="display: none" Text='<%# Bind("Guesttype") %>'></asp:Label>
                                                                <asp:Label ID="lbltitle" runat="server" Style="display: none" Text='<%# Bind("title") %>'></asp:Label>
                                                                <%--  <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>' __designer:wfdid="w1"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Room No">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblroomNo" runat="server" Text='<%# Bind("roomno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Guest Line No">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGuestLineNo" runat="server" Text='<%# Bind("guestlineno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tittle">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlGuestTittle" Enabled="false" Style="width: 100px" runat="server">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="First Name">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFirstName" runat="server" Enabled="false" Style="width: 150px"
                                                                    Text='<%# Bind("firstname") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Middle Name">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMiddeleName" runat="server" Enabled="false" Style="width: 150px"
                                                                    Text='<%# Bind("middlename") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Name">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtlastname" runat="server" Enabled="false" Style="width: 150px"
                                                                    Text='<%# Bind("lastname") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hotel Room No" ControlStyle-Width="70px">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtHotelRoomNo" Text='<%# Bind("hotelroomno") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mobile No" ControlStyle-Width="70px">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMobileNo" Text='<%# Bind("mobileno") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="padding-top: 5px">
                                            <asp:Button ID="btnGuestSave" runat="server" CssClass="btn" Text="Update Hotel Room No and Mobile" />
                                            <asp:Button ID="btnokcontract" runat="server" CssClass="btn" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:HiddenField ID="hdFlightDetails" runat="server" />
                            <asp:ModalPopupExtender ID="ModalFlightDetails" runat="server" BehaviorID="ModalFlightDetails"
                                CancelControlID="Td2" TargetControlID="hdFlightDetails" PopupControlID="dvFlightDetails"
                                PopupDragHandleControlID="Td1" Drag="true" BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <div id="dvFlightDetails" runat="server" style="min-height: 250px; max-height: 450px;
                                width: 90%; border: 3px solid #06788B; background-color: White;">
                                <table style="width: 98%; padding: 5px 5px 5px 5px">
                                    <tr>
                                        <td id="Td1" bgcolor="#06788B">
                                            <asp:Label ID="lblViewDetailsPopupHeading" runat="server" CssClass="field_heading"
                                                Style="padding: 3px 0px 3px 3px" Text="Flight Details" Width="205px"></asp:Label>
                                        </td>
                                        <td align="center" id="Td2" bgcolor="#06788B">
                                            <asp:Label ID="Label2" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                Font-Size="Large" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="dvflightDetailsPopup" runat="server" style="min-height: 200px; max-height: 400px;
                                                overflow: auto;">
                                                <asp:GridView ID="gvFlightDetails" TabIndex="9" runat="server" Font-Size="10px" Width="100%"
                                                    CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" AutoGenerateColumns="False"
                                                    BorderStyle="Solid" AllowSorting="False">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestId" HeaderText="Request Id" />
                                                        <asp:BoundField DataField="RLineno" HeaderText="RLineno" />
                                                        <asp:BoundField DataField="GuestLineNo" HeaderText="Guest Line No" />
                                                        <asp:BoundField DataField="GuestName" HeaderText="Guest Name" />
                                                        <asp:BoundField DataField="ArrivalDate" HeaderText="Arrival Date" />
                                                        <asp:BoundField DataField="ArrivalFlightCode" HeaderText="Arrival Flight" />
                                                        <asp:BoundField DataField="arrflighttime" HeaderText="Arrival Flight Time" />
                                                        <asp:BoundField DataField="ArrivalAirport" HeaderText="Arrival Airport" />
                                                        <asp:BoundField DataField="depdate" HeaderText="Departure Date" />
                                                        <asp:BoundField DataField="depflightcode" HeaderText="Dep Flight" />
                                                        <asp:BoundField DataField="depflighttime" HeaderText="Dep Flight Time" />
                                                        <asp:BoundField DataField="DepartureAirport" HeaderText="Departure Airport" />
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </div>
                                            <div id="dvBookingRemarks" runat="server" style="min-height: 200px; max-height: 400px;
                                                overflow: auto;">
                                                <asp:GridView ID="gvBookingRemarks" TabIndex="9" runat="server" Font-Size="10px"
                                                    Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                    AutoGenerateColumns="False" BorderStyle="Solid">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                                                        <asp:BoundField DataField="RequestId" Visible="false" HeaderText="Request Id" />
                                                        <asp:BoundField DataField="RLineno" Visible="false" HeaderText="RLineno" />
                                                        <asp:BoundField DataField="ServiceName" HeaderText="Service Name" />
                                                        <asp:BoundField DataField="ServiceDate" HeaderText="Services Date" />
                                                        <asp:TemplateField HeaderText="Party/Hotel Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestId" Style="display: none" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>
                                                                <asp:Label ID="lblServiceType" Style="display: none" runat="server" Text='<%# Bind("ServiceType") %>'></asp:Label>
                                                                <asp:Label ID="lblRLineno" Style="display: none" runat="server" Text='<%# Bind("RLineno") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPartyremarks" runat="server" Width="96%" Style="min-height: 50px;
                                                                    max-height: 300px;" TextMode="MultiLine" Text='<%# Bind("Partyremarks") %>'></asp:TextBox>
                                                                <asp:Label ID="lblPartyremarks" runat="server" Text='<%# Bind("Partyremarks") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Agent Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAgentremarks" runat="server" Text='<%# Bind("agentremarks") %>'></asp:Label>
                                                                <asp:TextBox ID="txtAgentremarks" Width="96%" Style="min-height: 50px; max-height: 300px;"
                                                                    runat="server" TextMode="MultiLine" Text='<%# Bind("agentremarks") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Arrival Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArrivalremarks" runat="server" Text='<%# Bind("arrivalremarks") %>'></asp:Label>
                                                                <asp:TextBox ID="txtArrivalremarks" runat="server" Width="96%" Style="min-height: 50px;
                                                                    max-height: 300px;" TextMode="MultiLine" Text='<%# Bind("arrivalremarks") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Departure Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDepartureRemarks" runat="server" Text='<%# Bind("departureremarks") %>'></asp:Label>
                                                                <asp:TextBox ID="txtDepartureRemarks" runat="server" Width="96%" Style="min-height: 50px;
                                                                    max-height: 300px;" TextMode="MultiLine" Text='<%# Bind("departureremarks") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnRemarksEdit" runat="server" CssClass="btn" Text="Edit" OnClick="btnRemarksEdit_Click" />
                                                                <asp:Button ID="btnRemarksUpdate" runat="server" CssClass="btn" Text="Update" OnClick="btnRemarksUpdate_Click" />
                                                                &nbsp;<asp:Button ID="btnRemarksCancel" runat="server" CssClass="btn" Text="Cancel"
                                                                    Style="margin-top: 5px;" OnClick="btnRemarksCancel_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="updateddate" HeaderText="Updated Date" />
                                                        <asp:BoundField DataField="updateduser" HeaderText="Updated User" />
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="padding-top: 5px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:HiddenField ID="hdPickTime" runat="server" />
                            <asp:ModalPopupExtender ID="ModalPopupExtenderPickupTime" runat="server" BehaviorID="modalPickupTime"
                                CancelControlID="TdPickupClose" TargetControlID="hdPickTime" PopupControlID="dvExcPickup"
                                BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <div id="dvExcPickup" runat="server" style="min-height: 100px; max-height: 450px;
                                min-width: 200px; max-width: 950px; border: 3px solid #06788B; background-color: White;">
                                <table style="min-width: 29%; max-width: 99%; padding: 5px 5px 5px 5px">
                                    <tr>
                                        <td id="PopupPickupHeader" bgcolor="#06788B">
                                            <asp:Label ID="Label1" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                Text="Update Pickup Time"></asp:Label>
                                        </td>
                                        <td align="center" id="TdPickupClose" bgcolor="#06788B">
                                            <asp:Label ID="Label5" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                Font-Size="Large" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div id="dvTourPickup" runat="server" style="min-height: 50px; max-height: 500px;
                                                overflow: auto;">
                                                <asp:GridView ID="gvExcursionPickupTime" TabIndex="9" runat="server" Font-Size="10px"
                                                    Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                    BorderStyle="Solid" AutoGenerateColumns="False" AllowSorting="True">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Request Id">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>
                                                                <asp:Label ID="lblRlineno" runat="server" Style="display: none" Text='<%# Bind("elineno") %>'></asp:Label>
                                                                <asp:Label ID="lblExcTypeCode" runat="server" Style="display: none" Text='<%# Bind("exctypcode") %>'></asp:Label>
                                                                <%--  <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>' __designer:wfdid="w1"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Excursion Name">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExcName" runat="server" Text='<%# Bind("service_name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Service Date">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("excdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pickup Time">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPickupTime" runat="server" Style="display: none" Text='<%# Bind("exc_starttime") %>'></asp:Label>
                                                                <asp:Label ID="lblReturnPickupTime" runat="server" Style="display: none" Text='<%# Bind("exc_endtime") %>'></asp:Label>
                                                                <asp:TextBox ID="txtPickupTime" runat="server" Text='<%# Eval("exc_starttime") %>'
                                                                    CssClass="field_input" Width="80px"></asp:TextBox>
                                                              <%--  <asp:MaskedEditExtender ID="Mestarttime" runat="server" Mask="99:99" MaskType="Time"
                                                                    AcceptAMPM="false" TargetControlID="txtPickupTime">
                                                                </asp:MaskedEditExtender>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Return Pickup Time">
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReturnPickupTime" runat="server" Text='<%# Eval("exc_endtime") %>'
                                                                    CssClass="field_input" Width="80px"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="MEndtime" runat="server" Mask="99:99" MaskType="Time"
                                                                    AcceptAMPM="false" TargetControlID="txtReturnPickupTime">
                                                                </asp:MaskedEditExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </div>
                                            <div id="dvTransferPickup" runat="server" style="min-height: 20px; max-height: 100px;
                                                overflow: auto;">
                                                <asp:Label ID="Label6" runat="server" CssClass="fiel_input" Style="padding: 3px 0px 3px 3px"
                                                    Text="Pickup Time"></asp:Label>
                                                <asp:TextBox ID="txtTransferPickupTime" runat="server" Text="" CssClass="field_input"
                                                    Width="80px"></asp:TextBox>
                                                <asp:MaskedEditExtender ID="Mestarttime" runat="server" Mask="99:99" MaskType="Time"
                                                    AcceptAMPM="false" TargetControlID="txtTransferPickupTime">
                                                </asp:MaskedEditExtender>
                                            </div>
                                            <asp:Label ID="lblServiceTypePopup" runat="server" Style="display: none;" Text=""></asp:Label>
                                            <asp:Label ID="lblRequestidPopup" runat="server" Style="display: none;" Text=""></asp:Label>
                                            <asp:Label ID="lbltlinenoPopup" runat="server" Style="display: none;" Text=""></asp:Label>
                                            <asp:Label ID="lbltransfertypePopup" runat="server" Style="display: none;" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="padding-top: 5px">
                                            <asp:Button ID="btnUpdatePickTime" runat="server" CssClass="btn" Text="Update" />
                                            &nbsp;<asp:Button ID="btnUpdatePickTimeClose" runat="server" CssClass="btn" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:HiddenField ID="hdGRNoteHiddenField" runat="server" />
                            <asp:ModalPopupExtender ID="mpGRNote" runat="server" BehaviorID="modalGRNote" CancelControlID="TdGRNoteClose"
                                TargetControlID="hdGRNoteHiddenField" PopupControlID="dvGRNote" BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <div id="dvGRNote" runat="server" style="min-height: 200px; max-height: 450px; min-width: 400px;
                                max-width: 950px; border: 3px solid #06788B; background-color: White;">
                                <table style="min-width: 99%; max-width: 99%; padding: 5px 5px 5px 5px">
                                    <tr>
                                        <td id="Td3" bgcolor="#06788B">
                                            <asp:Label ID="Label7" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                Text="GR Note"></asp:Label>
                                        </td>
                                        <td align="center" id="TdGRNoteClose" bgcolor="#06788B">
                                            <asp:Label ID="Label8" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                Font-Size="Large" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <div style="padding-top: 15px;">
                                                    <asp:Label ID="Label9" runat="server" Text="Note"></asp:Label>
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtGRNote" runat="server" Width="96%" Style="min-height: 50px; max-height: 300px;"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <asp:Label ID="lblGRNoteRequestId" runat="server" Style="display: none;" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="padding-top: 5px">
                                            <asp:Button ID="btnGRNoteSave" runat="server" CssClass="btn" Text="Save" />
                                            &nbsp;<asp:Button ID="btnGRNoteClose" runat="server" CssClass="btn" Text="Close" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
                <%--   </ContentTemplate>
            </asp:UpdatePanel>   --%>
            </div>
             <asp:HiddenField ID="hdFlightUpdatePopup" runat="server" />
            <asp:ModalPopupExtender ID="mpFlightUpdate" runat="server" BehaviorID="modalFlightUpdate"
                CancelControlID="TdFlightUpdateClose" TargetControlID="hdFlightUpdatePopup" PopupControlID="dvFlightUpdate"
                
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <div id="dvFlightUpdate" runat="server" style="min-height: 200px; max-height: 450px;
                min-width: 400px; max-width: 950px; border: 3px solid #06788B; background-color: White;">
                <table style="min-width: 99%; max-width: 99%; padding: 5px 5px 5px 5px">
                    <tr>
                        <td id="tdPopupFlightHeader"  bgcolor="#06788B">
                            <asp:Label ID="Label10" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                Text="Update Flight and Time"></asp:Label>
                        </td>
                        <td align="center" id="TdFlightUpdateClose" bgcolor="#06788B">
                            <asp:Label ID="Label11" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                Font-Size="Large" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="dvFlight" runat="server">
                                <div style="padding-top: 25px; padding-left: 15px;">
                                    <asp:TextBox ID="txtFlightCode" placeholder="Flight Code" onKeyUp="AutoCompleteExtender_Arrivalflight_KeyUp(this)"
                                        Width="120px" runat="server"></asp:TextBox>
                                    <%--Text='<%# Bind("flightNo") %>'--%>



                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtArrivalflight" runat="server"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1"  FirstRowSelected="True" MinimumPrefixLength="-1"
                                        ServiceMethod="Getflight" TargetControlID="txtFlightCode" UseContextKey="true"
                                         OnClientItemSelected="ArrivalflightAutocompleteSelected"
                                        OnClientPopulating="AutoCompleteExtender_txtFlightCode_OnClientPopulating">
                                        <%--   OnClientItemSelected="ArrivalflightAutocompleteSelected"--%>
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txtTranflightCodePopup" Style="display: none" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txtFlightTime" Style="margin-top: 2px;" placeholder="Flight Time"
                                        Width="120px" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblAirportbordercodePopup" Style="display: none" runat="server"></asp:Label>
                                    <asp:Label ID="lblFlightRequestId" Style="display: none" runat="server"></asp:Label>
                                    <asp:Label ID="lblFlightRlineNo" Style="display: none" runat="server"></asp:Label>
                                    <asp:Label ID="lblFlightServiceType" Style="display: none" runat="server"></asp:Label>
                                </div>
                                <div style="padding-top: 15px; padding-left: 15px;">
                                    <asp:Button ID="btnFlightSave" CssClass="btn" Style="margin-top: 2px;" OnClick="btnFlightSave_Click"
                                        runat="server" Text="Save" />
                                    <asp:Button ID="btnFlightCancel" CssClass="btn" Style="margin-top: 2px;" OnClick="btnFlightCancel_Click"
                                        runat="server" Text="Cancel" />
                                </div>
                                <%--Text='<%# Bind("flighttime") %>'--%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
                <h2 style="color: #06788B">
                    Processing please wait...</h2>
            </div>
            <asp:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
                TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
            <input id="btnCloseLoading" runat="server" type="button" value="Cancel" style="display: none" />
        </ContentTemplate>
        <Triggers >
        <asp:PostBackTrigger ControlID="btnLoadReport" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
