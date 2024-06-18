<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="OtherServicesPriceListSearch.aspx.vb" Inherits="OtherServicesPriceListSearch"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
     <%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="JavaScript" type="text/javascript" >
    window.history.forward(1);  
</script>
<script language="javascript" type="text/javascript">
function chkTextLock(e)
	{	    
		return false;
	}
  
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
			
function  GetValueGroupFromMarket()
{

	var ddl = document.getElementById("<%=ddlMarketName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlMarketCode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueGroupCodeMarket()
{
	var ddl = document.getElementById("<%=ddlMarketCode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlMarketName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


function  GetValueGroupFromOtherService()
{

	var ddl = document.getElementById("<%=ddlOtherSericeGName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlOtherSericeGCode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueGroupCodeOtherService()
{
	var ddl = document.getElementById("<%=ddlOtherSericeGCode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlOtherSericeGName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}

function  GetValueGroupFromOtherServiceSell()
{

	var ddl = document.getElementById("<%=ddlOtherSericeSellTypeN.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlOtherSericeSellType.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueGroupCodeOtherServiceSell()
{
	var ddl = document.getElementById("<%=ddlOtherSericeSellType.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlOtherSericeSellTypeN.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}



function  GetValueGroupFromCurrency()
{

	var ddl = document.getElementById("<%=ddlCurrencyName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyCode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueGroupCodeCurrency()
{
	var ddl = document.getElementById("<%=ddlCurrencyCode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


function  GetValueGroupFromSubSeason()
{

	var ddl = document.getElementById("<%=ddlSubSeasonName.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSubSeasonCD.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueGroupCodeSubSeason()
{
	var ddl = document.getElementById("<%=ddlSubSeasonCD.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlSubSeasonName.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


// <!CDATA[



// ]]>
</script>

    <table align="left" class="td_cell" style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading">
                Other Services Price List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="color: blue;">
                Type few characters of code or name and click search &nbsp; &nbsp;</td>
        </tr>
        <tr>
            <td>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 926px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="td_cell" align=center colSpan=4><asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" Checked="True" GroupName="GrSearch" OnCheckedChanged="rbtnsearch_CheckedChanged"></asp:RadioButton>
 <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged"></asp:RadioButton> 
    <asp:Button id="btnSearch" tabIndex=14 onclick="btnSearch_Click" runat="server" 
        Text="Search" Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=15 runat="server" Text="Clear" 
        Font-Bold="False" CssClass="search_button"></asp:Button>&nbsp;
        <asp:Button id="btnHelp" tabIndex=8 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
        id="btnAddNew" tabIndex=16 onclick="btnAddNew_Click" runat="server" 
        Text="Add New" Font-Bold="False" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
        id="btnPrint" tabIndex=18 runat="server" Text="Report" CssClass="field_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 338px" class="td_cell" align=left>&nbsp;PL Code </TD><TD style="WIDTH: 197px"><asp:TextBox id="TxtPLCD" tabIndex=1 runat="server" Width="194px" CssClass="field_input" MaxLength="10"></asp:TextBox></TD>
        <TD style="WIDTH: 363px" class="td_cell">Order By</TD><TD style="WIDTH: 801px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="152px" CssClass="field_input" AutoPostBack="True"><asp:ListItem Value="0">Plist Code Desc</asp:ListItem>
<asp:ListItem Value="1">Plist Code Asc</asp:ListItem>
<asp:ListItem Value="2">Group Code</asp:ListItem>
<asp:ListItem Value="3">Group Name</asp:ListItem>
<asp:ListItem Value="4">Market Code</asp:ListItem>
<asp:ListItem Value="5">Selltype</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 338px" class="td_cell" align=left>&nbsp;Market&nbsp;Code</TD><TD style="WIDTH: 197px">
<SELECT onblur="GetValueGroupFromMarket()" style="WIDTH: 200px" id="ddlMarketCode" class="field_input" tabIndex=2 runat="server"> <OPTION selected></OPTION></SELECT> </TD>
<TD style="WIDTH: 363px" class="td_cell" align=left>Market&nbsp;Name</TD><TD style="WIDTH: 801px"><SELECT onblur="GetValueGroupCodeMarket()" style="WIDTH: 300px" id="ddlMarketName" class="field_input" tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT> </TD></TR>
    <tr>
        <td align="left" class="td_cell" style="width: 338px; height: 16px">
            Approval/Unapproval</td>
        <td style="width: 197px; height: 16px">
            <asp:DropDownList ID="DDLstatus" runat="server" CssClass="field_input" Width="198px">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Unapprove</asp:ListItem>
                <asp:ListItem Value="2">Approve</asp:ListItem>
            </asp:DropDownList></td>
        <td align="left" class="td_cell" style="width: 363px; height: 16px">
            Stop Web</td>
        <td style="width: 801px; height: 16px"><asp:DropDownList ID="DDLshowagent" runat="server" CssClass="field_input" Width="198px">
            <asp:ListItem Value="0">All</asp:ListItem>
            <asp:ListItem Value="1">Stop Web</asp:ListItem>
        </asp:DropDownList></td>
    </tr>
    <TR><TD class="td_cell" align=left colSpan=4><asp:Panel id="Panel1" runat="server"><TABLE style="WIDTH: 810px"><TBODY><TR><TD style="WIDTH: 475px; height: 22px;" class="td_cell" align=left>Other&nbsp;Service&nbsp;Group&nbsp;Code</TD><TD style="WIDTH: 179px; height: 22px;"><SELECT onblur="GetValueGroupFromOtherService()" style="WIDTH: 200px" id="ddlOtherSericeGCode" class="field_input" tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 414px; height: 22px;" class="td_cell" align=left>Other&nbsp;Service&nbsp;Group&nbsp;Name</TD><TD style="WIDTH: 213px; height: 22px;"><SELECT onblur="GetValueGroupCodeOtherService()" style="WIDTH: 300px" id="ddlOtherSericeGName" class="field_input" tabIndex=5 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 475px" class="td_cell" align=left>Other Sell Type Code </TD><TD style="WIDTH: 179px"><SELECT onblur="GetValueGroupFromOtherServiceSell()" style="WIDTH: 200px" id="ddlOtherSericeSellType" class="field_input" tabIndex=6 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 414px" class="td_cell" align=left>Other Sell Type Name</TD><TD style="WIDTH: 213px"><SELECT onblur="GetValueGroupCodeOtherServiceSell()" style="WIDTH: 300px" id="ddlOtherSericeSellTypeN" class="field_input" tabIndex=7 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 475px" class="td_cell" align=left>Currency Code </TD><TD style="WIDTH: 179px"><SELECT onblur="GetValueGroupFromCurrency()" style="WIDTH: 200px" id="ddlCurrencyCode" class="field_input" tabIndex=8 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 414px" class="td_cell" align=left>Currency Name</TD><TD style="WIDTH: 213px"><SELECT onblur="GetValueGroupCodeCurrency()" style="WIDTH: 300px" id="ddlCurrencyName" class="field_input" tabIndex=9 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 475px" class="td_cell" align=left>Sub Season Code </TD><TD style="WIDTH: 179px"><SELECT onblur="GetValueGroupFromSubSeason()" style="WIDTH: 200px" id="ddlSubSeasonCD" class="field_input" tabIndex=10 runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 414px" class="td_cell" align=left>Sub Season Name</TD><TD style="WIDTH: 213px"><SELECT onblur="GetValueGroupCodeSubSeason()" style="WIDTH: 300px" id="ddlSubSeasonName" class="field_input" tabIndex=11 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 475px" class="td_cell" align=left>From Date</TD><TD style="WIDTH: 179px"><ews:DatePicker id="dpFromdate" tabIndex=12 runat="server" Width="200px" CssClass="field_input" OnSelectionChanged="dpFromdate_SelectionChanged" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker></TD><TD style="WIDTH: 414px" class="td_cell" align=left>To&nbsp;Date</TD><TD style="WIDTH: 213px"><ews:DatePicker id="dpToDate" tabIndex=13 runat="server" Width="211px" CssClass="field_input" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"></ews:DatePicker></TD></TR>
    <tr>
        <td align="left" class="td_cell" style="width: 475px">
        </td>
        <td style="width: 179px">
        </td>
        <td align="left" class="td_cell" style="width: 414px">
        </td>
        <td style="width: 213px">
        </td>
    </tr>
</TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>&nbsp;&nbsp; 
</contenttemplate>
    </asp:UpdatePanel>&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" >
                &nbsp;&nbsp;
                <asp:Button ID="btnExportToExcel" runat="server" CssClass="field_button" TabIndex="17"
                    Text="Export To Excel" />
                </td>
        </tr>
        <tr>
            <td align="left">
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=19 runat="server" Font-Size="10px" BackColor="White" Width="934px" CssClass="td_cell" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" >
<FooterStyle BackColor="#6B6B9A" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Price List Code"><EditItemTemplate>                                      
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lbloplistcode" runat="server" Text='<%# Bind("oplistcode") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField Visible="False" HeaderText="Approve"><EditItemTemplate>                                      
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblapprove" runat="server" Text='<%# Bind("approve") %>'></asp:Label> 
</ItemTemplate>
</asp:TemplateField>


<asp:BoundField DataField="oplistcode" SortExpression="oplistcode" HeaderText="Price List Code"></asp:BoundField>
<asp:BoundField DataField="othgrpcode" SortExpression="othgrpcode" HeaderText="Group Code">
<ItemStyle Width="1440pt" HorizontalAlign="Left"></ItemStyle>

<HeaderStyle Width="1440pt" HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group Name"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" DataField="frmdate" SortExpression="frmdate" HeaderText="From Date"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" DataField="todate" SortExpression="todate" HeaderText="To Date"></asp:BoundField>
<asp:BoundField DataField="approve" SortExpression="status" HeaderText="Status"></asp:BoundField>
<asp:BoundField DataField="showagent" SortExpression="ShowAgent" HeaderText="Show in Web"></asp:BoundField>
<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market Code"></asp:BoundField>
<asp:BoundField DataField="othsellcode" SortExpression="othsellcode" HeaderText="Sell Type"></asp:BoundField>
<asp:BoundField DataField="subseascode" SortExpression="subseascode" HeaderText="Sub Season"></asp:BoundField>
<asp:BoundField DataField="currcode" SortExpression="currcode" HeaderText="Currency"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{00:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle Width="1440px"></ItemStyle>

<HeaderStyle Width="1440px"></HeaderStyle>
</asp:BoundField>
<asp:ButtonField HeaderText="Action" Text="Edit" CommandName="Editrow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="View" CommandName="View">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Delete" CommandName="Deleterow">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
<asp:ButtonField HeaderText="Action" Text="Copy" CommandName="Copy">
<ItemStyle ForeColor="Blue"></ItemStyle>
</asp:ButtonField>
</Columns>

<RowStyle CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader"  ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="357px" CssClass="lblmsg" 
                Visible="False"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>

