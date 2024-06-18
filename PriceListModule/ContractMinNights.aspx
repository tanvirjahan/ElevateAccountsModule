<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="ContractMinNights.aspx.vb" Inherits="PriceListModule_ContractMinNights" %>

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

        function CheckContract(contractid) {
            var hdncontract = document.getElementById("<%=hdncontractid.ClientID%>");

            if ((hdncontract.value == '')) {
                alert('Please Save Contract Main details to continue');
                return false;
            }

        }

        $("[id*=chkweekAll]").live("click", function () {
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


        function fillcombination(result) {
            //var objGridView = document.getElementById('<v %=gv_Filldata.ClientID%>');

            var txtfromDateID = ltxtexhicode.replace("txtExhicode", "txtfromDate");
            var txtToDateID = ltxtexhicode.replace("txtExhicode", "txtToDate");
            $get(txtfromDateID).value = result[0];
            $get(txtToDateID).value = result[1];
        }
    </script>
    
         <script type="text/javascript">
         //code to highlight the selected row
             var SelectedRow = null;
             var SelectedRowIndex = null;
             var UpperBound = null;
             var LowerBound = null;
             var selectedgrdname = null;

             window.onload = function () {
                 LowerBound = 0;
                 SelectedRowIndex = -1;
             }

             function SelectRow(CurrentRow, RowIndex, gridname, focusIndex) {
              
                 if (gridname != selectedgrdname) {
                     if (SelectedRow != null) {
                         SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                         SelectedRow.style.color = SelectedRow.originalForeColor;
                     }
                     SelectedRow = null;
                     SelectedRowIndex = null;
                     selectedgrdname = null;
                 }
                 selectedgrdname = gridname;
                 var gridView = document.getElementById(gridname);
                 UpperBound = gridView.getElementsByTagName("tr").length - 2;
                 if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) {
                    
                     return;
                 }
                 if (SelectedRow != null) {

                     SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                     SelectedRow.style.color = SelectedRow.originalForeColor;
                     
                 }

                 if (CurrentRow != null) {
                     CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                     CurrentRow.originalForeColor = CurrentRow.style.color;
                      
                     CurrentRow.style.backgroundColor = '#FFCC99';
                     CurrentRow.style.color = 'Black';
                     var txtFrm = CurrentRow.cells[focusIndex].getElementsByTagName("input")[0];
                  txtFrm.focus();
                       }
              SelectedRow = CurrentRow;
              SelectedRowIndex = RowIndex;

              setTimeout("SelectedRow.focus();", 0);
          }


             function SelectSibling(e, gridname, focusIndex, Cur_row) {

                 //                   alert(Cur_row.rowIndex-1);
                 //                   alert(SelectedRowIndex);
                 var iflag = 0;
                 if (SelectedRowIndex != Cur_row.rowIndex - 1) {
                     //                       SelectedRow = Cur_row;
                     //                       SelectedRowIndex = Cur_row.rowIndex - 1;
                     iflag = 1;
                 }
                 var e = e ? e : window.event;
                 var KeyCode = e.which ? e.which : e.keyCode;
                 // alert(SelectedRowIndex);
                 if (KeyCode == 40 || KeyCode == 38 || KeyCode == 9) {
                     //                       alert(Cur_row.rowIndex - 1);
                     //                       alert(SelectedRowIndex);
                     if (KeyCode == 40) {
                         SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, gridname, focusIndex);
                     }
                     else if (KeyCode == 38) {
                         SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, gridname, focusIndex);
                     }
                     else if ((KeyCode == 9) && (iflag == 1)) {
                         //  alert('9');
                         SelectRow(Cur_row, Cur_row.rowIndex - 1, gridname, focusIndex);
                     }
                 }
                 return true;
             }
             function LastSelectRow(CurrentRow, RowIndex, gridname, focusIndex) {
                 var row = document.getElementById(CurrentRow);
                 SelectRow(row, RowIndex, gridname, focusIndex);

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

        function checkfromdates(txtrmtypname, txtfromdate, txtodate) {

            var fdate = document.getElementById(txtfromdate);
            var tdate = document.getElementById(txtodate);
            var rmtypname = document.getElementById(txtrmtypname);
                      if (rmtypname.value != "") {
                if (fdate.value == null || fdate.value == "") {
                    alert("Please select from date.");
                }
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

                newcfdate = getFormatedDate(newcfdate);
                newctdate = getFormatedDate(newctdate);

                newfdate = getFormatedDate(newfdate);
                newtdate = getFormatedDate(newtdate);

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

        $("[id*=chkrmtypeAll]").live("click", function () {
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

        $("[id*=chkAll]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                    $get("ctl00_Main_btngAlert").click();
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                    $get("ctl00_Main_btngAlert").click();
                }
            });
        });
      

  


       


      
       	
    </script>
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
                            <asp:Label ID="lblHeading" runat="server" Text="Minimum Nights" CssClass="field_heading"
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
                                                   <td style="display:none">
               <asp:Button id="btnExportToExcel" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
        <asp:Button id="btnprint" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>

                                                   
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                       <asp:Label ID="lblsortby" Text ="Sort By" runat ="server" >
                                                       </asp:Label>
                                                       <asp:DropDownList ID="ddlorder" AutoPostBack ="true" runat="server" >
                                                       <asp:ListItem  Value ="I"> Tran ID</asp:ListItem>
                                                        <asp:ListItem  Value ="F"> From Date</asp:ListItem>
                                                         <asp:ListItem  Value ="T"> To Date</asp:ListItem>
                                                          <asp:ListItem  Value ="A">Applicable To</asp:ListItem>
                                                          <asp:ListItem  Value ="L">Exhi/Price List ID</asp:ListItem>
                                                       <asp:ListItem Value="C">Created Date</asp:ListItem>
                                                        <asp:ListItem Value="CU">Created User</asp:ListItem>
                                                        <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                                        <asp:ListItem Value="MU">Modified User</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlorderby" autopostback="true" runat ="server" >
                                                        <asp:ListItem Value ="A"> ASC</asp:ListItem>
                                                        <asp:ListItem  Value ="D"> DESC</asp:ListItem>
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
                                                                                    <asp:GridView ID="gv_SearchResult" runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle"
                                                                                        __designer:wfdid="w42" GridLines="Vertical" CellPadding="3" BorderWidth="1px"
                                                                                        BorderStyle="Solid" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True"
                                                                                        AllowPaging="True">
                                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                                        <Columns>
                                                                                            <asp:TemplateField Visible="False" HeaderText="Supplier Code">
                                                                                                <EditItemTemplate>
                                                                                                </EditItemTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lbltranid" runat="server" Text='<%# Bind("tranid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="tranid" HeaderText="Tran.ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="FrmDate" HeaderText="From Date"></asp:BoundField>
                                                                                            
                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date"  ></asp:BoundField>
                                                                                          <asp:TemplateField HeaderText="Applicable To">
                                                                                             <ItemTemplate>
                                                           
                                                                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 10)%>' Tooltip='<%# Eval("applicableto")%>'  ></asp:Label>
                                                                                            <br />
                                                                                             <asp:LinkButton ID="ReadMoreLinkButton" runat="server" CommandName ="moreless" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 5) %>'  OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>

                                                                                            </ItemTemplate>
                                                         
                                                                                        </asp:TemplateField>
                                                                                         <asp:TemplateField HeaderText="Exhi/Price List ID">
                                                                                             <ItemTemplate>
                                                                                   
                                                                                          <asp:Label ID="lblpricelistid" runat="server" Text='<%# Bind("linkcode") %>' __designer:wfdid="w1"></asp:Label>
                                                                                           </ItemTemplate>
                                                         
                                                                                        </asp:TemplateField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
                                                                                                <ItemStyle ForeColor="Blue"></ItemStyle>
                                                                                            </asp:ButtonField>
                                                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate"
                                                                                                SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
                                                                                            <asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate"
                                                                                                SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
                                                                                            <asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
                                                                                            </asp:BoundField>
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
                                                                                Hotel&nbsp;Name&nbsp;&nbsp;&nbsp;
                                                                                <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                                                    ReadOnly="true" Width="300px"></asp:TextBox>&nbsp;
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;Auto&nbsp;ID&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtplistcode" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                                                                            </td>
                                                                            <td style="display:none">
                                                                                Country&nbsp;Groups&nbsp;
                                                                                <asp:CheckBox ID="chkctrygrp" runat="server" class="cls_chkctrygrp" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" class="td_cell" colspan="5" valign="top">
                                                                                <asp:Label ID="lblapp" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                                    Text="Applicable To"></asp:Label>
                                                                                <asp:TextBox ID="txtApplicableTo" runat="server" CssClass="field_input" Rows="2"
                                                                                    Style="margin: 0px; height: 48px; width: 300px;" TextMode="MultiLine"></asp:TextBox>
                                                                                     <asp:Label ID="lblstatustext" runat="server" Style="vertical-align: bottom;" Text="Status:"
                                                                                    Width="43px"></asp:Label>&nbsp;
                                                                                <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" Style="vertical-align: bottom;"
                                                                                    Text="Status" Width="43px"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                             <td></td>
                                                              <td width="150px">
                                                        <asp:GridView ID="grdWeekDays" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                            Caption="Select Days" CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px"
                                                            GridLines="Vertical" TabIndex="18">
                                                            <FooterStyle CssClass="grdfooter" />
                                                            <Columns>
                                                                  <asp:TemplateField>
                                                                         <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox runat="server" ID="chkweekAll" />
                                                                          </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                    </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="BlockSale ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSrNo" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="days" HeaderText="Days Of Week">
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <RowStyle CssClass="grdRowstyle" />
                                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                            <HeaderStyle CssClass="grdheader" />
                                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                                        </asp:GridView>
                                                    </td>
                                                        <td colspan="10">
                                                            <div id="divoffer" runat="server">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblselect" runat="server" Style="vertical-align: bottom;" Text="Offers"
                                                                                Width="100px"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtpromotionidnew" runat="server" ReadOnly="true" Width="130px" TabIndex="2"></asp:TextBox>
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
                                                            </tr>
                                                            <tr>
                                                                <td class="field_heading" colspan="8" width="100%">
                                                                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                                        <asp:Label ID="lable12" runat="server" Text="Minimum Nights Text Details" Style="text-align: center"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="display: none">
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="display: none">
                                                                    <asp:Button ID="btnfillrate" runat="server" CssClass="btn" TabIndex="31" Text="Copy Rates Other Type"
                                                                        Width="200px" />
                                                                </td>
                                                                <td style="display: none">
                                                                    <input style="width: 40px" id="txtfillrate" class="field_input" type="text" maxlength="4"
                                                                        runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr style="font-size: 8pt">
                                                                <td align="left" class="td_cell" rowspan="1" style="font-size: 10pt" valign="top"
                                                                    colspan="50">
                                                                    <asp:GridView ID="grdRoomrates" TabIndex="10" runat="server" Font-Size="10px" CssClass="td_cell"
                                                                        Width="100%" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999"
                                                                        BorderStyle="Solid" CellPadding="3" GridLines="Vertical">
                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText=" ">
                                                                                <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" CssClass="field_input"></asp:CheckBox>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="RoomTypecode" Visible="false">
                                                                                <FooterTemplate>
                                                                                    &nbsp;
                                                                                </FooterTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtrmtypcode" runat="server" Enabled="false" CssClass="field_input"
                                                                                        Width="150px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="180px" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Select RoomType">
                                                                                <FooterTemplate>
                                                                                    &nbsp;
                                                                                </FooterTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtrmtypname" runat="server" Enabled="false" CssClass="field_input"
                                                                                        Width="250px" Text="All"></asp:TextBox>
                                                                                    <asp:Button ID="btnrmtyp" runat="server" CssClass="btn" TabIndex="14" Text=".." Width="20px"
                                                                                        OnClick="btnrmtyp_Click" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="250px" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Meal Plan">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtmealcode" runat="server" Enabled="false" CssClass="field_input"
                                                                                        Width="130px" Text="All"></asp:TextBox>
                                                                                    <asp:Button ID="btnmeal" runat="server" CssClass="btn" OnClick="btnmeal_Click" TabIndex="14"
                                                                                        Text=".." Width="20px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="From Date">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
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
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="To Date">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
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
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left"  Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Min.Nights">
                                                                                <EditItemTemplate>
                                                                                    &nbsp;
                                                                                </EditItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtminnights" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Options">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                                                <ItemTemplate>
                                                                                    <select id="ddloptions" class="drpdown" runat="server">
                                                                                        <option value="Over All" >Over All</option>
                                                                                        <option value="Within Period" selected="selected">Within Period</option>
                                                                                        <option value="CheckIn">CheckIn</option>
                                                                                    </select>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                              <asp:TemplateField HeaderText="Week Options"  Visible ="false">
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                                                <ItemTemplate>
                                                                                    <select id="ddlweekoptions" class="drpdown" runat="server">
                                                                                        <option value="Week Day" >Week Day</option>
                                                                                        <option value="Week End" selected="selected">Week End</option>
                                                                                       
                                                                                    </select>
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
                                                                <td colspan="2">
                                                                    <asp:Button ID="btnAddrow" runat="server" CssClass="btn" TabIndex="9" Text="Add Row" />
                                                                    <asp:Button ID="btndeleterow" runat="server" CssClass="btn" TabIndex="10" Text="Delete Row" />
                                                                    <asp:Button ID="btncopyratesnextrow" runat="server" CssClass="btn" TabIndex="29"
                                                                        Text="Copy Row to Next Line" Width="200px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td style="display: none">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td style="width: 74px; height: 21px" class="td_cell">
                                                                                <asp:Panel ID="view1" runat="server">
                                                                                    <asp:Label ID="lblview" runat="server" Text="Remarks"> </asp:Label>
                                                                                    <span style="color: #ff0000">* </span>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <td style="height: 21px" class="td_cell" colspan="6">
                                                                                <textarea id="txtViewPolicy" runat="server" class="field_input" cols="20" rows="2"
                                                                                    style="width: 550px; height: 150px" tabindex="27" readonly="true">
                                                                    </textarea>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnClearPolicy" runat="server" CssClass="btn" OnClick="btnClearPolicy_Click"
                                                                                    TabIndex="28" Text="Clear Remarks" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnRemark" runat="server" CssClass="btn" TabIndex="29" Text="Generate Remarks" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                                                        TabIndex="24" Text="Save" Width="93px" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                                        Width="139px" />
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div id="ShowRoomtypes" runat="server" style="overflow: scroll; height: 300px; width: 250px;
                                                            border: 3px solid green; background-color: White; display: none">
                                                            <asp:GridView ID="gv_Showroomtypes" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                BorderColor="#999999" CssClass="td_cell" Width="450px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="rmtypcode" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtrmtypcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                                            <asp:Label ID="lblrmtypname" runat="server" Text='<%# Bind("rmtypname") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkrmtypeAll" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkrmtype" Width="10px" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="rmtypcode" SortExpression="rmtypcode" HeaderText="RoomType Code"
                                                                        Visible="false">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="80px"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="rmtypname" SortExpression="rmtypname" HeaderText="Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                                <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                                                <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                                                <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                                            </asp:GridView>
                                                            <table style="float: left">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnOk1" runat="server" CssClass="field_button" Text="Ok" Width="80px" />&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnmealok" runat="server" CssClass="field_button" Text="Ok" Width="80px"
                                                                            Style="display: none" />&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClear1" runat="server" CssClass="field_button" Text="Close" Width="80px" />&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                BackgroundCssClass="ModalPopupBG">
                                                            </cc1:ModalPopupExtender>
                                                            <cc1:ModalPopupExtender ID="ModalPopupNoshow" runat="server" BehaviorID="ModalPopupNoshow"
                                                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                BackgroundCssClass="ModalPopupBG">
                                                            </cc1:ModalPopupExtender>
                                                            <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="visibility: hidden" />
                                                            <input id="btnOkayEB" type="button" value="OK" style="visibility: hidden" />
                                                            <input id="btnCancelEB" type="button" value="Cancel" style="visibility: hidden" />
                                                        </div>
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
                                         <asp:HiddenField ID="hdncopycontractid" runat="server" />
                                           <asp:HiddenField ID="hdCurrentDate" runat="server" />
                                            <asp:HiddenField ID="hdnpromofrmdate" runat="server" />
                                        <asp:HiddenField ID="hdnpromotodate" runat="server" />
                                       
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
                            <asp:Label ID="Label5" runat="server" Text="Copy Minimum Nights From Other Contract" CssClass="field_heading"
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
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField HeaderText="" Text="Select" CommandName="Select">
                                        <ItemStyle ForeColor="Blue"></ItemStyle>
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="Contractid" SortExpression="Contractid" HeaderText="Contract ID">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="plistcode" SortExpression="plistcode" HeaderText="Tran ID">
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

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
