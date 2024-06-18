<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcSectorMaster.aspx.vb" Inherits="ExcSectorMaster" %>


<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
     <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

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



            function EndRequestUserControl(sender, args ) {


                CtryAutoCompleteExtenderKeyUp();
                // after update occur on UpdatePanel re-init the Autocomplete


                // after update occur on UpdatePanel re-init the Autocomplete

            }
            function CtryAutoCompleteExtenderKeyUp() {

            }



            function ctryautocompleteselected(source, eventArgs,city) {

                var hiddenfieldID = source.get_id().replace("txtsecgrpname_AutoCompleteExtender", "txtsecgrpcode" );
                $get(hiddenfieldID).value = eventArgs.get_value();
                var btn1 = source.get_id().replace("txtsecgrpname_AutoCompleteExtender", "btncityname");
                $get(btn1).click();

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


   <asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY>
  
    <TR><TD vAlign=top align=center width=150 colSpan=2>
    <asp:Label id="lblmainheading" runat="server" Text="Excursion Types" ForeColor="White" Width="800px" CssClass="field_heading"></asp:Label>
    </TD></TR>
        
        <TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; 
        <INPUT style="WIDTH: 274px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD>
        <TD style="WIDTH: 85%" vAlign=top>
   <%-- <DIV style="WIDTH: 682px" id="iframeINF" runat="server">--%>
<asp:Panel id="Panelctry" runat="server" Width="580px" GroupingText="SectorDetails">
<TABLE style="WIDTH: 100%" align=center>
<TBODY>

<TR>
     <TD style="HEIGHT: 18px" class="td_cell" align="center" colSpan=2>
     <asp:Label id="lblHeading" runat="server" Text="Excursion-Sector Details" 
             ForeColor="White" CssClass="field_heading" Width="582px" Height="16px"></asp:Label></TD></TR>
    <tr style="color: #ff0000">
        <td style="width: 145px; height: 27px" class="td_cell" colspan="2">
            <asp:GridView ID="grdsectorgrps" runat="server" AutoGenerateColumns="False" BackColor="White"
                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="td_cell"
                Font-Bold="true" Font-Size="12px" GridLines="Vertical" TabIndex="13" Width="298px">
                <FooterStyle CssClass="grdfooter" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" BackColor="#06788B" Width="2%" />
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chksectgrpsAll" Style="display: none" />
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="2%"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chksectgrps" Width="15px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sector Group">
                        <EditItemTemplate>
                            &nbsp;
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtsecgrpcode" runat="server" style="display:none" ></asp:TextBox>
                            <asp:TextBox ID="txtoldsecgrpcode" runat="server" style="display:none" ></asp:TextBox>
                            <asp:TextBox ID="txtsecgrpname" runat="server" CssClass="field_input" AutoPostBack="false"
                                Width="300px"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtsecgrpname_AutoCompleteExtender" runat="server"
                                CompletionInterval="10" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                CompletionSetCount="1" DelimiterCharacters="" EnableCaching="false" Enabled="True"
                                FirstRowSelected="false" MinimumPrefixLength="0" OnClientItemSelected="ctryautocompleteselected"
                                ServiceMethod="Getctrylist" TargetControlID="txtsecgrpname" UseContextKey="true">
                            </asp:AutoCompleteExtender>
                            <asp:Button ID="btncityname" runat="server" Text="Fill" OnClick="btncityname_Click"
                                Style="display: none" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City Code" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="txtcitycode" runat="server" Visible="False" CssClass="field_input"
                                Height="16px" Width="143px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtcityname" runat="server" ReadOnly="true" CssClass="field_input"
                                Height="16px" Width="143px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="grdRowstyle" />
                <SelectedRowStyle CssClass="grdselectrowstyle" />
                <PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center" />
                <HeaderStyle CssClass="grdheader" />
                <AlternatingRowStyle CssClass="grdAternaterow" />
            </asp:GridView>
            <asp:HiddenField ID="hdnMainGridRowid" runat="server" />
        </td>
    </tr>
                                            
                         
                          <TR><TD style="WIDTH: 58px" align=left>
        <asp:Button id="btnaddrow" tabIndex=38 runat="server" Text="Add Row" 
            CssClass="field_button" Width="93px"></asp:Button></TD>
            <TD style="WIDTH: 230px" align=left>
            <asp:Button id="btndelrow" tabIndex=39  runat="server" Text="Delete Row" CssClass="field_button">
            </asp:Button>&nbsp;
            </TD></TR>
                                     <%--  <td>
                                  
                                          
                                         </TD>--%></TR>
   <TR><TD style="WIDTH: 58px" align=left>
        <asp:Button id="BtnSave"   tabIndex=38 runat="server" Text="Save" 
            CssClass="field_button" Width="93px"></asp:Button></TD>
            <TD style="WIDTH: 230px" align=left>
            <asp:Button id="BtnCancel" tabIndex=39 onclick="BtnCancel_Click" runat="server" Text="Return to Search" CssClass="field_button">
            </asp:Button>&nbsp;
             <asp:Button id="btnHelp" tabIndex=40 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button">
             </asp:Button></TD></TR><tr><td colspan="4"><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr></TBODY></TABLE></asp:Panel> 
 <%--</DIV>--%></TD></TR></TBODY></TABLE>
  
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

