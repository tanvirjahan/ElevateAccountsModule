<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="ExcSuppCountryGrps.aspx.vb" Inherits="ExcSuppCountryGrps" %>

<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../PriceListModule/Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {


          AutoCompleteExtenderKeyUp();
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

      function AutoCompleteExtenderKeyUp() {
          //          $("#< %= txthotelname.ClientID %>").bind("keyup", function (e) {
          //              if (fnClearTextBoxNeeded(e.keyCode) == true)
          //              {
          //              document.getElementById('< %=txthotelcode.ClientID%>').value = '';
          //              }
          //              //var keybr = e.keyCode; //e.which || e.keyCode;             
          //              //if (keybr == 9 || keybr == 16 || keybr == 17 || keybr == 18 || keybr == 20 || keybr == 27 || keybr == 45 || keybr == 36 || keybr == 33 || keybr == 35 || keybr == 34 || keybr == 37 || keybr == 38 || keybr == 39 || keybr == 40 || keybr == 93 || keybr == 92 || keybr == 112 || keybr == 123 || keybr == 144 || keybr == 145 || keybr == 19 || keybr == 13) {
          //              //    //text
          //              //}
          //              //else {
          //              //    $("#< %= txthotelcode.ClientID %>").val('');
          //              //}
          //              //document.getElementById(' < %=txthotelcode.ClientID%>').value = '';
          //      });



      }

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
    <script type="text/javascript">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequestUserControl);
    prm.add_endRequest(EndRequestUserControl);

    function InitializeRequestUserControl(sender, args) {

    }

    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
        // after update occur on UpdatePanel re-init the Autocomplete
        visualsearchbox();
        AutoCompleteExtenderUserControlKeyUp();
    }
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="2">
                            <asp:Label ID="lblmainheading" runat="server" Text="Excursion Types" ForeColor="White"
                                Width="800px" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%" valign="top" align="left">
                            <span style="color: #ff0000" class="td_cell"></span>
                        </td>
                        <td style="width: 85%" class="td_cell" valign="top" align="left">
                            Code <span style="color: #ff0000" class="td_cell">*&nbsp; </span>
                            <input style="width: 196px" id="txtCustomerCode" class="field_input" tabindex="1"
                                type="text" maxlength="20" runat="server" />
                            &nbsp; Name&nbsp;
                            <input style="width: 274px" id="txtCustomerName" class="field_input" tabindex="2"
                                type="text" maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                      <td style="width: 15%" valign="top" align="left">
                            <div id="menudiv" style="height: 402px">
                                <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                            </div>
                            </td>
                      <td style="width: 85%" valign="top">
                            <%-- <DIV style="WIDTH: 682px" id="iframeINF" runat="server">--%>
                            <table style="width: 100%" align="center">
                                <tbody>
                                    <tr>
                                        <div id="searchresults">
                                            <td style="width: 100% ">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <table >
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="Panelsearch" runat="server" Font-Bold="true" GroupingText="Supplier Details"
                                                                        Width="100%">
                                                                        <div id="Showdetails"  runat="server" style="width: 900px; border: 3px solid #2D7C8A;
                                                                            background-color: White;">
                                                                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3"  
                                                                                Font-Bold="true" Font-Size="10px" GridLines="Vertical"  TabIndex="13" Width="100%">
                                                                                <FooterStyle CssClass="grdfooter" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Exc Type Code" Visible="False">
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbltranid" runat="server" __designer:wfdid="w1" Text='<%# Bind("exctypcode") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Supplier Code">
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblpartycode" runat="server" __designer:wfdid="w1" Text='<%# Bind("partycode") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Supplier">
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblpartyname" runat="server" __designer:wfdid="w1" Text='<%# Bind("partyname") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Countries">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblcountries" runat="server" Text='<%# Limit(Eval("countries"), 10)%>'
                                                                                                ToolTip='<%# Eval("countries")%>'></asp:Label>
                                                                                            <br />
                                                                                            <asp:LinkButton ID="ReadMoreLinkButton" runat="server" Text="More" Visible='<%# SetVisibility(Eval("countries"), 10) %>'
                                                                                                OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Agents">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAgents" runat="server" Text='<%# Limit(Eval("Agents"), 10)%>' ToolTip='<%# Eval("Agents")%>'></asp:Label>
                                                                                            <br />
                                                                                            <asp:LinkButton ID="ReadMoreLinkButtonAgt" runat="server" Text="More" Visible='<%# SetVisibility(Eval("Agents"), 10) %>'
                                                                                                OnClick="ReadMoreLinkButtonAgt_Click"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:ButtonField CommandName="EditRow" HeaderText="Action" Text="Edit">
                                                                                        <ItemStyle ForeColor="Blue" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:ButtonField CommandName="View" HeaderText="Action" Text="View">
                                                                                        <ItemStyle ForeColor="Blue" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:ButtonField CommandName="DeleteRow" HeaderText="Action" Text="Delete">
                                                                                        <ItemStyle ForeColor="Blue" />
                                                                                    </asp:ButtonField>
                                                                                    <asp:BoundField DataField="adddate" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"
                                                                                        HeaderText="Date Created" HtmlEncode="False" />
                                                                                    <asp:BoundField DataField="adduser" HeaderText="User Created" />
                                                                                    <asp:BoundField DataField="moddate" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"
                                                                                        HeaderText="Date Modified" HtmlEncode="False" />
                                                                                    <asp:BoundField DataField="moduser" HeaderText="User Modified" />
                                                                                </Columns>
                                                                                <RowStyle CssClass="grdRowstyle" />
                                                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </div>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSuppliercode" runat="server" Text="Supplier Code"></asp:Label>
                                            &nbsp; &nbsp;
                                            <asp:TextBox ID="txtpartycode" runat="server" CssClass="field_input" TabIndex="1"
                                                ReadOnly="true" Width="100px"></asp:TextBox>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSuppliername" runat="server" Text="Supplier Name"></asp:Label>
                                            &nbsp;
                                            <asp:TextBox ID="txtpartyname" runat="server" CssClass="field_input" TabIndex="2"
                                                ReadOnly="true" Width="300px"></asp:TextBox>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px" valign="top">
                                            <asp:Panel ID="PanelCtryGrps" runat="server" Font-Bold="true" GroupingText="Country Groups Details"
                                                Width="100%">
                                                <div style="width: 100%; min-height: 400px" id="iframeINF" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                                                <%--<asp:textbox id="txtvsprocessCity" runat="server"></asp:textbox>
                                <asp:textbox id="txtvsprocessCountry" runat="server"></asp:textbox>--%>
                                                                <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                            </div>
                                                            <div id="countrygroup1" style="float: left; margin-left: 40px; width: 90%">
                                                                <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                                            </div>
                                                            <script language="javascript">
                                            formmodecheck();
                                            load();
                                                            </script>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="24" Text="Save"
                                                Width="93px" />
                                            <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                Width="139px" />
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">

                                        <asp:Button ID="Button1" runat="server" onclick="btnHelp_Click" CssClass="field_button" TabIndex="26" Text="Help"
                                                Visible="true" />
                                                                               
                                            <asp:Button ID="BtnCancelsearch" runat="server" CssClass="field_button" 
                                                TabIndex="26" Text="Return To Search" Width="139px" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    </tbody> 
                    </table> 
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
