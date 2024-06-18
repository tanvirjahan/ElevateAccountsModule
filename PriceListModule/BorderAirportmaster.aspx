<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="BorderAirportmaster.aspx.vb" Inherits="PriceListModule_BorderAirportmaster" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language ="javascript" type ="text/javascript" >
    function checkNumber() {
        if (event.keyCode < 45 || event.keyCode > 57) {
            return false;
        }
    }

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
            if (state == 'New') { if (confirm('Are you sure you want to save Airport/Border Master?') == false) return false; }
            if (state == 'Edit') { if (confirm('Are you sure you want to update Airport/Border Master?') == false) return false; }
            if (state == 'Delete') { if (confirm('Are you sure you want to delete Airport/Border Master?') == false) return false; }
        }
    }  

</script> 
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid"><TBODY><TR><TD style="HEIGHT: 18px" class="td_cell" align=center colSpan=2>
    <asp:Label id="lblHeading" runat="server" Text="Add New Flight Class" 
        Width="500px" CssClass="field_heading"></asp:Label></TD></TR><TR style="COLOR: #ff0000"><TD style="WIDTH: 145px" class="td_cell"><SPAN style="COLOR: black">Code</SPAN> <SPAN style="COLOR: #ff0000" class="td_cell">*</SPAN></TD><TD style="COLOR: #000000">
        <INPUT style="WIDTH: 206px" id="txtCode" class="field_input" tabIndex=0 type=text maxLength=20 runat="server" /> </TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Name <SPAN style="COLOR: red" class="td_cell">*</SPAN></TD><TD>
        <INPUT style="WIDTH: 208px" id="txtName" class="field_input" tabIndex=0 type=text maxLength=100 runat="server" /> </TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 24px" class="td_cell">Active</TD><TD style="HEIGHT: 24px"><INPUT id="chkActive" tabIndex=0 type=checkbox CHECKED runat="server" /></TD></TR><TR><TD style="WIDTH: 145px"><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text="Save" CssClass="btn"></asp:Button></TD><TD width=230><asp:Button id="btnCancel" onclick="btnCancel_Click" runat="server" Text="Return To Search" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnHelp" onclick="btnHelp_Click" runat="server" Text="Help" __designer:dtid="1688858450198528" CssClass="btn" __designer:wfdid="w16"></asp:Button></TD></TR></TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

