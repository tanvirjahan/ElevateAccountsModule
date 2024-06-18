<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptSupplierTrialBalance.aspx.vb" Inherits="RptSupplierTrialBalance" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="Server">
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
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
            }

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
            $("#<%= txtsuppliername.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtsuppliername.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
                }

            });

            $("#<%= txtsuppliername.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtsuppliername.ClientID%>').value == '') {

                    document.getElementById('<%=txtsuppliercode.ClientID%>').value = '';
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
                    SetsupplierContextkey();
                }

            });

            $("#<%= txtcityname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtcityname.ClientID%>').value == '') {

                    document.getElementById('<%=txtcitycode.ClientID%>').value = '';
                    SetsupplierContextkey();
                }

            });
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
            $find('<%=txtsuppliername_AutoCompleteExtender.ClientID%>').set_contextKey(contxt);

        }


        //-----------------------------------------------------------------
        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }
        //------------------------------------------------------------------------

        //--------Function for diasabled terue/false-----------------------------
        function rbevent(rb1, rb2, Opt, Group) {
            var rb2 = document.getElementById(rb2);
            rb1.checked = true;
            rb2.checked = false;


            if (Opt == 'A') {

                ddlm1.value = '[Select]';
                ddlm2.value = '[Select]';
                ddlm3.value = '[Select]';
                ddlm4.value = '[Select]';

                ddlm1.disabled = true;
                ddlm2.disabled = true;
                ddlm3.disabled = true;
                ddlm4.disabled = true;

            }
            else {
                ddlm1.disabled = false;
                ddlm2.disabled = false;
                ddlm3.disabled = false;
                ddlm4.disabled = false;

            }

        }

        function fillSup(ddlSup) {
            var sup = document.getElementById(ddlSup);

            var connstr = document.getElementById("<%=txtconnection.ClientID%>");
            constr = connstr.value


            if (sup.value == 'S') {


                sqlstr = "select partycode, partyname from partymast where active=1 order by partycode";
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupCodes, ErrorHandler, TimeOutHandler);
                sqlstr1 = "select partyname, partycode from partymast where active=1 order by partyname";
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillFromSupNames, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr1, FillToSupNames, ErrorHandler, TimeOutHandler);
            }
            else if (sup.value == 'A') {

                sqlstr = "select supagentname, supagentcode from supplier_agents where active=1 order by supagentname";
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupNames, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupNames, ErrorHandler, TimeOutHandler);
                sqlstr = "select supagentcode, supagentname from supplier_agents where active=1 order by supagentcode";
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillFromSupCodes, ErrorHandler, TimeOutHandler);
                ColServices.clsServices.GetQueryReturnStringListnew(constr, sqlstr, FillToSupCodes, ErrorHandler, TimeOutHandler);

            }


        }


        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { txttdate.value = txtfdate.value; }
        }
    </script>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100%; border-bottom: gray 2px solid">
        <tbody>
            <tr>
     
                 <td class="field_heading" align="center" colspan="1">
                            <asp:Label ID="lblHeading" runat="server" Text="Supplier Trial Balance" ForeColor="White"
                                CssClass="field_heading" Width="100%"></asp:Label>
                        </td>
            </tr>
            <tr>
                <td >
                    <table style="border: 1px solid gray; width: 100%;" class="td_cell">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Select Supplier/Report Filters"
                                        CssClass="td_cell" Width="100%">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell" style="width: 178px">
                                                        Supplier/ Supplier Agent
                                                    </td>
                                                    <td class="td_cell" style="width: 164px">
                                                        <asp:DropDownList ID="ddlSupType" runat="server" AutoPostBack="True" CssClass="drpdown"
                                                            OnSelectedIndexChanged="ddlSupType_SelectedIndexChanged" Width="125px" 
                                                            TabIndex="1">
                                                            <asp:ListItem Value="S">Supplier</asp:ListItem>
                                                            <asp:ListItem Value="A">Supplier Agent</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="td_cell">
                                                        Currency
                                                    </td>
                                                    <td class="td_cell">
                                                        <select style="width: 123px" id="ddlcurrency" class="drpdown" tabindex="4" onchange="CallWebMethod('sptypecode');"
                                                            runat="server" name="D1">
                                                        </select><input id="Text1" runat="server" style="visibility: hidden; width: 12px;
                                                            height: 9px" type="text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="td_cell" style="width: 178px">
                                                        Report Type
                                                    </td>
                                                    <td class="td_cell" style="width: 164px">
                                                        <asp:DropDownList ID="ddlwithmovmt" runat="server" CssClass="drpdown" onchange="enabledatectrl()"
                                                            Width="125px" Height="19px" TabIndex="2">
                                                            <asp:ListItem Value="0">Transaction</asp:ListItem>
                                                            <asp:ListItem Value="1">Balance</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="td_cell">
                                                        Report Order
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:DropDownList ID="ddlrptord" runat="server" Width="123px" TabIndex="5">
                                                            <asp:ListItem Value="0">Code</asp:ListItem>
                                                            <asp:ListItem Value="1">Name</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="td_cell" style="width: 178px">
                                                        Group By
                                                    </td>
                                                    <td class="td_cell" style="width: 164px">
                                                        <asp:DropDownList ID="ddlgpby" runat="server" Width="122px" TabIndex="3">
                                                            <asp:ListItem Value="0">None</asp:ListItem>
                                                            <asp:ListItem Value="1">Type</asp:ListItem>
                                                            <asp:ListItem Value="2">Category</asp:ListItem>
                                                            <asp:ListItem Value="4">Control account Code</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="td_cell">
                                                        Include Zero
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:DropDownList ID="ddlinclzero" runat="server" Width="122px" TabIndex="6">
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel2" runat="server" GroupingText=" Select Date" CssClass="td_cell"
                                        Width="100%">
                                        <table >
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell">
                                                        <asp:Label ID="Label2" runat="server" Width="110px">From Date</asp:Label>
                                                    </td>
                                                    <td class="td_cell" style="width: 218px">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" Width="115px" 
                                                            Height="18px" TabIndex="7"></asp:TextBox>
                                                        <cc1:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate"
                                                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                                                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                                                            Width="1px"></cc1:MaskedEditValidator>
                                                        &nbsp;
                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="7" />
                                                    </td>
                                                    <td class="td_cell" style="width: 127px">
                                                        <asp:Label ID="Label1" runat="server" Text="To Date" Width="87px"></asp:Label>
                                                    </td>
                                                    <td class="td_cell" style="width: 206px">
                                                        <asp:TextBox ID="txttoDate" runat="server" CssClass="txtbox" Width="114px" 
                                                            Height="19px" TabIndex="8"></asp:TextBox>&#160;
                                                        <asp:ImageButton ID="ImgBtntoDt" runat="server" 
                                                            ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="8">
                                                        </asp:ImageButton>
                                                        <cc1:MaskedEditValidator ID="MskVToDate" runat="server" ControlExtender="MskToDate"
                                                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                                                            EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:Panel ID="Panel4" runat="server" GroupingText="Select Filters" Width="100%"
                                        CssClass="td_cell">
                                        <table style="width:100%">
                                            <tbody>
                                                <tr>
                                                    <td class="td_cell" style="height: 17px; width: 273px;">
                                                      Control Account
                                                    </td>
                                                    <td class="td_cell" style="height: 17px; width: 262px;">
                                                        <asp:TextBox ID="TxtBankName" runat="server" CssClass="field_input" MaxLength="500"
                                                            TabIndex="9" Width="310px" Height="16px"></asp:TextBox>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                            ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td class="td_cell" style="height: 17px">
                                                        <asp:TextBox ID="TxtBankCode" style="display:none" runat="server" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell" style="height: 17px">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                               
                                             
                                              
                                                <tr>
                                                    <td class="td_cell" style="width: 273px">
                                                        Country
                                                    </td>
                                                    <td class="td_cell" style="width: 262px">
                                                        <asp:TextBox ID="txtctryname" runat="server" CssClass="field_input" Height="16px"
                                                            MaxLength="500" TabIndex="10" Width="310px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="txtctryname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                            OnClientItemSelected="ctryautocompleteselected"   ServiceMethod="Getctrylist" TargetControlID="txtctryname">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="HiddenField4" runat="server" />
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:TextBox ID="txtctrycode" style="display:none" runat="server" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                  <tr>
                                                    <td class="td_cell" style="width: 273px">
                                                        City
                                                    </td>
                                                    <td class="td_cell" style="width: 262px">
                                                        <asp:TextBox ID="txtcityname" runat="server" CssClass="field_input" Height="16px"
                                                            MaxLength="500" TabIndex="11" Width="310px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="txtcityname_AutoCompleteExtender2" runat="server" CompletionInterval="10"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                            OnClientItemSelected="cityautocompleteselected" ContextKey="True" ServiceMethod="Getcitylist" TargetControlID="txtcityname">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="HiddenField5" runat="server" />
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:TextBox ID="txtcitycode" style="display:none" runat="server" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                 
                                                <tr>
                                                    <td class="td_cell" style="width: 273px">
                                                        Category
                                                    </td>
                                                    <td class="td_cell" style="width: 262px">
                                                        <asp:TextBox ID="txtcatname" runat="server" CssClass="field_input" Height="16px"
                                                            MaxLength="500" TabIndex="12" Width="310px"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtcatcode" style="display:none" runat="server" Width="100px"></asp:TextBox>
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
                                                            MaxLength="500" TabIndex="13" Width="310px"></asp:TextBox>
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
                                                    <td class="td_cell" style="width: 273px">
                                                        Supplier/Supplier Agent
                                                    </td>
                                                    <td class="td_cell" style="width: 262px">
                                                        <asp:TextBox ID="txtsuppliername" runat="server" CssClass="field_input" Height="16px"
                                                            MaxLength="500" TabIndex="14" Width="310px"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                                                            CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                            FirstRowSelected="false" MinimumPrefixLength="-1" OnClientItemSelected="supplierautocompleteselected"
                                                            ServiceMethod="Getsupplierlist" ContextKey="true"  TargetControlID="txtsuppliername">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                    </td>
                                                    <td class="td_cell">
                                                        <asp:TextBox ID="txtsuppliercode" style="display:none" runat="server" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td class="td_cell">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td colspan="1">
                                    &nbsp;
                                </td>
                            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                        height: 9px" type="text" />
                </td>
                <td colspan="1">
                </td>
            </tr>
            <tr>
                <td style="width: 200px; text-align: left" colspan="1" align="left">
                    <table style="width: 472px">
                        <tr>
                            <td style="width: 100px">
                                &nbsp;
                                <asp:Button ID="Button1" runat="server" CssClass="btn" OnClick="Button1_Click1" Text="Export" />
                            </td>
                            <td style="width: 100px">
                                <asp:Button ID="btnLoadreport" TabIndex="15" OnClick="btnLoadreport_Click" runat="server"
                                    Text="Load Report"  style="display:none"  CssClass="btn" EnableTheming="False" Width="122px"></asp:Button>
                            </td>
                                 <td style="width: 100px">
                                <asp:Button ID="btnPdfReport" TabIndex="15" OnClick="btnPdfReport_Click" runat="server"
                                    Text="Pdf Report"  CssClass="btn" EnableTheming="False" Width="122px"></asp:Button>
                            </td>
                             <td style="width: 100px">
                                <asp:Button ID="btnExcelReport" TabIndex="15" OnClick="btnExcelReport_Click" runat="server"
                                    Text="Excel Report" CssClass="btn" EnableTheming="False" Width="122px"></asp:Button>
                            </td>
                            <td style="width: 100px">
                                <asp:Button ID="btnExit" TabIndex="16" OnClick="btnExit_Click" runat="server" Text=" Exit"
                                    CssClass="btn" CausesValidation="False"></asp:Button>
                            </td>
                            <td style="width: 100px">
                                <asp:Button ID="btnhelp" TabIndex="39" OnClick="btnhelp_Click" runat="server" Text="Help"
                                    CssClass="btn"></asp:Button>
                            </td>
                            <td style="display: none">
                                <asp:Button ID="btnadd" TabIndex="16" runat="server" CssClass="field_button"></asp:Button>
                            </td>
                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" CssClass="td_cell" Font-Size="10px">
                            </asp:GridView>
                        </tr>
                    </table>
                    &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
        </tbody>
            </table>
                    <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
                        TargetControlID="txtFromDate">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MskFromDate" runat="server" TargetControlID="txtFromDate"
                        AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                        MaskType="Date" MessageValidatorTip="true">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditExtender ID="MsktoDate" runat="server" TargetControlID="txttoDate"
                        AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" Mask="99/99/9999"
                        MaskType="Date" MessageValidatorTip="true">
                    </cc1:MaskedEditExtender>
                    <cc1:CalendarExtender ID="ClsExtoDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtntoDt"
                        TargetControlID="txttoDate">
                    </cc1:CalendarExtender>
                    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                        <Services>
                            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
                        </Services>
                    </asp:ScriptManagerProxy>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
