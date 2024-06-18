<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="UploadImagesForHomePg.aspx.vb" Inherits="WebAdminModule_UploadImagesForHomePg" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<table class="td_cell" style="width: 712px">
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Add Images for Home Page"
                    Width="850px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 190px">
            </td>
            <td style="width: 97px">
                <asp:TextBox ID="txtImageId" runat="server" Visible="False" CssClass="txtbox"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 190px" class="td_cell">
                Image Position</td>
            <td style="width: 97px">
                 <asp:DropDownList ID="ddlImgPostion"   width="100px"  runat ="server" >                             
                            <asp:ListItem Value ="L" Text ="Left"></asp:ListItem>
                             <asp:ListItem Value ="R" Text ="Right"></asp:ListItem>
                            </asp:DropDownList>
                
                </td>
        </tr>
        <tr>
            <td style="width: 88px">
                Upload Image</td>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" CssClass="field_input" TabIndex="17" Width="360px"  />
                <%--<asp:Button ID="btnUpload" runat="server" CssClass="btn" Text="Upload" />--%>
                <asp:Label ID="lblimgSize" runat ="server" Text="(Image Size: 288x209)"></asp:Label>
                <asp:TextBox ID="txtImage" runat="server" Width="352px" Visible="False" CssClass="txtbox"></asp:TextBox>&nbsp;
                </td>
        </tr>
         
        <tr>
            <td style="width: 88px">
                Rank Order</td>
            <td style="width: 97px">
                <asp:TextBox ID="txtRankOrder" runat="server" CssClass="txtbox"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 88px">
             <asp:CheckBox id="chkActive" Text ="Active" runat="server"> </asp:CheckBox>
            </td>
            <td style="width: 97px">
           

            
            </td>
        </tr>
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
                        TabIndex="8" Text="Help" /></td><%-- OnClick="btnHelp_Click"--%>
        </tr>
        <tr>
            <td style="width:88px;height:35px;">
                &nbsp;Image View:</td>
            <td style="width: 97px; height: 35px;">
                <asp:Image ID="ImgBanner" runat="server" />
                </td>
        </tr>
    </table>


</asp:Content>

