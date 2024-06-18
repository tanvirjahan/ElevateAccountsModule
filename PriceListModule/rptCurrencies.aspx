<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptCurrencies.aspx.vb" Inherits="rptCurrencies"  MasterPageFile="~/SubPageMaster.master" Strict="true"%>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 

<asp:Content ContentPlaceHolderID="Main" runat="server">
<script type ="text/javascript"  >
function btnprint_click()
{
//window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
}
</script>

    &nbsp;
    <table style="width: 319px">
        <tr>
            <td style="width: 74px; height: 22px">
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back " /></td>
            <td style="width: 195px; height: 22px">
                <input id="Button1" class="btn" style="width: 76px" type="button" value="Print" onclick="btnprint_click();" runat="server"/></td>
            <td style="width: 65px; height: 22px">
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td style="width: 94px; height: 56px;">
                &nbsp;<CR:CrystalReportViewer ID="CRVCurrency" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
    </table>

</asp:Content>