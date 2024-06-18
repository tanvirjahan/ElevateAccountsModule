<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="ContractCommission.aspx.vb" Inherits="PriceListModule_ContractCommission" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="Countrygroup.ascx" TagName="Countrygroup" TagPrefix="uc2" %>
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

        var glcallback;

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ShowProgess() {

            var ModalPopupDays = $find("ModalPopupDays");

            ModalPopupDays.show();
            return true;
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

        function showusercontrol(chkctrygrpid) {
            var chkctrygrp = document.getElementById(chkctrygrpid);
            if (chkctrygrp.checked) {
                $("#" + "<%=divuser.ClientID %>").slideDown();
            }
            else {
                $("#" + "<%=divuser.ClientID %>").fadeOut();
            }
        }

        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
        }

        function AutoCompleteExtenderKeyUp() {

            $("#< %=gv_Filldata.ClientID%> tr input[id*='txtExhiname']").each(function () {
                $(this).change(function (event) {
                    var hiddenfieldID = $(this).attr("id").replace("txtExhiname", "txtExhicode");
                    $get(hiddenfieldID).value = '';
                });
            });
        }


        function promotionautocompleteselected(source, eventArgs) {

            //            if (eventArgs != null) {
            //                document.getElementById('< %=txtpromotionid.ClientID%>').value = eventArgs.get_value();
            //            }
            //            else {
            //                document.getElementById('< %=txtpromotionid.ClientID%>').value = '';
            //            }
        }


        function SetContextKey() {

            //            var hiddenfieldID = source.get_id().replace("txtnoofchild_AutoCompleteExtender", "txtrmtypcode");
            //            lroomcode = hiddenfieldID;
            //            $get(hiddenfieldID).value = eventArgs.get_value();

            //            var roomcode = eventArgs.get_value();
            //            alert(roomcode);

        }

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

        var ltxtcharge;
        function chargeautocompleteselected(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("AutoCompleteExtender2", "txtcharge");
            ltxtcharge = hiddenfieldID;
            $get(hiddenfieldID).value = eventArgs.get_value();

            var chargecode = eventArgs.get_value();



            var txtnightsid = ltxtcharge.replace("txtcharge", "txtnights");
            var txtperchargeid = ltxtcharge.replace("txtcharge", "txtpercharge");
            var txtvalueid = ltxtcharge.replace("txtcharge", "txtvalue");

            if (chargecode == 'Nights') {
                $get(txtnightsid).removeAttribute('disabled');
                $get(txtperchargeid).value = '';
                $get(txtvalueid).value = '';
                $get(txtperchargeid).setAttribute("disabled", false);
                $get(txtvalueid).setAttribute("disabled", false);
                return false;
            }
            else {
                $get(txtnightsid).setAttribute("disabled", false);
            }

            if (chargecode == '% of Nights') {

                $get(txtperchargeid).removeAttribute('disabled');
                $get(txtnightsid).removeAttribute('disabled');
                $get(txtvalueid).value = '';
                $get(txtvalueid).setAttribute("disabled", false);
                return false;
            }
            else {
                $get(txtperchargeid).setAttribute("disabled", false);

            }
            if (chargecode == '% of Entire Stay') {
                $get(txtperchargeid).removeAttribute('disabled');
                $get(txtnightsid).value = '';
                $get(txtvalueid).value = '';
                $get(txtvalueid).setAttribute("disabled", false);
                $get(txtnightsid).setAttribute("disabled", false);
                return false;
            }
            else {

                $get(txtperchargeid).setAttribute("disabled", false);
            }



            if (chargecode == 'Value') {
                $get(txtvalueid).removeAttribute('disabled');
                $get(txtnightsid).value = '';
                $get(txtperchargeid).value = '';
                $get(txtnightsid).setAttribute("disabled", false);
                $get(txtperchargeid).setAttribute("disabled", false);
                return false;
            }
            else {
                $get(txtvalueid).setAttribute("disabled", false);
            }


        }

        var ltxtnoshowcharge;
        function noshowchargeautocompleteselected(source, eventArgs) {


            var hiddenfieldID = source.get_id().replace("txtchargenoshow_AutoCompleteExtender", "txtchargenoshow");
            ltxtnoshowcharge = hiddenfieldID;
            $get(hiddenfieldID).value = eventArgs.get_value();

            var noshowchargecode = eventArgs.get_value();




            var txtnightsnoshowid = ltxtnoshowcharge.replace("txtchargenoshow", "txtnightsnoshow");
            var txtperchargenoshowid = ltxtnoshowcharge.replace("txtchargenoshow", "txtpercnoshow");
            var txtvaluenoshowid = ltxtnoshowcharge.replace("txtchargenoshow", "txtvaluenoshow");


            if (noshowchargecode == 'Value') {

                $get(txtvaluenoshowid).removeAttribute('disabled');

                $get(txtnightsnoshowid).value = '';
                $get(txtperchargenoshowid).value = '';
                $get(txtnightsnoshowid).setAttribute("disabled", false);
                $get(txtperchargenoshowid).setAttribute("disabled", false);
                return false;

            }
            else {
                $get(txtvaluenoshowid).setAttribute("disabled", false);
            }

            if (noshowchargecode == 'Nights') {
                $get(txtnightsnoshowid).removeAttribute('disabled');
                $get(txtvaluenoshowid).value = '';
                $get(txtperchargenoshowid).value = '';
                $get(txtperchargenoshowid).setAttribute("disabled", false);
                $get(txtvaluenoshowid).setAttribute("disabled", false);
                return false;
            }
            else {

                $get(txtnightsnoshowid).setAttribute("disabled", false);
            }

            if (noshowchargecode == '% of Nights') {
                $get(txtperchargenoshowid).removeAttribute('disabled');
                $get(txtnightsnoshowid).removeAttribute('disabled');
                $get(txtvaluenoshowid).value = '';
                $get(txtvaluenoshowid).setAttribute("disabled", false);
                return false;
            }
            else {

                $get(txtperchargenoshowid).setAttribute("disabled", false);
            }


            if (noshowchargecode == '% of Entire Stay') {
                $get(txtperchargenoshowid).removeAttribute('disabled');

                $get(txtvaluenoshowid).value = '';
                $get(txtnightsnoshowid).value = '';

                $get(txtnightsnoshowid).setAttribute("disabled", false);
                $get(txtvaluenoshowid).setAttribute("disabled", false);
                return false;

            }
            else {

                $get(txtperchargenoshowid).setAttribute("disabled", false);
            }

            if (noshowchargecode == '% of remaining Nights') {

                $get(txtperchargenoshowid).removeAttribute('disabled');

                $get(txtvaluenoshowid).value = '';
                $get(txtnightsnoshowid).value = '';

                $get(txtnightsnoshowid).setAttribute("disabled", false);
                $get(txtvaluenoshowid).setAttribute("disabled", false);
                return false;

            }
            else {

                $get(txtperchargenoshowid).setAttribute("disabled", false);
            }




        }



        function fillcombination(result) {
            //var objGridView = document.getElementById('<v %=gv_Filldata.ClientID%>');

            var txtfromDateID = ltxtexhicode.replace("txtExhicode", "txtfromDate");
            var txtToDateID = ltxtexhicode.replace("txtExhicode", "txtToDate");
            $get(txtfromDateID).value = result[0];
            $get(txtToDateID).value = result[1];
        }

        function CheckContract(contractid) {
            //var hdncontract = document.getElementById("< %=hdncontractid.ClientID%>");
            var hdncontract = document.getElementById(contractid);

            if ((hdncontract.value == '')) {
                alert('Please Save Contract Main details to continue');
                return false;
            }

        }

        function Checkcommission(promotionid,commissiontype) {

            var hdnpromotionid = document.getElementById(promotionid);

            if ((commissiontype != 'Special commissionable Rates')) {
                alert('Promotion Special commissionable Not Selected');
                return false;

            }


        }




    </script>
     <script language="javascript" type="text/javascript">



      
        function validateDigitOnly(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]/;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();
            }
        }

        function checkfromdates(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);

            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
            }



            var confromdate = document.getElementById('<%=hdnconfromdate.ClientID %>');
            var contodate = document.getElementById('<%=hdncontodate.ClientID %>');



            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


            var dp2 = confromdate.value.split("/");
            var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

            var dp3 = contodate.value.split("/");
            var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);



            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newcfdate = getFormatedDate(newcfdate);
            newctdate = getFormatedDate(newctdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);

            newcfdate = new Date(newcfdate);
            newctdate = new Date(newctdate);



            if (newfdate > newtdate) {
           
                alert("From date should not be greater than To date");
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }


            if (newfdate > newctdate || newfdate < newcfdate) {
               
                alert("From Date Should belongs to the Contracts Period - " + confromdate.value + " to " + contodate.value);
                setTimeout(function () { fdate.focus(); }, 1);
                fdate.value = "";
            }

            setdate();
        }

        function checkdates(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);


            var confromdate = document.getElementById('<%=hdnconfromdate.ClientID %>');
            var contodate = document.getElementById('<%=hdncontodate.ClientID %>');



            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
                setTimeout(function () { fdate.focus(); }, 1);
                fdate.value = "";
            }

            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


            var dp2 = confromdate.value.split("/");
            var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

            var dp3 = contodate.value.split("/");
            var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);

            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newcfdate = getFormatedDate(newcfdate);
            newctdate = getFormatedDate(newctdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);

            newcfdate = new Date(newcfdate);
            newctdate = new Date(newctdate);


            if (newtdate < newfdate) {
               
                alert("To date should  be greater than From date");
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }

            if (newtdate > newctdate) {
              
                alert("To Date Should belongs to the Contracts Period - " + confromdate.value + " to " + contodate.value);
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }

            setdate();
        }

        function checkdatespromo(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);


            var confromdate = document.getElementById('<%=hdnpromofrmdate.ClientID %>');
            var contodate = document.getElementById('<%=hdnpromotodate.ClientID %>');



            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
                setTimeout(function () { fdate.focus(); }, 1);
                fdate.value = "";
            }

            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


            var dp2 = confromdate.value.split("/");
            var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

            var dp3 = contodate.value.split("/");
            var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);

            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newcfdate = getFormatedDate(newcfdate);
            newctdate = getFormatedDate(newctdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);

            newcfdate = new Date(newcfdate);
            newctdate = new Date(newctdate);


            if (newtdate < newfdate) {

                alert("To date should  be greater than From date");
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }

            if (newtdate > newctdate) {

                alert("To Date Should belongs to the Promotions Period - " + confromdate.value + " to " + contodate.value);
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }

            setdate();
        }


        function checkfromdatespromo(txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);

            if (fdate.value == null || fdate.value == "") {
                alert("Please select from date.");
            }



            var confromdate = document.getElementById('<%=hdnpromofrmdate.ClientID %>');
            var contodate = document.getElementById('<%=hdnpromotodate.ClientID %>');



            var dp = fdate.value.split("/");
            var newfdate = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);

            var dp1 = tdate.value.split("/");
            var newtdate = new Date(dp1[2] + "/" + dp1[1] + "/" + dp1[0]);


            var dp2 = confromdate.value.split("/");
            var newcfdate = new Date(dp2[2] + "/" + dp2[1] + "/" + dp2[0]);

            var dp3 = contodate.value.split("/");
            var newctdate = new Date(dp3[2] + "/" + dp3[1] + "/" + dp3[0]);



            newfdate = getFormatedDate(newfdate);
            newtdate = getFormatedDate(newtdate);

            newcfdate = getFormatedDate(newcfdate);
            newctdate = getFormatedDate(newctdate);

            newfdate = new Date(newfdate);
            newtdate = new Date(newtdate);

            newcfdate = new Date(newcfdate);
            newctdate = new Date(newctdate);



            if (newfdate > newtdate) {

                alert("From date should not be greater than To date");
                setTimeout(function () { tdate.focus(); }, 1);
                tdate.value = "";
            }


//            if (newfdate > newctdate || newfdate < newcfdate) {

//                alert("From Date Should belongs to the Promotions Period - " + confromdate.value + " to " + contodate.value);
//                setTimeout(function () { fdate.focus(); }, 1);
//                fdate.value = "";
//            }

            setdate();
        }



        function FormValidationMainDetail(state) {
            //            var txtnameval = document.getElementById("< %=txtname.ClientID%>");
            //            if (txtnameval.value == '') {
            //                //            alert('Name Cannot be blank');
            //                //            return false;
            //            }
            //            else {
            //                if (state == 'New') { if (confirm('Are you sure you want to save Exhibition Supplements  ') == false) return false; }
            //                if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
            //                if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
            //            }
        }



        function formmodecheck() {
            var vartxtcode = document.getElementById(" <%=txthotelname.ClientID%>");


            if ((vartxtcode.value == '')) {
                doLinks(false);
            }
            else {
                doLinks(true);
            }


        }

        function doLinks(how) {
            for (var l = document.links, i = l.length - 1; i > -1; --i)
                if (!how)
                    l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
                else
                    l[i].onclick = function () { return true; };
        }
        function load() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
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



        function checkNumber(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
            if (charCode != 47 && (charCode > 44 && charCode < 58)) {
                return true;
            }
            return false;
        }





        function ChangeDate(FromId, ToId) {
            var ResultFrom = document.getElementById(FromId).value;
            var ResultTo = document.getElementById(ToId);
            ResultTo.value = ResultFrom;

        }


        function RadioCheck(rb) {
            var gv = document.getElementById("<%=grdcommission.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;

                        break;
                    }
                }
            }
        }


        function datefill() {
            $get("ctl00_Main_btngAlert").click();

        }


        $("[id*=chkAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }

            });

        });


        $("[id*=chkRmcatAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }

            });

        });


        $("[id*=chkRmtypAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }
            });
        });

        $("[id*=chkMealAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }
            });
        });

      
       	
    </script>
    <style>
          .displaynone
        {
        	display:none;
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
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
            font-size: 10pt;
            font-weight: bold;
            border-bottom-style: double;
            border-width: medium;
        }
        
        *
        {
            outline: none;
        }
        
        .fmhead
        {
            display: block;
            text-align: center;
        }
        
        .ModalPopupBG
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
        
        .ModalPopupBGmeal
        {
            filter: alpha(opacity=50);
            opacity: 0.9;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Commission" CssClass="field_heading"
                                Width="100%" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td valign="top" align="left" width="150">
                            &nbsp;
                        </td>
                        <td class="td_cell" valign="top" align="left" colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150" rowspan="2">
                            <div id="Div1" style="height: 402px;">
                                <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                            </div>
                        </td>
                        <td align="left" class="td_cell" valign="top" colspan="3">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" class="td_cell" valign="top" colspan="50">
                                        <asp:Panel ID="Panelsearch" runat="server" Font-Bold="true" GroupingText="Search Details"
                                            Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td width="120px">
                                                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" Font-Bold="False" CssClass="btn">
                                                        </asp:Button>
                                                    </td>
                                                    <td class="td_cell" valign="top" align="left">
                                                        <asp:Button ID="btncopycontract" runat="server" Text="Copy From Another Contract"
                                                            Font-Bold="False" CssClass="btn" Width="200px"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnselect" runat="server" Text="Copy From Another Offer" Font-Bold="False"
                                                            CssClass="btn" OnClick="btnselect_Click"></asp:Button>
                                                    </td>
                                                    <td style="display: none">
                                                        <asp:Button ID="btnExportToExcel" TabIndex="16" runat="server" CssClass="field_button">
                                                        </asp:Button>
                                                        <asp:Button ID="btnprint" TabIndex="16" runat="server" CssClass="field_button"></asp:Button>
                                                    </td>

                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                         <asp:Label ID="lblsortby" Text="Sort By" runat ="server" ></asp:Label>
                                                       <asp:DropDownList ID ="ddlorder" runat ="server" AutoPostBack ="true" >
                                                     <asp:ListItem value="I"> Tran ID</asp:ListItem>
                                                     <asp:ListItem value="P"> Promotion ID</asp:ListItem>
                                                      <asp:ListItem value="S"> Season</asp:ListItem>
                                                       <asp:ListItem Value="F">Min From Date</asp:ListItem>
                                                       <asp:ListItem Value ="T">Max To Date</asp:ListItem>
                                                       <asp:ListItem Value ="A">Applicable To</asp:ListItem>
                                                        <asp:ListItem Value="C">Created Date</asp:ListItem>
                                                        <asp:ListItem Value="CU">Created User</asp:ListItem>
                                                        <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                                        <asp:ListItem Value="MU">Modified User</asp:ListItem>
                                                       </asp:DropDownList>
                                                       <asp:DropDownList ID="ddlorderby" AutoPostBack ="true" runat="server" >
                                                       <asp:ListItem Value="A"> ASC</asp:ListItem>
                                                       <asp:ListItem Value ="D">DESC</asp:ListItem>
                                                       </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" valign="top" colspan="5">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="field_heading" colspan="8" width="100%">
                                                                            <asp:Label ID="Label1" runat="server" Text="List Of Entries"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <div id="searchresults">
                                                        <td style="width: 100%" valign="top" colspan="50">
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div id="Showdetails" runat="server" style="width: 100%; border: 3px solid #2D7C8A;
                                                                                    background-color: White;">
                                                                                    <asp:GridView ID="gv_SearchResult" runat="server" __designer:wfdid="w42" AllowPaging="True"
                                                                                        AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="Solid"
                                                                                        BorderWidth="1px" CellPadding="3" CssClass="grdstyle" Font-Size="10px" GridLines="Vertical"
                                                                                        Width="100%">
                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Supplier Code" Visible="False">
                                                                                                <EditItemTemplate>
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbltranid" runat="server" __designer:wfdid="w1" Text='<%# Bind("tranid") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="tranid" HeaderText="Tran.ID" />
                                                                                            <asp:BoundField DataField="promotionid" HeaderText="Promotion.ID" />
                                                                                             <asp:BoundField DataField="promotionname" HeaderText="Promotion.Name" />
                                                                                            <asp:BoundField DataField="season" HeaderText="Season" />
                                                                                            <asp:BoundField DataField="fromdate" HeaderText="Min From Date" />
                                                                                            <asp:BoundField DataField="todate" HeaderText="Max To Date" />
                                                                                            <asp:TemplateField HeaderText="Applicable To">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblapplicable" runat="server" Text='<%# Limit(Eval("applicableto"), 10)%>'
                                                                                                        ToolTip='<%# Eval("applicableto")%>'></asp:Label>
                                                                                                    <br />
                                                                                                    <asp:LinkButton ID="ReadMoreLinkButton" runat="server" CommandName="moreless" Text="More"
                                                                                                        Visible='<%# SetVisibility(Eval("applicableto"), 5) %>' OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>
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
                                                                                            <asp:ButtonField CommandName="Copy" HeaderText="Action" Text="Copy">
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
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </div>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="50">
                                        <asp:Panel ID="PanelMain" runat="server" Font-Bold="true" GroupingText="Entry Details"
                                            Width="100%" Style="display: none">
                                            <table>
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            Hotel&nbsp;Name&nbsp;&nbsp;&nbsp;
                                                                                            <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                                                                ReadOnly="true" Width="300px"></asp:TextBox>&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;Auto&nbsp;ID &nbsp;
                                                                                            <asp:TextBox ID="txtplistcode" runat="server" ReadOnly="true" Width="100px" TabIndex="2"></asp:TextBox>
                                                                                        </td>
                                                                                        <td style="display:none" >
                                                                                            Country&nbsp;Groups&nbsp;
                                                                                            <asp:CheckBox ID="chkctrygrp" runat="server" class="cls_chkctrygrp" TabIndex="3" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="td_cell" colspan="5" valign="top">
                                                                                            <asp:Label ID="lblapp" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                                                Text="Applicable To"></asp:Label>
                                                                                            <asp:TextBox ID="txtApplicableTo" TabIndex="4" runat="server" CssClass="field_input"
                                                                                                Rows="2" Style="margin: 0px; height: 48px; width: 300px;" TextMode="MultiLine"></asp:TextBox>
                                                                                                 <asp:Label ID="lblstatustext" runat="server" Style="vertical-align: bottom;" Text="Status:"
                                                                                                Width="43px"></asp:Label>&nbsp;
                                                                                            <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" Style="vertical-align: bottom;"
                                                                                                Text="Status" Width="43px"></asp:Label>

                                                                                            
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                        <td>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                   
                                                                                    <td colspan ="20">
                                                                                      <div id="divoffer" runat="server">
                                                                                        <table>
                                                                                        <tr>
                                                                                        <td>
                                                                                          <asp:Label ID="lblselect" runat="server" Style="vertical-align: bottom;" Text="Offers"
                                                                                                Width="100px"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                             <asp:TextBox ID="txtpromotionid" runat="server" ReadOnly="true" Width="130px" TabIndex="2"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtpromoitonname" runat="server" ReadOnly="true" Width="300px" TabIndex="2"></asp:TextBox>
                                                                                        </td>
                                                                                    
                                                                                        </tr>
                                                                                      

                                                                                        </table>

                                                                                       </div>
                                                                                    </td>
                                                                                   
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="10">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <div id="divuser" runat="server">
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
                                                                                                                <asp:Button ID="btnvsprocess" runat="server" Style="display: none" />
                                                                                                            </div>
                                                                                                            <div id="countrygroup1" style="float: left; margin-left: 10px; width: 100%">
                                                                                                                <uc2:Countrygroup ID="wucCountrygroup" runat="server" />
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                     <td  style ="display:none">
                                                                                        Select&nbsp;Season&nbsp;
                                                                                        <asp:TextBox ID="txtseasonname" runat="server" __designer:wfdid="w77" CssClass="field_input"
                                                                                            MaxLength="200" onkeyup="SetContextKey()" TabIndex="1" Width="175px"></asp:TextBox>
                                                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="1"
                                                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                            EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="0"
                                                                                            ServiceMethod="Getseasonlist" OnClientItemSelected="datefill" TargetControlID="txtseasonname"
                                                                                            UseContextKey="true">
                                                                                        </asp:AutoCompleteExtender>
                                                                                        <asp:Button ID="btngAlert" runat="server" Text="Fill" OnClick="btngAlert_Click" Style="display: none" />
                                                                                    </td>
                                                                                    
                                                                                      <td  valign="top">
                                                                                                <div id="Div3" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                                    <asp:GridView ID="grdseason" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                                                        Caption="Season Dates" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                                                        Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                                        <Columns>
                                                                                                            
                                                                                                             <asp:TemplateField HeaderText="SeasonName" Visible="false">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="txtseasoncode" runat="server" Text='<%# Bind("SeasonName") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                               <asp:TemplateField>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:CheckBox runat="server" ID="chkAll" />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox runat="server" ID="chkseason" Width="10px" AutoPostBack="true" OnCheckedChanged="check_changed" />
                                                                                
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>

                                                                                                             <asp:BoundField DataField="SeasonName" HeaderText="Season">
                                                                      
                                                                                                             <ControlStyle Width="100px" />
                                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px" />
                                                                                                             </asp:BoundField>

                                                                                                            <asp:BoundField DataField="fromdate" HeaderText="From Date">
                                                                                                                <ControlStyle Width="125px" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" Width="125px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date">
                                                                                                                <ControlStyle Width="125px" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" Width="125px" />
                                                                                                            </asp:BoundField>
                                                                                                            
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                       <td  valign="top" >
                                                                                        <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                            <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                                                Caption="Manual Dates not linked to Seasons" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                                                Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                                                <FooterStyle CssClass="grdfooter" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                                                                                                    
                                                                                                    <asp:TemplateField HeaderText="From Date">
                                                                                                     <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtfromDate" runat="server"   CssClass="fiel_input" Width="80px" ></asp:TextBox>
                                                                                                    <cc1:CalendarExtender ID="dpFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                                                                                                        TargetControlID="txtfromDate" PopupPosition="Right">
                                                                                                    </cc1:CalendarExtender>
                                                                                                    <cc1:MaskedEditExtender ID="MeFromDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                                                        TargetControlID="txtfromDate">
                                                                                                    </cc1:MaskedEditExtender>
                                                                                                    <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                                                                                        TabIndex="-1" /><br />
                                                                                                    <cc1:MaskedEditValidator ID="MevFromDate" runat="server" ControlExtender="MeFromDate"
                                                                                                        ControlToValidate="txtfromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                                                        EmptyValueMessage="Date is required" ErrorMessage="MeFromDate" InvalidValueBlurredMessage="Invalid Date"
                                                                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                                                    </cc1:MaskedEditValidator>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle Wrap="False" />
                                                                                                      <ItemStyle Wrap="False" />
                                                                                                      </asp:TemplateField>

                                                                                                     <asp:TemplateField HeaderText="To Date">
                                                                                                     <ItemTemplate>
                                                                                                       <asp:TextBox ID="txtToDate" runat="server"  CssClass="fiel_input" Width="80px" ></asp:TextBox>
                                                                                                    <cc1:CalendarExtender ID="dpToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDt"
                                                                                                        TargetControlID="txtToDate" PopupPosition="Right">
                                                                                                    </cc1:CalendarExtender>
                                                                                                    <cc1:MaskedEditExtender ID="MeToDate" runat="server" Mask="99/99/9999" MaskType="Date"
                                                                                                        TargetControlID="txtToDate">
                                                                                                    </cc1:MaskedEditExtender>
                                                                                                    <asp:ImageButton ID="ImgBtnToDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" 
                                                                                                        TabIndex="-1" /><br />
                                                                                                    <cc1:MaskedEditValidator ID="MevToDate" runat="server" ControlExtender="MeToDate"
                                                                                                        ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                                                                                        EmptyValueMessage="Date is required" ErrorMessage="MeToDate" InvalidValueBlurredMessage="Invalid Date"
                                                                                                        InvalidValueMessage="Invalid Date" TooltipMessage="Input a Date in Date/Month/Year">
                                                                                                    </cc1:MaskedEditValidator>
                                                                                                    </ItemTemplate>
                                                                                                       <HeaderStyle Wrap="False" />
                                                                                                      <ItemStyle Wrap="False" />
                                                                                                    </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Action">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="imgStayAdd" runat="server" ImageUrl="~/Images/PlusGreen.ico"
                                                                                                            Width="18px" OnClick="imgStayAdd_Click"  />
                                                                                                        <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                                                             Width="18px"  OnClick="imgSclose_Click" ToolTip="Delete Current Row" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                </Columns>
                                                                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                                <RowStyle CssClass="grdRowstyle" />
                                                                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                <HeaderStyle CssClass="grdheader" />
                                                                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                     <td  valign="top">
                                                                                        <div id="Div4" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                                    <asp:GridView ID="grdrmcat" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                                                        Caption="RoomCategory" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                                                        Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                                        <Columns>
                                                                                                            
                                                                                                             <asp:TemplateField HeaderText="Room Category" Visible="false">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="txtrmcatcode" runat="server" Text='<%# Bind("rmcatcode") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                               <asp:TemplateField>
                                                                                                                    <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:CheckBox runat="server" ID="chkRmcatAll" />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox runat="server" ID="chkrmcat" Width="10px" />
                                                                                
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>

                                                                                                             <asp:BoundField DataField="Rmcatname" HeaderText="Room Category">
                                                                      
                                                                                                             <ControlStyle Width="100px" />
                                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px" />
                                                                                                             </asp:BoundField>

                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                      </td>

                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td valign="top">
                                                                                <div id="divrmtype" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                    <asp:GridView ID="grdroomtype" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Caption="Select Room"
                                                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                        TabIndex="18" Width="400px">
                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkRmtypAll" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkrmtyp" Width="10px" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="RoomType Code" Visible="false">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="txtrmtypcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                                                                      <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label>
                                                                                                      </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="rmtypname" SortExpression="rmtypname" HeaderText="Room Type Name">
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="200px"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                                         <td valign="top">
                                                                                <div id="divmeal" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                    <asp:GridView ID="grdmealplan" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Caption="Select Meal"
                                                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                        TabIndex="18" Width="250px">
                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkMealAll" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkmeal" Width="10px" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Meal Code" Visible="false">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="txtmealcode" runat="server" Text='<%# Bind("mealcode") %>'></asp:Label>
                                                                                                     <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label>
                                                                                                     </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="mealname" SortExpression="mealname" HeaderText="Meal Name">
                                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="180px"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                    <td>
                                                                                    <asp:CheckBox ID="chkcomputed" runat="server" class="cls_chkcomm"  Text="Already Computed" TabIndex="8" ForeColor="Red"  />
                                                                                    </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <table>
                                                                                            <td style="width: 900px" valign="top">
                                                                                                <div id="divcommision" runat="server" style="max-height: 390px; overflow: auto; border-style: single;
                                                                                                    border-width: 6px">
                                                                                                   
                                                                                                    <asp:GridView ID="grdcommission" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Caption="Select Commision Type"
                                                                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                                        TabIndex="12" Width="700px">
                                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Formula Code" Visible="false">
                                                                                                                <EditItemTemplate>
                                                                                                                    &nbsp;
                                                                                                                </EditItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="txtformulacode" runat="server" Text='<%# Bind("formulaid") %>' CssClass="field_input"
                                                                                                                        Width="80px"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField Visible="false">
                                                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="25px" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox runat="server" ID="chkcomm" Width="10px" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="25px" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:RadioButton runat="server" ID="optcomm" Width="10px" GroupName="formula" AutoPostBack="true"
                                                                                                                        OnCheckedChanged="optcomm_CheckedChanged" onclick="RadioCheck(this);" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:TemplateField HeaderText="Formula Code"> <%-- changed by mohamed on 29/05/2018 --%>
                                                                                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblformulacodeNew" runat="server" Text='<%# Bind("formulaid") %>' CssClass="field_input"
                                                                                                                        Width="80px"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:TemplateField HeaderText="Formula Name" SortExpression="FormulaName">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblFormulaName" runat="server" Text='<%# Bind("FormulaName") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ControlStyle />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                              <asp:TemplateField HeaderText="Remarks" SortExpression="Remarks">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ControlStyle />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Commission Formula" SortExpression="CommissionFormula">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblFormula" runat="server" Text='<%# Bind("Formula") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ControlStyle />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                                    </asp:GridView>
                                                                                                    <div style="width: 100%; min-height: 10px" id="Div2" runat="server">
                                                                                                    </div>
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:GridView ID="grdcommissiondetail" runat="server" AutoGenerateColumns="False"
                                                                                                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                                                                    CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                                                    TabIndex="13" Width="200px">
                                                                                                                    <FooterStyle CssClass="grdfooter" />
                                                                                                                    <Columns>
                                                                                                                        <asp:TemplateField HeaderText="Formula Code" Visible="false">
                                                                                                                            <EditItemTemplate>
                                                                                                                                &nbsp;
                                                                                                                            </EditItemTemplate>
                                                                                                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="txtterm1" runat="server" Text='<%# Bind("term1") %>' CssClass="field_input"></asp:Label>
                                                                                                                            </ItemTemplate>
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Formula Name" SortExpression="termname">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblFormulaName" runat="server" Text='<%# Bind("termname") %>' Width="90px"></asp:Label>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ControlStyle />
                                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                                                        </asp:TemplateField>
                                                                                                                        <asp:TemplateField HeaderText="Value">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtperc" runat="server" Width="50px" Style="text-align: right" Text='<%# Bind("value") %>'></asp:TextBox>
                                                                                                                            </ItemTemplate>
                                                                                                                            <ControlStyle />
                                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                                                        </asp:TemplateField>
                                                                                                                    </Columns>
                                                                                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                                                                    <RowStyle CssClass="grdRowstyle" />
                                                                                                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                                    <HeaderStyle CssClass="grdheader" />
                                                                                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                                                </asp:GridView>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="display: none">
                                                                                                                Service&nbsp;Rate
                                                                                                            </td>
                                                                                                            <td align="left" style="display: none">
                                                                                                                <asp:TextBox ID="txtservicerate" runat="server" CssClass="field_input" Style="text-align: right"
                                                                                                                    TabIndex="14" Width="58px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td style="display: none">
                                                                                                                Tax&nbsp;%
                                                                                                            </td>
                                                                                                            <td align="left" style="display: none">
                                                                                                                <asp:TextBox ID="txttaxperc" runat="server" CssClass="field_input" Style="text-align: right"
                                                                                                                    TabIndex="15" Width="58px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td style="display: none">
                                                                                                                Commission&nbsp;%
                                                                                                            </td>
                                                                                                            <td align="left" style="display: none">
                                                                                                                <asp:TextBox ID="txtcommperc" runat="server" CssClass="field_input" Style="text-align: right"
                                                                                                                    TabIndex="16" Width="58px"></asp:TextBox>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <div id="divterms" runat="server" style="max-height: 325px; overflow: auto; border-style: single;
                                                                                                    border-width: 6px">
                                                                                                    <asp:GridView ID="grdCommTerms" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                                        BorderColor="#999999" BorderStyle="solid" BorderWidth="1px" Caption="Commision Terms"
                                                                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                                        TabIndex="17" Width="300px">
                                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField Visible="False" HeaderText="Line No">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblFLineNo" runat="server" Text='<%# Bind("RankOrder") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Term Code">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblTermCode" runat="server" Text='<%# Bind("TermCode") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle Wrap="false" HorizontalAlign="Left" VerticalAlign="Middle" Width="10%" />
                                                                                                                <ItemStyle Wrap="true" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Term Name">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblTermName" runat="server" Text='<%# Bind("TermName") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle Wrap="false" Width="25%" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                                                                <ItemStyle Wrap="true" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <RowStyle CssClass="grdRowstyle" />
                                                                                                        <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                                                        <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle CssClass="grdheader" />
                                                                                                        <AlternatingRowStyle CssClass="grdAternaterow" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td valign="top" style="display: none">
                                                                                                <asp:Label ID="lblfrmdate" runat="server" Text="From Date" Width="100px"></asp:Label>
                                                                                               
                                                                                            </td>
                                                                                            <td style="display: none">
                                                                                               
                                                                                                <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                                                                                              
                                                                                            </td>
                                                                                        </table>
                                                                                        <td>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="td_cell" colspan="3" valign="top" style ="display:none">
                                                                                            &nbsp;<asp:Label ID="lblpolicy" runat="server" Font-Bold="true" Width="100px" Style="vertical-align: top;"
                                                                                                Text="Policy"></asp:Label>
                                                                                            <asp:TextBox ID="txtpolicy" runat="server" CssClass="field_input" Rows="2" TabIndex="7"
                                                                                                Style="margin: 0px; height: 260px; width: 800px;" TextMode="MultiLine"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                        <tr>
                                                                            <%--         <td class="field_heading" colspan="8" width="100%">
                                                                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                                        <asp:Label ID="lable12" runat="server" Text="Minimum Nights Text Details" Style="text-align: center"></asp:Label>
                                                                    </div>
                                                                </td>--%>
                                                                        </tr>
                                                                      
                                                                        <tr style="font-size: 8pt">
                                                                            <td align="left" class="td_cell" rowspan="1" style="font-size: 10pt" valign="top"
                                                                                colspan="50">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                       
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="24" Text="Save" OnClientClick="ShowProgess();"
                                                                                    Width="93px" />
                                                                                &nbsp;
                                                                                <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                                                    Width="139px" />
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    
                                                                    <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td style="width: 100px" valign="top">
                        </td>
                        <td valign="top">
                            &nbsp;<asp:Button ID="btnhelp" runat="server" CssClass="field_button" TabIndex="26"
                                Text="Help" Visible="false" />
                        </td>
                        <td style="width: 100px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" width="150">
                            &nbsp;
                        </td>
                        <td style="width: 900px" valign="top" colspan="3">
                            <table style="width: 647px">
                                <tr>
                                    <td align="left" style="width: 140px">
                                        &nbsp;
                                    </td>
                                    <td align="left" style="width: 230px">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td align="left" style="width: 265px">
                                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 0px;"
                                            type="text" />
                                        <asp:Button ID="dummyCity" runat="server" Style="display: none;" />
                                        <asp:Button ID="dummyCityArea" runat="server" Style="display: none;" />
                                        <asp:HiddenField ID="hdnpartycode" runat="server" />
                                        <asp:HiddenField ID="hdncontractid" runat="server" />
                                        <asp:HiddenField ID="hdnconfromdate" runat="server" />
                                        <asp:HiddenField ID="hdncontodate" runat="server" />
                                        <asp:HiddenField ID="hdnpromofrmdate" runat="server" />
                                        <asp:HiddenField ID="hdnpromotodate" runat="server" />
                                        <asp:HiddenField ID="hdncopycontractid" runat="server" />
                                        <asp:HiddenField ID="hdncopypromotionid" runat="server" />
                                         <asp:HiddenField ID="hdnpromotionid" runat="server" />
                                          <asp:HiddenField ID="hdncommtype" runat="server" />
                                        <asp:Button ID="btnDummy" runat="server" Style="display: none" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
             <div id="Copycontract" runat="server" style="overflow: scroll; height: 500px; width: 600px;
                border: 3px solid green; background-color: White; display: none">
                <table>
                    <tr>
                      <td valign="top" align="center" colspan="2">
                            <asp:Label ID="Label5" runat="server" Text="Copy Commission From Other Contract" CssClass="field_heading"
                                Width="600px" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                       <td colspan="2">
                            <asp:GridView ID="grdviewrates" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" CssClass="td_cell" Width="550px">
                                <Columns>
                                    <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblplistcode" runat="server" Text='<%# Bind("plistcode") %>' __designer:wfdid="w1"></asp:Label>
                                            <asp:Label ID="lblcontract" runat="server" Text='<%# Bind("contractid") %>' __designer:wfdid="w1"></asp:Label>
                                            <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>' __designer:wfdid="w1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="" Text="Select" CommandName="Select">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Contractid" SortExpression="Contractid" HeaderText="Contract ID">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="plistcode" SortExpression="plistcode" HeaderText="Tran ID">
                                    </asp:BoundField>

                                     <asp:BoundField DataField="season" SortExpression="season" HeaderText="Season">
                                    </asp:BoundField>

                                     <asp:BoundField DataField="promotionid" SortExpression="promotionid" HeaderText="Promotion ID">
                                    </asp:BoundField>

                                    <asp:BoundField DataField="promotionname" SortExpression="promotionname" HeaderText="Promotion Name">
                                    </asp:BoundField>
                                  
                                    <asp:BoundField DataField="fromdate" SortExpression="fromdate" HeaderText="From Date">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="To Date">
                                    </asp:BoundField>
                                 <asp:TemplateField HeaderText="Applicable To">
                                                <ItemTemplate>
                                                           
                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 10)%>' Tooltip='<%# Eval("applicableto")%>'  ></asp:Label>
                                            <br />
                                                <asp:LinkButton ID="ReadMoreLinkButtoncopycont" runat="server" CommandName ="moreless" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 5) %>'  OnClick="ReadMoreLinkButtoncopycont_Click"></asp:LinkButton>

                                            </ItemTemplate>
                                                         
                                        </asp:TemplateField>
                                  
                                </Columns>
                                <RowStyle CssClass="grdRowstyle" />
                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                <HeaderStyle CssClass="grdheader" />
                                <AlternatingRowStyle CssClass="grdAternaterow" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="center">
                            <asp:Button ID="btnclose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
                        </td>
                    </tr>
                </table>
                <input id="btnokviewrates" runat="server" type="button" value="OK" style="display:none" />
                <input id="btncloseviewrates" runat="server" type="button" value="Cancel" style="display:none" />
                    <input id="btnviewchild" runat="server" type="button" value="Cancel" style="display:none" />
                                                           

                                                             
            </div>
             <cc1:ModalPopupExtender ID="ModalViewrates" runat="server" BehaviorID="ModalViewrates"
                    CancelControlID="btncloseviewrates" OkControlID="btnokviewrates" TargetControlID="btnviewchild"
                    PopupControlID="Copycontract" PopupDragHandleControlID="PopupHeader" Drag="true"
                    BackgroundCssClass="ModalPopupBG">
                </cc1:ModalPopupExtender>

               <table>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="Div14" runat="server" style="overflow: scroll; height: 400px; width: 750px;
                                    border: 3px solid green; background-color: White; display: none">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="grdpromotion" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="#999999" CssClass="td_cell" Width="720px">
                                                    <Columns>
                                                         <asp:ButtonField HeaderText="" Text="Select" CommandName="Select">
                                                            <ItemStyle ForeColor="Blue"></ItemStyle>
                                                        </asp:ButtonField>

                                                          <asp:TemplateField HeaderText="Tran.ID" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblplistcode" runat="server" Text='<%# Bind("plistcode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Promotion ID" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpromotionid" runat="server" Text='<%# Bind("promotionid") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Promotion Name" >
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpromotionname" runat="server" Text='<%# Bind("promotionname") %>'></asp:Label>
                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Applicable" Visible ="false"  >
                                                          <ItemTemplate>
                                                             <asp:Label ID="lblapplicableto" runat="server"   Text='<%# Bind("applicableto") %>'    ></asp:Label>
                                                            </ItemTemplate>
                                                         </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Applicable To">
                                                                <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 10)%>' Tooltip='<%# Eval("applicableto")%>'  ></asp:Label>
                                                            <br />
                                                                <asp:LinkButton ID="ReadMoreLinkButtonpromotion" runat="server" CommandName ="moreless" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 5) %>'  OnClick="ReadMoreLinkButtonpromotion_Click"></asp:LinkButton>

                                                            </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                           <asp:BoundField DataField="fromdate" SortExpression="fromdate" HeaderText="MinFrom Date">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="todate" SortExpression="todate" HeaderText="MaxTo Date">
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status">
                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                            <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        </asp:BoundField>
                                                    </Columns>
                                                   <RowStyle CssClass="grdRowstyle" />
                                                    <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                    <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="grdheader" />
                                                    <AlternatingRowStyle CssClass="grdAternaterow" />
                                                </asp:GridView>
                                            </td>
                                        </tr>

                                         <tr>
                                            <td align="center">
                                              
                                                <asp:Button ID="btncpromolose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
                                            </td>
                                        </tr>
                                     
                                    </table>
                                    <input id="btnInvisibleEBGuest1" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                    <input id="btnOkayEB1" type="button" value="OK" style="visibility: hidden" />
                                    <input id="btnCancelEB1" type="button" value="Cancel" style="visibility: hidden" />
                                </div>
                                <asp:ModalPopupExtender ID="ModalExtraPopup1" runat="server" BehaviorID="ModalExtraPopup1"
                                    CancelControlID="btnCancelEB1" OkControlID="btnOkayEB1" TargetControlID="btnInvisibleEBGuest1"
                                    PopupControlID="Div14" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                                </asp:ModalPopupExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>


  <%--Added Rosalin 30/10/2019--%>
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
                        <input id="Button1" type="button" value="Cancel" style="display: none" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
