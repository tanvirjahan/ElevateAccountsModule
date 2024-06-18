<%@ Page Language="VB"  MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="UploadPopularDeals.aspx.vb" Inherits="UploadPopularDeals" %>
 

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
 
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
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
                AutoCompleteExtenderKeyUp();


            });


    </script>
  
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestUserControl);
        function EndRequestUserControl(sender, args) {
            OthGrpAutoCompleteExtenderKeyUp();
            PrefSuppAutoCompleteExtenderKeyUp();

        }
    </script>

  

    
    <script language="javascript" type="text/javascript">

        function PopUpImageView(code) {

            var FileName = document.getElementById("<%=hdnFileName.ClientID%>");
            var lblfilename = document.getElementById("<%=txtimg.ClientID%>");
             
       
       
            if (FileName.value == "") {
                FileName.value = code;
            }

            if (lblfilename != "") {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?code=' + FileName.value + " &pagename=" + "UploadPopularDeals.aspx", 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
                FileName.value = "";
                return false
                
            }
            else {

                popWin = open('../PriceListModule/ImageViewWindow.aspx?', 'myWin', 'resizable=yes,scrollbars=yes,width=500,height=350,left=450,top=80');
                popWin.focus();
            }
        }

        function PrefSuppautocompleteselected(source, eventArgs) {
            if (eventArgs != null) {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = eventArgs.get_value();
            }
            else {
                document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
            }

        }

        function PrefSuppAutoCompleteExtenderKeyUp() {
            $("#<%= txtPrefSupName.ClientID %>").bind("change", function () {

                if (document.getElementById('<%=txtPrefSupName.ClientID%>').value == '') {

                    document.getElementById('<%=txtPrefSupCode.ClientID%>').value = '';
                }

            });

        }

        function FormValidation(state) {


            if ((document.getElementById("<%=txtPrefSupCode.ClientID%>").value == "") || (document.getElementById("<%=txtPrefSupName.ClientID%>").value == "")) {
                if (document.getElementById("<%= txtPrefSupCode .ClientID%>").value == "") {
                    document.getElementById("<%= txtPrefSupCode .ClientID%>").focus();
                    alert("Select Supplier!! ");
                    return false;

                }
            }

            else {
                //alert(state);
                if (state == 'New') { if (confirm('Are you sure you want to save Deal?') == false) return false; }
                if (state == 'Edit') { if (confirm('Are you sure you want to update Deal ?') == false) return false; }
                if (state == 'Delete') { if (confirm('Are you sure you want to delete Deal ?') == false) return false; }
            }
        }


        function checkNumber(e) {

            if ((event.keyCode < 47 || event.keyCode > 57)) {
                return false;
            }

        }
        function checkCharacter(e) {
            if (event.keyCode == 32 || event.keyCode == 46)
                return;
            if ((event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122)) {
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
     

			
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 714px; border-bottom: gray 2px solid; text-align: left">
                <tbody>
                    <tr>
                        <td style="height: 3px; text-align: center" class="td_cell" colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Add New Popular Deals" ForeColor="White"
                                __designer:wfdid="w17" Width="100%" CssClass="field_heading"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_cell" style ="height :26px">
                           <asp:Label ID="lblcode" Text="Code" runat ="server"></asp:Label>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td colspan="3" style="height: 26px">
                            <input id="txtCode" class="field_input" style="width: 182px" tabindex="1" type="text"
                                maxlength="20" runat="server" />
                        </td>
                    </tr>
                    </TR>
                    <tr>
           
                        <td class="td_cell" style=" height: 30px; ">
                            <asp:Label ID="lblprefsupp" runat="server" Text=" Supplier"></asp:Label>
                            <span style="color: #ff0000">*</span>
                        </td>
                        <td align="left" colspan="3" valign="top">
                            <asp:TextBox ID="txtPrefSupName" runat="server" CssClass="field_input" 
                                MaxLength="500" TabIndex="4" Width="548px"></asp:TextBox>
                            <asp:TextBox ID="txtPrefSupCode" runat="server" Style="display: none"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtPrefSupName_AutoCompleteExtender" 
                                runat="server" CompletionInterval="10" 
                                CompletionListCssClass="autocomplete_completionListElement" 
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                FirstRowSelected="false" MinimumPrefixLength="0" 
                                OnClientItemSelected="PrefSuppautocompleteselected" 
                                ServiceMethod="GetPreferSupplist" TargetControlID="txtPrefSupName">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
  
                        <td class="td_cell" 
                            style=" text-align :left;vertical-align:top; height: 24px; ">
                            Hotel Image</td>
                        <td colspan="3">
                            <asp:FileUpload ID="hotelimage" runat="server" TabIndex="4" />
(1920*500) &nbsp; <asp:Button ID="Upload" runat="server"  Visible ="false" Text="Upload"/></td>
                    </tr>
                    <tr>

                    <td class="td_cell" style=" text-align :left;vertical-align:top;">
                        &nbsp;</td>
                    <td align="left" colspan="3" style="height: 22px" valign="top">
                        <input style="width: 375px" id="txtimg"  readonly ="true"
    TabIndex="22"  type="text" maxlength="30" runat="server" />
                        <asp:Button ID="btnViewimage" runat="server" CssClass="field_button" 
                            TabIndex="5" Text="View" Width="64px" />
                        &nbsp;<asp:Button ID="Btnremove" runat="server" CssClass="field_button" TabIndex="6" 
                            Text="Remove" Width="77px" />
                    </td>
                    </tr>

                    <tr>
                        <td class="td_cell" style=" text-align :left;vertical-align:top; height: 24px;">
                            Remarks</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtRemark" runat="server" __designer:wfdid="w20" 
                                CssClass="field_input" Height="116px" TabIndex="7" TextMode="MultiLine" 
                                Width="548px"></asp:TextBox>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td class="td_cell" style="height: 24px; " >
                            &nbsp;Active &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <input id="ChkInactive" tabindex="8" type="checkbox" checked runat="server" />
                        </td>
                        <td class="td_cell" style="float:right">
                            &nbsp;  </td>
                        <td class="td_cell">
                            &nbsp;</td>
                        
                    </tr>
                    <tr>
                        <td >
                            &nbsp;</td>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
    
                    </tr>
                    <tr>
                        <td >
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        
    </asp:UpdatePanel>
        <asp:UpdatePanel>
        <ContentTemplate>
        <table>
        
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                                 TabIndex="9" Text="Save" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td  >
                            <asp:Button ID="btnCancel" runat="server" CssClass="field_button" 
                                OnClick="btnCancel_Click" TabIndex="10" Text="Return To Search" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w8" 
                                CssClass="field_button" OnClick="btnhelp_Click" TabIndex="11" 
                                Text="Help" />
                        </td>
                    </tr>
                    </table>
                    </ContentTemplate>
                      <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
                    </asp:UpdatePanel>
      <asp:TextBox ID="hdnFileName" Text="" runat="server" Style="display: none" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/clsServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
