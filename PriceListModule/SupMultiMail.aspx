<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="SupMultiMail.aspx.vb" Inherits="SupMultiMail"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<%@ Register Src="SubMenuUserControl.ascx" TagName="SubMenuUserControl" TagPrefix="uc1" %>
<%@ Register Src="wchotelproducts.ascx" TagName="hoteltab" TagPrefix="whc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript" >

<!--
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
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
 <style>
 .bgrow
 {
 background-color:white; 

 }

 </style>
    <asp:UpdatePanel id="UpdatePanel2" runat="server">
        <contenttemplate>
<TABLE align="left">
<TBODY>
                <tr>
                <td colspan ="20" align ="left" class="bgrow" >
               
                    <whc:hoteltab ID="whotelatbcontrol" runat="server" />
               
                
                </td>
                </tr>
                <tr>
                <td>
                <div style="margin-top:-6px;margin-left:13px;">
                <table style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid;" class="td_cell" align=left>
<TR><TD vAlign=top align=center width=150 colSpan=2><asp:Label id="lblHeading" runat="server" Text="Supplier" ForeColor="White" CssClass="field_heading" Width="100%"></asp:Label></TD></TR><TR><TD vAlign=top align=left width=150>Code <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD class="td_cell" vAlign=top align=left><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" disabled tabIndex=1 type=text maxLength=20 runat="server" /> &nbsp; Name&nbsp; <INPUT style="WIDTH: 213px" id="txtName" class="field_input" disabled tabIndex=4 type=text maxLength=100 runat="server" /></TD></TR>
<TR><TD vAlign=top align=left width=150><uc1:SubMenuUserControl id="SubMenuUserControl1" runat="server"></uc1:SubMenuUserControl></TD><TD style="WIDTH: 100px" vAlign=top><DIV style="WIDTH: 824px" id="iframeINF" runat="server"><asp:UpdatePanel id="UpdatePanel1" runat="server"><ContentTemplate>
<TABLE style="WIDTH: 656px"><TBODY><TR><TD class="td_cell" colSpan=2><asp:Panel id="PanelEmail" runat="server" Width="697px" GroupingText="Multiple Email"><TABLE style="WIDTH: 403px"><TBODY><TR><TD align=left></TD></TR><TR><TD align=right><asp:Button id="BtnAdd" tabIndex=58 onclick="BtnAdd_Click" runat="server" Text="Add" CssClass="field_button" Width="51px"></asp:Button>&nbsp;</TD></TR><TR><TD align=left>

<asp:GridView id="gv_Email" tabIndex=59 runat="server" AutoGenerateColumns="False"><Columns>
<asp:BoundField DataField="no" HeaderText="Sr No"></asp:BoundField>
<%--HeaderText="Contect Person Name &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"--%>
<asp:TemplateField ><ItemTemplate>

<table>
<tr>
<td>
<asp:Label ID='lblContPerson' runat="server" Text="Contact Person Name" ></asp:Label><span style="color:red">*</span>
</td>
<td>
<INPUT style="WIDTH: 215px" id="txtPerson" class="field_input" type=text maxLength=100 runat="server" /> 
</td></tr> 
<tr>
<td>
<asp:Label ID='lblContactNo' runat="server" Text="Contact Number"></asp:Label><span style="color:red">*</span> 
</td>
<td>
<INPUT style="WIDTH: 159px" id="txtContactNo" class="field_input" type=text maxLength=15 runat="server" /> 
</td>
</tr> 
<tr>
<td>
<asp:Label ID='lblEmailAdd' runat="server" Text="Email Address" ></asp:Label><span style="color:red">*</span>
</td>
<td>
<INPUT style="WIDTH: 220px" id="txtEmail" class="field_input" type=text maxLength=100 runat="server" /> 
</td>
</tr> 
<tr>
<td>
<asp:Label ID='lblDesignation' runat="server" Text="Designation" ></asp:Label>
</td>
<td>
<INPUT id="txtdesignation" runat="server" class="field_input" type=text maxLength=200 style ="width:220px;"/>
</td>
</tr> 
 </table> 
</ItemTemplate>
</asp:TemplateField>
<%--<asp:TemplateField HeaderText="Email Address &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Contact No &lt;font color=&quot;red&quot;&gt;*&lt;/font&gt;"><ItemTemplate>

</ItemTemplate>
</asp:TemplateField>--%>
</Columns>
</asp:GridView></TD></TR><TR><TD align=left><TABLE style="WIDTH: 222px"><TBODY><TR><TD style="WIDTH: 29px"><asp:Button id="BtnEmailSave" tabIndex=60 onclick="BtnEmailSave_Click" runat="server" Text="Save" CssClass="field_button" Width="60px"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="BtnEmailCancel" tabIndex=61 onclick="BtnEmailCancel_Click" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button></TD><TD style="WIDTH: 100px"><asp:Button id="btnhelp" tabIndex=9 onclick="btnhelp_Click" runat="server" Text="Help" CssClass="field_button" __designer:wfdid="w23"></asp:Button></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></asp:Panel></TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel> </DIV></TD></TR>
   </table>
                </div>
                </td>
                </tr>
</TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

