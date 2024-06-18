<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="SalesInvoiceFreeFormsearch.aspx.vb" Inherits="AccountsModule_SalesInvoiceFreeFormsearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ OutputCache Location="none" %>
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
            visualsearchbox_pending(); ;
            //txtNameAutoCompleteExtenderKeyUp();

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

        function visualsearchbox_pending() {

            var $txtvsprocess_pending = $(document).find('.cs_txtvsprocess_pending');
            //$txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"TRACKING STATUS":" "' + '"FROM EMAIL":" " TEXT:" "');
            var vCode = document.getElementById("<%=txttrantype.ClientID%>");

            if (vCode.value == 'CN') {
                $txtvsprocess_pending.val('CNNo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }

            if (vCode.value == 'DN') {
                $txtvsprocess_pending.val('DNNo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }

            if (vCode.value == 'PI') {
                $txtvsprocess_pending.val('PINo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }

            if (vCode.value == 'PE') {
                $txtvsprocess_pending.val('PENo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }
            if (vCode.value == 'IN') {
                $txtvsprocess_pending.val('InvoiceNo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }
            if (vCode.value == 'MN') {
                $txtvsprocess_pending.val('InvoiceNo:" "InvoiceType:" "Type:" " Customer:" "Supplier:" " SalesMan:" " ReferenceNo:" " SourceCountry:" "BookingNo:" "Narration:" "Text:"  "');

            }
            window.visualSearch = VS.init({
                container: $('#search_box_container_pending'),
                query: $txtvsprocess_pending.val(),
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

                        var $txtvsprocess_pending = $(document).find('.cs_txtvsprocess_pending');
                        $txtvsprocess_pending.val(visualSearch.searchQuery.serialize());

                        var $txtvsprocesssplit_pending = $(document).find('.cs_txtvsprocesssplit_pending');
                        $txtvsprocesssplit_pending.val(visualSearch.searchQuery.serializetosplit());

                        var txtvsprocess_pending = document.getElementById("<%=btnvsprocess_pending.ClientID%>");
                        txtvsprocess_pending.click(visualSearch.searchQuery.serializetosplit());

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
                        var divcode = document.getElementById("<%=txtdivcode.ClientID%>");


                        switch (category) {

                            case 'InvoiceNo' || 'CNNo' || 'DNNo' || 'PINo' || 'PENo':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) invoiceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by tran_id";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'CNNo':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) invoiceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by tran_id";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'DNNo':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) invoiceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by tran_id";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'PINo':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) invoiceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by tran_id";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'PENo':
                                var asSqlqry = "select  ltrim(rtrim(tran_id)) invoiceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by tran_id";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'InvoiceType':
                                var asSqlqry = "select  distinct ltrim(rtrim(invoicetype)) invoicetype from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  and invoicetype is not null order by invoicetype";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Type':
                                var asSqlqry = "select  distinct case ltrim(rtrim(acc_type)) when 'C' Then 'Customer' else 'Supplier' end  acc_type from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by acc_type";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Customer':
                                var asSqlqry = "select  ltrim(rtrim(agentname)) agentname from agentmast   order by agentname";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                            case 'Supplier':
                                var asSqlqry = "select  ltrim(rtrim(partyname)) tranid from partymast   order by partyname";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;


                            case 'SalesMan':

                                var asSqlqry = "select  ltrim(rtrim(username)) tranid from usermaster   order by username";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'ReferenceNo':

                                var asSqlqry = "select  ltrim(rtrim(referenceno)) referenceno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by referenceno";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'SourceCountry':

                                var asSqlqry = "select  ltrim(rtrim(ctryname)) ctryname from ctrymast   order by ctryname";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'BookingNo':

                                var asSqlqry = "select  ltrim(rtrim(bookingno)) bookingno from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by bookingno";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                            case 'Narration':

                                var asSqlqry = "select  narration narration from freeforminvoice_master where div_code='" + divcode.value + "' and tran_type='" + vCode.value + "'  order by narration";
                                glcallback = callback;
                                //ColServices.clsSectorServices.GetListOfTranDocVisualSearchStrNew(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;


                        }
                    },
                    facetMatches: function (callback) {

                        if (vCode.value == 'PE') {
                            callback([
                { label: 'Text' },
                { label: 'PENo', category: 'Hotels' },
                { label: 'InvoiceType', category: 'Hotels' },
                { label: 'Type', category: 'Hotels' },
                { label: 'Customer', category: 'Hotels' },
                   { label: 'Supplier', category: 'Hotels' },
                { label: 'SalesMan', category: 'Hotels' },
                 { label: 'ReferenceNo', category: 'Hotels' },
                 { label: 'SourceCountry', category: 'Hotels' },
                  { label: 'BookingNo', category: 'Hotels' },
                   { label: 'Narration', category: 'Hotels' },
              ]);
                        }
                        if (vCode.value == 'PI') {
                            callback([
                { label: 'Text' },
                { label: 'PINo', category: 'Hotels' },
                { label: 'InvoiceType', category: 'Hotels' },
                { label: 'Type', category: 'Hotels' },
                { label: 'Customer', category: 'Hotels' },
                   { label: 'Supplier', category: 'Hotels' },
                { label: 'SalesMan', category: 'Hotels' },
                 { label: 'ReferenceNo', category: 'Hotels' },
                 { label: 'SourceCountry', category: 'Hotels' },
                  { label: 'BookingNo', category: 'Hotels' },
                   { label: 'Narration', category: 'Hotels' },
              ]);
                        }

                        if (vCode.value == 'IN' || vCode.value == 'MN') {
                            callback([
                { label: 'Text' },
                { label: 'InvoiceNo', category: 'Hotels' },
                { label: 'InvoiceType', category: 'Hotels' },
                { label: 'Type', category: 'Hotels' },
                { label: 'Customer', category: 'Hotels' },
                   { label: 'Supplier', category: 'Hotels' },
                { label: 'SalesMan', category: 'Hotels' },
                 { label: 'ReferenceNo', category: 'Hotels' },
                 { label: 'SourceCountry', category: 'Hotels' },
                  { label: 'BookingNo', category: 'Hotels' },
                   { label: 'Narration', category: 'Hotels' },
              ]);
                        }

                        if (vCode.value == 'DN') {
                            callback([
                { label: 'Text' },
                { label: 'DNNo', category: 'Hotels' },
                { label: 'InvoiceType', category: 'Hotels' },
                { label: 'Type', category: 'Hotels' },
                { label: 'Customer', category: 'Hotels' },
                   { label: 'Supplier', category: 'Hotels' },
                { label: 'SalesMan', category: 'Hotels' },
                 { label: 'ReferenceNo', category: 'Hotels' },
                 { label: 'SourceCountry', category: 'Hotels' },
                  { label: 'BookingNo', category: 'Hotels' },
                   { label: 'Narration', category: 'Hotels' },
              ]);
                        }
                        if (vCode.value == 'CN') {
                            callback([
                { label: 'Text' },
                { label: 'CNNo', category: 'Hotels' },
                { label: 'InvoiceType', category: 'Hotels' },
                { label: 'Type', category: 'Hotels' },
                { label: 'Customer', category: 'Hotels' },
                   { label: 'Supplier', category: 'Hotels' },
                { label: 'SalesMan', category: 'Hotels' },
                 { label: 'ReferenceNo', category: 'Hotels' },
                 { label: 'SourceCountry', category: 'Hotels' },
                  { label: 'BookingNo', category: 'Hotels' },
                   { label: 'Narration', category: 'Hotels' },
              ]);
                        }


                    }
                }
            });



            //                   
        }



        function FormValidation(state) {



            if ((document.getElementById("<%=txtFromDate.ClientID%>").value == '') || (document.getElementById("<%=txtToDate.ClientID%>").value == '')) {

                alert("Select Dates ");
                return false;
            }





        }
        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
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
            //txtNameAutoCompleteExtenderKeyUp(); //a
            visualsearchbox_pending(); ;

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
        <ContentTemplate>
            <div runat="server" style="margin: 15px 5px 0px 5px; padding-bottom: 15px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" class="field_heading" style="height: 18px">
                                    <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Purchase Invoice Free Form "
                                        Width="698px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnhelp" TabIndex="3" runat="server" Text="Help" Font-Bold="False"
                                                    CssClass="btn"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAddNew" TabIndex="4" runat="server" Text="Add New" Font-Bold="False"
                                                    CssClass="btn"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPrint" TabIndex="5" runat="server" Text="Report" CssClass="btn">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlrpt" runat="server">
                                                    <asp:ListItem Value="Brief">Brief Report</asp:ListItem>
                                                    <asp:ListItem Value="Detailed">Detailed Report</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="width: 80%">
                                        <div id="Div_pendingsearchbox" runat="server">
                                            <div id="Div3" class="container">
                                                <div id="search_box_container_pending">
                                                </div>
                                                <asp:TextBox ID="txtvsprocess_pending" runat="server" class="cs_txtvsprocess_pending"
                                                    Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtvsprocesssplit_pending" runat="server" class="cs_txtvsprocesssplit_pending"
                                                    Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnvsprocess_pending" Style="display: none" runat="server" />
                                            </div>
                                        </div>
                                        <asp:DataList ID="dlInboxSearch_pending" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                            Width="949px">
                                            <ItemTemplate>
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                            <asp:Button ID="lbInboxCategorypending" class="button button4" runat="server" Style="font-family: Verdana;
                                                                font-size: 12px;" Text='<%# Eval("Value") %>' />
                                                            <asp:Button ID="lbCloseCategorypending" class="buttonClose button4" runat="server"
                                                                Style="font-family: Verdana; font-size: 12px;" Text="X" OnClick="lbCloseCategorypending_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" CssClass="field_caption" Text="Filter By "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOrder" runat="server">
                                                    <asp:ListItem Value="C">Created Date</asp:ListItem>
                                                    <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                                    <asp:ListItem Value="T">Transaction Date</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" TabIndex="2" runat="server" CssClass="fiel_input" Width="80px"
                                                    ValidationGroup="MKE"></asp:TextBox>
                                                <asp:ImageButton ID="ImgBtnFrmDt" TabIndex="2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                </asp:ImageButton>
                                                <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" CssClass="field_error" Width="23px"
                                                    ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                    InvalidValueBlurredMessage="Input a date in dd/mm/yyyy format" ErrorMessage="MskVFromDate"
                                                    EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic"
                                                    ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator>
                                                <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                                                    MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                    DisplayMoney="Left" AcceptNegative="Left">
                                                </cc1:MaskedEditExtender>
                                                <cc1:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtFromDate"
                                                    PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" CssClass="field_input" Width="80px"
                                                    ValidationGroup="MKE"></asp:TextBox>
                                                <asp:ImageButton ID="ImageButton1" TabIndex="4" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                </asp:ImageButton>
                                                <cc1:MaskedEditValidator ID="MskVToDt" runat="server" CssClass="field_error" Width="23px"
                                                    ValidationGroup="MKE" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                    InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate1" EmptyValueMessage="Date is required"
                                                    EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="MskChequeDate"></cc1:MaskedEditValidator>
                                                <cc1:CalendarExtender ID="ClExChequeDate" runat="server" TargetControlID="txtToDate"
                                                    PopupButtonID="ImageButton1" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender ID="MskChequeDate" runat="server" TargetControlID="txtToDate"
                                                    MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                                                    DisplayMoney="Left" AcceptNegative="Left">
                                                </cc1:MaskedEditExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                                    Text="Search by Date" />
                                                &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False"
                                                    TabIndex="4" Text="Reset Dates" />
                                            </td>
                                            <td>
                                                <asp:Label ID="RowSelectcs" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                                <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true">
                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                    <asp:ListItem Value="15">15</asp:ListItem>
                                                    <asp:ListItem Value="20">20</asp:ListItem>
                                                    <asp:ListItem Value="25">25</asp:ListItem>
                                                    <asp:ListItem Value="30">30</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <div style="width: 18%; display: inline-block; vertical-align: top; text-align: right;">
                                                    <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                                                        TabIndex="4" Text="Reset Search" />
                                                </div>
                                            </td>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                                        TabIndex="8" Style="display: none" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 33px; width: 100%">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gv_SearchResult" TabIndex="18" runat="server" Font-Size="10px"
                                                BackColor="White" Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3"
                                                BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                                                AllowSorting="True" AllowPaging="True">
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
                                                <Columns>
                                                    <asp:TemplateField Visible="False" HeaderText="Doc No">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("invoiceno") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("invoiceno") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="invoiceno" SortExpression="invoiceno" HeaderText="Invoice No">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="doctype" SortExpression="invoicetype" HeaderText="Invoice Type">
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" DataField="invoice_date"
                                                        SortExpression="invoice_date" HeaderText="Date"></asp:BoundField>
                                                    <asp:BoundField DataField="post_state" HeaderText="Status"></asp:BoundField>
                                                    <asp:BoundField DataField="referenceno" SortExpression="referenceno" HeaderText="RefNo">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="acctype" SortExpression="acctype" HeaderText="Type"></asp:BoundField>
                                                    <asp:BoundField DataField="supname" SortExpression="supname" HeaderText="Name"></asp:BoundField>
                                                    <asp:BoundField DataField="Basetotal" SortExpression="Basetotal" HeaderText="Amount">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="username" SortExpression="username" HeaderText="SalesMan">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ctryname" SortExpression="ctryname" HeaderText="SourceCtry">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="bookingno" SortExpression="bookingno" HeaderText="BookingNo">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="narration" SortExpression="narration" HeaderText="Narration">
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                                        DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                                    <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                                    </asp:BoundField>
                                                    <asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}"
                                                        DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                                                    <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                                    </asp:BoundField>
                                                    <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="Cancel" CommandName="Cancelrow">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="UndoCancel" CommandName="undoCancel">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField HeaderText="Action" Text="View Log" CommandName="ViewLog">
                                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                                    </asp:ButtonField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" Font-Size="10px" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
                                            </asp:GridView>
                                            <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                                                Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg"
                                                Visible="False"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                        height: 9px" type="text" />
                                </td>
                            </tr>
                            <caption>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="txtdivcode" runat="server" />
                                        <asp:HiddenField ID="txttrantype" runat="server" />
                                    </td>
                                </tr>
                            </caption>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="div1">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/clsServices.asmx" />
            </Services>
        </asp:ScriptManagerProxy>
    </div>
</asp:Content>
