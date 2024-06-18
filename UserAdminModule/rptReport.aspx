<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptReport.aspx.vb" Inherits="rptReport" MasterPageFile="~/UserAdminMaster.master" Strict="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table>
    <script type ="text/javascript"  >
function btnprint_click()
{
window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
}
</script>
        <tr>
            <td style="width: 230px">
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" Width="80px" />&nbsp;
                    <input id="btnprint" class="btn" style="width: 76px" type="button" value="Print" onclick="btnprint_click();"/></td>
        </tr>
        <tr>
            <td style="width: 100px">
               <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" ToolbarImagesFolderUrl="../images/crystaltoolbar/" GroupTreeImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
    </table>

</asp:Content>