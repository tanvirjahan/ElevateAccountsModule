<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefineControlAccounts-SupplierAgents.aspx.vb" Inherits="DefineControlAccounts_SupplierAgents" %>

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
        
        case "supagentcode":
                var select=document.getElementById("<%=ddlSuppleiAgentcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSuppleiAgentname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        case "supagentname":
                var select=document.getElementById("<%=ddlSuppleiAgentname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSuppleiAgentcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
                       
       case "sptypecode":
                var select=document.getElementById("<%=ddlSptypecode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSptypename.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       case "sptypename":
                var select=document.getElementById("<%=ddlSptypename.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSptypecode.ClientID%>");
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
            <td align="center" class="field_heading">
                Define Control Accounts -Supplier Agents</td>
        </tr>
        <tr>
            <td align="center" class="td_cell" style="height: 16px">
                <span style="color: blue">Type few characters of code or name and click search </span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD class="td_cell"><asp:Label id="Label1" runat="server" Text="Order By" Width="110px" __designer:wfdid="w26"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 200px" id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <OPTION value="supagentcode" selected>Supplier Agent Code</OPTION><OPTION value="supagentname">Supplier Agent Name</OPTION></SELECT></TD><TD>
    <asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" 
        Text=" Go" CssClass="search_button" __designer:wfdid="w27"></asp:Button></TD></TR><TR><TD class="td_cell">
    <asp:Label id="Label2" runat="server" Text="Supplier Agent Code" Width="150px" 
        __designer:wfdid="w28"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 200px" id="ddlSuppleiAgentcode" class="drpdown" 
        tabIndex=3 onchange="CallWebMethod('supagentcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">
    <asp:Label id="Label4" runat="server" Text="Supplier Agent Name" Width="150px" 
        __designer:wfdid="w29"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 300px" id="ddlSuppleiAgentname" class="drpdown" 
        tabIndex=4 onchange="CallWebMethod('supagentname');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp; </TD><TD>
        <asp:Button id="BtnSearch" tabIndex=7 onclick="BtnSearch_Click" runat="server" 
            Text="Search" CssClass="search_button" __designer:wfdid="w30"></asp:Button></TD><TD style="WIDTH: 51px">
            <asp:Button id="BtnClear" tabIndex=8 onclick="BtnClear_Click" runat="server" 
                Text="Clear" CssClass="search_button" __designer:wfdid="w31"></asp:Button> </TD><TD style="WIDTH: 51px">
            <asp:Button id="btnhelp" tabIndex=11 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="search_button" __designer:wfdid="w1"></asp:Button></TD></TR><TR><TD class="td_cell"><asp:Label id="Label25" runat="server" Text="SP Type Code" Width="110px" __designer:wfdid="w32"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 200px" id="ddlSptypecode" class="drpdown" tabIndex=5 
        onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell"><asp:Label id="Label5" runat="server" Text="SP Type Name" Width="110px" __designer:wfdid="w33"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 300px" id="ddlSptypename" class="drpdown" tabIndex=6 
        onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD></TD><TD style="WIDTH: 51px"></TD><TD style="WIDTH: 51px"></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" TabIndex="9"
                    Text="Export To Excel" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="btn" TabIndex="10" 
                    Text="Report" /></td>
                    
        </tr>
        <tr>
        <td style="width: 130px">
        <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell" Text="Save Changes in Pageindex Change" Width="341px" /></td>
    
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<DIV style="WIDTH: 940px; HEIGHT: 400px" class="container">
    <asp:GridView id="gv_SearchResult" tabIndex="11" runat="server" 
        CssClass="grdstyle" Width="917px" __designer:wfdid="w7" GridLines="Vertical" 
        CellPadding="3"  AutoGenerateColumns="False" PageSize="9">
<Columns>
<asp:TemplateField><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
    </EditItemTemplate>
  <ItemTemplate>
    <INPUT id="chkSelect" type="checkbox" name="ChkSelect" runat="server" />
  </ItemTemplate>
 </asp:TemplateField>
<asp:BoundField DataField="sptypecode" SortExpression=" " HeaderText="SPType Code">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="sptypename" SortExpression=" " HeaderText="SPType Name">
<HeaderStyle Width="25%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="supagentcode" HeaderText="SupplierAgent Code ">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="supagentname" HeaderText=" Supplier Agent Name">
<HeaderStyle Width="25%"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Accrual A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("incomecode") %>' __designer:wfdid="w4"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="15%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 75px" id="ddlIncomecode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Accrual A/C  Name"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="25%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 180px" id="ddlIncomename" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("expensecode") %>' __designer:wfdid="w5"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="15%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 75px" id="ddlCostcode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Control   A/C Name"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="25%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 180px" id="ddlCostname" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
</Columns>

 <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>
</DIV><asp:Label id="lblMsg" runat="server" Text="Records not found, Please redefine search criteria" 
                            Font-Size="8pt" Font-Names="Verdana" Font-Bold="True" CssClass="lblmsg" 
                            Width="333px" __designer:wfdid="w8" Visible="False"></asp:Label> 
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
            <td class="td_cell" align="right">
                <asp:Label ID="Label3" runat="server" Text="Select A/C Code" Width="113px" 
                    style="margin-left: 1px"></asp:Label>
                <select id="ddlControlacctcode" runat="server" class="drpdown" onchange="CallWebMethod('othgrpcode');"
                    style="width: 208px" tabindex="12">
                    <option selected="selected"></option>
                </select>
                <asp:Label ID="Label6" runat="server" Text="Select  A/C Type" Width="112px"></asp:Label>
                &nbsp;<select id="ddlAcType" runat="server" class="drpdown" onchange="CallWebMethod('othgrpcode');"
                    style="width: 208px" tabindex="13" name="">
                    <option selected="selected" value="[Select]">[Select]</option>
                    <option value="Accrual">Accrual</option>
                    <option value="Control">Control </option>
                </select>
                <asp:Button ID="BtnfillcontrolAC" runat="server" CssClass="btn" TabIndex="14"
                    Text="Fill " />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="True" 
                    TabIndex="15" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="btn" Font-Bold="True" TabIndex="16"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

