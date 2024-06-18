<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptAgentloginInformationReport.aspx.vb" Inherits="WebAdminModule_rptAgentloginInformationReport" MasterPageFile ="~/WebAdminMaster.master"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID ="Main"  runat ="server" >  
  
<script language="javascript" type="text/javascript">
//window.onbeforeunload = Unload;

</script>

    <table>
        <tr>
            <td>
                <asp:Button ID="btnBack" runat="server" CssClass="field_button" 
                    Text="Back To Report" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="field_button"
                        Text="Print" /></td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true"
                    HasCrystalLogo="False" HasPrintButton="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    
     </table>
       
</asp:Content>