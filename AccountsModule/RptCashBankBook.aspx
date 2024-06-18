<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false"
    CodeFile="RptCashBankBook.aspx.vb" Inherits="RptCashBankBook" %>

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

            bankAutoCompleteExtenderKeyUp();

        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            bankAutoCompleteExtenderKeyUp();


            // after update occur on UpdatePanel re-init the Autocomplete

        }
        function bankautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtbankcode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtbankcode.ClientID%>').value = '';
            }

        }
        function bankAutoCompleteExtenderKeyUp() {
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

        //        function setcontextkey() {
        //            var txtbanknameauto = document.getElementById("<%=AutoCompleteExtender1.ClientID%>");
        //            alert(txtbanknameauto);
        ////                    alert(document.getElementById('<%=ddlcashbanktype.ClientID%>').value);
        //            txtbanknameauto.set_contextKey(document.getElementById('<%=ddlcashbanktype.ClientID%>').value);

        //        }
        function AutoCompleteExtender_txtbankname_OnClientPopulating(sender, args) {
            sender.set_contextKey(document.getElementById('<%=ddlcashbanktype.ClientID%>').value);
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                width: 100%; border-bottom: gray 1px solid" class="td_cell" align="left">
                <tbody>
                    <tr>
                        <td style="width: 100%" class="field_heading" align="center" colspan="5">
                            <asp:Label ID="lblHeading" runat="server" Text="Report Cash / Bank Book" Width="100%"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" class="td_cell" align="left" colspan="5">
                            <table style="width: 100%" class="td_cell">
                                <tbody>
                            <tr>
                                         <td class="td_cell" style="width: 181px">
                            Select Type <span style="color: #ff0000">&nbsp;<span class="td_cell">*</span></span>
                        </td>
                        <td style="width: 392px"  >
                            <select style="width: 114px; height: 20px;" id="ddlCashBankType" class="field_input"
                                tabindex="1" runat="server">
                             <%--   <option value="[Select]" selected>[Select]</option>--%>
                                <option value="C">Cash</option>
                                <option value="B">Bank</option>
                            </select>
                        </td>
                            </tr>
                                       <tr>
                             
                                                            <td class="td_cell" style="height: 17px; width: 109px;">
                                                                Cash/ Bank
                                                            </td>
                                                            <td class="td_cell" style="height: 17px; width: 260px;">
                                                                <asp:TextBox ID="txtbankname"  runat="server" CssClass="field_input"
                                                                    MaxLength="500" TabIndex="2" Width="248px" Height="16px"></asp:TextBox>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                                                                    EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                                                                     UseContextKey ="True" ServiceMethod="Getbankslist" OnClientPopulating="AutoCompleteExtender_txtbankname_OnClientPopulating" TargetControlID="txtbankname" OnClientItemSelected="bankautocompleteselected">
                                                                </asp:AutoCompleteExtender>
                                                                     </td>
                                                            <td class="td_cell" style="height: 17px">
                                                               <asp:TextBox ID="txtbankcode" runat="server" style="display:none" Width="100px"
                                                               ></asp:TextBox>
                                                            </td>
                                                            <td class="td_cell" style="height: 17px">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                    <tr>
                                        <td style="width: 109px" class="td_cell" align="left">
                                            From Date
                                        </td>
                                        <td class="td_cell" valign="middle" align="left" colspan="2">
                                            <asp:TextBox ID="txtFromDate" runat="server" Width="80px" TabIndex="3" CssClass="txtbox" __designer:wfdid="w41"></asp:TextBox><asp:ImageButton
                                                ID="ImgBtnFrmDt" runat="server" __designer:wfdid="w42" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                            </asp:ImageButton>&nbsp;<cc1:MaskedEditValidator ID="MEVFromDate" runat="server"
                                                Width="80px" CssClass="field_error" __designer:wfdid="w45" Display="Dynamic"
                                                ControlToValidate="txtFromDate" ControlExtender="MEFromDate" EmptyValueBlurredText="Date is required"
                                                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>&nbsp;Todate&nbsp;&nbsp;
                                            <asp:TextBox ID="txtToDate" runat="server" Width="80px" TabIndex="4" CssClass="txtbox" __designer:wfdid="w43"></asp:TextBox><asp:ImageButton
                                                ID="ImgBtnToDate" runat="server" __designer:wfdid="w44" ImageUrl="~/Images/Calendar_scheduleHS.png">
                                            </asp:ImageButton>
                                            <cc1:MaskedEditValidator ID="MEVToDate" runat="server" CssClass="field_error" __designer:wfdid="w48"
                                                Display="Dynamic" ControlToValidate="txtToDate" ControlExtender="METoDate" EmptyValueBlurredText="Date is required"
                                                EmptyValueMessage="Date is required" InvalidValueBlurredMessage="Invalid Date"
                                                InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"></cc1:MaskedEditValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 109px" class="td_cell" align="left">
                                            Report Type
                                        </td>
                                        <td class="td_cell" valign="middle" align="left" colspan="2">
                                            <select style="width: 199px" id="ddlType" TabIndex="5" class="drpdown" onchange="CallWebMethod('SupplierAgentCode');"
                                                runat="server">
                                                <option value="Summary" selected>Summary</option>
                                                <option value="Detailed">Detailed</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 109px" class="td_cell" align="left">
                                            Currency
                                        </td>
                                        <td class="td_cell" valign="middle" align="left" colspan="2">
                                            <select style="width: 147px" id="ddlcurrency" class="drpdown" tabindex="6" onchange="CallWebMethod('sptypecode');"
                                                runat="server">
                                            </select>
                                        </td>
                                    </tr>
                                    
                                                  <tr >
                                                  <td>
                                                  </td>
                                        <td   style="width: 109px" class="td_cell"   >
                                           <asp:CheckBox ID="chkinclpagebr" Text ="Include Page Break" tabindex="7"  runat="server"/>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="4">
                                           &nbsp;&nbsp;&nbsp <asp:Button ID="btnLoadReprt" runat="server"  style="display:none" CssClass="btn" 
                                                OnClick="btnLoadReprt_Click" TabIndex="8" Text="Load Report" />
                                            &nbsp;<asp:Button ID="btnPdfReport" runat="server" CssClass="btn" 
                                                OnClick="btnPdfReport_Click" TabIndex="9" Text="Pdf Report" />
                                &nbsp;<asp:Button ID="btnExcelReport" runat="server" CssClass="btn" 
                                                OnClick="btnExcelReport_Click" TabIndex="10" Text="Excel Report" />
                                            &nbsp;
                                            <asp:Button ID="btnhelp" runat="server" __designer:dtid="2814749767106618" 
                                                __designer:wfdid="w4" CssClass="btn" TabIndex="11" OnClick="btnhelp_Click" Text="Help" 
                                                Width="50px" />
                                            <asp:Button ID="btnadd" runat="server" CssClass="field_button" tabIndex="12" />
                                            <asp:Button ID="Button1" runat="server" CssClass="field_button" tabIndex="13" />
                                        </td>

<asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1003px" colspan="5">
                            <cc1:CalendarExtender ID="CEFromDate" runat="server" __designer:wfdid="w46" TargetControlID="txtFromDate"
                                PopupButtonID="ImgBtnFrmDt" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MEFromDate" runat="server" __designer:wfdid="w47" TargetControlID="txtFromDate"
                                Mask="99/99/9999" MaskType="Date">
                            </cc1:MaskedEditExtender>
                            <cc1:CalendarExtender ID="CEToDate" runat="server" __designer:wfdid="w49" TargetControlID="txtToDate"
                                PopupButtonID="ImgBtnToDate" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="METoDate" runat="server" __designer:wfdid="w50" TargetControlID="txtToDate"
                                Mask="99/99/9999" MaskType="Date" Enabled="True">
                            </cc1:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1003px" colspan="5">
                            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                                <Services>
                                    <asp:ServiceReference Path="clsServices.asmx"></asp:ServiceReference>
                                </Services>
                            </asp:ScriptManagerProxy>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function FormValidation() {
            var select = document.getElementById("<%=txtbankcode.ClientID%>");
            var select2 = document.getElementById("<%=ddlcurrency.ClientID%>");

            if (select.options[select.selectedIndex].text == "Select") {
                alert("Please Select Your Bank");
                return false;
            }
            else

                if (select2.options[select2.selectedIndex].text == "Select") {
                    alert("Please Select Your Currency");
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
        function ChangeDate() {

            var txtfdate = document.getElementById("<%=txtFromDate.ClientID%>");
            var txttdate = document.getElementById("<%=txtToDate.ClientID%>");

            if (txtfdate.value == '') { alert("Enter From Date."); txtfdate.focus(); }
            else { txttdate.value = txtfdate.value; }
        }



    </script>
</asp:Content>
