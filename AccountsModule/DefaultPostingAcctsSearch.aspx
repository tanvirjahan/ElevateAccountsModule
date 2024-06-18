<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefaultPostingAcctsSearch.aspx.vb" Inherits="DefaultPostingAcctsSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />

    <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript"></script>
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
    
    <script type="text/javascript">
        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");
                var date2 = calendarBehavior2._selectedDate;

                var dp2 = txtfromDate2.value.split("/");
                var newDt2 = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);
                newDt2 = getFormatedDate(newDt2);
                newDt2 = new Date(newDt2);
                calendarBehavior2.set_selectedDate(newDt2);
            }

        }
        function getFormatedDate(chkdate) {
            var dd = chkdate.getDate();
            var mm = chkdate.getMonth() + 1; //January is 0!
            var yyyy = chkdate.getFullYear();
            if (dd < 10) { dd = '0' + dd };
            if (mm < 10) { mm = '0' + mm };
            chkdate = mm + '/' + dd + '/' + yyyy;
            return chkdate;
        }

        function filltodate(fDate) {
            var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");

            if ((txtToDate.value != null) && (txtToDate.value != '')) {

                var dpFrom = txtfromDate.value.split("/");
                var newDt = new Date(dpFrom[2] + "/" + dpFrom[1] + "/" + dpFrom[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                var dpTo = txtToDate.value.split("/");
                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                newDtTo = getFormatedDate(newDtTo);
                newDtTo = new Date(newDtTo);
                if (newDt > newDtTo) {

                    txtToDate.value = txtfromDate.value;
                    DateSelectCalExt();
                    return;
                }
            }
        }

        function ValidateChkInDate() {

            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            var txtToDate = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate.value == null || txtfromDate.value == "") {
                txtToDate.value = "";
                alert("Please select From date.");
            }

            var dp = txtfromDate.value.split("/");
            var newChkInDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = txtToDate.value.split("/");
            var newChkOutDt = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newChkInDt = getFormatedDate(newChkInDt);
            newChkOutDt = getFormatedDate(newChkOutDt);

            newChkInDt = new Date(newChkInDt);
            newChkOutDt = new Date(newChkOutDt);
            if (newChkInDt > newChkOutDt) {
                txtToDate.value = txtfromDate.value;
                alert("Todate date should not be greater than From date");
            }
        }

        $(document).ready(function () {
            var HFshowctry_agent = document.getElementById("<%=HFshowctry_agent.ClientID%>");
            if (HFshowctry_agent.value == "N") {
                visualsearchbox_Nagentctry();
            }
            else {
                visualsearchbox();
            }            
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
            $txtvsprocess.val('"Service ID":" "' + '"Service Name":" "' + '"Country":" "' + '"Agent":" "' + '"Classification":" "' + 'Text:" "');
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
                            case 'Service ID':
                                var asSqlqry = 'select distinct ltrim(rtrim(SNo)) as Serviceid from serviceCat';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                                
                            case 'Service Name':
                                var asSqlqry = 'select distinct ltrim(rtrim(ServiceCategory)) as ServiceName from serviceCat';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                                
                            case 'Country':
                                var asSqlqry = 'select distinct ltrim(rtrim(ctryname)) from ctrymast where active=1';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                                
                            case 'Agent':
                                var asSqlqry = 'select distinct ltrim(rtrim(agentname)) from agentmast where active=1';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                                
                            case 'Classification':
                                var asSqlqry = 'select 0 as Classification union select 1 as classification';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;                                
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'Text' },
                { label: 'SERVICE ID', category: 'DefaultPosting' },
                { label: 'SERVICE NAME', category: 'DefaultPosting' },
                { label: 'COUNTRY', category: 'DefaultPosting' },
                { label: 'AGENT', category: 'DefaultPosting' },
                { label: 'CLASSIFICATION', category: 'DefaultPosting' }
              ]);
                    }
                }
            });
        }

        function visualsearchbox_Nagentctry() {

            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('"Service ID":" "' + '"Service Name":" "' + '"Classification":" "' + 'Text:" "');
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
                            case 'Service ID':
                                var asSqlqry = 'select distinct ltrim(rtrim(SNo)) as Serviceid from serviceCat';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'Service Name':
                                var asSqlqry = 'select distinct ltrim(rtrim(ServiceCategory)) as ServiceName from serviceCat';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;

                            case 'Classification':
                                var asSqlqry = 'select 0 as Classification union select 1 as classification';
                                glcallback = callback;
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'Text' },
                { label: 'SERVICE ID', category: 'DefaultPosting' },
                { label: 'SERVICE NAME', category: 'DefaultPosting' },
                { label: 'CLASSIFICATION', category: 'DefaultPosting' }
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
            var HFshowctry_agent = document.getElementById("<%=HFshowctry_agent.ClientID%>");
            if (HFshowctry_agent.value == "N") {
                visualsearchbox_Nagentctry();
            }
            else {
                visualsearchbox();
            }            
        }
    </script>
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
        .style1
        {
            width: 100%;
        }
        
        .NoData
        {
            font-family: Verdana, Arial, Geneva, ms sans serif;
            font-size: 12px;
            text-align: center;
            vertical-align: middle;
            font-weight: bold;
            height: 70px;
            color: #084573;
        }
        .salesAmt
        {
            
            min-width:100px;
         }
         .plStyle
         {
             min-width:40px;
             }
    </style>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="margin-top: -6px; width: 100%">
                <table style="border: gray 2px solid; width: 100%" class="td_cell" align="left">
                    <tr>
                        <td valign="top" align="center" style="width: 100%;">
                            <asp:Label ID="lblHeading" runat="server" Text="Default Posting Accounts for Invoice" CssClass="field_heading"
                                Width="100%" ForeColor="White" style="padding:2px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; padding: 10px 0px 12px 0px" align="center">
                            <asp:Button ID="btnHelp" TabIndex="1" OnClick="btnhelp_Click" runat="server" Text="Help"
                                Font-Bold="False" CssClass="search_button"></asp:Button>
                            &nbsp;&nbsp;<asp:Button ID="btnAddNew" TabIndex="2" runat="server" Text="Add New"
                                Font-Bold="False" CssClass="btn"></asp:Button>
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" style="display:none" Text="Report" TabIndex="3" />                                    
                            <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px">
                            <div style="width: 100%">
                                <div style="width: 87%; display: inline-block; margin: -6px 4px 0 0;">
                                    <div id="VS" class="container" style="border: 0px;">
                                        <div id="search_box_container">
                                        </div>
                                        <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                        <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                        <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                    </div>
                                </div>
                                <div style="width: 12%; display: inline-block; vertical-align: top;">
                                    <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                                        Text="Reset Search" TabIndex="6" /></div>
                            </div>
                            <asp:DataList ID="dlList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" TabIndex="7">
                                <ItemTemplate>
                                    <table class="styleDatalist" style="border: 0px;">
                                        <tr style="">
                                            <td style="border: 0px; position: relative">
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
                    <tr>
                        <td style="padding-top: 5px; padding-bottom: 5px">
                            <table style="width: 100%; min-width: 850px">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="field_caption" Text="Filter By "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrder" runat="server" TabIndex="7">                                            
                                            <asp:ListItem Value="C">Created Date</asp:ListItem>
                                            <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="field_input" runat="server" onchange="filltodate(this);" Width="75px"
                                            TabIndex="8"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <asp:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                            PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtFromDate">
                                        </asp:MaskedEditExtender><br />
                                        <asp:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </asp:MaskedEditValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" CssClass="fiel_input" runat="server" onchange="ValidateChkInDate();" Width="75px"
                                            TabIndex="9"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        <asp:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                            PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                            TargetControlID="txtToDate">
                                        </asp:MaskedEditExtender><br />
                                        <asp:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                        </asp:MaskedEditValidator>
                                    </td>                                                                      
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn" Text="Search by Date" Font-Bold="False" TabIndex="10" />
                                        <asp:Button ID="btnResetSearch" runat="server" CssClass="btn" Text="Reset Dates" Font-Bold="False" TabIndex="11" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                        <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true" TabIndex="12">
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td style="width:100%">
                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Font-Bold="False" Text="Export To Excel" TabIndex="13" />
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divGrid" style="min-height: 370px; max-height: 370px; max-width:100vw; overflow: auto">
                                <asp:GridView ID="gvDefaultPosting" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    CssClass="grdstyle" Font-Bold="true" Font-Size="12px" GridLines="Vertical" ShowHeaderWhenEmpty="true"
                                    Width="100%" AllowPaging="true" AllowSorting="true">                                    
                                    <Columns>                                        
                                        <asp:TemplateField HeaderText="Posting Id" SortExpression="PostingId">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPostingId" runat="server" Text='<%# Bind("postingId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>                                        
                                        
                                        <asp:BoundField DataField="ApplicableTo" HeaderText="Applicable To" SortExpression="ApplicableTo" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="countriesExceptAgents" HeaderText="Countries Except Agents" SortExpression="countriesExceptAgents" ItemStyle-HorizontalAlign="Left" 
                                        ItemStyle-Wrap="true"></asp:BoundField>                                                                                                                      
                                       
                                       <asp:BoundField DataField="Agents" HeaderText="Agents" SortExpression="Agents" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="true"></asp:BoundField>                                       
                                                                                
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate"
                                        SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                        
                                        <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                        </asp:BoundField>      
                                        
                                        <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate"
                                        SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>

                                        <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" Text="Edit" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="EditPosting" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnView" Text="View" CssClass="field_input" CommandArgument='<%# Container.DisplayIndex %>'
                                                    CommandName="View" ForeColor="Blue" runat="server"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" />                                            
                                        </asp:TemplateField>                                            
                                    </Columns>                                    
                                    <HeaderStyle CssClass="grdheader" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" BorderColor="LightGray"  />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <RowStyle CssClass="grdRowstyle" Wrap="true" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" Wrap="true" />
                                    <FooterStyle CssClass="grdfooter" />
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>                                                                
                                </asp:GridView>
                                <asp:Label ID="lblMsg" runat="server" Text="Records not found, Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                            Visible="False"></asp:Label> 
                            <asp:HiddenField ID="HFshowctry_agent" runat="server" Value="N" />                                                                                                                              
                            </div> 
                        </td>
                    </tr>
                </table>
            </div>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
            </Triggers>
            </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>

</asp:Content>

