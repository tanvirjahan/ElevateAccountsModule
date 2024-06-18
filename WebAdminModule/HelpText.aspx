<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HelpText.aspx.vb" Inherits="WebAdminModule_HelpText" MasterPageFile="~/WebAdminMaster.master"  Strict="true"  ValidateRequest="false" EnableEventValidation="false"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<asp:Content ContentPlaceHolderID="Main" runat="server" >
 <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
    <table class="td_cell"><TBODY>
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text=" Add Help Text"
                    Width="944px"></asp:Label></td>
        </tr>
        <tr>
            <td>
                Help Id</td>
            <td>
                <asp:TextBox ID="txtHelpId" runat="server" MaxLength="100" Width="552px" CssClass="txtbox"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                Help Text</td>
            <td>
            <HTMLEditor:Editor ID="FCKeditor2" runat="server" 
        Height="300px" 
        Width="100%"
        AutoFocus="true"
/>
                <table  id="tblHelpText" runat="server" >
                    <tr>
                        <td id="TD1"  runat="server">
                            </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save" /></td>
            <td>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" Text="Return To Search"
                    Width="112px" /></td>
        </tr></TBODY>
    </table>

    </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
