<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptSuppConsolidateCommInvoice.aspx.vb" Inherits="RptSuppConsolidateCommInvoice" %>

<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            supplierAutoCompleteExtenderKeyUp();
        });
        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");
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
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");

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

        function ValidateChkInDate() {

            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
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
                alert("Todate date should not be greater than From date");
            }
        }


        function supplierautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
            }
        }

        function supplierAutoCompleteExtenderKeyUp() {
            $("#<%= txtsuppliername.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtsuppliername.ClientID%>').value == '') {
                    document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
                }
            });

            $("#<%= txtsuppliername.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtsuppliername.ClientID%>').value == '') {
                    document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
                }
            });
        }

        function validation() {
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfromDate.value == "") {
                alert("Enter From date");
                return false;
            }
            if (txtToDate.value == "") {
                alert("Enter To Date");
                return false;
            }
            return true;
        }

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) {
        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            supplierAutoCompleteExtenderKeyUp();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100%; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="1">
                            <asp:Label ID="lblHeading" runat="server" Text="Supplier Consolidated Commission Invoice"
                                ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 60%; padding:5px" cellpadding="5px" >
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" runat="server" GroupingText=" Select Date" CssClass="td_cell"
                                            Width="100%">
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="width:10%">
                                                            <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                                                        </td>
                                                        <td style="width:30%">
                                                            <asp:TextBox ID="txtFromDate" CssClass="field_input" runat="server" onchange="filltodate(this);"
                                                                Width="75px" TabIndex="1"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <asp:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                                                PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                TargetControlID="txtFromDate">
                                                            </asp:MaskedEditExtender>
                                                            <br />
                                                            <asp:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                            </asp:MaskedEditValidator>
                                                        </td>
                                                        <td style="width:10%">
                                                            <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                                                        </td>
                                                        <td style="width:30%">
                                                            <asp:TextBox ID="txtToDate" CssClass="fiel_input" runat="server" onchange="ValidateChkInDate();"
                                                                Width="75px" TabIndex="2"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <asp:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                                                PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                                            </asp:CalendarExtender>
                                                            <asp:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                TargetControlID="txtToDate">
                                                            </asp:MaskedEditExtender>
                                                            <br />
                                                            <asp:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                                ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                            </asp:MaskedEditValidator>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel1" runat="server" GroupingText=" Select Filters" CssClass="td_cell"
                                            Width="100%">
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td class="td_cell" style="width:12%">
                                                            Supplier
                                                        </td>
                                                        <td class="td_cell" >
                                                            <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input" Height="16px"
                                                                MaxLength="500" TabIndex="3" Width="310px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="supplierautocompleteselected"
                                                                ServiceMethod="Getsupplierlist" ContextKey="true" TargetControlID="txtsuppliername">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txtsuppliercode" Style="display: none" runat="server" Width="100px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnPdfReport" TabIndex="4" runat="server" Text="Pdf Report" OnClientClick="return validation();" CssClass="btn"
                                            Width="122px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
