<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptSupplierStatementOfAccount.aspx.vb"
    Inherits="RptSupplierStatementOfAccount" MasterPageFile="~/AccountsMaster.master"
    Strict="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
            costgrpAutoCompleteExtenderKeyUp();
            supplierAutoCompleteExtenderKeyUp();
            catAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            ctryAutoCompleteExtenderKeyUp();
            suppliertypeAutoCompleteExtenderKeyUp();
        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);

        function EndRequestUserControl(sender, args) {
            costgrpAutoCompleteExtenderKeyUp();
            supplierAutoCompleteExtenderKeyUp();
            catAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            ctryAutoCompleteExtenderKeyUp();
            suppliertypeAutoCompleteExtenderKeyUp();

            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }

        function suppliertypeAutoCompleteExtenderKeyUp() {
            $("#<%= txtsuptypename.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtsuptypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtsuptypename.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtsuptypename.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
        }
        function catautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcatcode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                SetsupplierContextkey();
            }

        }
        function supplierautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtpartycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtpartycode.ClientID%>').value = '';
            }

        }
        function costgrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtbankcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtbankcode.ClientID%>').value = '';
            }

        }

        function cityautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                SetsupplierContextkey();
            }

        }

        function ctryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtctrycode.ClientID%>').value = eventArgs.get_value();

                $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey(eventArgs.get_value());
                document.getElementById('<%=txtcityname.ClientID%>').value = '';
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                SetsupplierContextkey();

            }
            else {
                document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                SetsupplierContextkey();
            }

        }

        function supplierAutoCompleteExtenderKeyUp() {
            $("#<%= txtpartyname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtpartyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtpartycode.ClientID%>').value = '';
                }

            });

            $("#<%= txtpartyname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtpartyname.ClientID%>').value == '') {

                    document.getElementById('<%=txtpartycode.ClientID%>').value = '';
                }

            });
        }







        function costgrpAutoCompleteExtenderKeyUp() {
            $("#<%= txtbankname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtbankname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtbankname.ClientID%>').value == '') {

                    document.getElementById('<%=txtbankcode.ClientID%>').value = '';
                }

            });
        }



        function catAutoCompleteExtenderKeyUp() {
            $("#<%= txtcatname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtcatname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtcatname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcatname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
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

                    document.getElementById('<%=txtcatcode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
        }

        function suppliertypeautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtsuptypecode.ClientID%>').value = eventArgs.get_value();
                SetsupplierContextkey();
            }
            else {
                document.getElementById('<%=txtsuptypecode.ClientID%>').value = '';
                SetsupplierContextkey();
            }

        }
        function ctryAutoCompleteExtenderKeyUp() {
            $("#<%= txtctryname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtctryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                    $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtctryname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtctryname.ClientID%>').value == '') {

                    document.getElementById('<%=txtctrycode.ClientID%>').value = '';
                    $find('<%=txtcityname_AutoCompleteExtender2.ClientID%>').set_contextKey('');
                    SetsupplierContextkey();
                }

            });
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

        function SetsupplierContextkey() {

            var contxt = '';

            var ctrycode = document.getElementById("<%=txtctrycode.ClientID%>").value;
            var ctryname = document.getElementById("<%=txtctryname.ClientID%>").value;
            var citycode = document.getElementById("<%=txtcitycode.ClientID%>").value;
            var cityname = document.getElementById("<%=txtcityname.ClientID%>").value;
            var supptypecode = document.getElementById("<%=txtsuptypecode.ClientID%>").value;
            var supptypename = document.getElementById("<%=txtsuptypename.ClientID%>").value;

            var categorycode = document.getElementById("<%=txtcatcode.ClientID%>").value;
            var categoryname = document.getElementById("<%=txtcatname.ClientID%>").value;

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




            if (categoryname == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + categorycode;
            }



            if (supptypename == '') {
                contxt = contxt + '||' + '';
            }
            else {
                contxt = contxt + '||' + supptypecode;
            }



            // $find('AutoCompleteExtender_txtBankName').set_contextKey(contxt);
            $find('<%=txtpartyname_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

        }




        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");
            var rdbtnFromToDate = document.getElementById("<%=rdbtnFromToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else {
                if (rdbtnFromToDate.checked == true) {
                    txttdate.value = txtfdate.value;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100%; border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="1">
                            <asp:Label ID="lblHeading" runat="server" Text="Supplier Statement Of Account" ForeColor="White"
                                CssClass="field_heading" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 24px" class="td_cell">
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%">
                                            <table >
                                                <tbody>
                                                    <tr>
                                                        <td style="height: 22px">
                                                            <asp:Label ID="lblsupsupagnt" Width="235px" CssClass="td_cell" Text="Select Supplier / Supplier Agent"
                                                                runat="server"></asp:Label>
                                                        </td>
                                                        <td style="width: 168px; height: 22px" class="td_cell">
                                                            &nbsp;<asp:DropDownList ID="ddlSupType" runat="server" CssClass="drpdown" Width="125px"
                                                                AutoPostBack="True" 
                                                                OnSelectedIndexChanged="ddlSupType_SelectedIndexChanged" TabIndex="1">
                                                                <asp:ListItem Value="S">Supplier</asp:ListItem>
                                                                <asp:ListItem Value="A">Supplier Agent</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 168px; height: 22px" class="td_cell">
                                                            
                                                        </td>
                                                        <td style="width: 126px; height: 22px" class="td_cell">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
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
                                                       <select style="width: 125px" id="ddlAgeing" class="drpdown" onchange="CallWebMethod('toacccode');"
                                                                    runat="server" tabindex="2">
                                                                    <option value="Month" selected>Month</option>
                                                                    <option value="Date">Date</option>
                                                                    <option value="Due Date">Due Date</option>
                                                                </select>
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        Include Proforma</td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                        <select ID="ddlproforma" runat="server" class="field_input" name="D1" 
                                                            style="width: 127px" tabindex="1">
                                                            <option value="Yes">Yes</option>
                                                            <option selected="" value="No">No</option>
                                                        </select></td>
                                                </tr>
                                               
                                                <tr>
                                                    <td style="width: 124px">
                                                     <asp:Label Text="Include Zero" runat="server" ID="lblIncludeZero"></asp:Label>
                                                    </td>
                                                    <td>
                                                         <select style="width: 126px" id="ddlIncludeZero" class="drpdown" onchange="CallWebMethod('toacccode');"
                                                                    runat="server" tabindex="3">
                                                                    <option value="All">All</option>
                                                                    <option value="Pending" selected>Pending</option>
                                                                    <option value="Closed">Closed</option>
                                                                </select></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                         Currency Type
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                         <select style="width: 126px" id="ddlCurrencyType" class="drpdown" onchange="CallWebMethod('toacccode');"
                                                                    runat="server" tabindex="4">
                                                                    <option selected></option>
                                                                </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="td_cell" style="width: 124px">
                                                        <asp:Label ID="lblReportType" runat="server" CssClass="td_cell" 
                                                            Text="Report Type" Width="85px"></asp:Label>
                                                    </td>
                                                    <td align="left" class="td_cell" valign="middle">
                                                      <select style="width: 125px" id="ddlLedgerType" class="drpdown" runat="server" 
                                                            tabindex="5">
                                                                <option value="0" selected>[Select]</option>
                                                                <option value="1">Without PDC</option>
                                                                <option value="2">With PDC</option>
                                                            </select>
                                                    </td>
                                                </tr>
                                        
                                        </table>
                                    </td>
                                    <td style="width: 60px" class="td_cell" align="left" colspan="2">
                                        <table style="width:450px" >
                                            <tr>
                                                <td style="width: 85px">
                                                   <asp:RadioButton ID="rdbtnAsOnDate" runat="server" Text="As On   date" Width="100px"
                                                                    AutoPostBack="True" 
                                                        OnCheckedChanged="rdbtnAsOnDate_CheckedChanged" GroupName="DateRange"
                                                                    Checked="True"></asp:RadioButton>
                                                </td>
                                                <td>
                                                   <asp:RadioButton ID="rdbtnFromToDate" runat="server" Text="From To Date" Width="132px"
                                                                    AutoPostBack="True" 
                                                        OnCheckedChanged="rdbtnFromToDate_CheckedChanged" GroupName="DateRange">
                                                                </asp:RadioButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 85px; text-align: center;">
                                                     <asp:Label ID="lblasdate" runat="server" Text="As On Date" Width="103px"></asp:Label>
                                                </td>
                                                <td>
                                                   <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" Width="80px" 
                                                        TabIndex="8"></asp:TextBox>
                                                                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                        ImageUrl="../Images/Calendar_scheduleHS.png" TabIndex="8">
                                                                </asp:ImageButton>
                                                                <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" CssClass="field_error" Width="23px"
                                                                    ControlExtender="MskFromDate" ControlToValidate="txtFromDate" Display="Dynamic"
                                                                    EmptyValueBlurredText="*" EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate"
                                                                    InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                </td>
                                                <td style="width: 57px">
                                                     <asp:Label ID="lbltodate" runat="server" Text="To Date" Width="70px"></asp:Label>
                                                </td>
                                                <td>
                                                      <asp:TextBox ID="txtToDate" runat="server" CssClass="txtbox" Width="80px" 
                                                          TabIndex="9"></asp:TextBox>&nbsp;<asp:ImageButton
                                                                    ID="ImgBtnRevDate" runat="server" 
                                                          ImageUrl="../Images/Calendar_scheduleHS.png" TabIndex="9">
                                                                </asp:ImageButton>
                                                                <cc1:MaskedEditValidator ID="MskVToDate" runat="server" CssClass="field_error" ControlExtender="MskToDate"
                                                                    ControlToValidate="txtToDate" Display="Dynamic" EmptyValueBlurredText="*" EmptyValueMessage="Date is required"
                                                                    InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
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
                                                                    Width="60px" Style="display: none"></asp:Label>
                                    </td>
                                    <td style="display: none">
                                                                <asp:TextBox ID="txtagDate" runat="server" CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                                <cc1:MaskedEditExtender ID="txtagDate_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="txtagDate">
                                                                </cc1:MaskedEditExtender>
                                                                <asp:ImageButton ID="ImgBtnagDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                <cc1:MaskedEditValidator ID="MEagDate" runat="server" ControlExtender="txtagDate_MaskedEditExtender"
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
                                    <tr>
                                        <td style="width: 100%">
                                            <asp:Panel ID="Panel4" runat="server" CssClass="td_cell" Width="100%" 
                                                GroupingText="Selection Criteria">
                                                <table style="width: 676px">
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_cell" style="height: 17px; width: 205px;">
                                                                Control Account
                                                            </td>
                                                            <td class="td_cell" style="height: 17px; width: 334px;">
                                                                <asp:TextBox ID="TxtBankName" runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="10" Width="310px" Height="16px"></asp:TextBox>
                                                               <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px; width: 115px;">
                                                               <asp:TextBox ID="TxtBankCode" style="display:none" runat="server" Width="100px" ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                      
                                                       
                                                         <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                Country
                                                            </td>
                                                            <td class="td_cell" style="width: 334px">
                                                                <asp:TextBox ID="txtctryname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="11" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtctryname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    OnClientItemSelected="ctryautocompleteselected" ServiceMethod="Getctrylist" TargetControlID="txtctryname">
                                                                </asp:AutoCompleteExtender>
                                                               <%-- <asp:HiddenField ID="HiddenField4" runat="server"   />--%>
                                                            </td>
                                                            <td class="td_cell" style="width: 115px">
                                                                <asp:TextBox ID="txtctrycode" runat="server"   width="100px"  style="display:none"></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                City
                                                            </td>
                                                            <td class="td_cell" style="width: 334px">
                                                                <asp:TextBox ID="txtcityname" runat="server"  CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="12" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtcityname_AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
                                                                    OnClientItemSelected="cityautocompleteselected" ContextKey="True" ServiceMethod="Getcitylist" TargetControlID="txtcityname">
                                                                </asp:AutoCompleteExtender>
                                                                <%--<asp:HiddenField ID="HiddenField5" runat="server" />--%>
                                                            </td>
                                                            <td class="td_cell" style="width: 115px">
                                                                <asp:TextBox ID="txtcitycode" runat="server" style="display:none" Width="100px"  ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                              Category
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtcatname" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="13" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtcatname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    OnClientItemSelected="catautocompleteselected" ServiceMethod="Getcategorylist"
                                                                    TargetControlID="txtcatname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtcatcode" runat="server" style="display:none" Width="100px"     ></asp:TextBox>
                                                                                                            </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                    <td class="td_cell" style="width: 273px">
                                                        Type
                                                    </td>
                                                    <td class="td_cell" style="width: 262px">
                                                        <asp:TextBox ID="txtsuptypename" runat="server" CssClass="field_input" Height="16px"
                                                            MaxLength="500" TabIndex="14" Width="310px"></asp:TextBox>
                                                       
                                                             
                                                        <asp:AutoCompleteExtender ID="txtsuptypename_AutoCompleteExtender1" runat="server"
                                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                            FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="suppliertypeautocompleteselected"
                                                            ServiceMethod="Getsuppliertypelist" TargetControlID="txtsuptypename">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="HiddenField6" runat="server" />
                                                       
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:TextBox ID="txtsuptypecode" style="display:none"  runat="server" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                         <tr>
                                                            <td class="td_cell" style="width: 205px">
                                                                Supplier/Supplier Agent
                                                            </td>
                                                            <td class="td_cell" style="width: 338px">
                                                                <asp:TextBox ID="txtpartyname" runat="server" CssClass="field_input"
                                                                    Height="16px" MaxLength="500" TabIndex="15" Width="310px"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtpartyname_AutoCompleteExtender" runat="server"
                                                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                    FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="supplierautocompleteselected"
                                                                    ServiceMethod="Getsupplierlist" ContextKey="true"      TargetControlID="txtpartyname">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                            </td>
                                                            <td class="td_cell">
                                                                <asp:TextBox ID="txtpartycode" runat="server" style="display:none"  Width="100px"    ></asp:TextBox>
                                                                      </td>
                                                            <td class="td_cell">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 16px">
                                            <asp:Panel ID="Panel5" runat="server" Font-Bold="True" Width="100%" 
                                                GroupingText="Remarks" TabIndex="16">
                                                <table style="width: 676px" class="td_cell" align="left">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 188px" align="left" rowspan="2">
                                                                <textarea id="txtRemark" runat="server" class="txtbox" 
                                                                    style="width: 579px; height: 50px"></textarea>
                                                            </td>
                                                            <td align="left" style="width: 188px">
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
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" class="td_cell" align="right">
                            <input style="visibility: hidden; width: 12px; height: 9px" id="txtconnection" type="text"
                                runat="server" />&nbsp;<asp:Button ID="Button1" OnClick="Button1_Click1" runat="server"
                                    Text="Export" __designer:dtid="4222124650660080" CssClass="btn" __designer:wfdid="w4">
                                </asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnReport" 
                                OnClick="btnReport_Click" runat="server"
                                    Text="Load Report" CssClass="btn" CausesValidation="False"  style="display:none"
                                TabIndex="17">
                                </asp:Button>&nbsp;
                                <asp:Button ID="btnPdfReport" 
                                OnClick="btnPdfReport_Click" runat="server"
                                    Text="Pdf Report" CssClass="btn" CausesValidation="False" 
                                TabIndex="17" Visible = "false">
                                </asp:Button> &nbsp;
                                  <asp:Button ID="btnExcelReport" 
                                OnClick="btnExcelReport_Click" runat="server"
                                    Text="Excel Report" CssClass="btn" CausesValidation="False" 
                                TabIndex="18" >
                                </asp:Button>
                                &nbsp;<asp:Button
                                        ID="btnhelp" TabIndex="19" OnClick="btnhelp_Click" runat="server" Text="Help"
                                        CssClass="btn"></asp:Button>
                        </td>
                                 <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
   </td>

                    </tr>
                </tbody>
            </table>
                        
<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
            <cc1:CalendarExtender ID="ClsExFromDate" runat="server" TargetControlID="txtFromDate"
                PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
            <cc1:CalendarExtender ID="ClsExToDate" runat="server" TargetControlID="txtToDate"
                PopupButtonID="ImgBtnRevDate" Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender>
            <cc1:MaskedEditExtender ID="MskToDate" runat="server" TargetControlID="txtToDate"
                MessageValidatorTip="true" MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="True"
                DisplayMoney="Left" AcceptNegative="Left">
            </cc1:MaskedEditExtender>
        </ContentTemplate>
       
    </asp:UpdatePanel>
</asp:Content>
