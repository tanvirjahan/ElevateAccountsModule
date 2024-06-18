<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptSupplierpoliciesReport.aspx.vb" Inherits="rptSupplierpoliciesReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
// WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->

function btnprint_click()
{
window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
}
</script>
<script language="javascript" type="text/javascript" >

</script> &nbsp;&nbsp;<table style="width: 279px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" Text="Back To Supplier Policies Report"
        Width="230px" /></td>
                        <td style="width: 4px">
                            <asp:Button ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /></td>
        </tr>
    </table>
    <table style="width: 273px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVSupplierPolicies" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="width: 59px">
            </td>
            <td style="width: 254px">
            </td>
        </tr>
    </table>

</asp:Content>