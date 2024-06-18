<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptPackageCostSheetReport.aspx.vb" Inherits="rptPackageCostSheetReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">
<!--
WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');
// -->

</script>
<script language="javascript" type="text/javascript" >

</script> &nbsp;&nbsp;<table style="width: 243px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Package Cost Sheet Report" /></td>
            <td>
                
            <td>
                <asp:Button ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /></td>
        </tr>
    </table>
    <table style="width: 382px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPackageCostSheet" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" />
            </td>
        </tr>
    </table>

</asp:Content>