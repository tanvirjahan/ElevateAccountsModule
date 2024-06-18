<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="FlightClassMasterSearch.aspx.vb" Inherits="FlightClassMasterSearch"   %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table>
        <tr>
            <td style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
                width: 100px; border-bottom: gray 2px solid; height: 15px">
    <table style="width: 927px; height: 21px">
        <tr>
            <td class="field_heading" colspan="1" style="text-align: center">
                Flight Class Master List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" colspan="1" style="color: blue;">
                Type few characters of code or name and click search</td>
        </tr>
        <tr>
            <td style="width: 673px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 800px; HEIGHT: 16px"><TBODY><TR><TD style="WIDTH: 158px" class="td_cell"><SPAN style="COLOR: black">Flight&nbsp;Class&nbsp;Code</SPAN></TD>
<TD><asp:TextBox id="TxtFlightClassCode" tabIndex=1 runat="server" Width="179px" 
        CssClass="field_input" MaxLength="20" __designer:wfdid="w23"></asp:TextBox></TD>
<TD style="WIDTH: 195px" class="td_cell"><SPAN style="COLOR: black">Flight&nbsp;Class&nbsp;Name</SPAN></TD><TD style="WIDTH: 188px" class="td_cell">
<asp:TextBox id="TxtFlightClassName" tabIndex=2 runat="server" Width="179px" 
        CssClass="field_input" __designer:wfdid="w35"></asp:TextBox></TD><TD style="WIDTH: 100px" class="td_cell">
<asp:Button id="btnSearch" tabIndex=3 runat="server" Text="Search" Font-Bold="False" 
            CssClass="search_button" __designer:wfdid="w36"></asp:Button>
        <asp:Button id="btnClear" tabIndex=4 runat="server" Text="Clear" 
            Font-Bold="False" CssClass="search_button" __designer:wfdid="w37"></asp:Button></TD><TD style="WIDTH: 56px" class="td_cell">
        <asp:Button id="cmdhelp" tabIndex=8 onclick="cmdhelp_Click" runat="server" 
            Text="Help" Font-Bold="False" CssClass="search_button" __designer:wfdid="w18"></asp:Button></TD><TD style="WIDTH: 90px" class="td_cell">
        <asp:Button id="btnAddNew" tabIndex=5 runat="server" Text="Add New" 
            Font-Bold="False" __designer:dtid="8725724278030351" CssClass="btn" 
            __designer:wfdid="w7"></asp:Button></TD><TD style="WIDTH: 90px" class="td_cell"><asp:Button id="btnPrint" tabIndex=7 runat="server" Text="Report" __designer:dtid="8725724278030353" CssClass="btn" __designer:wfdid="w8"></asp:Button></TD></TR><TR><TD style="WIDTH: 158px" class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption" __designer:wfdid="w37"></asp:Label></TD><TD><asp:DropDownList id="ddlOrderBy" runat="server" Width="110px" CssClass="drpdown" __designer:wfdid="w38" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD><TD style="WIDTH: 195px" class="td_cell"></TD><TD style="WIDTH: 188px" class="td_cell"></TD><TD style="WIDTH: 90px" class="td_cell"></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td style="width: 673px">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn"
                                Text="Export To Excel" TabIndex="6" />
                            </td>
        </tr>
        <tr>
            <td style="width: 673px">
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" Font-Size="10px" Width="924px" CssClass="grdstyle" __designer:wfdid="w39" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Flight Class Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("flightclscode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblflightclscode" runat="server" Text='<%# Bind("flightclscode") %>' __designer:wfdid="w45"></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="flightclscode" SortExpression="flightclscode" HeaderText="Flight Class Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="flightclsname" SortExpression="flightclsname" HeaderText="Flight Class Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
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

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="921px" __designer:wfdid="w31" Visible="False"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
    </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

