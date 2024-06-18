<%@ Page Title="" Language="VB" MasterPageFile="~/WebAdminMaster.master" AutoEventWireup="false"
    CodeFile="ApproveCustomersforWeb.aspx.vb" Inherits="ApproveCustomersforWeb" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
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
    <style type="text/css">
        .styleDatalist
        {
            width: 100%;
        }
        
        div.container
        {
            border: 0px;
        }
        
        #VS code, #VS pre, #VS tt
        {
            font-family: Monaco, Consolas, "Lucida Console" , monospace;
            font-size: 12px;
            line-height: 18px;
            color: #444;
            background: none;
        }
        #VS code
        {
            margin-left: 8px;
            padding: 0 0 0 12px;
            font-weight: normal;
        }
        #VS pre
        {
            font-size: 12px;
            padding: 2px 0 2px 0;
            border-left: 6px solid #829C37;
            margin: 12px 0;
        }
        #search_query
        {
            margin: 8px 0;
            opacity: 0;
        }
        #search_query .raquo
        {
            font-size: 18px;
            line-height: 12px;
            font-weight: bold;
            margin-right: 4px;
        }
        #search_query2
        {
            margin: 18px 0;
            opacity: 0;
        }
        #search_query2 .raquo
        {
            font-size: 18px;
            line-height: 12px;
            font-weight: bold;
            margin-right: 4px;
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
//            $txtvsprocess.val('Country:" "City:" " Category:" "TEXT:" "');
            $txtvsprocess.val('Customer:" " CustomerGroup:" " CountryGroup:" " Country:" " City:" "  Sector:" " Category:" " Text:" "');
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
                            case 'Category':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfAgentCatNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'City':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Country':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Customer':
                                var asSqlqry = '';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsHotelGroupServices.GetListOfArrayCustomerVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                            case 'CustomerGroup':
                                var asSqlqry = '';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsHotelGroupServices.GetListOfArrayCustomerGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
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
                { label: 'TEXT', category: 'narration' },
                { label: 'Category', category: 'narration' },
                       { label: 'City', category: 'narration' },
                             { label: 'Country', category: 'narration' },
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
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid">
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="width: 100%; height: 11px" align="center" class="field_heading">
                       Approve Customers For Web
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; color: blue;" align="center" class="td_cell">
                            Type few characters of code or name and click search
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                           <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>
                                    <table style="width: 1011px">
                                        <tbody>
                                            <tr>
                                                <td align="center">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnHelp" TabIndex="3" OnClick="btnhelp_Click"
                                                        runat="server" Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnPrint"
                                                            TabIndex="5" runat="server" Text="Report" CssClass="btn"></asp:Button>
                                                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                                                        TabIndex="6" />
                                                </td>
                                                
                                            </tr>
                                        </tbody>
                                    </table>
                                                         <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                       <tr>
            <td colspan="6">
                <div style="width: 100%">
                    <div style="width: 80%; display: inline-block; ">
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
                            TabIndex="4" Text="Reset Search" /></div>
                </div>
                <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <table class="styleDatalist" style="border: 0px;">
                            <tr style="">
                                <td style="border: 0px;">
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
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
                   </table>
             
            </td>
        </tr>
        <tr>
        
        
        <td style="display:none">
               <asp:Button id="btnAddNew" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
    </td>
           
            <td>
             &nbsp;</td>
        </tr>
         <TR>
    <TD style="HEIGHT: 22px"><asp:Button id="btnSendMail" tabIndex=16 
              runat="server" Text="Send Mail to Selected" 
            CssClass="field_button" ></asp:Button><%--OnClientClick="return validatepage();"--%>
        &nbsp;
        <asp:Button ID="Btnmailcontract" runat="server" CssClass="field_button" 
            TabIndex="17" Text="Send Mail Contract" /><%--OnClick="Btnmailcontract_Click"--%>
        &nbsp;<asp:Button ID="btnSelectforApprove" runat="server" CssClass="field_button"
            OnClick="btnSelectforApprove_Click" TabIndex="17" 
            Text="Select All for Approve" />&nbsp;
        <asp:Button id="btnApprove" tabIndex=18 onclick="btnApprove_Click" 
            runat="server" Text="Approve Selected" CssClass="field_button" 
            OnClientClick="return validatepage();"></asp:Button>&nbsp;
        <asp:Button id="btnSelectforRemove" tabIndex=19 
            onclick="btnSelectforRemove_Click" runat="server" Text="Select All for Remove" 
            CssClass="field_button"></asp:Button>&nbsp;
        <asp:Button id="btnDeletefromWeb" tabIndex=20 onclick="btnDeletefromWeb_Click" 
            runat="server" Text="Remove Selected From Web " CssClass="field_button"></asp:Button>&nbsp;
        <asp:Button id="btnExit" tabIndex=21 onclick="btnExit_Click" runat="server" Text="Exit" Width="42px" CssClass="field_button"></asp:Button>&nbsp; &nbsp;
        <asp:Button id="Button1" tabIndex=22 onclick="btnHelp_Click" runat="server" Text="Help" Width="45px" CssClass="field_button"></asp:Button>
        </TD></TR>
        <tr>
            <td style="width: 100%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                      <TR><TD style="HEIGHT: 22px">
                     
       <DIV style="WIDTH: 1300px;; HEIGHT: 500px; overflow: auto;">
       
           <asp:GridView id="grdUploadClients" tabIndex=8 runat="server" Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" AutoGenerateColumns="False" AllowSorting="True">
                                        <FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
                                  
            <FooterStyle CssClass="grdfooter"  ForeColor="Black"></FooterStyle>
            <Columns>
            <asp:TemplateField Visible="False" HeaderText="S. Agent Code"><EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("agentcode") %>'></asp:TextBox>
                                            
            
            </EditItemTemplate>
            <ItemTemplate>
                        <asp:Label id="lblagentCode" runat="server" Text='<%# Bind("agentcode") %>'></asp:Label> 
                        <asp:Label id="lblagentname" runat="server" Text='<%# Bind("agentname") %>'></asp:Label> 
            
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="agentcode" SortExpression="agentcode" HeaderText="Code"></asp:BoundField>
            <asp:BoundField DataField="agentname" SortExpression="agentname" HeaderText="Agent Name"></asp:BoundField>
            <asp:TemplateField SortExpression="webuser" HeaderText="Web User"><ItemTemplate>
            <asp:TextBox id="txtwebuser" runat="server" Text='<%# Bind("webusername") %>' CssClass="field_input" Width="100px"></asp:TextBox> 
            </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField SortExpression="shortname" HeaderText="Short Name"><ItemTemplate>
            <asp:TextBox id="txtshortname" runat="server" Text='<%# Bind("shortname") %>' CssClass="field_input" Width="120px"></asp:TextBox> 
            </ItemTemplate>
            </asp:TemplateField>
                         <asp:TemplateField SortExpression="bookingengineratetype" HeaderText="Booking Engine Format"><ItemTemplate>
    
            <asp:DropDownList ID="ddlbookingengformat" runat="server">
             <asp:ListItem >[Select]</asp:ListItem>
                         <asp:ListItem Value="INDIVIDUAL">Individual Rates</asp:ListItem>
                            <asp:ListItem Value="CUMULATIVE">Cumulative Rates</asp:ListItem></asp:DropDownList>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Approve">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemTemplate>
                    <asp:CheckBox ID="chkApprove" runat="server" Width="26px" />
                
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Send Mail">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemTemplate>
                        <asp:CheckBox ID="chkSendMail" runat="server" Width="26px" />
        
            </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Send Mail Contract">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemTemplate>
            <asp:CheckBox ID="chkSendMailcontract" runat="server" Width="26px" />
        
            </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField SortExpression="webemail" HeaderText="Email Id"><ItemTemplate>
            <asp:TextBox id="txtEmailId" runat="server" Text='<%# Bind("webemail") %>' CssClass="field_input" Width="140px"></asp:TextBox> 
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="webcontact" HeaderText="Contact Person"><ItemTemplate>
            <asp:TextBox id="txtContactPerson" runat="server" Text='<%# Bind("webcontact") %>' CssClass="field_input" Width="100px"></asp:TextBox> 
            </ItemTemplate>
            </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Status"  >
                    <ItemTemplate>
                        <asp:Label ID="lblwebapprove" runat="server" Text='<%# Bind("webapprove") %>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle  />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle  HorizontalAlign="Left" />
                </asp:TemplateField> 
          <%--  <asp:BoundField DataField="webapprove" SortExpression="webapprove" HeaderText="Status"></asp:BoundField>--%>
            <asp:TemplateField HeaderText="Remove">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemTemplate>
                    <asp:CheckBox ID="chkRemove" runat="server" Width="26px" />
                
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="webpassword" SortExpression="webpassword"  HeaderText=" Web Password"></asp:BoundField>
            <asp:TemplateField  visible="false"  HeaderText="Web Password"> 
            <ItemTemplate>
            <asp:Label id="lblWPassword" runat="server"    Text='<%# Bind("webpassword") %>'></asp:Label> 
            </ItemTemplate>
                     <ControlStyle  />
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle  HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:BoundField DataField="locked_status" HeaderText="Lock Status"></asp:BoundField>
             <%--             <asp:TemplateField HeaderText="Country Group">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblCgName" runat="server" Text='<%# Bind("countrygroups") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Customer Group">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblCuName" runat="server" Text='<%# Bind("Customergroup") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>--%>
            <asp:BoundField DataField="appuser" HeaderText="User Approved" >
            <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>

            <asp:TemplateField HeaderText="Approved Date">
            <ItemTemplate>
            <asp:Label ID="lblDate" runat="server" Text='<%# FormatDate(DataBinder.Eval (Container.DataItem, "appdate")) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField> 

            </Columns>

            <RowStyle CssClass="grdRowstyle"  ForeColor="Black" Font-Size="10pt"></RowStyle>
            <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="False"></SelectedRowStyle>
            <PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
            <HeaderStyle CssClass="grdheader"  ForeColor="White" Font-Bold="True"></HeaderStyle>
            <AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10pt"></AlternatingRowStyle>
            </asp:GridView>
        
            
                  <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                            Visible="False"></asp:Label></TD></TR>    </DIV>
<tr>
        <td style="height: 22px">
            <asp:Label ID="lblfailmail" runat="server" Text=" " Visible="False" Width="888px"></asp:Label></td>
    </tr>
 
   
        <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label>
       
      
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    </asp:Content>
