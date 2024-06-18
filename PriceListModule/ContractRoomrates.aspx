<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="ContractRoomrates.aspx.vb" Inherits="ContractRoomrates" %>

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
            $txtvsprocess.val('CountryGroup:" " Region:" " Country:" " Text:" "');

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

        function fnFillSearchVS(result) {
            glcallback(result, {
                preserveOrder: true // Otherwise the selected value is brought to the top
            });
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
    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function InitializeRequestUserControl(sender, args) {

        }

        function EndRequestUserControl(sender, args) {
            // after update occur on UpdatePanel re-init the Autocomplete
            visualsearchbox();
            //AutoCompleteExtenderKeyUp();
            AutoCompleteExtenderUserControlKeyUp();

        }

        function ShowProgess() {

            var ModalPopupDays = $find("ModalPopupDays");

            ModalPopupDays.show();
            return true;
        }

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
     <script type="text/javascript">
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
                 //  alert('readonly')
                 var txtFrm;
                 for (i = parseInt(focusIndex); i < 9; i++) {
                     txtFrm = CurrentRow.cells[i].getElementsByTagName("input")[0];
                     if (txtFrm.disabled == false) {
                         txtFrm.focus();
                         i = 9;
                     }
                     else {

                     }
                 }


             }

             SelectedRow = CurrentRow;
             SelectedRowIndex = RowIndex;

             setTimeout("SelectedRow.focus();", 0);
         }

         function SelectSibling(e, gridname, focusIndex, Cur_row) {
             var iflag = 0;
             if (SelectedRowIndex != Cur_row.rowIndex - 1) {
                 iflag = 1;
             }
             var e = e ? e : window.event;
             var KeyCode = e.which ? e.which : e.keyCode;
             if (KeyCode == 40 || KeyCode == 38 || KeyCode == 9) {
                 if (KeyCode == 40) {
                     SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, gridname, focusIndex);
                 }
                 else if (KeyCode == 38) {
                     SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, gridname, focusIndex);
                 }
                 else if ((KeyCode == 9) && (iflag == 1)) {
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


        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }

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

        function chkTextLock(evt) {
            return false;
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



        function SetContextKey() {
            $find('<%=AutoCompleteExtender1.ClientID%>').set_contextKey($get("<%=hdncontractid.ClientID %>").value);
        }

        function datefill() {
            $get("ctl00_Main_btngAlert").click();

        }

        function cleartblsession() {
            var hdntbldata = document.getElementById("<%=hdntbldata.ClientID%>");
            hdntbldata.value = '1';
            ColServices.clsServices.setSessionValue("GV_HotelData", "NOTHING", cleartblsessionsuccess, ErrorHandler, TimeOutHandler);
            //  alert(hdntbldata.value);
        }

        function cleartblsessionsuccess() {
        }

        //changed by mohamed on 21/02/2018
        var txtID;
        var txtTvID;
        var txtNtvID;
        var txtVatID;

        function calculateVAT(txtID1, txtTvID1, txtNtvID1, txtVatID1) {
            txtID = txtID1;
            txtTvID = txtTvID1;
            txtNtvID = txtNtvID1;
            txtVatID = txtVatID1;

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value;

            var txt1 = document.getElementById(txtID);
            var txtTV = document.getElementById(txtTvID);
            var txtNTV = document.getElementById(txtNtvID);
            var txtVAT = document.getElementById(txtVatID);
            var vSerCharge = document.getElementById("<%=txtServiceCharges.ClientID%>");
            var vMunciFee = document.getElementById("<%=TxtMunicipalityFees.ClientID%>");
            var vTourFee = document.getElementById("<%=txtTourismFees.ClientID%>");
            var vVAT = document.getElementById("<%=txtVAT.ClientID%>");

            if (vSerCharge.value.toString() == '') { vSerCharge.value = "0"; }
            if (vMunciFee.value.toString() == '') { vMunciFee.value = "0"; }
            if (vTourFee.value.toString() == '') { vTourFee.value = "0"; }
            if (vVAT.value.toString() == '') { vVAT.value = "0"; }

            if (txt1.value == "N/A" || txt1.value == "On Request" || txt1.value == "Free" || txt1.value == "Incl" || txt1.value == "N.Incl") {
                txtTV.value = "0";
                txtNTV.value = "0";
                txtVAT.value = "0";
            }
            else { //if (txt1.value="N/A" || txt1.value="On Request")
                var chkvatreq = document.getElementById("<%=chkVATCalculationRequired.ClientID%>");
                if (chkvatreq.checked) {

                    var lSqlStr;
                    lSqlStr = "execute sp_calculate_taxablevalue_onlycost " + parseFloat(txt1.value.toString()).toString() + ",";
                    lSqlStr = lSqlStr + parseFloat(vSerCharge.value.toString()).toString() + ",";
                    lSqlStr = lSqlStr + parseFloat(vMunciFee.value.toString()).toString() + ",";
                    lSqlStr = lSqlStr + parseFloat(vVAT.value.toString()).toString() + ",";
                    lSqlStr = lSqlStr + parseFloat(vTourFee.value.toString()).toString();

                    if (txt1.value.toString() != '') {
                        var txt1ValueFloat = parseFloat(txt1.value.toString());
                        if (txt1ValueFloat != NaN) {
                            ColServices.clsServices.getCommonArrayOfCodeAndNameFromSqlQuery(constr, lSqlStr, setcalculatedVATValue, ErrorHandler, TimeOutHandler);
                        }
                    }
                }
                else { //if (chkvatreq.checked) {
                    txtTV.value = txt1.value;
                    txtNTV.value = "";
                    txtVAT.value = "";
                }
            }
        }

        //changed by mohamed on 21/02/2018
        function setcalculatedVATValue(result) {
            
            var txtTV = document.getElementById(txtTvID);
            var txtNTV = document.getElementById(txtNtvID);
            var txtVAT = document.getElementById(txtVatID);
            for (var i = 0; i < result.length; i++) {
                if (result[i].ListText == "taxablevalue") {
                    txtTV.value = parseFloat(result[i].ListValue);
                }
                if (result[i].ListText == "nontaxablevalue") {
                    txtNTV.value = parseFloat(result[i].ListValue);
                }
                if (result[i].ListText == "vatvalue") {
                    txtVAT.value = parseFloat(result[i].ListValue);
                }
            }
        }

        function validatedbltpl(txtdblid, txttplid) {
            var txtdbl = document.getElementById(txtdblid);
            var txttpl = document.getElementById(txttplid);

            //Dbl - Trpl validation
            if (txttpl.value.toString() != '' && txtdbl.value.toString() != '') {
                var tplValueFloat = parseFloat(txttpl.value.toString());
                var dblValueFloat = parseFloat(txtdbl.value.toString());

                var tplValueInt = parseInt(txttpl.value.toString());
                var dblValueInt = parseInt(txtdbl.value.toString());

                var tplDigit = 0;
                var dblDigit = 0;

                if (tplValueFloat != NaN && dblValueFloat != NaN) {
                    if (dblValueFloat > tplValueFloat) {
                        alert('DBL price is higher than TPL price');
                    }

                    if (dblValueInt != 0) { dblDigit = parseInt(Math.log10(dblValueInt) + 1); }
                    if (tplValueInt != 0) { tplDigit = parseInt(Math.log10(tplValueInt) + 1); }

                    if (tplDigit > dblDigit) {
                        alert('No of digits of TPL is higher than DBL');
                    }
                }
            }
            //Dbl - Trpl validation

        }


        function validatesgldbl(txtsglid, txtdblid) {
            var txtsgl = document.getElementById(txtsglid);
            var txtdbl = document.getElementById(txtdblid);

            //alert(txtsgl.value.toString()  + ' - ' + txtdbl.value.toString());

            //Sgl - Dbl validation
            if (txtsgl.value.toString() != '' && txtdbl.value.toString() != '') {
                var sglValueFloat = parseFloat(txtsgl.value.toString());
                var dblValueFloat = parseFloat(txtdbl.value.toString());

                var sglValueInt = parseInt(txtsgl.value.toString());
                var dblValueInt = parseInt(txtdbl.value.toString());

                var SglDigit = 0;
                var dblDigit = 0;

                if (sglValueFloat != NaN && dblValueFloat != NaN) {
                    if (sglValueFloat > dblValueFloat) {
                        alert('SGL price is higher than DBL price');
                    }

                    if (dblValueInt != 0) { dblDigit = parseInt(Math.log10(dblValueInt) + 1); }
                    if (sglValueInt != 0) { SglDigit = parseInt(Math.log10(sglValueInt) + 1); }

                    if (dblDigit > SglDigit) {
                        alert('No of digits of DBL is higher than SGL');
                    }
                }
            }
            //Sgl - Dbl validation



        }


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


        function CheckContract(contractid) {
            var hdncontract = document.getElementById("<%=hdncontractid.ClientID%>");

            if ((hdncontract.value == '')) {
                alert('Please Save Contract Main details to continue');
                return false;
            }

        }

        function Checkcommission(promotionid, commissiontype) {

            var hdnpromotionid = document.getElementById(promotionid);

            if ((commissiontype != 'Special Rates')) {
                alert('Promotion Special Rate Not Selected');
                return false;

            }


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


            if (newfdate > newctdate || newfdate < newcfdate) {

                alert("From Date Should belongs to the Promotions Period - " + confromdate.value + " to " + contodate.value);
                setTimeout(function () { fdate.focus(); }, 1);
                fdate.value = "";
            }

            setdate();
        }



        	
    </script>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Room Rates" CssClass="field_heading"
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
                                                     <asp:ListItem value="I"> PriceList Code</asp:ListItem>
                                                      <asp:ListItem value="P"> Promotion ID</asp:ListItem>
                                                       <asp:ListItem value="S">Season</asp:ListItem>
                                                       <asp:ListItem Value="F">From Date</asp:ListItem>
                                                       <asp:ListItem Value ="T">To Date</asp:ListItem>
                                                       <asp:ListItem Value ="A">Applicable To</asp:ListItem>
                                                        <asp:ListItem Value ="A">Days Of the Week</asp:ListItem>
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
                                                                                                    <asp:Label ID="lblplistcode" runat="server" Text='<%# Bind("plistcode") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="plistcode" HeaderText="Price list Code"></asp:BoundField>
                                                                                              <asp:BoundField DataField="promotionid" HeaderText="Promotion.ID" />
                                                                                             <asp:BoundField DataField="promotionname" HeaderText="Promotion.Name" />
                                                                                            <asp:BoundField DataField="seasoncode" HeaderText="Season"></asp:BoundField>
                                                                                            <asp:BoundField DataField="FrmDate" HeaderText="From Date"></asp:BoundField>
                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date"></asp:BoundField>
                                                                       <asp:TemplateField HeaderText="Applicable To">
                                                             <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblapplicable" runat="server"    Text='<%# Limit(Eval("applicableto"), 10)%>' Tooltip='<%# Eval("applicableto")%>'  ></asp:Label>
                                                            <br />
                                                             <asp:LinkButton ID="ReadMoreLinkButton" runat="server" CommandName ="moreless" Text="More" Visible='<%# SetVisibility(Eval("applicableto"), 5) %>'  OnClick="ReadMoreLinkButton_Click"></asp:LinkButton>

                                                            </ItemTemplate>
                                                         
                                                        </asp:TemplateField>
                                                                                            <asp:BoundField DataField="DaysoftheWeek" HeaderText="Days of the Week"></asp:BoundField>
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
                                                                                            <asp:ButtonField HeaderText="Action" Text="Cancellation Policy link" CommandName="cancellink" Visible ="false" >
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
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td width="75px">
                                                        Hotel&nbsp;Name
                                                    </td>
                                                    <td >

                                                        <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                            Width="300px" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td colspan ="4">
                                                    <table>
                                                    <tr>
                                                      <td>Rate&nbsp;Type</td>
                                                      <td><asp:Label ID="lblratetype" runat="server" Font-Bold="true" ForeColor="Blue" Style="vertical-align: Left;"
                                                            Text="Contract"></asp:Label></td>
                                                       <td>Price&nbsp;List&nbsp;Code</td>
                                                       <td><asp:TextBox ID="txtplistcode" runat="server" ReadOnly="true" Width="150px"></asp:TextBox></td>
                                                    </tr>
                                                    
                                                    </table>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td width="75px">
                                                        Applicable&nbsp;To
                                                    </td>
                                                    <td align="left" class="td_cell" colspan="5" valign="top">
                                                        <asp:TextBox ID="txtApplicableTo" runat="server" CssClass="field_input" Rows="2"
                                                            Style="margin: 0px; height: 48px; width: 300px;" TextMode="MultiLine"></asp:TextBox>
                                                        <asp:Label ID="lblstatustext" runat="server" Style="vertical-align: bottom;" Text="Status:"
                                                            Width="43px"></asp:Label>
                                                        &nbsp;
                                                        <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" Style="vertical-align: bottom;"
                                                            Text="Status" Width="43px"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                    </td>
                                                    <td style ="display:none">
                                                     <asp:CheckBox ID="chkctrygrp" runat="server" class="cls_chkctrygrp" Text="Country Groups" />
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="20">
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
                                                    <td colspan="20">
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
                                                <tr>
                                                    <td width="110px">
                                                      <asp:Label ID="lblseason" runat="server" Style="vertical-align: bottom;" Text="Select Season"
                                                                            Width="110px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="updseason" runat="server">
                                                            <ContentTemplate>
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
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td width="150px">
                                                        <asp:Label ID="lblbookingvaltype" runat="server" Style="vertical-align: Left;" Text="Booking Validity Type"
                                                            Font-Bold="true" Width="150px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select id="ddlBookingValidity" runat="server" class="drpdown" name="D1" style="width: 220px;">
                                                            <option selected="" value="1">Book Before Days from Checkin</option>
                                                            <option value="2">Book Before Months from Check In</option>
                                                            <option value="3">Range of Dates</option>
                                                            <option value="4">Book Before</option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td colspan="2" valign="top">
                                                        <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                                                            <asp:GridView ID="grdDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                Caption="Pricelist Dates" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                <FooterStyle CssClass="grdfooter" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                                                                    <asp:BoundField DataField="fromdate" HeaderText="From Date">
                                                                        <ControlStyle Width="150px" />
                                                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="todate" HeaderText="To Date">
                                                                        <ControlStyle Width="150px" />
                                                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <RowStyle CssClass="grdRowstyle" />
                                                                <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                                <HeaderStyle CssClass="grdheader" />
                                                                <AlternatingRowStyle CssClass="grdAternaterow" />
                                                            </asp:GridView>
                                                        </div>
                                                        <div id="divpromodates" runat="server" style="max-height: 250px; overflow: auto;display:none">
                                                            <asp:GridView ID="grdpromodates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                Caption="Promotion Dates" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                <FooterStyle CssClass="grdfooter" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
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
                                                                        <HeaderStyle Wrap="False" />
                                                                        <ItemStyle Wrap="False" />
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
                                                                        <HeaderStyle Wrap="False" />
                                                                        <ItemStyle Wrap="False" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgStayAdd" runat="server" ImageUrl="~/Images/PlusGreen.ico"
                                                                                Width="18px" OnClick="imgStayAdd_Click" />
                                                                            <asp:ImageButton ID="imgSclose" runat="server" ImageUrl="~/Images/crystaltoolbar/DeleteRed.png"
                                                                                Width="18px" OnClick="imgSclose_Click" ToolTip="Delete Current Row" />
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
                                                   
                                                </tr>
                                                <tr>
                                                    <td width="250px" colspan="2" valign="top">
                                                        <asp:GridView ID="grdmealplan" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                            Caption="Select MealPlan" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                            Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                            <FooterStyle CssClass="grdfooter" />
                                                            <Columns>
                                                                  <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkMealAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox runat="server" ID="chkSelect" Width="10px" />
                                                                                
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                               
                                                                <asp:BoundField DataField="mealcode" HeaderText="Meal Code">
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="mealname" HeaderText="Meal Name">
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="mealcode" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblmealcode" runat="server" Text='<%# Bind("mealcode") %>'></asp:Label>
                                                                        <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="rankorder" HeaderText="rankorder" Visible="false"></asp:BoundField>
                                                            </Columns>
                                                            <RowStyle CssClass="grdRowstyle" />
                                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                            <HeaderStyle CssClass="grdheader" />
                                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                                        </asp:GridView>
                                                    </td>
                                                    <td colspan="2">
                                                      
                                                       <asp:Label ID="lblsort" runat="server" Text="Sort By"></asp:Label>
                                                       <asp:DropDownList ID="ddlfilldata" runat="server" CssClass="drpdown" TabIndex="8">
                                                            <asp:ListItem Value="0">RoomType</asp:ListItem>
                                                            <asp:ListItem Value="1">Mealplan</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btngenerate" runat="server" CssClass="field_button" OnClientClick="cleartblsession();"
                                                            TabIndex="28" Text="Generate" Width="75px" />
                                                    </td>
                                                    
                                                </tr>

                                                <tr> <%--changed by mohamed on 21/02/2018--%>
                                                    <td class="field_heading" colspan="8" width="100%">
                                                        <asp:Label ID="Label4" runat="server" Text="Tax Detail" Style="text-align: center"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr> 
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="chkVATCalculationRequired" runat="server" Text="VAT Calculation Required" AutoPostBack="true" />
                                                    </td>
                                                </tr>

                                                <tr> 
                                                    <td colspan="1">
                                                        <asp:Label ID="Label5" runat="server" Text="Service Charge"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtServiceCharges" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label ID="Label6" runat="server" Text="Municipality Fees"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="TxtMunicipalityFees" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label ID="Label7" runat="server" Text="Tourism Fees"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtTourismFees" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1">
                                                        <asp:Label ID="Label8" runat="server" Text="VAT"></asp:Label> 
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtVAT" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td class="field_heading" colspan="8" width="100%">
                                                        <asp:Label ID="lable12" runat="server" Text="Room Rates" Style="text-align: center"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btncopyratesnextrow" runat="server" CssClass="btn" TabIndex="29"
                                                            Text="Copy Rates to Next Line" Width="200px" />
                                                             <select id="ddlrmcat" runat="server" class="field_input" style="width: 80px">
                                                            </select>
                                                    </td>
                                                      <td >
                                                          <select id="ddlmealcode" runat="server" class="field_input" style="width: 50px">
                                                         </select>
                                                           <asp:TextBox ID="txtfillamt" runat="server" __designer:wfdid="w77" 
                                                                CssClass="field_input"  TabIndex="1" 
                                                                Width="75px"></asp:TextBox>

                                                              <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="10"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="1"
                                                                ServiceMethod="getFillRateType"
                                                                TargetControlID="txtfillamt" UseContextKey="true">
                                                            </asp:AutoCompleteExtender>

                                                       </td>
                                                    <td >
                                                           <asp:Button ID="btnfillamt" runat="server" CssClass="btn" TabIndex="30" Text="Fill Rates"
                                                            Width="100px" />
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:DropDownList ID="ddlfillrow" runat="server" CssClass="drpdown" TabIndex="8">
                                                            <asp:ListItem Value="0">No.of Nights</asp:ListItem>
                                                            <asp:ListItem Value="1">Min.Nights</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="3">
                                                        <input style="width: 40px; text-align: center" id="txtminnights" class="field_input"
                                                            type="text" maxlength="3" runat="server" />
                                                        <asp:Button ID="btnfillminnts" runat="server" CssClass="btn" TabIndex="30" Text="Fill Nights"
                                                            Width="100px" />
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
                                                        <asp:GridView ID="grdRoomrates" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                                                            Font-Size="10px" GridLines="Vertical" Width="1px">
                                                            <FooterStyle CssClass="grdfooter" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="field_input"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <input id="ChkSelect" type="checkbox" name="ChkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Room Type Code" Visible="false">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Text='<%# Bind("Room_Type_Code") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtrmtypcode" runat="server" CssClass="field_input" ReadOnly="True"
                                                                            Text='<%# Bind("Room_Type_Code") %>' Width="40px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Room Type Name" HeaderText="Room Type Name">
                                                                    <HeaderStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Meal Plan" HeaderText="Meal Plan"></asp:BoundField>
                                                                <asp:BoundField DataField="Price Pax" HeaderText="Price Pax"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="unityesno" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtunityesno" runat="server" Text='<%# Bind("unityesno") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="noofextraperson" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtnoofextraperson" runat="server" Text='<%# Bind("noofextraperson") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="grdRowstyle" />
                                                            <EmptyDataTemplate>
                                                                <input id="TxtSGL" class="txtbox" type="text" />
                                                            </EmptyDataTemplate>
                                                            <SelectedRowStyle CssClass="grdselectrowstyle" />
                                                            <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                                                            <HeaderStyle CssClass="grdheader" />
                                                            <AlternatingRowStyle CssClass="grdAternaterow" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="15" align="left">
                                                        <div id="divcopy1" runat="server">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td >
                                                                        <table>
                                                                        <tr>
                                                                            <td>
                                                                             <asp:Button ID="btnaddsupamount" runat="server" CssClass="btn" Text="Add Supplement Amount" Width="200px" />
                                                                            </td>
                                                                            <td>
                                                                             <asp:Button ID="btnclear" runat="server" CssClass="btn" Text="Clear Rates" />
                                                                            </td>
                                                                        </tr>
                                                                        </table>
                                                                       
                                                                         
                                                                    </td>
                                                                    <td align ="left">
                                                                   
                                                                    </td>
                                                                    
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                 <td style="display: none">
                                                                     <asp:Label ID="lbl1" runat="server" Text="Enter Supplement Amount to Add" Style="display: none"></asp:Label>
                                                                        <input style="width: 70px" id="txtamt1" class="field_input" type="text" maxlength="4"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <asp:Label ID="lbl2" runat="server" Text="Select Base Room type from"></asp:Label>
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <select id="ddlRoomfrom" runat="server" class="field_input" style="width: 200px">
                                                                        </select>
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <asp:Label ID="lbl3" runat="server" Text="Select Base Mealplan from"></asp:Label>
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <select id="ddlmealfrom" runat="server" class="field_input" style="width: 75px">
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none">
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl4" runat="server" Text="Select    Room type  to Add  To"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlroomto" runat="server" class="field_input" style="width: 200px">
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl5" runat="server" Text="Select   Mealplan To Add"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <select id="ddlmealto" runat="server" class="field_input" style="width: 75px">
                                                                        </select>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnapply" runat="server" CssClass="btn" Text="Apply" />
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
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="24" Text="Save" OnClientClick="ShowProgess();" 
                                                            Width="93px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                            OnClientClick="cleartblsession();" Width="139px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btncommisioncal" runat="server" CssClass="field_button" Style="display: none"
                                                            TabIndex="26" Text="ViewRates After Commision" Width="225px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                </tr>
                            </table>
                            <div id="ShowRoomtypes" runat="server" style="overflow: scroll; height: 500px; width: 500px;
                                border: 3px solid green; background-color: White; display: none">
                                <table>
                                    <tr>
                                        <td valign="top" align="center" colspan="2">
                                            <asp:Label ID="lblpopup" runat="server" Text="Add Supplement Amount" CssClass="field_heading"
                                                Width="500px" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl7" runat="server" CssClass="td_cell" Text="Select Base RoomType from"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlRoomfromnew" runat="server" class="field_input" style="width: 200px">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl8" runat="server" CssClass="td_cell" Text="Select Base Mealplan from"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlmealfromnew" runat="server" class="field_input" style="width: 75px">
                                            </select>
                                            <asp:Button ID="btnfillrooms" runat="server" CssClass="field_button" Text="Fill Rooms"
                                                Width="80px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="gv_Showroomtypes" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" Width="450px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="rmtypcode" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtrmtypcode" runat="server" Text='<%# Bind("rmtypcode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="rmtypcode" SortExpression="rmtypcode" HeaderText="RoomType Code"
                                                        Visible="false">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="rmtypname" SortExpression="rmtypname" HeaderText="Room Type">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="mealcode" SortExpression="mealcode" HeaderText="Meal">
                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Supplement Amount">
                                                        <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtsupamount" runat="server" CssClass="fiel_input" Width="80px">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black"></RowStyle>
                                                <SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle BackColor="#454580" ForeColor="White" Font-Bold="True"></HeaderStyle>
                                                <AlternatingRowStyle BackColor="Transparent" Font-Size="12px"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnok" runat="server" CssClass="field_button" Text="Save" Width="75px"
                                                Style="display: none" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btncancel" runat="server" CssClass="field_button" Text="Cancel" Width="75px" />
                                        </td>
                                    </tr>
                                </table>
                                <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="display: none" />
                                <input id="btnOkayEB" type="button" value="OK" style="display: none" />
                                <input id="btnCancelEB" type="button" value="Cancel" style="display: none" />
                            </div>
                            <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                BackgroundCssClass="ModalPopupBG">
                            </cc1:ModalPopupExtender>
                            <div id="Viewrates" runat="server" style="overflow: scroll; height: 500px; width: 500px;
                                border: 3px solid green; background-color: White; display: none">
                                <table>
                                    <tr>
                                        <td valign="top" align="center" colspan="2">
                                            <asp:Label ID="Label2" runat="server" Text="View Rates After Commission" CssClass="field_heading"
                                                Width="500px" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="grdviewrates" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" CssClass="td_cell" Width="450px">
                                                <Columns>
                                                   
                                                    <asp:BoundField DataField="rmtypname" SortExpression="rmtypname" HeaderText="Room Type">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="mealcode" SortExpression="mealcode" HeaderText="Meal Plan">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="unit" SortExpression="unit" HeaderText="UNIT"></asp:BoundField>
                                                    <asp:BoundField DataField="sgl" SortExpression="sgl" HeaderText="SGL"></asp:BoundField>
                                                    <asp:BoundField DataField="dbl" SortExpression="dbl" HeaderText="DBL"></asp:BoundField>
                                                    <asp:BoundField DataField="tpl" SortExpression="tpl" HeaderText="TPL"></asp:BoundField>

                                                      <asp:TemplateField HeaderText="" Visible ="false" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" runat="server" Text='<%# Bind("unit") %>' />
                                                        <asp:Label ID="lbltpl" runat="server" Text='<%# Bind("tpl") %>' />
                                                        <asp:Label ID="lbldbl" runat="server" Text='<%# Bind("dbl") %>' />
                                                        <asp:Label ID="lblsgl" runat="server" Text='<%# Bind("sgl") %>' />
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
                                <input id="btnokviewrates" type="button" value="OK" style="display: none" />
                                <input id="btncloseviewrates" type="button" value="Cancel" style="display: none" />
                                <input id="btnInvisibleEBGuestnew" runat="server" type="button" value="Cancel" style="display: none" />
                                <input id="btnOkayEBnew" type="button" value="OK" style="display: none" />
                                <input id="btnCancelEBnew" type="button" value="Cancel" style="display: none" />
                            </div>
                            <cc1:ModalPopupExtender ID="ModalViewrates" runat="server" BehaviorID="ModalViewrates"
                                CancelControlID="btncloseviewrates" OkControlID="btnokviewrates" TargetControlID="btnInvisibleEBGuestnew"
                                PopupControlID="Viewrates" PopupDragHandleControlID="PopupHeader" Drag="true"
                                BackgroundCssClass="ModalPopupBG">
                            </cc1:ModalPopupExtender>
                            <div id="Copycontract" runat="server" style="overflow: scroll; height: 500px; width: 600px;
                                border: 3px solid green; background-color: White; display: none">
                                <table>
                                    <tr>
                                        <td valign="top" align="center" colspan="2">
                                            <asp:Label ID="Label3" runat="server" Text="Copy Rates From Other Contract" CssClass="field_heading"
                                                Width="600px" ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="grdcopyrates" runat="server" AutoGenerateColumns="False" BackColor="White"
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
                                                    <asp:BoundField DataField="plistcode" SortExpression="plistcode" HeaderText="Pricelist Code">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="seasoncode" SortExpression="seasoncode" HeaderText="Seasons">
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
                                                    <asp:BoundField DataField="DaysoftheWeek" SortExpression="DaysoftheWeek" HeaderText="Days of the Week">
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
                                        <td>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btncopyclose" runat="server" CssClass="field_button" Text="Close"
                                                Width="75px" />
                                        </td>
                                    </tr>
                                </table>
                                <input id="btnokcopyrates" type="button" value="OK" style="display: none" />
                                <input id="btnclosecopyrates" type="button" value="Cancel" style="display: none" />
                                 <input id="btnInvisibleEBGuestcopy" runat="server" type="button" value="Cancel" style="display: none" />
                                <input id="btnOkayEBcopy" type="button" value="OK" style="display: none" />
                                <input id="btnCancelEBcopy" type="button" value="Cancel" style="display: none" />
                            </div>
                            <cc1:ModalPopupExtender ID="ModalCopyrates" runat="server" BehaviorID="ModalCopyrates"
                                CancelControlID="btnclosecopyrates" OkControlID="btnokcopyrates" TargetControlID="btnInvisibleEBGuestcopy"
                                PopupControlID="Copycontract" PopupDragHandleControlID="PopupHeader" Drag="true"
                                BackgroundCssClass="ModalPopupBG">
                            </cc1:ModalPopupExtender>
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
                                        <asp:HiddenField ID="hdncopycontractid" runat="server" />
                                        <asp:HiddenField ID="hdndecimal" runat="server" />
                                        <asp:HiddenField ID="hdncopypromotionid" runat="server" />
                                         <asp:HiddenField ID="hdnpromotionid" runat="server" />
                                          <asp:HiddenField ID="hdncommtype" runat="server" />
                                           <asp:HiddenField ID="hdnpromofrmdate" runat="server" />
                                        <asp:HiddenField ID="hdnpromotodate" runat="server" />
                                        <asp:Label ID="hdntbldata" runat="server" Style="display: none"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
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

                         <%--Added shahul 08/08/18--%>
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
