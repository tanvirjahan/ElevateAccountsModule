<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="CancellationTypes.aspx.vb" Inherits="CancellationTypes"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript">

function FormValidation(state)
{
     if (document.getElementById("<%=txtCode.ClientID%>").value=="")
      {
                 alert("Code Field can not be blank");
                 document.getElementById("<%=txtCode.ClientID%>").focus();
                 return false;
      }
      if(document.getElementById("<%=txtName.ClientID %>").value=="")
      {
                 alert("Name Field can not be blank");
                document.getElementById("<%=txtName.ClientID %>").focus();
                return false;
      }
	
    
    if (state =='New'){if(confirm('Are you sure you want to save Cancellation type?')==false)return false;}
    else if (state =='Edit'){if(confirm('Are you sure you want to updateCancellation type?')==false)return false;}
    else if (state =='Delete'){if(confirm('Are you sure you want to delete Cancellation type?')==false)return false;}  
    
}


</script>


    <table>
        <tr>
            <td style="width: 100px">
                <asp:UpdatePanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Cancellation Type" CssClass="field_heading" Width="415px"></asp:Label></TD></TR><TR><TD style="WIDTH: 145px" class="td_cell"><SPAN style="COLOR: #000033">Code</SPAN> <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="COLOR: #000000"><INPUT style="WIDTH: 234px" id="txtCode" class="txtbox" tabIndex=1 type=text maxLength=20 runat="server" /> </TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 22px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD style="HEIGHT: 22px">
<INPUT style="WIDTH: 234px" id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" /> </TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Regret</TD><TD>&nbsp;<asp:DropDownList id="ddlRegret" tabIndex=3 runat="server" Width="52px"><asp:ListItem>Yes</asp:ListItem>
<asp:ListItem>No</asp:ListItem>
</asp:DropDownList> </TD></TR>
<TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Active</TD><TD style="HEIGHT: 24px">
<INPUT id="chkActive" tabIndex=4 type=checkbox CHECKED runat="server" class="chkbox" /></TD></TR><TR><TD style="WIDTH: 145px">
<asp:Button id="btnSave" tabIndex=5 runat="server" Text="Save" CssClass="btn"></asp:Button></TD><TD><asp:Button id="btnCancel" tabIndex=6 runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp; 
            <asp:Button id="btnhelp" tabIndex=8 onclick="btnhelp_Click" runat="server" 
                Text="Help" CssClass="btn" __designer:wfdid="w24"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel></td>
        </tr>
    </table>
</asp:Content>