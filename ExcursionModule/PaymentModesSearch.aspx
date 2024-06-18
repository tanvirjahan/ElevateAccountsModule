<%@ Page Title="" Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="PaymentModesSearch.aspx.vb" Inherits="ExcursionModule_PaymentModesSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >

    </script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
          border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading">
               Payment Modes List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell">
                <span style="color:blue">Type few characters of code or name and click search</span></td>
        </tr>
        <tr>
            <td>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE>
<TBODY>
<TR>
<TD class=" " align=center colSpan=4>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
<asp:Button id="BtnSearch" tabIndex=1  runat="server" Text="Search" Width="64px" CssClass="search_button"></asp:Button>&nbsp;
 <asp:Button id="BtnClear" tabIndex=2  runat="server" Text="Clear" Width="61px" CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnhelp" tabIndex=3  runat="server" Text="Help" Width="51px" CssClass="search_button"></asp:Button>&nbsp;
 <asp:Button id="BtnAddNew" tabIndex=4 runat="server" Text="Add New" Width="71px" CssClass="btn"></asp:Button> &nbsp;
 <asp:Button id="BtnPrint" tabIndex=5 runat="server" Text="Report"  Width="57px" CssClass="btn"></asp:Button></TD></TR>
 
 
 <TR>
 <TD style="WIDTH: 127px" class="td_cell">Payment &nbsp;Code</TD>
 <TD style="WIDTH: 128px; HEIGHT: 7px">
 <INPUT style="WIDTH: 194px" id="txtcode" tabIndex=6 type=text runat="server" class="txtbox" /></TD>
 <TD class="td_cell">Payment Name</TD><TD>
 <INPUT style="WIDTH: 294px" id="txtName" tabIndex=7 type=text class="txtbox" runat="server" />
 </TD>
 <td style="width=50px" class="td_cell">
 <asp:label ID="lblOrderBy" runat="server" Text="Order By" ForeColor ="Black" Width="50px" CssClass ="field_caption"></asp:label></td>
 <td style="width:137px" class="td_cell">
 <asp:DropDownList ID="ddlOrderBy" tabindex="8" runat="server" Width="130px" CssClass="drpdown" AutoPostBack ="true"></asp:DropDownList>
 </td>
 

 
 
 
 
 
 
 
 
 
 </TR>
 
 
 </TBODY>
 </TABLE>
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                &nbsp;<asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" Text="Export To Excel" TabIndex="8" />
                </td>
        </tr>
        <tr>
            <td  >
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle"  GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Department Code">
<EditItemTemplate>
<asp:TextBox id="txtmaingrpcode" runat="server" Text='<%# Bind("paycode") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("paycode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="paycode" SortExpression="paycode" HeaderText="Pay Code"></asp:BoundField>
<asp:BoundField DataField="payname" SortExpression="payname" HeaderText="Pay Name"></asp:BoundField>
<asp:BoundField DataField="Profreqd" SortExpression="Profreqd" HeaderText="Performa Required"></asp:BoundField>
<asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="333px" CssClass="lblmsg"  Visible="False"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>


