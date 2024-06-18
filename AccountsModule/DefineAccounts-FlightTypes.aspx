<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefineAccounts-FlightTypes.aspx.vb" Inherits="DefineAccounts_FlightTypes" %>

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
        
        case "airlinecode":
                var select=document.getElementById("<%=ddlAirlinecode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlAirlinename.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "partyname":
                var select=document.getElementById("<%=ddlAirlinename.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlAirlinecode.ClientID%>");
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
<TABLE style="WIDTH: 947px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=5>Define Accounts -Flight Types</TD></TR><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=4><SPAN style="COLOR:blue ">Type few characters of code or name and click search </SPAN></TD></TR>
<TR><TD style="WIDTH: 127px; HEIGHT: 6px" class="td_cell">Order By</TD><TD style="WIDTH: 5px; HEIGHT: 6px">
    <select style="WIDTH: 244px" id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <option value="Flight Code" selected>Flight Code</option><option value="Airline Code">Airline Code</option></select></TD><TD style="WIDTH: 2px; HEIGHT: 6px">
        <asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" 
            Text=" Go" CssClass="search_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 127px" class="td_cell">Airline Code</TD><TD>
    <select style="WIDTH: 244px" id="ddlAirlinecode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('airlinecode');" runat="server"> <option selected></option></select></TD><TD class="td_cell">Name</TD><TD>
    <select style="WIDTH: 244px" id="ddlAirlinename" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('partyname');" runat="server"> <option selected></option></select> <asp:Button id="btnSearch" tabIndex=5 onclick="btnSearch_Click" runat="server" Text=" Search" CssClass="search_button"></asp:Button>&nbsp;
         <asp:Button id="btnclear" tabIndex=6 onclick="btnclear_Click" runat="server" Text="Clear" CssClass="search_button"></asp:Button>&nbsp;
        <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="search_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
                <table style="width: 468px">
                    <tr>
                        <td style="width: 73px">
                            <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn"
                    TabIndex="7" Text="Export To Excel" /></td>
                        <td style="width: 267px">
                            <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell"
                                Text="Save Changes in Pageindex Chage" Width="239px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
                        <div style="overflow: auto; width: 950px; height: 500px">
<asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" CssClass="grdstyle" Width="950px" AllowSorting="True" AutoGenerateColumns="False"  CellPadding="3" GridLines="Vertical" AllowPaging="True">
<Columns>
<asp:BoundField DataField="airlinecode" HeaderText=" Airline Code"></asp:BoundField>
<asp:BoundField DataField="partyname" HeaderText=" Airline Name"></asp:BoundField>
<asp:BoundField DataField="flightcode" HeaderText="Flight Code"></asp:BoundField>
<asp:TemplateField HeaderText="Income Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("incomecode") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<select style="WIDTH: 131px" id="ddlIncomecode" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Income Account Name"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<select style="WIDTH: 131px" id="ddlIncomename" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("expensecode") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<select style="WIDTH: 131px" id="ddlCostcode" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Name"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<select style="WIDTH: 131px" id="ddlCostname" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Refund Income Code">
    <ItemTemplate>
        <select style="WIDTH: 131px" id="ddlRefIncomecode" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Income Name">
    <ItemTemplate>
        <select style="WIDTH: 131px" id="ddlRefIncomename" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Code">
    <ItemTemplate>
        <select style="WIDTH: 131px" id="ddlRefCostcode" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Name">
    <ItemTemplate>
        <select style="WIDTH: 131px" id="ddlRefCostname" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
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

                        </div>
                        <asp:Label id="lblMsg" runat="server" 
                            Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
                            Font-Names="Verdana" Font-Bold="True" Width="333px" Visible="False" 
                            CssClass="lblmsg"></asp:Label> 
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td style="height: 8px; text-align: right">
                <asp:Button ID="BtnProfitcenter" runat="server" CssClass="btn" TabIndex="9"
                    Text=" Fill from Profit Center Master" />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="True" 
                    TabIndex="10" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="btn" Font-Bold="True" TabIndex="11"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

