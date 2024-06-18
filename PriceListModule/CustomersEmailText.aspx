<%@ Page Language="VB" MasterPageFile="~/PriceListMaster.master" AutoEventWireup="false"   ValidateRequest="false" Strict ="true"  CodeFile="CustomersEmailText.aspx.vb" Inherits="CustomersEmailText"  %>

<%--<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>--%>
    <%--<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>--%>
    <%--<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>--%>

    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script language ="javascript" type ="text/javascript" >
    function checkNumber(e) {
        if (event.keyCode < 45 || event.keyCode > 57) {
            return false;
        }
    }

    function FormValidation(state) {
        if ((document.getElementById("<%=txtSubject.ClientID%>").value == "") || (document.getElementById("<%=txtEmailText.ClientID%>").value == "") || (document.getElementById("<%=txtFooterText.ClientID%>").value == "")) {
            if (document.getElementById("<%=txtSubject.ClientID%>").value == "") {
                document.getElementById("<%=txtSubject.ClientID%>").focus();
                alert("Subject field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtEmailText.ClientID%>").value == "") {
                document.getElementById("<%=txtEmailText.ClientID%>").focus();
                alert("Email Text field can not be blank");
                return false;
            }
            else if (document.getElementById("<%=txtFooterText.ClientID%>").value == "") {
                document.getElementById("<%=txtFooterText.ClientID%>").focus();
                alert("Footer Text field can not be blank");
                return false;
            }
        }
        else {
            if (state == 'Edit') { if (confirm('Are you sure you want to update CustomerEmailText?') == false) return false; }
        }
    }


    

</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" >
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; WIDTH: 696px;
     BORDER-BOTTOM: gray 2px solid">
     <TBODY><TR>
     <TD style="HEIGHT: 18px" class="td_cell" align="center" colSpan=2>
     <asp:Label id="lblHeading" runat="server" Text="Add New Customer Email " ForeColor="White" CssClass="field_heading" Width="707px"></asp:Label></TD></TR>
     <TR style="COLOR: #ff0000">
     <TD style="WIDTH: 145px; HEIGHT: 27px" class="td_cell">
     <SPAN style="COLOR: #ff0000" class="td_cell">
     <SPAN style="COLOR: black">Subject</SPAN>*</SPAN></TD>
     <TD style="WIDTH: 344px; COLOR: #000000; HEIGHT: 27px">
    <asp:TextBox id="txtSubject" tabIndex=1 runat="server" Height="16px" 
        CssClass="field_input" Width="626px"  MaxLength="500"></asp:TextBox> </TD></TR>
        <TR><TD style="WIDTH: 145px; HEIGHT: 14px" class="td_cell">
        <SPAN style="COLOR: red" class="td_cell">
        <SPAN style="COLOR: black">Email Text</SPAN>*</SPAN></TD>
        <TD style="WIDTH: 344px; HEIGHT: 14px">
        <asp:TextBox id="txtEmailText" tabIndex=2 runat="server" Height="150px" 
            CssClass="field_input" Width="630px"   MaxLength="1250" 
            TextMode="MultiLine"></asp:TextBox>
         
            
            </TD></TR>
    <TR><TD style="WIDTH: 145px; HEIGHT: 14px" class="td_cell">Footer Text</TD><TD style="WIDTH: 344px; HEIGHT: 14px">
        <asp:TextBox id="txtFooterText" tabIndex=3 runat="server" Height="100px" 
            CssClass="field_input" Width="626px"   MaxLength="1250" 
            TextMode="MultiLine"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 145px" class="td_cell">From Email ID</TD><TD style="WIDTH: 344px">
        <asp:TextBox id="txtFromEmailId" tabIndex=1 runat="server" Height="16px" 
            CssClass="field_input" Width="626px"   MaxLength="100"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 145px" class="td_cell">Active</TD><TD style="WIDTH: 344px"><INPUT id="chkActive" tabIndex=0 type=checkbox CHECKED runat="server" /></TD></TR><TR><TD style="WIDTH: 145px; HEIGHT: 22px">
    <asp:Button id="btnSave" tabIndex="4"  runat="server"  
        Text="Save" CssClass="btn"></asp:Button></TD><TD style="WIDTH: 344px; HEIGHT: 22px">
        <asp:Button id="btnCancel"  tabIndex=5 onclick="btnCancel_Click" runat="server" 
            Text="Exit" CssClass="btn"></asp:Button>&nbsp;<asp:Button id="btnHelp" 
            tabIndex=6 onclick="btnHelp_Click" runat="server" Text="Help" 
              CssClass="btn"  ></asp:Button> </TD></TR></TBODY></TABLE><%--onclick="btnSave_Click"--%>
</contenttemplate>
  
    </asp:UpdatePanel>

</asp:Content>

