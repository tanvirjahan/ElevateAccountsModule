
<%@  Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptCustomerStatementAccount.aspx.vb" Inherits="RptCustomerStatementAccount" %>

    
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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
   
   
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {

            txtacnameAutoCompleteExtenderKeyUp();
            txtcategorynameAutoCompleteExtenderKeyUp();
            txtmarketnameAutoCompleteExtenderKeyUp();
            txtcountrynameAutoCompleteExtenderKeyUp();
            txtcontrolnameAutoCompleteExtenderKeyUp();
            txtcustgroupAutoCompleteExtenderKeyUp();
        });
</script>
  
   <script language="javascript" type="text/javascript" >
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
       function txtacnameAutoCompleteExtenderKeyUp() {
           $("#<%= txtacname.ClientID %>").bind("change", function () {

               if (document.getElementById('<%=txtacname.ClientID%>').value == '') {

                   document.getElementById('<%=txtaccode.ClientID%>').value = '';
               }

           });

           $("#<%= txtacname.ClientID %>").keyup(function (event) {

               if (document.getElementById('<%=txtacname.ClientID%>').value == '') {

                   document.getElementById('<%=txtaccode.ClientID%>').value = '';
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
                   $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
                   SetcustomerContextkey();
               }

           });

           $("#<%= txtcountryname.ClientID %>").keyup(function (event) {

               if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                   document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
                   $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
                   SetcustomerContextkey();
               }

           });
       }




       function txtacnameautocompleteselected(source, eventArgs) {
           if (eventArgs != null) {
               document.getElementById('<%=txtaccode.ClientID%>').value = eventArgs.get_value();
           }
           else {
               document.getElementById('<%=txtaccode.ClientID%>').value = '';
           }
       }



       function txtmarketnameautocompleteselected(source, eventArgs) {
           if (eventArgs != null) {
               document.getElementById('<%= txtmarketcode.ClientID%>').value = eventArgs.get_value();
           }
           else {
               document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
           }
           SetcustomerContextkey();
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
           }
           else {
               document.getElementById('<%=txtcategorycode.ClientID%>').value = '';
           }
           SetcustomerContextkey();
       }


       function txtcountrynamecompleteselected(source, eventArgs) {

           if (eventArgs != null) {
               document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
               $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey(eventArgs.get_value());
               document.getElementById('<%=txtmarketname.ClientID%>').value = '';
               document.getElementById('<%=txtmarketcode.ClientID%>').value = '';
           }
           else {

               document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
               $find('<%=txtmarketname_AutoCompleteExtender.ClientID%>').set_contextKey('');
           }

           SetcustomerContextkey();
       }
       function txtcustgroupnamecompleteselected(source, eventArgs) {
           if (eventArgs != null) {
               document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = eventArgs.get_value();
               SetcustomerContextkey();
               document.getElementById('<%=txtacname.ClientID%>').value = '';
               document.getElementById('<%=txtaccode.ClientID%>').value = '';
           }
           else {
               document.getElementById('<%=Txtcustgroupcode.ClientID%>').value = '';

           }
           SetcustomerContextkey();
       }
       function TimeOutHandler(result) {
           alert("Timeout :" + result);
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
           $find('<%=txtacname_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

       }

       function ErrorHandler(result) {
           var msg = result.get_exceptionType() + "\r\n";
           msg += result.get_message() + "\r\n";
           msg += result.get_stackTrace();
           alert(msg);
       }
       function ChangeDate() {

           var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
           var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
           var rdbtnAsOnDate = document.getElementById("<%=rdbtnAsOnDate.ClientID%>");
           var rdbtnFromToDate = document.getElementById("<%=rdbtnFromToDate.ClientID%>");


           if (txtfdate.value == '') {
               alert("Enter From Date.");
               txtfdate.focus();
           }
           else {
               if (rdbtnFromToDate.checked == true) {
                   txttdate.value = txtfdate.value;
               }
           }
       }
    </script>
   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {

           txtacnameAutoCompleteExtenderKeyUp();
           txtcontrolnameAutoCompleteExtenderKeyUp();
           txtcustomnameAutoCompleteExtenderKeyUp();
           txtmarketnameAutoCompleteExtenderKeyUp();
           txtcountrynameAutoCompleteExtenderKeyUp();
           txtcustgroupAutoCompleteExtenderKeyUp();

       }
</script>
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        width:100%; border-bottom: gray 1px solid" class="td_cell" align="left">
        <tbody>
            <tr>
                <td class="field_heading" align="center">
                    <asp:Label ID="lblHeading" runat="server" Text="Customer Statement Of Account" CssClass="field_heading"
                        Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="td_cell" align="left">
                    <asp:Panel ID="pnlDate" runat="server" Font-Bold="True" Width="100%" GroupingText="Date">
                        <table style="width: 622px" class="td_cell" align="left">
                            <tbody>
                                <tr>
                                  
                                    <td colspan="2">
                                        <table style="height: 127px; width: 295px">
                                        
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        Ageing Type
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                        <select ID="ddlAgeing" runat="server" tabindex=1  class="field_input" 
                                                            onchange="CallWebMethod('toacccode');" style="width: 158px">
                                                            <option selected="" value="Month">Month</option>
                                                            <option value="Date">Date</option>
                                                            <option value="Due Date">Due Date</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        Include Proforma
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                        <select ID="ddlproforma" tabindex=1 runat="server" class="field_input" 
                                                            style="width: 158px">
                                                            <option value="Yes">Yes</option>
                                                            <option selected="" value="No">No</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 124px">
                                                        <asp:Label ID="lblIncludeZero" runat="server" Text="Include Zero"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <select ID="ddlIncludeZero"  tabindex=2 runat="server" class="field_input" 
                                                            onchange="CallWebMethod('toacccode');" style="width: 158px">
                                                            <option value="All">All</option>
                                                            <option selected="" value="Pending">Pending</option>
                                                            <option value="Closed">Closed</option>
                                                        </select></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        Currency Type
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                        <select ID="ddlCurrencyType" runat="server" tabindex=3 class="field_input" 
                                                            onchange="CallWebMethod('toacccode');" style="width: 158px">
                                                            <option selected=""></option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        <asp:Label ID="lblReportType" runat="server" CssClass="td_cell" 
                                                            Text="Report Type" Width="85px"></asp:Label>
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                        <asp:DropDownList ID="ddlwithmovmt"  tabindex=4 runat="server" CssClass="field_input" 
                                                            Height="16px" Width="158px">
                                                            <asp:ListItem Value="0">Summary</asp:ListItem>
                                                            <asp:ListItem Value="1">Detail</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                        
                                        </table>
                                    </td>
                                    <td style="width: 60px" class="td_cell" align="left" colspan="2">
                                        <table style="width:400px" >
                                            <tr>
                                                <td style="width: 85px">
                                                    <asp:RadioButton ID="rdbtnAsOnDate" tabindex=6  runat="server" Text="As On date" Width="105px"
                                                        AutoPostBack="True" OnCheckedChanged="rdbtnAsOnDate_CheckedChanged" GroupName="6">
                                                    </asp:RadioButton>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdbtnFromToDate"  tabindex=7 runat="server" Text="From To Date" Width="124px"
                                                        AutoPostBack="True" OnCheckedChanged="rdbtnFromToDate_CheckedChanged" GroupName="6">
                                                    </asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 85px; text-align: center;">
                                                    <asp:Label ID="lblasdate" runat="server" Text="As On Date" 
                                                        style="text-align: center"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" tabindex=8 CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton
                                                        ID="ImgBtnFrmDt" runat="server" tabindex=8 ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                    </asp:ImageButton><br />
                                                    <cc1:MaskedEditValidator ID="MEVFromDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                        InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date"
                                                        EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required"
                                                        ControlExtender="MEFromDate" ControlToValidate="txtFromDate" Display="Dynamic"></cc1:MaskedEditValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Text="To Date" ></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" tabindex=9 CssClass="fiel_input" Width="80px"></asp:TextBox><asp:ImageButton
                                                        ID="ImgBtnToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                    </asp:ImageButton>
                                                    <cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                        InvalidValueMessage="Invalid Date" InvalidValueBlurredMessage="Invalid Date"
                                                        EmptyValueMessage="Date is required" EmptyValueBlurredText="Date is required"
                                                        ControlExtender="METoDate" ControlToValidate="txtToDate" Display="Dynamic"></cc1:MaskedEditValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                
                                </tr>
                                <tr>
                                    <td class="td_cell" align="left">
                                    </td>
                                    <td class="td_cell" valign="middle" align="left">
                                    </td>
                                    <td style="width: 67px" class="td_cell" valign="middle" align="left" colspan="1">
                                    </td>
                                    <td class="td_cell" valign="middle" align="left" colspan="1">
                                    </td>
                                </tr>
                              
                                <tr>
                                    <td align="left" style="display: none">
                                        <asp:Label ID="lblagdate" runat="server" CssClass="td_cell" Text="Ageing as On Date"
                                            Width="60px"></asp:Label>
                                    </td>
                                    <td align="left" class="td_cell" valign="middle" style="display: none">
                                        <asp:TextBox ID="txtagDate" runat="server" tabindex=10 CssClass="fiel_input" Width="80px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtagDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                            PopupButtonID="ImgBtnagDate" TargetControlID="txtagDate"></cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="txtagDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtagDate"></cc1:MaskedEditExtender>
                                        &nbsp;<asp:ImageButton ID="ImgBtnagDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                        &nbsp;<cc1:MaskedEditValidator ID="MEagDate" runat="server" ControlExtender="MEFromDate"
                                            ControlToValidate="txtagDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                                            EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format">
                                        </cc1:MaskedEditValidator>
                                    </td>
                                    <td align="left" class="td_cell" colspan="1" style="width: 67px" valign="middle">
                                        &nbsp;</td>
                                    <td align="left" class="td_cell" colspan="1" valign="middle">
                                        &nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
       
      <tr><td>   <asp:Panel id="Panels" runat="server" CssClass="td_cell" Width="100%"
            __designer:wfdid="w22" GroupingText="Select Criteria">
            <TABLE style="WIDTH: 670px; HEIGHT: 28px"><TBODY>
         
          
                                       
                                         <tr>
                   <td style="width: 88px" align="left">
                  <asp:Label ID="Label1" runat="server" Text="Control A/C " Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                        </td>
                        <td align="left" valign="top" width="300px">
                      <asp:TextBox ID="txtcontrolname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="11" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcontrolcode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField1" runat="server" />
                          <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcontrolname" TargetControlID="txtcontrolname" OnClientItemSelected="txtcontrolnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text3" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text4" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr>
          
                          
                
           
                 <TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label5" runat="server" Text="Country" Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                        </td>
                        <td align="left" valign="top" width="300px">
                      <asp:TextBox ID="txtcountryname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="12" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcountrycode" style="display: none" runat="server" ></asp:TextBox>
                           <asp:HiddenField ID="HiddenField4" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcountryname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcountry" TargetControlID="txtcountryname" OnClientItemSelected="txtcountrynamecompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text11" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text12" class="field_input" type="text" runat="server" />
                                         </td>
                                       </tr> 
          <TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label4" runat="server" Text="City" Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                        </td>
                        <td align="left" valign="top" width="300px">
                      <asp:TextBox ID="txtmarketname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="13" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtmarketcode"  style="display: none" runat="server" ></asp:TextBox>
                           <asp:HiddenField ID="HiddenField3" runat="server" />
                          <asp:AutoCompleteExtender ID="txtmarketname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcity"  ContextKey ="True" TargetControlID="txtmarketname" OnClientItemSelected="txtmarketnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text9" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text10" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr> 
                                       
                  <TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label3" runat="server" Text="Customer Category" Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                        </td>
                        <td align="left" valign="top" width="300px">
                      <asp:TextBox ID="txtcategoryname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="14" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtcategorycode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField21" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcategoryname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcategory" TargetControlID="txtcategoryname" OnClientItemSelected="txtcategorynameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                 </td></TR>
                
                   
          
                        <tr>
                            <td align="left" style="width: 130px; height: 20px;">
                                <asp:Label ID="Label6" runat="server" Text="Customer Group" Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                            </td>
                            <td align="left" style="height: 20px" valign="top" width="300px">


                             <asp:TextBox ID="Txtcustgroupname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="15" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="Txtcustgroupcode" style="display: none" runat="server" ></asp:TextBox>
                           <asp:HiddenField ID="HiddenField2" runat="server" />
                          <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcustgroup" TargetControlID="Txtcustgroupname" OnClientItemSelected="txtcustgroupnamecompleteselected">
                               </asp:AutoCompleteExtender>
                            </td>
                </tr>
                         
             <tr>
                   <td style="width: 140px" align="left">
                  <asp:Label ID="lblType" runat="server" Text="Customer" Width="130px"></asp:Label><%--<span
                        style="color: #ff0000">*</span>--%>
                        </td>
                        <td align="left" valign="top" width="300px">
                      <asp:TextBox ID="txtacname"  runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="16" Width="300px"></asp:TextBox>
                         <asp:TextBox ID="txtaccode" style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="hdnpartycode" runat="server" />
                          <asp:AutoCompleteExtender ID="txtacname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getactype"  ContextKey ="True" TargetControlID="txtacname" OnClientItemSelected="txtacnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr>
                        </TBODY></TABLE>    </asp:Panel>    </td></tr>   
          
            <tr>
                <td style="vertical-align: top" class="td_cell" align="left">
                    <asp:Panel ID="Panel5" runat="server" Font-Bold="True" Width="100%" GroupingText="Remarks">
                        <table style="width: 495px" class="td_cell" align="left">
                            <tbody>
                                <tr>
                                    <td style="width: 188px" align="left" rowspan="2">
                                        <textarea style="width: 500px;  height: 50px" id="txtRemark" class="td_cell" tabindex=17  runat="server"></textarea>
                                    </td>
                                    <td style="width: 188px" align="left">
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 188px">
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            
            <tr>
                <td style="text-align: center">
                    &nbsp;<asp:Button ID="Button1" tabindex=19 OnClick="Button1_Click1" runat="server" Text="Export"
                        __designer:dtid="4785074604081363" CssClass="field_button" __designer:wfdid="w3" Visible="false">
                    </asp:Button>&nbsp;
                    <asp:Button ID="btnLoadReprt"  tabindex=18 OnClick="btnLoadReprt_Click" runat="server" Text="Load Report"
                        CssClass="field_button" style="display:none"  ></asp:Button>&nbsp;
       <asp:Button ID="btnPdfReport"  tabindex=19 OnClick="btnPdfReport_Click" runat="server" Text="Pdf Report"
                        CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;
                            <asp:Button ID="btnExcelReport"  tabindex=19 OnClick="btnExcelReport_Click" runat="server" Text="Excel Report"
                        CssClass="field_button" CausesValidation="False"></asp:Button>
                        &nbsp;<asp:Button ID="btnhelp" tabindex=20 OnClick="btnhelp_Click"
                            runat="server" Text="Help" Height="20px" CssClass="field_button"></asp:Button>
              
                </td>
         
             <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>
            </tr>

            
<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
            <tr>
                <td style="text-align: left">
                    <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                        TargetControlID="txtFromDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MEFromDate" runat="server" TargetControlID="txtFromDate"
                        MaskType="Date" Mask="99/99/9999">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnToDate"
                        TargetControlID="txtToDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="METoDate" runat="server" TargetControlID="txtToDate"
                        MaskType="Date" Mask="99/99/9999">
                    </cc1:MaskedEditExtender>
                </td>
            </tr>
          

        </tbody>
    </table>
    


</asp:Content>