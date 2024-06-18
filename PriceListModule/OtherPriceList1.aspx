<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="OtherPriceList1.aspx.vb" Inherits="PriceListModule_OtherPriceList1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>

        <%@ Register Src="Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
        <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <%@ OutputCache location="none" %> 



<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

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
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {


            //AutoCompleteExtenderKeyUp();
            visualsearchbox();
            AutoCompleteExtenderUserControlKeyUp();


        });


        function fnClearTextBoxNeeded(keybr) {
            if (keybr == 9 || keybr == 16 || keybr == 17 || keybr == 18 || keybr == 20 || keybr == 27 || keybr == 45 || keybr == 36 || keybr == 33 || keybr == 35 || keybr == 34 || keybr == 37 || keybr == 38 || keybr == 39 || keybr == 40 || keybr == 93 || keybr == 92 || keybr == 112 || keybr == 123 || keybr == 144 || keybr == 145 || keybr == 19 || keybr == 13) {
                return false;
            }
            else {
                return true;
            }
        }

        var glcallback;



        function visualsearchbox() {

            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('CountryGroup:"          " Region:"          " Country:"         " Text:"          "');

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
      </script>

    <script language ="javascript" type ="text/javascript" >

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function InitializeRequestUserControl(sender, args) {

        }

        function EndRequestUserControl(sender, args) {
            //AutoCompleteExtenderKeyUp();
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
            AutoCompleteExtenderUserControlKeyUp();
        }


        function CurrNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtCurrCodeNew.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtCurrCodeNew.ClientID%>').value = '';
            }
        }

        function othgroupautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtothgroupcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtothgroupcode.ClientID%>').value = '';
            }
        }

        // ** >>>>>>>>>>Added by Abin on 20180603
        function CalculateTax() {

            var VatPerc = document.getElementById("<%=txtVAT.ClientID%>").value;
            var chkPriceWithTax = document.getElementById("<%=chkPriceWithTax.ClientID%>");

            if ((VatPerc).trim() == "") {
                //                alert("Vat Percentage can not be empty");
                //                VatPerc.focus();
                //                return false;
            }
            else {
                var txt = document.getElementById(arguments[0]);
                var txtTax = document.getElementById(arguments[1]);
                // var txtNonTax = document.getElementById(arguments[2]);
                var txtVat = document.getElementById(arguments[2]);
                var txtNTax = document.getElementById(arguments[3]);
                txtNTax.value = '0';
                if ((txt.value).trim() == '') {

                    txtTax.value = '';
                    txtVat.value = '';
                }
                else if (chkPriceWithTax.checked == false) {
                    txtTax.value = parseFloat(txt.value).toFixed(2);
                    var vat = parseFloat(txt.value).toPrecision() * (parseFloat(VatPerc).toPrecision() / 100);

                    txtVat.value = vat.toFixed(2);
                }
                else {

                    var Taxable = parseFloat(txt.value).toPrecision() / (1 + (parseFloat(VatPerc).toPrecision() / 100));
                    var vat = parseFloat(txt.value).toPrecision() - Taxable;

                    txtTax.value = Taxable.toFixed(2);
                    txtVat.value = vat.toFixed(2);
                }
            }

        }
        function fnReadOnly(txt) {
            event.preventDefault();
        }

//************<<<<<
    function chkTextLock(evt) {
        return false;
    }
    function chkTextLock1(evt) {
        if (evt.keyCode = 9) {
            return true;
        }
        else {
            return false;
        }
        return false;
    }
    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
            return false;
        }
    }
    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }
    }
    function validateDecimalOnly(evt, txt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
        }
        else {
            key = String.fromCharCode(key);

            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                var inputValue = txt.value
                if (inputValue.indexOf('.') < 1) {
                    txt.value = txt.value + '.';
                    return true;
                }
                else {
                    return false;
                }
            }

        }

    }
    function checkCharacter(e) {
        if (event.keyCode == 32 || event.keyCode == 46)
            return;
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }
    }

    function CallWebMethod(methodType) {
        switch (methodType) {

          

            case "GroupCode":
                var select = document.getElementById("<%=ddlGroupCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlGroupName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
             
                break;
            case "GroupName":
                var select = document.getElementById("<%=ddlGroupName.ClientID%>");
                var selectname = document.getElementById("<%=ddlGroupCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "othsellcode":
                var select = document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var selectname = document.getElementById("<%=ddlOtherSellingTypeName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var constr = null;

                var filter = document.getElementById("<%=ddlGroupCode.ClientID%>");

                if (select.value != "[Select]") {



                    if (filter.value != "VISA") {
                        ColServices.clsServices.GetCurrNamefromOthsellingcodetype(constr, codeid, FillCurrCode, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetCurrNamefromOthsellingNametype(constr, codeid, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                    }
                    else {
                        ColServices.clsServices.GetCurrNamefromVisasellingcodetype(constr, codeid, FillCurrCode, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetCurrNamefromVisasellingNametype(constr, codeid, FillSupplierCurrName, ErrorHandler, TimeOutHandler);


                    }

                }

                else {
                    var currcode = document.getElementById("<%=txtCurrCode.ClientID%>");
                    var currname = document.getElementById("<%=txtCurrName.ClientID%>");

                    currcode.value = "";
                    currname.value = "";



                }


                break;
            case "othsellname":
                var select = document.getElementById("<%=ddlOtherSellingTypeName.ClientID%>");
                var codeid = select.options[select.selectedIndex].text;
                var codevalue = select.options[select.selectedIndex].value;
                var selectname = document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                var constr = null;
                var filter = document.getElementById("<%=ddlGroupCode.ClientID%>");
                if (select.value != "[Select]") {
                    if (filter.value != "VISA") {
                        ColServices.clsServices.GetCurrNamefromOthsellingcodetype(constr, codevalue, FillCurrCode, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetCurrNamefromOthsellingNametype(constr, codevalue, FillSupplierCurrName, ErrorHandler, TimeOutHandler);
                    }
                    else {
                        ColServices.clsServices.GetCurrNamefromVisasellingcodetype(constr, codevalue, FillCurrCode, ErrorHandler, TimeOutHandler);
                        ColServices.clsServices.GetCurrNamefromVisasellingNametype(constr, codevalue, FillSupplierCurrName, ErrorHandler, TimeOutHandler);


                    }
                }
                else {
                    var currcode = document.getElementById("<%=txtCurrCode.ClientID%>");
                    var currname = document.getElementById("<%=txtCurrName.ClientID%>");

                    currcode.value = "";
                    currname.value = "";

                }


                break;


            case "SeasCode":
                var select = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "SeasName":
                var select = document.getElementById("<%=ddlSubSeasName.ClientID%>");
                var selectname = document.getElementById("<%=ddlSubSeasCode.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;

        }
    }

    function FillVisa(result) {
        var acctcurr = document.getElementById("<%=txtVisa.ClientID%>");
        acctcurr.value = result;

    }
   




    function FillCurrCode(result) {
        var txt = document.getElementById("<%=txtCurrCode.ClientID%>");
    
      
        txt.value = result
       
    }

    function FillSupplierCurrName(result) {
        var txt = document.getElementById("<%=txtCurrName.ClientID%>");
        txt.value = result;
    }

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

    function FormValidation(state) {

        if (document.getElementById("<%=ddlGroupCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlGroupName.ClientID%>").focus();
            alert("Select Main Group Code");
            return false;
        }



        if (document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlOtherSellingTypeCode.ClientID%>").focus();
            alert("Select Selling Type Code");
            return false;
        }




        if (document.getElementById("<%=ddlSubSeasCode.ClientID%>").value == "[Select]") {
            document.getElementById("<%=ddlSubSeasCode.ClientID%>").focus();
            alert("Select Sub Season Code.");
            return false;
        }


        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to generate price list?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to generate  price list?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to generate  price list?') == false) return false; }

        }
    }



</script>


 <asp:UpdatePanel id="UpdatePanel1" runat="server">
   <contenttemplate>
       <table style="border-right: gray 2pt solid; border-top: gray 2pt solid; border-left: gray 2pt solid;
           border-bottom: gray 2pt solid">
           <tbody>
               <tr>
                   <td style="height: 18px; text-align: center" class="field_heading" align="left" colspan="4">
                       <asp:Label ID="lblHeading" runat="server" Text="Add Other Service price List" CssClass="field_heading"
                           Width="744px"></asp:Label>
                   </td>
               </tr>
               <tr>
                   <td style="width: 75px" class="td_cell" align="left">
                       <span style="font-family: Arial">PL Code</span>
                   </td>
                   <td style="width: 75px">
                       <input style="width: 194px" id="txtPlcCode" class="field_input" disabled tabindex="1"
                           type="text" runat="server" />
                   </td>
                    <td style="width: 120px" class="td_cell" align="right">
                       <span style="font-family: Arial">Currency Name</span>
                   </td>
                    <td>
                       <asp:TextBox ID="txtCurrNamenew" runat="server" CssClass="field_input" TabIndex="3"
                           Width="258px"></asp:TextBox>
                       <asp:TextBox ID="txtCurrcodenew" runat="server" Style="display: none" Width="194px"></asp:TextBox>
                       <asp:AutoCompleteExtender ID="txtCurrNamenew_AutoCompleteExtender" runat="server"
                           CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                           CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                           CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                           FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="CurrNameautocompleteselected"
                           ServiceMethod="Getcurrlist" TargetControlID="txtCurrNamenew">
                       </asp:AutoCompleteExtender>
                   </td>
               </tr>
               <tr>
              
                  <td style="width: 190px" class="td_cell" align="left">
                       <span style="font-family: Arial">Applicable To</span>
                   </td>
                   <td>
                       <asp:TextBox ID="txtApplicableTo" runat="server" Rows="2" Style="margin: 0px; height: 48px;
                           width: 300px" TabIndex="1" TextMode="MultiLine"></asp:TextBox>
                            </td>
                      <td style="width: 120px" class="td_cell" align="right">
                       <span style="font-family: Arial">Other Group</span>
                   </td>

                    <td>
                       <asp:TextBox ID="txtothgroup" runat="server" CssClass="field_input" TabIndex="3"
                           Width="258px"></asp:TextBox>
                       <asp:TextBox ID="txtothgroupcode" runat="server" Style="display: none" Width="194px"></asp:TextBox>
                       <asp:AutoCompleteExtender ID="txtothgroup_AutoCompleteExtender" runat="server"
                           CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                           CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                           CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                           FirstRowSelected="True" MinimumPrefixLength="0" OnClientItemSelected="othgroupautocompleteselected"
                           ServiceMethod="Getgrouplist" TargetControlID="txtothgroup">
                       </asp:AutoCompleteExtender>
                   </td>
               
               </tr>
               <tr>
                   <td style="width: 100%" valign="top" colspan="5">
                       <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                           <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                               <ContentTemplate>
                                   <div class="container" id="VS">
                                       <div id="search_box_container">
                                       </div>
                                   </div>
                                   <br />
                                   <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                       Width="366px">
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
                                   <div id="countrygroup1" style="float: left; margin-left: 40px; width: 99%">
                                       <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                   </div>
                               </ContentTemplate>
                           </asp:UpdatePanel>
                       </div>
                   </td>
               </tr>
             
               <tr style="display: none">
                   <td style="width: 201px" class="td_cell" align="left">
                       <span style="font-family: Arial">Group Code</span> <span style="color: #ff0000">*</span>
                   </td>
                   <td style="width: 122px">
                       <select style="width: 200px" id="ddlGroupCode" class="field_input" tabindex="10"
                           onchange="CallWebMethod('GroupCode');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
                   <td style="width: 190px" class="td_cell" align="left">
                       <span style="font-family: Arial">Group Name</span>
                   </td>
                   <td>
                       <select style="width: 300px" id="ddlGroupName" class="field_input" tabindex="11"
                           onchange="CallWebMethod('GroupName');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
               </tr>
               <tr style="display: none">
                   <td style="width: 201px" class="td_cell" align="left">
                       <span style="font-family: Arial">
                           <asp:Label ID="lblSellingTypeCode" runat="server" Text="Other Selling Type Code"></asp:Label></span>
                       <span style="color: #ff0000">*</span>
                   </td>
                   <td style="width: 122px">
                       <select style="width: 200px" id="ddlOtherSellingTypeCode" class="field_input" tabindex="11"
                           onchange="CallWebMethod('othsellcode');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
                   <td style="width: 190px" class="td_cell" align="left">
                       <span style="font-family: Arial">
                           <asp:Label ID="lblsellingtypeName" runat="server" Text="Other Selling Type Name"></asp:Label></span>
                   </td>
                   <td>
                       <select style="width: 300px" id="ddlOtherSellingTypeName" class="field_input" tabindex="12"
                           onchange="CallWebMethod('othsellname');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
               </tr>
               <tr style="display: none">
                   <td style="width: 201px" align="left">
                       <span style="font-family: Arial"><span style="font-size: 8pt">Currency Code</span>
                       </span>
                   </td>
                   <td style="width: 122px">
                       <asp:TextBox ID="txtCurrCode" TabIndex="13" runat="server" CssClass="field_input"
                           Width="194px"></asp:TextBox>
                   </td>
                   <td style="width: 190px" class="td_cell" align="left">
                       <span style="font-family: Arial">Currency Name</span>
                   </td>
                   <td>
                       <asp:TextBox ID="txtCurrName" TabIndex="14" runat="server" CssClass="field_input"
                           Width="294px"></asp:TextBox>
                   </td>
               </tr>
               <tr style="display: none">
                   <td style="width: 201px" align="left">
                       <span style="font-size: 8pt"><span style="font-family: Arial">Sub Season Code</span>
                           <span style="color: #ff0000; font-family: Arial">*</span></span>
                   </td>
                   <td style="width: 122px">
                       <select style="width: 200px" id="ddlSubSeasCode" class="field_input" tabindex="15"
                           onchange="CallWebMethod('SeasCode');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
                   <td style="width: 190px" class="td_cell" align="left">
                       <span style="font-family: Arial">Sub Season Code</span>
                   </td>
                   <td>
                       <select style="width: 300px" id="ddlSubSeasName" class="field_input" tabindex="16"
                           onchange="CallWebMethod('SeasName');" runat="server">
                           <option selected></option>
                       </select>
                   </td>
               </tr>
               <tr>
                   <td style="width: 201px; height: 36px" align="left">
                       <span style="font-size: 10pt; font-family: Arial">Remarks</span>
                   </td>
                   <td style="width: 183px; height: 36px">
                       <textarea style="width: 363px; height: 29px" id="txtRemark" class="field_input" tabindex="5"
                           runat="server"></textarea>
                   </td>
                   <td>
                       <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                           <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                               BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                               Caption="Price list Dates" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                               Font-Size="12px" GridLines="Vertical" TabIndex="6">
                               <FooterStyle CssClass="grdfooter" />
                               <Columns>
                                   <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                                   <asp:TemplateField HeaderText="From Date">
                                       <ItemTemplate>
                                           <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                           <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                               TargetControlID="txtfromDate" PopupPosition="Right">
                                           </cc1:CalendarExtender>
                                           <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                               TargetControlID="txtfromDate">
                                           </cc1:MaskedEditExtender>
                                           <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                               TabIndex="-1" /><br />
                                           <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                               ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                               EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                               InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</cc1:MaskedEditValidator>
                                       </ItemTemplate>
                                       <HeaderStyle Wrap="False" />
                                       <ItemStyle Wrap="False" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="To Date">
                                       <ItemTemplate>
                                           <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                           <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                               TargetControlID="txtToDate" PopupPosition="Right">
                                           </cc1:CalendarExtender>
                                           <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                               TargetControlID="txtToDate">
                                           </cc1:MaskedEditExtender>
                                           <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                               TabIndex="-1" /><br />
                                           <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                               ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                               EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                               InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</cc1:MaskedEditValidator>
                                       </ItemTemplate>
                                       <HeaderStyle Wrap="False" />
                                       <ItemStyle Wrap="False" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Action">
                                       <ItemTemplate>
                                           <asp:ImageButton ID="imgStayAdd" runat="server" ImageUrl="~/Images/PlusGreen.ico"
                                               Width="18px" OnClick="imgStayAdd_Click" />
                                           <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                               Width="18px" OnClick="imgSclose_Click" ToolTip="Delete Current Row" />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                               </Columns>
                               <FooterStyle BackColor="White" ForeColor="#000066" />
                               <RowStyle CssClass="grdRowstyle" />
                               <SelectedRowStyle CssClass="grdselectrowstyle" />
                               <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                               <HeaderStyle CssClass="grdheader" />
                               <AlternatingRowStyle CssClass="grdAternaterow" />
                           </asp:GridView>
                       </div>
                   </td>
               </tr>
               <tr>
          
                   <td style="width: 122px">
                       <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                           height: 9px" type="text" />
                           <asp:HiddenField ID="hdndecimal" runat="server" />
                            <asp:HiddenField ID="hdngroup" runat="server" />
                   </td>
                   <td style="width: 122px">
                       <input id="txtVisa" runat="server" style="display: none; width: 12px; height: 9px"
                           type="text" />
                   </td>
               </tr>
               <tr>
                   <td style="display:none" class="td_cell" align="left">
                       <%--<SPAN style="FONT-FAMILY: Arial">Active</SPAN>--%>
                       <input id="ChkActive" tabindex="19" type="checkbox" checked runat="server" />Active
                   </td>
                  
                   <td style="height: 23px">
                       <asp:CheckBox ID="chkConsdierForMarkUp" runat="server" Font-Bold="False" Visible="False"
                           Text="Consider this supplier for markup " />
                   </td>
               </tr>
               <tr>
                   <td align="left" class="td_cell" colspan="2" >
                    <table>
                                                <tr>
                 <td style="background-color:#06788B;color:White;" align="left" colspan="2">
                     &nbsp;Tax Details</td>
           </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="Label5" runat="server" Text="VAT"></asp:Label>
                                                        <asp:Label ID="Label6" runat="server" ForeColor="#CC3300" Text="*"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVAT" runat="server" AutoComplete="off" AutoPostBack="true" 
                                                            onkeypress="return   validateDecimalOnly(event,this)"  Width="75px"></asp:TextBox>
                                                        % &nbsp;<asp:CheckBox ID="chkPriceWithTax" runat="server" AutoPostBack="True" 
                                                            Text="All price are including tax" />
                                                    </td>
                                                </tr>
                                               </table>
                       &nbsp;</td>
               </tr>
               <tr style="font-size: 8pt">
                
                   <td align="left" class="td_cell"  style="font-size: 10pt;" 
                       valign="top">
                       <asp:GridView ID="grdtransferrates" runat="server" AutoGenerateColumns="False" 
                           BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                           CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                           Width="1px">
                           <FooterStyle CssClass="grdfooter" />
                           <Columns>
                               <asp:TemplateField>
                                   <EditItemTemplate>
                                       <asp:TextBox ID="TextBox1" runat="server" CssClass="field_input"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <input id="ChkSelect" type="checkbox" name="ChkSelect" runat="server" />
                                   </ItemTemplate>
                               </asp:TemplateField>
                                  <asp:BoundField DataField="othtypcode" HeaderText="OthTypecode" Visible="false">
                                   </asp:BoundField>
                                <asp:BoundField DataField="othtypname" HeaderText="Other Types" Visible="false"></asp:BoundField>

                                 <asp:TemplateField HeaderText="Service Type" >
                                        <ItemTemplate>
                                           
                                            <asp:Label ID="lblothtypename" runat="server" Text='<%# Bind("othtypname") %>' Width="170px"></asp:Label>
                                         
                                        </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Classification" Visible="false">
                                        <ItemTemplate>
                                           
                                            <asp:Label ID="lblothtypecode" runat="server" Text='<%# Bind("othtypcode") %>'></asp:Label>
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Unit">
                                    <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtunit" runat="server" CssClass="field_input" Style="text-align: left"
                                            Width="120px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtunit_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="1"
                                            UseContextKey="true" ServiceMethod="getFillRateType" TargetControlID="txtunit">
                                        </asp:AutoCompleteExtender><%--'Added by abin on 20180604 *********************--%>
                                         <br />
                                                         <div style="padding-top:3px;width:130px;">
                                                         <asp:TextBox ID="txtUnitTV"  CssClass="field_input" placeholder="&nbsp;&nbsp;TV"  AutoComplete="off"  onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                          <asp:TextBox ID="txtUnitNTV"  CssClass="field_input" placeholder="&nbsp;&nbsp;NTV"  AutoComplete="off" onkeydown="fnReadOnly(event)"   runat="server" Width="36px"></asp:TextBox>
                                                         <asp:TextBox ID="txtUnitVAT"  CssClass="field_input" placeholder="&nbsp;VAT"  AutoComplete="off" onkeydown="fnReadOnly(event)"  runat="server" Width="36px"></asp:TextBox>
                                                         </div>   
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                           </Columns>
                           <RowStyle CssClass="grdRowstyle" />
                         <RowStyle CssClass="grdRowstyle" />
                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="grdheader" />
                        <AlternatingRowStyle CssClass="grdAternaterow" />
                       </asp:GridView>
                   </td>
                
               </tr>
               <tr>

                 <td>
                    <asp:Button ID="btnclearprice" TabIndex="22" runat="server" Text="Clear Prices"
                           Height="20px" CssClass="field_button"></asp:Button>
                   </td>
                <td style="display:none">
                     <asp:Button ID="btncopyrates" TabIndex="22" runat="server" Text="Copy Rates to NextLine"
                           Height="20px" CssClass="field_button"></asp:Button>
                   </td>
                  
                    
                    <td style="width: 183px" align="left" class="td_cell">
                       <asp:CheckBox ID="chkapprove" runat="server" Font-Bold="False" Text="Approve/Unapprove"
                            />
                   </td>
                                              

                   <td align ="right" > 
                   
                   <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                 tabIndex="8" Text="Save" /></td>

                   <td align ="right">
                    <table>
                    <tr>
                    
                       <td style="height: 22px" class="td_cell" align="right" colspan="4">
                       <asp:Button ID="btnGenerate" TabIndex="20"  runat="server"
                           Text="Generate" CssClass="field_button"></asp:Button>
                    
                   <asp:Button ID="btnhelp" TabIndex="22" OnClick="btnhelp_Click" runat="server" Text="Help"
                     Height="20px" CssClass="field_button" visible="true"></asp:Button>
                     
                   </td>
                   <td>
                       <asp:Button ID="btnCancel" TabIndex="21" OnClick="btnCancel_Click" runat="server"
                           Text="Return to Search" CssClass="field_button"></asp:Button>
                     
                     
                   </td>
                    </tr>
                    </table>
                   </td>
               </tr>

           </tbody>
       </table>
          
</contenttemplate>
</asp:UpdatePanel>
                  <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
                    <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
                </asp:ScriptManagerProxy>

</asp:Content>

