<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false"  CodeFile="SupHotelAmenitiesSearch.aspx.vb" Inherits="SupHotelAmenitiesSearch"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
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
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript" charset="utf-8">

        function DateSelectCalExt() {
            var txtfromDate = document.getElementById("<%=txtfromDate.ClientID%>");
            if (txtfromDate.value != '') {
                var calendarBehavior1 = $find("<%=dpFromDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
                var date = calendarBehavior1._selectedDate;

                var dp = txtfromDate.value.split("/");
                var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
                newDt = getFormatedDate(newDt);
                newDt = new Date(newDt);
                calendarBehavior1.set_selectedDate(newDt);
            }
            var txtfromDate2 = document.getElementById("<%=txtToDate.ClientID%>");
            if (txtfromDate2.value != '') {
                var calendarBehavior2 = $find("<%=dpToDate.ClientID %>");  // document.getElementById("<%=dpFromDate.ClientID%>"); 
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

                var dpTo = txtToDate.value.split("/");

                var newDtTo = new Date(dpTo[2] + "/" + dpTo[1] + "/" + dpTo[0]);
                var today = new Date();

                newDtTo = getFormatedDate(newDtTo);
                today = getFormatedDate(today);
                newDtTo = new Date(newDtTo);
                today = new Date(today);

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
            $txtvsprocess.val('AMENITYNAME:" " AMENITYTYPE:" " TEXT:" "');
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
                            case 'AMENITYNAME':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfAmenityNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'AMENITYTYPE':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsSectorServices.GetListOfAmenityTypeNameVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                { label: 'TEXT', category: 'AMENITIES' },
                { label: 'AMENITYTYPE', category: 'AMENITIES' },
                { label: 'AMENITYNAME', category: 'AMENITIES' },
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
    <style>
        .btnExample
        {
            color: #2D7C8A;
            background: #e7e7e7;
            font-weight: bold;
            border: 1px solid #2D7C8A;
            padding: 5px 5px;
        }
        
        .btnExample:hover
        {
            color: #FFF;
            background: #2D7C8A;
        }
        .autocomplete_completionListElement
        {
            visibility: hidden;
            margin: 1px 0px 0px 0px !important;
            background-color: #FFFFFF;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: 200px;
            width: 100px;
            text-align: left;
            list-style-type: none;
            font-family: Verdana;
            font-size: small;
        }
        
        
        /* AutoComplete highlighted item */
        
        
        .autocomplete_highlightedListItem
        {
            background-color: Silver;
            color: black;
            margin-left: -35px;
            font-weight: bold;
        }
        
        
        /* AutoComplete item */
        
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            margin-left: -35px;
        }
    </style>
   
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%">
                <table>
                    <tr>
                        <td align="center" class="field_heading">
                            Amenities &nbsp;List
                        </td>
                    </tr>
                    <tr>
                        <td style="color: blue;" align="center" class="td_cell">
                            Type few characters of code or name and click search &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="width: 741px">
                                        <tbody>
                                            <tr>
                                                <td align="center" colspan="6">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnHelp" TabIndex="3" OnClick="btnhelp_Click"
                                                        runat="server" Text="Help" Font-Bold="False" CssClass="search_button"></asp:Button>
                                                    &nbsp;&nbsp;<asp:Button ID="btnAddNew" TabIndex="4" runat="server" Text="Add New"
                                                        Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnPrint"
                                                            TabIndex="5" runat="server" Text="Report" CssClass="btn"></asp:Button>
                                                </td>
                                            </tr>
                                            
                                           <%-- *** Following 3 Rows are not used in this form fo hided--%>
                                            <%--<tr style="display: none">
                                                <td style="height: 13px; text-align: center" class="td_cell" colspan="4">
                                                    <asp:Button ID="btnSearch" TabIndex="5" runat="server" Text="Search" Font-Bold="False"
                                                        Width="50px" CssClass="search_button"></asp:Button>
                                                    &nbsp;
                                                    <asp:Button ID="btnClear" TabIndex="6" runat="server" Text="Clear" Font-Bold="False"
                                                        Width="39px" CssClass="search_button"></asp:Button>&nbsp;
                                                    <asp:Button ID="btnHelp1" TabIndex="10" OnClick="btnHelp_Click" runat="server" Text="Help"
                                                        __designer:dtid="1688858450198528" CssClass="search_button" __designer:wfdid="w34">
                                                    </asp:Button>&nbsp;
                                                    <asp:Button ID="btnAddNew1" TabIndex="7" runat="server" Text="Add New" Font-Bold="False"
                                                        __designer:dtid="5348024557502480" Width="68px" CssClass="btn" __designer:wfdid="w5">
                                                    </asp:Button>&nbsp;
                                                    <asp:Button ID="btnPrinCt" TabIndex="9" runat="server" Text="Report" __designer:dtid="5348024557502482"
                                                        CssClass="btn" __designer:wfdid="w6"></asp:Button>
                                                </td>
                                                <td style="width: 322px; height: 13px; text-align: center" class="td_cell" colspan="1">
                                                </td>
                                                <td style="width: 322px; height: 13px; text-align: center" class="td_cell" colspan="1">
                                                </td>
                                            </tr>--%>
                                            <%--<tr style="display: none"> 
                                                <td style="width: 55px; height: 4px" class="td_cell">
                                                    <span style="color: black">Amenity&nbsp;Code</span>
                                                </td>
                                                <td style="width: 46px; height: 4px">
                                                    <asp:TextBox ID="txtCode" TabIndex="1" runat="server" Width="178px" CssClass="txtbox"
                                                        __designer:wfdid="w147" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td style="width: 58px; height: 4px" class="td_cell">
                                                    <span style="color: black">&nbsp;Amenity&nbsp;Name</span>
                                                </td>
                                                <td style="width: 40px; height: 4px" class="td_cell">
                                                    <asp:AutoCompleteExtender ID="txtName_AutoCompleteExtender" CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" runat="server"
                                                        FirstRowSelected="True" DelimiterCharacters="" Enabled="True" ServiceMethod="GetMeals"
                                                        MinimumPrefixLength="1" TargetControlID="txtName">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txtName" TabIndex="2" runat="server" Width="348px" CssClass="field_input"
                                                        __designer:wfdid="w148" MaxLength="100" AutoPostBack="True"></asp:TextBox>
                                                </td>
                                                <td style="width: 42px; height: 4px" class="td_cell">
                                                    <asp:Label ID="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"
                                                        __designer:wfdid="w1"></asp:Label>
                                                </td>
                                                <td style="width: 42px; height: 4px" class="td_cell">
                                                    <asp:DropDownList ID="ddlOrderBy" runat="server" Width="130px" CssClass="drpdown"
                                                        AutoPostBack="True" __designer:wfdid="w19" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>--%>
                                            <%--<tr style="display: none">
                                                <td style="width: 55px; height: 4px; display: none" class="td_cell">
                                                    <asp:Label ID="lblSupTypeCode" runat="server" Text="Supplier Type Code" Height="19px"
                                                        Width="110px" CssClass="field_caption" __designer:wfdid="w1" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 46px; height: 4px; display: none">
                                                    <select onchange="GetValueFrom()" style="width: 185px; display: none" id="ddlSSPTypeCode"
                                                        class="drpdown" tabindex="3" runat="server" visible="false">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 58px; height: 4px; display: none" class="td_cell">
                                                    <asp:Label ID="lblSupTypeName" runat="server" Text="Supplier Type Name" Height="19px"
                                                        Width="110px" CssClass="field_caption" __designer:wfdid="w2" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 40px; height: 4px" class="td_cell">
                                                    <select onchange="GetValueCode()" style="width: 354px; display: none" id="ddlSSPTypeName"
                                                        class="drpdown" tabindex="4" runat="server" visible="false">
                                                        <option selected></option>
                                                    </select>
                                                </td>
                                                <td style="width: 42px; height: 4px" class="td_cell">
                                                </td>
                                                <td style="width: 42px; height: 4px" class="td_cell">
                                                </td>
                                            </tr>--%>
                                        </tbody>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
        </tr>
        <tr>
            <td colspan="6">
                <div style="width: 100%">
                    <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
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
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="3" />
                            <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                TargetControlID="txtFromDate">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" Width="75px"></asp:TextBox>
                            <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="3" />
                            <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                PopupButtonID="ImgBtnToDt" PopupPosition="Right" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                TargetControlID="txtToDate">
                            </cc1:MaskedEditExtender>
                            <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                Text="Search by Date" />
                            &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False"
                                TabIndex="4" Text="Reset Dates" />
                        </td>
                        <td>
                            <asp:Label ID="RowSelectsmps" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                            <asp:DropDownList ID="RowsPerPageMPS" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                            </asp:DropDownList>
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
            <td style="width: 100%">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel"
                    TabIndex="8" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_SearchResult" TabIndex="8" runat="server" Font-Size="10px" Width="100%"
                            CssClass="td_cell" __designer:wfdid="w153" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                            GridLines="Vertical">
                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderText="Amenitycode">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("iCode") %>' ID="TextBox1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCode" runat="server" Text='<%# Bind("iCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="iCode" SortExpression="iCode" HeaderText="Code"></asp:BoundField>
                                <asp:TemplateField HeaderText="Name" SortExpression="iName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbliName" runat="server" Text='<%# Bind("iName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Width="8%" />
                                    <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                    <ItemStyle CssClass="iName" HorizontalAlign="Left" Width="8%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="AmenityTypecode" SortExpression="TB_HotelAmenitiesMaster.AmenityTypecode" HeaderText="Amenity Type"></asp:BoundField>
                                <%--<asp:BoundField DataField="sptypename" SortExpression="sptypename" HeaderText="Supplier Type Name"></asp:BoundField>--%>
                                <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
                                <asp:BoundField DataField="rankorder" Visible="false" HeaderText="Rank Order" />
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="adddate" HeaderText="Date Created"></asp:BoundField>
                                <asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
                                <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} "
                                    DataField="moddate" HeaderText="Date Modified"></asp:BoundField>
                                <asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
                                <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                                <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                    <ItemStyle ForeColor="Blue"></ItemStyle>
                                </asp:ButtonField>
                            </Columns>
                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                        </asp:GridView>
                        <asp:Label ID="lblMsg" runat="server" Text="Records not found. Please redefine search criteria"
                            Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px"
                            __designer:wfdid="w154" Visible="False"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="../clsSectorServices.asmx" />
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
