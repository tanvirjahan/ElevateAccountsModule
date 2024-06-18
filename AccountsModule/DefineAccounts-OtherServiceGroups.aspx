<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefineAccounts-OtherServiceGroups.aspx.vb" Inherits="DefineAccounts_OtherServiceGroups"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
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
        
        case "othgrpcode":
                var select=document.getElementById("<%=ddlOthergrpcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlOthergrpname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "othgrpname":
                var select=document.getElementById("<%=ddlOthergrpname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlOthergrpcode.ClientID%>");
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
                Define Accounts -Other Service Groups
            </td>
        </tr>
        <tr>
            <td align="center" class="td_cell">
                <span style="color: blue">Type few characters of code or name and click search </span>
            </td>
        </tr>
        <tr>
            <td  >
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                <contenttemplate>
<TABLE><TBODY><TR><TD class="td_cell">Order By</TD><TD><select style="WIDTH: 244px" 
        id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <option value="Other Service Group Code" selected>Other Service Group Code</option><option value="Other Service Group Name">Other Service Group Name</option></select></TD><TD>
    <asp:Button id="BtnGo" tabIndex=2 onclick="BtnGo_Click" runat="server" 
        Text=" Go" CssClass="search_button"></asp:Button></TD><TD></TD><TD style="WIDTH: 64px"></TD><TD style="WIDTH: 65px"></TD><TD style="WIDTH: 65px"></TD></TR><TR><TD class="td_cell"><asp:Label id="Label1" runat="server" Text="Other Service group Code" CssClass="field_caption" Width="128px"></asp:Label></TD><TD>
    <select style="WIDTH: 244px" id="ddlOthergrpcode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('othgrpcode');" runat="server"> <option selected></option></select></TD><TD class="td_cell">Name</TD><TD>
    <select style="WIDTH: 244px" id="ddlOthergrpname" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('othgrpname');" runat="server"> <option selected></option></select>&nbsp;&nbsp;</TD><TD style="WIDTH: 64px">
    <asp:Button id="btnSearch" tabIndex=5 onclick="btnSearch_Click" runat="server" 
        Text=" Search" Height="20px" CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 65px">
        <asp:Button id="btnClear" tabIndex=6 onclick="btnClear_Click" runat="server" 
            Text="Clear" CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 65px">
        <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
            Text="Help" CssClass="search_button"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td   >
                <table>
                    <tr>
                        <td style="width: 140px">
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn"
                    TabIndex="7" Text="Export To Excel" /></td>
                        <td style="width: 388px">
                            <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell"
                                Text="Save Changes in Pageindex Chage" Width="341px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 565px" >
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD>
    <div style="overflow: auto; width: 950px; height: 500px">
        <asp:GridView id="gv_SearchResult" tabIndex=8 runat="server" 
            CssClass="grdstyle" Width="920px" AutoGenerateColumns="False" CellPadding="3" 
            GridLines="Vertical" OnPageIndexChanging="gv_SearchResult_PageIndexChanging" 
            AllowPaging="True" PageSize="9">
<Columns>
<asp:TemplateField><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
    </EditItemTemplate>
  <ItemTemplate>
    <INPUT id="ChkSelect" type="checkbox" name="ChkSelect" runat="server" />
  </ItemTemplate>
 </asp:TemplateField>
<asp:BoundField DataField="othgrpcode" HeaderText="Other Service Group Code "></asp:BoundField>
<asp:BoundField DataField="othgrpname" HeaderText="Other Service Group Name"></asp:BoundField>
<asp:TemplateField HeaderText="Income Code">
    <ItemTemplate>
        <select style="WIDTH:75px" id="ddlIncomecode" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Income Account Name">
    <ItemTemplate>
        <select style="WIDTH: 185px" id="ddlIncomename" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Code">
    <ItemTemplate>
        <select style="WIDTH: 75px" id="ddlCostcode" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cost of Sales  A/C Name">
    <ItemTemplate>
        <select style="WIDTH: 185px" id="ddlCostname" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>


<asp:TemplateField HeaderText="Refund Income Code">
    <ItemTemplate>
        <select style="WIDTH: 75px" id="ddlRefIncomecode" class="drpdown" runat="server">
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Income Name">
    <ItemTemplate>
        <select style="WIDTH: 185px" id="ddlRefIncomename" class="drpdown" runat="server"> 
            <option selected>[Select]</option>
        </select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Code">
    <ItemTemplate>
        <select style="WIDTH: 75px" id="ddlRefCostcode" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Name">
    <ItemTemplate>
        <select style="WIDTH: 185px" id="ddlRefCostname" class="drpdown" runat="server"> <option selected>[Select]</option></select> 
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
        CssClass="lblmsg"></asp:Label></TD></TR></TBODY></TABLE>
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
            <td align="right">
                <asp:Button ID="BtnProfitcenter" runat="server" CssClass="btn" TabIndex="9"
                    Text=" Fill from Profit Center Master" />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="True" 
                    TabIndex="10" Text="Save" />&nbsp;
                    <asp:Button ID="btnExit" runat="server" CssClass="btn" Font-Bold="True" TabIndex="11"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

