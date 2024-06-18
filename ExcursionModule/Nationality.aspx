<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Nationality.aspx.vb" Inherits="Nationality"  MasterPageFile="~/SubPageMaster.master" Strict="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
    function FormValidation(state) {
        if ((document.getElementById("<%=txtCode.ClientID%>").value == "") || (document.getElementById("<%=txtName.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtCode.ClientID%>").value == "") {
                document.getElementById("<%=txtCode.ClientID%>").focus();
                alert("Code field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtName.ClientID%>").value == "") {
                document.getElementById("<%=txtName.ClientID%>").focus();
                alert("Name field can not be blank");
                return false;
            }
        }
        else {
            if (state == 'New') { if (confirm('Are you sure you want to save Nationality?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Nationality?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Nationality?') == false) return false; }
        }
    }
</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Add New Market" CssClass="field_heading" Width="415px">
</asp:Label></TD></TR><TR>
       <TD style="WIDTH: 145px" class="td_cell"><SPAN>Code</SPAN> <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
       <TD style="COLOR: #000000"><INPUT style="WIDTH: 194px" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" /> </TD></TR>

        <TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD><INPUT style="WIDTH: 194px" id="txtName" class="txtbox" tabIndex=2 type=text maxLength=100 runat="server" /> </TD></TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Language <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD>
        <TD><INPUT style="WIDTH: 194px" id="txtLanguage" class="txtbox" tabIndex="2" type=text   runat="server" /> </TD></TR>
        

        <TR><TD style="WIDTH: 145px; HEIGHT: 16px" class="td_cell">Active</TD>
        <TD style="HEIGHT: 16px"><INPUT id="chkActive" tabIndex=3 type=checkbox CHECKED runat="server" /></TD></TR>
        
        <TR><TD style="WIDTH: 145px; height: 16px;" class="td_cell">
            Show in Excursions</TD>
        <TD style="HEIGHT: 16px">
            <INPUT id="chkshowexc" tabIndex=3 
                type=checkbox CHECKED runat="server" />
            </TD></TR>
        <tr><td style="WIDTH: 145px">
            <asp:Button ID="btnSave" runat="server" CssClass="btn" tabIndex="5" 
                Text="Save" />
            </td>
            <td>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn" tabIndex="6" 
                    Text="Return To Search" />
                &nbsp;&nbsp;
                <asp:Button ID="btnhelp" runat="server" __designer:wfdid="w19" CssClass="btn" 
                    OnClick="btnhelp_Click" tabIndex="5" Text="Help" />
            </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblwebserviceerror" runat="server" style="display:none" 
                Text="Webserviceerror"></asp:Label>
        </td>
    </tr>
    </TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>


</asp:Content>
