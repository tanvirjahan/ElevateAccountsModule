<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false"
    CodeFile="RelatedDocuments.aspx.vb" Inherits="AccountsModule_RelatedDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                border-bottom: gray 2px solid; width: 100%;">
                <tbody>
                    <tr>
                        <td class="field_heading" align="center" colspan="1" style="height: 27px">
                            <asp:Label ID="lblHeading" runat="server" Text="Related Documents" Width="100%"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                    <table style="height: 108px">
                    <tr>
                    <td class="td_cell">
                    <asp:Label ID="lblUpload" runat="server" Text="Upload File (PDF only)" Width="170px" 
                                                            CssClass="field_Caption" ></asp:Label>                    
                    </td>
                    <td>
                    <asp:FileUpload ID="uplFile" runat="server"  />
                    </td>
                    
                    <td> <asp:Button runat="server" ID="btnAdd"  Text="Add" CssClass="field_button"/> </td>
                    </tr>
                    <tr>
                     <asp:GridView ID="gv_Docs" TabIndex="18" runat="server" Font-Size="10px"
                                                BackColor="White" Width="100%" CssClass="td_cell" GridLines="Vertical" CellPadding="3"
                                                BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False"
                                                AllowSorting="True" AllowPaging="True">
                                                <FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
                                                </asp:GridView>
                    </tr>
                    </table>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
