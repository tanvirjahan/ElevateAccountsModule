<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="DefaultPostingAccts.aspx.vb" Inherits="DefaultPostingAccts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/PriceListModule/Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
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
            visualsearchbox();
            AutoCompleteExtenderUserControlKeyUp();
        });

        var glcallback;

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        function visualsearchbox() {

            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('CountryGroup:" " Region:" " Country:" " Text:" "');

            window.visualSearch = VS.init({
                container: $('#search_box_container'),
                query: $txtvsprocess.val(),
                showFacets: true,
                readOnly: false,
                unquotable: [
            'text',
            'account',
            'filter',
            'access'
          ],
                placeholder: 'Search for',
                callbacks: {
                    search: function (query, searchCollection) {
                        var $query = $('.search_query');
                        $query.stop().animate({ opacity: 1 }, { duration: 300, queue: false });
                        $query.html('<span class="raquo">&raquo;</span> You searched for: <b>' + searchCollection.serialize() + '</b>');

                        var $txtvsprocess = $(document).find('.cs_txtvsprocess');
                        $txtvsprocess.val(visualSearch.searchQuery.serialize());

                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit());

                        var btnvsprocess = document.getElementById("<%=btnvsprocess.ClientID%>");
                        btnvsprocess.click();

                        clearTimeout(window.queryHideDelay);
                        window.queryHideDelay = setTimeout(function () {
                            $query.animate({
                                opacity: 0
                            }, {
                                duration: 1000,
                                queue: false
                            });
                        }, 2000);
                    },
                    valueMatches: function (category, searchTerm, callback) {
                        switch (category) {
                            case 'CountryGroup':
                                var asSqlqry = 'select ltrim(rtrim(countrygroupname)) countrygroupname  from countrygroup where active=1';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Region':
                                var asSqlqry = 'select ltrim(rtrim(plgrpname)) plgrpname  from plgrpmast where active=1';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Country':
                                var asSqlqry = 'select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'Text' },
                { label: 'CountryGroup', category: 'location' },
                { label: 'Region', category: 'location' },
                { label: 'Country', category: 'location' },
              ]);
                    }
                }
            });
        }

        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
        }

        function AutoCompleteIncomeAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtIncome.ClientID%>').value);
        }

        function AutoCompleteVatPayableAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtVatPayable.ClientID%>').value);
        }

        function AutoCompleteCostSalesAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtCostSale.ClientID%>').value);
        }

        function AutoCompleteProvVatCrAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtProvVatCredit.ClientID%>').value);
        }

        function AutoCompleteVatCrAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtVatCredit.ClientID%>').value);
        }

        function AutoCompleteSalesDiffAcct_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value + "|" + document.getElementById('<%=txtSalesDiff.ClientID%>').value);
        }

        function AutoCompleteProvCrCtrlAcct_OnClientPopulating(sender, args) {        
            sender.set_contextKey(document.getElementById('<%=txtDivCode.ClientID%>').value);
        }

        function IncomeAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteIncomeAcct", "txtIncomeAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function VatPayableAcctAutoCompleteSelected(source, eventArgs) {            
            var hiddenfieldID = source.get_id().replace("AutoCompleteVatPayableAcct", "txtVatPayableAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function CostSalesAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteCostSalesAcct", "txtCostSalesAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function ProvVatCrAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteProvVatCrAcct", "txtProvVatCrAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function VatCrAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteVatCrAcct", "txtVatCrAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function CostSalesDiffAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteCostSalesDiffAcct", "txtCostSalesDiffAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }

        function ProvCrCtrlAcctAutoCompleteSelected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("AutoCompleteProvCrCtrlAcct", "txtProvCrCtrlAcctCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }
        
</script>

<script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }

    function EndRequestUserControl(sender, args) {
        // after update occur on UpdatePanel re-init the Autocomplete
        visualsearchbox();
        AutoCompleteExtenderUserControlKeyUp();
    }

    function ClearCode(source, sourceCode) {
        //alert(document.getElementById(sourceCode).value);
        if (source.value == "") {
           document.getElementById(sourceCode).value="";            
        }
   }   
</script>

<style type="text/css">
 .ChildGrid
        {
            border: 1px none #999999;
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            background: White;
            color: #DDD9CF;
            font-weight: normal;
            margin-top: 0px;
        }
        .ChildGridHeader
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            background-color: #6C6C6C !important;
            color: White;
            font-size: 12px;
            line-height: 150%;
        }
        
        .ChildgrdAternaterow
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            font-weight: normal;
            background: #FFDDCC;
            color: black;
            line-height: 150%;
        }
        .ChildgrdRowstyle
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            background: white;
            font-weight: normal;
            color: black;
            font-size: 12px;
            line-height: 150%;
        }
</style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tr>
                    <td class="td_cell" align="center" style="width: 100%;">
                        <asp:Label ID="lblHeading" runat="server" Text="New Default Posting Accounts" Style="padding: 2px"
                            CssClass="field_heading" Width="100%">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="width: 100%; padding: 2px 4px 0px 4px;">
                            <tr valign="top">
                                <td style="width: 15%;">
                                    <label class="field_caption">
                                        Posting Id</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 35%;">
                                    <asp:TextBox ID="txtPostingId" CssClass="field_input" runat="server" Enabled="false" ></asp:TextBox>
                                    <asp:TextBox ID="txtDivCode" runat="server" style="display:none" ></asp:TextBox>   
                                    <asp:TextBox ID="txtIncome" runat="server" style="display:none" ></asp:TextBox>   
                                    <asp:TextBox ID="txtVatPayable" runat="server" style="display:none" ></asp:TextBox>   
                                    <asp:TextBox ID="txtCostSale" runat="server" style="display:none" ></asp:TextBox>   
                                    <asp:TextBox ID="txtProvVatCredit" runat="server" style="display:none" ></asp:TextBox>   
                                    <asp:TextBox ID="txtVatCredit" runat="server" style="display:none" ></asp:TextBox>                                    
                                    <asp:TextBox ID="txtSalesDiff" runat="server" style="display:none" ></asp:TextBox>                                                                        
                                </td>
                                <td style="width: 15%">
                                    <label class="field_caption">
                                        Applicable To</label><span style="color: red" class="td_cell">&nbsp;*</span>
                                </td>
                                <td style="width: 35%">
                                    <asp:TextBox ID="txtApplicableTo" CssClass="field_input" runat="server" TabIndex="1" TextMode="MultiLine" Width="98%" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="4" style="height:3px"></td>
                            </tr>
                             <tr>
                                        <td style="width: 100px" valign="top" colspan="4">
                                            <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <div class="container" id="VS">
                                                            <div id="search_box_container">
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                            <ItemTemplate>
                                                                <table class="styleDatalist">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="lnkCode" class="button button4" runat="server" Text='<%# Eval("Code") %>'
                                                                                Style="display: none" />
                                                                            <asp:Button ID="lnkValue" class="button button4" runat="server" Text='<%# Eval("Value") %>'
                                                                                Style="display: none" />
                                                                            <asp:Button ID="lnkCodeAndValue" class="button button4" runat="server" Text='<%# Eval("CodeAndValue") %>'
                                                                                OnClick="lnkCodeAndValue_Click" />
                                                                            <asp:Button ID="lbClose" class="buttonClose button4" runat="server" OnClick="lbClose_Click"
                                                                                Text="X" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                        <div style="display: none">
                                                            <div id="search_query" runat="server" class="search_query">
                                                                &nbsp;</div>
                                                            <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                            <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                                            <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                        </div>
                                                        <div id="countrygroup1" style="float: left; margin-left: 40px; width: 90%">
                                                            <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                                            <asp:HiddenField ID="HFshowctry_agent" runat="server" />
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="4">
                                        <div id="gvDivService" runat="server" style="max-width:100%;">
                                        <asp:Label ID="lblTitle" runat="server" Text='List of Services' style="font-weight:bold;" CssClass="field_input"></asp:Label>
                                        <asp:GridView ID="gvServices" AllowPaging="false" AllowSorting="false" runat="server"
                                            AutoGenerateColumns="false" BackColor="White" BorderColor="#CCCCCC"
                                            BorderStyle="None" CssClass="grdstyle" Font-Bold="true" BorderWidth="1px" Font-Size="10px" CellPadding="3"
                                            ShowHeaderWhenEmpty="true" TabIndex="5" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Service ID" HeaderStyle-Width="30">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%# Bind("serviceId") %>' style="font-weight:bold" CssClass="field_input"></asp:Label>
                                                </ItemTemplate>                                                
                                                </asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Service Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%# Bind("serviceName") %>' Width="95" style="font-weight:bold" CssClass="field_input"></asp:Label>
                                                </ItemTemplate>                                                
                                                </asp:TemplateField>                                                 
                                                <asp:TemplateField HeaderText="Income Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIncomeAcct" runat="server" Text='<%# Bind("IncomeAcct") %>' CssClass="field_input" ></asp:TextBox>
                                                    <asp:TextBox ID="txtIncomeAcctCode" runat="server" Text='<%# Bind("IncomeAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteIncomeAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtIncomeAcct" UseContextKey="true" OnClientPopulating="AutoCompleteIncomeAcct_OnClientPopulating" OnClientItemSelected="IncomeAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Payable Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVatPayableAcct" runat="server" Text='<%# Bind("VatPayableAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtVatPayableAcctCode" runat="server" Text='<%# Bind("VatPayableAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteVatPayableAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtVatPayableAcct" UseContextKey="true" OnClientPopulating="AutoCompleteVatPayableAcct_OnClientPopulating" OnClientItemSelected="VatPayableAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost of Sales Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCostSalesAcct" runat="server" Text='<%# Bind("CostSalesAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtCostSalesAcctCode" runat="server" Text='<%# Bind("CostSalesAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteCostSalesAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtCostSalesAcct" UseContextKey="true" OnClientPopulating="AutoCompleteCostSalesAcct_OnClientPopulating" OnClientItemSelected="CostSalesAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Provisional VAT Input Credit Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProvVatCrAcct" runat="server" Text='<%# Bind("ProvVatCrAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtProvVatCrAcctCode" runat="server" Text='<%# Bind("ProvVatCrAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteProvVatCrAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtProvVatCrAcct" UseContextKey="true" OnClientPopulating="AutoCompleteProvVatCrAcct_OnClientPopulating" OnClientItemSelected="ProvVatCrAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Input Credit Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVatCrAcct" runat="server" Text='<%# Bind("VatCrAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtVatCrAcctCode" runat="server" Text='<%# Bind("VatCrAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteVatCrAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtVatCrAcct" UseContextKey="true" OnClientPopulating="AutoCompleteVatCrAcct_OnClientPopulating" OnClientItemSelected="VatCrAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost of Sales difference Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCostSalesDiffAcct" runat="server" Text='<%# Bind("CostSalesDiffAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtCostSalesDiffAcctCode" runat="server" Text='<%# Bind("CostSalesDiffAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteCostSalesDiffAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetAccounts" TargetControlID="txtCostSalesDiffAcct" UseContextKey="true" OnClientPopulating="AutoCompleteSalesDiffAcct_OnClientPopulating" OnClientItemSelected="CostSalesDiffAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Provisional Creditors Control Account" HeaderStyle-Width="10%">                                                                
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProvCrCtrlAcct" runat="server" Text='<%# Bind("ProvCrCtrlAcct") %>' CssClass="field_input"></asp:TextBox>
                                                    <asp:TextBox ID="txtProvCrCtrlAcctCode" runat="server" Text='<%# Bind("ProvCrCtrlAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteProvCrCtrlAcct" runat="server" CompletionInterval="10"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                ServiceMethod="GetControlAccounts" TargetControlID="txtProvCrCtrlAcct" UseContextKey="true" OnClientPopulating="AutoCompleteProvCrCtrlAcct_OnClientPopulating" OnClientItemSelected="ProvCrCtrlAcctAutoCompleteSelected">
                                                </asp:AutoCompleteExtender>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Classification" HeaderStyle-Width="50">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkClassify" Checked= '<%# Bind("Classification") %>' ToolTip='<%# Bind("serviceName") %>' AutoPostBack="true" runat="server" OnCheckedChanged="chkClassify_CheckedChanged" />                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="30">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnFill" runat="server" CssClass="btn" style="font-weight:normal" Text="Fill Accts" OnClick="btnFill_Click"/>
                                                     <tr id="trClass" runat="server" >
                                                            <td colspan="11">
                                                        <asp:Panel ID="panClass" runat="server" style="width: 100%;">
                                                            <div style="max-width:100%;">
                                                                <asp:GridView ID="gvClass" runat="server" ShowHeader="false" AutoGenerateColumns="false" CssClass="ChildGrid"
                                                                    Width="100%" OnRowDataBound="gvClass_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="150">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblClassCode" runat="server" Text='<%# Bind("ClassCode") %>' style="display:none"></asp:Label>
                                                                                <asp:Label ID="lblClassName" runat="server" Text='<%# Bind("ClassName") %>' CssClass="field_input"></asp:Label>                                                                                
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="150" HorizontalAlign="Left" BackColor="#abf8de" />
                                                                        </asp:TemplateField>                                                                        
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtIncomeAcct" runat="server" Text='<%# Bind("IncomeAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtIncomeAcctCode" runat="server" Text='<%# Bind("IncomeAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteIncomeAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtIncomeAcct" UseContextKey="true" OnClientPopulating="AutoCompleteIncomeAcct_OnClientPopulating" OnClientItemSelected="IncomeAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtVatPayableAcct" runat="server" Text='<%# Bind("VatPayableAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtVatPayableAcctCode" runat="server" Text='<%# Bind("VatPayableAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteVatPayableAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtVatPayableAcct" UseContextKey="true" OnClientPopulating="AutoCompleteVatPayableAcct_OnClientPopulating" OnClientItemSelected="VatPayableAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCostSalesAcct" runat="server" Text='<%# Bind("CostSalesAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtCostSalesAcctCode" runat="server" Text='<%# Bind("CostSalesAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteCostSalesAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtCostSalesAcct" UseContextKey="true" OnClientPopulating="AutoCompleteCostSalesAcct_OnClientPopulating" OnClientItemSelected="CostSalesAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtProvVatCrAcct" runat="server" Text='<%# Bind("ProvVatCrAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtProvVatCrAcctCode" runat="server" Text='<%# Bind("ProvVatCrAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteProvVatCrAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtProvVatCrAcct" UseContextKey="true" OnClientPopulating="AutoCompleteProvVatCrAcct_OnClientPopulating" OnClientItemSelected="ProvVatCrAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtVatCrAcct" runat="server" Text='<%# Bind("VatCrAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtVatCrAcctCode" runat="server" Text='<%# Bind("VatCrAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteVatCrAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtVatCrAcct" UseContextKey="true" OnClientPopulating="AutoCompleteVatCrAcct_OnClientPopulating" OnClientItemSelected="VatCrAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCostSalesDiffAcct" runat="server" Text='<%# Bind("CostSalesDiffAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtCostSalesDiffAcctCode" runat="server" Text='<%# Bind("CostSalesDiffAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteCostSalesDiffAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetAccounts" TargetControlID="txtCostSalesDiffAcct" UseContextKey="true" OnClientPopulating="AutoCompleteSalesDiffAcct_OnClientPopulating" OnClientItemSelected="CostSalesDiffAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%">                                                                
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtProvCrCtrlAcct" runat="server" Text='<%# Bind("ProvCrCtrlAcct") %>' ToolTip='<%# Bind("ClassName") %>' CssClass="field_input"></asp:TextBox>
                                                                            <asp:TextBox ID="txtProvCrCtrlAcctCode" runat="server" Text='<%# Bind("ProvCrCtrlAcctCode") %>'  Style="display: none"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteProvCrCtrlAcct" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                        ServiceMethod="GetControlAccounts" TargetControlID="txtProvCrCtrlAcct" UseContextKey="true" OnClientPopulating="AutoCompleteProvCrCtrlAcct_OnClientPopulating" OnClientItemSelected="ProvCrCtrlAcctAutoCompleteSelected">
                                                                        </asp:AutoCompleteExtender>
                                                                        </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="170">
                                                                            <ItemTemplate>
                                                                                
                                                                            </ItemTemplate>
                                                                        <ItemStyle Width="170" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="ChildgrdRowstyle" HorizontalAlign="Center"></RowStyle>
                                                                    <AlternatingRowStyle CssClass="ChildgrdAternaterow" HorizontalAlign="Center"></AlternatingRowStyle>
                                                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                </asp:GridView>
                                                            </div>
                                                        </asp:Panel>
                                                        </td>
                                                        </tr>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>                                                            
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" BorderColor="DarkGray" HorizontalAlign="Center" VerticalAlign="Middle"/>
                                            <AlternatingRowStyle CssClass="grdAternaterow" />  
                                            <FooterStyle CssClass="grdfooter" />                                                                                                                  
                                        </asp:GridView>
                                        </div>
                                    </td>
                                    </tr>
                                    <tr>
                                <td colspan="4" align="center" style="padding-top:10px">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="40" Text="Save" />&nbsp;&nbsp;                                    
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="43" Text="Return To Search" />                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>

