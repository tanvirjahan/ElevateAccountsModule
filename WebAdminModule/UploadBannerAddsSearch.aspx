<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadBannerAddsSearch.aspx.vb" Inherits="UploadBannerAddsSearch"  MasterPageFile="~/WebAdminMaster.master" Strict="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <table style="width: 100%">
        <tr>
            <td class="field_heading" style="text-align: center">
                Upload Banner Ads List</td>
        </tr>
        <tr>
            <td style="width: 100px">
                <table class="td_cell" style="width: 936px">
                    <tr>
                        <td style="width: 67px">
                            Banner Id</td>
                        <td style="width: 55px">
                            <asp:TextBox ID="txtBannerId" runat="server" CssClass="txtbox"></asp:TextBox></td>
                        <td style="width: 100px">
                            Alternate Text</td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtAlternateText" runat="server" Width="304px" CssClass="txtbox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Navigate Url</td>
                        <td style="width: 55px">
                            <asp:TextBox ID="txtNavigateUrl" runat="server" Width="344px" CssClass="txtbox"></asp:TextBox></td>
                        <td style="width: 100px">
                            Image Name</td>
                        <td style="width: 100px">
                            <asp:TextBox ID="txtImageName" runat="server" Width="304px" CssClass="txtbox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 67px">
                        </td>
                        <td style="width: 55px">
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 250px">
                            <asp:Button ID="btnSearch" runat="server" CssClass="search_button" Font-Bold="False"
                                TabIndex="3" Text="Search" Width="48px" />&nbsp;<asp:Button ID="btnClear" runat="server"
                                    CssClass="search_button" Font-Bold="False" TabIndex="4" Text="Clear" Width="39px" />&nbsp;<asp:Button
                                        ID="btnHelp" runat="server" CssClass="search_button" OnClick="btnHelp_Click"
                                        TabIndex="8" Text="Help" Width="35px" />&nbsp;
                <asp:Button ID="btnAddNew" runat="server" CssClass="btn" Font-Bold="True"
                    TabIndex="5" Text="Add New" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="btnExportToExcel"
                        runat="server" CssClass="btn" TabIndex="6" Text="Export To Excel" 
                    Visible="False" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="btn" TabIndex="7"
                            Text="Report" Visible="False" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" Font-Size="10px" CssClass="grdstyle" Width="100%" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField SortExpression="bannerid" Visible="False" HeaderText="Banner Id"><ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("bannerid") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="bannerid" SortExpression="bannerid" HeaderText="Banner Id"></asp:BoundField>
<asp:BoundField DataField="AlternateText" SortExpression="AlternateText" HeaderText="Alternate Text
"></asp:BoundField>
<asp:BoundField DataField="ImageUrl" SortExpression="ImageUrl" HeaderText="Image Url"></asp:BoundField>
<asp:BoundField DataField="NavigateUrl" SortExpression="NavigateUrl" HeaderText="Navigate Url"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="EditRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="DeleteRow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" HorizontalAlign ="Left"  ForeColor="White" ></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>

</asp:Content>