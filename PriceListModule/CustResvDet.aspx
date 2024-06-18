<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CustResvDet.aspx.vb" Inherits="CustResvDet"  %>

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
function FormValidationMainDetail(state)
{
    if ((document.getElementById("<%=txtResAddress1.ClientID%>").value=="")||(document.getElementById("<%=txtResPhone1.ClientID%>").value=="")||(document.getElementById("<%=txtResFax.ClientID%>").value==""))
    {
          if (document.getElementById("<%=txtResAddress1.ClientID%>").value=="")
          {
            alert("Address field can not be blank");
            document.getElementById("<%=txtResAddress1.ClientID%>").focus(); 
             return false;
           }
           else if (document.getElementById("<%=txtResPhone1.ClientID%>").value=="") 
           {
           alert("Telephone field can not be blank");
           document.getElementById("<%=txtResPhone1.ClientID%>").focus();
            return false;
           }
    }
  else
       {
       if (state=='New'){if(confirm('Are you sure you want to save customer reservation details?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update customer reservation details?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete customer reservation details?')==false)return false;}
       }
}

</script>

    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 800px; 
    BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left>
<TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Customers" Width="800px" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left><SPAN style="COLOR: #ff0000" class="td_cell"></SPAN></TD><TD style="WIDTH: 85%" class="td_cell" vAlign=top align=left>Code <SPAN style="COLOR: #ff0000" class="td_cell">*&nbsp; </SPAN><INPUT style="WIDTH: 196px" id="txtCustomerCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; 
    <INPUT style="WIDTH: 271px" id="txtCustomerName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 15%" vAlign=top align=left>&nbsp;<uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl> </TD><TD style="WIDTH: 85%" vAlign=top>
        <DIV style="WIDTH: 666px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<asp:Panel id="PanelReservstion" runat="server" Width="600px" GroupingText="Reservation Details"><TABLE style="WIDTH: 100%" align=center><TBODY><TR><TD style="WIDTH: 58px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Address</SPAN><SPAN style="FONT-SIZE: 8pt; COLOR: #ff0000; FONT-FAMILY: Arial" class="td_cell">*</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress1" class="field_input" tabIndex=28 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px; HEIGHT: 24px" align=left></TD><TD style="WIDTH: 5px; HEIGHT: 24px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress2" class="field_input" tabIndex=29 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResAddress3" class="field_input" tabIndex=30 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Telephone<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></SPAN></SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone1" class="field_input" tabIndex=31 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResPhone2" class="field_input" tabIndex=32 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px; HEIGHT: 15px" align=left>Mobile no.</TD><TD style="WIDTH: 5px; HEIGHT: 15px" align=left><INPUT style="WIDTH: 296px" id="txtresmob"  class="field_input" type=text runat="server" /></TD></TR><TR><TD style="WIDTH: 58px; HEIGHT: 4px" align=left><SPAN style="FONT-FAMILY: Arial"><SPAN style="FONT-SIZE: 8pt">Fax<%--<SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN>--%></SPAN></SPAN></TD><TD style="WIDTH: 5px; HEIGHT: 4px" align=left><INPUT style="WIDTH: 297px" id="txtResFax" class="field_input" tabIndex=33 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Contact</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResContact1" class="field_input" tabIndex=34 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResContact2" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px; HEIGHT: 24px" align=left>Designation</TD><TD style="WIDTH: 5px; HEIGHT: 24px" align=left><INPUT style="WIDTH: 297px" id="txtdesignation" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left>IATA no.</TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtiatano" class="field_input" tabIndex=35 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">Email</SPAN></TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtResEmail" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left>Web</TD><TD style="WIDTH: 5px" align=left><INPUT style="WIDTH: 297px" id="txtweb" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 58px" align=left><SPAN style="FONT-SIZE: 8pt; FONT-FAMILY: Arial">CommunicateBy</SPAN></TD><TD style="WIDTH: 5px" align=left><asp:DropDownList id="ddlCommunicateBy" tabIndex=37 runat="server" Width="189px" CssClass="field_input" AutoPostBack="True"><asp:ListItem>Email</asp:ListItem>
<asp:ListItem>Fax</asp:ListItem>
<asp:ListItem Value="Both">Both(Email/Fax)</asp:ListItem>
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 58px" align=left>
        <asp:Button id="BtnResSave" tabIndex=38 runat="server" Text="Save" 
            CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 230px" align=left><asp:Button id="BtnResCancel" tabIndex=39 onclick="BtnResCancel_Click" runat="server" Text="Return to Search" CssClass="field_button"></asp:Button>&nbsp; <asp:Button id="btnHelp" tabIndex=40 onclick="btnHelp_Click" runat="server" Text="Help" CssClass="field_button"></asp:Button></TD></TR><tr><td colspan="4"><asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr></TBODY></TABLE></asp:Panel> 
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

