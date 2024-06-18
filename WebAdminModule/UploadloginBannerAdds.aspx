<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadLoginBannerAdds.aspx.vb" Inherits="UploadLoginBannerAdds" MasterPageFile="~/SubPageMaster.master" Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %> 
<asp:Content ContentPlaceHolderID ="Main" runat="server" >
    <table class="td_cell" style="width: 712px">
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Add New Banner Adds"
                    Width="850px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 88px">
            </td>
            <td style="width: 97px">
                <asp:TextBox ID="txtBannerId" runat="server" Visible="False" CssClass="txtbox"></asp:TextBox></td>
        </tr>
        <!--
        <tr>
            <td style="width: 120px" class="td_cell">
                Alternate&nbsp;Text</td>
            <td style="width: 97px">
                <asp:TextBox ID="txtAlternateText" runat="server" CssClass="txtbox"></asp:TextBox></td>
        </tr>-->
        <tr>
            <td style="width: 88px">
                Upload Image</td>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" CssClass="field_input" TabIndex="17" Width="360px"  />(1350 X 748)
                <asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload" />
                <asp:TextBox ID="txtImage" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>&nbsp;
                
                </td>
        </tr>
        <tr>
                        <td  style="HEIGHT: 4px">
                           
                            <INPUT id="ChkInactive" tabIndex=9 type=checkbox CHECKED runat="server" />
                            Active<td  style="WIDTH: 193px; HEIGHT: 4px">
                         
                    </tr>
        
        <!--
        <tr>
            <td style="width: 88px">
                Upload Flash</td>
            <td>
                <asp:FileUpload ID="fuflash" runat="server" CssClass="field_input" TabIndex="17" Width="360px"  />
                <asp:Button ID="btnUpload1" runat="server" CssClass="btn" Text="Upload" /><br />
                <asp:TextBox ID="txtflash" runat="server" CssClass="txtbox" Visible="False"
                    Width="352px"></asp:TextBox></td>
        </tr>
        
        <tr>
            <td style="width: 88px">
                Navigate Url</td>
            <td style="width: 97px">
                <asp:TextBox ID="txtNavigateUrl" runat="server" CssClass="txtbox"></asp:TextBox></td>
        </tr>
        -->
        <tr>
            <td style="width: 88px">
            </td>
            <td style="width: 97px">
            </td>
        </tr>
        <tr>
            <td style="width: 88px">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" TabIndex="6" Text="Save" /></td>
            <td style="width: 250px">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" TabIndex="7" 
                    Text="Return to Search" />&nbsp;<asp:Button ID="btnHelp" runat="server" CssClass="btn"
                        OnClick="btnHelp_Click" TabIndex="8" Text="Help" /></td>
        </tr>
        <tr>
            <td style="width: 88px; height: 35px;">
                &nbsp;Image View:</td>
            <td style="width: 97px; height: 35px;">
                <asp:Image ID="ImgBanner" runat="server" />
                </td>
        </tr>
    </table>

</asp:Content>
