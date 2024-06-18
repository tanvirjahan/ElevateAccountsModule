<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptVisaPriceListReport.aspx.vb" Inherits="rptVisaPriceListReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    <script type="text/javascript">
<!--
//WebForm_AutoFocus('ctl00_SampleContent_WUCCamperMenuOptionsAsTab2_TabStCamperOptions_TbCamperLkp_WUCCamperLookup2_WUCCamperInquiry1_TBLastName');


// -->
</script>
<script language="javascript" type="text/javascript" >

</script> 

&nbsp;&nbsp;<table style="width: 403px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Other Pricelist Report" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
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