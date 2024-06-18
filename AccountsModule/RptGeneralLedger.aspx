<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RptGeneralLedger.aspx.vb"
    Inherits="RptGeneralLedger" MasterPageFile="~/AccountsMaster.master" Strict="true" %>

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
        charset="utf-8"></script>r
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

        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            costgrpAutoCompleteExtenderKeyUp();

            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
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
        function rbevent(rb1, rb2, Opt, Group) {
            var rb2 = document.getElementById(rb2);
            rb1.checked = true;
            rb2.checked = false;
            switch (Group) {

            }

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
        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { txttdate.value = txtfdate.value; }
        }

    </script>
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
         border-bottom: gray 2px solid" Width="100%">
        <tbody>
            <tr>
                <td class="field_heading" align="center" colspan="1">
                    <asp:Label ID="lblHeading" runat="server" Text="General Ledger" ForeColor="White"
                        CssClass="field_heading" Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; height: 24px" class="td_cell">
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td >
                                    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%" GroupingText="Select Date">
                                        <table style="width: 437px">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 48px" class="td_cell">
                                                        &nbsp;From
                                                    </td>
                                                    <td style="width: 293px" class="td_cell">
                                                        <asp:TextBox ID="txtFromDate" runat="server" tabindex=1 CssClass="fiel_input" Width="80px"></asp:TextBox>
                                                        <asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>&nbsp;
                                                        <cc1:MaskedEditValidator ID="MskVFromDt" tabindex=2 runat="server" CssClass="field_error" Width="23px"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="*" ErrorMessage="MskVFromDate" EmptyValueMessage="Date is required"
                                                            EmptyValueBlurredText="*" Display="Dynamic" ControlToValidate="txtFromDate" ControlExtender="MskFromDate"></cc1:MaskedEditValidator>
                                                    </td>
                                                    <td style="width: 24px" class="td_cell">
                                                        To
                                                    </td>
                                                    <td style="width: 274px" class="td_cell">
                                                        <asp:TextBox ID="txtToDate" runat="server" tabindex=3 CssClass="fiel_input" Width="80px"></asp:TextBox>&nbsp;<asp:ImageButton
                                                            ID="ImgBtnRevDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                                        </asp:ImageButton>&nbsp;<cc1:MaskedEditValidator tabindex=4 ID="MskVToDate" runat="server" CssClass="field_error"
                                                            TooltipMessage="Input a date in dd/mm/yyyy format" InvalidValueMessage="Invalid Date"
                                                            InvalidValueBlurredMessage="*" EmptyValueMessage="Date is required" EmptyValueBlurredText="*"
                                                            Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="MskToDate"></cc1:MaskedEditValidator>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Panel ID="Panel3" runat="server" Width="100%"  GroupingText="Select Account">
                                        <table >
                                            <tbody>
                                              <tr>
                                                            <td class="td_cell" style="height: 17px; width: 205px;">
                                                                Select Control Account
                                                            </td>
                                                            <td class="td_cell" style="height: 17px; width: 262px;">
                                                                <asp:TextBox ID="TxtBankName"  runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="5" Width="248px" Height="16px"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                    ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px">
                                                               <asp:TextBox ID="TxtBankCode" runat="server" style="display:none" Width="100px"
                                                               ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                <tr>
                                                    <td class="td_cell">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="td_cell">
                                                        &nbsp;</td>
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
                <td class="td_cell">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 144px" class="td_cell">
                                    Report Type
                                </td>
                                <td style="width: 100px">
                                    <select style="width: 109px" tabindex=6 id="ddlLedgerType" class="field_input" runat="server">
                                        <option value="Detailed" selected>Detailed</option>
                                        <option value="Summary">Summary</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td_cell">
                    <table>
                        <tbody>
                            <tr>
                                <td style="width: 144px" class="td_cell">
                                    Do you&nbsp;print PDC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Details ?&nbsp;
                                </td>
                                <td style="width: 100px">
                                    <select style="width: 109px" tabindex=7 id="ddlPDC" class="field_input" runat="server">
                                        <option value="[Select]" selected>[Select]</option>
                                        <option value="Yes">Yes</option>
                                        <option value="No">No</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="td_cell" style="text-align: center">
                    &nbsp;<asp:Button ID="Button1" runat="server" tabindex=8 Text="Export" CssClass="field_button"
                        OnClick="Button1_Click" style="Display:none"></asp:Button>&nbsp;<asp:Button ID="btnReport" style="display:none"  OnClick="btnReport_Click"
                            runat="server" Text="Load Report" tabindex=9  CssClass="field_button" CausesValidation="False">
                        </asp:Button>&nbsp;<asp:Button ID="btnPdfReport" OnClick="btnPdfReport_Click"
                            runat="server" Text="Pdf Report" tabindex=9  CssClass="field_button" CausesValidation="False">
                                 </asp:Button>&nbsp;<asp:Button ID="btnExlReport" OnClick="btnExlReport_Click"
                            runat="server" Text="Excel Report" tabindex=9  CssClass="field_button" CausesValidation="False">
                        </asp:Button>&nbsp;<asp:Button ID="btnhelp" TabIndex="10" OnClick="btnhelp_Click"
                            runat="server" Text="Help" CssClass="field_button"></asp:Button>
                </td>

   <td style="display:none">
               <asp:Button id="btnadd" tabIndex=16  runat="server"  
        CssClass="field_button"></asp:Button>
        </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>

            </tr>
        </tbody>
    </table>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
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
    &nbsp;&nbsp;
</asp:Content>
