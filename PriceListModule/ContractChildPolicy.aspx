<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" 
EnableEventValidation="false" CodeFile="ContractChildPolicy.aspx.vb" Inherits="PriceListModule_ContractChildPolicy" %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Src="Countrygroup.ascx"  TagName="Countrygroup" TagPrefix="uc2" %>

    <%@ OutputCache location="none" %> 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
      <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
      <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

 <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
  <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />

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

        function ShowProgess() {

            var ModalPopupDays = $find("ModalPopupDays");

            ModalPopupDays.show();
            return true;
        }

        var ltxtexhicode;
        function Exhibitionautocompleteselected(source, eventArgs) {

            var hiddenfieldID = source.get_id().replace("AutoCompleteExtender2", "txtExhicode");
            ltxtexhicode = hiddenfieldID;
            $get(hiddenfieldID).value = eventArgs.get_value();

            var exhicode = eventArgs.get_value();

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            var hdncont = document.getElementById("<%=hdncontractid.ClientID%>");


            ColServices.clsServices.Filldates(connstr.value, exhicode, hdncont.value, fillcombination, ErrorHandler, TimeOutHandler);
        }




        function fillcombination(result) {
            //var objGridView = document.getElementById('<v %=gv_Filldata.ClientID%>');

            var txtfromDateID = ltxtexhicode.replace("txtExhicode", "txtfromDate");
            var txtToDateID = ltxtexhicode.replace("txtExhicode", "txtToDate");
            $get(txtfromDateID).value = result[0];
            $get(txtToDateID).value = result[1];
        }
    </script>

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

            // alert(gridname);
            // alert(selectedgrdname);
            // alert(RowIndex);

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
                //  alert('selectedrow' + SelectedRow);
                return;
            }
            if (SelectedRow != null) {

                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
                //alert('background original');
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                //  alert('background changed color');
                CurrentRow.style.backgroundColor = '#FFCC99';
                CurrentRow.style.color = 'Black';
                var txtFrm = CurrentRow.cells[focusIndex].getElementsByTagName("input")[0];
                txtFrm.focus();
                // alert(txtFrm.value);

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

    </script>
    <script language="javascript" type="text/javascript" >
    function CheckGridList(obj) {
    var grid = obj.parentNode.parentNode.parentNode;
    var inputs = grid.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox") {
            if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                inputs[i].checked = false;
            }
        }
    }
}  
 
         </script>

    <script language="javascript" type="text/javascript" >
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

                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");

                }
            });
           // $get("ctl00_Main_btngAlert").click();
        });


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




        function datefill(chkseason) {
            $get("ctl00_Main_btngAlert").click();

        }


        function CheckContract(contractid) {
            var hdncontract = document.getElementById("<%=hdncontractid.ClientID%>");

            if ((hdncontract.value == '')) {
                alert('Please Save Contract Main details to continue');
                return false;
            }

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
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table style="width: 100%; height: 100%; border-right: gray 2px solid; border-top: gray 2px solid;
                border-left: gray 2px solid; border-bottom: gray 2px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Child Policy" CssClass="field_heading"
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
                                                      
                                                                                                   <asp:Label ID="lblsortby" Text="Sort By" runat ="server" ></asp:Label>
                                                       <asp:DropDownList ID ="ddlorder" runat ="server" AutoPostBack ="true" >
                                                     <asp:ListItem value="I"> Tran ID</asp:ListItem>
                                                     <asp:ListItem value="S"> Season</asp:ListItem>
                                                       <asp:ListItem Value="F">From Date</asp:ListItem>
                                                       <asp:ListItem Value ="T">To Date</asp:ListItem>
                                                       <asp:ListItem Value ="A">Applicable To</asp:ListItem>
                                                        <asp:ListItem Value ="D">Days Of the Week</asp:ListItem>
                                                        <asp:ListItem Value="C">Created Date</asp:ListItem>
                                                        <asp:ListItem Value="CU">Created User</asp:ListItem>
                                                        <asp:ListItem Value="M">Modified Date</asp:ListItem>
                                                        <asp:ListItem Value="MU">Modified User</asp:ListItem>
                                                         <asp:ListItem value="P"> Promotion.Id</asp:ListItem>
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
                                                                                                    <asp:Label ID="lbltranid" runat="server" Text='<%# Bind("tranid") %>' __designer:wfdid="w1"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="tranid" HeaderText="Tran.ID"></asp:BoundField>
                                                                                             <asp:BoundField DataField="promotionid" HeaderText="Promotion.ID"></asp:BoundField>
                                                                                              <asp:BoundField DataField="promotionname" HeaderText="Promotion Name"></asp:BoundField>
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
                                             
                                                        <table>
                                                            <tr>
                                                                <td align="left" class="td_cell" colspan="4" valign="top">
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                        Text="Hotel Name"></asp:Label>&nbsp;&nbsp;
                                                                    <asp:TextBox ID="txthotelname" runat="server" CssClass="field_input" TabIndex="1"
                                                                        ReadOnly="true" Width="300px"></asp:TextBox>&nbsp;
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                        Text="Rate Type"></asp:Label>
                                                                    <asp:Label ID="lblratetype" runat="server" Font-Bold="true" ForeColor="Blue" Style="vertical-align: Left;"
                                                                        Text="Contract"></asp:Label>
                                                                    <asp:Label ID="Label4" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                        Text="Auto ID"></asp:Label>
                                                                    <asp:TextBox ID="txtplistcode" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="td_cell" colspan="4" valign="top">
                                                                    <asp:Label ID="lblapp" runat="server" Font-Bold="true" Style="vertical-align: top;"
                                                                        Text="Applicable To"></asp:Label>
                                                                    <asp:TextBox ID="txtApplicableTo" runat="server" CssClass="field_input" Rows="2"
                                                                        Style="margin: 0px; height: 48px; width: 300px;" TextMode="MultiLine"></asp:TextBox>
                                                                   &nbsp;
                                                                    <asp:Label ID="lblstatustext" runat="server" Style="vertical-align: bottom;" Text="Status:"
                                                                        Width="43px"></asp:Label>&nbsp;
                                                                    <asp:Label ID="lblstatus" runat="server" Font-Bold="True" ForeColor="#3366FF" Style="vertical-align: bottom;"
                                                                        Text="Status" Width="43px"></asp:Label>
                                                                </td>
                                                                <td style="display:none" >
                                                                   Country&nbsp;Groups&nbsp;
                                                                    <asp:CheckBox ID="chkctrygrp" runat="server" class="cls_chkctrygrp" />&nbsp;
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
                                                                <td colspan="10">
                                                                    <table width="85%">
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
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td valign="top" style ="display:none">
                                                                                <div id="divseason" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                    <asp:GridView ID="gv_Seasons" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Caption="Select Season"
                                                                                        CellPadding="3" CssClass="td_cell" Font-Bold="true" Font-Size="12px" GridLines="Vertical"
                                                                                        TabIndex="18" Width="200px">
                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="seasonname" Visible="false">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="txtseasoncode" runat="server" Text='<%# Bind("seasonname") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblselect" runat="server" Text='<%# Bind("selected") %>'></asp:Label></ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkAllold" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox runat="server" ID="chkseasonold" Width="10px" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="seasonname" SortExpression="seasonname" HeaderText="Season">
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
                                                                                </div>
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
                                                                              <div id="Div4" runat="server" style="max-height: 250px; overflow: auto;">
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
                                                                            <td valign="top" style ="display:none">
                                                                                <div id="dv_SearchResult" runat="server" style="max-height: 250px; overflow: auto;">
                                                                                    <asp:GridView ID="grdoldDates" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                                                                        Caption="Pricelist Dates" CellPadding="3" CssClass="td_cell" Font-Bold="true"
                                                                                        Font-Size="12px" GridLines="Vertical" TabIndex="18">
                                                                                        <FooterStyle CssClass="grdfooter" />
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="clinenno" HeaderText="Sr No" Visible="False" />
                                                                                            <asp:BoundField DataField="fromdate" HeaderText="From Date">
                                                                                                <ControlStyle Width="100px" />
                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="todate" HeaderText="To Date">
                                                                                                <ControlStyle Width="100px" />
                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" Width="100px" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="SeasonName" HeaderText="Season">
                                                                                                <HeaderStyle BackColor="#06788B" HorizontalAlign="Left" />
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
                                                                            <td>
                                                                                &nbsp;
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
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>

                                                            <tr> <%--changed by mohamed on 21/02/2018--%>
                                                                <td class="field_heading" colspan="8" width="100%">
                                                                    <asp:Label ID="Label6" runat="server" Text="Tax Detail" Style="text-align: center"></asp:Label>
                                                                </td>
                                                            </tr>

                                                            <tr> 
                                                                <td colspan="3">
                                                                    <asp:CheckBox ID="chkVATCalculationRequired" runat="server" Text="VAT Calculation Required" AutoPostBack="true" />
                                                                </td>
                                                            </tr>

                                                            <tr> 
                                                                <td colspan="1">
                                                                    <table> <tr> <td width="150px">
                                                                        <asp:Label ID="Label7" runat="server" Text="Service Charge"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2" align="left">
                                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtServiceCharges" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                                    </td></tr></table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="1">
                                                                    <table> <tr> <td width="150px">
                                                                        <asp:Label ID="Label8" runat="server" Text="Municipality Fees"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2" align="left">
                                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="TxtMunicipalityFees" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                                    </td></tr></table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="1">
                                                                    <table> <tr> <td width="150px">
                                                                        <asp:Label ID="Label9" runat="server" Text="Tourism Fees"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2" align="left">
                                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtTourismFees" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                                    </td></tr></table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="1">
                                                                    <table> <tr> <td width="150px">
                                                                        <asp:Label ID="Label10" runat="server" Text="VAT"></asp:Label> 
                                                                    </td>
                                                                    <td colspan="2" align="left">
                                                                        <asp:TextBox style="WIDTH: 170px; TEXT-ALIGN: right" id="txtVAT" class="txtbox" onkeypress="return checkNumber()" AutoPostBack="true" runat="server" OnTextChanged="VATTextBox_TextChanged" /> %
                                                                    </td></tr></table>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="field_heading" colspan="8" width="100%">
                                                                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                                        <asp:Label ID="lable12" runat="server" Text="Child Rates including Child Meal Supplements per child per Night"
                                                                            Style="text-align: center"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btncopyratesnextrow" runat="server" CssClass="btn" TabIndex="29"
                                                                        Text="Copy Rates to Next Line" Width="200px" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    Select&nbsp;Meal
                                                                </td>
                                                                <td>
                                                                    <select id="ddlCopymeal" runat="server" class="field_input" style="width: 125px">
                                                                    </select>
                                                                </td>
                                                                <td>
                                                                     <asp:TextBox ID="txtfillrate" runat="server" __designer:wfdid="w77" 
                                                                        CssClass="field_input"  TabIndex="1" 
                                                                        Width="75px"></asp:TextBox>
                                                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="10"
                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="True" MinimumPrefixLength="0"
                                                                        ServiceMethod="getFillRateType"
                                                                        TargetControlID="txtfillrate" UseContextKey="true">
                                                                    </asp:AutoCompleteExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnfillrate" runat="server" CssClass="btn" TabIndex="31" Text="Fill Rates"
                                                                        Width="100px" />
                                                                </td>
                                                            </tr>
                                                            <tr style="font-size: 8pt">
                                                                <td align="left" class="td_cell" rowspan="1" style="font-size: 10pt" valign="top"
                                                                    colspan="50">
                                                                    <asp:GridView ID="grdRoomrates" TabIndex="10" runat="server" Font-Size="10px" CssClass="td_cell"
                                                                        Width="960px" BackColor="White" AutoGenerateColumns="False" BorderColor="#999999"
                                                                        BorderStyle="Solid" CellPadding="3" GridLines="Vertical">
                                                                        <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText=" " >
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server"   onclick="CheckGridList(this);" ></asp:CheckBox>
                                                                                    <asp:Label ID="lblCLineno" runat="server" CssClass="displaynone" Text=""></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
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
                                                                                        Width="150px"></asp:TextBox>
                                                                                    <asp:Button ID="btnrmtyp" runat="server" CssClass="btn" TabIndex="14" Text=".." Width="20px"
                                                                                        OnClick="btnrmtyp_Click" />
                                                                                        
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="180px" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Meal Plan">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtmealcode" runat="server" Enabled="false" CssClass="field_input"
                                                                                        Width="70px"></asp:TextBox>
                                                                                    <asp:Button ID="btnmeal" runat="server" CssClass="btn" OnClick="btnmeal_Click" TabIndex="14"
                                                                                        Text=".." Width="20px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="MaxChild Allowed">
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtmaxchild" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="MaxEB Allowed" Visible ="false">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtmaxeb" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="No.of Children">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtnoofchild" runat="server" CssClass="field_input" Style="text-align: left"
                                                                                        onkeyup="SetContextKey()" Width="80px"></asp:TextBox>
                                                                                    <asp:AutoCompleteExtender ID="txtnoofchild_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                        UseContextKey="true" ServiceMethod="Getfillchilds" TargetControlID="txtnoofchild">
                                                                                    </asp:AutoCompleteExtender>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="From Age">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txtfromage" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="To Age">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txttoage" runat="server" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                             <asp:TemplateField HeaderText="No Of Baby Cot">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                    &nbsp;<asp:TextBox ID="txt_Baby_CotNo" runat="server" MaxLength="1" ToolTip="Enter 0-9" CssClass="field_input" Style="text-align: center"
                                                                                        Width="80px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField >
                                                                                <HeaderStyle HorizontalAlign="Left"  Width="135px" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblHeadSH" runat="server" Style="text-align: center" Width="135px" Text="Charge for Sharing on same meal plan"></asp:Label> <br />
                                                                                    <asp:Label ID="txtHeadTVSH" runat="server" Style="text-align: center" Width="43px"  Text="TV"></asp:Label>
                                                                                    <asp:Label ID="txtHeadNTVSH" runat="server" Style="text-align: center" Width="43px"  Text="NTV"></asp:Label>
                                                                                    <asp:Label ID="txtHeadVATSH" runat="server" Style="text-align: center" Width="43px"  Text="VAT"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtsharing" runat="server" CssClass="field_input" Style="text-align: left"
                                                                                        Width="134px"></asp:TextBox>
                                                                                    <asp:AutoCompleteExtender ID="txtsharing_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                        ServiceMethod="getFillRateType" TargetControlID="txtsharing">
                                                                                    </asp:AutoCompleteExtender>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTVSH" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:0px;" TabIndex="-1"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtNTVSH" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:-4px;" TabIndex="-1"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtVATSH" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:-4px;" TabIndex="-1"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField >
                                                                                <HeaderStyle HorizontalAlign="Left" Width="135px" />

                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblHeadEB" runat="server" Style="text-align: center" Width="135px" Text="Charge for EB on same meal plan"></asp:Label> <br />
                                                                                    <asp:Label ID="txtHeadTVEB" runat="server" Style="text-align: center" Width="43px"  Text="TV"></asp:Label>
                                                                                    <asp:Label ID="txtHeadNTVEB" runat="server" Style="text-align: center" Width="43px"  Text="NTV"></asp:Label>
                                                                                    <asp:Label ID="txtHeadVATEB" runat="server" Style="text-align: center" Width="43px"  Text="VAT"></asp:Label>
                                                                                </HeaderTemplate>


                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtEB" runat="server" CssClass="field_input" Style="text-align: left"
                                                                                        Width="134px"></asp:TextBox>
                                                                                    <asp:AutoCompleteExtender ID="txtEB_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                        EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                        ServiceMethod="getFillRateType" TargetControlID="txtEB">
                                                                                    </asp:AutoCompleteExtender>
                                                                                    
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTVEB" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:0px;" TabIndex="-1"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtNTVEB" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:-4px;" TabIndex="-1"></asp:TextBox>
                                                                                    <asp:TextBox ID="txtVATEB" runat="server" CssClass="field_input" Width="43px" Font-Size="10px" Height="15px" ReadOnly="true" style="margin-left:-4px;" TabIndex="-1"></asp:TextBox>

                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                              <asp:TemplateField HeaderText="Child EB charge type" Visible ="false"><%----%>
                                                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                                                <ItemTemplate>
                                                                                   <select id="ddlEbType" class="drpdown" runat="server">
                                                                                        <option value="Per Night" selected="selected">Per Night</option>
                                                                                        <option value="Per Stay">Per Stay</option>
                                                                                    </select>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
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
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="15" align="left">
                                                                    <div id="divcopy1" runat="server">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td width="150px">
                                                                                    <asp:Label ID="lbl1" runat="server" Text="Enter Supplement Amount to Add"></asp:Label>
                                                                                </td>
                                                                                <td width="75px">
                                                                                    <input style="width: 70px" id="txtamt1" class="field_input" type="text" maxlength="4"
                                                                                        runat="server" />
                                                                                </td>
                                                                                <td width="230px">
                                                                                    <asp:Label ID="lbl2" runat="server" Text="Select Base Room type from"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <select id="ddlRoomfrom" runat="server" class="field_input" style="width: 200px">
                                                                                    </select>
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
                                                                                    <asp:Label ID="lbl4" runat="server" Text="Select    Room type  to Add  To"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <select id="ddlroomto" runat="server" class="field_input" style="width: 200px">
                                                                                    </select>
                                                                                    &nbsp;<asp:Button ID="btnapply" runat="server" CssClass="btn" Text="Apply" />
                                                                                </td>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="display: none">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                            <td style="width: 74px; height: 21px" class="td_cell">
                                                                                <asp:Panel ID="view1" runat="server">
                                                                                    <asp:Label ID="lblview" runat="server" Text="View Policy"> </asp:Label>
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
                                                                                    TabIndex="28" Text="Clear Policy" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnRemark" runat="server" CssClass="btn" TabIndex="29" Text="Generate Remark" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="24" Text="Save" OnClientClick="ShowProgess();" 
                                                                        Width="93px" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnreset" runat="server" CssClass="field_button" TabIndex="25" Text="Return To Search"
                                                                        Width="139px" />
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
                                                        

                                                            <input id="btnInvisibleEBGuest" runat="server" type="button" value="Cancel" style="display:none" />
                                                            <input id="btnOkayEB" type="button" value="OK" style="display:none" />
                                                            <input id="btnCancelEB" type="button" value="Cancel" style="display:none" />
                                                        </div>
                                                        <asp:HiddenField ID="hdnMainGridRowid" runat="server" />

                                                        

                                                          <cc1:ModalPopupExtender ID="ModalExtraPopup" runat="server" BehaviorID="ModalExtraPopup"
                                                                CancelControlID="btnCancelEB" OkControlID="btnOkayEB" TargetControlID="btnInvisibleEBGuest"
                                                                PopupControlID="ShowRoomtypes" PopupDragHandleControlID="PopupHeader" Drag="true"
                                                                BackgroundCssClass="ModalPopupBG">
                                                            </cc1:ModalPopupExtender>

                                                           

                                                
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
                                        <asp:HiddenField ID="hdndecimal" runat="server" />
                                         <asp:HiddenField ID="hdncopycontractid" runat="server" />
                                          <asp:HiddenField ID="hdncopypromotionid" runat="server" />
                                         <asp:HiddenField ID="hdnpromotionid" runat="server" />
                                          <asp:HiddenField ID="hdncommtype" runat="server" />
                                           <asp:HiddenField ID="hdnpromofrmdate" runat="server" />
                                        <asp:HiddenField ID="hdnpromotodate" runat="server" />
                                         <asp:HiddenField ID="hdnconfromdate" runat="server" />
                                        <asp:HiddenField ID="hdncontodate" runat="server" />
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
                                <asp:Label ID="Label5" runat="server" Text="Copy Child Policy From Other Contract" CssClass="field_heading"
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
                                <asp:Button ID="btnclose" runat="server" CssClass="field_button" Text="Close" Width="75px" />
                            </td>
                        </tr>
                    </table>
                    <input id="btnokviewrates" runat="server" type="button" value="OK" style="display:none" />
                    <input id="btncloseviewrates" runat="server" type="button" value="Cancel" style="display:none" />
                        <input id="btnviewchild" runat="server" type="button" value="Cancel" style="display:none" />
                                                           

                                                             
                </div>
                  <cc1:ModalPopupExtender ID="ModalViewrates" runat="server" BehaviorID="ModalViewrates"
                    CancelControlID="btnclose" OkControlID="btnokviewrates" TargetControlID="btnviewchild"
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

  

    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

