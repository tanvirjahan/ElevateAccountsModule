
<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustMainDet.aspx.vb" Inherits="CustMainDet"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

           AutoCompleteExtenderKeyUp();
           AgentCatAutoCompleteExtenderKeyUp();
         MarketAutoCompleteExtenderKeyUp();
           CurrencyAutoCompleteExtenderKeyUp();
           CtryAutoCompleteExtenderKeyUp();
           CityAutoCompleteExtenderKeyUp();
           SectorAutoCompleteExtenderKeyUp();
          // SalesPersonResAutoCompleteExtenderKeyUp();
           SalesPersonConAutoCompleteExtenderKeyUp();
           ContAccAutoCompleteExtenderKeyUp();
           DivisionAutoCompleteExtenderKeyUp();
       });



       

        </script>

<script type="text/javascript">

    function AutoCompleteExtenderKeyUp() {

        $("#<%= TxtSalespersonresname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtSalespersonresname.ClientID%>').value == '') {

                document.getElementById('<%=TxtSalesPersonRescode.ClientID%>').value = '';
            }

        });

        $("#<%= TxtSalespersonresname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtSalespersonresname.ClientID%>').value == '') {

                document.getElementById('<%=TxtSalesPersonRescode.ClientID%>').value = '';
            }

        });

    }


   
    function SalesPersonResautocompleteselected(source, eventArgs) {
                if (eventArgs != null) {
            document.getElementById('<%=TxtSalesPersonRescode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtSalesPersonRescode.ClientID%>').value = '';
        }

    }

    function AgentCatAutoCompleteExtenderKeyUp() {

        $("#<%= TxtAgentCatname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtAgentCatname.ClientID%>').value == '') {

                document.getElementById('<%=TxtAgentCatCode.ClientID%>').value = '';
            }

        });

        $("#<%= TxtAgentCatname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtAgentCatname.ClientID%>').value == '') {

                document.getElementById('<%=TxtAgentCatCode.ClientID%>').value = '';
            }

        });

    }
    function MarketAutoCompleteExtenderKeyUp() {

        $("#<%= TxtMarketname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtMarketname.ClientID%>').value == '') {

                document.getElementById('<%=TxtMarketcode.ClientID%>').value = '';
            }

        });

        $("#<%= TxtMarketname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtMarketname.ClientID%>').value == '') {

                document.getElementById('<%=TxtMarketCode.ClientID%>').value = '';
            }

        });

    }
    function MarketAutoCompleteselected(source, eventArgs) {
        if (eventArgs != null) {
        
            document.getElementById('<%=TxtMarketCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtMarketCode.ClientID%>').value = '';
        }

    }


    function AgentCatAutoCompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtAgentCatCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtAgentCatCode.ClientID%>').value = '';
        }

    }


    function CurrencyAutoCompleteExtenderKeyUp() {

        $("#<%= Txtcurrname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=Txtcurrname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
            }

        });

        $("#<%= Txtcurrname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=Txtcurrname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
            }

        });

    }

   



    function currencyautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=Txtcurrcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=Txtcurrcode.ClientID%>').value = '';
        }

    }

    function CtryAutoCompleteExtenderKeyUp() {

        $("#<%= Txtctryname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=Txtctryname.ClientID%>').value == '') {

                document.getElementById('<%=Txtctrycode.ClientID%>').value = '';
            }

        });

        $("#<%= Txtctryname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=Txtctryname.ClientID%>').value == '') {

                document.getElementById('<%=Txtctrycode.ClientID%>').value = '';
            }

        });

    }
   

    function ctryautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=Txtctrycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=Txtctrycode.ClientID%>').value = '';
        }

    }

    function CityAutoCompleteExtenderKeyUp() {

        $("#<%= Txtcityname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=Txtcityname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcitycode.ClientID%>').value = '';
            }

        });

        $("#<%= Txtcityname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=Txtcityname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcitycode.ClientID%>').value = '';
            }

        });

    }
   

    function cityautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=Txtcitycode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=Txtcitycode.ClientID%>').value = '';
        }

    }


    function SectorAutoCompleteExtenderKeyUp() {

        $("#<%= Txtsectorname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=Txtsectorname.ClientID%>').value == '') {

                document.getElementById('<%=Txtsectorcode.ClientID%>').value = '';
            }

        });

        $("#<%= Txtsectorname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=Txtsectorname.ClientID%>').value == '') {

                document.getElementById('<%=Txtsectorcode.ClientID%>').value = '';
            }

        });

    }
   



    function sectorautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=Txtsectorcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=Txtsectorcode.ClientID%>').value = '';
        }

    }

    function SalespersonconAutoCompleteExtenderKeyUp() {

        $("#<%= TxtSalesPersonConName.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtSalesPersonConName.ClientID%>').value == '') {

                document.getElementById('<%=TxtSalesPersonConCode.ClientID%>').value = '';
            }

        });

        $("#<%=TxtSalesPersonConName.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtSalesPersonConName.ClientID%>').value == '') {

                document.getElementById('<%=TxtSalesPersonConCode.ClientID%>').value = '';
            }

        });

    }


   
    function salespersonconautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtSalesPersonConCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtSalesPersonConCode.ClientID%>').value = '';
        }

    }
    function DivisionAutoCompleteExtenderKeyUp() {

        $("#<%= TxtDivisionName.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtDivisionName.ClientID%>').value == '') {

                document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
            }

        });

        $("#<%=TxtDivisionName.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtDivisionName.ClientID%>').value == '') {

                document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
            }

        });

    }


  

    function divisionautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtDivisionCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
        }

    }

    function ContAccAutoCompleteExtenderKeyUp() {

        $("#<%= TxtControlAccName.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtControlAccName.ClientID%>').value == '') {

                document.getElementById('<%=TxtControlAccCode.ClientID%>').value = '';
            }

        });

        $("#<%=TxtControlAccName.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtControlAccName.ClientID%>').value == '') {

                document.getElementById('<%=TxtControlAccCode.ClientID%>').value = '';
            }

        });

    }

   
    function controlaccautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtControlAccCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtControlAccCode.ClientID%>').value = '';
        }

    }



    function TimeOutHandler(result) {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result) {
        var msg = result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }

</script>
<script language="javascript" type="text/javascript" >
  

    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txtCustomerCode.ClientID%>");
         var vartxtname = document.getElementById("<%=txtCustomername.ClientID%>");

        if ((vartxtcode.value == '') || (vartxtname.value == '')) {
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


    //===================Validation For Main Detail Form======================================================

    
    function FormValidationMainDetail(state) {

        if ((document.getElementById("<%=txtCustomerName.ClientID%>").value == "") || (document.getElementById("<%=txtshortname.ClientID%>").value == "")  {

            if (document.getElementById("<%=txtCustomerName.ClientID%>").value == "") {
                alert("Name field can not be blank");
                document.getElementById("<%=txtCustomerName.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtshortname.ClientID%>").value == "") {
                document.getElementById("<%=txtshortname.ClientID%>").focus();
                alert("Short Name field can not be blank");
                return false;
            }


       
          

         
    
        }

        else {
            if (state == 'New') { if (confirm('Are you sure you want to save customer main details?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update customer main details?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete customer?') == false) return false; }
        }

    }

    function checkNumber(e) {
        if (event.keyCode < 45 || event.keyCode > 57) {
            return false;
        }
    }


    function checkCharacter(e) {

        //  if (event.keyCode == 32 || event.keyCode == 46)
        //alert(event.keyCode);
        //      return;
        //  if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
        //      return false;
        //  }

    }

  





</script>
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();
        AgentCatAutoCompleteExtenderKeyUp();
        CurrencyAutoCompleteExtenderKeyUp();
        CtryAutoCompleteExtenderKeyUp();
        CityAutoCompleteExtenderKeyUp();
        SectorAutoCompleteExtenderKeyUp();
        // SalesPersonResAutoCompleteExtenderKeyUp();
        SalesPersonConAutoCompleteExtenderKeyUp();
        ContAccAutoCompleteExtenderKeyUp();
        divisionAutoCompleteExtenderKeyUp();
    }
</script>
 <asp:UpdatePanel id="UpdatePanel2" runat="server" >
        <contenttemplate>
<table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 1000px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align="left"><tbody><tr><td valign="top" align="center" width="150" colspan="2">
    <asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" Width="800px" CssClass="field_heading"></asp:Label>
    </td></tr>
    <tr>
    <td style="WIDTH: 15%" valign="top" align="left"><span style="COLOR: #ff0000" class="td_cell"></span></td>
    <td style="WIDTH: 85%;" class="td_cell" valign="top" align="left">
    <table>
    <tr>
   <td>
    code <span style="COLOR: #ff0000" class="td_cell">*&nbsp; </span>
    <input style="WIDTH: 275px; height: 20px;" id="txtCustomerCode" readonly="readonly"  class="field_input" tabindex="1" type="text" maxlength="20" runat="server" />
       </td>
         <td>
    <span style="COLOR: #ff0000" class="td_cell">&nbsp; </span><asp:Label  ID="LblExtappid" runat="server" Visible ="false" Height ="18px"  />
 
  
     <asp:Label  ID="Extappspan" runat="server" Visible ="false" style="color: #ff0000" text="*" Height ="18px"  />
        <input style="WIDTH: 275px; height: 20px;" id="TxtExtappid"  class="field_input" tabindex="1" type="text" maxlength="20" runat="server" visible="False" />
    <br /> 
    </td>
        </tr>
    <tr>

  <td>

 
    Name &nbsp;&nbsp;

    <asp:TextBox id="txtCustomerName" tabIndex="2" runat="server"  
            CssClass="field_input" MaxLength="100" Width="275px" Height="19px" ></asp:TextBox>
<%--<INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" />--%>
 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtCustomerName" FilterMode="InvalidChars" 
 FilterType="Custom" InvalidChars="+-=/_#@$%^()!*<>?:;'{}[]" runat="server"></cc1:FilteredTextBoxExtender>
   </td>
   <td >
  &nbsp;&nbsp;<span style="FONT-SIZE:8pt; FONT-FAMILY: Arial" >Market</span>
                                <span style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                       
                   <%-- <td align="left" valign="top" colspan="1" width="300px">--%>
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="TxtMarketName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="4" Width="276px"></asp:TextBox>
                            <asp:TextBox ID="TxtMarketcode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdnmarketcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="GetMarketlist" TargetControlID="TxtMarketName" OnClientItemSelected="MarketAutoCompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text19" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text20" class="field_input" type="text"
                             runat="server" />


    <asp:Button ID="btnExistClient" Text ="Select Existing Client " runat ="server" Visible="false" CssClass="field_button"
        Width="142px" />
        </td>
           </tr>
        </table>
        </td>
 
</tr>
<tr><td style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%;" class="td_cell" vAlign=top align=left> 
    &nbsp;&nbsp;<asp:Label  ID="lblDisplay" runat="server" Visible ="false" Height ="18px" ForeColor ="Red" Font-Bold ="true"/>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label  ID="lblAgentName" runat="server" Visible ="false" Text ="Existing Client Name" Height ="18px"/> <asp:DropDownList ID="ddlagentname" runat ="server" AutoPostBack ="true"  CssClass ="field_input " Width="213px" Visible ="false" ></asp:DropDownList> </td>
 </tr>
<%--<TR> <td><INPUT style="WIDTH: 213px" id="Text2" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" />
        <asp:Button /><asp:Button runat ="server" />
    </td>
    </TR>--%>
<TR>
   
    <td align="left" style="WIDTH: 15%" valign="top">
        &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
    </td>
    <td style="WIDTH: 85%" valign="top">
        <div ID="iframeINF" runat="server" style="WIDTH: 600px">
            <asp:Panel ID="PanelMain" runat="server" GroupingText="Main Details" 
                Width="600px">
                <table style="WIDTH: 454px; margin-right: 0px;">
                    <tbody>
                        <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE:8pt; FONT-FAMILY: Arial">Short Name</span>
                                <span style="FONT-SIZE:8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                            </td>
                            <td align="left" style="width: 1362px">
                            <asp:TextBox id="txtshortname" style="WIDTH: 297px" tabIndex="3" runat="server"  
                                    CssClass="field_input" MaxLength="100" Width="300px" ></asp:TextBox>
                                <%--<INPUT style="WIDTH: 188px" id="txtshortname" class="field_input" tabIndex=3 type=text runat="server" />--%>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtshortname"
                                 FilterMode="InvalidChars"  FilterType="Custom" InvalidChars="+-=/_#@$%^()!*<>?:;'{}[]" runat="server"></cc1:FilteredTextBoxExtender>
                                   <asp:HiddenField ID="txtCode" runat="server" />
                            </td>
                        
                        </tr>



                                          <tr>
    <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE:8pt; FONT-FAMILY: Arial">Category</span>
                                <span style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                            </td>
                    <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtAgentCatName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="4" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtAgentCatCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdnCatCode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtAgentCatName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="GetAgentCatlist" TargetControlID="TxtAgentCatName" OnClientItemSelected="AgentCatAutoCompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
            </tr>
            <tr> 
            <td align="left" class="field_input" style="WIDTH: 2339px;">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Currency
                                <span style="COLOR: #ff0000">*</span></span>
                            </td><td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtCurrName" runat="server" AutoPostBack="True" 
                            CssClass="field_input" MaxLength="500" TabIndex="5" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtCurrCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdnCurrcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtCurrName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcurrencylist" TargetControlID="TxtCurrName" OnClientItemSelected="currencyautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text5" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text6" class="field_input" type="text"
                             runat="server" />
                    </td>
                       <%--   <td align="left" style="WIDTH: 100px">
                      
                            </td> --%>
                        </tr>
            
                      <tr>
    <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Country
                                <span style="COLOR: red">*</span></span>
                            </td>          <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtCtryName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="6" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtCtryCode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="hdnCtrycode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtCtryName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getctrylist" TargetControlID="TxtCtryName" OnClientItemSelected="ctryautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text1" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text2" class="field_input" type="text"
                             runat="server" />
                    </td>
</tr>  
                      
                    
                        <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">City</span>
                                <span class="td_cell" 
                                    style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial">*</span>
                            </td>
                        
                                              <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtCityName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="7" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtCityCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdncitycode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtCityName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcitylist" TargetControlID="TxtCityName" OnClientItemSelected="cityautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text7" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text8" class="field_input" type="text"
                             runat="server" />
                    </td>
                     <%--      <td align="left" style="WIDTH: 61px">
                            
                            </td>--%>
                        </tr>
                        <tr>
                               <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Sector</span>
                                </td>
                          <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtSectorName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="8" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtSectorCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdnsectorcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtSectorName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getsectorlist" TargetControlID="TxtSectorName" OnClientItemSelected="sectorautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text9" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text10" class="field_input" type="text"
                             runat="server" />
                    </td>
                        <%--   <td align="left" style="WIDTH: 61px">
                              
                            </td>--%>
                        </tr>
                        <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Reservation Contact
                                <span style="COLOR: red">*</span></span>
                            </td>
                                           <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtSalesPersonResName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="9" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtSalesPersonResCode" runat="server" style="display:none"></asp:TextBox>
                            <asp:HiddenField ID="hdnsalespcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtSalesPersonResName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="GetSalespersonlist" TargetControlID="TxtSalesPersonResName" OnClientItemSelected="SalesPersonResautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text11" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text12" class="field_input" type="text"
                             runat="server" />
                    </td>
                   <%--      <td align="left" style="WIDTH: 61px">


                            </td>--%>
                        </tr>




                        <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Sales Contact
                                <span style="COLOR: red">*</span></span>
                            </td>
                                           <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtSalesPersonConName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="10" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtSalesPersonConCode" runat="server" style="display:none"  ></asp:TextBox>
                            <asp:HiddenField ID="hdnsalesconcode" runat="server"  />
                     <asp:AutoCompleteExtender ID="TxtSalesPersonConName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getsalespersonlist" TargetControlID="TxtSalesPersonConName" OnClientItemSelected="salespersonconautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text13" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text14" class="field_input" type="text"
                             runat="server" />
                    </td>
                        <%--  <td align="left" style="WIDTH: 61px">
                       
                            </td>   --%>                      </tr>

                        <tr>
                         <td align="left" style="WIDTH: 2339px">
                              
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">    Status :&nbsp;
                                </span>
                                  </td>
                             <td align="left" valign="top" colspan="1" width="300px">
                    
                         
                                <asp:Label ID="lbllockstatus" runat="server" CssClass="field_input" 
                                    ForeColor="Red" Width="180px"></asp:Label>
                            </td>
                          
                 <%--         <td style="display:none; width: 61px;">
                          
                            </td>
                       --%>
                        </tr>


                                     <tr>
                         <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Control a/c Code
                                <span style="COLOR: red">*</span></span>
                            </td>
                                              <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtControlAccName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="11" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtControlAccCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdncontacccode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtControlAccName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getcontrolacclist" TargetControlID="TxtControlAccName" OnClientItemSelected="controlaccautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text15" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text16" class="field_input" type="text"
                             runat="server" />
                    </td>
            <%--            <td align="left" style="WIDTH: 61px">
               
                            </td>--%>
                        </tr>
                                           <tr>
                         <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Division
                                <span style="COLOR: red">*</span></span>
                            </td>
                                              <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtDivisionName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="12" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtDivisionCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdndivisioncode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtDivisionName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getdivisionslist" TargetControlID="TxtDivisionName" OnClientItemSelected="divisionautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text17" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text18" class="field_input" type="text"
                             runat="server" />
                    </td>
                      <tr>
                         <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial"> Booking&nbsp; Engine&nbsp;&nbsp;&nbsp; Format
                                <span style="COLOR: red">&nbsp;*</span></span>
                            </td>
                   
                                                <td>
                        <asp:DropDownList ID="ddlbookengratetype" runat="server" TabIndex="13">
                         <asp:ListItem >[Select]</asp:ListItem>  
                            <asp:ListItem Value="INDIVIDUAL" Selected="True" >Individual Rates</asp:ListItem>
                            <asp:ListItem Value="CUMULATIVE">Cumulative Rates</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                            </tr> 
            <%--            <td align="left" style="WIDTH: 61px">
               
                            </td>--%>
                        </tr>
                                         <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Active</span>
                                <INPUT id="chkActive" tabIndex=14 type=checkbox CHECKED runat="server" />
                            </td>
                        <%--    <td align="left" style="width: 1362px">
                                
                            </td>--%>
                            <td align="left">
                                &nbsp;</td>
                        </tr>

                         <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">TRN No.</span>
                            </td>
                            <td align="left">  <asp:TextBox ID="txtTRNNo" runat="server"></asp:TextBox>
                                &nbsp;</td>
                        </tr>
                                      
                        <tr>
                            <td align="left" class="field_input" style="WIDTH: 2339px">
                                <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial;visibility:hidden">Market(For Itinerary).</span></td>
                            <td align="left">
                                <%-- <asp:DropDownList ID="ddlMarket" runat="server" TabIndex="13">
                         
                        </asp:DropDownList>--%>
                              <select ID="ddlMarket" runat="server" class="field_input" 

                                    style="WIDTH: 189px;visibility:hidden" 

                                    tabindex="13">
                                    <option selected=""></option>
                                </select>
                        </td>
                        </tr>
                                      
                        <tr>
                            <td align="left" class="field_input" colspan="2">
                                  <span style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Allow Booking in Reservation Module </span> <INPUT id="chkAllowBooking" tabIndex=14 type=checkbox CHECKED runat="server" /></td>
                        </tr>
                                      
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="webctry" runat="server" Visible="false"> Registered Country</asp:Label>
                                 
                                            <INPUT style="WIDTH: 51px" id="txtwebctry" class="field_input"  visible="false" tabIndex=2 type=text maxLength=100 runat="server" />
                                        </td>
                                    
                                        <td>
                                            <asp:Label ID="webcity" runat="server" Visible="false"> Registered City</asp:Label>
                                        </td>
                                        <td>
                                            <INPUT style="WIDTH: 109px" id="txtwebcity" class="field_input"  visible="false" tabIndex=2 type=text maxLength=100 runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                                </tbody>
                </table>
                        <table style="WIDTH: 454px; margin-right: 0px;">
                    <tbody>
                     <tr>
                            <td align="left" class="field_input" style="WIDTH: 2522px">
                                <asp:Button ID="btnSave_Main" runat="server" CssClass="field_button" 
                                    tabIndex="16" Text="Save" Width="62px" /> 
                                        <asp:Button ID="btnHelp" runat="server" CssClass="field_button" 
                                    onclick="btnHelp_Click" tabIndex="18" Text="Help" />
                            </td>
                            <td align="left" style="width: 1362px">
                                <asp:Button ID="btnCancel_Main" runat="server" CssClass="field_button" 
                                    onclick="btnCancel_Main_Click" tabIndex="17" Text="Return to Search" 
                                    Width="152px" />
                                
                            </td>
                       <%--   <td align="left" style="WIDTH: 61px">
                                
                            </td> --%>
                        </tr>
 
  <tr>
                        
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                                  
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                                  
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                        </tr>
                                                            <tr>
                                                               <td align="left" class="field_input" style="WIDTH: 1071px">
                                <asp:Button ID="btnaddnewagentcat" runat="server" CssClass="field_button" 
                                    tabIndex="28" Text="Add New Category" Width="153px" />
                            </td>
                                                   <td align="left" style="width: 1362px">                  
                                    <asp:Button ID="btnaddnewcurr" runat="server" CssClass="field_button" 
                                   tabIndex="30" Text="Add New Currency" Width="153px" />
                                   </td>
                                                 <td align="left" style="width: 1362px">
                                   
                                    <asp:Button ID="btnaddmarket" runat="server" CssClass="field_button" 
                                   tabIndex="30" Text="Add New Market" Width="160px" /> 
                            </td>
                       <%--   <td align="left" style="WIDTH: 61px">
                                
                            </td> --%>
                        </tr>
                        <tr>
                        
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                                  
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                                  
                                               <td align="left" style="width: 1362px; height: 2px;">
                                </td>
                        </tr>
                                                          
                                                          
                                                          
                                                            <tr>
                         
                
                                        
                                               <td align="left" style="width: 1362px">
                                <asp:Button ID="btnaddnewctry" runat="server" CssClass="field_button" 
                                    tabIndex="29" Text="Add New Country" Width="153px" />
                          </td>
                            <td align="left" class="field_input" style="WIDTH: 1071px">
                                <asp:Button ID="btnaddnewcity" runat="server" CssClass="field_button" 
                                    tabIndex="28" Text="Add New City" Width="155px" />
                            </td>
                            <td align="left" style="width: 1362px">
                                <asp:Button ID="btnaddnewsector" runat="server" CssClass="field_button" 
                                    tabIndex="29" Text="Add New Sector" Width="153px" />
                                    </td>
                      
                            
                           
                    
                        </tr>
                        </tbody>
                        </table> 
                <TABLE style="border: 0px solid gray; WIDTH: 386px; " class="td_cell" align=left><TBODY>
                <tr>
                            <td align="left" class="field_input" 
                                style="width: 1688px; display:none; height: 36px;">
                                Show Currency web</td>
                            <td align="left" style="width:1156px; height: 36px;">
                                <select ID="ddlwebcurrency" runat="server" class="field_input" 
                                    onchange="CallWebMethod('webcurrcode')" style="WIDTH: 189px;display:none;" 
                                    tabindex="12">
                                    <option selected=""></option>
                                </select>
                                         <select ID="ddlwebCurrencyName" runat="server" class="field_input" 
                                    onchange="CallWebMethod('webcurrname')" style="WIDTH: 189px;display:none;" 
                                    tabindex="13">
                                    <option selected=""></option>
                                </select>
                           
                            </td>
                           
                    <%--        <td align="left" style="height: 36px;"> 
                            
                                </td>--%>
                        </tr>

                        <tr style="display:none">
                            <td class="field_input" style="height: 46px; width: 1688px;">
                                Logo for Voucher,(Maxfile size should be less than 500kb).
                            </td>
                            <td style="height: 46px; width: 1156px;">
                                <asp:FileUpload ID="filelogo" runat="server" CssClass="field_input" />
                                <asp:Button ID="btnfileupload" runat="server" CssClass="field_button" 
                                    style="display:none" />
                            </td>
                        </tr>
                        <tr>
                            <td class="field_input" style="width: 1688px">
                                &nbsp;</td>
                            <td style="width: 1156px">
                                <asp:CheckBox ID="chkVoucher" runat="server" 
                                    Text="Remove Voucher Logo&amp;Save" Visible="False" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="field_input" style="width: 1688px">
                                Address for Voucher
                            </td>
                            <td style="width: 1156px">
                                <asp:TextBox ID="txtVoucheraddress" runat="server" CssClass="field_input" 
                                    Height="50px" style="WIDTH: 189px" TextMode="MultiLine"></asp:TextBox>
                                          <asp:Panel ID="imgpanel" runat="server" ScrollBars="Auto" Width="136px">
                                    <asp:Image ID="imgvoucher" runat="server" Height="100px" Visible="False" 
                                        Width="143px" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td field_input="" style="width: 1688px">
                                Login Allowed</td>
                            <td field_input="" style="width: 1156px">
                                <asp:CheckBoxList ID="ChkBoxloginbox" runat="server" 
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">UAE</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="OMN">Oman</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="ROTW">Rest of the World
                                    </asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    
                    </tbody>
                </table>
             <INPUT style="DISPLAY: none; width: 44px;" id="countryhdncode" class="field_input" type=text runat="server" visible="true" />
               <INPUT style="DISPLAY: none; width: 44px;" id="countryhdn" class="field_input" type=text runat="server" visible="true" />
                <INPUT style="DISPLAY: none; width: 44px;" id="currhiddcode" class="field_input" type=text runat="server" visible="true" />
                <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
        height: 9px" type="text" />

                <asp:Button ID="btnDummy" runat="server" CssClass="field_button" 
                    style="display:none;" Text="Dum" />
                <INPUT style="DISPLAY: none; width: 33px;" id="ctryhidd1" class="field_input" type=text runat="server" visible="true" />
                <INPUT style="DISPLAY: none; width: 20px;" id="cityhdn" class="field_input" type=text runat="server" visible="true" />
                <INPUT style="DISPLAY: none; width: 20px;" id="cityhdncode" class="field_input" type=text runat="server" visible="true" />
                <INPUT style="DISPLAY: none; width: 20px;" id="secthidd" class="field_input" type=text runat="server" visible="true" />
                <INPUT style="DISPLAY: none; width: 20px;" id="secthiddcode" class="field_input" type=text runat="server" visible="true" />
                <script language="javascript">
                    formmodecheck();

                    load();
                    
</script>
            </asp:Panel>
        </div>
    </td>
    </TR>
   
    </tbody></table>
 </contenttemplate>
 
 <Triggers>
           <asp:PostBackTrigger  ControlID="btnSave_Main" /> 
        </Triggers>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

