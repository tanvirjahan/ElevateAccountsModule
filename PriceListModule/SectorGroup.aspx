<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="SectorGroup.aspx.vb" Inherits="Sector" %>
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

            countryAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            AcctAutoCompleteExtenderKeyUp();
            AcctExpAutoCompleteExtenderKeyUp();

        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            countryAutoCompleteExtenderKeyUp();
            cityAutoCompleteExtenderKeyUp();
            AcctAutoCompleteExtenderKeyUp();
            AcctExpAutoCompleteExtenderKeyUp();
            // after update occur on UpdatePanel re-init the Autocomplete
        }
    </script>
    <script language="javascript" type="text/javascript">

        function cityautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcitycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
            }
        }

        function cityAutoCompleteExtenderKeyUp() {
            $("#<%=txtcityname.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtcitycode.ClientID%>').value = '';
            });
        }

        function countryAutoCompleteExtenderKeyUp() {
            $("#<%=txtcountryname.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
            });
        }



        function countryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtcountrycode.ClientID%>').value = '';
            }
        }

        function AcctAutoCompleteExtenderKeyUp() {
            $("#<%=txtincomename.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtincomecode.ClientID%>').value = '';
            });
        }
        function accountautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtincomecode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtincomecode.ClientID%>').value = '';
            }
        }


        function AcctExpAutoCompleteExtenderKeyUp() {
            $("#<%=txtexpensename.ClientID %>").bind("change", function () {
                document.getElementById('<%=txtexpensecode.ClientID%>').value = '';
            });
        }
        function accountExpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtexpensecode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtexpensecode.ClientID%>').value = '';
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


        function FormValidation(state) {
            if ((document.getElementById("<%=txtSectorCode.ClientID%>").value == "") || (document.getElementById("<%=txtSectorName.ClientID%>").value == "") || (document.getElementById("<%=txtCountryName.ClientID%>").value == "") || (document.getElementById("<%=txtCityName.ClientID%>").value == "") || (document.getElementById("<%=txtExpenseName.ClientID%>").value == "") || (document.getElementById("<%=txtIncomeName.ClientID%>").value == "")) {
                if (document.getElementById("<%=txtSectorCode.ClientID%>").value == "") {
                    document.getElementById("<%=txtSectorCode.ClientID%>").focus();
                    alert("Code field cannot be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtSectorName.ClientID%>").value == "") {
                    document.getElementById("<%=txtSectorName.ClientID%>").focus();
                    alert("Name  field cannot be blank");
                    return false;
                }

                else if (document.getElementById("<%=txtcountryName.ClientID%>").value == "") {
                    document.getElementById("<%=txtcountryName.ClientID%>").focus();
                    alert("Country  field cannot be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtcityName.ClientID%>").value == "") {
                    document.getElementById("<%=txtcityName.ClientID%>").focus();
                    alert("City  field cannot be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtincomeName.ClientID%>").value == "") {
                    document.getElementById("<%=txtincomeName.ClientID%>").focus();
                    alert("Income field cannot be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtexpenseName.ClientID%>").value == "") {
                    document.getElementById("<%=txtexpenseName.ClientID%>").focus();
                    alert("Expense field cannot be blank");
                    return false;
                }
            }
            else {
                if (state == 'New') { if (confirm('Are you sure you want to save Sector Group?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Sector Group') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Sector Group?') == false) return false; }
            }
        }
 

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border: 2px solid gray; width: 367px; height: 327px;">
                <tbody>
                    <tr>
                        <td class="td_cell" align="center" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Sector Group" Width="514px"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #ff0000">
                        <td style="width: 132px; height: 15px;" class="td_cell">
                            <span style="color: #000000">Sector Code </span><span style="color: #ff0000" class="td_cell">
                                *</span>
                        </td>
                        <td style="width: 231px; height: 15px; color: #000000">
                            <input style="width: 200px" id="txtSectorCode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 15px;" class="td_cell">
                            Sector Name <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="width: 231px; color: #000000; height: 15px;">
                            <input style="width: 200px" id="txtSectorName" class="field_input" tabindex="2" type="text"
                                maxlength="150" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 15px;" align="left" class="td_cell"> 
                            <asp:Label ID="lblcountry" runat="server" Text="Country" Width="52px"></asp:Label><span
                                style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" style="width: 231px; height: 15px;">
                            <asp:TextBox ID="txtcountryname" runat="server" AutoPostBack="True" CssClass="field_input"
                                MaxLength="500" TabIndex="3" Width="200px"></asp:TextBox>
                            <asp:TextBox ID="txtcountrycode" runat="server" Style="display: none"></asp:TextBox>
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
                    <tr>
                        <td style="width: 132px; height:15px;" align="left"  class="td_cell" >  
                            <asp:Label ID="lblcity" runat="server" Text="City" Width="33px"></asp:Label><span
                                style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" style="width: 231px; height: 15px;">
                            <asp:TextBox ID="txtcityname" runat="server" AutoPostBack="True" CssClass="field_input"
                                MaxLength="500" TabIndex="4" Width="200px"></asp:TextBox>
                            <asp:TextBox ID="txtcitycode" runat="server" Style="display: none"></asp:TextBox>
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
                        <td style="width: 132px;height:15px;" align="left"  class="td_cell" >  
                            <asp:Label ID="lblincomecode" runat="server" Text="Income " Width="50px" 
                                Height="16px"></asp:Label>
                                <span
                                style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" style="width: 231px">
                            <asp:TextBox ID="txtincomename" runat="server" AutoPostBack="True" CssClass="field_input"
                                MaxLength="500" TabIndex="5" Width="235px" Height="16px"></asp:TextBox>
                            <asp:TextBox ID="txtincomecode" runat="server" Style="display: none"></asp:TextBox>
                            <asp:HiddenField ID="hdnacctname" runat="server" />
                            <asp:AutoCompleteExtender ID="Income_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                ServiceMethod="GetAccountlist" TargetControlID="txtincomename" OnClientItemSelected="accountautocompleteselected">
                            </asp:AutoCompleteExtender>
                            <input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
                            <input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 15px;" align="left"  class="td_cell" > 
                            <asp:Label ID="lblexpensecode" runat="server" Text="Expense" Width="57px" 
                                Height="20px"></asp:Label><span
                                style="color: #ff0000">*</span>
                        </td>
                        <td align="left" valign="top" colspan="2" style="width: 231px; height: 15px;">
                            <asp:TextBox ID="TxtExpenseName" runat="server" AutoPostBack="True" CssClass="field_input"
                                MaxLength="500" TabIndex="6" Width="235px" Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtExpenseCode" runat="server" Style="display: none"></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:AutoCompleteExtender ID="Expense_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                ServiceMethod="GetAccountexplist" TargetControlID="TxtExpenseName" OnClientItemSelected="accountExpautocompleteselected">
                            </asp:AutoCompleteExtender>
                            <input style="display: none" id="Text3" class="field_input" type="text" runat="server" />
                            <input style="display: none" id="Text4" class="field_input" type="text" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px;height: 15px;" class="td_cell">
                            Active
                        </td>
                        <td style="width: 231px">
                            <input id="chkActive" tabindex="7" type="checkbox" checked runat="server" />
                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                height: 9px" type="text" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 56px;">
                           <asp:Button ID="btnSave" TabIndex="8" runat="server" Text="Save" CssClass="field_button">
                            </asp:Button>
                        </td>
                        <td style="width: 231px; height: 56px;">
                            <asp:Button ID="btnCancel" TabIndex="9" OnClick="btnCancel_Click" runat="server"
                                Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;
                            <asp:Button ID="btnhelp" TabIndex="10" OnClick="btnhelp_Click" runat="server" Text="Help"
                                CssClass="field_button"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <table style="border-right: gray 0px solid; border-top: gray 0px solid; border-left: gray 0px solid;
                width: 656px; border-bottom: gray 0px solid">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblwebserviceerror" runat="server" Style="display: none" Text="Webserviceerror"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
