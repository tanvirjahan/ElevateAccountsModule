<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HelpTextSearch.aspx.vb" Inherits="WebAdminModule_HelpTextSearch"  MasterPageFile ="~/WebAdminMaster.master" Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content  ContentPlaceHolderID ="Main" runat="server" >
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 936px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading">Help Text&nbsp;List</TD></TR><TR><TD style="WIDTH: 100px; height: 49px;"><TABLE style="WIDTH: 936px" class="td_cell"><TBODY><TR><TD style="WIDTH: 67px">Help Id</TD><TD><asp:TextBox id="txtHelpId" runat="server" MaxLength="100" Width="568px" CssClass="txtbox"></asp:TextBox> <asp:Button id="btnSearch" tabIndex=3 runat="server" Text="Search" Font-Bold="False" Width="48px" CssClass="search_button"></asp:Button>&nbsp;
 <asp:Button id="btnClear" tabIndex=4 runat="server" Text="Clear" Font-Bold="False" Width="39px" CssClass="search_button"></asp:Button></TD></TR></TBODY></TABLE></TD></TR><TR><TD>
        <asp:Button id="btnAddNew" tabIndex=5 runat="server" Text="Add New" 
            Font-Bold="True" CssClass="btn"></asp:Button>&nbsp;<asp:Button 
            id="btnExportToExcel" tabIndex=6 runat="server" Text="Export To Excel" 
            CssClass="btn" Visible="False"></asp:Button>&nbsp;<asp:Button id="btnPrint" tabIndex=7 runat="server" Text="Print" CssClass="btn" Visible="False"></asp:Button></TD></TR><TR><TD style="WIDTH: 100px"><asp:UpdatePanel id="UpdatePanel2" runat="server"><ContentTemplate>
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" Font-Size="10px" Width="950px" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Help ID"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("help_id") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("help_id") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="help_id" SortExpression="help_id" HeaderText="Help Id"></asp:BoundField>
<asp:BoundField DataField="adddate" SortExpression="adddate" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="moddate" SortExpression="moddate" HeaderText="User Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="Date Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="RowEdit">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="RowView">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="RowDelete">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="White" ></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
            CssClass="lblmsg"></asp:Label> 
</ContentTemplate>
</asp:UpdatePanel></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>


</asp:Content>