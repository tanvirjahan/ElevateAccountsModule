<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcessProvisionReversal.aspx.vb" Inherits="ExcessProvisionReversal" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            AutoCompleteSupplier_Supplier_KeyUp();

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
            var conkey = document.getElementById('<%=hdnDivcode.ClientID%>').value;
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
            var gvExcessProvision = document.getElementById("<%=gvExcessProvision.ClientID%>");
            var checkBoxes = gvExcessProvision.getElementsByTagName("input").length;
            for (var i = 0; i < checkBoxes; i++) {
                var node = gvExcessProvision.getElementsByTagName("input")[i];
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

        function AutoCompleteSupplier_Supplier_KeyUp() {
            $("#<%=txtSupplier.ClientID%>").bind("change", function () {
                var supplier = document.getElementById('<%=txtSupplier.ClientID%>');
                if (supplier.value == '') {
                    document.getElementById('<%=txtSupplierCode.ClientID%>').value = '';
                }
            });
            $("#<%=txtSupplier.ClientID%>").keyup("change", function () {
                var supplier = document.getElementById('<%=txtSupplier.ClientID%>');
                if (supplier.value == '') {
                    document.getElementById('<%=txtSupplierCode.ClientID%>').value = '';
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
            AutoCompleteSupplier_Supplier_KeyUp();
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
                        <asp:Label ID="lblHeading" runat="server" Text="Excess Provision Reversal" Style="padding: 2px"
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
                                        Journal No</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDocNo" CssClass="field_input" runat="server" TabIndex="1" Width="91%"
                                        Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hdnDivcode" runat="server" />
                                </td>
                                <td style="width: 13%">
                                    <label class="field_caption">
                                        Transaction Type</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtTranType" CssClass="field_input" runat="server" TabIndex="1"
                                        Width="91%" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hdnTranType" runat="server" />
                                </td>
                                <td style="width: 13%">
                                    <label class="field_caption">
                                        Journal Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtDocDate" CssClass="field_input" runat="server" Width="75px" TabIndex="2"></asp:TextBox>
                                    <asp:ImageButton ID="ImgBtnDocDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" />
                                    <asp:CalendarExtender ID="dpDocDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DocDateSelectCalExt"
                                        PopupButtonID="ImgBtnDocDate" PopupPosition="Right" TargetControlID="txtDocDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MeDocDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                        TargetControlID="txtDocDate"></asp:MaskedEditExtender>
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
                                        Type</label><span style="color: red" class="td_cell">&nbsp;*</span>
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
                                <td colspan="2">
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
                                <td></td>                               
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <label class="field_caption">
                                        Narration</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtNarration" CssClass="field_input" TextMode="MultiLine" runat="server"
                                        TabIndex="11" Width="96%"></asp:TextBox>
                                </td>                                
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblchkFromDt" runat="server" class="field_caption">
                                        From Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                    <asp:HiddenField ID="hdnChkDtFlag" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChkFromDt" CssClass="field_input" runat="server" TabIndex="12"
                                        onchange="filltodate(this);" Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="ChkFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkFromDt"
                                        PopupPosition="Right" TargetControlID="txtChkFromDt"></asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="ChkFromDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtChkFromDt"></asp:MaskedEditExtender>
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
                                        To Check Out Date</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChkToDt" CssClass="field_input" onchange="ValidateDate();" runat="server"
                                        TabIndex="13" Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtChkToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnChkToDt"
                                        PopupPosition="Right" TargetControlID="txtChkToDt"></asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="ChkToDt_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtChkToDt"></asp:MaskedEditExtender>
                                    <asp:ImageButton ID="imgBtnChkToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                        TabIndex="-1" /><br />
                                    <asp:MaskedEditValidator ID="MevChkToDt" runat="server" ControlExtender="ChkToDt_MaskedEditExtender"
                                        ControlToValidate="txtChkToDt" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="ChkToDt_MaskedEditExtender"
                                        InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </asp:MaskedEditValidator>
                                </td>
                                <td colspan="1">
                                <asp:CheckBox ID="chkValidate" runat="server" Text="Show only provision entries with Purchase Invoice (Only for Excel)"
                                        Checked="true" />
                                </td>
                                <td colspan="1">
                                <asp:CheckBox ID="ChkToCheckAsCutOff" runat="server" Text="As on To Check In Date (Only for Excel)"
                                        Checked="false" />
                                </td>
                                
                            </tr>   
                            
                            <%--changed by mohamed on 11/01/2022--%>

                            <tr>
                                <td style="vertical-align: top">
                                    <label class="field_caption">
                                        Booking Code</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtBookingCode" CssClass="field_input"  runat="server"></asp:TextBox>
                                </td>                                
                                <td colspan="4" class="field_caption">
                                  <asp:CheckBox ID="ChkExcGLProvision" runat="server" Text="Post to GL Provision instead of Supplier Provision"
                                        Checked="false" />
                                </td>           
                            </tr>
                                                     
                            <tr>
                                <td colspan="6" align="center">
                                    <asp:Button ID="btnDisplay" runat="server" OnClientClick="return ShowProgress();"
                                        CssClass="btn" TabIndex="14" Text="Display Provision" />&nbsp;&nbsp;
                                    <asp:Button ID="btnExportExcel" runat="server"  
                                        CssClass="btn" TabIndex="14" Text="Export To Excel" />&nbsp;&nbsp;
                                    <asp:Button ID="btnClear" runat="server" CssClass="btn" TabIndex="15" Text="Clear" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        <label class="gv_Title" id="lblHeadingGrid" runat="server">
                            List of Excess Provision</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:GridView ID="gvExcessProvision" runat="server" AutoGenerateColumns="False" CellPadding="3"
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
                                        <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%# Bind("requestId") %>' />
                                        <asp:HiddenField ID="hdnRlineNo" runat="server" Value='<%# Bind("rlineNo") %>' />
                                        <asp:HiddenField ID="hdnRoomNo" runat="server" Value='<%# Bind("roomNo") %>' />                                        
                                        <asp:HiddenField ID="hdnProvControlAcct" runat="server" Value='<%# Bind("ProvControlAcct") %>' />
                                        <asp:HiddenField ID="hdnProvVatAcct" runat="server" Value='<%# Bind("ProvVatAcct") %>' />
                                        <asp:HiddenField ID="hdnCostSalesDiffAcct" runat="server" Value='<%# Bind("CostSalesDiffAcct") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupName" runat="server" Text='<%# Bind("partyName") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnSupCode" runat="server" Value='<%# Bind("partyCode") %>' />
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
                                <asp:TemplateField HeaderText="Supplier Invoice No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupInvNo" runat="server" Text='<%# Bind("supplierInvoiceNo") %>'></asp:Label>
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
                                        <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%# Bind("servicetype") %>' />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrcode" runat="server" Text='<%# Bind("currcode") %>'></asp:Label>
                                        <asp:Label ID="lblConvrate" runat="server" Text='<%# Bind("convrate") %>'></asp:Label>                                        
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='Total'></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provision Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvAmt" runat="server" Text='<%# Bind("provisionAmount") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnProvAmt" runat="server" Value='<%# Bind("provisionAmount") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalProvAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provision VAT Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProvVatAmt" runat="server" Text='<%# Bind("provisionVatAmount") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnProvVatAmt" runat="server" Value='<%# Bind("provisionVatAmount") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalProvVatAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actual Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualAmt" runat="server" Text='<%# Bind("actualAmount") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnActualAmt" runat="server" Value='<%# Bind("actualAmount") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalActualAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actual VAT Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualVatAmt" runat="server" Text='<%# Bind("actualVatAmount") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnActualVatAmt" runat="server" Value='<%# Bind("actualVatAmount") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalActualVatAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Provision Reversal Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtReversalAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("reversalAmount") %>' OnTextChanged="txtReversalAmt_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalReversalAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reversal VAT Amount" HeaderStyle-CssClass="FixWidthCol">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtReversalVatAmt" Style="text-align: right" runat="server" Width="98%"
                                            Text='<%# Bind("reversalVatAmount") %>' OnTextChanged="txtReversalVatAmt_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalReversalVatAmt" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
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
                        <asp:Button ID="btnPdfReport" TabIndex="18" runat="server" Text="Pdf Report" Visible="false" CssClass="btn">
                        </asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="19" Text="Return To Search" />
                    </td>
                </tr>
            </table>
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
            <asp:PostBackTrigger ControlID="btnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnbookingno" runat="server" />    
    <asp:HiddenField ID="hdnSealDate" runat="server" /> 
    <asp:HiddenField ID="hdnDecimalplaces" runat="server" />
</asp:Content>

