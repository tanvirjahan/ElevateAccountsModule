<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LockAgentsforWeb.aspx.vb"
    Inherits="LockAgentsforWeb" MasterPageFile="~/WebAdminMaster.master" Strict="true" %>

     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
    <asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
    
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
   
    <style  type="text/css">
          .styleDatalist
    {
        width: 100%;
    }
    
    div.container {
      border:0px;
    }
 
    #VS code, #VS pre, #VS tt {
      font-family: Monaco, Consolas, "Lucida Console", monospace;
      font-size: 12px;
      line-height: 18px;
      color: #444;
      background: none;
    }
      #VS code {
        margin-left: 8px;
        padding: 0 0 0 12px;
        font-weight: normal;
      }
      #VS pre {
        font-size: 12px;
        padding: 2px 0 2px 0;
        border-left: 6px solid #829C37;
        margin: 12px 0;
      }
    #search_query {
      margin: 8px 0;
      opacity: 0;
    }
      #search_query .raquo {
        font-size: 18px;
        line-height: 12px;
        font-weight: bold;
        margin-right: 4px;
      }
    #search_query2 {
      margin: 18px 0;
      opacity: 0;
    }
      #search_query2 .raquo {
        font-size: 18px;
        line-height: 12px;
        font-weight: bold;
        margin-right: 4px;
      }
        .style1
        {
            width: 99%;
        }
        .style2
        {
            width: 180px;
        }
        .style3
        {
            width: 169px;
        }
        .style4
        {
            width: 160px;
        }
        .style5
        {
            width: 62px;
        }
        .style6
        {
            width: 50px;
        }
        .style8
        {
            font-family: Verdana,Arial, Geneva, ms sans serif;
            font-size: 10pt;
            font-weight: normal;
            height: 22px;
            width: 913px;
        }
        .style9
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 9pt;
            font-weight: bold;
            background-color: #06788B;
            color: #ffffff;
            height: 11px;
            width: 109%;
        }
        .style10
        {
            font-family: Verdana,Arial, Geneva, ms sans serif;
            font-size: 10pt;
            font-weight: normal;
            width: 109%;
        }
        .style11
        {
            width: 109%;
        }
        .style12
        {
            width: 844px;
        }
        .style13
        {
            width: 913px;
        }
  </style>

<script type="text/javascript" charset="utf-8">

    
    $(document).ready(function () {
        visualsearchbox();

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
        $txtvsprocess.val('Customer:" " CustomerGroup:" "CountryGroup:" "Category:" "Country:" "City:" " Sector:" " TEXT:" "');
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
                        case 'Country':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsSectorServices.GetListOfCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'City':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsSectorServices.GetListOfCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'Category':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsSectorServices.GetListOfAgentCatNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'CustomerGroup':
                            var asSqlqry = '';
                            glcallback = callback;
                            //fnTestCallback();
                            ColServices.clsHotelGroupServices.GetListOfArrayCustomerGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'Customer':
                            var asSqlqry = '';
                            glcallback = callback;
                            //fnTestCallback();
                            ColServices.clsHotelGroupServices.GetListOfArrayCustomerVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'Sector':
                            var asSqlqry = '';
                            glcallback = callback;
                            //fnTestCallback();
                            ColServices.clsHotelGroupServices.GetListOfArrayASectorVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'CountryGroup':
                            var asSqlqry = '';
                            glcallback = callback;
                            //fnTestCallback();
                            ColServices.clsHotelGroupServices.GetListOfArrayCountryGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                      
                    }
                },
                facetMatches: function (callback) {
                    callback([
                { label: 'TEXT', category: 'AGENTS' },
                { label: 'CountryGroup', category: 'AGENTS' },
                           { label: 'Customer', category: 'AGENTS' },
                { label: 'Sector', category: 'AGENTS' },
                                           { label: 'Category', category: 'AGENTS' },
                { label: 'City', category: 'AGENTS' },
                       { label: 'Country', category: 'AGENTS' },
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
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);

        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
        }
    </script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid" 
            class="style12">
        <tr>
 
            <td colspan="6" class="style13">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="width: 871px"><TBODY>   <tr>
                        <td align="center" class="style9">
              Lock Agents For Web
                        </td>
                    </tr>
        
        <TR>
<TD align=center class="style11">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" 
        tabIndex=3 onclick="btnhelp_Click" runat="server" Text="Help" 
        Font-Bold="False" CssClass="search_button"></asp:Button> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
        id="btnPrint" tabIndex=5 runat="server" Text="Report" CssClass="btn"></asp:Button></TD>
        
<td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
    </td>
        </TR>
        
        <TR>
          <td style="color: blue;" align="center" class="style10">
                            Type few characters of code or name and click search
                        </td></TR>
    <tr>
        <td align="center" class="style10" style="color: blue;">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" 
                Font-Bold="False" tabIndex="4" Text="Reset Search" />
            &nbsp;</td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            
                </td>
        </tr>

                            <tr>
        <td colspan="6" class="style13">
   <div style="width: 100%; display: inline-block; ">
                        <div id="Div1" class="container" style="border: 0px;">
            <div ID="VS" class="container" style="border:0px;">
                <div ID="search_box_container">
                </div>                        
                <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" 
                    style="display:none"></asp:TextBox>
                <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" 
                    style="display:none"></asp:TextBox>
                <asp:Button ID="btnvsprocess" runat="server" style="display:none" />
            </div> </div> 
             <div style=" display: inline-block;vertical-align:top;">
                        </div></div>
            <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                RepeatDirection="Horizontal" Width="834px">
                <ItemTemplate>
                    <table class="styleDatalist" style="border:0px;">
                        <tr style="">
                            <td style="border:0px;">
                                <asp:Button ID="lnkCode" runat="server" class="button button4" 
                                    style="display:none" text='<%# Eval("Code") %>' />
                                <asp:Button ID="lnkValue" runat="server" class="button button4" 
                                    style="display:none" text='<%# Eval("Value") %>' />
                                <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" 
                                    Font-Bold="False" Font-Size="Small" ForeColor="#000099" 
                                    OnClientClick="return false;" text='<%# Eval("CodeAndValue") %>' />
                                <asp:Button ID="lbClose" runat="server" class="buttonClose button4" 
                                    onclick="lbClose_Click" Text="X" />
                                      </td>
                 
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        &nbsp;</td>
    </tr>
    <tr>
        <td colspan="6" class="style13">
            <table class="style1">

                <tr>
 
                 
                    <td class="style2">
                    <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" 
                    Text="Export To Excel" TabIndex="6" />
                    </td>
                    <td>
                        &nbsp;</td>
                    <td class="style5">
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td class="style6">
                        &nbsp;</td>
                    <td class="style4">
                        &nbsp;</td>
                   
                    <td>
                        &nbsp;&nbsp;</td>              <td>  &nbsp;</td>
                <tr>
                <td class="style2">
                    &nbsp;</td></tr>
            </table>
        </td>
    </tr>
<tr>
  <td class="style13">
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        width: 100%; border-bottom: gray 1px solid">
        <tr>
            <td style="width: 100%; height: 21px">
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<div style="height: 400px" class="container">
                                                <asp:GridView ID="grdSupplier" runat="server" Font-Size="10px" CssClass="grdstyle"
                                                    Width="776px" AllowSorting="True" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False">
                                                    <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="agentcode" SortExpression="agentcode" HeaderText="Customer Code">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Customer Name">
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Lock">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkbox"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reason">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReason" runat="server" CssClass="txtbox" Width="304px" MaxLength="200"
                                                                    __designer:wfdid="w63"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                            </div>
 <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" 
                __designer:wfdid="w28" Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
    </table>

    </td>
     <tr>
                                        <td class="style8" align="right" colspan="5">
                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn">
                                            </asp:Button>&nbsp;
                                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" CssClass="btn">
                                            </asp:Button>
                                        </td>
                                    </tr>
    </tr>    </table>
     <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
              <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />

        </Services>
    </asp:ScriptManagerProxy>

</asp:Content>


