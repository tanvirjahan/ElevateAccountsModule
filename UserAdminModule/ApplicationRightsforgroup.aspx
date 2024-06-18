<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ApplicationRightsforgroup.aspx.vb" Inherits="ApplicationRightsforgroup"  %>
    
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
  <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
   
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

         txtusernameAutoCompleteExtenderKeyUp();

     });
</script>
     
     
      
<script language="javascript" type="text/javascript">

    function txtusernameAutoCompleteExtenderKeyUp() {
        $("#<%= txtusername.ClientID %>").bind("change", function () {

            if (document.getElementById('<%=txtusername.ClientID%>').value == '') {

                document.getElementById('<%=txtusercode.ClientID%>').value = '';
            }

        });

        $("#<%= txtusername.ClientID %>").keyup(function (event) {

            if (document.getElementById('<%=txtusername.ClientID%>').value == '') {

                document.getElementById('<%=txtusercode.ClientID%>').value = '';
            }

        });
    }


    function txtusernameautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtusercode.ClientID%>').value = eventArgs.get_value();
        }
        else {
            document.getElementById('<%=txtusercode.ClientID%>').value = '';
        }
    }

</script>


   <script type="text/javascript">
       var prm = Sys.WebForms.PageRequestManager.getInstance();
       prm.add_endRequest(EndRequestUserControl);
       function EndRequestUserControl(sender, args) {
           txtusernameAutoCompleteExtenderKeyUp();
          
       }
</script>
   
   
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD align=center colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Assign Application Rights for groups " CssClass="field_heading" Width="100%"></asp:Label></TD></TR>


   
                                                    <tr>    <td style="width: 130px" align="left">
                                                            <asp:Label ID="Label2" runat="server" Text="User Group" Width="90px"></asp:Label><span
                                                                style="color: #ff0000">*</span>
                                                        
                                                            <asp:TextBox ID="txtusername" runat="server" CssClass="field_input" MaxLength="500"
                                                                TabIndex="3" Width="200px"></asp:TextBox>
                                                            <asp:TextBox ID="txtusercode" runat="server"></asp:TextBox>
                                                            <asp:HiddenField ID="HiddenField21" runat="server" />
                                                            <asp:AutoCompleteExtender ID="txtusername_AutoCompleteExtender" runat="server"
                                                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                                                FirstRowSelected="false" MinimumPrefixLength="0" ServiceMethod="Getusergroup"
                                                                TargetControlID="txtusername" OnClientItemSelected="txtusernameautocompleteselected">
                                                            </asp:AutoCompleteExtender>
                                                            <input style="display: none" id="Text5" class="field_input" type="text" runat="server" />
                                                            <input style="display: none" id="Text6" class="field_input" type="text" runat="server" />
                                                        </td>
                                                    </tr>



<TR><TD style="HEIGHT: 24px" class="td_cell" align=right colSpan=1><asp:Panel id="pnlApplicableMarkets" runat="server" align=center Width="100%" ScrollBars="Auto" GroupingText="Application<font color=red >*</>"><asp:GridView id="grdApplicaion" tabIndex=12 runat="server" Font-Size="10px" BackColor="White" CssClass="td_cell" Width="100%" PageSize="20" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:BoundField Visible="False" HeaderText="Sr No"></asp:BoundField>
<asp:TemplateField HeaderText="Select">



<ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>

<HeaderStyle Width="60px" HorizontalAlign="Center"></HeaderStyle>
<ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="appid" HeaderText="Application Code">
<ItemStyle Width="400px"></ItemStyle>

<HeaderStyle Width="400px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="appname" HeaderText="Application Name">
<ItemStyle Width="1000px"></ItemStyle>

<HeaderStyle Width="1000px"></HeaderStyle>
</asp:BoundField>
</Columns>

<RowStyle CssClass="grdRowstyle"  ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle"  ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="#E7FFFF" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"  Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <BR /></asp:Panel></TD></TR>

<TR><TD align=center>
<asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Save" CssClass="field_button" Width="76px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnCancel" onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp; <asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="field_button" __designer:wfdid="w3"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

