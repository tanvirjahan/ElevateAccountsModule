<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="VisaSellingTypes.aspx.vb" Inherits="Visa_Selling_Types"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript " type ="text/javascript" >
function checkNumber()
{
   if (event.keyCode < 45 || event.keyCode > 57)
    {
         return false;
    }
}

function FormValidation(state)
{
if ((document.getElementById("<%=TxtOtherServiceCode.ClientID%>").value=="")||(document.getElementById("<%=txtOtherServiceSelling.ClientID%>").value=="")||(document.getElementById("<%=ddlCurrencyCd.ClientID%>").value=="[Select]"))
{
    if (document.getElementById("<%=TxtOtherServiceCode.ClientID%>").value=="")
    {
           document.getElementById("<%=TxtOtherServiceCode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=txtOtherServiceSelling.ClientID%>").value=="") 
     {
           document.getElementById("<%=txtOtherServiceSelling.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=ddlCurrencyCd.ClientID%>").value=="[Select]") 
     {
           document.getElementById("<%=ddlCurrencyCd.ClientID%>").focus();
           alert("Select Currency Code");
            return false;
     }
             
 }
 else
 {
       if (state=='New'){if(confirm('Are you sure you want to Save Visa Selling Types?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to Update Visa Selling Types?')==false)return false;}
       if (state == 'Delete') { if (confirm('Are you sure you want to Delete Visa Selling Types?') == false) return false; }
 }
}  
   function  GetValueFrom()
{

	var ddl = document.getElementById("<%=ddlCurrencyNm.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyCd.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlCurrencyCd.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlCurrencyNm.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}
   
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 656px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Visa  Selling Types" CssClass="field_heading" Width="645px"></asp:Label></TD></TR><TR style="COLOR: #ff0000"><TD style="WIDTH: 209px" class="td_cell"><SPAN style="COLOR: #000000">Code </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="COLOR: #000000"><INPUT style="WIDTH: 196px" id="TxtOtherServiceCode" class="txtbox" tabIndex=1 type=text maxLength=150 runat="server" /></TD></TR><TR><TD style="WIDTH: 209px; HEIGHT: 24px" class="td_cell">
    Name <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="COLOR: #000000"><INPUT style="WIDTH: 196px" id="txtOtherServiceSelling" class="txtbox" tabIndex=2 type=text maxLength=150 runat="server" /> </TD></TR><TR><TD style="WIDTH: 209px; HEIGHT: 26px" class="td_cell">Currency</TD><TD style="HEIGHT: 26px" align=left>&nbsp;<SELECT onchange="GetValueFrom()" style="WIDTH: 197px" id="ddlCurrencyCd" class="drpdown" tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT>&nbsp;&nbsp;<SELECT onchange="GetValueCode()" style="WIDTH: 201px" id="ddlCurrencyNm" class="drpdown" tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR><tr><td class="td_cell">
    <asp:Label runat="server" ID="LblcstHF" Text="Cost of Handling Fee Required" 
        Visible="False"></asp:Label></td><td>
        <INPUT id="chkHandlingFees" tabIndex=5 type=checkbox CHECKED runat="server" visible="False" />
    </td></tr><TR><TD style="WIDTH: 209px; HEIGHT: 24px" class="td_cell">Active</TD><TD style="HEIGHT: 24px">
        <INPUT id="chkActive" tabIndex=5 type=checkbox CHECKED runat="server" /></TD></TR><TR><TD style="WIDTH: 209px"><asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="btn" Width="46px"></asp:Button></TD><TD><asp:Button id="btnCancel" tabIndex=7 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp; 
    <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server"
    
        Text="Help" CssClass="btn" __designer:wfdid="w20"></asp:Button>
        
        <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label>
  
        </TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

