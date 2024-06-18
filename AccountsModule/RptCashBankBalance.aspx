<%@ Page Title="" Language="VB" MasterPageFile="~/AccountsMaster.master" AutoEventWireup="false" CodeFile="RptCashBankBalance.aspx.vb" Inherits="RptCashBankBalance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script type="text/javascript">
    function DateSelectCalExt() {
        var txtfromDate = document.getElementById("<%=txtFromDate.ClientID%>");
        if (txtfromDate.value != '') {
            var calendarBehavior1 = document.getElementById("<%=txtFromDate_CalendarExtender.ClientID %>");
            var date = calendarBehavior1._selectedDate;

            var dp = txtfromDate.value.split("/");
            var newDt = new Date(dp[2] + "/" + dp[1] + "/" + dp[0]);
            newDt = getFormatedDate(newDt);
            newDt = new Date(newDt);
            calendarBehavior1.set_selectedDate(newDt);
        }        
    }

</script>
<table style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
        width:100%; border-bottom: gray 1px solid" class="td_cell" align="left">
        
        <tbody>
            <tr>
                <td class="field_heading" align="center">
                    <asp:Label ID="lblHeading" runat="server" Text="Cash/Bank Balance" CssClass="field_heading"
                        Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
            <td style="width:50%">
            <table style="width:100%" cellpadding="10px">
            <tr>
            <td style="width:20%" align="center"><asp:Label ID="lblasdate" runat="server" Text="As On Date" CssClass="field_caption"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="field_input" Width="80px" TabIndex="1"></asp:TextBox>
                <asp:ImageButton ID="ImgBtnFrmDt" runat="server" tabindex="2" ImageUrl="~/Images/Calendar_scheduleHS.png"> </asp:ImageButton><br />
                <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="DateSelectCalExt"
                                        PopupButtonID="ImgBtnFrmDt" PopupPosition="Right" TargetControlID="txtFromDate">
                </asp:CalendarExtender>
                <asp:MaskedEditExtender ID="txtFromDate_MaskedEditExtender" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFromDate">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator ID="MevtxtFromDate" runat="server" ControlExtender="txtFromDate_MaskedEditExtender"
                ControlToValidate="txtFromDate" CssClass="field_error" Display="Dynamic" EmptyValueBlurredText="Date is required"
                EmptyValueMessage="Date is required" ErrorMessage="ChkFromDt_MaskedEditExtender"
                InvalidValueBlurredMessage="Invalid Date" InvalidValueMessage="Invalid Date"
                TooltipMessage="Input a Date in Date/Month/Year">
                </asp:MaskedEditValidator>               
             </td>
            </tr>
            <tr>
            <td colspan="2">
                <asp:Button ID="btnPdfReport"  tabindex="9" OnClick="btnPdfReport_Click" runat="server" Text="Pdf Report"
                        CssClass="field_button" CausesValidation="False"></asp:Button>&nbsp;
                <asp:Button ID="btnExcelReport"  tabindex="10" OnClick="btnExcelReport_Click" runat="server" Text="Excel Report"
                        CssClass="field_button" CausesValidation="False"></asp:Button>
                        &nbsp;<asp:Button ID="btnhelp" tabindex="20" OnClick="btnhelp_Click"
                            runat="server" Text="Help" Height="20px" CssClass="field_button"></asp:Button>
                            <asp:Button ID="btnAddNew" TabIndex="2" runat="server" Text="Add New"
                                Font-Bold="False" CssClass="btn" style="display:none" ></asp:Button>
                            <input style="visibility: hidden; width: 29px" id="txtDivcode" type="text" maxlength="20" runat="server" />
                            <asp:GridView ID="gv_SearchResult" runat="server" AutoGenerateColumns="False" 
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                               CssClass="td_cell" Font-Size="10px">
                                                 </asp:GridView>
            </td>
            </tr>
            </table>
            </td>
            </tr>

        </tbody>
</table>

</asp:Content>

