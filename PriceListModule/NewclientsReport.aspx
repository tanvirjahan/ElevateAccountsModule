<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewclientsReport.aspx.vb" Inherits="NewclientsReport"  MasterPageFile="~/SubPageMaster.master" Strict="true"  %>

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

</script> 

    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
        Text="Back To New Clients Query" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
            Text="Print" /><br />
<CR:CrystalReportViewer ID="CRVNewClients" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />

</asp:Content>