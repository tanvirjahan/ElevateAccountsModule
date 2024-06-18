<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptspleventPricelistReport.aspx.vb" Inherits="rptspleventPricelistReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"%>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">

</script>
<script language="javascript" type="text/javascript" >

</script> &nbsp;&nbsp;<table style="width: 331px">
        <tr>
            <td style="width: 239px; height: 22px">

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Pricelist Report" /></td>
                           
            <td style="width: 1782px; height: 22px">
                <asp:Button ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /></td>
        </tr>
    </table>
    <table style="width: 363px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPricelist" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False"  />
            </td>
        </tr>
        <tr>
            <td style="width: 3px">
            </td>
            <td style="width: 96px">
            </td>
            <td style="width: 215px">
            </td>
        </tr>
    </table>

</asp:Content>