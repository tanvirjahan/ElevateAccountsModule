<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupAgentAccts.aspx.vb" Inherits="SupAgentAccts"  %>

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
			

    function CallWebMethod(methodType)
    {
        switch(methodType)
        {
            case "postcode":
                var select=document.getElementById("<%=ddlPostCode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=ddlPostName.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "postname":
                var select=document.getElementById("<%=ddlPostName.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=ddlPostCode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
                
                
                
                
                
                
                  case "ctrlcode":
                var select=document.getElementById("<%=cmbctrlcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%=cmbctrlname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "ctrlname":
                var select=document.getElementById("<%=cmbctrlname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%=cmbctrlcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
                
               
                
                  case "accrualcode":
                var select=document.getElementById("<%= cmbaccrualcode.ClientID%>");
                var codeid=select.options[select.selectedIndex].text;
                var selectname=document.getElementById("<%= cmbaccrualname.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
            case "accrualname":
                var select=document.getElementById("<%= cmbaccrualname.ClientID%>");
                var codeid=select.options[select.selectedIndex].value;
                var selectname=document.getElementById("<%= cmbaccrualcode.ClientID%>");
                selectname.value=select.options[select.selectedIndex].text;
                break;
        }
    }


</script>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid" class="td_cell" align=left><TBODY><TR><TD style="HEIGHT: 18px" vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier Agents" Width="900px" CssClass="field_heading" ForeColor="White"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 827px" class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtSuppCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtSuppName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /></TD></TR><TR><TD vAlign=top align=left width=150>Type <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="WIDTH: 827px" class="td_cell" vAlign=top align=left><SELECT onblur="GetValueFrom()" style="WIDTH: 201px" id="ddlType" class="field_input" tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp; Name&nbsp; <SELECT onblur="GetValueCode()" style="WIDTH: 238px" id="ddlTName" class="field_input" tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 827px" vAlign=top><DIV style="WIDTH: 824px; HEIGHT: 450px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD style="WIDTH: 80px" class="td_cell" colSpan=2><asp:Panel id="PanelAccounts" runat="server" Width="699px" GroupingText="Account Details">
    <TABLE style="WIDTH: 667px"><TBODY><TR><TD style="WIDTH: 202px; HEIGHT: 15px" align=left>Telephone</TD>
        <TD style="WIDTH: 400px; " align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone1" class="field_input" tabIndex=41 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left></TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccTelephone2" class="field_input" tabIndex=42 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left>Mobile no.</TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccmob" class="field_input" tabIndex=42 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left>Fax</TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccFax" class="field_input" tabIndex=43 type=text maxLength=50 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left>Contact</TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact1" class="field_input" tabIndex=44 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left></TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccContact2" class="field_input" tabIndex=45 type=text maxLength=100 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left>E-mail</TD>
        <TD style="WIDTH: 400px" align=left><INPUT style="WIDTH: 315px" id="txtAccEmail" class="field_input" tabIndex=46 type=text maxLength=100 runat="server" /> </TD></TR><TR><TD style="WIDTH: 202px; HEIGHT: 24px" align=left>Control A/C Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
        <TD style="WIDTH: 400px" align=left><SELECT style="WIDTH: 150px" id="cmbctrlcode" 
                class="field_input" tabIndex=51 onchange="CallWebMethod('ctrlcode');" 
                runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;<SELECT 
                style="WIDTH: 246px" id="cmbctrlname" class="field_input" tabIndex=52 
                onchange="CallWebMethod('ctrlname');" runat="server" name="D1"> <OPTION selected></OPTION></SELECT></TD></TR><TR>
        <TD style="height: 0px;" align=left>&nbsp;</TD><TD style="WIDTH: 400px" align=left>
        <SELECT style="WIDTH: 60px; height: 0px;" id="cmbaccrualcode" 
            class="field_input" tabIndex=51 onchange="CallWebMethod('accrualcode');" 
            runat="server" visible="False"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp;<SELECT 
            style="WIDTH: 246px; height: 0px;" id="cmbaccrualname" class="field_input" 
            tabIndex=52 onchange="CallWebMethod('accrualname');" runat="server" 
            visible="False"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 202px" align=left>Cash Sup Agent</TD>
        <TD style="WIDTH: 400px" align=left><INPUT id="ChkCashSup" tabIndex=48 type=checkbox runat="server" /></TD></TR><TR><TD style="WIDTH: 202px" align=left>Credit Days</TD>
        <TD align=left style="width: 400px"><INPUT style="WIDTH: 75px; TEXT-ALIGN: right" id="TxtAccCreditDays" class="field_input" tabIndex=49 type=text maxLength=5 runat="server" /> &nbsp; &nbsp; Credit Limit <INPUT style="WIDTH: 112px; TEXT-ALIGN: right" id="txtAccCreditLimit" class="field_input" tabIndex=50 type=text maxLength=15 runat="server" /></TD></TR><TR><TD style="WIDTH: 202px; HEIGHT: 22px" align=left>
        &nbsp;</TD><TD style="width: 400px;" align=left>&nbsp;<SELECT style="WIDTH: 29px" 
                id="ddlPostCode" class="field_input" tabIndex=51 
                onchange="CallWebMethod('postcode');" runat="server" visible="False"> <OPTION selected></OPTION></SELECT>&nbsp;<SELECT 
                style="WIDTH: 21px" id="ddlPostName" class="field_input" tabIndex=52 
                onchange="CallWebMethod('postname');" runat="server" name="D2" visible="False"> <OPTION selected></OPTION></SELECT></TD></TR><TR><TD style="WIDTH: 202px" align=left>
    <asp:Button id="BtnAccSave" tabIndex=53 onclick="BtnAccSave_Click" 
        runat="server" Text="Save" CssClass="field_button"></asp:Button></TD>
        <TD align=left style="width: 400px"><asp:Button id="BtnAccCancel" tabIndex=54 onclick="BtnAccCancel_Click" runat="server" Text="Return To Search" CssClass="field_button" Height="20px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;<asp:Button id="btnhelp" tabIndex=55 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w23"></asp:Button></TD></TR></TBODY></TABLE></asp:Panel> <asp:DropDownList id="ddlAccPostTo" tabIndex=51 runat="server" Visible="False" Width="140px" CssClass="field_input" AutoPostBack="True" OnSelectedIndexChanged="ddlAccPostTo_SelectedIndexChanged"><asp:ListItem>[Select]</asp:ListItem>
</asp:DropDownList> <INPUT style="WIDTH: 171px" id="txtAccPostTo2" class="field_input" tabIndex=52 readOnly type=text runat="server" Visible="false" /></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> <asp:ScriptManagerProxy id="ScriptManagerProxy1" runat="server" __designer:wfdid="w1"><Services>
<asp:ServiceReference Path="~/clsServices.asmx"></asp:ServiceReference>
</Services>
</asp:ScriptManagerProxy></DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
    
    </asp:UpdatePanel>
</asp:Content>

