<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptExcursionCostPricelistReport.aspx.vb" Inherits="rptPricelistReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
// WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->
</script>
<script language="javascript" type="text/javascript" >

</script> &nbsp;&nbsp;<table style="width: 331px">
        <tr>
            <td style="width: 250px; height: 22px">

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Pricelist Report" Width="180px" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
            Text="Print" /></td>
                    </tr>
    </table>
    <table style="width: 363px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPricelist" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
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