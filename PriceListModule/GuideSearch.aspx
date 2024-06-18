<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GuideSearch.aspx.vb" Inherits="GuideSearch" MasterPageFile="~/MainPageMaster.master" Strict="true" enableEventValidation ="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td>
                <table>
                    <tr align=center>
                        <td style="width: 708px; height: 6px; text-align:center;" align="center" class="field_heading">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Guides List</td>
                    </tr>
                    <tr>
                        <td style="width: 708px; color: blue; text-align:center;" class="td_cell" align="center">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Type few characters of code or name and click search &nbsp; &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 708px;">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                <contenttemplate>
<TABLE style="WIDTH: 818px"><TBODY><TR><TD style="WIDTH: 85px; HEIGHT: 24px" 
        class="td_cell"><SPAN style="COLOR: black">Guide Code</SPAN></TD><TD style="WIDTH: 46px; HEIGHT: 24px"><asp:TextBox id="txtGuideCode" tabIndex=1 runat="server" CssClass="txtbox" MaxLength="20"></asp:TextBox></TD>
    <TD style="WIDTH: 82px; HEIGHT: 24px" class="td_cell"><SPAN style="COLOR: black">Guide Name</SPAN></TD><TD style="WIDTH: 30px; HEIGHT: 24px" class="td_cell"><asp:TextBox id="txtGuideName" tabIndex=2 runat="server" Width="118px" CssClass="txtbox" MaxLength="100"></asp:TextBox></TD><TD style="WIDTH: 42px; HEIGHT: 24px" class="td_cell">
    <asp:Button id="btnSearch" tabIndex=3 onclick="btnSearch_Click" runat="server" 
        Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 42px; HEIGHT: 24px" class="td_cell">
        <asp:Button id="btnClear" tabIndex=4 onclick="btnClear_Click" runat="server" 
            Text="Clear" Font-Bold="False" CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 42px; HEIGHT: 24px" class="td_cell">
        <asp:Button id="btnhelp" tabIndex=4 onclick="btnClear_Click" runat="server" 
            Text="Help" Font-Bold="False" CssClass="search_button" Width="38px"></asp:Button></TD>
    <td class="td_cell" style="width: 42px; height: 24px">
                <asp:Button ID="btnAddNew" runat="server" CssClass="btn" Font-Bold="False"
                    OnClick="btnAddNew_Click" Text="Add New" TabIndex="5" 
                    style="position: relative; top: 0px; left: 0px;" /></td>
    <td class="td_cell" style="width: 42px; height: 24px">
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Report" TabIndex="7" style="position: relative" /></td>
</TR><TR><TD style="WIDTH: 85px" class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD><TD style="WIDTH: 46px; HEIGHT: 24px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="128px" CssClass="drpdown" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD>
        <TD style="WIDTH: 82px; HEIGHT: 24px" class="td_cell"></TD><TD style="WIDTH: 30px; HEIGHT: 24px" class="td_cell"></TD><TD style="WIDTH: 42px; HEIGHT: 24px" class="td_cell"></TD><TD style="WIDTH: 42px; HEIGHT: 24px" class="td_cell"></TD></TR></TBODY></TABLE>
</contenttemplate>
                            </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="width: 708px;">
                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="6" />
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 708px; height: 6px">
               <asp:UpdatePanel id="UpdatePanel2" runat="server">
                <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" Font-Size="10px" Width="950px" CssClass="grdstyle" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Guide Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("guidecode") %>'></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("guidecode") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="guidecode" SortExpression="guidecode" HeaderText="Guide Code"></asp:BoundField>
<asp:BoundField DataField="guidename" SortExpression="guidename" HeaderText="Guide Name"></asp:BoundField>
<asp:BoundField DataField="tel" SortExpression="tel" HeaderText="Telephone"></asp:BoundField>
<asp:BoundField DataField="mobile" SortExpression="mobile" HeaderText="Mobile"></asp:BoundField>


<asp:BoundField DataField="IsActive" SortExpression="IsActive" HeaderText="Active"></asp:BoundField>

<asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="Records not found, Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


</asp:Content>