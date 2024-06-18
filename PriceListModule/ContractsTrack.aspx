<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"
    CodeFile="ContractsTrack.aspx.vb" Inherits="ContractsTrack" %>

<%--<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>--%>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <meta http-equiv="content-type" content="text/html;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="chrome=1">
    <link href="../css/Styles.css" rel="stylesheet" type="text/css" />
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
         tr.Pending
        {
            background-color: #FFFFBF;
                background: #FFFFBF;
     
        }
        
        td.lockedAlternative, th.lockedAlternative
        {
            background-color: #FFFFBF;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #FFFFBF;
            background: #FFFFBF;
            overflow: hidden;
            padding-right: 50px;
        }
        td.locked, th.locked
        {
            background-color: #FFD7F3;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #FFD7F3;
            overflow: hidden;
            padding-right: 50px;
        }
        td.lockedAlternativeLast, th.lockedAlternativeLast
        {
            background-color: #FFFFBF;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #FFFFBF;
            background: #FFFFBF;
        }
        td.lockedLast, th.lockedLast
        {
            background-color: #FFD7F3;
            position: absolute;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #FFD7F3;
        }
        td.lockedAlternativeNext, th.lockedAlternativeNext
        {
            background-color: #DDD9CF;
            position: static;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #DDD9CF;
        }
        td.lockedNext, th.lockedNext
        {
            background-color: #FFFFFF;
            position: static;
            display: block;
            padding-top: 5px;
            padding-bottom: 7px;
            border-right-color: #FFFFFF;
        }
        td.lockedHeader, th.lockedHeader
        {
            background-color: #06788B;
            position: absolute;
            display: block;
            border-right-color: #06788B;
            overflow: hidden;
            padding-right: 50px;
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
            min-width: 1100px;
            max-width: 1100px;
            min-height: 200px;
            max-height: 800px;
            color: #000;
        }
        
        .clearfix:after
        {
            content: '';
            display: table;
            clear: both;
        }
        #main
        {
            background-color: White;
            float: right;
            width: 65%;
            min-width: 25%;
            max-width: 75%;
        }
        #sidebar
        {
            background-color: White;
            min-width: 25%;
            max-width: 75%;
            width: 35%;
            float: left;
            overflow-y: auto;
            height: 800px;
        }
        
        #dragbar
        {
            background-color: #E5E5E5;
            height: 100%;
            float: right;
            width: 2px;
            cursor: col-resize;
        }
        
         #mainIgnore
        {
            background-color: White;
            float: right;
            width: 65%;
            min-width: 25%;
            max-width: 75%;
        }
        #sidebarIgnore
        {
            background-color: White;
            min-width: 25%;
            max-width: 75%;
            width: 35%;
            float: left;
            overflow-y: auto;
            height: 800px;
        }
        
        #dragbarIgnore
        {
            background-color: #E5E5E5;
            height: 100%;
            float: right;
            width: 2px;
            cursor: col-resize;
        }
        
        #ghostbar
        {
            width: 3px;
            background-color: #000;
            opacity: 0.5;
            position: absolute;
            cursor: col-resize;
            z-index: 999;
        }
        
        #mainPopup
        {
            background-color: White;
            float: right;
            width: 65%;
            min-width: 25%;
            max-width: 75%;
        }
        #sidebarPopup
        {
            background-color: White;
            min-width: 25%;
            max-width: 75%;
            width: 35%;
            float: left;
            overflow-y: auto;
            height: 800px;
        }
        
        .dragbarPopup
        {
            background-color: #E5E5E5;
            min-height: 180px;
            height: 100%;
            float: left;
            width: 2px;
            cursor: col-resize;
        }
        #ghostbarPopup
        {
            width: 3px;
            background-color: #000;
            opacity: 0.5;
            position: absolute;
            cursor: col-resize;
            z-index: 999;
        }
        
        
        .EntryLine
        {
            cursor: hand;
            background-color: #FFFFFF;
        }
        
        .EntryLine:hover
        {
            display: block;
            cursor: hand;
            background-color: #FFD1F5;
        }
        .EntryLineActive
        {
            cursor: hand;
            background-color: #C1F6EA;
        }
        
        .EntryLineActive:hover
        {
            cursor: hand;
            background-color: #FFD1F5;
        }
        
        .Item
        {
            font-family: Arial;
            height: 150px;
            width: 180px;
            margin: 5px;
        }
        .Item, .Item td, .Item td
        {
            border: 1px solid #ccc;
        }
        .Item .header
        {
            background-color: #F7F7F7 !important;
            color: Black;
            font-size: 10pt;
            line-height: 200%;
        }
        .page_enabled, .page_disabled
        {
            display: inline-block;
            height: 25px;
            min-width: 25px;
            line-height: 25px;
            text-align: center;
            text-decoration: none;
            border: 1px solid #ccc;
        }
        .page_enabled
        {
            font-family: Arial;
            background-color: #eee;
            color: #000;
        }
        .page_disabled
        {
            font-family: Arial;
            background-color: #6C6C6C;
            color: #fff !important;
        }
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">

        function setCurrentDate(txtAssignedDate, chkAssign, date) {

            var adate = document.getElementById(txtAssignedDate);
            var chkAsgn = document.getElementById(chkAssign);

            if (chkAsgn.checked == true) {

                if (adate.value == null || adate.value == '') {
                    adate.value = date;
                }

            }
            else {
                adate.value = "";
            }
        }
        function setCurrentAppDate(txtApprovalAssDateTime, chkApprovalTime, date) {

            var adate = document.getElementById(txtApprovalAssDateTime);
            var chkAsgn = document.getElementById(chkApprovalTime);

            if (chkAsgn.checked == true) {

                if (adate.value == null || adate.value == '') {
                    var currentdate = new Date().toLocaleDateString()//
                    var curTime = new Date();
                    var datetime = currentdate 
                 //+ " "
               // + curTime.getHours() + ":"
               // + curTime.getMinutes()

                 adate.value = datetime;
                }

            }
            else {
                adate.value = "";
            }
        }
        function checkfromdates(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);

            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
            }

            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);


            if (newfdate > newtdate) {
                tdate.value = "";
                alert("From date should not be greater than To date");
            }

            setdate();
        }

        function checkdates(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);



            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
            }

            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);

            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);
            if (newtdate < newfdate) {
                tdate.value = "";
                alert("To date should  be greater than From date");
            }



            setdate();
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

        function setdate() {
            var btnDummy = document.getElementById('<%=btnDummy.ClientID %>');
            //  btnDummy.click();
        }

        function divrowclick(code, obj) {
            var hdEmailCode = document.getElementById("<%=hdEmailCode.ClientID%>");
            hdEmailCode.value = code;
            var btnProcessclickMail = document.getElementById("<%=btnProcessclickMail.ClientID%>");
            btnProcessclickMail.click();
            // GetEmailDetails(code);
        }
        function divrowIgnoreclick(code, obj) {
            var hdEmailCodeIgnore = document.getElementById("<%=hdEmailCodeIgnore.ClientID%>");
            hdEmailCodeIgnore.value = code;
            var btnProcessIgnoreclickMail = document.getElementById("<%=btnProcessIgnoreclickMail.ClientID%>");
            btnProcessIgnoreclickMail.click();
            // GetEmailDetails(code);
        }

        function TrackingStatusautocompletesKeyup() {

            $("#<%=gvHotels.ClientID%> tr input[id*='txtTrackingStatus']").each(function () {
                $(this).change(function (event) {

                    var hiddenfieldID = $(this).attr("id").replace("txtTrackingStatus", "txtTrackCode");
                    $get(hiddenfieldID).value = '';
                });
            });

        }
        function UpDateTypeautocompletesKeyup() {

            $("#<%=gvTracking.ClientID%> tr input[id*='txtUpdateType']").each(function () {
                $(this).change(function (event) {

                    var hiddenfieldID = $(this).attr("id").replace("txtUpdateType", "txtUpdateTypeCode");
                    $get(hiddenfieldID).value = '';
                });
            });

        }
        function AssignedToautocompletesKeyup() {

            $("#<%=gvTracking.ClientID%> tr input[id*='txtAssignedTo']").each(function () {
                $(this).change(function (event) {

                    var hiddenfieldID = $(this).attr("id").replace("txtAssignedTo", "txtAssignedToCode");
                    $get(hiddenfieldID).value = '';
                });
            });

        }

        function ApproverautocompletesKeyup() {
            $("#<%= txtApprover.ClientID %>").bind("change", function () {
               // document.getElementById('<%=txtApproverCode.ClientID%>').value = '';
            });
        }
        function ReApproverautocompletesKeyup() {
            $("#<%= txtReApprover.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtReApproverCode.ClientID%>').value = '';
            });
        }

        function ReAssignedToautocompletesKeyup() {
            $("#<%= txtReAssignedTo.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtReAssignedToCode.ClientID%>').value = '';
            });
        }
        function TrackingNumberAutoCompletesKeyup() {
            $("#<%= txtTrackingNo.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtTrackingCode.ClientID%>').value = '';
            });
        }

        function hotelautocompleteselected(source, eventArgs) {

            var hiddenfieldID = source.get_id().replace("TrackingStatus_AutoCompleteExtender", "txtTrackCode");
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        function UpdateTypeautocompleteselected(source, eventArgs) {

            var hiddenfieldID = source.get_id().replace("UpadateTypeAutoCompleteExtender", "txtUpdateTypeCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
            //  alert(eventArgs.get_value());

        }
        function TrackingNumberAutoCompleteSelected(source, eventArgs) {

            var hiddenfieldID = source.get_id().replace("aeTrackingNumber", "txtTrackingcode");
            $get(hiddenfieldID).value = eventArgs.get_value();
            //  alert(eventArgs.get_value());

        }
        function AssignedToautocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("txtAssignedTo_AutoCompleteExtender", "txtAssignedToCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
        }
        function ReAssignedToautocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("txtReAssignedTo_AutoCompleteExtender", "txtReAssignedToCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
            document.getElementById('<%=txtReAssignedToCode.ClientID%>').value = $get(hiddenfieldID).value;
           // alert(document.getElementById('<%=txtReAssignedToCode.ClientID%>').value);
        }
        function ApproverAutocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("aeApprover", "txtApproverCode");

            $get(hiddenfieldID).value = eventArgs.get_value();
            document.getElementById('<%=txtApproverCode.ClientID%>').value = $get(hiddenfieldID).value;
         //   alert(document.getElementById('<%=txtApproverCode.ClientID%>').value);

        }
        function ReApproverAutocompleteselected(source, eventArgs) {
            var hiddenfieldID = source.get_id().replace("aeReApprover", "txtReApproverCode");
            $get(hiddenfieldID).value = eventArgs.get_value();
            document.getElementById('<%=txtReApproverCode.ClientID%>').value = $get(hiddenfieldID).value;
           // alert(document.getElementById('<%=txtReApproverCode.ClientID%>').value);
        }

    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            TrackingStatusautocompletesKeyup();
            UpDateTypeautocompletesKeyup();
            AssignedToautocompletesKeyup();
           // ReAssignedToautocompletesKeyup();
            TrackingNumberAutoCompletesKeyup();
            ApproverautocompletesKeyup();
           // ReApproverautocompletesKeyup();
        }
    </script>
    <script language="javascript" type="text/javascript">
        var i = 0;
        var dragging = false;
        $(document).ready(function () {
            $('#dragbar').mousedown(function (e) {
                // alert('test');
                e.preventDefault();
                dragging = true;
                var main = $('#main');
                var ghostbar = $('<div>',
                        { id: 'ghostbar',
                            css: {
                                height: main.outerHeight(),
                                top: main.offset().top,
                                left: main.offset().left
                            }
                        }).appendTo('body');

                $(document).mousemove(function (e) {
                    ghostbar.css("left", e.pageX + 2);
                });

            });

            $('#dragbarIgnore').mousedown(function (e) {
                // alert('test');
                e.preventDefault();
                dragging = true;
                var main = $('#mainIgnore');
                var ghostbar = $('<div>',
                        { id: 'ghostbar',
                            css: {
                                height: main.outerHeight(),
                                top: main.offset().top,
                                left: main.offset().left
                            }
                        }).appendTo('body');

                $(document).mousemove(function (e) {
                    ghostbar.css("left", e.pageX + 2);
                });

            });


            visualsearchbox();
            visualsearchboxInbox();
            visualsearchboxPopup();
            visualsearchboxTrackView();
            visualsearchboxInboxIgnore();
            TrackingStatusautocompletesKeyup();
            UpDateTypeautocompletesKeyup();
            AssignedToautocompletesKeyup();
            ReAssignedToautocompletesKeyup();
            TrackingNumberAutoCompletesKeyup();
            ReApproverautocompletesKeyup();
            ApproverautocompletesKeyup();
        });
        $(document).mouseup(function (e) {
            if (dragging) {
                var percentage = (e.pageX / window.innerWidth) * 100;
                var mainPercentage = 100 - percentage;
                $('#console').text("side:" + percentage + " main:" + mainPercentage);
                $('#sidebar').css("width", percentage + "%");
                $('#main').css("width", mainPercentage + "%");
                $('#ghostbar').remove();
                $(document).unbind('mousemove');
                dragging = false;
            }
        });
        $(document).mouseup(function (e) {
            if (dragging) {
                var percentage = (e.pageX / window.innerWidth) * 100;
                var mainPercentage = 100 - percentage;
                $('#console').text("side:" + percentage + " main:" + mainPercentage);
                $('#sidebarIgnore').css("width", percentage + "%");
                $('#mainIgnore').css("width", mainPercentage + "%");
                $('#ghostbar').remove();
                $(document).unbind('mousemove');
                dragging = false;
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
            //  alert('test');
            var $txtvsprocess = $(document).find('.cs_txtvsprocess1');
            $txtvsprocess.val('HOTELS:" " TEXT:" "');
            window.visualSearch1 = VS.init({
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
                        var $txtvsprocess = $(document).find('.cs_txtvsprocess1');
                        $txtvsprocess.val(visualSearch1.searchQuery.serialize());

                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit1');
                        $txtvsprocesssplit.val(visualSearch1.searchQuery.serializetosplit());
                        // alert(visualSearch.searchQuery.serializetosplit())
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
                        }
                    },
                    facetMatches: function (callback) {
                        callback([

                { label: 'HOTELS', category: 'HOTELS' },
                { label: 'TEXT', category: 'HOTELS' },
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


        function visualsearchboxInbox() {
            //  alert('test');
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('"TICKECT NO":" " HOTELS:" " HOTEL_STATUS:" " TRACKING_STATUS:" "  FROM_EMAIL:" " TEXT:" "');
            window.visualSearch2 = VS.init({
                container: $('#search_box_containerInbox'),
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
                        $txtvsprocess.val(visualSearch2.searchQuery.serialize().replace('<', '___').replace('>', '...'));
                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch2.searchQuery.serializetosplit().replace('<', '___').replace('>', '...'));
                        var btnvsprocess = document.getElementById("<%=btnvsprocessInbox.ClientID%>");
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
                            case 'TICKECT NO':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayTicketNoVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'HOTEL_STATUS':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayHotelStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'TRACKING_STATUS':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayTrackingStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'FROM_EMAIL':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayFromEmailIdVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                    { label: 'TICKECT NO', category: 'INBOX' },
                    { label: 'HOTELS', category: 'INBOX' },
                    { label: 'HOTEL_STATUS', category: 'INBOX' },
                    { label: 'TRACKING_STATUS', category: 'INBOX' },
                    { label: 'FROM_EMAIL', category: 'INBOX' },
                    { label: 'TEXT', category: 'INBOX' },
              ]);
                    }
                }
            });
        }


        function visualsearchboxInboxIgnore() {
            //  alert('test');
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('"TICKECT NO":" " HOTELS:" " HOTEL_STATUS:" " TRACKING_STATUS:" "  FROM_EMAIL:" " TEXT:" "');
            window.visualSearch4 = VS.init({
                container: $('#search_box_containerInboxIgnore'),
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
                        $txtvsprocess.val(visualSearch4.searchQuery.serialize().replace('<', '___').replace('>', '...'));
                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch4.searchQuery.serializetosplit().replace('<', '___').replace('>', '...'));
                        var btnvsprocess = document.getElementById("<%=btnvsprocessInboxIgnore.ClientID%>");
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
                            case 'TICKECT NO':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayTicketNoVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'HOTEL_STATUS':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayHotelStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'TRACKING_STATUS':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayTrackingStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'FROM_EMAIL':
                                var asSqlqry = '';
                                glcallback = callback;
                                ColServices.clsVisualSearchService.GetListOfArrayFromEmailIdVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                        }
                    },
                    facetMatches: function (callback) {
                        callback([
                    { label: 'TICKECT NO', category: 'INBOX' },
                    { label: 'HOTELS', category: 'INBOX' },
                    { label: 'HOTEL_STATUS', category: 'INBOX' },
                    { label: 'TRACKING_STATUS', category: 'INBOX' },
                    { label: 'FROM_EMAIL', category: 'INBOX' },
                    { label: 'TEXT', category: 'INBOX' },
              ]);
                    }
                }
            });
        }


        function visualsearchboxPopup() {
            //  alert('test');
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"TRACKING STATUS":" "' + '"FROM EMAIL":" " TEXT:" "' + '"PROGRESS STAGE":" "');
            window.visualSearch = VS.init({
                container: $('#search_box_containerPopup'),
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

                        var btnvsprocess = document.getElementById("<%=btnvsprocessPopup.ClientID%>");
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
                    { label: 'TRACKING STATUS', category: 'TRACKING' },
                    { label: 'FROM EMAIL', category: 'TRACKING' },
                    { label: 'TEXT', category: 'TRACKING' },
                    { label: 'UPDATE TYPE', category: 'TRACKING' },
                    { label: 'ASSIGNED TO', category: 'TRACKING' },
                    { label: 'PROGRESS STAGE', category: 'TRACKING' },
              ]);
                    }
                }
            });
        }


        function visualsearchboxTrackView() {
            //  alert('test');
            var $txtvsprocess = $(document).find('.cs_txtvsprocess');
            $txtvsprocess.val('"EMAIL CODE":" "' + '"EMAIL DATE":" " HOTELS:" "' + '"HOTEL STATUS":" "' + '"EMAIL SUBJECT":" "' + '"TRACKING STATUS":" "' + '"FROM EMAIL":" " TEXT:" "');
            window.visualSearch3 = VS.init({
                container: $('#search_box_container_TrackView'),
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
                        $txtvsprocess.val(visualSearch3.searchQuery.serialize().replace('<', '___').replace('>', '...'));
                        var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit');
                        $txtvsprocesssplit.val(visualSearch3.searchQuery.serializetosplit().replace('<', '___').replace('>', '...'));

                        var btnvsprocess = document.getElementById("<%=btnvsprocess_TrackView.ClientID%>");
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
                    { label: 'TRACKING STATUS', category: 'TRACKING' },
                    { label: 'FROM EMAIL', category: 'TRACKING' },
                    { label: 'TEXT', category: 'TRACKING' },
                    { label: 'UPDATE TYPE', category: 'TRACKING' },
                    { label: 'ASSIGNED TO', category: 'TRACKING' },
//                    { label: 'PROGRESS STAGE', category: 'TRACKING' },
              ]);
                    }
                }
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
            visualsearchboxPopup();
            visualsearchboxTrackView();
            visualsearchboxInboxIgnore();
            visualsearchboxInbox()
        }
    </script>
    <table style="width: 100%">
        <tr>
            <td colspan="2" class="field_heading" align="center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Contracts Tracking"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                    <cc1:TabPanel HeaderText="To Action Emails" ID="Emailinbox" Font-Size="Medium" runat="server">
                        <ContentTemplate>
                          <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                            <table style="width: 100%; border-right: gray 0px solid; border-top: gray 0px solid;
                                border-left: gray 0px solid; border-bottom: gray 0px solid;">
                                <tr>
                                    <td style="width: 99%; padding-left: 8px;" valign="top">
                                        <div style="width: 99.5%; display: inline-block; margin: 6px 4px 0 0;">
                                            <div id="VSInbox" class="container" style="border: 0px;">
                                                <div id="search_box_containerInbox">
                                                </div>
                                                <asp:TextBox ID="txtvsprocessInbox" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtvsprocesssplitInbox" runat="server" class="cs_txtvsprocesssplit"
                                                    Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnvsprocessInbox" runat="server" Style="display: none" />
                                            </div>
                                            <asp:DataList ID="dlInboxSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                <asp:LinkButton ID="lbInboxCategory" class="button button4" runat="server" Text='<%# Eval("Value") %>'>LinkButton</asp:LinkButton>
                                                                <asp:LinkButton ID="lbCloseInboxCategory" class="buttonClose button4" runat="server"
                                                                    OnClick="lbCloseInboxCategory_Click">X</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                    <td style="width: 1%;" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 99%; padding-left: 8px;" valign="top">
                                        <div class="clearfix">
                                            <div id="sidebar">
                                                <div id="dragbar">
                                                </div>
                                                <asp:UpdatePanel ID="upnlInbox" runat="server">
                                                    <ContentTemplate>
                                                        <div style="border-color: #E0E0E0; width: 95%; margin-top: 15px; background-color: #E0E0E0;
                                                            padding-right: 0px; padding-left: 0px; border-right: gray 0px solid">
                                                            <asp:DataList ID="dlMailInbox" runat="server" RepeatColumns="1" Width="100%">
                                                                <ItemTemplate>
                                                                    <div id="tblRow" class="EntryLine" runat="server">
                                                                        <table style="width: 100%;" cellpadding="1" cellspacing="1">
                                                                            <tr>
                                                                                <td valign="top" width=".5%">
                                                                                    <asp:CheckBox ID="chkAssigned" runat="server" AutoPostBack="True" Checked='<%# Eval("HotelStatus") %>'
                                                                                        OnCheckedChanged="chkAssigned_CheckedChanged" />
                                                                                </td>
                                                                                <td width="99.5%">
                                                                                    <div id="row" style="width: 100%;" onclick='divrowclick(<%# Eval("EmailId") %>,this.Id)'
                                                                                        style="float: left;">
                                                                                        <table cellpadding="1" cellspacing="1" style="width: 100%;">
                                                                                            <tr>
                                                                                                <td width="60%">
                                                                                                    <asp:Image ID="imgMail" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                                                                                                    <asp:Label ID="lblEmailNo" runat="server" Font-Names="Arial" Font-Size="14px" Text='<%# Eval("EmailNo") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" width="40%">
                                                                                                    <asp:Label ID="lblDate" runat="server" Font-Names="Arial" Font-Size="12px" Text='<%# Eval("EmailDate") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" width="100%">
                                                                                                    <asp:Label ID="lblFrom" runat="server" Font-Names="Arial" Font-Size="14px" ForeColor="#003366"
                                                                                                        Text='<%# Eval("EmailFrom") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("EmailId") %>' Visible="False"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblSubject" runat="server" Style="font-family: Verdana; font-size: 12px;"
                                                                                                        Text='<%# Eval("EmailSubject") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </div>
                                                        <div style="margin: 5px">
                                                            <asp:Repeater ID="rptPager" runat="server">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                                                        OnClick="Page_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="main">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <div style="padding-top: 15px; overflow: auto; width: 100%;">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSubject" runat="server" Font-Names="Arial" Font-Size="Large"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDate" runat="server" Font-Names="Arial" Font-Size="Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblFrom" runat="server" Font-Names="Arial" Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DataList ID="DLAttachments" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                                            <ItemTemplate>
                                                                                <table class="style1">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Image ID="imgAttachmentType" runat="server" ImageUrl="~/Images/crystaltoolbar/mail-attachment2.png"
                                                                                                Width="25px" />
                                                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("content") %>' Visible="False"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lbAttachment" runat="server" Font-Names="Arial" Font-Size="Small"
                                                                                                Font-Underline="False" ForeColor="#333333" Text='<%# Eval("FileName") %>' OnClick="lbAttachment_Click"></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:DataList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trMessage" runat="server">
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblMessage" runat="server" BackColor="#BAEDFC" BorderColor="#CCCCCC"
                                                                            Font-Names="Arial"> Select an item 
                                        to read </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <div style="overflow: auto; width: 100%;">
                                                                            <asp:Label ID="lblBody" runat="server" Font-Names="Arial"></asp:Label></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 1%;" valign="top">
                                    </td>
                                </tr>
                            </table>
                             </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Tracking" ID="Tracking" runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upTracking" runat="server">
                                <ContentTemplate>
                                    <div style="width: 98%; overflow: auto;">
                                        <div style="width: 99.5%; display: inline-block; margin: 10px 4px 0 0;">
                                            <div id="VSPopup" class="container" style="border: 0px;">
                                                <div id="search_box_containerPopup">
                                                </div>
                                                <asp:TextBox ID="txtvsprocessPopup" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtvsprocesssplitPopup" runat="server" class="cs_txtvsprocesssplit"
                                                    Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnvsprocessPopup" runat="server" Style="display: none" />
                                            </div>
                                            <div style="width: 80%; display: inline-block; margin: 6px 4px 0 0; min-height: 50px;">
                                                <asp:DataList ID="dlTrackingVS" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                    <ItemTemplate>
                                                        <table class="style1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lbHotelPopup" class="button button4" runat="server" Text='<%# Eval("Value") %>'
                                                                        OnClick="lbHotel_Click">LinkButton</asp:LinkButton>
                                                                    <asp:LinkButton ID="lbClosePopup" class="buttonClose button4" runat="server" OnClick="lbClosePopup_Click">X</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="width: 98%; overflow: auto; padding-bottom: 35px;">
                                        <asp:GridView ID="gvTracking" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                            Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="200%" AllowPaging="True">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbUpdate" runat="server" OnClick="lbUpdate_Click">Update</asp:LinkButton>
                                                        <asp:HiddenField ID="hdEmailId_" runat="server" Value='<%# Eval("EmailId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbUpdate1" runat="server">Update</asp:LinkButton>
                                                        <asp:HiddenField ID="hdEmailId_1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px"></HeaderStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailCode" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdEmailId" runat="server" Value='<%# Eval("EmailId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailCode1" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdEmailId1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailDate" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailDate1" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailTime" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Time">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailTime1" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Hotel Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCHotelName1" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Hotel Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCHotelName" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdCHotelCode" runat="server" Value='<%# Eval("HotelCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Hotel Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHotelStatus" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Email Subject" DataField="EmailSubject">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Tracking Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackingStatus" runat="server" Text='<%# Eval("TrackingStatus") %>'></asp:Label>
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
                                                <asp:TemplateField HeaderText="Assigned Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssignedDate" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned To">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssignedToName" runat="server" Text='<%# Eval("AssignedToName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Progress Stage">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProgressStage" runat="server" Text='<%# Eval("ProgressStage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Update Start">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUpdateStart" runat="server" Text='<%# Eval("UpdateStart") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Update End">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUpdateEnd" runat="server" Text='<%# Eval("UpdateEnd") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approval Start">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovalStart" runat="server" Text='<%# Eval("ApprovalStart") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approval End">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovalEnd" runat="server" Text='<%# Eval("ApprovalEnd") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Assigned To code" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssignedToCode" runat="server" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" Visible="false" HeaderText="Approver">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("Approver") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                        <asp:Button ID="btnDummy" runat="server" Style="display: none" Text="" /></div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Updated Tracking & Files ( Actioned Emails )" ID="ActionedEmails"
                        runat="server">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <div style="width: 98%; overflow: auto;">
                                            <div style="width: 99.5%; display: inline-block; margin: 10px 4px 0 0;">
                                                <div id="VSTrackView" class="container" style="border: 0px;">
                                                    <div id="search_box_container_TrackView">
                                                    </div>
                                                    <asp:TextBox ID="txtvsprocess_TrackView" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                    <asp:TextBox ID="txtvsprocesssplit_TrackView" runat="server" class="cs_txtvsprocesssplit"
                                                        Style="display: none"></asp:TextBox>
                                                    <asp:Button ID="btnvsprocess_TrackView" runat="server" Style="display: none" />
                                                </div>
                                                <div style="width: 80%; display: inline-block; margin: 6px 4px 0 0; min-height: 50px;">
                                                    <asp:DataList ID="dlTrackViewSerach" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                        <asp:LinkButton ID="lbHotelTrackview" class="button button4" runat="server" Text='<%# Eval("Value") %>'
                                                                         >LinkButton</asp:LinkButton>
                                                                        <asp:LinkButton ID="lbCloseTrackview" class="buttonClose button4" runat="server"
                                                                            OnClick="lbCloseTrackview_Click">X</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width: 98%; overflow: auto; padding-bottom: 35px;">
                                            <asp:GridView ID="gvTrackingView" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="200%" AllowPaging="True">
                                                <FooterStyle CssClass="grdfooter" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdateTrackView" runat="server" OnClick="lbUpdateTrackView_Click">View</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbUpdate1" runat="server">View</asp:LinkButton>
                                                            <asp:HiddenField ID="hdEmailId_1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailCode" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdEmailId" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailCode1" runat="server" Text='<%# Bind("EmailLineNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdEmailId1" runat="server" Value='<%# Eval("EmailId") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailDate" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailDate1" runat="server" Text='<%# Bind("EmailDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailTime" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Email Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailTime1" runat="server" Text='<%# Bind("EmailTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCHotelName1" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCHotelName" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdCHotelCode" runat="server" Value='<%# Eval("HotelCode") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hotel Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHotelStatus" runat="server" Text='<%# Eval("HotelStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Email Subject" DataField="EmailSubject">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Tracking Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTrackingStatus" runat="server" Text='<%# Eval("TrackingStatus") %>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Assigned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedDate" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Assigned To">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssignedToName" runat="server" Text='<%# Eval("AssignedToName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Progress Stage">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgressStage" runat="server" Text='<%# Eval("ProgressStage") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update Start">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpdateStart" runat="server" Text='<%# Eval("UpdateStart") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Update End">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUpdateEnd" runat="server" Text='<%# Eval("UpdateEnd") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Start">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalStart" runat="server" Text='<%# Eval("ApprovalStart") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval End">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalEnd" runat="server" Text='<%# Eval("ApprovalEnd") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Assigned To code" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssignedToCode" runat="server" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderStyle-Width="80px" Visible="false" HeaderText="Approver">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprover" runat="server" Text='<%# Bind("Approver") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle" />
                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="grdheader" ForeColor="White" />
                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                            </asp:GridView>
                                            <asp:Button ID="Button2" runat="server" Style="display: none" Text="" /></div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Ignored Emails" ID="IgnoredEmails" runat="server">
                        <ContentTemplate>
                            <div>
                               <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                            <table style="width: 100%; border-right: gray 0px solid; border-top: gray 0px solid;
                                border-left: gray 0px solid; border-bottom: gray 0px solid;">
                                <tr>
                                    <td style="width: 99%; padding-left: 8px;" valign="top">
                                        <div style="width: 99.5%; display: inline-block; margin: 6px 4px 0 0;">
                                            <div id="VSInbox_Ignore" class="container" style="border: 0px;">
                                                <div id="search_box_containerInboxIgnore">
                                                </div>
                                                <asp:TextBox ID="txtvsprocessInbox_Ingnore" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                <asp:TextBox ID="txtvsprocesssplitInbox_Ignore" runat="server" class="cs_txtvsprocesssplit"
                                                    Style="display: none"></asp:TextBox>
                                                <asp:Button ID="btnvsprocessInboxIgnore" runat="server" Style="display: none" />
                                            </div>
                                            <asp:DataList ID="dlIgnore" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblType" runat="server" Visible="false" Text='<%# Eval("Code") %>'></asp:Label>
                                                                <asp:LinkButton ID="lbInboxIgnore" class="button button4" runat="server" Text='<%# Eval("Value") %>'>LinkButton</asp:LinkButton>
                                                                <asp:LinkButton ID="lbCloseInboxIgnore" class="buttonClose button4" runat="server"
                                                                    OnClick="lbCloseInboxIgnore_Click">X</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </td>
                                    <td style="width: 1%;" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 99%; padding-left: 8px;" valign="top">
                                        <div class="clearfix">
                                            <div id="sidebarIgnore">
                                                <div id="dragbarIgnore">
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <div style="border-color: #E0E0E0; width: 95%; margin-top: 15px; background-color: #E0E0E0;
                                                            padding-right: 0px; padding-left: 0px; border-right: gray 0px solid">
                                                            <asp:DataList ID="dlMailInboxIgnore" runat="server" RepeatColumns="1" Width="100%">
                                                                <ItemTemplate>
                                                                    <div id="tblRowIgnore" class="EntryLine" runat="server">
                                                                        <table style="width: 100%;" cellpadding="1" cellspacing="1">
                                                                            <tr>
                                                                                <td valign="top" width=".5%">
                                                                                    <asp:CheckBox ID="chkAssigned1" Visible="false" runat="server" AutoPostBack="True" Checked='<%# Eval("HotelStatus") %>'
                                                                                        OnCheckedChanged="chkAssigned_CheckedChanged" />
                                                                                </td>
                                                                                <td width="99.5%">
                                                                                    <div id="rowIgnore" style="width: 100%;" onclick='divrowIgnoreclick(<%# Eval("EmailId") %>,this.Id)'    style="float: left;">
                                                                                        <table cellpadding="1" cellspacing="1" style="width: 100%;">
                                                                                            <tr>
                                                                                                <td width="60%">
                                                                                                    <asp:Image ID="imgMail" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                                                                                                    <asp:Label ID="lblEmailNo" runat="server" Font-Names="Arial" Font-Size="14px" Text='<%# Eval("EmailNo") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" width="40%">
                                                                                                    <asp:Label ID="lblDate" runat="server" Font-Names="Arial" Font-Size="12px" Text='<%# Eval("EmailDate") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" width="100%">
                                                                                                    <asp:Label ID="lblFrom" runat="server" Font-Names="Arial" Font-Size="14px" ForeColor="#003366"
                                                                                                        Text='<%# Eval("EmailFrom") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("EmailId") %>' Visible="False"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblSubject" runat="server" Style="font-family: Verdana; font-size: 12px;"
                                                                                                        Text='<%# Eval("EmailSubject") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </div>
                                                        <div style="margin: 5px">
                                                            <asp:Repeater ID="rptrIgnore" runat="server">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                        CssClass='<%# If(Convert.ToBoolean(Eval("Enabled")), "page_enabled", "page_disabled")%>'
                                                                        OnClick="IgnorePage_Changed" OnClientClick='<%# If(Not Convert.ToBoolean(Eval("Enabled")), "return false;", "") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div id="mainIgnore">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <div style="padding-top: 15px; overflow: auto; width: 100%;">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSubjectIgnore" runat="server" Font-Names="Arial" Font-Size="Large"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDateIgnore" runat="server" Font-Names="Arial" Font-Size="Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblFromIgnore" runat="server" Font-Names="Arial" Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DataList ID="dlIgnoreAttachment" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                                            <ItemTemplate>
                                                                                <table class="style1">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Image ID="imgAttachmentType" runat="server" ImageUrl="~/Images/crystaltoolbar/mail-attachment2.png"
                                                                                                Width="25px" />
                                                                                            <asp:Label ID="lblFile" runat="server" Text='<%# Eval("content") %>' Visible="False"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="lbAttachment" runat="server" Font-Names="Arial" Font-Size="Small"
                                                                                                Font-Underline="False" ForeColor="#333333" Text='<%# Eval("FileName") %>' OnClick="lbAttachment_Click"></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                        </asp:DataList>
                                                                    </td>
                                                                </tr>
                                                                <tr id="tr6" runat="server">
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblBodyIgnore" runat="server" BackColor="#BAEDFC" BorderColor="#CCCCCC"
                                                                            Font-Names="Arial"> Select an item 
                                        to read </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <div style="overflow: auto; width: 100%;">
                                                                            <asp:Label ID="Label14" runat="server" Font-Names="Arial"></asp:Label></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 1%;" valign="top">
                                    </td>
                                </tr>
                            </table>
                             </ContentTemplate>
                            </asp:UpdatePanel>
                                </div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Tracking Report" ID="TrackingReport" runat="server">
                        <ContentTemplate>
                            <div>
                                <asp:Label ID="Label3" runat="server">
                                </asp:Label></div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel HeaderText="Email Count" ID="EmailCount" runat="server">
                        <ContentTemplate>
                            <div>
                                <asp:Label ID="lblEmailCount" runat="server">
                                </asp:Label></div>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
            <td style="width: 20%">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <!-- ModalPopupExtender -->
            <asp:Button ID="btnShow" Style="display: none" runat="server" Text="Show Modal Popup" />
            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="pnlPopup" TargetControlID="btnShow"
                EnableViewState="true" CancelControlID="btnClose1" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <%-- style = "display:none"--%>
            <!-- ModalPopupExtender -->
            <asp:Panel ID="pnlPopup" runat="server"  style="display:none;"  CssClass="modalPopup" align="center">
                <table width="900px" style="font-family: Monaco, Consolas, monospace;">
                    <tr>
                        <td width="850px">
                            <asp:Image ID="imgMailpopup" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                            <asp:Label ID="lblIdPopup" runat="server" Text="Label"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                                Width="25px" />
                            <asp:Button ID="btnClose1" Style="display: none" runat="server" Text="Close" />
                        </td>
                    </tr>
                    <tr>
                        <td width="850px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                           
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                              <cc1:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
                                <cc1:TabPanel HeaderText="Assign Emails" ID="TabPanel1" Font-Size="Medium" runat="server">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                             <table class="style1">
                                              <tr>
                                    <td style="padding-right:100px" align="right">
                                    <asp:Button ID="btnAddHotel" runat="server" CssClass="btn" Font-Bold="False" 
                tabIndex="4" Text="Add New Hotel" />
                                    </td></tr>
                                <tr>
                                    <td>
                                       <div style="width: 90%; display: inline-block; margin: 6px 4px 0 0;">
                                <div id="VS" class="container" style="border: 0px;">
                                    <div id="search_box_container">
                                    </div>
                                    <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess1" Style="display: none"></asp:TextBox>
                                    <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit1"
                                        Style="display: none"></asp:TextBox>
                                    <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                </div>
                            </div></td>
                                </tr>
                                <tr>
                                    <td>
                                           <asp:DataList ID="dlList" runat="server" RepeatColumns="3" 
                                RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Code") %>' 
                                                    Visible="false"></asp:Label>
                                                <asp:LinkButton ID="lbHotel" runat="server" class="button button4" 
                                                    OnClick="lbHotel_Click" Text='<%# Eval("Value") %>'>LinkButton</asp:LinkButton>
                                                <asp:LinkButton ID="lbClose" runat="server" class="buttonClose button4" 
                                                    OnClick="lbClose_Click">X</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList></td>
                                </tr>
                                <tr>
                                    <td>
                                          <div style="max-height: 400px; overflow: auto;">
                                <asp:GridView ID="gvHotels" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                    Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                    <FooterStyle CssClass="grdfooter" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                                    AutoPostBack="True" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hotel Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHotelCode" runat="server" Text='<%# Bind("HotelCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hotel Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHotelName" runat="server" Text='<%# Bind("HotelName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Hotel Status" DataField="HotelStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Tracking Status">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTrackingStatus" runat="server" Width="200px" AutoPostBack="True"
                                                    OnTextChanged="txtTrackingStatus_TextChanged" Text='<%# Eval("TrackingStatus") %>'></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="TrackingStatus_AutoCompleteExtender" runat="server"
                                                    DelimiterCharacters="" Enabled="True" ServicePath="" CompletionInterval="10"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" EnableCaching="false"
                                                    FirstRowSelected="true" MinimumPrefixLength="-1" OnClientItemSelected="hotelautocompleteselected"
                                                    ServiceMethod="GetTrackingStatus" TargetControlID="txtTrackingStatus">
                                                </cc1:AutoCompleteExtender>
                                                <asp:HiddenField ID="hdnTrackingStatus" runat="server" />
                                                <asp:TextBox Style="display: none" ID="txtTrackCode" class="field_input" type="text"
                                                    runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="grdRowstyle" />
                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                    <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                </asp:GridView>
                            </div>
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></td>
                                </tr>
 <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="8" Text="Save" /></td>
                                </tr>


                            </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                 <cc1:TabPanel HeaderText="Additional Email Assignment" ID="TabPanel2" Font-Size="Medium" runat="server">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                             <table class="style1">
                                               <tr>
                                    <td colspan="3">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblAdditionalEmails" runat="server" Width="150px"  Text="Select Track Number"></asp:Label><asp:TextBox 
                                            ID="txtTrackingNo"  Width="550px" height="25px" runat="server"></asp:TextBox>
                                             <cc1:AutoCompleteExtender ID="aeTrackingNumber" runat="server" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                        EnableCaching="false" Enabled="True" FirstRowSelected="true" MinimumPrefixLength="-1"
                                       ServiceMethod="GetTrackingNumbers" OnClientItemSelected="TrackingNumberAutoCompleteSelected"
                                        ServicePath="" TargetControlID="txtTrackingNo">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox Style="display: none" ID="txtTrackingcode" class="field_input" type="text"
                                        runat="server"></asp:TextBox>
                                            </td>
                                </tr>
                              
                                <tr>
                                    <td colspan="3">
                                        &nbsp;</td>
                                </tr>
                                 <tr>
                                     <td style="padding-left:150px;">
                                         <asp:Button ID="btnAssign" runat="server" CssClass="btn" Text="Assign" /></td>
                                     <td>
                                         &nbsp;</td>
                                     <td>
                                         &nbsp;</td>
                                </tr>
                                </tr>
                            </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                         
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                          
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">

                         
                           
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <asp:HiddenField ID="hdPopupStatus" runat="server" />
            <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
            <asp:Button ID="btnTrackPopup" Style="display: none" runat="server" Text="Show Modal Popup" />
            <cc1:ModalPopupExtender ID="meContractTracking" runat="server" PopupControlID="pnlcontractTrack"
                TargetControlID="btnTrackPopup" EnableViewState="true" CancelControlID="lbCloseTrack1"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <%-- style = "display:none"--%>
            <!-- ModalPopupExtender -->
      <%--      <asp:Panel ID="pnlcontractTrack"  style="display:none;"  runat="server" CssClass="modalPopupNew" align="center">
                <div style="width: 100%; vertical-align: middle;">
                    <div style="width: 35px; float: right;">
                        <asp:ImageButton ID="lbCloseTrack" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                            Width="25px" />
                        <asp:HiddenField ID="lbCloseTrack1" runat="server" />
                    </div>
                    <div style="width: 35%; float: left; margin-top: 15px; min-height: 200px; max-height: 550px; overflow: auto;--%>
                      <asp:Panel ID="pnlcontractTrack"  style="display:none;"  runat="server" CssClass="modalPopupNew" align="center">
                <div style="width: 100%; vertical-align: middle;">
                    <div style="width: 35px; float: right;">
                        <asp:ImageButton ID="lbCloseTrack" runat="server" ImageUrl="~/Images/crystaltoolbar/close.png"
                            Width="25px" />
                        <asp:HiddenField ID="lbCloseTrack1" runat="server" />
                    </div>
                    <div style="width: 35%; float: left; margin-top: 15px; min-height: 200px; max-height: 550px;
                        margin-bottom: 20px; overflow: auto;">
                        <table style="font-family: Calibri, Arial,Monaco, Consolas, monospace;">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    <asp:Image ID="imgMail1" runat="server" ImageUrl="~/Images/Mail2.png" Width="25px" />
                                    <asp:Label ID="lblEmailCodePopup" runat="server" Font-Names="Calibri, Arial,Monaco, Consolas, monospace"
                                        Font-Size="14px" Text="" Font-Bold="True"></asp:Label>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblHotelNamePopup" runat="server" Text="" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <div style="height: 2px; background-color: #E5E5E5; width: 100%;">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Update Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUpdateType" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="UpadateTypeAutoCompleteExtender" runat="server" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                        EnableCaching="false" Enabled="True" FirstRowSelected="true" MinimumPrefixLength="-1"
                                        OnClientItemSelected="UpdateTypeautocompleteselected" ServiceMethod="GetUpdateType"
                                        ServicePath="" TargetControlID="txtUpdateType">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox Style="display: none" ID="txtUpdateTypeCode" class="field_input" type="text"
                                        runat="server"></asp:TextBox>
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
                                    <asp:Label ID="Label5" runat="server" Text="Valid From"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValidFrom" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtValidFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgValidFrom" PopupPosition="Right" TargetControlID="txtValidFrom">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="txtValidFromDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtValidFrom">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgValidFrom" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:MaskedEditValidator ID="MevtxtValidFromDate" runat="server" ControlExtender="txtValidFromDate_MaskedEditExtender"
                                        ControlToValidate="txtValidFrom" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="txtValidFromDate_MaskedEditExtender"
                                        InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
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
                                    <asp:Label ID="Label6" runat="server" Text="Valid To"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValidTo" Width="100px" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtValidTo_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgtxtValidTo" PopupPosition="Right" TargetControlID="txtValidTo">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="txtValidTo_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtValidTo">
                                    </cc1:MaskedEditExtender>
                                    <asp:ImageButton ID="ImgtxtValidTo" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                    <cc1:MaskedEditValidator ID="MevValidToDate" runat="server" ControlExtender="txtValidTo_MaskedEditExtender"
                                        ControlToValidate="txtValidTo" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                        EmptyValueMessage="Date is required" ErrorMessage="txtValidTo_MaskedEditExtender"
                                       InvalidValueMessage="Invalid Date"
                                        TooltipMessage="Input a Date in Date/Month/Year">
                                    </cc1:MaskedEditValidator>
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
                                    <asp:Label ID="Label7" runat="server" Text="Assigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAssignedDate" Width="100px" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtAssignedDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgAssignedDate" PopupPosition="Right" TargetControlID="txtAssignedDate">
                                    </cc1:CalendarExtender>
                                    <asp:CheckBox ID="chkAssign" runat="server" />
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
                                    <asp:Label ID="Label8" runat="server" Text="Assigned To"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAssignedTo" Width="100px" runat="server"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="txtAssignedTo_AutoCompleteExtender" runat="server"
                                        DelimiterCharacters="" Enabled="True" ServicePath="" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" EnableCaching="false"
                                        FirstRowSelected="true" MinimumPrefixLength="-1" OnClientItemSelected="AssignedToautocompleteselected"
                                        ServiceMethod="GetAssignedTo" TargetControlID="txtAssignedTo">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox Style="display: none" ID="txtAssignedToCode" class="field_input" type="text"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkClarified" runat="server" Text="Clarified" AutoPostBack="True" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr2" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Reassigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReAssignedDate" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtAssignedDate0_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgAssignedDate" PopupPosition="Right" TargetControlID="txtReAssignedDate">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr3" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Reassigned To"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReAssignedTo" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="txtReAssignedTo_AutoCompleteExtender" runat="server"
                                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                        FirstRowSelected="true" MinimumPrefixLength="-1" OnClientItemSelected="ReAssignedToautocompleteselected"
                                        ServiceMethod="GetAssignedTo" ServicePath="" TargetControlID="txtReAssignedTo">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox Style="display: none" ID="txtReAssignedToCode" class="field_input" type="text"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr4" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Comments"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                               <tr  id="trApprovalAssdate" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Approval Assigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprovalAssDateTime" Width="100px" runat="server"></asp:TextBox>
                                    <cc1:CalendarExtender ID="ceApprovalAssDateTime" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgAssignedDate" PopupPosition="Right" TargetControlID="txtApprovalAssDateTime">
                                    </cc1:CalendarExtender>
                                    <asp:CheckBox ID="chkApprovalTime" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr >
                            <tr id="trApproverAss" runat="server">
                                <td>
                                    &nbsp; 
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Approver"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprover" Width="100px" runat="server"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="aeApprover" runat="server"
                                        DelimiterCharacters="" Enabled="True" ServicePath="" CompletionInterval="10"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" EnableCaching="false"
                                        FirstRowSelected="true" MinimumPrefixLength="-1" OnClientItemSelected="ApproverAutocompleteselected"
                                        ServiceMethod="GetApprover" TargetControlID="txtApprover">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox  ID="txtApproverCode" Style="display: none"  class="field_input" type="text"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr7" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkApprovalClarify" runat="server" Text="Approval Clarified" AutoPostBack="True" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr8" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="Approval Reassigned Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprovalReassignTime" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="ceApprovalReassignTime" runat="server" Format="dd/MM/yyyy"
                                        PopupButtonID="ImgAssignedDate" PopupPosition="Right" TargetControlID="txtApprovalReassignTime">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr9" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="Reapprover"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReApprover" runat="server" Width="100px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="aeReApprover" runat="server"
                                        CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                        FirstRowSelected="true" MinimumPrefixLength="-1" OnClientItemSelected="ReApproverAutocompleteselected"
                                        ServiceMethod="GetReApprover" ServicePath="" TargetControlID="txtReApprover">
                                    </cc1:AutoCompleteExtender>
                                    <asp:TextBox ID="txtReApproverCode" Style="display: none"  class="field_input" type="text"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr id="tr10" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Comments"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReapproveComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                         
                            <tr id="tr5">
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
                                </td>
                                <td>
                                    <asp:Button ID="btnUpdatePopup" runat="server" CssClass="btn" Text="Update" />
                                    &nbsp;<asp:Button ID="btnCancelPopup" runat="server" CssClass="btn" Text="Cancel" />
                                </td>
                                <td>
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
                                <td colspan="4">
                                    &nbsp;
                                    <div style="width: 100%; vertical-align: middle; max-height: 200px; overflow: auto;">
                                        <asp:GridView ID="gvClarify" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                            CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                            TabIndex="12" Width="90%">
                                            <Columns>
                                                <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                                <asp:BoundField DataField="ProgressStatus" HeaderText="Progress Status" />
                                                <asp:BoundField DataField="LogDate" HeaderText="Date &amp; Time" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                            </Columns>
                                            <FooterStyle CssClass="grdfooter" />
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                    </div>
                                    &nbsp;
                                            <div style="width: 100%; vertical-align: middle; max-height: 200px; overflow: auto;">
                                        <asp:GridView ID="gvClarifyForApproval" runat="server" AutoGenerateColumns="False" 
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                            CellPadding="3" CssClass="td_cell" Font-Size="10px" GridLines="Vertical" 
                                            TabIndex="12" Width="90%">
                                            <Columns>
                                                <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                                <asp:BoundField DataField="ProgressStatus" HeaderText="Progress Status" />
                                                <asp:BoundField DataField="LogDate" HeaderText="Date &amp; Time" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                            </Columns>
                                            <FooterStyle CssClass="grdfooter" />
                                            <RowStyle CssClass="grdRowstyle" />
                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="grdheader" ForeColor="white" />
                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td colspan="2">
                                    <asp:Label ID="Label15" runat="server" Font-Bold="true" Text="Additional Emails"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div style="height: 2px; background-color: #E5E5E5; width: 100%;">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td colspan="2">
                                    <asp:DataList ID="dlAdditionalEmails" runat="server" RepeatColumns="1">
                                        <ItemTemplate>
                                            <table class="style1">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lbAdditionalEmails" runat="server" 
                                                            onclick="lbAdditionalEmails_Click" Text='<%# Eval("AdditionalEmailIdName") %>'></asp:LinkButton>
                                                        <asp:HiddenField ID="hdAdditionalEmails" runat="server" 
                                                            Value='<%# Eval("AdditionalEmailId") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdPopupHotelCode" runat="server" />
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
                                    <asp:HiddenField ID="hdPopupMailId" runat="server" />
                                    <asp:HiddenField ID="hdTrackpopupStatus" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 64%; float: left; margin-top: 15px; min-height: 200px; max-height: 550px;
                        margin-bottom: 20px; overflow: auto;">
                        <div style="border-left: 2px solid #E5E5E5; min-height: 180px;">
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
                                                                Font-Underline="False" ForeColor="#333333" Text='<%# Eval("FileName") %>' OnClick="lbAttachmentPopup_Click"></asp:LinkButton>
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
                                            <asp:Label ID="lblBodyPopup" runat="server" Font-Names="Arial"></asp:Label></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <br />
            </asp:Panel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="HiddenField2" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:HiddenField ID="hdEmailCode" runat="server" />
    <asp:HiddenField ID="hdEmailCodeIgnore" runat="server" />
    <asp:HiddenField ID="hdPopupId" runat="server" />
    <asp:UpdatePanel ID="upnl" runat="server" ><ContentTemplate>

    <asp:Button ID="btnProcessclickMail" runat="server" Style="display: none" />
        <asp:Button ID="btnProcessIgnoreclickMail" runat="server" Style="display: none" />
        </ContentTemplate> </asp:UpdatePanel>
    <div id="div1">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
                <asp:ServiceReference Path="~/clsVisualSearchService.asmx" />
            </Services>
        </asp:ScriptManagerProxy>
    </div>
</asp:Content>

