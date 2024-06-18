<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" EnableEventValidation="false"  CodeFile="RoutesSearch.aspx.vb" Inherits="PriceListModule_RoutesSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type="text/javascript" >
    function GetOtherGrpValueFrom() {

        var ddl = document.getElementById("<%=ddlOtherGrpName.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlOtherGrpCode.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
    function GetOtherGrpValueCode() {
        var ddl = document.getElementById("<%=ddlOtherGrpCode.ClientID%>");
        ddl.selectedIndex = -1;
        // Iterate through all dropdown items.
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].text ==
			document.getElementById("<%=ddlOtherGrpName.ClientID%>").value) {
                // Item was found, set the selected index.
                ddl.selectedIndex = i;
                return true;
            }
        }
    }
</script>

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        width: 964px; border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading" colspan="4" style="width: 791px; height: 7px">
                Routes List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" colspan="4" style="color: blue; width: 791px; height: 7px;">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width: 791px; color: red;">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 666px"><TBODY><TR>
<TD style="TEXT-ALIGN: right" class="td_cell" colSpan=4>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" __designer:dtid="2814749767106575" CssClass="td_cell" Checked="True" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w6"></asp:RadioButton>&nbsp;
<asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" 
        ForeColor="Black" __designer:dtid="2814749767106576" CssClass="td_cell" 
        OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" 
        AutoPostBack="True" __designer:wfdid="w7" Visible="False"></asp:RadioButton> 
    <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button"></asp:Button> 
&nbsp;<asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" 
        Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=10 onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="search_button" __designer:wfdid="w7"></asp:Button></TD>
<TD style="WIDTH: 151px" class="td_cell" colSpan=1><asp:Button id="btnAddNew" 
        tabIndex=7 runat="server" Text="Add New" Font-Bold="False" 
        __designer:dtid="10977524091715597" CssClass="btn" __designer:wfdid="w13"></asp:Button></TD>
        <TD style="WIDTH: 137px" class="td_cell" colSpan=1><asp:Button id="btnPrint" tabIndex=9 runat="server" Text="Report" __designer:dtid="10977524091715599" CssClass="btn" __designer:wfdid="w14"></asp:Button></TD></TR><TR><TD style="WIDTH: 76px; HEIGHT: 24px" class="td_cell"><SPAN style="COLOR: black">
    Code&nbsp;&nbsp; </SPAN></TD>
<TD style="WIDTH: 11px">
    <INPUT style="WIDTH: 163px" id="txtCode" tabIndex=1 type=text maxLength=20 runat="server" class="field_input" /></TD><TD style="WIDTH: 54px; HEIGHT: 24px" class="td_cell"><SPAN style="COLOR: black"><asp:Label id="Label1" runat="server" Text="Name" Width="100px" __designer:wfdid="w1"></asp:Label></SPAN></TD><TD style="WIDTH: 118px" class="td_cell">
    <INPUT style="WIDTH: 241px" id="txtName" tabIndex=2 type=text maxLength=100 runat="server" class="field_input" /></TD>
<TD style="WIDTH: 151px" class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" ForeColor="Black" Width="50px" CssClass="field_caption" __designer:wfdid="w1"></asp:Label></TD><TD style="WIDTH: 137px" class="td_cell"><asp:DropDownList id="ddlOrderBy" runat="server" Width="130px" CssClass="drpdown" AutoPostBack="True" __designer:wfdid="w23" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR>
<TR><TD style="WIDTH: 76px; HEIGHT: 16px" class="td_cell"><SPAN style="COLOR: black"><asp:Label id="lblgrpcode" runat="server" Text="Group Code" __designer:wfdid="w1" Visible="False"></asp:Label></SPAN></TD><TD style="WIDTH: 11px"><SELECT onblur="GetOtherGrpValueFrom()" style="WIDTH: 170px" id="ddlOtherGrpCode" class="drpdown" tabIndex=3 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 54px; HEIGHT: 16px" class=" "><SPAN style="COLOR: black">
    <asp:Label id="lblgrpname" runat="server" Text="Group Name" Height="16px" 
        Width="93px" __designer:wfdid="w2" Visible="False"></asp:Label></SPAN></TD><TD style="WIDTH: 118px" class="td_cell"><SELECT onblur="GetOtherGrpValueCode()" style="WIDTH: 247px" id="ddlOtherGrpName" class="drpdown" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 151px" class="td_cell"></TD><TD style="WIDTH: 137px" class="td_cell"></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width: 791px; color: red;">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                    TabIndex="8" Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="width: 791px; color: red">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="950px" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Type Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othtypcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblothtypcode" runat="server" Text='<%# Bind("othtypcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othtypcode" SortExpression="othtypcode" HeaderText="Type Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText="Type Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othgrpcode" SortExpression="othgrpcode" HeaderText="Group Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="rankorder" SortExpression="rankorder" HeaderText=" Display Order">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="minpax" SortExpression="minpax" HeaderText="Min Pax">
<ItemStyle HorizontalAlign="Right"></ItemStyle>

<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="shuttle" SortExpression="shuttle" HeaderText="Shuttle">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField DataField="printconf" SortExpression="printconf" 
        HeaderText="Print In Confirmation" Visible="False"></asp:BoundField>
<asp:BoundField DataField="paxcheckreq" SortExpression="paxcheckreq" 
        HeaderText="Pax Check Required" Visible="False"></asp:BoundField>
<asp:BoundField DataField="printremarks" SortExpression="printremarks" 
        HeaderText="Print Remarks" Visible="False"></asp:BoundField>
<asp:BoundField DataField="autocancelreq" SortExpression="autocancelreq" 
        HeaderText="Auto Cancellation Req." Visible="False"></asp:BoundField>
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
</asp:GridView>  <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>


