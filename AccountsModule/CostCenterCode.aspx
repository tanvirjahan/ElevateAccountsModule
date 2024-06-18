<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="CostCenterCode.aspx.vb" Inherits="CostCenterCode" %>

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

        function FormValidation(state) {
            if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtCode.ClientID%>").value <= 0) || (document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=txtcostgrpcode.ClientID%>").value == "") || (document.getElementById("<%=txtcostgrpname.ClientID%>").value == "")) {
                if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    alert("Code field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtCode.ClientID%>").value <= 0) {
                    alert("Code must be greater than zero.");
                    document.getElementById("<%=txtCode.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                    document.getElementById("<%=txtName.ClientID%>").focus();
                    alert("Name field can not be blank");
                    return false;
                }
                else if (document.getElementById("<%=txtcostgrpcode.ClientID%>").value == "") {
                    document.getElementById("<%=txtcostgrpcode.ClientID%>").focus();
                    alert("Group field can not be blank");
                    return false;
                }

            }

            else {
                //       alert(state);
                if (state == 'New') { if (confirm('Are you sure you want to save Cost Center Code ?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Cost Center Code?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Cost Center Code ?') == false) return false; }
            }
        }

  function checkNumber() {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }


        function costgrpautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=TxtCostgrpCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=TxtCostgrpCode.ClientID%>').value = '';
            }

        }



        function CurrencyAutoCompleteExtenderKeyUp() {
            $("#<%=TxtCostGrpName.ClientID %>").bind("change", function () {

                document.getElementById('<%=TxtCostgrpCode.ClientID%>').value = '';
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
            <table style="border: 2px solid gray; width: 425px; height: 187px;">
                <tbody>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Cost Center Code" Width="588px"
                                CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 153px" class="td_cell">
                            Code <span style="color: #ff0000">*</span>
                        </td>
                        <td style="width: 961px; color: #000000">
                            <input style="width: 197px" id="txtCode" class="field_input" tabindex="1" onkeypress="return checkNumber()"
                                type="text" maxlength="20" runat="server" />&nbsp;
                        </td>
                    </tr>
                    <tr style="color: #000000">
                        <td style="width: 153px" class="td_cell">
                            Name <span style="color: #ff0000">*</span>
                        </td>
                        <td style="width: 961px; color: #000000">
                            <input style="width: 198px" id="txtName" class="field_input" tabindex="2" type="text"
                                maxlength="100" runat="server" />&nbsp;
                        </td>
                    </tr>
                                                    <tr>
                         <td style="width: 209px" class="td_cell">Group<span
style="color: #ff0000">*</span>
</td>
                    <td style="width: 961px; color: #000000">
                        <asp:TextBox ID="TxtCostGrpName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="196px"></asp:TextBox>
                            <asp:TextBox ID="TxtCostgrpCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtCostGrpName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getctrylist" TargetControlID="TxtCostGrpName" OnClientItemSelected="costgrpautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
</tr>
                    <tr>
                        <td style="width: 153px" class="td_cell">
                            Active
                        </td>
                        <td style="width: 961px">
                            <input id="chkActive" tabindex="4" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 153px; height: 31px;">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="field_button"></asp:Button>
                        </td>
                        <td style="width: 961px; height: 31px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Return To Search" CssClass="field_button">
                            </asp:Button>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" CssClass="field_button" 
                                OnClick="btnhelp_Click" TabIndex="7" Text="Help" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
