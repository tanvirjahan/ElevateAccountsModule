<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrivilegeRightsforGroup.aspx.vb"
    Inherits="PrivilegeRightsforGroup" MasterPageFile="~/SubPageMaster.master" Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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

            AppAutoCompleteExtenderKeyUp();
            UserGrpAutoCompleteExtenderKeyUp;
        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            AppAutoCompleteExtenderKeyUp();
            UserGrpAutoCompleteExtenderKeyUp;


            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1);  
    </script>
    <script language="javascript" type="text/javascript">
        function appautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtappcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtappcode.ClientID%>').value = '';
            }

        }

        function AppAutoCompleteExtenderKeyUp() {
            $("#<%= txtappname.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtappname.ClientID%>').value == '') {

                    document.getElementById('<%=txtappcode.ClientID%>').value = '';
                }

            });

            $("#<%= txtappname.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=txtappname.ClientID%>').value == '') {

                    document.getElementById('<%=txtappcode.ClientID%>').value = '';
                }

            });
        }

        function usergrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TxtusergrpCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TxtusergrpCode.ClientID%>').value = '';
            }

        }

        function UserGrpAutoCompleteExtenderKeyUp() {
            $("#<%= TxtusergrpName.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=TxtusergrpName.ClientID%>').value == '') {

                    document.getElementById('<%=TxtusergrpCode.ClientID%>').value = '';
                }

            });

            $("#<%= TxtusergrpName.ClientID %>").keyup(function (event) {

                if (document.getElementById('<%=TxtusergrpName.ClientID%>').value == '') {

                    document.getElementById('<%=TxtusergrpCode.ClientID%>').value = '';
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


        function FormValidation(state) {

            if (document.getElementById("<%=Txtusergrpcode.ClientID%>").value == "") {
                document.getElementById("<%=Txtusergrpname.ClientID%>").focus();
                alert("Select User Group");
                return false;
            }
            else if (document.getElementById("<%=txtappcode.ClientID%>").value == "") {
                document.getElementById("<%=txtappname.ClientID%>").focus();
                alert("Select Application");
                return false;
            }
            else {
                //alert(state);
                if (state == 'New') { if (confirm('Are you sure you want to save ?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update ?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete ?') == false) return false; }
            }
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 428px; border-bottom: gray 2px solid; height: 215px">
                <tbody>
                    <tr>
                        <td style="width: 864px" class="td_cell" align="center" colspan="1">
                            <asp:Label ID="lblHeading" runat="server" Text="Privilege Rights for Group" Width="763px"
                                CssClass="field_heading" Height="14px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 864px" valign="top">
                            <table style="width: 782px; height: 148px">
                                <tbody>
                                        <tr>
                          <td style=" height: 24px; width: 98px;">
                                <span style="font-size: 8pt; font-family: Arial"> User Group <span style="color: #ff0000">
                                    *</span></span>
                            </td>
                    <td style="color: #000000; width: 267px;">
                        <asp:TextBox ID="TxtusergrpName" runat="server"  
                            CssClass="field_input" MaxLength="500" TabIndex="1" Width="248px" 
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtusergrpCode" runat="server"   style="display:none"  ></asp:TextBox>
                        <asp:AutoCompleteExtender ID="TxtusergrpName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getusergrpslist" TargetControlID="TxtusergrpName" OnClientItemSelected="usergrpautocompleteselected" >
                        </asp:AutoCompleteExtender>
           
                    </td>
                                          <td style=" height: 24px; width: 98px;">
                                <span style="font-size: 8pt; font-family: Arial">Application <span style="color: #ff0000">
                                    *</span></span>
                            </td>
                    <td style="color: #000000; width: 267px;">
                        <asp:TextBox ID="txtappname" runat="server"  
                            CssClass="field_input" MaxLength="500" TabIndex="1" Width="248px" 
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="txtappcode" runat="server"   style="display:none"  ></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtappname_AutoCompleteExtender1" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getappslist" TargetControlID="txtappname" OnClientItemSelected="appautocompleteselected" >
                        </asp:AutoCompleteExtender>
           
                    </td>
</tr>
                                    <tr>
                                        <td style="width: 512px; height: 1px" class="td_cell">
                                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                                height: 9px" type="text" />
                                        </td>
                                        <td style="width: 261px; height: 1px" class="td_cell">
                                        </td>
                                        <td style="width: 191px; height: 1px" class="td_cell">
                                        </td>
                                        <td style="width: 375px; height: 1px" class="td_cell">
                                            <asp:Button ID="btnFillGrid" runat="server" Text="FillGrid" CssClass="btn"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" align="center" colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" valign="top" align="left" colspan="4">
                                            <asp:GridView ID="grdPrivilege" TabIndex="3" runat="server" Font-Size="10px" Width="518px"
                                                CssClass="grdstyle" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="3"
                                                BorderWidth="1px" BorderStyle="None" BorderColor="#999999">
                                                <FooterStyle CssClass="grdfooter"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="PrivilegeId" HeaderText="Privilege Code"></asp:BoundField>
                                                    <asp:BoundField DataField="PrivilegeName" HeaderText="Privilege Name"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkbox"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="grdRowstyle"></RowStyle>
                                                <SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>
                                                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
                                                <HeaderStyle CssClass="grdheader"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" align="right" colspan="4">
                                            <asp:Button ID="btnSave" TabIndex="4" OnClick="btnSave_Click" runat="server" Text="Save"
                                                CssClass="btn"></asp:Button>&nbsp; &nbsp;<asp:Button ID="btnExit" TabIndex="5" OnClick="btnExit_Click"
                                                    runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp;
                                            <asp:Button ID="btnHelp" TabIndex="6" OnClick="btnHelp_Click" runat="server" Text="Help"
                                                CssClass="btn"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
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
