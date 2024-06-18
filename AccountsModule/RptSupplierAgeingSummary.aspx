<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptSupplierAgeingSummary.aspx.vb"
    Inherits="RptSupplierAgeingSummary" MasterPageFile="~/AccountsMaster.master"
    Strict="true" %>

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
            CurrencyAutoCompleteExtenderKeyUp();
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

            CurrencyAutoCompleteExtenderKeyUp();
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
        function CurrencyAutoCompleteExtenderKeyUp() {

            $("#<%= Txtcurrname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=Txtcurrname.ClientID%>').value == '') {

                    document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
                }

            });

            $("#<%= Txtcurrname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=Txtcurrname.ClientID%>').value == '') {

                    document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
                }

            });

        }





        function currencyautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=Txtcurrcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
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

                    document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
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

                //           ddlm1.style .visibility="hidden";
                //           ddlm2.style .visibility="hidden";
                //           ddlm3.style .visibility="hidden";
                //           ddlm4.style .visibility="hidden";
                //           lbl1.style .visibility="hidden";
                //           lbl2.style .visibility="hidden";

            }
            else {
                ddlm1.disabled = false;
                ddlm2.disabled = false;
                ddlm3.disabled = false;
                ddlm4.disabled = false;

                //           ddlm1.style .visibility="visible";
                //           ddlm2.style .visibility="visible";
                //           ddlm3.style .visibility="visible";
                //           ddlm4.style .visibility="visible";
                //           lbl1.style .visibility="visible";
                //           lbl2.style .visibility="visible";

            }

        }


        function SetsupplierContextkey() {

            var contxt = '';

            var ctrycode = document.getElementById("<%=txtctrycode.ClientID%>").value;
            var ctryname = document.getElementById("<%=txtctryname.ClientID%>").value;
            var citycode = document.getElementById("<%=txtcitycode.ClientID%>").value;
            var cityname = document.getElementById("<%=txtcityname.ClientID%>").value;

            var supptypecode = document.getElementById("<%=txtsuptypecode.ClientID%>").value;
            var supptypename = document.getElementById("<%=txtsuptypename.ClientID%>").value;



            var categorycode = document.getElementById("<%=txtcatcode.ClientID%>").value;
            var categoryname = document.getElementById("<%=txtcatname.ClientID%>").value;

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

        function FormValidation() {
            if ((document.getElementById("<%=txtFromDate.ClientID%>").value == "") || (document.getElementById("<%=ddlReportGroup.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlCurrency.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlReportOrder.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlAgeingType.ClientID%>").value == "[Select]")) {
                if (document.getElementById("<%=txtFromDate.ClientID%>").value == "") {
                    document.getElementById("<%=txtFromDate.ClientID%>").focus();
                    alert("As on date field can not be blank.");
                    return false;
                }
                else if (document.getElementById("<%=ddlReportGroup.ClientID%>").value == "[Select]") {
                    document.getElementById("<%=ddlReportGroup.ClientID%>").focus();
                    alert("Select report group.");
                    return false;
                }
                //           else if (document.getElementById("<%=ddlIncludeZero.ClientID%>").value=="[Select]")
                //           {
                //            document.getElementById("<%=ddlIncludeZero.ClientID%>").focus();
                //            alert("Select include zero.");
                //            return false;
                //            }
                else if (document.getElementById("<%=ddlCurrency.ClientID%>").value == "[Select]") {
                    document.getElementById("<%=ddlCurrency.ClientID%>").focus();
                    alert("Select currency type.");
                    return false;
                }
                else if (document.getElementById("<%=ddlReportOrder.ClientID%>").value == "[Select]") {
                    document.getElementById("<%=ddlReportOrder.ClientID%>").focus();
                    alert("Select report order.");
                    return false;
                }

                else if (document.getElementById("<%=ddlAgeingType.ClientID%>").value == "[Select]") {
                    document.getElementById("<%=ddlAgeingType.ClientID%>").focus();
                    alert("Select ageing type.");
                    return false;
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100%; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="1">
                            <asp:Label ID="lblHeading" runat="server" Text="Customer Ageing Summary" ForeColor="White"
                                Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    

                         <tr>
                                <td style="width: 100%">
                                    <table style="width: 400x">
                                        <tbody>
                                            <tr>
                                                <td style="width: 219px" class="td_cell">
                                                <asp:Label ID="lblsupplier" runat="server" Text=" Select Suppiler / Supplier Agent"></asp:Label>
                                                </td>
                                                <td style="width: 84px" class="td_cell">
                                                    <asp:DropDownList ID="ddlSupType" runat="server" Width="125px" CssClass="drpdown"
                                                        OnSelectedIndexChanged="ddlSupType_SelectedIndexChanged" 
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
                        <td style="width: 100%; height: 24px" class="td_cell">
        
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%; height: 174px;">
                                                         <asp:Panel ID="Panel1" runat="server" Width="100%" CssClass="td_cell" 
                                                GroupingText="Select  Date" Height="151px">
                                            <table style="width: 578px">
                                                <tbody>
                           
                                                    <tr>
                                                        <td class="td_cell">
                                                            As On Date
                                                        </td>
                                                        <td class="td_cell" style="width: 170px">
                                                            <asp:TextBox ID="txtFromDate" TabIndex="2" runat="server" Width="80px" CssClass="txtbox"></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                            </asp:ImageButton>
                                                            <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" CssClass="field_error" ControlExtender="MskFromDate"
                                                                ControlToValidate="txtFromDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                                                ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                                TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                        </td>
                                                        <td class="td_cell">
                                                            &nbsp;Currency Type
                                                        </td>
                                                        <td class="td_cell">
                                                            <select ID="ddlCurrency" runat="server" class="drpdown" name="D2" 
                                                                style="width: 120px" tabindex="5"      >
                                                                <option selected="" value="[Select]">[Select]</option>
                                                            </select>
                                                            
                                                            
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_cell">
                                                            Report Group</td>
                                                        <td class="td_cell" style="width: 170px">
                                                           <select ID="ddlReportGroup" runat="server" class="drpdown" name="D1" 
                                                                style="width: 120px" tabindex="3">
                                                                <option selected="" value="[Select]">[Select]</option>
                                                                <option value="None">None</option>
                                                                <option value="Control Account">Control Account</option>
                                                                <option value="Category">Category</option>
                                                                <option value="Country">Country</option>
                                                                <option value="City">City</option>
                                                            </select></td>
                                                        <td class="td_cell">
                                                            Ageing Type
                                                        </td>
                                                        <td class="td_cell">
                                                            <select style="width: 120px" id="ddlAgeingType" class="drpdown" tabindex="6" 
                                                                runat="server">
                                                                <option value="Month" selected>Month</option>
                                                                <option value="Date">Date</option>
                                                                <option value="Due Date">Due Date</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_cell">
                                                            Report Order
                                                        </td>
                                                        <td class="td_cell" style="width: 170px">
                                                            <select style="width: 120px" id="ddlReportOrder" class="drpdown" tabindex="4" 
                                                                runat="server">
                                                                <option value="[Select]" selected>[Select]</option>
                                                                <option value="Code">Code</option>
                                                                <option value="Name">Name</option>
                                                            </select>
                                                        </td>
                                                        <td class="td_cell">
                                                            Supplier Type
                                                        </td>
                                                        <td class="td_cell">
                                                            <select style="width: 120px" id="ddlSupplierType" class="drpdown" tabindex="7" 
                                                                runat="server">
                                                                <option value="All" selected>All</option>
                                                                <option value="Cash Supplier">Cash Supplier</option>
                                                                <option value="Credit Supplier">Credit Supplier</option>
                                                            </select>
                                                        </td>
                                                        <td class="td_cell">
                                                            <select style="width: 109px" id="ddlIncludeZero" class="drpdown" tabindex="7" runat="server"
                                                                visible="false">
                                                                <option value="Yes" selected>Yes</option>
                                                                <option value="No">No</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_cell">
                                                            Include Proforma</td>
                                                        <td class="td_cell" style="width: 170px">
                                                            <select ID="ddlproforma" runat="server" class="field_input" name="D3" 
                                                                style="width: 117px" tabindex="1">
                                                                <option value="Yes">Yes</option>
                                                                <option selected="" value="No">No</option>
                                                            </select></td>
                                                        <td>
                                                           <asp:Label ID="lblcurrency" runat="server" Text=" Currency"></asp:Label>
                                  
                                                        </td>

                                                        
                      <td colspan=2>
                            <asp:TextBox ID="TxtCurrName" runat="server" AutoPostBack="True" 
                            CssClass="field_input" MaxLength="500" TabIndex="5" Width="100px"></asp:TextBox>
                            <asp:TextBox ID="TxtCurrCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdnCurrcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtCurrName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcurrencylist" TargetControlID="TxtCurrName" OnClientItemSelected="currencyautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text5" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text6" class="field_input" type="text"
                             runat="server" />                          
                            
                            </td>
                   
 
                     
                      
                                                    
                                                    </tr>
                                                </tbody>
                                            </table>
                                            </asp:Panel> 
                                        </td>
                                        <td style="width: 100px; height: 174px;">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 16px">
                                            <asp:Panel ID="Panel3" runat="server" Width="100%" CssClass="td_cell" 
                                                GroupingText="Selection  Criteria">
                                                <table style="width: 578px">
                                                    <tbody>
                                                       <tr>
                                                            <td class="td_cell" style="height: 17px; width: 205px;">
                                                                Control Account
                                                            </td>
                                                            <td class="td_cell" style="height: 17px; width: 262px;">
                                                                <asp:TextBox ID="TxtBankName" runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="8" Width="310px" Height="16px"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnbankname" runat="server" />
                                                                <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px">
                                                               <asp:TextBox ID="TxtBankCode" style="display:none" runat="server" Width="100px"
                                                               ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                         
                                                           
                                                        
                                                        <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                Country
                                                            </td>
                                                            <td class="td_cell" style="width: 262px">
                                                                <asp:TextBox ID="txtctryname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="9" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtctryname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    OnClientItemSelected="ctryautocompleteselected" ServiceMethod="Getctrylist" TargetControlID="txtctryname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField4" runat="server"   />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtctrycode" runat="server" style="display:none" Width="100px"  ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                City
                                                            </td>
                                                            <td class="td_cell" style="width: 262px">
                                                                <asp:TextBox ID="txtcityname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="10" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtcityname_AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    OnClientItemSelected="cityautocompleteselected" ContextKey="True"  ServiceMethod="Getcitylist" TargetControlID="txtcityname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField5" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtcitycode" runat="server" style="display:none" Width="100px" ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                              Category
                                                            </td>
                                                            <td class="td_cell" style="width: 262px">
                                                                <asp:TextBox ID="txtcatname" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="11" Width="310px"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtcatcode" runat="server" style="display:none" Width="100px"   ></asp:TextBox>
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
                                                                    Height="16px" MaxLength="500" TabIndex="12" Width="310px"></asp:TextBox>
                                                                     
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
                                                                Supplier/Supplier Agent
                                                            </td>
                                                            <td class="td_cell" style="width: 262px">
                                                                <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="13" Width="310px"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtsuppliercode" style="display:none" runat="server" Width="100px"   ></asp:TextBox>
                                                                      </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 100px; height: 16px">
                                            &nbsp;</td>
                                    </tr>
                          
                                </tbody>
                            </table>
                            
                        </td>
                    </tr>
                    <tr>
                          <td style="text-align: center" class="td_cell" align="right">
                            <input style="visibility: hidden; width: 12px; height: 9px" id="txtconnection" type="text"
                                runat="server" />&nbsp;<asp:Button ID="Button1" OnClick="Button1_Click1" runat="server"
                                    Text="Export" __designer:dtid="4222124650660080" CssClass="btn" __designer:wfdid="w1">
                                </asp:Button>&nbsp;
                            <asp:Button ID="btnReport" TabIndex="14" OnClick="btnReport_Click" runat="server"
                                Text="Load Report" style="display:none" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;
                                 <asp:Button ID="btnPdfReport" TabIndex="14" OnClick="btnPdfReport_Click" runat="server"
                                Text="Pdf Report" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;
                                 <asp:Button ID="btnExlReport" TabIndex="14" OnClick="btnExlReport_Click" runat="server"
                                Text="Excel Report" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;<asp:Button
                                    ID="btnhelp" TabIndex="15" OnClick="btnhelp_Click" runat="server" Text="Help"
                                    CssClass="btn"></asp:Button>
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
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                TargetControlID="txtFromDate">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                MaskType="Date" MessageValidatorTip="true">
            </cc1:MaskedEditExtender>
            &nbsp;
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
