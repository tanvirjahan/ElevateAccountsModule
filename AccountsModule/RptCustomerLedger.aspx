<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustomerLedger.aspx.vb"
    Inherits="RptCustomerLedger" MasterPageFile="~/AccountsMaster.master" Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {

            txtcategorynameAutoCompleteExtenderKeyUp();
            txtcustomernameAutoCompleteExtenderKeyUp();
            txtmarketnameAutoCompleteExtenderKeyUp();
            txtcountrynameAutoCompleteExtenderKeyUp();
            txtcontrolnameAutoCompleteExtenderKeyUp();
            txtcustgroupAutoCompleteExtenderKeyUp();


        });
    </script>
    <script language="javascript" type="text/javascript">



        function txtcustgroupAutoCompleteExtenderKeyUp() {
            $("#<%= Txtcustgroupname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=Txtcustgroupname.ClientID%>').value == '') {

                    document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });

            $("#<%= Txtcustgroupname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=Txtcustgroupname.ClientID%>').value == '') {

                    document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });
        }


        function txtmarketnameAutoCompleteExtenderKeyUp() {
            $("#<%= txtmarketname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtmarketname.ClientID%>').value == '') {

                    document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });

            $("#<%= txtmarketname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtmarketname.ClientID%>').value == '') {

                    document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });
        }


        function txtcustomernameAutoCompleteExtenderKeyUp() {
            $("#<%= txtcustomername.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcustomername.ClientID%>').value == '') {

                    document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcustomername.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcustomername.ClientID%>').value == '') {

                    document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
                }

            });
        }

        function txtcontrolnameAutoCompleteExtenderKeyUp() {
            $("#<%= txtcontrolname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcontrolname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcontrolname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcontrolname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
                }

            });
        }

        function txtcategorynameAutoCompleteExtenderKeyUp() {
            $("#<%= txtcategoryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });

            $("#<%= txtcategoryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                    SetcustomerContextkey();
                }

            });
        }

        function txtcountrynameAutoCompleteExtenderKeyUp() {
            $("#<%= txtcountryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                    $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
                    SetcustomerContextkey();
                }

            });

            $("#<%= txtcountryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                    $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
                    SetcustomerContextkey();
                }

            });
        }

        function txtcustomernameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%= txtcustomercode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
            }
        }
        function txtmarketnameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%= txtmarketcode.ClientID%>').value = eventArgs.get_value();
                SetcustomerContextkey();
            }
            else {
                document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }
        }

        function txtcontrolnameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcontrolcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
            }
        }

        function txtcategorynameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcategorycode.ClientID%>').value = eventArgs.get_value();
                SetcustomerContextkey();
            }
            else {
                document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                SetcustomerContextkey();
            }
        }


        function txtcountrynamecompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
                $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey(eventArgs.get_value());
                document.getElementById('<%=txtmarketname.ClientID%>').value = '';
                document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }
            else {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
                SetcustomerContextkey();
            }
        }



        function SetcustomerContextkey() {

            var contxt = '';

            var ctrycode = document.getElementById("<%=txtcountrycode.ClientID%>").value;
            var ctryname = document.getElementById("<%=txtcountryname.ClientID%>").value;
            var citycode = document.getElementById("<%=txtmarketcode.ClientID%>").value;
            var cityname = document.getElementById("<%=txtmarketname.ClientID%>").value;
            var custgroupcode = document.getElementById("<%=Txtcustgroupcode.ClientID%>").value;
            var custgroupname = document.getElementById("<%=Txtcustgroupname.ClientID%>").value;
            var categorycode = document.getElementById("<%=txtcategorycode.ClientID%>").value;
            var categoryname = document.getElementById("<%=txtcategoryname.ClientID%>").value;

            if (ctryname == '') {
                contxt = '';
            }
            else {
                contxt = ctrycode;
            }




            if (cityname == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + citycode;
            }


            if (custgroupname == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + custgroupcode;
            }


            if (categoryname == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + categorycode;
            }





            // $find('AutoCompleteExtender_txtBankName').set_contextKey(contxt);
            $find('<%=txtcustomername_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

        }

        function txtcustgroupnamecompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = eventArgs.get_value();
                SetcustomerContextkey();
                document.getElementById('<%=txtcustomername.ClientID%>').value = '';
                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
            }
            else {
                document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';

            }
            SetcustomerContextkey();
        }

        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { txttdate.value = txtfdate.value; }
        }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            txtcategorynameAutoCompleteExtenderKeyUp();
            txtmarketnameAutoCompleteExtenderKeyUp();
            txtcustomernameAutoCompleteExtenderKeyUp();
            txtcontrolnameAutoCompleteExtenderKeyUp();
            txtcountrynameAutoCompleteExtenderKeyUp();
            txtcustgroupAutoCompleteExtenderKeyUp();
        }
    </script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        width: 100%; border-bottom: gray 2px solid">
        <tbody>
            <tr>
                <td class="field_heading" align="center" colspan="1">
                    <asp:Label ID="lblHeading" runat="server" Text="Customer Ledger" ForeColor="White"
                        CssClass="field_heading" Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; height: 24px" class="td_cell">
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Panel ID="Panel6" runat="server" CssClass="td_cell" Width="100%" __designer:wfdid="w22"
                                        GroupingText="Select Date">
                                        <table style="width: 550px">
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell" style="width: 48px">
                                                        <asp:Label ID="lblagdate" runat="server" CssClass="td_cell" Text="Ageing as On Date"
                                                            Width="130px"></asp:Label>
                                                    </td>
                                                    <td class="td_cell" style="width: 259px">
                                                        <asp:TextBox ID="txtagDate" TabIndex="1" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txtagDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                            PopupButtonID="ImgBtnagDate" TargetControlID="txtagDate"></cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="txtagDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                            MaskType="Date" TargetControlID="txtagDate"></cc1:MaskedEditExtender>
                                                        <asp:ImageButton ID="ImgBtnagDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                        <cc1:MaskedEditValidator ID="MEagDate" runat="server" ControlExtender="txtagDate_MaskedEditExtender"
                                                            ControlToValidate="txtagDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                            EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format">
                                                        </cc1:MaskedEditValidator>
                                                    </td>
                                                    <td class="td_cell" style="width: 24px">
                                                        &nbsp;
                                                    </td>
                                                    <td class="td_cell" style="width: 269px">
                                                        &nbsp;
                                                        <asp:Label ID="lblageing" runat="server" Text="Ageing Type" Visible="false"></asp:Label>
                                                    </td>
                                                    <td width="100">
                                                        <select id="ddlAgeing" runat="server" class="field_input" name="D1" onchange="CallWebMethod('toacccode');"
                                                            style="width: 158px" visible="false">
                                                            <option selected="" value="Month">Month</option>
                                                            <option value="Date">Date</option>
                                                            <option value="Due Date">Due Date</option>
                                                        </select>&nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="td_cell" style="width: 48px">
                                                        &#160;From
                                                    </td>
                                                    <td class="td_cell" style="width: 259px">
                                                        <asp:TextBox ID="txtFromDate" TabIndex="2" runat="server" __designer:wfdid="w13"
                                                            CssClass="txtbox" Width="80px"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" __designer:wfdid="w14" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>&#160;<br />
                                                        <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" __designer:wfdid="w15" ControlExtender="MskFromDate"
                                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                            Width="23px"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td class="td_cell" style="width: 24px">
                                                        To
                                                    </td>
                                                    <td class="td_cell" style="width: 269px">
                                                        <asp:TextBox ID="txtToDate" TabIndex="3" runat="server" __designer:wfdid="w16" CssClass="txtbox"
                                                            Width="80px"></asp:TextBox>&#160;<asp:ImageButton ID="ImgBtnRevDate" runat="server"
                                                                __designer:wfdid="w17" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton>&#160;<cc1:MaskedEditValidator
                                                                    ID="MskVToDate" runat="server" __designer:wfdid="w18" ControlExtender="MskToDate"
                                                                    ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                                    EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                                    TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td width="100">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="td_cell" Width="100%" __designer:wfdid="w22"
                                        GroupingText="Select Criteria">
                                        <asp:Panel ID="Panel22" runat="server" CssClass="td_cell" Width="477px" __designer:wfdid="w22">
                                            <table style="width: 650px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label1" runat="server" Text="Control A/C " Width="88px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                            <asp:TextBox ID="txtcontrolname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="4" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="txtcontrolcode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtcontrolname_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" ServiceMethod="Getcontrolname"
                                                                TargetControlID="txtcontrolname" OnClientItemSelected="txtcontrolnameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text3" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text4" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="td_cell" Width="477px" __designer:wfdid="w22">
                                            <table style="width: 700px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label4" runat="server" Text="Country" Width="60px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="200px">
                                                            <asp:TextBox ID="txtcountryname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="5" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="txtcountrycode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField4" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtcountryname_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" ServiceMethod="Getcountry"
                                                                TargetControlID="txtcountryname" OnClientItemSelected="txtcountrynamecompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="td_cell" Width="477px" __designer:wfdid="w22">
                                            <table style="width: 700px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label3" runat="server" Text="City" Width="50px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                            <asp:TextBox ID="txtmarketname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="6" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="txtmarketcode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField3" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtmarketname_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" ContextKey="True" ServiceMethod="Getcity"
                                                                TargetControlID="txtmarketname" OnClientItemSelected="txtmarketnameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="td_cell" Width="477px" __designer:wfdid="w22">
                                            <table style="width: 700px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label2" runat="server" Text="Category" Width="65px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                            <asp:TextBox ID="txtcategoryname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="7" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="txtcategorycode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField21" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtcategoryname_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" ServiceMethod="Getcategory"
                                                                TargetControlID="txtcategoryname" OnClientItemSelected="txtcategorynameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel5" runat="server" CssClass="td_cell" Width="500px" __designer:wfdid="w22">
                                            <table style="width: 700px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label6" runat="server" Text="Customer Group" Width="114px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                            <asp:TextBox ID="Txtcustgroupname" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="8" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="Txtcustgroupcode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField2" runat="server" />
                                                            <asp:AutoCompleteExtender ID="custgroupExtender2" runat="server" CompletionInterval="10"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                ServiceMethod="Getcustgroup" TargetControlID="Txtcustgroupname" OnClientItemSelected="txtcustgroupnamecompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text11" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text12" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel7" runat="server" CssClass="td_cell" Width="500px" __designer:wfdid="w22">
                                            <table style="width: 700px; height: 28px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label5" runat="server" Text="Customer" Width="65px"></asp:Label><%--<span
                                                                style="color: #ff0000">*</span>--%>
                                                        </td>
                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                            <asp:TextBox ID="txtcustomername" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="9" Width="300px"></asp:TextBox>
                                                            <asp:TextBox ID="txtcustomercode" Style="display: none" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField5" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtcustomername_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="-1" ServiceMethod="Getcustomer"
                                                                TargetControlID="txtcustomername" ContextKey="True" OnClientItemSelected="txtcustomernameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td style="width: 260px" class="td_cell">
                                                    &nbsp;<asp:Label ID="lblledgertype" runat="server" Text="Ledger Type"></asp:Label>
                                                </td>
                                                <td style="width: 100px">
                                                    <select style="width: 109px" tabindex="10" id="ddlLedgerType" class="drpdown" runat="server">
                                                        <option value="0" selected>Summary</option>
                                                        <option value="1">Detailed</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td style="width: 260px" class="td_cell">
                                                    &nbsp;<asp:Label ID="lblpdcdet" runat="server" Text="Do you Print PDC Details?"></asp:Label>
                                                </td>
                                                <td style="width: 100px">
                                                    <select style="width: 109px" tabindex="11" id="ddlPDC" class="drpdown" runat="server">
                                                        <option value="Yes" selected>Yes</option>
                                                        <option value="No">No</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td style="width: 260px" class="td_cell">
                                                    &nbsp;Do you want to print Party currency &nbsp;or Base currency ?
                                                </td>
                                                <td style="width: 100px">
                                                    <select style="width: 109px" tabindex="12" id="ddlCurrency" class="drpdown" runat="server">
                                                        <option value="[Select]" selected>[Select]</option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_cell" align="center">
                                    &nbsp;<asp:Button ID="Button1" OnClick="Button1_Click1" TabIndex="13" runat="server"
                                        Text="Export" __designer:dtid="1970324836974798" CssClass="btn" __designer:wfdid="w1">
                                    </asp:Button>&nbsp;
                                    <asp:Button ID="btnReport" OnClick="btnReport_Click" TabIndex="14" runat="server"
                                        Text="Load Report" CssClass="btn" CausesValidation="False" style="display:none" ></asp:Button>&nbsp;
                                    <asp:Button ID="btnPdfReport" TabIndex="14" OnClick="btnPdfReport_Click" runat="server"
                                        Text="Pdf Report" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;
                                    <asp:Button ID="btnExlReport" TabIndex="14" OnClick="btnExlReport_Click" runat="server"
                                        Text="Excel Report" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;
                                    <asp:Button ID="btnhelp" TabIndex="15" OnClick="btnhelp_Click" runat="server" Text="Help"
                                        Height="20px" CssClass="btn" __designer:wfdid="w3"></asp:Button>
                                </td>
                                <td style="display: none">
                                    <asp:Button ID="btnadd" TabIndex="16" runat="server" CssClass="field_button"></asp:Button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="None" CssClass="td_cell" Font-Size="10px">
                </asp:GridView>
                <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                    <Services>
                        <asp:ServiceReference Path="../clsServices.asmx"></asp:ServiceReference>
                    </Services>
                </asp:ScriptManagerProxy>
                <cc1:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtFromDate"
                    PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                <cc1:CalendarExtender ID="ClsExToDate" runat="server" __designer:wfdid="w19" TargetControlID="txtToDate"
                    PopupButtonID="ImgBtnRevDate" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                    MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                    DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MskToDate" runat="server" __designer:wfdid="w20" TargetControlID="txtToDate"
                    MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                    DisplayMoney="Left" AcceptNegative="Left"></cc1:MaskedEditExtender>
                &nbsp;&nbsp;
            </tr>
        </tbody>
    </table>
</asp:Content>
