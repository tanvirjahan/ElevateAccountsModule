<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustSubUser.aspx.vb" Inherits="CustSubUser"  %>

<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


<script language="javascript" type="text/javascript" >
        
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
			
		
function checkNumber(e)
{
    if ( event.keyCode < 45 || event.keyCode > 57 )
    {
    return false;
    }
}


</script>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" Width="882px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD style="WIDTH: 85%" vAlign=top><DIV style="WIDTH: 100%" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelGeneral" runat="server" Width="600px" GroupingText="Sub User"><TABLE style="WIDTH: 414px"><TBODY><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><asp:GridView id="Gv_subuser" runat="server" Width="410px" __designer:wfdid="w12" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="agent_sub_code" HeaderText="Sub User Code "></asp:BoundField>
<asp:BoundField DataField="sub_user_name" HeaderText="Sub User Name"></asp:BoundField>
<asp:BoundField DataField="sub_user_email" HeaderText="User Email"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 76px" align=left><TABLE><TBODY><TR><TD style="WIDTH: 100px"><asp:Button id="BtnGeneralCancel" tabIndex=91 onclick="BtnGeneralCancel_Click" runat="server" Text="Return To Search" CssClass="field_button" __designer:wfdid="w10"></asp:Button></TD><TD style="WIDTH: 100px">
        <asp:Button id="btnHelp" tabIndex=92 onclick="btnHelp_Click" runat="server" 
            Text="Help" __designer:dtid="1688858450198528" CssClass="field_button" 
            __designer:wfdid="w11"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel></DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

