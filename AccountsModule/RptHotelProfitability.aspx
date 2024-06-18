<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptHotelProfitability.aspx.vb" Inherits="RptHotelProfitability" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript">
    function DateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = $find("<%=FromDt_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }
        var txtfromDate2 = document.getElementById("<%=txtToDt.ClientID%>");
        if (txtfromDate2.value != '') {
            var calendarBehavior2 = $find("<%=ToDt_CalendarExtender.ClientID %>");
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
        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");

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

        var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtToDt.ClientID%>");
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
            alert("To date should be greater than or equal to From date");
        }
    }

    function CountryGroupAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
        }
    }

    function AutoComplete_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivcode.ClientID%>').value);
    }

    function CustAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtCustCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtCustCode.ClientID%>').value = '';
        }
    }

    function AcctAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtAcctCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtAcctCode.ClientID%>').value = '';
        }
    }

    function SalePersonAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtSalePersonCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtSalePersonCode.ClientID%>').value = '';
        }
    }

    function PartyAutoCompleteSelected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtPartyCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtPartyCode.ClientID%>').value = '';
        }
    }
    
    function validation() {
        try {
            var txtfromDate = document.getElementById("<%=txtFromDt.ClientID%>");
            var txttoDate = document.getElementById("<%=txtToDt.ClientID%>");
            if (txtfromDate.value == "") {
                alert("From date can not be empty");
                return false;
            }
            if (txttoDate.value == "") {
                alert("To date can not be empty");
                return false;
            }

            var vfdt = (txtfromDate.value).includes("_");
            if (vfdt) {
                alert("Check from date");
                return false;
            }
            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var vtdt = (txttoDate.value).includes("_");
            if (vtdt) {
                alert("Check to date");
                return false;
            }
            var dp1 = txttoDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);
            ShowProgress();
        }
        catch (err) {
            alert("Check from date and to date");
            return false;
        }
    }

    function ShowProgress() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.show();
        $.removeCookie('Downloaded', { path: '/' });
        //Check if receive cookie from server by second
        intervalProgress = setInterval("$.checkDownloadFileCompletely()", 1000);
        return true;
    }

    function HideProgess() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.hide(500);
    }

    $.checkDownloadFileCompletely = function () {
        var cookieValue = $.getCookie('Downloaded');
        console.log(cookieValue + "---> Cookie Value;");
        if (cookieValue == 'True') {
            $.removeCookie('Downloaded');
            clearInterval(intervalProgress);
            HideProgess();
        }
    }

    /* get cookie from document.cookie */
    $.getCookie = function (cookieName) {
        var cookieValue = document.cookie;
        var c_start = cookieValue.indexOf(" " + cookieName + "=");
        if (c_start == -1) {
            c_start = cookieValue.indexOf(cookieName + "=");
        }
        if (c_start == -1) {
            cookieValue = null;
        }
        else {
            c_start = cookieValue.indexOf("=", c_start) + 1;
            var c_end = cookieValue.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = cookieValue.length;
            }
            cookieValue = unescape(cookieValue.substring(c_start, c_end));
        }
        return cookieValue;
    }

    /* Remove cookie in document.cookie */
    $.removeCookie = function (cookieName) {
        var cookies = document.cookie.split(";");

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            if (name == cookieName) {
                document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
            }
        }
    }
</script>

<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtender_CountryGroup_KeyUp();
        AutoCompleteExtender_Agent_KeyUp();
        AutoCompleteExtender_Acct_KeyUp();
        AutoCompleteExtender_SalesPerson_KeyUp()
        AutoCompleteExtender_Supplier_KeyUp()
    }
              
</script>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        AutoCompleteExtender_CountryGroup_KeyUp();
        AutoCompleteExtender_Agent_KeyUp();
        AutoCompleteExtender_Acct_KeyUp();
        AutoCompleteExtender_SalesPerson_KeyUp()
        AutoCompleteExtender_Supplier_KeyUp()
    });

    function AutoCompleteExtender_CountryGroup_KeyUp() {
        $("#<%= txtCtryGroup.ClientID %>").bind("change", function () {
            var ctryGroup = document.getElementById('<%=txtCtryGroup.ClientID%>');
            if (ctryGroup.value == '') {
                document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
            }
        });
        $("#<%= txtCtryGroup.ClientID %>").keyup("change", function () {
            var ctryGroup = document.getElementById('<%=txtCtryGroup.ClientID%>');
            if (ctryGroup.value == '') {
                document.getElementById('<%=txtCtryGroupCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_Agent_KeyUp() {
        $("#<%=txtCust.ClientID%>").bind("change", function () {
            var Cust = document.getElementById('<%=txtCust.ClientID%>');
            if (Cust.value == '') {
                document.getElementById('<%=txtCustCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtCust.ClientID%>").keyup("change", function () {
            var Cust = document.getElementById('<%=txtCust.ClientID%>');
            if (Cust.value == '') {
                document.getElementById('<%=txtCustCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_Acct_KeyUp() {
        $("#<%=txtAcct.ClientID%>").bind("change", function () {
            var Acct = document.getElementById('<%=txtAcct.ClientID%>');
            if (Acct.value == '') {
                document.getElementById('<%=txtAcctCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtAcct.ClientID%>").keyup("change", function () {
            var Acct = document.getElementById('<%=txtAcct.ClientID%>');
            if (Acct.value == '') {
                document.getElementById('<%=txtAcctCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_SalesPerson_KeyUp() {
        $("#<%=txtSalePerson.ClientID%>").bind("change", function () {
            var saleperson = document.getElementById('<%=txtSalePerson.ClientID%>');
            if (saleperson.value == '') {
                document.getElementById('<%=txtSalePersonCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtSalePerson.ClientID%>").keyup("change", function () {
            var saleperson = document.getElementById('<%=txtSalePerson.ClientID%>');
            if (saleperson.value == '') {
                document.getElementById('<%=txtSalePersonCode.ClientID%>').value = '';
            }
        });
    }

    function AutoCompleteExtender_Supplier_KeyUp() {
        $("#<%=txtParty.ClientID%>").bind("change", function () {
            var party = document.getElementById('<%=txtParty.ClientID%>');
            if (party.value == '') {
                document.getElementById('<%=txtPartyCode.ClientID%>').value = '';
            }
        });
        $("#<%=txtParty.ClientID%>").keyup("change", function () {
            var party = document.getElementById('<%=txtParty.ClientID%>');
            if (party.value == '') {
                document.getElementById('<%=txtPartyCode.ClientID%>').value = '';
            }
        });
    }

    function showProgress() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.show();
        $.removeCookie('DownloadHotelProfit', { path: '/' });
        //Check if receive cookie from server by second
        intervalProgress = setInterval("$.checkDownloadFileCompletely()", 1000);
        return true;
    }
    function HideProgess() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.hide(500);
    }

    $.checkDownloadFileCompletely = function () {
        var cookieValue = $.getCookie('DownloadHotelProfit');
        console.log(cookieValue + "---> Cookie Value;");
        if (cookieValue == 'True') {
            $.removeCookie('DownloadHotelProfit');
            clearInterval(intervalProgress);
            HideProgess();
        }
    }

    /* get cookie from document.cookie */
    $.getCookie = function (cookieName) {
        var cookieValue = document.cookie;
        var c_start = cookieValue.indexOf(" " + cookieName + "=");
        if (c_start == -1) {
            c_start = cookieValue.indexOf(cookieName + "=");
        }
        if (c_start == -1) {
            cookieValue = null;
        }
        else {
            c_start = cookieValue.indexOf("=", c_start) + 1;
            var c_end = cookieValue.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = cookieValue.length;
            }
            cookieValue = unescape(cookieValue.substring(c_start, c_end));
        }
        return cookieValue;
    }

    /* Remove cookie in document.cookie */
    $.removeCookie = function (cookieName) {
        var cookies = document.cookie.split(";");

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            if (name == cookieName) {
                document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/";
            }
        }
    }
</script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%; height:77vh;">
                <table style="border: gray 2px solid; width: 100%;" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Hotel Profitability Report" CssClass="field_heading"
                                Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                            <asp:Button ID="btnHelp" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"
                                Style="display: none"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False"
                                CssClass="btn" Style="display: none"></asp:Button>                            
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Font-Bold="False"
                                Text="Report" Style="display: none" />
                            <asp:GridView ID="gvSearchResult" runat="server" style="display:none"></asp:GridView> 
                            <asp:TextBox ID="txtRptType" runat="server" style="display:none"></asp:TextBox>
                            <asp:TextBox ID="txtDivcode" runat="server" style="display:none"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%">
                            <table cellpadding="7" width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <table cellpadding="7" width="100%">
                                            <tr>
                                                <td>
                                                    <label visible="false" runat="server" id="lbldatetype" class="field_caption">
                                                        Date Type</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList Visible="false" ID="ddldatetype" CssClass="field_input" runat="server"
                                                        Width="90%" TabIndex="3">
                                                        <asp:ListItem Text="ARRIVAL" Value="Arrival">
                                                        </asp:ListItem>
                                                        <asp:ListItem Text="DEPARTURE" Value="Departure"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;">
                                                    <asp:Label ID="frmdate" runat="server" class="field_caption" Text="From Date"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:TextBox ID="txtFromDt" CssClass="field_input" runat="server" TabIndex="1" onchange="filltodate(this);"
                                                        Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="FromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFromDt"
                                                        PopupPosition="Right" TargetControlID="txtFromDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="FromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtFromDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevFromDt" runat="server" ControlExtender="FromDt_MaskedEditExtender"
                                                        ControlToValidate="txtFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="FromDt_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:Label class="field_caption" ID="lbltodate" runat="server" Text=" To Date"></asp:Label>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                                        TabIndex="2" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="ToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" PopupPosition="Right"
                                                        TargetControlID="txtToDt"></asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="ToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtToDt"></asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="imgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MevToDt" runat="server" ControlExtender="ToDt_MaskedEditExtender"
                                                        ControlToValidate="txtToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="ToDt_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Country Group</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCtryGroup" CssClass="field_input" runat="server" TabIndex="6"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCtryGroupCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCtryGroup" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCountryGroup" TargetControlID="txtCtryGroup" OnClientItemSelected="CountryGroupAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        
                                                        Agent</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCust" CssClass="field_input" runat="server" TabIndex="10" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtCustCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCust" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetCustomers" TargetControlID="txtCust" UseContextKey="true" OnClientPopulating="AutoComplete_OnClientPopulating" OnClientItemSelected="CustAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Account</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAcct" CssClass="field_input" runat="server" TabIndex="9" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtAcctCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteAcct" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetAccounts" TargetControlID="txtAcct" UseContextKey="true" OnClientPopulating="AutoComplete_OnClientPopulating"
                                                        OnClientItemSelected="AcctAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Sales Person</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSalePerson" CssClass="field_input" runat="server" TabIndex="9"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtSalePersonCode" runat="server" Style="display:none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtenderSalePerson" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetSalesPersons" TargetControlID="txtSalePerson" OnClientItemSelected="SalePersonAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Supplier</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParty" CssClass="field_input" runat="server" TabIndex="10" Width="90%"></asp:TextBox>
                                                    <asp:TextBox ID="txtPartyCode" runat="server" Style="display: none"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                        ServiceMethod="GetSuppliers" TargetControlID="txtParty" OnClientItemSelected="PartyAutoCompleteSelected">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Booking Service</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBookingType" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                           <%-- <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Group By</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGroupBy" CssClass="field_input" runat="server" Width="90%"
                                                        TabIndex="5">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button ID="btnLoadReport" runat="server" CssClass="btn" Text="Report" OnClientClick="return validation();" />
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" Text="Reset" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <center>
                    <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                        <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
                        <h2 style="color: #06788B">
                            Processing please wait...</h2>
                    </div>
                </center>
                <asp:ModalPopupExtender ID="ModalPopupLoading" runat="server" BehaviorID="ModalPopupLoading"
                    TargetControlID="btnInvisibleLoading" CancelControlID="btnCloseLoading" PopupControlID="Loading1"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <input id="btnInvisibleLoading" runat="server" type="button" value="Cancel" style="display: none" />
                <input id="btnCloseLoading" type="button" value="Cancel" style="display: none" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoadReport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

