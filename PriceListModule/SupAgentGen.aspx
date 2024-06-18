<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupAgentGen.aspx.vb" Inherits="SupAgentGen" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier Agents" ForeColor="White" Width="900px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtSuppCode" class="field_input" disabled tabIndex=1 type=text maxLength=20 runat="server" /> 
    &nbsp;&nbsp;&nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtSuppName" class="field_input" disabled tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR><TR><TD vAlign=top align=left width=150>Type <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><SELECT onblur="GetValueFrom()" style="WIDTH: 201px" id="ddlType" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp; Name&nbsp; <SELECT onblur="GetValueCode()" style="WIDTH: 238px" id="ddlTName" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server" __designer:wfdid="w54"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD style="WIDTH: 80px" class="td_cell" colSpan=2><asp:Panel id="PanelGeneral" runat="server" Width="699px" GroupingText="General"><TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px" align=left>General Comments</TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><asp:TextBox id="txtGeneral" runat="server" Height="100px" Width="389px" CssClass="field_input" TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><TABLE style="WIDTH: 388px"><TBODY><TR><TD style="WIDTH: 100px">
    <asp:Button id="BtnGeneralSave" tabIndex=56 onclick="BtnGeneralSave_Click" 
        runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 437px"><asp:Button id="BtnGeneralCancel" tabIndex=57 onclick="BtnGeneralCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" tabIndex=58 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w26"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

