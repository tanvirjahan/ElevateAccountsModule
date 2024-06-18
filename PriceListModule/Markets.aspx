<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Markets.aspx.vb" Inherits="Markets"  MasterPageFile="~/SubPageMaster.master" Strict="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
function FormValidation(state)
{
    if ((document.getElementById("<%=txtCode.ClientID%>").value=="") || (document.getElementById("<%=txtName.ClientID%>").value=="") )
       {
//           if (document.getElementById("<%=txtCode.ClientID%>").value==""){
//            document.getElementById("<%=txtCode.ClientID%>").focus(); 
//             alert("Code field can not be blank");
//            return false;
//           }
            if (document.getElementById("<%=txtName.ClientID%>").value=="") {
           document.getElementById("<%=txtName.ClientID%>").focus();
           alert("Name field can not be blank");
            return false;
           }
       }
       else
       {
       if (state=='New'){if(confirm('Are you sure you want to save region?')==false)return false;}
       if (state == 'Edit') { if (confirm('Are you sure you want to update region?') == false) return false; }
       if (state == 'Delete') { if (confirm('Are you sure you want to delete region?') == false) return false; }
       }
}
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Region" CssClass="field_heading" Width="415px">
</asp:Label></TD></TR>


       <TR><TD style="WIDTH: 145px" class="td_cell"><SPAN>Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
       <TD style="COLOR: #000000">
        <INPUT style="WIDTH: 194px" id="txtCode" class="field_input"  tabIndex=1 type=text maxLength=20 runat="server" contenteditable="inherit" readonly="readonly" /> </TD></TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
            <TD><INPUT style="WIDTH: 194px" id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /> </TD>
        </TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 24px;display:none" class="td_cell">Head Of Department <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
            <TD style="display:none"> <asp:DropDownList style="WIDTH: 194px" ID="ddlHOD" runat ="server" ></asp:DropDownList>  </TD>
        </TR>
        <TR>
            <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell">Active</TD>
            <TD style="HEIGHT: 16px"><INPUT id="chkActive" tabIndex=3 type=checkbox CHECKED runat="server" /></TD></TR>
        <TR>
           <%-- <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell">Show in Web</TD>--%>
            <TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell"></TD>
            <TD style="HEIGHT: 16px"><INPUT id="chkShowInweb" tabIndex=4 type=checkbox CHECKED runat="server" style="display:none" /></TD></TR>
        <TR><TD style="WIDTH: 145px"><asp:Button id="btnSave" tabIndex=5 runat="server" Text="Save" CssClass="btn"></asp:Button></TD>
            <TD><asp:Button id="btnCancel" tabIndex=6 runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;&nbsp;
            <asp:Button id="btnhelp" tabIndex=5 runat="server" Text="Help" CssClass="btn" __designer:wfdid="w19" OnClick="btnhelp_Click">
            </asp:Button></TD></TR>
            <tr><td colspan=4>
            <asp:Label ID="lblwebserviceerror" runat="server"  style="display:none" Text="Webserviceerror"></asp:Label></td></tr>
            </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>


</asp:Content>
