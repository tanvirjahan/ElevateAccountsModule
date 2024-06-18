<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UpdateLoginSignInTerms.aspx.vb" Inherits="UpdateLoginSignInTerms" MasterPageFile="~/WebAdminMaster.master" Strict="true" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<asp:Content ContentPlaceHolderID="Main" runat="server" >
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 750px"><TBODY><TR><TD style="FONT-WEIGHT: bold; FONT-SIZE: 12px; FONT-FAMILY: Verdana" class="field_heading">Update Login Sign In Terms :</TD></TR><TR><TD>

<HTMLEditor:Editor ID="FCKeditor1" runat="server" 
        Height="450" 
        Width="900"
        AutoFocus="true"
/>
</TD></TR><TR><TD style="HEIGHT: 26px; TEXT-ALIGN: left"><asp:Button id="btnSave" runat="server" Text="Save" CssClass="btn"></asp:Button>&nbsp; <asp:Button id="btnCancel" runat="server" Text="Cancel" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" Width="45px" CssClass="btn" __designer:wfdid="w1"></asp:Button>
&nbsp; <asp:TextBox id="txtWebDetailId" runat="server" Width="80px" Visible="False"></asp:TextBox></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>



</asp:Content>