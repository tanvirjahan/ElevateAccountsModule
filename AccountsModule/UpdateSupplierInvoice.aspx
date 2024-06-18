<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="UpdateSupplierInvoice.aspx.vb" Inherits="UpdateSupplierInvoice" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

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
        });

        function DocDateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtDocDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpDocDate.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
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

        function ValidateDate() {

            var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
            var txtToDate = document.getElementById("<%=txtChkToDt.ClientID%>");
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

        function Supplier_OnClientPopulating(sender, args) {
            var conkey = document.getElementById('<%=txtDivCode.ClientID%>').value;
            var conkey = conkey + "|" + $('#<%=ddlType.ClientID %> option:selected').text();
            sender.set_contextKey(conkey);
        }

        function Supplier_OnClientItemSelected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtSupplierCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtSupplierCode.ClientID%>').value = '';
            }
        }

        function validateSearch() {
            var txtfromDate = document.getElementById("<%=txtChkFromDt.ClientID%>");
            var txttoDate = document.getElementById("<%=txtChkToDt.ClientID%>");
            var chkDtFlag = document.getElementById("<%=hdnChkDtFlag.ClientID%>");

            if ((txtfromDate.value == null) || (txtfromDate.value == '')) 
            {
                if (chkDtFlag.value=='Y')
                {
                    alert("Please select check in from date.");
                }
                else
                {
                    alert("Please select check out from date.");
                }
                txtfromDate.focus();
                return false;
            }

            if ((txttoDate.value == null) || (txttoDate.value == '')) {
                 if (chkDtFlag.value=='Y')
                {
                     alert("Please select check in to date.");
                }
                else
                {
                    alert("Please select check out to date.");
                }
                txttoDate.focus();
                return false;
            }


            return true;
        }
        function ShowProgresssave() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            if (validateSearch() == true) {
                ModalPopupLoading.show();
                return true;
            }
            else
                return false;
        }

        function ShowProgress() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            if (validateSearch() == true) {
                ModalPopupLoading.show();
                return true;
            }
            else
                return false;
        }

        function ShowProgrespopssave() {
            var ModalPopupLoading = $find("ModalPopupLoading");
            ModalPopupLoading.show();
            return true;
        }
        function CompleteValidate(state) {

            var BookingSelectValid = false;
            var gvUpdateSupplier = document.getElementById("<%=gvUpdateSupplier.ClientID%>");
            var checkBoxes = gvUpdateSupplier.getElementsByTagName("input").length;
            for (var i = 0; i < checkBoxes; i++) {
                var node = gvUpdateSupplier.getElementsByTagName("input")[i];
                if (node != null && node.type == "checkbox" && node.checked) {
                    BookingSelectValid = true;
                    return true;
                    break;
                }
            }
            if (BookingSelectValid == false) {
                alert("Select rows in the list");
                return false;
            }


        }

    </script>
    <style type="text/css">
        .gv_Title
        {
            font-family: Arial,Verdana, Geneva, ms sans serif;
            font-size: 11pt;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            border-width: 1px;
            border-color: #06788B;
            color: #06788B;
            margin-left: 0px;
        }
        
        .FixWidthCol
        {
            min-width: 100px;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="Update Supplier Invoice" Style="padding: 2px"
                            CssClass="field_heading" Width="100%">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%" cellpadding="7">
                            <tr>
                                <td style="width: 13%">
                                    <label class="field_caption">
                                        Purchase Document No</label>
                                </td>
                                <td style="width: 37%">
                                    <asp:TextBox ID="txtDocNo" CssClass="field_input" runat="server" TabIndex="1" Width="91%"
                                        Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtDivcode" runat="server" Style="display: none"></asp:TextBox>
                                </td>
                                <td style="width: 13%">
                                    <label class="field_caption">
                                        Purchase Document Date</label>
                                </td>
                                <td style="width: 37%">
                                    <asp:TextBox ID="txtDocDate" CssClass="field_input" runat="server" Width="75px" TabIndex="2"></asp:TextBox>
                                    <asp:ImageButton ID="ImgBtnDocDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" />
                                    <asp:CalendarExtender ID="dpDocDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DocDateSelectCalExt"
                                        PopupButtonID="ImgBtnDocDate" PopupPosition="Right" TargetControlID="txtDocDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MeDocDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtDocDate">
                                    </asp:MaskedEditExtender>
                                    <br />
                                    <asp:MaskedEditValidator ID="MevDocDate" runat="server" ControlExtender="MeDocDate"
                                        ControlToValidate="txtDocDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="field_caption">
                                        Type</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="field_input" TabIndex="3"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <label class="field_caption">
                                        Supplier Name</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupplier" CssClass="field_input" runat="server" TabIndex="4"
                                        Width="91%" AutoPostBack="true"></asp:TextBox>
                                    <asp:TextBox ID="txtSupplierCode" runat="server" TabIndex="-1" Style="display: none"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteSupplier" runat="server" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                        ServiceMethod="GetSuppliers" TargetControlID="txtSupplier" UseContextKey="true"
                                        OnClientPopulating="Supplier_OnClientPopulating" OnClientItemSelected="Supplier_OnClientItemSelected">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="field_caption">
                                        Control Account</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCtrlAcct" CssClass="field_input" runat="server" TabIndex="5"
                                        Width="91%" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtCtrlAcctCode" runat="server" TabIndex="-1" Style="display: none"></asp:TextBox>
                                </td>
                                <td>
                                    <label class="field_caption">
                                        Currency</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrency" CssClass="field_input" runat="server" TabIndex="6"
                                        Width="91%" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtCurrencyCode" runat="server" TabIndex="-1" Style="display: none"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="field_caption">
                                        Conversion Rate</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConvRate" CssClass="field_input" runat="server" TabIndex="7"
                                        Width="91%" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <label class="field_caption">
                                        TRN No</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTrnNo" CssClass="field_input" runat="server" TabIndex="8" Width="91%"
                                        Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="field_caption">
                                        Supplier Invoice No.</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" CssClass="field_input" runat="server" TabIndex="9"
                                        Width="91%"></asp:TextBox>
                                </td>
                                <td>
                                    <label class="field_caption">
                                        Booking Reference No.</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRequestId" CssClass="field_input" runat="server" TabIndex="10"
                                        Width="91%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <label class="field_caption">
                                        Narration</label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNarration" CssClass="field_input" TextMode="MultiLine" runat="server"
                                        TabIndex="11" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblchkFromDt" runat="server" class="field_caption">
                                        From Check Out Date</label>
                                    <asp:HiddenField ID="hdnChkDtFlag" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChkFromDt" CssClass="field_input" runat="server" TabIndex="12"
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
                                <td>
                                    <label id="lblChkToDt" runat="server" class="field_caption">
                                        To Check Out Date</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChkToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                        TabIndex="13" Width="75px"></asp:TextBox>
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
                                <td>
                                    <label class="field_caption" style="margin-left: -351px;">
                                        Invoice Type
                                    </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCommissionType" runat="server" CssClass="field_input" Style="margin-left: -267px;
                                        width: 195px;" TabIndex="14" AutoPostBack="true" OnSelectedIndexChanged="ddlCommissionType_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Non Commissionable" Value="NonCommissionable"></asp:ListItem>
                                        <asp:ListItem Text="Commissionable" Value="Commissionable"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnDisplay" runat="server" OnClientClick="return ShowProgress();"
                                        CssClass="btn" TabIndex="14" Text="Display Provision" />
                                    <asp:Button ID="btnClear" runat="server" CssClass="btn" TabIndex="15" Text="Clear" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        <label class="gv_Title">
                            List of Hotel Rooms</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <%--   <div id="divGrid" style="min-height: 400px; max-height: 400px; max-width: 96vw; overflow: scroll">--%>
                        <asp:GridView ID="gvUpdateSupplier" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                            Width="100%" Style="padding-top: 2px; padding-bottom: 3px;" TabIndex="16" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelectAll" CssClass="chkHeader" AutoPostBack="true"
                                            OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkSelection" Checked='<%# Bind("selection") %>'
                                            Width="10px" CssClass="chkItem" AutoPostBack="true" OnCheckedChanged="chkSelection_CheckedChanged" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestId" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                                        <asp:TextBox ID="txtRlineNo" runat="server" Style="display: none" Text='<%# Bind("rlineNo") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtRoomNo" runat="server" Style="display: none" Text='<%# Bind("roomNo") %>'></asp:TextBox>
                                        <asp:Label ID="lblPaxType" runat="server" Style="display: none" Text='<%# Bind("PaxType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check In">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCheckIn" runat="server" Text='<%# Bind("checkin","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check Out">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCheckOut" runat="server" Text='<%# Bind("checkOut","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pax Details">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPax" runat="server" Text='<%# Bind("paxdetails") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Guest Details">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGuestDetails" runat="server" Text='<%# Bind("GuestDetails") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Details">
                                    <ItemTemplate>
                                        <asp:Label ID="lblService" runat="server" Text='<%# Bind("servicedetails") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceType" runat="server" Text='<%# Bind("servicetype") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Conf. No." HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupConfNo" runat="server" Text='<%# Bind("supconfno") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='Total'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provision Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvAmt" runat="server" Text='<%# Bind("provisionAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalProvAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT Input Prov. Amt." HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVatInputProvAmt" runat="server" Text='<%# Bind("vatprovision") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalVatInputProvAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actual Amount">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnActualAmt" runat="server" Text='<%# bind("actualAmount") %>'
                                            CommandName="ShowPrice" CommandArgument='<%# Container.DisplayIndex %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalActualAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Non Taxable Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtNonTaxAmt" Style="text-align: right;display:none" runat="server" Width="98%"
                                            Text='<%# Bind("prices_costnontaxablevaluebase") %>' AutoPostBack="true" OnTextChanged="txtNonTaxAmt_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtNonTaxAmtBase" Style="text-align: right;" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalNonTaxAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTaxAmt" Style="text-align: right;display:none" runat="server" Width="98%"
                                            Text='<%# Bind("prices_costtaxablevaluebase") %>' AutoPostBack="true" OnTextChanged="txtTaxAmt_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtTaxAmtBase" Style="text-align: right;" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalTaxAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT %" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtVatPerc" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("vatperc") %>' AutoPostBack="true" OnTextChanged="txtVatPerc_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Vat Amount" HeaderStyle-CssClass="FixWidthCol"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrentCostVatAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("prices_costvatvaluebase") %>'></asp:Label>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vat Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCostVatAmt" Style="text-align: right;display:none" runat="server" Width="98%"
                                            Text='<%# Bind("prices_costvatvaluebase") %>' AutoPostBack="true" OnTextChanged="txtVatAmt_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtCostVatAmtBase" Style="text-align: right;" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalVatAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTotalAmt" Style="text-align: right;display:none" runat="server" Width="98%"
                                            Enabled="false" Text='<%# Bind("totalprice") %>'></asp:TextBox>
                                        <asp:TextBox ID="txtTotalAmtBase" Style="text-align: right;" runat="server" Width="98%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblNetTotalAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                

                                <asp:TemplateField HeaderText="Commission" HeaderStyle-CssClass="FixWidthCol" Visible="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCommisionAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("Commission") %>' AutoPostBack="true" OnTextChanged="txtVatAmt_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblNetCommisionAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Commission Amount" HeaderStyle-CssClass="FixWidthCol"
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommisionAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("Commission") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Non-Taxable Amount" HeaderStyle-CssClass="FixWidthCol"
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblnontaxAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("prices_costnontaxablevaluebase") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Taxable Amount" HeaderStyle-CssClass="FixWidthCol"
                                    Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltaxAmt" Style="text-align: right" runat="server" Width="98%" Text='<%# Bind("prices_costtaxablevaluebase") %>'></asp:Label>
                                    </ItemTemplate>
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
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding-top: 8px;">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="17" Text="Save"
                            OnClientClick="return ShowProgresssave();" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="18" Text="Return To Search" />
                    </td>
                </tr>
            </table>
            <div id="ShowPriceList" runat="server" style="overflow-y: scroll; height: 400px;
                width: 1100px; border: 3px solid green; background-color: White; display: none;">
                <table cellpadding="7px" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label1" runat="server" Text="Booking Price Breakup" Style="padding-top: 4px;
                                padding-bottom: 5px;" CssClass="field_heading" Width="100%"></asp:Label>
                            <asp:HiddenField ID="hdnDocNo" runat="server" />
                            <asp:HiddenField ID="hdnType" runat="server" />
                            <asp:HiddenField ID="hdPriceRequestId" runat="server" />
                            <asp:HiddenField ID="hdPriceRlineNo" runat="server" />
                            <asp:HiddenField ID="hdPriceRoomNo" runat="server" />
                            <asp:HiddenField ID="hdServiceType" runat="server" />
                            <asp:HiddenField ID="hdCurrenttotalvatactual" runat="server" />
                            <asp:HiddenField ID="hdCurrenttotalCommissionactual" runat="server" />
                            <asp:HiddenField ID="hdcurrenttaxactual" runat="server" />
                            <asp:HiddenField ID="hdcurrentnontaxactual" runat="server" />
                            <asp:HiddenField ID="currentcommissionamt" runat="server" />
                            <asp:HiddenField ID="hdVatOldValue" runat="server" />
                            <asp:HiddenField ID="currentnontaxamt" runat="server" />
                            <asp:HiddenField ID="currenttaxamt" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:HiddenField ID="HdnPaxType" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                    </tr>
                    <tr style="padding-top: 0%">
                        <td align="left">
                            <asp:Label ID="lblvatprovision" class="field_input" Style="width: 9.4%; padding-right: 1%"
                                Text="Total Vat Provision" runat="server"></asp:Label>
                            <asp:TextBox ID="txttotalvatprovison" class="field_input" Style="width: 9.4%; text-align: right"
                                Enabled="false" runat="server"></asp:TextBox>
                            <asp:Label ID="lbltotalvatactual" class="field_input" Style="width: 9.4%; padding-right: 1%"
                                Text="Total Vat Actual" runat="server"></asp:Label>
                            <asp:TextBox ID="txttotalvatactual" class="field_input" Style="width: 9.4%; text-align: right"
                                AutoPostBack="true" runat="server" OnTextChanged="txttotalvatactual_textchanged"></asp:TextBox>
                            <asp:Label ID="lblvatperc" class="field_input" Style="width: 10.4%;" Text="Vat %"
                                runat="server"></asp:Label>
                            <asp:TextBox ID="txtvatpercpop" class="field_input" Style="width: 9.4%; text-align: right"
                                AutoPostBack="true" runat="server" OnTextChanged="txtvatpercpop_textchanged"></asp:TextBox>
                            <asp:Label ID="lblselectamount" class="field_input" Style="width: 10.4%;" Text="Select"
                                runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlfillamount" CssClass="field_input" Style="width: 13.5%"
                                runat="server" TabIndex="3">
                                <asp:ListItem Text="Cost Price" Value="CostPrice"></asp:ListItem>
                                <asp:ListItem Text="Non Taxable Amount" Value="NonTaxableAmount"></asp:ListItem>
                                <asp:ListItem Text="Taxable Amount" Value="TaxableAmount"></asp:ListItem>
                                <asp:ListItem Text="Vat Amount" Value="VatAmount"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtfillAmount" class="field_input" Style="width: 9.4%; text-align: right"
                                runat="server"></asp:TextBox>
                            <asp:Button ID="btnfillCostPricepopup" CssClass="btn" Style="width: 11%;" Text="Fill "
                                runat="server"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="gvPriceList" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" CssClass="grdstyle" Width="100%" ShowHeaderWhenEmpty="true"
                                TabIndex="25" Font-Bold="true" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Stay Date" HeaderStyle-Width="14%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstayDate" runat="server" Text='<%# Bind("pricedate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pax Type" HeaderStyle-Width="18%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaxType" runat="server" Text='<%# Bind("paxtype") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No. Of Pax" HeaderStyle-Width="18%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoOfPax" runat="server" Text='<%# Bind("noofpax") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Child Ages" HeaderStyle-Width="18%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChildAges" runat="server" Text='<%# Bind("childages") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booking Code" HeaderStyle-Width="27%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBookingCode" runat="server" Text='<%# Bind("bookingcode") %>'></asp:Label>
                                            <asp:Label ID="lblBookingName" runat="server" Style="display: none" Text='<%# Bind("bookingname") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="true" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblBookinCode" runat="server" Text='Total'></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pax Rate" HeaderStyle-Width="27%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblRateTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sale Price" HeaderStyle-Width="19%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalePrice" Style="text-align: right" runat="server" Text='<%# Bind("salePrice") %>'></asp:Label>
                                            <asp:TextBox ID="txtcurrcode" runat="server" Style="display: none" Text='<%# Bind("currcode") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalePriceTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                        <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sale Price" HeaderStyle-Width="19%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalePriceBase" Style="text-align: right" runat="server" Text='<%# Bind("salePricebase") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalePriceBaseTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                        <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Price" HeaderStyle-CssClass="FixWidthCol" HeaderStyle-Width="13%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCostPrice" Style="text-align: right" Width="98%" runat="server"
                                                Text='<%# Bind("costPrice") %>' AutoPostBack="true" OnTextChanged="txtCostPrice_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblCostPriceTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Value" HeaderStyle-CssClass="FixWidthCol" HeaderStyle-Width="13%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCostValue" Style="text-align: right" Width="98%" runat="server"
                                                Text='<%# Bind("costvalue") %>' AutoPostBack="true" OnTextChanged="txtCostValue_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblCostValueTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Non Taxable Amount" HeaderStyle-CssClass="FixWidthCol"
                                        HeaderStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNonTaxAmount" Style="text-align: right" Width="98%" runat="server"
                                                Text='<%# Bind("CostNonTaxableValue")%>' AutoPostBack="true" OnTextChanged="txtNonTaxAmountPop_TextChanged"></asp:TextBox></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNonTaxAmountTotal" runat="server">
                                            </asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Taxable Amount" HeaderStyle-CssClass="FixWidthCol"
                                        HeaderStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTaxAmount" Style="text-align: right" Width="98%" runat="server"
                                                Text='<%#Bind("CostTaxableValue")%>' AutoPostBack="true" OnTextChanged="txtTaxAmountPop_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTaxAmountTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="FixWidthCol" HeaderText="VatAmount" HeaderStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCostVatAmount" Width="98%" Style="text-align: right" AutoPostBack="true"
                                                OnTextChanged="txtCostVatAmountPop_TextChanged" runat="server" Text='<%#Bind("CostVatValue") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblCostVatAmountTotal" Style="text-align: right" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="FixWidthCol" HeaderText="Commission" Visible="False"
                                        HeaderStyle-Width="21%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCommissionAmount" Width="98%" Style="text-align: right" AutoPostBack="true"
                                                OnTextChanged="txtCommissionAmountPop_TextChanged" runat="server" Text='<%#Bind("Commission") %>'>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblCommissionAmountTotal" Style="text-align: right" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                <PagerStyle CssClass="grdpagerstyle"></PagerStyle>
                                <HeaderStyle CssClass="grdheader" ForeColor="White" BorderColor="LightGray"></HeaderStyle>
                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                <FooterStyle CssClass="grdfooter" />
                            </asp:GridView>
                            <asp:HiddenField ID="hdVatValueOldMainGrid" runat="server" />
                            <asp:HiddenField ID="hdnPricedateFlag" runat="server" />
                            <asp:Label ID="lblMsgPrice" runat="server" Text="Records not found, Please check booking details"
                                Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnPriceSave" runat="server" OnClientClick="ShowProgrespopssave();"
                                CssClass="field_button" Text="Save" Width="80px" TabIndex="26" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnClose" runat="server" CssClass="field_button" Text="Cancel" Width="80px"
                                TabIndex="27" />
                        </td>
                    </tr>
                </table>
                <input id="btnInvisiblePriceList" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                <input id="btnOkayPriceList" type="button" value="OK" style="visibility: hidden" />
                <input id="btnCancelPriceList" type="button" value="Cancel" style="visibility: hidden" />
            </div>
            <asp:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                CancelControlID="btnCancelPriceList" OkControlID="btnOkayPriceList" TargetControlID="btnInvisiblePriceList"
                PopupControlID="ShowPriceList" PopupDragHandleControlID="ShowPriceList" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
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
    </asp:UpdatePanel>
    <%--    <asp:CheckBox ID="checkbox1" runat="server" />--%>
    <asp:HiddenField ID="hdnCommissionFlag" Value="false" runat="server" />    
 <%--   <asp:HiddenField ID="hdnsuppno" runat="server" />
    <asp:HiddenField ID="hdnbookingno" runat="server" />
    <asp:HiddenField ID="hdnnarration" runat="server" />--%>
    <asp:HiddenField ID="hdnSealDate" runat="server" />
</asp:Content>
