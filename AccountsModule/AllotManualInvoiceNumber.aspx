<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="AllotManualInvoiceNumber.aspx.vb" Inherits="AllotManualInvoiceNumber" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <style type="text/css">
        .hiddencol
        {
            display: none;
        }
        .showcol
        {
            display: block;
        }
        .style1
        {
            height: 56px;
        }
        .filed_caption
        {
            margin-left: 0px;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
        });

        function showtextbox(dvtxtinvno, dvlbl, txtInvno) {

            document.getElementById(dvtxtinvno).setAttribute("Class", "showcol");
            document.getElementById(dvlbl).setAttribute("Class", "hiddencol");
            document.getElementById(txtInvno).focus();
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
                txtfromDate.value = txtToDate.value
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
 
    </script>
    <script type="text/javascript">

        function validateSearch() {
            var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
            var txttoDate = document.getElementById("<%=txtChkToDt.ClientID%>");

            if ((txtfromDate.value == null) || (txtfromDate.value == '')) {
                alert("Please select check out from date.");
                txtfromDate.focus();
                return false;
            }

            if ((txttoDate.value == null) || (txttoDate.value == '')) {
                alert("Please select check out to date.");
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
        function ShowProgress() {
        
            var ModalPopupLoading = $find("ModalPopupLoading");

            if (validateSearch() == true) {
                
                ModalPopupLoading.show();
                return true;
            }
            else {
                return false;
            }
             }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function InitializeRequestUserControl(sender, args) {

        }

        function EndRequestUserControl(sender, args) {

        }        
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%">
                <table style="border: gray 2px solid; width: 100%" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Allot Manual Invoice Number" CssClass="field_heading"
                                Width="100%" ForeColor="White" Style="padding: 2px"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td colspan="4" style="padding: 10px 0px 12px 0px;" align="center">
                            <asp:Button ID="btnHelp" runat="server" Text="Help" Font-Bold="False" CssClass="search_button"
                                Style="display: none"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False"
                                CssClass="btn" Style="display: none"></asp:Button>
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Style="display: none" Text="Report" />
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False"
                                Text="Export To Excel" Style="display: none" />
                            <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table style="width: 100%" cellpadding="10px">
                                <tr>
                                    <td style="width: 15%;">
                                        <label class="field_caption">
                                            From Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:TextBox ID="TextBox1" runat="server" Style="display: none"> </asp:TextBox>
                                        <asp:TextBox ID="txtChkFromDt" CssClass="field_input" runat="server" TabIndex="1"
                                            onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                        <asp:CalendarExtender ID="ChkFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkFromDt"
                                            PopupPosition="Right" TargetControlID="txtChkFromDt">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="ChkFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtChkFromDt">
                                        </asp:MaskedEditExtender>
                                        <asp:ImageButton ID="ImgBtnChkFromDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                            TabIndex="-1" /><br />
                                        <asp:MaskedEditValidator ID="MevChkFromDt" runat="server" ControlExtender="ChkFromDt_MaskedEditExtender"
                                            ControlToValidate="txtChkFromDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="ChkFromDt_MaskedEditExtender"
                                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                            TooltipMessage="Input a Date in Date/Month/Year">
                                        </asp:MaskedEditValidator>
                                    </td>
                                    <td style="width: 15%">
                                        <label class="field_caption">
                                            To Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtChkToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                            TabIndex="2" Width="75px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtChkToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkToDt"
                                            PopupPosition="Right" TargetControlID="txtChkToDt">
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
                                    <td style="width: 15%">
                                    </td>
                                    <td style="width: 15%">
                                    </td>
                                </tr>
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
                                            ServiceMethod="GetRequestId" TargetControlID="txtBookingNo" UseContextKey="true"
                                            OnClientPopulating="AutoCompleteBookingNo_OnClientPopulating">
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
                                            ServiceMethod="GetCustomers" TargetControlID="txtCustName" UseContextKey="true"
                                            OnClientPopulating="AutoCompleteCustName_OnClientPopulating">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="field_caption">
                                            Supplier Name</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSupName" CssClass="field_input" runat="server" TabIndex="6" Width="90%"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteSupName" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                            ServiceMethod="GetSuppliers" TargetControlID="txtSupName" UseContextKey="true"
                                            OnClientPopulating="AutoCompleteSupName_OnClientPopulating">
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
                                            ServiceMethod="GetguestNames" TargetControlID="txtGuestName" UseContextKey="true"
                                            OnClientPopulating="AutoCompleteGuestName_OnClientPopulating">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="field_caption">
                                            Customer Reference</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustRef" CssClass="field_input" runat="server" TabIndex="8" Width="90%"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteCustRef" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                            ServiceMethod="GetAgentRef" TargetControlID="txtCustRef" UseContextKey="true"
                                            OnClientPopulating="AutoCompleteCustRef_OnClientPopulating">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <label class="field_caption">
                                            Booking Type</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBookingType" CssClass="field_input" runat="server" Width="90%"
                                            TabIndex="9">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label class="field_caption">
                                            From Request Date</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReqFromDt" CssClass="field_input" runat="server" TabIndex="10"
                                            onchange="fillRequestTodate(this);" Width="75px"></asp:TextBox>
                                        <asp:CalendarExtender ID="ReqFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            PopupButtonID="ImgBtnReqFromDt" OnClientDateSelectionChanged="RequestDateSelectCalExt"
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
                                        <asp:TextBox ID="txtReqToDt" CssClass="field_input" runat="server" TabIndex="11"
                                            onchange="ValidateRequestDate();" Width="75px"></asp:TextBox>
                                        <asp:CalendarExtender ID="ReqToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            PopupButtonID="ImgBtnReqToDt" OnClientDateSelectionChanged="RequestDateSelectCalExt"
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
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 8px;" colspan="4">
                            <asp:Button ID="btnDisplay" runat="server" CssClass="btn" TabIndex="12" Text="Display "
                                OnClientClick="return ShowProgress();" />&nbsp;&nbsp;
                            <asp:Button ID="btnReset" runat="server" CssClass="btn" TabIndex="13" Text="Clear" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="4">
                            <table width="100%;">
                                <tr>
                                    <td align="left">
                                        <label class="gv_Title">
                                            Search Result
                                        </label>
                                    </td>
                                    <td align="right" style="padding-right: 10px">
                                        <%--    <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                    <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true" TabIndex="12">
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="15">15</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="25">25</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                    </asp:DropDownList>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="hdReadyIndex" runat="server" />
                            <asp:HiddenField ID="hdRequestId" runat="server" />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div id="divGrid" style="min-height: 370px; max-height: 370px; max-width: 95vw; overflow: scroll">
                                        <asp:GridView ID="gvReadyInvoice" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                            CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                            Width="100%" Style="padding-top: 3px; padding-bottom: 3px;" AllowSorting="true"
                                            TabIndex="14">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelectAll" CssClass="chkHeader" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelection" Checked='<%# Bind("selection") %>'
                                                            Width="10px" CssClass="chkItem" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Request ID" SortExpression="RequestId">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("RequestId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--   <asp:BoundField DataField="Status" HeaderText="" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>--%>
                                                <asp:BoundField DataField="Amended" HeaderText="Amended" SortExpression="Amended"
                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true"></asp:BoundField>
                                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="ArrivalDate"
                                                    SortExpression="ArrivalDate" HeaderText="Arrival Date"></asp:BoundField>
                                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="DepartureDate"
                                                    SortExpression="DepartureDate" HeaderText="Departure Date"></asp:BoundField>
                                                <asp:BoundField DataField="agentName" HeaderText="Customer Name" SortExpression="agentName"
                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Customer Ref." SortExpression="agentRef">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAgentRef" runat="server" Text='<%# Bind("agentRef") %>' ItemStyle-Wrap="true"></asp:Label>
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
                                                <asp:TemplateField HeaderText="Received">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceivedStatus" runat="server" Text='<%# Bind("ReceivedStatus") %>'
                                                            Style="display: none"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <%--                                         <asp:TemplateField HeaderText="Proforma Invoice">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnProforma" Text="View" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="Proforma" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>   --%>
                                                <asp:TemplateField HeaderText="Proforma Invoice ">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnProformavat" Text="View" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                            CommandName="ProformaVat" ForeColor="Blue" runat="server"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice No.">
                                                    <ItemTemplate>
                                                        <div id="dvlblinvno" runat="server">
                                                            <asp:Label ID="lblInvno" runat="server" Text='<%# Bind("manualinvoiceno") %>'></asp:Label>
                                                        </div>
                                                        <div id="dvtxtinvno" class="hiddencol" runat="server">
                                                            <asp:TextBox ID="txtInvno" Text='<%# Bind("manualinvoiceno") %>' runat="server" Style="width: 100px"></asp:TextBox>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="30px" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <%--             <asp:LinkButton ID="lbtnProforma" Text="View" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                   CommandName="ShowInvoiceNo" ForeColor="Blue" runat="server"></asp:LinkButton>--%>
                                                        <%--     <asp:Button ID="btnsave" runat="server" CssClass="btn" TabIndex="14" Text=".." Width="20px"
                                                                                OnClick="btnsave_Click" /> --%>
                                                        <img style="width: 25px; border-width: 0px" id="Imggvedit" type="image" src="../Images/crystaltoolbar/edit.png"
                                                            alt="edit" runat="server" />
                                                        <asp:ImageButton ID="ImageSave" runat="server" Style="width: 20px; border-width: 0px;"
                                                            CommandArgument='<%# Container.DisplayIndex %>' CommandName="SaveInvoiceNo" ToolTip="save"
                                                            ImageUrl="../Images/crystaltoolbar/Saveicon9.jpg" />
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
                                        </asp:GridView>
                                        <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                            Visible="False"></asp:Label>
                                        <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="display: none" />
                                    </div>
                                    <div id="ShowRoomtypes" runat="server" style="overflow: scroll; height: 300px; width: 250px;
                                        border: 3px solid green; background-color: White; display: none">
                                        <table style="float: left">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOk1" runat="server" CssClass="field_button" Text="Ok" Width="80px" />&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnmealok" runat="server" CssClass="field_button" Text="Ok" Width="80px"
                                                        Style="display: none" />&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClear1" runat="server" CssClass="field_button" Text="Close" Width="80px" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <input id="Button1" runat="server" type="button" value="Cancel" style="display: none" />
                                        <input id="btnOkayEB" type="button" value="OK" style="display: none" />
                                        <input id="btnCancelEB" type="button" value="Cancel" style="display: none" />
                                    </div>
                                    <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
                                    <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                        CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                        PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                        BackgroundCssClass="ModalPopupBG">
                                    </cc1:ModalPopupExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
    <input id="btnCloseLoading" runat="server" type="button" value="Cancel" style="display: none" />
</asp:Content>
