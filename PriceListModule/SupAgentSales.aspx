<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupAgentSales.aspx.vb" Inherits="SupAgentSales"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript" >

<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->


function checkTelephoneNumber(e)
			{	    
			    	
				if ( (event.keyCode < 45 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkNumber(e)
			{	    
			    	
				if ( (event.keyCode < 47 || event.keyCode > 57) )
				{
					return false;
	            }   
	         	
			}
function checkCharacter(e)
			{	    
			    if (event.keyCode == 32 || event.keyCode ==46)
			        return;			
				if ( (event.keyCode < 47 || event.keyCode > 57) && (event.keyCode < 65 || event.keyCode > 91) && (event.keyCode < 96 || event.keyCode > 122))
				{
					return false;
	            }   
	         	
			}
			

</script> 
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier Agents" CssClass="field_heading" Width="913px" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtSuppCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtSuppName" class="field_input" tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR><TR><TD vAlign=top align=left width=150>Type <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><SELECT onblur="GetValueFrom()" style="WIDTH: 201px" id="ddlType" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp; Name&nbsp; <SELECT onblur="GetValueCode()" style="WIDTH: 238px" id="ddlTName" class="field_input" runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD style="WIDTH: 80px; HEIGHT: 302px" class="td_cell" colSpan=2><asp:Panel id="PanelSales" runat="server" Width="699px" GroupingText="Sales Details"><TABLE style="WIDTH: 412px" border=0><TBODY><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Telephone</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone1" class="field_input" tabIndex=33 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleTelephone2" class="field_input" tabIndex=34 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Mobile no.</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleMob" class="field_input" tabIndex=34 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left>Fax</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleFax" class="field_input" tabIndex=35 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>Contact</TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact1" class="field_input" tabIndex=36 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left><INPUT style="WIDTH: 295px" id="txtSaleContact2" class="field_input" tabIndex=37 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 26px" align=left>E-mail</TD><TD style="WIDTH: 100px; HEIGHT: 26px" align=left><INPUT style="WIDTH: 295px" id="txtSaleEmail" class="field_input" tabIndex=38 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 100px" align=left></TD><TD style="WIDTH: 100px" align=left></TD></TR><TR><TD style="WIDTH: 100px" align=left>
    <asp:Button id="BtnSaleSave" tabIndex=39 onclick="BtnSaleSave_Click" 
        runat="server" Text="Save" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 437px" align=left><asp:Button id="BtnSaleCancel" tabIndex=40 onclick="BtnSaleCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button id="btnhelp" tabIndex=41 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w22"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

