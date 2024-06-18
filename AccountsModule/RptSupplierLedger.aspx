<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptSupplierLedger.aspx.vb"
    Inherits="RptSupplierLedger" MasterPageFile="~/AccountsMaster.master" Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
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

            costgrpAutoCompleteExtenderKeyUp();
            supplierAutoCompleteExtenderKeyUp();
            catAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            ctryAutoCompleteExtenderKeyUp();
            suppliertypeAutoCompleteExtenderKeyUp();
        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            costgrpAutoCompleteExtenderKeyUp();
            supplierAutoCompleteExtenderKeyUp();
            catAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            ctryAutoCompleteExtenderKeyUp();
            suppliertypeAutoCompleteExtenderKeyUp();
            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }


        function suppliertypeAutoCompleteExtenderKeyUp() {
            $("#<%= txtsuptypename.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtsuptypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtsuptypename.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtsuptypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
        }
        function catautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcatcode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                SetsupplierContextkey();
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
        function costgrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtbankcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtbankcode.ClientID%>').value = '';
            }

        }

        function cityautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                SetsupplierContextkey();
            }

        }
        function ctryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtctrycode.ClientID%>').value = eventArgs.get_value();

                $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey(eventArgs.get_value());
                document.getElementById('<%=txtcityname.ClientID%>').value = '';
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                SetsupplierContextkey();


            }
            else {
                document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                SetsupplierContextkey();
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

        function costgrpAutoCompleteExtenderKeyUp() {
            $("#<%= txtbankname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtbankname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });
        }
        function catAutoCompleteExtenderKeyUp() {
            $("#<%= txtcatname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcatname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtcatname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcatname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
        }
        function cityAutoCompleteExtenderKeyUp() {
            $("#<%= txtcityname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcityname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtcityname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcityname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
        }
        function ctryAutoCompleteExtenderKeyUp() {
            $("#<%= txtctryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtctryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                    $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtctryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtctryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                    $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                    SetsupplierContextkey();
                }

            });
        }
        function rbevent(rb1, rb2, Opt, Group) {
            var rb2 = document.getElementById(rb2);
            rb1.checked = true;
            rb2.checked = false;


            if (Opt == 'A') {
                ddlm1.value = '[Select]';
                ddlm2.value = '[Select]';
                ddlm3.value = '[Select]';
                ddlm4.value = '[Select]';

                ddlm1.disabled = true;
                ddlm2.disabled = true;
                ddlm3.disabled = true;
                ddlm4.disabled = true;
            }
            else {
                ddlm1.disabled = false;
                ddlm2.disabled = false;
                ddlm3.disabled = false;
                ddlm4.disabled = false;
            }

        }

        function suppliertypeautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtsuptypecode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                SetsupplierContextkey();
            }

        }

        function SetsupplierContextkey() {

            var contxt = '';

            var ctrycode = document.getElementById("<%=txtctrycode.ClientID%>").value;
            var ctryname = document.getElementById("<%=txtctryname.ClientID%>").value;
            var citycode = document.getElementById("<%=txtcitycode.ClientID%>").value;
            var cityname = document.getElementById("<%=txtcityname.ClientID%>").value;

            var categorycode = document.getElementById("<%=txtcatcode.ClientID%>").value;
            var categoryname = document.getElementById("<%=txtcatname.ClientID%>").value;
            var supptypecode = document.getElementById("<%=txtsuptypecode.ClientID%>").value;
            var supptypename = document.getElementById("<%=txtsuptypename.ClientID%>").value;


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




            if (categoryname == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + categorycode;
            }


            if (supptypename == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + supptypecode;
            }



            // $find('AutoCompleteExtender_txtBankName').set_contextKey(contxt);
            $find('<%=txtsuppliername_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

        }


        //-----    Common

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }

        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { txttdate.value = txtfdate.value; }
        }

    </script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        width: 100%; border-bottom: gray 2px solid">
        <tbody>
            <tr>
                <td class="field_heading" align="center" colspan="1" style="width: 100%">
                    <asp:Label ID="lblHeading" runat="server" Text="Supplier Ledger" ForeColor="White"
                        Width="100%" CssClass="field_heading" Height="16px"></asp:Label>
                </td>
            </tr>

            <tr>
                <td style="width: 100%; height: 24px" class="td_cell">
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td style="width: 100%">
                                    <table style="width: 350px">
                                        <tbody>
                                            <tr>
                                                <td style="width: 304px" class="td_cell">
                                                    Suppiler/ Supplier Agent
                                                </td>
                                                <td style="width: 84px" class="td_cell" colspan=2>
                                                    <asp:DropDownList ID="ddlSupplierType" runat="server" Width="125px" CssClass="drpdown"
                                                        OnSelectedIndexChanged="ddlsupptype_SelectedIndexChanged" 
                                                        AutoPostBack="True" TabIndex="1">
                                                        <asp:ListItem Value="S">Supplier</asp:ListItem>
                                                        <asp:ListItem Value="A">Supplier Agent</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%" GroupingText="Select Date">
                                        <table style="width: 437px">
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell" style="width: 48px">
                                                        &#160;From
                                                    </td>
                                                    <td class="td_cell" style="width: 293px">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" Width="80px" 
                                                            TabIndex="2"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="2">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate"
                                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                            Width="23px"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td class="td_cell" style="width: 24px">
                                                        To
                                                    </td>
                                                    <td class="td_cell" style="width: 276px">
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtbox" Width="80px" 
                                                            TabIndex="3"></asp:TextBox>&#160;<asp:ImageButton
                                                            ID="ImgBtnRevDate" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MskVToDate" runat="server" ControlExtender="MskToDate"
                                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                            <td style="width: 196px">
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 196px">
                                    <asp:Panel ID="Panel4" runat="server" Width="100%" CssClass="td_cell" 
                                        GroupingText="Selection Criteria">
                                        <table>
                                            <tbody>
                                                  <tr>
                                                            <td class="td_cell" style="height: 17px; width: 205px;">
                                                                &nbsp;Control Account
                                                            </td>
                                                            <td class="td_cell" style="height: 17px; width: 338px;">
                                                                <asp:TextBox ID="TxtBankName" runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="4" Width="310px" Height="16px"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnbankname" runat="server" />
                                                                <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px">
                                                               <asp:TextBox ID="TxtBankCode" runat="server" style="display:none"  Width="100px"
                                                               ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                               
                                                     
                                                       
                                                        <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                &nbsp;Country
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtctryname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="5" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtctryname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    OnClientItemSelected="ctryautocompleteselected" ServiceMethod="Getctrylist" TargetControlID="txtctryname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField4" runat="server"   />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtctrycode" runat="server" Width="100px" style="display:none"  ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                &nbsp;City
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtcityname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="6" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtcityname_AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    OnClientItemSelected="cityautocompleteselected" ContextKey="True"  ServiceMethod="Getcitylist" TargetControlID="txtcityname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField5" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtcitycode" runat="server" Width="100px" style="display:none"   ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>

                                                           <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                &nbsp;Category
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtcatname" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="7" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtcatname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    OnClientItemSelected="catautocompleteselected" ServiceMethod="Getcategorylist"
                                                                    TargetControlID="txtcatname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtcatcode" runat="server" Width="100px" style="display:none"     ></asp:TextBox>
                                                                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                 &nbsp;Type
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtsuptypename" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="8" Width="310px"></asp:TextBox>
                                                              
                                                                      
                                                        <asp:AutoCompleteExtender ID="txtsuptypename_AutoCompleteExtender1" runat="server"
                                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                            FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="suppliertypeautocompleteselected"
                                                            ServiceMethod="Getsuppliertypelist" TargetControlID="txtsuptypename">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="HiddenField6" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtsuptypecode" runat="server" Width="100px" style="display:none"     ></asp:TextBox>
                                                                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                &nbsp;Supplier/Supplier Agent
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="9" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                    FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="supplierautocompleteselected"
                                                                    ServiceMethod="Getsupplierlist" ContextKey="true"  TargetControlID="txtsuppliername">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtsuppliercode" runat="server" style="display:none"  Width="100px"    ></asp:TextBox>
                                                                      </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td_cell" style="width: 104%">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 211px" class="td_cell">
                                    &nbsp;Ledger Type
                                </td>
                                <td style="width: 100px">
                                    <select style="width: 109px" id="ddlLedgerType" class="drpdown" runat="server" 
                                        tabindex="10">
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
                <td class="td_cell" style="height: 24px; width: 104%;">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 210px" class="td_cell">
                                    &nbsp;Do you print PDC Details ?&nbsp;
                                </td>
                                <td style="width: 100px">
                                    <select style="width: 109px" id="ddlPDC" class="drpdown" runat="server" 
                                        tabindex="10">
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
                <td class="td_cell" style="width: 104%">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 210px" class="td_cell">
                                    &nbsp;Do you want to print Party currency &nbsp;or Base currency ?
                                </td>
                                <td style="width: 100px">
                                    <select style="width: 109px" id="ddlCurrency" class="drpdown" runat="server" 
                                        tabindex="11">
                                        <option value="[Select]" selected>[Select]</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td_cell" align="center" style="width: 104%">
                    <input style="visibility: hidden; width: 12px; height: 9px" id="txtconnection" type="text"
                        runat="server" />&nbsp;&nbsp;<asp:Button ID="Button1" OnClick="Button1_Click1" runat="server"
                            Text="Export" __designer:dtid="3659174697238721" CssClass="btn" __designer:wfdid="w2">
                        </asp:Button>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnReport" OnClick="btnReport_Click"
                            runat="server" Text="Load Report" CssClass="btn" style="display:none"
                        CausesValidation="False" TabIndex="12">
                    </asp:Button>&nbsp;<asp:Button ID="btnhelp" TabIndex="13" OnClick="btnhelp_Click"
                        runat="server" Text="Help" CssClass="btn"></asp:Button>&nbsp;
                        <asp:Button ID="btnPdfReport" OnClick="btnPdfReport_Click"
                            runat="server" Text="Pdf Report" CssClass="btn" 
                        CausesValidation="False" TabIndex="12">
                    </asp:Button>&nbsp;
                    <asp:Button ID="btnExlReport" OnClick="btnExlReport_Click"
                            runat="server" Text="Excel Report" CssClass="btn" 
                        CausesValidation="False" TabIndex="12">
                    </asp:Button>
                </td>
                <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
       </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>


            </tr>

        </tbody>
    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
    <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
        TargetControlID="txtFromDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="ClsExToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnRevDate"
        TargetControlID="txtToDate">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
        AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
        MaskType="Date" MessageValidatorTip="true">
    </cc1:MaskedEditExtender>
    <cc1:MaskedEditExtender ID="MskToDate" runat="server" TargetControlID="txtToDate"
        AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
        MaskType="Date" MessageValidatorTip="true">
    </cc1:MaskedEditExtender>
</asp:Content>
