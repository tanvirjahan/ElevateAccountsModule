<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupMain.aspx.vb" Inherits="SupMain"  %>
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
          //  AutoCompleteExtenderKeyUp();
            categoryAutoCompleteExtenderKeyUp();
            currencyAutoCompleteExtenderKeyUp();
            countryAutoCompleteExtenderKeyUp();
           cityAutoCompleteExtenderKeyUp();
            sectorAutoCompleteExtenderKeyUp();
            controlAutoCompleteExtenderKeyUp();
           areaAutoCompleteExtenderKeyUp();
            accuralAutoCompleteExtenderKeyUp();
            hotelchainAutoCompleteExtenderKeyUp();
           hotelstatusAutoCompleteExtenderKeyUp();
            propertytypeAutoCompleteExtenderKeyUp();
        });


        function categoryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                
                document.getElementById('<%=txtcategorycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
            }

        }


        function hotelautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txthotelcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txthotelcode.ClientID%>').value = '';
            }

        }

        function areaautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtareacode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtareacode.ClientID%>').value = '';
            }
        }

        function ShowTextboxes(e) {
//            alert('Hi');
            var vSerCharge = document.getElementById("<%=txtServiceCharges.ClientID%>");            
            var vMunciFee = document.getElementById("<%=TxtMunicipalityFees.ClientID%>");
            var vTourFee = document.getElementById("<%=txtTourismFees.ClientID%>");
            var vVAT = document.getElementById("<%=txtVAT.ClientID%>");

            var vlblservicecharges = document.getElementById("<%=lblservicecharges.ClientID%>");
            var vlblmncpltyfees = document.getElementById("<%=lblmncpltyfees.ClientID%>");
            var vlbltourismfees = document.getElementById("<%=lbltourismfees.ClientID%>");
            var vlblvtax = document.getElementById("<%=lblvtax.ClientID%>");

            var vlblperc1 = document.getElementById("<%=lblperc1.ClientID%>");
            var vlblperc2 = document.getElementById("<%=lblperc2.ClientID%>");
            var vlblperc3 = document.getElementById("<%=lblperc3.ClientID%>");
            var vlblperc4 = document.getElementById("<%=lblperc4.ClientID%>");

            

            var urlParams = new URLSearchParams(window.location.search);
            var myParam = urlParams.get('type');
//            alert(myParam);
            if (e.target.value == "Taxable") {
//                alert('hello');
                if (myParam == 'HOT') {
                    vlblservicecharges.style.visibility = "visible";
                    vlblmncpltyfees.style.visibility = "visible";
                    vlbltourismfees.style.visibility = "visible";
                    vlblvtax.style.visibility = "visible";

                    vSerCharge.style.visibility = "visible";
                    vMunciFee.style.visibility = "visible";
                    vTourFee.style.visibility = "visible";
                    vVAT.style.visibility = "visible";
                    vVAT.value = '';

                    vlblperc1.style.visibility = "visible";
                    vlblperc2.style.visibility = "visible";
                    vlblperc3.style.visibility = "visible";
                    vlblperc4.style.visibility = "visible";
                }
                else {
//                    alert('again hello');
                    vlblservicecharges.style.visibility = "hidden";
                    vlblmncpltyfees.style.visibility = "hidden";
                    vlbltourismfees.style.visibility = "hidden";
                    vlblvtax.style.visibility = "visible";

                    vSerCharge.style.visibility = "hidden";
                    vMunciFee.style.visibility = "hidden";
                    vTourFee.style.visibility = "hidden";
                    vVAT.style.visibility = "visible";
                    vVAT.value = '';

                    vlblperc1.style.visibility = "hidden";
                    vlblperc2.style.visibility = "hidden";
                    vlblperc3.style.visibility = "hidden";
                    vlblperc4.style.visibility = "visible";
                }
            }
            else if (e.target.value == "ZeroRated") {
                vlblservicecharges.style.visibility = "hidden";
                vlblmncpltyfees.style.visibility = "hidden";
                vlbltourismfees.style.visibility = "hidden";
                vlblvtax.style.visibility = "visible";

                vSerCharge.style.visibility = "hidden";
                vMunciFee.style.visibility = "hidden";
                vTourFee.style.visibility = "hidden";
                vVAT.style.visibility = "visible";
                vVAT.value = '0';                

                vlblperc1.style.visibility = "hidden";
                vlblperc2.style.visibility = "hidden";
                vlblperc3.style.visibility = "hidden";
                vlblperc4.style.visibility = "visible";
            }
            else {
                alert('Hi');
                vlblservicecharges.style.visibility = "hidden";
                vlblmncpltyfees.style.visibility = "hidden";
                vlbltourismfees.style.visibility = "hidden";
                vlblvtax.style.visibility = "hidden";

                vSerCharge.style.visibility = "hidden";
                vMunciFee.style.visibility = "hidden";
                vTourFee.style.visibility = "hidden";
                vVAT.style.visibility = "hidden";
                vVAT.value = '';

                vlblperc1.style.visibility = "hidden";
                vlblperc2.style.visibility = "hidden";
                vlblperc3.style.visibility = "hidden";
                vlblperc4.style.visibility = "hidden";
            }            
        }

        

        function currencyautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcurrencycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
            }

        }

        function countryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
               
            }
            else {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
            }
        }



        function cityautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
              
            }
            else {
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
            }
        }


        function sectorautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtsectorcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtsectorcode.ClientID%>').value = '';
            }
        }
        function controlautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcontrolacccode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcontrolacccode.ClientID%>').value = '';
            }
        }

        function accrualautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtaccrualcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtaccrualcode.ClientID%>').value = '';
            }
        }

        function hotelchainautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txthotelchaincode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txthotelchaincode.ClientID%>').value = '';
            }
        }
        function hotelstatusautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txthotelstatuscode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txthotelstatuscode.ClientID%>').value = '';
            }
        }
        function propertytypeautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtpropertytypecode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
            }
        }
        function AutoCompleteExtenderKeyUp() {

            $("#<%= txthotelname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txthotelname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelcode.ClientID%>').value = '';
                }

            });

            $("#<%= txthotelname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txthotelname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelcode.ClientID%>').value = '';
                }

            });

        }
        function categoryAutoCompleteExtenderKeyUp() {

            $("#<%= txtcategoryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcategoryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                }

            });


        }
        function currencyAutoCompleteExtenderKeyUp() {

            $("#<%= txtcurrencyname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcurrencyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcurrencyname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcurrencyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcurrencycode.ClientID%>').value = '';
                }

            });

        }

        function countryAutoCompleteExtenderKeyUp() {
            $("#<%= txtcountryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcountryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                }

            });
        }


        function cityAutoCompleteExtenderKeyUp() {

            $("#<%= txtcityname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcityname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcityname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcityname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                }

            });

        }

        function sectorAutoCompleteExtenderKeyUp() {

            $("#<%= txtsectorname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtsectorname.ClientID%>').value == '') {

                    document.getElementById('<%=txtsectorcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtsectorname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtsectorname.ClientID%>').value == '') {

                    document.getElementById('<%=txtsectorcode.ClientID%>').value = '';
                }

            });

        }

        function controlAutoCompleteExtenderKeyUp() {

            $("#<%= txtcontrolaccname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcontrolaccname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontrolacccode.ClientID%>').value = '';
                }

            });

            $("#<%= txtcontrolaccname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcontrolaccname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcontrolacccode.ClientID%>').value = '';
                }

            });

        }


        function areaAutoCompleteExtenderKeyUp() {

            $("#<%= txtareaname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtareaname.ClientID%>').value == '') {

                    document.getElementById('<%=txtareacode.ClientID%>').value = '';
                }

            });

            $("#<%= txtareaname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtareaname.ClientID%>').value == '') {

                    document.getElementById('<%=txtareacode.ClientID%>').value = '';
                }

            });

         
        }
        function accuralAutoCompleteExtenderKeyUp() {

            $("#<%= txtaccrualname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtaccrualname.ClientID%>').value == '') {

                    document.getElementById('<%=txtaccrualcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtaccrualname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtaccrualname.ClientID%>').value == '') {

                    document.getElementById('<%=txtaccrualcode.ClientID%>').value = '';
                }

            });

        
        }
        function hotelchainAutoCompleteExtenderKeyUp() {

            $("#<%= txthotelchainname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txthotelchainname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelchaincode.ClientID%>').value = '';
                }

            });

            $("#<%= txthotelchainname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txthotelchainname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelchaincode.ClientID%>').value = '';
                }

            });

          
        }

        function hotelstatusAutoCompleteExtenderKeyUp() {

            $("#<%= txthotelstatusname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txthotelstatusname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelstatuscode.ClientID%>').value = '';
                }

            });

            $("#<%= txthotelstatusname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txthotelstatusname.ClientID%>').value == '') {

                    document.getElementById('<%=txthotelstatuscode.ClientID%>').value = '';
                }

            });

        }
        function propertytypesAutoCompleteExtenderKeyUp() {

            $("#<%= txtpropertytypename.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtpropertytypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
                }

            });

            $("#<%= txtpropertytypename.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtpropertytypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtpropertytypecode.ClientID%>').value = '';
                }

            });

        } 


</script>

<script language="javascript" type="text/javascript" >
    function FormValidationMainDetail(state) {
        var vSerCharge = document.getElementById("<%=txtServiceCharges.ClientID%>");
        var vMunciFee = document.getElementById("<%=TxtMunicipalityFees.ClientID%>");
        var vTourFee = document.getElementById("<%=txtTourismFees.ClientID%>");
        var vVAT = document.getElementById("<%=txtVAT.ClientID%>");

        var txtnameval = document.getElementById("<%=txtname.ClientID%>");
        var txttypeval = document.getElementById("<%=txthotelcode.ClientID%>");
        var txtcategoryval = document.getElementById("<%=txtcategorycode.ClientID%>");
        var txtcurrencyval = document.getElementById("<%=txtcurrencycode.ClientID%>");
        var txtcountryval = document.getElementById("<%=txtcountrycode.ClientID%>");
        var txtcityval = document.getElementById("<%=txtcitycode.ClientID%>");
        var txtsectorval = document.getElementById("<%=txtsectorcode.ClientID%>");
        var txtcontrolval = document.getElementById("<%=txtcontrolacccode.ClientID%>");
        var txtorderval = document.getElementById("<%=txtOrder.ClientID%>");
        var txtpropertytypeval = document.getElementById("<%=txtpropertytypecode.ClientID%>");

        if (txtnameval.value == '') {
            alert('Name Cannot be blank');
            return false;
        }
        if (txttypeval.value == '') {
            alert('Please select supplier type');
            return false;
        }
        if (txtcategoryval.value == '') {
            alert('Please select value from Category Type');
            return false;
        }
        if (txtcurrencyval.value == '') {
            alert('Please select value from Currency Type');
            return false;
        }
        if (txtcountryval.value == '') {
            alert('Please select value from Country Type');
            return false;
        }
        if (txtcityval.value == '') {
            alert('Please select value from City Type');
            return false;
        }
        //changed by mohamed on 31/10/2021
        //        if (txtsectorval.value == '') {
        //            alert('Please select value from Sector Type');
        //            return false;
        //        }
////        if (txtpropertytypeval.value == '') {
////            alert('Please select value from Property Type');
////            return false;
////        }

////        if (txtcontrolval.value == '') {
////            alert('Please select value from Control A/C Code');
////            return false;
////        }

        else if (parseFloat(vSerCharge.value) < 0.0 || parseFloat(vSerCharge.value) > 100.0) {
            vSerCharge.focus();
            alert("Service Charge Percentage Range should be 0 to 100");
            return false;
        }

        else if (parseFloat(vMunciFee.value) < 0.0 || parseFloat(vMunciFee.value) > 100.0) {
            vMunciFee.focus();
            alert("Municipality Fee Percentage Range should be 0 to 100");
            return false;
        }

        else if (parseFloat(vTourFee.value) < 0.0 || parseFloat(vTourFee.value) > 100.0) {
            vTourFee.focus();
            alert("Tourism Fee Percentage Range should be 0 to 100");
            return false;
        }

        else if (parseFloat(vVAT.value) < 0.0 || parseFloat(vVAT.value) > 100.0) {
            vVAT.focus();
            alert("VAT Percentage Range should be 0 to 100");
            return false;
        }

        if (state != 'Delete') {
            if (txtorderval.value != '') {
                if (txtorderval.value <= 0) {
                    alert('Please enter order greater than zero.');
                    return false;
                }
            }
            if (txtorderval.value == '') {

                alert('Please enter order greater than zero.');
                return false;

            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete?') == false) return false; }
        }
    }

  


   
  


    //    Added AddNewArea function by Archana on 19/05/2015

    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

    function checkTelephoneNumber(e) {

        if ((event.keyCode < 45 || event.keyCode > 57)) {
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
    function checkCharacter(e) {
        
          if (event.keyCode == 32 || event.keyCode == 46)
        
              return;
          if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122) && (event.keyCode == 39) ) {
             
              return false;
          }
    }

    //    added by sribish
    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txtcode.ClientID%>");
        var vartxtname = document.getElementById("<%=txtname.ClientID%>");

        if ((vartxtcode.value == '') || (vartxtname.value == '')) {
            doLinks(false);
        }
        else {
            doLinks(true);
        }


    }
    //    added by sribish
    function doLinks(how) {
        for (var l = document.links, i = l.length - 1; i > -1; --i)
            if (!how)
                l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
            else
                l[i].onclick = function () { return true; };
    }
    function load() {
        //    added by sribish
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
    }

			
</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
        categoryAutoCompleteExtenderKeyUp();
        currencyAutoCompleteExtenderKeyUp();
        countryAutoCompleteExtenderKeyUp();
        cityAutoCompleteExtenderKeyUp();
        sectorAutoCompleteExtenderKeyUp();
        controlAutoCompleteExtenderKeyUp();
        areaAutoCompleteExtenderKeyUp();
        accuralAutoCompleteExtenderKeyUp();
        hotelchainAutoCompleteExtenderKeyUp();
        hotelstatusAutoCompleteExtenderKeyUp();
        propertytypesAutoCompleteExtenderKeyUp();
    }
</script>
 <style>
 .bgrow
 {
 background-color:white; 
 }

     .style1
     {
         height: 11px;
     }
     .style2
     {
         height: 23px;
     }
     .style3
     {
         font-family: Verdana, Arial, Geneva, "ms sans serif";
         font-size: 10pt;
         font-weight: normal;
         height: 23px;
     }

 </style>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
       
        <contenttemplate>
       
            <table style="" align=left>
                <tbody>
                <tr>
                <td colspan ="20" align ="left"  >
                 <whc:hoteltab ID="whotelatbcontrol" runat="server" />
                </td>
                </tr>
                <tr>
                <td>
                <div style="margin-top:-6px;margin-left:13px;">
                <table style="border: 2px solid gray; height: 700px;" class="td_cell" align=left>
                    <tr>
                        <td valign="top" align="center" width="150" colspan="2" class="style1">
                            <asp:Label ID="lblHeading" runat="server" Text="Suppliers" CssClass="field_heading"
                                Width="100%" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" width="150" class="style2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Code <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td class="style3" valign="top" align="left">
                            <input style="width: 196px" id="txtCode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                            &nbsp; Name&nbsp;
                            <input style="width: 213px" id="txtName" class="field_input" tabindex="2" type="text"
                                maxlength="100" runat="server" />
                                &nbsp; <asp:Label  ID="LblExtappid" runat="server" Visible ="false" Height ="18px"  /> <asp:Label  ID="Extappspan" runat="server" Visible ="false" style="color: #ff0000" text="*" Height ="18px"  />&nbsp;
                            <input style="width:196px" id="TxtExtappid" visible="false" class="field_input" tabindex="2" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" width="150">
                            &nbsp;
                            <div id="menudiv" style="height: 302px">
                                <uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
                            </div>
                        </td>
                        <td style="width: 100px" valign="top">
                            <div style="width: 824px; height: 450px" id="iframeINF" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table style="width: 656px">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 80px; height: 255px" class="td_cell" colspan="2">
                                                        <asp:Panel ID="PanelMain" runat="server" Width="699px" 
                                                            GroupingText="Main Details" Height="1214px">
                                                            <table style="width: 647px">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblType" runat="server" Text="Type" Width="30px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txthotelname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txthotelcode" runat="server" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnpartycode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="txthotelname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Gethoteltypelist" TargetControlID="txthotelname" OnClientItemSelected="hotelautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                     <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblalias" runat="server" Text="Legal Name of Entity" Width="90px"></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                          <asp:TextBox ID="txtalaisname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblcategory" runat="server" Text="Category" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcategoryname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcategorycode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncategorycode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="txtcategoryname_AutoCompleteExtender" runat="server"
                                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                                FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="Getcategorylist"
                                                                                TargetControlID="txtcategoryname" OnClientItemSelected="categoryautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text3" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text4" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 140px; display: none">
                                                                            Selling Category <span style="color: #ff0000" class="td_cell">*</span>
                                                                        </td>
                                                                        <td style="width: 88px; display: none" align="left">
                                                                            <select style="width: 223px; display: none" id="ddlSellingCode" class="field_input"
                                                                                tabindex="7" onchange="CallWebMethod('sellcatcode')" runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                        <td style="width: 265px; display: none" align="left">
                                                                            <select style="width: 237px; display: none" id="ddlSellingName" class="field_input"
                                                                                tabindex="8" onchange="CallWebMethod('sellcatname')" runat="server">
                                                                                <option selected></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblcurrency" runat="server" Text="Currency" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcurrencyname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcurrencycode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncurrency" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Currency_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcurrencylist" TargetControlID="txtcurrencyname" OnClientItemSelected="currencyautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblcountry" runat="server" Text="Country" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcountryname" runat="server"  CssClass="field_input"
                                                                             AutoPostBack ="true"   MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcountrycode" runat="server" style="display:none"  ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncountry" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Country_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                 ServiceMethod="Getcountrylist"   TargetControlID="txtcountryname" OnClientItemSelected="countryautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblcity" runat="server" Text="City" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcityname" runat="server"  CssClass="field_input"
                                                                               AutoPostBack ="true"  MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcitycode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncity" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="City_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcitylist" TargetControlID="txtcityname" OnClientItemSelected="cityautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblsector" runat="server" Text="Sector" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtsectorname" runat="server"  CssClass="field_input"
                                                                              AutoPostBack ="true"   MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtsectorcode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnsector" runat="server" />
                                                                           <asp:AutoCompleteExtender ID="Sector_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getsectorlist" TargetControlID="txtsectorname" OnClientItemSelected="sectorautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text11" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text12" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblhotelchain" runat="server" Text="Hotelchain Code" Width="90px"></asp:Label>
                                                                        </td>
                                                                          
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                         <asp:TextBox ID="txthotelchainname" runat="server"  
                                                                                CssClass="field_input" MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                          
                                                                            <asp:TextBox ID="txthotelchaincode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnchaincode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Hotelchain_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Gethotelchainlist" TargetControlID="txthotelchainname" OnClientItemSelected="hotelchainautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text19" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text20" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblhotelstatus" runat="server" Text="Hotelstatus Code" Width="90px"></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txthotelstatusname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txthotelstatuscode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnstatuscode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Hotelstatus_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Gethotelstatuslist" TargetControlID="txthotelstatusname" OnClientItemSelected="hotelstatusautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text21" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text22" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="Label1" runat="server" Text="Property Type" Width="90px"></asp:Label>
                                                                          <%--  <span style="color: #ff0000">*</span>--%>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtpropertytypename" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtpropertytypecode" runat="server" style="display:none" ></asp:TextBox>
                                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="PropertyType_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getpropertytypelist" TargetControlID="txtpropertytypename" OnClientItemSelected="propertytypeautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text23" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text24" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtareaname" runat="server"  CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px" Visible="False"></asp:TextBox>
                                                                            <asp:TextBox ID="txtareacode" runat="server" style="display:none"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnareacode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="AreaCode_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getarealist" TargetControlID="txtareaname" OnClientItemSelected="areaautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text15" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text16" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <%--  Added Area for AreaCode and AreaName by Archana on 19/05/2015--%>
                                                                    <tr>
                                                                        <td style="width: 140px" align="left">
                                                                            <strong>Active</strong>
                                                                        </td>
                                                                        <td style="width: 88px" align="left">
                                                                            <input id="chkActive" tabindex="17" type="checkbox" checked runat="server" />
                                                                           
                                                                        </td>
                                                                         <td style="width: 140px" align="left">
                                                                       
                                                                         <asp:Label ID="lblvat" runat="server" Text="VAT Excluded" Width="140px"></asp:Label>
                                                                        <input id="chkvat" tabindex="18" type="checkbox" runat="server" value="" />
                                                                        </td>
                                                                         
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 140px" align="left">
                                                                            <strong>Preferred</strong>
                                                                        </td>
                                                                        <td style="width: 88px" align="left">
                                                                            <input id="ChkPreferred" tabindex="18" type="checkbox" runat="server" value="" />
                                                                        </td>
                                                                        <td style="width: 265px" align="left">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 140px" align="left">
                                                                            <strong>Showin Web</strong>
                                                                        </td>
                                                                        <td style="width: 88px" align="left">
                                                                            <input id="chkshow" tabindex="18" type="checkbox" runat="server" value="" />
                                                                        </td>
                                                                      
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 140px" align="left">
                                                                            Order
                                                                        </td>
                                                                        <td style="width: 88px" align="left">
                                                                            <input style="width: 223px; text-align: right" id="txtOrder" class="field_input"
                                                                                tabindex="19" type="text" maxlength="4" runat="server" />
                                                                        </td>
                                                                        <td style="width: 265px" align="left">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 88px" align="left">
                                                                            <asp:Label ID="lblControlaccode" runat="server" Text="Control A/C" Width="70px"></asp:Label><span
                                                                                style="color: #ff0000">*</span>
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtcontrolaccname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px"></asp:TextBox>
                                                                            <asp:TextBox ID="txtcontrolacccode" runat="server" ></asp:TextBox>
                                                                            <asp:HiddenField ID="hdncontrolacccode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="ControlAcc_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getcontrolacclist" TargetControlID="txtcontrolaccname" OnClientItemSelected="controlautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text13" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text14" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                   
                                                                    
                                                                    
                                                                    <tr>
                                                                        <td style="width: 140px" align="left"><b>Tax Detail</b> </TD>

                                                                        <td style="WIDTH: 88px" align="left"> 
                                                                        <%--<asp:DropDownList runat="server"I boxesD="ddlTax" OnSelectedIndexChanged >
                                                                        <asp:ListItem Selected="True" Text="Taxable" Value="Taxable"></asp:ListItem>
                                                                         <asp:ListItem  Text="Exempted" Value="Exempted"></asp:ListItem>
                                                                          <asp:ListItem  Text="ZeroRated" Value="ZeroRated"></asp:ListItem>
                                                                        </asp:DropDownList>--%>

                                                                        <asp:DropDownList ID="ddlTax" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTax_OnSelectedIndexChanged">
                                                                          <asp:ListItem Value="Taxable" selected="True" Text="Taxable"></asp:ListItem>
                                                                           <asp:ListItem Value="Exempted" Text="Exempted" ></asp:ListItem>
                                                                           <asp:ListItem Value="ZeroRated" Text="ZeroRated"></asp:ListItem>    
                                                                               <%--  <asp:ListItem Value="NonRegistered" Text="Non Registered"></asp:ListItem> Tanvir 19062023 --%>
                                                                                     <asp:ListItem Value="OutofScope" Text="Out of Scope"></asp:ListItem>   
                                                                                     <asp:ListItem Value="RC" Text="RCM"></asp:ListItem>                                                                       
                                                                        </asp:DropDownList>
                                                                        </td>
                                                                        <%--<td style="WIDTH: 88px" align="left"> 
                                                                    <asp:RadioButton  runat="server" ID="rdotaxable" GroupName="tax" Checked="true" Text="Taxable"  onclick="handleClick(this);" />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                     
                                                                    <asp:RadioButton  runat="server"  ID="rdoexempted" GroupName="tax" Text="Exempted" onclick="handleClick(this);"  />
                                                                    
                                                                    </td>
                                                                    <td style="WIDTH: 88px" align="left">
                                                                    <asp:RadioButton  runat="server"  ID="rdozerorated" GroupName="tax" Text="Zero Rated" onclick="handleClick(this);"  />
                                                                    
                                                                     </td>    --%>                                                                                                                                 
                                                                  
                                                                    
                                                                        </tr>
                                                                        
                                                                        <tr>
                                                                        <td style="width: 140px" align="left" runat="server" id="lblservicecharges">Service Charges</td>
                                                                        <td style="WIDTH: 88px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtServiceCharges" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /><asp:Label ID="lblperc1" runat="server" Text=" %"></asp:Label></td>
                                                                        </tr>

                                                                        <tr>
                                                                        <td style="width: 140px" align="left" runat="server" id="lblmncpltyfees">Municipality Fees</TD>
                                                                        <td style="WIDTH: 88px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="TxtMunicipalityFees" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /><asp:Label ID="lblperc2" runat="server" Text=" %"></asp:Label></td>
                                                                        </tr>

                                                                        <tr>
                                                                        <td style="width: 140px" align="left" runat="server" id="lbltourismfees">Tourism Fees</TD>
                                                                        <td style="WIDTH: 88px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtTourismFees" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /><asp:Label ID="lblperc3" runat="server" Text=" %"></asp:Label></td>
                                                                        </tr>

                                                                        <tr>
                                                                        <td style="width: 140px" align="left" runat="server" id="lblvtax">VAT</TD>
                                                                        <td style="WIDTH: 88px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtVAT" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /><asp:Label ID="lblperc4" runat="server" Text=" %"></asp:Label></td>
                                                                        </tr>
                                                                        
                                                                    <tr>

                                                                        <td style="width: 88px" align="left">
                                                                        </td>
                                                                        <td align="left" valign="top" colspan="2" width="300px">
                                                                            <asp:TextBox ID="txtaccrualname" runat="server" AutoPostBack="True" CssClass="field_input"
                                                                                MaxLength="500" TabIndex="3" Width="300px" Visible="False"></asp:TextBox>
                                                                            <asp:TextBox ID="txtaccrualcode" runat="server" Style="display: none"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnaccrualcode" runat="server" />
                                                                            <asp:AutoCompleteExtender ID="Accrual_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                                ServiceMethod="Getaccruallist" TargetControlID="txtaccrualname" OnClientItemSelected="accrualautocompleteselected">
                                                                            </asp:AutoCompleteExtender>
                                                                            <input style="display: none" id="Text17" class="field_input" type="text" runat="server" />
                                                                            <input style="display: none" id="Text18" class="field_input" type="text" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 140px" align="left">
                                                                            <asp:Button ID="btnSave" TabIndex="24" OnClick="btnSave_Click" runat="server" Text="Save"
                                                                                CssClass="field_button" Width="93px"></asp:Button>
                                                                        </td>
                                                                        <td style="width: 230px" align="left">
                                                                            <asp:Button ID="btnCancel" TabIndex="25" OnClick="btnCancel_Click" runat="server"
                                                                                Text="Return To Search" CssClass="field_button" Width="139px"></asp:Button>&nbsp;&nbsp;
                                                                            <asp:Button ID="btnhelp" TabIndex="26" OnClick="btnhelp_Click" runat="server" Text="Help"
                                                                                CssClass="field_button"></asp:Button>
                                                                        </td>
                                                                        <td style="width: 265px" align="left">
                                                                            <input id="txtconnection" type="text" runat="server" style="visibility: hidden; width: 0px;" />
                                                                            <asp:Button ID="dummyCity" runat="server" Style="display: none;" />
                                                                            <asp:Button ID="dummyCityArea" runat="server" Style="display: none;" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    
                                                                    <td></td>
                                                                    
                                                                    </tr>
                                                                      <tr>
                                                                    
                                                                    <td></td>
                                                                    
                                                                    </tr>

                                                                    <tr>
                                                                    <td style="width: 150px" align="left">
                                                                            <asp:Button ID="btnCategory" TabIndex="27" OnClick="btnCategory_Click" 
                                                                                runat="server" Text="Add New Category"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                      <td style="width: 150px" align="left" colspan="2">
                                                                            <asp:Button ID="btnCurrency" TabIndex="27" OnClick="btnCurrency_Click" 
                                                                                runat="server" Text="Add New Currency"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                  
                                                                            <asp:Button ID="btnCountry" TabIndex="27" OnClick="btnCountry_Click" 
                                                                                runat="server" Text="Add New Country"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td style="width: 150px" align="left">
                                                                            <asp:Button ID="btnCity" TabIndex="27" OnClick="btnCity_Click" 
                                                                                runat="server" Text="Add New City"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                        </td>
                                                                         <td style="width: 150px" align="left" colspan="2">
                                                                            <asp:Button ID="btnSector" TabIndex="27" OnClick="btnSector_Click" 
                                                                                runat="server" Text="Add New Sector"
                                                                                CssClass="field_button" Width="147px"></asp:Button>
                                                                  
                                                                            <asp:Button ID="btnHotelChain" TabIndex="27" OnClick="btnHotelChain_Click" 
                                                                                runat="server" Text="Add New Hotelchain"
                                                                                CssClass="field_button" Width="160px"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <script language="javascript">
                                            formmodecheck();
                                            load();
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </table>
                </div>
                </td>
                </tr>
         
                </tbody>
            </table>
 <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</contenttemplate>
    </asp:UpdatePanel>
    </asp:Content>