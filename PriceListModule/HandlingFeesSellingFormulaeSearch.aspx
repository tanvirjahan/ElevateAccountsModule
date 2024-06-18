<%@ Page Title="" Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="HandlingFeesSellingFormulaeSearch.aspx.vb" Inherits="PriceListModule_HandlingFeesSellingFormulaeSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language="javascript" type="text/javascript" >

    function GetValueFromName() {

        var ddl = document.getElementById("<%=ddlCurrencyName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlCurrencyCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function GetValueFromCode() {
        var ddl = document.getElementById("<%=ddlCurrencyCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlCurrencyName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function GetValueFromGrpName() {

        var ddl = document.getElementById("<%=ddlGrpName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGrpCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }

    function GetValueFromGrpCode() {
        var ddl = document.getElementById("<%=ddlGrpCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text == document.getElementById("<%=ddlGrpName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
   

</script>

<table  style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid; width: 898px;">
  <tr><td align="center" class="field_heading">
           <asp:Label id="lblheading" runat="server" Text="Handling Fees Selling Formulas List" CssClass="field_heading" __designer:wfdid="w30"></asp:Label></td></tr>
   <tr><td style="color: blue" align="center" class="td_cell">
          Type few characters of code or name and click search
   </td></tr>
   <tr>
    <td>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<table><tbody>
<tr><td class="td_cell" align=center colSpan=6>
    <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" Checked="True" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w127"></asp:RadioButton>
    &nbsp;&nbsp;&nbsp;
    <asp:RadioButton id="rbtnadsearch" runat="server" Text="AdvanceSearch" ForeColor="Black" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w128"></asp:RadioButton>&nbsp;
    <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" __designer:dtid="1125899906842639" CssClass="search_button" __designer:wfdid="w129"></asp:Button>&nbsp;
    <asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" Font-Bold="False" __designer:dtid="1125899906842640" CssClass="search_button" __designer:wfdid="w130"></asp:Button>&nbsp; 
    <asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
        Text="Help" Font-Bold="False" CssClass="search_button" __designer:wfdid="w29"></asp:Button>&nbsp;
    <asp:Button id="btnAddNew" tabIndex=7 runat="server" Text="Add New" 
        Font-Bold="False" __designer:dtid="4785074604081165" CssClass="btn"  __designer:wfdid="w1"></asp:Button>&nbsp;
    <asp:Button id="btnPrint" tabIndex=9 runat="server" Text="Report" __designer:dtid="4785074604081167" CssClass="btn" __designer:wfdid="w2"></asp:Button></TD></TR><TR><TD class="td_cell" colSpan=1>
    <asp:Label id="Label2" runat="server" Text="Selling Formula code" Width="125px" CssClass="td_cell" __designer:wfdid="w30"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><asp:TextBox id="txtSellingCode" tabIndex=1 runat="server" Width="143px" CssClass="field_caption" __designer:wfdid="w31" MaxLength="20"></asp:TextBox></TD><TD class="td_cell" colSpan=1>
    <asp:Label id="Label1" runat="server" Text="Selling Formula Name" ForeColor="Black" Width="133px" CssClass="td_cell" __designer:wfdid="w33"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><asp:TextBox id="txtSellingName" tabIndex=2 runat="server" Width="199px" CssClass="field_caption" __designer:wfdid="w32"></asp:TextBox></TD>
    <TD class="td_cell" colSpan=1><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption" __designer:wfdid="w5"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown" AutoPostBack="True" __designer:wfdid="w6" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR>
    
    <TR><TD class="td_cell" colSpan=1><asp:Label id="lblcurrcode" runat="server" Text="Currency Code" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w17" Visible="False"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><SELECT onchange="GetValueFromName()" style="WIDTH: 148px" id="ddlCurrencyCode" class="drpdown" tabIndex=3 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
    <TD class="td_cell" colSpan=1><asp:Label id="lblcurrname" runat="server" Text="Currency Nmae" ForeColor="Black" Width="100px" CssClass="td_cell" __designer:wfdid="w17" Visible="False"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><SELECT onchange="GetValueFromCode()" style="WIDTH: 205px" id="ddlCurrencyName" class="drpdown" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
    <TD class="td_cell" colSpan=1></TD><TD class="td_cell" colSpan=1></TD></TR>

    <TR><TD class="td_cell" colSpan=1><asp:Label id="lblgrpcode" runat="server" Text="Group Code" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w17" Visible="False"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><SELECT onchange="GetValueFromGrpName()" style="WIDTH: 148px" id="ddlGrpCode" class="drpdown" tabIndex=5 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
    <TD class="td_cell" colSpan=1><asp:Label id="lblgrpname" runat="server" Text="Group Name" ForeColor="Black" Width="100px" CssClass="td_cell" __designer:wfdid="w17" Visible="False"></asp:Label></TD>
    <TD class="td_cell" colSpan=1><SELECT onchange="GetValueFromGrpCode()" style="WIDTH: 205px" id="ddlGrpName" class="drpdown" tabIndex=6 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
    <TD class="td_cell" colSpan=1></TD><TD class="td_cell" colSpan=1></TD></TR>
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
            </td>
        </tr>
         <tr>
             <td align="left" >
                 &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="8"
                    Text="Export To Excel" />
                 </td>
         </tr>
        <tr>
            <td>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="942px" CssClass="grdstyle" __designer:wfdid="w121" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField SortExpression="sellcode" Visible="False" HeaderText="Selling Code"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("othsellcode") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("othsellcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othsellcode" SortExpression="othsellcode" HeaderText="Selling Code"></asp:BoundField>
<asp:BoundField DataField="othsellname" SortExpression="othsellname" HeaderText="Selling Name"></asp:BoundField>
<asp:BoundField DataField="othgrpcode" SortExpression="othgrpcode" HeaderText="Group Code"></asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group Name"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency Code"></asp:BoundField>
<asp:BoundField DataField="calcfrom" SortExpression="calcfrom" HeaderText="Formula From"></asp:BoundField>
<asp:BoundField DataField="fmlacurr" SortExpression="fmlacurr" HeaderText="Formula Currency"></asp:BoundField>
<asp:BoundField DataField="sellstring" SortExpression="sellstring" HeaderText="Formula"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy HH:mm:ss }" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
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

<HeaderStyle CssClass="grdheader" ForeColor="White"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="937px" __designer:wfdid="w122" 
                Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel>
</td>
        </tr>
    </table>




    </table>




</asp:Content>

