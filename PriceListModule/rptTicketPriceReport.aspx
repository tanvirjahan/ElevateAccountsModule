<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptTicketPriceReport.aspx.vb" Inherits="rptTicketPriceReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
   
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
// -->

</script>
<script language="javascript" type="text/javascript" >

</script> &nbsp;&nbsp;<table style="width: 247px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Ticket Pricelist Report" /></td>
            
            <td>
                <asp:Button ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /></td>
        </tr>
    </table>
    <table style="width: 389px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVTicketPricelist" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
        <tr>
            <td style="width: 236px">
            </td>
            <td>
            </td>
            <td style="width: 118px">
            </td>
        </tr>
    </table>

</asp:Content>