<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="Concierge.aspx.vb" Inherits="Other_Services_Selling_Types"  %>

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
if ((document.getElementById("<%=txtcode.ClientID%>").value=="")||(document.getElementById("<%=txtname.ClientID%>").value=="")||(document.getElementById("<%=ddlgpcode.ClientID%>").value=="[Select]"))
{
    if (document.getElementById("<%=txtcode.ClientID%>").value=="")
    {
           document.getElementById("<%=txtcode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=txtname.ClientID%>").value=="") 
     {
           document.getElementById("<%=txtname.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
     }
     else if (document.getElementById("<%=ddlgpname.ClientID%>").value=="[Select]") 
     {
           document.getElementById("<%=ddlgpcode.ClientID%>").focus();
           alert("Select Currency Code");
            return false;
     }
             
 }
 else
 {
       if (state=='New'){if(confirm('Are you sure you want to Save Handling Fees Selling Types?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to Update Handling Fees Selling Types?')==false)return false;}
       if (state == 'Delete') { if (confirm('Are you sure you want to Delete Handling Fees Selling Types?') == false) return false; }
 }
}  
   function  GetValueFrom()
{

	var ddl = document.getElementById("<%=ddlgpname.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpcode.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}






function  GetValueCode()
{
	var ddl = document.getElementById("<%=ddlgpcode.ClientID%>");
	ddl.selectedIndex = -1;
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if (ddl.options[i].text == 
			document.getElementById("<%=ddlgpname.ClientID%>").value)
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
}




function checkNumber(evt) {

    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));

    if (charCode != 47 && (charCode > 44 && charCode < 58)) {
        return true;
    }
    return false;
}

   
</script>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 700px; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=4>
    <asp:Label id="lblHeading" runat="server" Text="Add New Concierge" 
        CssClass="field_heading" Width="646px" Height="18px"></asp:Label></TD>

    </TR>
    <TR style="COLOR: #ff0000">
    <TD style="WIDTH: 549px; height: 24px;" class="td_cell"><SPAN style="COLOR: #000000">Code </SPAN><SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD>
    <TD style="COLOR: #000000; height: 24px; width: 487px;">
        <INPUT style="WIDTH: 191px; height: 20px;"  id="txtcode" class="txtbox" 
                 tabIndex=2 type=text 
            maxLength=200 runat="server" /></TD>
       
         
        <td> 
            
            &nbsp;</td>

        </TR>
        <TR>
    
    <td class="td_cell" style="COLOR: #000000; width: 549px;" height: 29px;"> Name</td>
    <td style="height: 23px; width: 487px">
        <INPUT style="WIDTH: 191px; height: 20px;"  id="txtname" class="txtbox" 
                 tabIndex=2 type=text 
            maxLength=200 runat="server" /></td></TR><TR>
    <TD style="WIDTH: 549px; HEIGHT: 22px" class="td_cell">Dept Code</TD>
   
    <TD style="HEIGHT: 22px; width: 487px;" align=left>
        <SELECT 
            onchange="GetValueFrom()" style="WIDTH: 193px" id="ddlgpcode" class="drpdown" 
            tabIndex=3 runat="server"> <OPTION selected></OPTION></SELECT></td> 

             <td class="td_cell" style="width: 466px; height: 29px;">
        Dept Name</td>
        <td style="height: 22px; width: 294px;">
        <SELECT 
            onchange="GetValueCode()" style="WIDTH: 201px" id="ddlgpname" class="drpdown" 
            tabIndex=4 runat="server"> <OPTION selected></OPTION></SELECT></TD></TR>
     <tr>
         <td class="td_cell" style="WIDTH: 549px; HEIGHT: 29px">
             commission %</td>
         <td align="left" style="HEIGHT: 29px; width: 487px;">
             <INPUT style="WIDTH: 90px; height: 20px;"  id="txtcommi" class="txtbox" 
                 tabIndex=2 type=text 
            maxLength=200 runat="server" />
         </td>
         <td class="td_cell" style="width: 466px; height: 29px;">
             </td>
         <td style="height: 29px; width: 294px;">
             </td>
    </tr>
        <tr>
   
     
    <td class="td_cell" style="width: 549px; height: 29px;">
        &nbsp;</td><td style=" height: 29px; width: 487px;" >
             &nbsp;</td>
     <td class="td_cell" style="width: 466px; height: 29px;">
         &nbsp;</td><td style="height: 29px; width: 294px;" >
             &nbsp;</td>

    </tr>
    <TR><TD style="WIDTH: 549px; HEIGHT: 24px" class="td_cell">Active</TD>
        <TD style="HEIGHT: 24px; width: 487px;">
        <INPUT id="chkActive" tabIndex=5 type=checkbox CHECKED runat="server" /></TD></TR>
        <TR>
        <TD style="WIDTH: 549px; height: 27px;"><asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="btn" Width="46px"></asp:Button></TD>
        <TD style="width: 487px; height: 27px;"><asp:Button id="btnCancel" tabIndex=7 onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp; 
    <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server"
    
        Text="Help" CssClass="btn" __designer:wfdid="w20"></asp:Button><td></td><td></td>
        
        <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label>
  
        </TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

