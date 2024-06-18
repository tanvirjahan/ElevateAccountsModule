<%@ Page Language="VB" MasterPageFile="~/ExcursionMaster.master" AutoEventWireup="false" CodeFile="ExcursionDaysoftheweekSearch.aspx.vb" Inherits="Other_Services_Selling_Types_Search"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type ="text/javascript" >
function  GetValueFrom()
{

	var ddl = document.getElementById("<%=ddlgpname.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpcode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlgpcode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpname.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}


</script>

   <table>
        <tr>
            <td >
                <table id="TABLE1"  style="border-right: gray 2px solid;
                    border-top: gray 2px solid; border-left: gray 2px solid; width: 918px; border-bottom: gray 2px solid;
                    ">
                    <tr>
                        <td class="td_cell" colspan="1" >
                            <table >
                                <tr align=center>
                                    <td align="center" class="field_heading"   ><asp:Label runat="server" ID="Lblselltypes" Text="Excursion  Days of the Week List"></asp:Label>
                                        &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp; &nbsp;</td>
                                </tr>
                                <tr align=center >
                                    <td align="center" class="td_cell" style=" color: blue; text-align: center; " >
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td  valign="bottom">
                                        <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                            <contenttemplate>
<TABLE><TBODY><TR><TD style="TEXT-ALIGN: center" class="td_cell" align=center colSpan=7>&nbsp; &nbsp;&nbsp;
 <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w51" Checked="True" GroupName="GrSearch" AutoPostBack="True" OnCheckedChanged="rbtnsearch_CheckedChanged"></asp:RadioButton>&nbsp;
 <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w52" GroupName="GrSearch" AutoPostBack="True" OnCheckedChanged="rbtnadsearch_CheckedChanged"></asp:RadioButton>&nbsp;&nbsp;
 <asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button" __designer:wfdid="w56"></asp:Button>&nbsp; 
    <asp:Button id="btnClear" tabIndex=6 onclick="btnClear_Click1" runat="server" 
        Text="Clear" Font-Bold="False" CssClass="search_button" __designer:wfdid="w57"></asp:Button>&nbsp;
 <asp:Button id="btnHelp" tabIndex=10 onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="search_button" __designer:wfdid="w19"></asp:Button>&nbsp;<asp:Button 
        id="btnAddNew" tabIndex=7 runat="server" Text="Add New" Font-Bold="False" 
        __designer:dtid="6192449487634451" CssClass="btn" __designer:wfdid="w5"></asp:Button>&nbsp;<asp:Button 
        id="btnPrint" tabIndex=9 runat="server" Text="Report" 
        __designer:dtid="6192449487634453" CssClass="btn" __designer:wfdid="w6" 
        Height="19px"></asp:Button></TD></TR>
 <TR><TD class="td_cell" colSpan=4 style="height: 53px">
     <asp:Panel id="PnlCustSect" runat="server" 
         __designer:wfdid="w55" Height="91px" Width="876px"><TABLE ><TBODY><TR><TD >
     <asp:Label id="Label1" runat="server" Text=" Code" Width="146px" 
         __designer:wfdid="w68"></asp:Label></TD>
 <TD ><asp:TextBox id="txtSellingCode" tabIndex=1 runat="server" Width="183px" CssClass="txtbox" __designer:wfdid="w26" MaxLength="20"></asp:TextBox></TD>
 <TD ><asp:Label id="Label2" runat="server" Text=" Name" Width="80px" __designer:wfdid="w68"></asp:Label></TD>
 <TD style="width: 233px"><asp:TextBox id="txtSellingName" tabIndex=2 runat="server" Width="244px" CssClass="txtbox" __designer:wfdid="w27"></asp:TextBox></TD>
 <TD ><asp:Label id="Label3" runat="server" Text="Order By" Width="50px" CssClass="field_caption" __designer:wfdid="w7"></asp:Label></TD>
 <TD ><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown" __designer:wfdid="w8" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR>
 <TR><TD style="height: 29px" ><asp:label ID="lblgpcode" runat="server"  Text ="Group code" Visible="False" ></asp:label></TD>
 <TD style="height: 29px"><SELECT style="WIDTH: 189px" id="ddlgpcode" class="drpdown" tabIndex=3 
         onchange="GetValueFrom()" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
 <TD style="height: 29px"><asp:Label id="lblgpname" runat="server" Text="Group Name" Width="104px" 
         __designer:wfdid="w68" Visible="False"></asp:Label></TD>
 <TD style="height: 29px; width: 233px;"><SELECT style="WIDTH: 250px" id="ddlgpname" class="drpdown" tabIndex=4 
         onchange="GetValueCode()" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD>
 <TD style="height: 29px"></TD><TD style="height: 29px" ></TD></TR>
             <tr  >
                 <td style="height: 29px">
                     &nbsp;</td>
                 <td style="height: 29px" >

                       &nbsp;</td>

                 <td style="height: 29px" align =left>
                     &nbsp;</td>
                 <td style="height: 29px" >

                  

                        &nbsp;</td>
                
             </tr>
             </TBODY></TABLE></asp:Panel> </TD></TR></TBODY></TABLE>
</contenttemplate>
                                        </asp:UpdatePanel></td>
                                </tr>
                                <tr>
                                    <td style="width: 1022px;" valign="bottom">
                                        &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" TabIndex="8"
                                            Text="Export To Excel" />
                                        </td>
                                </tr>
                              
                            </table>
    <asp:UpdatePanel id="UpdatePanel3" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="902px" CssClass="grdstyle" __designer:wfdid="w61" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None"  AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>



<asp:TemplateField Visible="False" HeaderText="Selling Code">
                        

<ItemTemplate>
<asp:Label id="lblothtypname" runat="server" Text='<%# Bind("othtypname") %>' __designer:wfdid="w1"></asp:Label> 
      <asp:Label ID="lblothtypcode" runat="server" Text='<%# Bind("othtypcode") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="othtypcode" SortExpression="othtypcode" HeaderText=" Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="othtypname" SortExpression="othtypname" HeaderText=" Name">
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

<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy } " DataField="adddate" SortExpression="adddate" HeaderText="Date Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataField="adduser" SortExpression="adduser" HeaderText="User Created">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy } " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
</Columns>

<RowStyle CssClass="grdRowstyle"></RowStyle>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Font-Bold="True" Width="905px" __designer:wfdid="w62" 
                Visible="False" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
    </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="z-index: 100; left: 0px; position: absolute; top: 0px">
        <tr>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

