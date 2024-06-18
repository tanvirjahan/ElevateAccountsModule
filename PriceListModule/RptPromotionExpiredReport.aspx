<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="RptPromotionExpiredReport.aspx.vb" Inherits="PriceListModule_RptPromotionExpiredReport" %>
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

</script> &nbsp;&nbsp;<table style="width: 303px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Promotion Expiry Report" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /></td>
            
        </tr>
    </table>
    <table style="width: 343px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPriceExpiry" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="width: 39px">
            </td>
            <td style="width: 161px">
            </td>
        </tr>
    </table>







</asp:Content>

