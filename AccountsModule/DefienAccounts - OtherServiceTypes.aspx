<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="DefienAccounts - OtherServiceTypes.aspx.vb" Inherits="DefienAccounts___OtherServiceTypes" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type = "text/javascript" >

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
        
        
         case "othtypcode":
                var select=document.getElementById("<%=ddlOthertypecode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlothername.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "othtypname":
                var select=document.getElementById("<%=ddlothername.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlOthertypecode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        
        case "othgrpcode":
                var select=document.getElementById("<%=ddlOthergroupcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlOthergroupname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "othgrpname":
                var select=document.getElementById("<%=ddlOthergroupname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlOthergroupcode.ClientID%>");
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
    <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid;
        border-bottom: gray 2px solid">
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="WIDTH: 947px"><TBODY><TR><TD style="TEXT-ALIGN: center" class="field_heading" colSpan=5>Define Accounts -Other Service Types</TD></TR><TR><TD style="TEXT-ALIGN: center" class="td_cell" colSpan=4><SPAN style="COLOR: blue">Type few characters of code or name and click search </SPAN></TD></TR><TR><TD style="WIDTH: 129px; HEIGHT: 22px" class="td_cell">Order By</TD>
<TD style="WIDTH: 200px; HEIGHT: 22px">
    <SELECT style="WIDTH: 208px" id="ddlOrderby" class="drpdown" tabIndex=1 
        onchange="CallWebMethod('acctcode');" runat="server"> <OPTION value="Other Service Type Code" selected>Other Service Type Code</OPTION><OPTION value="Other Service Type Name">Other Service Type Name</OPTION></SELECT></TD>
        <TD style="WIDTH: 2px; HEIGHT: 22px"><asp:Button id="BtnGo" tabIndex=2 
                onclick="BtnGo_Click" runat="server" Text=" Go" CssClass="search_button"></asp:Button></TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">Other&nbsp;Service&nbsp;Type&nbsp;Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlOthertypecode" class="drpdown" tabIndex=3 
        onchange="CallWebMethod('othtypcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlothername" class="drpdown" tabIndex=4 
        onchange="CallWebMethod('othtypname');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;<asp:Button 
            id="BtnSearch" tabIndex=7 onclick="BtnSearch_Click" runat="server" 
            Text="Search" CssClass="search_button"></asp:Button>&nbsp;<asp:Button 
            id="btnClear" tabIndex=8 onclick="btnClear_Click" runat="server" Text="Clear" 
            CssClass="search_button"></asp:Button>&nbsp;<asp:Button id="btnhelp" 
            tabIndex=10 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="search_button"></asp:Button></TD><TD style="WIDTH: 2px"></TD></TR><TR><TD style="WIDTH: 129px" class="td_cell">Other&nbsp;Service&nbsp;Group&nbsp;Code</TD><TD style="WIDTH: 2px">
    <SELECT style="WIDTH: 208px" id="ddlOthergroupcode" class="drpdown" tabIndex=5 
        onchange="CallWebMethod('othgrpcode');" runat="server"> <OPTION selected></OPTION></SELECT></TD><TD class="td_cell">Name</TD><TD>
    <SELECT style="WIDTH: 208px" id="ddlOthergroupname" class="drpdown" tabIndex=6 
        onchange="CallWebMethod('othgrpname');" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;</TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td style="width: 100px; height: 13px">
                <table style="width: 462px">
                    <tr>
                        <td style="width: 162px; height: 21px">
                <asp:Button ID="BtnExportToExcel" runat="server" CssClass="btn" TabIndex="9"
                    Text="Export To Excel" /></td>
                        <td style="width: 427px; height: 21px">
                            <asp:CheckBox ID="chkSaveChanges" runat="server" Checked="True" CssClass="td_cell"
                                Text="Save Changes in Pageindex Chage" Width="239px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100px; height: 51px">
                <asp:UpdatePanel id="UpdatePanel2" runat="server">
                    <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 100px">
    <div style="overflow: auto; width: 950px; height: 500px">
        <asp:GridView id="gv_SearchResult" tabIndex=10 runat="server" 
            CssClass="grdstyle" Width="920px" AutoGenerateColumns="False" CellPadding="3" 
            GridLines="Vertical" AllowPaging="True" 
            OnPageIndexChanging="gv_SearchResult_PageIndexChanging" PageSize="9">
<Columns>
<asp:TemplateField><EditItemTemplate>
    <asp:TextBox id="TextBox1" runat="server" CssClass="field_input"></asp:TextBox> 
    </EditItemTemplate>
  <ItemTemplate>
    <INPUT id="ChkSelect" type="checkbox" name="ChkSelect" runat="server" />
  </ItemTemplate>
 </asp:TemplateField>
<asp:BoundField DataField="othtypcode" HtmlEncode="False" HeaderText="Other Service Type  Code "></asp:BoundField>
<asp:BoundField DataField="othtypname" HeaderText="Other Service Type Name"></asp:BoundField>
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
        <SELECT style="WIDTH: 75px" id="ddlRefIncomecode" class="drpdown" runat="server"> 
            <OPTION selected>[Select]</OPTION>
        </SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Income Name">
    <ItemTemplate>
        <SELECT style="WIDTH: 185px" id="ddlRefIncomename" class="drpdown" runat="server"> 
            <OPTION selected>[Select]</OPTION>
        </SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Code">    
    <ItemTemplate>
        <SELECT style="WIDTH: 75px" id="ddlRefCostcode" class="drpdown" runat="server"> 
            <OPTION selected>[Select]</OPTION>
        </SELECT> 
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Refund Cost Name">
    <ItemTemplate>
        <SELECT style="WIDTH: 185px" id="ddlRefCostname" class="drpdown" runat="server"> 
            <OPTION selected>[Select]</OPTION>
        </SELECT> 
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
        Font-Names="Verdana" Font-Bold="True" Width="333px" 
        Visible="False" CssClass="lblmsg"></asp:Label></TD></TR><TR><TD style="WIDTH: 100px"></TD></TR></TBODY></TABLE>
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
            <td style="height: 8px; text-align: right">
                <asp:Button ID="BtnOtherServicegrp" runat="server" CssClass="btn" TabIndex="11"
                    Text="Fill From Other Service Group" />&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Font-Bold="True" 
                    TabIndex="12" Text="Save" />&nbsp;<asp:Button ID="btnExit"
                        runat="server" CssClass="btn" Font-Bold="False" TabIndex="13"
                        Text="Exit" /></td>
        </tr>
    </table>
</asp:Content>

