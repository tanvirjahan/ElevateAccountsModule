<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CostCenterGroups.aspx.vb" Inherits="CostCenterGroups"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >

function FormValidation(state)
{

    if ((document.getElementById("<%=txtCode.ClientID%>").value=="") || (document.getElementById("<%=txtCode.ClientID%>").value<=0)||(document.getElementById("<%=txtName.ClientID%>").value==""))
    {
       if (document.getElementById("<%=txtCode.ClientID%>").value=="")
	    {
            document.getElementById("<%=txtCode.ClientID%>").focus(); 
             alert("Code field can not be blank");
            return false;
         }
         else if (document.getElementById("<%=txtCode.ClientID%>").value<=0)
              {
              alert("Code must be greater than zero.");
              document.getElementById("<%=txtCode.ClientID%>").focus();
              return false;
              }
           else if (document.getElementById("<%=txtName.ClientID%>").value=="") 
	    {
           document.getElementById("<%=txtName.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
         }
           
           
  }
    else
       {
       //alert(state);
       if (state=='New'){if(confirm('Are you sure you want to save Cost Center Groups ?')==false)return false;}
       if (state=='Edit'){if(confirm('Are you sure you want to update Cost Center Groups ?')==false)return false;}
       if (state=='Delete'){if(confirm('Are you sure you want to delete Cost Center Groups ?')==false)return false;}
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
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 456px; BORDER-BOTTOM: gray 2px solid">
<TBODY><TR><TD align=center colSpan=5><asp:Label id="lblHeading" runat="server" Text="Add New Cost Center Groups" Width="471px" CssClass="field_heading"></asp:Label></TD></TR>
<TR><TD style="WIDTH: 100px" class="td_cell"><SPAN style="COLOR: #000000" class="td_cell">Group Code <SPAN style="COLOR: red">*</SPAN></SPAN></TD><TD style="COLOR: #000000"><INPUT style="WIDTH: 196px" id="txtCode" class="field_input" tabIndex=0 onkeypress="return checkNumber()" type=text maxLength=20 runat="server" /> </TD></TR><TR>
<TD style="WIDTH: 100px" class="td_cell"><SPAN style="COLOR: black" class="td_cell">Group Name <SPAN style="COLOR: red">*</SPAN></SPAN></TD>
<TD><INPUT style="WIDTH: 196px" id="txtName" class="field_input" tabIndex=0 onkeypress="return checkNumber()" type=text maxLength=100 runat="server" /></TD></TR>
<TR><TD style="WIDTH: 100px" class="td_cell">Active</TD><TD><INPUT id="chkActive" tabIndex=0 type=checkbox CHECKED runat="server" /></TD></TR><TR><TD style="WIDTH: 67px">
    <asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Save" 
        CssClass="field_button"></asp:Button></TD><TD><asp:Button id="btnCancel" runat="server" Text="Return To Search" CssClass="field_button"></asp:Button>&nbsp;<asp:Button 
            id="btnhelp" tabIndex=7 onclick="btnhelp_Click" runat="server" Text="Help" 
            CssClass="field_button" __designer:wfdid="w5"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

