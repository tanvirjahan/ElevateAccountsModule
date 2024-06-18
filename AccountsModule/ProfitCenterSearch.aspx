<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="ProfitCenterSearch.aspx.vb" Inherits="ProfitCenterSearch" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type ="text/javascript" >
function CallWebMethod(methodType)
{
    switch(methodType)
    {
        case "acctcode":
            var select=document.getElementById("<%=ddlIncomecode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlIncomename.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "acctname":
            var select=document.getElementById("<%=ddlIncomename.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlIncomecode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;        
        case "costcode":
            var select=document.getElementById("<%=ddlCostcode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlCostname.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "costname":
            var select=document.getElementById("<%=ddlCostname.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlCostcode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refincomecode":
            var select=document.getElementById("<%=ddlRefIncomecode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlRefIncomename.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refincomename":
            var select=document.getElementById("<%=ddlRefIncomename.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlRefIncomecode.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;        
        case "refcostcode":
            var select=document.getElementById("<%=ddlRefCostcode.ClientID%>");
            var codeid=select.options[select.selectedIndex].text;
            var selectname=document.getElementById("<%=ddlRefCostname.ClientID%>");
            selectname.value=select.options[select.selectedIndex].text;
            break;
        case "refcostname":
            var select=document.getElementById("<%=ddlRefCostname.ClientID%>");
            var codeid=select.options[select.selectedIndex].value;
            var selectname=document.getElementById("<%=ddlRefCostcode.ClientID%>");
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

    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid">
        <tr>
            <td align="center" class="field_heading">
                Profit Center List</td>
        </tr>
        <tr>
            <td  align="center">
                <span class="td_cell" style="color: blue">Type few characters of code or name and
                    click search </span>
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD class="td_cell" align=center colSpan=4>&nbsp;<asp:RadioButton id="rbsearch" runat="server" Text="Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" Checked="True" GroupName="GrSearch" OnCheckedChanged="rbsearch_CheckedChanged"></asp:RadioButton> <asp:RadioButton id="rbtnadsearch" runat="server" Text="Advance Search" ForeColor="Black" CssClass="td_cell" AutoPostBack="True" GroupName="GrSearch" OnCheckedChanged="rbtnadsearch_CheckedChanged"></asp:RadioButton>
 <asp:Button id="BtnSearch" tabIndex=7 onclick="BtnSearch_Click" runat="server" 
        Text="Search" CssClass="search_button"></asp:Button>&nbsp;
  <asp:Button id="BtnClear" tabIndex=8 onclick="BtnClear_Click" runat="server" 
        Text="Clear" CssClass="search_button"></asp:Button>&nbsp;
<asp:Button id="btnhelp" tabIndex=12 onclick="btnhelp_Click" runat="server" 
        Text="Help" CssClass="search_button"></asp:Button> 
  &nbsp;  <asp:Button id="BtnAddNew" tabIndex=9 runat="server" Text="Add New" 
        CssClass="btn"></asp:Button>&nbsp; 
    <asp:Button id="BtnPrint" 
        tabIndex=11 runat="server" Text="Report" CssClass="btn" 
        OnClick="BtnPrint_Click"></asp:Button></TD></TR><TR><TD class="td_cell" style="width: 108px"><asp:Label id="Label1" runat="server" Text="Service Category" Width="105px"></asp:Label></TD><TD>
        <INPUT style="WIDTH: 194px" id="txtServicecode" class="txtbox" tabIndex=1 
            type=text maxLength=100 runat="server" /></TD><TD class="td_cell"><asp:Label id="Label2" runat="server" Text="Display Name" Width="105px"></asp:Label></TD><TD>
        <INPUT style="WIDTH: 294px" id="txtDisplayname" class="txtbox" tabIndex=2 
            type=text maxLength=100 runat="server" /></TD></TR><TR><TD class="td_cell" style="width: 148px"><asp:Label id="lblInccode" runat="server" Text="Income Code" Width="100px"></asp:Label></TD><TD>
        <SELECT style="WIDTH: 200px" id="ddlIncomecode" class="drpdown" tabIndex=3 
            onchange="CallWebMethod('acctcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell"><asp:Label id="lblIncname" runat="server" Text="Income Name" Width="100px"></asp:Label></TD><TD>
        <SELECT style="WIDTH: 300px" id="ddlIncomename" class="drpdown" tabIndex=4 
            onchange="CallWebMethod('acctname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD class="td_cell" style="width: 168px"><asp:Label id="lblCostcode" runat="server" Text="Cost of Sale Code" Width="160px"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 200px" id="ddlCostcode" class="drpdown" tabIndex=5 
        onchange="CallWebMethod('costcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell"><asp:Label id="lblCostname" runat="server" Text="Cost of Sale Name" Width="155px"></asp:Label></TD><TD>
    <SELECT style="WIDTH: 300px" id="ddlCostname" class="drpdown" tabIndex=6 
        onchange="CallWebMethod('costname');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
    <tr>
        <td class="td_cell" style="width: 160px">
            <asp:Label ID="lblRefInccode" runat="server" Text="Refund Income Code" Width="160px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 200px" id="ddlRefIncomecode" class="drpdown" tabIndex=3 
                onchange="CallWebMethod('refincomecode');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
        <td class="td_cell">
            <asp:Label ID="lblRefIncname" runat="server" Text="Refund Income Name" Width="160px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 300px" id="ddlRefIncomename" class="drpdown" tabIndex=4 
                onchange="CallWebMethod('refincomename');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
    </tr>
    <tr>
        <td class="td_cell" style="width: 208px">
            <asp:Label ID="lblRefCostcode" runat="server" Text="Refund Cost Code" Width="160px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 200px" id="ddlRefCostcode" class="drpdown" tabIndex=5 
                onchange="CallWebMethod('refcostcode');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
        <td class="td_cell">
            <asp:Label ID="lblRefCostname" runat="server" Text="Refund Cost Name" Width="160px"></asp:Label></td>
        <td>
            <SELECT style="WIDTH: 300px" id="ddlRefCostname" class="drpdown" tabIndex=6 
                onchange="CallWebMethod('refcostname');" runat="server">
                <OPTION selected></option>
            </select>
        </td>
    </tr>
</TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn"
                        Text="Export To Excel" TabIndex="10" />
                </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<asp:GridView id="gv_SearchResult" OnPageIndexChanging="gv_SearchResult_PageIndexChanging" tabIndex=12 runat="server" Width="900px" CssClass="grdstyle" GridLines="Vertical" CellPadding="3" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True">
<Columns>
<asp:TemplateField Visible="False" HeaderText="Service Category"><EditItemTemplate>
<asp:TextBox runat="server" Text='<%# Bind("servicecat") %>' id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblCode" runat="server" Text='<%# Bind("servicecat") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="servicecat" SortExpression="servicecat" HeaderText="Service Category"></asp:BoundField>
<asp:BoundField DataField="dispname" SortExpression="dispname" HeaderText="Display Name"></asp:BoundField>
<asp:BoundField DataField="incomecode" SortExpression="incomecode" HeaderText="Income Code"></asp:BoundField>
<asp:BoundField DataField="costcode" SortExpression="costcode" HeaderText="Cost of Sale Code"></asp:BoundField>
<asp:BoundField DataField="refundincomecode" SortExpression="refundincomecode" HeaderText="Refund Income Code"></asp:BoundField>
<asp:BoundField DataField="refundcostcode" SortExpression="refundcostcode" HeaderText="Refund Cost Code"></asp:BoundField>
<asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="adddate" SortExpression="adddate" HeaderText="Date Created"></asp:BoundField>
<asp:BoundField DataField="adduser" SortExpression="adduser" HeaderText="User Created"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt} " DataField="moddate" SortExpression="moddate" HeaderText="Date Modified"></asp:BoundField>
<asp:BoundField DataField="moduser" SortExpression="moduser" HeaderText="User Modified"></asp:BoundField>
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

 <FooterStyle CssClass="grdfooter" />

<RowStyle CssClass="grdRowstyle"></RowStyle>
<SelectedRowStyle CssClass="grdselectrowstyle" ></SelectedRowStyle>
<PagerStyle CssClass="grdpagerstyle" HorizontalAlign="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader" ForeColor="white"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView> <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="333px" 
                            CssClass="lblmsg" Visible="False"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>

