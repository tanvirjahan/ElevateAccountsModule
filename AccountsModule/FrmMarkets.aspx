
<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FrmMarkets.aspx.vb" Inherits="FrmMarkets"  MasterPageFile="~/SubPageMaster.master" Strict="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
 
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
 
 
       <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
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


           DivisionAutoCompleteExtenderKeyUp();
       });



       

        </script>
        <script type="text/javascript">

    function AutoCompleteExtenderKeyUp() {  

 
    function DivisionAutoCompleteExtenderKeyUp() {

        $("#<%= TxtDivisionName.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=TxtDivisionName.ClientID%>').value == '') {

                document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
            }

        });

        $("#<%=TxtDivisionName.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=TxtDivisionName.ClientID%>').value == '') {

                document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
            }

        });

    }




    function divisionautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=TxtDivisionCode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=TxtDivisionCode.ClientID%>').value = '';
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
<script language="javascript" type="text/javascript">
    function FormValidation(state) {
        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "")) {
            //           if (document.getElementById("<%=txtCode.ClientID%>").value==""){
            //            document.getElementById("<%=txtCode.ClientID%>").focus(); 
            //             alert("Code field can not be blank");
            //            return false;
            //           }
            if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Market?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Market?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Market?') == false) return false; }
        }
    }
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Market" CssClass="field_heading" Width="415px">
</asp:Label></TD></TR>


       <TR><TD style="WIDTH: 145px" class="td_cell"><SPAN>Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
       <TD style="COLOR: #000000">
        <INPUT style="WIDTH: 194px" id="txtCode" class="field_input"  tabIndex=1 type=text maxLength=20 runat="server" contenteditable="inherit" readonly="readonly" /> </TD></TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
            <TD><INPUT style="WIDTH: 194px" id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /> </TD>
        </TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 24px;display:none" class="td_cell">Head Of Department <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
            <TD style="display:none"> <asp:DropDownList style="WIDTH: 194px" ID="ddlHOD" runat ="server" ></asp:DropDownList>  </TD>
        </TR>
        <TR>
            <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell">Active</TD>
            <TD style="HEIGHT: 16px"><INPUT id="chkActive" tabIndex=3 type=checkbox CHECKED runat="server" /></TD></TR>
                                 
        <TR>
           <%-- <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell">Show in Web</TD>--%>
            <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell"></TD>
            <TD style="HEIGHT: 16px"><INPUT id="chkShowInweb" tabIndex=4 type=checkbox CHECKED runat="server" style="display:none" /></TD></TR>
        <TR><TD style="WIDTH: 145px"><asp:Button id="btnSave" tabIndex=5 runat="server" Text="Save" CssClass="btn"></asp:Button></TD>
            <TD><asp:Button id="btnCancel" tabIndex=6 runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp;
            <asp:Button id="btnhelp" tabIndex=5 runat="server" Text="Help" CssClass="btn" __designer:wfdid="w19" OnClick="btnhelp_Click">
            </asp:Button></TD></TR>
            <tr><td colspan=4>
            <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr>
            


            <tr  style="display:none">
          


              <td align="left" valign="top" colspan="1" width="300px">
                        <asp:TextBox ID="TxtDivisionName" runat="server" AutoPostBack="True"
                            CssClass="field_input" MaxLength="500" TabIndex="12" Width="297px"></asp:TextBox>
                            <asp:TextBox ID="TxtDivisionCode" runat="server" style="display:none"   ></asp:TextBox>
                            <asp:HiddenField ID="hdndivisioncode" runat="server"  />
                        <asp:AutoCompleteExtender ID="TxtDivisionName_AutoCompleteExtender" runat="server" CompletionInterval="10"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
                            EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="0"
                            ServiceMethod="Getdivisionslist" TargetControlID="TxtDivisionName" OnClientItemSelected="divisionautocompleteselected" >
                        </asp:AutoCompleteExtender>
                        <input style="display:none" id="Text17" class="field_input" type="text"
                           runat="server" />
                        <input style="display:none" id="Text18" class="field_input" type="text"
                             runat="server" />
                    </td>
              </tr>
            
            </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>


</asp:Content>

