<%@ Page Language="VB" MasterPageFile="~/MainPageMaster.master" AutoEventWireup="false" CodeFile="rptPackagePrint.aspx.vb" Inherits="PriceListModule_rptPackagePrint" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table style="width: 403px">
        <tr>
            <td>

    <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back To Page" />&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="btn"
            Text="Print" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp;
            </td>
                    </tr>
    </table>
    <table style="width: 407px">
        <tr>
            <td colspan="3">
<CR:CrystalReportViewer ID="CRVPackage" runat="server" AutoDataBind="true" HasCrystalLogo="False" ToolbarImagesFolderUrl="../images/crystaltoolbar/" />
            </td>
        </tr>
    </table>




</asp:Content>

