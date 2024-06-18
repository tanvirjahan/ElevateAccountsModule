<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustomerAgeingSummary.aspx.vb" Inherits="RptCustomerAgeingSummary" MasterPageFile="~/AccountsMaster.master" Strict ="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
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
  <script type="text/javascript" charset="utf-8">
      $(document).ready(function () {

          txtcategorynameAutoCompleteExtenderKeyUp();
          txtcustomernameAutoCompleteExtenderKeyUp();
          txtmarketnameAutoCompleteExtenderKeyUp();
          txtcountrynameAutoCompleteExtenderKeyUp();
          txtcontrolnameAutoCompleteExtenderKeyUp();
          txtcustgroupAutoCompleteExtenderKeyUp();

      });
  </script>
<script language="javascript" type="text/javascript">

    function FormValidation() {
        if ((document.getElementById("<%=txtFromDate.ClientID%>").value == "") || (document.getElementById("<%=ddlReportGroup.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlCurrency.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlReportOrder.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlCustomerType.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlAgeingType.ClientID%>").value == "[Select]")) {

            if (document.getElementById("<%=txtFromDate.ClientID%>").value == "") {
                document.getElementById("<%=txtFromDate.ClientID%>").focus();
                alert("As on date field can not be blank.");
                return false;
            }
            else if (document.getElementById("<%=ddlReportGroup.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlReportGroup.ClientID%>").focus();
                alert("Select report group.");
                return false;
            }
            else if (document.getElementById("<%=ddlCurrency.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlCurrency.ClientID%>").focus();
                alert("Select currency type.");
                return false;
            }
            else if (document.getElementById("<%=ddlReportOrder.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlReportOrder.ClientID%>").focus();
                alert("Select report order.");
                return false;
            }
            else if (document.getElementById("<%=ddlCustomerType.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlCustomerType.ClientID%>").focus();
                alert("Select customer order.");
                return false;
            }
            else if (document.getElementById("<%=ddlAgeingType.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlAgeingType.ClientID%>").focus();
                alert("Select ageing type.");
                return false;
            }
        }

    }

    function txtcustgroupAutoCompleteExtenderKeyUp() {
        $("#<%= Txtcustgroupname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=Txtcustgroupname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });

        $("#<%= Txtcustgroupname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=Txtcustgroupname.ClientID%>').value == '') {

                document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });
    }
    function txtcustomernameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcustomercode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
        }
    }

    function SetcustomerContextkey() {

        var contxt = '';

        var ctrycode = document.getElementById("<%=txtcountrycode.ClientID%>").value;
        var ctryname = document.getElementById("<%=txtcountryname.ClientID%>").value;
        var citycode = document.getElementById("<%=txtmarketcode.ClientID%>").value;
        var cityname = document.getElementById("<%=txtmarketname.ClientID%>").value;
        var custgroupcode = document.getElementById("<%=Txtcustgroupcode.ClientID%>").value;
        var custgroupname = document.getElementById("<%=Txtcustgroupname.ClientID%>").value;
        var categorycode = document.getElementById("<%=txtcategorycode.ClientID%>").value;
        var categoryname = document.getElementById("<%=txtcategoryname.ClientID%>").value;

        if (ctryname == '') {
            contxt = '';
        }
        else {
            contxt = ctrycode;
        }




        if (cityname == '') {
            contxt = contxt + '||' + '';
        }
        else {
            contxt = contxt + '||' + citycode;
        }


        if (custgroupname == '') {
            contxt = contxt + '||' + '';
        }
        else {
            contxt = contxt + '||' + custgroupcode;
        }


        if (categoryname == '') {
            contxt = contxt + '||' + '';
        }
        else {
            contxt = contxt + '||' + categorycode;
        }





        // $find('AutoCompleteExtender_txtBankName').set_contextKey(contxt);
        $find('<%=txtcustomername_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

    }


    function txtcustgroupnamecompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = eventArgs.get_value();
            SetcustomerContextkey();
            document.getElementById('<%=txtcustomername.ClientID%>').value = '';
            document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
        }
        else {
            document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';

        }
        SetcustomerContextkey();
    }

    function txtmarketnameAutoCompleteExtenderKeyUp() {
        $("#<%= txtmarketname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtmarketname.ClientID%>').value == '') {

                document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });

        $("#<%= txtmarketname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtmarketname.ClientID%>').value == '') {

                document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });
    }


    function txtcustomernameAutoCompleteExtenderKeyUp() {
        $("#<%= txtcustomername.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtcustomername.ClientID%>').value == '') {

                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });

        $("#<%= txtcustomername.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcustomername.ClientID%>').value == '') {

                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });
    }

    function txtcontrolnameAutoCompleteExtenderKeyUp() {
        $("#<%= txtcontrolname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtcontrolname.ClientID%>').value == '') {

                document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
            }

        });

        $("#<%= txtcontrolname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcontrolname.ClientID%>').value == '') {

                document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
            }

        });
    }

    function txtcategorynameAutoCompleteExtenderKeyUp() {
        $("#<%= txtcategoryname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });

        $("#<%= txtcategoryname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcategoryname.ClientID%>').value == '') {

                document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });
    }







    function txtcountrynameAutoCompleteExtenderKeyUp() {
        $("#<%= txtcountryname.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                SetcustomerContextkey();

            }

        });

        $("#<%= txtcountryname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                SetcustomerContextkey();
            }

        });
    }





    function txtmarketnameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%= txtmarketcode.ClientID%>').value = eventArgs.get_value();
            SetcustomerContextkey();
        }
        else {
            document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
            SetcustomerContextkey();
        }
    }


    function txtcontrolnameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcontrolcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
        }
    }


    function txtcategorynameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcategorycode.ClientID%>').value = eventArgs.get_value();
            SetcustomerContextkey();
        }
        else {
            document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
            SetcustomerContextkey();

        }
    }

    function txtcountrynamecompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
            $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey(eventArgs.get_value());
            document.getElementById('<%=txtmarketname.ClientID%>').value = '';
            document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
            SetcustomerContextkey();
        }
        else {
            document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
            $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
            SetcustomerContextkey();
        }
    }

    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }

</script>

  <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {
           txtcategorynameAutoCompleteExtenderKeyUp();
           txtmarketnameAutoCompleteExtenderKeyUp();
           txtcustomernameAutoCompleteExtenderKeyUp();
           txtcontrolnameAutoCompleteExtenderKeyUp();
           txtcountrynameAutoCompleteExtenderKeyUp();
           txtcustgroupAutoCompleteExtenderKeyUp();
       }
</script>
   
 
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%; BORDER-BOTTOM: gray 2px solid" id="TABLE1"><TBODY><TR><TD class="field_heading" align=center colSpan=1><asp:Label id="lblHeading" runat="server" Text="Customer Ageing Summary" ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label></TD></TR><TR><TD class="td_cell">
    <TABLE style="WIDTH: 100%"><TBODY><TR><TD vAlign=top>
           <asp:Panel id="Panel5" runat="server" CssClass="td_cell"  Width="100%" 
            __designer:wfdid="w22" GroupingText="Select">
        
        <TABLE style=" HEIGHT: 83px"><TBODY><TR><TD class="td_cell">&nbsp;As On Date</TD><TD class="td_cell">&nbsp;<asp:TextBox 
        id="txtFromDate" tabIndex=1 runat="server" CssClass="txtbox" Width="80px" 
        __designer:wfdid="w9"></asp:TextBox> <asp:ImageButton id="ImgBtnFrmDt" runat="server" __designer:wfdid="w10" ImageUrl="~/Images/Calendar_scheduleHS.png"></asp:ImageButton> <cc1:MaskedEditValidator id="MskVFromDt" runat="server" CssClass="field_error" __designer:wfdid="w11" TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required" EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator></TD><TD class="td_cell">
            Currency Type</TD><TD class="td_cell">
                <select id="ddlCurrency" runat="server" class="drpdown" name="D2" 
                    style="WIDTH: 109px" tabindex="2">
                    <option selected="" value="[Select]">[Select]</option>
                </select>
            </TD></TR><TR><TD class="td_cell">Report Group</TD><TD class="td_cell">
                <select id="ddlReportGroup" runat="server" class="drpdown" name="D1" 
                    style="WIDTH: 109px" tabindex="5">
                    <option selected="" value="[Select]">[Select]</option>
                    <option value="None">None</option>
                    <option value="Control A/C Code">Control A/C Code</option>
                    <option value="Category">Category</option>
                    <option value="Country">Country</option>
                    <option value="Market">City</option>
                </select>
                </TD><TD class="td_cell">Ageing Type</TD><TD class="td_cell">
    <SELECT style="WIDTH: 109px" id="ddlAgeingType" class="drpdown" tabIndex=3 
        runat="server"> <OPTION value="Month" selected>Month</OPTION><OPTION value="Date">Date</OPTION><OPTION value="Due Date">Due Date</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD></TR>
        <TR><TD class="td_cell">Report Order</TD><TD class="td_cell">
    <SELECT style="WIDTH: 109px" id="ddlReportOrder" class="drpdown" tabIndex=6 
        runat="server"> <OPTION value="[Select]" selected>[Select]</OPTION><OPTION value="Code">Code</OPTION><OPTION value="Name">Name</OPTION></SELECT></TD>

        <TD class="td_cell">Customer Type</TD>
        <TD class="td_cell">
    <SELECT style="WIDTH: 109px" id="ddlCustomerType" class="drpdown" tabIndex=4 
        runat="server"> <OPTION value="All" selected>All</OPTION><OPTION value="Cash Customer">Cash Customer</OPTION><OPTION value="Credit Customer">Credit Customer</OPTION></SELECT></TD>
         <%--Added Customer Type dropdown by Archana on 02/04/2015--%>
       </TR>

        <TR>
         <TD class="td_cell">&nbsp;</TD>
        <TD class="td_cell">
       <SELECT style="WIDTH: 109px" id="ddlIncludeZero" class="drpdown" tabIndex=6 
        runat="server" Visible="False"> 
        <OPTION value="Yes" selected>Yes</OPTION><OPTION value="No">No</OPTION><OPTION value="[Select]">[Select]</OPTION></SELECT></TD>
            <td class="td_cell">
                Include Proforma</td>
            <td>
                <select ID="ddlproforma" runat="server" class="field_input" name="D3" 
                    style="width: 109px" tabindex="1">
                    <option value="Yes">Yes</option>
                    <option selected="" value="No">No</option>
                </select></td>
        </TR>


         </TBODY></TABLE>
         </asp:Panel>
         </TD></TR>

         <tr><td>
       <asp:Panel id="Panel1" runat="server" CssClass="td_cell"  Width="100%"
            __designer:wfdid="w22" GroupingText="Select Criteria">
        

         <asp:Panel id="Panel22" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label1" runat="server" Text="Control A/C" Width="102px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="300px">
                      <asp:TextBox ID="txtcontrolname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="7" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcontrolcode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField1" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcontrolname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcontrolname" TargetControlID="txtcontrolname" OnClientItemSelected="txtcontrolnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text3" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text4" class="field_input" type="text" runat="server" />
                                      

                           
              </TD></TR>
            
             </TBODY></TABLE></asp:Panel>
                     
        <asp:Panel id="Panel3" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 700px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label4" runat="server" Text="Country" Width="60px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" Width="200px">
                      <asp:TextBox ID="txtcountryname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="8" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcountrycode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField4" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcountryname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcountry" TargetControlID="txtcountryname" OnClientItemSelected="txtcountrynamecompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
                                         </td>
                                       </tr> 
                                       </TBODY></TABLE></asp:Panel>
            
           <asp:Panel id="Panel2" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 700px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label3" runat="server" Text="City" Width="50px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="300px">
                      <asp:TextBox ID="txtmarketname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="9" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtmarketcode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField3" runat="server" />
                          <asp:AutoCompleteExtender ID="txtmarketname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getmarket" TargetControlID="txtmarketname" ContextKey ="True"  OnClientItemSelected="txtmarketnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr> 
                                       </TBODY></TABLE></asp:Panel>
       
               <asp:Panel id="Panel4" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 700px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label2" runat="server" Text="Category" Width="65px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="300px">
                      <asp:TextBox ID="txtcategoryname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="10" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcategorycode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField21" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcategoryname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcategory" TargetControlID="txtcategoryname" OnClientItemSelected="txtcategorynameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
                           
  
                            
             </TD></TR>
            
             </TBODY></TABLE></asp:Panel>
              
                 <asp:Panel id="Panel6" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 700px; HEIGHT: 28px"><TBODY><TR>

            <td style="width: 130px" align="left">    
                <asp:Label ID="Label6" runat="server" Text="Customer Group" ></asp:Label>
                <span style="color: #ff0000">*</span>
                </td>
            <td align="left" valign="top" colspan="2" width="300px">
                    <asp:TextBox ID="Txtcustgroupname" runat="server" CssClass="field_input" 
                        MaxLength="500" TabIndex="11" Width="300px"></asp:TextBox>
                
                    <asp:TextBox ID="Txtcustgroupcode" runat="server" style="display: none"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:AutoCompleteExtender ID="custgroupExtender2" runat="server" 
                        CompletionInterval="10" 
                        CompletionListCssClass="autocomplete_completionListElement" 
                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                        DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                        FirstRowSelected="false" MinimumPrefixLength="-1" 
                        OnClientItemSelected="txtcustgroupnamecompleteselected" 
                        ServiceMethod="Getcustgroup" TargetControlID="Txtcustgroupname">
                    </asp:AutoCompleteExtender>
                    <input style="display: none" id="Text11" class="field_input" type="text" 
                                                                runat="server" />
                    <input style="display: none" id="Text12" class="field_input" type="text" 
                                                                runat="server" />
                </td>
                  
            </TR>
            </TBODY>
            </TABLE>
            </asp:Panel>
           
        <asp:Panel id="Panel7" runat="server" CssClass="td_cell" Width="500px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 700px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label5" runat="server" Text="Customer" Width="65px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="300px">
                      <asp:TextBox ID="txtcustomername" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="12" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcustomercode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField5" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcustomername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcustomer" ContextKey="True" TargetControlID="txtcustomername" OnClientItemSelected="txtcustomernameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                           
  
                            
             </TD></TR>
            
             </TBODY></TABLE></asp:Panel>
           
        
 </asp:Panel></td></tr>
      

            
             </TBODY></TABLE> </TD></TR><TR><TD></TD><TD>&nbsp;</TD></TR></TD></TR><TR><TD class="td_cell" align=center>
    <asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" 
        __designer:dtid="4222124650660080" CssClass="btn" __designer:wfdid="w5"></asp:Button>&nbsp;&nbsp;<asp:Button 
        id="btnReport" tabIndex=13 onclick="btnReport_Click" runat="server" 
        Text="Load Report"   style="display:none" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=14 onclick="btnhelp_Click" runat="server" Text="Help" 
        CssClass="btn" __designer:wfdid="w19"></asp:Button>&nbsp;<asp:Button 
        id="btnExit" tabIndex=39 onclick="btnExit_Click" runat="server" Text=" Exit" 
        CssClass="btn" __designer:wfdid="w20" style="Display:none" CausesValidation="False"></asp:Button> <%-- style="display:none"--%>
        <asp:Button 
        id="pdfformat" tabIndex=39 onclick="btnpdfformat_Click" runat="server" Text="Pdf Format" 
        CssClass="btn" __designer:wfdid="w20"   CausesValidation="False"></asp:Button>
           <asp:Button 
        id="btnExlReport" tabIndex=39 onclick="btnExlReport_Click" runat="server" Text="Excel Report" 
        CssClass="btn" __designer:wfdid="w20"   CausesValidation="False"></asp:Button>

        </TD>
        
        
             <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td></TR>
   <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
   <tr><td></td></tr></TBODY></TABLE><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td>&nbsp;&nbsp;&nbsp;&nbsp;</td></tr></TBODY></TABLE><asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>


<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy> <cc1:CalendarExtender id="ClsExFromDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
            </cc1:CalendarExtender> <cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True" DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender> &nbsp; 
</contenttemplate>


</asp:Content>

