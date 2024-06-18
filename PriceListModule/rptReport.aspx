<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptReport.aspx.vb" Inherits="rptReport" MasterPageFile="~/PriceListMaster.master" Strict="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script type ="text/javascript"  >
function btnprint_click()
{
window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
}
</script>
    <table>
        <tr>
            <td colspan="3" style="height: 66px; width: 396px;">
                <table style="width: 359px">
                    <tr>
                        <td style="width: 78px; height: 22px">
                <asp:Button ID="btnBack" runat="server" CssClass="field_button" Text="Back" Width="80px" /></td>
                        <td style="height: 22px">
                <input id="Button1" class="field_button" onclick="btnprint_click();" style="width: 76px"
                    type="button" value="Print" /></td>
                        <td style="width: 3px; height: 22px">
                        </td>
                    </tr>
                </table>
                <table style="width: 461px">
                    <tr>
                        <td>
               <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" ToolbarImagesFolderUrl="../images/crystaltoolbar/" GroupTreeImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>