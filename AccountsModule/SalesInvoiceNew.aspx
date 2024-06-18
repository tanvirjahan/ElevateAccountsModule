<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SalesInvoiceNew.aspx.vb" Inherits="SalesInvoiceNew" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
<script type="text/jscript">
    $(document).ready(function () {
        $("[id*=chkSelectAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }
            });
        });

        $("[id*=chkReceiptAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }
            });
        });
    });

    function showProgress() {
        var ModalPopupLoading = $find("ModalPopupLoading");
        ModalPopupLoading.show();
        return true;
    }

    function DateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = $find("<%=ChkFromDt_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }
        var txtfromDate2 = document.getElementById("<%=txtChkToDt.ClientID%>");
        if (txtfromDate2.value != '') {
            var calendarBehavior2 = $find("<%=txtChkToDt_CalendarExtender.ClientID %>");
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
        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtChkToDt.ClientID%>");
        txtToDate.value = txtfromDate.value;
        return;

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

        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtChkToDt.ClientID%>");
        if (txtfromDate.value == null || txtfromDate.value == "") {
            txtToDate.value = "";
            alert("Please select From date.");
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

    function RequestDateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtReqFromDt.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = $find("<%=ReqFromDt_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }
        var txtfromDate2 = document.getElementById("<%=txtReqToDt.ClientID%>");
        if (txtfromDate2.value != '') {
            var calendarBehavior2 = $find("<%=ReqToDt_CalendarExtender.ClientID %>");
            var date2 = calendarBehavior2._selectedDate;

            var dp2 = txtfromDate2.value.split("/");
            var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
            newDt2 = getFormatedDate(newDt2);
            newDt2 = new Date(newDt2);
            calendarBehavior2.set_selectedDate(newDt2);
        }

    }


    function fillRequestTodate(fDate) {
        var txtfromDate = document.getElementById("<%=txtReqFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtReqToDt.ClientID%>");

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

    function ValidateRequestDate() {

        var txtfromDate = document.getElementById("<%=txtReqFromDt.ClientID%>");
        var txtToDate = document.getElementById("<%=txtReqToDt.ClientID%>");
        if (txtfromDate.value == null || txtfromDate.value == "") {
            //txtToDate.value = "";
            txtfromDate.value=txtToDate.value
            //alert("Please select From date.");
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


    function AdvancedOption() {
        var lnkBtnAdvanced = document.getElementById("<%=lnkBtnAdvanced.ClientID%>");
        lnkBtnAdvanced.click();
    }

    function validateSearch() { 
        var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
        var txttoDate = document.getElementById("<%=txtChkToDt.ClientID%>");
        var chkDtFlag = document.getElementById("<%=hdnChkDtFlag.ClientID%>");

        if ((txtfromDate.value == null) || (txtfromDate.value == '')) {
            if (chkDtFlag.value == 'Y') {
                alert("Please select check in from date.");
            }
            else {
                alert("Please select check out from date.");
            }
            txtfromDate.focus();
            return false;
        }

        if ((txttoDate.value == null) || (txttoDate.value == '')) {
            if (chkDtFlag.value == 'Y') {
                alert("Please select check in to date.");
            }
            else {
                alert("Please select check out to date.");
            }
            txttoDate.focus();
            return false;
        }

        var txtReqFromDate = document.getElementById("<%=txtReqFromDt.ClientID%>");
        var txtReqToDate = document.getElementById("<%=txtReqToDt.ClientID%>");

        if (txtReqFromDate != null && txtReqToDate != null) {
            if ((txtReqFromDate.value != null) && (txtReqFromDate.value != '')) {

                if ((txtReqToDate.value == null) || (txtReqToDate.value == '')) {
                    alert("Please select request to date.");
                    txtReqToDate.focus();
                    return false;
                }
            }
        }        
        return true;
    }

    function AutoCompleteBookingNo_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }
    function AutoCompleteCustName_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }
    function AutoCompleteCustRef_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }
    function AutoCompleteGuestName_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }
    function AutoCompleteSupName_OnClientPopulating(sender, args) {
        sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
    }

    function disableGenInvoice() {
        //var btnGenInvoice1 = document.getElementById("<%=btnGenInvoice.ClientID%>");
        //btnGenInvoice1.disabled = true;

    }
</script>
    <style type="text/css">
        .advLink
        {
            font-family: Arial,Verdana, Geneva, ms sans serif;
            font-size: 10pt;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            border-width: 1px;
            border-color: #06788B;
            margin-left: 0px;
        }  
        
        .disablebtn
        {          
            border-radius: 2px;
	        border: 1px solid #06788B;
	        font-family: Verdana, Arial, Geneva, ms sans serif;
	        font-size: 10pt;
	        font-weight: bold;
	        font-style: normal;
	        font-variant: normal;
	        color:white;
	        background-color :Gray;
	        margin-top: 0px;
            height: 24px;
        }
            
    </style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="New Sales Invoice" style="padding:2px"
                            CssClass="field_heading" Width="100%" >
                        </asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%; padding:2px 4px 0px 4px">
                            <tr>
                                <td style="width: 15%;">
                                    <label id="lblchkFromDt" runat="server" class="field_caption">
                                       From Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                    <asp:HiddenField ID="hdnChkDtFlag" runat="server" />
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtDivCode" runat="server" style="display:none" > </asp:TextBox>
                                    <asp:TextBox ID="txtChkFromDt" CssClass="field_input" runat="server" TabIndex="1" onchange="filltodate(this);"
                                         Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="ChkFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                        PopupButtonID="ImgBtnChkFromDt" PopupPosition="Right" TargetControlID="txtChkFromDt">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="ChkFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtChkFromDt">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgBtnChkFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="-1" /><br />
                                    <asp:MaskedEditValidator ID="MevChkFromDt" runat="server" ControlExtender="ChkFromDt_MaskedEditExtender"
                                        ControlToValidate="txtChkFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="ChkFromDt_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                                <td style="width: 15%">
                                    <label id="lblChkToDt" runat="server" class="field_caption">
                                        To Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtChkToDt" CssClass="field_input" onchange="ValidateDate();" runat="server" TabIndex="2"
                                        Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtChkToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                        PopupButtonID="ImgBtnChkToDt" PopupPosition="Right" TargetControlID="txtChkToDt">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="ChkToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtChkToDt">
                                    </asp:MaskedEditExtender>
                                    <asp:ImageButton ID="imgBtnChkToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" /><br />
                                    <asp:MaskedEditValidator ID="MevChkToDt" runat="server" ControlExtender="ChkToDt_MaskedEditExtender"
                                        ControlToValidate="txtChkToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="ChkToDt_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                            </tr>   
                            <tr>
                            <td colspan="6">
                            <table>
                            <tr>
                            <td><img id="ImgAdvanced" src="~/Images/rightArrow.png" alt="image" style="cursor: pointer" runat="server" onclick="AdvancedOption()"/>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkBtnAdvanced" CssClass="advLink"  runat="server" TabIndex="3" >Advanced Search</asp:LinkButton>
                            </td>
                            </tr>                              
                            </table>                                
                            </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div id="divAdvanced" class="advanced" runat="server" visible="false" style="border:1px; border-style:solid;border-color:#C0C0C0; width:100%;" >                                    
                                    <table style="width:100%" cellpadding="10px">
                                            <tr>
                                                <td style="width: 15%">
                                                    <label class="field_caption">
                                                        Booking Number</label>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtBookingNo" CssClass="field_input" runat="server" TabIndex="4"
                                                        Width="90%"></asp:TextBox> 
                                                    <asp:AutoCompleteExtender ID="AutoCompleteBookingNo" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetRequestId" TargetControlID="txtBookingNo" UseContextKey="true" OnClientPopulating="AutoCompleteBookingNo_OnClientPopulating">
                                                    </asp:AutoCompleteExtender>                                                                                                
                                                </td>
                                                <td style="width: 15%">
                                                    <label class="field_caption">
                                                        Customer Name</label>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtCustName" CssClass="field_input" runat="server" TabIndex="5"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCustName" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetCustomers" TargetControlID="txtCustName" UseContextKey="true" OnClientPopulating="AutoCompleteCustName_OnClientPopulating">
                                                    </asp:AutoCompleteExtender>                                                                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Supplier Name</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSupName" CssClass="field_input" runat="server" TabIndex="6"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteSupName" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetSuppliers" TargetControlID="txtSupName" UseContextKey="true" OnClientPopulating="AutoCompleteSupName_OnClientPopulating">
                                                    </asp:AutoCompleteExtender>                                                                                                
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Lead Guest Name</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGuestName" CssClass="field_input" runat="server" TabIndex="7"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteGuestName" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetguestNames" TargetControlID="txtGuestName" UseContextKey="true" OnClientPopulating="AutoCompleteGuestName_OnClientPopulating">
                                                    </asp:AutoCompleteExtender>                                                                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        Customer Reference</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustRef" CssClass="field_input" runat="server" TabIndex="8"
                                                        Width="90%"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteCustRef" runat="server" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                    ServiceMethod="GetAgentRef" TargetControlID="txtCustRef" UseContextKey="true" OnClientPopulating="AutoCompleteCustRef_OnClientPopulating">
                                                    </asp:AutoCompleteExtender>                                                                                                
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        Booking Type</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBookingType" CssClass="field_input" runat="server" Width="90%" TabIndex="9">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="field_caption">
                                                        From Request Date</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReqFromDt" CssClass="field_input" runat="server" TabIndex="10" onchange="fillRequestTodate(this);" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="ReqFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnReqFromDt" OnClientDateSelectionChanged="RequestDateSelectCalExt"
                                                        PopupPosition="Right" TargetControlID="txtReqFromDt">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="ReqFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtReqFromDt">
                                                    </asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnReqFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="ReqFromDt_mev" runat="server" ControlExtender="ReqFromDt_MaskedEditExtender"
                                                        ControlToValidate="txtReqFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="ReqFromDt_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                                <td>
                                                    <label class="field_caption">
                                                        To Request Date</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReqToDt" CssClass="field_input" runat="server" TabIndex="11" onchange="ValidateRequestDate();" Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="ReqToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnReqToDt" OnClientDateSelectionChanged="RequestDateSelectCalExt"
                                                        PopupPosition="Right" TargetControlID="txtReqToDt">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="ReqToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtReqToDt">
                                                    </asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnReqToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="ReqToDt_mev" runat="server" ControlExtender="ReqToDt_MaskedEditExtender"
                                                        ControlToValidate="txtReqToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="ReqToDt_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>
                                                </td>
                                            </tr>
                                        </table>
                                       
                                    </div>                                                                        
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center" style="padding-top:8px;">
                                <table>
                                <tr>
                            <td>
                                    <asp:Button ID="btnDisplay" runat="server" CssClass="btn" TabIndex="15" Text="Display Pending for Invoicing" OnClientClick="return validateSearch();"/>&nbsp;&nbsp;
                                    <asp:Button ID="btnReset" runat="server" CssClass="btn" TabIndex="16" Text="Clear" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnViewSchedule" runat="server" CssClass="btn" TabIndex="16" Text="View Schedule" />
                                  <%--  Tanvir 27102023--%>
                                  </td>
                                  <td>
                                         <asp:CheckBox ID="chkschedule" AutoPostBack="true"     text="Schedule" runat="server" Width="100px"> </asp:CheckBox>
                               
                                                           <asp:TextBox ID="txtscheduledate" CssClass="field_input" runat="server" TabIndex="11" onchange="ValidateRequestDate();" Width="75px"></asp:TextBox>
                             									       <asp:TextBox ID="txttime" CssClass="field_input" runat="server"  
                                                        Width="40px"></asp:TextBox>																								   
                                   <asp:CalendarExtender ID="txtSchedule_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnscheduledate" OnClientDateSelectionChanged="RequestDateSelectCalExt"
                                                        PopupPosition="Right" TargetControlID="txtscheduledate">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="txtschedule_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" TargetControlID="txtscheduledate">
                                                    </asp:MaskedEditExtender>
                                                    <asp:ImageButton ID="ImgBtnscheduledate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                        TabIndex="-1" /><br />
                                                    <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="txtschedule_MaskedEditExtender"
                                                        ControlToValidate="txtscheduledate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                        EmptyValueMessage="Date is required" ErrorMessage="txtschedule_MaskedEditExtender"
                                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                                        TooltipMessage="Input a Date in Date/Month/Year">
                                                    </asp:MaskedEditValidator>  
                                                    </td>
                                                    <td>
                                                                                                  
                                                            <asp:Button ID="btnsaveschedule" runat="server" CssClass="btn" TabIndex="16" Text="Save" /> </td>
                                                             </td>  
                                                            </tr>
                                                 </table>
                              
                               </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left" style="padding-top:8px; padding-bottom:8px">
                               <asp:TabContainer ID="TabBookList" runat="server" ActiveTabIndex="0" TabIndex="17" >
                                <asp:TabPanel ID="panConfirm" runat="server" HeaderText="Ready To Invoice" Font-Bold="true" TabIndex="18">
                                <ContentTemplate>
                                <div id="divGrid" style="min-height: 370px; max-height: 370px; max-width:95vw; overflow:scroll">
                                <asp:GridView ID="gvReadyInvoice" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" style="padding-top:3px; padding-bottom:3px;" AllowSorting="true" TabIndex="19">                                    
                                    <Columns>                                        
                                        <asp:TemplateField>
                                        <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelectAll" cssclass="chkHeader" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelection" Checked='<%# Bind("selection") %>' Width="10px" cssclass="chkItem" />
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Request ID" SortExpression="RequestId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                                                               
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="Amended" HeaderText="Amended" SortExpression="Amended" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Arrival Date" SortExpression="ArrivalDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArrivalDt" runat="server" Text='<%# Bind("ArrivalDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnArrivalDt"  runat="server" Value='<%# Bind("ArrivalDate","{0:dd/MM/yyyy}") %>' />                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Departure Date" SortExpression="DepartureDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartureDt" runat="server" Text='<%# Bind("DepartureDate","{0:dd/MM/yyyy}") %>'></asp:Label>                                                
                                                <asp:HiddenField ID="hdnDepartureDt"  runat="server" Value='<%# Bind("DepartureDate","{0:dd/MM/yyyy}") %>' />                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                    
                                        <asp:BoundField DataField="agentName" HeaderText="Customer Name" SortExpression="agentName" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      
                                        
                                        <asp:TemplateField HeaderText="Customer Ref." SortExpression="agentRef">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgentRef" runat="server" Text='<%# Bind("agentRef") %>'></asp:Label>                                                
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                        
                                        <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   

                                        <asp:BoundField DataField="Currency" HeaderText="Currency" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   

                                        <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>                                       

                                        <asp:TemplateField HeaderText="Sales Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesAmount" runat="server" Text='<%# Bind("SalesAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="salesAmt" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountReceived" runat="server" Text='<%# Bind("AmountReceived") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        </asp:TemplateField> 
                                        <asp:TemplateField  HeaderText="Received">
                                        <ItemTemplate>
                                            <asp:Button ID="btnAmtReceived" runat="server" CssClass="btn" Text="Receipts" CommandName="Receipts" CommandArgument='<%# Container.DisplayIndex %>' />
                                            <asp:Label ID="lblReceivedStatus" runat="server" Text='<%# Bind("ReceivedStatus") %>' style="display:none"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>      
                                        <asp:TemplateField HeaderText="Proforma Invoice" Visible="false">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnProforma" Text="View" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="Proforma" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>                                                                                                                  
                                        <asp:TemplateField HeaderText="Preview Invoice">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnView" Text='<%# Bind("InvoiceNo") %>' CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="View" ForeColor="Blue" runat="server"></asp:LinkButton>
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
                                </asp:GridView>
                                <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                                </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabPanPending" runat="server" HeaderText="Pending Confirmation" TabIndex="20">
                                <ContentTemplate>
                                <div id="div1" style="min-height: 370px; max-height: 370px; max-width:95vw; overflow:scroll">
                                <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" AllowSorting="true" TabIndex="21">                                    
                                    <Columns>                                                                                
                                        <asp:TemplateField HeaderText="Request ID" SortExpression="RequestId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>                                               
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>                                        
                                                                               
                                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="Amended" HeaderText="Amended" SortExpression="Amended" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>

                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="ArrivalDate"
                                        SortExpression="ArrivalDate" HeaderText="Arrival Date"></asp:BoundField>

                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="DepartureDate"
                                        SortExpression="DepartureDate" HeaderText="Departure Date"></asp:BoundField>
                                                                                
                                        <asp:BoundField DataField="agentName" HeaderText="Customer Name" SortExpression="agentName" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      
                                        
                                        <asp:BoundField DataField="agentRef" HeaderText="Customer Ref." SortExpression="agentRef" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      

                                        <asp:BoundField DataField="GuestName" HeaderText="Guest Name" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   
                                        
                                        <asp:BoundField DataField="Currency" HeaderText="Currency" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>   

                                        <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>                                       

                                        <asp:TemplateField HeaderText="Sales Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesAmount" runat="server" Text='<%# Bind("SalesAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle CssClass="salesAmt" />
                                        </asp:TemplateField>                                                                                                                            
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />                                   
                                </asp:GridView>
                                <asp:Label ID="lblPendingMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>  
                                </div>                                
                                </ContentTemplate>
                                </asp:TabPanel>
                                </asp:TabContainer>                                    
                                </td>
                            </tr>
                            <tr>
                            <td colspan="6" align="center" style="padding-top:8px;">
                                    <asp:Button ID="btnValidate" runat="server" CssClass="btn" TabIndex="23" Text="Validate Selection" />&nbsp;&nbsp;
                                    <asp:Button ID="btnGenInvoice" runat="server" Enabled="false"  TabIndex="24" Text="Generate Invoice" OnClientClick="showProgress();" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="25" Text="Return To Search" />
                                </td>
                            </tr>                                                       
                            <tr>
                                <td colspan="6" align="left" style="padding-top: 8px;">
                                    <label class="field_caption" style="vertical-align:bottom; text-align:left;text-decoration:underline; font-weight:bold">Validation Status</label>
                                    <div id="div2" style="min-height: 370px; max-height: 370px; max-width:70vw;overflow-y:scroll">
                                    <asp:GridView ID="gvErrorList" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                        CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true" Width="100%"
                                        Style="padding-top: 3px; padding-bottom: 3px;" AllowSorting="true" TabIndex="26">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Request ID" SortExpression="requestId">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="ServiceDesc" HeaderText="Service Description" SortExpression="ServiceDesc" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Wrap="true"></asp:BoundField>

                                            <asp:BoundField DataField="errorMessage" HeaderText="Error Message" SortExpression="errorMessage"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true"></asp:BoundField>

                                            <asp:BoundField DataField="severity" HeaderText="Severity" SortExpression="severity"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true"></asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                        <FooterStyle CssClass="grdfooter" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle> 
                                    </asp:GridView>
                                    <asp:Label ID="lblValidateMsg" runat="server" Text="Validation Completed Successfully, Now Generate Invoice"
                                    Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                    Visible="False"></asp:Label>
                                    </div>                                    
                                </td>
                            </tr>
                            <tr>
                            <td colspan="6">
                                <div id="ShowReceipt" runat="server" style="overflow: scroll; height: 400px; width: 700px;
                                    border: 3px solid green; background-color: White; display: none">
                                    <table cellpadding="5px">
                                        <tr>
                                            <th colspan="3" id="thTitle" runat="server" align="center" class="field_heading">
                                                List of Receipts
                                            </th>
                                        </tr>                                        
                                        <tr>
                                            <td align="center">
                                                <asp:HiddenField ID="hdReadyIndex" runat="server" />
                                                <asp:HiddenField ID="hdRequestId" runat="server" />
                                                <asp:GridView ID="gvShowReceipt" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" CssClass="grdstyle" Width="660px" ShowHeaderWhenEmpty="true"
                                                    TabIndex="30" HeaderStyle-BorderColor="White">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox runat="server" ID="chkReceiptAll" CssClass="chkHeader" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkSelection" Checked='<%# Bind("selection") %>'
                                                                    Width="10px" CssClass="chkItem" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestid") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Voucher No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Bind("voucherno") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Voucher Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoucherDt" runat="server" Text='<%# Bind("voucherDate","{0:dd/MM/yyyy}") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Voucher Line No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoucherLineno" runat="server" Text='<%# Bind("voucherlineno") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>                                                        
                                                        <asp:TemplateField HeaderText="Voucher Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoucherType" runat="server" Text='<%# Bind("voucherType") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>                                                        
                                                        <asp:TemplateField HeaderText="Amount Received">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAmountReceived" runat="server" Text='<%# Bind("amountreceived") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Middle"></ItemStyle>
                                                        </asp:TemplateField>                                                                                                                
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnRecalculate" runat="server" CssClass="field_button" Text="Recalculate Amount Received" Width="250px"
                                                    TabIndex="31" />                                                
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnclose" runat="server" CssClass="field_button" Text="Close" Width="80px"
                                                    TabIndex="32" />
                                            </td>
                                        </tr>
                                    </table>
                                    <input id="btnInvisibleReceipt" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkayReceipt" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancelReceipt" type="button" value="Cancel" style="visibility: hidden" />
                                </div>
                                <asp:modalpopupextender id="ModalExtraPopup" runat="server" behaviorid="ModalExtraPopup"
                                    cancelcontrolid="btnCancelReceipt" okcontrolid="btnOkayReceipt" targetcontrolid="btnInvisibleReceipt"
                                    popupcontrolid="ShowReceipt" popupdraghandlecontrolid="PopupHeader" drag="true"
                                    backgroundcssclass="ModalPopupBG">
                                </asp:modalpopupextender>
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>                
            </table>


            <div>
            <center>

                    <div id="Loading1" runat="server" style="height: 150px; width: 500px; vertical-align: middle">
                    <img alt="" id="Img1" runat="server" src="~/Images/loader-progressbar.gif" width="150" />
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
            </div>
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>

