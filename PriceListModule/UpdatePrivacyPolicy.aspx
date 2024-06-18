<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UpdatePrivacyPolicy.aspx.vb" Inherits="UpdatePrivacyPolicy" MasterPageFile="~/PriceListMaster.master" Strict="false" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<asp:Content ContentPlaceHolderID="Main" runat="server">

<TABLE style="WIDTH: 750px"><TBODY><TR><TD style="FONT-WEIGHT: bold; FONT-SIZE: 12px; FONT-FAMILY: Verdana" class="field_heading">Update Privacy Policy :</TD></TR><TR><TD>


<HTMLEditor:Editor ID="Editor1" runat="server" 
        Height="300px" 
        Width="100%"
        AutoFocus="true"
/>

</TD></TR><TR><TD style="TEXT-ALIGN: left"><asp:Button id="btnSave" runat="server" Text="Save" __designer:wfdid="w40" CssClass="btn"></asp:Button>&nbsp; <asp:Button id="btnCancel" runat="server" Text="Cancel" __designer:wfdid="w41" CssClass="btn"></asp:Button>&nbsp;
                <asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" __designer:wfdid="w3" CssClass="btn" Width="45px"></asp:Button> 
                &nbsp;<asp:TextBox id="txtWebDetailId" runat="server" __designer:wfdid="w42" Width="80px" Visible="False"></asp:TextBox></TD></TR></TBODY></TABLE>

            <asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <contenttemplate>    </contenttemplate>
    </asp:UpdatePanel>

</asp:Content>