<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rptDebitnotes.aspx.vb" Inherits="rptDebitnotes" MasterPageFile="~/SubPageMaster.master" Strict="true" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
        
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="ews" %>
    
    <%@ OutputCache location="none" %> 
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<%--<script type ="text/javascript"  >
function btnprint_click()
{
window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');
}
</script>
--%>
    <table style="width: 535px">
        <tr>
            <td style="width: 15%">
                <asp:Label ID="Label1" runat="server" CssClass="td_cell" Text="From Date" Visible="False"></asp:Label></td>
            <td style="width: 230px">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtbox" Visible="False"
                    Width="80px"></asp:TextBox><asp:ImageButton ID="ImgBtnFrmDt" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"
                        Visible="False" /><cc1:MaskedEditValidator ID="MskVFromDt" runat="server" ControlExtender="MskFromDate"
                            ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                            EmptyValueMessage="Date is required" ErrorMessage="MskVFromDate" InvalidValueBlurredMessage="*"
                            InvalidValueMessage="Invalid Date" TooltipMessage="Input a date in dd/mm/yyyy format"
                            Visible="False" Width="23px"></cc1:MaskedEditValidator></td>
            <td style="width: 15%">
                <asp:Label ID="Label2" runat="server" CssClass="td_cell" Text="To Date" Visible="False"></asp:Label></td>
            <td style="width: 230px">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="txtbox" Visible="False"
                    Width="80px"></asp:TextBox><asp:ImageButton ID="ImgBtnRevDate" runat="server" ImageUrl="../Images/Calendar_scheduleHS.png"
                        Visible="False" /><cc1:MaskedEditValidator ID="MskVToDate" runat="server" ControlExtender="MskToDate"
                            ControlToValidate="txtToDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="*"
                            EmptyValueMessage="Date is required" InvalidValueBlurredMessage="*" InvalidValueMessage="Invalid Date"
                            TooltipMessage="Input a date in dd/mm/yyyy format" Visible="False"></cc1:MaskedEditValidator></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" CssClass="td_cell" Text="Select Order" Visible="False"></asp:Label></td>
            <td style="width: 230px">
                <select id="ddlorderby" runat="server" class="drpdown" onchange="CallWebMethod('sptypecode');"
                    style="width: 120px" tabindex="0" visible="false">
                    <option selected="selected" value="Docno">Document Number</option>
                    <option value="Date">Transaction Date</option>
                </select>
            </td>
            <td style="width: 230px">
                <asp:Label ID="Label4" runat="server" CssClass="td_cell" Text="Report Type" Visible="False"></asp:Label></td>
            <td style="width: 230px">
                <select id="ddlreporttype" runat="server" class="drpdown" onchange="CallWebMethod('sptypecode');"
                    style="width: 120px" tabindex="0" visible="false">
                    <option selected="selected" value="Brief">Brief</option>
                    <option value="Detail">Detail</option>
                </select>
            </td>
        </tr>
        <tr>
            <td style="text-align: center" colspan="4">
                &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="btn" Text="Back" />&nbsp;
                <asp:Button ID="btnPrint" runat="server" CssClass="btn" Text="Print" /></td>
        </tr>
    </table>
               <CR:CrystalReportViewer ID="CRVReport" runat="server" AutoDataBind="true" ToolbarImagesFolderUrl="../images/crystaltoolbar/"  HasPrintButton="False" />
    <cc1:MaskedEditExtender ID="MskFromDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
        ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
        TargetControlID="txtFromDate">
    </cc1:MaskedEditExtender>
    <cc1:MaskedEditExtender ID="MskToDate" runat="server" AcceptNegative="Left" DisplayMoney="Left"
        ErrorTooltipEnabled="True" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
        TargetControlID="txtToDate">
    </cc1:MaskedEditExtender>
    <cc1:CalendarExtender ID="ClsExFromDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
        TargetControlID="txtFromDate">
    </cc1:CalendarExtender>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="ImgBtnFrmDt"
        TargetControlID="txtFromDate">
    </cc1:CalendarExtender>

</asp:Content>