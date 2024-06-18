<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Visitfollowup.aspx.vb" Inherits="Visitfollowup"  MasterPageFile="~/SubPageMaster.master" Strict ="true"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    <%@ OutputCache location="none" %> 

<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"   TagPrefix="ews" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

<script language="javascript" type="text/javascript">



function checkNumber(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode : 
        ((evt.which) ? evt.which : 0));
    if (charCode != 47 && (charCode > 45 && charCode < 58))
     {
    //alert("Enter numerals only in this field. "+ charCode);
        return true;
     }
       return false;
}

function validate()
{
var txtCode=document.getElementById("<%=txtCode.ClientID%>");
var txtName=document.getElementById("<%=txtName.ClientID%>");
var dpVDate=document.getElementById("<%=dpVDate.ClientID%>");
var txtcontact=document.getElementById("<%=txtcontact.ClientID%>");
var txtdesc=document.getElementById("<%=txtdesc.ClientID%>");


if (txtCode.value=='')
 {
      alert("Agent code cannot be blank");
     return false;  
 }
if (txtName.value=='')
 {
      alert("Agent Name cannot be blank");
     return false;  
 }
if (dpVDate.value=='')
 {
      alert("Date cannot be blank");
     return false;  
 }
if (txtcontact.value=='')
 {
      alert("Contact Person cannot be blank");
     return false;  
 }
if (txtdesc.value=='')
 {
      alert("Description cannot be blank");
     return false;  
 }

}



</script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="BORDER-RIGHT: gray 2px solid; BORDER-TOP: gray 2px solid; BORDER-LEFT: gray 2px solid; BORDER-BOTTOM: gray 2px solid; width: 935px;" class="td_cell"><TBODY><TR><TD class="td_cell" align=center colSpan=2><asp:Label id="lblHeading" runat="server" Text="Visit Follow Up" Width="100%" CssClass="field_heading"></asp:Label></TD></TR>
    <tr>
        <td style="width: 106px">
            Visit Id</td>
        <td style="color: #000000">
            <INPUT onblur="chgValue()" id="txtvisitid" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" readonly="readOnly" /></td>
    </tr>
    <TR><TD style="WIDTH: 106px"><SPAN style="COLOR: black">Agent &nbsp;Code</SPAN> </TD><TD style="COLOR: #000000"><INPUT onblur="chgValue()" id="txtCode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" readonly="readOnly" />&nbsp;
    <INPUT id="txtName" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" style="width: 418px" readonly="readOnly" /></TD></TR><TR><TD style="WIDTH: 106px; HEIGHT: 25px">
    Sales Person</TD><TD style="height: 25px"><INPUT onblur="chgValue()" id="txtspersoncode" class="field_input" tabIndex=1 type=text maxLength=20 runat="server" readonly="readOnly" />&nbsp;
    <INPUT id="txtspersonname" class="field_input" tabIndex=2 type=text maxLength=100 runat="server" style="width: 418px" readonly="readOnly" /></TD></TR>
    <TR><TD style="WIDTH: 106px; HEIGHT: 24px">
        Visit
    Date</TD><TD>
        <ews:DatePicker ID="dpVDate" runat="server" DateFormatString="dd/MM/yyyy" RegExErrorMessage="Please enter a date in the format: dd/mm/yyyy"
            TabIndex="1" Width="225px" />
    </TD></TR><TR><TD class="td_cell" style="height: 16px">
        Contact Person</TD><TD style="height: 16px">
            <asp:TextBox ID="txtcontact" runat="server" Width="343px"></asp:TextBox></TD></TR><TR><TD class="td_cell">
                Remarks</TD><TD>
            <asp:TextBox ID="txtdesc" runat="server" Width="799px"></asp:TextBox></TD></TR>
    <tr>
        <td class="td_cell" style="height: 16px">
            Action need to take</td>
        <td style="height: 16px">
            <asp:TextBox ID="txtaction" runat="server" Width="799px"></asp:TextBox></td>
    </tr>
    <TR><TD class="td_cell" style="height: 52px"></TD><TD style="height: 52px">
            &nbsp;
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 134px; height: 22px;">
                        <asp:Button id="btnSave" tabIndex=6 runat="server" Text="Save" CssClass="field_button" OnClientClick="return validate();"></asp:Button></td>
                    <td style="width: 148px; height: 22px;">
                        <asp:Button id="btnCancel" tabIndex=7 runat="server" Text="Return to Search" Width="104px" CssClass="field_button"></asp:Button></td>
                    <td style="width: 116px; height: 22px;">
                        <asp:Button ID="btnHelp" runat="server" CssClass="field_button" TabIndex="8" Text="Help" /></td>
                    <td style="width: 581px; height: 22px;">
                        &nbsp;&nbsp;
                        <input id="txtconnection" runat="server" style="visibility: hidden; width: 12px;
                            height: 9px" type="text" /></td>
                    <td style="width: 92px; height: 22px;">
                        </td>
                    <td style="height: 22px">
                    </td>
                    <td style="width: 114px; height: 22px;">
                        </td>
                    <td style="width: 100px; height: 22px;">
                    </td>
                </tr>
            </table>
        </TD></TR>
    <tr>
        <td class="td_cell" style="height: 52px">
        </td>
        <td style="height: 52px">
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </td>
    </tr>
</TBODY></TABLE>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
