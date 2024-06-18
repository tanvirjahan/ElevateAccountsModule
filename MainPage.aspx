<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="MainPage.aspx.vb" Inherits="Default3"  MasterPageFile="MainPageMaster.master" Strict="true" %>
<%@ OutputCache location="none" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

   

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server" >
    <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1">
    <link href="css/Styles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="Content/lib/css/reset.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="Content/lib/css/icons.css" type="text/css" media="screen"
        charset="utf-8">
    <link rel="stylesheet" href="Content/lib/css/workspace.css" type="text/css" media="screen"
        charset="utf-8">
    <script src="Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/jquery.ui.autocomplete.js" type="text/javascript"
        charset="utf-8"></script>
      

    <script src="Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/utils/backbone_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/utils/jquery_extensions.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="Content/lib/js/utils/search_parser.js" type="text/javascript" charset="utf-8"></script>
    <script src="Content/lib/js/utils/inflector.js" type="text/javascript" charset="utf-8"></script>
        <script src="Content/lib/js/templates/templates.js" type="text/javascript" charset="utf-8"></script>
    <style type="text/css">
          .dragbar
        {
            background-color: #E5E5E5;
            height: 100%;
            float: right;
            width: 2px;
            cursor: col-resize;
        }
        td.lockedAlternative, th.lockedAlternative
        {
        /*     */
        background-color: #FFFFBF;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFFFBF;
            background: #FFFFBF;
            overflow: hidden;
      /*   padding-right: 50px;*/
        }
        td.lockedAlternativeAssDate, th.lockedAlternativeAssDate
        {
            background-color: #FFFFBF;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFFFBF;
            background: #FFFFBF;
            overflow: hidden;
            /*      padding-right:10px;*/
        }
        td.locked, th.locked
        {
          /*  */
          background-color: #FFD7F3;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFD7F3;
            overflow: hidden;
          /*  padding-right: 50px;*/
        }
        
         td.lockedAssDate, th.lockedAssDate
        {
            background-color: #FFD7F3;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFD7F3;
            overflow: hidden;
                       /*     padding-right: 10px; */
        }  
        td.lockedAlternativeLast, th.lockedAlternativeLast
        {
            background-color: #FFFFBF;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFFFBF;
            background: #FFFFBF;
        }
        td.lockedLast, th.lockedLast
        {
            background-color: #FFD7F3;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFD7F3;
        }
        td.lockedAlternativeNext, th.lockedAlternativeNext
        {
            background-color: #DDD9CF;
            position: static;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #DDD9CF;
        }
        td.lockedNext, th.lockedNext
        {
            background-color: #FFFFFF;
            position: static;
            display: block;
            padding-top: 5px;
            padding-bottom: 5px;
            border-right-color: #FFFFFF;
        }
        td.lockedHeader, th.lockedHeader
        {
            background-color: #06788B;
            position: absolute;
            display: block;
            border-right-color: #06788B;
            overflow: hidden;

        }
       td.lockedHeaderAssDate, th.lockedHeaderAssDate
        {
            background-color: #06788B;
            position: absolute;
            display: block;
            border-right-color: #06788B;
            overflow: hidden;
                        /*    padding-right: 10px; */
        }   
        td.lockedHeaderLast, th.lockedHeaderLast
        {
            background-color: #06788B;
            position: absolute;
            display: block;
            border-right-color: #06788B;
        }
        td.lockedHeaderNext, th.lockedHeaderNext
        {
            background-color: #06788B;
            position: absolute;
            display: block;
            border-right-color: #06788B;
            overflow: hidden;
            padding-right: 50px;
        }
        
           
        .modalBackground
        {
            background-color: Black;
            opacity: 0.3;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 5px;
            border-style: solid;
            border-color: #38756C;
            padding-top: 10px;
            padding-left: 10px;
            min-width: 500px;
            max-width: 900px;
            min-height: 200px;
            max-height: 800px;
            color: #000;
        }
          .modalPopupNew
        {
            background-color: #FFFFFF;
            border-width: 5px;
            border-style: solid;
            border-color: #38756C;
            padding-top: 10px;
            padding-left: 10px;
            min-width: 1000px;
            max-width: 1000px;
            min-height: 200px;
            max-height: 800px;
            color: #000;
        }
        .modalPopupagents_countries
        {
            background-color: #FFFFFF;
            border-width: 5px;
            border-style: solid;
            border-color: #38756C;
            padding-top: 10px;
            padding-left: 10px;
            min-width: 200px;
            max-width: 300px;
            min-height: 200px;
            max-height: 400px;
            color: #000;
        }
           .modalPopupClarify
        {
            background-color: #FFFFFF;
            border-width: 5px;
            border-style: solid;
            border-color: #38756C;
            padding-top: 10px;
            padding-left: 10px;
         
            color: #000;
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
        <script language="javascript" type="text/javascript">
            var i = 0;
            var dragging = false;
            $(document).ready(function () {
                visualsearchbox();
                visualsearchbox_pending();
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
                //$txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"TRACKING STATUS":" "' + '"FROM EMAIL":" " TEXT:" "');
                $txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"FROM EMAIL":" " TEXT:" "');


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
                            $txtvsprocess.val(visualSearch.searchQuery.serialize().replace('<', '___').replace('>', '...'));
                            var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                            $txtvsprocesssplit.val(visualSearch.searchQuery.serializetosplit().replace('<', '___').replace('>', '...'));

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
                                case 'HOTELS':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'EMAIL CODE':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayTicketNoVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'EMAIL DATE':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayEmailDateVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'HOTEL STATUS':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayHotelStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'EMAIL SUBJECT':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayEmailSubjectVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'TRACKING STATUS':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayTrackingStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'FROM EMAIL':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayFromEmailIdVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'UPDATE TYPE':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayUpdateTypeVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'ASSIGNED TO':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayAssinedToVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'PROGRESS STAGE':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    ColServices.clsVisualSearchService.GetListOfArrayProgressStageVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                            }
                        },
                        facetMatches: function (callback) {
                            callback([
                    { label: 'EMAIL CODE', category: 'TRACKING' },
                    { label: 'EMAIL DATE', category: 'TRACKING' },
                    { label: 'HOTELS', category: 'TRACKING' },
                    { label: 'HOTEL STATUS', category: 'TRACKING' },
                    { label: 'FROM SUBJECT', category: 'TRACKING' },
                            //   { label: 'TRACKING STATUS', category: 'TRACKING' },
                    {label: 'FROM EMAIL', category: 'TRACKING' },
                    { label: 'TEXT', category: 'TRACKING' },
                    { label: 'UPDATE TYPE', category: 'TRACKING' },
                    { label: 'ASSIGNED TO', category: 'TRACKING' },
                    { label: 'PROGRESS STAGE', category: 'TRACKING' },
              ]);
                        }
                    }
                });
            }





            function visualsearchbox_pending() {

                var $txtvsprocess_pending = $(document).find('.cs_txtvsprocess_pending');
                //$txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"TRACKING STATUS":" "' + '"FROM EMAIL":" " TEXT:" "');

                $txtvsprocess_pending.val('Country:" " City:" " Sector:" " Category:" "Hotel:"  " HotelChain:"  " Text:" "');




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
                            switch (category) {

                                case 'Country':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    //fnTestCallback();
                                    ColServices.clsHotelGroupServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'City':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    //fnTestCallback();
                                    ColServices.clsHotelGroupServices.GetListOfArrayCityVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'Sector':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    //fnTestCallback();
                                    ColServices.clsHotelGroupServices.GetListOfArraySectorVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;
                                case 'Category':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    //fnTestCallback();
                                    ColServices.clsHotelGroupServices.GetListOfArrayCategoryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;

                                case 'HotelChain':
                                    var asSqlqry = '';
                                    glcallback = callback;
                                    //fnTestCallback();
                                    ColServices.clsHotelGroupServices.GetListOfArrayHotelChainVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;


                                case 'Hotel':

                                    var asSqlqry = '';
                                    glcallback = callback;

                                    ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                    break;


                            }
                        },
                        facetMatches: function (callback) {
                            callback([
                { label: 'Text' },
                { label: 'Country', category: 'Hotels' },
                { label: 'City', category: 'Hotels' },
                { label: 'Sector', category: 'Hotels' },
                { label: 'Category', category: 'Hotels' },

                 { label: 'HotelChain', category: 'Hotels' },

                     { label: 'Hotel', category: 'Hotels' },
              ]);
                        }
                    }
                });



                //                   
            }





            function fnFillSearchVS(result) {
                glcallback(result, {
                    preserveOrder: true // Otherwise the selected value is brought to the top
                });
            }


    </script>

     

     <script language="javascript" type="text/javascript">
         var prm = Sys.WebForms.PageRequestManager.getInstance();
         prm.add_initializeRequest(InitializeRequest);
         prm.add_endRequest(EndRequest);

         function InitializeRequest(sender, args) {

         }
         function EndRequest(sender, args) {
             // after update occur on UpdatePanel re-init the Autocomplete
             visualsearchbox();
             visualsearchbox_pending();

         }
         </script>

          <asp:UpdatePanel id="UpdatePanel12" runat="server">
        <ContentTemplate>
    <div id="dvContractdashBoard" runat="server" style="margin: 15px 5px 0px 5px; padding-bottom: 15px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td colspan="2" class="field_heading" align="center">
                            <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text=""> </asp:Label>
                        </td>
                    </tr>
                    <tr><td>
                     <div id="Div_pendingsearchbox" runat="server"   style="margin: 15px 5px 0px 5px;   padding-bottom: 15px;">
                                                            <div id="Div3" class="container" style="border: 0px;">
                                                                <div id="search_box_container_pending">
                                                                </div>
                                                                <asp:TextBox ID="txtvsprocess_pending" runat="server" class="cs_txtvsprocess_pending"
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtvsprocesssplit_pending" runat="server" class="cs_txtvsprocesssplit_pending"
                                                                    Style="display: none"></asp:TextBox>
                                                                <asp:Button ID="btnvsprocess_pending" Style="display: none" runat="server" />
                                                            </div>
                                                            <asp:DataList ID="dlInboxSearch_pending" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                                <ItemTemplate>
                                                                    <table class="style1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                                <asp:Button ID="lbInboxCategorypending" class="button button4" runat="server" Style="font-family: Verdana;
                                                                                    font-size: 12px;" Text='<%# Eval("Value") %>'  />
                                                                                <asp:Button ID="lbCloseCategorypending" class="buttonClose button4" runat="server"
                                                                                    Style="font-family: Verdana; font-size: 12px;" OnClick="lbCloseCategorypending_Click"
                                                                                    Text="X" />
                                                                               
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                                </div>
                                            
                    </td></tr>
                    <tr>
                        <td colspan="2">
                            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                                <cc1:TabPanel HeaderText=" Tracking" ID="TabPanel1" Font-Size="Medium" runat="server">
                                    <ContentTemplate>
                                        <div style="width: 90.5%; display: inline-block; margin: 6px 4px 0 0;">
                                            <div id="VS" class="container" style="border: 0px;">
                                                <div id="search_box_container">
                                                </div>
                                                <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                            </div>
                                            <asp:DataList ID="dlInboxSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                <asp:Button ID="lbInboxCategory" class="button button4" runat="server" Style="font-family: Verdana;
                                                                    font-size: 12px;" Text='<%# Eval("Value") %>' OnClick="lbInboxCategory_Click" />
                                                                <asp:Button ID="lbCloseCategory" class="buttonClose button4" runat="server" Style="font-family: Verdana;
                                                                    font-size: 12px;" OnClick="lbCloseCategory_Click" Text="X" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                        <div id="Div2" runat="server" style="width: 92%; overflow: auto; margin: 15px 5px 0px 5px;
                                            padding-bottom: 15px;">
                                            <asp:GridView ID="gvTracking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="200%" AllowPaging="True">
                                                <FooterStyle CssClass="grdfooter" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Mail Link">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbMailLink" Width="60px" runat="server" OnClick="lbMailLink_Click">Mail Link</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mail Link">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbMailLink1" runat="server" Width="60px" OnClick="lbMailLink_Click">Mail Link</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Start Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartDate" runat="server" Width="160px" Text='<%# Eval("TaskStartDate") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbStart" runat="server" Width="160px" OnClick="lbStart_Click">Start</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Start Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartDate1" runat="server" Width="160px" Text='<%# Eval("TaskStartDate") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbStart1" runat="server" Width="160px" OnClick="lbStart_Click">Start</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedDate" Width="120px" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedDate1" Width="120px" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdate" runat="server" Width="50px" OnClick="lbUpdate_Click">Update</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdate1" runat="server" Width="50px" OnClick="lbUpdate_Click">Update</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clarify">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbClarify" runat="server" Width="50px" OnClick="lbClarify_Click">Clarify</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Clarify">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbClarify1" runat="server" Width="50px" OnClick="lbClarify_Click">Clarify</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Complete Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComplete" runat="server" Width="185px" Text='<%# Eval("TaskCompleteDate") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbComplete" runat="server" Width="185px" OnClick="lbComplete_Click">Complete</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="185px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Complete Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComplete1" runat="server" Width="185px" Text='<%# Eval("TaskCompleteDate") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbComplete1" runat="server" Width="185px" OnClick="lbComplete_Click">Complete</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="185px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHotelStatus" Width="85px" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHotelStatus1" Width="85px" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailCode" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdEmailId" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailDate" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailTime" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCHotelName" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdCHotelCode" runat="server" Value='<%# Eval("HotelCode") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Email Subject" DataField="EmailSubject">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Progress Stage">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgressStage" runat="server" Text='<%# Eval("ProgressStage") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpdateTypeName" runat="server" Text='<%# Eval("UpdateTypeName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid From">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValidFrom" runat="server" Text='<%# Eval("ValidFrom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid To">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValidTo" runat="server" Text='<%# Eval("ValidTo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False" HeaderText="AssignedTo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedTo" runat="server" Text='<%# Bind("AssignedTo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" ForeColor="White" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </div>
                                        <div>
                                            <asp:GridView ID="gvApprovalTracking" runat="server" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" TabIndex="12"
                                                Width="200%" AllowPaging="True">
                                                <FooterStyle CssClass="grdfooter" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Mail Link">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbMailLink" Width="60px" runat="server" OnClick="lbMailLink_Click">Mail Link</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mail Link">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbMailLink1" runat="server" Width="60px" OnClick="lbMailLink_Click">Mail Link</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Start Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalStartDate" runat="server" Width="160px" Text='<%# Eval("ApprovalStart") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbApprovalStart" runat="server" Width="160px" OnClick="lbApprovalStart_Click">Start</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Start Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalStartDate1" runat="server" Width="160px" Text='<%# Eval("ApprovalStart") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbApprovalStart1" runat="server" Width="160px" OnClick="lbApprovalStart_Click">Start</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approver Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalAssignedDate" Width="120px" runat="server" Text='<%# Eval("ApprovalAssignmentDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approver Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalAssignedDate1" Width="120px" runat="server" Text='<%# Eval("ApprovalAssignmentDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdate" runat="server" Width="50px" OnClick="lbUpdate_Click">Update</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdate1" runat="server" Width="50px" OnClick="lbUpdate_Click">Update</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Clarify">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbClarify" runat="server" Width="50px" OnClick="lbApprovalClarify_Click">Clarify</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Clarify">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbClarify1" runat="server" Width="50px" OnClick="lbApprovalClarify_Click">Clarify</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Complete Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalComplete" runat="server" Width="185px" Text='<%# Eval("ApprovalEnd") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbApprovalComplete" runat="server" Width="185px" OnClick="lbApprovalComplete_Click">Complete</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="185px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Complete Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalComplete1" runat="server" Width="185px" Text='<%# Eval("ApprovalEnd") %>'></asp:Label>
                                                            <asp:LinkButton ID="lbApprovalComplete1" runat="server" Width="185px" OnClick="lbApprovalComplete_Click">Complete</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="185px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHotelStatus" Width="85px" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHotelStatus1" Width="85px" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailCode" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdEmailId" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailDate" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailTime" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCHotelName" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdCHotelCode" runat="server" Value='<%# Eval("HotelCode") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Email Subject" DataField="EmailSubject">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Progress Stage">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgressStage" runat="server" Text='<%# Eval("ProgressStage") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpdateTypeName" runat="server" Text='<%# Eval("UpdateTypeName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid From">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValidFrom" runat="server" Text='<%# Eval("ValidFrom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid To">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValidTo" runat="server" Text='<%# Eval("ValidTo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedDate11" Width="120px" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Start Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartDate11" runat="server" Width="160px" Text='<%# Eval("TaskStartDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="160px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task Complete Date &amp; Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComplete11" runat="server" Width="185px" Text='<%# Eval("TaskCompleteDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="185px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False" HeaderText="AssignedTo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedTo" runat="server" Text='<%# Bind("AssignedTo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="False" HeaderText="Approver">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("Approver") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" ForeColor="White" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel HeaderText="Pending Contracts/Offers for Approval" ID="TabPanel3" Font-Size="Medium"
                                    runat="server">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                            <table>
                                            
                                            <tr><td>
                                <%--</div>--%> <%--Commented by Shabreena so that full screen was coming on 17/05/2018--%>      
                           </td></tr>
                                            <tr><td>   <div>
                                                    <asp:Label ID="RowSelectcos" runat="server" CssClass="field_caption" Text="Rows Selected "></asp:Label>
                                                    <asp:DropDownList ID="RowsPerPageCUS" runat="server" AutoPostBack="true">
                                                        <asp:ListItem Value="5">5</asp:ListItem>
                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                </td></tr>
                                                <tr><td>
                                                
                                               
                                             
                                                       
                                                   
                                                <asp:GridView ID="gv_pendingcontracts" TabIndex="9" runat="server" Font-Size="10px"
                                                    Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                    BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
                                                    <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ID">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkcontoffID" runat="server" Text='<%# Bind("contractid") %>'
                                                                    CommandName="Contract"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="" Visible="FALSE">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartycode" runat="server" Text='<%# Bind("Partycode") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hotel Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("partyname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Offer Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOfferName" runat="server" Text='<%# Bind("promotionname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Applicable To">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkapp" runat="server" Text='<%#bind("applicableto") %>' OnClick="lnkapp_Click"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Min From Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfromdate" runat="server" Text='<%# Bind("fromdate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Max To Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbltodate" runat="server" Text='<%# Bind("todate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ActiveState">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblactivestate" runat="server" Text='<%# Bind("activestate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Print" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkexcel" runat="server" Text="view excel" OnClick="lnkexcel_Click"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                    <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                    <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                </asp:GridView>
                                                <asp:HiddenField ID="btnDisplayAGents_Countries" runat="server" Value="Show Modal Popup" />
                                                <cc1:ModalPopupExtender ID="DisplayAGents_Countries" runat="server" PopupControlID="pnlDisplayAGents_Countries"
                                                    TargetControlID="btnDisplayAGents_Countries" EnableViewState="true" CancelControlID="btnPopupShowCountryClose"
                                                    BackgroundCssClass="modalBackground">
                                                </cc1:ModalPopupExtender>
                                                <asp:Panel runat="server" ID="pnlDisplayAGents_Countries" Style="display: none; overflow: scroll;
                                                    height: 500px; width: 700px; z-index: -100;" BorderStyle="Double" BorderWidth="6px"
                                                    BackColor="white">
                                                    <div style="margin: 5%">
                                                        
                                                        <div>
                                                           

 

  <asp:GridView ID="gvDisplayCountries" runat="server" Font-Size="13px" Width="600px"
                                                                CellPadding="3" BorderWidth="1px" AutoGenerateColumns="False" AllowPaging="false"
                                                                AlternatingRowStyle-BackColor="lightGray">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Country Code">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <div align="center">
                                                                                    <asp:Label ID="lblctrycode" runat="server" Text='<%# Bind("ctrycode") %>'></asp:Label>
                                                                                </div>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="200px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Country Name">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <div align="center">
                                                                                    <asp:Label ID="lblcountryname" runat="server" Text='<%# Bind("ctryname") %>'></asp:Label>
                                                                                </div>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="300px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>



                                                        </div>
                                                        <br>
                                                        </br>
                                                       
                                                        <div>
                                                            <asp:GridView ID="gvDisplayAgents" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="#099999" CssClass="td_cell" Width="100%" 
                                            TabIndex="10">
                                            <Columns>
                                                <asp:BoundField DataField="ctrycodename" HeaderText="Country" 
                                                    SortExpression="ctrycodename">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>

                                               
                                              
                                                <asp:BoundField DataField="agentname" HeaderText="Agent Name" 
                                                    SortExpression="agentname">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px" />
                                                </asp:BoundField>
                                               
                                            </Columns>
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#454580" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Transparent" Font-Size="12px" />
                                        </asp:GridView>
                                                        </div>
                                                        <br />
                                                        <div style="align: center">
                                                            <center>
                                                                <asp:Button ID="btnPopupShowCountryClose" runat="server" Text="Close" />
                                                            </center>
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                                 
                                                </td></tr>
                                            </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdPopupStatus" runat="server" />
            <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
            <asp:HiddenField ID="hdTrackpopupStatus" runat="server" />
            <asp:Button ID="btnTrackPopup" Style="display: none" runat="server" Text="Show Modal Popup" />
            <cc1:ModalPopupExtender ID="meContractTracking" runat="server" PopupControlID="pnlcontractTrack"
                TargetControlID="btnTrackPopup" EnableViewState="true" CancelControlID="lbCloseTrack"
                BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
            <%-- style = "display:none"--%>
            <!-- ModalPopupExtender -->
            <asp:Panel ID="pnlcontractTrack" runat="server" CssClass="modalPopupNew" Style="display: none;"
                align="center">
                <div style="width: 100%; vertical-align: middle; min-height: 200px; max-height: 550px;
                    margin-bottom: 20px; overflow: auto;">
                    <div style="width: 30px; float: right;">
                        <asp:ImageButton ID="lbCloseTrack" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                            Width="25px" />
                    </div>
                    <div style="width: 100%; float: left;">
                        <div style="width: 20%; float: left;">
                            <div>
                                <asp:Label ID="Label15" runat="server" Font-Names="Arial" Font-Bold="true" Text="Additional Emails"></asp:Label>
                            </div>
                            <div>
                                <asp:DataList ID="dlAdditionalEmails" runat="server" RepeatColumns="1">
                                    <ItemTemplate>
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lbAdditionalEmails" runat="server" Font-Names="Arial" Font-Size="Small"
                                                        OnClick="lbAdditionalEmails_Click" Text='<%# Eval("AdditionalEmailIdName") %>'></asp:LinkButton>
                                                    <asp:HiddenField ID="hdAdditionalEmails" runat="server" Value='<%# Eval("AdditionalEmailId") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </div>
                        <div style="width: 79%; float: left; border-left: 2px groove #E5E5E5;">
                            <table class="style1">
                                <tr>
                                    <td width="5%">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubjectPopup" runat="server" Font-Names="Arial" Font-Size="Large"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5%">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDatePopup" runat="server" Font-Names="Arial" Font-Size="Small"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5%">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFromPopup" runat="server" Font-Names="Arial" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5%">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:DataList ID="DLAttachmentPopup" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="imgAttachmentType" runat="server" ImageUrl="~/Images/crystaltoolbar/mail-attachment2.png"
                                                                Width="25px" />
                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("content") %>' Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lbAttachmentPopup" runat="server" Font-Names="Arial" Font-Size="Small"
                                                                Font-Underline="False" ForeColor="#333333" OnClick="lbAttachmentPopup_Click"
                                                                Text='<%# Eval("FileName") %>'></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="5%">
                                        &nbsp;
                                    </td>
                                    <td align="left">
                                        <div style="overflow: auto; width: 100%;">
                                            <asp:Label ID="lblBodyPopup" runat="server" Font-Names="Arial"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <br />
            </asp:Panel>
            <asp:Button ID="btnClarifyPopup" Style="display: none" runat="server" Text="Show Modal Popup" />
            <cc1:ModalPopupExtender ID="meClarify" runat="server" PopupControlID="pnlClarify"
                TargetControlID="btnClarifyPopup" EnableViewState="true" CancelControlID="lbClarifyClose"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlClarify" CssClass="modalPopupClarify" Style="display: none;" align="center"
                runat="server">
                <div style="width: 740px; vertical-align: middle; min-height: 100px; max-height: 350px;
                    margin-bottom: 20px; overflow: auto;">
                    <div>
                        <table style="width: 725px;">
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
                                <td align="right">
                                    <asp:ImageButton ID="lbClarifyClose" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                                        Width="25px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Image ID="imgMail1" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                                    <asp:Label ID="lblEmailCodePopup" runat="server" Font-Bold="True" Font-Names="Calibri, Arial,Monaco, Consolas, monospace"
                                        Font-Size="14px" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblHname" runat="server" Font-Size="Small" Style="font-family: Verdana"
                                        Text="Hotel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHotelNamePopup" runat="server" Font-Bold="True" Font-Names="Calibri"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblAssDate" runat="server" Font-Size="Small" Style="font-family: Verdana"
                                        Text="Assigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssignDate" runat="server" Font-Bold="True" Font-Names="Calibri,Arial,Monaco,Consolas,monospace"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblClarifyRemark" Style="font-family: Verdana" runat="server" Text="Clarify Remarks"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtClarifyRemark" TextMode="MultiLine" runat="server" Height="50px"
                                        Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdPopupHotelCode" runat="server" />
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdPopupMailId" runat="server" />
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
                                    <asp:HiddenField ID="hdAssignedUser" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="brnClarifySubmit" CssClass="btn" runat="server" Text="Submit" />
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
                                <td colspan="3">
                                    <asp:GridView ID="gvClarify" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                        Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="EmailLineNo" HeaderText="Email No" />
                                            <asp:BoundField DataField="HotelName" HeaderText="Hotel Name" />
                                            <asp:BoundField DataField="AssignedDate" HeaderText="Clarify Date" />
                                            <asp:BoundField DataField="ClarifyRemarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                        </Columns>
                                        <FooterStyle CssClass="grdfooter" />
                                        <RowStyle CssClass="grdRowstyle" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
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
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdPopupsClosetatus" runat="server" />
            <asp:Button ID="btnApprovalClarifyPopup" Style="display: none" runat="server" Text="Show Modal Popup" />
            <cc1:ModalPopupExtender ID="meApprovalClarify" runat="server" PopupControlID="pnlApprovalClarify"
                TargetControlID="btnApprovalClarifyPopup" EnableViewState="true" CancelControlID="lbApprovalClarifyClose"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlApprovalClarify" CssClass="modalPopupClarify" Style="display: none;"
                align="center" runat="server">
                <div style="width: 740px; vertical-align: middle; min-height: 100px; max-height: 350px;
                    margin-bottom: 20px; overflow: auto;">
                    <div>
                        <table style="width: 725px;">
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
                                <td align="right">
                                    <asp:ImageButton ID="lbApprovalClarifyClose" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                                        Width="25px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                                    <asp:Label ID="lblEmailCodePopup1" runat="server" Font-Bold="True" Font-Names="Calibri, Arial,Monaco, Consolas, monospace"
                                        Font-Size="14px" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Font-Size="Small" Style="font-family: Verdana"
                                        Text="Hotel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblHotelNamePopup1" runat="server" Font-Bold="True" Font-Names="Calibri"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblApprovalAssignDate" runat="server" Font-Size="Small" Style="font-family: Verdana"
                                        Text="Approval Assigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblAppAssdate" runat="server" Font-Bold="True" Font-Names="Calibri,Arial,Monaco,Consolas,monospace"
                                        Font-Size="14px"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label7" Style="font-family: Verdana" runat="server" Text="Clarify Remarks"
                                        Font-Size="Small"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAppClarifyRemarks" TextMode="MultiLine" runat="server" Height="50px"
                                        Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdPopupHotelCode1" runat="server" />
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdPopupMailId1" runat="server" />
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
                                    <asp:HiddenField ID="hdAssignedUser1" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="btnApprovalClarifySubmit" CssClass="btn" runat="server" Text="Submit" />
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
                                <td colspan="3">
                                    <asp:GridView ID="gvApprovalClarify" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                        Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="EmailLineNo" HeaderText="Email No" />
                                            <asp:BoundField DataField="HotelName" HeaderText="Hotel Name" />
                                            <asp:BoundField DataField="AssignedDate" HeaderText="Clarify Date" />
                                            <asp:BoundField DataField="ClarifyRemarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                        </Columns>
                                        <FooterStyle CssClass="grdfooter" />
                                        <RowStyle CssClass="grdRowstyle" />
                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                    </asp:GridView>
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
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField4" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
   <a href="clsHotelGroupServices.asmx" style="display:none">clsHotelGroupServices.asmx</a >
   
      </ContentTemplate>
    </asp:UpdatePanel>
     <div id="div1">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
                <asp:ServiceReference Path="~/clsVisualSearchService.asmx" />
            </Services>
        </asp:ScriptManagerProxy>
        
    </div>
</asp:Content>
