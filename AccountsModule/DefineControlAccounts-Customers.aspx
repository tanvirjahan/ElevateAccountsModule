<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefineControlAccounts-Customers.aspx.vb" Inherits="DefineControlAccounts_Customers"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >

//----------------------------------------
function FillIncomepCode(ddlIccd,ddlIcnm)
{  
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
           
    var codeid=ddlIccode.options[ddlIccode.selectedIndex].text;
    ddlIcname.value=codeid;
                      
}

function FillIncomeName(ddlIccd,ddlIcnm)
{
    ddlIccode = document.getElementById(ddlIccd);
    ddlIcname = document.getElementById(ddlIcnm);
    
    var codeid=ddlIcname.options[ddlIcname.selectedIndex].text;
   
    ddlIccode.value=codeid;
           
}
//-----------------------------------------
function CallWebMethod(methodType)
    {
       switch(methodType)
        {
        
        case "agentcode":
                var select=document.getElementById("<%=ddlCustomercode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCustomername.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        case "agentname":
                var select=document.getElementById("<%=ddlCustomername.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCustomercode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;            
        case "agentcatcode":
                var select=document.getElementById("<%=ddlCategorycode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCategoryname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       case "agentcatname":
                var select=document.getElementById("<%=ddlCategoryname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlCategorycode.ClientID%>");
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
    <table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        border-bottom: gray 1px solid">
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 947px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=5>Define Control Accounts -Customers </TD></TR><TR><TD style="TEXT-ALIGN: center" class="field_input" colSpan=4><SPAN style="COLOR: blue">Type few characters of code or name and click search </SPAN></TD></TR><TR><TD style="WIDTH: 129px" class="field_input">Order By</TD><TD style="WIDTH: 2px"><SELECT style="WIDTH: 208px" id="ddlOrderby" class="field_input" tabIndex=1 onchange="CallWebMethod('acctcode');" runat="server"> <OPTION value="agentcode" selected>Customer Code</OPTION><OPTION value="agentname">Customer Name</OPTION></SELECT></TD><TD style="WIDTH: 2px">
<asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" Text=" Go" CssClass="search_button" Width="54px"></asp:Button></TD><td>
        
    </td></TR><TR><TD style="WIDTH: 129px" class="field_input">Customer Code</TD><TD style="WIDTH: 2px"><SELECT style="WIDTH: 208px" id="ddlCustomercode" class="field_input" tabIndex=3 onchange="CallWebMethod('agentcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="field_input">Name</TD>
<TD><SELECT style="WIDTH: 208px" id="ddlCustomername" class="field_input" tabIndex=4 onchange="CallWebMethod('agentname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 129px" class="field_input">Category Code</TD>
<TD style="WIDTH: 2px"><SELECT style="WIDTH: 208px" id="ddlCategorycode" class="field_input" tabIndex=5 onchange="CallWebMethod('agentcatcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD>
<TD class="field_input">Name</TD>
<TD><SELECT style="WIDTH: 208px" id="ddlCategoryname" class="field_input" tabIndex=6 onchange="CallWebMethod('agentcatname');" runat="server"> <OPTION selected></OPTION></SELECT>
 <asp:Button id="BtnSearch" tabIndex=7 onclick="BtnSearch_Click" runat="server" 
        Text="Search" CssClass="search_button"></asp:Button>&nbsp;
 <asp:Button id="BtnClear" tabIndex=8 onclick="BtnClear_Click" runat="server" 
        Text="Clear" CssClass="search_button"></asp:Button>&nbsp;
 <asp:Button id="btnhelp" tabIndex=9 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="search_button" __designer:wfdid="w3"></asp:Button>&nbsp;
<asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Report" /></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="field_button" TabIndex="9"
                    Text="Export To Excel" /></td>
        </tr>

        <tr>
        <td style="width: 130px">
        <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell" Text="Save Changes in Pageindex Change" Width="341px" /></td>
    
        </tr>
        <tr>
            <td style="width: 100px; height: 11px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<DIV style="WIDTH: 940px; HEIGHT: 400px" class="container">
    <asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" Font-Size="10px" 
        BackColor="White" CssClass="td_cell" Width="911px" AutoGenerateColumns="False" 
        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        GridLines="Vertical" __designer:wfdid="w13" AllowPaging="True" 
        PageSize="9">
<FooterStyle CssClass="grdfooter" ForeColor="Black"></FooterStyle>
<Columns>
<asp:TemplateField><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
    </EditItemTemplate>
  <ItemTemplate>
    <INPUT id="chkSelect" type="checkbox" name="ChkSelect" runat="server" />
  </ItemTemplate>
 </asp:TemplateField>
<asp:BoundField DataField="agentcatcode" SortExpression=" " HeaderText="Category Code">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="agentcatname" SortExpression=" " HeaderText="Category Name">
<HeaderStyle Width="20%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="agentcode" HeaderText="Customer Code ">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="agentname" HeaderText="Customer   Name">
<HeaderStyle Width="25%"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Control A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("expensecode") %>' __designer:wfdid="w11"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="15%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 110px" id="ddlCostcode" class="field_input" runat="server"> <OPTION selected>[Select]</OPTION></SELECT>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control   A/C Name"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("acctname") %>' __designer:wfdid="w16"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="25%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 250px" id="ddlCostname" class="field_input" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

<RowStyle  CssClass="grdRowstyle" ForeColor="Black"></RowStyle>

<SelectedRowStyle BackColor="#008A8C" ForeColor="White" Font-Bold="True"></SelectedRowStyle>

<PagerStyle  CssClass="grdpagerstyle" ForeColor="Black" HorizontalAlign="Center"></PagerStyle>

<HeaderStyle  CssClass="grdheader" ForeColor="white" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle  CssClass="grdAternaterow" Font-Size="10px"></AlternatingRowStyle>
</asp:GridView></DIV><asp:Label id="lblMsg" runat="server" 
                            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="333px" Visible="False" 
                            __designer:wfdid="w14" CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>

         <tr>
    <td>
    <asp:Button ID="btnCopySelected" 
                    runat="server" CssClass="btn" 
                    Text="Copy Selected to all rows in current page"  />&nbsp;
                <asp:Button ID="btnSaveselected" runat="server" CssClass="btn" 
                     Text="Save selected to all rows in all pages" />&nbsp;
    </td>
    </tr>

        <tr>
            <td class="field_input" style="height: 8px; text-align: right">
                Fill Control A/C
                <select id="ddlControlacctcode" runat="server" class="field_input" onchange="CallWebMethod('othgrpcode');"
                    style="width: 208px" tabindex="11">
                    <option selected="selected"></option>
                </select>
                <asp:Button ID="BtnfillcontrolAC" runat="server" CssClass="field_button" TabIndex="12"
                    Text="Fill " />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="field_button" 
                    Font-Bold="True" TabIndex="13" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="field_button" Font-Bold="True" TabIndex="14"
                        Text="Exit" Width="60px" /></td>
        </tr>
    </table>
</asp:Content>

