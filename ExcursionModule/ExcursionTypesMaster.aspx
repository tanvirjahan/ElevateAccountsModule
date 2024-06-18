<%@ Page Language="VB"   MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ExcursionTypesMaster.aspx.vb" Inherits="ExcursionTypesMaster" %>
<%@ Register Src="../PriceListModule/SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <%@ OutputCache location="none" %> 


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
            AutoCompleteExtenderKeyUp();


        });
</script>


<script language="javascript" type="text/javascript" >
    function load() {
        //    added by sribish
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(formmodecheck);
    }

    function formmodecheck() {
        var vartxtcode = document.getElementById("<%=txtCode.ClientID%>");
        if (vartxtcode.value == '') {
            doLinks(false);
        }
        else {
            doLinks(true);
        }
    }

    function doLinks(how) {
        for (var l = document.links, i = l.length - 1; i > -1; --i)
            if (!how)
                l[i].onclick = function () { alert('Please Save Main details to continue'); return false; };
            else
                l[i].onclick = function () { return; };
    }

    function checkNumber() {
        if (event.keyCode < 45 || event.keyCode > 57) {
            return false;
        }
    }
    function FormValidation(state) {
                if (state == "Edit") {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
        }

        if ((document.getElementById("<%=txtName.ClientID%>").value == "") || (document.getElementById("<%=ddlSupplierType.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlratebasis.ClientID%>").value == "[Select]") || (document.getElementById("<%=ddlentrytkt.ClientID%>").value == "[Select]")|| (document.getElementById("<%=ddltktbased.ClientID%>").value == "[Select]")  ||(document.getElementById("<%=ddlautoconf.ClientID%>").value == "[Select]")) {

         
            if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtclassificationcode.ClientID%>").value == "") {
                document.getElementById("<%=txtclassificationcode.ClientID%>").focus();
                alert("Select Supplier Type");
                return false;
            }

          else if (document.getElementById("<%=ddlratebasis.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlratebasis.ClientID%>").focus();
                alert("Select  Rate Basis ");
                return false;
            }

              
                
            else if (document.getElementById("<%=ddlentrytkt.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlentrytkt.ClientID%>").focus();
                alert("Please Select  EntryTicket Required or Not ");
                return false;
            }
                      else if (document.getElementById("<%=ddltktbased.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddltktbased.ClientID%>").focus();
                alert("Please Select Ticket Based on Time ");
                return false;
            }
                            else if (document.getElementById("<%=ddlautoconf.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlautoconf.ClientID%>").focus();
                alert("Please Select Auto Confirm");
                return false;
            }
            
                              else if (document.getElementById("<%=ddltransinc.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddltransinc.ClientID%>").focus();
                alert("Please Select Transfer Included");
                return false;
            }
            
                                else if (document.getElementById("<%=ddlsicpri.ClientID%>").value == "[Select]") {
                document.getElementById("<%=ddlsicpri.ClientID%>").focus();
                alert("Please Select SIC/PRIVATE");
                return false;
            }
        
//        else {
//            var val = document.getElementById("<%=txtCode.ClientID%>").value;

//            if (!val.match(/^[a-zA-Z0-9\-\/\_]+$/)) {
//                alert('Only alphabets,digits,-,_,/ are allowed');
//                return false
//            }


            return true;

            if (state == 'New') { if (confirm('Are you sure you want to save Excursion Type?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Excursion Type?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Excursion Type?') == false) return false; }
        }
    }

    function hotelautocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            document.getElementById('<%=txtclassificationcode.ClientID%>').value = eventArgs.get_value();

        }
        else {
            document.getElementById('<%=txtclassificationcode.ClientID%>').value = '';
        }

    }
    function excursiontype_autocompleteselected(source, eventArgs) {
        if (eventArgs != null) {
            var hiddenfieldID = source.get_id().replace("txtExcursionType_AutoCompleteExtender", "txtExcursionTypeCode");
       
            $get(hiddenfieldID).value = eventArgs.get_value();

        }
        else {
            var hiddenfieldID = source.get_id().replace("txtExcursionType_AutoCompleteExtender", "txtExcursionTypeCode");
            $get(hiddenfieldID).value = '';
        }

    }





    function AutoCompleteExtenderKeyUp() {


        $("#<%= txtclassificationname.ClientID %>").bind("change", function () {
            var hiddenfieldID1 = document.getElementById('<%=txtclassificationcode.ClientID%>');
            var hiddenfieldID = document.getElementById('<%=txtclassificationname.ClientID%>');
            if (hiddenfieldID.value == '') {
                hiddenfieldID1.value = '';
            }
        });

        $("#<%= txtclassificationname.ClientID %>").keyup("change", function () {
            var hiddenfieldID1 = document.getElementById('<%=txtclassificationcode.ClientID%>');
            var hiddenfieldID = document.getElementById('<%=txtclassificationname.ClientID%>');
            if (hiddenfieldID.value == '') {
                hiddenfieldID1.value = '';
            }
        });


    }
   

</script>


<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(EndRequestUserControl);
    function EndRequestUserControl(sender, args) {
        AutoCompleteExtenderKeyUp();

    }
</script>











    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY>
  
    <TR><TD vAlign=top align=center width=150 colSpan=2>
    <asp:Label id="lblHeading" runat="server" Text="Excursion Types" ForeColor="White" Width="800px" CssClass="field_heading"></asp:Label>
    </TD></TR>
    <TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD>
    <TD style="WIDTH: 85%;" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN>
        <asp:TextBox id="txtCode" ReadOnly ="true"    runat="server" Width="100px" Height="16px" ></asp:TextBox>  &nbsp; Name&nbsp; 

    <asp:TextBox id="txtName" tabIndex="1" runat="server"  
            CssClass="field_input" MaxLength="150" Width="370px" Height="16px" 
            TextMode="MultiLine" ></asp:TextBox>

 
        </TD>
 
</TR>
<TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%;" class="td_cell" vAlign=top align=left> 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD>
 </TR>

<TR>
   
    <td align="left" style="WIDTH: 15%" valign="top">
        &nbsp;<uc1:SubMenuUserControl ID="SubMenuUserControl1" runat="server" />
    </td>
    
 <td style="WIDTH: 85%" valign="top">
   <div ID="iframeINF" runat="server" style="WIDTH: 600px">
            <asp:Panel ID="PanelMain" runat="server" GroupingText="Main Details" 
                Width="600px">
                <table style="WIDTH: 414px; margin-right: 0px;">
                    <tbody>
                  
    
<%--<TD style="HEIGHT: 17px" class="td_cell" align=center colSpan=4><asp:Label id="lblHeading" runat="server" Text="Add New Supplier Category" ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label></TD></TR>--%>
<TR style="display:none">
    <TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">Rate Basis&nbsp;</TD>
<TD style="WIDTH: 202px; HEIGHT: 16px">
    <select ID="ddlSupplierType" runat="server" class="drpdown" 
        onchange="GetSpTypeValueFrom()" style="WIDTH: 128px" tabindex="3">
        <option selected=""></option>
    </select>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
<TD style="WIDTH: 200px; HEIGHT: 16px" class="td_cell">&nbsp;</TD>
</TR>

<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">Classification<span
style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
<asp:TextBox ID="txtclassificationname" runat="server" AutoPostBack="True" CssClass="field_input"
MaxLength="300" TabIndex="2" Width="140px"></asp:TextBox>
<asp:TextBox ID="txtclassificationcode" runat="server" style="display:none"></asp:TextBox>
<asp:HiddenField ID="hdnclassificationcode" runat="server" />
<asp:AutoCompleteExtender ID="txtclassificationname_AutoCompleteExtender" runat="server" CompletionInterval="10"
CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" DelimiterCharacters=""
EnableCaching="false" Enabled="True" FirstRowSelected="false" MinimumPrefixLength="-1"
ServiceMethod="Gethoteltypelist" TargetControlID="txtclassificationname" OnClientItemSelected="hotelautocompleteselected">
</asp:AutoCompleteExtender>
<input style="display: none" id="Text1" class="field_input" type="text" runat="server" />
<input style="display: none" id="Text2" class="field_input" type="text" runat="server" />
</td>
</tr>
<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell"> Rate Basis
    <span style="color: #ff0000">*</span>
</td>
<td align="left" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlratebasis" TabIndex="3" runat ="server" AutoPostBack ="true" width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>
<asp:ListItem Text ="Adult/Child/Senior" Value ="ACS">
</asp:ListItem>
<asp:ListItem Text ="Unit Rates" Value ="UNIT">
</asp:ListItem>
<asp:ListItem Text ="Half Day" Value ="HALF">
</asp:ListItem>
<asp:ListItem Text ="Full Day" Value ="FULL">
</asp:ListItem>
</asp:DropDownList>
</td>
</tr>

<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell"> Entry Ticket Required
    <span style="color: #ff0000">*</span>
</td>
<td align="left" TabIndex="4" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlentrytkt"  runat ="server" AutoPostBack ="true" width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="YES" Value ="YES">
</asp:ListItem>
<asp:ListItem Text ="NO" Value ="NO">
</asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<TR>

    <td class="td_cell" style="WIDTH: 1779px; HEIGHT: 16px">
        Tickets Based on Time<span style="color: #ff0000">*</span> </td>
    <td align="left" colspan="2" valign="top" width="300px">
        <asp:DropDownList ID="ddltktbased" runat="server" AutoPostBack="true" TabIndex="5" width="140px" >
        <asp:ListItem Text ="[Select]">
</asp:ListItem>
            <asp:ListItem Text="YES" Value="YES">
            </asp:ListItem>
            <asp:ListItem Text="NO" Value="NO">
            </asp:ListItem>
        </asp:DropDownList>
    </td>

<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">Auto Confirm<span 
        style="color: #ff0000">*</span> </td>
<td align="left" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlautoconf" TabIndex="6" runat ="server" AutoPostBack ="true" width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="YES" Value ="YES">
</asp:ListItem>
<asp:ListItem Text ="NO" Value ="NO">
</asp:ListItem>
</asp:DropDownList>
</td>
</tr>

<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">Star Category</td>
<td align="left" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlstarcat" TabIndex="7"  runat ="server" AutoPostBack ="true" 
        width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="2" Value ="2">
</asp:ListItem>
<asp:ListItem Text ="3" Value ="3">
</asp:ListItem>
<asp:ListItem Text ="4" Value ="4">
</asp:ListItem>
<asp:ListItem Text ="5" Value ="5">
</asp:ListItem>
</asp:DropDownList>
</td>
</tr>

<TR>

<TD style="WIDTH: 1779px; height: 22px;" class="td_cell">Meal Included</TD>
<TD style="WIDTH: 202px; height: 22px;">
    <INPUT id="chkmealinc" TabIndex="8"    type=checkbox CHECKED runat="server" />
    </TD>
<TD style="WIDTH: 147px; height: 22px;">
    </TD>
<TD style="WIDTH: 10px; height: 22px;"></TD>
</TR>
<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px;display:none" class="td_cell">Transfer Included<span 
        style="color: #ff0000">*</span> </td>
<td align="left" valign="top" colspan="2" width="300px" style="display:none">
 

<asp:DropDownList  ID="ddltransinc" TabIndex="9" runat ="server" AutoPostBack ="true" width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="YES" Value ="YES">
</asp:ListItem>
<asp:ListItem Text ="NO" Value ="NO">
</asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">SIC/Private<span 
        style="color: #ff0000">*</span> </td>
<td align="left" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlsicpri" TabIndex="10" runat ="server" AutoPostBack ="true" width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="SIC" Value ="SIC">
</asp:ListItem>
<asp:ListItem Text ="PRIVATE" Value ="PRIVATE">
</asp:ListItem>
<asp:ListItem Text ="WITHOUT TRANSFERS" Value ="WITHOUT TRANSFERS">
</asp:ListItem>

</asp:DropDownList>
</td>
</tr>
<tr>
<TD style="WIDTH: 1779px; HEIGHT: 16px" class="td_cell">Sector&nbsp;Wise&nbsp;Rates <span 
        style="color: #ff0000">*</span> </td>
<td align="left" valign="top" colspan="2" width="300px">
 

<asp:DropDownList  ID="ddlsectorwise" TabIndex="10" runat ="server"  width="140px" >
<asp:ListItem Text ="[Select]">
</asp:ListItem>

<asp:ListItem Text ="YES" Value ="YES">
</asp:ListItem>
<asp:ListItem Text ="NO" Value ="NO">
</asp:ListItem>


</asp:DropDownList>
</td>
</tr>
    <tr>
        <td style="width: 1779px; height: 16px" class="td_cell">
            Multiple&nbsp;Cost <span style="color: #ff0000">*</span>
        </td>
        <td align="left" valign="top" colspan="2" width="300px">
            <asp:DropDownList ID="ddlmultiplecost" TabIndex="10" runat="server" Width="140px">
                <asp:ListItem Text="[Select]">
                </asp:ListItem>
                <asp:ListItem Text="YES" Value="YES">
                </asp:ListItem>
                <asp:ListItem Text="NO" Value="NO">
                </asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
        <tr>
        <td style="width: 1779px; height: 22px" class="td_cell">
            Active
        </td>
        <td style="width: 202px; height: 22px">
            <input id="chkactive" tabindex="11" type="checkbox" checked runat="server" />
        </td>
        <td style="width: 147px; height: 22px">
        </td>
        <td style="width: 10px; height: 22px">
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 1779px; height: 22px">
            Multiple Dates<span style="color: #ff0000">*</span>
        </td>
        <td style="width: 202px; height: 22px">
            <asp:DropDownList ID="ddlMultipleDates" runat="server" AutoPostBack="True" 
                TabIndex="10" Width="140px">
                <asp:ListItem Text="[Select]">
                </asp:ListItem>
                <asp:ListItem Text="YES" Value="YES">
                </asp:ListItem>
                <asp:ListItem Text="NO" Value="NO">
                </asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 147px; height: 22px">
            &nbsp;</td>
        <td style="width: 10px; height: 22px">
            &nbsp;</td>
    </tr>
    <TR>

<TD style="WIDTH: 1779px; height: 22px;" class="td_cell">Preferred</TD>
<TD style="WIDTH: 202px; height: 22px;">
    <INPUT id="chkprefer" TabIndex="8"    type=checkbox CHECKED runat="server" />
    </TD>
<TD style="WIDTH: 147px; height: 22px;">
    </TD>
<TD style="WIDTH: 10px; height: 22px;"></TD>
</TR>
    <tr>
        <td style="width: 1779px; height: 16px;" class="td_cell">
            Combo <span style="color: #ff0000">*</span>
        </td>
        <td align="left" colspan="2" valign="top" width="300px">
            <asp:DropDownList ID="ddlCombo" TabIndex="10" runat="server" Width="140px" 
                AutoPostBack="True">
                <asp:ListItem Text="[Select]">
                </asp:ListItem>
                <asp:ListItem Text="YES" Value="YES">
                </asp:ListItem>
                <asp:ListItem Text="NO" Value="NO">
                </asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="HEIGHT: 22px" class="td_cell" colspan="4">
            &nbsp;
            <asp:GridView ID="gvCombo" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Excursion Type">
                        <ItemTemplate>
                            <asp:TextBox ID="txtExcursionType" runat="server" 
                                Text='<%# Bind("exctypname") %>' Width="300px"></asp:TextBox>
                            <asp:TextBox ID="txtExcursionTypeCode" runat="server" style="display:none;" 
                                Text='<%# Bind("exctypcode") %>'></asp:TextBox>
                            <asp:Label ID="lblId" runat="server" style="display:none;" 
                                Text='<%# Bind("exctypcombocode") %>'></asp:Label>
                            <asp:AutoCompleteExtender ID="txtExcursionType_AutoCompleteExtender" 
                                runat="server" CompletionInterval="10" 
                                CompletionListCssClass="autocomplete_completionListElement" 
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="1" 
                                DelimiterCharacters="" EnableCaching="false" Enabled="True" 
                                FirstRowSelected="false" MinimumPrefixLength="-1" 
                                OnClientItemSelected="excursiontype_autocompleteselected" 
                                ServiceMethod="GeExcursionType" TargetControlID="txtExcursionType">
                            </asp:AutoCompleteExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnAddrow" runat="server" CssClass="btn" tabIndex="12" 
                Text="Add Row" />
            &nbsp;<asp:Button ID="btnRowDelete" runat="server" CssClass="btn" tabIndex="12" 
                Text="Delete Selected Row" />
            <br />
        </td>
    </tr>

    <tr>
        <td style="WIDTH: 1779px; HEIGHT: 22px">
            <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="12" 
                Text="Save" />
            &nbsp; &nbsp; &nbsp;
            <asp:Button ID="btnhelp0" runat="server" __designer:wfdid="w14" CssClass="btn" 
                OnClick="btnhelp_Click" tabIndex="13" Text="Help" />
        </td>
        <td style="WIDTH: 202px; HEIGHT: 22px">
            <asp:Button ID="btnCancel" runat="server" CssClass="btn" tabIndex="14" 
                Text="Return To Search" />
        </td>
        <td style="WIDTH: 147px; HEIGHT: 22px">
            &nbsp;</td>
        <td style="WIDTH: 10px; HEIGHT: 22px">
        </td>
        </td>
        <script language="javascript">






           load();
           formmodecheck();
</script>
    </tr>

</tbody>
</table>
 </asp:Panel> 
</div>




 </td>

    </tbody>
                        </table> 
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
