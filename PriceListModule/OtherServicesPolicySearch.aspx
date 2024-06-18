<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false" CodeFile="OtherServicesPolicySearch.aspx.vb" Inherits="OtherServicesPolicySearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
function CallWebMethod(methodType)
    {
       switch(methodType)
        {
            
            case "groupcode":
                var select=document.getElementById("<%=ddlGroupCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlGrpName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;            
            case "groupname":
                var select=document.getElementById("<%=ddlGrpName.ClientID%>");
                var selectname=document.getElementById("<%=ddlGroupCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;            
              case "marketcode":
                var select=document.getElementById("<%=ddlMarketCode.ClientID%>");
                var selectname=document.getElementById("<%=ddlMarketName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;            
            case "marketname":
                var select=document.getElementById("<%=ddlMarketName.ClientID%>");
                var selectname=document.getElementById("<%=ddlMarketCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;                
                break;            
             
        }
    }

 function TimeOutHandler(result)
    {
        alert("Timeout :" + result);
    }

    function ErrorHandler(result)
    {
        var msg=result.get_exceptionType() + "\r\n";
        msg += result.get_message() + "\r\n";
        msg += result.get_stackTrace();
        alert(msg);
    }



</script>

    <table style="height: 1px; border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid;" id="TABLE1" >
        <tr>
            <td align="center" class="field_heading">
                Other Services Policy List</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="color: blue">
                Type few characters of code or name and click search</td>
        </tr>
        <tr>
            <td class="td_cell" align="left">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 852px"><TBODY><TR><TD align=center colSpan=4>&nbsp;<asp:RadioButton id="rbtnSearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w7" AutoPostBack="True" Checked="True" GroupName="GrSearch" OnCheckedChanged="rbtnSearch_CheckedChanged"></asp:RadioButton> <asp:RadioButton id="rbtnAdvance" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" __designer:wfdid="w8" AutoPostBack="True" GroupName="GrSearch"></asp:RadioButton> <asp:Button id="btnSearch" tabIndex=6 runat="server" Text="Search" Font-Bold="False" Width="56px" CssClass="search_button" __designer:wfdid="w9"></asp:Button> <asp:Button id="btnClear" tabIndex=7 runat="server" Text="Clear" Font-Bold="False" Width="45px" CssClass="search_button" __designer:wfdid="w10"></asp:Button>&nbsp;<asp:Button id="btnHelp" tabIndex=8 onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="search_button" __designer:wfdid="w28"></asp:Button>&nbsp;<asp:Button id="btnAddNew" tabIndex=8 runat="server" Text="Add New" Font-Bold="False" __designer:dtid="15481123719086093" Width="73px" CssClass="btn" __designer:wfdid="w11"></asp:Button>&nbsp;<asp:Button id="btnPrint" tabIndex=10 runat="server" Text="Report" Font-Bold="False" __designer:dtid="15481123719086095" Width="62px" CssClass="btn" __designer:wfdid="w12"></asp:Button></TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 24px; TEXT-ALIGN: left" align=left>Transaction ID&nbsp;&nbsp;&nbsp;&nbsp; </TD><TD style="WIDTH: 191px; HEIGHT: 24px; TEXT-ALIGN: left" align=left><INPUT style="WIDTH: 200px" id="txtTransid" class="txtbox" tabIndex=1 type=text maxLength=20 runat="server" /></TD><TD style="WIDTH: 132px; HEIGHT: 24px; TEXT-ALIGN: left" align=left>Order By</TD><TD style="WIDTH: 763px; HEIGHT: 24px; TEXT-ALIGN: left" align=left><asp:DropDownList id="ddlOrderBy" runat="server" CssClass="drpdown" __designer:wfdid="w5" AutoPostBack="True"><asp:ListItem Value="0">Tran Id Desc</asp:ListItem>
<asp:ListItem Value="1">Tran Id Asc</asp:ListItem>
<asp:ListItem Value="2">Group Code</asp:ListItem>
<asp:ListItem Value="3">Group Name</asp:ListItem>
<asp:ListItem Value="4">Market Code</asp:ListItem>
<asp:ListItem Value="5">Market Name</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 26px; TEXT-ALIGN: left" align=left>Group&nbsp; Code</TD><TD style="WIDTH: 191px; HEIGHT: 26px; TEXT-ALIGN: left" align=left><SELECT style="WIDTH: 206px" id="ddlGroupCode" class="drpdown" tabIndex=2 onchange="CallWebMethod('groupcode')" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 132px; HEIGHT: 26px">Group&nbsp; Name</TD><TD style="WIDTH: 763px; HEIGHT: 26px; TEXT-ALIGN: left" align=left><SELECT style="WIDTH: 270px" id="ddlGrpName" class="drpdown" tabIndex=3 onchange="CallWebMethod('groupname')" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 16px; TEXT-ALIGN: left" align=left><asp:Label id="lblMarketCode" runat="server" Text="Market Code" CssClass="field_input" __designer:wfdid="w59" Visible="False"></asp:Label></TD><TD style="WIDTH: 191px; HEIGHT: 16px; TEXT-ALIGN: left" align=left><SELECT style="WIDTH: 206px" id="ddlMarketCode" class="drpdown" tabIndex=4 onchange="CallWebMethod('marketcode')" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD><TD style="WIDTH: 132px" align=left><asp:Label id="lblMarketName" runat="server" Text="Market Name" CssClass="field_input" __designer:wfdid="w59" Visible="False"></asp:Label></TD><TD align=left><SELECT style="WIDTH: 270px" id="ddlMarketName" class="drpdown" tabIndex=5 onchange="CallWebMethod('marketname')" runat="server" Visible="false"> <OPTION selected></OPTION></SELECT></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left" class="td_cell" style="height: 16px">
                &nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="btn" Text="Export To Excel"
                                Width="106px" TabIndex="9" />
                            </td>
        </tr>
        <tr>
            <td class="td_cell" align="left" style="height: 16px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" tabIndex=11 runat="server" Font-Size="10px" Width="924px" CssClass="grdstyle" __designer:wfdid="w15" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
<FooterStyle CssClass="grdfooter"></FooterStyle>
<Columns>
<asp:TemplateField SortExpression="tranid" Visible="False" HeaderText="Transaction ID"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("tranid") %>' __designer:wfdid="w80"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lbltranid" runat="server" Text='<%# Bind("tranid") %>' CssClass="field_input" __designer:wfdid="w79"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="tranid" SortExpression="tranid" HeaderText="Transaction ID"></asp:BoundField>
<asp:BoundField DataField="othgrpcode" SortExpression="othgrpcode" HeaderText="Group Code"></asp:BoundField>
<asp:BoundField DataField="othgrpname" SortExpression="othgrpname" HeaderText="Group Name"></asp:BoundField>
<asp:BoundField DataField="plgrpcode" SortExpression="plgrpcode" HeaderText="Market Code"></asp:BoundField>
<asp:BoundField DataField="plgrpname" SortExpression="plgrpname" HeaderText="Market Name"></asp:BoundField>
<asp:BoundField DataField="active" SortExpression="active" HeaderText="Active"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
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
<EmptyDataTemplate>
<asp:Label  runat="server" __designer:wfdid="w74"></asp:Label> 
</EmptyDataTemplate>

<SelectedRowStyle CssClass="grdselectrowstyle"></SelectedRowStyle>

<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle CssClass="grdheader" ForeColor="white"></HeaderStyle>

<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" Text="No Record Found. Please Redefine Search Criteria." Font-Size="8pt" Font-Names="Verdana" ForeColor="#084573" Font-Bold="True" __designer:dtid="5629499534213138" Width="400px" CssClass="field_input" __designer:wfdid="w16" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel>
                </td>
        </tr>
    </table>
</asp:Content>

