<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustWebApprvl.aspx.vb" Inherits="CustWebApprvl" Title="::::" %>

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
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" ForeColor="White" Width="800px" CssClass="field_heading"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN>
    <INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; 
    <INPUT style="WIDTH: 213px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD vAlign=top><DIV style="WIDTH: 800px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelWebApproval" runat="server" Width="600px" GroupingText="Web Approval"><TABLE style="WIDTH: 553px" border=0><TBODY><TR><TD style="WIDTH: 84px" align=left>ApprovalForWeb</TD><TD align=left><INPUT id="ChkWebApprove" tabIndex=70 type=checkbox runat="server" /></TD></TR><TR><TD align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">User Name</SPAN></TD><TD align=left>
    <INPUT id="txtWebAppUsername" class="field_input" tabIndex=71 type=text maxLength=50 runat="server" /></TD></TR><TR><TD align=left>Password</TD><TD align=left>
        <INPUT id="txtWebAppPassword" class="field_input" tabIndex=72 type=text maxLength=50 runat="server" />&nbsp;<asp:Button 
            id="BtnShowPassword" tabIndex=73 onclick="BtnShowPassword_Click" runat="server" 
            Text="Generate Auto Password" CssClass="btn"></asp:Button></TD></TR><TR><TD align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD align=left>
        <INPUT id="txtWebAppContact" class="field_input" tabIndex=74 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="HEIGHT: 30px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Email ID To Send</SPAN></TD><TD align=left>
        <INPUT style="WIDTH: 262px" id="txtWebAppEmail" class="field_input" tabIndex=75 type=text maxLength=100 runat="server" /></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=left colSpan=2>
    <asp:Button id="BtnWebInviteCustomer" tabIndex=76 
          runat="server" 
        Text="Invite Customer First Time For Web  Access" CssClass="btn"></asp:Button></TD></TR><TR><TD align=left colSpan=2>
        <asp:Button id="BtnWebResendPasswprd" tabIndex=77 
            onclick="BtnWebResendPasswprd_Click" runat="server" 
            Text="Resend Password Email To Customer" CssClass="btn"></asp:Button></TD></TR><TR><TD align=left colSpan=2></TD></TR><TR><TD align=left>
    <asp:Button id="BtnWebAppSave" tabIndex=78 onclick="BtnWebAppSave_Click" 
        runat="server" Text="Save" CssClass="btn"></asp:Button></TD><TD align=left><asp:Button id="BtnWebAppCancel" tabIndex=79 onclick="BtnWebAppCancel_Click" runat="server" Text="Return to Search" CssClass="btn"></asp:Button>&nbsp; 
        <asp:Button id="btnHelp" tabIndex=80 onclick="btnHelp_Click" runat="server" 
            Text="Help" CssClass="btn"></asp:Button></TD></TR><tr><td><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr></TBODY></TABLE></asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
    <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server">
        <services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</services>
    </asp:ScriptManagerProxy>
</asp:Content>

