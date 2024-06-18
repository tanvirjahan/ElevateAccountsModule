<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptUpdatesupplierInvoices.aspx.vb" Inherits="AccountsModule_rptUpdatesupplierInvoices"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ContentPlaceHolderID="Main" runat="server" >
<script type ="text/javascript"  >
//function btnprint_click()
//{
////window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
//}

</script>

    <table style="width: 936px">
        <tr>
            <td>
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Print" /></td>
        </tr>
        <tr>
            <td>
                <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" 
                    HasPrintButton="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
            </td>
        </tr>
    </table>

</asp:Content>