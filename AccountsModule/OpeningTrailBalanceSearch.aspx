<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="OpeningTrailBalanceSearch.aspx.vb" Inherits="OpeningTrailBalanceSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table>
            <tr>  <td align="center" class="field_heading" colspan="4">
                Opening Trail Balance</td>  </tr>
        <tr>
            <td>
                 <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD align=center>
    <asp:Button id="btnAddNew" 
        tabIndex=1 runat="server" Text="Add New" Font-Bold="True" 
        CssClass="btn" __designer:wfdid="w22"></asp:Button>&nbsp; 
    <asp:Button id="btnPrint" tabIndex=3 runat="server" Text="Report" 
        CssClass="btn" __designer:wfdid="w24"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=7 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="btn" __designer:wfdid="w25"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
                            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="2"
                    Text="Export To Excel" /></td>
        </tr>
        <tr>
            <td style="width:100%">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=4 runat="server"  Width="100%" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="3" GridLines="Vertical">
<Columns>
<asp:TemplateField Visible="False" HeaderText="Transaction Id"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("tran_id") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblTranID" runat="server" Text='<%# Bind("tran_id") %>' __designer:wfdid="w15"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField Visible="False" HeaderText="Tran Type"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("tran_type") %>' __designer:wfdid="w17"></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblTranType" runat="server" Text='<%# Bind("tran_type") %>' __designer:wfdid="w16"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="tran_type" Visible="False" HeaderText="Type"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("tran_type") %>' __designer:wfdid="w19"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblType" runat="server" Text='<%# Bind("type") %>' __designer:wfdid="w13"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tran_id" SortExpression="tran_id" HeaderText="Transaction ID"></asp:BoundField>
<asp:BoundField DataField="tran_type" SortExpression="tran_type" HeaderText="Transaction Type"></asp:BoundField>
<asp:BoundField DataFormatString="{00:dd/MM/yyyy} " DataField="tran_date" SortExpression="tran_date" HeaderText="Transaction Date"></asp:BoundField>
<asp:BoundField DataField="type" SortExpression="type" HeaderText="Type"></asp:BoundField>
<asp:BoundField DataField="code" SortExpression="code" HeaderText="Code"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency"></asp:BoundField>
<asp:BoundField DataField="currency_rate" SortExpression="currency_rate" HeaderText="Conversion Rate"></asp:BoundField>
<asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount"></asp:BoundField>
<asp:BoundField DataField="baseamount" SortExpression="baseamount" HeaderText="Base Amount"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
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

  <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>
 <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" 
                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" Width="357px" 
                            Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

