<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptCustomerTrialBalance.aspx.vb" Inherits="RptCustomerTrialBalance" MasterPageFile="~/AccountsMaster.master" Strict ="true"  %>

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
          txtcontrolnameAutoCompleteExtenderKeyUp();
          txtmarketnameAutoCompleteExtenderKeyUp();
          txtcountrynameAutoCompleteExtenderKeyUp();
          txtcustgroupAutoCompleteExtenderKeyUp();
      });
  </script>
<script language="javascript" type="text/javascript">
    function checkNumber(e) {

        if ((event.keyCode < 47 || event.keyCode > 57)) {
            return false;
        }

    }



    function enabledatectrl() {
        if (document.getElementById("<%=ddlwithmovmt.ClientID%>").value == 1)//without ason
        {
            document.getElementById("<%=label2.ClientID%>").innerText = "AsOnDate";
            document.getElementById("<%=label1.ClientID%>").style.display = 'none';
            document.getElementById("<%=txttoDate.ClientID%>").style.display = 'none';
            document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = 'none';
            return true;
        }
        else {
            document.getElementById("<%=label2.ClientID%>").innerText = "FromDate";
            document.getElementById("<%=label1.ClientID%>").style.display = 'block';
            document.getElementById("<%=txttoDate.ClientID%>").style.display = 'block';
            document.getElementById("<%=ImgBtntoDt.ClientID%>").style.display = 'block';
            return true;
        }
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
            }

        });

        $("#<%= txtcustomername.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcustomername.ClientID%>').value == '') {

                document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
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
            }

        });

        $("#<%= txtcountryname.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtcountryname.ClientID%>').value == '') {

                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
            }

        });
    }



    function txtcontrolnameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcontrolcode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcontrolcode.ClientID%>').value = '';
        }
    }


    function txtcustomernameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtcustomercode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtcustomercode.ClientID%>').value = '';
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

    function ChangeDate() {

        var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
        var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

        if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
        else { txttdate.value = txtfdate.value; }
    }
</script>

   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {
           txtcustomernameAutoCompleteExtenderKeyUp();
           txtcontrolnameAutoCompleteExtenderKeyUp();
           txtcategorynameAutoCompleteExtenderKeyUp();
           txtmarketnameAutoCompleteExtenderKeyUp();
           txtcountrynameAutoCompleteExtenderKeyUp();
           txtcustgroupAutoCompleteExtenderKeyUp();
       }
</script>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 100%; BORDER-BOTTOM: gray 2px solid">
<TBODY>
<TR>
<TD class="field_heading" align=center colSpan=1 style="width: 100%"><asp:Label id="lblHeading" runat="server" Text="Customer Trial Balance" ForeColor="White" Width="100%" CssClass="field_heading"></asp:Label></TD></TR>
<TR>
<TD style="WIDTH: 100%; HEIGHT: 24px" class="td_cell">
<TABLE style="WIDTH: 100%">
<TBODY>
<TR>
<TD>&nbsp;<asp:Panel id="Panel1" runat="server" Width="100%" CssClass="field_input" GroupingText="Select Date">
<TABLE style="WIDTH: 100%"><TBODY>
        <tr><td class="field_input">Report Type</td>
            <td class="field_input"><asp:DropDownList ID="ddlwithmovmt" 
        runat="server" CssClass="drpdown" onchange="enabledatectrl()" Width="120px" 
                    TabIndex="1"><asp:ListItem 
            Value="0">Transactions</asp:ListItem>
<asp:ListItem Value="1">Balances</asp:ListItem>
        </asp:DropDownList></td></tr><tr><td class="field_input" style="WIDTH: 7px"><asp:Label 
        ID="Label2" runat="server" Width="104px">From Date</asp:Label></td>
            <td class="field_input">
                <asp:TextBox ID="txtFromDate" runat="server" 
        CssClass="txtbox" Width="111px" TabIndex="2"></asp:TextBox>&#160;<asp:ImageButton 
        ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" 
                    TabIndex="2">
    </asp:ImageButton> 
                <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" 
        ControlExtender="MskFromDate" ControlToValidate="txtFromDate" 
        CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
        EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
        InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format" Width="1px"></cc1:MaskedEditValidator></td></tr>
        <tr><td class="field_input" style="WIDTH: 7px">
            <asp:Label ID="Label1" 
        runat="server" Text="To Date" Width="81px"></asp:Label></td>
            <td class="field_input">
                <asp:TextBox ID="txttoDate" runat="server" 
        CssClass="txtbox" Width="111px" TabIndex="3"></asp:TextBox> 
                <asp:ImageButton ID="ImgBtntoDt" runat="server" 
        ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="3">
    </asp:ImageButton> 
                <cc1:MaskedEditValidator ID="MskVtoDt" runat="server" 
        ControlExtender="MsktoDate" ControlToValidate="txttoDate" 
        CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*" 
        EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" 
        InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" 
        TooltipMessage="Input a date in dd/mm/yyyy format" Width="1px"></cc1:MaskedEditValidator></td></tr></TBODY></TABLE></asp:Panel></TD></TR><TR><TD>
      
       <asp:Panel id="Panel6" runat="server" CssClass="td_cell" Width="100%" 
            __designer:wfdid="w22" GroupingText="Select Filters">
       
       <asp:Panel id="Panel2" runat="server" CssClass="td_cell" Width="100%" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 129px" align="left">
                  <asp:Label ID="Label111" runat="server" Text="Control A/C" Width="82px"></asp:Label>&nbsp;<span 
                           style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                      <asp:TextBox ID="txtcontrolname" runat="server" CssClass="field_input"
                       MaxLength="500" TabIndex="4" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtcontrolcode" style="display: none" runat="server" ></asp:TextBox>
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
    <asp:Panel id="Panel5" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label6" runat="server" Text="Country" Width="60px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                      <asp:TextBox ID="txtcountryname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="5" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtcountrycode" style="display: none" runat="server" ></asp:TextBox>
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
                                                 <asp:Panel id="Panel4" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH: 650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label4" runat="server" Text="City" Width="50px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                      <asp:TextBox ID="txtmarketname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="6" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtmarketcode"  style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField3" runat="server" />
                          <asp:AutoCompleteExtender ID="txtmarketname_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getmarket"  ContextKey="true" TargetControlID="txtmarketname" OnClientItemSelected="txtmarketnameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text7" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text8" class="field_input" type="text" runat="server" />
                                        </td>
                                       </tr> 
                                       </TBODY></TABLE></asp:Panel>
    
              <asp:Panel id="Panel3" runat="server" CssClass="td_cell" Width="477px" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH:650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label3" runat="server" Text="Category" Width="65px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                      <asp:TextBox ID="txtcategoryname" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="7" Width="200px"></asp:TextBox>
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
    
                
                   
                 <asp:Panel id="Panel8" runat="server" CssClass="td_cell" Width="100%" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH:650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label7" runat="server" Text="Customer Group" Width="117px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                    <asp:TextBox ID="Txtcustgroupname" runat="server" CssClass="field_input" 
                        MaxLength="500" TabIndex="8" Width="200px"></asp:TextBox>
                
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
  
                            
             </TD></TR>
            
             </TBODY></TABLE></asp:Panel>


                                           <asp:Panel id="Panel7" runat="server" CssClass="td_cell" Width="100%" 
            __designer:wfdid="w22">
            <TABLE style="WIDTH:650px; HEIGHT: 28px"><TBODY><TR>
         
                   <td style="width: 130px" align="left">
                  <asp:Label ID="Label5" runat="server" Text="Customer" Width="65px"></asp:Label><span
                        style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" width="200px">
                      <asp:TextBox ID="txtcustomername" runat="server"  CssClass="field_input"
                       MaxLength="500" TabIndex="9" Width="200px"></asp:TextBox>
                         <asp:TextBox ID="txtcustomercode"  style="display: none" runat="server"></asp:TextBox>
                           <asp:HiddenField ID="HiddenField5" runat="server" />
                          <asp:AutoCompleteExtender ID="txtcustomername_AutoCompleteExtender" runat="server" CompletionInterval="10"
                           CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                              EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                               ServiceMethod="Getcustomer"  ContextKey="true" TargetControlID="txtcustomername" OnClientItemSelected="txtcustomernameautocompleteselected">
                               </asp:AutoCompleteExtender>
                                  <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                                       <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                           
  
                            
             </TD></TR>
            
             </TBODY></TABLE></asp:Panel>



           
                
      </asp:Panel>
 </TD></TR></TBODY></TABLE></TD></TR><TR><TD class="td_cell" style="width: 100%">&nbsp; <TABLE style="WIDTH: 399px"><TBODY><TR><TD style="WIDTH: 72px">Currency</TD><TD style="WIDTH: 137px">
        <SELECT style="WIDTH: 121px" id="ddlCurrency" class="drpdown" runat="server" 
            tabindex="10"><OPTION value="[Select]" selected>[Select]</OPTION></SELECT></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 100px">Report Order</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlrptord" runat="server" Width="123px" 
            CssClass="drpdown" TabIndex="11"><asp:ListItem Value="0">Code</asp:ListItem>
<asp:ListItem Value="1">Name</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 100px"></TD></TR><TR><TD style="WIDTH: 72px">Group By</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlgpby" runat="server" Width="122px" CssClass="drpdown" 
            TabIndex="12"><asp:ListItem Value="0">None</asp:ListItem>
<asp:ListItem Value="1">Market</asp:ListItem>
<asp:ListItem Value="2">Category</asp:ListItem>
<asp:ListItem Value="4">Control account Code</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR><TR><TD style="WIDTH: 100px">Include Zero</TD><TD style="WIDTH: 137px">
        <asp:DropDownList id="ddlinclzero" runat="server" Width="122px" 
            CssClass="drpdown" TabIndex="13"><asp:ListItem Value="0">No</asp:ListItem>
<asp:ListItem Value="1">Yes</asp:ListItem>
</asp:DropDownList></TD><TD style="WIDTH: 71px"></TD></TR></TBODY></TABLE></TD></TR><TR>
    <TD class="td_cell" align=center style="width: 100%">&nbsp; 
    <asp:Button id="Button1" onclick="Button1_Click1" runat="server" Text="Export" 
        CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnReport" 
        onclick="btnReport_Click" runat="server" Text="Load Report" Width="100px" 
        CssClass="btn" CausesValidation="False" style="display:none" TabIndex="14"></asp:Button> &nbsp;
   <asp:Button id="btnPdfReport" 
        onclick="btnPdfReport_Click" runat="server" Text="Pdf Report" Width="100px" 
        CssClass="btn" CausesValidation="False" TabIndex="14"></asp:Button> 
           <asp:Button id="btnExcelReport" 
        onclick="btnExcelReport_Click" runat="server" Text="Excel Report" Width="120px" 
        CssClass="btn" CausesValidation="False" TabIndex="14"></asp:Button> 
    <asp:Button id="btnExit" tabIndex=6 onclick="btnExit_Click" runat="server" 
        Text=" Exit" Width="64px" style="display:none" CssClass="btn" CausesValidation="False"></asp:Button>&nbsp;<asp:Button 
        id="btnhelp" tabIndex=15 onclick="btnhelp_Click" runat="server" Text="Help" 
        Width="50px" CssClass="btn"></asp:Button></TD>
        
        
             <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>
        
        
        </TR></TBODY></TABLE>
        
        <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
        
        <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server"><Services>
<asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy>&nbsp;&nbsp;&nbsp; <cc1:CalendarExtender id="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt" TargetControlID="txtFromDate">
    </cc1:CalendarExtender><cc1:MaskedEditExtender id="MskFromDate" runat="server" TargetControlID="txtFromDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:MaskedEditExtender><cc1:MaskedEditExtender id="MsktoDate" runat="server" TargetControlID="txttoDate" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"></cc1:MaskedEditExtender><cc1:CalendarExtender id="ClsExtoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt" TargetControlID="txttoDate"></cc1:CalendarExtender> 

</asp:Content>
