<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="AcctCode.aspx.vb" Inherits="AcctCode" %>

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

            CtryAutoCompleteExtenderKeyUp();
        });

    </script>
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequestUserControl);
        prm.add_endRequest(EndRequestUserControl);



        function EndRequestUserControl(sender, args) {


            CtryAutoCompleteExtenderKeyUp();


            // after update occur on UpdatePanel re-init the Autocomplete

        }
    </script>
    <script language="javascript" type="text/javascript">

 

        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }

        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            alert(msg);
        }

        function sel_control_ac(a) {


            if (a.value == 'Y') {
                var lbl = document.getElementById("<%=lblCustSupp.ClientId%>");
                $("#" + "<%=lblCustSupp.ClientID %>").slideDown();
                var ddl = document.getElementById("<%=ddCustSupp.ClientId%>");
                $("#" + "<%=ddCustSupp.ClientID %>").slideDown();

            }
            else {
                var lbl = document.getElementById("<%=lblCustSupp.ClientId%>");
                $("#" + "<%=lblCustSupp.ClientID %>").fadeOut();
                var ddl = document.getElementById("<%=ddCustSupp.ClientId%>");
                $("#" + "<%=ddCustSupp.ClientID %>").fadeOut();
               
            }
        }

        function sel_bank_ac() {
          
            var sel = document.getElementById("<%=ddlBankAc.ClientId%>");
              if (sel.options[sel.selectedIndex].value == 'Y') {
                          //document.getElementById("yourID").style.display = "block";
                  $("#" + "<%=banktype.ClientID %>").slideDown();
                                     
            }
            else {
        
               // document.getElementById("yourID").style.display = "none";
               $("#" + "<%=banktype.ClientID %>").fadeOut();

                     }
        }

        function costgrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TxtCurrencyCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TxtCurrencyCode.ClientID%>').value = '';
            }

        }



        function CurrencyAutoCompleteExtenderKeyUp() {
            $("#<%=TxtCurrencyName.ClientID %>").bind("change", function () {

                document.getElementById('<%=TxtCurrencyCode.ClientID%>').value = '';
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

        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
                return false;
            }
        }
    
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid" class="td_cell">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" style="width: 565px">
                            <asp:Label ID="lblHeading" runat="server" Text="Add Account Code" CssClass="field_heading"
                                Width="469px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 565px">
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="td_cell" style="width: 125px">
                                            Account Code
                                        </td>
                                        <td id="Td1" style="width: 212px">
                                            <input id="txtAccCode" class="field_input" tabindex="1" type="text" maxlength="20"
                                                runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" style="width: 125px">
                                            Account Name
                                        </td>
                                        <td colspan="3">
                                            <input style="width: 207px" id="txtAccName" class="field_input" tabindex="2" type="text"
                                                maxlength="100" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" style="width: 125px">
                                            Control A/C
                                        </td>
                                        <td style="width: 212px">
                                            <select style="width: 90px" id="ddlControlAc" class="field_input" tabindex="3" onchange="sel_control_ac(this);"
                                                runat="server">
                                                <option value="N" selected>No</option>
                                                <option value="Y">Yes</option>
                                            </select>
                                        </td>
                                        <td>
                                            <div id="lblCustSupp" class="td_cell" runat="server">
                                                Customer / supplier</div>
                                        </td>
                                        <td>
                                        <div  id="ddCustSupp" class="td_cell" runat="server">
                                            <select style="width: 72px" id="ddlCustSupp" class="field_input" tabindex="4" runat="server">
                                                <option value="C" selected>C</option>
                                                <option value="S">S</option>
                                            </select>
                                            </div> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" style="width: 125px; height: 27px;">
                                            Bank A/C
                                        </td>
                                        <td style="width: 212px; height: 27px;">
                                            <select style="width: 90px" id="ddlBankAc" class="field_input" tabindex="5" onchange="sel_bank_ac();"  
                                                runat="server">
                                                <option value="N" selected>No</option>
                                                <option value="Y">Yes</option>
                                            </select>
                                        </td>
                                        <td style="height: 27px">
                                            &nbsp;</td>
                                        <td style="height: 27px">
                                            </td>
                                    </tr>
                                    <tr>
                                    <td colspan ="4">
                                      <div ID="banktype" runat="server" class="td_cell" style="display:none" >
                                    <table >
                                    <tbody>
                                  
                                                                                            <tr>
                                                                                                <td class="td_cell" style="width: 125px; height: 30px;">
                                                                                                    <div ID="lblbantypename" runat="server" class="td_cell">
                                                                                                        Select Bank Type</div>
                                                                                                </td>
                                                                                                <td style="width: 212px; height: 30px;">
                                                                                                    <select ID="ddlBankType" runat="server" class="field_input" name="D1" 
                                                                                                        style="width: 207px" tabindex="6">
                                                                                                    </select>
                                                                                                    <%-- <asp:DropDownList  ID="ddlBankType" runc ="Server" onchange="sel_bank_ac();"  tabindex="6">
                                                                                                <asp:ListItem Text ="Yes" Value ="Y" />
                                                                                                <asp:ListItem >

                                                                                                </asp:DropDownList>--%>
                                                                                                </td>
                                                                                            </tr>

                                                                                                  <tr>
                         <td style="width: 125px" class="td_cell">
                             <div ID="lblCurrency" runat="server" class="td_cell">
                                 Currency
                             </div>
</td>
                    <td style="width: 212px; color: #000000">
                        <asp:TextBox ID="TxtCurrencyName" runat="server" 
                            CssClass="field_input" MaxLength="500" TabIndex="7" Width="207px" 
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtCurrencyCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtCurrencyName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getctrylist" TargetControlID="TxtCurrencyName" OnClientItemSelected="costgrpautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
</tr>
                                                                                                 
                                    </tbody>
                                    </table>
                                    </div>
                                    </td>
                                    
                                    </tr>
                                                                      
                                                                                  

                             
                                        
                                    <tr>
                                        <td class="td_cell" style="width: 125px">
                                            </td>
                                        <td style="width: 212px; color: #000000">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td class="td_cell" style="width: 125px">
                                            &nbsp;</td>
                                        <td style="width: 212px; color: #000000">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="4">
                                            <input id="txtconnection" runat="server" style="visibility: hidden; width: 38px;
                                                height: 9px" type="text" />&nbsp;<asp:Button ID="btnSave" TabIndex="8" runat="server"
                                                    Text="Save" Font-Bold="True" CssClass="field_button"></asp:Button>&nbsp;<asp:Button
                                                        ID="btnExit" TabIndex="9" runat="server" Text="Exit" Font-Bold="True" CssClass="field_button">
                                                    </asp:Button>&nbsp;<asp:Button ID="btnhelp" TabIndex="10" OnClick="btnhelp_Click"
                                                        runat="server" Text="Help" CssClass="field_button"></asp:Button>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="4">
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
</asp:Content>
