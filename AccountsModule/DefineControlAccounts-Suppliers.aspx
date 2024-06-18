<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefineControlAccounts-Suppliers.aspx.vb" Inherits="DefineControlAccounts_Suppliers" %>

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
        
        case "partycode":
                var select=document.getElementById("<%=ddlSuppleircode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlSuppleirname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        case "partyname":
                var select=document.getElementById("<%=ddlSuppleirname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlSuppleircode.ClientID%>");
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
       case "catcode":
                var select=document.getElementById("<%=ddlCategorycode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlCategoryname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
       case "catname":
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
<TABLE style="WIDTH: 947px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=5>Define Control Accounts -Suppliers</TD></TR><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=4><SPAN style="COLOR: blue">Type few characters of code or name and click search </SPAN></TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">Order By</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <OPTION value="partycode" selected>Supplier Code</OPTION><OPTION value=" partyname">Supplier Name</OPTION></SELECT></TD><TD style="WIDTH: 2px">
    <asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" 
        Text=" Go" CssClass="search_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">Supplier&nbsp; Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlSuppleircode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('partycode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlSuppleirname" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('partyname');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp; </TD><TD style="WIDTH: 2px"></TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">SP Type Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlSptypecode" class="drpdown" tabIndex=5 
        onchange="CallWebMethod('sptypecode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlSptypename" class="drpdown" tabIndex=6 
        onchange="CallWebMethod('sptypename');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;</TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">Category</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlCategorycode" class="drpdown" tabIndex=7 
        onchange="CallWebMethod('catcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlCategoryname" class="drpdown" tabIndex=8 
        onchange="CallWebMethod('catname');" runat="server"> <OPTION selected></OPTION></SELECT>
         <asp:Button id="BtnSearch" tabIndex=9 onclick="BtnSearch_Click" 
        runat="server" Text="Search" CssClass="search_button"></asp:Button>&nbsp;
          <asp:Button id="BtnClear" tabIndex=10 onclick="BtnClear_Click" 
        runat="server" Text="Clear" CssClass="search_button"></asp:Button>&nbsp;
          <asp:Button id="btnhelp" tabIndex=12 onclick="btnhelp_Click" 
        runat="server" Text="Help" CssClass="search_button" __designer:wfdid="w2"></asp:Button> &nbsp;
    <asp:Button id="btnPrint" tabIndex=10 runat="server" Text="Report" 
        __designer:dtid="5629499534213128" CssClass="btn" 
        __designer:wfdid="w4"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" TabIndex="11"
                    Text="Export To Excel" /></td>
        </tr>
        <tr>
        <td style="width: 130px">
        <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell" Text="Save Changes in Pageindex Change" Width="341px" /></td>
    
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<DIV style="WIDTH: 940px; HEIGHT: 400px" class="container">
    <asp:GridView id="gv_SearchResult" tabIndex=12 runat="server" 
        CssClass="grdstyle"  Width="917px" GridLines="Vertical" CellPadding="3" 
        AutoGenerateColumns="False" AllowPaging="True" PageSize="9">
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
<HeaderStyle Width="20%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="partycode" HeaderText="Supplier Code ">
<HeaderStyle Width="15%"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="partyname" HeaderText=" Supplier  Name">
<HeaderStyle Width="25%"></HeaderStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Accrual A/C Code"><EditItemTemplate>
<asp:TextBox id="TextBox1" runat="server" Text='<%# Bind("incomecode") %>' __designer:wfdid="w4"></asp:TextBox> 
</EditItemTemplate>

<HeaderStyle Width="15%"></HeaderStyle>
<ItemTemplate>
<SELECT style="WIDTH: 75px" id="ddlIncomecode" class="field_input" runat="server"> <OPTION selected>[Select]</OPTION></SELECT>
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
<PagerStyle CssClass="grdpagerstyle"  HorizontalAlign ="Center"></PagerStyle>
<HeaderStyle  CssClass="grdheader"></HeaderStyle>
<AlternatingRowStyle CssClass="grdAternaterow"></AlternatingRowStyle>
</asp:GridView>
</DIV>
</contenttemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Names="Verdana"
                    Font-Size="8pt" Text="Records not found, Please redefine search criteria"
                    Visible="False" Width="333px" CssClass="lblmsg"></asp:Label></td>
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
            <td class="td_cell" style="height: 8px; text-align: right">
                <asp:Label ID="Label3" runat="server" Text="Select A/C Code" Width="95px"></asp:Label>&nbsp;
                <select id="ddlControlacctcode" runat="server" class="drpdown" onchange="CallWebMethod('othgrpcode');"
                    style="width: 208px" tabindex="13">
                    <option selected="selected"></option>
                </select>
                <asp:Label ID="Label6" runat="server" Text="Select  A/C Type" Width="103px"></asp:Label>
                <select id="ddlAcType" runat="server" class="drpdown" name="" onchange="CallWebMethod('othgrpcode');"
                    style="width: 208px" tabindex="14">
                    <option selected="selected" value="[Select]">[Select]</option>
                    <option value="Accrual">Accrual</option>
                    <option value="Control">Control </option>
                </select>
                &nbsp;
                <asp:Button ID="BtnfillcontrolAC" runat="server" CssClass="btn" TabIndex="15"
                    Text="Fill " />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="True" 
                    TabIndex="16" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="btn" Font-Bold="True" TabIndex="17"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

