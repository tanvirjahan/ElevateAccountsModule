<%@ Page Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="Resetdocgen.aspx.vb" Inherits="AccountsModule_Resetdocgen"  %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">


function disp_confirm()
{
  var r=confirm("Are you sure to close the year?");
  return r
}



function checkNumber1(thi,e)
{
    var year=document.getElementById("<%=txtyear.ClientID%>");
    var value=year.value;
    if ( (event.keyCode < 47 || event.keyCode > 57) )
	{
	    return false;
    } 

   if(value.length+1>4)
    {
     alert('Year cannot be more than 4 digits');
      return false;
    }

} 


</script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border-right: gray 2px solid; border-top: gray 2px solid; border-left: gray 2px solid; border-bottom: gray 2px solid; width: 709px;">
                <tr>
                    <td align="center" class="field_heading" style="width: 100px; height: 21px">
                        <asp:Label ID="lblHeading" runat="server" CssClass="field_heading" Text="Generate New no"
                            Width="668px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <table style="width: 774px">
                            <tr>
                                <td style="width: 31px" class="td_cell">
                                    <asp:Label ID="Label2" runat="server" Text="Enter the year" Width="114px"></asp:Label></td>
                                <td style="width: 64px">
                                    <asp:TextBox ID="txtyear" runat="server" 
                                        onkeypress="return checkNumber1(this,event);" CssClass="txtbox" Width="106px"></asp:TextBox></td>
                                <td style="width: 81px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 31px">
                                </td>
                                <td style="width: 64px">
                                </td>
                                <td style="width: 81px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 31px">
                                </td>
                                <td style="width: 64px">
                        <asp:Button ID="btnseal" runat="server" CssClass="btn" 
                                        OnClientClick="return disp_confirm();"    OnClick="btnseal_Click"
                             Text="New no. Generation" /></td>
                                <td style="width: 81px">
                                    <input id="txtDivCode" runat="server" maxlength="20" style="visibility: hidden; width: 5px"
                                        type="text" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="Label1" runat="server" Text="New no. will generate for the Reservation and Invoice and the no. will start from begining."
                            Width="608px" CssClass="td_cell"></asp:Label></td>
                </tr>
            </table>
            <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
                <Services>
                    <asp:ServiceReference Path="~/clsServices.asmx" />
                </Services>
            </asp:ScriptManagerProxy>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

