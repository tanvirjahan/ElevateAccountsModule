<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptAgentletterprint.aspx.vb" Inherits="rptAgentletterprint"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

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

</script> 
    <table style="width: 343px">
        <tr>
            <td colspan="3">
                &nbsp;<CR:CrystalReportViewer ID="CRVPLprint" runat="server"  AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" HasPageNavigationButtons="False" />
            </td>
        </tr>
    </table>

</asp:Content>