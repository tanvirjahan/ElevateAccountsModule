<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false"  EnableEventValidation="false"  CodeFile="SalesexpertsSearch.aspx.vb" Inherits="CitiesSearch"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type ="text/javascript" >
function CallWebMethod(methodType)
    {
        switch(methodType)
        {
        case "ctrycode":        
                var select=document.getElementById("<%=ddldeptcode.ClientID%>");                
                var selectname=document.getElementById("<%=ddldeptname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;
        case "ctryname":
                var select=document.getElementById("<%=ddldeptname.ClientID%>");                
                var selectname=document.getElementById("<%=ddldeptcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        }
        }
        
         function checkNumber(e)
			{	    
			   
			   if (event.keycode=13 )
			   {
			   return true;
			   } 	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
			function checkCharacter(e)
			{	   
			if (event.keycode=13 )
			   {
			   return true;
			   } 	
			 
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
				
</script> 
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100%; height: 11px">
                <table style="width: 100%">
                    <tr>
                        <td align="center" class="field_heading" style="width:100%; height: 1px">
                Sales Experts  List</td>
                    </tr>
                    <tr>
                        <td align="center" class="td_cell" style="color: blue">
                Type few characters of code or name and click search 
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 872px"><TBODY><TR><TD style="HEIGHT: 22px; TEXT-ALIGN: center" colSpan=4>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:RadioButton id="rbtnsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" Checked="True" OnCheckedChanged="rbtnsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w40"></asp:RadioButton>&nbsp;
<asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" Width="120px" CssClass="td_cell" OnCheckedChanged="rbtnadsearch_CheckedChanged" GroupName="GrSearch" AutoPostBack="True" __designer:wfdid="w41"></asp:RadioButton>&nbsp;
<asp:Button id="btnSearch" tabIndex=5 runat="server" Text="Search" 
        Font-Bold="False" CssClass="search_button" __designer:wfdid="w42" 
        EnableTheming="True" Font-Italic="False"></asp:Button>
&nbsp;<asp:Button id="btnClear" tabIndex=6 runat="server" Text="Clear" 
        CssClass="search_button" __designer:wfdid="w43"></asp:Button>&nbsp;
<asp:Button id="btnhelp" tabIndex=10 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="search_button" __designer:wfdid="w3"></asp:Button>&nbsp;
<asp:Button id="btnAddNew" tabIndex=7 runat="server" Text="Add New" 
        Font-Bold="False" __designer:dtid="14636698788954128" CssClass="btn" 
        __designer:wfdid="w4"></asp:Button>&nbsp;
<asp:Button id="btnPrint" tabIndex=9 runat="server" Text="Report" 
        __designer:dtid="14636698788954130" CssClass="btn" __designer:wfdid="w5"></asp:Button></TD><TD style="WIDTH: 63px; HEIGHT: 22px; TEXT-ALIGN: center" colSpan=1></TD><TD style="WIDTH: 100px; HEIGHT: 22px; TEXT-ALIGN: center" colSpan=1></TD></TR><TR><TD style="WIDTH: 62px">
    <asp:Label id="Label1" runat="server" Text=" Code" Width="72px" 
        CssClass="field_caption" __designer:wfdid="w44" Height="16px"></asp:Label></TD><TD style="WIDTH: 73px">
    <asp:TextBox id="txtcitycode" tabIndex=1 runat="server" Width="203px" 
        CssClass="field_input" __designer:wfdid="w45" MaxLength="20"></asp:TextBox></TD><TD style="WIDTH: 100px"><asp:Label id="Label2" runat="server" Text=" Name" Width="72px" CssClass="field_caption" __designer:wfdid="w46"></asp:Label></TD><TD style="WIDTH: 100px">
    <asp:TextBox id="txtcityname" tabIndex=2 runat="server" Width="263px" 
        CssClass="field_input" __designer:wfdid="w47"></asp:TextBox></TD><TD style="WIDTH: 63px"><asp:Label id="Label3" runat="server" Text="Order By" Width="72px" CssClass="field_caption" __designer:wfdid="w3"></asp:Label></TD><TD style="WIDTH: 100px"><asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" CssClass="drpdown" AutoPostBack="True" __designer:wfdid="w4" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 62px; HEIGHT: 22px"><asp:Label id="lblctrycode" runat="server" Text="Dept Code" Width="72px" CssClass="field_caption" __designer:wfdid="w48" Visible="False"></asp:Label></TD><TD style="WIDTH: 73px; HEIGHT: 22px">
<SELECT style="WIDTH: 209px" id="ddldeptcode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('ctrycode')" runat="server" visible="false" 
        enableviewstate="true"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 100px; HEIGHT: 22px"><asp:Label id="lblctryname" runat="server" Text="Dept  Name" Width="74px" CssClass="field_caption" __designer:wfdid="w49" Visible="False"></asp:Label></TD><TD style="WIDTH: 100px; HEIGHT: 22px">
<SELECT style="WIDTH: 269px" id="ddldeptname" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('ctryname')" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 63px; HEIGHT: 22px"></TD><TD style="WIDTH: 100px; HEIGHT: 22px"></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td style="height: 15px">
                            &nbsp;<asp:Button ID="btnExportToExcel" runat="server" CssClass="btn" 
                                Text="Export To Excel" TabIndex="8" />
                            </td>
                    </tr>
                    <tr>
                        <td style="height: 33px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" Width="100%" CssClass="grdstyle" __designer:wfdid="w50" GridLines="Vertical" CellPadding="3" BorderWidth="1px" BorderStyle="None" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField Visible="False" HeaderText="Sperson Code">
<EditItemTemplate>
 <asp:TextBox ID="spersoncd" runat="server" Text='<%# Bind("spersoncode") %>' 
        CssClass="field_input"></asp:TextBox>
                                        
</EditItemTemplate>

<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("spersoncode") %>'></asp:Label>
</ItemTemplate>

</asp:TemplateField>

<asp:BoundField DataField="spersoncode" SortExpression="spersoncode" HeaderText="Sperson Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="spersonname" SortExpression="spersonname" HeaderText="Sperson Name">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="deptcode" SortExpression="deptcode" HeaderText="Dep Code">
<ItemStyle HorizontalAlign="Left"></ItemStyle>

<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
</asp:BoundField>

<asp:BoundField DataField="adddate" SortExpression="adddate" DataFormatString="{0:dd/MM/yyyy } " HeaderText="Date Created">
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
</asp:GridView> 
<asp:Label id="lblMsg" runat="server" Text="Records not found. Please redefine search criteria" Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" Width="357px" __designer:wfdid="w51" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
