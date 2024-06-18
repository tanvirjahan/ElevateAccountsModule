<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="rptTrfJeepPriceListReport.aspx.vb" Inherits="PriceListModule_rptTrfJeepPriceListReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">


&nbsp;&nbsp;<table style="width: 403px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Transfer/Jeepwadi Pricelist Report" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
            Text="Print" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp;
            </td>
                    </tr>
    </table>
    <table style="width: 407px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPricelist" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
    </table>
<br />
    &nbsp;


</asp:Content>

