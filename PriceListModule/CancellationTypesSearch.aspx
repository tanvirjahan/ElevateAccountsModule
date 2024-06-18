<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="CancellationTypesSearch.aspx.vb" Inherits="CancellationTypesSearch"    EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table style=" border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;">
        <tr>
            <td class="field_heading" style="text-align: center">
                Cancellation &nbsp;List</td>
        </tr>
        <tr>
            <td class="td_cell" style="text-align: center; color: blue; ">
                Type few characters of code or name and click search &nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td  style="width:100%">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD class="td_cell"></TD><TD></TD><TD class="td_cell" colSpan=2>
    <asp:Button id="btnSearch" tabIndex=3 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button" __designer:wfdid="w62"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=4 runat="server" Text="Clear" 
        Font-Bold="False" CssClass="search_button" __designer:wfdid="w63"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
        Text="Help" Font-Bold="False" CssClass="search_button" __designer:wfdid="w23"></asp:Button>&nbsp; 
    <asp:Button id="btnAddNew" tabIndex=5 runat="server" Text="Add New" 
        Font-Bold="False" __designer:dtid="14355223812243468" CssClass="btn" 
        __designer:wfdid="w8"></asp:Button>&nbsp; <asp:Button id="btnPrint" tabIndex=7 runat="server" Text="Report" __designer:dtid="14355223812243470" CssClass="btn" __designer:wfdid="w9"></asp:Button></TD></TR><TR><TD class="td_cell">Cancellation Type Code</TD><TD><asp:TextBox id="txtCancellationCode" tabIndex=1 runat="server" Width="152px" CssClass="txtbox" MaxLength="20" __designer:wfdid="w64"></asp:TextBox></TD>
        <TD style="WIDTH: 122px" class="td_cell">Cancellation&nbsp;Type&nbsp;Name</TD><TD style="WIDTH: 421px"><asp:TextBox id="txtCancellationName" tabIndex=2 runat="server" Width="258px" CssClass="txtbox" MaxLength="100" __designer:wfdid="w65"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 165px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="160px" CssClass="drpdown" __designer:wfdid="w2" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD style="WIDTH: 122px" class="td_cell"></TD><TD style="WIDTH: 421px"></TD></TR></TBODY></TABLE>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td style="100%">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                    Text="Export To Excel" TabIndex="6" />
                </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" Font-Size="10px" BackColor="White" Width="100%" CssClass="grdstyle" __designer:wfdid="w50" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle  CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Cancellation Type Code"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("ctypecode") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("ctypecode") %>' __designer:wfdid="w1"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="ctypecode" SortExpression="ctypecode" HeaderText="Cancellation Type Code"></asp:BoundField>
<asp:BoundField DataField="ctypename" SortExpression="ctypename" HeaderText="Cancellation Type Name"></asp:BoundField>
<asp:BoundField DataField="IsRegred" SortExpression="IsRegred" HeaderText="Regret"></asp:BoundField>
<asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" HeaderText="User Modified"></asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle  CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" __designer:dtid="844424930131989" Width="357px" __designer:wfdid="w51" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>

