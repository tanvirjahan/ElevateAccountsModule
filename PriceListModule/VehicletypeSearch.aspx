<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" EnableEventValidation="false" AutoEventWireup="false" CodeFile="VehicletypeSearch.aspx.vb" Inherits="PriceListModule_VehicletypeSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type="text/javascript" >

function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
			
		
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}

</script>


<table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid">
<tr>
            <td align="center" class="field_heading" colspan="4" >
                <asp:Label ID="lblHead" runat="server" Text="Vehicle Type List"></asp:Label>     </td>
        </tr>
        <tr>
            <td align="center" class="td_cell" colspan="4" style="color: blue; ">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" >
 <table  >
                    <tr>
                        <td colspan="4" rowspan="2"">
<asp:UpdatePanel id="UpdatePanel1" runat="server">
 <contenttemplate>
<table>
<tbody>
<tr>
    <td style="TEXT-ALIGN: center" class="td_cell" colSpan=4>
        <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged" Checked="True"></asp:RadioButton>&nbsp; 
       <%-- <asp:RadioButton id="rbtnadsearch" runat="server" Text="AdvanceSearch" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged"></asp:RadioButton>&nbsp;--%>
      <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
      <asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear"  Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
      <asp:Button id="btnHelp" tabIndex=10 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;
       <asp:Button id="btnAddNew" tabIndex=7 runat="server" Text="Add New"  Font-Bold="False" CssClass="btn"></asp:Button>&nbsp;
       <asp:Button id="btnPrint" tabIndex=9 runat="server" Text="Report" CssClass="btn"></asp:Button></TD>
<td style="TEXT-ALIGN: center" class="td_cell" colSpan=1></TD><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=1></td></tr>


<tr><td style="WIDTH: 86px; HEIGHT: 26px" class="td_cell"><SPAN style="COLOR: black">
    Code</SPAN></TD><TD style="HEIGHT: 26px"><INPUT style="WIDTH: 179px" id="txtCode" tabIndex=1 type=text maxLength=20 runat="server" /></TD>
<TD style="HEIGHT: 26px" class="td_cell"><SPAN style="COLOR: black">Name</SPAN></TD><TD style="HEIGHT: 26px" class="td_cell"><INPUT style="WIDTH: 350px" id="txtName" tabIndex=2 type=text maxLength=100 runat="server" /></TD>
<TD style="HEIGHT: 26px" class="td_cell"><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption"></asp:Label></TD>
<TD style="HEIGHT: 26px" class="td_cell"><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR>


<%--<tr><td style="WIDTH: 86px; HEIGHT: 22px" class="td_cell">
<asp:Label id="lblgrpcode" runat="server" Text="Group Code" Width="72px" Visible="False"></asp:Label></TD>
<TD style="HEIGHT: 22px"><SELECT onblur="GetOtherGrpValueFrom()" style="WIDTH: 187px" id="ddlOtherGrpCode" class="drpdown" tabIndex=3 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
<TD style="HEIGHT: 22px" class="td_cell"><asp:Label id="lblgrpname" runat="server" Text="Group Name" Height="15px" Width="76px" Visible="False"></asp:Label></TD>
<TD style="HEIGHT: 22px" class="td_cell"><SELECT onblur="GetOtherGrpValueCode()" style="WIDTH: 356px" id="ddlOtherGrpName" class="drpdown" tabIndex=4 runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
<TD style="HEIGHT: 22px" class="td_cell"></TD><TD style="HEIGHT: 22px" class="td_cell"></TD></TR>
--%>
</TBODY></TABLE>
</contenttemplate>
</asp:UpdatePanel></td>
</tr>
</table>
</td>
</tr>
 <tr>
<td class="td_cell" colspan="4" style="color: red; width: 90%;">
                &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="8" Text="Export To Excel" Width="108px" />
                </td>
        </tr>
        <tr>
            <td class="td_cell" colspan="4" style="color: red; width: 90%;">
<asp:UpdatePanel id="UpdatePanel2" runat="server">
<contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" BackColor="White" Width="97%" CssClass="td_cell" GridLines="Vertical" CellPadding="3" 
BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Category Code"><EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("othcatcode") %>'></asp:TextBox>
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblothtypcode" runat="server" Text='<%# Bind("othcatcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="othcatcode" SortExpression="othcatcode" 
        HeaderText="Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othcatname" SortExpression="othcatname" 
        HeaderText="Name">
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
<asp:BoundField DataField="grporder" SortExpression="grporder" HeaderText="Rank Order">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="minpax" SortExpression="minpax" HeaderText="Min Pax">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="maxpax" SortExpression="maxpax" HeaderText="Max. Pax">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>
<%--<asp:BoundField DataField="unitname" SortExpression="unitname" HeaderText="Unit Name" Visible="False"></asp:BoundField>--%>
<%--<asp:BoundField DataField="printremarks" SortExpression="printremarks" HeaderText="Print Remarks" Visible="False"></asp:BoundField>--%>
<asp:BoundField DataField="paxcheckreqd" SortExpression="paxcheckreqd" HeaderText="Pax Check Required">
<ItemStyle HorizontalAlign="Center"></ItemStyle>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="capacity" SortExpression="capacity" HeaderText="Capacity" ></asp:BoundField>
<asp:BoundField DataField="options" SortExpression="options" HeaderText="Options" ></asp:BoundField>
<asp:BoundField DataField="shuttle" SortExpression="shuttle" HeaderText="Shuttle" ></asp:BoundField>
<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<%--<asp:BoundField DataField="calcyn" SortExpression="calcyn"         HeaderText="Calculate By Pax/Units" Visible="False"></asp:BoundField>--%>
<%--<asp:BoundField DataField="adultchild" HeaderText="Adult/Child" Visible="False"></asp:BoundField>--%>
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
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="357px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>

      <asp:HiddenField ID="hdnGrpCode" runat="server" Value="1" />


</asp:Content>