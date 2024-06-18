<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcSuppCtryGroups.aspx.vb" Inherits="ExcSuppCtryGroups" %>

<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl"
    TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../PriceListModule/Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
<%@ OutputCache Location="none" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"  charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>

    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {
            //AutoCompleteExtenderKeyUp();
            visualsearchbox();
            //AutoCompleteExtenderUserControlKeyUp();
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

                placeholder: 'Search for..',
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
            //AutoCompleteExtenderKeyUp();
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
            //AutoCompleteExtenderUserControlKeyUp();
        }

    </script>
     
 

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 800px; border-bottom: gray 2px solid" class="td_cell" align="left">
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
                            &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl>
                        </td>
                        <td style="width: 85%" valign="top">
                            <%-- <DIV style="WIDTH: 682px" id="iframeINF" runat="server">--%>
                            <table style="width: 100%" align="center">
                                <tbody>
                                    <%-- <tr>
                                        <td style="height: 18px" class="td_cell" align="center" colspan="2">
                                            <asp:Label ID="lblHeading" runat="server" Text="Excursion-Sector Details" ForeColor="White"
                                                CssClass="field_heading" Width="921px" Height="16px"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr style="color: #ff0000">
                                        <td style="width: 145px; height: 27px" class="td_cell" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <div id="searchresults">
                                            <td style="width: 100%" valign="top" colspan="5">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="Panelsearch" runat="server" Font-Bold="true" GroupingText="Supplier Details"
                                                                        Width="100%">
                                                                        <div id="Showdetails" runat="server" style="width: 900px; border: 3px solid #2D7C8A;
                                                                            background-color: White;">
                                                                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                                                Font-Bold="true" Font-Size="10px" GridLines="Vertical" TabIndex="13" Width="100%">
                                                                                <FooterStyle CssClass="grdfooter" />
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Exc Type Code" Visible="False">
                                                                                        <EditItemTemplate>
                                                                                        </EditItemTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbltranid" runat="server" __designer:wfdid="w1" Text='<%# Bind("exctypcode") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Supplier Code" >
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
                                                           
                                                            <asp:Label ID="lblcountries" runat="server"    Text='<%# Limit(Eval("countries"), 10)%>' Tooltip='<%# Eval("countries")%>'  ></asp:Label>
                                                            <br />
                                                             <asp:LinkButton ID="ReadMoreLinkButton" runat="server" Text="More" Visible='<%# SetVisibility(Eval("countries"), 10) %>'    OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>

                                                            </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Agents">
                                                             <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblAgents" runat="server"    Text='<%# Limit(Eval("Agents"), 10)%>' Tooltip='<%# Eval("Agents")%>'  ></asp:Label>
                                                            <br />
                                                             <asp:LinkButton ID="ReadMoreLinkButtonAgt" runat="server" Text="More" Visible='<%# SetVisibility(Eval("Agents"), 10) %>'    OnClick="ReadMoreLinkButtonAgt_Click"></asp:LinkButton>

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
                                        <td colspan="50">
                                            <asp:Panel ID="PanelCtryGrps" runat="server" Font-Bold="true" GroupingText="Country Groups Details"
                                                Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td colspan="10">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                Supplier&nbsp;Code&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                <asp:TextBox ID="txtpartycode" runat="server" CssClass="field_input" TabIndex="1"
                                                                                    ReadOnly="true" Width="100px"></asp:TextBox>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Supplier&nbsp;Name&nbsp;&nbsp;&nbsp;
                                                                                <asp:TextBox ID="txtpartyname" runat="server" CssClass="field_input" TabIndex="2"
                                                                                    ReadOnly="true" Width="300px"></asp:TextBox>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div id="divuser" runat="server">
                                                                                            <div id="VS" class="container">
                                                                                                <div id="search_box_container">
                                                                                                </div>
                                                                                            </div>
                                                                                            <br />
                                                                                            <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                                                                <ItemTemplate>
                                                                                                    <table class="styleDatalist">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Button ID="lnkCode" runat="server" class="button button4" Style="display: none"
                                                                                                                    Text='<%# Eval("Code") %>' />
                                                                                                                <asp:Button ID="lnkValue" runat="server" class="button button4" Style="display: none"
                                                                                                                    Text='<%# Eval("Value") %>' />
                                                                                                                <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" OnClick="lnkCodeAndValue_Click"
                                                                                                                    Text='<%# Eval("CodeAndValue") %>' />
                                                                                                                <asp:Button ID="lbClose" runat="server" class="buttonClose button4" OnClick="lbClose_Click"
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
                                                                                            <div id="countrygroup1" style="float: left; margin-left: 10px; width: 100%">
                                                                                                <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                         
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                            <asp:HiddenField ID="hdnMainGridRowid1" runat="server" />
                                        </td>
                                    </tr>
                                                           <tr>
                                                                                    <td align="center">
                                                                                        <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="24" Text="Save"
                                                                                            Width="93px" />
                                                                                        &nbsp;
                                                                                        <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                                                            Width="139px" />
                                                                                        &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="26"
                                                                                            Text="Help" Visible="false" />
                                                                                    </td>
                                                                                    <td>
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
