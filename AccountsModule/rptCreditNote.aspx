<%@ Page Language="VB" MasterPageFile="~/SubPageMaster.master" AutoEventWireup="false" CodeFile="rptCreditNote.aspx.vb" Inherits="AccountsModule_rptCreditNote"%>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID ="Main" runat="server" >
    <table>
        <tr>
            <td style="width: 100px">
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" />&nbsp;<asp:Button
                    ID="btnPrint" runat="server" CssClass="btn" Text="Print" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" HasPrintButton="False"
                    ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
            </td>
        </tr>
    </table>

</asp:Content>