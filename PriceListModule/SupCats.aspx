<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupCats.aspx.vb" Inherits="SupCats"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript" >

    function CallWebMethod(methodType) 
    {
        switch (methodType) 
        {
            case "partycode":
                var select = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select = document.getElementById("<%=ddlSuppierNM.ClientID%>");
                var selectname = document.getElementById("<%=ddlSuppierCD.ClientID%>");
                selectname.value = select.options[select.selectedIndex].text;
                break;
        }
    }


</script> 

 <style>
 .bgrow
 {
 background-color:white; 

 }
 </style>  
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE  align="left">
<TBODY>
                <tr>
                <td colspan ="20" align ="left" class="bgrow" >
               
                    <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
                </td>
                </tr>
                <tr>
                <td>
                <div style="margin-top:-6px;margin-left:13px;">
                <table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell" align=left>
<TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier " Width="100%" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" disabled tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" disabled tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR>
    <tr>
        <td align="left" valign="top" width="150">
            &nbsp;</td>
        <td align="left" class="td_cell" valign="top">
            <table style="width: 100%">
                <tr>
                    <td>
                        Supplier</td>
                    <td>
                        <select ID="ddlSuppierCD" runat="server" class="field_input" name="D1" 
                            onchange="CallWebMethod('partycode');" style="WIDTH: 220px">
                            <option selected=""></option>
                        </select></td>
                    <td>
                        <select ID="ddlSuppierNM" runat="server" class="field_input" name="D2" 
                            onchange="CallWebMethod('partyname');" style="WIDTH: 310px">
                            <option selected=""></option>
                        </select></td>
                    <td>
                        <asp:Button ID="btnfilldetail" runat="server" CssClass="field_button" 
                            Text="Fill Details" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 100%" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD style="WIDTH: 80px" class="td_cell" colSpan=2>&nbsp;<asp:Panel id="PanelCategories" runat="server" Width="743px" __designer:wfdid="w1" GroupingText="Room Occupancies"><TABLE><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 4px"></TD></TR><TR><TD style="WIDTH: 100px"><asp:GridView id="Gv_Categories" tabIndex=73 runat="server" Width="607px" __designer:wfdid="w2" AutoGenerateColumns="False"><Columns>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center"></ItemStyle>
<ItemTemplate>
<INPUT id="ChkSelect" type=checkbox runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="rmcatname" HeaderText="Category Name"></asp:BoundField>
<asp:BoundField DataField="rmcatcode" HeaderText="Category Code"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><asp:Button id="BtnSelectAllCategories" tabIndex=74 onclick="BtnSelectAllCategories_Click" runat="server" Text="Select All" CssClass="field_button" __designer:wfdid="w3"></asp:Button>&nbsp;<asp:Button id="BtnDeSelectAllCategories" tabIndex=75 onclick="BtnDeSelectAllCategories_Click" runat="server" Text="DeSelect All" CssClass="field_button" __designer:wfdid="w4"></asp:Button>
        <INPUT style="WIDTH: 12px" id="txtaccom_extra" class="field_input" disabled 
                tabIndex=1 type=text maxLength=20 runat="server" visible="False" />
        &nbsp;
        </TD></TR><TR><TD style="WIDTH: 100px"><TABLE style="HEIGHT: 22px"><TBODY><TR>
        <TD style="height: 10px;">
            &nbsp;<asp:Button ID="btnAddNew" runat="server" CssClass="field_button" 
                Font-Bold="True" onclick="btnAddNew_Click" tabIndex="5" 
                Text="Add New in Master" />
        </TD><TD style="HEIGHT: 10px"><asp:Button id="BtnSaveCategory" 
                tabIndex=76 onclick="BtnSaveCategory_Click" runat="server" Text="Save" 
                CssClass="field_button" __designer:wfdid="w5"></asp:Button></TD>
        <TD style="HEIGHT: 10px">
            <asp:Button id="BtnCancelCategory" tabIndex=77 
                onclick="BtnCancelCategory_Click" runat="server" Text="Return to Search" 
                CssClass="field_button" __designer:wfdid="w6"></asp:Button></TD>
        <td style="HEIGHT: 10px">
            <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w1" 
                CssClass="field_button" onclick="btnhelp_Click" tabIndex="88" Text="Help" />
        </td>
        </TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR>
     </table>
                </div>
                </td>
                </tr>
</TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

