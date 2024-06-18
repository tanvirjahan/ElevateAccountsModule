<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefieneAccountsRoomTypes.aspx.vb" Inherits="DefieneAccountsRoomTypes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type="text/javascript" >

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
        
        case "rmtypcode":
                var select=document.getElementById("<%=ddlRoomcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlRoomname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "rmtypname":
                var select=document.getElementById("<%=ddlRoomname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlRoomcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "sptypecode":
                var select=document.getElementById("<%=ddlSpcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSPname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
          case "sptypename":
                var select=document.getElementById("<%=ddlSPname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSpcode.ClientID%>");
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
<TABLE style="WIDTH: 947px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=5>Define Accounts - Room Types</TD></TR><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=4><SPAN style="COLOR: blue">Type few characters of code or name and click search </SPAN></TD></TR><TR><TD style="WIDTH: 83px" class="td_cell">Order By</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <OPTION selected>[Select]</OPTION><OPTION value="Room/Service Type Code">Room/Service Type Code</OPTION><OPTION value="Room/Service Type Name">Room/Service Type Name</OPTION></SELECT></TD><TD style="WIDTH: 2px">
    <asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" 
        Text=" Go" CssClass="search_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 83px" class="td_cell">Room&nbsp;Type&nbsp;Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlRoomcode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('rmtypcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlRoomname" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('rmtypname');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;<asp:Button 
            id="BtnSearch" tabIndex=7 onclick="BtnSearch_Click" runat="server" 
            Text="Search" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
            id="btnClear" tabIndex=8 onclick="btnClear_Click" runat="server" Text="Clear" 
            CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnhelp" 
            tabIndex=10 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 2px"></TD></TR><TR><TD style="WIDTH: 83px" class="td_cell">SP Type Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlSpcode" class="drpdown" tabIndex=5 
        onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD style="WIDTH: 100px">
    <SELECT style="WIDTH: 208px" id="ddlSPname" class="drpdown" tabIndex=6 
        onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
<table>
    <tr>
    <td style="width: 207px">
        <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" 
            Text="Export To Excel" TabIndex="9" /></td>
    <td style="width: 130px">
        <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell" Text="Save Changes in Pageindex Chage" Width="341px" /></td>
    </tr>
</table>
            </td>
        </tr>
        <tr>
            <td style="width: 100px; height: 114px;">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 100px">
 <div style="overflow: auto; width: 950px; height: 500px">
        <asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" 
            CssClass="grdstyle" Width="920px"
         GridLines="Vertical" CellPadding="3" AutoGenerateColumns="False" 
            AllowSorting="True" AllowPaging="True" 
         OnPageIndexChanging="gv_SearchResult_PageIndexChanging" PageSize="9">
<Columns>
<asp:TemplateField><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
    </EditItemTemplate>
  <ItemTemplate>
    <INPUT id="ChkSelect" type="checkbox" name="ChkSelect" runat="server" />
  </ItemTemplate>
 </asp:TemplateField>
<asp:BoundField DataField="sptypecode" HeaderText="SPType Code"></asp:BoundField>
<asp:BoundField DataField="sptypename" HeaderText="SPType Name"></asp:BoundField>
<asp:BoundField DataField="rmtypcode" HeaderText="Room Type Code"></asp:BoundField>
<asp:BoundField DataField="rmtypname" HeaderText="Room Type Name"></asp:BoundField>
<asp:TemplateField HeaderText="Income Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("incomecode") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 75px" id="ddlIncomecode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Income Account Name"><EditItemTemplate>
<asp:TextBox id="TextBox2" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 185px" id="ddlIncomename" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox3" runat="server" Text='<%# Bind("expensecode") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 75px" id="ddlCostcode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Name"><EditItemTemplate>
<asp:TextBox id="TextBox4" runat="server" Text='<%# Bind("acctname") %>'></asp:TextBox> 
</EditItemTemplate>
<ItemTemplate>
<SELECT style="WIDTH: 185px" id="ddlCostname" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Refund Income Code">
    <ItemTemplate>
        <SELECT style="WIDTH: 75px" id="ddlRefIncomecode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Income Account Name">
    <ItemTemplate>
        <SELECT style="WIDTH: 185px" id="ddlRefIncomename" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Code">
    <ItemTemplate>
        <SELECT style="WIDTH: 75px" id="ddlRefCostcode" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Name">
    <ItemTemplate>
        <SELECT style="WIDTH: 185px" id="ddlRefCostname" class="drpdown" runat="server"> <OPTION selected>[Select]</OPTION></SELECT> 
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
    </div></TD></TR>
    <tr>
    <td>
     <asp:Label id="lblMsg" runat="server" 
        Text="Records not found, Please redefine search criteria" Font-Size="8pt" 
        Font-Names="Verdana" Font-Bold="True" Width="333px" Visible="False" 
        CssClass="lblmsg"></asp:Label>
    </td>
    </tr>
   
        
        </TBODY></TABLE>
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
            <td style="text-align: right; height: 8px;">
                <asp:Button ID="BtnProfitcenter" 
                    runat="server" CssClass="btn"
                    Text=" Fill from Profit Center Master" TabIndex="11" />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="False" 
                    TabIndex="12" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="btn" Font-Bold="False" TabIndex="13"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

