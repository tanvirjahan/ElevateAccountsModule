<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="CustomerSector.aspx.vb" Inherits="CustomerSector" %>
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
      function FormValidation(state) {
          if ((document.getElementById("<%=txtSectorCode.ClientID%>").value == "") || (document.getElementById("<%=txtSectorName.ClientID%>").value == "")) {
              if (document.getElementById("<%=txtSectorCode.ClientID%>").value == "") {
                  document.getElementById("<%=txtSectorCode.ClientID%>").focus();
                  alert("Code field cannot be blank");
                  return false;
              }
              else if (document.getElementById("<%=txtSectorName.ClientID%>").value == "") {
                  document.getElementById("<%=txtSectorName.ClientID%>").focus();
                  alert("Name field cannot be blank");
                  return false;
              }
              else if ((document.getElementById("<%=TxtCtryCode.ClientID%>").value == "") || (document.getElementById("<%=TxtCtryName.ClientID%>").value == "")) {
                   document.getElementById("<%=txtCtryName.ClientID%>").focus();
                    alert("Please Select Country ");
                                  return false;
                              }

          }
          else {
              if (state == 'New') { if (confirm('Are you sure you want to save Customer Sector Type?') == false) return false; }
              if (state == 'Edit') { if (confirm('Are you sure you want to update Customer Sector Type?') == false) return false; }
              if (state == 'Delete') { if (confirm('Are you sure you want to delete Customer Sector Type?') == false) return false; }
          }
      }
        function checkNumber() {
            if (event.keyCode < 45 || event.keyCode > 57) {
                return false;
            }
        }


        function ctryautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=Txtctrycode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=Txtctrycode.ClientID%>').value = '';
            }

        }



        function CurrencyAutoCompleteExtenderKeyUp() {
            $("#<%=TxtCtryname.ClientID %>").bind("change", function () {

                document.getElementById('<%=Txtctrycode.ClientID%>').value = '';
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
              <TABLE style="border: 2px solid gray; width: 320px;">
                <tbody>
                    <tr>
                         <TD  colspan ="2"class="td_cell" align="center" style="width: 49px">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Sector" CssClass="field_heading"
                               Width="310px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                       <TD style="WIDTH: 117px" class="td_cell">
                            <span style="color: #000000">Code </span><span style="color: #ff0000" class="td_cell">
                                *</span>
                        </td>
                        <TD style="COLOR: #000000; width: 355px;" >
                            <input style="width: 196px" id="txtSectorCode" class="field_input" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                                            <td style="width: 209px" class="td_cell">
                       
                            Name <span style="color: #ff0000" class="td_cell">*</span>
                        </td>
                        <td style="width: 535px; color: #000000">
                            <input style="width: 196px" id="txtSectorName" class="field_input" tabindex="2" type="text"
                                maxlength="150" runat="server" />
                        </td>
                    </tr>
                                                    <tr>
                         <td style="width: 209px" class="td_cell">Country<span
style="color: #ff0000">*</span>
</td>
                    <td style="width: 535px; color: #000000">
                        <asp:TextBox ID="TxtCtryName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="3" Width="196px"></asp:TextBox>
                            <asp:TextBox ID="TxtCtryCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="HiddenField1" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtCtryName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getctrylist" TargetControlID="TxtCtryName" OnClientItemSelected="ctryautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text3" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text4" class="field_input" type="text"
                             runat="server" />
                    </td>
</tr>
      <tr>
                        <td style="width: 209px" class="td_cell">
                            Active
                        </td>
                        <td style="width: 535px; height: 24px">
                            <input id="chkActive" tabindex="7" type="checkbox" checked runat="server" />
                        </td>
                    </tr>
  <tr>
                        <td style="WIDTH: 117px">
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="8" 
                                Text="Save" />
                        </td>
                        <td style="width: 355px">
                            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                TabIndex="9" Text="Return To Search" Width="140px" />
                          
                            <asp:Button ID="btnHelp" runat="server" CssClass="field_button" 
                                TabIndex="10" Text="Help" OnClick="btnhelp_click" />
                                </td>
                                <tr>
                                <td style="WIDTH: 117px">
                                  <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                                height: 9px" type="text" /></td>
                             
                            <td style="width: 355px">
                            <input id="txtctry" runat="server" style="visibility: hidden; width: 12px; height: 9px"
                                type="text" />
                        </td>   </tr>
                  </tbody> 
</table> 

    <table style="border-right: gray 0px solid; border-top: gray 0px solid; border-left: gray 0px solid;
                width: 656px; border-bottom: gray 0px solid">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblwebserviceerror" runat="server" Style="display: none" 
                                Text="Webserviceerror"></asp:Label>
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
