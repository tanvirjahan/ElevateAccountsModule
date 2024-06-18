<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Cities.aspx.vb" Inherits="Cities"  %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ OutputCache location="none" %> 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script type="text/jscript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js">
    </script>
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
            countryAutoCompleteExtenderKeyUp();
        });
</script>
<script language ="javascript" type="text/javascript">
    function checkNumber(e) {
        //        if ((event.keyCode < 47 || event.keyCode > 57)) {
        //            return false;
        //        }
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
        if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
            return false;
        }
    }

    function FormValidation(state) {
        var vSerCharge = document.getElementById("<%=txtServiceCharges.ClientID%>");
        var vMunciFee = document.getElementById("<%=TxtMunicipalityFees.ClientID%>");
        var vTourFee = document.getElementById("<%=txtTourismFees.ClientID%>");
        var vVAT = document.getElementById("<%=txtVAT.ClientID%>");

        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=txtorder.ClientID%>").value == "") || (document.getElementById("<%=txtorder.ClientID%>").value <= 0)) {



            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }


            else if (document.getElementById("<%=txtorder.ClientID%>").value == "") {
                alert("Order field can not be blank");
                document.getElementById("<%=txtorder.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtorder.ClientID%>").value <= 0) {
                alert("Order must be greater than zero.");
                document.getElementById("<%=txtorder.ClientID%>").focus();
                return false;
            }

        }

        else if (document.getElementById("<%=txtcountrycode.ClientID%>").value == "") {


            document.getElementById("<%=txtcountrycode.ClientID%>").focus();
            alert("Select Country Code");
            return false;
        }

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

        else {

            if (document.getElementById("<%=txtCode.ClientID%>").value != "")
            //               if (document.getElementById("<%=txtCode.ClientID%>").value.match(!/^[a-z]+$/));
            {
                var val = document.getElementById("<%=txtCode.ClientID%>").value;

                if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
                    alert('Only alphabets,digits,-,_,/ are allowed');
                    return false
                }


                return true;
            }

            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to save City type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update City type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete City type?') == false) return false; }
        }
    }

    function GetValueFrom() {

        var ddl = document.getElementById("<%=ddlcname.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlCCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetValueCode() {
        var ddl = document.getElementById("<%=ddlCCode.ClientID%>");
        //alert(document.getElementById("<%=ddlcname.ClientID%>").value)
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            //alert(ddl.options[i].text);
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlcname.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function assignvalue(source, eventargs) {
        var txt = document.getElementById("<%=txtAutoComplete.ClientID%>");

        alert(eventargs.get_value());
        eventargs.get_text()
    }



    function countryautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
        }
    }

    function countryAutoCompleteExtenderKeyUp() {
        $("#<%=txtcountryname.ClientID %>").bind("change", function () {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
        });
    }

</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        countryAutoCompleteExtenderKeyUp();
    }
</script>



<head>
<style >
   .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left:10px;
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

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 680px; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD style="HEIGHT: 5px" class="td_cell" align=center colSpan=4>
<asp:Label id="lblHeading" runat="server" Text="Add New Cites" Width="100%" CssClass="field_heading"></asp:Label></TD>
</TR>
<TR>
<TD  class="td_cell">Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 37px; COLOR: #000000">
<INPUT  id="txtCode" class="field_input" style="width:196px"  tabIndex=1 type=text maxLength=20 runat="server" /> </TD>
<TD style="COLOR: #000000"></TD>
<TD style="WIDTH: 279px; COLOR: #000000"></TD>
</TR>















<TR>
<TD  class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 37px; HEIGHT: 8px">
<INPUT style="WIDTH: 196px" id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /></TD>
<TD style="HEIGHT: 8px"></TD>
<TD style="HEIGHT: 8px"></TD>
</TR>

<tr>
<TD style="WIDTH: 200px; HEIGHT: 16px" class="td_cell">Country<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txtcountryname" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="500" TabIndex="3" Width="196px"></asp:TextBox>
<asp:TextBox ID="txtcountrycode" runat="server" style="display:none" ></asp:TextBox>
<asp:HiddenField ID="hdncountry" runat="server" />
<asp:AutoCompleteExtender ID="Country_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
ServiceMethod="Getcountrylist" TargetControlID="txtcountryname" OnClientItemSelected="countryautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
</td>
</tr>












<TR style="display:none">
<TD  class="td_cell">Country Code<SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
<TD style="WIDTH: 37px; HEIGHT: 8px" align=left>
<SELECT  id="ddlCCode" class="drpdown" tabIndex=3 onchange="GetValueFrom()" runat="server">
<OPTION selected></OPTION></SELECT>&nbsp;</TD>
<TD  class="td_cell">Country Name</TD>
<TD style="WIDTH: 279px" align=left>
<SELECT style="WIDTH: 318px" id="ddlcname" class="drpdown" tabIndex=4 onchange="GetValueCode()" runat="server">
<OPTION selected>
</OPTION>
</SELECT>
</TD>
</TR>
<TR>
<TD  class="td_cell">Order <SPAN style="COLOR: #ff0000">*</SPAN></TD>
<TD style="WIDTH: 37px; HEIGHT: 5px">
<INPUT style="WIDTH: 196px; TEXT-ALIGN: right" id="txtorder" class="txtbox" tabIndex=5 onkeypress="return checkCharacter()" type=text maxLength=31 onchange="ValidationForExchate()" runat="server" /></TD>
<TD style="WIDTH: 855px; HEIGHT: 5px"></TD>
<TD style="WIDTH: 279px; HEIGHT: 5px"></TD>
</TR>
<TR>
<TD  class="td_cell">Active</TD>
<TD style="WIDTH: 37px">
<INPUT id="chkActive" tabIndex=6 type=checkbox CHECKED runat="server" /></TD>
<TD style="WIDTH: 855px"></TD><TD style="WIDTH: 279px"></TD>
</TR>
<TR>
<TD  class="td_cell" style="width:200px">Show In Web</TD><TD style="WIDTH: 37px">
<INPUT id="chk_showinweb" tabIndex=7 type=checkbox CHECKED runat="server" /></TD>
<TD style="WIDTH: 855px"></TD><TD style="WIDTH: 279px"></TD>
</TR>
 
<TR>
<TD  class="td_cell" style="width:200px"><b>Tax Detail</b> </TD>
<TD style="WIDTH: 37px"></TD>
<TD style="WIDTH: 855px"></TD>
<TD style="WIDTH: 279px"></TD>
</TR>

<TR>
<TD  class="td_cell" style="width:200px">Service Charges</TD>
<TD style="WIDTH: 37px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtServiceCharges" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /> %</TD>
<TD style="WIDTH: 855px"></TD>
<TD style="WIDTH: 279px"></TD>
</TR>

<TR>
<TD  class="td_cell" style="width:200px">Municipality Fees</TD>
<TD style="WIDTH: 37px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="TxtMunicipalityFees" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /> %</TD>
<TD style="WIDTH: 855px"></TD>
<TD style="WIDTH: 279px"></TD>
</TR>

<TR>
<TD  class="td_cell" style="width:200px">Tourism Fees</TD>
<TD style="WIDTH: 37px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtTourismFees" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /> %</TD>
<TD style="WIDTH: 855px"></TD>
<TD style="WIDTH: 279px"></TD>
</TR>

<TR>
<TD  class="td_cell" style="width:200px">VAT</TD>
<TD style="WIDTH: 37px"><input style="WIDTH: 170px; TEXT-ALIGN: right" id="txtVAT" class="txtbox" onkeypress="return checkNumber()" type="text" runat="server" /> %</TD>
<TD style="WIDTH: 855px"></TD>
<TD style="WIDTH: 279px"></TD>
</TR>

<TR>
<TD class="td_cell"><asp:Button id="btnSave" tabIndex=8 runat="server" Text="Save" CssClass="btn"></asp:Button></TD>
<TD style="WIDTH: 37px"><asp:Button id="btnCancel" tabIndex=9 onclick="btnCancel_Click" runat="server" Text="Return To Search" Width="196px" CssClass="btn"></asp:Button></TD>
<TD class="td_cell"><asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="btn" __designer:wfdid="w4"></asp:Button></TD>
<TD><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></TD></TR>


<tr>
            
                <td style="width: 150px" align="left" colspan="2">
              <asp:Button ID="btnCountry" TabIndex="27" OnClick="btnCountry_Click" 
               runat="server" Text="Add New Country"
            CssClass="field_button" Width="147px"></asp:Button></td>
 
 </tr>

</TBODY>



</TABLE>

<div ID="divwidth"></div>

<asp:TextBox ID="txtAutoComplete" runat="server" onkeyup = "SetContextKey()"  Width="252px" Visible="False"></asp:TextBox>   
<div ID="div1">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </services>
    </asp:ScriptManagerProxy>
            </div>
<cc1:AutoCompleteExtender runat="server" 
             ID="AutoComplete1"
             BehaviorID="AutoComplete1"
             UseContextKey = "true"
             TargetControlID="txtAutoComplete"
             ServicePath="~/clsServices.asmx" 
             ServiceMethod="GetCityCodeListnewtst"
             MinimumPrefixLength="1" 
             CompletionInterval="10"
             EnableCaching="true"
             CompletionSetCount="12"
             CompletionListCssClass="AutoExtender"
             CompletionListItemCssClass="AutoExtenderList"
             CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
             CompletionListElementID="divwidth"
              OnClientItemSelected ="assignvalue"
             FirstRowSelected="true">
</cc1:AutoCompleteExtender>
 <script type="text/javascript">
     function SetContextKey() {


         // alert(ctl00_Main_AutoComplete1);
         // $find('ctl00_Main_AutoComplete1').set_contextKey('ar'); 
         //  $find('AutoComplete1').set_contextKey('moo');
         //  var autoc = document.getElementById("<%=AutoComplete1.ClientID%>");

         $find("AutoComplete1").set_contextKey('dd');

         //         var autoc = document.getElementById('ctl00_Main_AutoComplete1');
         //   alert($find("AutoComplete1"));
         //  ctl00_Main_AutoComplete1.ContextKey = "AR"  
     }
 </script>

</contenttemplate>


    </asp:UpdatePanel>

     </asp:Content>
 