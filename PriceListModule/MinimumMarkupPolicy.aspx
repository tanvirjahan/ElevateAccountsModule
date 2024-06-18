<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" EnableEventValidation="false" MaintainScrollPositionOnPostBack = "false"
    AutoEventWireup="false" CodeFile="MinimumMarkupPolicy.aspx.vb" Inherits="MinimumMarkupPolicy" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1">
    <link href="../../css/Styles.css" rel="stylesheet" type="text/css" />
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
    <script type="text/jscript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js">
    </script>

    <style type="text/css">
       
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
            border-color: Blue;
        }
        
        *
        {
            outline: none;
        }
        
        .fmhead
        {
            display: block;
            text-align: center;
        }
        
        .ModalPopupBG
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
        
        .ModalPopupBGmeal
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
        .field_heading1
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 9pt;
            font-weight: bold;
            background-color: #06788B;
            color: #ffffff;
            padding: 3px;
        }
    </style>
       <script type="text/javascript">

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);

            function InitializeRequest(sender, args) {

            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                visualsearchbox();
                CurrencyNameAutoCompleteExtenderKeyUp();
            }
            function FormValidation(state) {
                var btn = document.getElementById("<%=btnSave.ClientID%>");
                if (btn.value == 'Save') { if (confirm('Are you sure you want to save?') == false) return false; }
                if (btn.value == 'Update') { if (confirm('Are you sure you want to update?') == false) return false; }
                if (btn.value == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
            }
    </script>

    <script language="javascript" type="text/javascript">
     
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 ||  event.keyCode > 122)) {
                return false;
            }
        }

        function checkNumber(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 45 && charCode < 58)) {
                //alert("Enter numerals only in this field. "+ charCode);
                return true;
            }
            return false;
        }

        function ValidationTerm() {

            
        }
        
     
  

      
        function validateDigitOnly(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
          
            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
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


        function CurrencyNameautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
            }

        }
        function CurrencyNameAutoCompleteExtenderKeyUp() {
            $("#<%=TxtCurrencyName.ClientID %>").bind("change", function () {
                document.getElementById('<%=TextCurrencyCode.ClientID%>').value = '';
            });
        }
    </script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            visualsearchbox();
            CurrencyNameAutoCompleteExtenderKeyUp();
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
            // $txtvsprocess.val('Country:" " City:" " Sector:" " Category:" " HotelGroup:" " HotelChain:" " HotelStatus:" "' + '"Room Classification":" " Text:" "');
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
//                        var $query = $('.search_query');
//                        $query.stop().animate({ opacity: 1 }, { duration: 300, queue: false });
//                        $query.html('<span class="raquo">&raquo;</span> You searched for: <b>' + searchCollection.serialize() + '</b>');

                        var $txtvsprocess = $(document).find('.cs_txtvsprocess');
                        $txtvsprocess.val(visualSearch.searchQuery.serialize());

                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit());

                        var btnvsprocess = document.getElementById("<%=btnvsprocess.ClientID%>");
                        btnvsprocess.click(visualSearch.searchQuery.serializetosplit());

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
                                var asSqlqry = '';
                              
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayCountryGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Region':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayRegionVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Country':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
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

  <script type="text/javascript">
      var SelectedRow = null;
      var SelectedRowIndex = null;
      var UpperBound = null;
      var LowerBound = null;

      window.onload = function () {
          LowerBound = 0;
          SelectedRowIndex = -1;
      }

      function SelectRow(CurrentRow, RowIndex) {
          var gridView = document.getElementById("<%=gvCommFormula.ClientID %>"); // *********** Change gridview name
          UpperBound = gridView.getElementsByTagName("tr").length - 2;
          if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
              return;

          if (SelectedRow != null) {
              SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
              SelectedRow.style.color = SelectedRow.originalForeColor;
          }

          if (CurrentRow != null) {
              CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
              CurrentRow.originalForeColor = CurrentRow.style.color;
              CurrentRow.style.backgroundColor = '#FFCC99';
              CurrentRow.style.color = 'Black';
              var txtFrm = CurrentRow.cells[1].getElementsByTagName("input")[0];
              txtFrm.focus();
              //alert(txtFrm.value);
          }

          SelectedRow = CurrentRow;
          SelectedRowIndex = RowIndex;
          setTimeout("SelectedRow.focus();", 0);
      }

      function SelectSibling(e) {
          var e = e ? e : window.event;
          var KeyCode = e.which ? e.which : e.keyCode;


          //  alert('test');
          if (KeyCode == 40 || KeyCode == 38) {
              if (KeyCode == 40)
                  SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
              else if (KeyCode == 38)
                  SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
          }
          else {
              if (KeyCode == 13) {
                  //alert(KeyCode);
              }
              return true;

          }
      }
      function LastSelectRow(CurrentRow, RowIndex) {
          var row = document.getElementById(CurrentRow);
          SelectRow(row, RowIndex);

      }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell" width="100%">
                <tbody>
                    <tr>
                        <td class="td_cell" align="center">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Minimum Markup and Discount" Width="100%"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 70%" align="center">
                            <table style="width: 75%">
                                <tbody>
                                    <tr>
                                        <td class="td_cell" style="width: 100%" valign="top">
                                          <table style="width:70%; padding:0px; position:relative;">
                    <tr>
                        <td class="td_cell">
                            &nbsp;</td>
                        <td style="color: #000000;">
                            &nbsp;</td>                        
                    </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                <span style="color: black">Formula Code</span> 
                            </td>
                            <td style="color: #000000;" align="left">
                                <input onblur="chgvalue()" id="txtcode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" readonly="readonly" style="width: 220px" />
                            </td>
                        </tr>
                    <tr>
                        <td style="height:24px;" class="td_cell" align="left">
                            Formula Name<span style="color: red" class="td_cell">*</span>
                        </td>
                        <td>
                            <input id="txtname" class="field_input" tabindex="2" type="text" maxlength="1000"
                                style="width:100%" runat="server" />
                        </td>                        
                    </tr>
                        <tr>
                            <td class="td_cell" style="height:24px;" align="left">
                                Adult with Visa<span class="td_cell" style="color: red">*</span>
                            </td>
                            <td align="left">
                                <input id="txtAdultWithVisa" class="field_input" tabindex="3" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server" style="width: 220px" />
                              
                            </td>
                        </tr>
                    <tr>
                        <td class="td_cell""  style="height:24px;" align="left">
                            Adult without Visa<span class="td_cell" style="color: red">*</span>
                        </td>
                        <td align="left">
                           <input id="txtAdultWithoutVisa" class="field_input" tabindex="4" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" /></td>                        
                    </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                Child with Visa</td>
                            <td align="left">
                                <input id="txtChildWithVisa" class="field_input" tabindex="5" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                Child without Visa</td>
                            <td align="left">
                                <input  id="txtChildWithoutVisa" class="field_input" tabindex="6" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" />
                            </td>
                        </tr>
                         <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="label1" runat="server" CssClass="td_ce" Text="Child Free Upto" 
                                    ViewStateMode="enabled" Width="220px"></asp:Label>
                            </td>
                            <td align="left">
                                  <input  id="txtchildfreeupto" class="field_input" tabindex="7" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" />
                            </td>
                        </tr>
                         <tr>
                            <td align="left" class="td_cell">
                                Child Free with Visa</td>
                            <td align="left">
                                <input id="txtChildfreeWithVisa" class="field_input" tabindex="8" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                Child Free without Visa</td>
                            <td align="left">
                                <input  id="txtChildfreeWithoutVisa" class="field_input" tabindex="9" type="text"   onkeypress="validateDecimalOnly(event,this)"
                                maxlength="20" runat="server"  style="width: 220px" />
                            </td>
                        </tr>
                          <tr>
                            <td align="left" class="td_cell">
                                <asp:Label ID="label5" runat="server" CssClass="td_ce" Text="Active" 
                                    ViewStateMode="enabled" Width="44px"></asp:Label>
                            </td>
                            <td align="left">
                                <input id="chkActive" tabindex="9" type="checkbox" checked="checked" 
                                runat="server" />
                            </td>
                        </tr>
                        
                                              <tr>
                                                  <td align="left" class="td_cell">
                                                      <asp:Label ID="label2" runat="server" CssClass="td_ce" Text="Currency" 
                                                          ViewStateMode="Enabled" Width="44px"></asp:Label>
                                                  </td>
                                                  <td align="left">
                                                    <asp:TextBox ID="TxtCurrencyName" runat="server" AutoPostBack="True" CssClass="field_input"
                                    MaxLength="500" TabIndex="8" Width="220px"></asp:TextBox>
                                <asp:TextBox ID="TextCurrencyCode" runat="server" Style="display: none"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="CurrencyNameAutoCompleteExtender" runat="server" CompletionInterval="10"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1" 
                                    ServiceMethod="GetCurrencyName" TargetControlID="TxtCurrencyName" OnClientItemSelected="CurrencyNameautocompleteselected">
                                </asp:AutoCompleteExtender></td>
                                              </tr>
                        <tr>
                            <td align="left" class="td_cell">
                                Applicable To <span class="td_cell" style="color: red">*</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtApplicableTo" runat="server" class="field_input" 
                                    maxlength="2000" style="width:100%;height:50px;" tabindex="9" 
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                       

                    </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" valign="top" colspan="10">
                                            <div style="width: 100%">
                                                <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                                                    <div ID="VS" class="container" style="border: 0px;">
                                                        <div ID="search_box_container">
                                                        </div>
                                                        <%-- Input Grid --%>
                                                        <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                                                            Style="display: none"></asp:TextBox>
                                                        <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                                                            Style="display: none"></asp:TextBox>
                                                        <%-- Show Grid --%>
                                                        <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                    </div>
                                                </div>
                                                <div style="width: 18%; display: inline-block; vertical-align: top;">
                                                    <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" 
                                                        Font-Bold="False" TabIndex="14" Text="Reset Search" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td colspan="10">
                                            <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <table style="border: 0px;">
                                                        <tr style="">
                                                            <td style="border: 0px;">
                                                                <asp:Button ID="lnkCode" runat="server" class="button button4" Style="display: none"
                                                                    Text='<%# Eval("Code") %>' />
                                                                <asp:Button ID="lnkValue" runat="server" class="button button4" Style="display: none"
                                                                    Text='<%# Eval("Value") %>' />
                                                                <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" Font-Bold="False"
                                                                    OnClick="lnkCodeAndValue_Click" Font-Size="Small" ForeColor="#000099" Text='<%# Eval("CodeAndValue") %>' />
                                                                <asp:Button ID="lbClose1" runat="server" class="buttonClose button4" OnClick="lbCloseSearch_Click"
                                                                    Text="X" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td colspan="10">
                                            <uc1:Countrygroup ID="ucMinMarkupPolicy" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:Label ID="Label3" runat="server" BackColor="#CCCCCC" 
                                                CssClass="field_input" Font-Bold="True" Height="25px" 
                                                Text="Discount per booking over and above minimum markup" Width="395px"></asp:Label>
                                        </td>

                                         <td>
                                            <asp:Label ID="Label4" runat="server" BackColor="#CCCCCC" 
                                                CssClass="field_input" Font-Bold="True" Height="25px" 
                                                Text="Markup Period" Width="395px"></asp:Label>
                                        </td>
                                       
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:GridView ID="gvCommFormula" runat="server" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Serial No" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFLineNo" runat="server" Text='<%# Eval("fLineNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFrom" runat="server" CssClass="fiel_input" 
                                                                onkeypress="validateDigitOnly(event)" Text="0" Width="60px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTo" runat="server" CssClass="fiel_input" 
                                                                onkeypress="validateDigitOnly(event)" Width="60px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Discount %" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="fiel_input" 
                                                                onkeypress="validateDecimalOnly(event,this)" Width="60px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnGridAddRow" runat="server" onclick="btnGridAddRow_Click" 
                                                                Text="Add Row" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDelete" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" />
                                                <HeaderStyle CssClass="grdheader" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </td>
                                          <td  valign="top">
                                            <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                                                  <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                     CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                    Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                    <FooterStyle CssClass="grdfooter" />
                                                    <Columns>
                                                        <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                                                                                                    
                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                        <asp:TextBox ID="txtfromDate" runat="server"   CssClass="fiel_input" Width="80px" ></asp:TextBox>
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
                                                        </cc1:MaskedEditValidator>
                                                        </ItemTemplate>
                                                        <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                            <asp:TextBox ID="txtToDate" runat="server"  CssClass="fiel_input" Width="80px" ></asp:TextBox>
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
                                                        </cc1:MaskedEditValidator>
                                                        </ItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgStayAdd" runat="server" ImageUrl="~/Images/PlusGreen.ico"
                                                                Width="18px" OnClick="imgStayAdd_Click"  />
                                                            <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                    Width="18px"  OnClick="imgSclose_Click" ToolTip="Delete Current Row" />
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
                                    <tr align="left">
                                        <td>
                                            <asp:Button ID="btnAddRow" runat="server" CssClass="btn" 
                                                OnClick="btnAddRow_Click" TabIndex="45" Text="Add Row" />
                                            &nbsp;<asp:Button ID="btnDeleteRow" runat="server" CssClass="btn" 
                                                OnClick="btnDeleteRow_Click" TabIndex="46" Text="Delete Row" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr align="center">
                        <td class="td_cell" style="height: 23px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="16" Text="Save" />&nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="17" Text="Return to Search" />&nbsp;
                            <asp:Button ID="btnHelp" runat="server" CssClass="btn" TabIndex="18" Text="Help" onClick="btnHelp_click"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </tbody>
            </table>
          
        </ContentTemplate>
    </asp:UpdatePanel>
      <div id="div1">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/clsVisualSearchService.asmx" />
            </Services>

        </asp:ScriptManagerProxy>
    </div>
</asp:Content>
