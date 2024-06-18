<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="Bankdetails.aspx.vb" Inherits="AccountsModule_Bankdetails" %>

 <%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
   <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link rel="stylesheet" href="../Content/lib/css/reset.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/icons.css" type="text/css" media="screen" charset="utf-8">
  <link rel="stylesheet" href="../Content/lib/css/workspace.css" type="text/css" media="screen" charset="utf-8">
  <link href="../css/VisualSearch.css" rel="stylesheet" type="text/css" />
  <script src="../Content/vendor/jquery-1.11.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.core.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.widget.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.position.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.menu.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/jquery.ui.autocomplete.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/underscore-1.5.2.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/vendor/backbone-1.1.0.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/visualsearch.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_box.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_facet.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/views/search_input.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_facets.js" type="text/javascript" charset="utf-8"></script> 
  <script src="../Content/lib/js/models/search_query.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/backbone_extensions.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/hotkeys.js" type="text/javascript" charset="utf-8"></script>
  <script src="../Content/lib/js/utils/jquery_extensions.js" type="text/javascript" charset="utf-8"></script>
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
 

    function Fillcurr(result) {
        var acctcurr = document.getElementById("<%=ddlacctcurr.ClientID%>");
        acctcurr.value = result;

    }
 


    function FormValidation(state) {

        if ((document.getElementById("<%=txtbankcode.ClientID%>").value == "") || (document.getElementById("<%=txtbankname.ClientID%>").value == "") || (document.getElementById("<%=txtAcctName.ClientID%>").value == "") || (document.getElementById("<%=ddlacctcurr.ClientID%>").value == "[Select]" || (document.getElementById("<%=txtIBANNumber.ClientID%>").value == "") || (document.getElementById("<%=txtAcctNumber.ClientID%>").value == "") || (document.getElementById("<%=txtswiftcode.ClientID%>").value == ""))) {
             if (document.getElementById("<%=txtbankcode.ClientID%>").value == "") {                     
                       alert("Select Bank Code!! ");
                return false;

            }


          else  if (document.getElementById("<%=txtbankname.ClientID%>").value == "[Select]") {
              document.getElementById("<%=txtbankname.ClientID%>").focus();
                alert("Select Bank Name!! ");
                return false;

            }
            else if (document.getElementById("<%=txtAcctName.ClientID%>").value == "") {
                alert("Account Name cannot be blank.");
                document.getElementById("<%=txtAcctName.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=ddlacctcurr.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlacctcurr.ClientID%>").focus();
                alert("Account Currency is Empty!! ");
                return false;

                  }  else if (document.getElementById("<%=txtAcctNumber.ClientID%>").value =="") {
                alert("Account Number cannot be blank.");
                document.getElementById("<%=txtAcctNumber.ClientID%>").focus();
                return false;
            }
                     else if (document.getElementById("<%=txtIBANNumber.ClientID%>").value =="") {
                alert("IBAN Number cannot be blank.");
                document.getElementById("<%=txtIBANNumber.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtswiftcode.ClientID%>").value == "") {
                document.getElementById("<%=txtswiftcode.ClientID%>").focus();
                alert("Swift Code cannot be blank");
                return false;
            }
        


        }
        else {
            //alert(state);
            if (state == 'New') { if (confirm('Are you sure you want to save Bank Details?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Bank Details ?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Bank Details ?') == false) return false; }
        }
    }



   function costgrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TxtBankCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TxtBankCode.ClientID%>').value = '';
            }

        }



        function CurrencyAutoCompleteExtenderKeyUp() {
            $("#<%=TxtBankName.ClientID %>").bind("change", function () {

                document.getElementById('<%=TxtBankCode.ClientID%>').value = '';
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
                border-bottom: gray 2px solid">
                <tbody>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Bank Details" CssClass="field_heading"
                                Width="680px" Height="16px"></asp:Label>
                        </td>
                    </tr>
                                                                        <tr>
                         <td   class="td_cell" style="width: 131px">Select Bank
</td>
                    <td style="width: 397px; color: #000000">
                        <asp:TextBox ID="TxtBankName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="1" Width="248px" 
                            Height="16px"></asp:TextBox>
                            <asp:TextBox ID="TxtBankCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtBankName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getbankslist" TargetControlID="TxtBankName" OnClientItemSelected="costgrpautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
</tr>
                    <tr>
                        <td style="height: 23px; width: 131px;" class="td_cell">
                            Display Bank Name
                        </td>
                        <td style="height: 23px; width: 397px;">
                            <input style="width: 250px" id="txtDispBankName" class="field_input" tabindex="2"
                                type="text" maxlength="200" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 23px; width: 131px;" class="td_cell">
                            Account Name
                        </td>
                        <td style="height: 23px; width: 397px;">
                            <input style="width: 250px" id="txtAcctName" class="field_input" tabindex="3" type="text"
                                maxlength="200" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            Account Currency
                        </td>
                        <td style="width: 397px">
                            <select id="ddlacctcurr" runat="server" class="field_input"  tabindex="4" name="D1" style="width: 159px"
                                >
                                <option selected=""></option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            Account Number
                        </td>
                        <td style="width: 397px">
                            <input style="width: 159px" id="txtAcctNumber" class="field_input" tabindex="5" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            IBAN Number
                        </td>
                        <td style="width: 397px">
                            <input style="width: 159px" id="txtIBANNumber" class="field_input" tabindex="6" type="text"
                                maxlength="200" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            Swift Code
                        </td>
                        <td style="width: 397px">
                            <input style="width: 160px" id="txtswiftcode" class="field_input" tabindex="7" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell"   height: 29px;"   style="width: 131px; vertical-align: top;" >
                            Others
                        </td>
                        <td style="width: 397px; height: 29px;">
                                  <asp:TextBox ID="txtothers" runat="server" CssClass="field_input" Rows="2"  
                                      TabIndex="8"  
                                                                                    
                                      TextMode="MultiLine" Height="68px" Width="530px"      ></asp:TextBox>
                        </td>
         
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            Branch Name
                        </td>
                        <td style="width: 397px">
                            <input style="width: 253px" id="txtbranch" class="field_input" tabindex="9" type="text"
                                maxlength="100" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            Active
                        </td>
                        <td style="width: 397px">
                            <input id="chkActive" tabindex="10" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style="width: 131px"  >
                            &nbsp;</td>
                        <td style="width: 397px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 400px"  >
                            <asp:Button ID="btnSave" runat="server" tabindex="11" CssClass="field_button" Text="Save" />
                       
                            <asp:Button ID="btnCancel" runat="server" tabindex="12" CssClass="field_button" Text="Return To Search" />
                      
                       <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                                OnClick="btnhelp_Click" TabIndex="8" Text="Help" />
                      
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
