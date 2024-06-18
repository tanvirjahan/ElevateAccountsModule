<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="OtherServicesPLTypes.aspx.vb" Inherits="OtherServicesPLTypes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
            AutoCompleteExtenderKeyUp();


        });
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            OthGrpAutoCompleteExtenderKeyUp();
            PrefSuppAutoCompleteExtenderKeyUp();

        }
    </script>
    <script language="javascript" type="text/javascript">

        function PopUpImageView(code) {

            var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
            var lblfilename = document.getElementById("<%=txtimg.ClientID%>");

            if (FileName.value == "") {
                FileName.value = code;
            }

            if (lblfilename != "") {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value, 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
                FileName.value = "";
                return false

            }
            else {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
            }
        }
        function OthGrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtOthGrpcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtOthGrpcode.ClientID%>').value = '';
            }

        }

        function OthGrpAutoCompleteExtenderKeyUp() {
            $("#<%= txtOthGrpname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtOthGrpname.ClientID%>').value == '') {

                    document.getElementById('<%=txtOthGrpcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtOthGrpname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtOthGrpname.ClientID%>').value == '') {

                    document.getElementById('<%=txtOthGrpcode.ClientID%>').value = '';
                }

            });
        }

        function PrefSuppautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
            }

        }

        function PrefSuppAutoCompleteExtenderKeyUp() {
            $("#<%= txtPrefSupName.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtPrefSupName.ClientID%>').value == '') {

                    document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
                }

            });

            $("#<%= txtOthGrpname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtOthGrpname.ClientID%>').value == '') {

                    document.getElementById('<%=txtOthGrpcode.ClientID%>').value = '';
                }

            });
        }
        function Validate(state) {

            if (state == 'New' || state == 'Edit') {

                //             if (document.getElementById("<%=txtCode.ClientID%>").value == '') {
                //                 document.getElementById("<%=txtCode.ClientID%>").focus();
                //                 alert('Please Enter Code');
                //                 return false;
                //             } else

                if (document.getElementById("<%=txtName.ClientID%>").value == '') {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert('Please Enter Name');
                    return false;
                }
                else {
                    if (state == 'New') { if (confirm('Are you sure you want to save this Type?') == false) return false; }
                    if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                    if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
                }

            }
        }

        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 714px; border-bottom: gray 2px solid; text-align: left">
                <tbody>
                    <tr>
                        <td style="height: 3px; text-align: center" class="td_cell" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Other Services Types" ForeColor="White"
                                __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="height: 24px">
                            Code<span style="color: #ff0000" class="td_cell">*</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td colspan="4">
                            <input id="txtCode" class="field_input" style="width: 182px" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    </TR>
                    <tr>
                        <td class="td_cell">
                            Name<span class="td_cell" style="color: #ff0000">*</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td colspan="4">
                            <input id="txtName" class="field_input" style="width: 540px" tabindex="1" type="text"
                                maxlength="500" runat="server" />
                        </td>
                        <tr>
                            <td class="td_cell" style="height: 24px">
                                Group<span style="color: #ff0000">*</span>
                            </td>
                            <td align="left" valign="top" colspan="4">
                                <asp:TextBox ID="txtOthGrpname" Width="300px" runat="server" CssClass="field_input"
                                    MaxLength="500" TabIndex="3"></asp:TextBox>
                                <asp:TextBox ID="txtOthGrpCode" runat="server" Style="display: none"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txtOthGrpname_AutoCompleteExtender" runat="server"
                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                    FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="OthGrpautocompleteselected"
                                    ServiceMethod="GetOthGrpslist" TargetControlID="txtOthGrpname">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="height: 24px;">
                                <asp:Label runat="server" ID="lblprefsupp" Text="Preferred Supplier"></asp:Label><span
                                    style="color: #ff0000">*</span>
                            </td>
                            <td align="left" valign="top" colspan="4">
                                <asp:TextBox ID="txtPrefSupName" runat="server" CssClass="field_input" MaxLength="500"
                                    TabIndex="4" Width="300px"></asp:TextBox>
                                <asp:TextBox ID="txtPrefSupCode" runat="server" Style="display: none"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txtPrefSupName_AutoCompleteExtender" runat="server"
                                    CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                    FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="PrefSuppautocompleteselected"
                                    ServiceMethod="GetPreferSupplist" TargetControlID="txtPrefSupName">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="text-align: left; vertical-align: top; height: 24px;">
                                Image
                            </td>
                            <td colspan="4">
                                <asp:FileUpload ID="OthServiceImg" TabIndex="4" runat="server" />(203 X151) &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="text-align: left; vertical-align: top;">
                            </td>
                            <td align="left" valign="top" style="height: 22px" colspan="4">
                                <input style="width: 329px" id="txtimg" readonly="true" tabindex="22" type="text"
                                    maxlength="30" runat="server" />
                                <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" Text="View"
                                    Width="64px" TabIndex="5" />
                                &nbsp;<asp:Button ID="Btnremove" runat="server" CssClass="field_button" Text="Remove"
                                    Width="77px" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_cell" style="text-align: left; vertical-align: top; height: 24px;">
                                Info For Online
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" CssClass="field_input"
                                    Height="116px" TabIndex="7" TextMode="MultiLine" Width="548px"></asp:TextBox>
                            </td>
                        </tr>
                    </tr>
                    <tr>
                        <td class="td_cell" style="height: 24px;">
                          <label id="lblExcTaxInv"  class="field_caption" runat="server" visible="false" >Exclude Tax Invoice</label>                            
                        </td>
                        <td>
                            <input id="chkExcTaxInv" class="field_input" tabindex="8" type="checkbox" runat="server" visible="false" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="height: 24px;">
                            &nbsp;Active &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <input id="ChkInactive" tabindex="8" type="checkbox" checked runat="server" />
                        </td>
                        <td class="td_cell" style="float: right">
                            TimeLimit To Check
                        </td>
                        <td class="td_cell">
                            <input id="chktimelimit" tabindex="9" type="checkbox" checked runat="server" />
                        </td>
                    </tr>                    
                    <tr>
                        <td style="height: 3px; height: 24px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" OnClick="btnSave_Click"
                                TabIndex="9" Text="Save" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td colspan="4">
                            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" OnClick="btnCancel_Click"
                                TabIndex="10" Text="Return To Search" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8" CssClass="field_button"
                                OnClick="btnhelp_Click" TabIndex="11" Text="Help" />
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
