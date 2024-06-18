<%@ Page Title="" Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="rptFLMReport.aspx.vb" Inherits="rptFLMReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table>
        <tr>
            <td style="width: 74px; height: 22px">
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back " /></td>
            <td style="width: 195px; height: 22px">
                <asp:Button t id="btnprint" CssClass="btn" style="width: 76px"  text="Print" runat="server"/></td>
            <td style="width: 65px; height: 22px">
            </td>
        </tr>
    </table>
    <table>
    <tr>
    <td>
                            <CR:CrystalReportViewer ID="CRVflmReport" runat="server" AutoDataBind="true" ToolbarImagesFolderUrl="../images/crystaltoolbar/" GroupTreeImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" Height="50px"  />
                            
                        </td>
    </tr>
    </table>
</asp:Content>

