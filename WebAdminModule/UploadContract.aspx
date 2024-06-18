<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadContract.aspx.vb" Inherits="UploadContract"  MasterPageFile="~/WebAdminMaster.master" Strict="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID ="Main" runat="server" >

<script language ="javascript" type="text/javascript" >

function validateupload()
{
var txtAlternateText=document.getElementById("<%=txtAlternateText.ClientID%>"); 	
if (txtAlternateText.value=='')
{
 alert('Alter Text should not blank...');
return false;
}
}
</script>

<TABLE style="border: 2px solid gray; WIDTH: 600px; height: 210px;">
<TBODY>
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Upload Contract"
                    Width="657px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 149px; height: 21px;" >
                Alternate&nbsp;Text</td>
            <td style="height: 21px; width: 645px;" >
                <asp:TextBox ID="txtAlternateText" runat="server" CssClass="field_input"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 149px; height:22px" >
                Upload&nbsp;Image</td>
            <td style="height: 22px; width: 645px;">
                <asp:FileUpload ID="fuImage" runat="server" CssClass="field_input" 
                    TabIndex="17" Width="226px"  />
                <asp:Button ID="btnUpload" runat="server" CssClass="field_button" Text="Upload" OnClientClick="return validatepage();" />
   
                &nbsp;
                <asp:Button ID="Btnrmv" runat="server" CssClass="field_button" Text="Remove" 
                                                                                Width="77px" />
   
                </td>
        </tr>
        <tr>
            <td style="width: 149px; height: 34px;" >
            </td>
            <td style="width: 645px; height: 34px;" >
   
                <asp:label ID="lblImage" runat="server" Width="484px" CssClass="field_input"></asp:label>
            </td>
        </tr>
        <tr>
            <td style="width: 149px" >
                <asp:Button ID="btnSave" runat="server" CssClass="field_button" TabIndex="6" 
                    Text="Save" Width="69px" />&nbsp;&nbsp;&nbsp;&nbsp; </td>
            <td style="width: 645px" >
                &nbsp; <asp:Button ID="btnHelp" runat="server" CssClass="field_button"
                        OnClick="btnHelp_Click" TabIndex="8" Text="Help" Width="45px" />&nbsp;
                            <asp:Button ID="btnCancel" TabIndex="8" OnClick="btnCancel_Click" runat="server"
                                Text="Exit" CssClass="btn"></asp:Button></td>
        </tr>
    </TBODY>
    </table>

</asp:Content>
