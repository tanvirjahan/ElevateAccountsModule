<%@ Page Language="VB" AutoEventWireup="false" CodeFile="chequeprint.aspx.vb" Inherits="chequeprint"
    MasterPageFile="~/SubPageMaster.master" Strict="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript">

    </script>
    <script language="javascript" type="text/javascript">

    </script>
    <script type="text/javascript">

    </script>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:Button ID="btnBack" runat="server" CssClass="field_button" Text="Back To Report" />&nbsp;<asp:Button
                    ID="btnPrint" runat="server" CssClass="field_button" Text="Print" /><br />
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" />
            </td>
        </tr>
    </table>
</asp:Content>
