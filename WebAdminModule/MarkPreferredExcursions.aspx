<%@ Page Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false" CodeFile="MarkPreferredExcursions.aspx.vb" Inherits="MarkPreferredExcursions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
            width: 100%;
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
        $txtvsprocess.val('EXCURSIONGROUP:" "EXCURSIONTYPE:" "CLASSIFICATION:" "RATEBASIS:" "TEXT:" "');

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
                        case 'EXCURSIONGROUP':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsVisualSearchService.GetListOfExcursionVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'EXCURSIONTYPE':
                            var asSqlqry = '';
                            glcallback = callback;
                            //fnTestCallback();
                            ColServices.clsVisualSearchService.GetListOfExcursionTypeVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'CLASSIFICATION':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsVisualSearchService.GetListOfExcursionclassiVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                        case 'RATEBASIS':
                            var asSqlqry = '';
                            glcallback = callback;
                            ColServices.clsVisualSearchService.GetListOfExcRateBasisVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                            break;
                       
                     
                    }
                },
                facetMatches: function (callback) {
                    callback([
                { label: 'TEXT', category: 'BANKTYPE' },
                { label: 'EXCURSIONTYPE', category: 'BANKTYPE' },
                     { label: 'CLASSIFICATION', category: 'BANKTYPE' },
                          { label: 'RATEBASIS', category: 'BANKTYPE' },
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
   <TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%; BORDER-BOTTOM: gray 2px solid;vertical-align:top;"  ><TBODY>
      <tr>
            <td 
             class="style6">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate> 
                        <table style="width: 1118px; height: 85px;">
                            <tbody>
                            <TR>
    <TD style="HEIGHT: 15px" class="field_heading" align=center colSpan=4 
        width="10">
    <asp:Label id="lblHeading" runat="server" Text="  Preferred Excursions" 
        Width="738px" CssClass=""></asp:Label></TD></TR><TR>
        <TD style="HEIGHT: 3px; text-align: center;" align="center" colspan="4">
            &nbsp;</TD>
    </TR>
                       
                                <tr>
                                    <td align="center" colspan="6">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnhelp" TabIndex="3" OnClick="btnhelp_Click"
                                            runat="server" Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>
                                &nbsp;&nbsp;<asp:Button ID="btnPrint"
                                                TabIndex="5" runat="server" Text="Report" CssClass="btn"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center" class="field_input">
                                        <span style="color: blue">Type few characters of code or name and click search </span>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
        </ContentTemplate>
                </asp:UpdatePanel> 
            </td>
        </tr>

        <tr><td class="style9" align ="right" ></td>
                       
        </tr> 
        <tr>
        <td class="style10"></td></tr>
        <tr>
            <td class="style7">
                <div style="width: 92%; min-height: 25px;">
                    <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0; min-height: 23px;">
                        <div id="VS" class="container" style="border: 0px;">
                            <div id="search_box_container">
                            </div>
                            <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                            <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                        </div>
                       
                        
                    </div>
                    <div style="width: 18%; display: inline-block; vertical-align: top;">
                    <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                            TabIndex="4" Text="Reset Search" />                   
                        </div>
                </div>
                

                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" 
                    RepeatDirection="Horizontal" Height="58px">
                    <ItemTemplate>
                        <table class="styleDatalist" >
                            <tr style="">
                                <td >
                                    <asp:Button ID="lnkCode" runat="server" class="button button4" Style="display: none"
                                        Text='<%# Eval("Code") %>' />
                                    <asp:Button ID="lnkValue" runat="server" class="button button4" Style="display: none"
                                        Text='<%# Eval("Value") %>' />
                                    <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" Font-Bold="False"
                                        Font-Size="Small" ForeColor="#000099" OnClientClick="return false;" Text='<%# Eval("CodeAndValue") %>' />
                                    <asp:Button ID="lbClose" runat="server" class="buttonClose button4" OnClick="lbClose_Click"
                                        Text="X" />
                                </td>
                            </tr>
                            <tr>
                            <td>
                            &nbsp;</td></tr>
                                         </table>
                    </ItemTemplate>
                </asp:DataList>
        
            </td>
        </tr>
        <tr>
            <td class="style7">
                <table >
                         <tr>
                            <td>
                            &nbsp;</td></tr>
                    <tr>
                        <td>
                                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                                    TabIndex="6" /></td>

                                     <td style="display:none">
                                <asp:Button ID="btnAddNew" runat="server" CssClass="btn" 
                                    TabIndex="6" /></td>
                                              </tr>
                </table>
            </td>
        </tr>
            
                          
        <tr>
            <td class="style7">
                <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    width: 100%; border-bottom: gray 1px solid">
                    <tr>
                        <td style="width: 100%; height: 21px">
                              <DIV style=" HEIGHT: 500px; overflow: auto;">
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView id="gv_SearchResult" tabIndex=8  runat="server" Font-Size="10px" Width="100%"
                CssClass="grdstyle" __designer:wfdid="w61" GridLines="Vertical" CellPadding="3"
                BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True">
                                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                                        <Columns>
                                        <asp:TemplateField Visible ="false"  HeaderText="Excursion Type Code">
                        

                                        <ItemTemplate>
 
                                              <asp:Label ID="lblpartyCode" runat="server" Text='<%# Bind("exctypcode") %>'></asp:Label>
                                         </ItemTemplate> 
                                        </asp:TemplateField>
                                            <asp:BoundField DataField="exctypcode" HeaderText="Excursion Type Code">
                                               <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                             
                                           <asp:TemplateField HeaderText ="Excursion Type Name">
                                            <ItemTemplate >
                                            <asp:Label ID="lblpartyname"  runat="server" Text='<%#Bind("exctypname")%>'></asp:Label>
                                            </ItemTemplate>
                                             <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="excursiongroupcode" HeaderText="Group Code" Visible="false">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText ="Excursion Group Name">
                                            <ItemTemplate>
                                            <asp:Label ID="lblCategory"   runat="server" Text='<%#Bind("excursiongroup")%>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText ="Classification Name">
                                            <ItemTemplate>
                                            <asp:Label ID="lblSector"   Width="130px" runat="server" Text='<%#Bind("classificationname")%>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:BoundField DataField="Active" HeaderText="Active"></asp:BoundField>
                                            <asp:TemplateField HeaderText ="Rate Basis">
                                            <ItemTemplate >
                                            <asp:Label ID="lblCity" Width="120px" runat="server" Text='<%#Bind("ratebasis")%>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preferred">
                                                <HeaderStyle Width="5%"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server"  ></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <RowStyle CssClass="grdRowstyle" />
                <SelectedRowStyle CssClass="grdselectrowstyle" />
                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                <HeaderStyle CssClass="grdheader" ForeColor="white" />
                <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
                                    <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                                        Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" __designer:wfdid="w28"
                                        Visible="False" CssClass="lblmsg"></asp:Label>
                        </ContentTemplate>
                            </asp:UpdatePanel>
                </DIV> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" abbr=" " class="style8">
                <asp:Button ID="btnSave" TabIndex="14" CssClass="field_button"  runat="server" Text="Save"
                 ></asp:Button>

                &nbsp;
                <asp:Button ID="btnExit" onclick="btnExit_Click" CssClass="field_button"  TabIndex="15" runat="server" Text="Exit"
                  ></asp:Button>
            </td>
        </tr>

        <asp:Button ID="dummybtnPivotGridupdate" runat="server" Style="display: none;" ClientIDMode="Static" />


    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsVisualSearchService.asmx"/>
            <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
