<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="UpdateSupplierBookingSearch.aspx.vb" Inherits="UpdateSupplierBookingSearch" %>

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
<script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>

<script type="text/javascript">

    $(document).ready(function () {
        AutoCompleteExtender_Destination_KeyUp();
        AutoCompleteExtender_HotelName_KeyUp();
        AutoCompleteExtender_Customer_KeyUp();
        AutoCompleteExtender_RO_KeyUp();
    });

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
            alert("To date should not be greater than From date");
        }
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
            alert("To date should not be greater than From date");
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
        sender.set_contextKey(document.getElementById("divCode").value);        
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
            url: "UpdateSupplierBookingSearch.aspx/GetCountryDetails",
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
        $("#<%=txtDestinationName.ClientID %>").bind("change", function () {
            var destName = document.getElementById('<%=txtDestinationName.ClientID%>');
            if (destName.value == '') {
                document.getElementById('<%=txtDestinationCode.ClientID%>').value = '';

            }
        });

        $("#<%=txtDestinationName.ClientID %>").keyup("change", function () {
            var destName = document.getElementById('<%=txtDestinationName.ClientID%>');
            if (destName.value == '') {
                document.getElementById('<%=txtDestinationCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_HotelName_KeyUp() {
        $("#<%=txtHotelName.ClientID %>").bind("change", function () {
            var hotelName = document.getElementById('<%=txtHotelName.ClientID%>');
            if (hotelName.value == '') {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
            }
        });

        $("#<%=txtHotelName.ClientID %>").keyup("change", function () {
            var hotelName = document.getElementById('<%=txtHotelName.ClientID%>');
            if ( hotelName.value == '') {
                document.getElementById('<%=txtHotelCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_Customer_KeyUp() {
        $("#<%=txtCustomer.ClientID %>").bind("change", function () {
            var customerName = document.getElementById('<%=txtCustomer.ClientID%>');
            if (customerName.value == '') {
                document.getElementById('<%=txtCustomerCode.ClientID%>').value = '';
                document.getElementById('<%=txtCountry.ClientID%>').value = '';
                document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
                $find('<%=AutoCompleteExtender_txtCountry.ClientID%>').set_contextKey('');
            }
        });

        $("#<%=txtCustomer.ClientID %>").keyup("change", function () {
            var customerName = document.getElementById('<%=txtCustomer.ClientID%>');
            if (customerName.value == '') {
                document.getElementById('<%=txtCustomerCode.ClientID%>').value = '';
                document.getElementById('<%=txtCountry.ClientID%>').value = '';
                document.getElementById('<%=txtCountryCode.ClientID%>').value = '';
                $find('<%=AutoCompleteExtender_txtCountry.ClientID%>').set_contextKey('');
            }
        });
    }

    function AutoCompleteExtender_RO_KeyUp() {
        $("#<%=txtRO.ClientID %>").bind("change", function () {
            var ro = document.getElementById('<%=txtRO.ClientID%>');
            if (ro.value == '') {
                document.getElementById('<%=txtROCode.ClientID%>').value = '';                
            }
        });

        $("#<%=txtRO.ClientID %>").keyup("change", function () {
            var ro = document.getElementById('<%=txtRO.ClientID%>');
            if (ro.value == '') {
                document.getElementById('<%=txtROCode.ClientID%>').value = '';                
            }
        });
    }
    

</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtender_Destination_KeyUp();
        AutoCompleteExtender_HotelName_KeyUp();
        AutoCompleteExtender_Customer_KeyUp();
        AutoCompleteExtender_RO_KeyUp();
    }
              
</script>

<style type="text/css">
.divCol
{
	padding-left:7px;
	padding-top:15px;
	float:left;
	width:420px;
}

.divCaption
{
	width:150px;
	float:left;
}

.divInput
{
	width:270px;
	float:left;
}

.gv_Title 
{
    
	font-family: Arial,Verdana, Geneva, ms sans serif;
	font-size: 12pt;
	font-weight: bold;
	font-style:normal;
	font-variant: normal;
	border-width:1px;
	border-color:#06788B;
	color:#06788B;
	margin-left: 0px;
}
</style>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>           
                <table style="border: gray 2px solid; width: 100%" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Update Supplier in Booking" CssClass="field_heading"
                                Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>                            
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                            <asp:Button ID="btnHelp" runat="server" Text="Help"
                                Font-Bold="False" CssClass="search_button" style="display:none"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New"
                                Font-Bold="False" CssClass="btn" style="display:none"></asp:Button>
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" style="display:none" Text="Report" />    
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False" Text="Export To Excel" style="display:none" />                                
                            <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <div style="width: 1290px; float: left;">
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label id="lblBookingRef" class="field_caption">
                                            Booking Reference</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtBookingRef" TabIndex="1" runat="server"  Width="250"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">
                                            Service Type</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:DropDownList ID="ddlServiceType" TabIndex="2" class="field_input" Width="250" Style="text-transform: uppercase; "
                                            runat="server">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Accommodation</asp:ListItem>
                                            <asp:ListItem>Transfers</asp:ListItem>
                                            <asp:ListItem>Airport Services</asp:ListItem>
                                            <asp:ListItem>Visa</asp:ListItem>
                                            <asp:ListItem>Tours</asp:ListItem>
                                            <asp:ListItem>Other Services</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Destination/Location</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtDestinationName" TabIndex="3" runat="server"  Width="250"></asp:TextBox>
                                        <asp:TextBox ID="txtDestinationCode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtDestinationName_AutoCompleteExtender" runat="server"
                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                            FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="GetDestinationList"
                                            TargetControlID="txtDestinationName" OnClientItemSelected="DestinationNameautocompleteselected">
                                        </asp:AutoCompleteExtender>                                        
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Agent Reference</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtAgentRef" runat="server" TabIndex="4"  Width="250"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="dvGFName" class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Guest First Name</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtGuestFirstName" TabIndex="5" runat="server"  Width="250" ></asp:TextBox>
                                    </div>
                                </div>
                                <div id="dvGSName" class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Guest Second Name</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtGuestSecondName" TabIndex="6" runat="server"  Width="250" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Travel Date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:DropDownList ID="ddlTravelDate" TabIndex="7" class="field_input" Width="250" Style="text-transform: uppercase;"
                                            runat="server" AutoPostBack="true">
                                            <asp:ListItem>Any Dates</asp:ListItem>
                                            <asp:ListItem>Future bookings</asp:ListItem>
                                            <asp:ListItem>Past bookings</asp:ListItem>
                                            <asp:ListItem>Specific date</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divCol" id="dvTravelFromDate" runat="server">
                                    <div class="divCaption">
                                        <label class="field_caption">Travel From date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtTravelFromDate" class="field_input" runat="server" Width="75" onchange="filltodate(this);"></asp:TextBox>
                                        <asp:CalendarExtender ID="travelFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                        PopupButtonID="ImgBtnTravelFromDt" PopupPosition="Right" TargetControlID="txtTravelFromDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="travelFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtTravelFromDate">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnTravelFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" />
                                    <asp:MaskedEditValidator ID="MevTravelFromDt" runat="server" ControlExtender="travelFromDt_MaskedEditExtender"
                                        ControlToValidate="txtTravelFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="travelFromDt_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                    </div>
                                </div>
                                <div class="divCol" id="dvTravelToDate" runat="server">
                                    <div class="divCaption">
                                        <label class="field_caption">Travel To Date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtTravelToDate" class="field_input" runat="server" Width="75" onchange="ValidateDate();" ></asp:TextBox>
                                        <asp:CalendarExtender ID="TravelToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                        PopupButtonID="ImgBtnTravelToDate" PopupPosition="Right" TargetControlID="txtTravelToDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="TravelToDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtTravelToDate">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="imgBtnTravelToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" />
                                    <asp:MaskedEditValidator ID="MevTravelToDate" runat="server" ControlExtender="TravelToDate_MaskedEditExtender"
                                        ControlToValidate="txtTravelToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="TravelToDate_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label id="lblBookingDate" class="field_caption">Booking Date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:DropDownList ID="ddlBookingDate" TabIndex="8" class="field_input" Width="250" Style="text-transform: uppercase;"
                                            runat="server" AutoPostBack="true">
                                            <asp:ListItem>Any Dates</asp:ListItem>
                                            <asp:ListItem>Specific date</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divCol" id="dvBookingFromDate" runat="server" >
                                    <div class="divCaption">
                                        <label id="lblBookingFromDate" class="field_caption">Booking From date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtBookingFromDate" class="field_input" runat="server" Width="75" onchange="fillBookingTodate(this);"></asp:TextBox>
                                        <asp:CalendarExtender ID="BookingFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnBookingFromDate" 
                                        OnClientDateSelectionChanged="BookingDateSelectCalExt" PopupPosition="Right" TargetControlID="txtBookingFromDate">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="BookingFromDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtBookingFromDate">
                                        </asp:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnBookingFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <asp:MaskedEditValidator ID="BookingFromDate_mev" runat="server" ControlExtender="BookingFromDate_MaskedEditExtender"
                                            ControlToValidate="txtBookingFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="BookingFromDate_MaskedEditExtender"
                                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                            TooltipMessage="Date/Month/Year">
                                        </asp:MaskedEditValidator>
                                    </div>
                                </div>
                                <div class="divCol"  id="dvBookingToDate" runat="server">
                                    <div class="divCaption">
                                        <label id="lblBookingToDate" class="field_caption">Booking To Date</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtBookingToDate" class="field_input" runat="server" Width="75" onchange="ValidateBookingDate();"></asp:TextBox>
                                        <asp:CalendarExtender ID="BookingToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnBookingToDate" 
                                        OnClientDateSelectionChanged="BookingDateSelectCalExt" PopupPosition="Right" TargetControlID="txtBookingToDate">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="BookingToDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtBookingToDate">
                                        </asp:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnBookingToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" />
                                        <asp:MaskedEditValidator ID="BookingToDate_mev" runat="server" ControlExtender="BookingToDate_MaskedEditExtender"
                                            ControlToValidate="txtBookingToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="BookingToDate_MaskedEditExtender"
                                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                            TooltipMessage="Date/Month/Year">
                                        </asp:MaskedEditValidator>
                                    </div>
                                </div>
                                <div id="dvBookingStatus" class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Booking Status</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:DropDownList ID="ddlBookingStatus" TabIndex="9" class="field_input" Style="text-transform: uppercase;"
                                            runat="server"  Width="250">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Confirmed bookings</asp:ListItem>
                                            <asp:ListItem>On request bookings</asp:ListItem>
                                            <asp:ListItem>Amended bookings</asp:ListItem>
                                            <asp:ListItem>Cancelled bookings</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Hotels</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtHotelName" TabIndex="10" runat="server" Width="250"></asp:TextBox>
                                        <asp:TextBox ID="txtHotelCode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtHotelName" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                            ServiceMethod="GetHotelName" TargetControlID="txtHotelName" UseContextKey="true"
                                            OnClientItemSelected="HotelNameautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                </div>
                                <div id="dvHotelConfNo" class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Hotel Conf No</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtHotelConfNo" TabIndex="11" runat="server"  Width="250"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="divCol" id="dvForAgent" runat="server">
                                    <div class="divCaption">
                                        <label class="field_caption">Agent</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtCustomer" runat="server" TabIndex="12" Width="250"></asp:TextBox>
                                        <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtCustomer" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                            ServiceMethod="GetCustomers" TargetControlID="txtCustomer" ContextKey="true" 
                                            OnClientPopulating="customer_OnClientPopulating" OnClientItemSelected="Customersautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                </div>
                                <div class="divCol">
                                    <div class="divCaption">
                                        <label class="field_caption">Source Country</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtCountry" runat="server" TabIndex="13"  Width="250"></asp:TextBox>
                                        <asp:TextBox ID="txtCountryCode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender_txtCountry" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="-1"
                                            ServiceMethod="GetCountry" TargetControlID="txtCountry" UseContextKey="true"
                                            OnClientItemSelected="Countryautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                </div>
                                <div class="divCol" id="dvForRO" runat="server">
                                    <div class="divCaption">
                                        <label class="field_caption">RO</label>
                                    </div>
                                    <div class="divInput">
                                        <asp:TextBox CssClass="field_input" ID="txtRO" TabIndex="14" runat="server"  Width="250"></asp:TextBox>
                                        <asp:TextBox ID="txtROCode" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="aceRO" runat="server" CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                            FirstRowSelected="True" MinimumPrefixLength="-1" ServiceMethod="GetRODetails"
                                            TargetControlID="txtRO" UseContextKey="true" OnClientItemSelected="ROautocompleteselected">
                                        </asp:AutoCompleteExtender>
                                    </div>
                                </div>
                            </div>
                        </td>                        
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 8px;">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn" TabIndex="15" Text="Search" />&nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" CssClass="btn" TabIndex="16" Text="Reset" OnClientClick="return fnReset();" />
                        </td>
                    </tr>
                    <tr>
                    <td style="width:100%;">
                        <table width="100%;">
                            <tr>
                                <td align="left">
                                    <label class="gv_Title">
                                        Search Result
                                    </label>
                                </td>
                                <td align="right" style="padding-right:10px" >
                                    <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                    <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true" TabIndex="12">
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
                    </td>
                    </tr>
                    <tr>
                    <td>
                    <div id="divGrid" style="min-height: 370px; max-height: 370px; max-width:96vw; overflow: auto">                    
                                                   
                                <asp:GridView ID="gvSearchResult" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                     AllowPaging="true" AllowSorting="true" Width="100%">                                    
                                    <Columns>                                        
                                        <asp:TemplateField HeaderText="Request No" SortExpression="requestId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>                                        
                                        
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="requestDate"
                                            SortExpression="requestDate" HeaderText="Request Date"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="agentName" HeaderText="Agent Name" SortExpression="agentName" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>                                        
                                                                                
                                        <asp:BoundField DataField="agentref" HeaderText="Agent Ref." SortExpression="agentref" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      
                                        
                                        <asp:BoundField DataField="hotelConfNo" HeaderText="Hotel Ref." SortExpression="hotelConfNo" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      
                                        
                                        <asp:BoundField DataField="serviceName" HeaderText="Hotel Name" SortExpression="serviceName" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>                                        
                                        
                                        <asp:BoundField DataField="guestName" HeaderText="Guest Name" SortExpression="guestName" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>                                                                                

                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="createddate"
                                        SortExpression="createddate" HeaderText="Date Created"></asp:BoundField>

                                        <asp:BoundField DataField="createdby" SortExpression="createdby" HeaderText="User Created">
                                        </asp:BoundField>      
                                        
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="lastmodified"
                                        SortExpression="lastmodified" HeaderText="Date Modified"></asp:BoundField>

                                        <asp:BoundField DataField="modifiedby" SortExpression="modifiedby" HeaderText="User Modified">
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnUpdate" Text="Update Supplier" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="UpdateSupplier" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnPrint" Text="Print" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="Print" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>   
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
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
                    </td>
                    </tr>
                </table>
            </ContentTemplate>
            </asp:UpdatePanel>            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
