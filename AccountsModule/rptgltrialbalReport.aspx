<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptgltrialbalReport.aspx.vb" Inherits="rptgltrialbalReport"  MasterPageFile="~/SubPageMaster.master" Strict="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    <%@ OutputCache location="none" %> 
    


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript">

</script>
<script language="javascript" type="text/javascript" >

</script> 
<script type ="text/javascript"  >
</script>
    <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
        Text="Back To Report" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
            Text="Print" /><br />
<CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/"  HasPrintButton="False" />

</asp:Content>