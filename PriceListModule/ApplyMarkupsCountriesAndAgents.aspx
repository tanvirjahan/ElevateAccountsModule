<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" 
    CodeFile="ApplyMarkupsCountriesAndAgents.aspx.vb" Inherits="ApplyMarkupsCountriesAndAgents" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Src="Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
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
     <script type="text/javascript" charset="utf-8">
         $(document).ready(function () {

             visualsearchbox();
             // txtNameAutoCompleteExtenderKeyUp();
             visualsearch();
             visualsearchbar();
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
             $txtvsprocess.val('CountryGroup:"          " Region:"          " Country:"         " CustomerGroup:"          " Text:"          "');
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
                                 var asSqlqry = 'select ltrim(rtrim(countrygroupname)) countrygroupname  from countrygroup where active=1';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'CustomerGroup':
                                 var asSqlqry = 'select ltrim(rtrim(customergroupname)) customergroupname from customergroup where active=1';
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
                { label: 'CustomerGroup', category: 'Category' },
              ]);
                     }
                 }
             });


         }


         function visualsearch() {
             var $txtvsprocess = $(document).find('.cs_txtvsprocess1');
             $txtvsprocess.val('Country:" " City:" " Sector:" " Category:" " HotelGroup:" " HotelChain:" " HotelStatus:" "' + '"Room Classification":" " Text:" "');
             window.visualSearch1 = VS.init({
                 container: $('#search_box_container1'),
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
                         var $query = $('.search_query1');

                         var $txtvsprocess = $(document).find('.cs_txtvsprocess1');
                         $txtvsprocess.val(visualSearch1.searchQuery.serialize());

                         var $txtvsprocesssplit = $(document).find('.cs_txtvsprocesssplit1');
                         $txtvsprocesssplit.val(visualSearch1.searchQuery.serializetosplit());

                         var btnvsprocess = document.getElementById("<%=btnvsprocess1.ClientID%>");
                         btnvsprocess.click(visualSearch1.searchQuery.serializetosplit());

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
                             case 'HotelGroup':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayHotelGroupVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'HotelChain':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayHotelChainVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'HotelStatus':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayHotelStatusVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'Room Classification':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayRoomClassificationVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
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
                { label: 'HotelGroup', category: 'Hotels' },
                 { label: 'HotelChain', category: 'Hotels' },
                    { label: 'HotelStatus', category: 'Hotels' },
                         { label: 'Room Classification', category: 'Hotels' },
              ]);
                     }
                 }
             });


         }


         function visualsearchbar() {
             var $txtvsprocess2 = $(document).find('.cs_txtvsprocess2');
             $txtvsprocess2.val('InventoryType:" "  Country:" " Agent:" " Text:" "');
             window.visualSearch2 = VS.init({
                 container: $('#search_box_container2'),
                 query: $txtvsprocess2.val(),
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
                         var $query = $('.search_query2');

                         var $txtvsprocess2 = $(document).find('.cs_txtvsprocess2');
                         $txtvsprocess2.val(visualSearch2.searchQuery.serialize());

                         var $txtvsprocesssplit2 = $(document).find('.cs_txtvsprocesssplit2');
                         $txtvsprocesssplit2.val(visualSearch2.searchQuery.serializetosplit());

                         var btnvsprocess2 = document.getElementById("<%=btnvsprocess2.ClientID%>");
                         btnvsprocess2.click(visualSearch2.searchQuery.serializetosplit());

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

                             case 'InventoryType':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayInventorytypesVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'Formula-Id':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayFormulaIDVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;

                             case 'RoomClassification':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayRoomClassificationVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'Country':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayCountryVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'Agent':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayCustomerVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                             case 'Hotel':
                                 var asSqlqry = '';
                                 glcallback = callback;
                                 //fnTestCallback();
                                 ColServices.clsHotelGroupServices.GetListOfArrayHotelsVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                 break;
                         }
                     },
                     facetMatches: function (callback) {
                         callback([
                { label: 'Text' },
                { label: 'InventoryType', category: 'ApplyMarkup' },
                { label: 'FormulaID', category: 'ApplyMarkup' },
                { label: 'RoomClassification', category: 'ApplyMarkup' },
                { label: 'Country', category: 'ApplyMarkup' },
                { label: 'Agent', category: 'ApplyMarkup' },
                { label: 'Hotel', category: 'ApplyMarkup' },
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
    <script language="javascript" type="text/javascript">

        function ShowProgess() {

            var ModalPopupDays = $find("ModalPopupDays");

            ModalPopupDays.show();
            return true;
        }


        function ExhibitionAndHolidayDateSelectCalExt() {

            var grid = document.getElementById("<%=gvExhibitionAndHolidayInput.ClientID%>");
            var inputs = grid.getElementsByTagName("input");
            var txtToDate, txtfromDate;
            var calendarBehavior1;
            var calendarBehavior2;
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "text") {

                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayfromDate") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayfromDate") >= 0) {
                        txtfromDate = document.getElementById(inputs[i].id); // inputs[i];
                    }

                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayToDate") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayToDate") >= 0) {
                        txtToDate = document.getElementById(inputs[i].id); // inputs[i];
                    }

                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayfromDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayfromDate_CalendarExtender") >= 0) {
                        calendarBehavior1 = inputs[i];
                    }
                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayToDate_CalendarExtender") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayToDate_CalendarExtender") >= 0) {
                        calendarBehavior2 = inputs[i];
                    }
                }
            }

        }


        function ValidateChkInDate() {

            var grid = document.getElementById("<%=gvExhibitionAndHolidayInput.ClientID%>");
            var inputs = grid.getElementsByTagName("input");
            var txtToDate, txtfromDate;
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "text") {

                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayfromDate") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayfromDate") >= 0) {
                        txtfromDate = document.getElementById(inputs[i].id); // inputs[i];
                    }
                    if (inputs[i].name.indexOf("$txtExhibitionAndHolidayToDate") >= 0 || inputs[i].id.indexOf("$txtExhibitionAndHolidayToDate") >= 0) {
                        txtToDate = document.getElementById(inputs[i].id); // inputs[i];
                    }
                }
            }

            //            if (txtfromDate.value == null || txtfromDate.value == "") {
            //                txtToDate.value = "";
            //                alert("Please select From date.");
            //            }

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

        function getFormatedDate(chkdate) {
            var dd = chkdate.getDate();
            var mm = chkdate.getMonth() + 1; //January is 0!
            var yyyy = chkdate.getFullYear();
            if (dd < 10) { dd = '0' + dd };
            if (mm < 10) { mm = '0' + mm };
            chkdate = mm + '/' + dd + '/' + yyyy;
            return chkdate;
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
            var gridView = document.getElementById("<%=gvExhibitionAndHolidayInput.ClientID %>"); // *********** Change gridview name
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
                var txtFrm = CurrentRow.cells[0].getElementsByTagName("input")[0];
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
            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            // return false;
        }
        function LastSelectRow(CurrentRow, RowIndex) {
            var row = document.getElementById(CurrentRow);
            SelectRow(row, RowIndex);

        }
    </script>
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
            // $txtvsprocess.val('Country:" " City:" " Sector:" " Category:" " HotelGroup:" " HotelChain:" " HotelStatus:" "' + '"Room Classification":" " Text:" "');
            $txtvsprocess.val('CountryGroup:"          " Region:"          " Country:"         " CustomerGroup:"          " Text:"          "');
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
                                var asSqlqry = 'select ltrim(rtrim(countrygroupname)) countrygroupname  from countrygroup where active=1';
                                glcallback = callback;
                                //fnTestCallback();
                                ColServices.clsServices.GetListOfArrayVisualSearch(asSqlqry, fnFillSearchVS, ErrorHandler, TimeOutHandler);
                                break;
                            case 'CustomerGroup':
                                var asSqlqry = 'select ltrim(rtrim(customergroupname)) customergroupname from customergroup where active=1';
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
                { label: 'CustomerGroup', category: 'Category' },
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
            //txtNameAutoCompleteExtenderKeyUp();
            visualsearchbox();           
            visualsearch();
            visualsearchbar();
          

        }
        function FormValidationMainDetail(state) {
            var btn = document.getElementById("<%=btnSave.ClientID%>");
            //            if (btn.value == 'Save') { if (confirm('Are you sure you want to save?') == false) return false; }
            //            if (btn.value == 'Update') { if (confirm('Are you sure you want to update?') == false) return false; }
            //            if (btn.value == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
        }

        function ConfirmMarkup(msg, btnsave) {
            var btnid = document.getElementById(btnsave);
            var btnhidden = document.getElementById("<%=btnhidden.ClientID%>");
            if (confirm(msg) == false) {
                var hdMissing = document.getElementById("<%=hdMissing.ClientID%>");
                hdMissing.value = '';
                btnhidden.click();
               
            }
            else {

                btnid.click();
            }
        }


    </script>
    <head>
        <style>
            .btnExample
            {
                color: #2D7C8A;
                background: #e7e7e7;
                font-weight: bold;
                border: 1px solid #2D7C8A;
                padding: 5px 5px;
            }
            
            .btnExample:hover, .btnExample:hover
            {
                color: #FFF;
                background: #2D7C8A;
                transition: all 500ms ease-in-out;
            }
            .btnExample.selected
            {
                color: #FFF;
                background: #2D7C8A;
                font-weight: bold;
                border: 1px solid #2D7C8A;
                padding: 5px 5px;
            }
            
            .btnExampleHold
            {
                color: #FFF;
                background: #2D7C8A;
                font-weight: bold;
                border: 1px solid #2D7C8A;
                padding: 5px 5px;
            }
        </style>
        <style>
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
        <style>
            .AutoExtender
            {
                font-family: Verdana, Helvetica, sans-serif;
                font-size: .8em;
                font-weight: normal;
                border: solid 1px #006699;
                line-height: 20px;
                padding: 10px;
                background-color: White;
                margin-left: 10px;
            }
            .AutoExtenderList
            {
                border-bottom: dotted 1px #006699;
                cursor: pointer;
                color: Maroon;
            }
            .AutoExtenderHighlight
            {
                color: White;
                background-color: #006699;
                cursor: pointer;
            }
            #divwidth
            {
                width: 150px !important;
            }
            #divwidth div
            {
                width: 150px !important;
            }
        </style>
    </head>
    <asp:UpdatePanel ID="upnlFull" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                font-family: Arial,Verdana, Geneva, ms sans serif; width: 97%; border-bottom: gray 2px solid;
                margin-left: 35px; vertical-align: top;">
                <tr>
                    <td style="height: 15px" class="field_heading" align="center" width="10">
                        <asp:Label ID="lblHeading" runat="server" Text="Apply Markups" Width="738px" CssClass=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--      <asp:UpdatePanel ID="upnlTopMenu" runat="server">
                    <ContentTemplate>--%>
                        <div style="padding-left: 100px;">
                            <asp:Button ID="btnNew" runat="server" Text="New" CssClass="btnExample" />
                            &nbsp;<asp:Button ID="btnCancel_new" runat="server" CssClass="btnExample" 
                                Text="Cancel" />
                            &nbsp;<asp:Button ID="btnView" runat="server" CssClass="btnExample" Text="View" Visible="true" />
                            &nbsp;<asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnExample" Visible="False" />
                            &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnExample"
                                Visible="False" />
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btnAddHotel" runat="server" CssClass="btn" Font-Bold="False"
                                TabIndex="4" Text="Add New Hotel" Visible="False" />
                            &nbsp;<asp:Button ID="btnLog" runat="server" CssClass="btn" Font-Bold="False" TabIndex="4"
                                Text="Log" />
                            &nbsp;<asp:Button ID="btnPending" runat="server" CssClass="btn" Font-Bold="False"
                                TabIndex="4" Text="Pending for Approval" />
                            &nbsp;<asp:Button ID="btnHotelReport" runat="server" CssClass="btn" Font-Bold="False"
                                TabIndex="4" Text="Hotel Report" Visible="False" /></div>
                        <%--  </ContentTemplate>
                </asp:UpdatePanel>--%>
                    </td>
                </tr>
                             <tr>
    <td style="display:none">
            &nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="btnExample" />
    
       </td>
    </tr>
              
              
                <tr>
                    <td>
                        <asp:HiddenField ID="hdFillByGrid" runat="server" />
                        <asp:HiddenField ID="hdLinkButtonValue" runat="server" />
                        <asp:HiddenField ID="hdOPMode" runat="server" />
                        <asp:HiddenField ID="hdApplyMarkupId" runat="server" />
                        <asp:TextBox ID="txtNameNew" runat="server" Visible="False"></asp:TextBox>
                        <asp:HiddenField ID="hdMissing" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="dvNew" runat="server">
                            <table id="tblNew" runat="server">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="width: 100%">
                                            <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                                                <div id="VS" class="container" style="border: 0px;">
                                                    <div id="search_box_container">
                                                    </div>
                                                    <%-- Input Grid --%>
                                                    <asp:TextBox ID="txtvsprocess" runat="server" class="cs_txtvsprocess" Style="display: none"></asp:TextBox>
                                                    <asp:TextBox ID="txtvsprocesssplit" runat="server" class="cs_txtvsprocesssplit" Style="display: none"></asp:TextBox>
                                                    <%-- Show Grid --%>
                                                    <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                </div>
                                            </div>
                                            <div style="width: 18%; display: inline-block; vertical-align: top;">
                                                <asp:Button ID="btnResetSelection" runat="server" CssClass="btn" Font-Bold="False"
                                                    TabIndex="4" Text="Reset Search" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--    <asp:UpdatePanel runat="server" ID="upnlDL">
                    <ContentTemplate>--%>
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
                                                                OnClick="lnkCodeAndValue_Click1" Font-Size="Small" ForeColor="#000099" Text='<%# Eval("CodeAndValue") %>' />
                                                            <asp:Button ID="lbClose1" runat="server" class="buttonClose button4" OnClick="lbCloseSearch_Click"
                                                                Text="X" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--   <asp:UpdatePanel runat="server" class="fiel_input" ID="upnl_ucMarkup">
                    <ContentTemplate>--%>
                                        <uc1:Countrygroup ID="ucMarkup" runat="server" />
                                        <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <asp:UpdatePanel ID="upnlExhibition" runat="server">
                    <ContentTemplate>--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table width="450px" class="fiel_input">
                                                        <tr>
                                                            <td class="field_heading" colspan="4">
                                                                <asp:Label ID="Label11" runat="server" Text="Markup Dates"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <%-- Input Grid --%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <%-- Show Grid --%>
                                                                <asp:GridView ID="gvExhibitionAndHolidayInput" runat="server" AutoGenerateColumns="False"
                                                                    Width="449px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                                                    CellPadding="3">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="From Date">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label13" runat="server" Text="From Date"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtExhibitionAndHolidayfromDate" runat="server" CssClass="fiel_input"
                                                                                    onchange="ValidateChkInDate();" Width="80px" Text='<%# Eval("FromDate") %>'></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="txtExhibitionAndHolidayfromDate_CalendarExtender" runat="server"
                                                                                    Format="dd/MM/yyyy" OnClientDateSelectionChanged="ExhibitionAndHolidayDateSelectCalExt"
                                                                                    PopupButtonID="ImgExhibitionAndHolidayBtnFrmDt" PopupPosition="Right" TargetControlID="txtExhibitionAndHolidayfromDate">
                                                                                </cc1:CalendarExtender>
                                                                                <cc1:MaskedEditExtender ID="txtExhibitionAndHolidayFromDate_MaskedEditExtender" runat="server"
                                                                                    Mask="99/99/9999" MaskType="Date" TargetControlID="txtExhibitionAndHolidayfromDate">
                                                                                </cc1:MaskedEditExtender>
                                                                                <asp:ImageButton ID="ImgExhibitionAndHolidayBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                                    TabIndex="-1" />
                                                                                <cc1:MaskedEditValidator ID="MevExhibitionAndHolidayFromDate" runat="server" ControlExtender="txtExhibitionAndHolidayFromDate_MaskedEditExtender"
                                                                                    ControlToValidate="txtExhibitionAndHolidayfromDate" CssClass="field_error" Display="Dynamic"
                                                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                                    ErrorMessage="txtExhibitionAndHolidayFromDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="*"></cc1:MaskedEditValidator>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Date">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label8" runat="server" Text="To Date"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtExhibitionAndHolidayToDate" runat="server" CssClass="fiel_input"
                                                                                    onchange="ValidateChkInDate();" Width="80px" Text='<%# Eval("ToDate") %>'></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="txtExhibitionAndHolidayToDate_CalendarExtender" runat="server"
                                                                                    Format="dd/MM/yyyy" OnClientDateSelectionChanged="ExhibitionAndHolidayDateSelectCalExt"
                                                                                    PopupButtonID="ImgExhibitionAndHolidayBtnTomDt" PopupPosition="Right" TargetControlID="txtExhibitionAndHolidayToDate">
                                                                                </cc1:CalendarExtender>
                                                                                <cc1:MaskedEditExtender ID="txtExhibitionAndHolidayToDate_MaskedEditExtender" runat="server"
                                                                                    Mask="99/99/9999" MaskType="Date" TargetControlID="txtExhibitionAndHolidayToDate">
                                                                                </cc1:MaskedEditExtender>
                                                                                <asp:ImageButton ID="ImgExhibitionAndHolidayBtnTomDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                                    TabIndex="-1" />
                                                                                <cc1:MaskedEditValidator ID="MevExhibitionAndHolidaytoDate" runat="server" ControlExtender="txtExhibitionAndHolidayToDate_MaskedEditExtender"
                                                                                    ControlToValidate="txtExhibitionAndHolidayToDate" CssClass="field_error" Display="Dynamic"
                                                                                    EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required"
                                                                                    ErrorMessage="txtExhibitionAndHolidayToDate_MaskedEditExtender" InvalidValueBlurredMessage="Invalid Date"
                                                                                    InvalidValueMessage="Invalid Date" TooltipMessage="*">
                                                                                </cc1:MaskedEditValidator>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="btnAddRowGvS" runat="server" OnClick="btnAddRowGVs_Click" Text="Add Row" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                    <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                    <RowStyle ForeColor="#000066" />
                                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                                </asp:GridView>
                                                                <%-- End Show Grid --%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <%-- Show Grid --%><%-- End Show Grid --%>
                                                                <asp:Button ID="btnDeleteRow" runat="server" CssClass="btn" TabIndex="8" Text="Delete Row" />
                                                                &nbsp;<asp:Button ID="btnExhibition" runat="server" CssClass="btn" TabIndex="8" Text="Exhibition Calendar" />
                                                                <cc1:ModalPopupExtender ID="ModalExhibitionPopup" runat="server" BehaviorID="ModalExhibitionPopup"
                                                                    CancelControlID="imgExClose" TargetControlID="hdEx" PopupControlID="dvPopupExhibitionCalander"
                                                                    PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                                </cc1:ModalPopupExtender>
                                                                <asp:HiddenField ID="hdEx" runat="server" />
                                                                &nbsp;<asp:Button ID="btnHoliday" runat="server" CssClass="btn" TabIndex="8" Text="Holiday Calendar" />
                                                                <cc1:ModalPopupExtender ID="ModalHolidayPopup" runat="server" BehaviorID="ModalHolidayPopup"
                                                                    CancelControlID="TdClose" TargetControlID="hdHoliday" PopupControlID="dvPopupHoliday"
                                                                    PopupDragHandleControlID="HPopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                                </cc1:ModalPopupExtender>
                                                                <asp:HiddenField ID="hdHoliday" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" valign="top">
                                                                <div id="dvPopupExhibitionCalander" runat="server" style="overflow: auto; height: 450px;
                                                                    display: none; width: 550px; border: 3px solid #06788B; background-color: White;">
                                                                    <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                                        <tr>
                                                                            <td id="PopupHeader" bgcolor="#06788B">
                                                                                <asp:Label ID="Label14" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                                    Text="Exhibitions" Width="105px"></asp:Label>
                                                                            </td>
                                                                            <td align="center" id="imgExClose" bgcolor="#06788B">
                                                                                <asp:Label ID="lblcls" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                    Font-Size="Large" ForeColor="White"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div style="height: 350px; overflow: auto;">
                                                                                    <asp:GridView ID="gvExhibition" Width="100%" runat="server" AutoGenerateColumns="False"
                                                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                                                                        CellPadding="3">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Exhibition Name" DataField="exhibitionname" />
                                                                                            <asp:TemplateField HeaderText="From Date">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Bind("fromdate") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="To Date">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblToDate" runat="server" Text='<%# Bind("todate") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                        <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                                        <RowStyle ForeColor="#000066" />
                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                                                    </asp:GridView>
                                                                                </div>
                                                                                <asp:CheckBox ID="CheckBox2" Visible="false" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="2" style="padding-top: 5px">
                                                                                <asp:Button ID="btnFillExhibitions" runat="server" CssClass="btn" TabIndex="8" Text="Fill Selected Exhibitions" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="dvPopupHoliday" runat="server" style="overflow: auto; height: 450px; display: none;
                                                                    width: 550px; border: 3px solid #06788B; background-color: White;">
                                                                    <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                                        <tr>
                                                                            <td id="HPopupHeader" bgcolor="#06788B">
                                                                                <asp:Label ID="Label1" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                                    Text="Holiday Calendar" Width="205px"></asp:Label>
                                                                            </td>
                                                                            <td align="center" id="TdClose" bgcolor="#06788B">
                                                                                <asp:Label ID="Label2" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                                    Font-Size="Large" ForeColor="White"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div style="height: 350px; overflow: auto;">
                                                                                    <asp:GridView ID="gvHoliday" Width="100%" runat="server" AutoGenerateColumns="False"
                                                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                                                                        CellPadding="3">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="chkHSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkHSelectAll_CheckedChanged" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkHSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkHSelect_CheckedChanged" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField HeaderText="Holiday Name" DataField="holidayname" />
                                                                                            <asp:TemplateField HeaderText="From Date">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# Bind("fromdate") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="To Date">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblToDate" runat="server" Text='<%# Bind("todate") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                        <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                                        <RowStyle ForeColor="#000066" />
                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="2" style="padding-top: 5px">
                                                                                <asp:Button ID="btnHFillDates" runat="server" CssClass="btn" TabIndex="8" Text="Fill Selected Exhibitions" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdCurrentDate" runat="server" />
                                                                <asp:Button ID="btnhidden" runat="server" CssClass="field_button" Style="display: none" 
                                                                    Text="" Width="150px" />
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
                                                <td align="left" class="fiel_input" style="padding-top: 77px; padding-left: 5px;">
                                                    <asp:GridView ID="gvWeekOfDays" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkWSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkWSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkWSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkWSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Days of Week">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWeekDays" runat="server" Text='<%# Eval("DaysOfWeek") %>'></asp:Label>
                                                                    <asp:Label ID="lblWeekDaysCode" Visible="false" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    </asp:GridView>
                                                </td>
                                                <td align="left" class="fiel_input" style="padding-top: 27px; padding-left: 5px;">
                                                    <asp:GridView ID="gvInventoryType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkInvSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkInvSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkInvSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkInvSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Inventory Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInventoryType" runat="server" Text='<%# Eval("InventoryType") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#2D7C8A" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--  </ContentTemplate>
                </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <asp:UpdatePanel runat="server" ID="upnlSelectMarkup"><ContentTemplate>--%>
                                        <table class="fiel_input">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSelectMarkup" runat="server" CssClass="btn" TabIndex="8" Text="Select Markup" />
                                                    <cc1:ModalPopupExtender ID="ModalSelectMarkup" runat="server" BehaviorID="ModalSelectMarkup"
                                                        CancelControlID="TdMarkupClose" TargetControlID="hdMarkUp" PopupControlID="dvMarkupPopup"
                                                        PopupDragHandleControlID="PopupMarkupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:HiddenField ID="hdMarkUp" runat="server" />
                                                    <div id="dvMarkupPopup" runat="server" style="height: 450px; width: 650px; border: 3px solid #06788B;
                                                        background-color: White;">
                                                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                            <tr>
                                                                <td id="PopupMarkupHeader" bgcolor="#06788B">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                        Text="Markup Formulas" Width="205px"></asp:Label>
                                                                </td>
                                                                <td align="center" id="TdMarkupClose" bgcolor="#06788B">
                                                                    <asp:Label ID="Label4" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div style="height: 350px; overflow: auto;">
                                                                        <asp:GridView ID="gvMarkupFormulas" TabIndex="9" runat="server" Font-Size="10px" PageSize="10"
                                                                            Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                                            BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
                                                                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Select">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkmarkupSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkmarkupSelect_CheckedChanged" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Formula ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCode" runat="server" Text='<%# Bind("FormulaID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Formula Name" SortExpression="FormulaName">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFormulaName" runat="server" Text='<%# Bind("FormulaName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ControlStyle />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Mark-Up Formula" SortExpression="MarkupFormula">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFormula" runat="server" Text='<%# Bind("MarkupFormula") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ControlStyle />
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField HeaderText="Currency" DataField="currname" />
                                                                            </Columns>
                                                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" style="padding-top: 5px">
                                                                    &nbsp;
                                                                    <asp:Button ID="btnSelectedMarkup" runat="server" CssClass="btn" Text="Select Markup" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnAddNewMarkup" runat="server" CssClass="btn" Text="Add New Markup" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" Text="Selected Markup Formula Name: "></asp:Label>
                                                    &nbsp;<asp:TextBox ID="txtMarkupFormulaName" runat="server" ReadOnly="True" Width="600px"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <asp:TextBox ID="txtmarkupCode" runat="server" ReadOnly="True" Visible="false" Width="600px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" Text="Selected Markup Formula            :"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtMarkupFormula" runat="server" ReadOnly="True" Width="600px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                       <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Applicable To           :"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtApplicableTo" runat="server" Width="600px" 
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--  </ContentTemplate></asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                             
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--     <asp:UpdatePanel runat="server" ID="UpdatePanel1"><ContentTemplate>--%>
                                        <%--    </ContentTemplate></asp:UpdatePanel>--%>
                                        <asp:CheckBox ID="chkActive" runat="server" Text=" Approve" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         <cc1:ModalPopupExtender ID="modalDuplication" runat="server" BehaviorID="modalDuplication"
                                                        CancelControlID="tdDuplicationClose" TargetControlID="hdDuplication" PopupControlID="dvDuplication"
                                                        PopupDragHandleControlID="tdDuplHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:HiddenField ID="hdDuplication" runat="server" />
                                                    <div id="dvDuplication" runat="server" style="height: 350px; width: 850px; border: 3px solid #06788B;
                                                        background-color: White;">
                                                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                            <tr>
                                                                <td id="tdDuplHeader" bgcolor="#06788B">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                        Text="Existing Applied Markups for countries or Agents" Width="355px"></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdDuplicationClose" bgcolor="#06788B">
                                                                    <asp:Label ID="Label7" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div style="height: 350px; overflow: auto;">
                                                                        <asp:GridView ID="gvDuplication" TabIndex="9" runat="server" Font-Size="10px"
                                                                            Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                                            BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false">
                                                                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                            <Columns>
                                                                            
                                                                                <asp:TemplateField HeaderText="Apply Markup ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApplyMarkupId" runat="server" Text='<%# Bind("ApplyMarkupId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                   
                                                                                <asp:BoundField HeaderText="From Date" DataField="From Date" />
                                                                                <asp:BoundField HeaderText="To Date" DataField="To Date" />
                                                                                <asp:BoundField HeaderText="Existing" DataField="type" />
                                                                                 <asp:BoundField HeaderText="Formula Id" DataField="FormulaId" />
                                                                                  <asp:BoundField HeaderText="Formula String" DataField="FormulaString" />
                                                                                   <asp:BoundField HeaderText="Applicable To" DataField="ApplicableTo" />
                                                                                        <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                                                                                  <asp:BoundField HeaderText="Days of the Week" DataField="DaysOfTheWeek" />
                                                                            </Columns>
                                                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" style="padding-top: 5px">
                                                                 
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCheckExist" runat="server" CssClass="btn" TabIndex="8"   OnClientClick="ShowProgess();"
                                            Text="View Existing" />&nbsp;<asp:Button ID="btnSave" runat="server"  OnClientClick="ShowProgess();"
                                            CssClass="btn" TabIndex="8" Text="Save" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnhelp" runat="server" CssClass="btn" OnClick="btnhelp_Click" TabIndex="10"
                                            Text="Help" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click"
                                            TabIndex="9" Text="Return To Search" Visible="False" Width="196px" />
                                        <asp:Label ID="lblwebserviceerror" runat="server" Style="display: none" Text="Webserviceerror"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                     <div id="dvSearch" runat="server" class="fiel_input">
                            <table width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                  <tr>
                                                <td>
                                                    <div style="width: 100%">
                                                        <div style="width: 80%; display: inline-block; margin: -6px 4px 0 0;">
                                                            <div id="Div2" class="container" style="border: 0px;">
                                                                <div id="search_box_container2">
                                                                </div>
                                                                <%-- Input Grid --%>
                                                                <asp:TextBox ID="txtvsprocess2" runat="server" class="cs_txtvsprocess2" Style="display: none"></asp:TextBox>
                                                                <asp:TextBox ID="txtvsprocesssplit2" runat="server" class="cs_txtvsprocesssplit2"
                                                                    Style="display: none"></asp:TextBox>
                                                                <%-- Show Grid --%>
                                                                <asp:Button ID="btnvsprocess1" runat="server" Style="display: none" />
                                                                <asp:Button ID="btnvsprocess2" runat="server" Style="display: none" />
                                                            </div>
                                                        </div>
                                                        <div style="width: 18%; display: inline-block; vertical-align: top;">
                                                            <asp:Button ID="btnResetSelection2" runat="server" CssClass="btn" Font-Bold="False"
                                                                TabIndex="4" Text="Reset Search" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="fiel_input" align="left">
                                                    <asp:DataList ID="dlList2" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="lnkCode" runat="server" class="button button4" 
                                    style="display:none" text='<%# Eval("Code") %>' />
                                <asp:Button ID="lnkValue" runat="server" class="button button4" 
                                    style="display:none" text='<%# Eval("Value") %>' />
                                <asp:Button ID="lnkCodeAndValue" runat="server" class="button button4" 
                                    Font-Bold="False" Font-Size="Small" ForeColor="#000099" 
                                    OnClientClick="return false;" text='<%# Eval("CodeAndValue") %>' />
                                <asp:Button ID="lbClose2" runat="server" class="buttonClose button4" 
                                    onclick="lbClose2_Click" Text="X" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                               
                                <tr>
                                <td>
                                </td>
                                <td></td>
                                </tr>


                                <tr>


                            <td>

                            <table>
                            <tr>
                            <td>&nbsp;&nbsp;
                            </td>
           <td>
         <asp:Label ID="lblfrmdate" runat="server" CssClass="field_caption" Text="From Date"></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" onchange="filltodate(this);" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnFrmDt" 
                            PopupPosition="Right" TargetControlID="txtFromDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtFromDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MevFromDate" runat="server" 
                            ControlExtender="MeFromDate" ControlToValidate="txtFromDate" 
                            CssClass="field_error" Display="Dynamic" 
                            EmptyValueBlurredText="Date is required" EmptyValueMessage="Date is required" 
                            ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date" 
                            InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                    </td>
                    <td>
                        <asp:Label ID="lbltodate" runat="server" CssClass="field_caption" Text="To Date"></asp:Label>
                    
                   
                        <asp:TextBox ID="txtToDate" runat="server" onchange="ValidateChkInDate();" 
                            Width="75px"></asp:TextBox>
                        <asp:ImageButton ID="ImgBtnToDt" runat="server" 
                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3" />
                        <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" 
                            OnClientDateSelectionChanged="DateSelectCalExt" PopupButtonID="ImgBtnToDt" 
                            PopupPosition="Right" TargetControlID="txtToDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtToDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MevToDate" runat="server" 
                            ControlExtender="MeToDate" ControlToValidate="txtToDate" CssClass="field_error" 
                            Display="Dynamic" EmptyValueBlurredText="Date is required" 
                            EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" 
                            InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date" 
                            TooltipMessage="Input a Date in Date/Month/Year"></cc1:MaskedEditValidator>
                    </td>


                    <td>
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Search by Date" />
                        &nbsp;<asp:Button ID="btnClearDate" runat="server" CssClass="btn" Font-Bold="False" 
                            tabIndex="4" Text="Reset Dates" />
                             </td>      
                                                  <td>  <asp:Label ID="RowSelectcs" runat="server" CssClass="field_caption" 
                            Text="Rows Selected "></asp:Label>
                     <asp:DropDownList ID="RowsPerPageCS" runat="server" AutoPostBack="true">
                      
                          <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                             <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                             <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                        </asp:DropDownList>
                    </td>           </tr>   
                            </table>
                    </td>
                                </tr>
                               <%-- <tr>
                                <td>
                                &nbsp;</td>
                                </tr>
                                <tr>
                        <td style="width: 100%;">
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="6" />
                            &nbsp;</td>
                    </tr>
                                <tr>

                                    <td>
                                 

                                        <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell" AllowPaging="true"
                                               Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ApplyMarkup Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApplyMarkupId" Text='<%# Bind("ApplyMarkupId") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Applicable To " DataField="ApplicableTo" />
                                                <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                                                <asp:BoundField HeaderText="Days Of the Week" DataField="DaysOfTheWeek" />
                                                <asp:BoundField HeaderText="Hotel Name" DataField="partyname" />
                                                <asp:BoundField HeaderText="Room Classification" DataField="RoomClassName" />
                                                <asp:BoundField HeaderText="MarkUp From Date"   DataFormatString="{0:dd/MM/yyyy}" DataField="MarkUpFromDate" />
                                                    <asp:BoundField HeaderText="MarkUp To Date" DataFormatString="{0:dd/MM/yyyy}"  DataField="MarkUpToDate" />
                                                <asp:BoundField HeaderText="Formula Id" DataField="FormulaId" />
                                                <asp:BoundField HeaderText="Formula String" DataField="FormulaString" />
                             
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                        </asp:GridView>
                                    
                                         <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" __designer:wfdid="w51" Visible="False"></asp:Label> 
                                    </td>
                                </tr>--%>

                     
                            </table>
                        </div>
                        <%--    <asp:UpdatePanel runat="server" ID="UpdatePanel2"><ContentTemplate>--%>
                        <div id="dvLog" runat="server" class="fiel_input">
                            <table width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                            Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ApplyMarkup Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApplyMarkupId" Text='<%# Bind("ApplyMarkupId") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Approve Status" DataField="ApprovedStatus" />
                                                <asp:BoundField HeaderText="Formula Id" DataField="FormulaId" />
                                                <asp:BoundField HeaderText="Formula Name" DataField="FormulaName" />
                                                <asp:BoundField HeaderText="Search Criteria" DataField="SearchCriteria" />
                                                <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                                                <asp:BoundField HeaderText="Days of the Week" DataField="DaysOfTheWeek" />
                                                    <asp:BoundField HeaderText="Applicable To" DataField="ApplicableTo" />
                                                <asp:BoundField HeaderText="Date Created" DataField="AddDate" />
                                                <asp:BoundField HeaderText="User Created" DataField="AddUser" />
                                                <asp:BoundField HeaderText="Date Modified" DataField="ModDate" />
                                                <asp:BoundField HeaderText="User Modified" DataField="ModUser" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbView" runat="server" OnClick="lbView_Click">View</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbCopy" runat="server" onclick="lbCopy_Click">Copy</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbEdit" runat="server" onclick="lbEdit_Click">Edit</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbDelete" runat="server" onclick="lbDelete_Click">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--  </ContentTemplate></asp:UpdatePanel>--%>
                        <%--   <asp:UpdatePanel runat="server" ID="UpdatePanel3"><ContentTemplate> --%>
                        <div id="dvPendingApproval" runat="server">
                            <table width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvPendingApproval" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                            Font-Size="10px" GridLines="Vertical" TabIndex="12" Width="100%">
                                            <FooterStyle CssClass="grdfooter" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ApplyMarkup Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApplyMarkupId" Text='<%# Bind("ApplyMarkupId") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Approve Status" DataField="ApprovedStatus" />
                                                <asp:BoundField HeaderText="Formula Id" DataField="FormulaId" />
                                                <asp:BoundField HeaderText="Formula Name" DataField="FormulaName" />
                                                <asp:BoundField HeaderText="Search Criteria" DataField="SearchCriteria" />
                                                <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                                                <asp:BoundField HeaderText="Days of the Week" DataField="DaysOfTheWeek" />
                                                    <asp:BoundField HeaderText="Applicable To" DataField="ApplicableTo" />
                                                <asp:BoundField HeaderText="Date Created" DataField="AddDate" />
                                                <asp:BoundField HeaderText="User Created" DataField="AddUser" />
                                                <asp:BoundField HeaderText="Date Modified" DataField="ModDate" />
                                                <asp:BoundField HeaderText="User Modified" DataField="ModUser" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbSelect" runat="server" OnClick="lbSelect_Click">Select</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbPcopy" runat="server" onclick="lbPcopy_Click">Copy</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbPEdit" runat="server" onclick="lbPEdit_Click">Edit</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbPDelete" runat="server" onclick="lbPDelete_Click">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <%--     </ContentTemplate></asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
             <center>
                            <div id="Loading1" runat="server" style="height: 150px; width: 500px;">
                                <img alt="" id="Image1" runat="server" src="~/Images/loader-progressbar.gif" width="200" />
                                <h2 style="color: #06788B">
                                    Processing please wait...</h2>
                            </div>
                        </center>
                        <asp:ModalPopupExtender ID="ModalPopupDays" runat="server" BehaviorID="ModalPopupDays"
                            TargetControlID="btnInvisibleGuest" CancelControlID="btnClose" PopupControlID="Loading1"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                         <input id="btnInvisibleGuest" runat="server" type="button" value="Cancel" style="display: none" />
                        <input id="btnClose" type="button" value="Cancel" style="display: none" />


                           <cc1:ModalPopupExtender ID="mpShowExistMarkup" runat="server" BehaviorID="mpShowExistMarkup"
                                                        CancelControlID="tdShowExistClose" TargetControlID="hdShowExistMarkup" PopupControlID="dvShowExistMarkup"
                                                        PopupDragHandleControlID="tdShowExistHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:HiddenField ID="hdShowExistMarkup" runat="server" />
                                                    <div id="dvShowExistMarkup" runat="server" style="height: 350px; width: 850px; border: 3px solid #06788B;
                                                        background-color: White;">
                                                        <table style="width: 99%; padding: 5px 5px 5px 5px">
                                                            <tr>
                                                                <td id="tdShowExistHeader" bgcolor="#06788B">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="field_heading" Style="padding: 3px 0px 3px 3px"
                                                                        Text="Existing Applied Markups for countries or Agents" Width="355px"></asp:Label>
                                                                </td>
                                                                <td align="center" id="tdShowExistClose" bgcolor="#06788B">
                                                                    <asp:Label ID="Label10" Text="X" runat="server" Font-Bold="True" Font-Names="Verdana"
                                                                        Font-Size="Large" ForeColor="White"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div style="height: 350px; overflow: auto;">
                                                                        <asp:GridView ID="gvShowExistMarkup" TabIndex="9" runat="server" Font-Size="10px"
                                                                            Width="100%" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                                            BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="false">
                                                                            <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                            <Columns>
                                                                            
                                                                                <asp:TemplateField HeaderText="Apply Markup ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApplyMarkupId" runat="server" Text='<%# Bind("ApplyMarkupId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                   
                                                                                <asp:BoundField HeaderText="From Date" DataField="FROM_DATE" />
                                                                                <asp:BoundField HeaderText="To Date" DataField="TO_DATE" />
                                                                                <asp:BoundField HeaderText="Countries" DataField="Countries" />
                                                                                 <asp:BoundField HeaderText="Agents" DataField="Agents" />
                                                                                        <asp:BoundField HeaderText="Inventory Types" DataField="InventoryTypes" />
                                                                                  <asp:BoundField HeaderText="Days of the Week" DataField="DaysOfTheWeek" />
                                                                            </Columns>
                                                                            <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                                            <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                                            <HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>
                                                                            <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" style="padding-top: 5px">
                                                                 
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;<div id="div1">
        <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/clsHotelGroupServices.asmx" />
                <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
            </Services>
        </asp:ScriptManagerProxy>
    </div>
    <script type="text/javascript">
        function SetContextKey() {


        }
    </script>
</asp:Content>
