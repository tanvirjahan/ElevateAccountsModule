<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptReportNew.aspx.vb" Inherits="rptReportNew" MasterPageFile="~/SubPageMaster.master" Strict="true" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ OutputCache Location="none" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Main">




    <script type="text/javascript">

    </script>

    <table>
        <tr>
            <td colspan="3" style="height: 66px; width: 396px;">
                <table style="width: 359px">
                    <tr>
                        <td style="width: 78px; height: 22px">
                            <asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" Width="80px" /></td>
                        <td style="height: 22px">
                            <asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Print" Width="47px"  />
                        <td style="width: 4px; height: 22px">
                        </td>
                    </tr>
                </table>
                <table style="width: 461px">
                    <tr>
                        <td>
                            <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" 
                                ToolbarImagesFolderUrl="../images/crystaltoolbar/"  
                                GroupTreeImagesFolderUrl="../images/crystaltoolbar/" HasPrintButton="False" 
                                Height="50px" HasCrystalLogo="False" ShowAllPageIds="True" />


                         





                            
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>