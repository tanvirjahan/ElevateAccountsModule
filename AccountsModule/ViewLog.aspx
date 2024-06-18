<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="ViewLog.aspx.vb" Inherits="AccountsModule_ViewLog" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table  style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell">

<tbody>

<TR>
<TD class="field_heading" align="center" style="width: 526px"><asp:Label id="lblHeading" runat="server" Text="View Log" CssClass="field_heading" Width="500px"></asp:Label> </TD>

</TR>



<tr>

<td style="height:101px; width:426px;">
<asp:UpdatePanel id="UpdatePanel1" runat="server">
<contenttemplate>


<table style="width: 612px">
<tbody>
<tr>
<td class="td_cell" colspan="4" align="center">


</td>

</tr>


<tr>
  <td class="td_cell">
     Doc No:</td>
                        
<td>
<asp:Label ID="lblDocNo" runat="server" class="field_input"  Text=""></asp:Label>
</td>



<td class="td_cell">
Document Type
</td>
<td>
<asp:Label ID="lblTrantype" runat="server" class="field_input"  Text="">

</asp:Label>
</td>


<td class="td_cell">
Transaction Date
</td>
<td>
<asp:Label ID="lblTrandate" runat="server" class="field_input"  Text="">

</asp:Label>
</td>

</tr>
<tr>
<td colspan="6">

</td>
</tr>

<tr>

<td class="td_cell">
User
</td>
<td>
<select id="ddlUser" runat="server" class="dropdown" style="WIDTH: 154px" 
            tabindex="4"> 
                <option selected="" value="[Select]">[Select]</option>

                </select>
                </td>

<td class="td_cell">
Order By
</td>
<td>
<td>

<asp:DropDownList id="ddlOrderBy" runat="server" Width="104px" AutoPostBack="True" CssClass="field_input" OnSelectedIndexChanged="ddlOrderBy_SelectedIndexChanged"></asp:DropDownList>




                </td>

</td>


</tr>

<tr>
<td colspan="6">

</td>
</tr>

<tr>
<td class="td_cell" colspan="4" align="center">
<asp:Button ID="btnSearch" Text="Search" runat="server" align="center" 
        font_bold="false" CssClass="search_button">
        </asp:button>




&nbsp;



<asp:Button ID="btnClear" Text="Clear" runat="server" align="center" 
        font_bold="false" CssClass="search_button">
        
</asp:button>
&nbsp;


<asp:Button ID="btnreport" Text="Report" runat="server" align="center" 
        font_bold="false" CssClass="search_button">
        
</asp:button>
</td>
</tr>











<tr>
<td>



</td>


</tr>






</tbody>

</table>


</contenttemplate> 
</asp:UpdatePanel>


<tr>
<td>
<asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=11 runat="server" Width="988px" 
                CssClass="grdstyle" AllowPaging="True" AllowSorting="True" 
                AutoGenerateColumns="False" CellPadding="3" GridLines="Vertical">
                
<Columns>

<asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User">
<HeaderStyle HorizontalAlign="Center">
</headerstyle>
<ItemStyle HorizontalAlign ="Center">
</itemstyle>
</asp:BoundField>



<asp:BoundField DataField= "moddate" SortExpression="moddate" HeaderText="Transaction Date"  DataFormatString="{0:dd/MM/yyyy}">
<HeaderStyle HorizontalAlign="Center">
</headerstyle>

<ItemStyle HorizontalAlign ="Center">
</itemstyle>

</asp:BoundField>
<asp:BoundField DataField="modtime" SortExpression="modtime" HeaderText="Time">
<HeaderStyle HorizontalAlign="Center">
</headerstyle>

<ItemStyle HorizontalAlign ="Center">
</itemstyle>
</asp:BoundField>

<asp:BoundField DataField="description" SortExpression="description" HeaderText="Reason">
<HeaderStyle HorizontalAlign="Center">
</headerstyle>
<ItemStyle HorizontalAlign ="Center">
</itemstyle>
</asp:BoundField>

</Columns> 

<FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> 
<asp:Label id="lblMsg" runat="server" Width="365px" 
                Text="Records not found. Please redefine search criteria" Font-Size="8pt" 
                Font-Names="Verdana" Height="25px" ForeColor="Red" Font-Bold="True" 
                Visible="False"></asp:Label>
</contenttemplate> 
</asp:updatepanel>


</td>


</tr>







</td>
</tr>


</tbody>

</table>
    
    
   

    <script language="javascript" type="text/javascript">
// <![CDATA[







       

// ]]>
    </script>
</asp:Content>

